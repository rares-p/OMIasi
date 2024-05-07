namespace Domain.Entities;

public class Submission(Guid userId, Guid problemId, uint score, DateTime date)
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; private set; } = userId;
    public Guid ProblemId { get; private set; } = problemId;
    public uint Score { get; private set; } = score;
    public DateTime Date { get; private set; } = date;
}