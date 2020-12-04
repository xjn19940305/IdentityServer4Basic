using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdsServer.ValidateExtension
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var userName = context.UserName;
                var password = context.Password;

                //验证用户,这么可以到数据库里面验证用户名和密码是否正确
                if (userName != "test" || password != "123")
                    throw new Exception("账号密码错误");
                // 验证账号
                context.Result = new GrantValidationResult
                (
                    subject: userName,
                    authenticationMethod: "custom",
                    claims: new[]
                {
                   new Claim("sub","123456"),
                   new Claim("ctExt",Guid.NewGuid().ToString("N")),
                   new Claim("typ","Bearer"),

             }
                 );
            }
            catch (Exception ex)
            {
                //验证异常结果
                context.Result = new GrantValidationResult()
                {
                    IsError = true,
                    Error = ex.Message
                };
            }
        }
    }
}
