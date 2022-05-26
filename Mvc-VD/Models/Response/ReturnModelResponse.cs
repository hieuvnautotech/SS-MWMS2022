using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class ReturnModelResponse
    {
        public int wmtid { get; set; }

        public string material_code { get; set; }

        public int gr_qty { get; set; }

        public string mt_no { get; set; }

        public string bundle_unit { get; set; }

        public string length { get; set; }

        public string length1 { get; set; }

        public string size { get; set; }

        public string spec { get; set; }

        public string qty { get; set; }

        public string sts_nm { get; set; }

        public string machine { get; set; }

        public DateTime return_date { get; set; }

    }
}