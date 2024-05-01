namespace Argo.CA.Application.Companies.Queries.GetCompanyDetails;

using System.Threading.Tasks;
using Domain.CompanyAggregate;
using Common.CQRS;
using Common.Exceptions;
using Common.Persistence;
using Microsoft.EntityFrameworkCore;

public class GetCompanyDetailsQueryHandler(IAppDbContext dbContext)
    : IQueryHandler<GetCompanyDetailsQuery, CompanyDetailsDto>
{
    public async Task<CompanyDetailsDto> Handle(GetCompanyDetailsQuery query,
        CancellationToken cancellationToken)
    {
        var result = await dbContext.Companies
            .Where(c => c.Id == query.CompanyId)
            .Select(c => new CompanyDetailsDto(
                c.Id,
                c.Name,
                c.Description,
                c.Email,
                c.PhoneNumber,
                new CompanyDetailsLocationDto(
                    c.Location.CountryCode,
                    c.Location.City,
                    c.Location.Street,
                    c.Location.PostCode)))
            .FirstOrDefaultAsync(cancellationToken);

        // an alternative to using exceptions for flow of control is to use the result pattern
        return result ?? throw new NotFoundException(query.CompanyId.ToString(), nameof(Company));
    }
}