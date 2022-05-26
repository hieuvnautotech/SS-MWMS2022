using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.APIRequest
{
    public class MaterialRequestApi
    {
        public string type { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string sp { get; set; }
    }
}