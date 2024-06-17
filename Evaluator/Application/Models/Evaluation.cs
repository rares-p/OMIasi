namespace Application.Models;

public class Evaluation
{
    public required Guid ProblemId { get; set; }
    public required string Solution { get; set; }
}