using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.DTO
{
    public class ListStaffTims
    {
        public int psid { get; set; }
        public string at_no { get; set; }
        public string staff_id { get; set; }
        public int ActualQty { get; set; }
        public string use_yn { get; set; }
        public string staff_tp { get; set; }
        public string uname { get; set; }
        public string Defective { get; set; }
        public string reg_dt { get; set; }
        public string chg_dt { get; set; }
        public DateTime start_dt { get; set; }
        public DateTime end_dt { get; set; }
        public string het_ca { get; set; }
    }
}