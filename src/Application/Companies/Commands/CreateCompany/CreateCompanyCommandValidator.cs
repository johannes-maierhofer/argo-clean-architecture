namespace Argo.CA.Application.Companies.Commands.CreateCompany;

using Common.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    private readonly IAppDbContext _dbContext;

    public CreateCompanyCommandValidator(IAppDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .MustAsync(BeUniqueName)
            .WithMessage("A company named '{PropertyValue}' already exists.");

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

    private async Task<bool> BeUniqueName(string companyName, CancellationToken cancellationToken)
    {
        return await _dbContext.Companies
            .AllAsync(c => c.Name != companyName, cancellationToken);
    }
}