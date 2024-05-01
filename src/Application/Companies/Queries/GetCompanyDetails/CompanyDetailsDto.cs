namespace Argo.CA.Application.Companies.Queries.GetCompanyDetails;
public record CompanyDetailsDto(
    Guid Id,
    string Name,
    string? Description,
    string Email,
    string PhoneNumber,
    CompanyDetailsLocationDto Location);

public record CompanyDetailsLocationDto(
    string CountryCode,
    string City,
    string Street,
    string PostCode);