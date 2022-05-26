using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.APIRequest
{
    public class GetDetailContainerComposite:Pageing
    {
        public string bb_no { get; set; }
        public string mt_no { get; set; }
        public string mt_cd { get; set; }
    }
}