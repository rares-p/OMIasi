namespace Application.Features.Submissions;

public class SubmissionTestDto
{
    public Guid Id { get; set; }
    public string Message { get; set; }
    public uint Score { get; set; }
    public uint Runtime { get; set; }
    public uint TestIndex { get; set; }
}