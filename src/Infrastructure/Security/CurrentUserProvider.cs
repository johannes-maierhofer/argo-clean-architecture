using Argo.CA.Application.Common.Security.CurrentUserProvider;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Argo.CA.Infrastructure.Security;

public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    public CurrentUser GetCurrentUser()
    {
        if (httpContextAccessor.HttpContext == null)
        {
            return new CurrentUser(Guid.Empty, string.Empty, string.Empty, false, []);
        }

        var id = Guid.Empty;
        if (Guid.TryParse(GetClaimValue(ClaimTypes.NameIdentifier), out Guid idFromClaim))
        {
            id = idFromClaim;
        }

        var userName = GetClaimValue(ClaimTypes.Name);
        var email = GetClaimValue(ClaimTypes.Email);
        var roles = GetClaimValues(ClaimTypes.Role);
        bool isAuthenticated = httpContextAccessor.HttpContext.User.Identity?.IsAuthenticated ?? false;

        return new CurrentUser(
            id,
            userName ?? string.Empty,
            email ?? string.Empty,
            isAuthenticated,
            roles);
    }

    private List<string> GetClaimValues(string claimType) =>
        httpContextAccessor.HttpContext!.User.Claims
            .Where(claim => claim.Type == claimType)
            .Select(claim => claim.Value)
            .ToList();

    private string? GetClaimValue(string claimType) =>
        httpContextAccessor.HttpContext!.User.Claims
            .SingleOrDefault(claim => claim.Type == claimType)?
            .Value;
}