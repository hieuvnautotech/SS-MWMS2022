using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.WIP
{
    public class GeneralWIP
    {
        public string wmtid { get; set; }
        public string mt_no { get; set; }
        public string product_cd { get; set; }
        public string mt_nm { get; set; }
        public string qty { get; set; }
        public  int? DSD { get; set; }
        public  int? CSD { get; set; }
        public  int? returnMachine { get; set; }

    }
}