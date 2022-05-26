using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class MaterialToDeviceResponse
    {
        public int wmtid { get; set; }
        public string mt_cd { get; set; }
        public string mt_no { get; set; }
        public int gr_qty { get; set; }
        public int gr_qty1 { get; set; }

        public int real_qty { get; set; }
        public string bbmp_sts_cd { get; set; }
        public string lct_cd { get; set; }
        public string bb_no { get; set; }
        public DateTime chg_dt { get; set; }
        public int count_table2 { get; set; }
        public string sl_tru_ng { get; set; }
    }
}