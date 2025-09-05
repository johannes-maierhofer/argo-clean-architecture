using Argo.CA.Api.IntegrationTests.Controllers.Companies.TestUtils;
using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;

namespace Argo.CA.Api.IntegrationTests.Controllers.Companies;

using System.Net;
using ApiClients;
using Testing;
using Testing.Builders;
using Testing.Constants;
using Xunit.Abstractions;

public class UpdateCompanyTests(
    ITestOutputHelper output,
    DatabaseFixture database)
    : IntegrationTestBase(output, database)
{
    [Fact]
    public async Task UpdateCompany_WhenRequestIsValid_ShouldSucceed()
    {
        // Arrange
        var company = CompanyBuilder.Create()
            .WithName("Company A")
            .Build();

        await AddEntityToDb(company);

        await using var factory = CreateWebAppFactory();
        var client = factory.CreateApiClient();

        // Act
        var request = CreateRequest(name: company.Name);
        await client.UpdateCompanyAsync(company.Id, request);

        // Assert
        await using var dbContext = CreateDbContext();
        var companyFromDb = await dbContext.Companies.SingleAsync(c => c.Id == company.Id);
        companyFromDb.ValidateUpdatedFrom(request);
    }

    [Fact]
    public async Task UpdateCompany_WithNameOfAnotherCompany_ShouldReturnBadRequest()
    {
        // Arrange
        var company = CompanyBuilder.Create()
            .WithName("Company A")
            .Build();

        var anotherCompany = CompanyBuilder.Create()
            .WithName("Company B")
            .Build();

        await AddEntityRangeToDb([company, anotherCompany]);

        await using var factory = CreateWebAppFactory();
        var client = factory.CreateApiClient();

        // Act
        var request = CreateRequest(name: anotherCompany.Name);
        var action = () => client.UpdateCompanyAsync(company.Id, request);

        // Assert
        var assertions = await action.Should().ThrowAsync<ApiException<ValidationProblemDetails>>("Company name must be unique.");
        assertions.And.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        assertions.And.Result.Title.Should().Be("Invalid");
    }

    private static UpdateCompanyRequest CreateRequest(
        string name = Constants.Company.Name + " (upd)",
        string email = "upd-" + Constants.Company.Email,
        string city = Constants.CompanyLocation.City + " (upd)",
        string countryCode = "SE",
        string? description = "updated",
        string phoneNumber = Constants.Company.PhoneNumber + " (upd)",
        string postCode = "1010",
        string street = Constants.CompanyLocation.Street + " (upd)")
    {
        return new UpdateCompanyRequest
        {
            Name = name,
            Email = email,
            City = city,
            CountryCode = countryCode,
            Description = description,
            PhoneNumber = phoneNumber,
            PostCode = postCode,
            Street = street
        };
    }
}
