using Flashcards.DiegoPetrola.Context;
using Flashcards.DiegoPetrola.Entities;
using Flashcards.DiegoPetrola.Utils;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.DiegoPetrola.Services;

public class FlashcardService(FlashcardContext flashcardContext) : IFlashcardService
{
    private readonly FlashcardContext _context = flashcardContext;

    public async Task AddFlashcard(Flashcard card)
    {
        var validationMessage = ValidatorHelper.ValidateFlashard(card);

        if (validationMessage != "")
        {
            throw new ArgumentException(validationMessage);
        }
        try
        {
            await _context.Flashcards.AddAsync(card);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception($"Database error: {e.Message}");
        }
    }

    public async Task DeleteFlashcard(int Id)
    {
        var card = await _context.Flashcards.FindAsync(Id) ?? throw new Exception("Card not found.");
        _context.Flashcards.Remove(card);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception($"Database error: {e.Message}");
        }
    }

    public async Task UpdateCard(Flashcard card)
    {
        var validationMessage = ValidatorHelper.ValidateFlashard(card);
        if (validationMessage != "") throw new ArgumentException(validationMessage);

        card.UpdatedAt = DateTime.Now;
        try
        {
            _context.Flashcards.Update(card);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception($"Database error: {e.Message}");
        }
    }

    public async Task<List<Flashcard>> GetFlashcardsByStackId(int stackId, bool fakeId = true)
    {
        try
        {
            var cards = await _context.Flashcards.Where(f => f.CardStackId == stackId).ToListAsync();
            return cards;
        }
        catch (Exception e)
        {
            throw new Exception($"Database error: {e.Message}");
        }
    }
}

