using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("w_product_qc_value")]
    public class ProductQcValue
    {
        [Key]
        [Column("pqhno")]
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [Column("pq_no")]
        public string PqNp { get; set; }

        [Column("item_vcd")]
        public string ItemVcd { get; set; }

        [Column("check_id")]
        public string CheckId { get; set; }

        [Column("check_cd")]
        public string CheckCode { get; set; }

        [Column("check_value")]
        public string CheckValue { get; set; }

        [Column("check_qty")]
        public int CheckQty { get; set; }

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