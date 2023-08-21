using Finamoid.Import.Readers;
using Finamoid.Import.Writers;
using IbanNet;
using IbanNet.Registry;
using Microsoft.Extensions.DependencyInjection;

namespace Finamoid.Import
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMutationImport(this IServiceCollection services)
        {
            return services
                .AddScoped<IMutationImportService, MutationImportService>()
                .AddScoped<IBankStatementFormatDetector, BankStatementFormatDetector>()
                .AddScoped<IMutationWriter, JsonMutationWriter>()
                .AddScoped<IJsonMutationReader, JsonMutationReader>()
                .AddScoped<IRawMutationReaderFactory, RawMutationReaderFactory>()
                .AddSingleton<IIbanParser, IbanParser>()
                .AddSingleton<IIbanRegistry>(IbanRegistry.Default);
        }
    }
}
