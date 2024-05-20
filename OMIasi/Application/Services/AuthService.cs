using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Contracts.Identity;
using Application.Contracts.Repositories;
using Application.Models.Identity;
using Domain.Common;
using Domain.Data;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class AuthService(IUserRepository userRepository, IConfiguration configuration) : IAuthService
{
    public async Task<Result<string>> Registration(RegistrationModel model)
    {
        if (await userRepository.ExistsAsync(model.Username))
            return Result<string>.Failure($"User with username {model.Username} already exists!");

        var userCreationResult = User.Create(model.Username, model.Password, model.Firstname, model.Lastname,
            model.Email, UserRole.Student);

        if (!userCreationResult.IsSuccess)
            return Result<string>.Failure(userCreationResult.Error);
        try
        {
            await userRepository.AddAsync(userCreationResult.Value);
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(ex.Message);
        }

        return Result<string>.Success(userCreationResult.Value.Id.ToString());
    }

    public async Task<Result<string>> Login(LoginModel model)
    {
        var user = await userRepository.FindByUserNameAsync(model.Username);
        if (!user.IsSuccess)
            return Result<string>.Failure($"User with username {model.Username} not found!");

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Value.Username),
            new(JwtRegisteredClaimNames.Jti, user.Value.Id.ToString()),
            new(ClaimTypes.Role, user.Value.Role.ToString()),
        };

        return Result<string>.Success(GenerateToken(claims));
    }

    //TODO: Implement server-side logout
    public Task<Result<string>> Logout()
    {
        return Task.FromResult(Result<string>.Success("Logged out successfully"));
    }

    public async Task<Result<string>> DeleteUser(string userId)
    {
        var result = await userRepository.DeleteAsync(new Guid(userId));

        return result.IsSuccess ? Result<string>.Success("User deleted successfully") : Result<string>.Failure(result.Error);
    }

    private string GenerateToken(IEnumerable<Claim> claims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = configuration["JWT:ValidIssuer"]!,
            Audience = configuration["JWT:ValidAudience"]!,
            Expires = DateTime.UtcNow.AddHours(5),
            SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(claims)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}