using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.TIMS
{
    public class WProductQc
    {
        public int pqno { get; set; }
        public string pq_no { get; set; }
        public string ml_no { get; set; }
        public string work_dt { get; set; }
        public string item_vcd { get; set; }
        public Nullable<int> check_qty { get; set; }
        public Nullable<int> ok_qty { get; set; }
        public string reg_id { get; set; }
        public System.DateTime reg_dt { get; set; }
        public string chg_id { get; set; }
        public System.DateTime chg_dt { get; set; }
    }
}