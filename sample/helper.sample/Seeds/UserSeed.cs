using asp.net.core.helper.core.Seed;
using helper.sample.Database;
using BCryptNet = BCrypt.Net.BCrypt;

namespace helper.sample.Seeds
{
    public class UserSeed : BaseSeed<SampleContext>
    {
        #region ctor
        public UserSeed(SampleContext context) : base(context)
        {
        }
        #endregion

        #region BaseSeed<SampleContext>
        protected override string Key => "User";

        protected override int Order => 1;

        protected override bool PerformSeed()
        {
            try
            {
                Context.Users.Add(new User
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = BCryptNet.HashPassword("admin"),
                });
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
