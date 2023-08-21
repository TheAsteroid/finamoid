using Finamoid.Aggregation;
using Finamoid.Categorization;
using Finamoid.Import;
using Microsoft.Extensions.Hosting;
using Spectre.Console;
using System.Diagnostics;
using System.IO;

namespace Finamoid.Cli
{
    internal class CliService : BackgroundService
    {
        private readonly IMutationImportService _mutationImportService;
        private readonly ICategoryImportService _categoryImportService;
        private readonly IMutationCategorizerService _mutationCategorizerService;
        private readonly IMutationAggregatorService _mutationAggregatorService;

        public CliService(
            IMutationImportService mutationImportService,
            ICategoryImportService categoryImportService,
            IMutationCategorizerService mutationCategorizerService,
            IMutationAggregatorService mutationAggregatorService)
        {
            _mutationImportService = mutationImportService;
            _categoryImportService = categoryImportService;
            _mutationCategorizerService = mutationCategorizerService;
            _mutationAggregatorService = mutationAggregatorService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                stoppingToken.ThrowIfCancellationRequested();

                var funcs = new Dictionary<string, Func<CancellationToken, Task<bool>>>
                {
                    { "import mutations", ImportMutationsAsync },
                    { "import categories", ImportCategoriesAsync },
                    { "categorize", CategorizeAsync },
                    { "aggregate", AggregateAsync },
                    { "exit", ExitAsync }
                };

                var selectionPrompt = new SelectionPrompt<string>()
                    .Title("What do you want to do?")
                    .AddChoices(funcs.Keys);

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

        private async Task<bool> ImportMutationsAsync(CancellationToken stoppingToken)
        {
            var path = AnsiConsole.Ask<string>("Please provide the directory containing the bank statements, or the path to a single file:");

            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                var fileCount = await _mutationImportService.ImportAndStoreAsync(path);
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

        private async Task<bool> ImportCategoriesAsync(CancellationToken stoppingToken)
        {
            var path = AnsiConsole.Ask<string>("Please provide the file path containing the categories:");

            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                await _categoryImportService.ImportAndStoreAsync(path);
                stopwatch.Stop();

                AnsiConsole.WriteLine($"Great news! The categories were imported in {stopwatch.ElapsedMilliseconds}ms.");

                return true;
            }
            catch (Exception ex)
            when (ex is IOException || ex is InvalidDataException || ex is InvalidOperationException)
            {
                AnsiConsole.WriteLine("Something went wrong when importing the categories. Details:");
                AnsiConsole.WriteLine(ex.ToString());
            }

            return !AnsiConsole.Confirm($"Do you want to try again?");
        }

        private async Task<bool> CategorizeAsync(CancellationToken stoppingToken)
        {
            DateTime? startDate = null;
            DateTime? endDate = null;

            if (AnsiConsole.Confirm("Do you want to start categorizing at a certain date? Select no if you want to start categorizing from the beginning of the history."))
            {
                startDate = AnsiConsole.Ask<DateTime?>("Please provide the start date in yyyy-mm-dd format:");
            }

            if (AnsiConsole.Confirm("Do you want to stop categorizing at a certain date? Select no if you want to categorize until the last available date."))
            {
                endDate = AnsiConsole.Ask<DateTime?>("Please provide the end date in yyyy-mm-dd format:");
            }

            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                await _mutationCategorizerService.CategorizeAndStoreAsync(startDate, endDate);
                stopwatch.Stop();

                AnsiConsole.WriteLine($"Great news! The categorization was done in {stopwatch.ElapsedMilliseconds}ms.");

                return true;
            }
            catch (Exception ex)
            when (ex is IOException || ex is InvalidDataException || ex is InvalidOperationException)
            {
                AnsiConsole.WriteLine("Something went wrong while categorizing. Details:");
                AnsiConsole.WriteLine(ex.ToString());
            }

            return !AnsiConsole.Confirm($"Do you want to try again?");
        }

        private async Task<bool> AggregateAsync(CancellationToken stoppingToken)
        {
            DateTime? startDate = null;
            DateTime? endDate = null;

            if (AnsiConsole.Confirm("Do you want to start aggregating at a certain date? Select no if you want to start aggregating from the beginning of the history."))
            {
                startDate = AnsiConsole.Ask<DateTime?>("Please provide the start date in yyyy-mm-dd format:");
            }

            if (AnsiConsole.Confirm("Do you want to stop aggregating at a certain date? Select no if you want to aggregate until the last available date."))
            {
                endDate = AnsiConsole.Ask<DateTime?>("Please provide the end date in yyyy-mm-dd format:");
            }

            var writeTonew = AnsiConsole.Confirm("Do you want to update the latest existing aggregation? Select no to create a new aggregation.");
            var periodType = AnsiConsole.Prompt(
                new SelectionPrompt<PeriodType>()
                .Title("Which period do you want to store?")
                .AddChoices(PeriodType.Month, PeriodType.Year, PeriodType.Week));

            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                await _mutationAggregatorService.AggregateAndStoreAsync(startDate, endDate, periodType, writeTonew);
                stopwatch.Stop();

                AnsiConsole.WriteLine($"Great news! The categorization was done in {stopwatch.ElapsedMilliseconds}ms.");

                return true;
            }
            catch (Exception ex)
            when (ex is IOException || ex is InvalidDataException || ex is InvalidOperationException)
            {
                AnsiConsole.WriteLine("Something went wrong while categorizing. Details:");
                AnsiConsole.WriteLine(ex.ToString());
            }

            return !AnsiConsole.Confirm($"Do you want to try again?");
        }

        private Task<bool> ExitAsync(CancellationToken stoppingToken)
        {
            AnsiConsole.WriteLine("Goodbye...");

            Environment.Exit(0);

            return Task.FromResult(true);
        }
    }
}
