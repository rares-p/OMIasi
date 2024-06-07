using Domain.Common;

namespace Domain.Entities;

public class Test
{
    public Guid Id { get; private set; }
    public Guid ProblemId { get; private set; }
    public uint Index { get; private set; }
    public byte[] Input { get; private set; }
    public byte[] Output { get; private set; }
    public uint Score { get; private set; }

    private Test(Guid problemId, uint index, byte[] input, byte[] output, uint score)
    {
        Id = Guid.NewGuid();
        ProblemId = problemId;
        Index = index;
        Input = input;
        Output = output;
        Score = score;
    }

    public static Result<Test> Create(Guid problemId, uint index, byte[] input, byte[] output, uint score)
    {
        if (input == null! || input.Length == 0)
            return Result<Test>.Failure("Test Input cannot be empty!");

        if(output == null! || output.Length == 0)
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

    public Result<Test> UpdateInput(byte[] input)
    {
        if (input == null! || input.Length == 0)
            return Result<Test>.Failure("Test Input cannot be empty!");
        Input = input;
        return Result<Test>.Success(this);
    }

    public Result<Test> UpdateOutput(byte[] output)
    {
        if (output == null! || output.Length == 0)
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