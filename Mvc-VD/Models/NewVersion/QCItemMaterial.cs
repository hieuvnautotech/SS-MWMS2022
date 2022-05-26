using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("qc_item_mt")]
    public class QCItemMaterial
    {
        [Key]
        public int ino { get; set; }

        public string item_type { get; set; }

        public string item_vcd { get; set; }

        public string item_cd { get; set; }

        public string ver { get; set; }

        public string item_nm { get; set; }

        public string item_exp { get; set; }

        public string use_yn { get; set; }

        public string del_yn { get; set; }

        public string reg_id { get; set; }

        public DateTime reg_dt { get; set; }

        public string chg_id { get; set; }

        public DateTime chg_dt { get; set; }

        public bool active { get; set; }
    }
}