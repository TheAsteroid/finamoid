using Finamoid.Categorization;

namespace Finamoid.Aggregation
{
    internal interface IMutationAggregator
    {
        IEnumerable<CategoryAggregation> Aggregate(
            IEnumerable<CategorizedMutation> categorizedMutations,
            PeriodType periodType);
    }
}
