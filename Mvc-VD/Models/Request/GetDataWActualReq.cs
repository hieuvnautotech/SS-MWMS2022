using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Request
{
    public class GetDataWActualReq:Pageing
    {
        public string at_no { get; set; }
    }
}