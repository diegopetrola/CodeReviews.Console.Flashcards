namespace Flashcards.DiegoPetrola.Entities;

public class Flashcard
{
    public int Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public int CardStackId { get; set; }
    public CardStack CardStack { get; set; } = null!;
}
