using Finamoid.Storage;
using Finamoid.Utils;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace Finamoid.Encryption
{
    internal class KeyProvider : IKeyProvider
    {
        private readonly IPasswordProvider _passwordProvider;
        private readonly IEncryptor _encryptor;
        private readonly string _recoveryKeyDirectory;
        private readonly string _defaultKeyPath;

        public KeyProvider(
            IOptions<StorageOptions> storageOptions,
            IPasswordProvider passwordProvider,
            IEncryptor encryptor)
        {
            var keyDirectory = Path.Combine(
                storageOptions.Value.RootDirectory ?? throw new ArgumentException($"{nameof(storageOptions.Value.RootDirectory)} cannot be null."),
                "k");

            _recoveryKeyDirectory = Path.Combine(keyDirectory, "r");
            _defaultKeyPath = Path.Combine(keyDirectory, $"d{Constants.DataFileExtension}");

            _passwordProvider = passwordProvider;
            _encryptor = encryptor;
        }

        public async Task<byte[]> GetSymmetricKeyAsync()
        {
            var passwordKey = _passwordProvider.GetPasswordKey();

            var bytes = await File.ReadAllBytesAsync(_defaultKeyPath);

            byte[]? key = null;
            try
            {
                key = _encryptor.Decrypt(passwordKey, bytes);
            }
            catch (CryptographicException)
            {
                // If decryption with password fails, perhaps a recovery key is provided.
                // In this case attempt to decrypt the keys in the recovery directory
                foreach (var keyFile in Directory.EnumerateFiles(
                    _recoveryKeyDirectory,
                    $"*{Constants.DataFileExtension}",
                    SearchOption.AllDirectories))
                {
                    bytes = await File.ReadAllBytesAsync(keyFile);
                    try
                    {
                        key = _encryptor.Decrypt(passwordKey, bytes);
                    }
                    catch (CryptographicException) { }
                }
            }

            if (key == null)
            {
                throw new CryptographicException("Could not decrypt key using provided password.");
            }

            return key;
        }
    }
}
