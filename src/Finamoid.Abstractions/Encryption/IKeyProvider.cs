namespace Finamoid.Abstractions.Encryption
{
    public interface IKeyProvider
    {
        Task<byte[]> GetSymmetricKeyAsync();
    }
}
