
using Mvc_VD.Commons;
using Mvc_VD.Models;
using Mvc_VD.Models.HomeModel;
using Mvc_VD.Models.Language;
using Mvc_VD.Models.NewVersion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Mvc_VD.Services.Interface
{
    public interface IhomeService
    {
        Task<Models.NewVersion.VersionApp> GetLatestVersionApp(string appName);
        Task<int> ApplicationLogin(LoginRequest request);
        Task<int> GetListSdInfo(int alert);
        Task<IReadOnlyList<mb_info>> GetlistMbInfo(string UserID);
        Task<IEnumerable<SdInfo>> Notification1(int alert);
        Task<IEnumerable<ExtInfo>> Notification2(int alert);
        IEnumerable<Language> GetLanguage(string language, string router);
        Task<string> GetListAuthorMenuInfo(string user_id, string url);
    }
}
