namespace Argo.CA.Application.Common.CQRS;

public interface ICompanyCommand : ICommand
{
    public Guid CompanyId { get; }
}
