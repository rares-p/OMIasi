namespace Application.Features.Problems.Commands.CreateProblem.CreateProblemCommandModels;

public class TestModel
{
    public uint Index { get; set; }
    public byte[] Input { get; set; }
    public byte[] Output { get; set; }
    public uint Score { get; set; }
}