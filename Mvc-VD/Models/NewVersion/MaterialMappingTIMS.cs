using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("w_material_mapping_tims")]
    public class MaterialMappingTIMS
    {
        [Key]
        public int wmmid { get; set; }

        public string mt_lot { get; set; }

        public string mt_cd { get; set; }
        public string material_code { get; set; }

        public string mt_no { get; set; }

        public DateTime mapping_dt { get; set; }

        public string bb_no { get; set; }

        public string remark { get; set; }

        public string sts_share { get; set; }

        public string use_yn { get; set; }

        public string del_yn { get; set; }

        public string reg_id { get; set; }

        public DateTime reg_date { get; set; }

        public string chg_id { get; set; }

        public DateTime chg_date { get; set; }

        public string status { get; set; }

        public bool active { get; set; }
    }
}