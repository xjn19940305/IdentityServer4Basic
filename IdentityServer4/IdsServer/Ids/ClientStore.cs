using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdsServer.Ids
{
    public class ClientStore : IClientStore
    {
        public static List<Client> list = new List<Client>
        {
            new Client
            {
                ClientId = "ClientDemo",
                ClientName="ClientDemo",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.Implicit,
                //重定向地址
                RedirectUris = { "https://localhost:6001/signin-oidc" },
                RequireConsent=false,
                PostLogoutRedirectUris = { "https://localhost:6001/signout-callback-oidc" },
                //允许将token通过浏览器传递
                AllowAccessTokensViaBrowser=true,
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "api"
                }
            },
            new Client
            {
                ClientId = "CodeDemo",
                ClientName = "CodeDemo",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                //重定向地址
                RedirectUris = { "https://localhost:7001/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:7001/signout-callback-oidc" },
                //是否需要授权页
                RequireConsent=true,
                //允许将token通过浏览器传递
                AllowAccessTokensViaBrowser=true,
                AllowedScopes = new List<string>
                {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api"
                }
            }
        };
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            await Task.Delay(1000);
            return list.FirstOrDefault(x => x.ClientId == clientId);
        }
    }
}
