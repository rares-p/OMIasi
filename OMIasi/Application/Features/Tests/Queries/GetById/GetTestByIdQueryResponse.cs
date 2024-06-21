using Application.Responses;

namespace Application.Features.Tests.Queries.GetById;

public class GetTestByIdQueryResponse : BaseResponse
{
    public string Input { get; set; }
    public string Output { get; set; }
}