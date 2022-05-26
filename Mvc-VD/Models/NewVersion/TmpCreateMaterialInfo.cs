using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    public class TmpCreateMaterialInfo
    {
        public int mt_qr_id { get; set; }
        public string mt_no { get; set; }
        public DateTime export_date { get; set; }
        public DateTime date_of_receipt { get; set; }
        public int month { get; set; }
        public DateTime expiry_date { get; set; }
        public string lot_no { get; set; }
        public string type { get; set; }
        public int lengh { get; set; }
        public int number_qr { get; set; }
        public string number_qr_mapped { get; set; }
        public string status { get; set; }
        public bool active { get; set; }
        public string reg_id { get; set; }
        public DateTime reg_date { get; set; }
        public string chg_id { get; set; }
        public DateTime chg_date { get; set; }
    }
}