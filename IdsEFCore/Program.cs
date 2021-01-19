using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IDS.Database;
using IDS.Database.Entities;
using IdsEFCore.AllConfig;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
                var context = scope.ServiceProvider.GetRequiredService<IDSContext>();
                var shouldSeed = !await context.Database.CanConnectAsync();
                await context.Database.MigrateAsync();
                if (shouldSeed)
                {
                    var Usercontext = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                    await InitializeDatabase(context, Usercontext);
                    logger.LogInformation($"{DateTime.Now} 初始化数据库初始化数据");
                }
            }
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        private static async Task InitializeDatabase(IDSContext context, UserManager<User> userManager)
        {
            if (!context.Users.Any())
            {
                foreach (var user in IdsConfig.Users)
                {
                    await userManager.CreateAsync(user, user.PasswordHash);
                }
                await context.SaveChangesAsync();
            }
            if (!context.ApiResources.Any())
            {
                foreach (var res in IdsConfig.ApiResource)
                {
                    await context.ApiResources.AddAsync(res.ToEntity());
                }
                await context.SaveChangesAsync();
            }
            if (!context.Clients.Any())
            {
                foreach (var client in IdsConfig.Clients)
                {
                    await context.Clients.AddAsync(client.ToEntity());
                }
                await context.SaveChangesAsync();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in IdsConfig.IdentityResources)
                {
                    await context.IdentityResources.AddAsync(resource.ToEntity());
                }
                await context.SaveChangesAsync();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var resource in IdsConfig.ApiScopes)
                {
                    await context.ApiScopes.AddAsync(resource.ToEntity());
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
