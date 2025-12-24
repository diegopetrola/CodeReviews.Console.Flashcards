using Spectre.Console;

namespace Flashcards.DiegoPetrola.Controllers;

public class MainMenuController(StackController stackController)
{
    private enum MainMenuOptions
    {
        StartStudy,
        ShowStacks,
        Exit
    };
    private static string MenuOptionsToString(MainMenuOptions options)
    {
        return options switch
        {
            MainMenuOptions.StartStudy => "Start Study Session",
            MainMenuOptions.ShowStacks => "Show Stacks",
            _ => options.ToString()
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
                    // TODO StudyController
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