using asp.net.core.helper.core.Auth;
using asp.net.core.helper.core.Auth.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace asp.net.core.helper.core.Jwt
{
    internal class DefaultJwtService : IJwtService
    {
        private readonly IAuthenticationConfiguration _authenticationConfiguration;

        public DefaultJwtService(IAuthenticationConfiguration authenticationConfiguration)
        {
            _authenticationConfiguration = authenticationConfiguration;
        }


        #region IJwtService
        public JwtValidationResult ValidateToken(string? token)
        {
            if (token is null)
            {
                return new JwtValidationResult(false, null, null);
            }
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_authenticationConfiguration.Key);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                if (validatedToken is not JwtSecurityToken jwtToken)
                {
                    throw new InvalidOperationException(Constants.ERR_INVALID_TOKEN_TYPE);
                }
                var id = jwtToken.Claims.First(c => c.Type == Constants.USER_ID);
                var username = jwtToken.Claims.First(c => c.Type == Constants.USER_NAME);

                return new JwtValidationResult(true, id.Value, username.Value);
            }
            catch
            {
                // return false if validation fails
                return new JwtValidationResult(false, null, null);
            }
        }
        public JwtCreationResult CreateToken(string userId, string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authenticationConfiguration.Key);
            var expiration = DateTime.UtcNow.AddMinutes(_authenticationConfiguration.TokenLifetime);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(Constants.USER_ID, userId),
                    new Claim(Constants.USER_NAME, username),
                }),
                Expires = expiration,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new JwtCreationResult(tokenHandler.WriteToken(token), expiration);
        }

        #endregion
    }
}
