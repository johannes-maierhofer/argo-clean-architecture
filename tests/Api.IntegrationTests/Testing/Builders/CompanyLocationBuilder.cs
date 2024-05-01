namespace Argo.CA.Api.IntegrationTests.Testing.Builders;

using Constants;
using Domain.CompanyAggregate.ValueObjects;

public class CompanyLocationBuilder
{
    private string _countryCode = Constants.CompanyLocation.CountryCode;
    private string _city = Constants.CompanyLocation.City;
    private string _street = Constants.CompanyLocation.Street;
    private string _postCode = Constants.CompanyLocation.PostCode;

    public static CompanyLocationBuilder Create()
    {
        return new CompanyLocationBuilder();
    }

    private CompanyLocationBuilder()
    {
    }

    public CompanyLocationBuilder WithCountryCode(string countryCode)
    {
        _countryCode = countryCode;
        return this;
    }

    public CompanyLocationBuilder WithCity(string city)
    {
        _city = city;
        return this;
    }

    public CompanyLocationBuilder WithStreet(string street)
    {
        _street = street;
        return this;
    }

    public CompanyLocationBuilder WithPostCode(string postCode)
    {
        _postCode = postCode;
        return this;
    }

    public CompanyLocation Build()
    {
        return CompanyLocation.Create(
            _countryCode,
            _city,
            _street,
            _postCode);
    }
}