using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Request
{
    public class FGRecevingScanRequest
    {
        public string id { get; set; }
        public string StampCode { get; set; }
        public string ProductCode { get; set; }
        public string ModelCode { get; set; }
        public string LotDate { get; set; }
        public string Status { get; set; }
        public string TypeSystem { get; set; }
        public List<string> WmtidMES { get; set; }
        public List<string> WmtidsAP { get; set; }
    }
}