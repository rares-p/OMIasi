namespace Application.Features.Problems.Commands.Update;

public class UpdateProblemTestModel
{
    public Guid? Id { get; set; }
    public uint Index { get; set; }
    public byte[] Input { get; set; }
    public byte[] Output { get; set; }
    public uint Score { get; set; }
}