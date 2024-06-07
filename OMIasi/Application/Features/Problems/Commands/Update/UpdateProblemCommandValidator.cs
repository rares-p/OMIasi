using FluentValidation;

namespace Application.Features.Problems.Commands.Update;

public class UpdateProblemCommandValidator : AbstractValidator<UpdateProblemCommand>
{
    public UpdateProblemCommandValidator()
    {
        RuleFor(problem => problem.Id).NotEmpty().WithMessage("Title is required");
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
        RuleFor(problem => problem.Year).NotEmpty().Must(year => year < DateTime.Now.Year + 1).WithMessage("Problem year cannot be greater than current year!");
    }
}