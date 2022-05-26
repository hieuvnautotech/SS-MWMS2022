using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
   
    public class GetQMSNGModel
    {
        public int? stt { get; set; }
        public int? Total { get; set; }
        public int? NG { get; set; }
        public string Shift { get; set; }
        public int? OK { get; set; }
        public int? chuaphanloai { get; set; }
        public string CreateOn { get; set; }
        public string fq_no { get; set; }
        public string ProductCode { get; set; }
        public string at_no { get; set; }
    }
}