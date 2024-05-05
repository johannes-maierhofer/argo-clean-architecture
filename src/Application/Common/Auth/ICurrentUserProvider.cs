namespace Argo.CA.Application.Common.Auth;

public interface ICurrentUserProvider
{
    CurrentUser GetCurrentUser();
}