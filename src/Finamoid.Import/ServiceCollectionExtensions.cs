using Finamoid.Import.Categories;
using Finamoid.Import.Mutations;
using IbanNet;
using IbanNet.Registry;
using Microsoft.Extensions.DependencyInjection;

namespace Finamoid.Import
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddImport(this IServiceCollection services)
        {
            return services
                .AddScoped<IBankStatementFormatDetector, BankStatementFormatDetector>()
                .AddScoped<IMutationImportService, MutationImportService>()
                .AddScoped<IMutationImporterFactory, MutationImporterFactory>()
                .AddScoped<ICategoryImportService, CategoryImportService>()
                .AddScoped<ICategoryImporter, CsvCategoryImporter>()
                .AddSingleton<IIbanParser, IbanParser>()
                .AddSingleton<IIbanRegistry>(IbanRegistry.Default);
        }
    }
}
