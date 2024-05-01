namespace Argo.CA.Application.Common.Authorization;

public class DummyCurrentUserService : ICurrentUserService
{
    public static readonly string DummyUserName = "dummy_user";

    public string GetName() => DummyUserName;
}