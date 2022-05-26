using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Request
{
    public class ModifyProcessUnitStaffReq
    {
        public int psid { get; set; }
        public string staff_id { get; set; }
        public string use_yn { get; set; }
        public DateTime end { get; set; }
        public DateTime start { get; set; }

    }
}