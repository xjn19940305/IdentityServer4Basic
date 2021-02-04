using IDS.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDS.Client.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IDSContext context;

        public AccountController(IDSContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [HttpPost]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync("IdsClientCookie");
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public async Task<IActionResult> GetTokenInfo()
        {
            var auth = await HttpContext.AuthenticateAsync();
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            var idToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            return Ok(new { accessToken, refreshToken });
        }
        [Route("/Account/AccessDenied")]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return Unauthorized();
        }

    }
}
