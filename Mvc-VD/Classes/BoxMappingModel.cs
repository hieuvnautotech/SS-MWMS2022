using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Classes
{
    public class BoxMappingModel
    {
        public int? Id { get; set; }
        public string BoxCode { get; set; }
        public string BuyerCode { get; set; }
        public string MaterialCode { get; set; }
        public int? Quantity { get; set; }
        public string MappingDate { get; set; }
        public string Status { get; set; }
        public string UseYN { get; set; }
        public string DelYN { get; set; }
        public string RegisterId { get; set; }
        public DateTime? RegisterDate { get; set; }
        public string ChangeId { get; set; }
        public DateTime? ChangeDate { get; set; }
    }
}