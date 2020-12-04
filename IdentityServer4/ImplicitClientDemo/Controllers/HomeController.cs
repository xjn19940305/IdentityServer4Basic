using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ImplicitClientDemo.Models;
using Microsoft.AspNetCore.Authorization;

namespace ImplicitClientDemo.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        [HttpGet("GetData")]
        public string GetData()
        {
            return "这里是跳转回来的页面";
        }

        [HttpGet("GetClaim")]
        public Dictionary<string, string> GetClaim()
        {
            var obj = new Dictionary<string, string>();
            User.Claims.ToList().ForEach(x => obj.Add(x.Type, x.Value));
            return obj;
        }
    }
}
