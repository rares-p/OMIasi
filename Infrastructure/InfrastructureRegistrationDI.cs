using Application.Contracts.Repositories;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        return services;
    }
}