using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    public class TmpWMaterialInfo
    {
        public int tmpid { get; set; }
        public string mt_no { get; set; }
        public double gr_qty { get; set; }
        public double real_qty { get; set; }
        public string mt_type { get; set; }
        public DateTime expiry_date { get; set; }
        public DateTime date_of_receipt { get; set; }
        public DateTime export_date { get; set; }
        public string lot_no { get; set; }
        public string status { get; set; }
        public string reg_id { get; set; }
        public DateTime reg_date { get; set; }
        public string chg_id { get; set; }
        public DateTime chg_date { get; set; }
        public bool active { get; set; }
        public string product_code { get; set; }
        public int month { get; set; }
        public int lengh { get; set; }
        public int number_qr { get; set; }
        public string number_qr_mapped { get; set; }
    }
}