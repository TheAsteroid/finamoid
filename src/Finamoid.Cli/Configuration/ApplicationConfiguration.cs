using Finamoid.Abstractions.Encryption;
using Microsoft.Extensions.Configuration;

namespace Finamoid.Cli.Configuration
{
    internal class ApplicationConfiguration : IEncryptionConfiguration
    {
        private readonly IConfiguration _configuration;

        public ApplicationConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IConfigurationSection Encryption => _configuration.GetSection("Encryption");

        public bool EncryptCategories => Encryption.GetValue(nameof(EncryptCategories), true);

        public bool EncryptImportedMutations => Encryption.GetValue(nameof(EncryptImportedMutations), true);

        public bool EncryptAggregations => Encryption.GetValue(nameof(EncryptAggregations), true);

        public string EncryptionKeyDirectory => Encryption.GetValue<string>(nameof(EncryptionKeyDirectory)) ?? "e";
    }
}
