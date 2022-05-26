using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("w_sd_info")]
    public class SdInfo
    {
        [Key]
       
        public int sid { get; set; }

       
        public string sd_no { get; set; }

       
        public string sd_nm { get; set; }

       
        public string status { get; set; }

      
        public string product_cd { get; set; }

        public string lct_cd { get; set; }

        
        public int alert { get; set; }

       
        public string remark { get; set; }

        public string use_yn { get; set; }

        
        public string del_yn { get; set; }

      
        public string reg_id { get; set; }

       
        public DateTime reg_dt { get; set; }

        
        public string chg_id { get; set; }

        
        public DateTime chg_dt { get; set; }



        //Ngoại Biến
        public string sts_nm { get; set; }
        public string lct_nm { get; set; }
    }
}