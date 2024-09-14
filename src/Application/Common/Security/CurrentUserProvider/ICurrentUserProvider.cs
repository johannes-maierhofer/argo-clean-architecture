namespace Argo.CA.Application.Common.Security.CurrentUserProvider;

public interface ICurrentUserProvider
{
    CurrentUser GetCurrentUser();
}