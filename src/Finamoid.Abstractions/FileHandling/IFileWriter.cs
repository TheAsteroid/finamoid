namespace Finamoid.Abstractions.FileHandling
{
    public interface IFileWriter
    {
        public virtual Task WriteAsync(string path, string data)
        {
            return File.WriteAllTextAsync(path, data);
        }
    }
}
