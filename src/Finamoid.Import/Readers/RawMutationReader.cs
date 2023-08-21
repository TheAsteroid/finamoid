namespace Finamoid.Import.Readers
{
    public abstract class RawMutationReader : IRawMutationReader
    {
        public async Task<IEnumerable<Mutation>> ReadFromDirectoryAsync(string directory)
        {
            var result = new List<Mutation>();

            foreach (var path in Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories))
            {
                result.AddRange(await ReadAsync(path));
            }

            return result;
        }

        public abstract Task<IEnumerable<Mutation>> ReadAsync(string path);
    }
}
