using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.WOModel
{
    public class WActualBom
    {
        public string stt { get; set; }
        public string at_no { get; set; }
        public string ProductCode { get; set; }
        public string process_code { get; set; }
        public string MaterialNo { get; set; }
        public string MaterialName { get; set; }
        public string shift_dt { get; set; }
        public string shift_name { get; set; }
        public float? Actulalythuyet { get; set; }
        public float? Actual { get; set; }
        public float? lieuthaythe { get; set; }
        public float? mLieu { get; set; }
        public float? need_m { get; set; }
        public int? SoCuonNVL { get; set; }//so cuon NVL đã sử dụng

    }
}