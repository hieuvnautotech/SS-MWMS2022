using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD
{
    public class Pageing
    {
     
        public string sidx { get; set; }
        public string sord { get; set; }
        public int page { get; set; }
        public int rows { get; set; }
        public bool _search { get; set; }


    }

}