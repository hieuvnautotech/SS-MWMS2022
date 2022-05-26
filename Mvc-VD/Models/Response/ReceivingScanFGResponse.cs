using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
   
    public class ReceivingScanFGResponse
    {
        public int wmtid { get; set; }

        public string po { get; set; }

        public string product { get; set; }

        public string product_nm { get; set; }

        public string mt_no { get; set; }

        public string material_type { get; set; }

        public string gr_qty { get; set; }

        public string real_qty { get; set; }

        public DateTime expiry_dt { get; set; }

        public DateTime receipt_date { get; set; }

        public DateTime expore_dt { get; set; }

        public string status { get; set; }

        public string sts_nm { get; set; }

        public string material_code { get; set; }

        public string bb_no { get; set; }

        public string buyer_qr { get; set; }

        public string from_lct_code { get; set; }

        public string from_lct_nm { get; set; }

        public string mt_type_nm { get; set; }

        public string lct_sts_cd { get; set; }


        //Ngoai Bien
        public string lot_date { get; set; }
    }
}