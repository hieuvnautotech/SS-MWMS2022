using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.WOModel
{
    public class DSMappingSTA
    {
        public int wmtid { get; set; }
       // public string date { get; set; }
        public string material_code { get; set; }
        public string mt_no { get; set; }
        public int gr_qty { get; set; }
        public int gr_qty1 { get; set; }
        public string bbmp_sts_cd { get; set; }
        //public string mt_qrcode { get; set; }
        public string lct_cd { get; set; }
        public string bb_no { get; set; }
        //public string mt_barcode { get; set; }
        public string chg_dt { get; set; }
        public int sl_tru_ng { get; set; }
       // public int real_qty { get; set; }

    }
}