using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Classes
{
    public class FGBoxMappingModel
    {
        public string BoxCode { get; set; }
        public string ProductCode { get; set; }
        public string DlNo { get; set; }
        public string TypeSystem { get; set; }
        public List<string> Wmtids { get; set; }
        public List<string> ListBoxCode { get; set; }
        public List<string> WmtidMES { get; set; }
        public List<string> WmtidsAP { get; set; }
    }
}