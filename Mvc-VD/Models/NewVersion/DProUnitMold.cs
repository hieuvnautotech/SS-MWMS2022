using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("d_pro_unit_mold")]
    public class DProUnitMold
    {
        [Key]
        public int mdid { get; set; }

        public int id_actual { get; set; }

        public DateTime start_dt { get; set; }

        public DateTime end_dt { get; set; }

        public string remark { get; set; }

        public string md_no { get; set; }

        public string use_yn { get; set; }

        public string del_yn { get; set; }

        public string reg_id { get; set; }

        public DateTime reg_dt { get; set; }

        public string chg_id { get; set; }

        public DateTime chg_dt { get; set; }

        public bool active { get; set; }
    }
}