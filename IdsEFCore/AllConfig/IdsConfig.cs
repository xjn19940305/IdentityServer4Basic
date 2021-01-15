using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdsEFCore.AllConfig
{
    public static class IdsConfig
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
                new IdentityResources.Address()
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("api1"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "Code_Client_Demo",
                    ClientName="Code_Client_Demo",
                    ClientSecrets = { new Secret("Code_Client_Demo".Sha256(),"desc") },
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    //登录成功后返回的客户端地址
                    RedirectUris={"http://localhost:5002/signin-oidc"},
                    FrontChannelLogoutUri="http://localhost:5002/signout-oidc",
                    //注销登录后返回的客户端地址
                    PostLogoutRedirectUris={"http://localhost:5002/signout-callback-oidc"},
                    AlwaysIncludeUserClaimsInIdToken=true,//将用户所有的claims包含在IdToken内
                    AllowOfflineAccess=true,//offline_access，其实指的是能否用refreshtoken重新申请令牌
                    AllowedScopes =
                    {
                        "api1",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Phone,
                        IdentityServerConstants.StandardScopes.Email
                    }
                },
            };
    }
}
