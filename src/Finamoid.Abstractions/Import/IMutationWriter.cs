namespace Finamoid.Abstractions.Import
{
    public interface IMutationWriter
    {
        Task WriteAsync(string directory, IEnumerable<Mutation> mutations, PeriodType periodType);
    }
}
