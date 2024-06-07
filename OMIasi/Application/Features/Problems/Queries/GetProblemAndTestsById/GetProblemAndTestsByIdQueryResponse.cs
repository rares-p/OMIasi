using Application.Features.Tests;

namespace Application.Features.Problems.Queries.GetProblemAndTestsById;

public class GetProblemAndTestsByIdQueryResponse
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
    public uint Year { get; set; }
    public List<TestDto> Tests { get; set; }
}