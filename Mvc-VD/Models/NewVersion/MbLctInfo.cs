using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("mb_lct_info")]
    public class MbLctInfo
    {
        [Key]
        [Column("dlno")]
        public string DlNo { get; set; }
        [Column("userid")]
        public string StaffId { get; set; }
        [Column("lct_cd")]
        public string LocationCode { get; set; }
        [Column("re_mark")]
        public string Remark { get; set; }
        [Column("use_yn")]
        public string UseYn { get; set; }
        [Column("reg_id")]
        public string CreateId { get; set; }
        [Column("reg_dt")]
        public DateTime CreateDate { get; set; }
        [Column("chg_id")]
        public string ChangeId { get; set; }
        [Column("chg_dt")]
        public DateTime ChangeDate { get; set; }
        [Column("active")]
        public bool Active { get; set; }
    }
}