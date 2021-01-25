using System;
using System.Collections.Generic;
using System.Text;

namespace IDS.Model
{
    public class PagingModel
    {
        public string key { get; set; }
        public int pageSize { get; set; }
        public int page { get; set; }
    }
}
