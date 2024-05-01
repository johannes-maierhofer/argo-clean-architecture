namespace Argo.CA.Application.Common.CQRS;

using MediatR;

// See Mediatr IRequest interface at
// https://github.com/jbogard/MediatR/blob/master/src/MediatR.Contracts/IRequest.cs

/// <summary>
/// Marker interface to represent a command with a void response
/// </summary>
public interface ICommand : IRequest, IBaseCommand;

/// <summary>
/// Marker interface to represent a command with a response
/// </summary>
/// <typeparam name="TResponse">Response type</typeparam>
public interface ICommand<out TResponse> : IRequest<TResponse>, IBaseCommand
    where TResponse : notnull;

/// <summary>
/// Allows for generic type constraints of objects implementing ICommand or ICommand{TResponse}
/// </summary>
public interface IBaseCommand;