using Argo.CA.Application.Common.Security;

namespace Argo.CA.Api.IntegrationTests.Testing.Authentication;

public static class JwtTokenBuilderExtensions
{
    public static JwtTokenBuilder AsAdmin(this JwtTokenBuilder builder)
    {
        return builder
            .WithName(Constants.Constants.User.Admin.Name)
            .WithEmail(Constants.Constants.User.Admin.Email)
            .WithRole(Roles.Admin);
    }

    public static JwtTokenBuilder AsEditor(this JwtTokenBuilder builder)
    {
        return builder
            .WithName(Constants.Constants.User.Editor.Name)
            .WithEmail(Constants.Constants.User.Editor.Email)
            .WithRole(Roles.Editor);
    }

    public static JwtTokenBuilder AsReader(this JwtTokenBuilder builder)
    {
        return builder
            .WithName(Constants.Constants.User.Reader.Name)
            .WithEmail(Constants.Constants.User.Reader.Email)
            .WithRole(Roles.Reader);
    }
}