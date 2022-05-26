using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.WOModel
{
    public class w_material_info_wo
    {
        public int wmtid { get; set; }
        public DateTime? reg_dt { get; set; }
        public string mt_cd { get; set; }
        public string mt_no { get; set; }
        public string bbmp_sts_cd { get; set; }
        public int gr_qty { get; set; }
        public string mt_qrcode { get; set; }
        public string lct_cd { get; set; }
        public string bb_no { get; set; }
        public string mt_barcode { get; set; }
        public int count_table2 { get; set; }
    }
}