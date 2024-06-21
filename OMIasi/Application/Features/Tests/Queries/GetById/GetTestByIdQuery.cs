using MediatR;

namespace Application.Features.Tests.Queries.GetById;

public record GetTestByIdQuery(Guid Id) : IRequest<GetTestByIdQueryResponse>;