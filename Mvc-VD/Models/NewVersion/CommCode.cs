//using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Linq;
using System.Web;


namespace Mvc_VD.Models.NewVersion
{
    [Table("comm_dt")]
    public class CommCode
    {
        [Key]
        public int cdid { get; set; }
       
        public string mt_cd { get; set; }
        
        public string dt_cd { get; set; }
       
        public string dt_nm { get; set; }
        
        public string dt_kr { get; set; }
        
        public string dt_vn { get; set; }
        
        public string dt_exp { get; set; }
       
        public string up_cd { get; set; }
       
        public string val1 { get; set; }
       
        public string val1_nm { get; set; }
       
        public string val2 { get; set; }
       
        public string val2_nm { get; set; }
        
        public string val3 { get; set; }
       
        public string val3_nm { get; set; }
        
        public string val4 { get; set; }
       
        public string val4_nm { get; set; }
        
        public int dt_order { get; set; }
       
        public string use_yn { get; set; }
       
        public string del_yn { get; set; }
       
        public string reg_id { get; set; }
       
        public DateTime reg_dt { get; set; }
       
        public string chg_id { get; set; }
      
        public DateTime chg_dt { get; set; }
       
        public string unit { get; set; }
        
        public bool active { get; set; }


        //Ngoại Biến
        public string mt_nm { get; set; }
    }
}