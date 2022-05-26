using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class GetInventoryGeneralDetailResponse
    {
        public string at_no { get; set; }
        public int Id { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialNo { get; set; }
        public string Length { get; set; }
        public string Size { get; set; }
        public float Qty { get; set; }
        public string Unit { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string ReceivedDates { get; set; }
        public string VBobbinCd { get; set; }
        public string product_cd { get; set; }
        public string buyer_qr { get; set; }
        

    }
}