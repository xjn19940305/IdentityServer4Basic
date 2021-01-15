using IDS.Database;
using IDS.Database.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdsEFCore
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
            var config = Configuration.GetSection("Connection");
            services.AddDbContext<IDSContext>(
            options => options.UseMySql(config?.Value ?? string.Empty, mysql =>
            {
                var builder = mysql
                     .MigrationsAssembly(System.Reflection.Assembly.Load("IDS.MySql").FullName)
                     .EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), null);
            }));
            services.AddIdentity<IDS.Database.Entities.User, Microsoft.AspNetCore.Identity.IdentityRole>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequiredLength = 6;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
            })
           //.AddClaimsPrincipalFactory<Providers.UserClaimsPrincipalFactory>()
           .AddDefaultTokenProviders()
           .AddEntityFrameworkStores<IDSContext>();

            services
                .AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddJwtBearerClientAuthentication()
                .AddAppAuthRedirectUriValidator()
                .AddMutualTlsSecretValidators()
                .AddAspNetIdentity<User>()
                .AddOperationalStore<IDSContext>()
                .AddConfigurationStore<IDSContext>()
                //.AddSigningCredential(jwk, "RS256")
                //.AddExtensionGrantValidator<CodeExtensionGrantValidator>()
                .AddConfigurationStoreCache();
            //.AddRedisCaching(r =>
            //{
            //    var cacheConfig = Configuration.GetSection("Cache");
            //    var connectionName = cacheConfig["ConnectionName"];
            //    var dbconfig = Configuration.GetDatabaseConfiguration(connectionName);
            //    r.KeyPrefix = cacheConfig["Prefix"] ?? "IDFAS";
            //    r.RedisConnectionString = dbconfig.Connection;

            //});


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseIdentityServer();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
