using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.TIMS
{
    public class QCItemCheck_Mt_Model
    {
        public int qc_itemcheck_mt__icno { get; set; }
        public string qc_itemcheck_mt__check_subject { get; set; }
        public string qc_itemcheck_mt__check_id { get; set; }
        public List<ViewQCModel> view_qc_Model { get; set; }
    }
}