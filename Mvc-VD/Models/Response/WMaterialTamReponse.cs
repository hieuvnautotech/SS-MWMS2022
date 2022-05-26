using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class WMaterialTamReponse
    {
        public int wmtid { get; set; }
        public string mt_cd { get; set; }
        public string mt_no { get; set; }
        public string gr_qty { get; set; }
        public int seq { get; set; }
        public string dt_of_receipt { get; set; }
        public string expiry_dt { get; set; }
        public string expore_dt { get; set; }
        public string lot_no { get; set; }
    }
}