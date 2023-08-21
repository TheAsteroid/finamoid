namespace Finamoid.Import
{
    public interface IMutationWriter
    {
        Task WriteAsync(IEnumerable<Mutation> mutations, PeriodType periodType);
    }
}
