using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.DTO
{
    public class WIP_ParentInventoryModelExport
    {
        public string mt_no { get; set; }
        public string product_cd { get; set; }
        public string ExportCode { get; set; }
        public string material_code { get; set; }
        public string mt_nm { get; set; }
        public string qty { get; set; }
        public string DSD { get; set; }
        public string CSD { get; set; }
        public string returnMachine { get; set; }
        public string lenght { get; set; }
        public string size { get; set; }
        public string receipt_date { get; set; }
        public string sd_no { get; set; }
        public string sts_nm { get; set; }
    }
}