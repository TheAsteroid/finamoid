using Finamoid.Categorization;

namespace Finamoid.Import.Categories
{
    internal class CategoryImportService : ICategoryImportService
    {
        private readonly ICategoryImporter _categoryImporter;
        private readonly ICategoryWriter _categoryWriter;

        public CategoryImportService(
            ICategoryImporter categoryImporter,
            ICategoryWriter categoryWriter)
        {
            _categoryImporter = categoryImporter;
            _categoryWriter = categoryWriter;
        }

        public async Task ImportAndStoreAsync(string fullPath)
        {
            var categories = await _categoryImporter.ReadAsync(fullPath);

            await _categoryWriter.WriteAsync(categories);
        }
    }
}
