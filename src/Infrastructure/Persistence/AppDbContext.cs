namespace Argo.CA.Infrastructure.Persistence
{
    using Application.Common.Persistence;
    using Domain.CompanyAggregate;
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext(DbContextOptions<AppDbContext> options)
        : DbContext(options), IAppDbContext, ITransactionalDbContext
    {
        public DbSet<Company> Companies => Set<Company>();
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
