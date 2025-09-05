namespace Argo.CA.Api.IntegrationTests.Controllers.Companies;

using System.Net;
using ApiClients;
using AwesomeAssertions;
using Testing;
using Testing.Builders;
using TestUtils;
using Xunit.Abstractions;

public class GetCompanyDetailsTests(
    ITestOutputHelper output,
    DatabaseFixture database)
    : IntegrationTestBase(output, database)
{
    [Fact]
    public async Task GetCompanyDetails_ForExistingCompany_ShouldReturnExpectedResponse()
    {
        // Arrange
        var company = CompanyBuilder.Create().Build();
        await AddEntityToDb(company);

        await using var factory = CreateWebAppFactory();
        var client = factory.CreateApiClient();

        // Act
        var response = await client.GetCompanyDetailsAsync(company.Id);

        // Assert
        response.ValidateDetailsResponseFrom(company);
    }

    [Fact]
    public async Task GetCompanyDetails_ForNonExistingCompany_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistingCompanyId = Guid.NewGuid();

        await using var factory = CreateWebAppFactory();
        var client = factory.CreateApiClient();

        // Act
        var action = () => client.GetCompanyDetailsAsync(nonExistingCompanyId);

        // Assert
        var assertions = await action.Should().ThrowAsync<ApiException<ProblemDetails>>();
        assertions.And.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
}
