using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("tmp_w_material_divide_mms")]
    public class MaterialInfoDivideMMS
    {
        [Key]
        public int wmtid { get; set; }

        public string material_code_parent { get; set; }

        public string material_code { get; set; }
        public string mt_cd { get; set; }

        public int id_actual { get; set; }

        public string mt_no { get; set; }

        public int gr_qty { get; set; }
        
        public int real_qty { get; set; }

        public string bb_no { get; set; }

        public string chg_id { get; set; }

        public string reg_id { get; set; }

        public DateTime chg_date { get; set; }

        public DateTime reg_date { get; set; }

        public string status { get; set; }

        public bool active { get; set; }
    }
}