using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    public class viewQCModel
    {
        public int qc_itemcheck_dt__icdno { get; set; }
        public string qc_itemcheck_dt__check_name { get; set; }
    }
}