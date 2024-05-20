using MediatR;

namespace Application.Features.Problems.Queries.GetById;

public record GetByIdProblemQuery(Guid Id) : IRequest<ProblemDto>;