using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("w_material_info_memo")]
    public class MaterialInfoMemo
    {
        [Key]
        public int id { get; set; }
        public string md_cd { get; set; }
        public string style_no { get; set; }
        public string style_nm { get; set; }
        public string mt_cd { get; set; }
        public decimal width { get; set; }
        public string width_unit { get; set; }
        public decimal spec { get; set; }
        public string spec_unit { get; set; }
        public string sd_no { get; set; }
        public string lot_no { get; set; }
        public string status { get; set; }
        public string memo { get; set; }
        public string month_excel { get; set; }
        public string receiving_dt { get; set; }
        public int TX { get; set; }
        public decimal total_m { get; set; }
        public decimal total_m2 { get; set; }
        public decimal total_ea { get; set; }
        public string use_yn { get; set; }
        public string reg_id { get; set; }
        public DateTime reg_dt { get; set; }
        public string chg_id { get; set; }
        public DateTime chg_dt { get; set; }
        public bool active { get; set; }
    }
}