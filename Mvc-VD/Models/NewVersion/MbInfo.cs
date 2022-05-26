
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
   [Table("mb_info")]
    public class MbInfo
    {
        public int mbid { get; set; }
        public string userid { get; set; }
        public string uname { get; set; }
        public string nick_name { get; set; }
        public string upw { get; set; }
        public string grade { get; set; }
        public string depart_cd { get; set; }
        public string gender { get; set; }
        public string position_cd { get; set; }
        public string tel_nb { get; set; }
        public string cel_nb { get; set; }
        public string e_mail { get; set; }
        public string sms_yn { get; set; }
        public string join_dt { get; set; }
        public string birth_dt { get; set; }
        public string scr_yn { get; set; }
        public string mail_yn { get; set; }
        public string join_ip { get; set; }
        public string join_domain { get; set; }
        public DateTime ltacc_dt { get; set; }
        public string ltacc_domain { get; set; }
        public DateTime mbout_dt { get; set; }
        public string mbout_yn { get; set; }
        public string accblock_yn { get; set; }
        public string session_key { get; set; }
        public DateTime session_limit { get; set; }
        public string memo { get; set; }
        public string del_yn { get; set; }
        public string check_yn { get; set; }
        public string rem_me { get; set; }
        public string barcode { get; set; }
        public DateTime mbjoin_dt { get; set; }
        public string log_ip { get; set; }
        public string lct_cd { get; set; }
        public string reg_id { get; set; }
        public DateTime reg_dt { get; set; }
        public string chg_id { get; set; }
        public DateTime chg_dt { get; set; }
        public string re_mark { get; set; }
    }
}