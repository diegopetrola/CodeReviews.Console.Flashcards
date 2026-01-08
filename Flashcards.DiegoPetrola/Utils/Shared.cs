using Spectre.Console;
using System.Globalization;

namespace Flashcards.DiegoPetrola.Utils;

public static class Shared
{
    public static readonly string addNew = $"[{ColorHelper.success}] + Add New[/]";
    public static readonly string goBack = $"[{ColorHelper.subtle}]<- Go Back[/]";
    public static string dateFormat = "dd/MM/yy";

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

    public static bool ValidateStringDate(string dateString, out DateTime date)
    {
        var isValid = DateTime.TryParseExact(
                    dateString,
                    dateFormat,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out date);

        return isValid;
    }

    public static DateTime AskDate(string message)
    {
        var date = DateTime.Now;
        AnsiConsole.Prompt(new TextPrompt<string>(message)
            .Validate(input =>
                ValidateStringDate(input, out date) ?
                ValidationResult.Success() : ValidationResult.Error("Invalid date!")));

        return date;
    }
}
