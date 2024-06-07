namespace Application.Responses;

public class BaseResponse
{
    public BaseResponse() => Success = true;

    public BaseResponse(string error, bool success)
    {
        Success = success;
        Error = error;
    }

    public bool Success { get; set; }
    public string Error { get; set; } = string.Empty;

    public List<string>? ValidationsErrors { get; set; }
}