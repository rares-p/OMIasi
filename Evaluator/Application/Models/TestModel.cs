namespace Application.Models;

public class TestModel(uint index, uint score, (string input, string output) testContent)
{
    public uint Index { get; } = index;
    public string Input { get; } = testContent.input;
    public string Output { get; } = testContent.output;
    public uint Score { get; } = score;
}