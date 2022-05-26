using Mvc_VD.Models;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Mvc_VD.Services.Interface
{
    public interface IUserServices
    {
        Task<IEnumerable<MenuInfo>> getListMenuInfo();
        Task<string> CheckLoginUser(string username, string password);
        Task<string> GetAuthData(string userid);
        Task<BuyerLoginResponse> GetRoleFromAuthData(string AuthData);
        Task<IEnumerable<MenuResponse>> GetListMenuByAuthData(string AuthData, string language);
        Task<IReadOnlyList<notice_board>> GetNoticeBoard();
        Task<IReadOnlyList<mb_message>> GetMBMessage();
        Task<IEnumerable<AuthorAction>> GetListAuthorAction(string url);
        Task<string> GetListAuthorMenuInfo(string user_id,string url);
    }
}