using Argo.CA.Application.Common.Auth;

namespace Argo.CA.Api.IntegrationTests.Testing.TestDoubles;

public class FakeCurrentUserProvider : ICurrentUserProvider
{
    private CurrentUser? _currentUser;

    public void Returns(CurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public CurrentUser GetCurrentUser() => _currentUser
                                           ?? new CurrentUser(
                                               Guid.Empty,
                                               Constants.Constants.User.UserName,
                                               Constants.Constants.User.Email,
                                               true,
                                               []);
}
