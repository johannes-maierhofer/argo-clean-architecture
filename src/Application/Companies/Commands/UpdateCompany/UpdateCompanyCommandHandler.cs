using Argo.CA.Domain.Common.Exceptions;

namespace Argo.CA.Application.Companies.Commands.UpdateCompany;

using Domain.CompanyAggregate;
using Common.CQRS;
using Common.Persistence;
using Domain.CompanyAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

public class UpdateCompanyCommandHandler(IAppDbContext dbContext) : ICommandHandler<UpdateCompanyCommand>
{
    public async Task Handle(UpdateCompanyCommand command, CancellationToken cancellationToken)
    {
        var company = await dbContext.Companies
            .FirstOrDefaultAsync(c => c.Id == command.CompanyId, cancellationToken);

        // an alternative to using exceptions for flow of control is to use the result pattern
        if (company == null)
        {
            throw new NotFoundException(command.CompanyId.ToString(), nameof(Company));
        }

        company.Update(
            command.Name,
            command.Description,
            CompanyLocation.Create(
                command.CountryCode,
                command.City,
                command.Street,
                command.PostCode),
            command.PhoneNumber,
            command.Email);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}