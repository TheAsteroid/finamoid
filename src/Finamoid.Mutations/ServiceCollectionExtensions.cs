using Finamoid.Import.Readers;
using Finamoid.Import.Writers;
using Microsoft.Extensions.DependencyInjection;

namespace Finamoid.Mutations
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMutations(this IServiceCollection services)
        {
            return services
                .AddScoped<IMutationReader, MutationReader>()
                .AddScoped<IMutationWriter, MutationWriter>();
        }
    }
}
