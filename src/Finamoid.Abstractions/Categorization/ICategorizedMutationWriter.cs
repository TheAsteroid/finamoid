namespace Finamoid.Categorization
{
    public interface ICategorizedMutationWriter
    {
        Task WriteAsync(IEnumerable<CategorizedMutation> categorizedMutations, PeriodType periodType);
    }
}
