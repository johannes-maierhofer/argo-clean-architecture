namespace Argo.CA.Infrastructure.Persistence.Configurations;

using Domain.CompanyAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");
        builder.HasKey(c => c.Id);

        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.PhoneNumber).HasMaxLength(100);
        builder.Property(x => x.Email).HasMaxLength(256);

        builder.OwnsOne(c => c.Location, l =>
        {
            l.Property(x => x.City).HasMaxLength(100);
            l.Property(x => x.PostCode).HasMaxLength(10);
            l.Property(x => x.Street).HasMaxLength(100);
            l.Property(x => x.CountryCode).HasMaxLength(2);
        });

        builder
            .HasIndex(x => x.Name)
            .IsUnique();
    }
}