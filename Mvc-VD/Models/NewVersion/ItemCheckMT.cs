using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("qc_itemcheck_mt")]
    public class ItemCheckMT
    {
        [Key]
        [Required(ErrorMessage = "IdNo is required")]
        [Column("icno")]
        public int IdNo { get; set; }

        [Column("item_vcd")]
        public string ItemVcd { get; set; }

        [Column("check_id")]
        public string CheckId { get; set; }

        [Column("check_type")]
        public string CheckType { get; set; }

        [Column("check_subject")]
        public string CheckSubject { get; set; }

        [Column("min_value")]
        public decimal MinValue { get; set; }

        [Column("max_value")]
        public decimal MaxValue { get; set; }

        [Column("range_type")]
        public string RangeType { get; set; }

        [Column("order_no")]
        public int OrderNo { get; set; }

        [Column("re_mark")]
        public string Remark { get; set; }

        [Column("use_yn")]
        public string UseYN { get; set; }

        [Column("del_yn")]
        public string DeleteYN { get; set; }

        [Column("reg_id")]
        public string CreateId { get; set; }

        [Column("reg_dt")]
        public DateTime CreateDate{ get; set; }

        [Column("chg_id")]
        public string ChangeId { get; set; }

        [Column("chg_dt")]
        public DateTime ChangeDate { get; set; }

        [Column("active")]
        public bool Active { get; set; }
    }
}