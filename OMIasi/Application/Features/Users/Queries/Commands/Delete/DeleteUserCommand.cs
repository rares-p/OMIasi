using MediatR;

namespace Application.Features.Users.Queries.Commands.Delete;

public record DeleteUserCommand(string Username) : IRequest<DeleteUserCommandResponse>;