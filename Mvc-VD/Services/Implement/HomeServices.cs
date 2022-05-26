using Mvc_VD.Respositories.Irepository;
using Mvc_VD.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models;
using Mvc_VD.Commons;
using Mvc_VD.Models.Language;

namespace Mvc_VD.Services.Implement
{
    public class HomeServices : DbConnection1RepositoryBase, IhomeService
    {
        public HomeServices(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {

        }

        public async Task<VersionApp> GetLatestVersionApp(string appName)
        {
            string sqlquery = @"SELECT id_app,type,name_file,version,chg_dt FROM version_app
                WHERE type=@Type
                ORDER BY version DESC";
            return await base.DbConnection.QueryFirstOrDefaultAsync<VersionApp>(sqlquery, new { Type = appName });
        }

        public async Task<int> ApplicationLogin (Models.HomeModel.LoginRequest request)
        {
            string sqlquery = @"SELECT COUNT(a.userid)
            FROM mb_info a
            join mb_author_info b ON a.userid =b.userid
            JOIN author_menu_info as c ON c.at_cd = b.at_cd
            JOIN menu_info d on c.mn_cd=d.mn_cd and c.at_cd=b.at_cd
            WHERE a.userid = @userId AND a.upw =@userPassword AND d.mn_full LIKE @Type";
            return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sqlquery, new { userId = request.userName, userPassword = request.password, Type = "%" + request.type + "%" });
        }

        public async Task<IReadOnlyList<mb_info>> GetlistMbInfo(string UserID)
        {
            string sql = "SELECT * FROM [dbo].[mb_info] where userid=@userid";
            var result = await base.DbConnection.QueryAsync<mb_info>(sql,new { userid= UserID });
            return result.ToList();
        }

        public async Task<int> GetListSdInfo(int alert)
        {
            try
            {
                var query = @"Select Count(sid) from w_sd_info where alert = @Alert";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Alert = alert});
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<SdInfo>> Notification1(int alert)
        {
            try
            {
                var query = @"Select * From w_sd_info where alert = @Alert Order by sd_no Desc";
                var result = await base.DbConnection.QueryAsync<SdInfo>(query, new { @Alert = alert });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<ExtInfo>> Notification2(int alert)
        {
            try
            {
                var query = @"Select * From w_ext_info where alert = @Alert Order by sd_no Desc";
                var result = await base.DbConnection.QueryAsync<ExtInfo>(query, new { @Alert = alert });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public IEnumerable<Language> GetLanguage(string language, string router)
        {
            try
            {
                if (string.IsNullOrEmpty(language))
                {
                    language = "en";
                }
                string sqlQuerry = $"SELECT keyname,{language} FROM language WHERE router=@router or router='public'";
                var result = base.DbConnection.Query<Language>(sqlQuerry, new { @language = language, @router= router });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<string> GetListAuthorMenuInfo(string user_id, string url)
        {

            try
            {
                var query = @"SELECT role from author_menu_info as a  where  a.at_cd = @userid and REPLACE(a.url_link, '/', '') =@url_link";
                var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { userid = user_id, url_link = url });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}