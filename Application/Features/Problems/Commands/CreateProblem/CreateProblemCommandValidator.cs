using Application.Features.Problems.Commands.CreateProblem.CreateProblemCommandModels;
using FluentValidation;

namespace Application.Features.Problems.Commands.CreateProblem;

public class CreateProblemCommandValidator : AbstractValidator<CreateProblemCommand>
{
    public CreateProblemCommandValidator()
    {
        RuleFor(problem => problem.Title).NotEmpty().WithMessage("Title is required");
        RuleFor(problem => problem.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(problem => problem.NoTests).NotEmpty().WithMessage("Number of tests is required");
        RuleFor(problem => problem.Author).NotEmpty().WithMessage("Author is required");
        RuleFor(problem => problem.TimeLimitInSeconds).NotEmpty().WithMessage("Time Limit In Seconds is required");
        RuleFor(problem => problem.TotalMemoryLimitInMb).NotEmpty().WithMessage("Total Memory Limit In Mb is required");
        RuleFor(problem => problem.StackMemoryLimitInMb).NotEmpty().WithMessage("Stack Memory Limit In Mb is required");
        RuleFor(problem => problem.Grade).NotEmpty().WithMessage("Grade is required");
        RuleFor(problem => problem.InputFileName).NotEmpty().WithMessage("Input File Name is required");
        RuleFor(problem => problem.OutputFileName).NotEmpty().WithMessage("Output File Name is required");
        RuleFor(problem => problem.Contest).NotEmpty().WithMessage("Contest is required");

        RuleFor(problem => problem.Tests).NotNull().WithMessage("Tests are required")
            .Must((problem, tests) => tests.Count == problem.NoTests)
            .WithMessage(problem => $"Number of tests must be {problem.NoTests}");
        RuleFor(problem => problem.Tests).Must(tests => tests.Sum(test => test.Score) == 100)
            .WithMessage("Test scores must sum up to 100");
        RuleFor(problem => problem.Tests).Must(tests =>
                !tests.Any(test => test.Input == null! || test.Input.Length == 0 || test.Output == null! || test.Output.Length == 0))
            .WithMessage("Input or Output files for tests cannot be empty");
        RuleFor(command => command.Tests)
            .NotNull().WithMessage("Problem Tests are required")
            .Must((command, tests) => AreIndicesSequential(tests, command.NoTests))
            .WithMessage("Test indices must be sequential from 0 to NoTests-1");
    }

    private bool AreIndicesSequential(IReadOnlyCollection<TestModel> tests, uint noTests)
    {
        if (tests == null!)
        {
            return false;
        }

        var indices = tests.Select(test => test.Index).OrderBy(index => index).ToList();

        for (var i = 0; i < noTests; i++)
            if (i != indices[i])
                return false;

        return true;
    }
}