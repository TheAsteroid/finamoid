namespace Finamoid.Import.Mutations
{
    internal interface IMutationImporterFactory
    {
        IMutationImporter Get(BankStatementType bankStatementType);
    }
}
