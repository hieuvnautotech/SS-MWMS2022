using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.WOModel
{
    public class w_actual_dto
    {
        public int IdActual { get; set; }
        public string AtNo { get; set; }
        public string Type { get; set; }
        public string Product { get; set; }
        public Nullable<double> Actual { get; set; }
        public Nullable<double> Defect { get; set; }
        public string Name { get; set; }
        public Nullable<int> Level { get; set; }
        public string Date { get; set; }
        public string UnitPr { get; set; }
        public string ItemVcd { get; set; }
        public string CreateId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string ChangeId { get; set; }
        public Nullable<System.DateTime> ChangeDate { get; set; }
    }
}