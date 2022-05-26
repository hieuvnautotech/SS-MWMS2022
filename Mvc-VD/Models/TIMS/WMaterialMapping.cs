using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.TIMS
{
    public class WMaterialMapping
    {
        public int    wmmid  { get; set; }
        public string mt_lot { get; set; }
        public string mt_cd  { get; set; }
        public string mt_no  { get; set; }
        public string mapping_dt { get; set; }
        public string bb_no      { get; set; }
        public string remark     { get; set; }
        public string sts_share  { get; set; }
        public string use_yn     { get; set; }
        public string del_yn     { get; set; }
        public string reg_id     { get; set; }
        public DateTime reg_dt   { get; set; }
        public string   chg_id     { get; set; }
        public DateTime chg_dt   { get; set; }
        public int? Used { get; set; }
        public int? Remain { get; set; }
        public int? gr_qty { get; set; }
    }
}