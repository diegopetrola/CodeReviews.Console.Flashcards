//using Flashcards.DiegoPetrola.Controllers;
//using Flashcards.DiegoPetrola.Entities;
//using Flashcards.DiegoPetrola.Utils;
//using Microsoft.Extensions.DependencyInjection;
//using Spectre.Console;
//using static Flashcards.DiegoPetrola.Utils.Shared;

//namespace Flashcards.DiegoPetrola.Screens;

//internal class FlashcardScreen(ServiceProvider service) : FlashcardController(service)
//{

//    private async Task AddStack()
//    {
//        var name = AnsiConsole.Ask<string>("Type the stacks [bold]name[/]:");
//        var description = AnsiConsole.Ask<string>("Type the stacks [bold]description[/]:");

//        CardStack stack = new() { Name = name, Description = description };
//        try
//        {
//            await _cardStackService.AddStack(stack);
//            AnsiConsole.MarkupLine($"[{ColorHelper.success}]Card added.[/]");
//        }
//        catch (Exception e)
//        {
//            AnsiConsole.MarkupLine($"[{ColorHelper.error}]ERROR: {e.Message}[/]");
//        }
//        AskForKey();
//    }

//    private async Task EditStackScreen(CardStack stack)
//    {
//        stack.Name = AnsiConsole.Ask($"Type the new [{ColorHelper.bold}]name[/]", stack.Name);
//        stack.Description = AnsiConsole.Ask($"Type the new [{ColorHelper.bold}]description[/]", stack.Description);

//        try
//        {
//            await _cardStackService.UpdateStack(stack);
//            AnsiConsole.MarkupLine($"[{ColorHelper.success}]Stack updated.[/]");
//        }
//        catch (ArgumentException e)
//        {
//            AnsiConsole.MarkupLine($"[{ColorHelper.warning}] {e.Message} [/]");
//        }
//        catch (Exception e)
//        {
//            AnsiConsole.MarkupLine($"[{ColorHelper.error}] {e.Message} [/]");
//        }
//        AskForKey();
//    }
//}
