using System.Text.RegularExpressions;

namespace Application.Features.Problems;

public class ProblemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public uint NoTests { get; set; }
    public string Author { get; set; }
    public float TimeLimitInSeconds { get; set; }
    public float TotalMemoryLimitInMb { get; set; }
    public float StackMemoryLimitInMb { get; set; }
    public uint Grade { get; set; }
    public string InputFileName { get; set; }
    public string OutputFileName { get; set; }
    public string Contest { get; set; }
}