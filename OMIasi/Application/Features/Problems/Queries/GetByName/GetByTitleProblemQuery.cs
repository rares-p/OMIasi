using MediatR;

namespace Application.Features.Problems.Queries.GetByName;

public record GetByTitleProblemQuery(string Title) : IRequest<ProblemDto>;