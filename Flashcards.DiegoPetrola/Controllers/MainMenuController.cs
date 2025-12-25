using Spectre.Console;

namespace Flashcards.DiegoPetrola.Controllers;

public class MainMenuController(StackController stackController, StudyController studyController)
{
    private enum MainMenuOptions
    {
        StartStudy,
        ShowStacks,
        Exit
    };
    private static string MenuOptionsToString(MainMenuOptions option)
    {
        return option switch
        {
            MainMenuOptions.StartStudy => "Start Study Session",
            MainMenuOptions.ShowStacks => "Show Stacks",
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
                case MainMenuOptions.Exit:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}