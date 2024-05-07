using HealthcareManagementSystem.Domain.Common;
using System.Text.RegularExpressions;

namespace Domain.Entities;

public partial class Problem : AuditableEntity
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public uint NoTests { get; private set; }
    public string Author { get; private set; }
    public float TimeLimitInSeconds { get; private set; }
    public float TotalMemoryLimitInMb { get; private set; }
    public float StackMemoryLimitInMb { get; private set; }
    public uint Grade { get; private set; }
    [GeneratedRegex(@"^[^\r\n\\/:""*?<>|]+\.(in)$")]
    private static partial Regex InputFileRegex();
    public string InputFileName { get; private set; }
    [GeneratedRegex(@"^[^\r\n\\/:""*?<>|]+\.(out)$")]
    private static partial Regex OutputFileRegex();
    public string OutputFileName { get; private set;}
    public string Contest { get; private set; }

    private Problem(string title, string description, uint noTests, string author, float timeLimitInSeconds,
        float totalMemoryLimitInMb, float stackMemoryLimitInMb, uint grade, string inputFileName, string outputFileName,
        string contest)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        NoTests = noTests;
        Author = author;
        TimeLimitInSeconds = timeLimitInSeconds;
        TotalMemoryLimitInMb = totalMemoryLimitInMb;
        StackMemoryLimitInMb = stackMemoryLimitInMb;
        Grade = grade;
        InputFileName = inputFileName;
        OutputFileName = outputFileName;
        Contest = contest;
    }

    public static Result<Problem> Create(string title, string description, uint noTests, string author,
        float timeLimitInSeconds, float totalMemoryLimitInMb, float stackMemoryLimitInMb, uint grade,
        string inputFileName, string outputFileName, string contest)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result<Problem>.Failure("Problem title cannot be empty!");
        if(string.IsNullOrWhiteSpace(description))
            return Result<Problem>.Failure("Problem description cannot be empty!");
        if(string.IsNullOrWhiteSpace(author))
            return Result<Problem>.Failure("Author is empty!");
        if(timeLimitInSeconds <= 0)
            return Result<Problem>.Failure("Time limit must be > 0 seconds");
        if(totalMemoryLimitInMb < 0)
            return Result<Problem>.Failure("Total Memory Limit must be >= 0 MB");
        if(stackMemoryLimitInMb < 0)
            return Result<Problem>.Failure("Stack Memory Limit must be >= 0 MB");
        if(grade is < 5 or > 12)
            return Result<Problem>.Failure("Grade must be between 5 and 12");
        if(!InputFileRegex().IsMatch(inputFileName))
            return Result<Problem>.Failure("Input file name must end with extension .in");
        if(!OutputFileRegex().IsMatch(outputFileName))
            return Result<Problem>.Failure("Output file name must end with extension .out");
        if(string.IsNullOrWhiteSpace(contest))
            return Result<Problem>.Failure("Problem contest cannot be empty!");
        return Result<Problem>.Success(new Problem(title, description, noTests, author,
            timeLimitInSeconds, totalMemoryLimitInMb,  stackMemoryLimitInMb, grade,
            inputFileName, outputFileName, contest));
    }

}