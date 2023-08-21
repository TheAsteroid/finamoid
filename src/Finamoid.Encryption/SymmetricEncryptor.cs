using System.Security.Cryptography;

namespace Finamoid.Encryption
{
    internal class SymmetricEncryptor : IEncryptor
    {
        private const int _ivLength = 16;

        public byte[] Decrypt(byte[] key, byte[] encryptedData)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = encryptedData[.._ivLength];

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream(encryptedData[_ivLength..]);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

            return memoryStream.ToArray();
        }

        public byte[] Encrypt(byte[] key, byte[] dataToEncrypt)
        {
            var iv = new byte[_ivLength];
            RandomNumberGenerator.Fill(iv);

            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream();
            memoryStream.Write(iv, 0, iv.Length);

            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

            memoryStream.Write(dataToEncrypt);

            return memoryStream.ToArray();
        }
    }
}