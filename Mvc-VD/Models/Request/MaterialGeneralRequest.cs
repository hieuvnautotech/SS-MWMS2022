using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Request
{
    public class MaterialGeneralRequest: Pageing
    {
        public string mt_no { get; set; }

        public string mt_cd { get; set; }

        public string mt_nm { get; set; }

        public string product_cd { get; set; }

        public string lct_cd { get; set; }

        public string recevice_dt_start { get; set; }

        public string recevice_dt_end { get; set; }
        
        public string sts { get; set; }
    }
}