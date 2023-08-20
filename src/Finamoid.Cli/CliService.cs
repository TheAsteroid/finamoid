using Microsoft.Extensions.Hosting;
using Spectre.Console;

namespace Finamoid.Cli
{
    internal class CliService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)            
            {
                stoppingToken.ThrowIfCancellationRequested();

                var funcs = new Dictionary<string, Func<CancellationToken, Task>>
                {
                    { "import", ImportAsync },
                    { "categorize", CategorizeAsync },
                    { "aggregate", AggregateAsync },
                    { "exit", ExitAsync }
                };

                var selectionPrompt = new SelectionPrompt<string>
                {
                    Title = "What do you want to do?"
                };
                selectionPrompt.AddChoices(funcs.Keys);

                var action = AnsiConsole.Prompt(selectionPrompt);

                AnsiConsole.Clear();
                AnsiConsole.WriteLine($"Ok, let's {action} stuff!");

                await funcs[action](stoppingToken);
            }
        }

        private Task ImportAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }

        private Task CategorizeAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }

        private Task AggregateAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }

        private Task ExitAsync(CancellationToken stoppingToken)
        {
            AnsiConsole.WriteLine("Goodbye...");

            Environment.Exit(0);

            return Task.CompletedTask;
        }
    }
}
