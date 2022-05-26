using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("location_product")]
    public class LocationProduct
    {
        [Key]
        [Column("location_code")]
        public string LocationCode { get; set; }
        [Column("location_number")]
        public string LocationNumber { get; set; }
        [Column("gr_qty")]
        public int GroupQuantiy { get; set; }
    }
}