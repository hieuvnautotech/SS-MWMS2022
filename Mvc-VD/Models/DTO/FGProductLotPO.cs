using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.DTO
{
    public class FGProductLotPO
    {
        public string mt_cd { get; set; }
        public string mt_no { get; set; }
        public string process { get; set; }
        public string machine { get; set; }
        public string congnhan_time { get; set; }
        public string size { get; set; }
        public string mt_nm { get; set; }
        public string expiry_dt { get; set; }
        public string dt_of_receipt { get; set; }
        public string expore_dt { get; set; }
        public string lot_no { get; set; }
        public int SLLD { get; set; }
    }
}