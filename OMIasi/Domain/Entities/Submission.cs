using Domain.Common;

namespace Domain.Entities;

public class Submission
{
    private Submission()
    {
    }

    private Submission(Guid userId, Guid problemId, string solution, uint score, DateTime date, List<SubmissionTestResult> scores)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        ProblemId = problemId;
        Solution = solution;
        Score = score;
        Date = date;
        Scores = [..scores];
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid ProblemId { get; private set; }
    public uint Score { get; private set; }
    public DateTime Date { get; private set; }
    public string Solution { get; private set; }
    public List<SubmissionTestResult> Scores { get; private set; }

    public static Result<Submission> Create(Guid userId, Guid problemId, string solution, uint score, DateTime date, List<(string message, uint score, uint runtime, uint testIndex)> scores)
    {
        if(userId == null)
            return Result<Submission>.Failure("User Id cannot be null!");

        if(problemId == null)
            return Result<Submission>.Failure("Problem Id cannot be null!");

        if(string.IsNullOrWhiteSpace(solution))
            return Result<Submission>.Failure("Solution cannot be empty");

        if(score > 100)
            return Result<Submission>.Failure("Score cannot be bigger than 100");

        if (date == null)
            return Result<Submission>.Failure("Submission date cannot be null!");

        var testResults = new List<SubmissionTestResult>();
        foreach (var testResult in scores.Select(s => SubmissionTestResult.Create(s.message, s.score, s.runtime, s.testIndex)))
        {
            if(!testResult.IsSuccess)
                return Result<Submission>.Failure(testResult.Error);
            testResults.Add(testResult.Value);
        }

        return Result<Submission>.Success(new Submission(userId, problemId, solution, score, date, testResults));
    }
}