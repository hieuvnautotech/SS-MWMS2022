using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class ModelReturnMachineExcelResponse
    {
        public string mno { get; set; }
        public string mc_type { get; set; }
        public string mc_no { get; set; }
        public string mc_nm { get; set; }
        public string purpose { get; set; }
        public string re_mark { get; set; }
        public string reg_id { get; set; }
        public string chg_id { get; set; }
        public string reg_dt { get; set; }

        public string chg_dt { get; set; }
        public string STATUS { get; set; }
    }
}