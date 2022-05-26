using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    public class SdInfos
    {
        public int sid { get; set; }
        public string sd_no { get; set; }
        public string sd_nm { get; set; }
        public string sd_sts_cd { get; set; }
        public string status { get; set; }
        public string product_cd { get; set; }
        public string lct_cd { get; set; }
        public string alert { get; set; }
        public string remark { get; set; }
        public string yse_yn { get; set; }
        public string del_yn { get; set; }
        public string reg_id { get; set; }
        public DateTime reg_dt { get; set; }
        public string chg_id { get; set; }
        public DateTime chg_dt { get; set; }
        public bool active { get; set; }
        public string sts_nm { get; set; }
        public string lct_nm { get; set; }
    }
}