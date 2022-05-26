using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Request
{
    public class ModifyProcessMachineUnit
    {
       public string mc_no { get; set; }
        public int pmid { get; set; }
        public int id_actual { get; set; }
        public string use_yn { get; set; }
        public DateTime end { get; set; }
        public DateTime start { get; set; }
          
    }
}