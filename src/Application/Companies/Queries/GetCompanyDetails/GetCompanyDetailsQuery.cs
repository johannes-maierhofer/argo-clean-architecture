namespace Argo.CA.Application.Companies.Queries.GetCompanyDetails;

using Common.CQRS;

public record GetCompanyDetailsQuery(
    Guid CompanyId
) : IQuery<CompanyDetailsDto>;
