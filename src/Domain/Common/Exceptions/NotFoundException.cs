namespace Argo.CA.Domain.Common.Exceptions;

public class NotFoundException(string key, string objectName)
    : CustomException($"Queried object {objectName} was not found, Key: {key}");