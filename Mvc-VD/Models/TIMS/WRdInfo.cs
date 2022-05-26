using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.TIMS
{
    public class WRdInfo
    {
        public int rid { get; set; }
        public string rd_no { get; set; }
        public string rd_nm { get; set; }
        public string rd_sts_cd { get; set; }
        public string lct_cd { get; set; }
        public string receiving_dt { get; set; }
        public string remark { get; set; }
        public string use_yn { get; set; }
        public string reg_id { get; set; }
        public System.DateTime reg_dt { get; set; }
        public string chg_id { get; set; }
        public System.DateTime chg_dt { get; set; }
    }
}