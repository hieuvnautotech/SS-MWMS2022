using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    public class QcitemCheckModel
    {
        public int qc_itemcheck_mt__icno { get; set; }
        public string qc_itemcheck_mt__check_subject { get; set; }
        public string qc_itemcheck_mt__check_id { get; set; }
        public List<viewQCModel> view_qc_Model { get; set; }
    }
}