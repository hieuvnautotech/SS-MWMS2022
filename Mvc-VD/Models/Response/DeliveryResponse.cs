using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class DeliveryResponse
    {
        public int dlid { get; set; }
        public string dl_no { get; set; }
        public string dl_no1 { get; set; }
        public string dl_nm { get; set; }
        public string Status { get; set; }
        public string work_dt { get; set; }
        public string lct_cd { get; set; }
        public string remark { get; set; }
        public string mt_no { get; set; }
        public int quantity { get; set; }
        public string productCode { get; set; }
    }
}