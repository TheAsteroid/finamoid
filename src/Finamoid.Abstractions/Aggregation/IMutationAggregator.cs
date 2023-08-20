using Finamoid.Abstractions.Categorization;

namespace Finamoid.Abstractions.Aggregation
{
    public interface IMutationAggregator
    {
        IEnumerable<CategoryAggregation> Aggregate(
            IEnumerable<CategorizedMutation> categorizedMutations,
            PeriodType periodType);
    }
}
