using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("m_facline_qc_value")]
    public class FaclineQCValue
    {
        [Key]
        
        public int fqhno { get; set; }
      
        public string fq_no { get; set; }
       
        public string product { get; set; }
        
        public string shift { get; set; }
       
        public string item_vcd { get; set; }
       
        public string check_id { get; set; }
        
        public string check_cd { get; set; }
        
        public string check_value { get; set; }
        
        public int check_qty { get; set; }
       
        public string date_ymd { get; set; }
      
        public string reg_id { get; set; }
      
        public DateTime reg_dt { get; set; }
      
        public string chg_id { get; set; }
        
        public DateTime chg_dt { get; set; }
       
        public bool active { get; set; }
    }

}