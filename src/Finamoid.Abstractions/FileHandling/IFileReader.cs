namespace Finamoid.Abstractions.FileHandling
{
    public interface IFileReader
    {
        public virtual Task<string> ReadAsync(string path)
        {
            return File.ReadAllTextAsync(path);
        }
    }
}
