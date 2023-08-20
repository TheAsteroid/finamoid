namespace Finamoid.Abstractions.Import
{
    public interface IBankStatementFormatDetector
    {
        Task<BankStatementType> DetectAsync(string path);
    }
}
