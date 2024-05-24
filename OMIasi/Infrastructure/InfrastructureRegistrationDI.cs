using System.Text;
using Application.Contracts.Identity;
using Application.Contracts.Repositories;
using Domain.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class InfrastructureRegistrationDi
{
    public static IServiceCollection AddInfrastructureToDi(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OMIIasiDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString
                    ("OMIIasiDbConnection"),
                builder => builder.MigrationsAssembly("API")));
        services.AddScoped
        (typeof(IAsyncRepository<>),
            typeof(BaseRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProblemRepository, ProblemRepository>();
        services.AddScoped<ITestRepository, TestRepository>();
        services.AddScoped<ISubmissionRepository, SubmissionRepository>();

        services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();


        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                };
            });

        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}