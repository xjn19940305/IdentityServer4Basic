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
        // machine to machine client
            new Client
            {
                ClientId = "client",
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                // scopes that client has access to
                AllowedScopes = { "api" }
            },

            // interactive ASP.NET Core MVC client
            new Client
            {
                ClientId = "Code",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                //需要确认授权
                RequireConsent=true,
                RequirePkce=true,
                //允许token通过浏览器
                AllowAccessTokensViaBrowser=true,
                AlwaysIncludeUserClaimsInIdToken=true,
                //是否允许刷新token
                AllowOfflineAccess=true,
                // where to redirect to after login
                RedirectUris = { "https://localhost:6001/signin-oidc" },
                //RequireConsent=false,
                // where to redirect to after logout
                PostLogoutRedirectUris = { "https://localhost:6001/signout-callback-oidc" },
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "api"
                }
            },
              new Client
            {
                ClientId = "ClientDemo",
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.Implicit,

                // where to redirect to after login
                RedirectUris = { "https://localhost:7001/signin-oidc" },
                RequireConsent=false,
                // where to redirect to after logout
                PostLogoutRedirectUris = { "https://localhost:7001/signout-callback-oidc" },

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
