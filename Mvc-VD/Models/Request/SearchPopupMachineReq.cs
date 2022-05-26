using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Request
{
    public class SearchPopupMachineReq: Pageing
    {
        public string mc_type { get; set; } 
        public string mc_no { get; set; } 
        public string mc_nm { get; set; }
    }
}