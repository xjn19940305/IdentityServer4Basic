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
        public static IEnumerable<ApiResource> ApiResource =>
           new ApiResource[]
           {
               new ApiResource("api1","api1",new string[]{"api1" })
           };
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Address(),
                new IdentityResource("Mobile","手机号",new string[]{ "Mobile"})
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
                    ClientName="授权码模式",
                    ClientSecrets = { new Secret("Code_Client_Demo".Sha256(),"code") },
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    //登录成功后返回的客户端地址
                    RedirectUris={"https://localhost:5001/signin-oidc"},
                    //注销前端登出后返回的客户端地址
                    FrontChannelLogoutUri="https://localhost:5001/signout-oidc",
                    //注销后端登出后返回的客户端地址
                    PostLogoutRedirectUris={"https://localhost:5001/signout-callback-oidc"},
                    //将用户所有的claims包含在IdToken内
                    AlwaysIncludeUserClaimsInIdToken=true,
                    //offline_access，其实指的是能否用refreshtoken重新申请令牌
                    AllowOfflineAccess=true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "api1",
                        "Mobile"
                    },
                    //是否需要手动授权
                    RequireConsent=true
                },
                 new Client
                {
                    ClientId = "Implicit_client",
                    ClientName="简化模式",
                    ClientSecrets = { new Secret("Implicit_client_secret".Sha256(),"implicit") },
                    AllowedGrantTypes = GrantTypes.ImplicitAndClientCredentials,
                    //登录成功后返回的客户端地址
                    RedirectUris={"https://localhost:1001/signin-oidc"},
                    //注销前端登出后返回的客户端地址
                    FrontChannelLogoutUri="https://localhost:1001/signout-oidc",
                    //注销后端登出后返回的客户端地址
                    PostLogoutRedirectUris={"https://localhost:1001/signout-callback-oidc"},
                    //将用户所有的claims包含在IdToken内
                    AlwaysIncludeUserClaimsInIdToken=true,
                    //offline_access，其实指的是能否用refreshtoken重新申请令牌
                    AllowOfflineAccess=true,
                    AllowAccessTokensViaBrowser=true,
                    AccessTokenType=AccessTokenType.Reference,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1",
                        "Mobile"
                    },
                    //是否需要手动授权
                    RequireConsent=true
                },
                new Client
                {
                    ClientId="hybrid_client",
                    ClientName="混合模式",
                    ClientSecrets={ new Secret("hybridsecret".Sha256(),"hybrid") },
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
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "api1",
                        "Mobile"
                    },
                    //不需要手动授权
                    RequireConsent=true
                },
                 new Client
                {
                    ClientId="Client_Mode",
                    ClientName="客户端授权模式",
                    ClientSecrets={ new Secret("client_mode".Sha256(),"client") },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    //offline_access，其实指的是能否用refreshtoken重新申请令牌
                    AllowOfflineAccess=true,
                    AllowedScopes =
                    {
                         "api1"
                    }
                },
                  new Client
                {
                    ClientId="Jwt_Client",
                    ClientName="密码授权方式",
                    ClientSecrets={ new Secret("Jwt_secret".Sha256(),"password") },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    RefreshTokenUsage=TokenUsage.ReUse,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowOfflineAccess = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1",
                        "Mobile"
                    }
                },
            };
    }
}
