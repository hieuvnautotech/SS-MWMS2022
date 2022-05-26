using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.DMS
{
    public class ProductProcess
    {
        public int id { get; set; }
        public string style_no { get; set; }
        public int process_code { get; set; }
        public string process_name { get; set; }
        public string description { get; set; }
        public string IsApply { get; set; }
        public string reg_id { get; set; }
        public System.DateTime reg_dt { get; set; }
        public string chg_id { get; set; }
        public System.DateTime chg_dt { get; set; }
    }
}