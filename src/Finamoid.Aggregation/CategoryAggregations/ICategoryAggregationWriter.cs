namespace Finamoid.Aggregation.CategoryAggregations
{
    internal interface ICategoryAggregationWriter
    {
        Task WriteToLatestAsync(IEnumerable<CategoryAggregation> categoryAggregations);

        Task WriteToNewAsync(IEnumerable<CategoryAggregation> categoryAggregations);
    }
}
