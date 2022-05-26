using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Classes
{
    public class MappedProductModel
    {
        public int Id { get; set; }
        public string MaterialCode { get; set; }
        public string Status { get; set; }
        public string MaterialNo { get; set; }
        public string TypeSystem { get; set; }
        public string ProductNo { get; set; }
        public string Quantity { get; set; }
        public string BuyerCode { get; set; }
        public int? Count { get; set; }
        public int? Total { get; set; }
        public string StampType { get; set; }
        public string lot_date { get; set; }
    }
}