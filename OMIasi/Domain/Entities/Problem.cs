using System.Text.RegularExpressions;
using Domain.Common;

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
    public uint Year { get; private set; }
    public ICollection<Test> Tests { get; private set; }

    private Problem(string title, string description, uint noTests, string author, float timeLimitInSeconds,
        float totalMemoryLimitInMb, float stackMemoryLimitInMb, uint grade, string inputFileName, string outputFileName,
        uint year)
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
        Year = year;
    }

    public static Result<Problem> Create(string title, string description, uint noTests, string author,
        float timeLimitInSeconds, float totalMemoryLimitInMb, float stackMemoryLimitInMb, uint grade,
        string inputFileName, string outputFileName, uint year)
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
        if(year > DateTime.Now.Year + 1)
            return Result<Problem>.Failure("Problem year cannot be greater than current year!");
        return Result<Problem>.Success(new Problem(title, description, noTests, author,
            timeLimitInSeconds, totalMemoryLimitInMb,  stackMemoryLimitInMb, grade,
            inputFileName, outputFileName, year));
    }

    public Result<Problem> UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result<Problem>.Failure("Problem title cannot be empty!");
        Title = title;
        return Result<Problem>.Success(this);
    }

    public Result<Problem> UpdateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Result<Problem>.Failure("Problem description cannot be empty!");
        Description = description;
        return Result<Problem>.Success(this);
    }
    public Result<Problem> UpdateNoTests(uint noTests)
    {
        NoTests = noTests;
        return Result<Problem>.Success(this);
    }

    public Result<Problem> UpdateAuthor(string author)
    {
        if (string.IsNullOrWhiteSpace(author))
            return Result<Problem>.Failure("Author is empty!");
        Author = author;
        return Result<Problem>.Success(this);
    }

    public Result<Problem> UpdateTimeLimitInSeconds(float timeLimitInSeconds)
    {
        if (timeLimitInSeconds <= 0)
            return Result<Problem>.Failure("Time limit must be > 0 seconds");
        TimeLimitInSeconds = timeLimitInSeconds;
        return Result<Problem>.Success(this);
    }

    public Result<Problem> UpdateTotalMemoryLimitInMb(float totalMemoryLimitInMb)
    {
        if (totalMemoryLimitInMb < 0)
            return Result<Problem>.Failure("Total Memory Limit must be >= 0 MB");
        TotalMemoryLimitInMb = totalMemoryLimitInMb;
        return Result<Problem>.Success(this);
    }

    public Result<Problem> UpdateStackMemoryLimitInMb(float stackMemoryLimitInMb)
    {
        if (stackMemoryLimitInMb < 0)
            return Result<Problem>.Failure("Stack Memory Limit must be >= 0 MB");
        StackMemoryLimitInMb = stackMemoryLimitInMb;
        return Result<Problem>.Success(this);
    }

    public Result<Problem> UpdateGrade(uint grade)
    {
        if (grade is < 5 or > 12)
            return Result<Problem>.Failure("Grade must be between 5 and 12");
        Grade = grade;
        return Result<Problem>.Success(this);
    }

    public Result<Problem> UpdateInputFileName(string inputFileName)
    {
        if (!InputFileRegex().IsMatch(inputFileName))
            return Result<Problem>.Failure("Input file name must end with extension .in");
        InputFileName = inputFileName;
        return Result<Problem>.Success(this);
    }

    public Result<Problem> UpdateOutputFileName(string outputFileName)
    {
        if (!OutputFileRegex().IsMatch(outputFileName))
            return Result<Problem>.Failure("Output file name must end with extension .out");
        OutputFileName = outputFileName;
        return Result<Problem>.Success(this);
    }

    public Result<Problem> UpdateYear(uint year)
    {
        if (year > DateTime.Now.Year + 1)
            return Result<Problem>.Failure("Problem year cannot be greater than current year!");
        Year = year;
        return Result<Problem>.Success(this);
    }
}