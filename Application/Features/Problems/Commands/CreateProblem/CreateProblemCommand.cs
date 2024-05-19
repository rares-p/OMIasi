using MediatR;
using System.ComponentModel.DataAnnotations;
using Application.Features.Problems.Commands.CreateProblem.CreateProblemCommandModels;

namespace Application.Features.Problems.Commands.CreateProblem;

public class CreateProblemCommand : IRequest<CreateProblemCommandResponse>
{
    [Required(ErrorMessage = "Problem Title is required")]
    public required string Title { get; set; }
    [Required(ErrorMessage = "Problem Description is required")]
    public required string Description { get; set; }
    [Required(ErrorMessage = "Number of tests is required")]
    public uint NoTests { get; set; }
    [Required(ErrorMessage = "Author is required")]
    public required string Author { get; set; }
    [Required(ErrorMessage = "TimeLimit is required")]
    public float TimeLimitInSeconds { get; set; }
    [Required(ErrorMessage = "Total Memory is required")]
    public float TotalMemoryLimitInMb { get; set; }
    [Required(ErrorMessage = "Stack Memory is required")]
    public float StackMemoryLimitInMb { get; set; }
    [Required(ErrorMessage = "Grade is required")]
    public uint Grade { get; set; }
    [Required(ErrorMessage = "Input File Name is required")]
    public required string InputFileName { get; set; }
    [Required(ErrorMessage = "Output File Name is required")]
    public required string OutputFileName { get; set; }
    [Required(ErrorMessage = "Problem year is required")]
    public required string Contest { get; set; }
    //[Required(ErrorMessage = "Problem Tests are required")]
    public List<TestModel> Tests { get; set; }
}
