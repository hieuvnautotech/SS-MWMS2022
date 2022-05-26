using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.WOModel
{
    public class ProcessUnitStaff
    {
        public int psid { get; set; } 
        public string staff_id { get; set; } 
        public string staff_tp { get; set; } 
        public string staff_tp_nm { get; set; } 
        public string uname { get; set; } 
        public DateTime start_dt { get; set; } 
        public DateTime end_dt { get; set; }
        public string del_yn { get; set; }
        public string use_yn { get; set; } 
    }
}