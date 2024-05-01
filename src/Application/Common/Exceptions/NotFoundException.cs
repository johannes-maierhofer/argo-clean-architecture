namespace Argo.CA.Application.Common.Exceptions;

public class NotFoundException(string key, string objectName)
    : CustomException($"Queried object {objectName} was not found, Key: {key}");