using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("qc_itemcheck_dt")]
    public class QCItemCheckDetail
    {
        [Key]
        public int icdno { get; set; }
        
        public string item_vcd { get; set; }
       
        public string check_id { get; set; }
       
        public string check_cd { get; set; }
        
        public string defect_yn { get; set; }
       
        public string check_name { get; set; }
       
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