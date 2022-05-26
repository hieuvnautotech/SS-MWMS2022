using Mvc_VD.Models;
using Mvc_VD.Models.NewVersion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_VD.Services.ShinsungNew.Iservices
{
    public interface ItestWoServiceNew
    {
        Task<IReadOnlyList<Models.MaterialInfoMMS>> GetAllAsync();
        Task<IReadOnlyList<d_style_info>> GetStyleInfo();
    }
}
