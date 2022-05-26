using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("generalfg")]
    public class Generalfg
    {
        [Key]

        public int id { get; set; }

        public string buyer_qr { get; set; }

        public string product_code { get; set; }

        public string type { get; set; }

        public string md_cd { get; set; }

        public string dl_no { get; set; }

        public int qty { get; set; }

        public string lot_no { get; set; }

        public string status { get; set; }

        public string use_yn { get; set; }

        public string reg_id { get; set; }

        public string buyer_cd { get; set; }

        public DateTime reg_dt { get; set; }

        public string chg_id { get; set; }

        public DateTime chg_dt { get; set; }

        public bool active { get; set; }
    }
}