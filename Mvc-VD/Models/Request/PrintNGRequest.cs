using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Request
{
    public class PrintNGRequest:Pageing
    {
        public string code { get; set; }
        public string at_no { get; set; }
        public string product { get; set; }
    }
}