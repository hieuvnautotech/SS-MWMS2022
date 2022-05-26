using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Request
{
    public class CreateProcessMachineUnitReq
    {
        public string mc_no { get; set; }
        public int id_actual { get; set; }
        public string use_yn { get; set; }
        public string remark { get; set; }
    }
}