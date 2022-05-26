using Mvc_VD.Respositories.Irepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvc_VD.Services.Interface;
using Mvc_VD.Models.WIP;
using System.Threading.Tasks;
using Dapper;
using Mvc_VD.Models.WOModel;
using Mvc_VD.Models.Response;
using Mvc_VD.Models.NewVersion;

namespace Mvc_VD.Services.Implement
{
    public class WMSServices : DbConnection1RepositoryBase, IWMSServices
    {
        public WMSServices(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {

        }
        public async Task<IReadOnlyList<WMaterialInfo>> GetlistMaterialInfo(string MtNo, string StyleNo, int intpage, int introw)
        {
            string sql = @"SELECT max(mt.mtid) AS id, MAX(mt.mt_no)AS mt_no,max(mt.mt_nm) as mt_nm, max(bom.style_no) AS style_no , '' as meter
                            FROM d_material_info AS mt
                            LEFT JOIN (Select style_no, mt_no From d_bom_info) AS bom ON mt.mt_no = bom.mt_no
                            WHERE mt.barcode = 'Y' AND mt.mt_type !='MMT' And mt.active = 1
                            AND  (@MtNo ='' OR @MtNo IS NULL OR  mt.mt_no Like '%'+ @MtNo + '%' )
							AND (@StyleNo ='' OR @StyleNo IS NULL OR  bom.style_no Like '%'+ @StyleNo + '%' )
                            GROUP BY mt.mt_no
                            Order by mt.mt_no OFFSET @intpage ROWS FETCH NEXT @introw ROWS ONLY";
            var result = await base.DbConnection.QueryAsync<WMaterialInfo>(sql, new { @MtNo = MtNo, @StyleNo = StyleNo, @intpage = intpage, @introw = introw });
            return result.ToList();
        }

        public async Task<int> CountListMaterialInfo(string MtNo, string StyleNo)
        {
            string sql = @"	SELECT Count(mt.mtid)
                            FROM d_material_info AS mt
                            WHERE mt.barcode = 'Y' AND mt.mt_type !='MMT' And mt.active = 1";
            var result = await base.DbConnection.ExecuteScalarAsync<int>(sql, new { @MtNo = MtNo, @StyleNo = StyleNo });
            return result;
        }

        public async Task<int> InsertShippingdMaterial(Models.NewVersion.shippingsdmaterial item)
        {
            string sql = @"INSERT INTO shippingsdmaterial(sd_no,mt_no,quantity,meter,reg_id,chg_id) Values (@sd_no,@mt_no,@quantity,@meter,@reg_id,@chg_id) Select Scope_Identity()";
            return await base.DbConnection.ExecuteScalarAsync<int>(sql, item);
        }

        public async Task<int> UpdateSDinfo(int alert, string status, string chg_id, string sd_no, DateTime chg_dt)
        {
            string sql = @"UPDATE w_sd_info SET alert=@alert,status = @status , chg_id = @chg_id, chg_dt = @chg_dt
                                     WHERE sd_no=@sd_no";
            return await base.DbConnection.ExecuteAsync(sql, new { @alert = alert, @status = status, @chg_id = chg_id, @chg_dt = chg_dt, @sd_no = sd_no });
        }

        public async Task<string> GetDMaterialInfo(int idmaterial)
        {
            string sql = @"SELECT mt_no FROM d_material_info WHERE mtid = @MtId AND active = 1";
            return await base.DbConnection.QueryFirstOrDefaultAsync<string>(sql, new { MtId = idmaterial });
        }

        public async Task<int> CheckShippingMaterial(string SdNo, string MtNo)
        {
            string sql = @"select Count(id) from shippingsdmaterial where sd_no = @sdno and mt_no = @mtno and active = 1";
            return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { sdno = SdNo, mtno = MtNo });

        }

        public async Task<int> UpdateDuplicateShipping(string SdNo, string MtNo, string ChangeId, DateTime ChangeDate)
        {
            string sql = @"Update shippingsdmaterial set active=0,chg_id=@chgid,chg_dt=@chgdt where sd_no = @sdno and mt_no = @mtno";
            return await base.DbConnection.ExecuteAsync(sql, new { sdno = SdNo, mtno = MtNo, chgid = ChangeId, chgdt = ChangeDate });
        }

        public async Task<IReadOnlyList<PickingScanResponse>> GetListMaterialInfoBySdNo(string SdNo, string MtNo)
        {
            try
            {
                var result = await base.DbConnection.QueryAsync<PickingScanResponse>("EXEC [dbo].[GetListMaterialInfo]@sdno,@mtno", new { @sdno = SdNo, @mtno = MtNo });
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
       //     string sql = @"SELECT  MAX(abc.id) As wmtid, MAX(abc.mt_no) AS mt_no, MAX(abc.SoluongCap) AS SoluongCap, isnull(info.SoLuongNhanDuoc,0) SoLuongNhanDuoc, MAX(abc.meter) AS meter,
	      //                  CASE
		     //                   WHEN MAX(abc.SoluongCap) > 0  THEN (Max(abc.SoluongCap) -  isnull(info.SoLuongNhanDuoc,0)) 
		     //                   ELSE 0
	      //                  END AS SoluongConLai
       //                 FROM (
	      //                  SELECT  a.mt_no, Max(a.id) as id, Sum(a.quantity) as SoluongCap, Max(a.sd_no) as sd_no, Sum(a.meter) as meter
	      //                  FROM shippingsdmaterial AS a
	      //                  WHERE a.sd_no = @sdno and a.active = 1
       //                     GROUP BY a.mt_no
       //                 ) AS abc
       //                  LEFT JOIN (
							//select sd_no,mt_no,count(material_code)SoLuongNhanDuoc from(
							//select sd_no,mt_no,material_code from inventory_products  where sd_no=@sdno AND ( orgin_mt_cd IS NULL OR orgin_mt_cd = '')
							//union all
							//select sd_no,mt_no,material_code from w_material_info_mms where sd_no=@sdno AND ( orgin_mt_cd IS NULL OR orgin_mt_cd = '')
							//) aa
							//group by sd_no,mt_no
       //                 ) AS info  ON info.sd_no = abc.sd_no AND abc.mt_no = info.mt_no 
       //                 GROUP BY abc.mt_no, abc.SoluongCap, info.SoLuongNhanDuoc
       //                 Order By abc.mt_no ASC";
       //     var result = await base.DbConnection.QueryAsync<PickingScanResponse>(sql, new { @sdno = SdNo, @mtno = MtNo });
       //     return result.ToList();
        }


        public async Task<IReadOnlyList<WMaterialInfo>> GetListPickingScanBySdNo(string SdNo, string MtNo)
        {
            try
            {
                var result = await base.DbConnection.QueryAsync<WMaterialInfo>("EXEC [dbo].[GetListPickingScan]@mtno,@sdno", new { @sdno = SdNo, @mtno = MtNo });
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            //string sql = @"SELECT max(abc.mt_no) as mt_no,max(abc.SoluongCap) as SoluongCap, COUNT(info.material_code) AS SoLuongNhanDuoc,
            //                CASE
            //                    WHEN max(abc.SoluongCap) > 0 THEN (max(abc.SoluongCap) -  COUNT(info.material_code)) 
            //                    ELSE 0
            //                END as SoluongConLai

            //                FROM (
            //                SELECT  max(a.mt_no) AS mt_no,max(a.id) AS id, sum(a.quantity) AS SoluongCap, max(a.sd_no) AS sd_no
            //                FROM shippingsdmaterial AS a
            //                WHERE a.sd_no = @sdno
            //                group by a.mt_no
            //                ) AS abc
            //                left JOIN inventory_products info ON info.sd_no = abc.sd_no AND abc.mt_no = info.mt_no 
            //                Where abc.mt_no = @mtno
            //                GROUP BY abc.mt_no";
            //var result = await base.DbConnection.QueryAsync<WMaterialInfo>(sql, new { @sdno = SdNo, @mtno = MtNo });
            //return result.ToList();
        }

        public async Task<IReadOnlyList<Models.NewVersion.shippingsdmaterial>> GetListShippngMaterial(string SdNo, string MtNo)
        {
            string sql = @"SELECT * FROM shippingsdmaterial WHERE sd_no = @sdno AND mt_no = @mtno";
            var result = await base.DbConnection.QueryAsync<Models.NewVersion.shippingsdmaterial>(sql, new { sdno = SdNo, mtno = MtNo });
            return result.ToList();
        }

        public async Task<IEnumerable<MbInfo>> GetListMbInfo(string Id)
        {
            try
            {
                var query = @"Select * from mb_info where userid = @Id";
                var result = await base.DbConnection.QueryAsync<MbInfo>(query, new { @Id = Id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<SdInfo> GetListSdInfoById(int Id)
        {
            try
            {
                var query = @"Select * from w_sd_info where sid = @Id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<SdInfo>(query, new { @Id = Id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateSdInfoTable(SdInfo item)
        {
            try
            {
                var query = @"Update w_sd_info Set sd_nm = @sd_nm, status = @status, product_cd = @product_cd, lct_cd = @lct_cd, alert = @alert, remark = @remark, use_yn = @use_yn, del_yn = @del_yn, 
                            reg_dt = @reg_dt, reg_id = @reg_id, chg_id = @chg_id, chg_dt = @chg_dt
                            Where sid = @Sid";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<string> GetListDatabySdNo(string sd_no)
        {
            try
            {
                var query = @"Select Top 1 sd_no From inventory_products where sd_no = @Sd_No";
                var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @Sd_No = sd_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> DeleteSdInfo(int sid)
        {
            try
            {
                var query = @"Delete w_sd_info Where sid = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Id = sid });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> DeleteShippingSDInfo(string sd_no)
        {
            try
            {
                var query = @"DELETE shippingsdmaterial WHERE sd_no = @Sd_no";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Sd_no = sd_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<MaterialInfoMemo>> GetListMaterialInfoMemo(string sd_no, string md_cd, string lot_no, string style_no)
        {
            try
            {
                var query = @"SELECT a.* FROM w_material_info_memo AS a 
                            Where (@Sd_No ='' OR @Sd_No IS NULL OR a.sd_no Like '%' + @Sd_No + '%')
                                        and (@Md_Cd ='' OR @Md_Cd IS NULL OR a.md_cd LIKE '%' + @Md_Cd + '%')
                                        and (@Lot_No ='' OR @Lot_No IS NULL OR a.lot_no LIKE '%' + @Lot_No + '%')
                                        and (@Style_No = '' OR @Style_No IS NULL OR a.style_no LIKE '%' + @Style_No + '%')";
                var result = await base.DbConnection.QueryAsync<MaterialInfoMemo>(query, new { @Sd_No = sd_no, @Md_Cd = md_cd, @Lot_No = lot_no, @Style_No = style_no });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<StyleInfo>> GetListDataStyleInfo(string style_no)
        {
            try
            {
                var query = @"Select * from d_style_info where style_no  = @StyleNo";
                var result = await base.DbConnection.QueryAsync<StyleInfo>(query, new { @StyleNo = style_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> InsertIntoMaterialInfoMemo(MaterialInfoMemo item)
        {
            try
            {
                var query = @"Insert Into w_material_info_memo (md_cd, style_no, style_nm, mt_cd, width, width_unit, spec, spec_unit, sd_no, lot_no, status, memo, month_excel, receiving_dt, 
                            tx, total_m, total_m2, total_ea, use_yn, reg_id, reg_dt, chg_id, chg_dt)
                            Values (@md_cd, @style_no, @style_nm, @mt_cd, @width, @width_unit, @spec, @spec_unit, @sd_no, @lot_no, @status, @memo, @month_excel, @receiving_dt, 
                            @tx, @total_m, @total_m2, @total_ea, @use_yn, @reg_id, @reg_dt, @chg_id, @chg_dt)
                            Select SCOPE_IDENTITY()";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<MaterialInfoMemo> GetMaterialInfoMemoById(int Id)
        {
            try
            {
                var query = @"Select * From w_material_info_memo where id = @Id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoMemo>(query, new { @Id = Id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> RemoveMaterialInfoMemo(int Id)
        {
            try
            {
                var query = @"Delete w_material_info_memo where id = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Id = Id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateMaterialInfoMemo(MaterialInfoMemo item)
        {
            try
            {
                var query = @"Update w_material_info_memo Set md_cd = @md_cd, style_no = @style_no, style_nm = @style_nm, mt_cd = @mt_cd, width = @width, width_unit = @width_unit, spec = @spec, spec_unit = @spec_unit, sd_no = @sd_no,
                                lot_no = @lot_no, status = @status, memo = @memo, month_excel = @month_excel, receiving_dt = @receiving_dt, tx = @tx, total_m = @total_m, total_m2 = @total_m2, total_ea = @total_ea, use_yn = @use_yn,
                                reg_id = @reg_id, reg_dt = @reg_dt, chg_id = @chg_id, chg_dt = @chg_dt
                                Where id = @id";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<MaterialInfoTam> CheckMaterialInfoTam(string mt_cd)
        {
            try
            {
                var query = @"Select * From w_material_info_tam where mt_cd = @Mt_Cd";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoTam>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Models.NewVersion.shippingsdmaterial> CheckMaterialInfoTam(int id)
        {
            try
            {
                var query = @"Select * From shippingsdmaterial where id = @Id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<Models.NewVersion.shippingsdmaterial>(query, new { @Id = id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateshippingMaterial(int id, int Qty)
        {
            try
            {
                var query = @"Update shippingsdmaterial Set quantity = @Qty where id = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Qty = Qty, @Id = id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<int> UpdateshippingMeterMaterial(int id, int Qty)
        {
            try
            {
                var query = @"Update shippingsdmaterial Set meter = @Qty where id = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Qty = Qty, @Id = id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<MaterialInfoMMS>> GetListWMaterial(string sd_no, string mt_no)
        {
            try
            {
                var query = @"SELECT *
                            FROM inventory_products
                            WHERE sd_no = @SdNo AND mt_no = @MtNo";
                var result = await base.DbConnection.QueryAsync<MaterialInfoMMS>(query, new { @SdNo = sd_no, @MtNo = mt_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<WMaterialInfoResponse>> GetListMaterialInfoBySdNo(string sd_no)
        {
            try
            {
                var query = @"SELECT  ROW_NUMBER() OVER(ORDER BY abc.id DESC)  AS wmtid, abc.mt_no, abc.SoluongCap, 
		                    COUNT(info.material_code) AS SoLuongNhanDuoc,abc.meter AS meter,
                            CASE
                                WHEN abc.SoluongCap > 0 THEN ((abc.SoluongCap) -  COUNT(info.material_code))
                                ELSE 0
                            END as SoluongConLai

                            FROM (
                            SELECT  max(a.mt_no) AS mt_no, max(a.id) AS id, sum(a.quantity) AS SoluongCap, max(a.sd_no) AS sd_no, sum(a.meter) AS meter
                            FROM shippingsdmaterial AS a
                             WHERE a.sd_no = @SdNo
                                GROUP BY a.mt_no
                            ) AS abc
                            left JOIN w_material_info_mms  info ON info.sd_no = abc.sd_no AND abc.mt_no = info.mt_no AND ( info.status IS NULL OR info.status = '')
                            GROUP BY abc.mt_no, abc.SoluongCap, abc.meter, abc.id";
                var result = await base.DbConnection.QueryAsync<WMaterialInfoResponse>(query, new { @SdNo = sd_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<SdInfo>> GetPickingScanPP(string sd_no)
        {
            try
            {
                var query = @"Select sd_no, sd_nm, (Select dt_nm From comm_dt where mt_cd = 'WHS005' and dt_cd = status) As sts_nm, 
                                (Select lct_nm From lct_info where lct_cd = lct_bar_cd) As lct_nm, remark, alert
                            From w_sd_info where sd_no = @SdNo";
                var result = await base.DbConnection.QueryAsync<SdInfo>(query, new { @SdNo = sd_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<MaterialInfoMemo>> GetPickingScanPP_Memo(string sd_no)
        {
            try
            {
                var query = @"Select * from w_material_info_memo where sd_no = @SdNo";
                var result = await base.DbConnection.QueryAsync<MaterialInfoMemo>(query, new { @SdNo = sd_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<MaterialInfoTam>> GetPickingScanListPP(string sd_no)
        {
            try
            {


                var query = @"SELECT a.materialid As id, convert(varchar(19),a.recei_wip_date,121) As rece_wip_dt, a.material_code As mt_cd, a.mt_no, a.lot_no,  a.gr_qty, a.location_code,
								convert(varchar(10),a.expiry_date,121) As expiry_dt, convert(varchar(10),a.date_of_receipt,121) As dt_of_receipt ,convert(varchar(10), a.export_date,121) As expore_dt, a.sd_no, a.status, a.ExportCode,
								(SELECT dt_nm FROM comm_dt WHERE mt_cd = 'WHS005' AND dt_cd = a.status) AS sts_nm,
								 CASE  WHEN a.ExportCode IS NULL OR a.ExportCode = ''  THEN b.lct_nm ELSE a.ExportCode END lct_nm ,
								(SELECT at_no FROM w_actual WHERE id_actual = a.id_actual) AS po
								FROM inventory_products As a
								LEFT JOIN (Select lct_nm, lct_cd From lct_info) As b ON a.location_code = b.lct_cd
								WHERE a.sd_no = @Sd_No AND a.location_code LIKE '002%' 
								
								UNION ALL 

								SELECT a.wmtid As id,convert(varchar(19), a.reg_date,121) As rece_wip_dt, a.material_code As mt_cd, a.mt_no, a.lot_no,  a.gr_qty, a.location_code,
                               convert(varchar(10), a.expiry_date,121) As expiry_dt, convert(varchar(10),a.date_of_receipt,121) As dt_of_receipt , convert(varchar(10),a.export_date,121) As expore_dt, a.sd_no, a.status, a.ExportCode,
								(SELECT dt_nm FROM comm_dt WHERE mt_cd = 'WHS005' AND dt_cd = a.status) AS sts_nm,
								CASE  WHEN a.ExportCode IS NULL OR a.ExportCode = ''  THEN b.lct_nm ELSE a.ExportCode END As lct_nm,
			                    (SELECT at_no FROM w_actual WHERE id_actual = a.id_actual) AS po
                                FROM w_material_info_mms As a
						        LEFT JOIN (Select lct_nm, lct_cd From lct_info) As b ON a.location_code = b.lct_cd
							    WHERE a.sd_no = @Sd_No AND a.location_code LIKE '002%' ";

                var result = await base.DbConnection.QueryAsync<MaterialInfoTam>(query, new { @Sd_No = sd_no });

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<GetPickingScanPPCountMtNoResponse>> GetPickingScanPP_Count_MT_no(string sd_no)
        {
            try
            {
                //          var query = @"
                //                      SELECT  
                //                      MAX(abc.id) As wmtid, MAX(abc.mt_no) AS mt_no, MAX(abc.SoluongCap) AS cap, isnull(info.SoLuongNhanDuoc,0) nhan,
                //                      isnull(infodasd.dasudung,0) dasd,
                //                      isnull(infodang.dangsd,0) dangsd,
                //                      isnull(infotrave.trave,0) trave,
                //                       MAX(abc.meter) AS meter
                //                  FROM (
                //                   SELECT  concat(a.mt_no,isnull(slnhandc,0)) mt_no, Max(a.id) as id, Sum(a.quantity) as SoluongCap, Max(a.sd_no) as sd_no
                //                   FROM shippingsdmaterial AS a
                //                   WHERE a.sd_no = @SdNo and a.active = 1
                //                      GROUP BY a.sd_no, a.mt_no
                //                  ) AS abc
                //                    JOIN (
                //	select sd_no,mt_no, count(material_code)SoLuongNhanDuoc,sum(aa.gr_qty) slnhandc from(
                //	select sd_no,mt_no,material_code,gr_qty from inventory_products  where sd_no=@SdNo AND ( orgin_mt_cd IS NULL OR orgin_mt_cd = '')
                //	union all
                //	select sd_no,mt_no,material_code,gr_qty from w_material_info_mms where sd_no=@SdNo AND ( orgin_mt_cd IS NULL OR orgin_mt_cd = '')
                //	) aa
                //	group by sd_no,mt_no
                //                  ) AS info  ON info.sd_no = abc.sd_no AND abc.mt_no = info.mt_no 

                //LEFT JOIN (
                //	select sd_no,mt_no, count(material_code)dasudung from(
                //	select sd_no,mt_no,material_code from inventory_products  where sd_no=@SdNo AND  location_code LIKE '002%' AND status ='005'
                //	union all
                //	select sd_no,mt_no,material_code from w_material_info_mms where sd_no=@SdNo AND  location_code LIKE '002%' AND status ='005'
                //	) aa
                //	group by sd_no,mt_no
                //                  ) AS infodasd  ON infodasd.sd_no = abc.sd_no AND abc.mt_no = infodasd.mt_no 

                //LEFT JOIN (
                //	select sd_no,mt_no, count(material_code)dangsd from(
                //	select sd_no,mt_no,material_code from inventory_products  where sd_no=@SdNo AND location_code LIKE '002%' AND status ='002'
                //	union all
                //	select sd_no,mt_no,material_code from w_material_info_mms where sd_no=@SdNo AND location_code LIKE '002%' AND status ='002'
                //	) aa
                //	group by sd_no,mt_no
                //                  ) AS infodang  ON infodang.sd_no = abc.sd_no AND abc.mt_no = infodang.mt_no 

                //LEFT JOIN (
                //	select sd_no,mt_no, count(material_code)trave from(
                //	select sd_no,mt_no,material_code from inventory_products  where sd_no=@SdNo AND location_code LIKE '002%' AND status ='013'
                //	union all
                //	select sd_no,mt_no,material_code from w_material_info_mms where sd_no=@SdNo AND location_code LIKE '002%' AND status ='013'
                //	) aa
                //	group by sd_no,mt_no
                //                  ) AS infotrave  ON infotrave.sd_no = abc.sd_no AND abc.mt_no = infotrave.mt_no 
                //                  GROUP BY abc.mt_no, abc.SoluongCap, info.SoLuongNhanDuoc,infodasd.dasudung,infodang.dangsd,infotrave.trave";
                string query = @"SELECT  
                            MAX(abc.id) As wmtid, concat(MAX(abc.mt_no),' (',cast(isnull(FORMAT(infosldanhan.slnhandc,'#,#'),0) as varchar(50)),'M)') AS mt_no, MAX(abc.SoluongCap) AS cap, isnull(info.SoLuongNhanDuoc,0) nhan,
                            isnull(infodasd.dasudung,0) dasd,
                            isnull(infodang.dangsd,0) dangsd,
                            isnull(infotrave.trave,0) trave
                        FROM (
	                        SELECT  a.mt_no , Max(a.id) as id, Sum(a.quantity) as SoluongCap, Max(a.sd_no) as sd_no
	                        FROM shippingsdmaterial AS a
	                        WHERE a.sd_no = @SdNo and a.active = 1
                            GROUP BY a.sd_no, a.mt_no
                        ) AS abc
                         left JOIN (
							select sd_no,mt_no, count(material_code)SoLuongNhanDuoc from(
					select sd_no,mt_no,material_code,gr_qty from inventory_products  where sd_no=@SdNo AND ( orgin_mt_cd IS NULL OR orgin_mt_cd = '')
                	union all
                	select sd_no,mt_no,material_code,gr_qty from w_material_info_mms where sd_no=@SdNo AND ( orgin_mt_cd IS NULL OR orgin_mt_cd = '')
							--select sd_no,mt_no,material_code from inventory_products  where sd_no=@SdNo --AND ( orgin_mt_cd IS NULL OR orgin_mt_cd = '' )
							--union all
							--select sd_no,mt_no,material_code from w_material_info_mms where sd_no=@SdNo AND material_code not in (select material_code from inventory_products  where sd_no=@SdNo)
							) aa
							group by sd_no,mt_no
                        ) AS info  ON info.sd_no = abc.sd_no AND abc.mt_no = info.mt_no 
                        LEFT JOIN (
							select sd_no,mt_no, sum(gr_qty)slnhandc from(
							select sd_no,mt_no,gr_qty from inventory_products  where sd_no=@SdNo 
							union all
							select sd_no,mt_no,gr_qty from w_material_info_mms where sd_no=@SdNo 
							) aa
							group by sd_no,mt_no
                        ) AS infosldanhan  ON infosldanhan.sd_no = abc.sd_no AND abc.mt_no = infosldanhan.mt_no 
						LEFT JOIN (
							select sd_no,mt_no, count(material_code)dasudung from(
							select sd_no,mt_no,material_code from inventory_products  where sd_no=@SdNo AND  location_code LIKE '002%' AND status ='005' AND ( orgin_mt_cd IS NULL OR orgin_mt_cd = '')
							union all
							select sd_no,mt_no,material_code from w_material_info_mms where sd_no=@SdNo AND  location_code LIKE '002%' AND status ='005' AND ( orgin_mt_cd IS NULL OR orgin_mt_cd = '')
							) aa
							group by sd_no,mt_no
                        ) AS infodasd  ON infodasd.sd_no = abc.sd_no AND abc.mt_no = infodasd.mt_no 

						LEFT JOIN (
							select sd_no,mt_no, count(material_code)dangsd from(
							select sd_no,mt_no,material_code from inventory_products  where sd_no=@SdNo AND location_code LIKE '002%' AND status ='002' AND ( orgin_mt_cd IS NULL OR orgin_mt_cd = '')
							union all
							select sd_no,mt_no,material_code from w_material_info_mms where sd_no=@SdNo AND location_code LIKE '002%' AND status ='002' AND ( orgin_mt_cd IS NULL OR orgin_mt_cd = '')
							) aa
							group by sd_no,mt_no
                        ) AS infodang  ON infodang.sd_no = abc.sd_no AND abc.mt_no = infodang.mt_no 

						LEFT JOIN (
							select sd_no,mt_no, count(material_code)trave from(
							select sd_no,mt_no,material_code from inventory_products  where sd_no=@SdNo AND location_code LIKE '002%' AND status ='013' AND ( orgin_mt_cd IS NULL OR orgin_mt_cd = '')
							union all
							select sd_no,mt_no,material_code from w_material_info_mms where sd_no=@SdNo AND location_code LIKE '002%' AND status ='013' AND ( orgin_mt_cd IS NULL OR orgin_mt_cd = '')
							) aa
							group by sd_no,mt_no
                        ) AS infotrave  ON infotrave.sd_no = abc.sd_no AND abc.mt_no = infotrave.mt_no 
                        GROUP BY abc.mt_no, abc.SoluongCap, info.SoLuongNhanDuoc,infodasd.dasudung,infodang.dangsd,infotrave.trave,infosldanhan.slnhandc";
                var result = await base.DbConnection.QueryAsync<GetPickingScanPPCountMtNoResponse>(query, new { @SdNo = sd_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateMaterialInfoTam(string wmtid, string sd_no, string time)
        {
            try
            {
                var query = @"UPDATE  w_material_info_tam  
                            SET picking_dt= @Time, sd_no = @Sd_No , lct_cd = '002000000000000000' , from_lct_cd = '002000000000000000' , lct_sts_cd = '000' 
                            WHERE wmtid In @Id And mt_barcode Is Not Null";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Time = time, @Sd_No = sd_no, @Id = wmtid });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateMaterialInfoTam(string mt_cd, string sd_no)
        {
            try
            {
                var query = @"UPDATE  w_material_info_tam  
                            SET sd_no = @Sd_No , lct_cd = '002000000000000000' , from_lct_cd = '002000000000000000' , lct_sts_cd = '000' 
                            WHERE mt_cd In @Mt_Cd And mt_barcode Is Not Null";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Sd_No = sd_no, @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateSdNoInfo(string sd_no, int alert, string status)
        {
            try
            {
                var query = @"UPDATE  w_sd_info SET w_sd_info.alert = @Alert ,status = @Status WHERE w_sd_info.sd_no in (@SdNo) ";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Sd_No = sd_no, @Alert = alert, @Status = status });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<MaterialInfoTam> CountMaterialInfoTamByMtCd(string mt_cd)
        {
            try
            {
                var query = @"Select * from w_material_info_tam Where mt_cd = @Mt_Cd And status = 000";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoTam>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UpdateMaterialInfoTam(MaterialInfoTam Item)
        {
            try
            {
                var query = @"Update w_material_info_tam Set sd_no = @sd_no, lct_cd = @lct_cd, to_lct_cd = @to_lct_cd, from_lct_cd = @from_lct_cd, lct_sts_cd = @lct_sts_cd, 
                                remark = @remark, reg_id = @reg_id, reg_dt = @reg_dt, chg_id = @chg_id, chg_dt = chg_dt Where wmtid = @wmtid";
                var result = await base.DbConnection.ExecuteAsync(query, Item);
                return result;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<MaterialInfoTam> CountMaterialInfoTamBySdNo(string sd_no)
        {
            try
            {
                var query = @"Select * from w_material_info_tam Where sd_no = @SdNo And status = 000";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoTam>(query, new { @SdNo = sd_no });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<MaterialInfoTam>> GetListDataMaterialInfoTam(string mt_no, string mt_cd)
        {
            try
            {
                var query = @"SELECT a.wmtid ,a.mt_cd,a.mt_no,a.lot_no,a.gr_qty ,a.expiry_dt ,a.dt_of_receipt,a.expore_dt  FROM w_material_info_tam as a 
                            WHERE  a.status ='000'  
                            AND (@Mt_No ='' OR @Mt_No IS NULL OR  a.mt_no Like '%' + @Mt_No + '%' ) 
                            AND (@Mt_Cd ='' OR @Mt_Cd IS NULL OR  a.mt_cd Like '%' + @Mt_Cd + '%' )";
                var result = await base.DbConnection.QueryAsync<MaterialInfoTam>(query, new { @Mt_No = mt_no, @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<MaterialInfoTam> GetMaterialInfoTamById(int id)
        {
            try
            {
                var query = @"Select * From w_material_info_tam where wmtid = @Id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoTam>(query, new { @Id = id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<MaterialInfoTam>> GetMaterialInfoTamByMtCd(string mt_cd)
        {
            try
            {
                var query = @"Select wmtid, mt_cd, lot_no, gr_qty, expiry_dt, dt_of_receipt, expore_dt From w_material_info_tam where mt_cd = @Mt_Cd";
                var result = await base.DbConnection.QueryAsync<MaterialInfoTam>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> CheckMaterialInfo(string mt_no)
        {
            try
            {
                var query = "SELECT Count(mt_no) FROM d_material_info WHERE mt_no = @Mt_No and mt_type != 'MMT' and barcode = 'Y' ";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Mt_No = mt_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> DeleteShippingMaterial(int id)
        {
            try
            {
                var query = "DELETE FROM shippingsdmaterial WHERE id= @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Id = id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<string> CheckMaterialInfoToUpload(string mt_no)
        {
            try
            {
                var query = "SELECT mt_no FROM d_material_info WHERE mt_no = @Mt_No and mt_type != 'MMT' and barcode = 'Y' ";
                var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @Mt_No = mt_no });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<IEnumerable<MaterialShippingMemo>> GetListSearchShowMemo(string datemonth, string product, string material, string date)
        {
            try
            {
                var result = await base.DbConnection.QueryAsync<MaterialShippingMemo>("EXEC [dbo].[MaterialMemo]@datemonth,@product,@material,@date", new { @datemonth = datemonth, @product = product, @material = material, @date = date });
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            //try
            //{
            //    string viewSql = @"
            //             SELECT max(TABLE1.mt_no) mt_no, max(TABLE1.reg_date) reg_date, max(TABLE1.style_no) product,MAX(TABLE1.width) width, MAX(TABLE1.spec) spec, SUM(TABLE1.TX) total_roll, SUM(TABLE1.total_m) total_m, SUM(TABLE1.total_m2) total_m2, SUM(TABLE1.total_ea) total_ea, MAX(TABLE1.lot_no) AS lot_no
            //            FROM (
            //            SELECT (
            //               CASE
            //               WHEN ('08:00:00' <= FORMAT( CAST( a.receiving_dt AS datetime ),'%H:%i:%s') AND  FORMAT( CAST( a.receiving_dt AS datetime ),'yyyy/MM/dd')  <  '23:59:00') THEN
            //               FORMAT( CAST( a.receiving_dt AS DATETIME ),'yyyy/MM/dd')

            //               when (FORMAT( CAST(a.receiving_dt AS datetime ),'yyyy/MM/dd')  < '08:00:00') THEN  FORMAT(DATEADD(day,-1,a.receiving_dt),'yyyy/MM/dd')
            //                 ELSE ''
            //               END )  as reg_date,a.receiving_dt,
            //            a.mt_cd AS mt_no ,a.style_no,
            //            a.width AS width ,
            //            a.spec AS spec ,
            //            a.TX  ,a.total_m,a.total_m2,a.total_ea, a.lot_no
            //            FROM w_material_info_memo a
            //             WHERE   FORMAT(CAST(a.receiving_dt as datetime), 'yyyy-MM') = @datemonth
            //                and (@product='' OR  a.style_no like @product )
            //                     and (@material='' OR  a.mt_cd like @material )
            //            ) AS TABLE1
            //                WHERE   (@date='' OR   FORMAT(CAST(TABLE1.reg_date as datetime),'yyyy-MM-dd') like @date )
            //               GROUP BY TABLE1.reg_date, TABLE1.mt_no, TABLE1.style_no
            //               ORDER BY  TABLE1.reg_date DESC, TABLE1.style_no

            //                         ";
            //    var result = await base.DbConnection.QueryAsync<MaterialShippingMemo>(viewSql, new { @datemonth = datemonth, @product = product, @material = material, @date = date });
            //    return result.ToList();
            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}
        }
        public async Task<IEnumerable<MaterialShipping>> GetListSearchShowMaterialShipping(string datemonth, string product, string material, string date)
        {
            try
            {
                var result = await base.DbConnection.QueryAsync<MaterialShipping>("EXEC [dbo].[MaterialShipping] @datemonth,@date,@product,@material", new { @datemonth = datemonth, @product = product, @material = material, @date = date });
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        //    try
        //    {
        //        string viewSql = @"
        //                        SELECT 
        //                          TABLE1.mt_no as MaterialNo,
        //                          TABLE1.unit_cd,
        //                          TABLE1.product_cd as product,
        //                          count(TABLE1.gr_qty) countSocuon,
        //                          SUM(CONVERT(int,TABLE1.gr_qty)) TongSoMet,
        //                          ( TABLE1.reg_date ) recevingDate
        //                          FROM 
        //                        (SELECT b.lot_no, a.sd_no,a.product_cd, b.mt_no, b.gr_qty, c.spec,c.width,c.unit_cd, 
        //                        b.recei_wip_date, b.real_qty
        //                        ,
        //                         (
        //                                        CASE 
        //                                        WHEN ('08:00:00' <= FORMAT( CAST( b.recei_wip_date AS datetime ),'HH:mm:ss') AND  FORMAT( CAST( b.recei_wip_date AS datetime ),'HH:mm:ss')  <  '23:59:00') THEN
        //                                        FORMAT( CAST( b.recei_wip_date AS DATETIME ),'yyyy-MM-dd')

        //                                        when (FORMAT( CAST( b.recei_wip_date AS datetime ),'HH:mm:ss')  < '08:00:00') THEN  FORMAT(DATEADD(day,-1,b.recei_wip_date),'yyyy-MM-dd')
        //                                          ELSE ''
        //                                        END )  as reg_date

        //                         FROM w_sd_info AS a
        //                       JOIN inventory_products AS b ON a.sd_no = b.sd_no
        //                       JOIN d_material_info AS c ON b.mt_no = c.mt_no
							
        //                        WHERE  FORMAT(CAST(b.recei_wip_date as datetime), 'yyyy-MM') = @datemonth and b.mt_type='PMT' AND b.status!='004'
        //                         and (@product='' OR  a.product_cd like @product )
        //                        and (@material='' OR  b.mt_no like @material )
        //               --         and (@date='' OR  FORMAT(CAST(b.recei_wip_date as datetime),'yyyy-MM-dd') like @date )
								//)
        //                        AS TABLE1  
        //                          where  (@date='' OR  FORMAT(CAST(TABLE1.reg_date as datetime),'yyyy-MM-dd') like @date )
        //                          GROUP BY TABLE1.reg_date, TABLE1.mt_no, TABLE1.product_cd,TABLE1.unit_cd

        //                          ORDER BY  TABLE1.reg_date DESC, TABLE1.product_cd
        //                             ";
        //        var result = await base.DbConnection.QueryAsync<MaterialShipping>(viewSql, new { @datemonth = datemonth, @product = product, @material = material, @date = date });
        //        return result.ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        }

        public async Task<IEnumerable<MaterialShipping>> getShowMaterialShippingDetail(string style_no, string mt_no, string recei_dt, string datemonth)
        {
            try
            {
                string viewSql = @"
                                SELECT (TABLE1.mt_no) MaterialNo,table1.recei_wip_date as rece_wip_dt , TABLE1.product_cd product,TABLE1.lot_no, TABLE1.spec,table1.sd_no, TABLE1.width,TABLE1.unit_cd,table1.mt_cd, (TABLE1.real_qty) countSocuon, (TABLE1.gr_qty) TongSoMet,( TABLE1.reg_date ) recevingDate FROM 
                                (SELECT b.lot_no, a.sd_no, a.product_cd, b.mt_no, b.gr_qty, c.spec,c.width,c.unit_cd,b.material_code as mt_cd, b.real_qty,
                                b.recei_wip_date
                                ,
                                 (
                                                CASE 
                                                WHEN ('08:00:00' <= FORMAT( CAST( b.recei_wip_date AS datetime ),'HH:mm:ss') AND  FORMAT( CAST( b.recei_wip_date AS datetime ),'HH:mm:ss')  <  '23:59:00') THEN
                                                FORMAT( CAST( b.recei_wip_date AS DATETIME ),'yyyy-MM-dd')

                                                when (FORMAT( CAST( b.recei_wip_date AS datetime ),'HH:mm:ss')  < '08:00:00') THEN  FORMAT(DATEADD(day,-1,b.recei_wip_date),'yyyy-MM-dd')
                                                  ELSE ''
                                                END )  as reg_date

                               FROM w_sd_info AS a
                               JOIN inventory_products AS b ON a.sd_no = b.sd_no
                               JOIN d_material_info AS c ON b.mt_no = c.mt_no
                                WHERE   FORMAT(b.recei_wip_date, 'yyyy-MM') = @datemonth AND b.mt_type ='PMT'  
                                  and (a.product_cd = @style_no )
                                 and (b.mt_no = @mt_no))
                                AS TABLE1
                                where FORMAT(CAST(TABLE1.reg_date as datetime),'yyyy-MM-dd') like @recei_dt
                                ORDER BY  TABLE1.reg_date DESC , table1.mt_cd asc
                                     ";
                var result = await base.DbConnection.QueryAsync<MaterialShipping>(viewSql, new { @style_no = style_no, @mt_no = mt_no, @recei_dt = recei_dt, @datemonth = datemonth });
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}