using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class MemoResponse
    {
        public string mt_cd { get; set; }

        public string width { get; set; }

        public string spec { get; set; }

        public string total_roll { get; set; }

        public DateTime chg_dt { get; set; }
    }
}