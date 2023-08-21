using Finamoid.Encryption;
using Finamoid.Storage;
using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;

namespace Finamoid.Storage
{
    internal class StorageHandlerFactory : IStorageHandlerFactory
    {
        private static readonly string _mutationsDirectory = "mutations";
        private static readonly string _categoriesDirectory = "categories";
        private static readonly string _categorizedMutationsDirectory = "categorized-mutations";
        private static readonly string _aggregationsDirectory = "aggregations";

        private readonly IReadOnlyDictionary<StorageType, Lazy<IStorageHandler>> _storageHandlers;

        public StorageHandlerFactory(IOptions<StorageOptions> storageOptions, IEncryptor encryptor, IKeyProvider keyProvider)
        {
            var options = storageOptions.Value;
            var rootDirectory = options.RootDirectory ?? throw new ArgumentException($"{nameof(options.RootDirectory)} cannot be null.");

            IStorageHandler getStorageHandler(bool encrypted, string relativeDirectory)
            {
                return encrypted ? 
                    new ProtectedStorageHandler(Path.Combine(rootDirectory, relativeDirectory), encryptor, keyProvider) : 
                    new StorageHandler(rootDirectory);
            };


            // TODO: These could be registered by the libraries that use them. It could be name-based rather than enum based, so we don't bleed unnecessary details.
            _storageHandlers = new ReadOnlyDictionary<StorageType, Lazy<IStorageHandler>>(
                new Dictionary<StorageType, Lazy<IStorageHandler>>
                {
                    { StorageType.Aggregations, new(() => getStorageHandler(options.EncryptAggregations, _aggregationsDirectory)) },
                    { StorageType.Categories, new(() => getStorageHandler(options.EncryptCategories, _categoriesDirectory)) },
                    { StorageType.CategorizedMutations, new(() => getStorageHandler(options.EncryptCategorizedMutations, _categorizedMutationsDirectory)) },
                    { StorageType.Mutations, new(() => getStorageHandler(options.EncryptMutations, _mutationsDirectory)) }
                });
        }

        public IStorageHandler Get(StorageType storageType)
        {
            if (!_storageHandlers.ContainsKey(storageType))
            {
                throw new NotSupportedException($"{nameof(StorageType)} {storageType} is not registered.");
            }

            return _storageHandlers[storageType].Value;
        }
    }
}
