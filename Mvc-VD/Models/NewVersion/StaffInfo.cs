using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("mb_info")]
    public class StaffInfo
    {
        [Key]
        [Column("userid")]
        public string StaffId { get; set; }
        [Column("uname")]
        public string StaffName { get; set; }
        [Column("nick_name")]
        public string NickName { get; set; }
        [Column("upw")]
        public string StaffPassword { get; set; }
        [Column("grade")]
        public string Grade { get; set; }
        [Column("depart_cd")]
        public string DepartCode { get; set; }
        [Column("gender")]
        public string Gender { get; set; }
        [Column("position_cd")]
        public string PositionCode { get; set; }
        [Column("tel_nb")]
        public string TelNumber { get; set; }
        [Column("cel_nb")]
        public string CelNumber { get; set; }
        [Column("e_mail")]
        public string Email { get; set; }
        [Column("sms_yn")]
        public string SmsYn { get; set; }
        [Column("join_dt")]
        public DateTime JoinDate { get; set; }
        [Column("birth_dt")]
        public DateTime BirthDate { get; set; }
        [Column("scr_yn")]
        public string ScrYn { get; set; }
        [Column("mail_yn")]
        public string MailYn { get; set; }
        [Column("join_ip")]
        public string JoinIp { get; set; }
        [Column("join_domain")]
        public string JoinDomain { get; set; }
        [Column("ltacc_dt")]
        public DateTime ItaccDate { get; set; }
        [Column("ltacc_domain")]
        public string ItaccDomain { get; set; }
        [Column("mbout_dt")]
        public DateTime MboutDate { get; set; }
        [Column("mbout_yn")]
        public string MboutYn { get; set; }
        [Column("accblock_yn")]
        public string AccBlockYn { get; set; }
        [Column("session_key")]
        public string SessionKey { get; set; }
        [Column("session_limit")]
        public DateTime SessionLimit { get; set; }

        [Column("memo")]
        public string Memo { get; set; }
        [Column("del_yn")]
        public string DeleteYn { get; set; }
        [Column("check_yn")]
        public string CheckYn { get; set; }
        [Column("rem_me")]
        public string RememberMe { get; set; }
        [Column("barcode")]
        public string Barcode { get; set; }
        [Column("mbjoin_dt")]
        public DateTime MbJoinDate { get; set; }
        [Column("log_ip")]
        public string LogIp { get; set; }
        [Column("lct_cd")]
        public string LocationCode { get; set; }
        [Column("reg_id")]
        public string CreateId { get; set; }

        [Column("reg_dt")]
        public DateTime CreateDate { get; set; }
        [Column("chg_id")]
        public string ChangeId { get; set; }
        [Column("chg_dt")]
        public DateTime ChangeDate { get; set; }
        [Column("re_mark")]
        public string ReMark { get; set; }
        [Column("active")]
        public bool Active { get; set; }
    }
}