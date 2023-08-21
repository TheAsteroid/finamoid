namespace Finamoid.Import
{
    public interface IBankStatementFormatDetector
    {
        Task<BankStatementType> DetectAsync(string path);
    }
}
