namespace Finamoid.Import.Readers
{
    internal interface IJsonMutationReader
    {
        Task<IEnumerable<Mutation>> ReadAsync(string relativePath);
    }
}
