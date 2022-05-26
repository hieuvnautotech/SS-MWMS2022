using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class ProductActivitionFailedVm
    {
        public string ModelName { get; set; }
        public string ItemName { get; set; }
        public string ProductCode { get; set; }
        public int Stt { get; set; }
        public int Total { get; set; }
        public int OK { get; set; }
        public int NG { get; set; }
        public string CreateOn { get; set; }
        public string Shift { get; set; }
    }
}