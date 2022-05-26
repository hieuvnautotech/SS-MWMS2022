using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("comm_mt")]
    public class CommMt
    {
        [Key]
       
        public int mt_id { get; set; }
        
        public string div_cd { get; set; }
       
        public string mt_cd { get; set; }
        
        public string mt_nm { get; set; }
       
        public string mt_exp { get; set; }
        
        public string memo { get; set; }
       
        public string use_yn { get; set; }
       
        public string reg_id { get; set; }
       
        public DateTime reg_dt { get; set; }
       
        public string chg_id { get; set; }
       
        public DateTime chg_dt { get; set; }
       
        public bool active { get; set; }
    }
}