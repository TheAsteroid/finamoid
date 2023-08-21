namespace Finamoid.Mutations
{
    public interface IMutationReader
    {
        Task<IEnumerable<Mutation>> ReadAsync(DateTime? startDate = null, DateTime? endDate = null);

        Task<IEnumerable<Mutation>> ReadAsync(string relativePath);
    }
}
