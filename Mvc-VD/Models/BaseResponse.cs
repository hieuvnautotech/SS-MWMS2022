using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models
{
    public class BaseResponse
    {

        public BaseResponse(bool result, string message)
        {
            this.result = result;
            this.message = message;
        }

        public bool result { get; set; }
        public string message { get; set; }
    }
}