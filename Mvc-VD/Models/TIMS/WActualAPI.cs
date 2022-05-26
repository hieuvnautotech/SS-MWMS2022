using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.TIMS
{
    public class WActualAPI
    {

        public string Name { get; set; }
        public double target { get; set; }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Name_View { get; set; }
        public string QCCode { get; set; }
        public string QCName { get; set; }
        public double Defective { get; set; }
        public string RollCode { get; set; }
        public string RollName { get; set; }
        public double ActualQty { get; set; }
        public string at_no { get; set; }
        public int ProcessRun { get; set; }
        public int IsFinish { get; set; }
    }
}