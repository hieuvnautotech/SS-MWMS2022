using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("product_material")]
    public class ProductMaterial
    {
        [Key]
        public int Id { get; set; }
        public string style_no { get; set; }
        public string process_code { get; set; }
        public int level { get; set; }
        public string mt_no { get; set; }
        public string mt_nm { get; set; }
        public double need_time { get; set; }
        public double cav { get; set; }
        public double need_m { get; set; }
        public double buocdap { get; set; }
        public string use_yn { get; set; }
        public string reg_id { get; set; }
        public DateTime reg_dt { get; set; }
        public string chg_id { get; set; }
        public DateTime chg_dt { get; set; }
        public bool active { get; set; }


        //ngoại biến
        public string name { get; set; }
    }
}