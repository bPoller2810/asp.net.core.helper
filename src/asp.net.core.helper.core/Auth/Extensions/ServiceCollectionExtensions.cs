using Microsoft.AspNetCore.Mvc.Filters;
using System;
using asp.net.core.helper.core.Auth;
using Microsoft.Extensions.DependencyInjection;
using asp.net.core.helper.core.Jwt;
using asp.net.core.helper.core.Auth.Services;

namespace asp.net.core.helper.core.Extensions
{
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Adds and configures alle needed services to use BpAuthenitcation
        /// </summary>
        /// <param name="self"></param>
        /// <param name="authPredicate">action to check if the user exists and is allowed to log in</param>
        public static void AddBpAuthentication<TConfiguration>(this IServiceCollection self)
            where TConfiguration : class, IAuthenticationConfiguration
        {
            self.AddSingleton<IAuthenticationConfiguration, TConfiguration>();
            self.AddScoped<IJwtService, DefaultJwtService>();
        }

    }
}
