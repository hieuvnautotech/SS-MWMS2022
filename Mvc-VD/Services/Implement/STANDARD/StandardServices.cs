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
using Mvc_VD.Models;

namespace Mvc_VD.Services.Implement
{
    public class StandardServices :DbConnection1RepositoryBase, IStandardServices
    {
        public StandardServices(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {
        }

        #region Standard Infomation

        #region Supplier Infomation
        public async Task<int> CheckSupplierInfo(string sp_cd)
        {
            try
            {
                var query = @"Select Count(*) from supplier_info where sp_cd = @Sp_Cd";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Sp_Cd = sp_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<CommCode>> GetListCountrỵ()
        {
            try
            {
                var query = @"Select * from comm_dt where mt_cd = 'COM001' And use_yn = 'Y' and active = 1 Order by dt_nm ";
                var result = await base.DbConnection.QueryAsync<CommCode>(query);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<SupplierInfo>> GetListSupplierInfo()
        {
            try
            {
                var query = @"Select a.spno,a.sp_cd,a.sp_nm,a.bsn_tp,a.chg_id,a.phone_nb,a.cell_nb,a.fax_nb,a.e_mail,a.web_site,a.address,a.re_mark,
                            (Select dt_nm from comm_dt where dt_cd =  a.bsn_tp And mt_cd = 'COM001') As bsn_tp1 
                            From supplier_info As a
                            Order by chg_dt Desc";
                var result = await base.DbConnection.QueryAsync<SupplierInfo>(query);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<SupplierInfo> GetSupplierInfoById(int spno)
        {
            try
            {
                var query = @"Select * from supplier_info where spno = @Id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<SupplierInfo>(query, new { @Id = spno });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> InsertSupplierInfo(SupplierInfo item)
        {
            try
            {
                var query = @"Insert Into supplier_info(sp_cd, sp_nm, bsn_tp, phone_nb, cell_nb, fax_nb, e_mail, web_site, address, 
                                re_mark, use_yn, del_yn, reg_id, reg_dt, chg_id, chg_dt)
                                Values(@sp_cd, @sp_nm, @bsn_tp, @phone_nb, @cell_nb, @fax_nb, @e_mail, @web_site, @address, 
                                    @re_mark, @use_yn, @del_yn, @reg_id, GetDate(), @chg_id, GETDATE())
                            Select scope_identity()";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> RemoveSupplierInfo(int spno)
        {
            try
            {
                var query = @"Delete supplier_info where spno = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Id = spno});
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<SupplierInfo>> SearchSupplierInfo(string codeData, string nameData, string bsn_searchData)
        {
            try
            {
                var query = @"SELECT si.*
                                FROM  supplier_info as si INNER JOIN comm_dt ON ( comm_dt.dt_cd= si.bsn_tp )
                                WHERE comm_dt.mt_cd='COM001' 
                                AND (@Code ='' OR @Code IS NULL OR  si.sp_cd like '%' + @Code + '%' )
                                AND (@Name ='' OR @Name IS NULL OR  si.sp_nm like '%' + @Name + '%' )
                                AND (@bsn_search = '' OR @bsn_search IS NULL OR  comm_dt.dt_cd = '@bsn_search')";
                var result = await base.DbConnection.QueryAsync<SupplierInfo>(query, new { @Code = codeData, @Name = nameData, @bsn_search = bsn_searchData });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateSupplierInfo(SupplierInfo item)
        {

            try
            {
                var query = @"Update supplier_info SET sp_cd = @sp_cd, sp_nm = @sp_nm, bsn_tp = @bsn_tp, phone_nb = @phone_nb,
                                cell_nb = @cell_nb, fax_nb = @fax_nb, e_mail = @e_mail, web_site = @web_site, address = @address, 
                                re_mark = @re_mark, use_yn = @use_yn, del_yn = @del_yn , reg_id = @reg_id, reg_dt = @reg_dt, 
                                chg_id = @chg_id, chg_dt = @chg_dt Where spno = @spno";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region Buyer
        public async Task<int> CheckBuyerInfo(string buyer_cd)
        {
            try
            {
                var query = @"Select Count(*) From buyer_info where buyer_cd = @Code";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Code = buyer_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<BuyerInfo> GetBuyerInfoById(int byno)
        {
            try
            {
                var query = @"Select * From buyer_info where byno = @Id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<BuyerInfo>(query, new { @Id = byno });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<BuyerInfo>> GetListBuyerInfo()
        {
            try
            {
                var query = @"Select * from buyer_info Order by chg_dt Desc";
                var result = await base.DbConnection.QueryAsync<BuyerInfo>(query);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> InsertIntoBuyerInfo(BuyerInfo item)
        {
            try
            {
                var query = @"Insert into buyer_info (buyer_cd, buyer_nm, ceo_nm, manager_nm, brd_nm, logo,phone_nb, cell_nb, fax_nb, e_mail, 
                                address, web_site, re_mark, use_yn, del_yn, stampid, reg_id, reg_dt, chg_id, chg_dt, active)
                            Values(@buyer_cd, @buyer_nm, @ceo_nm, @manager_nm, @brd_nm, @logo,@phone_nb, @cell_nb, @fax_nb, @e_mail, 
                                @address, @web_site, @re_mark, @use_yn, @del_yn, @stampid, @reg_id, @reg_dt, @chg_id, @chg_dt, @active)
                            Select Scope_Identity()";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<int> UpdateBuyerInfo(BuyerInfo item)
        {
            try
            {
                var query = @"Update buyer_info SET buyer_cd = @buyer_cd, buyer_nm = @buyer_nm, ceo_nm = @ceo_nm, manager_nm = @manager_nm, 
                                brd_nm = @brd_nm, logo = @logo,phone_nb = @phone_nb, cell_nb = @cell_nb, fax_nb = @fax_nb, e_mail = @e_mail, address = @address,
                                web_site = @web_site, re_mark = @re_mark, use_yn = @use_yn, del_yn = @del_yn, stampid = @stampid, reg_id = @reg_id,
                                reg_dt = @reg_dt, chg_id = @chg_id, chg_dt = @chg_dt, active = @active
                            Where byno = @byno";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> RemoveBuyerInfo(int byno)
        {
            try
            {
                var query = @" delete buyer_info where byno = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Id = byno });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<BuyerInfo>> SearchBuyerInfo(string buyer_cd, string buyer_nm)
        {
            try
            {
                var query = @"SELECT * 
                            FROM  buyer_info 
                            WHERE (@Buyer_cd_search ='' OR @Buyer_cd_search IS NULL OR   buyer_cd like '%' + @Buyer_cd_search + '%' ) 
                            AND (@Buyer_nm_search ='' OR @Buyer_nm_search IS NULL OR   buyer_nm like '%' + @Buyer_nm_search + '%' )";
                var result = await base.DbConnection.QueryAsync<BuyerInfo>(query, new { @Buyer_cd_search = buyer_cd, @Buyer_nm_search = buyer_nm });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        #endregion

        #region Supplier Type Infomation
        public async Task<int> CheckSupplierType(string dt_cd, string mt_cd)
        {
            try
            {
                var query = @"Select Count(*) From comm_dt Where dt_cd = @Dt_Cd And mt_cd = @Mt_Cd";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Dt_Cd = dt_cd, @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<CommCode>> GetListSupplierType(string mt_cd)
        {
            try
            {
                var query = @"Select d.cdid, d.dt_cd, d.dt_nm, d.dt_exp, d.use_yn, d.chg_dt 
                            From comm_dt d Join comm_mt m on d.mt_cd = m.mt_cd  
                            Where m.mt_cd = @Mt_Cd
                            Order by d.chg_dt Desc";
                var result = await base.DbConnection.QueryAsync<CommCode>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<CommCode> GetSupplierTypeById(int cdid)
        {
            try
            {
                var query = @"Select * From comm_dt where cdid = @Id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<CommCode>(query, new { @Id = cdid });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> RemoveSupplierType(int cdid)
        {
            try
            {
                var query = @"Delete comm_dt Where cdid = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Id = cdid });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<CommCode>> SearchSupplierType(string dt_cd, string dt_nm)
        {
            try
            {
                var query = @"SELECT * 
                                    FROM  comm_dt 
                                    WHERE mt_cd = 'COM001' 
                                    AND (@dt_cd_search = '' OR @dt_cd_search IS NULL OR   dt_cd like '%' + @dt_cd_search + '%' )
                                    AND (@dt_nm_search = '' OR @dt_nm_search IS NULL OR   dt_nm like '%' + @dt_nm_search + '%' )";
                var result = await base.DbConnection.QueryAsync<CommCode>(query, new { @dt_cd_search = dt_cd, @dt_nm_search = dt_nm });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateSupplierType(CommCode item)
        {
            try
            {
                var query = @"Update comm_dt SET mt_cd = @mt_cd, dt_cd = @dt_cd, dt_nm = @dt_nm, dt_exp = @dt_exp, dt_order = @dt_order, 
                            use_yn = @use_yn, del_yn = @del_yn, reg_id = @reg_id, reg_dt = @reg_dt, chg_id = @chg_id, chg_dt = @chg_dt
                            Where cdid = @cdid";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> ÍnertIntoSupplierType(CommCode item)
        {
            try
            {
                var query = @"Insert into comm_dt (mt_cd, dt_cd, dt_nm, dt_exp, dt_order, use_yn, del_yn, reg_id, reg_dt, chg_id, chg_dt)
                            Values (@mt_cd, @dt_cd, @dt_nm, @dt_exp, @dt_order, @use_yn, @del_yn, @reg_id, @reg_dt, @chg_id, @chg_dt)
                            select scope_identity()";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        #endregion

        #endregion

        #region System Management

        #region Authority Management
        public async Task<IEnumerable<AuthorInfo>> GetListAuthorInfo(string code, string name)
        {
            try
            {
                var query = @"Select a.atno, a.at_cd, a.at_nm, a.use_yn, a.reg_id, a.reg_dt, a.chg_id, a.chg_dt, a.re_mark,a.role,
                            (Select dt_nm from comm_dt where dt_cd = a.role and mt_cd = 'MMS014') As role_nm
                            From author_info As a 
                            Where use_yn = 'Y' And active = 1
                            AND (@Code = '' OR @Code IS NULL OR a.at_cd Like '%' + @Code + '%')
                            AND (@Name = '' OR @Name IS NULL OR a.at_nm Like '%' + @Name + '%')";

                var result = await base.DbConnection.QueryAsync<AuthorInfo>(query, new { @Code = code, @Name = name });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<MbInfo>> GetListMbInfo(string userid, string uname)
        {
            try
            {
                var query = @"Select * from mb_info As a
                            where a.lct_cd != 'staff' and a.lct_cd != '' and a.active = 1
                            AND(@Code = '' OR @Code IS NULL OR a.userid Like '%' + @Code + '%')
                            AND(@Name = '' OR @Name IS NULL OR a.uname Like '%' + @Name + '%')
                            Order by a.uname";


                var result = await base.DbConnection.QueryAsync<MbInfo>(query, new { @Code = userid, @Name = uname });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<MenuInfo>> GetListMenuInfo(string mn_cd, string mn_full, string at_cd)
        {
            try
            {
                var query = @"Select a.*, b.role From menu_info As a
                                left join author_menu_info as b on a.mn_cd = b.mn_cd and b.at_cd = @at_cd 

                            Where a.use_yn = 'Y' And a.url_link != '' And a.mn_cd != '011001001000'
                
                            AND (@Menu_Code = '' OR @Menu_Code IS NULL OR a.mn_cd Like '%' + @Menu_Code + '%')
                            AND (@Menu_Full = '' OR @Menu_Full IS NULL OR a.mn_full Like '%' + @Menu_Full + '%')
                            Order by a.mn_cd";
                var result = await base.DbConnection.QueryAsync<MenuInfo>(query, new { @Menu_Code = mn_cd, @Menu_Full = mn_full , at_cd = at_cd });
                return result.ToList();
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> RemoveMenuInfo(int Id)
        {
            try
            {
                var query = @"Delete menu_info where mnno = @MNNO";
                var result = await base.DbConnection.ExecuteAsync(query, new { @MNNO = Id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> RemoveMbMenuInfo(int Id)
        {
            try
            {
                var query = @"Delete mb_author_info where mano = @MaNo";
                var result = await base.DbConnection.ExecuteAsync(query, new { @MaNo = Id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<AuthorMenuInfo>> GetListAuthorMenuInfo(string at_cd)
        {
            try
            {
                var query = @"Select a.* from author_menu_info as a  where a.at_cd = @Code ";
                var result = await base.DbConnection.QueryAsync<AuthorMenuInfo>(query, new { @Code = at_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<CommCode>> GetRole()
        {
            try
            {
                var query = @"Select * from comm_dt where mt_cd = 'MMS014' ";
                var result = await base.DbConnection.QueryAsync<CommCode>(query);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<MbAuthorInfo>> GetMbAuthorInfoByCode(string at_cd)
        {
            try
            {
                var query = @"Select * From mb_author_info where at_cd = @Code";
                var result = await base.DbConnection.QueryAsync<MbAuthorInfo>(query, new { @Code = at_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<string> GetNameFromAuthorInfo(string at_cd)
        {
            try
            {
                var query = @"Select at_nm From author_info where at_cd = @Code";
                var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @Code = at_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> CountMbAuthorInfo(string userid)
        {
            try
            {
                var query = @"Select Count(*) From mb_author_info where userid = @Id";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Id = userid });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<MbAuthorInfo> GetMbAuthorInfo(string userid)
        {
            try
            {
                var query = @"Select * From mb_author_info where userid = @Id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<MbAuthorInfo>(query, new { @Id = userid });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<List<menu_info>> getmenuinfo(string at_cd, string kq)
        {
            var query = @"
                            SELECT *
                            FROM menu_info
                            WHERE mn_cd NOT IN (Select mn_cd From author_menu_info Where at_cd = @At_Cd And mn_cd In (@KQ) )";

            var result = await base.DbConnection.QueryAsync<menu_info>(query, new { @At_Cd = at_cd,@KQ = kq });
            return result.ToList();
        }
        //public async Task<int> InsertIntoAuthorMenuInfo(string at_cd, DateTime time, string kq)
        //{
        //    try
        //    {
        //        var query = @"INSERT INTO author_menu_info (at_cd, mn_cd, mn_nm,url_link,st_yn,ct_yn,mt_yn,del_yn,reg_dt,chg_dt)
        //                    SELECT @At_Cd , mn_cd, mn_nm,url_link,'Y','N','N','N', GETDATE(), GETDATE()
        //                    FROM menu_info
        //                    WHERE mn_cd NOT IN (Select mn_cd From author_menu_info Where at_cd = @At_Cd And mn_cd In (@KQ) )";

        //        var result = await base.DbConnection.ExecuteAsync(query, new { @At_Cd = at_cd, @Time = time, @KQ = kq });
        //        return result;
        //    }
        //    catch (Exception e)
        //    {

        //        throw e;
        //    }
        //}
        public async Task<int> InsertIntoAuthorMenuInfo(string at_cd, DateTime time, string role, string mn_cd)
        {
            try
            {
                var query = @"INSERT INTO author_menu_info (at_cd, mn_cd, mn_nm,url_link,st_yn,ct_yn,mt_yn,del_yn,reg_dt,chg_dt,role)
                            SELECT @At_Cd , mn_cd, mn_nm,url_link,'Y','N','N','N', GETDATE(), GETDATE(),@role
                            FROM menu_info
                            WHERE mn_cd  = @mnCd";

                var result = await base.DbConnection.ExecuteAsync(query,
                    new { @At_Cd = at_cd,
                        @Time = time,
                        role = role ,
                        mnCd = mn_cd
                    });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> RemoveAuthorMenuInfo(string bo_check, string at_cd)
        {
            try
            {
                var result = 0;
                var listData = bo_check.Split(',');
                foreach (var item in listData)
                {
                    var query = @"DELETE author_menu_info WHERE mn_cd = @BoCheck And at_cd = @At_Cd";
                    result += await base.DbConnection.ExecuteAsync(query, new { @BoCheck = item, @At_Cd = at_cd });
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<int> RemoveAuthorMenuInfoWithAtCd(string at_cd)
        {
            try
            {
                var query = @"DELETE author_menu_info WHERE at_cd = @At_Cd";
                return await base.DbConnection.ExecuteAsync(query, new { @At_Cd = at_cd });
                //return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<int> CountAuthorInfo(string at_nm)
        {
            try
            {
                var query = @"Select Count(*) From author_info where at_nm = @Name";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Name = at_nm });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> InsertIntoAuthorInfo(AuthorInfo item)
        {
            try
            {
                var query = @"Insert into author_info (at_cd, at_nm, role, use_yn, reg_id, reg_dt, chg_id, chg_dt, re_mark)
                            Values (@at_cd, @at_nm, @role, @use_yn, @reg_id, @reg_dt, @chg_id, @chg_dt, @re_mark)
                            Select scope_identity()";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<AuthorInfo> GetAuthorInfoById(int Id)
        {
            try
            {
                var query = @"Select a.atno, a.at_cd, a.at_nm, a.use_yn, a.reg_id, a.reg_dt, a.chg_id, a.chg_dt, a.re_mark,a.role,
                            (Select dt_nm from comm_dt where dt_cd = a.role and mt_cd = 'MMS014') As role_nm
                            From author_info As a
                            Where atno = @Id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<AuthorInfo>(query, new { @Id = Id });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<AuthorInfo> GetAuthorInfoByAt_Code(string at_cd)
        {
            try
            {
                var query = @"Select a.atno, a.at_cd, a.at_nm, a.use_yn, a.reg_id, a.reg_dt, a.chg_id, a.chg_dt, a.re_mark,a.role,
                            (Select dt_nm from comm_dt where dt_cd = a.role and mt_cd = 'MMS014') As role_nm
                            From author_info As a
                            Where at_cd = @Code";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<AuthorInfo>(query, new { @Code = at_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> RemoveAuthorInfo(int Id)
        {
            try
            {
                var query = @"Delete author_info Where atno = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Id = Id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> RemoveAuthorMenuInfo(int Id)
        {
            try
            {
                var query = @"Delete author_menu_info Where amno = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Id = Id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> RemoveMbAuthorInfo(int Id)
        {
            try
            {
                var query = @"Delete mb_author_info where mano = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Id = Id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateAuthorInfo(AuthorInfo item)
        {
            try
            {
                var query = @"Update author_info SET at_cd = @at_cd, at_nm = @at_nm, role = @role, use_yn = @use_yn, 
                              reg_id = @reg_id, reg_dt = @reg_dt, chg_id = @chg_id, chg_dt = @chg_dt, re_mark =@re_mark
                              Where atno = @atno";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        #endregion

        #region Common Management
        public async Task<IEnumerable<CommMt>> GetListCommonMT()
        {
            try
            {
                var query = @"Select mt_id, mt_cd, mt_nm, mt_exp, use_yn, chg_dt From comm_mt where mt_cd Like'COM%' Order by mt_cd";
                var result = await base.DbConnection.QueryAsync<CommMt>(query);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> CountCommMTByName(string mt_name)
        {
            try
            {
                var query = @"Select count(*) From comm_mt where mt_nm = @Name";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Name = mt_name });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> CountCommMtByCode()
        {
            try
            {
                var query = @"Select Count(*) From comm_mt where mt_cd Like 'COM%'";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<CommMt>> GetListCommMtByCode()
        {
            try
            {
                var query = @"Select * From comm_mt where mt_cd Like 'COM%' Order by mt_cd";
                var result = await base.DbConnection.QueryAsync<CommMt>(query);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<GetDataCommDtResponse>> GetListDataCommDt(string mt_cd)
        {
            try
            {
                var query = @"Select d.cdid, m.mt_cd, m.mt_nm, d.dt_cd, d.dt_nm, d.dt_exp, d.dt_order, d.use_yn, d.chg_dt 
                            From comm_dt d Join comm_mt m On d.mt_cd = m.mt_cd
                            Where d.mt_cd = @ModelCode";
                var result = await base.DbConnection.QueryAsync<GetDataCommDtResponse>(query, new { @ModelCode = mt_cd});
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> RemoveCommMt(int mt_id)
        {
            try
            {
                var query = @"Delete comm_mt Where mt_id = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Id = mt_id});
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<CommCode>> GetListCommDT(string mt_cd)
        {
            var query = @"Select * From comm_dt Where mt_cd = @ModelCode";
            var result = await base.DbConnection.QueryAsync<CommCode>(query, new { @ModelCode = mt_cd });
            return result;
        }

        public async Task<int> RemoveCommDT(int cdid)
        {
            try
            {
                var query = @"Delete comm_dt Where cdid = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Id = cdid });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<CommCode> GetCommCodeById(string dt_cd, string mt_cd)
        {
            try
            {
                var query = @"Select * From comm_dt where dt_cd = @DetailCode And mt_cd = @Code";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<CommCode>(query, new { @DetailCode = dt_cd, @Code = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region Manual Management

        public async Task<NoticeBoard> GetNoticeBoardById(int bno)
        {
            try
            {
                var query = @"Select * From notice_board where bno = @ID";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<NoticeBoard>(query, new { @ID = bno });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<IEnumerable<NoticeBoard>> GetListNoticeBoard()
        {
            try
            {
                var query = @"Select * From notice_board where mn_cd IS NOT NULL";
                var result = await base.DbConnection.QueryAsync<NoticeBoard>(query);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public async Task<IEnumerable<NoticeBoard>> SearchDataNoticeBoard1(string title, string mn_cd)
        {
            try
            {
                var query = @"Select * From notice_board 
				            Where mn_cd IS NOT NULL 
				            And (@MenuCode = '' OR  @MenuCode IS NULL OR mn_cd Like '%' + @MenuCode + '%')
				            And (@Title = '' OR  @Title IS NULL OR title Like '%' + @Title + '%')";
                var result = await base.DbConnection.QueryAsync<NoticeBoard>(query, new { @MenuCode = mn_cd, @Title = title});
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<NoticeBoard>> SearchDataNoticeBoard2(string title, string mn_cd, string lng_cd)
        {
            try
            {
                var query = @"Select * From notice_board 
				            Where mn_cd IS NOT NULL 
				            And (@MenuCode = '' OR  @MenuCode IS NULL OR mn_cd Like '%' + @MenuCode + '%')
				            And (@Title = '' OR  @Title IS NULL OR title Like '%' + @Title + '%')
				            And (@Lang = '' OR  @Lang IS NULL OR lng_cd Like '%' + @Lang + '%')";
                var result = await base.DbConnection.QueryAsync<NoticeBoard>(query, new { @MenuCode = mn_cd, @Title = title, @Lang = lng_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> CheckDataNoticeBoard(string mn_cd, string lng_cd)
        {
            try
            {
                var query = @"Select Count(*) From notice_board where mn_cd = @Menu_Code and lng_cd = @Lang_Code";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Menu_Code = mn_cd, @Lang_Code = lng_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> InsertIntoNoticeBoard(NoticeBoard item)
        {
            try
            {
                var query = @"Insert Into notice_board (title, content, mn_cd, viewcnt, replycnt, div_cd, lng_cd, start_dt, end_dt, widthsize, heightsize, back_color, order_no, del_yn, reg_id, reg_dt, chg_id, chg_dt, active)
					        Values (@title, @content, @mn_cd, @viewcnt, @replycnt, @div_cd, @lng_cd, @start_dt, @end_dt, @widthsize, @heightsize, @back_color, @order_no, @del_yn, @reg_id, @reg_dt, @chg_id, @chg_dt, @active)
					        Select scope_identity()";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<int> UpdateNoticeBoard(NoticeBoard item)
        {
            try
            {
                var query = @"Update notice_board Set title = @title, content = @content, mn_cd = @mn_cd, viewcnt = @viewcnt, replycnt = @replycnt, div_cd = @div_cd, lng_cd = @lng_cd, 
                            start_dt = @start_dt, end_dt = @end_dt, widthsize = @widthsize, heightsize = @heightsize, back_color = @back_color, order_no = @order_no, del_yn = @del_yn, 
                            reg_id = @reg_id, reg_dt = @reg_dt, chg_id =  @chg_id, chg_dt = @chg_dt, active =  @active 
                            Where bno = @bno";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<IEnumerable<NoticeBoard>> GetListDataNoticeBoard(string mn_cd, string lng_cd)
        {
            try
            {
                var query = @"Select * From notice_board where mn_cd = @Menu_Code and lng_cd = @Lang_Code";
                var result = await base.DbConnection.QueryAsync<NoticeBoard>(query, new { @Menu_Code = mn_cd, @Lang_Code = lng_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        public async Task<int> DeleteNoticeBoard(int bno)
        {
            try
            {
                var query = @"Delete notice_board where bno = @ID";
                var result = await base.DbConnection.ExecuteAsync(query, new { @ID = bno });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<NoticeBoard>> GetListNoticeBoardByMenuCode(string mn_cd)
        {
            try
            {
                var query = @"Select * From notice_board where mn_cd = @Menu_Code";
                var result = await base.DbConnection.QueryAsync<NoticeBoard>(query, new { @Menu_Code = mn_cd });
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