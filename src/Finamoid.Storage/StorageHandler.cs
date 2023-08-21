namespace Finamoid.Storage
{
    internal class StorageHandler : IStorageHandler
    {
        public StorageHandler(string rootDirectory)
        {
            // This makes it easier to work with stripping paths to their relative part.
            // Might want to consider supporting relative rootDirectory though.
            if (!Path.IsPathFullyQualified(rootDirectory))
            {
                throw new ArgumentException($"{nameof(rootDirectory)} must be fully qualified.");
            }

            RootDirectory = rootDirectory;
        }

        protected string RootDirectory { get; }

        public IEnumerable<string> EnumerateFiles(string relativeDirectory, string searchPattern, SearchOption searchOption)
        {
            var directory =
                !string.IsNullOrEmpty(relativeDirectory) ?
                GetFullPath(relativeDirectory) :
                RootDirectory;

            // Return the relative path (strip the root directory)
            return Directory.EnumerateFiles(directory, searchPattern, searchOption).Select(f => f.Substring(RootDirectory.Length));
        }

        public bool FileExists(string relativePath)
        {
            return File.Exists(GetFullPath(relativePath));
        }

        public virtual Task<string> ReadAllTextAsync(string relativePath)
        {
            return File.ReadAllTextAsync(GetFullPath(relativePath));
        }

        public virtual Task<byte[]> ReadAllBytesAsync(string relativePath)
        {
            return File.ReadAllBytesAsync(GetFullPath(relativePath));
        }

        public virtual Task WriteAsync(string relativePath, string data)
        {
            var path = GetFullPath(relativePath);

            CreateDirectoryIfNotExists(path);

            return File.WriteAllTextAsync(path, data);
        }

        public virtual Task WriteAsync(string relativePath, byte[] data)
        {
            var path = GetFullPath(relativePath);

            CreateDirectoryIfNotExists(path);

            return File.WriteAllBytesAsync(path, data);
        }

        private string GetFullPath(string relativePath)
        {
            return Path.Combine(RootDirectory, relativePath);
        }

        private static void CreateDirectoryIfNotExists(string path)
        {
            var directory = Path.GetDirectoryName(path);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}
