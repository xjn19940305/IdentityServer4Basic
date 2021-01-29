using System;
using System.Collections.Generic;
using System.Text;

namespace IDS.Model.Client
{
    public class ClientGrantTypeDTO
    {
        public int Id { get; set; }
        public string GrantType { get; set; }
        public int ClientId { get; set; }
    }
}
