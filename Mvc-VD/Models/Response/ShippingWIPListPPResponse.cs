using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class ShippingWIPListPPResponse
    {
        public int wmtid { get; set; }

        public string mt_cd { get; set; }

        public string bb_no { get; set; }

        public int gr_qty { get; set; }

        public string from_lct_cd { get; set; }

        public string from_lct_nm { get; set; }

        public string lct_sts_cd { get; set; }

        public string mt_type_nm { get; set; }

        public string mt_type{ get; set; }

        public string status { get; set; }

        public string sts_nm { get; set; }

        public string recevice_dt_tims { get; set; }
    }
}