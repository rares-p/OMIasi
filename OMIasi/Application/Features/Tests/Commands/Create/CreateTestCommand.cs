using MediatR;

namespace Application.Features.Tests.Commands.Create;

public record CreateTestCommand(Guid problemId, string input, string output) : IRequest<CreateTestCommandResponse>;