using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Classes
{
    public class ActualPrimaryModel
    {
        public string id_actualpr { get; set; }
        public string at_no { get; set; }
        public string type { get; set; }
        public string target { get; set; }
        public string product { get; set; }
        public string process_code { get; set; }
        public string remark { get; set; }
        public string reg_id { get; set; }
        public DateTime? reg_dt { get; set; }
        public string chg_id { get; set; }
        public DateTime? chg_dt { get; set; }
        public string style_nm { get; set; }
        public string process_count { get; set; }
        public string process_percent { get; set; }
    }
}