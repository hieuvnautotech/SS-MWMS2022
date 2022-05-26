using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.APIRequest
{
    public class GetActualPrimaryRequest:Pageing
    {
        public string Product { get; set; }
        public string ProductName { get; set; }
        public string Model { get; set; }
        public string AtNo { get; set; }
        public string RegStart { get; set; }
        public string RegEnd { get; set; }
        public string Type { get; set; }
    }
}