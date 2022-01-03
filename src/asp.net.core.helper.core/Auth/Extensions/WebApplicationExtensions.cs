using asp.net.core.helper.core.Auth;
using asp.net.core.helper.core.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;

namespace asp.net.core.helper.core.Extensions
{
    public static class WebApplicationExtensions
    {

        public static void UseBpAuthentication(this IApplicationBuilder self)
        {
            self.UseMiddleware<JwtMiddleware>();
        }

        public static void MapBpAuthenticationController(
            this IEndpointRouteBuilder self,
            string route)
        {
            self.MapPost(route, AuthController.Authenticate);
        }
    }
}
