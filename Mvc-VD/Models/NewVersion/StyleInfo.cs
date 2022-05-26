using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mvc_VD.Models.NewVersion
{
    [Table("d_style_info")]
    public class StyleInfo
    {
        [Key]
        public int sid { get; set; }

        public string style_no { get; set; }

        public string style_nm { get; set; }

        public string md_cd { get; set; }

        public string prj_nm { get; set; }

        public string ssver { get; set; }

        public string part_nm { get; set; }

        public string standard { get; set; }

        public string cust_rev { get; set; }

        public string order_num { get; set; }

        public int pack_amt { get; set; }

        public string cav { get; set; }

        public string bom_type { get; set; }

        public string tds_no { get; set; }

        public string item_vcd { get; set; }

        public string qc_range_cd { get; set; }

        public string stamp_code { get; set; }

        public string expiry_month { get; set; }

        public string expiry { get; set; }

        public string use_yn { get; set; }

        public string del_yn { get; set; }

        public string reg_id { get; set; }

        public DateTime reg_dt { get; set; }

        public string chg_id { get; set; }

        public DateTime chg_dt { get; set; }

        public string drawingname { get; set; }

        public string loss { get; set; }

        public string Description { get; set; }

        public bool active { get; set; }



        //Ngoại Biến
        public string stamp_name { get; set; }

        public string partname { get; set; }
        public string productType { get; set; }

    }
}