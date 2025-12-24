using Flashcards.DiegoPetrola.Entities;

namespace Flashcards.DiegoPetrola.Services;

public interface IFlashcardService
{
    Task AddFlashcard(Flashcard card);
    Task DeleteFlashcard(int Id);
    Task<List<Flashcard>> GetFlashcardsByStackId(int stackId, bool fakeId = true);
    Task UpdateCard(Flashcard card);
}
