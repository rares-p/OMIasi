using Application.Contracts.Repositories;
using MediatR;

namespace Application.Features.Users.Queries.Commands.Delete;

public class DeleteUserCommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand, DeleteUserCommandResponse>
{
    public async Task<DeleteUserCommandResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var userResult = await userRepository.FindByUserNameAsync(request.Username);

        if (!userResult.IsSuccess)
            return new DeleteUserCommandResponse()
            {
                Success = false,
                Error = userResult.Error
            };

        try
        {
            await userRepository.DeleteAsync(userResult.Value.Id);
        }
        catch (Exception e)
        {
            return new DeleteUserCommandResponse()
            {
                Success = false,
                Error = "Unknown error encountered, please try again later"
            };
        }

        return new DeleteUserCommandResponse()
        {
            Success = true,
            Error = ""
        };
    }
}