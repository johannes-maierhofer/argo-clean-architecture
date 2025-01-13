using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Argo.CA.Api.IntegrationTests.Testing.Authentication;

// see https://github.com/FrodeHus/testing-jwt-apps/tree/main

public static class TestJwtTokenProvider
{
    public static string Issuer => "Test_Auth_Server";

    public static SecurityKey SecurityKey { get; } =
        new SymmetricSecurityKey("This_is_a_super_secure_key_and_you_know_it"u8.ToArray());

    public static SigningCredentials SigningCredentials { get; } = new(SecurityKey, SecurityAlgorithms.HmacSha256);
    public static readonly JwtSecurityTokenHandler JwtSecurityTokenHandler = new();
}
