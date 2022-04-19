using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace asp.net.core.helper.core.Seed.Extensions
{
    public static class WebAppExtensions
    {
        public static void MigrateDatabase<TDbContext>(this IApplicationBuilder self)
            where TDbContext : DbContext
        {
            var scope = self.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
            context?.Database.Migrate();
        }

        public static void SeedDatabase<TDbContext>(this IApplicationBuilder self, Type assemblyType,IServiceProvider serviceProvider)
            where TDbContext : DbContext
        {
            var scope = self.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TDbContext>();

            var baseSeedType = typeof(BaseSeed<TDbContext>);
            var seeders = assemblyType
                .Assembly
                .GetExportedTypes()
                .Where(t => baseSeedType.IsAssignableFrom(t) && !t.IsAbstract)
                .Select(t => Activator.CreateInstance(t, context) as BaseSeed<TDbContext>);

            foreach (var seeder in seeders)
            {
                seeder?.Seed(serviceProvider);
            }
        }
    }
}
