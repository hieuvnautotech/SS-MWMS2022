using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("d_bobbin_lct_hist")]
    public class BobbinLctHist
    {
        [Key]
        public int blno { get; set; }
       
        public string mc_type { get; set; }
       
        public string bb_no { get; set; }
    
        public string mt_cd { get; set; }
      
        public string bb_nm { get; set; }
      
        public DateTime start_dt { get; set; }
        
        public DateTime end_dt { get; set; }
       
        public string use_yn { get; set; }
      
        public string del_yn { get; set; }
       
        public string reg_id { get; set; }
      
        public DateTime reg_dt { get; set; }
       
        public string chg_id { get; set; }
   
        public DateTime chg_dt { get; set; }

        public bool active { get; set; }
    }
}