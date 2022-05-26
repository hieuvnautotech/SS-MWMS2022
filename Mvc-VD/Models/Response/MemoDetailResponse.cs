using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class MemoDetailResponse
    {
        public int id { get; set; }

        public string md_cd { get; set; }

        public string style_no { get; set; }

        public string style_nm { get; set; }

        public string width { get; set; }

        public string width_unit { get; set; }

        public string spec { get; set; }

        public string spec_unit { get; set; }

        public int TX { get; set; }

        public DateTime receiving_dt { get; set; }
    }
}