namespace Finamoid.Aggregation
{
    public interface IMutationAggregatorService
    {
        Task AggregateAndStoreAsync(DateTime? startDate, DateTime? endDate, PeriodType periodType, bool writeToNew);
    }
}
