using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Request
{
    public class BobbinRequest : Pageing
    {
        public string bb_no { get; set; }
        public string bb_nm { get; set; } 
        public string mt_cd { get; set; }
        public int id_actual { get; set; }
    }
}