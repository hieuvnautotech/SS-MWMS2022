using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("notice_board")]
    public class NoticeBoard
    {
        [Key]
       
        public int bno { get; set; }
       
        public string title { get; set; }
       
        public string content { get; set; }
       
        public string mn_cd { get; set; }
       
        public int viewcnt { get; set; }
       
        public int replycnt { get; set; }
       
        public string div_cd { get; set; }
       
        public string lng_cd { get; set; }
       
        public DateTime? start_dt { get; set; }
       
        public DateTime? end_dt { get; set; }
       
        public int widthsize { get; set; }
       
        public int heightsize { get; set; }
       
        public string back_color { get; set; }
       
        public int order_no { get; set; }
       
        public string del_yn { get; set; }
       
        public string reg_id { get; set; }
       
        public DateTime? reg_dt { get; set; }
       
        public string chg_id { get; set; }
       
        public DateTime? chg_dt { get; set; }
       
        public bool active { get; set; }
    }
}