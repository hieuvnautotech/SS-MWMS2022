using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.TIMS
{
    public class DBobbinLctHist
    {
        public int blno { get; set; }
        public string mc_type { get; set; }
        public string bb_no { get; set; }
        public string mt_cd { get; set; }
        public string bb_nm { get; set; }
        public string start_dt { get; set; }
        public string end_dt { get; set; }
        public string use_yn { get; set; }
        public string del_yn { get; set; }
        public string reg_id { get; set; }
        public System.DateTime reg_dt { get; set; }
        public string chg_id { get; set; }
        public System.DateTime chg_dt { get; set; }
    }
}