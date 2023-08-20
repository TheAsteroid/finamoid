using Finamoid.Abstractions.Encryption;
using Finamoid.Utils;
using System.Security.Cryptography;
using System.Text;

namespace Finamoid.Encryption
{
    public class KeyProvider : IKeyProvider
    {
        private readonly IEncryptionConfiguration _encryptionConfiguration;
        private readonly IPasswordProvider _passwordProvider;
        private readonly IEncryptor _encryptor;

        private const string _recoveryKeyDirectory = "r";
        private const string _defaultKeyFileName = "d";

        public KeyProvider(
            IEncryptionConfiguration encryptionConfiguration,
            IPasswordProvider passwordProvider,
            IEncryptor encryptor)
        {
            _encryptionConfiguration = encryptionConfiguration;
            _passwordProvider = passwordProvider;
            _encryptor = encryptor;
        }

        public async Task<byte[]> GetSymmetricKeyAsync()
        {
            var passwordKeyBytes = Encoding.UTF8.GetBytes(_passwordProvider.GetPassword());

            var keyDirectory = _encryptionConfiguration.EncryptionKeyDirectory;

            var defaultKeyFile = Path.Combine(keyDirectory, $"{_defaultKeyFileName}{Constants.DataFileExtension}");

            var bytes = await File.ReadAllBytesAsync(defaultKeyFile);

            byte[]? key = null;
            try
            {
                key = _encryptor.Decrypt(passwordKeyBytes, bytes);
            }
            catch (CryptographicException)
            {
                // If decryption with password fails, perhaps a recovery key is provided.
                // In this case attempt to decrypt the keys in the recovery directory
                var alternateKeyDirectory = Path.Combine(keyDirectory, _recoveryKeyDirectory);
                foreach (var keyFile in Directory.GetFiles(alternateKeyDirectory, $"*{Constants.DataFileExtension}"))
                {
                    bytes = await File.ReadAllBytesAsync(defaultKeyFile);
                    try
                    {
                        key = _encryptor.Decrypt(passwordKeyBytes, bytes);
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
