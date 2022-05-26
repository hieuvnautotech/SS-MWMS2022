using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.WOModel
{
    public class qc_itemcheck_mt_Model
    {
        public int qc_itemcheck_mt__icno { get; set; }
        public string qc_itemcheck_mt__check_subject { get; set; }
        public string qc_itemcheck_mt__check_id { get; set; }
        public List<view_qc_Model> view_qc_Model { get; set; }
    }
}