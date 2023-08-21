using Finamoid.Categorization.Categories;
using Finamoid.Categorization.CategorizedMutations;
using Finamoid.Mutations;

namespace Finamoid.Categorization
{
    internal class MutationCategorizerService : IMutationCategorizerService
    {
        private readonly ICategoryReader _categoryReader;
        private readonly IMutationReader _mutationReader;
        private readonly IMutationCategorizer _mutationCategorizer;
        private readonly ICategorizedMutationWriter _categorizedMutationWriter;

        public MutationCategorizerService(
            ICategoryReader categoryReader,
            IMutationReader mutationReader,
            IMutationCategorizer mutationCategorizer,
            ICategorizedMutationWriter categorizedMutationWriter)
        {
            _categoryReader = categoryReader;
            _mutationReader = mutationReader;
            _mutationCategorizer = mutationCategorizer;
            _categorizedMutationWriter = categorizedMutationWriter;
        }
        public async Task CategorizeAndStoreAsync(DateTime? startDate, DateTime? endDate)
        {
            var categories = await _categoryReader.ReadLatestAsync();

            var mutations = await _mutationReader.ReadAsync(startDate, endDate);

            var categorizedMutations = _mutationCategorizer.Categorize(categories, mutations);

            await _categorizedMutationWriter.WriteAsync(categorizedMutations);
        }
    }
}
