using Mvc_VD.Models;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models.TIMS;
using Mvc_VD.Models.WIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_VD.Services.Interface
{
    public interface IHieuCommonServices
    {
        
        Task<IEnumerable<SdInfos>> GetListSDInfo(string SdNo, string SdName, string ProductCode, string remark);
       
    }
}
