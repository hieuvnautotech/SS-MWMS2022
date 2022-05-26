using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("w_product_qc")]
    public class ProductQC
    {
        [Key]
        public int pqno { get; set; }
      
        public string pq_no { get; set; }

        public string ml_no { get; set; }

        public string work_dt { get; set; }

        public string item_vcd { get; set; }

        public int check_qty { get; set; }

        public int ok_qty { get; set; }

        public string reg_id { get; set; }

        public DateTime reg_dt { get; set; }

        public string chg_id { get; set; }

        public DateTime chg_dt { get; set; }

        public bool active { get; set; }

    }
}