using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Classes
{
    public class FGShippingModel
    {
        public int wmtid { get; set; }
        public string mt_no { get; set; }
        public string mt_type { get; set; }
        public string lot_no { get; set; }
        public string expiry_dt { get; set; }
        public string dt_of_receipt { get; set; }
        public string expore_dt { get; set; }
        public string sd_sts_cd { get; set; }
        public string sts_nm { get; set; }
        public string mt_cd { get; set; }
        public string bb_no { get; set; }
        public string buyer_qr { get; set; }
        public int? gr_qty { get; set; }
        public string recevice_dt_tims { get; set; }
        public string from_lct_cd { get; set; }
        public string from_lct_nm { get; set; }
        public string lct_cd { get; set; }
        public string lct_nm { get; set; }
        public string to_lct_cd { get; set; }
        public string to_lct_nm { get; set; }
        public string lct_sts_cd { get; set; }
        public string mt_type_nm { get; set; }
        public string mt_sts_cd { get; set; }
    }
}