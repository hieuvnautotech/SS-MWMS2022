using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Request
{
    public class DataWActualPrimaryReq:Pageing
    {
        public string product { get; set; }
        public string product_name { get; set; }
        public string model { get; set; }
        public string at_no { get; set; }
        public string regstart { get; set; }
        public string regend { get; set; }
        public string type { get; set; }
        public string bom_type { get; set; }
    }
}