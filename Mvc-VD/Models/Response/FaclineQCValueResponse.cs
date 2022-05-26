using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class FaclineQCValueResponse
    {
        public string fqhno { get; set; }
        public string check_subject { get; set; }
        public string check_value { get; set; }
        public int check_qty { get; set; }
        public DateTime date_ymd { get; set; }
    }
}