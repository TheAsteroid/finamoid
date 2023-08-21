using Finamoid.Mutations;

namespace Finamoid.Import.Mutations
{
    public abstract class MutationImporter : IMutationImporter
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

        public abstract Task<IEnumerable<Mutation>> ReadAsync(string fullPath);
    }
}
