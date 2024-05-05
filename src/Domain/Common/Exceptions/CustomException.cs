namespace Argo.CA.Domain.Common.Exceptions;

public class CustomException : Exception
{
    protected CustomException(string message) : base(message)
    {
    }
}