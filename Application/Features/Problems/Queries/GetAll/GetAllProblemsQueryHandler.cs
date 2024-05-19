using Application.Contracts.Repositories;
using MediatR;

namespace Application.Features.Problems.Queries.GetAll;

public class GetAllProblemsQueryHandler(IProblemRepository repository) : IRequestHandler<GetAllProblemsQuery, GetAllProblemsResponse>
{
    public async Task<GetAllProblemsResponse> Handle(GetAllProblemsQuery request, CancellationToken cancellationToken)
    {
        var response = new GetAllProblemsResponse();
        var result = await repository.GetAllAsync();

        if (result.IsSuccess)
        {
            response.Problems = result.Value.Select(problem => new ProblemDto()
            {
                Id = problem.Id,
                Title = problem.Title,
                Author = problem.Author,
                Contest = problem.Contest,
                Description = problem.Description,
                Grade = problem.Grade,
                InputFileName = problem.InputFileName,
                OutputFileName = problem.OutputFileName,
                NoTests = problem.NoTests,
                StackMemoryLimitInMb = problem.StackMemoryLimitInMb,
                TotalMemoryLimitInMb = problem.TotalMemoryLimitInMb,
                TimeLimitInSeconds = problem.TimeLimitInSeconds

            }).ToList();
        }

        return response;
    }
}
