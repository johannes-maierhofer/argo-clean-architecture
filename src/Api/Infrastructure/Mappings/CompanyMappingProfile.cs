namespace Argo.CA.Api.Infrastructure.Mappings;

using Application.Companies.Queries.GetCompanyDetails;
using Application.Companies.Queries.GetCompanyList;
using AutoMapper;
using Contracts.Companies;

public class CompanyMappingProfile : Profile
{
    public CompanyMappingProfile()
    {
        CreateMap<CompanyListDto, CompanyListItem>();
        CreateMap<CompanyDetailsDto, GetCompanyDetailsResponse>();
        CreateMap<CompanyDetailsLocationDto, CompanyDetailsLocation>();
    }
}
