namespace Argo.CA.Domain.CompanyAggregate.ValueObjects;

using Common;

public class CompanyLocation : ValueObject
{
    // TODO: make countryCode a ValueObject
    public static CompanyLocation Create(
        string countryCode,
        string city,
        string street,
        string postCode)
    {
        // TODO: enforce invariants (CompanyLocation)
        return new CompanyLocation(
            countryCode,
            city,
            street,
            postCode);
    }

    private CompanyLocation(
        string countryCode,
        string city,
        string street,
        string postCode)
    {
        CountryCode = countryCode;
        City = city;
        Street = street;
        PostCode = postCode;
    }

    public string CountryCode { get; }
    public string City { get; }
    public string Street { get; }
    public string PostCode { get; }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return CountryCode;
        yield return City;
        yield return Street;
        yield return PostCode;
    }
}