using Argo.CA.Application.Common.CQRS;
using Argo.CA.Application.Common.Models;
using Argo.CA.Application.Common.Persistence;

namespace Argo.CA.Application.Companies.Queries.GetCompanyList;

public class GetCompanyListQueryHandler(IAppDbContext dbContext)
    : IQueryHandler<GetCompanyListQuery, PaginatedList<CompanyListDto>>
{
    public async Task<PaginatedList<CompanyListDto>> Handle(GetCompanyListQuery query,
        CancellationToken cancellationToken)
    {
        return await dbContext.Companies
            .OrderBy(c => c.Name)
            .Select(c => new CompanyListDto(
                c.Id,
                c.Name,
                c.Description))
            .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);
    }
}