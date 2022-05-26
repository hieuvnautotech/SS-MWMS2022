using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class BomCreateMaterialReponse
    {
        public int id { get; set; }
        public string productCode { get; set; }
        public string materialprarent { get; set; }
        public string materialno { get; set; }
        public string materialname { get; set; }
        public string CreateId { get; set; }
        public DateTime CreateDate { get; set; }
        public string ChangeId { get; set; }
        public DateTime ChangeDate { get; set; }
        public bool active { get; set; }

        public List<string> ListMaterial { get; set; }
    }
}