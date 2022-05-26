using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("manufac_info")]
    public class ManufacInfo
    {
        public int mfno { get; set; }

        public string mf_cd { get; set; }

        public string mf_nm { get; set; }

        public string brd_nm { get; set; }

        public string logo { get; set; }

        public string phone_nb { get; set; }

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
    }
}