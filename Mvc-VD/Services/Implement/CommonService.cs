using Mvc_VD.Respositories.Irepository;
using Mvc_VD.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using Mvc_VD.Models.WIP;
using System.Threading.Tasks;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models;
using Mvc_VD.Models.TIMS;

namespace Mvc_VD.Services.Implement
{
    public class CommonService : DbConnection1RepositoryBase, IcommonService
    {
        public CommonService(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {

        }
        public async Task<IReadOnlyList<WMaterialInfo>> GetListMAterialNo(string mt_no, string style_no)
        {
            string sql = @"SELECT max(mt.mtid) AS id, MAX(mt.mt_no)AS mt_no,max(mt.mt_nm) as mt_nm, max(bom.style_no) AS style_no , '' as meter
                            FROM d_material_info AS mt
                            left JOIN d_bom_info AS bom
                            ON mt.mt_no = bom.mt_no
                            WHERE mt.barcode = 'Y' AND mt.mt_type !='MMT'
                            AND  (@MtNo ='' OR @MtNo is null OR  mt.mt_no like '%'+@MtNo+'%' ) and
                         (@StyleNo ='' OR @StyleNo is null OR  bom.style_no like '%'+@StyleNo+'%' )
                            GROUP BY mt.mt_no";
            var result = await base.DbConnection.QueryAsync<WMaterialInfo>(sql, new { MtNo = mt_no, StyleNo = style_no });
            return result.ToList();
        }
        public async Task<IEnumerable<SdInfos>> GetListSDInfo(string SdNo,string SdName,string ProductCode,string remark)
        {
            try
            {
                string sql = @"SELECT
	            a.*,
	            ( SELECT dt_nm FROM comm_dt WHERE mt_cd = 'WHS005' AND dt_cd = a.status ) AS sts_nm,
	            ( SELECT lct_nm FROM lct_info WHERE lct_cd = a.lct_cd ) AS lct_nm 
            FROM w_sd_info AS a 
            WHERE
	            a.use_yn = 'Y'  and a.active=1
	            AND ( @sdno = '' OR @sdno is null  OR a.sd_no LIKE '%'+@sdno+'%' ) 
	            AND ( @sdnm = '' OR @sdnm is null OR a.sd_nm LIKE '%'+@sdnm+'%' ) 
	            AND ( @procd = '' OR @procd is null OR a.product_cd LIKE '%'+@procd+'%' ) 
	            AND ( @remark = '' OR @remark is null OR a.remark LIKE '%'+@remark+'%' ) 
            ORDER BY sid DESC";
                var result = await base.DbConnection.QueryAsync<SdInfos>(sql, new { sdno = SdNo, sdnm = SdName, procd = ProductCode, remark = remark });
                return result.ToList();
            }
            catch (Exception e)
            {

                throw;
            }
           
        }


        public async Task<int> CountListSDInfo(string SdNo, string SdName, string ProductCode)
        {
            string sql = @"SELECT Count(a.sid) FROM w_sd_info AS a 
                        WHERE
	                        a.use_yn = 'Y'  and a.active=1
	                        AND ( @sdno = '' OR @sdno is null  OR a.sd_no LIKE '%'+@sdno+'%' ) 
	                        AND ( @sdnm = '' OR @sdnm is null OR a.sd_nm LIKE '%'+@sdnm+'%' ) 
	                        AND ( @procd = '' OR @procd is null OR a.product_cd LIKE '%'+@procd+'%') 
                        ORDER BY sid DESC";
            var result = await base.DbConnection.ExecuteScalarAsync<int>(sql, new { sdno = SdNo, sdnm = SdName, procd = ProductCode });
            return result;
        }


        public async Task<int> InsertSdInfo(SdInfo item)
        {
            try
            {
                string sql = @"INSERT INTO w_sd_info(sd_no,sd_nm,status,product_cd,lct_cd,alert,remark,use_yn,del_yn,reg_id,chg_id)
                            VALUES (@sd_no,@sd_nm,@status,@product_cd,@lct_cd,@alert,@remark,@use_yn,@del_yn,@reg_id,@chg_id)";
                return await base.DbConnection.ExecuteAsync(sql, item);
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public async Task<string> GetLastSdNo()
        {
            string sql = @"Select Top 1 sd_no From w_sd_info Order by sid Desc";
            var result = await base.DbConnection.ExecuteScalarAsync<string>(sql);
            return result;
        }
        public async Task<IReadOnlyList<lct_info>> GetLocationFactory()
        {
            string sql = @"Select * from lct_info where lct_cd  like '002%' and level_cd='002'";
            var result = await base.DbConnection.QueryAsync<lct_info>(sql);
            return result.ToList();
        }
        public async Task<IReadOnlyList<ListPickingScan>> GetListPickingScan(string SdNo,string SdNm,string ProductCode, int intpage, int introw)
        {
            try
            {
                string sql = @" SELECT a.*, 
		                (select dt_nm  from comm_dt where mt_cd='WHS005' and dt_cd = a.status) as sts_nm, 
		                (select lct_nm  from lct_info where lct_cd = a.lct_cd) as lct_nm 
		                FROM w_sd_info as a 
		                WHERE a.use_yn ='Y' and (a.alert > 0 OR  a.status <> '000' ) 
		                AND (@Sd_No = '' OR @Sd_No IS NULL OR a.sd_no Like '%' + @Sd_No + '%' )
		                AND (@Sd_Nm ='' OR @Sd_Nm IS NULL OR  a.sd_nm Like '%' + @Sd_Nm + '%' )
		                AND (@Product_Cd = '' OR @Product_Cd IS NULL OR a.product_cd Like '%' + @Product_Cd + '%' )
		                order by sid desc
OFFSET @intpage ROWS FETCH NEXT @introw ROWS ONLY";
                var result = await base.DbConnection.QueryAsync<ListPickingScan>(sql, new { @Sd_No = SdNo, @Sd_Nm = SdNm, @Product_Cd = ProductCode , @intpage= intpage, @introw= introw });
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public int TottalRowPickingScan(string SdNo, string SdNm, string ProductCode)
        {
            try
            {
                string sql = @" SELECT Count(a.sid) 
		                FROM w_sd_info as a 
		                WHERE a.use_yn ='Y' and (a.alert > 0 OR  a.status <> '000' ) 
		                AND (@Sd_No = '' OR @Sd_No IS NULL OR a.sd_no Like '%' + @Sd_No + '%' )
		                AND (@Sd_Nm ='' OR @Sd_Nm IS NULL OR  a.sd_nm Like '%' + @Sd_Nm + '%' )
		                AND (@Product_Cd = '' OR @Product_Cd IS NULL OR a.product_cd Like '%' + @Product_Cd + '%' )";
                return base.DbConnection.QueryFirstOrDefault<int>(sql, new { @Sd_No = SdNo, @Sd_Nm = SdNm, @Product_Cd = ProductCode});
                //return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public async Task<TmpWMaterialInfo> GetTmpMaterialInfo(string MtNo)
        {
            string sql = @"SELECT * FROM tmp_w_material_info where charindex(mt_no,@mtno)>0 AND active=1";
            return await base.DbConnection.QueryFirstOrDefaultAsync<TmpWMaterialInfo>(sql, new { mtno = MtNo });
        }
        public async Task<string> CheckSDInfo(string SdNo)
        {
            string sql = @"SELECT TOP 1 sd_no FROM w_sd_info WHERE sd_no = @sdno AND active = 1 ORDER BY sid DESC";
            string ressdno = await base.DbConnection.QueryFirstOrDefaultAsync<string>(sql, new { sdno = SdNo });
            return ressdno;

        }
        public async Task<int> InsertInventoryProduct(InventoryProduct item)
        {
            try
            {
                string sql = @"INSERT INTO inventory_products(material_code,recei_wip_date,sd_no,mt_no,mt_type,gr_qty,real_qty,recei_date,expiry_date,export_date,date_of_receipt,lot_no,from_lct_cd,location_code,status,create_id,change_id,create_date,change_date,lct_sts_cd,ShippingToMachineDatetime)
                VALUES (@material_code,@recei_wip_date,@sd_no,@mt_no,@mt_type,@gr_qty,@real_qty,@recei_date,@expiry_date,@export_date,@date_of_receipt,@lot_no,@from_lct_cd,@location_code,@status,@create_id,@change_id,getdate(),getdate(),@lct_sts_cd,getdate())";
               var result = await base.DbConnection.ExecuteAsync(sql, item);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }

        }
        public async Task<InventoryProduct> CheckExistMaterialProduct(string MtCode)
        {
            string sql = @"SELECT sd_no, create_date FROM inventory_products WHERE material_code=@mtcd";
            var checkmtcd = await base.DbConnection.QueryFirstOrDefaultAsync<InventoryProduct>(sql, new { mtcd = MtCode });
            return checkmtcd;
        }

        public async Task<bool> CheckMaterialNoShipp(string MtNo, string SdNo)
        {
            string sql = "SELECT a.id FROM shippingsdmaterial AS a WHERE a.mt_no = @mtno and a.sd_no = @sdno";
            string id = await base.DbConnection.QueryFirstOrDefaultAsync<string>(sql, new { mtno = MtNo, sdno = SdNo });
            return string.IsNullOrEmpty(id);
        }

        public async Task<bool> CheckTmpMaterial(string MtNo,DateTime datenow )
        {
            try
            {
                string sql = @"SELECT mt_no
                            FROM tmp_w_material_info
                            where mt_no=@mtno AND active=1 AND DATEDIFF(DAY,reg_date,@regdt)";
                string mtno = await base.DbConnection.QueryFirstOrDefaultAsync<string>(sql, new { mtno = MtNo, regdt = datenow });
                return string.IsNullOrEmpty(mtno);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> DeleteTmpMaterial(string MtNo,string ChangeId,DateTime ChangeDate)
        {
            string sql = @"Update tmp_w_material_info SET active=0,chg_id=@chgid,chg_date=@chgdt WHERE mt_no=@mtno";
            return await base.DbConnection.ExecuteAsync(sql, new {chgid=ChangeId,chgdt=ChangeDate,mtno=MtNo });
        }

        public async Task<IReadOnlyList<d_bobbin_lct_hist>> GetlistBobbinHist(string BbNo)
        {
            string sql = @"SELECT * FROM d_bobbin_lct_hist WHERE bb_no= @bbno AND active=1";
            var result =await base.DbConnection.QueryAsync<d_bobbin_lct_hist>(sql,new { bbno= BbNo });
            return result.ToList();
        }

        public async Task<d_bobbin_info> GetBobbinInfo(string BbNo)
        {
            string sql = @"SELECT * FROM d_bobbin_info WHERE bb_no= @bbno AND active =1";
            var result = await base.DbConnection.QueryFirstOrDefaultAsync<d_bobbin_info>(sql, new { bbno = BbNo });
            return result;
        }

        public async Task<int> UpdateBobbinInfo(string BbNo,string MtCd,string ChangeId,DateTime ChangeDate)
        {
            string sql = @"Update d_bobbin_info set mt_cd=@mtcd,chg_id=@chgid,chg_dt=@chgdt where bb_no=@bbno";
            return await base.DbConnection.ExecuteAsync(sql,new { mtcd= MtCd, chgid= ChangeId, chgdt= ChangeDate, bbno= BbNo });

        }

        public async Task<Models.NewVersion.MaterialInfoMMS> GetMaterialInfoMMS(string mt_cd)
        {
            string sql = @"SELECT Top 1 * FROM  w_material_info_mms WHERE material_code = @Mt_Cd AND active=1 Order by reg_date Desc";
            return await base.DbConnection.QueryFirstOrDefaultAsync<Models.NewVersion.MaterialInfoMMS>(sql,new { @Mt_Cd = mt_cd });
        }

        public async Task<DateTime> GetEndDateProcessUnit(float timeNow)
        {
            try
            {
                var query = "SELECT ( " +

                    " CASE " +
                        " WHEN 8 <= @TimeNow AND  @TimeNow <= 20 " +
                        " THEN DateAdd(HOUR,20,CAST(CAST(GETDATE() AS DATE) AS DATETIME)) WHEN 20 < @TimeNow AND  @TimeNow <= 24 " +
                        " THEN DateAdd(HOUR,32,CAST(CAST(GETDATE() AS DATE) AS DATETIME)) WHEN 0 <= @TimeNow AND @TimeNow < 8 " +
                        " THEN DateAdd(HOUR,8,CAST(CAST(GETDATE() AS DATE) AS DATETIME)) " +
                        " ELSE '' " +
                    " END) ";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<DateTime>(query, new { @TimeNow = timeNow });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<CommCode>> getListRoutingProccess()
        {
            try
            {
                var sql = "Select * from comm_dt where mt_cd = 'COM007' and use_yn = 'Y'";
                var result = await base.DbConnection.QueryAsync<CommCode>(sql);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Kiểm tra xem nhân viên nó có đang trông slot nào hay không?
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<int> checkStaffIsWorking(string Id)
        {
            try
            {
                var query = "SELECT COUNT(a.id_actual) FROM  w_material_info_mms AS a " +
                             " JOIN d_pro_unit_staff AS b ON a.id_actual = b.id_actual " +
                             " WHERE b.psid = @id AND(a.reg_date BETWEEN b.start_dt AND  b.end_dt) ";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(query, new { @id = Id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// Kiểm tra xem máy nó có đang hoạt động hay không ?
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<int> checkMachineOrMoldIsWorking(string Id)
        {
            try
            {
                var query = "SELECT COUNT(a.id_actual) FROM  w_material_info_mms AS a " +
                            " JOIN d_pro_unit_mc AS b ON a.id_actual = b.id_actual " +
                            " WHERE b.pmid = @id AND(a.reg_date BETWEEN b.start_dt AND  b.end_dt) ";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(query, new { @id = Id });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<WMaterialnfo> FindOneWMaterialInfoLike(string bbno)
        {
            string sql = @"select wmtid , id_actual ,material_type,material_code,mt_no,status mt_sts_cd,
			gr_qty,real_qty,location_code lct_cd,reg_date as reg_dt
			from w_material_info_mms 
			where   bb_no = @bbno";
            return await base.DbConnection.QueryFirstOrDefaultAsync<WMaterialnfo>(sql, new { @bbno = bbno });
            //return db.Database.SqlQuery<WMaterialnfo>(sql, new MySqlParameter("1", mt_cd)).SingleOrDefault();
        }
        public async Task<WMaterialnfo> FindOneWMaterialInfoLikeTIMS(string bbno)
        {
            string sql = @"select wmtid , id_actual ,material_type,material_code,mt_no,status mt_sts_cd,
			gr_qty,real_qty,location_code lct_cd,reg_date as reg_dt
			from w_material_info_tims
			where  location_code like '006%' and bb_no = @bbno";
            return await base.DbConnection.QueryFirstOrDefaultAsync<WMaterialnfo>(sql, new { @bbno = bbno });
            //return db.Database.SqlQuery<WMaterialnfo>(sql, new MySqlParameter("1", mt_cd)).SingleOrDefault();
        }

    }
}