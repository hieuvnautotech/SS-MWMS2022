using Mvc_VD.Models.NewVersion;
using Mvc_VD.Respositories.Irepository;
using Mvc_VD.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using Mvc_VD.Models.Response;

namespace Mvc_VD.Services.Implement
{
    public class StandardManagementWO : DbConnection1RepositoryBase, IStandardManagementWO
    {
        public StandardManagementWO(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {

        }

        #region Standard Management (WO)

        #region WO Common
        public async Task<int> checkCommDT(string mt_cd, string dt_cd)
        {
            try
            {
                var query = @"Select Count(*) From comm_dt where mt_cd = @Mt_Cd And dt_cd = @Dt_Cd";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Mt_Cd = mt_cd, @Dt_Cd = dt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<CommCode> GetCommDTById(int cdid)
        {
            try
            {
                var query = @"Select * from comm_dt where cdid = @Id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<CommCode>(query, new { @Id = cdid });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<CommMt>> GetListCommMT()
        {
            try
            {
                var query = @"Select * from comm_mt where div_cd = 'MMS' And mt_cd Like 'MMS%' ";
                var result = await base.DbConnection.QueryAsync<CommMt>(query);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<CommMt> GetCommMTById(int mt_id)
        {
            try
            {
                var query = @"Select * from comm_mt where mt_id = @Id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<CommMt>(query, new { @Id = mt_id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> CheckCommMT()
        {
            try
            {
                var query = @"Select Count(*) from comm_mt where div_cd = 'MMS'";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<WOCommonResponse>> GetListComMTDetail(string mt_cd)
        {
            try
            {
                var query = @"Select d.cdid, m.mt_cd, m.mt_nm, d.dt_cd, d.dt_nm, d.dt_exp, d.dt_order, d.use_yn
                              From comm_dt d Join comm_mt m on m.mt_cd = d.mt_cd where d.mt_cd = @Mt_Cd";
                var result = await base.DbConnection.QueryAsync<WOCommonResponse>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<CommMt>> SearchCommMT(string mt_cd, string mt_nm, string mt_exp)
        {
            try
            {
                var query = @"SELECT * FROM  comm_mt 
				            WHERE (@Mt_Cd = '' OR  @Mt_Cd IS NULL OR  mt_cd like '%' + @Mt_Cd + '%' )
				            AND (@Mt_Nm = '' OR  @Mt_Nm IS NULL OR  mt_nm like '%' + @Mt_Nm + '%' )
				            AND (@Mt_Exp = '' OR  @Mt_Exp IS NULL OR mt_exp like '%' + @Mt_Exp + '%' )
				            AND div_cd='MMS'";
                var result = await base.DbConnection.QueryAsync<CommMt>(query, new { @Mt_Cd = mt_cd, @Mt_Nm = mt_nm, @Mt_Exp = mt_exp });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> InsertIntoCommDT(CommCode item)
        {
            try
            {
                var query = @"Insert Into comm_dt (mt_cd, dt_cd, dt_nm, dt_exp, dt_order, use_yn, reg_dt, chg_dt, del_yn)
                            Values(@mt_cd, @dt_cd, @dt_nm, @dt_exp, @dt_order, @use_yn, @reg_dt, @chg_dt, @del_yn)";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateCommDT(CommCode item)
        {
            try
            {
                var query = @"Update comm_dt SET dt_nm = @dt_nm, dt_exp = @dt_exp, dt_order = @dt_order, use_yn = @use_yn, reg_dt = @reg_dt, chg_dt = @chg_dt
                              Where cdid = @cdid";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> InsertIntoCommMT(CommMt item)
        {
            try
            {
                var query = @"Insert Into comm_mt (mt_cd, div_cd, mt_nm, mt_exp, use_yn, reg_dt, chg_dt)
                               Values(@mt_cd, @div_cd, @mt_nm, @mt_exp, @use_yn, @reg_dt, @chg_dt)";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateCommMT(CommMt item)
        {
            try
            {
                var query = @"Update comm_mt SET mt_cd = @mt_cd, div_cd = @div_cd, mt_nm = @mt_nm, mt_exp = @mt_exp, use_yn = @use_yn, reg_dt = @reg_dt, chg_dt = @chg_dt
                              Where mt_id = @mt_id";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> DeleteCommMT(int mt_id)
        {
            try
            {
                var query = @"Delete comm_mt where mt_id = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Id = mt_id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> DeleteCommDT(int cdid)
        {
            try
            {
                var query = @"Delete comm_dt where cdid = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Id = cdid });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region Work Policy
        public async Task<IEnumerable<PolicyMT>> GetListPolicyMT()
        {
            try
            {
                var query = @"Select * from w_policy_mt";
                var result = await base.DbConnection.QueryAsync<PolicyMT>(query);
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> CountListPolicyMT()
        {
            try
            {
                var query = @"Select Count(*) From w_policy_mt";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> InsertIntoPolicyMT(PolicyMT item)
        {
            try
            {
                var query = @"Insert into w_policy_mt (policy_code, policy_name, work_starttime, work_endtime, lunch_start_time, lunch_end_time, dinner_start_time, dinner_end_time, work_hour,
                                use_yn, re_mark, reg_id, chg_id, reg_dt, chg_dt, policy_start_dt, policy_end_dt, last_yn)
                                Values (@policy_code, @policy_name, @work_starttime, @work_endtime, @lunch_start_time, @lunch_end_time, @dinner_start_time, @dinner_end_time, @work_hour,
                                @use_yn, @re_mark, @reg_id, @chg_id, @reg_dt, @chg_dt, @policy_start_dt, @policy_end_dt, @last_yn)";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<PolicyMT> GetPolicyMTById(int wid)
        {
            try
            {
                var query = @"Select * From w_policy_mt where wid = @Id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<PolicyMT>(query, new { @Id = wid});
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> DeletePolicyMTById(int wid)
        {
            try
            {
                var query = @"Delete w_policy_mt where wid = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Id = wid });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdatePolicy(PolicyMT item)
        {
            try
            {
                var query = @"Update w_policy_mt SET policy_code = @policy_code, policy_name = @policy_name, work_starttime = @work_starttime, work_endtime = @work_endtime, 
                                lunch_start_time = @lunch_start_time, lunch_end_time = @lunch_end_time, dinner_start_time = @dinner_start_time, dinner_end_time = @dinner_end_time, 
                                work_hour = @work_hour, use_yn = @use_yn, re_mark = @re_mark, reg_id = @reg_id, chg_id = @chg_id, reg_dt = @reg_dt, chg_dt = @chg_dt, policy_start_dt = @policy_start_dt, 
                                policy_end_dt = @policy_end_dt, last_yn = @last_yn
                             Where wid = @wid";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #endregion

        #region WIP

        #region Inverntory
        public async Task<IEnumerable<LocationInfo>> GetListLocationInfo()
        {
            try
            {
                var query = @"Select * From lct_info Where lct_cd Like '002%' Order By lct_cd ";
                var result = await base.DbConnection.QueryAsync<LocationInfo>(query);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<LocationInfo>> SearchLocationInfọ(string lct_cd, string lct_nm)
        {
            try
            {
                var query = @"Select * From lct_info 
                            Where lct_cd Like '002%' 
                            And(@Name = '' OR @Name IS NULL OR lct_nm Like '%'+ @Name +'%') 
                            And (@Code = '' OR @Code IS NULL OR lct_cd Like '%'+ @Code +'%')
                            Order By lct_cd,level_cd";
                var result = await base.DbConnection.QueryAsync<LocationInfo>(query, new{ @Code = lct_cd, @Name = lct_nm });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<LocationInfo> GetDataLocationInfoById(int lct_no)
        {
            try
            {
                var query = @"Select * From lct_info where lctno = @Id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<LocationInfo>(query, new { @Id = lct_no});
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<LocationInfo>> GetListLocationInfoByCode(string lct_cd)
        {
            try
            {
                var query = @"Select * From lct_info where lct_cd = @Code";
                var result = await base.DbConnection.QueryAsync<LocationInfo>(query, new { @Code = lct_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UpdateDataLocationInfo(LocationInfo item)
        {
            try
            {
                var query = @"Update lct_info SET lct_cd = @lct_cd, lct_nm = @lct_nm, up_lct_cd = @up_lct_cd, level_cd = @level_cd, index_cd = @index_cd, shelf_cd  = @shelf_cd, order_no = @order_no,
                            real_use_yn = @real_use_yn, re_mark = @re_mark, use_yn = @use_yn, lct_rfid = @lct_rfid, lct_bar_cd = @lct_bar_cd, sf_yn = @sf_yn, is_yn = @is_yn, mt_yn = @mt_yn, mv_yn = @mv_yn,
                            ti_yn = @ti_yn, fg_yn = @fg_yn, rt_yn = @rt_yn, ft_yn = @ft_yn, wp_yn = @wp_yn, nt_yn = @nt_yn, pk_yn = @pk_yn, reg_id = @reg_id, reg_dt = @reg_dt, chg_id = @chg_id, 
                            chg_dt = @chg_dt, mn_full = @mn_full
                            Where lctno = @lctno";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> InsertIntoDataLocationInfo(LocationInfo item)
        {
            try
            {
                var query = @"Insert Into lct_info (lct_cd, lct_nm , up_lct_cd, level_cd, index_cd, shelf_cd , order_no,real_use_yn, re_mark, use_yn, lct_rfid, lct_bar_cd, sf_yn, is_yn , mt_yn , 
                            mv_yn, ti_yn, fg_yn, rt_yn, ft_yn, wp_yn, nt_yn, pk_yn, reg_id, reg_dt, chg_id, chg_dt, mn_full)
                            Values (@lct_cd, @lct_nm , @up_lct_cd, @level_cd, @index_cd, @shelf_cd, @order_no, @real_use_yn, @re_mark, @use_yn, @lct_rfid, @lct_bar_cd, @sf_yn, @is_yn ,@mt_yn, 
                            @mv_yn, @ti_yn, @fg_yn, @rt_yn, @ft_yn, @wp_yn, @nt_yn, @pk_yn, @reg_id, @reg_dt, @chg_id, @chg_dt, @mn_full)
                            select scope_identity()";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> DeleteDataLocationInfoById(int lctno)
        {
            try
            {
                var query = @"Delete lct_info Where lctno = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Id = lctno});
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public async Task<IEnumerable<LocationInfo>> GetListLocationInfo(string lct_cd)
        {
            try
            {
                var query = @"Select * From lct_info where lct_cd Like '002%' And level_cd = '001' And lct_cd != @Code";
                var result = await base.DbConnection.QueryAsync<LocationInfo>(query, new { @Code = lct_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion

        #endregion
    }
}