using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.WOModel
{
    public class DMachineInfoAPI
    {
        public int mno { get; set; }
        public string mc_type { get; set; }
        public string mc_no { get; set; }
        public string mc_nm { get; set; }
        public string purpose { get; set; }
        public string color { get; set; }
        public string barcode { get; set; }
        public string remark { get; set; }
        public string use_yn { get; set; }
        public string del_yn { get; set; }
        public string reg_id { get; set; }
        public string reg_dt { get; set; }
        public string chg_id { get; set; }
        public string chg_dt { get; set; }
        public string uses { get; set; }
    }
}