using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.DTO
{
    public class wipgeneralexportexcel
    {
        public string ProductCode { get; set; }

        public string CODE { get; set; }
        public string CompositeCode { get; set; }
        public string NAME { get; set; }
        public string QTY { get; set; }
        public string stk_qty { get; set; }

        public string LENGTH { get; set; }
        public string SIZE { get; set; }
        public string STATUS { get; set; }
        public string ReceviceDate { get; set; }

        //public string qty { get; set; }
        //public string return_date { get; set; }
        public string MT_NO { get; set; }
    }
}