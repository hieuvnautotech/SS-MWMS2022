using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class CompositeMaterialDetailResponse
    {
        public int wmtid { get; set; }
        public string at_no { get; set; }
        public string mt_cd { get; set; }
        public string mt_nm { get; set; }
        public string mt_no { get; set; }
        public string bb_no { get; set; }
        public DateTime reg_date { get; set; }
        public string lenght { get; set; }
        public string size { get; set; }
        public string spec { get; set; }
        public string qty { get; set; }
        public string sts_nm { get; set; }
    }
}