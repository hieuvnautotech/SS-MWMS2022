using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.DTO
{
    public class spFGWMMSGetFGGeneral
    {
        public int id_actualpr { get; set; }
        public int qty { get; set; }
        public string product { get; set; }
        public string bundle_unit { get; set; }
        public string product_name { get; set; }
        public string model { get; set; }
        public string sts_nm { get; set; }
    }
}