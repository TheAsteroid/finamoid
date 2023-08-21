namespace Finamoid.Import
{
    public interface IRawMutationReader
    {
        Task<IEnumerable<Mutation>> ReadAsync(string fullPath);

        Task<IEnumerable<Mutation>> ReadFromDirectoryAsync(string fullDirectory);
    }
}
