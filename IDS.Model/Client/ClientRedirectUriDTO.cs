using System;
using System.Collections.Generic;
using System.Text;

namespace IDS.Model.Client
{
    public class ClientRedirectUriDTO
    {
        public int Id { get; set; }
        public string RedirectUri { get; set; }
        public int ClientId { get; set; }
    }
}
