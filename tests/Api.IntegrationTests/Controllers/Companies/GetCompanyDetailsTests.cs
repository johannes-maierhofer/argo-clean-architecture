namespace Argo.CA.Api.IntegrationTests.Controllers.Companies;

using System.Net;
using ApiClients;
using FluentAssertions;
using Testing;
using Testing.Builders;
using TestUtils;
using Xunit.Abstractions;

public class GetCompanyDetailsTests(
    ITestOutputHelper output,
    CustomWebApplicationFactory factory,
    DatabaseFixture database)
    : IntegrationTestBase(output, factory, database)
{
    [Fact]
    public async Task GetCompanyDetails_ForExistingCompany_ShouldReturnExpectedResponse()
    {
        // Arrange
        var company = CompanyBuilder.Create().Build();
        await AddEntityToDb(company);

        var client = Factory.CreateApiClient();

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
        var client = Factory.CreateApiClient();

        // Act
        var action = () => client.GetCompanyDetailsAsync(nonExistingCompanyId);

        // Assert
        var assertions = await action.Should().ThrowAsync<ApiException<ProblemDetails>>();
        assertions.And.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
}
