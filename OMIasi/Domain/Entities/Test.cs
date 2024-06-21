using Domain.Common;

namespace Domain.Entities;

public class Test
{
    public Guid Id { get; private set; }
    public Guid ProblemId { get; private set; }
    public uint Index { get; private set; }
    public uint Score { get; private set; }

    private Test(Guid problemId, uint index, uint score)
    {
        Id = Guid.NewGuid();
        ProblemId = problemId;
        Index = index;
        Score = score;
    }

    public static Result<Test> Create(Guid problemId, uint index, uint score)
    {
        if(score == 0)
            return Result<Test>.Failure("Test score must pe a positive number!");

        return Result<Test>.Success(new Test(problemId, index, score));
    }

    public Result<Test> UpdateIndex(uint index)
    {
        Index = index;
        return Result<Test>.Success(this);
    }

    public Result<Test> UpdateScore(uint score)
    {
        if (score == 0)
            return Result<Test>.Failure("Test score must pe a positive number!");
        Score = score;
        return Result<Test>.Success(this);
    }
}