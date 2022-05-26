using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.TIMS
{
    public class DProUnitStaffAPIRespond
    {
        public int psid { get; set; }
        public string staff_id { get; set; }
        public double? ActualQty { get; set; }
        public string use_yn { get; set; }
        public string staff_tp { get; set; }
        public string uname { get; set; }
        public double? Defective { get; set; }
        public double? actual { get; set; }
        public DateTime start_dt { get; set; }
        public DateTime end_dt { get; set; }
        public string het_ca { get; set; }
        public Nullable<double> defect { get; set; }
        public int id_actual { get; set; }
        public string del_yn { get; set; }
        public string reg_id { get; set; }
        public string reg_dt { get; set; }
        public string chg_id { get; set; }
        public string chg_dt { get; set; }
        public string at_no { get; set; }
        public int? actual_cn { get; set; }
        public int? actual_cd { get; set; }

    }
}