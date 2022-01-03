using asp.net.core.helper.core.Auth.Services;
using BCryptNet = BCrypt.Net.BCrypt;

namespace helper.sample.Services
{
    public class SampleAuthenticationConfiguration : IAuthenticationConfiguration
    {
        public const string ROUTE = "/auth";

        #region IAuthenticationConfiguration
        public string Key => "here lies the app secret. pssst dont tell anybody right now";

        public double TokenLifetime => 90;

        public string? GetUserHashById(string userId)
        {
            return userId switch
            {
                "1" => BCryptNet.HashPassword("test"),

                _ => null,
            };
        }

        public string? GetUserIdByName(string username)
        {
            return username switch
            {
                "admin" => "1",

                _ => null,
            };
        }
        public bool IsUserAuthenticationAllowed(string userId, string username)
        {
            return userId == "1" && username == "admin";
        }
        #endregion

    }
}
