using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.DTO
{
    public class FaclineQc
    {
        public int fqno { get; set; }
        public string fq_no { get; set; }
        public string work_dt { get; set; }
        public int check_qty { get; set; }
        public int ok_qty { get; set; }
        public int ng_qty { get; set; }
        public int remain_qty { get; set; }
    }
}