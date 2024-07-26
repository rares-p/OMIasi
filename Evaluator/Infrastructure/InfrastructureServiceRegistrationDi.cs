using Application.Contracts;
using Azure.Storage.Blobs;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureServiceRegistrationDi
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OMIIasiDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString
                    ("OMIIasiDbConnection"),
                builder => builder.MigrationsAssembly("API")));

        services.AddScoped<IEvaluationService, EvaluationService>();
        services.AddScoped<ITestRepository, TestRepository>();
        services.AddScoped<IProblemRepository, ProblemRepository>();
        var blobStorageSettings = new BlobStorageSettings();
        configuration.GetSection("BlobStorage").Bind(blobStorageSettings);
        services.AddSingleton(new BlobContainerClient(blobStorageSettings.ConnectionString, blobStorageSettings.StorageName));

    }
}