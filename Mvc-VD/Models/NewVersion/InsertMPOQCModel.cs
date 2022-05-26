using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    public class InsertMPOQCModel
    {
        public int icno { get; set; }
        public List<objTr> objTr { get; set; }
    }
    public class objTr
    {
        public int icdno { get; set; }
        public int input { get; set; }
    }
}