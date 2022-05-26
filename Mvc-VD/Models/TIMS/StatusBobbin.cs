using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.TIMS
{
    public class StatusBobbin
    {
        public int? gr_qty { get; set; }
        public int? gr_qty_bf { get; set; }
        public string process { get; set; }
        public string ketluan { get; set; }
        public string mt_type { get; set; }
        public string recevice_dt_tims { get; set; }
        public string mt_sts_nm { get; set; }
        public string lct_nm { get; set; }
        public string mt_cd { get; set; }
        public string po { get; set; }
        public string product { get; set; }
        public string staff_id { get; set; }
        public string staff_nm { get; set; }
    }
}