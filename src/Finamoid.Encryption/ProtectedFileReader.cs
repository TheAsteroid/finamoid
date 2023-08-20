using Finamoid.Abstractions.Encryption;
using Finamoid.Abstractions.FileHandling;
using System.Text;

namespace Finamoid.Encryption
{
    public class ProtectedFileReader : IFileReader
    {
        private readonly IEncryptor _encryptor;
        private readonly IKeyProvider _keyProvider;

        public ProtectedFileReader(IEncryptor encryptor, IKeyProvider keyProvider)
        {
            _encryptor = encryptor;
            _keyProvider = keyProvider;
        }

        public async Task<string> ReadAsync(string path)
        {
            var encryptedData = await File.ReadAllBytesAsync(path);

            return Encoding.UTF8.GetString(
                _encryptor.Decrypt(await _keyProvider.GetSymmetricKeyAsync(), encryptedData));
        }
    }
}
