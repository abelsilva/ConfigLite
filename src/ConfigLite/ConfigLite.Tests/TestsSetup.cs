using System;
using Xunit;

[assembly: CollectionBehavior(MaxParallelThreads = 1, DisableTestParallelization = true)]

namespace ConfigLite.Tests
{
    [CollectionDefinition("ConfigLite")]
    public class SetUpCollection : ICollectionFixture<SetUpFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

    public class SetUpFixture : IDisposable
    {
        public void Dispose()
        {
        }
    }
}