using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("stamp_detail")]
    public class StampDetail
    {
        [Key]
        public int id { get; set; }
     
        public string buyer_qr { get; set; }
       
        public string stamp_code { get; set; }
        public string ssver { get; set; }
        public string product_code { get; set; }

        public string vendor_code { get; set; }

        public string vendor_line { get; set; }

        public string label_printer { get; set; }

        public string is_sample { get; set; }

        public string pcn { get; set; }

        public string lot_date { get; set; }

        public string serial_number { get; set; }

        public string machine_line { get; set; }

        public string shift { get; set; }

        public int standard_qty { get; set; }

        public string is_sent { get; set; }

        public string box_code { get; set; }

        public string reg_id { get; set; }

        public DateTime reg_dt { get; set; }

        public string chg_id { get; set; }
       
        public DateTime chg_dt { get; set; }

        public bool active { get; set; }


        //Biến Ngoại Lai
        public string product_name { get; set; }
        public string model { get; set; }
        public string stamp_name { get; set; }
        public int quantity { get; set; }
        public string lotNo { get; set; }

    }
}