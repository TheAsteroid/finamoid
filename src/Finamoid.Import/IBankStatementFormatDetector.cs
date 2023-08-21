namespace Finamoid.Import
{
    internal interface IBankStatementFormatDetector
    {
        Task<BankStatementType> DetectAsync(string path);
    }
}
