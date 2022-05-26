using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.DTO
{
    public class MaterialInfoTimsDTO
    {
        public int wmtid { get; set; }
        public int id_actual { get; set; }
        public string mt_cd { get; set; }
        public string bb_no { get; set; }
        public int gr_qty { get; set; }
        public string mt_sts_cd { get; set; }
        public string mt_no { get; set; }
        public string mc_type { get; set; }
    }
}