namespace Argo.CA.Api.IntegrationTests.Controllers.Companies;

using System.Net;
using ApiClients;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Testing;
using Testing.Builders;
using Testing.Constants;
using TestUtils;
using Xunit.Abstractions;

public class UpdateCompanyTests(
    ITestOutputHelper output,
    CustomWebApplicationFactory factory,
    DatabaseFixture database)
    : IntegrationTestBase(output, factory, database)
{
    [Fact]
    public async Task UpdateCompany_WhenRequestIsValid_ShouldSucceed()
    {
        // Arrange
        var company = CompanyBuilder.Create()
            .WithName("Company A")
            .Build();

        await AddEntityToDb(company);

        var client = Factory.CreateApiClient();

        // Act
        var request = CreateRequest(name: company.Name);
        await client.UpdateCompanyAsync(company.Id, request);

        // Assert
        await using var dbContext = Factory.CreateDbContext();
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

        await AddEntityRangeToDb(new[] { company, anotherCompany });

        var client = Factory.CreateApiClient();

        // Act
        var request = CreateRequest(name: anotherCompany.Name);
        var action = () => client.UpdateCompanyAsync(company.Id, request);

        // Assert
        var assertions = await action.Should().ThrowAsync<ApiException<ValidationProblemDetails>>();
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
