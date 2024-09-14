namespace Argo.CA.Contracts.Authentication;

public record LoginRequest(
    string Email,
    string Password);
