namespace Argo.CA.Application.Common.Auth;

public record CurrentUser(
    Guid Id,
    string UserName,
    string Email,
    bool IsAuthenticated,
    IReadOnlyList<string> Roles);