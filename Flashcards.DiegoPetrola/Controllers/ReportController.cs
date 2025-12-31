using Flashcards.DiegoPetrola.Services;
using Flashcards.DiegoPetrola.Utils;
using Spectre.Console;
using static Flashcards.DiegoPetrola.Utils.Shared;

namespace Flashcards.DiegoPetrola.Controllers;

public class ReportController(IStudyService studyService)
{
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
        var studyReport = await studyService.GetStudyReport(DateTime.Now.AddDays(-30), DateTime.Now);
        if (studyReport.Count == 0)
        {
            AnsiConsole.MarkupLine($"[{ColorHelper.warning}]Nothing to display.[/]");
        }
        else
        {
            var plot = new BarChart();
            foreach (var result in studyReport)
            {
                plot.AddItem($"{result.Year}/{result.Month} - {result.CardStackName}", result.Score);
            }
            AnsiConsole.Write(plot);
        }
        AskForKey();
    }
}
