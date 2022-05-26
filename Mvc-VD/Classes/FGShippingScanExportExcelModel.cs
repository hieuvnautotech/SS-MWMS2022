using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Classes
{
    public class FGShippingScanExportExcelModel
    {
        public string id { get; set; }
        public string product { get; set; }
        public string end_production_dt { get; set; }
        public string buyer_qr { get; set; }
        public string box_code { get; set; }
        public string work_dt { get; set; }
        public int? qty { get; set; }


    }
}