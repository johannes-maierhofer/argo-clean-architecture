using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Argo.CA.Api.IntegrationTests.Testing.Authentication;

public class JwtTokenBuilder
{
    private readonly List<Claim> _claims = new();
    private int _expiresInMinutes = 30;

    public static JwtTokenBuilder Create()
    {
        return new JwtTokenBuilder();
    }

    public JwtTokenBuilder WithName(string name)
    {
        return WithClaim(new Claim(ClaimTypes.Name, name));
    }

    public JwtTokenBuilder WithEmail(string email)
    {
        return WithClaim(new Claim(ClaimTypes.Email, email));
    }

    public JwtTokenBuilder WithRole(string role)
    {
        return WithClaim(new Claim(ClaimTypes.Role, role));
    }

    public JwtTokenBuilder WithClaim(Claim claim)
    {
        _claims.Add(claim);
        return this;
    }

    public JwtTokenBuilder WithClaims(params Claim[] additionalClaims)
    {
        _claims.AddRange(additionalClaims);
        return this;
    }

    public JwtTokenBuilder WithExpiration(int expiresInMinutes)
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
