using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.TIMS
{
    public class truyxuatlot
    {
        public string order_lv { get; set; }
        public string mapping_dt { get; set; }
        public int id { get; set; }
        public string buyer_qr { get; set; }
        public string cha { get; set; }
        public string mt_lot { get; set; }
        public string mt_cd { get; set; }
        public string type { get; set; }
        public string bb_no { get; set; }
        public string process { get; set; }
        public string process_cd { get; set; }
        public string congnhan_time { get; set; }
        public string machine { get; set; }
        public int SLLD { get; set; }
        public string mt_no { get; set; }
        public string size { get; set; }
        public string mt_nm { get; set; }
        public string date { get; set; }
        public string expiry_dt { get; set; }
        public string dt_of_receipt { get; set; }
        public string expore_dt { get; set; }
        public string lot_no { get; set; }
        public string mt_type { get; set; }
        public DateTime? reg_dt { get; set; }
    }
}