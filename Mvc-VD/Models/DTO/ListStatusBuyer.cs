using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.DTO
{
    public class ListStatusBuyer
    {
        public int gr_qty { get; set; }
        public int gr_qty_bf { get; set; }
        public string mt_sts_nm { get; set; }
        public string product { get; set; }
        public string po { get; set; }
    }
}