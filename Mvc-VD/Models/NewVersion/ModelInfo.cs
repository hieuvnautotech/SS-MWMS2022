using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("d_model_info")]
    public class ModelInfo
    {
        [Key]
        public int mdid { get; set; }
       
        public string md_cd { get; set; }
       
        public string md_nm { get; set; }
        
        public string use_yn { get; set; }
      
        public string del_yn { get; set; }
       
        public string reg_id { get; set; }
        
        public DateTime reg_dt { get; set; }
       
        public string chg_id { get; set; }
       
        public DateTime chg_dt { get; set; }
        
        public bool active { get; set; }
    }
}