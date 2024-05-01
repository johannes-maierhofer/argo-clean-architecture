namespace Argo.CA.Application.Common.Persistence;

using Domain.CompanyAggregate;
using Microsoft.EntityFrameworkCore;

public interface IAppDbContext
{
    public DbSet<Company> Companies { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
   
}
