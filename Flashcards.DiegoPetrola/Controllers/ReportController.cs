using Flashcards.DiegoPetrola.Services;
using Spectre.Console;

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
    }

    private async Task GenerateReport(MenuOptions option)
    {
        var studyReport = await studyService.GetStudyReport(DateTime.Now, DateTime.Now);
        var plot = new BarChart();
        foreach (var result in studyReport)
        {
            AnsiConsole.MarkupLine("");
        }
    }
}
