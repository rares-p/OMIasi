using Application.Contracts.Repositories;
using MediatR;

namespace Application.Features.Problems.Queries.GetByName;

public class GetByTitleProblemQueryHandler(IProblemRepository repository) : IRequestHandler<GetByTitleProblemQuery, ProblemDto>
{
    public async Task<ProblemDto> Handle(GetByTitleProblemQuery request, CancellationToken cancellationToken)
    {
        //var problem = await repository.F(request.Title);

        //if (problem.IsSuccess)
        //    return new ProblemDto()
        //    {
        //        Id = problem.Value.Id,
        //        Title = problem.Value.Title,
        //        Author = problem.Value.Author,
        //        Contest = problem.Value.Contest,
        //        Description = problem.Value.Description,
        //        Grade = problem.Value.Grade,
        //        InputFileName = problem.Value.InputFileName,
        //        OutputFileName = problem.Value.OutputFileName,
        //        NoTests = problem.Value.NoTests,
        //        StackMemoryLimitInMb = problem.Value.StackMemoryLimitInMb,
        //        TotalMemoryLimitInMb = problem.Value.TotalMemoryLimitInMb,
        //        TimeLimitInSeconds = problem.Value.TimeLimitInSeconds
        //    };
        return new ProblemDto();
    }
}