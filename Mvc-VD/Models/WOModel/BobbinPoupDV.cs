using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.WOModel
{
    public class BobbinPoupDV
    {
        public int bno { get; set; }
        public string mc_type { get; set; }
        public string bb_no { get; set; }
        public string mt_cd { get; set; }
        public string bb_nm { get; set; }
        public string use_yn { get; set; }
        public string purpose { get; set; }
        public string barcode { get; set; }
        public string re_mark { get; set; }
        public string count_number { get; set; }
        public string del_yn { get; set; }
        public string reg_id { get; set; }
        public string reg_dt { get; set; }
        public string chg_id { get; set; }
        public string chg_dt { get; set; }
    }
}