using Domain.Common;

namespace Domain.Entities;

public class Test
{
    public Guid Id { get; private set; }
    public Guid ProblemId { get; private set; }
    public uint Index { get; private set; }
    public string Input { get; private set; }
    public string Output { get; private set; }
    public uint Score { get; private set; }

    private Test(Guid problemId, uint index, string input, string output, uint score)
    {
        Id = Guid.NewGuid();
        ProblemId = problemId;
        Index = index;
        Input = input;
        Output = output;
        Score = score;
    }

    public static Result<Test> Create(Guid problemId, uint index, string input, string output, uint score)
    {
        if (string.IsNullOrWhiteSpace(input))
            return Result<Test>.Failure("Test Input cannot be empty!");

        if (string.IsNullOrWhiteSpace(output))
            return Result<Test>.Failure("Test Input cannot be empty!");

        if(score == 0)
            return Result<Test>.Failure("Test score must pe a positive number!");

        return Result<Test>.Success(new Test(problemId, index, input, output, score));
    }

    public Result<Test> UpdateIndex(uint index)
    {
        Index = index;
        return Result<Test>.Success(this);
    }

    public Result<Test> UpdateInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return Result<Test>.Failure("Test Input cannot be empty!");
        Input = input;
        return Result<Test>.Success(this);
    }

    public Result<Test> UpdateOutput(string output)
    {
        if (string.IsNullOrWhiteSpace(output))
            return Result<Test>.Failure("Test Input cannot be empty!");
        Output = output;
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