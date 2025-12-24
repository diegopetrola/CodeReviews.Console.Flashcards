namespace Flashcards.DiegoPetrola.DTOs;

public class StudySessionDto
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int CardStackId { get; set; }
    public TimeSpan Duration => EndTime - StartTime;
    public int Score { get; set; }
    public int TotalQuestions { get; set; }
}
