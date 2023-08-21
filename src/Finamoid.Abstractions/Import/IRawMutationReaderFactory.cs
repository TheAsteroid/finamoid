namespace Finamoid.Import
{
    public interface IRawMutationReaderFactory
    {
        IRawMutationReader Get(BankStatementType bankStatementType);
    }
}
