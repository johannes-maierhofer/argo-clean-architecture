namespace Argo.CA.Application.Common.Persistence;

using Microsoft.EntityFrameworkCore.Infrastructure;

/// <summary>
/// Abstraction of DbContext to be used in BuildingBlocks
/// </summary>
public interface ITransactionalDbContext
{
    DatabaseFacade Database { get; }
}