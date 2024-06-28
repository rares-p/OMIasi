using Domain.Data;
using MediatR;

namespace Application.Features.Users.Queries.Commands.ChangeRole;

public class ChangeRoleCommand : IRequest<ChangeRoleCommandResponse>
{
    public string Username { get; set; }
    public UserRole Role { get; set; }
}