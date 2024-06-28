using Application.Contracts;
using Azure.Storage.Blobs;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TestRepository(OMIIasiDbContext context, BlobContainerClient blobContainer) : ITestRepository
{
    public async Task<Result<IReadOnlyList<Test>>> GetTestsByProblemIdAsync(Guid id)
    {
        var result = await context.Tests.Where(test => test.ProblemId == id).ToListAsync();
        return result == null ? Result<IReadOnlyList<Test>>.Failure("No tests found!") : Result<IReadOnlyList<Test>>.Success(result);
    }

    public async Task<(string input, string output)> GetTest(Guid problemId, Guid testId)
    {
        var inputBlob = await blobContainer.GetBlobClient($"{problemId}/{testId}.in").DownloadAsync();

        using var inputReader = new StreamReader(inputBlob.Value.Content);
        string input = await inputReader.ReadToEndAsync();


        var outputBlob = await blobContainer.GetBlobClient($"{problemId}/{testId}.ok").DownloadAsync();

        using var outputReader = new StreamReader(outputBlob.Value.Content);
        string output = await outputReader.ReadToEndAsync();


        return (input, output);
    }
}