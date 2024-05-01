namespace Argo.CA.Application.Companies.Commands.UpdateCompany;

using Common.CQRS;

public record UpdateCompanyCommand(
    Guid CompanyId,
    string Name,
    string Description,
    string Street,
    string City,
    string PostCode,
    string CountryCode,
    string Email,
    string PhoneNumber
) : ICompanyCommand;
