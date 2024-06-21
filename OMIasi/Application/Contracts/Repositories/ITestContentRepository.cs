namespace Application.Contracts.Repositories;

public interface ITestContentRepository
{
    public Task<bool> CreateTest(Guid problemId, Guid testId, string input, string output);
    public Task<bool> UpdateInput(Guid problemId, Guid testId, string input);
    public Task<bool> UpdateOutput(Guid problemId, Guid testId, string output);
    public Task<(string input, string output)> GetTest(Guid problemId, Guid testId);
    public Task<bool> DeleteTest(Guid problemId, Guid testId);
}