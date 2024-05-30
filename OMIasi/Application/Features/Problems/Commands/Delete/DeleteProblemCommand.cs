using MediatR;

namespace Application.Features.Problems.Commands.Delete;

public record DeleteProblemCommand(Guid ProblemId) : IRequest<DeleteProblemCommandResponse>;