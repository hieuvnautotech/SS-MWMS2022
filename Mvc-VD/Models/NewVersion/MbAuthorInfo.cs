using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("mb_author_info")]
    public class MbAuthorInfo
    {
        [Key]
        public int mano { get; set; }

        public string userid { get; set; }

        public string at_cd { get; set; }

        public string re_mark { get; set; }

        public string use_yn { get; set; }

        public string reg_id { get; set; }

        public DateTime reg_dt { get; set; }

        public string chg_id { get; set; }

        public DateTime chg_dt { get; set; }

        public bool active { get; set; }
    }
}