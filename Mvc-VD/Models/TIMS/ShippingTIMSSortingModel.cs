using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.TIMS
{
    public class ShippingTIMSSortingModel
    {
        public int id { get; set; }
        public string ShippingCode { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string MachineCode { get; set; }
        public string LocationCode { get; set; }
        public string IsFinish { get; set; }
        public string Description { get; set; }
        public string CreateId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string ChangeId { get; set; }
        public System.DateTime ChangeDate { get; set; }
    }
}