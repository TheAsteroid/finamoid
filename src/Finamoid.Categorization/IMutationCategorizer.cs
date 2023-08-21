using Finamoid.Mutations;

namespace Finamoid.Categorization
{
    internal interface IMutationCategorizer
    {
        CategorizedMutation AssignMutationToCategory(CategorizedMutation categorizedMutation, Category category);

        IEnumerable<CategorizedMutation> Categorize(IEnumerable<Category> categories, IEnumerable<Mutation> mutations);
    }
}
