using Finamoid.Categorization;
using Finamoid.Utils;

namespace Finamoid.Aggregation
{
    public class MutationAggregator : IMutationAggregator
    {
        public IEnumerable<CategoryAggregation> Aggregate(
            IEnumerable<CategorizedMutation> categorizedMutations,
            PeriodType periodType)
        {
            if (!categorizedMutations.Any())
            {
                return Enumerable.Empty<CategoryAggregation>();
            }

            if (categorizedMutations.Any(c => !c.CategoryCodes.Any() || c.CategoryCodes.Count() > 1))
            {
                throw new InvalidDataException("Mutations must be assigned to exactly one category before aggregating.");
            }

            if (categorizedMutations.Select(c => c.Amount.Currency).Distinct().Count() > 1)
            {
                throw new InvalidDataException("Multiple currencies are currently not supported for aggregation.");
            }

            var categoryAggregations = categorizedMutations
                .GroupBy(c => new
                {
                    CategoryCode = c.CategoryCodes.First(),
                    StartDate = PeriodHelper.GetPeriodStartDate(c.DateTime, periodType)
                })
                .Select(g => new CategoryAggregation(
                    g.Key.CategoryCode,
                    g.Key.StartDate,
                    new CurrencyAmount(g.Sum(m => m.Amount.Amount), g.First().Amount.Currency))
                );

            return categoryAggregations;
        }
    }
}