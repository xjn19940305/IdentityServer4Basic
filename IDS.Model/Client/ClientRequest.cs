using System;
using System.Collections.Generic;
using System.Text;

namespace IDS.Model.Client
{
    public class ClientRequest : PagingModel
    {
        public string clientId { get; set; }

        public string clientName { get; set; }
    }
}
