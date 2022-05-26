using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class MaterialBomResponse
    {
        
        public int MaterialBOMID { get; set; }
        public string materialNo { get; set; }
        public string materialName { get; set; }
        public bool active { get; set; }
    }
}