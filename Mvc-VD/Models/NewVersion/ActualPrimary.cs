using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("w_actual_primary")]
    public class ActualPrimary
    {
        [Key]
        public int id_actualpr { get; set; }

       
        public string at_no { get; set; }

        
        public string type { get; set; }

        
        public int target { get; set; }

        public string process_code { get; set; }

        public string product { get; set; }

       
        public string remark { get; set; }

   
        public string finish_yn { get; set; }

        public string isapply { get; set; }


        public string reg_id { get; set; }

       
        public DateTime reg_dt { get; set; }

       
        public string chg_id { get; set; }

        public DateTime chg_dt { get; set; }

        public bool active { get; set; }
    }
}