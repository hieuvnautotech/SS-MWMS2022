using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.TIMS
{
    public class WMaterialInfoStamp
    {
        public int wmtid { get; set; }
     
        public string buyer_qr { get; set; }
        public string mt_cd { get; set; }
        public int? gr_qty { get; set; }
        public string NameStatus { get; set; }
        public string locationName { get; set; }
        public string mt_sts_cd { get; set; }
        public string lct_cd { get; set; }
        public string product { get; set; }
        public string chg_id { get; set; }
        public System.DateTime reg_dt { get; set; }
        public System.DateTime chg_dt { get; set; }

    }
}