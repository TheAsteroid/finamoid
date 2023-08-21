using Finamoid.Mutations;

namespace Finamoid.Import.Mutations
{
    internal interface IMutationImporter
    {
        Task<IEnumerable<Mutation>> ReadAsync(string fullPath);

        Task<IEnumerable<Mutation>> ReadFromDirectoryAsync(string fullDirectory);
    }
}
