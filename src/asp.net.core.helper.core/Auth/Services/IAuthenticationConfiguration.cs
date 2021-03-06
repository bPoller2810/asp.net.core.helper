using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace asp.net.core.helper.core.Auth.Services
{
    public interface IAuthenticationConfiguration
    {
        string Key { get; }
        double TokenLifetime { get; }

        string? GetUserIdByName(string username);
        string? GetUserHashById(string userId);
        bool IsUserAuthenticationAllowed(string userId, string username, HttpContext context);
        Task SuccessfullAuthentication(HttpContext context);
    }
}
