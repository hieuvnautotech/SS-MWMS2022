using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.WOModel
{
    public class DatawActualPrimaryResponse
    {
        public int id_actualpr { get; set; }
        public int id_actualpr1 { get; set; }
        public string at_no { get; set; }
        public string po { get; set; }
        public string type { get; set; }
        public int totalTarget { get; set; }
        public int target { get; set; }
        public string product { get; set; }
        public string md_cd { get; set; }
        public string remark { get; set; }
        public string process_code { get; set; }
        public string style_nm { get; set; }
        public int process_count { get; set; }
        public int actual { get; set; }
        public int count_pr_w { get; set; }
        public int poRun { get; set; }
        //public string poRun { get; set; }
        public string IsApply { get; set; }
        public string bom_type { get; set; }
        public int CountProcess { get; set; }
    }
}