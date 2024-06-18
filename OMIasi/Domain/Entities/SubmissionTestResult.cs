using Domain.Common;

namespace Domain.Entities;

public class SubmissionTestResult
{
    private SubmissionTestResult(string message, uint score, uint runtime, uint testIndex)
    {
        Id = Guid.NewGuid();
        Message = message;
        Score = score;
        Runtime = runtime;
        TestIndex = testIndex;
    }

    public Guid Id { get; private set; }
    public string Message { get; private set; }
    public uint Score { get; private set; }
    public uint Runtime { get; private set; }
    public uint TestIndex { get; private set; }

    public static Result<SubmissionTestResult> Create(string message, uint score, uint runtime, uint testIndex)
    {
        if(string.IsNullOrWhiteSpace(message))
            return Result<SubmissionTestResult>.Failure("Message cannot be empty");

        return Result<SubmissionTestResult>.Success(new SubmissionTestResult(message, score, runtime, testIndex));
    }
}