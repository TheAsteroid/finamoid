namespace Finamoid.Abstractions.Aggregation
{
    public interface ICategoryAggregationWriter
    {
        Task WriteAsync(string path, IEnumerable<CategoryAggregation> categoryAggregations);
    }
}
