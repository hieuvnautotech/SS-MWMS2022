using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class GetPickingScanPPCountMtNoResponse
    {
        public string mt_no { get; set; }
        public int cap { get; set; }
        public int nhan { get; set; }
        public int dasd { get; set; }
        public int dangsd { get; set; }
        public int trave { get; set; }
    }
}