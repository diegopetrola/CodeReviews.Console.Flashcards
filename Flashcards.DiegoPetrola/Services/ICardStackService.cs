using Flashcards.DiegoPetrola.Entities;

namespace Flashcards.DiegoPetrola.Services;

public interface ICardStackService
{
    Task<List<CardStack>> GetAllStacks();
    Task AddStack(CardStack stack);
    Task DeleteStack(CardStack stack);
    Task UpdateStack(CardStack stack);
}
