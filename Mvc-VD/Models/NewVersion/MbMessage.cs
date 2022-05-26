using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("mb_message")]
    public class MbMessage
    {
        [Key]
        [Column("tid")]
        public int MessageId { get; set; }
        [Column("message")]
        public string Message { get; set; }
        [Column("del_yn")]
        public string DeleteYn { get; set; }
        [Column("chg_dt")]
        public DateTime ChangeDate { get; set; }
        [Column("reg_dt")]
        public DateTime CreateDate { get; set; }
        [Column("chg_id")]
        public string ChangeId { get; set; }
        [Column("reg_id")]
        public string CreateId { get; set; }
        [Column("active")]
        public bool Active { get; set; }
    }
}