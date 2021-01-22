using IDS.Database;
using IDS.Database.Entities;
using IdsEFCore.Extension;
using IdsEFCore.filter;
using IdsEFCore.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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
            var jwk = Configuration.GetSection("Jwk").Get<JsonWebKey>();

            services.AddControllers(o =>
            {
                o.Filters.Add(typeof(CustomeExceptionFilter));
            });
            var config = Configuration.GetSection("Connection");
            services.AddDbContext<IDSContext>(
            options => options.UseMySql(config?.Value ?? string.Empty, mysql =>
            {
                var builder = mysql
                     .MigrationsAssembly(System.Reflection.Assembly.Load("IDS.MySql").FullName)
                     .EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), null);
            }));
            services.AddIdentity<IDS.Database.Entities.User, IDS.Database.Entities.IdentityRole>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequiredLength = 6;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
            })
           .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory>()
           .AddDefaultTokenProviders()
           .AddEntityFrameworkStores<IDSContext>();

            services
                .AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    //options.UserInteraction.LoginUrl = "https://localhost:7001/user/login";
                })
                .AddJwtBearerClientAuthentication()
                .AddAppAuthRedirectUriValidator()
                .AddMutualTlsSecretValidators()
                .AddAspNetIdentity<User>()
                .AddOperationalStore<IDSContext>()
                .AddConfigurationStore<IDSContext>()
                .AddDeveloperSigningCredential()
                .AddSigningCredential(jwk, "RS256")
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

            var cors = (string[])Configuration.GetSection("Cors:Url").Get(typeof(string[]));
            services.AddCors(options =>
            {
                options.AddPolicy("cors",

                builder => builder.AllowAnyOrigin()

                .WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS")

                );
                options.AddPolicy("CustomCorsPolicy", policy =>
                {
                    policy.WithOrigins(cors)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
                options.AddPolicy("AllAllow", policy =>
                {
                    policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();

                });
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });
            services.TryAddScoped<RedisCache>();
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
            app.UseCors("CustomCorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseIdentityServer();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSpa(spa =>
            {
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:8001");
                }
            });
        }
    }
}
