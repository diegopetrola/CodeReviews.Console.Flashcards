using Flashcards.DiegoPetrola.Context;
using Flashcards.DiegoPetrola.Entities;
using Flashcards.DiegoPetrola.Utils;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.DiegoPetrola.Services;

public class CardStackService(FlashcardContext context) : ICardStackService
{
    private readonly FlashcardContext _context = context;

    public async Task<List<CardStack>> GetAllStacks()
    {
        List<CardStack> stacks = await _context.CardStacks.Include(s => s.Flashcards).ToListAsync() ?? [];
        return stacks;
    }

    public async Task UpdateStack(CardStack stack)
    {
        string validationMessage = ValidatorHelper.ValidateCardStack(stack);
        if (validationMessage != "") throw new ArgumentException(validationMessage);
        stack.UpdatedAt = DateTime.Now;
        try
        {
            _context.CardStacks.Update(stack);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw new Exception("Database error!");
        }
    }

    public async Task DeleteStack(CardStack stack)
    {
        var s = await _context.CardStacks.FindAsync(stack.Id)
            ?? throw new ArgumentException("Card not found, nothing was deleted");
        try
        {
            _context.CardStacks.Remove(s);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw new Exception("Database error!");
        }
    }

    public async Task AddStack(CardStack stack)
    {
        var s = await _context.CardStacks.Where(s => s.Id == stack.Id || s.Name == stack.Name).ToListAsync();
        if (s.Count > 0) throw new ArgumentException("Stack already exists.");
        await _context.CardStacks.AddAsync(stack);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw new Exception("Database error!");
        }
    }
}
