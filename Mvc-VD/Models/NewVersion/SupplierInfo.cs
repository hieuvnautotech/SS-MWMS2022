using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("supplier_info")]
    public class SupplierInfo
    {
        [Key]
        
        public int spno { get; set; }

      
        public string sp_cd { get; set; }

       
        public string sp_nm { get; set; }

       
        public string bsn_tp { get; set; }

       
        public string phone_nb { get; set; }

        
        public string cell_nb { get; set; }

        
        public string fax_nb { get; set; }

        
        public string e_mail { get; set; }

       
        public string web_site { get; set; }

       
        public string address { get; set; }

  
        public string re_mark { get; set; }

       
        public string use_yn { get; set; }

       
        public string del_yn { get; set; }

        
        public string reg_id { get; set; }

        
        public DateTime reg_dt { get; set; }

        
        public string chg_id { get; set; }

    
        public DateTime chg_dt { get; set; }

       
        public bool active { get; set; }


        public string bsn_tp1 { get; set; }
    }
}