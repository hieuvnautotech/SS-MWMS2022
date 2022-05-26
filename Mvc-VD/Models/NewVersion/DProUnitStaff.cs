using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("d_pro_unit_staff")]
    public class DProUnitStaff
    {
        [Key]
        
        public int psid { get; set; }
       
        public string staff_id { get; set; }
        
        public int actual { get; set; }
        
        public int defect { get; set; }

        public int id_actual { get; set; }
       
        public string staff_tp { get; set; }
      
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