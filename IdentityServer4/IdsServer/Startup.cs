using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdsServer.Ids;
using IdsServer.ValidateExtension;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdsServer
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
            services.AddControllersWithViews();
            services.AddIdentityServer(options =>
            {
                //可以通过此设置来指定登录路径，默认的登陆路径是/account/login
                options.UserInteraction.LoginUrl = "/Account/Login";
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;
            })
              //添加证书加密方式，执行该方法，会先判断tempkey.rsa证书文件是否存在，如果不存在的话，就创建一个新的tempkey.rsa证书文件，如果存在的话，就使用此证书文件。
              .AddDeveloperSigningCredential()
              //把受保护的Api资源添加到内存中
              .AddInMemoryApiResources(IdsConfig.GetApiResources())
              //客户端配置添加到内存中
              //.AddInMemoryClients(OAuthMemoruData.GetClients())
              //测试的用户添加进来
              //.AddTestUsers(OAuthMemoruData.GetTestUsers())
              //     .AddInMemoryIdentityResources(new List<IdentityResource>
              //{
              //    new IdentityResources.OpenId(), //未添加导致scope错误
              //    new IdentityResources.Profile()
              //})
              //身份信息资源
              .AddInMemoryIdentityResources(IdsConfig.GetIdentityResources())
              //添加自定义客户端
              .AddClientStore<ClientStore>()
              //添加自定义账号密码的方法
              .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
              //新增微信认证登陆的方法
              .AddExtensionGrantValidator<WechatLoginValidator>()
              .AddInMemoryApiScopes(IdsConfig.ApiScopes);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseIdentityServer();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
