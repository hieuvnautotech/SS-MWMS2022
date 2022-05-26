using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("stamp_master")]
    public class StampMaster
    {
        [Key]
      
        public int id { get; set; }

        public string stamp_code { get; set; }

        public string stamp_name { get; set; }
    }
}