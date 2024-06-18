namespace Domain.Entities;

public class Test
{
    public required Guid Id { get; init; }
    public required Guid ProblemId { get; init; }
    public required uint Index { get; init; }
    public required string Input { get; init; }
    public required string Output { get; init; }
    public required uint Score { get; init; }
}