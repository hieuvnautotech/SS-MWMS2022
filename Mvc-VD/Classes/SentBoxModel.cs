using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Classes
{
    public class SentBoxModel
    {
        public int wmtid { get; set; }
        public string mt_cd { get; set; }
        public string bb_no { get; set; }
        public string buyer_qr { get; set; }
        public string mt_type_nm { get; set; }
        public double? gr_qty { get; set; }
        public string sts_nm { get; set; }
    }
}