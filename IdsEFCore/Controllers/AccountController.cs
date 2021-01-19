
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using IDS.Database;
using IDS.Database.Entities;
using IdsEFCore.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsentRequest = IdsEFCore.Model.ConsentRequest;

namespace IdsEFCore.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IDSContext context;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IIdentityServerInteractionService interaction;
        private readonly IAuthenticationSchemeProvider authenticationSchemeProvider;
        private readonly IEventService events;

        public AccountController(
            IDSContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IIdentityServerInteractionService interaction,
            IAuthenticationSchemeProvider authenticationSchemeProvider,
            IEventService events
            )
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.interaction = interaction;
            this.authenticationSchemeProvider = authenticationSchemeProvider;
            this.events = events;
        }

        [Route("Account/LoginAsync")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(
            [FromBody] UserDTO model
            )
        {
            User user = null;
            user = await userManager.FindByNameAsync(model.Account);
            if (user == null)
                throw new Exception("账号不存在");

            Microsoft.AspNetCore.Identity.SignInResult sr = null;
            sr = await signInManager.PasswordSignInAsync(user, model.Password, true, false);

            if (sr.Succeeded == false)
            {
                throw new Exception("密码错误");
            }
            return Ok();
        }
        [Route("Account/Logout")]
        [HttpGet]
        public async Task<IActionResult> Logout(
        [FromQuery] string logoutId
        )
        {
            var logout = await interaction.GetLogoutContextAsync(logoutId);
            await signInManager.SignOutAsync();
            //获取客户端点击注销登录的地址
            var refererUrl = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrWhiteSpace(refererUrl))
            {
                return Redirect(refererUrl);
            }
            //获取配置的默认的注销登录后的跳转地址
            if (logout.PostLogoutRedirectUri != null)
            {
                return Redirect(logout.PostLogoutRedirectUri);
            }
            return Ok();
        }
        [Route("Account/GetConsentContext")]
        [HttpGet]
        public async Task<IActionResult> GetProGetConsentContextAsync([FromQuery] string returnUrl)
        {
            var request = await interaction.GetAuthorizationContextAsync(returnUrl);
            //用于刷新缓存
            var client = await context.Clients.FirstOrDefaultAsync(x => x.ClientId == request.Client.ClientId);
            var result = new ConsentContextModel()
            {
                AllowRememberConsent = true,
                Logo = request.Client.LogoUri,
                ClientUrl = request.Client.ClientUri,
                ClientName = request.Client.ClientName,
                IdentityScopes = request.ValidatedResources.Resources.IdentityResources.Select(x => CreateScopeModel(x, false)).OrderBy(x => x.Required).ToArray(),
                RequireConsent = client.RequireConsent
            };
            var apiScopes = new List<ScopeModel>();
            foreach (var parsedScope in request.ValidatedResources.ParsedScopes)
            {
                var apiScope = request.ValidatedResources.Resources.FindApiScope(parsedScope.ParsedName);
                if (apiScope != null)
                {
                    var scopeVm = CreateScopeModel(parsedScope, apiScope, true);
                    apiScopes.Add(scopeVm);
                }
            }
            if (request.ValidatedResources.Resources.OfflineAccess)
            {
                apiScopes.Add(GetOfflineAccessScope(true));
            }
            result.ApiScopes = apiScopes;
            return Ok(result);
        }

        [Route("Account/ProcessConsent")]
        public async Task<IActionResult> ProcessConsentAsync([FromBody] ConsentRequest model)
        {
            var request = await interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            var result = new ProcessConsentResult();
            ConsentResponse grantedConsent = null;
            if (model.Type == "no")
            {
                grantedConsent = new ConsentResponse { Error = AuthorizationError.AccessDenied };
            }
            else if (model.Type == "yes")
            {
                if (model.ScopesConsented != null && model.ScopesConsented.Any())
                {
                    var scopes = model.ScopesConsented;
                    //scopes = scopes.Where(x => x != IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess);
                    grantedConsent = new ConsentResponse
                    {
                        RememberConsent = model.RememberConsent,
                        ScopesValuesConsented = scopes.ToArray(),
                        Description = model.Description
                    };
                }
                else
                {
                    result.ValidationError = "You must pick at least one permission";
                }
            }
            else
            {
                result.ValidationError = "Invalid selection";
            }
            if (grantedConsent != null)
            {
                await interaction.GrantConsentAsync(request, grantedConsent);
                result.RedirectUri = model.ReturnUrl;
            }

            return Ok(result);
        }



        ScopeModel CreateScopeModel(ParsedScopeValue parsedScopeValue, ApiScope apiScope, bool check)
        {
            return new ScopeModel
            {
                Value = parsedScopeValue.RawValue,
                // todo: use the parsed scope value in the display?
                DisplayName = apiScope.DisplayName ?? apiScope.Name,
                Description = apiScope.Description,
                Emphasize = apiScope.Emphasize,
                Required = apiScope.Required,
                Checked = check || apiScope.Required
            };
        }
        ScopeModel CreateScopeModel(IdentityResource identity, bool check)
        {
            return new ScopeModel
            {
                Value = identity.Name,
                DisplayName = identity.DisplayName ?? identity.Name,
                Description = identity.Description,
                Emphasize = identity.Emphasize,
                Required = identity.Required,
                Checked = check || identity.Required
            };
        }

        private ScopeModel GetOfflineAccessScope(bool check)
        {
            return new ScopeModel
            {
                Value = IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess,
                DisplayName = "Offline Access",
                Description = "Access to your applications and resources, even when you are offline",
                Emphasize = true,
                Checked = check
            };
        }
        /// <summary>
        /// 注册账号
        /// </summary>
        /// <param name="registerModel"></param>
        /// <returns></returns>
        [Route("Account/Register")]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegisterDTO registerModel)
        {
            var user = new User()
            {
                Nickname = registerModel.NickName,
                PhoneNumber = registerModel.Mobile
            };

            user.UserName = registerModel.Account;
            var result = await userManager.CreateAsync(user);
            if (result.Succeeded == false)
            {
                if (result.Errors.Any(p => p.Code == nameof(IdentityErrorDescriber.DuplicateUserName)))
                {
                    throw new Exception("该账号已存在");
                }
                else if (result.Errors.Any(p => p.Code == nameof(IdentityErrorDescriber.DuplicateEmail)))
                {
                    throw new Exception("该邮箱已存在");
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(registerModel.Password) == false)
                {
                    await userManager.AddPasswordAsync(user, registerModel.Password);
                }
                await signInManager.SignInAsync(user, true);
            }
            return Ok();
        }
    }

}
