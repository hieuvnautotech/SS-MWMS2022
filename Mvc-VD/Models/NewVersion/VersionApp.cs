using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("version_app")]
    public class VersionApp
    {
        [Key]
        public int id_app { get; set; }

        public string type { get; set; }

        public string name_file { get; set; }

        public int version { get; set; }

        public DateTime chg_dt { get; set; }

        public DateTime reg_dt { get; set; }

        public bool active { get; set; }


      
    }
}