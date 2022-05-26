using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Mvc_VD.Models.HomeModel
{
    public class LoginRequest
    {
        public string userName { get; set; }
        public string password { get; set; }
        public string type { get; set; }
    }
}