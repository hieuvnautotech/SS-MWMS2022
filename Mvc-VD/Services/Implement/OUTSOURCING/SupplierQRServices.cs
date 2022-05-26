using Mvc_VD.Models.NewVersion;
using Mvc_VD.Respositories.Irepository;
using Mvc_VD.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Dapper;

namespace Mvc_VD.Services.Implement
{
    public class SupplierQRServices: DbConnection1RepositoryBase, ISupplierQRServiecs
    {
        public SupplierQRServices(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {

        }

        #region Supplier QR Management
        public async Task<IEnumerable<MaterialInfo>> GetListMaterialInfo(string mt_cd, string mt_no)
        {
            try
            {
                var query = @"Select * From d_material_info Where del_yn = 'N' And mt_cd Like @Mt_Cd+'%' And mt_no != @Mt_No And mt_type = 'MMT' ";
                var result = await base.DbConnection.QueryAsync<MaterialInfo>(query, new { @Mt_Cd = mt_cd, @Mt_No = mt_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<MaterialInfoTam>> GetListMaterialInfoTam(List<string> wmtid)
        {
            try
            {
                var listData = new List<MaterialInfoTam>();
                foreach (var item in wmtid)
                {
                    var query = @"Select * From w_material_info_tam Where wmtid = @Id";
                    var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoTam>(query, new { @Id = item });
                    listData.Add(result);
                }
                return listData;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<MaterialInfo>> SearchListMaterialInfo(string user, string type, string name, string code, string start, string end)
        {
            try
            {
                //a.del_yn = 'N' 
                var query = @"SELECT a.barcode,a.mtid, 
                            (select dt_nm from comm_dt where mt_cd='COM004' and dt_cd=mt_type) AS mt_type_nm, a.mt_type, a.bundle_qty,a.bundle_unit, a.mt_no, a.mt_nm, a.unit_cd,  a.sp_cd, a.mt_no_origin, a.width, a.width_unit,
                            a.spec, a.spec_unit, a.re_mark, a.reg_id, a.reg_dt, a.chg_id, a.chg_dt,
                          
                            (case when (a.bundle_unit IS NULL OR a.bundle_unit = '') then '' ELSE (SELECT dt_nm FROM comm_dt WHERE dt_cd = a.bundle_unit AND use_yn = 'Y' AND mt_cd = 'COM027')END) AS bundle_unit_nm,
                            ROW_NUMBER() OVER (ORDER BY a.mtid DESC) AS RowNum
                            FROM d_material_info AS a
                            WHERE a.del_yn = 'N' 
                            AND (a.sp_cd = @User)
                            AND (@Name = '' OR @Name IS NULL OR a.mt_nm LIKE '%' +@Name +'%')
                            AND (@Type = '' OR @Type IS NULL OR a.mt_type LIKE '%' +@Type+ '%')
                            AND (@Code = '' OR @Code IS NULL OR a.mt_no LIKE '%' +@Code+ '%')
                            AND (@Start ='' OR @Start IS NULL OR a.reg_dt >=  @Start)
                            AND (@End ='' OR @End IS NULL OR a.reg_dt <=  @End)
                            ORDER BY a.mtid DESC";
                var result = await base.DbConnection.QueryAsync<MaterialInfo>(query, new {@User = user, @Name = name, @Type  = type, @Code  = code, @Start = start, @End = end});
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion


        public async Task<IEnumerable<MaterialInfo>> searchMachiningQR(string type, string name, string code, string start, string end)
        {
            try
            {
                //a.del_yn = 'N' 
                var query = @" SELECT a.barcode,a.mtid, a.mt_cd, a.thick, a.thick_unit, a.stick, a.stick_unit, a.tot_price, a.price_least_unit, a.area, a.area_unit, a.item_vcd, a.qc_range_cd,  "
                + "(select dt_nm from comm_dt where mt_cd='COM004' and dt_cd=mt_type) mt_type_nm, a.mt_type, a.bundle_qty,a.bundle_unit, a.mt_no, a.mt_nm, a.unit_cd, a.mf_cd, a.sp_cd, a.mt_no_origin, a.s_lot_no, a.width, a.gr_qty, a.width_unit, "
                + " a.spec, a.spec_unit, a.price, a.price_unit, a.photo_file, a.re_mark, a.reg_id, a.reg_dt, a.chg_id, a.chg_dt, a.consum_yn consumable, "
                + " (case when a.price IS NULL then '' ELSE a.price END) AS new_price, "
                + " (case when a.spec IS NULL then '' ELSE a.spec END) AS new_spec, "
                + " (case when a.width IS NULL then '' ELSE a.width END) AS new_width,"
                + " (case when a.area IS NULL then '' ELSE a.area END) AS area_all, "
                + " (case when a.tot_price IS NULL then '' ELSE a.tot_price END) AS tot_price_new, "
                + " (case when a.stick IS NULL then '' ELSE a.stick END) AS stick_new, "
                + " (case when a.thick IS NULL then '' ELSE a.thick END) AS thick_new, "
                + " (case when (a.item_vcd IS NULL OR a.item_vcd = '') then '' ELSE (SELECT item_nm FROM qc_item_mt WHERE item_vcd = a.item_vcd)END) AS item_nm, "
                + " (case when (a.qc_range_cd IS NULL OR a.qc_range_cd = '') then '' ELSE (SELECT dt_nm FROM comm_dt WHERE dt_cd = a.qc_range_cd AND mt_cd = 'COM017')END) AS qc_range_cd_nm, "
                + " (case when (a.bundle_unit IS NULL OR a.bundle_unit = '') then '' ELSE (SELECT dt_nm FROM comm_dt WHERE dt_cd = a.bundle_unit AND use_yn = 'Y' AND mt_cd = 'COM027')END) AS bundle_unit_nm, "
                + " ROW_NUMBER() OVER (ORDER BY a.mtid DESC) AS RowNum "
                + " FROM d_material_info AS a "
                + " WHERE a.del_yn='N' AND  a.mt_type ='PMT'  "
                + " AND (a.mt_nm LIKE '%' + @name  +'%') "
                + " AND (a.mt_type LIKE '%' + @type + '%') "
                + " AND (a.mt_no LIKE '%' + @code + '%') "
                +" AND(@Start = '' OR @Start IS NULL OR a.reg_dt >= @Start) "
                +" AND(@End = '' OR @End IS NULL OR a.reg_dt <= @End) "
                +" ORDER BY a.mtid DESC";

                var result = await base.DbConnection.QueryAsync<MaterialInfo>(query, new { @Name = name, @Type = type, @Code = code, @Start = start, @End = end });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}