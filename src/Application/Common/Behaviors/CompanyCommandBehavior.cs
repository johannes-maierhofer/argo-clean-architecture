namespace Argo.CA.Application.Common.Behaviors;

using System.Diagnostics;
using System.Threading.Tasks;
using CQRS;
using MediatR;

public class CompanyCommandBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICompanyCommand
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        Activity.Current?.SetTag("company.id", request.CompanyId);

        return await next();
    }
}