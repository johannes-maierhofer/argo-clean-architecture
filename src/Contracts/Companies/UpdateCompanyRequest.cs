namespace Argo.CA.Contracts.Companies;

public record UpdateCompanyRequest(
    string Name,
    string Description,
    string Street,
    string City,
    string PostCode,
    string CountryCode,
    string Email,
    string PhoneNumber);