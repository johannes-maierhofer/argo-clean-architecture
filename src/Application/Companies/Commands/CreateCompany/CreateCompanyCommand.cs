namespace Argo.CA.Application.Companies.Commands.CreateCompany;

using Common.CQRS;

public record CreateCompanyCommand(
    string Name,
    string? Description,
    string Street,
    string City,
    string PostCode,
    string CountryCode,
    string Email,
    string PhoneNumber
) : ICommand<Guid>;
