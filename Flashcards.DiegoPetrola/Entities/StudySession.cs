namespace Flashcards.DiegoPetrola.Entities;

public class StudySession
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; } = DateTime.UtcNow;
    public DateTime EndTime { get; set; } = DateTime.UtcNow;
    public int CardStackId { get; set; }
    public CardStack CardStack { get; set; } = null!;
    public double Score { get; set; }
    public int TotalQuestions { get; set; }
}
