using Application.Contracts.Repositories;
using MediatR;

namespace Application.Features.Users.Queries.Commands.ChangeRole;

public class ChangeRoleCommandHandler(IUserRepository userRepository) : IRequestHandler<ChangeRoleCommand, ChangeRoleCommandResponse>
{
    public async Task<ChangeRoleCommandResponse> Handle(ChangeRoleCommand request, CancellationToken cancellationToken)
    {
        var userResponse = await userRepository.FindByUserNameAsync(request.Username);
        if (!userResponse.IsSuccess)
            return new ChangeRoleCommandResponse()
            {
                Success = false,
                Error = userResponse.Error
            };

        var updatedUser = userResponse.Value.UpdateRole(request.Role);
        if (!updatedUser.IsSuccess)
            return new ChangeRoleCommandResponse()
            {
                Success = false,
                Error = updatedUser.Error
            };

        try
        {
            await userRepository.UpdateAsync(updatedUser.Value);
        }
        catch (Exception e)
        {
            return new ChangeRoleCommandResponse()
            {
                Success = false,
                Error = "Unknown error encountered, please try again later"
            };
        }

        return new ChangeRoleCommandResponse()
        {
            Success = true,
            Error = "",
            Role = updatedUser.Value.Role.ToString(),
        };
    }
}