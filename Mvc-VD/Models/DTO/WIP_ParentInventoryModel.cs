using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.DTO
{
    public class WIP_ParentInventoryModel
    {
        public string mt_no { get; set; }
        public string material_code { get; set; }
        public string mt_nm { get; set; }

        public string mt_cd { get; set; }

        public string qty { get; set; }
        public string DSD { get; set; }
        public string CSD { get; set; }
        public string TK { get; set; }
        public string lenght { get; set; }
        public string size { get; set; }
        public string recevice_dt { get; set; }
        public string sts_nm { get; set; }
    }
}