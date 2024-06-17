using Application.Contracts;
using Application.Models;
using Application.Responses;
using Domain.Entities;

namespace Infrastructure.Services;


public class EvaluationService(IProblemRepository problemRepository, ITestRepository testRepository) : IEvaluationService
{
    private readonly string[] _messages = ["Correct", "Incorrect", "Time Limit", "Memory limit"];
    public async Task<EvaluationResponse> Evaluate(Evaluation evaluation)
    {
        var problemResult = await problemRepository.FindByIdAsync(evaluation.ProblemId);
        if (!problemResult.IsSuccess)
            return new EvaluationResponse
            {
                Success = false,
                Error = problemResult.Error,
                Results = null!
            };
        var testsResult = await testRepository.GetTestsByProblemIdAsync(evaluation.ProblemId);
        if (!testsResult.IsSuccess)
            return new EvaluationResponse()
            {
                Success = false,
                Error = testsResult.Error,
                Results = null!
            };
        var tests = new List<Test>(testsResult.Value.ToList());
        tests.Sort((t1, t2) => (int)(t2.Index - t1.Index));
        return new EvaluationResponse()
        {
            Success = true,
            Results = tests.Select(test => new TestResultModel()
            {
                Message = _messages[Random.Shared.Next(0, _messages.Length)],
                Score = (uint)100 / problemResult.Value.NoTests,
                Runtime = (Random.Shared.Next(1) == 0 ? 12u : 2u)
            }).ToList()
        };
    }
}

//public class EvaluationService
//{
//    private readonly SemaphoreSlim _semaphore;
//    private readonly int _maxThreads;

//    public EvaluationService()
//    {
//        _maxThreads = ;
//        _semaphore = new SemaphoreSlim(maxThreads);
//    }

//    public async Task<List<Result>> EvaluateSolution(int problemId, string code)
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
