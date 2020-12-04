using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdsServer.Ids
{
    public class IdsConfig
    {
        public static IEnumerable<ApiScope> ApiScopes =>
       new ApiScope[]
       {
            new ApiScope("user_api"),
            new ApiScope("api"),
       };
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Phone()
            };
        }
        /// <summary>
        /// Api资源 静态方式定义
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
                {
                    new ApiResource("user_api","user_api"){
                        Scopes={"user_api"}
                    },
                    new ApiResource("api","api"){
                        Scopes={ "api" }
                }
            };
        }
    }
}
