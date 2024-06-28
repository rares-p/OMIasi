using System.Text.Json.Serialization;

namespace Application.Models;

public class TestResultModel
{
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("runtime")]
    public uint Runtime { get; set; }

    [JsonPropertyName("score")]
    public uint Score { get; set; }

    [JsonPropertyName("testIndex")]
    public uint TestIndex { get; set; }
}