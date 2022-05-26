using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class WMaterialInfoResponse
    {
        public int wmtid { get; set; }
        public string mt_no { get; set; }
        public string mt_nm { get; set; }
        public int SoLuongCap { get; set; }
        public int SoLuongNhanDuoc { get; set; }
        public int SoluongConLai { get; set; }
        public string meter { get; set; }
    }
}