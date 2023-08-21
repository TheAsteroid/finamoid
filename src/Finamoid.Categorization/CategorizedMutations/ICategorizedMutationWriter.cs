namespace Finamoid.Categorization.CategorizedMutations
{
    internal interface ICategorizedMutationWriter
    {
        Task WriteAsync(IEnumerable<CategorizedMutation> categorizedMutations);
    }
}
