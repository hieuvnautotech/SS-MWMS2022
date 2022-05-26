using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.WIP
{
    public class GeneralDetailWIP
    {
        public int wmtid { get; set; }
        public string material_code { get; set; }
        public string mt_nm { get; set; }
        public string lenght { get; set; }
        public string size { get; set; }
        public string spec { get; set; }
        public string mt_no { get; set; }
        public string qty { get; set; }
        public string bundle_unit { get; set; }
        public DateTime? receipt_date { get; set; }
        public string sd_no { get; set; }
        public string sts_nm { get; set; }

        public string lct_nm { get; set; }
        public string ExportCode { get; set; }
    }
}