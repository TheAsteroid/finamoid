using Finamoid.Aggregation.CategoryAggregations;
using Microsoft.Extensions.DependencyInjection;

namespace Finamoid.Aggregation
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAggregation(this IServiceCollection services)
        {
            return services
                .AddScoped<ICategoryAggregationReader, CategoryAggregationReader>()
                .AddScoped<ICategoryAggregationWriter, CategoryAggregationWriter>()
                .AddScoped<IMutationAggregator, MutationAggregator>()
                .AddScoped<IMutationAggregatorService, MutationAggregatorService>();
        }
    }
}
