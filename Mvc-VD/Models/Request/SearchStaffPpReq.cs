using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Request
{
    public class SearchStaffPpReq: Pageing
    {
 
        public string position_cd { get; set; }
        public string uname { get; set; }
        public string userid { get; set; }

    }
}