namespace Finamoid.Encryption
{
    public interface IKeyProvider
    {
        Task<byte[]> GetSymmetricKeyAsync();
    }
}
