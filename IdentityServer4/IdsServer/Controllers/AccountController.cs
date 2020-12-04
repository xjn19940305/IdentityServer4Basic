using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdsServer.Controllers
{
    public class AccountController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IEventService _events;

        public AccountController(IIdentityServerInteractionService identityServerInteractionService,
            IEventService events)
        {
            this._interaction = identityServerInteractionService;
            this._events = events;
        }
        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.ReturnUrl = Request.Query["ReturnUrl"];
            return View();
        }
        [Route("Consent")]
        public async Task<IActionResult> Consent(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            ViewBag.returnUrl = $"/authorize?grant_type=authorization_code&code=${Guid.NewGuid().ToString().Substring(0, 12)}&redirect_uri={context.RedirectUri}";
            return View();
        }

        public async Task<IActionResult> Red(string returnUrl)
        {
            return Redirect(returnUrl);
        }
        [HttpPost]
        public async Task<IActionResult> Login(string Account, string Password, string ReturnUrl)
        {
            if (Account == "test" && Password == "123")
            {
                AuthenticationProperties props = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(30))
                };
                // issue authentication cookie with subject ID and username
                var isuser = new IdentityServerUser("1")
                {
                    DisplayName = Account
                };
                await HttpContext.SignInAsync(isuser, props);
                if (!string.IsNullOrWhiteSpace(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
            }
            return Json("账号或密码错误!");
        }
    }
}
