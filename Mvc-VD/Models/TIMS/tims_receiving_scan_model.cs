using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models
{
    public class tims_receiving_scan_model
    {
        public int wmtid { get; set; }
        public int id_actual { get; set; }
        public string mt_cd { get; set; }
        public string bb_no { get; set; }
        public int gr_qty { get; set; }
        public string from_lct_cd { get; set; }
        public string lct_sts_cd { get; set; }
        public string mt_sts_cd { get; set; }
        public DateTime input_dt { get; set; }
        public string at_no { get; set; }
        public string product { get; set; }
        public string sts_nm { get; set; }
        public string mt_type_nm { get; set; }
        public string from_lct_nm { get; set; }
    }
}