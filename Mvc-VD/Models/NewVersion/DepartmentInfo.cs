using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("department_info")]
    public class DepartmentInfo
    {
        [Key]
        public int dpno { get; set; }
   
        public string depart_cd { get; set; }
       
        public string depart_nm { get; set; }
     
        public string up_depart_cd { get; set; }
      
        public string level_cd { get; set; }
      
        public string re_mark { get; set; }
       
        public string use_yn { get; set; }
      
        public int order_no { get; set; }
      
        public string del_yn { get; set; }
     
        public string reg_id { get; set; }
    
        public DateTime reg_dt { get; set; }
     
        public string chg_id { get; set; }
       
        public DateTime chg_dt { get; set; }
    
        public string mn_full { get; set; }
 
        public bool active { get; set; }
    }
}