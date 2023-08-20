using Finamoid.Abstractions.Encryption;
using Finamoid.Abstractions.FileHandling;
using System.Text;

namespace Finamoid.Encryption
{
    public class ProtectedFileWriter : IFileWriter
    {
        private readonly IEncryptor _encryptor;
        private readonly IKeyProvider _keyProvider;

        public ProtectedFileWriter(IEncryptor encryptor, IKeyProvider keyProvider)
        {
            _encryptor = encryptor;
            _keyProvider = keyProvider;
        }

        public async Task WriteAsync(string path, string data)
        {
            var encryptedData = _encryptor.Encrypt(
                await _keyProvider.GetSymmetricKeyAsync(), 
                Encoding.UTF8.GetBytes(data));

            await File.WriteAllBytesAsync(path, encryptedData);
        }
    }
}
