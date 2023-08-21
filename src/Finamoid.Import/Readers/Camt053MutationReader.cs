using Finamoid;

namespace Finamoid.Import.Readers
{
    internal class Camt053MutationReader : RawMutationReader
    {
        public override Task<IEnumerable<Mutation>> ReadAsync(string path)
        {
            // TODO https://github.com/TheAsteroid/finamoid/issues/3: Implement CAMT.053
            throw new NotImplementedException();
        }
    }
}
