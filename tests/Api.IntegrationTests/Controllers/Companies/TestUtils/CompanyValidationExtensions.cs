namespace Argo.CA.Api.IntegrationTests.Controllers.Companies.TestUtils;

using ApiClients;
using Domain.CompanyAggregate;
using AwesomeAssertions;

public static class CompanyValidationExtensions
{
    public static void ValidateCreatedFrom(this Company company, CreateCompanyRequest request)
    {
        company.Name.Should().Be(request.Name);
        company.Description.Should().Be(request.Description);
        company.Email.Should().Be(request.Email);
        company.PhoneNumber.Should().Be(request.PhoneNumber);
        company.Location.City.Should().Be(request.City);
        company.Location.Street.Should().Be(request.Street);
        company.Location.PostCode.Should().Be(request.PostCode);
        company.Location.CountryCode.Should().Be(request.CountryCode);
    }

    public static void ValidateUpdatedFrom(this Company company, UpdateCompanyRequest request)
    {
        company.Name.Should().Be(request.Name);
        company.Description.Should().Be(request.Description);
        company.Email.Should().Be(request.Email);
        company.PhoneNumber.Should().Be(request.PhoneNumber);
        company.Location.City.Should().Be(request.City);
        company.Location.Street.Should().Be(request.Street);
        company.Location.PostCode.Should().Be(request.PostCode);
        company.Location.CountryCode.Should().Be(request.CountryCode);
    }

    public static void ValidateDetailsResponseFrom(this GetCompanyDetailsResponse response, Company company)
    {
        response.Name.Should().Be(company.Name);
        response.Description.Should().Be(company.Description);
        response.Email.Should().Be(company.Email);
        response.PhoneNumber.Should().Be(company.PhoneNumber);
        response.Location.City.Should().Be(company.Location.City);
        response.Location.Street.Should().Be(company.Location.Street);
        response.Location.PostCode.Should().Be(company.Location.PostCode);
        response.Location.CountryCode.Should().Be(company.Location.CountryCode);
    }
}