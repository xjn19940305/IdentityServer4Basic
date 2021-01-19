using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HybirdDemo.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public async Task<IActionResult> IndexAsync()
        {
            var auth = await HttpContext.AuthenticateAsync();
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            var idToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            ViewBag.accessToken = accessToken;
            ViewBag.idToken = idToken;
            ViewBag.refreshToken = refreshToken;
            return View();
        }
    }
}
