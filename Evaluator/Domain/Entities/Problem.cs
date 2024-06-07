namespace Domain.Entities;

public class Problem
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required uint NoTests { get; init; }
    public required string Author { get; init; }
    public required float TimeLimitInSeconds { get; init; }
    public required float TotalMemoryLimitInMb { get; init; }
    public required float StackMemoryLimitInMb { get; init; }
    public required uint Grade { get; init; }
    public required string InputFileName { get; init; }
    public required string OutputFileName { get; init; }
    public required uint Year { get; init; }
}