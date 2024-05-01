namespace Argo.CA.Application.Common.Authorization;

using Microsoft.AspNetCore.Http;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string GetName()
    {
        return httpContextAccessor.HttpContext?.User.Identity?.Name
               ?? httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "client_id")?.Value
               ?? string.Empty;
    }
}