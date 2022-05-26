using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class ActualPrimaryModify
    {
       
        public int id_actualpr { get; set; }

        public int target { get; set; }

        public string product { get; set; }
        public string process_code { get; set; }

        public string remark { get; set; }
        public string at_no { get; set; }
        public string chg_id { get; set; }

        

    }
}