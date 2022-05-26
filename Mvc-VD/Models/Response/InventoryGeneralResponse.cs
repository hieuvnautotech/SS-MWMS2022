using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class InventoryGeneralResponse
    {
        public string product_cd { get; set; }

        public string product_nm { get; set; }

        public string md_cd { get; set; }

        public string at_no { get; set; }

        public string bom_type { get; set; }

        public float HCK { get; set; }

        public float HDG { get; set; }

        public float DKT { get; set; }
        public int MAPPINGBUYER { get; set; }
        public float CKT { get; set; }
        public int SORTING { get; set; }
    }
}