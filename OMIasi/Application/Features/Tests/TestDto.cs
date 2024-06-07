namespace Application.Features.Tests;

public class TestDto
{
    public Guid Id { get; set; }
    public uint Index { get; set; }
    public byte[] Input { get; set; } = null!;
    public byte[] Output { get; set; } = null!;
    public uint Score { get; set; }
}