namespace Application.Features.Submissions;

public class SubmissionDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Solution { get; set; } = string.Empty;
    public List<SubmissionTestDto> Scores { get; set; }
}