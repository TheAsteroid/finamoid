using Finamoid.Import;
using Microsoft.Extensions.Hosting;
using Spectre.Console;
using System.Diagnostics;

namespace Finamoid.Cli
{
    internal class CliService : BackgroundService
    {
        private readonly IMutationImportService _mutationImportService;

        public CliService(IMutationImportService mutationImportService)
        {
            _mutationImportService = mutationImportService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                stoppingToken.ThrowIfCancellationRequested();

                var funcs = new Dictionary<string, Func<CancellationToken, Task<bool>>>
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

                AnsiConsole.WriteLine($"Ok, let's {action} stuff!");

                while (true)
                {
                    if (await funcs[action](stoppingToken))
                    {
                        break;
                    }
                }
            }
        }

        private async Task<bool> ImportAsync(CancellationToken stoppingToken)
        {
            var path = AnsiConsole.Ask<string>("Please provide the directory containing the bank statements, or the path to a single file:");

            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                var fileCount = await _mutationImportService.ImportAsync(path);
                stopwatch.Stop();

                if (fileCount > 0)
                {
                    AnsiConsole.WriteLine($"Great news! {fileCount} file(s) were imported in {stopwatch.ElapsedMilliseconds}ms.");

                    return true;
                }

                AnsiConsole.WriteLine("No files were found in this location.");
            }
            catch (Exception ex)
            when (ex is NotSupportedException || ex is IOException || ex is InvalidOperationException)
            {
                AnsiConsole.WriteLine("Something went wrong when importing the mutations. Details:");
                AnsiConsole.WriteLine(ex.ToString());
            }
            catch (Exception ex)
            when (ex is NotImplementedException)
            {
                AnsiConsole.WriteLine("Oops, this part of the code is not done yet. Details:");
                AnsiConsole.WriteLine(ex.ToString());
            }

            return !AnsiConsole.Confirm($"Do you want to try again?");
        }

        private Task<bool> CategorizeAsync(CancellationToken stoppingToken)
        {
            return Task.FromResult(true);
        }

        private Task<bool> AggregateAsync(CancellationToken stoppingToken)
        {
            return Task.FromResult(true);
        }

        private Task<bool> ExitAsync(CancellationToken stoppingToken)
        {
            AnsiConsole.WriteLine("Goodbye...");

            Environment.Exit(0);

            return Task.FromResult(true);
        }
    }
}
