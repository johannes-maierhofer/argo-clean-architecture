namespace Argo.CA.Api.IntegrationTests.Testing.TestDoubles;

using Application.Common.Authorization;
using Constants;

public class FakeCurrentUserService : ICurrentUserService
{
    public string GetName()
    {
        return Constants.User.UserName;
    }
}
