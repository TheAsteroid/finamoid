using Finamoid.Categorization;

namespace Finamoid.Import.Categories
{
    internal interface ICategoryImporter
    {
        Task<IEnumerable<Category>> ReadAsync(string fullPath);
    }
}
