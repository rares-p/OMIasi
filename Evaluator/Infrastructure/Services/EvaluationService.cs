using System.Diagnostics;
using System.Threading;
using Application.Contracts;
using Application.Models;
using Application.Responses;
using Domain.Common;
using Domain.Entities;

namespace Infrastructure.Services;


//public class EvaluationService(IProblemRepository problemRepository, ITestRepository testRepository) : IEvaluationService
//{
//    private readonly string[] _messages = ["Correct", "Incorrect", "Time Limit", "Memory limit"];
//    public async Task<EvaluationResponse> Evaluate(Evaluation evaluation)
//    {
//        var problemResult = await problemRepository.FindByIdAsync(evaluation.ProblemId);
//        if (!problemResult.IsSuccess)
//            return new EvaluationResponse
//            {
//                Success = false,
//                Error = problemResult.Error,
//                Results = null!
//            };
//        var testsResult = await testRepository.GetTestsByProblemIdAsync(evaluation.ProblemId);
//        if (!testsResult.IsSuccess)
//            return new EvaluationResponse()
//            {
//                Success = false,
//                Error = testsResult.Error,
//                Results = null!
//            };
//        var tests = new List<Test>(testsResult.Value.ToList());
//        tests.Sort((t1, t2) => (int)(t2.Index - t1.Index));
//        return new EvaluationResponse()
//        {
//            Success = true,
//            Results = tests.Select(test => new TestResultModel()
//            {
//                Message = _messages[Random.Shared.Next(0, _messages.Length)],
//                Score = (uint)100 / problemResult.Value.NoTests,
//                Runtime = (Random.Shared.Next(1) == 0 ? 12u : 2u)
//            }).ToList()
//        };
//    }
//}

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

//public class EvaluationService2
//{
//    private readonly SemaphoreSlim _semaphore = new(5);

//    public async Task<List<Result<List<Task>>>> EvaluateSolution(int problemId, string code)
//    {
//        var problem = await _context.Problems.Include(p => p.Tests).FirstOrDefaultAsync(p => p.Id == problemId);
//        if (problem == null) throw new ArgumentException("Problem not found");

//        var tasks = problem.Tests.Select(test => EvaluateTest(test, code)).ToList();
//        var results = await Task.WhenAll(tasks);

//        return results.ToList();
//    }

//    private async Task<Result> EvaluateTest(Test test, string code)
//    {
//        await _semaphore.WaitAsync();

//        try
//        {
//            var result = new Result { TestId = test.Id };

//            var compileProcess = new Process
//            {
//                StartInfo = new ProcessStartInfo
//                {
//                    FileName = "g++",
//                    Arguments = "-o solution.out solution.cpp",
//                    RedirectStandardError = true,
//                    RedirectStandardOutput = true,
//                    UseShellExecute = false,
//                    CreateNoWindow = true
//                }
//            };

//            compileProcess.Start();
//            var compileOutput = await compileProcess.StandardError.ReadToEndAsync();
//            compileProcess.WaitForExit();

//            if (compileProcess.ExitCode != 0)
//            {
//                result.Status = "CompilationError";
//                result.Output = compileOutput;
//            }
//            else
//            {
//                var runProcess = new Process
//                {
//                    StartInfo = new ProcessStartInfo
//                    {
//                        FileName = "./solution.out",
//                        RedirectStandardInput = true,
//                        RedirectStandardOutput = true,
//                        RedirectStandardError = true,
//                        UseShellExecute = false,
//                        CreateNoWindow = true
//                    }
//                };

//                runProcess.Start();
//                await runProcess.StandardInput.WriteAsync(test.Input);
//                await runProcess.StandardInput.FlushAsync();
//                runProcess.StandardInput.Close();

//                var runOutput = await runProcess.StandardOutput.ReadToEndAsync();
//                runProcess.WaitForExit();

//                result.Output = runOutput;
//                result.Status = runOutput.Trim() == test.ExpectedOutput.Trim() ? "Success" : "Failed";
//            }

//            return result;
//        }
//        finally
//        {
//            _semaphore.Release();
//        }
//    }
//}
