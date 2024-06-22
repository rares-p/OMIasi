namespace Application.Models;

public class TestResultModel
{
    public string Message { get; set; } = string.Empty;
    public uint Score { get; set; }
    public uint Runtime { get; set; }
    public uint TestIndex { get; set; }
}