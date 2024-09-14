namespace Argo.CA.Application.Common.Security.CurrentUserProvider;

public record CurrentUser(
    Guid Id,
    string UserName,
    string Email,
    bool IsAuthenticated,
    IReadOnlyList<string> Roles);