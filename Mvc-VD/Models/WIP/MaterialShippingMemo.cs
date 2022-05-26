using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.WIP
{
    public class MaterialShippingMemo
    {
        public string mt_no { get; set; }
        public string product { get; set; }
        public string lot_no { get; set; }
        public string width { get; set; }
        public string spec { get; set; }
        public string total_roll { get; set; }
        public string total_m { get; set; }
        public string total_m2 { get; set; }
        public string total_ea { get; set; }
        public DateTime? reg_date { get; set; }
        public string reg_date_convert { get { return this.reg_date?.ToString("yyyy-MM-dd"); } }
    }
}