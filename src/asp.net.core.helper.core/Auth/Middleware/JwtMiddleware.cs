using asp.net.core.helper.core.Auth;
using asp.net.core.helper.core.Auth.Services;
using asp.net.core.helper.core.Jwt;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace asp.net.core.helper.core.Middleware
{
    internal class JwtMiddleware
    {

        #region middleware
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IJwtService jwtUtils, IAuthenticationConfiguration authenticationConfiguration)
        {
            var token = context
                .Request
                .Headers[Constants.HEADER_AUTH]
                .FirstOrDefault()?
                .Split(Constants.SPACE)
                .Last();
            context.Items[Constants.KEY_TOKEN] = token;

            var result = jwtUtils.ValidateToken(token);
            if (result.Succes 
                && result.Username is not null
                && result.UserId is not null
                && authenticationConfiguration.IsUserAuthenticationAllowed(result.UserId, result.Username))
            {
                context.Items[Constants.KEY_USER_ID] = result.UserId;
                context.Items[Constants.KEY_TOKEN_VALID] = true;
            }


            await _next(context);
        }
        #endregion

    }
}
