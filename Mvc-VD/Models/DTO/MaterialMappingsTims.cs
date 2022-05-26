using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.DTO
{
    public class MaterialMappingsTims
    {
        public int wmmid { get; set; }
        public string mt_lot { get; set; }
        public string mt_cd { get; set; }
        public string sts_share { get; set; }
        public DateTime mapping_dt { get; set; }
        public string use_yn { get; set; }
        public DateTime reg_dt { get; set; }
        public int gr_qty { get; set; }
        public int real_qty { get; set; }
        public string mt_no { get; set; }
        public string bb_no { get; set; }
        public string Used { get; set; }
        public int Remain { get; set; }
    }
}