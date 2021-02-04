using AutoMapper;
using IdentityModel;
using IDS.Client.filter;
using IDS.Database;
using IDS.Service.Implement;
using IDS.Service.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace IDS.Client
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
            services.AddControllersWithViews(o =>
            {
                //o.Filters.Add(typeof(IdsPermissionFilter));
                o.Filters.Add(typeof(AuthorizeIDSFilter));
            });
            services
               .AddAutoMapper(
               Assembly.Load("IDS.Service"));
            services.TryAddScoped<IClientService, ClientService>();

            var config = Configuration.GetSection("Connection");
            services.AddDbContext<IDSContext>(
            options => options.UseMySql(config?.Value ?? string.Empty, mysql =>
            {
                var builder = mysql
                     .MigrationsAssembly(System.Reflection.Assembly.Load("IDS.MySql").FullName)
                     .EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), null);
            }));
            services.AddIdentityServer()
               .AddOperationalStore<IDSContext>()
               .AddConfigurationStore<IDSContext>();
            //jwt claim类型映射关闭
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "IdsClientCookie";
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                .AddCookie("IdsClientCookie")
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                {
                    options.SignInScheme = "IdsClientCookie";
                    options.Authority = "https://sso.xzsp2020.com";
                    options.RequireHttpsMetadata = false;
                    options.ClientId = "Ids_Client";
                    options.ClientSecret = "Ids_Client_secret";
                    //代表Authorization Code
                    options.ResponseType = "code id_token";
                    options.GetClaimsFromUserInfoEndpoint = true;
                    //保存到cookies中
                    options.SaveTokens = true;
                    options.UsePkce = false;
                    options.Scope.Clear();
                    options.Scope.Add(OidcConstants.StandardScopes.OpenId);
                    options.Scope.Add(OidcConstants.StandardScopes.Profile);
                    options.Scope.Add(OidcConstants.StandardScopes.OfflineAccess);
                    options.Scope.Add("IdsScope");
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSpa(spa =>
            {
                spa.ApplicationBuilder.Use(async (ctx, next) =>
                {
                    var res = await ctx.AuthenticateAsync();
                    if (!res.Succeeded)
                    {
                        await ctx.ChallengeAsync(new AuthenticationProperties()
                        {
                            RedirectUri = ctx.Request.Path
                        });
                    }
                    else
                    {
                        await next();
                    }

                });

                if (System.Diagnostics.Debugger.IsAttached)
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:8001");
                }
            });
        }
    }
}
