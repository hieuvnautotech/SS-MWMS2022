using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Request
{
    public class PolicyMtResquest
    {
        public int wid { get; set; }

        public string work_start { get; set; }

        public string work_end { get; set; }

        public decimal work_hour { get; set; }

        public string policy_name { get; set; }

        public string use_yn { get; set; }

        public string re_mark { get; set; }

        public string lunch_start { get; set; }  
        
        public string lunch_end { get; set; } 

        public string dinner_start { get; set; } 

        public string dinner_end { get; set; }
    }
}