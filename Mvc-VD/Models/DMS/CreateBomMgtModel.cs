using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models
{
    public class CreateBomMgtModel
    {
        [Required]

        public string ProductCode { get; set; }
        public string materialNo { get; set; }
        [Required]
        public int? cavit { get; set; }
        [Required]
        public float? need_time { get; set; }
        [Required]
        public decimal buocdap { get; set; }
        [Required]
        public bool isActive { get; set; }
    }
}