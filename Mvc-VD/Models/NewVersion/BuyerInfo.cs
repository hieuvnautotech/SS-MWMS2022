using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("buyer_info")]
    public class BuyerInfo
    {
        [Key]
        
        public int byno { get; set; }
        
        public string buyer_cd { get; set; }
     
        public string buyer_nm { get; set; }
        
        public string ceo_nm { get; set; }
      
        public string manager_nm { get; set; }
      
        public string brd_nm { get; set; }
      
        public string logo { get; set; }
       
        public string phone_nb { get; set; }
     
        public string cell_nb { get; set; }
       
        public string fax_nb { get; set; }
      
        public string e_mail { get; set; }
       
        public string address { get; set; }
       
        public string web_site { get; set; }
       
        public string re_mark { get; set; }
      
        public string use_yn { get; set; }
       
        public string del_yn { get; set; }
       
        public string stampid { get; set; }
      
        public string reg_id { get; set; }
       
        public DateTime reg_dt { get; set; }
      
        public string chg_id { get; set; }
        
        public DateTime chg_dt { get; set; }
     
        public bool active { get; set; }
    }
}