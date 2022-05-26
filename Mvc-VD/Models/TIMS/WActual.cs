using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.TIMS
{
    public class WActual
    {
        public int id_actual { get; set; }
        public string at_no { get; set; }
        public string type { get; set; }
        public string product { get; set; }
        public double? actual { get; set; }
        public double? defect { get; set; }
        public string name { get; set; }
        public int? level { get; set; }
        public string date { get; set; }
        public string don_vi_pr { get; set; }
        public string item_vcd { get; set; }
        public string reg_id { get; set; }
        public DateTime? reg_dt { get; set; }
        public string chg_id { get; set; }
        public DateTime? chg_dt { get; set; }
        //
    }
}