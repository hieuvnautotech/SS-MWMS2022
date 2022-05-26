using Dapper;
using Mvc_VD.Models;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models.Response;
using Mvc_VD.Respositories.Irepository;
using Mvc_VD.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Mvc_VD.Services.Implement
{
    public class UserServices : DbConnection1RepositoryBase, IUserServices
    {
        public UserServices(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {
        }

        public async Task<string> CheckLoginUser(string username, string password)
        {
            try
            {
                var query = @"SELECT userid FROM mb_info WHERE userid = @Username and upw = @Password";
                var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @Username = username, @Password = password });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<string> GetAuthData(string userid)
        {
            try
            {
                var query = @"SELECT at_cd FROM mb_author_info WHERE userid = @UserId";
                var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @UserId = userid });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<MenuResponse>> GetListMenuByAuthData(string AuthData, string language)
        {
            try
            {
                var query = $"  select b.mn_cd,b.name{language} as mn_nm,b.up_mn_cd,b.url_link " +
                            "    from author_menu_info as a " +
                            "    join menu_info as b on concat(substring(a.mn_cd,1,3),'000000000') = b.mn_cd " +
                            "    where a.at_cd=@authorData AND b.use_yn='Y' " +
                            $"   group by  substring(a.mn_cd,1,3), b.name{language},b.mn_cd,b.up_mn_cd,b.url_link " +
                            "    UNION " +
                            $"    select b.mn_cd,b.name{language} as mn_nm,b.up_mn_cd,b.url_link " +
                            "    from author_menu_info as a " +
                            "    join menu_info as b on concat(substring(a.mn_cd,1,6),'000000') = b.mn_cd " +
                            "    where a.at_cd=@authorData AND b.use_yn='Y' " +
                            $"   group by b.name{language},b.mn_cd,b.up_mn_cd,b.url_link  " +
                            "    UNION " +
                            $"    SELECT  b.mn_cd,b.name{language} as mn_nm,b.up_mn_cd,b.url_link " +
                            "    from author_menu_info AS a " +
                            "    join menu_info as b on a.mn_cd = b.mn_cd " +
                            $"   where a.at_cd=@authorData AND b.use_yn='Y'  ";
                           // $" ORDER BY b.name{language},b.mn_cd,b.up_mn_cd,b.url_link";
                var result = await base.DbConnection.QueryAsync<MenuResponse>(query, new { @authorData = AuthData, @language = language });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<MenuInfo>> getListMenuInfo()
        {
            try
            {
                var query = @"Select * from menu_info where use_yn = 'Y' Order by mn_cd";
                var result = await base.DbConnection.QueryAsync<MenuInfo>(query);
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<BuyerLoginResponse> GetRoleFromAuthData(string AuthData)
        {
            try
            {
                var query = @"SELECT role,at_nm FROM author_info WHERE at_cd = @authorData";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<BuyerLoginResponse>(query, new { @authorData = AuthData });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<IReadOnlyList<notice_board>> GetNoticeBoard()
        {
            string sql = @"select * from notice_board";
            var result = await base.DbConnection.QueryAsync<notice_board>(sql);
            return result.ToList();
        }
        public async Task<IReadOnlyList<mb_message>> GetMBMessage()
        {
            string sql = @"select * from mb_message";
            var result = await base.DbConnection.QueryAsync<mb_message>(sql);
            return result.ToList();
        }
        public async Task<IEnumerable<AuthorAction>> GetListAuthorAction(string url)
        {
            try
            {
                var query = $"  select *  from author_action as a  where  REPLACE(a.url_link, '/', '') =@url_link ";
                var result = await base.DbConnection.QueryAsync<AuthorAction>(query, new { url_link = url });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<string> GetListAuthorMenuInfo(string user_id,string url)
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