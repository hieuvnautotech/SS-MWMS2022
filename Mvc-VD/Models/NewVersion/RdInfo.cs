using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("w_rd_info")]
    public class RdInfo
    {
        [Key]
        [Column("rid")]
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [Column("rd_no")]
        public string RdNo { get; set; }

        [Column("rd_nm")]
        public string RdName { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("lct_cd")]
        public string LocationCode { get; set; }

        [Column("receiving_dt")]
        public string ReceivingDate { get; set; }

        [Column("remark")]
        public string Remark { get; set; }

        [Column("use_yn")]
        public string UseYN { get; set; }

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