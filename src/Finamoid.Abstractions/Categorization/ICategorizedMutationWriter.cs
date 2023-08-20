namespace Finamoid.Abstractions.Categorization
{
    public interface ICategorizedMutationWriter
    {
        Task WriteAsync(string directory, IEnumerable<CategorizedMutation> categorizedMutations, PeriodType periodType);
    }
}
