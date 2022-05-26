using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.DTO
{
    public class Insert_MPO_QC_Model
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