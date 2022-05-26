using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.WOModel
{
    public class MtDateWebWO
    {
        public int id_actual { get; set; }
        public string staff_id { get; set; }
        public DateTime reg_dt { get; set; }
        public int wmtid { get; set; }
        public DateTime DATE { get; set; }
        public string mt_cd { get; set; }
        public string mt_no { get; set; }
        public int gr_qty { get; set; }
        public string bbmp_sts_cd { get; set; }
        public string mt_qrcode { get; set; }
        public string bb_no { get; set; }
        public string chg_dt { get; set; }
        public string description { get; set; }
        public int sl_tru_ng { get; set; }
        public int count_table2 { get; set; }
        public int het_ca { get; set; }

        public string QC { get; set; }
    }
}