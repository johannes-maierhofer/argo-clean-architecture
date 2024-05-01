namespace Argo.CA.Contracts.Companies;

public record GetCompanyListRequest(
    int PageNumber = 1,
    int PageSize = 10);
