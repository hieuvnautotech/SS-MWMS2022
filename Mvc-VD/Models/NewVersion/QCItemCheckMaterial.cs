using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("qc_itemcheck_mt")]
    public class QCItemCheckMaterial
    {
        [Key]
        public int icno { get; set; }
      
        public string item_vcd { get; set; }

        public string check_id { get; set; }

        public string check_type { get; set; }
     
        public string check_subject { get; set; }
       
        public decimal min_value { get; set; }
      
        public decimal max_value { get; set; }
        
        public string range_type { get; set; }
       
        public int order_no { get; set; }
       
        public string re_mark { get; set; }

        public string use_yn { get; set; }

        public string del_yn { get; set; }

        public string reg_id { get; set; }

        public DateTime reg_dt { get; set; }
       
        public string chg_id { get; set; }
        
        public DateTime chg_dt { get; set; }
       
        public bool active { get; set; }
    }
}