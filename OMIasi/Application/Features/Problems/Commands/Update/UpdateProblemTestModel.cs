namespace Application.Features.Problems.Commands.Update;

public class UpdateProblemTestModel
{
    public Guid? Id { get; set; }
    public uint Index { get; set; }
    public string? Input { get; set; }
    public string? Output { get; set; }
    public uint Score { get; set; }
}