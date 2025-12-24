using Flashcards.DiegoPetrola.Entities;
using Flashcards.DiegoPetrola.Services;
using Flashcards.DiegoPetrola.Utils;
using Spectre.Console;
using static Flashcards.DiegoPetrola.Utils.Shared;

namespace Flashcards.DiegoPetrola.Controllers;

public class FlashcardController(IFlashcardService _flashcardService)
{
    private enum FlashcardMenu
    {
        Edit,
        Delete,
        GoBack
    }

    private static string FlashcardMenuToString(FlashcardMenu option)
    {
        return option switch
        {
            FlashcardMenu.Delete => "Delete Card",
            FlashcardMenu.Edit => "Edit Card",
            FlashcardMenu.GoBack => "Go Back",
            _ => option.ToString()
        };
    }

    public async Task AddCardScreen(CardStack cardStack)
    {
        var exit = false;
        while (!exit)
        {
            var question = AnsiConsole.Ask<string>("Type the card's [bold]question[/]: ");
            var answer = AnsiConsole.Ask<string>("Type the card's [bold]answer[/]: ");
            Flashcard card = new() { Answer = answer, Question = question, CardStackId = cardStack.Id };
            try
            {
                await _flashcardService.AddFlashcard(card);
                AnsiConsole.MarkupLine($"[{ColorHelper.success}]Card added![/]");
                exit = true;
            }
            catch (ArgumentException e)
            {
                AnsiConsole.MarkupLine($"[{ColorHelper.error}] Error: {e.Message}[/]");
            }
            catch (Exception)
            {
                throw;
            }
            AskForKey();
        }
    }

    public async Task CardsFromStackScreen(CardStack stack)
    {
        var exit = false;
        while (!exit)
        {
            AnsiConsole.Clear();
            var flashcards = await _flashcardService.GetFlashcardsByStackId(stack.Id, false);
            if (flashcards.Count == 0)
                AnsiConsole.MarkupLine($"[{ColorHelper.warning}]This stack is empty.\n[/]");

            var selection = AnsiConsole.Prompt(
                    new SelectionPrompt<Flashcard>()
                    .Title("Choose a flashcard to show more:")
                    .WrapAround(true)
                    .AddChoices(flashcards)
                    .AddChoices(new Flashcard { Question = addNew })
                    .AddChoices(new Flashcard { Question = goBack })
                    .UseConverter(c => c.Question)
                );

            if (selection.Question == addNew) await AddCardScreen(stack);
            else if (selection.Question == goBack) exit = true;
            else await ShowFlashcard(selection);
        }
    }

    private async Task ShowFlashcard(Flashcard card)
    {
        Dictionary<FlashcardMenu, Func<Task>> options = new()
            {
               { FlashcardMenu.Edit, () => EditCardScreen(card) },
               { FlashcardMenu.Delete, () => DeleteCardScreen(card)},
               { FlashcardMenu.GoBack, () => { return Task.CompletedTask; } }
            };

        var panel = new Panel($"""
            [{ColorHelper.subtle}]Id:[/] {card.Id}      
            [{ColorHelper.subtle}]Question:[/] {card.Question}
            [{ColorHelper.subtle}]Answer:[/] {card.Answer}
            """);
        panel.Border = BoxBorder.Rounded;
        panel.BorderColor(Color.DarkCyan);
        panel.Padding = new Padding(2, 0);
        panel.Header = new PanelHeader($"[{ColorHelper.bold}]Card Details[/]").Centered();

        AnsiConsole.Write(panel);
        AnsiConsole.Write("\n\n");

        var selection = AnsiConsole.Prompt(
                new SelectionPrompt<FlashcardMenu>()
                .AddChoices(options.Keys)
                .WrapAround(true)
                .UseConverter(FlashcardMenuToString)
            );

        await options[selection]();
    }

    private async Task EditCardScreen(Flashcard card)
    {
        var question = AnsiConsole.Ask("Type the card's new [bold]question[/] (leave empty for no change)", card.Question);
        card.Question = question == "" ? card.Question : question;
        var answer = AnsiConsole.Ask("Type the card's new [bold]answer[/] (leave empty for no change)", card.Answer);
        card.Answer = answer == "" ? card.Answer : answer;

        try
        {
            await _flashcardService.UpdateCard(card);
            AnsiConsole.MarkupLine($"\n[green3]Update completed![/]\n");
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"[red]Error: {e.Message}[/]");
        }
        AskForKey();
    }

    private async Task DeleteCardScreen(Flashcard card)
    {
        var confirm = AnsiConsole.Prompt(
                new SelectionPrompt<bool>()
                .Title($"Are you sure? [{ColorHelper.warning}]This operation is irreverssible.[/]")
                .AddChoices([false, true])
                .UseConverter(b => b ? "Yes" : "No")
            );

        if (confirm)
        {
            try
            {
                await _flashcardService.DeleteFlashcard(card.Id);
                AnsiConsole.MarkupLine($"\n[{ColorHelper.warning}]Card deleted![/]\n");
            }
            catch (Exception e)
            {
                AnsiConsole.MarkupLine($"[{ColorHelper.error}]Error: {e.Message}[/]");
            }
        }
        AskForKey();
    }
}
