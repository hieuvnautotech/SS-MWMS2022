using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class ChangeBobbinForMaterialChildrenResponse
    {
        
        public int wmtid { get; set; }

        public int id_actual { get; set; }

        public string material_code { get; set; }

        public string material_code_parent { get; set; }

        public int gr_qty { get; set; }

        public string status { get; set; }

        public string mt_no { get; set; }

        public string bb_no { get; set; }

        public string chg_id { get; set; }

        public string chg_date { get; set; }

        public string reg_id { get; set; }

        public string reg_date { get; set; }

    }
}