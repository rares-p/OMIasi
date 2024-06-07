using Domain.Common;
using MediatR;

namespace Application.Features.Problems.Queries.GetProblemAndTestsById;

public record GetProblemAndTestsByIdQuery(Guid ProblemId) : IRequest<Result<GetProblemAndTestsByIdQueryResponse>>;