using MediatR;

namespace Application.Features.Tests.Queries.GetAll;

public record GetAllTestsQuery(Guid ProblemId) : IRequest<List<TestDto>>;