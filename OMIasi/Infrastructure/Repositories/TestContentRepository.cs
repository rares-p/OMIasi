using Application.Contracts.Repositories;
using Azure.Storage.Blobs;

namespace Infrastructure.Repositories;

public class TestContentRepository(BlobContainerClient blobContainer) : ITestContentRepository
{
    public async Task<bool> CreateTest(Guid problemId, Guid testId, string input, string output)
    {
        //var blobContainer = blobService.GetBlobContainerClient(_container);
        //if (blobContainer == null)
        //    return false;
        var inputResponse = await blobContainer.UploadBlobAsync($"{problemId}/{testId}.in",
            new MemoryStream(System.Text.Encoding.UTF8.GetBytes(input)));
        if (inputResponse == null) return false;
        var outputResponse = await blobContainer.UploadBlobAsync($"{problemId}/{testId}.ok",
            new MemoryStream(System.Text.Encoding.UTF8.GetBytes(output)));
        if (outputResponse == null)
        {
            await blobContainer.GetBlobClient($"{problemId}/{testId}.in").DeleteIfExistsAsync();
            return false;
        }

        return true;
    }

    public async Task<bool> UpdateInput(Guid problemId, Guid testId, string input)
    {
        var inputResponse = await blobContainer.UploadBlobAsync($"{problemId}/{testId}.in",
            new MemoryStream(System.Text.Encoding.UTF8.GetBytes(input)));
        if (inputResponse == null) return false;
        return true;
    }

    public async Task<bool> UpdateOutput(Guid problemId, Guid testId, string output)
    {
        var outputResponse = await blobContainer.UploadBlobAsync($"{problemId}/{testId}.ok",
            new MemoryStream(System.Text.Encoding.UTF8.GetBytes(output)));
        if (outputResponse == null) return false;
        return true;
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

    public async Task<bool> DeleteTest(Guid problemId, Guid testId)
    {
        await blobContainer.GetBlobClient($"{problemId}/{testId}.in").DeleteAsync();
        await blobContainer.GetBlobClient($"{problemId}/{testId}.ok").DeleteAsync();
        return false;
    }
}