using Flashcards.DiegoPetrola.Entities;
using Flashcards.DiegoPetrola.Services;
using Flashcards.DiegoPetrola.Utils;
using Spectre.Console;
using static Flashcards.DiegoPetrola.Utils.Shared;

namespace Flashcards.DiegoPetrola.Controllers;

public class StackController(ICardStackService cardStackService, FlashcardController flashcardController)
{
    enum FlashcardMenu
    {
        ShowCards,
        AddNewCard,
        DeleteStack,
        EditStack,
        GoBack,
    }

    private static string FlashcardMenuToString(FlashcardMenu menu)
    {
        return menu switch
        {
            FlashcardMenu.ShowCards => "Show Cards",
            FlashcardMenu.AddNewCard => "Add New Card",
            FlashcardMenu.DeleteStack => "Delete Stack",
            FlashcardMenu.EditStack => "Edit Stack",
            FlashcardMenu.GoBack => "Go Back",
            _ => menu.ToString()
        };
    }

    public async Task ShowStacks()
    {
        var exit = false;
        while (!exit)
        {
            var stacks = await cardStackService.GetAllStacks();
            if (stacks.Count == 0)
                AnsiConsole.MarkupLine($"[{ColorHelper.warning}]Nothing to display, try creating some stacks![/]");
            AnsiConsole.Clear();
            AnsiConsole.WriteLine();
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<CardStack>()
                    .Title("Choose a stack to show more.")
                    .WrapAround(true)
                    .UseConverter(s => s.Name)
                    .AddChoices(stacks)
                    .AddChoices(new CardStack { Name = addNew })
                    .AddChoices(new CardStack { Name = goBack })
                );

            if (selection.Name == addNew) await AddStack();
            else if (selection.Name == goBack) exit = true;
            else { await ShowStack(selection); }
        }
    }

    private async Task ShowStack(CardStack stack)
    {
        var exit = false;
        Dictionary<FlashcardMenu, Func<Task>> options = new()
            {
               { FlashcardMenu.ShowCards, () => flashcardController.CardsFromStackScreen(stack)},
               { FlashcardMenu.AddNewCard, () => flashcardController.AddCardScreen(stack)},
               { FlashcardMenu.DeleteStack, () => DeleteStackScreen(stack)},
               { FlashcardMenu.EditStack, () => EditStackScreen(stack)},
               { FlashcardMenu.GoBack, () => { exit=true; return Task.CompletedTask; } }
            };
        while (!exit)
        {
            AnsiConsole.Clear();
            var panel = new Panel($"""
            [{ColorHelper.subtle}]Id:[/] {stack.Id}      [{ColorHelper.subtle}]Name:[/] {stack.Name}

            {stack.Description}

            [{ColorHelper.subtle}]Created:[/]{stack.CreatedAt:dd/MMM/yy}   [{ColorHelper.subtle}]Updated[/]:{stack.UpdatedAt:dd/MMM/yy}
            [{ColorHelper.subtle}]Contains:[/]{stack.Flashcards.Count} [{ColorHelper.subtle}]cards[/]
            """);
            panel.Border = BoxBorder.Rounded;
            panel.BorderColor(Color.DarkCyan);
            panel.Padding = new Padding(2, 0);
            panel.Header = new PanelHeader($"[{ColorHelper.bold}]Stack: {stack.Name}[/]").Centered();

            AnsiConsole.Write(panel);
            AnsiConsole.Write("\n\n");

            var selection = AnsiConsole.Prompt(
                    new SelectionPrompt<FlashcardMenu>()
                    .WrapAround(true)
                    .AddChoices(options.Keys)
                    .UseConverter(FlashcardMenuToString)
                );

            await options[selection]();
        }
    }

    private async Task AddStack()
    {
        var name = AnsiConsole.Ask<string>("Type the stacks [bold]name[/]:");
        var description = AnsiConsole.Ask<string>("Type the stacks [bold]description[/]:");

        CardStack stack = new() { Name = name, Description = description };
        try
        {
            await cardStackService.AddStack(stack);
            AnsiConsole.MarkupLine($"[{ColorHelper.success}]Card added.[/]");
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"[{ColorHelper.error}]ERROR: {e.Message}[/]");
        }
        AskForKey();
    }

    public async Task DeleteStackScreen(CardStack stack)
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
                await cardStackService.DeleteStack(stack);
                AnsiConsole.MarkupLine($"\n[{ColorHelper.warning}]Card deleted![/]\n");
            }
            catch (Exception e)
            {
                AnsiConsole.MarkupLine($"[{ColorHelper.error}]Error: {e.Message}[/]");
            }
        }
        AskForKey();
    }

    public async Task EditStackScreen(CardStack stack)
    {
        stack.Name = AnsiConsole.Ask($"Type the new [{ColorHelper.bold}]name[/]", stack.Name);
        stack.Description = AnsiConsole.Ask($"Type the new [{ColorHelper.bold}]description[/]", stack.Description);

        try
        {
            await cardStackService.UpdateStack(stack);
            AnsiConsole.MarkupLine($"[{ColorHelper.success}]Stack updated.[/]");
        }
        catch (ArgumentException e)
        {
            AnsiConsole.MarkupLine($"[{ColorHelper.warning}] {e.Message} [/]");
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"[{ColorHelper.error}] {e.Message} [/]");
        }
        AskForKey();
    }
}
