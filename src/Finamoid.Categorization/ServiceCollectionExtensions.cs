using Finamoid.Categorization.Categories;
using Finamoid.Categorization.CategorizedMutations;
using Microsoft.Extensions.DependencyInjection;

namespace Finamoid.Categorization
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCategorization(this IServiceCollection services)
        {
            return services
                .AddScoped<ICategoryReader, CategoryReader>()
                .AddScoped<ICategoryWriter, CategoryWriter>()
                .AddScoped<ICategorizedMutationReader, CategorizedMutationReader>()
                .AddScoped<ICategorizedMutationWriter, CategorizedMutationWriter>()
                .AddScoped<IMutationCategorizer, MutationCategorizer>()
                .AddScoped<IMutationCategorizerService, MutationCategorizerService>();
        }
    }
}
