namespace Argo.CA.Contracts.Companies;

public record GetCompanyDetailsResponse(
    Guid Id,
    string Name,
    string? Description,
    string Email,
    string PhoneNumber,
    CompanyDetailsLocation Location);

public record CompanyDetailsLocation(
    string CountryCode,
    string City,
    string Street,
    string PostCode);