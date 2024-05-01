namespace Argo.CA.Application.Companies.Queries.GetCompanyList;

using System.Threading.Tasks;
using Common.CQRS;
using Common.Mappings;
using Common.Models;
using Common.Persistence;

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
