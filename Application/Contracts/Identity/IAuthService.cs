using Application.Models.Identity;
using Domain.Common;

namespace Application.Contracts.Identity;

public interface IAuthService
{
    Task<Result<string>> Registration(RegistrationModel model);
    Task<Result<string>> Login(LoginModel model);
    Task<Result<string>> Logout();
    Task<Result<string>> DeleteUser(string userId);
}