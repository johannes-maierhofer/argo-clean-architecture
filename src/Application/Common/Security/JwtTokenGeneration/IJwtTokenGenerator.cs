namespace Argo.CA.Application.Common.Security.JwtTokenGeneration;

public interface IJwtTokenGenerator
{
    string GenerateToken(
        string id,
        string userName,
        string email,
        IList<string> roles);
}