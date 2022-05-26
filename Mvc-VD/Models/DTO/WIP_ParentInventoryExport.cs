using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.DTO
{
    public class WIP_ParentInventoryExport
    {
        public string wmtid { get; set; }
        public string mt_no { get; set; }
        public string mt_nm { get; set; }
        public string sd_no { get; set; }

        public string product_cd { get; set; }

        public string mt_cd { get; set; }
        public string DSD { get; set; }
        public string CSD { get; set; }
        public string size { get; set; }
        public string spec { get; set; }
        public string qty { get; set; }
        public string receipt_date { get; set; }
        public string sts_nm { get; set; }
        public string lct_nm { get; set; }
    }
}