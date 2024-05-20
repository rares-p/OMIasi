namespace Application.Features.Problems;

public class ProblemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public uint NoTests { get; set; }
    public string Author { get; set; } = string.Empty;
    public float TimeLimitInSeconds { get; set; }
    public float TotalMemoryLimitInMb { get; set; }
    public float StackMemoryLimitInMb { get; set; }
    public uint Grade { get; set; }
    public string InputFileName { get; set; } = string.Empty;
    public string OutputFileName { get; set; } = string.Empty;
    public string Contest { get; set; } = string.Empty;
}