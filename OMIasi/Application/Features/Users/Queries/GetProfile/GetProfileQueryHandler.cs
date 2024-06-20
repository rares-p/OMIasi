using Application.Contracts.Repositories;
using MediatR;

namespace Application.Features.Users.Queries.GetProfile;

public class GetProfileQueryHandler(IUserRepository userRepository, ISubmissionRepository submissionRepository, IProblemRepository problemRepository) : IRequestHandler<GetProfileQuery, GetProfileQueryResponse>
{
    public async Task<GetProfileQueryResponse> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var userResult = await userRepository.FindByUserNameAsync(request.Username);

        if (!userResult.IsSuccess)
            return new GetProfileQueryResponse()
            {
                Success = false,
                Error = userResult.Error
            };

        var submissions = await submissionRepository.GetAllAsync();
        if (!submissions.IsSuccess)
            return new GetProfileQueryResponse()
            {
                Success = false,
                Error = submissions.Error
            };
        var solvedProblemsIds = submissions.Value.Where(s => s.UserId == userResult.Value.Id && s.Score == 100).Select(s => s.ProblemId).Distinct().ToList();
        var attemptedProblemsIds = submissions.Value.Where(s => s.UserId == userResult.Value.Id).Select(s => s.ProblemId).Distinct()
            .Where(problemId => !solvedProblemsIds.Contains(problemId));
        var solvedProblems = new List<string>();
        foreach (var problemId in solvedProblemsIds)
            solvedProblems.Add((await problemRepository.FindByIdAsync(problemId)).Value.Title);
        var attemptedProblems = new List<string>();
        foreach (var problemId in attemptedProblemsIds)
            attemptedProblems.Add((await problemRepository.FindByIdAsync(problemId)).Value.Title);
        return new GetProfileQueryResponse()
        {
            Success = true,
            Username = userResult.Value.Username,
            Role = userResult.Value.Role.ToString(),
            SolvedProblems = solvedProblems,
            AttemptedProblems = attemptedProblems,
        };
    }
}