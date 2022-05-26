using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("m_facline_qc")]
    public class FaclineQC
    {
        [Key]
        public int fqno { get; set; }

        public string fq_no { get; set; }

        public string ml_no { get; set; }

        public string ml_tims { get; set; }

        public string product_cd { get; set; }

        public string shift { get; set; }

        public string at_no { get; set; }

        public string work_dt { get; set; }

        public string item_vcd { get; set; }

        public string item_nm { get; set; }

        public string item_exp { get; set; }

        public int check_qty { get; set; }

        public int ok_qty { get; set; }

        public string reg_id { get; set; }

        public DateTime reg_dt { get; set; }

        public string chg_id { get; set; }

        public DateTime chg_dt { get; set; }

        public bool active { get; set; }


        //Ngoại Biến
        public int defect_qty { get; set; }

    }

}