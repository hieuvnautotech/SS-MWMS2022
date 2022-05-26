using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class WOCommonResponse
    {
        public string cdid {get;set;}
        public string mt_cd { get; set; }
        public string mt_nm { get; set; }
        public string dt_cd { get; set; }
        public string dt_nm { get; set; }
        public string dt_exp { get; set; }
        public string dt_order { get; set; }
        public string use_yn { get; set; }
    }
}