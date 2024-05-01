namespace Argo.CA.Application.Companies.Commands.UpdateCompany;

using Common.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

public class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
{
    private readonly IAppDbContext _dbContext;

    public UpdateCompanyCommandValidator(IAppDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x)
            .MustAsync(NoOtherCompanyWithSameName)
            .WithMessage("Another company with this name already exists.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.Street)
            .MaximumLength(100);

        RuleFor(x => x.City)
            .MaximumLength(100);

        RuleFor(x => x.PostCode)
            .MaximumLength(100);

        // TODO: validate CountryCode is a valid ISO code
        RuleFor(x => x.CountryCode)
            .MaximumLength(2);

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("'{PropertyValue}' is not a valid email.");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(100);
    }

    private async Task<bool> NoOtherCompanyWithSameName(UpdateCompanyCommand cmd, CancellationToken cancellationToken)
    {
        var anotherCompanyWithSameNameExists = await _dbContext.Companies
            .AnyAsync(c => c.Id != cmd.CompanyId && c.Name == cmd.Name, cancellationToken);

        return !anotherCompanyWithSameNameExists;
    }
}