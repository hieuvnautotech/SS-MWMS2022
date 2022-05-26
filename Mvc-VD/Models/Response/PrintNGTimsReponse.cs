using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class PrintNGTimsReponse
    {
        public int Id { get; set; }
        public double Qty { get; set; }
        public string WMaterialCode { get; set; }
        public string at_no { get; set; }
        public string product { get; set; }
        public double SLCK { get; set; }
        public string buyer_qr { get; set; }
        public DateTime reg_dt { get; set; }
    }
}