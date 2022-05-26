using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.WOModel
{
    public class wo_info_mc_wk_mold
    {
        public int id_actual { get; set; }
        public string code { get; set; }
        public string use_yn { get; set; }
        public int pmid { get; set; }
        public string staff_tp { get; set; }
        public DateTime start_dt { get; set; }
        public DateTime end_dt { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string het_ca { get; set; }

        //Ngoại Biến
        public string new_start_dt { get; set; }
        public string new_end_dt { get; set; }
    }
}