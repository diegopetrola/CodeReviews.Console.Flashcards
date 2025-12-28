using Flashcards.DiegoPetrola.DTOs;
using Flashcards.DiegoPetrola.Entities;

namespace Flashcards.DiegoPetrola.Services
{
    public interface IFlashcardService
    {
        Task AddFlashcard(Flashcard card);
        Task DeleteFlashcard(int Id);
        Task<List<Flashcard>> GetFlashcardsByStackId(int stackId);
        Task<List<FlashcardDto>> GetFlashcardsByStackIdDto(int stackId);
        Task UpdateCard(Flashcard card);
    }
}