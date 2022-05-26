using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class ProductActivitionFailedDetailVm
    {
        public string CheckCode { get; set; }
        public string CheckName { get; set; }
        public string CheckDate { get; set; }
        public int CDQty { get; set; }
        public int CNQty { get; set; }
    }
}