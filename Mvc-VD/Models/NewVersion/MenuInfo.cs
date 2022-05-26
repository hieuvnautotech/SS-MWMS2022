using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("menu_info")]
    public class MenuInfo
    {
        [Key]
        public int mnno { get; set; }
        public string mn_cd { get; set; }
        public string mn_nm { get; set; }
        public string up_mn_cd { get; set; }
        public string level_cd { get; set; }
        public string url_link { get; set; }
        public string re_mark { get; set; }
        public string col_css { get; set; }
        public string sub_yn { get; set; }
        public int order_no { get; set; }
        public string use_yn { get; set; }
        public string mn_full { get; set; }
        public string mn_cd_full { get; set; }
        public string reg_id { get; set; }
        public DateTime reg_dt { get; set; }
        public string chg_id { get; set; }
        public DateTime chg_dt { get; set; }
        public string selected { get; set; }
        public string role { get; set; }
        public bool active { get; set; }
    }
}