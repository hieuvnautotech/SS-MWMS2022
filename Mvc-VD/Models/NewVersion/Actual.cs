using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("w_actual")]
    public class Actual
    {
        [Key]
        public int id_actual { get; set; }

        public string at_no { get; set; }

        public string type { get; set; }
        public string product { get; set; }

        public float actual { get; set; }

        public float defect { get; set; }

        public string name { get; set; }

        public int? level { get; set; }

        public string date { get; set; }

        public string don_vi_pr { get; set; }

        public string item_vcd { get; set; }

        public string description { get; set; }
        public bool IsFinish { get; set; }

        public string reg_id { get; set; }

        public DateTime reg_dt { get; set; }

        public string chg_id { get; set; }

        public DateTime chg_dt { get; set; }
        public bool IsFinished { get; set; }

        public bool active { get; set; }
    }
}