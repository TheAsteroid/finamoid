namespace Finamoid.Storage
{
    public interface IStorageHandler
    {
        IEnumerable<string> EnumerateFiles(string relativeDirectory, string searchPattern, SearchOption searchOption);

        bool FileExists(string relativePath);

        Task<string> ReadAllTextAsync(string relativePath);

        Task<byte[]> ReadAllBytesAsync(string relativePath);

        Task WriteAsync(string relativePath, string data);

        Task WriteAsync(string relativePath, byte[] data);
    }
}
