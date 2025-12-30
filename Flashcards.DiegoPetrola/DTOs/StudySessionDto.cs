namespace Flashcards.DiegoPetrola.DTOs;

//public class StudySessionDto
//{
//    public int Key { get; set; }
//    public DateTime StartTime { get; set; }
//    public DateTime EndTime { get; set; }
//    public string CardStack { get; set; } = "";
//    public TimeSpan Duration => EndTime - StartTime;
//    public int Score { get; set; }
//    public int TotalQuestions { get; set; }
//}

public class StudySessionDto
{
    public int Month { get; set; }
    public int Year { get; set; }
    public string CardStackName { get; set; } = "";
    public double Score { get; set; }
}