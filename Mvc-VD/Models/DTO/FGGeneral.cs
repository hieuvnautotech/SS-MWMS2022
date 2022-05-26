using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.DTO
{
    public class FGGeneral
    {
        public int ids { get; set; }
        public int id { get; set; }
        public string buyer_qr { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string statusName { get; set; }
        public string at_no { get; set; }
        public string type { get; set; }
        public string md_cd { get; set; }
        public string dl_no { get; set; }
        public int qty { get; set; }
        public string lot_no { get; set; }
        public string status { get; set; }
        public string use_yn { get; set; }
        public string reg_id { get; set; }
        public DateTime reg_dt { get; set; }
        public string chg_id { get; set; }
        public DateTime chg_dt { get; set; }
        public string active { get; set; }
        public string bom_type { get; set; }
    }
}