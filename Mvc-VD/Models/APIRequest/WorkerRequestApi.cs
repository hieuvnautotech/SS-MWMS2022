using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.APIRequest
{
    public class WorkerRequestApi:Pageing
    {
        public string User_Id { get; set; }
        public string User_Name { get; set; }
        public string Position_Code { get; set; }
    }
}