using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.HomeModel.Request
{
    public class CheckVersionRequest
    {
     
        public int versionCode { get; set; }
        public string appName { get; set; }


    }
}