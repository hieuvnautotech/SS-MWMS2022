using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class PrintMaterialQRInventoryResponse
    {
        public int wmtid { get; set; }
        public string mt_cd { get; set; }
        public string mt_no { get; set; }
        public string lot_no { get; set; }
        public DateTime? recei_date { get; set; }
        public string expiry_dt { get; set; }
        public string expore_dt { get; set; }
        public string dt_of_receipt { get; set; }
        public string mt_nm { get; set; }
        public string bundle_unit { get; set; }
        public string lenght { get; set; }
        public string size { get; set; }
        public decimal qty { get; set; }
        public string sts_nm { get; set; }
    }
}