namespace Domain.Entities;

public class Test
{
    public required Guid Id { get; init; }
    public required Guid ProblemId { get; init; }
    public required uint Index { get; init; }
    public required byte[] Input { get; init; }
    public required byte[] Output { get; init; }
    public required uint Score { get; init; }
}