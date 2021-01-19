using IDS.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdsEFCore.Extension
{
    public class UserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User>
    {
        public UserClaimsPrincipalFactory(UserManager<User> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var result = await base.GenerateClaimsAsync(user);
            if (!string.IsNullOrWhiteSpace(user.Mobile))
            {
                result.AddClaim(new Claim("Mobile", user.Mobile));
            }          
            return result;
        }
    }
}
