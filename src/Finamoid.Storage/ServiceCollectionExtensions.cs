using Finamoid.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Finamoid.Encryption
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStorage(this IServiceCollection services)
        {
            return services
                .AddScoped<IStorageHandlerFactory, StorageHandlerFactory>();
        }
    }
}
 