using Finamoid.Aggregation.CategoryAggregations;
using Finamoid.Categorization.CategorizedMutations;
using Finamoid.Utils;

namespace Finamoid.Aggregation
{
    internal class MutationAggregatorService : IMutationAggregatorService
    {
        private readonly ICategorizedMutationReader _categorizedMutationReader;
        private readonly IMutationAggregator _mutationAggregator;
        private readonly ICategoryAggregationWriter _categoryAggregationWriter;

        public MutationAggregatorService(
            ICategorizedMutationReader categorizedMutationReader,
            IMutationAggregator mutationAggregator,
            ICategoryAggregationWriter categoryAggregationWriter)
        {
            _categorizedMutationReader = categorizedMutationReader;
            _mutationAggregator = mutationAggregator;
            _categoryAggregationWriter = categoryAggregationWriter;
        }

        public async Task AggregateAndStoreAsync(DateTime? startDate, DateTime? endDate, PeriodType periodType, bool writeToNew)
        {
            // Extend the period to make sure it's full.
            if (startDate != null)
            {
                startDate = PeriodHelper.GetPeriodStartDate(startDate.Value, periodType);
            }

            if (endDate != null)
            {
                endDate = PeriodHelper.GetPeriodEndDate(endDate.Value, periodType);
            }

            var categorizedMutations = await _categorizedMutationReader.ReadAsync(startDate, endDate);

            var categoryAggregations = _mutationAggregator.Aggregate(categorizedMutations, periodType);

            if (writeToNew)
            {
                await _categoryAggregationWriter.WriteToNewAsync(categoryAggregations);
                return;
            }

            await _categoryAggregationWriter.WriteToLatestAsync(categoryAggregations);
        }
    }
}
