using Spectre.Console;

namespace Flashcards.DiegoPetrola.Utils;

public static class Shared
{
    public static readonly string addNew = $"[{ColorHelper.success}] + Add New[/]";
    public static readonly string goBack = $"[{ColorHelper.subtle}]<- Go Back[/]";

    public static Panel GetStandardPanel(string bodyText, string header)
    {
        var panel = new Panel(bodyText);
        panel.Header = new PanelHeader(header).Centered();
        panel.Border = BoxBorder.Rounded;
        panel.BorderColor(Color.DarkCyan);
        return panel;
    }

    public static void AskForKey(string message = "\nPress any key to continue...")
    {
        AnsiConsole.MarkupLine($"\n[{ColorHelper.subtle}]{message}[/]");
        Console.ReadKey(true);
    }
}
