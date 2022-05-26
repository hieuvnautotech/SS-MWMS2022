using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.WOModel
{
    public class ProcessMachineunit
    {
        public int pmid { get; set; }
        public int id_actual { get; set; }
        public DateTime start_dt { get; set; }
        public DateTime end_dt { get; set; }
        public string remark { get; set; }
        public string mc_no { get; set; }
        public string use_yn { get; set; }
    }
}