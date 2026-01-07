using Flashcards.DiegoPetrola.Services;
using Flashcards.DiegoPetrola.Utils;
using Spectre.Console;
using static Flashcards.DiegoPetrola.Utils.Shared;

namespace Flashcards.DiegoPetrola.Controllers;

public class ReportController(IStudyService studyService)
{
    private static readonly List<Color> colors = [
                Color.Aqua,
                Color.DarkMagenta,
                Color.IndianRed,
                Color.HotPink,
                Color.CadetBlue,
                Color.Cornsilk1,
            ];
    public enum MenuOptions
    {
        ByMonth,
        ByWeek,
        TotalHours,
        Count,
    }
    private string MenuOptionsToString(MenuOptions option)
    {
        return option switch
        {
            MenuOptions.ByMonth => "By Month",
            MenuOptions.ByWeek => "By Week",
            MenuOptions.TotalHours => "Total Hours",
            _ => option.ToString()
        };
    }
    public async Task MainScreen()
    {
        var selection = AnsiConsole.Prompt(
                new SelectionPrompt<MenuOptions>()
                .Title("How would you like to view your report?")
                .AddChoices([MenuOptions.ByMonth, MenuOptions.ByWeek])
                .WrapAround(true)
                .UseConverter(MenuOptionsToString)
            );

        await GenerateReport(selection);
    }

    private async Task GenerateReport(MenuOptions option)
    {
        AnsiConsole.Clear();
        var startDate = AskDate($"Please type the start date (format=[{ColorHelper.subtle}]{dateFormat}[/]):");
        var endDate = AskDate($"\nPlease type the end date (format=[{ColorHelper.subtle}]{dateFormat}[/]):");

        var studyReport = await studyService.GetStudyReport(startDate, endDate);
        if (studyReport.Count == 0)
        {
            AnsiConsole.MarkupLine($"[{ColorHelper.warning}]Nothing to display.[/]");
        }
        else
        {
            var plot = new BarChart().Label($"[{ColorHelper.subtle}]Study Report[/] [bold] - Average Score[/]")
                .UseValueFormatter(v => v.ToString("F2"));
            for (var i = 0; i < studyReport.Count; i++)
            {
                plot.AddItem($"[{ColorHelper.subtle}]{studyReport[i].Year}/{studyReport[i].Month}[/] - {studyReport[i].CardStackName}",
                    studyReport[i].Score, colors[i % colors.Count]);
            }
            AnsiConsole.Write(plot);

            AnsiConsole.WriteLine();

            var plotCount = new BarChart().Label($"[{ColorHelper.subtle}]Study Report[/] [bold] - Number of Sessions[/]")
                .UseValueFormatter(v => v.ToString("F0"));
            for (var i = 0; i < studyReport.Count; i++)
            {
                plotCount.AddItem($"[{ColorHelper.subtle}]{studyReport[i].Year}/{studyReport[i].Month}[/] - {studyReport[i].CardStackName}",
                    studyReport[i].Count, colors[i % colors.Count]);
            }
            AnsiConsole.Write(plotCount);
        }
        AskForKey();
    }
}
