using Spectre.Console;

namespace Flashcards.DiegoPetrola.Controllers;

public class MainMenuController(StackController stackController, StudyController studyController, ReportController reportController)
{
    private enum MainMenuOptions
    {
        StartStudy,
        ShowStacks,
        SeeStudyHistory,
        Exit
    };
    private static string MenuOptionsToString(MainMenuOptions option)
    {
        return option switch
        {
            MainMenuOptions.StartStudy => "Start Study Session",
            MainMenuOptions.ShowStacks => "Show Stacks",
            MainMenuOptions.SeeStudyHistory => "See Study History",
            _ => option.ToString()
        };
    }
    public async Task ShowMainMenu()
    {
        while (true)
        {
            AnsiConsole.Clear();
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<MainMenuOptions>()
                    .Title("What would you like to do?")
                    .WrapAround(true)
                    .AddChoices(Enum.GetValues<MainMenuOptions>())
                    .UseConverter(MenuOptionsToString)
                );

            switch (selection)
            {
                case MainMenuOptions.StartStudy:
                    await studyController.MainScreen();
                    break;
                case MainMenuOptions.ShowStacks:
                    await stackController.ShowStacks();
                    break;
                case MainMenuOptions.SeeStudyHistory:
                    await reportController.MainScreen();
                    break;
                case MainMenuOptions.Exit:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}