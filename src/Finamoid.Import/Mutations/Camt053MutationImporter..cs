using Finamoid.Mutations;

namespace Finamoid.Import.Mutations
{
    internal class Camt053MutationImporter : MutationImporter
    {
        public override Task<IEnumerable<Mutation>> ReadAsync(string fullPath)
        {
            // TODO https://github.com/TheAsteroid/finamoid/issues/3: Implement CAMT.053
            throw new NotImplementedException();
        }
    }
}
