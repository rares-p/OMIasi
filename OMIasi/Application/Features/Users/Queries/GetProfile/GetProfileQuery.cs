using MediatR;

namespace Application.Features.Users.Queries.GetProfile;

public record GetProfileQuery(string Username) : IRequest<GetProfileQueryResponse>;