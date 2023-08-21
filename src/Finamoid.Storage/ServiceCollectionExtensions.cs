using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finamoid.Storage
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStorage(this IServiceCollection services, IConfigurationRoot configurationRoot)
        {
            return services
                .AddScoped<IStorageHandlerFactory, StorageHandlerFactory>()
                .Configure<StorageOptions>(configurationRoot.GetSection("Storage"));
        }
    }
}
