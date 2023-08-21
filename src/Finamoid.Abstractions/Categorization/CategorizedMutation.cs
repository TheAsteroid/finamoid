using Finamoid.Mutations;

namespace Finamoid.Categorization
{
    /// <summary>
    /// A Mutation with one or more categories assigned to it.
    /// A Mutation can match with multiple categories, however before aggregating this should be reduced to a single category.
    /// </summary>
    /// <param name="Categories"></param>
    /// <param name="Mutation"></param>
    public record CategorizedMutation(IEnumerable<string> CategoryCodes, Mutation Mutation)
        : Mutation(
            Mutation.Id,
            Mutation.DateTime,
            Mutation.Amount,
            Mutation.Name,
            Mutation.Description,
            Mutation.Reference,
            Mutation.AccountNumber,
            Mutation.MutationType);

}
