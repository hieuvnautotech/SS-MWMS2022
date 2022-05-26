using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("d_rounting_info")]
    public class RountingInfo
    {
        [Key]
        [Column("idr")]
        public int RoutingId { get; set; }
        [Column("style_no")]
        public string StyleNo { get; set; }
        [Column("name")]
        public string RoutingName { get; set; }
        [Column("level")]
        public int RoutingLevel { get; set; }
        [Column("don_vi_pr")]
        public string RoutingUnitPr { get; set; }
        [Column("type")]
        public string RoutingType { get; set; }
        [Column("item_vcd")]
        public string RoutItemVCode { get; set; }
        [Column("reg_dt")]
        public DateTime CreateDate { get; set; }
        [Column("reg_id")]
        public string CreateId { get; set; }
        [Column("chg_id")]
        public string ChangeId { get; set; }
        [Column("chg_dt")]
        public DateTime ChangeDate { get; set; }
        [Column("active")]
        public bool Active { get; set; }
    }
}