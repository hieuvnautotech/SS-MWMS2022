using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class ProductActivitionFailedResponse
    {
        public List<ProductActivitionFailedVm> ProductActivitionFaileds { get; set; }
        public List<ProductActivitionFailedDetailVm> ProductActivitionFailedDetails { get; set; }
    }
}