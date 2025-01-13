using IdentityModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Argo.CA.Api.IntegrationTests.Testing.Authentication;

public class OidcJwtTokenBuilder
{
    private readonly List<Claim> _claims = new();
    private int _expiresInMinutes = 30;

    public static OidcJwtTokenBuilder Create()
    {
        return new OidcJwtTokenBuilder();
    }

    public OidcJwtTokenBuilder WithName(string name)
    {
        return WithClaim(new Claim(JwtClaimTypes.Name, name));
    }

    public OidcJwtTokenBuilder WithEmail(string email)
    {
        return WithClaim(new Claim(JwtClaimTypes.Email, email));
    }

    public OidcJwtTokenBuilder WithRole(string role)
    {
        return WithClaim(new Claim(JwtClaimTypes.Role, role));
    }

    public OidcJwtTokenBuilder WithClientId(string clientId)
    {
        return WithClaim(new Claim(JwtClaimTypes.ClientId, clientId));
    }

    public OidcJwtTokenBuilder WithScope(string scope)
    {
        return WithClaim(new Claim(JwtClaimTypes.Scope, scope));
    }

    public OidcJwtTokenBuilder WithAudience(string audience)
    {
        return WithClaim(new Claim(JwtClaimTypes.Audience, audience));
    }

    public OidcJwtTokenBuilder WithClaim(string type, string value)
    {
        _claims.Add(new Claim(type, value));
        return this;
    }

    public OidcJwtTokenBuilder WithClaim(Claim claim)
    {
        _claims.Add(claim);
        return this;
    }

    public OidcJwtTokenBuilder WithClaims(params Claim[] additionalClaims)
    {
        _claims.AddRange(additionalClaims);
        return this;
    }

    public OidcJwtTokenBuilder WithExpiration(int expiresInMinutes)
    {
        _expiresInMinutes = expiresInMinutes;
        return this;
    }

    public string Build()
    {
        var token = new JwtSecurityToken(
            TestJwtTokenProvider.Issuer,
            TestJwtTokenProvider.Issuer,
            _claims,
            expires: DateTime.Now.AddMinutes(_expiresInMinutes),
            signingCredentials: TestJwtTokenProvider.SigningCredentials
        );

        return TestJwtTokenProvider.JwtSecurityTokenHandler.WriteToken(token);
    }
}
