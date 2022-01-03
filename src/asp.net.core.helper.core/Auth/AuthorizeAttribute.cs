using asp.net.core.helper.core.Auth.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace asp.net.core.helper.core.Auth
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            #region skip authorization AllowAnonymous
            var allowAnonymous = context
                .ActionDescriptor
                .EndpointMetadata
                .OfType<AllowAnonymousAttribute>()
                .Any();
            if (allowAnonymous)
            {
                return;
            }
            #endregion

            #region token
            var token = (string?)context.HttpContext.Items[Constants.KEY_TOKEN];
            var tokenOk = (bool?)context.HttpContext.Items[Constants.KEY_TOKEN_VALID];
            var userId = (string?)context.HttpContext.Items[Constants.KEY_USER_ID];
            if (tokenOk is not true || token is null || userId is null)
            {
                HandleUnauthorized(context);
                return;
            }
            #endregion

            return;//auth okay
        }

        private static void HandleUnauthorized(AuthorizationFilterContext context)
        {
            context.Result = new JsonResult(new
            {
                message = Constants.UNAUTHORIZED
            })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }
    }

}
