namespace Flashcards.DiegoPetrola.DTOs;

public class StudySessionDto
{
    public int Month { get; set; }
    public int Year { get; set; }
    public string CardStackName { get; set; } = "";
    public double Score { get; set; }
    public int Count { get; set; }
}