//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mvc_VD.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MaterialBomModel
    {
        public int MaterialBOMID { get; set; }
        public int MaterialID { get; set; }
        public int BOMID { get; set; }
        public string materialNo { get; set; }
        public string materialName { get; set; }
        public string ModelCode { get; set; }
        public System.DateTime CreateOn { get; set; }
        public string ProductCode { get; set; }
        public bool IsActive { get; set; }
    }
}