using IDS.Database;
using Microsoft.AspNetCore.Authentication;
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
        [Route("/Account/Login")]
        [HttpGet]
        public IActionResult Login([FromQuery] string ReturnUrl)
        {
            return Redirect($"/user/login?ReturnUrl={ReturnUrl}");
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

    }
}
