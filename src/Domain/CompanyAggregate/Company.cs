namespace Argo.CA.Domain.CompanyAggregate;

using Ardalis.GuardClauses;
using Common;
using Events;
using ValueObjects;

public class Company : AuditableAggregate<Guid>
{
    private Company(
        Guid id,
        string name,
        string? description,
        CompanyLocation location,
        string phoneNumber,
        string email)
    {
        Id = id;
        Name = name;
        Description = description;
        Location = location;
        PhoneNumber = phoneNumber;
        Email = email;
    }

    private Company()
    {
        // EF Core only
    }

    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string PhoneNumber { get; private set; }

    // TODO: use Email value object
    public string Email { get; private set; }
    public CompanyLocation Location { get; private set; }

    public static Company Create(
        string name,
        string? description,
        CompanyLocation location,
        string phoneNumber,
        string email)
    {
        Guard.Against.Null(name);

        // TODO: email must be valid

        var company = new Company(
            Guid.NewGuid(),
            name,
            description,
            location,
            phoneNumber,
            email);

        company.DomainEvents.Add(new CompanyCreated(company));

        return company;
    }

    public void Update(
        string name,
        string description,
        CompanyLocation location,
        string phoneNumber,
        string email)
    {
        Guard.Against.Null(name);
        Guard.Against.Null(email);

        SetName(name);
        SetEmail(email);
        SetLocation(location);

        Description = description;
        PhoneNumber = phoneNumber;

        DomainEvents.Add(new CompanyUpdated(this));
    }

    public void SetName(string name)
    {
        Guard.Against.Null(name);

        if (name != Name)
        {
            DomainEvents.Add(new CompanyNameChanged(Id, name, Name));
        }

        Name = name;
    }

    public void SetEmail(string email)
    {
        Guard.Against.Null(email);

        if (!string.Equals(Email, email, StringComparison.InvariantCultureIgnoreCase))
        {
            DomainEvents.Add(new CompanyEmailChanged(Id, email, Email));
        }

        Email = email;
    }

    public void SetLocation(CompanyLocation location)
    {
        if (Location.Equals(location))
        {
            return;
        }

        DomainEvents.Add(new CompanyLocationChanged(Id, location, Location));
        Location = location;
    }
}