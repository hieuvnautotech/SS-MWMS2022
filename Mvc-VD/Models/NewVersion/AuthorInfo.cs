using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("author_info")]
    public class AuthorInfo
    {
      
        public int atno { get; set; }
       
        public string at_cd { get; set; }
        
        public string at_nm { get; set; }
      
        public string role { get; set; }
      
        public string use_yn { get; set; }
        
        public string reg_id { get; set; }
      
        public DateTime reg_dt { get; set; }
        
        public string chg_id { get; set; }
        
        public DateTime chg_dt { get; set; }
       
        public string re_mark { get; set; }
       
        public bool isActive { get; set; }

        //biến ngoại lai
        public string role_nm { get; set; }
    }
}