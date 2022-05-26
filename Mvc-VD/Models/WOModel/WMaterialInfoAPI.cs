using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.WOModel
{
    public class WMaterialInfoAPI
    {
        public int wmtid { get; set; }
        public string mt_cd { get; set; }
        public int gr_qty { get; set; }
        public string bb_no { get; set; }
        public int sl_tru_ng { get; set; }
        public DateTime reg_date { get; set; }
        public string reg_date1 { get; set; }
        public string ca { get; set; }

        //Ngoại Biến
        public string reg_dt { get { return this.reg_date.ToString("yyyy-MM-dd"); } }
    }
}