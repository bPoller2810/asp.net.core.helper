using System;

namespace asp.net.core.helper.core.Auth
{
    public record AuthenticationRequest(string Username, string Password);
    public record AuthenticationResponse(string Username, string Token, DateTime ExpirationUtc);
    public record JwtValidationResult(bool Succes, string? UserId, string? Username);
    public record JwtCreationResult(string Token, DateTime ExpirationUtc);
}
