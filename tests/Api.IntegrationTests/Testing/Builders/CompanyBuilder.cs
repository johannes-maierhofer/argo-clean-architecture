namespace Argo.CA.Api.IntegrationTests.Testing.Builders;

using Constants;
using Domain.CompanyAggregate;
using Domain.CompanyAggregate.ValueObjects;

public class CompanyBuilder
{
    private string _name = Constants.Company.Name;
    private string? _description;
    private string _phoneNumber = Constants.Company.PhoneNumber;
    private string _email = Constants.Company.Email;
    private CompanyLocation _location = CompanyLocationBuilder.Create().Build();

    private CompanyBuilder()
    {
    }

    public static CompanyBuilder Create()
    {
        return new CompanyBuilder();
    }

    public CompanyBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public CompanyBuilder WithDescription(string? description)
    {
        _description = description;
        return this;
    }

    public CompanyBuilder WithPhoneNumber(string phoneNumber)
    {
        _phoneNumber = phoneNumber;
        return this;
    }

    public CompanyBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public CompanyBuilder WithLocation(CompanyLocation location)
    {
        _location = location;
        return this;
    }

    public Company Build()
    {
        var company = Company.Create(
            _name,
            _description,
            _location,
            _phoneNumber,
            _email);

        company.SetCreated(DateTimeOffset.UtcNow, "integration_tests");
        company.SetModified(DateTimeOffset.UtcNow, "integration_tests");

        return company;
    }
}