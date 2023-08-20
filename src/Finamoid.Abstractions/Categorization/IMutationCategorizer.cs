namespace Finamoid.Abstractions.Categorization
{
    public interface IMutationCategorizer
    {
        CategorizedMutation AssignMutationToCategory(CategorizedMutation categorizedMutation, Category category);

        IEnumerable<CategorizedMutation> Categorize(IEnumerable<Category> categories, IEnumerable<Mutation> mutations);
    }
}
