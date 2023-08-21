namespace Finamoid.Import
{
    public interface IMutationImportService
    {
        Task<int> ImportAsync(string relativePath);
    }
}
