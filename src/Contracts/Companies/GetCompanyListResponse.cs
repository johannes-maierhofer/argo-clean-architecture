namespace Argo.CA.Contracts.Companies;

public record GetCompanyListResponse(
    List<CompanyListItem> Items,
    int PageNumber,
    int TotalPages,
    int TotalCount);

public record CompanyListItem(
    Guid Id,
    string Name,
    string? Description);