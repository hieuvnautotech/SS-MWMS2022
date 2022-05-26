using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Request
{
    public class StyleManagementRequest
    {
        public int sid { get; set; }

        public string style_no { get; set; }

        public string style_nm { get; set; }

        public string md_cd { get; set; }

        public string prj_nm { get; set; }

        public string ssver { get; set; }

        public string part_nm { get; set; }

        public string standard { get; set; }

        public string cust_rev { get; set; }

        public string order_num { get; set; }

        public int pack_amt { get; set; }

        public string cav { get; set; }

        public string bom_type { get; set; }

        public string tds_no { get; set; }

        public string item_vcd { get; set; }

        public string qc_range_cd { get; set; }

        public string stamp_code { get; set; }

        public string expiry_month { get; set; }

        public string expiry { get; set; }
    }
}