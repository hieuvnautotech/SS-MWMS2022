using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("materialbom")]
    public class MaterialBom
    {
        [Key]
        public string id { get; set; }
       
        public string productcode { get; set; }
      
        public string materialprarent { get; set; }
      
        public string materialno { get; set; }
      
        public string reg_id { get; set; }
      
        public string chg_id { get; set; }
        
        public DateTime reg_dt { get; set; }
      
        public DateTime chg_dt { get; set; }
      
        public bool active { get; set; }
    }
}