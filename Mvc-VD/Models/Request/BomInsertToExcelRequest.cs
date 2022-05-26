using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Request
{
    public class BomInsertToExcelRequest
    {
        public string md_cd { get; set; }

        public string style_no { get; set; }

        public string mt_no { get; set; }

        public int? cav { get; set; }

        public int? need_time { get; set; }

        public float? buocdap { get; set; }

    }
}