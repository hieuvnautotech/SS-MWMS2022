using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.TIMS
{
    public class WMaterialInfoOQCAPI
    {
        public int wmtid { get; set; }
        public string bb_no { get; set; }
        public string mt_no { get; set; }
        public string mt_cd { get; set; }
        public string count_ng { get; set; }
        public  int gr_qty { get; set; }
    }
}