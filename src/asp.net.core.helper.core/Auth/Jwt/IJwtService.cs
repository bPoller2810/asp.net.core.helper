using asp.net.core.helper.core.Auth;

namespace asp.net.core.helper.core.Jwt
{
    public interface IJwtService
    {
        JwtValidationResult ValidateToken(string? token);
        JwtCreationResult CreateToken(string userId, string username);
    }
}
