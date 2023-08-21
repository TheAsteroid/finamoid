using Finamoid.Categorization;

namespace Finamoid.Aggregation
{
    public interface IMutationAggregator
    {
        IEnumerable<CategoryAggregation> Aggregate(
            IEnumerable<CategorizedMutation> categorizedMutations,
            PeriodType periodType);
    }
}
