using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.TIMS
{
    public class WActualPrimary
    {
        public int id_actualpr { get; set; }
        public string at_no { get; set; }
        public string type { get; set; }
        //public int target { get; set; }
        public int target { get; set; }
        public string product { get; set; }
        public string remark { get; set; }
        public string finish_yn { get; set; }
        public string reg_id { get; set; }
        public Nullable<System.DateTime> reg_dt { get; set; }
        public string chg_id { get; set; }
        public Nullable<System.DateTime> chg_dt { get; set; }
        //
        public string name { get; set; }
        public string date { get; set; }
        public string name_view { get; set; }
        public string qccode { get; set; }
        public string qcname { get; set; }
        public double defective { get; set; }
        public string rollcode { get; set; }
        public string rollname { get; set; }
        public double  actual {get;set;}
        public int totalTarget { get; set; }
        public int process_count { get; set; }
    
        public string poRun { get; set; }
        public string md_cd { get; set; }
        public string style_nm { get; set; }
    }
}