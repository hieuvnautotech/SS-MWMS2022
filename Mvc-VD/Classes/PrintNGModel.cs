using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Classes
{
    public class PrintNGModel
    {
        public string Id { get; set; }
        public string WMaterialCode { get; set; }
        public string WMaterialNumber { get; set; }
        public string WMaterialName { get; set; }
        public string WMaterialType { get; set; }
        public string WAlertNG { get; set; }
        public string WMaterialStatusCode { get; set; }
        public string WMaterialGrQty { get; set; }
        public string DMaterialSpec { get; set; }
        public string DMaterialSpecUnit { get; set; }
        public string DMaterialBundleUnit { get; set; }
        public string Product { get; set; }
        public string Process { get; set; }
        public string WorkerId { get; set; }
        public string WorkerName { get; set; }
        public string Qty { get; set; }
        public string Qty_Unit { get; set; }
        public string CreatedDate { get; set; }
    }
}