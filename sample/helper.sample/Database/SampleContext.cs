using Microsoft.EntityFrameworkCore;

namespace helper.sample.Database
{
    public class SampleContext : DbContext
    {

        #region ctor
        public SampleContext() { }
        public SampleContext(DbContextOptions<SampleContext> options) : base(options) { }
        #endregion

        public DbSet<User> Users { get; set; }

    }
}
