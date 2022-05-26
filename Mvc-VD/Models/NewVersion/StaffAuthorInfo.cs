using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("mb_author_info")]
    public class StaffAuthorInfo
    {
        [Key]
        [Column("mano")]
        public int UserId { get; set; }
        [Column("userid")]
        public string StaffId { get; set; }
        [Column("at_cd")]
        public string AtCode { get; set; }
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