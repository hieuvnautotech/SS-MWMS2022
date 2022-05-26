using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class RoutingInfoResponse
    {
        public int idr { get; set; }
        public string style_no { get; set; }
        public string name { get; set; }
        public int? level { get; set; }
        public string don_vi_pr { get; set; }
        public string type { get; set; }
        public string item_vcd { get; set; }
        public string description { get; set; }
        public string IsFinish { get; set; }
        public string reg_id { get; set; }
        public DateTime reg_dt { get; set; }
        public string chg_id { get; set; }
        public DateTime? chg_dt { get; set; }
        public string name_pr { get; set; }
        public string don_vi_prnm { get; set; }
    }
}