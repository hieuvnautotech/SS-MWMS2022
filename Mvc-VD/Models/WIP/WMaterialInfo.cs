using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.WIP
{
    public class WMaterialInfo
    {
        public int id { get; set; }
        public string mt_cd { get; set; }
        public string style_no { get; set; }
        public string lot_no { get; set; }
        public double? gr_qty { get; set; }
        public string expiry_dt { get; set; }
        public string dt_of_receipt { get; set; }
        public string sd_sts_cd { get; set; }
        public string sts_nm { get; set; }




        //
        public int wmtid { get; set; }
        public string mt_no { get; set; }
        public string mt_nm { get; set; }
        public int SoLuongCap { get; set; }
        public int SoLuongNhanDuoc { get; set; }
        public int SoluongConLai { get; set; }
        public string meter { get; set; }



    }
}
