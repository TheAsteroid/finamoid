namespace Finamoid.Import
{
    public interface IMutationImportService
    {
        Task<int> ImportAndStoreAsync(string fullPath);
    }
}
