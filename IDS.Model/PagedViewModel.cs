using System;
using System.Collections.Generic;
using System.Text;

namespace IDS.Model
{
    public class PagedViewModel
    {
        public dynamic Data { get; set; }

        public int totalElements { get; set; }
    }
}
