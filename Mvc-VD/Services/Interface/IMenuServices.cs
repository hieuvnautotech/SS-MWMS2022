using Mvc_VD.Models.NewVersion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Mvc_VD.Services.Interface
{
    public interface IMenuServices
    {
        Task<IEnumerable<MenuInfo>> GetListMenuInfo(string mn_cd, string mn_nm, string full_nm);

        Task<IEnumerable<MenuInfo>> GetListMenuInfo();

        Task<int> InsertIntoMenuInfo(MenuInfo item);

        Task<int> RemoveMenuInfo(int mnno);

        Task<int> UpdateMenuInfo(MenuInfo item);

        Task<MenuInfo> GetMenuInfoById(int mnno);
    }
}