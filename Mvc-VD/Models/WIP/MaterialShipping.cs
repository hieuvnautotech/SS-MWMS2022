using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.WIP
{
    public class MaterialShipping
    {
        public string MaterialNo { get; set; }
        public string mt_cd { get; set; }
        public string mt_no { get; set; }
        public string sd_no { get; set; }
        public string lot_no { get; set; }
        public string unit_cd { get; set; }
        public string product { get; set; }
        public string countSocuon { get; set; }
        public string TongSoMet { get; set; }
        public DateTime? recevingDate { get; set; }
        public DateTime? rece_wip_dt { get; set; }
        public string reg_date_convert { get { return this.recevingDate?.ToString("yyyy-MM-dd"); } }
        public string receving_date_convert { get { return this.rece_wip_dt?.ToString("yyyy-MM-dd HH:mm:ss"); } }
    }
}