namespace Finamoid.Mutations
{
    public interface IMutationWriter
    {
        Task WriteAsync(IEnumerable<Mutation> mutations, PeriodType periodType);
    }
}
