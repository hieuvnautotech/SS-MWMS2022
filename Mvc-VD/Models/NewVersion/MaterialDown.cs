using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("w_material_down")]
    public class MaterialDown
    {
        [Key]
        public int wmtid { get; set; }

        public string mt_cd { get; set; }

        public double gr_qty { get; set; }

        public double gr_down { get; set; }

        public string reason { get; set; }

        public string status_now { get; set; }

        public string bb_no { get; set; }

        public string use_yn { get; set; }

        public string reg_id { get; set; }

        public string reg_dt { get; set; }

        public string chg_id { get; set; }

        public string chg_dt { get; set; }

    }
}