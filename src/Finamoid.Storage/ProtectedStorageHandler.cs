using Finamoid.Encryption;
using System.Text;

namespace Finamoid.Storage
{
    internal class ProtectedStorageHandler : StorageHandler, IStorageHandler
    {
        private readonly IEncryptor _encryptor;
        private readonly IKeyProvider _keyProvider;

        public ProtectedStorageHandler(string rootDirectory, IEncryptor encryptor, IKeyProvider keyProvider)
            : base(rootDirectory)
        {
            _encryptor = encryptor;
            _keyProvider = keyProvider;
        }

        public override async Task<string> ReadAllTextAsync(string relativePath)
        {
            return Encoding.UTF8.GetString(await ReadAllBytesAsync(relativePath));
        }

        public override async Task<byte[]> ReadAllBytesAsync(string relativePath)
        {
            var encryptedData = await base.ReadAllBytesAsync(relativePath);

            return _encryptor.Decrypt(await _keyProvider.GetSymmetricKeyAsync(), encryptedData);

        }

        public override async Task WriteAsync(string relativePath, string data)
        {
            var encryptedData = _encryptor.Encrypt(
                await _keyProvider.GetSymmetricKeyAsync(),
                Encoding.UTF8.GetBytes(data));

            await base.WriteAsync(relativePath, encryptedData);
        }

        public override async Task WriteAsync(string relativePath, byte[] data)
        {
            var encryptedData = _encryptor.Encrypt(
                await _keyProvider.GetSymmetricKeyAsync(),
                data);

            await base.WriteAsync(relativePath, encryptedData);
        }
    }
}
