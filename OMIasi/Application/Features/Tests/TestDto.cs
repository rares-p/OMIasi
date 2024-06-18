namespace Application.Features.Tests;

public class TestDto
{
    public Guid Id { get; set; }
    public uint Index { get; set; }
    public string Input { get; set; } = null!;
    public string Output { get; set; } = null!;
    public uint Score { get; set; }
}