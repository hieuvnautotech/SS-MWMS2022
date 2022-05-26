using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class PrintQRMaterialResponse
    {
        public string material_code { get; set; }
        public string mt_no { get; set; }
        public int gr_qty { get; set; }
        public string export_date { get; set; }
        public string expiry_date { get; set; }
        public string date_of_receipt { get; set; }
        public string lot_no { get; set; }
        public int send_qty { get; set; }
        public int bundle_qty { get; set; }
        public string bundle_unit { get; set; }
        public string mt_type { get; set; }
        public double width { get; set; }
        public int spec { get; set; }
        public string width_unit { get; set; }
        public string spec_unit { get; set; }
        public string mt_nm { get; set; }

    }
}