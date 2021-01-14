using IdentityServer4.Models;
using IdentityServer4.Validation;
using IdsServer.EnumList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdsServer.ValidateExtension
{
    public class WechatLoginValidator : IExtensionGrantValidator
    {
        public string GrantType => GrantTypeExtend.WeChat;

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var Code = context.Request.Raw.Get("OpenId");
            var Phone = context.Request.Raw.Get("UnionId");

            if ("aa" == "bb")
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "密码错误");
            }
            else
            {
                //如果验证成功
                context.Result = new GrantValidationResult(subject: "12345", authenticationMethod: "PhoneCode");
            }
        }
    }
}
