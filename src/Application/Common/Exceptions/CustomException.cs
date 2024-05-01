namespace Argo.CA.Application.Common.Exceptions;

public class CustomException : Exception
{
    protected CustomException(string message) : base(message)
    {
    }
}