using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class FGReceiveDataResponse
    {
        public int wmtid { get; set; }
        public string ProductNo { get; set; }
        public string ProductName { get; set; }
        public string Model { get; set; }
        public string BuyerCode { get; set; }
        public int Quantity { get; set; }
        public string TypeSystem { get; set; }
        public string bb_no { get; set; }
    }
}