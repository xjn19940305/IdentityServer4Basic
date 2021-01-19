using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdsEFCore.Model
{
    public class UserDTO
    {
        public string Account { get; set; }

        public string Password { get; set; }
    }

    public class UserRegisterDTO : UserDTO
    {
        public string NickName { get; set; }

        public string Mobile { get; set; }
    }
}
