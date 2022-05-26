using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class GeneralResponse
    {
        public int wmtid { get; set; }
        public string qty { get; set; }
        public string model { get; set; }
        public string product_cd { get; set; }
        public string product_nm { get; set; }
        public int BTP { get; set; }
        public int TP { get; set; }
    }
}