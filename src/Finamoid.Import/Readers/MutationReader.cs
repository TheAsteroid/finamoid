using Finamoid.Abstractions;
using Finamoid.Abstractions.Import;

namespace Finamoid.Import.Readers
{
    public abstract class MutationReader : IMutationReader
    {
        public async Task<IEnumerable<Mutation>> ReadFromDirectoryAsync(string directory)
        {
            var result = new List<Mutation>();

            foreach (var path in Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories))
            {
                result.AddRange(await ReadFromFileAsync(path));
            }

            return result;
        }

        public abstract Task<IEnumerable<Mutation>> ReadFromFileAsync(string path);
    }
}
