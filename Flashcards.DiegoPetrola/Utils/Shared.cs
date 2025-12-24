using Spectre.Console;

namespace Flashcards.DiegoPetrola.Utils;

public static class Shared
{
    public static readonly string addNew = $"[{ColorHelper.success}] + Add New[/]";
    public static readonly string goBack = $"[{ColorHelper.subtle}]<- Go Back[/]";

    public static void AskForKey()
    {
        AnsiConsole.MarkupLine($"\n[{ColorHelper.subtle}]Press any key to continue...[/]");
        Console.ReadKey(true);
    }
}
