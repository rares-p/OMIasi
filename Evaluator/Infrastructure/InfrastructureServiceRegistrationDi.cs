using Application.Contracts;
using Azure.Storage.Blobs;
using Infrastructure.Repositories;
using Infrastructure.Services;
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
        services.AddSingleton(new BlobContainerClient(
            "DefaultEndpointsProtocol=https;AccountName=omiiasi;AccountKey=4gKrBBFwBVf52jIKOIlxFqj/ps0PT/p8TyVBQKUVtjfEV8YgUJcmfgpwNf8uJulbC32Fc22XjwDw+ASt89LbmA==;BlobEndpoint=https://omiiasi.blob.core.windows.net/;TableEndpoint=https://omiiasi.table.core.windows.net/;QueueEndpoint=https://omiiasi.queue.core.windows.net/;FileEndpoint=https://omiiasi.file.core.windows.net/",
            "tests"));
    }
}