using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.DTO
{
    public class ModelInsertTemGeneral
    {
        public string Code { get; set; }
        public string Model { get; set; }
        public string BuyerCode { get; set; }
        public string Quantity { get; set; }
        public string LotNo { get; set; }
    }
}