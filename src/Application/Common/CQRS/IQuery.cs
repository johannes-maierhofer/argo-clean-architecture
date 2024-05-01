namespace Argo.CA.Application.Common.CQRS;

using MediatR;

public interface IQuery<out T> : IRequest<T>;
