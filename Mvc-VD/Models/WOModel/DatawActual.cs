using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.WOModel
{
    public class DatawActual
    {
        public int id_actual { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string date { get; set; }
        public string item_vcd { get; set; }
        public string QCName { get; set; }
        public string defect { get; set; }
        public string actual { get; set; }
        public string actual_cn { get; set; }
        public string actual_cd { get; set; }
        public string RollName { get; set; }
        public string reg_id { get; set; }
        public string reg_dt { get; set; }
        public string chg_id { get; set; }
        public string chg_dt { get; set; }
        public int target { get; set; }
        public int level { get; set; }
        public string mc_no { get; set; }
        public string description { get; set; }
    }
}