using Argo.CA.Domain.CompanyAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Argo.CA.Application.Common.Persistence
{
    public interface IAppDbContext
    {
        public DbSet<Company> Companies { get; }

        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}