using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models
{
    public class CreateBomMaterialModel
    {
        public int? Id { get; set; }
        public string ProductCode { get; set; }
        public string MaterialPrarent { get; set; }
        public string MaterialNo { get; set; }
        public string MaterialName { get; set; }
        public string CreateId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string ChangeId { get; set; }
        public System.DateTime ChangeDate { get; set; }
        [Required]
        public string[] ListMaterial { get; set; }
    }
}