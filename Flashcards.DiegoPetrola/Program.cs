//Challenge
//If you want to expand on this project, here’s an idea.
//Try to create a report system where you can see the number of sessions per month per stack.
//And another one with the average score per month per stack.
//This is not an easy challenge if you’re just getting started with databases,
//but it will teach you all the power of SQL and the possibilities it gives you to ask interesting questions from your tables.

using Flashcards.DiegoPetrola.Context;
using Flashcards.DiegoPetrola.Controllers;
using Flashcards.DiegoPetrola.Services;
using Flashcards.DiegoPetrola.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

IServiceCollection services = new ServiceCollection();
services.AddDbContext<FlashcardContext>();
services.AddTransient<IFlashcardService, FlashcardService>();
services.AddTransient<ICardStackService, CardStackService>();
services.AddTransient<IStudyService, StudyService>();
services.AddTransient<FlashcardController>();
services.AddTransient<StackController>();
services.AddTransient<ReportController>();
services.AddTransient<StudyController>();
services.AddTransient<MainMenuController>();

ServiceProvider serviceProvider = services.BuildServiceProvider();

await DatabaseSeeding.CustomSeeding();

var mainMenu = serviceProvider.GetService<MainMenuController>()!;
await mainMenu.ShowMainMenu();
