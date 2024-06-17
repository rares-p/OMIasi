using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Submissions.Commands.Create;

public class CreateSubmissionCommand : IRequest<CreateSubmissionCommandResponse>
{
    public Guid UserId { get; set; }
    [Required(ErrorMessage = "Problem Id is required")]
    public required Guid ProblemId { get; set; }
    [Required(ErrorMessage = "Source code is required")]
    public required string Solution { get; set; }
}