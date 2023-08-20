using Finamoid.Abstractions;

namespace Finamoid.Import.Readers
{
    public class Camt053MutationReader : MutationReader
    {
        public override Task<IEnumerable<Mutation>> ReadFromFileAsync(string path)
        {
            // TODO https://github.com/TheAsteroid/finamoid/issues/3: Implement CAMT.053
            throw new NotImplementedException();
        }
    }
}
