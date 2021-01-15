using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace IDS.Database.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string Location { get; set; }
        public int Gender { get; set; }
        public DateTime? BirthDate { get; set; }

        public string Mobile { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? LastSignIn { get; set; }
        public DateTime? DateModified { get; set; }
    }
    public class IdentityRole : Microsoft.AspNetCore.Identity.IdentityRole
    {

    }
    public class IdentityUserClaim : IdentityUserClaim<string>
    {

    }
    public class IdentityUserRole : IdentityUserRole<string>
    {

    }
    public class IdentityUserLogin : IdentityUserLogin<string>
    {

    }
    public class IdentityRoleClaim : IdentityRoleClaim<string>
    {

    }
    public class IdentityUserToken : IdentityUserToken<string>
    {

    }
}
