using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Request
{
    public class ReceiveScanFGWMSRequest :Pageing
    {
        public string product { get; set; } 
        public string buyer { get; set; } 
        public string po { get; set; }
        public string lot_date { get; set; }
        public string lot_date_end { get; set; }
    }
}