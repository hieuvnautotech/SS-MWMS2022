using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.WIP
{
    public class SdInfoModel
    {
        public int sid { get; set; }
        public string sd_no { get; set; }
        public string sd_nm { get; set; }
        public string product_cd { get; set; }
        public string sts_nm { get; set; }
        public string sd_sts_cd { get; set; }
        public string lct_nm { get; set; }
        public string lct_cd { get; set; }
        public int? alert { get; set; }
        public string remark { get; set; }
       
      
    }
}
