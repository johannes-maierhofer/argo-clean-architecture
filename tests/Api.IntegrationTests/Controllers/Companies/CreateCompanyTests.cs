using Argo.CA.Api.IntegrationTests.Testing.Authentication;
using Argo.CA.Api.IntegrationTests.Testing.Extensions;

namespace Argo.CA.Api.IntegrationTests.Controllers.Companies;

using System.Net;
using ApiClients;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Testing;
using Testing.Constants;
using TestUtils;
using Xunit.Abstractions;

public class CreateCompanyTests(
    ITestOutputHelper output,
    CustomWebApplicationFactory factory,
    DatabaseFixture database)
    : IntegrationTestBase(output, factory, database)
{
    [Fact]
    public async Task CreateCompany_WhenRequestIsValid_ShouldSucceed()
    {
        // Arrange
        var client = Factory.CreateApiClient(b => b.AsAdmin());
        var request = CreateRequest();

        // Act
        var response = await client.CreateCompanyAsync(request);

        // Assert
        response.Id.Should().NotBeEmpty();

        await using var dbContext = Factory.CreateDbContext();

        var company = await dbContext.Companies.SingleAsync(c => c.Id == response.Id);
        company.ValidateCreatedFrom(request);
        company.ValidateCreatedBy(Constants.User.Admin.Name);
        company.ValidateModifiedBy(Constants.User.Admin.Name);
    }

    [Fact]
    public async Task CreateCompany_WithInvalidEmail_ShouldReturnBadRequest()
    {
        // Arrange
        var client = Factory.CreateApiClient();
        var request = CreateRequest(email: "not-a-valid-email");

        // Act
        var action = () => client.CreateCompanyAsync(request);

        // Assert
        var assertions = await action.Should().ThrowAsync<ApiException<ValidationProblemDetails>>();
        assertions.And.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        assertions.And.Result.Title.Should().Be("Invalid");
        assertions.And.Result.Errors.First().Key.Should().Be("Email");
    }

    [Fact]
    public async Task CreateCompany_WhenUserIsOnlyReader_ShouldReturnForbidden()
    {
        // Arrange
        var client = Factory.CreateApiClient(b => b.AsReader());
        var request = CreateRequest();

        // Act
        var action = () => client.CreateCompanyAsync(request);

        // Assert
        var assertions = await action.Should().ThrowAsync<ApiException>();
        assertions.And.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
    }

    private static CreateCompanyRequest CreateRequest(
        string name = Constants.Company.Name,
        string email = Constants.Company.Email,
        string city = Constants.CompanyLocation.City,
        string countryCode = Constants.CompanyLocation.CountryCode,
        string? description = null,
        string phoneNumber = Constants.Company.PhoneNumber,
        string postCode = Constants.CompanyLocation.PostCode,
        string street = Constants.CompanyLocation.Street)
    {
        return new CreateCompanyRequest
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
