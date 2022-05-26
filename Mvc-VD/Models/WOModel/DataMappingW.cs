using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.WOModel
{
    public class DataMappingW
    {
        public int wmmid { get; set; }
        public string mt_lot { get; set; }
        public string mt_cd { get; set; }
        public string mt_type { get; set; }
        public string mapping_dt { get; set; }
        public string use_yn { get; set; }
        public string reg_dt { get; set; }
        public int? gr_qty { get; set; }
        public string mt_no { get; set; }
        public string bb_no { get; set; }
        public int? Used { get; set; }
        public int? Remain { get; set; }
    }
}