using Mvc_VD.Models;
using Mvc_VD.Models.HomeModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Mvc_VD.Services
{
    public interface IHomeService
    {
        Task<VersionApp> GetLatestVersionApp(string appName);
        Task<int> ApplicationLogin(LoginRequest request);
    }
    public class HomeService: IHomeService
    {
        private Entities _db;
        public HomeService(IDbFactory dbFactory)
        {
            _db = dbFactory.Init();
        }
        public async Task<VersionApp> GetLatestVersionApp(string appName)
        {
            string sqlquery = @"SELECT id_app,type,name_file,version,Date_format(chg_dt, '%Y-%m-%d %H:%i:%s') AS chg_dt FROM version_app
            WHERE type=@1
            ORDER BY version DESC
            LIMIT 1";
            return _db.Database.SqlQuery<VersionApp>(sqlquery, new MySqlParameter("@1", appName)).FirstOrDefault();
        }

        public async Task<int> ApplicationLogin(LoginRequest request)
        {
            string sqlquery = @"SELECT COUNT(a.userid)
            FROM mb_info a
            join mb_author_info b ON a.userid =b.userid
            JOIN author_menu_info as c ON c.at_cd = b.at_cd
            JOIN menu_info d on c.mn_cd=d.mn_cd and c.at_cd=b.at_cd
            WHERE a.userid = @1 AND a.upw =@2 AND d.mn_full LIKE @3";
            return  _db.Database.SqlQuery<int>(sqlquery, new MySqlParameter("@1", request.userName), new MySqlParameter("@2", request.password), new MySqlParameter("@3", "%" + request.type + "%")).FirstOrDefault();
        }
    }
}