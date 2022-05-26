using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.WIP
{
    public class ExcelInventoryWIPComposite
    {
        public string model { get; set; }
        public string product_cd { get; set; }
        public string product_name { get; set; }
        public string at_no { get; set; }
        public string mt_cd { get; set; }
        public string bb_no { get; set; }
        public string quantity { get; set; }
        public string BTP { get; set; }
        public string TP { get; set; }
        public string Total { get; set; }
        public string create_date { get; set; }
    }
}