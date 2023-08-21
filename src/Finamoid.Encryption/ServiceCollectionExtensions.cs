using Microsoft.Extensions.DependencyInjection;

namespace Finamoid.Encryption
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEncryption(this IServiceCollection services)
        {
            return services
                .AddSingleton<IPasswordProvider, PasswordProvider>()
                .AddScoped<IKeyProvider, KeyProvider>()
                .AddScoped<IEncryptor, SymmetricEncryptor>();
        }
    }
}
