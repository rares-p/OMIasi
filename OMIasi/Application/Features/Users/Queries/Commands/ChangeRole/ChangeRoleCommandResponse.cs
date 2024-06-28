using Application.Responses;

namespace Application.Features.Users.Queries.Commands.ChangeRole;

public class ChangeRoleCommandResponse : BaseResponse
{
    public string Role { get; set; }
}