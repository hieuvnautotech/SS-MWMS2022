using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.TIMS
{
    public class WMaterialInfoTIMSAPIReceing
    {
        public int wmtid { get; set; }
        public int? gr_qty { get; set; }
        public int? real_qty { get; set; }
        public string recevice_dt_tims { get; set; }
        public string receive_date { get; set; }
        public string bb_no { get; set; }
        public string product { get; set; }
        public string at_no { get; set; }
        public string mt_cd { get; set; }
        public string shift { get; set; }
    }
}