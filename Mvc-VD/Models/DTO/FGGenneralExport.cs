using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.DTO
{
    public class FGGenneralExport
    {

        public string product_code { get; set; }
        public string product_name { get; set; }
        public string md_cd { get; set; }
        public string buyer_qr { get; set; }
        public string at_no { get; set; }
        public string lot_no { get; set; }
        public int qty { get; set; }
        public DateTime reg_dt { get; set; }
        public string bom_type { get; set; }
        public string statusName { get; set; }

    }
}