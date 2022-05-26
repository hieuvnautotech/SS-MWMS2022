using Mvc_VD.Respositories.Irepository;
using Mvc_VD.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Threading.Tasks;
using Mvc_VD.Models.NewVersion;

namespace Mvc_VD.Services
{
    public class MenuServices : DbConnection1RepositoryBase, IMenuServices
    {
        public MenuServices(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {
        }

        public async Task<IEnumerable<MenuInfo>> GetListMenuInfo(string mn_cd, string mn_nm, string full_nm)
        {
            try
            {
                var query = @"Select * from menu_info 
                            where (@Mn_Cd = '' OR @Mn_Cd IS NULL OR mn_cd LIKE '%'+ @Mn_Cd +'%')
                            And (@Mn_Nm = '' OR @Mn_Nm IS NULL OR mn_nm LIKE '%'+ @Mn_Nm +'%')
                            And (@Mn_Full = '' OR @Mn_Full IS NULL OR mn_full LIKE '%'+ @Mn_Full +'%')
                            Order by mn_cd";
                var result = await base.DbConnection.QueryAsync<MenuInfo>(query, new { @Mn_Cd = mn_cd, @Mn_Nm = mn_nm, @Mn_Full = full_nm });
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<MenuInfo>> GetListMenuInfo()
        {
            try
            {
                var query = @"Select * From menu_info Order By mn_cd Desc, level_cd Desc";
                var result = await base.DbConnection.QueryAsync<MenuInfo>(query);
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<MenuInfo> GetMenuInfoById(int mnno)
        {
            try
            {
                var query = @"Select * From menu_info Where mnno = @Id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<MenuInfo>(query, new { @Id = mnno });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> InsertIntoMenuInfo(MenuInfo item)
        {
            try
            {
                var query = @"Insert into menu_info (mn_cd, mn_nm, up_mn_cd, level_cd, url_link, re_mark, col_css, sub_yn, order_no, use_yn, mn_full, mn_cd_full, reg_id, chg_id, reg_dt, chg_dt)
                            Values(@mn_cd, @mn_nm, @up_mn_cd, @level_cd, @url_link, @re_mark, @col_css, @sub_yn, @order_no, @use_yn, @mn_full, @mn_cd_full, @reg_id, @chg_id, @reg_dt, @chg_dt)
                            Select Scope_Identity()";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> RemoveMenuInfo(int mnno)
        {
            try
            {
                var query = @"Delete menu_info Where mnno = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Id = mnno});
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UpdateMenuInfo(MenuInfo item)
        {
            try
            {
                var query = @"Update menu_info SET mn_cd = @mn_cd, mn_nm = @mn_nm, up_mn_cd = @up_mn_cd, level_cd = @level_cd, url_link = @url_link, 
                            re_mark = @re_mark, col_css = @col_css, sub_yn = @sub_yn, order_no = @order_no, use_yn = @use_yn, mn_full = @mn_full,
                            mn_cd_full = @mn_cd_full, reg_id = @reg_id, chg_id = @chg_id, reg_dt = @reg_dt, chg_dt = @chg_dt Where mnno = @mnno";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}