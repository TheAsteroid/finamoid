namespace Finamoid.Abstractions.Encryption
{
    public interface IEncryptor
    {
        byte[] Decrypt(byte[] key, byte[] encryptedData);

        byte[] Encrypt(byte[] key, byte[] dataToEncrypt);
    }
}
