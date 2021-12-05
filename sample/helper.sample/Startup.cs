using asp.net.core.helper.core.Extensions;
using asp.net.core.helper.core.Seed.Extensions;
using helper.sample.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace helper.sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "helper.sample", Version = "v1" });
            });

            var connectionString = "server=localhost;user=root;password=unsecure1Admin;database=SampleX1";
            var version = new Version(10, 6, 4);
            var serverVersion = new MariaDbServerVersion(version);
            services.AddDbContext<SampleContext>(options =>
                options.UseMySql(connectionString, serverVersion));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "helper.sample v1"));
            }

            app.UseRouting();

            app.MigrateDatabase<SampleContext>();
            app.SeedDatabase<SampleContext>(typeof(Startup));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBpAuthenticationController("/authenticate");
                endpoints.MapControllers();
            });
        }
    }
}
