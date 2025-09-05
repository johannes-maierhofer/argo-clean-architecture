namespace Argo.CA.Api.IntegrationTests.Controllers.Companies;

using Domain.CompanyAggregate;
using FluentAssertions;
using Testing;
using Testing.Builders;
using Xunit.Abstractions;

public class GetCompanyListTests(
    ITestOutputHelper output,
    DatabaseFixture database)
    : IntegrationTestBase(output, database)
{
    [Fact]
    public async Task GetCompanyList_FirstPage_ShouldReturnExpectedResult()
    {
        // Arrange
        const int totalCount = 3;
        const int pageSize = 2;

        var companies = CreateRangeOfCompanies(totalCount);
        await AddEntityRangeToDb(companies);

        await using var factory = CreateWebAppFactory();
        var client = factory.CreateApiClient();

        // Act
        var response = await client.GetCompanyListAsync(1, pageSize);

        // Assert
        response.Items.Count.Should().Be(pageSize);
        response.TotalCount.Should().Be(totalCount);
    }

    private static IEnumerable<Company> CreateRangeOfCompanies(int number)
    {
        for (var i = 1; i <= number; i++)
        {
            yield return CompanyBuilder.Create()
                .WithName("Company " + i)
                .Build();
        }
    }
}
