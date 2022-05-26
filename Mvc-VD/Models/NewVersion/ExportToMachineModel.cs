using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.WIP
{
    public class ExportToMachineModel
    {
        public int Id { get; set; }
        public string ExportCode { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string MachineCode { get; set; }
        public string LocationCode { get; set; }
        public string IsFinish { get; set; }
        public string Description { get; set; }
        public string CreateId { get; set; }
        public DateTime CreateDate { get; set; }
        public string ChangeId { get; set; }
        public DateTime ChangeDate { get; set; }
        public bool active { get; set; }
    }
}
