using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.HomeModel
{
    public class VersionApp
    {
        public int id_app { get; set; }
        public string type { get; set; }
        public string name_file { get; set; }
        public int version { get; set; }
        public string chg_dt { get; set; }
    }
}