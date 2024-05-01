namespace Argo.CA.Application.Companies.Commands.CreateCompany;

using Common.CQRS;
using Common.Persistence;
using Domain.CompanyAggregate;
using Domain.CompanyAggregate.ValueObjects;

public class CreateCompanyCommandHandler(IAppDbContext dbContext) : ICommandHandler<CreateCompanyCommand, Guid>
{
    public async Task<Guid> Handle(CreateCompanyCommand command, CancellationToken cancellationToken)
    {
        var company = Company.Create(
            command.Name,
            command.Description,
            CompanyLocation.Create(
                command.CountryCode,
                command.City,
                command.Street,
                command.PostCode),
            command.PhoneNumber,
            command.Email);

        await dbContext.Companies.AddAsync(company, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return company.Id;
    }
}