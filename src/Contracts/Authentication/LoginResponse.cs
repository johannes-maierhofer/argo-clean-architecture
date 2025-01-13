namespace Argo.CA.Contracts.Authentication;

public record LoginResponse(
    string? Username,
    string? Email,
    string? Token);
