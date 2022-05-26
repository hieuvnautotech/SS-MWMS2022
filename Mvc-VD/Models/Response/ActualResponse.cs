using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class ActualResponse
    {
        public int id_actual { get; set; }

        public string type { get; set; }

        public string name { get; set; }

       public DateTime? date { get; set; }
       // public string date { get; set; }

        public string item_vcd { get; set; }

        public int ProcessRun { get; set; }

        public string QCName { get; set; }

        public double defect { get; set; }

        public double actual { get; set; }

        public double actual_cn { get; set; }

        public double actual_cd { get; set; }

        public string RollName { get; set; }

        public string reg_id { get; set; }

        //public DateTime? reg_dt { get; set; }
        public string reg_dt { get; set; }

        public string chg_id { get; set; }

        public DateTime? chg_dt { get; set; }

        public double target { get; set; }

        public int level { get; set; }

        public string mc_no { get; set; }

        public string description { get; set; }

    }
}