using Finamoid.Mutations;

namespace Finamoid.Categorization
{
    public class MutationCategorizer : IMutationCategorizer
    {
        public IEnumerable<CategorizedMutation> Categorize(IEnumerable<Category> categories, IEnumerable<Mutation> mutations)
        {
            var result = new List<CategorizedMutation>();

            var inCategories = categories.Where(c => c.BalanceType == BalanceType.In);
            var outCategories = categories.Where(c => c.BalanceType == BalanceType.Out);

            var inWildcardCategory = inCategories.Where(i => i.IsWildcard).FirstOrDefault();
            var outWildcardCategory = outCategories.Where(i => i.IsWildcard).FirstOrDefault();

            if (inWildcardCategory == null)
            {
                throw new InvalidDataException($"No wildcard category found in provided categories for {nameof(BalanceType)} {BalanceType.In}.");
            }

            if (outWildcardCategory == null)
            {
                throw new InvalidDataException($"No wildcard category found in provided categories for {nameof(BalanceType)} {BalanceType.Out}.");
            }

            foreach (var mutation in mutations)
            {
                var balanceType = mutation.Amount.Amount > 0 ? BalanceType.In : BalanceType.Out;
                var mutationCategories =
                    (balanceType == BalanceType.In ? inCategories : outCategories)
                    .Where(c => c.Filters.Any(c => IsMatch(c, mutation)));

                if (!mutationCategories.Any())
                {
                    mutationCategories = new[] 
                    {
                        balanceType == BalanceType.In ? inWildcardCategory : outWildcardCategory
                    };
                }

                result.Add(new CategorizedMutation(mutationCategories.Select(c => c.Code), mutation));
            }

            return result;
        }

        public CategorizedMutation AssignMutationToCategory(CategorizedMutation categorizedMutation, Category category)
        {
            var balanceType = category.BalanceType;
            var expectedBalanceType = categorizedMutation.Amount.Amount <= 0 ? BalanceType.Out : BalanceType.In;

            if (balanceType != expectedBalanceType)
            {
                throw new InvalidOperationException($"Mutation amount does not match the balance type of category {category.Code}." +
                    $"Assign the mutation to a category with balance type {expectedBalanceType}.");
            }

            if (!category.IsWildcard && !category.Filters.Any(c => IsMatch(c, categorizedMutation)))
            {
                throw new InvalidOperationException(
                    $"Mutation does not match category {category.Code}." +
                    "Mutations can only be assigned to a category if there is a matching filter.");
            }

            return new(new[] { category.Code }, categorizedMutation);
        }

        private static bool IsMatch(CategoryFilter categoryFilter, Mutation mutation)
        {
            var accountNumber = mutation.AccountNumber;
            var name = mutation.Name;
            var description = mutation.Description;
            var reference = mutation.Reference;
            var categoryAccountNumber = categoryFilter.AccountNumber;
            var exactMatch = categoryFilter.ExactMatch;
            var partialMatch = categoryFilter.PartialMatch;

            return
                (
                    !string.IsNullOrEmpty(categoryAccountNumber) &&
                    categoryAccountNumber.Equals(accountNumber, StringComparison.InvariantCultureIgnoreCase)
                ) ||
                (
                    !string.IsNullOrEmpty(exactMatch) &&
                    (
                        exactMatch.Equals(name, StringComparison.InvariantCultureIgnoreCase) ||
                        exactMatch.Equals(description, StringComparison.InvariantCultureIgnoreCase) ||
                        exactMatch.Equals(reference, StringComparison.InvariantCultureIgnoreCase)
                    )
                ) ||
                (
                    !string.IsNullOrEmpty(partialMatch) &&
                    (
                        !string.IsNullOrEmpty(name) && name.Contains(partialMatch, StringComparison.InvariantCultureIgnoreCase) ||
                        !string.IsNullOrEmpty(description) && description.Contains(partialMatch, StringComparison.InvariantCultureIgnoreCase) ||
                        !string.IsNullOrEmpty(reference) && reference.Contains(partialMatch, StringComparison.InvariantCultureIgnoreCase)
                    )
                );
        }
    }
}
