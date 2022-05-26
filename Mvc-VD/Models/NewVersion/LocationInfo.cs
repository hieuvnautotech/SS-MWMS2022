using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("lct_info")]
    public class LocationInfo
    {
        [Key]
        public int lctno { get; set; }
        public string lct_cd { get; set; }

        public string lct_nm { get; set; }

        public string up_lct_cd { get; set; }

        public string level_cd { get; set; }

        public string index_cd { get; set; }

        public string shelf_cd { get; set; }

        public int order_no { get; set; }

        public string real_use_yn { get; set; }

        public string re_mark { get; set; }

        public string use_yn { get; set; }
        
        public string lct_rfid { get; set; }
       
        public string lct_bar_cd { get; set; }
       
        public string sf_yn { get; set; }
       
        public string is_yn { get; set; }
        
        public string mt_yn { get; set; }
        
        public string mv_yn { get; set; }
      
        public string ti_yn { get; set; }

        public string fg_yn { get; set; }
       
        public string rt_yn { get; set; }
       
        public string ft_yn { get; set; }
       
        public string wp_yn { get; set; }
       
        public string nt_yn { get; set; }
       
        public string pk_yn { get; set; }
       
        public string manager_id { get; set; }
       
        public string reg_id { get; set; }
       
        public DateTime reg_dt { get; set; }
      
        public string chg_id { get; set; }
       
        public DateTime chg_dt { get; set; }
       
        public string mn_full { get; set; }
        
        public string sap_lct_cd { get; set; }
        
        public string userid { get; set; }
       
        public string selected { get; set; }
       
        public bool active { get; set; }
    }
}