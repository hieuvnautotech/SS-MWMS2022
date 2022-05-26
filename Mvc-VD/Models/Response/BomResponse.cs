using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class BomResponse
    {
        public int bid { get; set; }
        public string bom_no { get; set; }
        public string style_no { get; set; }
        public string mt_no { get; set; }
        public string mt_nm { get; set; }
        public float? need_time { get; set; }
        public int? cav { get; set; }
        public float? need_m { get; set; }
        public float? buocdap { get; set; }
        public DateTime reg_dt { get; set; }
        public string md_cd { get; set; }
        public string style_nm { get; set; }
        public bool IsActive { get; set; }
        public string IsApply { get; set; }
    }
}