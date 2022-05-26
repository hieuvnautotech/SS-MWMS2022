using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.TIMS
{
    public class NGPO
    {
        public int wmtid { get; set; }
        public string mt_cd { get; set; }
        public string product { get; set; }
        public string mt_no { get; set; }
        public Nullable<double> gr_qty { get; set; }
        public string at_no { get; set; }
    }
}