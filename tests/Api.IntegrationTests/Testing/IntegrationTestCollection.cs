namespace Argo.CA.Api.IntegrationTests.Testing;

[CollectionDefinition("IntegrationTests")]
public class IntegrationTestCollection : ICollectionFixture<DatabaseFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}