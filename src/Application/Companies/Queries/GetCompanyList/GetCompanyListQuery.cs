namespace Argo.CA.Application.Companies.Queries.GetCompanyList;

using Common.CQRS;
using Common.Models;

public record GetCompanyListQuery(
    int PageNumber = 1,
    int PageSize = 10
) : IQuery<PaginatedList<CompanyListDto>>;
