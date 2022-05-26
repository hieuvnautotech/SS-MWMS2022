using Mvc_VD.Models.NewVersion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Mvc_VD.Services.Interface
{
    public interface ISupplierQRServiecs
    {
        #region Supplier QR Management
        Task<IEnumerable<MaterialInfo>> SearchListMaterialInfo(string user, string type, string name, string code, string start, string end);

        Task<IEnumerable<MaterialInfo>> GetListMaterialInfo(string mt_cd, string mt_no);

        Task<IEnumerable<MaterialInfoTam>> GetListMaterialInfoTam(List<string> wmtid);
        #endregion

        Task<IEnumerable<MaterialInfo>> searchMachiningQR(string type, string name, string code, string start, string end);
    }
}