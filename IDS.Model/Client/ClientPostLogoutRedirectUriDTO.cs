using System;
using System.Collections.Generic;
using System.Text;

namespace IDS.Model.Client
{
    public class ClientPostLogoutRedirectUriDTO
    {
        public int Id { get; set; }
        public string PostLogoutRedirectUri { get; set; }
        public int ClientId { get; set; }
    }
}
