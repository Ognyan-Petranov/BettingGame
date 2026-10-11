using BettingGame;
using BettingGame.Factories.ActionProcessorFactory;
using BettingGame.Processors;
using BettingGame.Services.BetRewardCalculator;
using BettingGame.Services.InputReaders;
using BettingGame.Services.OutputWriter;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
RegisterServices(services);
var serviceProvider = services.BuildServiceProvider();

var gameEngine = BuildGameEngine();

try
{
    gameEngine.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}

void RegisterServices(IServiceCollection services)
{
    services.AddTransient<IConsoleInputReader, ConsoleInputReader>();
    services.AddTransient<IConsoleOutputWriter, ConsoleOutputWriter>();

    services.AddTransient<IActionProcessorFactory, ActionProcessorFactory>();
    services.AddTransient<IBetRewardCalculator, BetRewardCalculator>();

    services.AddScoped<IActionProcessor, DepositActionProcessor>();
    services.AddScoped<IActionProcessor, WithdrawActionProcessor>();
    services.AddScoped<IActionProcessor, BetActionProcessor>();
    services.AddScoped<IActionProcessor, ExitGameProcessor>();

    services.AddSingleton<Random>();
}

GameEngine BuildGameEngine()
{
    var inputReader = serviceProvider.GetService<IConsoleInputReader>() ?? throw new InvalidOperationException("Console input reader not found.");
    var outputWriter = serviceProvider.GetService<IConsoleOutputWriter>() ?? throw new InvalidOperationException("Console output writer not found.");
    var actionProcessorFactory = serviceProvider.GetService<IActionProcessorFactory>() ?? throw new InvalidOperationException("Action processor factory not found.");

    return new GameEngine(inputReader, actionProcessorFactory, outputWriter);
}