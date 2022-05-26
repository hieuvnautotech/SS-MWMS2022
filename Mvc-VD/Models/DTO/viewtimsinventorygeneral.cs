using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.DTO
{
    public class viewtimsinventorygeneral
    {
        public int id_actual { get; set; }
        public string product_cd { get; set; }
        public string product_nm { get; set; }
        public string md_cd { get; set; }
        public int Id { get; set; }
        public string WMaterialCode { get; set; }
        public string MaterialCode { get; set; }
        public string WMaterialNo { get; set; }
        public string WMaterialType { get; set; }
        public string WMaterialStatusCode { get; set; }
        public string Size { get; set; }
        public string Unit { get; set; }
        public int Length { get; set; }
        public int WMaterialGrQty { get; set; }
        public int Qty { get; set; }
        public string VBobbinCd { get; set; }
        public double WMaterialLength { get; set; }
        public string buyer_qr { get; set; }
        public string WMaterialStatusName { get; set; }
        public string WMaterialSize { get; set; }
        public string WMaterialQty { get; set; }
        public DateTime WMaterialReceivedDate { get; set; }
    }
}