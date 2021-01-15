using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdsEFCore.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TestController : ControllerBase
    {
        private readonly ConfigurationDbContext context;

        public TestController(ConfigurationDbContext Context)
        {
            this.context = Context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await context.Clients.AddAsync(new IdentityServer4.EntityFramework.Entities.Client
            {
                 ClientId="Test001",
                 ClientName="测试客户端"
            });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
