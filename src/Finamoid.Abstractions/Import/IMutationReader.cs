namespace Finamoid.Abstractions.Import
{
    public interface IMutationReader
    {
        Task<IEnumerable<Mutation>> ReadFromFileAsync(string path);

        Task<IEnumerable<Mutation>> ReadFromDirectoryAsync(string directory);
    }
}
