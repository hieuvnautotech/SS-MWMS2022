using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("w_vt_mt")]
    public class VTMaterial
    {
        [Key]
        [Column("vno")]
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [Column("vn_cd")]
        public string VnCode { get; set; }

        [Column("vn_nm")]
        public string VnName { get; set; }

        [Column("start_dt")]
        public string StartDate { get; set; }

        [Column("end_dt")]
        public string EndDate { get; set; }

        [Column("re_mark")]
        public string Remark { get; set; }

        [Column("use_yn")]
        public string UseYN { get; set; }

        [Column("del_yn")]
        public string DeleteYN { get; set; }

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