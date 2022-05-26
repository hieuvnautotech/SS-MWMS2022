using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Request
{
    public class CreateProcessUnitStaffReq
    {
        public string staff_tp { get; set; }
        public string staff_id { get; set; }
        public string use_yn { get; set; }
    }
}