using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("w_ext_info")]
    public class ExtInfo
    {
        [Key]
        public int extid { get; set; }

        public string ext_no { get; set; }

        public string ext_nm { get; set; }

        public string status { get; set; }

        public string work_dt { get; set; }

        public string lct_cd { get; set; }

        public int alert { get; set; }

        public string remark { get; set; }

        public string use_yn { get; set; }

        public string reg_id { get; set; }

        public DateTime reg_dt { get; set; }

        public string chg_id { get; set; }

        public DateTime chg_dt { get; set; }

        public DateTime active { get; set; }
    }
}