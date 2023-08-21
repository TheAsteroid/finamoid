namespace Finamoid.Aggregation.CategoryAggregations
{
    internal interface ICategoryAggregationReader
    {
        Task<IEnumerable<CategoryAggregation>> ReadAsync(string relativePath);
    }
}
