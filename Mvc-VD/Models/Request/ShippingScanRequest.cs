using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Request
{
    public class ShippingScanRequest : Pageing
    {
        public string dl_no { get; set; }
        public string dl_nm { get; set; }
        public string productCode { get; set; }
        public string start { get; set; }
        public string end { get; set; }
    }
}