using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Request
{
    public class ModelInsertProductExcel
    {
        public string style_no { get; set; }
        public string style_nm { get; set; }
        public string md_cd { get; set; }
        public string prj_nm { get; set; }
        public string pack_amt { get; set; }
        public string stamp_code { get; set; }
        public string expiry { get; set; }
    }
}