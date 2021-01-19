using IdentityServer4;
using IdentityServer4.Models;
using IDS.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdsEFCore.AllConfig
{
    public static class IdsConfig
    {

        public static IEnumerable<User> Users => new User[]
        {
            new User
            {
               Nickname="xia",
               Mobile="18661230305",
               UserName="milo",
               PasswordHash="123456"
            }
        };

        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Address(),
                new IdentityResource("Mobile",new string[]{ "Mobile"})
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
                    RedirectUris={"https://localhost:5001/signin-oidc"},
                    FrontChannelLogoutUri="https://localhost:5001/signout-oidc",
                    //注销登录后返回的客户端地址
                    PostLogoutRedirectUris={"https://localhost:5001/signout-callback-oidc"},
                    //将用户所有的claims包含在IdToken内
                    AlwaysIncludeUserClaimsInIdToken=true,
                    //offline_access，其实指的是能否用refreshtoken重新申请令牌
                    AllowOfflineAccess=true,
                    AllowedScopes =
                    {
                        "api1",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "Mobile"
                    }
                },
                new Client
                {
                    ClientId="hybrid_client",
                    ClientName="混合模式客户端",
                    ClientSecrets={ new Secret("hybrid_client".Sha256(),"desc") },
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    //登录成功后返回的客户端地址
                    RedirectUris={"https://localhost:6001/signin-oidc"},
                    FrontChannelLogoutUri="https://localhost:6001/signout-oidc",
                    //注销登录后返回的客户端地址
                    PostLogoutRedirectUris={"https://localhost:6001/signout-callback-oidc"},
                    //将用户所有的claims包含在IdToken内
                    AlwaysIncludeUserClaimsInIdToken=true,
                    //offline_access，其实指的是能否用refreshtoken重新申请令牌
                    AllowOfflineAccess=true,
                    AllowedScopes =
                    {
                        "api1",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "Mobile"
                    }
                }
            };
    }
}
