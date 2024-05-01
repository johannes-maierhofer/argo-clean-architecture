namespace Argo.CA.Infrastructure.Persistence
{
    using Application.Common.Persistence;
    using Identity;
    using Domain.CompanyAggregate;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    // TODO: get rid of ITransactionalDbContext
    public class AppDbContext(DbContextOptions<AppDbContext> options)
        : IdentityDbContext<ApplicationUser>(options), IAppDbContext, ITransactionalDbContext
    {
        public DbSet<Company> Companies => Set<Company>();
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
