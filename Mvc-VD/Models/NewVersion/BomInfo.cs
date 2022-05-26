using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("d_bom_info")]
    public class BomInfo
    {
        [Key]
        
        public int bid { get; set; }
        
        public string bom_no { get; set; }
      
        public string style_no { get; set; }
        
        public string mt_no { get; set; }

        public string mt_nm { get; set; }

        public double? need_time { get; set; }
    
        public int? cav { get; set; }
        
        public double? need_m { get; set; }
     
        public double? buocdap { get; set; }
        
        public string del_yn { get; set; }
      
        public string isapply { get; set; }

        public int? IsActive { get; set; }

        public bool active { get; set; }
       
        public string reg_id { get; set; }
       
        public DateTime reg_dt { get; set; }
     
        public string chg_id { get; set; }
       
        public DateTime chg_dt { get; set; }
    }
}