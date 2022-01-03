using asp.net.core.helper.core.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BCryptNet = BCrypt.Net.BCrypt;
using System.Threading.Tasks;
using asp.net.core.helper.core.Auth.Services;

namespace asp.net.core.helper.core.Auth
{
    internal static class AuthController
    {

        public static async Task Authenticate(HttpContext context)
        {
            #region check json body 
            if (!context.Request.HasJsonContentType())
            {
                context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
                return;
            }
            var body = await context.Request.ReadFromJsonAsync<AuthenticationRequest>();
            if (body is null)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }
            #endregion

            #region services
            var jwtService = context.RequestServices.GetService<IJwtService>();
            var authConfig = context.RequestServices.GetService<IAuthenticationConfiguration>();
            if (jwtService is null || authConfig is null)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                return;
            }
            #endregion

            #region user data
            var userId = authConfig.GetUserIdByName(body.Username);
            if (userId is null)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                return;
            }

            var userHash = authConfig.GetUserHashById(userId);
            if (userHash is null)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                return;
            }
            #endregion

            var verifyHash = BCryptNet.Verify(body.Password, userHash);
            if (!verifyHash)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var token = jwtService.CreateToken(userId, body.Username);

            var response = new AuthenticationResponse(body.Username, token.Token, token.ExpirationUtc);


            await context.Response.WriteAsJsonAsync(response);
            return;
        }

    }
}
