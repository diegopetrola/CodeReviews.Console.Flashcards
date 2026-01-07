using Flashcards.DiegoPetrola.Context;
using Flashcards.DiegoPetrola.Entities;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace Flashcards.DiegoPetrola.Utils;

public static class DatabaseSeeding
{
    private static readonly string _errorMsg =
        $"[{ColorHelper.error}]Error while seeding the database. The application might not work![/]";

    public static async Task CustomSeeding()
    {
        using var context = new FlashcardContext();
        if (context.CardStacks.Any()) return;

        var biologyStack = new CardStack { Name = "Biology", Description = "Biology questions.", };
        var csStack = new CardStack { Name = "Computer Science", Description = "CS questions.", };
        var physicsStack = new CardStack { Name = "Physics", Description = "Physics questions.", };
        List<CardStack> stacks = [biologyStack, csStack, physicsStack];
        await context.CardStacks.AddRangeAsync(stacks);
        await context.SaveChangesAsync();

        var card1 = new Flashcard { Question = "What is natural selection?", Answer = "The process that results in the continued existence of only the types of animals and plants that are best able to produce young or new plants in the conditions in which they live.", CardStackId = biologyStack.Id };
        var card2 = new Flashcard { Question = "What is evolution?", Answer = "The result of natural selection.", CardStackId = biologyStack.Id };
        var card3 = new Flashcard { Question = "What do DNA stands for?", Answer = "Deoxyribonucleic acid.", CardStackId = biologyStack.Id };
        List<Flashcard> cards = [card1, card2, card3];

        var card4 = new Flashcard { Question = "What is a stack?", Answer = "A stack is a linear data structure following the LIFO (Last-In, First-Out) principle.", CardStackId = csStack.Id };
        var card5 = new Flashcard { Question = "What is the ALU?", Answer = "Arithmetic-logic unit, the component of a processor responsible for doing mathematical and logical operations.", CardStackId = csStack.Id };
        var card6 = new Flashcard { Question = "Briefly explain the big O notation.", Answer = "Big O notation is used to classify algorithms according to how their run time or space requirements grow as the input size grows.", CardStackId = csStack.Id };
        cards.AddRange(card4, card5, card6);

        var card7 = new Flashcard { Question = "List the 2 postulates of special relativity.", Answer = "1. The laws of physics are identical in all inertial frames\n2. The speed of light in a vacuum is constant in all inertial frames.", CardStackId = physicsStack.Id };
        var card8 = new Flashcard { Question = "List the 3 laws of Newton.", Answer = "1. Inertia: an object at rest or moving at constant speed retains that speed until acted upon by a force.\n2. A force causes an accelaration according to the formula: F=m.a\n3. Action-Reaction: for every action (force), there is an equal and opposite reaction.", CardStackId = physicsStack.Id };
        var card9 = new Flashcard { Question = "Briefly explain the conclusions of Newton's shell theorem.", Answer = "For an object outside of a uniform sphere, all the gravitational force of that sphere can be counting as of coming from the center of it. For an object inside a spherically symmetric shell, there is no net gravitational force on it.", CardStackId = physicsStack.Id };
        cards.AddRange(card7, card8, card9);
        try
        {
            AnsiConsole.MarkupLine($"[{ColorHelper.warning}]Adding custom cards for the first run.[/]");
            await context.Flashcards.AddRangeAsync(cards);
            await context.SaveChangesAsync();
        }
        catch
        {
            AnsiConsole.MarkupLine(_errorMsg);
            throw;
        }

        List<StudySession> sessions = [];
        for (int i = 0; i < 100; i++)
        {
            var session = new StudySession
            {
                CardStackId = stacks[Random.Shared.Next(0, stacks.Count)].Id
            };
            session.StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            session.StartTime = session.StartTime.AddDays(-Random.Shared.Next(1, 60))
                .AddHours(Random.Shared.Next(8, 22))
                .AddMinutes(Random.Shared.Next(0, 59));
            session.EndTime = session.StartTime
                .AddHours(Random.Shared.Next(1, 5))
                .AddMinutes(Random.Shared.Next(1, 59));
            session.Score = Random.Shared.NextDouble();
            session.TotalQuestions = cards.Where(c => c.CardStackId == session.CardStackId).Count();
            sessions.Add(session);
        }
        try
        {
            AnsiConsole.MarkupLine($"[{ColorHelper.warning}]Adding custom study sessions for the first run.[/]");
            await context.StudySessions.AddRangeAsync(sessions);
            await context.SaveChangesAsync();
        }
        catch
        {
            AnsiConsole.MarkupLine(_errorMsg);
            throw;
        }
    }
}
