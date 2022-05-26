using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.HomeModel.Response
{
    public class AppVersionResponse
    {
        public AppVersionResponse(int appId, string type, string fileName, int version, string updatedAt)
        {
            this.appId = appId;
            this.type = type;
            this.fileName = fileName;
            this.version = version;
            this.updatedAt = updatedAt;
        }

        public int appId { get; set; }
        public string type { get; set; }
        public string fileName { get; set; }
        public int version { get; set; }
        public string updatedAt { get; set; }
    }
}