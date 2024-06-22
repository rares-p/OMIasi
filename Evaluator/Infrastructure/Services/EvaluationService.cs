using System.Diagnostics;
using Application.Contracts;
using Application.Models;
using Application.Responses;
using Domain.Entities;

namespace Infrastructure.Services;

public class EvaluationService(IProblemRepository problemRepository, ITestRepository testRepository)
    : IEvaluationService
{
    private readonly SemaphoreSlim _semaphore = new(2);
    private readonly string _baseFolderPath = "evaluate";
    private readonly string _compilerPath = "C:\\MinGW\\bin\\g++.exe";

    public async Task<EvaluationResponse> Evaluate(Evaluation evaluation)
    {
        return await EvaluateProblemAsync(evaluation.ProblemId, evaluation.Solution);
    }

    public async Task<EvaluationResponse> EvaluateProblemAsync(Guid problemId, string solution)
    {
        var problemResult = await problemRepository.FindByIdAsync(problemId);
        if (!problemResult.IsSuccess)
        {
            return new EvaluationResponse
            {
                Success = false,
                Results = null!
            };
        }

        var testsResult = await testRepository.GetTestsByProblemIdAsync(problemId);
        var evaluationTasks = testsResult.Value.Select(test => EvaluateTestAsync(problemResult.Value, test, solution));
        var solutionResult = await Task.WhenAll(evaluationTasks);

        return new EvaluationResponse
        {
            Success = true,
            Results = solutionResult.ToList()
        };
    }

    private async Task<TestResultModel> EvaluateTestAsync(Problem problem, Test test, string solution)
    {
        await _semaphore.WaitAsync();
        var evaluationDirectory = Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(),
            _baseFolderPath, Guid.NewGuid().ToString()));
        Process? run = null;
        try
        {
            var solutionPath = Path.Join(evaluationDirectory.FullName, "solution.cpp");
            await File.WriteAllTextAsync(solutionPath, solution);

            var compileProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _compilerPath,
                    Arguments = $"{solutionPath} -o {evaluationDirectory}/solution.exe",
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };



            compileProcess.Start();
            var compileOutput = await compileProcess.StandardError.ReadToEndAsync();
            await compileProcess.WaitForExitAsync();

            if (compileProcess.ExitCode != 0)
            {
                return new TestResultModel()
                {
                    Message = compileOutput,
                    Runtime = 0,
                    Score = 0,
                    TestIndex = test.Index
                };
            }

            await File.WriteAllTextAsync(Path.Join(evaluationDirectory.FullName, problem.InputFileName), test.Input);

            var runProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Path.Join(evaluationDirectory.FullName, "solution.exe"),
                    WorkingDirectory = evaluationDirectory.FullName,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            run = runProcess;

            runProcess.Start();
            var stopwatch = Stopwatch.StartNew();

            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(problem.TimeLimitInSeconds));
                bool memoryExceeded = await Task.Run(() =>
                {
                    while (!runProcess.HasExited)
                    {
                        if (cts.IsCancellationRequested || runProcess.HasExited)
                            return false;
                        if (!(runProcess.PrivateMemorySize64 / (1024 * 1024) > problem.StackMemoryLimitInMb)) continue;
                        if (!runProcess.HasExited)
                            runProcess.Kill();
                        return true;
                    }
                    return false;
                }, cts.Token);

                if (memoryExceeded)
                {
                    return new TestResultModel()
                    {
                        Message = "Memory limit exceeded",
                        Runtime = 0,
                        Score = 0,
                        TestIndex = test.Index
                    };
                }

                if (cts.IsCancellationRequested)
                    return new TestResultModel()
                    {
                        Message = "Time limit exceeded",
                        Runtime = (uint)problem.TimeLimitInSeconds,
                        Score = 0,
                        TestIndex = test.Index
                    };
            }
            catch (Exception)
            {
                return new TestResultModel()
                {
                    Message = "Server Problem. Try again later",
                    Runtime = (uint)problem.TimeLimitInSeconds,
                    Score = 0,
                    TestIndex = test.Index
                };
            }

            var solutionContent = (await File.ReadAllTextAsync(Path.Combine(evaluationDirectory.FullName, problem.OutputFileName))).TrimEnd();
            var testOutput = test.Output.TrimEnd();

            if (solutionContent == testOutput)
                return new TestResultModel()
                {
                    Message = "Correct!",
                    Score = test.Score,
                    Runtime = (uint)stopwatch.Elapsed.Milliseconds,
                    TestIndex = test.Index
                };


            return new TestResultModel()
            {
                Message = "Wrong answer",
                Runtime = (uint)stopwatch.Elapsed.Milliseconds,
                Score = 0,
                TestIndex = test.Index
            };
        }
        catch (Exception e)
        {
            return new TestResultModel()
            {
                Message = e.Message,
                Runtime = 0,
                Score = 0,
                TestIndex = test.Index
            };
        }
        finally
        {
            run?.Kill();
            while (true)
            {
                try
                {
                    evaluationDirectory.Delete(true);
                    break;
                }
                catch (UnauthorizedAccessException)
                {
                }
                catch (Exception e)
                {
                    await Task.Delay(5);
                }
            }
            _semaphore.Release();
        }
    }
}