using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("author_menu_info")]
    public class AuthorMenuInfo
    {
        [Key]
        public int amno { get; set; }
     
        public string at_cd { get; set; }
       
        public string mn_cd { get; set; }
        public string role { get; set; }
        public string dt_nm { get; set; }
       
        public string mn_nm { get; set; }
       
        public string url_link { get; set; }
        
        public string re_mark { get; set; }
       
        public string use_yn { get; set; }
       
        public string st_yn { get; set; }
       
        public string ct_yn { get; set; }
       
        public string mt_yn { get; set; }
       
        public string del_yn { get; set; }
        
        public string reg_id { get; set; }
        
        public DateTime reg_dt { get; set; }
       
        public string chg_id { get; set; }
        
        public DateTime chg_dt { get; set; }
       
        public bool active { get; set; }
    }
}