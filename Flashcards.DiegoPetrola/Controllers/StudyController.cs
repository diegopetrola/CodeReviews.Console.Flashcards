using Flashcards.DiegoPetrola.Entities;
using Flashcards.DiegoPetrola.Services;
using Flashcards.DiegoPetrola.Utils;
using Spectre.Console;
using System.Runtime.InteropServices;
using static Flashcards.DiegoPetrola.Utils.Shared;

namespace Flashcards.DiegoPetrola.Controllers;

public class StudyController(CardStackService cardStackService, FlashcardService flashcardService)
{
    private enum MenuOptions
    {
        Correct,
        PartialyCorrect,
        Incorrect
    }
    private string MenuOptionsToString(MenuOptions option)
    {
        return option switch
        {
            MenuOptions.PartialyCorrect => "Partialy Correct",
            _ => option.ToString()
        };
    }
    public async Task MainScreen()
    {
        var stacks = await cardStackService.GetAllStacks();
        if (stacks.Count == 0)
        {
            AnsiConsole.MarkupInterpolated($"[{ColorHelper.warning}]No card stack, please create one first.[/]");
            return;
        }

        var selection = AnsiConsole.Prompt(
                new SelectionPrompt<CardStack>()
                .Title("Please choose the stack you want to study.")
                .AddChoices(stacks)
                .AddChoices(new CardStack { Name = goBack })
                .WrapAround(true)
                .UseConverter(s => s.Name)
            );

        if (selection.Name == goBack) return;
        else await StudyScreen(selection);
    }

    public async Task StudyScreen(CardStack stack)
    {
        var cards = await flashcardService.GetFlashcardsByStackIdDto(stack.Id);
        if (cards.Count == 0)
        {
            AnsiConsole.MarkupLine($"[{ColorHelper.warning}]No cards to study, please add some and try again.[/]");
            return;
        }
        Random.Shared.Shuffle(CollectionsMarshal.AsSpan(cards));
        var studySession = new StudySession
        {
            CardStackId = stack.Id,
            StartTime = DateTime.Now,
            TotalQuestions = cards.Count
        };

        foreach (var card in cards)
        {
            AnsiConsole.Clear();
            var questionPanel = GetStandardPanel($"{card.Question}", $"Question {card.DisplayId} of {cards.Count}");
            AnsiConsole.Write(questionPanel);
            AskForKey("Press any key to see the result...");

            var answerPanel = GetStandardPanel($"{card.Answer}", "Answer");
            AnsiConsole.Write(answerPanel);

            await GradingScreen(studySession);
        }
        await ResultsScreen(studySession);
    }

    private async Task GradingScreen(StudySession session)
    {
        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<MenuOptions>()
                .Title("How would you rate your answer?")
                .WrapAround(true)
                .AddChoices(Enum.GetValues<MenuOptions>())
                .UseConverter(MenuOptionsToString)
        );

        session.Score += selection switch
        {
            MenuOptions.Correct => 1,
            MenuOptions.PartialyCorrect => 0.5,
            _ => 0
        };
    }

    private async Task ResultsScreen(StudySession session)
    {
        AnsiConsole.Write(new Rule($"[{ColorHelper.success}]Results[/]"));
        AnsiConsole.WriteLine($"""
            You got {session.Score} out of {session.TotalQuestions} - {(session.Score / session.TotalQuestions) * 100}%
            """);
        //save session
    }
}
