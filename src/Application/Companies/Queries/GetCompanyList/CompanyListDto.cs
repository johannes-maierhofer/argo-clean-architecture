namespace Argo.CA.Application.Companies.Queries.GetCompanyList;

public record CompanyListDto(
    Guid Id,
    string Name,
    string? Description);
