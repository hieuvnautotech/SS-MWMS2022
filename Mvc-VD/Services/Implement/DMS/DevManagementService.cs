using Dapper;
using Mvc_VD.Classes;
using Mvc_VD.Models;
using Mvc_VD.Models.APIRequest;
using Mvc_VD.Models.APIRespones;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models.Request;
using Mvc_VD.Models.Response;
using Mvc_VD.Models.WIP;
using Mvc_VD.Models.WMS;
using Mvc_VD.Respositories.Irepository;
using Mvc_VD.Services.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static Mvc_VD.Controllers.DevManagementController;
using MaterialInfoMMS = Mvc_VD.Models.NewVersion.MaterialInfoMMS;

namespace Mvc_VD.Services.Implement
{
    public class DevManagementService : DbConnection1RepositoryBase, IdevManagementService
    {
        public DevManagementService(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {

        }

		#region DMS

		#region BOM Management

		public async Task<IEnumerable<BomInfo>> GetListBom()
		{
			try
			{
				var query = @"Select * from d_bom_info Order by bom_no";
				var result = await base.DbConnection.QueryAsync<BomInfo>(query);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<int> DelMaterialBom(int Id)
		{
			try
			{
				var query = @"DELETE FROM materialbom WHERE id=@Id ";
				var result = await base.DbConnection.ExecuteAsync(query, new { id = @Id });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> InsertToMaterialBom(BomCreateMaterialReponse item)
		{
			try
			{
				var query = @"Insert Into materialbom (productCode, materialprarent, materialno, CreateId, CreateDate, ChangeId, ChangeDate)
							VALUES (@productCode, @materialprarent, @materialno, @CreateId, @CreateDate, @ChangeId, @ChangeDate)
							Select Scope_Identity()";
				var result = await base.DbConnection.ExecuteAsync(query, item);
				return result;
			}
			catch (Exception e)
			{
				throw;
			}
		}

		public async Task<BomInfo> CheckBomExist(string style_no, string mt_no)
		{
			try
			{
				var query = @"SELECT * FROM d_bom_info WHERE style_no = @styleNo and mt_no = @Mt_No";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<BomInfo>(query, new { @styleNo = style_no, @Mt_No = mt_no });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<BomResponse> GetBomManagementById(int bid)
		{
			try
			{
				var query = @"SELECT 
								(a.bid) AS bid,
								(b.md_cd) AS md_cd,
								(b.style_nm ) AS style_nm,
								(a.reg_dt) AS reg_dt, 
								(b.style_no) AS style_no, 
								(a.IsApply) as IsApply,
								(a.IsActive) as IsActive,
								(a.cav) As cav,
								(a.bom_no) As bom_no, 
								(a.style_no) As style_no,
								(a.mt_no) As mt_no,
								(c.mt_nm) As mt_nm,
								 (a.need_m) As need_m,
								(a.need_time) As need_time,
								 (a.buocdap) As buocdap,
								(a.active) As active
								FROM d_bom_info AS a JOIN d_style_info AS b ON a.style_no=b.style_no join d_material_info as c on a.mt_no = c.mt_no
								Where a.bid = @BomId		
								";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<BomResponse>(query, new { @BomId = bid });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IEnumerable<BomResponse>> GetListBomManagement(string bom_no, string product, string product_name, string md_cd, string mt_no, string mt_name)
		{
			try
			{

				var query = @"SELECT MAX(a.bid) AS bid,max(b.md_cd) AS md_cd,max(b.style_nm ) AS style_nm,MAX(b.reg_dt) AS reg_dt, MAX(b.reg_id) AS reg_id  , MAX(b.style_no) AS style_no, max(a.IsApply) as IsApply
							FROM d_bom_info AS a 
							JOIN d_style_info AS b ON a.style_no=b.style_no
							Where	(@BomNo = '' OR @BomNo IS NULL OR a.bom_no LIKE '%'+@BomNo+'%')
									And (@Product = '' OR @Product IS NULL OR a.style_no LIKE '%'+@Product+'%')
									And (@ProductName = '' OR @ProductName IS NULL OR b.style_nm LIKE '%'+@ProductName+'%')
									And (@Md_Cd = '' OR @Md_Cd IS NULL OR b.md_cd LIKE '%'+@Md_Cd+'%')
									And (@Mt_No = '' OR @Mt_No IS NULL OR a.mt_no LIKE '%'+@Mt_No+'%')
									And (@Mt_Nm = '' OR @Mt_Nm IS NULL OR (SELECT mt_nm FROM d_material_info WHERE mt_no=a.mt_no AND active = 1) LIKE '%'+ @Mt_Nm + '%')
							GROUP BY a.style_no";
				var result = await base.DbConnection.QueryAsync<BomResponse>(query, new { @BomNo = bom_no, @Product = product, @ProductName = product_name, @Md_Cd = md_cd, @Mt_No = mt_no, @Mt_Nm = mt_name });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IEnumerable<GetMaterialChildForBomResponse>> GetListBomManagement(string bom_no, string styleNo, string product, string product_name, string md_cd, string mt_no, string mt_name)
		{
			try
			{
				var query = @"SELECT a.*, b.md_cd, b.style_nm,(SELECT Top 1 mt_nm FROM d_material_info WHERE mt_no=a.mt_no AND active = 1) mt_nm
							FROM d_bom_info AS a 
							JOIN d_style_info AS b ON a.style_no = b.style_no 
							Where (@BomNo = '' OR @BomNo IS NULL OR a.bom_no LIKE '%'+ @BomNo +'%')
								And a.style_no = @StyleNo 
								And (@Product = '' OR @Product IS NULL OR a.style_no LIKE '%'+ @Product +'%')
								And (@ProductName = '' OR @ProductName IS NULL OR b.style_nm LIKE '%'+ @ProductName +'%')
								And (@Md_Cd = '' OR @Md_Cd IS NULL OR b.md_cd LIKE '%'+ @Md_Cd +'%')
								And (@Mt_No = '' OR @Mt_No IS NULL OR a.mt_no LIKE '%'+ @Mt_No +'%')
								And (@Mt_Nm = '' OR @Mt_Nm IS NULL OR (SELECT Top 1 mt_nm FROM d_material_info WHERE mt_no=a.mt_no AND active = 1) LIKE '%'+ @Mt_Nm +'%')
							Order by a.bid DESC";
				var result = await base.DbConnection.QueryAsync<GetMaterialChildForBomResponse>(query, new { @BomNo = bom_no, @StyleNo = styleNo, @Product = product, @ProductName = product_name, @Md_Cd = md_cd, @Mt_No = mt_no, @Mt_Nm = mt_name });
                foreach (var item in result)
                {
					if(item.IsActive == true)
                    {
						item.isactive = 1;
                    }
                    else
                    {
						item.isactive = 0;

					}
                }
				return result;
			}
			catch (Exception e)
			{

				throw e;
			}
		}

		public async Task<IEnumerable<MaterialBomResponse>> GetListMaterialForBom(int id)
		{
			try
			{
				var query = @"SELECT a.id As MaterialBOMID, active,
							(SELECT mt_no FROM d_material_info WHERE mtid = a.id AND active = 1) AS materialNo,
							(SELECT mt_nm FROM d_material_info WHERE mtid = a.id AND active = 1) AS materialName
							FROM materialbom a
							where a.id= @Id order by a.id desc";
				var result = await base.DbConnection.QueryAsync<MaterialBomResponse>(query, new { @Id = id });
				return result;
			}
			catch (Exception e)
			{
				throw;
			}
		}

		public async Task<IEnumerable<BomCreateMaterialReponse>> GetListMaterialForBom(string style_no, string mt_no)
		{
			try
			{
				var query = @"SELECT *,
							(SELECT mt_nm FROM d_material_info WHERE mt_no = a.MaterialNo AND active = 1) AS materialName
							FROM materialbom a
							where a.ProductCode = @productCode and a.MaterialPrarent = @materialParent  Order By a.id desc";
				var result = await base.DbConnection.QueryAsync<BomCreateMaterialReponse>(query, new { @productCode = style_no, @materialParent = mt_no });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> CheckBom(string style_no, string mt_no, int bid)
		{
			try
			{
				var query = @"SELECT Count(*) FROM d_bom_info WHERE style_no = @StyleNo AND mt_no = @Mt_No AND bid != @Id";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @StyleNo = style_no, @Mt_No = mt_no, @Id = bid });
				return result;
			}
			catch (Exception e)
			{

				throw e;
			}
		}

		public async Task<int> CheckBom(string styleNo)
		{
			try
			{
				var query = @"SELECT Count(*) FROM d_bom_info WHERE style_no = @StyleNo";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @StyleNo = styleNo });
				return result;
			}
			catch (Exception e)
			{

				throw e;
			}
		}

		public async Task<int> CheckBom(string bomNo, string mtNo)
		{
			try
			{
				var query = @"SELECT Count(*) FROM d_bom_info WHERE bom_no = @BomNo And mt_no = @Mt_No";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @BomNo = bomNo, @Mt_No = mtNo });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> CheckBom()
		{
			try
			{
				var query = @"SELECT Count(*) FROM d_bom_info";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> CheckExistedBonInfo(string productCode, string materialNo)
		{
			try
			{
				var query = @"SELECT Count(*) FROM d_bom_info  WHERE style_no = @ProductCode AND mt_no = @Mt_No";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @ProductCode = productCode, @Mt_No = materialNo });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<BomInfo> GetBomInfo(int id)
		{
			try
			{
				var query = @"SELECT * FROM d_bom_info  WHERE bid = @Id";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<BomInfo>(query, new { @Id = id });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}


		public async Task<int> UpdateBomToCalculatePerformance(string product)
		{
			try
			{
				var query = @"Update d_bom_info SET IsActive = 0 WHERE style_no = @Product";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Product = product });
				return result;
			}
			catch (Exception e)
			{
				throw;
			}
		}

		public async Task<IEnumerable<BomInfo>> GetListBomÌnfo(int id)
		{
			try
			{
				//,(SELECT mt_nm from d_material_info where a.mt_no = mt_no limit 1) as mt_nm
				var query = @"SELECT a.*,b.md_cd,b.style_nm,(SELECT Top 1 mt_nm from d_material_info where a.mt_no = mt_no AND active = 1) As mt_nm
								FROM d_bom_info AS a
								JOIN d_style_info AS b ON a.style_no=b.style_no     
								WHERE a.bid = @Id";
				var result = await base.DbConnection.QueryAsync<BomInfo>(query, new { @Id = id });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> InsertToBomInfo(BomInfo item)
		{
			try
			{
				var query = @"INSERT INTO d_bom_info(bom_no, style_no, mt_no, need_time, cav, need_m, buocdap, del_yn, isapply, IsActive, reg_id, reg_dt, chg_id, chg_dt)
								VALUES (@bom_no, @style_no, @mt_no, @need_time, @cav, @need_m, @buocdap, @del_yn, 'N', @IsActive, @reg_id, @reg_dt, @chg_id, @chg_dt);
								SELECT SCOPE_IDENTITY()";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> UpdateBomInfo(BomInfo item)
		{
			try
			{
				var query = @"UPDATE d_bom_info SET bom_no = @bom_no, style_no = @style_no, mt_no = @mt_no, need_time = @need_time, cav = @cav, need_m = @need_m, buocdap = @buocdap, 
								del_yn = @del_yn, isapply = @isapply, IsActive = @IsActive, reg_id = @reg_id, reg_dt = @reg_dt, chg_id = @chg_id, chg_dt =  @chg_dt
								Where bid = @bid";
				var result = await base.DbConnection.ExecuteAsync(query, item);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> DeleteBomInfo(string style_no)
		{
			try
			{
				var query = @"Delete d_bom_info where style_no = @StyleNo";
				var result = await base.DbConnection.ExecuteAsync(query, new { @StyleNo = style_no });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<int> DelBomMaterial(int bid)
		{
			try
			{
				var query = @"Delete d_bom_info where bid = @bid";
				var result = await base.DbConnection.ExecuteAsync(query, new { bid = bid });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<int> CheckMaterialCode(string mt_cd)
		{
			try
			{
				var query = @"Select Count(*) from w_material_info_tam where mt_cd = @Mt_Cd";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Mt_Cd = mt_cd });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> CheckExistMaterialInBom(string mt_no)
		{
			try
			{
				var query = @"Select Count(*) From d_bom_info where mt_no = @Mt_No";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Mt_No = mt_no });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		#endregion

		#region Material Management

		public async Task<int> UpdateQtyWMaterailTam(double gr_qty, double real_qty, string chg_id, DateTime chg_dt, string id)
		{
			try
			{
				//var result = 0;
    //            foreach (var item in id)
    //            {
					var query = $"Update w_material_info_tam SET gr_qty= '{gr_qty}' , real_qty = '{real_qty}', chg_id = '{chg_id}', chg_dt = '{chg_dt}'                              WHERE mt_cd in ({id})  ";
					var result = await base.DbConnection.ExecuteAsync(query/*, new { @Gr_Qty = gr_qty, @Real_Qty = real_qty, @Chg_Id = chg_id, @Chg_dt = chg_dt, @Id = item }*/);
				//}
				return result;
			}
			catch (Exception e)
			{

				throw e;
			}
		}
		public async Task<d_material_info> CheckBarCode(string mt_no)
		{
			try
			{
				var query = @"select * from d_material_info a where a.mt_no = @mt_no  AND active = 1";
					var result = await base.DbConnection.QueryFirstOrDefaultAsync<d_material_info>(query, new { @mt_no = mt_no });
					return result;
			}
			catch (Exception e)
			{

				throw e;
			}
		}
		public async Task<IReadOnlyList<MaterialInfo>> GetListDMaterialInfo(string type, string name, string code, string start, string end, string sp)
		{

			string sql = @"SELECT
				a.barcode,
				a.mtid,
				a.mt_cd,
				a.thick,
				a.thick_unit,
				a.stick,
				a.stick_unit,
				a.tot_price,
				a.price_least_unit,
				a.area,
				a.area_unit,
				a.item_vcd,
				a.qc_range_cd,
				( SELECT dt_nm FROM comm_dt WHERE mt_cd = 'COM004' AND dt_cd = mt_type ) mt_type_nm,
				a.mt_type,
				a.bundle_qty,
				a.bundle_unit,
				a.mt_no,
				a.mt_nm,
				a.unit_cd,
				a.mf_cd,
				a.sp_cd,
				a.mt_no_origin,
				a.s_lot_no,
				a.width,
				a.gr_qty,
				a.width_unit,
				a.spec,
				a.spec_unit,
				a.price,
				a.price_unit,
				a.photo_file,
				a.re_mark,
				a.reg_id,
				a.reg_dt,
				a.chg_id,
				a.chg_dt,
				(a.width + ' ' +  a.width_unit) As new_width,
				a.consum_yn consumable,
				( CASE WHEN a.price IS NULL THEN '' ELSE a.price END ) AS new_price,
				( CASE WHEN a.spec IS NULL THEN '' ELSE a.spec END ) AS new_spec,
				( CASE WHEN a.width IS NULL THEN '' ELSE a.width END ) AS new_with,
				( CASE WHEN a.area IS NULL THEN '' ELSE a.area END ) AS area_all,
				( CASE WHEN a.tot_price IS NULL THEN '' ELSE a.tot_price END ) AS tot_price_new,
				( CASE WHEN a.stick IS NULL THEN '' ELSE a.stick END ) AS stick_new,
				( CASE WHEN a.thick IS NULL THEN '' ELSE a.thick END ) AS thick_new,
				(
				CASE
			
					WHEN ( a.item_vcd IS NULL OR a.item_vcd = '' ) THEN
					'' ELSE ( SELECT item_nm FROM qc_item_mt WHERE item_vcd = a.item_vcd ) 
				END 
				) AS item_nm,
				(
				CASE
				
						WHEN ( a.qc_range_cd IS NULL OR a.qc_range_cd = '' ) THEN
						'' ELSE ( SELECT dt_nm FROM comm_dt WHERE dt_cd = a.qc_range_cd AND mt_cd = 'COM017' ) 
					END 
					) AS qc_range_cd_nm,
					(
					CASE
					
							WHEN ( a.bundle_unit IS NULL OR a.bundle_unit = '' ) THEN
							'' ELSE ( SELECT dt_nm FROM comm_dt WHERE dt_cd = a.bundle_unit AND use_yn = 'Y' AND mt_cd = 'COM027' ) 
						END 
						) AS bundle_unit_nm
					FROM
						d_material_info AS a 
					WHERE
					a.del_yn = 'N' AND a.active = 1
					AND a.mt_type != 'CMT' 
					AND ( @nm = '' OR @nm IS NULL OR a.mt_nm LIKE '%'+@nm+'%' ) 
					AND ( @tp = '' OR @tp IS NULL OR a.mt_type LIKE '%'+@tp+'%' ) 
					AND ( @cd = '' OR @cd IS NULL OR a.mt_no like '%'+@cd+'%' ) 
					AND ( @spp = '' OR @spp IS NULL OR a.sp_cd LIKE '%'+@spp+'%' ) 
					AND (@st = '' OR @st IS NULL OR CONVERT(datetime,a.reg_dt,103) >= CONVERT(datetime,@st,103))
					AND (@en = '' OR @en IS NULL OR CONVERT(datetime,a.reg_dt,103) <= CONVERT(datetime,@en,103))
					order by reg_dt DESC";
			var result = await base.DbConnection.QueryAsync<MaterialInfo>(sql, new { @nm = name, @tp = type, @cd = code, @spp = sp, @st = start, @en = end });

			return result.ToList();

		}
		public async Task<int> InsertDMaterialInfo(MaterialInfo item)
		{
            try
            {
				string sql = @"INSERT INTO d_material_info(mt_type,mt_no,mt_cd,mt_no_origin,mt_nm,mf_cd,gr_qty,unit_cd,bundle_qty,bundle_unit,sp_cd,s_lot_no,
								item_vcd,qc_range_cd,width,width_unit,spec,spec_unit,area,area_unit,thick,thick_unit,stick,stick_unit,consum_yn,price,tot_price
								,price_unit,price_least_unit,photo_file,re_mark,use_yn,del_yn,barcode ,reg_id,chg_id)
								VALUES(@mt_type,@mt_no,@mt_cd,@mt_no_origin,@mt_nm,@mf_cd,@gr_qty,@unit_cd,@bundle_qty,@bundle_unit,@sp_cd,@s_lot_no,
								@item_vcd,@qc_range_cd,@width,@width_unit,@spec,@spec_unit,@area,@area_unit,@thick,@thick_unit,@stick,@stick_unit,@consum_yn,@price,@tot_price
								,@price_unit,@price_least_unit,@photo_file,@re_mark,@use_yn,@del_yn,@barcode ,@reg_id,@chg_id)
							Select Scope_Identity()";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(sql, item);
				return result;
			}
            catch (Exception e)
            {
                throw e;
            }
		}
		public async Task<int> UpdateDMaterialInfo(MaterialInfo item)
		{
			try
			{
				string sql = @"UPDATE d_material_info SET mt_type = @mt_type,mt_no = @mt_no,mt_cd = @mt_cd,mt_no_origin = @mt_no_origin,mt_nm = @mt_nm,mf_cd = @mf_cd,
								gr_qty=@gr_qty,unit_cd = @unit_cd,bundle_qty = @bundle_qty,bundle_unit= @bundle_unit,sp_cd= @sp_cd,s_lot_no = @s_lot_no,
								item_vcd=@item_vcd,qc_range_cd = @qc_range_cd,width = @width,width_unit = @width_unit,spec = @spec,spec_unit = @spec_unit,area = @area,area_unit = @area_unit,
								thick = @thick,thick_unit = @thick_unit,stick = @stick,stick_unit = @stick_unit,consum_yn = @consum_yn,price = @price,tot_price = @tot_price,
								price_unit = @price_unit,price_least_unit = @price_least_unit,photo_file = @photo_file,re_mark = @re_mark,use_yn = @use_yn,del_yn = @del_yn,barcode = @barcode,
								chg_id = @chg_id,chg_dt = @chg_dt
								WHERE mtid=@mtid";
				return await base.DbConnection.ExecuteAsync(sql, item);
			}
			catch (Exception ex)
			{

				throw;
			}
		}
		public async Task<int> DeleteMaterialInfo(int mtid)
		{
			string sql = @"Update d_material_info SET active = 0, del_yn = 'Y' where mtid=@Mtid";
			return await base.DbConnection.ExecuteAsync(sql, new { Mtid = mtid });
		}
		public async Task<int> InsertTmpCreateQRForMaterial(TmpCreateMaterialInfo item)
		{
			string sql = @"INSERT INTO tmp_create_material_info(mt_no,export_date,date_of_receipt,month,expiry_date,lot_no,type,lengh,number_qr,number_qr_mapped,status,reg_id)
			VALUES (@mt_no,@export_date,@date_of_receipt,@month,@expiry_date,@lot_no,@type,@lengh,@number_qr,@number_qr_mapped,@status,@reg_id)";
			return await base.DbConnection.ExecuteAsync(sql, item);
		}
		public async Task<int> InsertWMaterialInfoTmp(List<MaterialInfoTam> item)
		{
   //         try
   //         {
			//	var query = @"INSERT INTO w_material_info_tam (mt_type, mt_cd, mt_no, gr_qty, date, expiry_dt, dt_of_receipt, expore_dt, lot_no, mt_barcode, mt_qrcode, 
			//				status, use_yn, reg_id, reg_dt, chg_id, chg_dt,real_qty)
			//			Values (@mt_type, @mt_cd, @mt_no, @gr_qty, @date, @expiry_dt, @dt_of_receipt, @expore_dt, @lot_no, @mt_barcode, @mt_qrcode, 
			//					@status, @use_yn, @reg_id, @reg_dt, @chg_id, @chg_dt, @real_qty);
			//			Select Scope_Identity()";

			//	return await base.DbConnection.ExecuteScalarAsync<int>(query, item);
			//}
   //         catch (Exception e)
   //         {

   //             throw e;
   //         }

			try
			{
				string listItem = JsonConvert.SerializeObject(item);
				var result = await base.DbConnection.ExecuteScalarAsync<int>("EXEC [dbo].[PrintMaterialStamp] @ListItem", new { @ListItem = listItem });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}

		}
		public async Task<int> CheckTmpMaterial(string MtNo)
		{
			string sql = @"Select tmpid From tmp_w_material_info where mt_no=@Mtno and active=1";
			return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { Mtno = MtNo });
		}
		public async Task<IEnumerable<QCItemMaterial>> GetListItemMaterial(string item_type, string del_yn)
		{
			try
			{
				var query = @"Select * from qc_item_mt where item_type = @ItemType And del_yn = @DelYn";
				var result = await base.DbConnection.QueryAsync<QCItemMaterial>(query, new { @ItemType = item_type, @DelYn = del_yn });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<IEnumerable<DMaterialResponse>> SearchMaterial(string type, string name, string code, DateTime? start, DateTime? end, string sp)
		{
			try
			{
				var query = @" SELECT a.barcode,a.mtid, a.mt_cd, a.thick, a.thick_unit, a.stick, a.stick_unit, a.tot_price, a.price_least_unit, a.area, a.area_unit, a.item_vcd, a.qc_range_cd, 
								(select dt_nm from comm_dt where mt_cd='COM004' and dt_cd=mt_type) mt_type_nm, a.mt_type, a.bundle_qty,a.bundle_unit, a.mt_no, a.mt_nm, a.unit_cd, a.mf_cd, a.sp_cd, a.mt_no_origin, a.s_lot_no, a.width, a.gr_qty, a.width_unit, 
								a.spec, a.spec_unit, a.price, a.price_unit, a.photo_file, a.re_mark, a.reg_id, a.reg_dt, a.chg_id, a.chg_dt, a.consum_yn consumable,
								(case when a.price IS NULL then '' ELSE a.price END) AS new_price, 
								(case when a.spec IS NULL then '' ELSE a.spec END) AS new_spec, 
								(case when a.width IS NULL then '' ELSE a.width END) AS new_with,
								(case when a.area IS NULL then '' ELSE a.area END) AS area_all, 
								(case when a.tot_price IS NULL then '' ELSE a.tot_price END) AS tot_price_new, 
								(case when a.stick IS NULL then '' ELSE a.stick END) AS stick_new, 
								(case when a.thick IS NULL then '' ELSE a.thick END) AS thick_new, 
								(case when (a.item_vcd IS NULL OR a.item_vcd = '') then '' ELSE (SELECT item_nm FROM qc_item_mt WHERE item_vcd = a.item_vcd)END) AS item_nm, 
								(case when (a.qc_range_cd IS NULL OR a.qc_range_cd = '') then '' ELSE (SELECT dt_nm FROM comm_dt WHERE dt_cd = a.qc_range_cd AND mt_cd = 'COM017')END) AS qc_range_cd_nm, 
								(case when (a.bundle_unit IS NULL OR a.bundle_unit = '') then '' ELSE (SELECT dt_nm FROM comm_dt WHERE dt_cd = a.bundle_unit AND use_yn = 'Y' AND mt_cd = 'COM027')END) AS bundle_unit_nm, 
								ROW_NUMBER() OVER (ORDER BY a.mtid DESC) AS RowNum 
								FROM d_material_info AS a
								WHERE a.del_yn='N' and a.mt_type!='CMT' and a.active = 1
								 AND (@Name = '' OR @Name IS NULL OR a.mt_nm LIKE '%' + @Name + '%')
								 AND (@Type = '' OR @Type IS NULL OR a.mt_type LIKE '%' + @Type + '%')
								 AND(@Code = '' OR @Code IS NULL OR a.mt_no LIKE '%' + @Code + '%')
								 AND(@SP = '' OR @SP IS NULL OR a.sp_cd LIKE '%'+ @SP +'%')
								 AND(@Start IS NULL OR a.reg_dt >= @Start)
								 AND(@End IS NULL OR a.reg_dt <= @End)
								 ORDER BY a.mtid DESC";
				var result = await base.DbConnection.QueryAsync<DMaterialResponse>(query, new { @Name = name, @Type = type, @Code = code, @SP = sp, @Start = start, @End = end });
				return result;
			}
			catch (Exception e)
			{

				throw e;
			}
		}
		public async Task<MaterialInfo> GetMaterialNoFromMaterialInfo(string mt_no)
		{
			try
			{
				var query = @"Select * from d_material_info where mt_no = @Mt_No And active = 1 And del_yn = 'N'";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfo>(query, new { @Mt_No = mt_no });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<IEnumerable<MaterialInfo>> GetListDataMaterialNoFromMaterialInfo(string mt_no)
		{
			try
			{
				var query = @"Select * from d_material_info where mt_no = @Mt_No And active = 1 And del_yn = 'N'";
				var result = await base.DbConnection.QueryAsync<MaterialInfo>(query, new { @Mt_No = mt_no });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<MaterialInfo> GetDMaterialInfoById(int mtid)
		{
			try
			{
				var query = @"Select * From d_material_info Where mtid = @Id And active = 1 And del_yn = 'N'";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfo>(query, new { @Id = mtid });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<string> GetItemNameByItemvcd(string itemVcd)
		{
            try
            {
				var query = @"Select item_nm From qc_item_mt where item_vcd = @Item";
				var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @Item = itemVcd});
				return result;
			}
            catch (Exception e)
            {
                throw e;
            }
		}
		public async Task<int> CountDMaterialInfo(string mt_no)
		{
            try
            {
				var query = @"Select Count(*) From d_material_info where mt_no = @Mt_No";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Mt_No = mt_no });
				return result;
            }
            catch (Exception e)
            {
                throw e;
            }
		}
		public async Task<IEnumerable<SupplierInfo>> GetListSupplierInfo(string sp_cd)
		{
            try
            {
				var query = @"Select * From supplier_info where sp_cd = @Sp_Cd";
				var result = await base.DbConnection.QueryAsync<SupplierInfo>(query, new { @Sp_Cd = sp_cd });
				return result;
			}
            catch (Exception e)
            {
                throw e;
            }
		}

		public async Task<IEnumerable<SupplierInfo>> GetListSupplierInfoForPopup(string suplier_no, string suplier_nm)
		{
            try
            {
				var query = @" SELECT a.*
                        FROM supplier_info AS a
	                    Where  (@suplier_no = '' OR @suplier_no IS NULL OR  a.sp_cd Like '%' + @suplier_no + '%' )
										AND (@suplier_nm = '' OR @suplier_nm IS NULL OR  a.sp_nm Like '%' + @suplier_nm + '%' )";
				var result = await base.DbConnection.QueryAsync<SupplierInfo>(query, new { suplier_no = suplier_no, suplier_nm = suplier_nm });
				return result;
            }
            catch (Exception e)
            {
                throw e;
            }
		}

		public async Task<comm_dt> GetListComDetailForPopup(string dt_cd)
		{
			try
			{
				var query = @"SELECT Top 1 * FROM comm_dt WHERE dt_cd= @DtCd";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<comm_dt>(query, new { @DtCd = dt_cd});
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IEnumerable<ManufacInfo>> GetListManuFacInfoForPopup()
		{
			try
			{
				var query = @"Select * from manufac_info";
				var result = await base.DbConnection.QueryAsync<ManufacInfo>(query);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IEnumerable<MaterialInfo>> SearchMaterialInfo(string name, string type, string code, string start, string end)
		{
			try
			{
				var query = @" SELECT dmi.*
										FROM d_material_info  as dmi
										Where dmi.del_yn='N'
										AND (@Name = '' OR @Name IS NULL OR  dmi.mt_nm Like '%' + @Name + '%' )
										AND (@Type = '' OR @Type IS NULL OR  dmi.mt_type Like '%' + @Type + '%' )
										AND (@Code = '' OR @Code IS NULL OR  dmi.mt_no Like '%' + @Code + '%')
										AND (@Start = '' OR @Start IS NULL OR dmi.reg_dt >=  @Start )
										AND (@End = '' OR @End IS NULL OR dmi.reg_dt <= @End )";
				var result = await base.DbConnection.QueryAsync<MaterialInfo>(query, new { @Name = name, @Type = type, @Code = code, @Start = start, @End = end });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}


		public async Task<IEnumerable<MaterialInfo>> GetDataMaterialToExport(string name, string type, string code, string sp, string start, string end)
		{
			try
			{
				var query = @" SELECT dmi.*
										FROM d_material_info  as dmi
										Where dmi.del_yn='N' AND dmi.mt_type != 'CMT' 
										AND (@Name = '' OR @Name IS NULL OR  dmi.mt_nm Like '%' + @Name + '%' )
										AND (@Type = '' OR @Type IS NULL OR  dmi.mt_type Like '%' + @Type + '%' )
										AND (@Code = '' OR @Code IS NULL OR  dmi.mt_no Like '%' + @Code + '%')
										AND (@SP = '' OR @SP IS NULL OR dmi.sp_cd LIKE '%' + @SP + '%')
										AND (@Start = '' OR @Start IS NULL OR dmi.reg_dt >=  @Start )
										AND (@End = '' OR @End IS NULL OR dmi.reg_dt <= @End )
										ORDER BY dmi.mt_no DESC";
				var result = await base.DbConnection.QueryAsync<MaterialInfo>(query, new { @Name = name, @Type = type, @Code = code, @SP = sp, @Start = start, @End = end });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}



		public async Task<string> GetNameFromMaterialInfo(string mt_no)
		{
			try
			{
				var query = @"Select mt_nm From d_material_info Where mt_no = @Mt_No And active = 1";
				var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @Mt_No = mt_no });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<IEnumerable<DMaterialResponse>> SearchMaterialRepalce(string type, string name, string code, DateTime? start, DateTime? end, string sp, string MaterialPrarent)
		{


			try
			{
				var query = @" SELECT a.barcode,a.mtid, a.mt_cd, a.thick, a.thick_unit, a.stick, a.stick_unit, a.tot_price, a.price_least_unit, a.area, a.area_unit, a.item_vcd, a.qc_range_cd, 
								(select dt_nm from comm_dt where mt_cd='COM004' and dt_cd=mt_type) mt_type_nm, a.mt_type, a.bundle_qty,a.bundle_unit, a.mt_no, a.mt_nm, a.unit_cd, a.mf_cd, a.sp_cd, a.mt_no_origin, a.s_lot_no, a.width, a.gr_qty, a.width_unit, 
								a.spec, a.spec_unit, a.price, a.price_unit, a.photo_file, a.re_mark, a.reg_id, a.reg_dt, a.chg_id, a.chg_dt, a.consum_yn consumable,
								(case when a.price IS NULL then '' ELSE a.price END) AS new_price, 
								(case when a.spec IS NULL then '' ELSE a.spec END) AS new_spec, 
								(case when a.width IS NULL then '' ELSE a.width END) AS new_with,
								(case when a.area IS NULL then '' ELSE a.area END) AS area_all, 
								(case when a.tot_price IS NULL then '' ELSE a.tot_price END) AS tot_price_new, 
								(case when a.stick IS NULL then '' ELSE a.stick END) AS stick_new, 
								(case when a.thick IS NULL then '' ELSE a.thick END) AS thick_new, 
								(case when (a.item_vcd IS NULL OR a.item_vcd = '') then '' ELSE (SELECT item_nm FROM qc_item_mt WHERE item_vcd = a.item_vcd)END) AS item_nm, 
								(case when (a.qc_range_cd IS NULL OR a.qc_range_cd = '') then '' ELSE (SELECT dt_nm FROM comm_dt WHERE dt_cd = a.qc_range_cd AND mt_cd = 'COM017')END) AS qc_range_cd_nm, 
								(case when (a.bundle_unit IS NULL OR a.bundle_unit = '') then '' ELSE (SELECT dt_nm FROM comm_dt WHERE dt_cd = a.bundle_unit AND use_yn = 'Y' AND mt_cd = 'COM027')END) AS bundle_unit_nm, 
								ROW_NUMBER() OVER (ORDER BY a.mtid DESC) AS RowNum 
								FROM d_material_info AS a
								WHERE a.del_yn='N' and a.mt_type!='CMT' and a.active = 1 and  a.mt_no !=  @MaterialPrarent
								 AND (@Name = '' OR @Name IS NULL OR a.mt_nm LIKE '%' + @Name + '%')
								 AND (@Type = '' OR @Type IS NULL OR a.mt_type LIKE '%' + @Type + '%')
								 AND(@Code = '' OR @Code IS NULL OR a.mt_no LIKE '%' + @Code + '%')
								 AND(@SP = '' OR @SP IS NULL OR a.sp_cd LIKE '%'+ @SP +'%')
								 AND(@Start IS NULL OR a.reg_dt >= @Start)
								 AND(@End IS NULL OR a.reg_dt <= @End)
								 ORDER BY a.mtid DESC";
				var result = await base.DbConnection.QueryAsync<DMaterialResponse>(query, new { @Name = name, @Type = type, @Code = code, @SP = sp, @Start = start, @End = end, MaterialPrarent = MaterialPrarent });
				return result;
			}

			catch (Exception)
			{

				throw;
			}
		}
		public async Task<int> CheckIsExistMaterialRouting(string mt_no)
		{
			try
			{
				string checkExist = @"SELECT id FROM product_material  WHERE mt_no=@mt_no ";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(checkExist, new { @mt_no = mt_no });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> CheckIsExistMaterialRouting2(string mt_no)
		{
			string checkExist = @"SELECT id FROM product_material_detail  WHERE MaterialNo=@mt_no ";
			var result = await base.DbConnection.ExecuteScalarAsync<int>(checkExist, new { @mt_no = mt_no });
			return result;
		}

		public async Task<int> CheckIsExistProductRouting(string style_no)
		{
            try
            {
				string checkExist = @"SELECT idr FROM d_rounting_info  WHERE style_no=@style_no ";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(checkExist, new { @style_no = style_no });
				return result;
			}
			catch(Exception e)
            {
				throw e;
            }
		}

		public async Task<int> CheckIsExistProductBom(string style_no)
		{
			try
			{
				string checkExist = @"SELECT bid FROM d_bom_info  WHERE style_no=@style_no ";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(checkExist, new { @style_no = style_no });
				return result;
			}
			catch(Exception e)
            {
				throw e;
            }
}
		#endregion

		#region Develop Common

		public async Task<IEnumerable<CommMt>> GetListCommMT()
		{
			try
			{
				var query = @"Select * from comm_mt where div_cd = 'DEV'";
				var result = await base.DbConnection.QueryAsync<CommMt>(query);
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<int> DelateMoldInfo(int mdno)
		{
			try
			{
				var query = @" delete d_mold_info where mdno =@mdno ";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @mdno = mdno });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<IEnumerable<DevelopCommonResponse>> GetListDevelopCommonData(string mt_cd)
		{
			try
			{
				var query = @"Select d.cdid, m.mt_cd, m.mt_nm, d.dt_cd, d.dt_nm, d.dt_exp, d.dt_order, d.use_yn
								From comm_dt d JOIN comm_mt m ON d.mt_cd = m.mt_cd
								Where d.mt_cd = @Mt_Cd";
				var result = await base.DbConnection.QueryAsync<DevelopCommonResponse>(query, new { @Mt_Cd = mt_cd });
				return result;
			}
			catch (Exception e)
			{
				throw;
			}
		}

		public async Task<int> InsertIntoComDT(CommCode comm_dt)
		{
			try
			{
				var query = @"Insert into comm_dt(mt_cd, dt_cd, dt_nm, dt_exp, dt_order, use_yn, reg_dt, chg_dt, del_yn)
							 Values(@mt_cd, @dt_cd, @dt_nm, @dt_exp, @dt_order, @use_yn, @reg_dt, @chg_dt, @del_yn)
							 Select Scope_Identity()";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, comm_dt);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> CheckComDT(string dt_cd, string mt_cd)
		{
			try
			{
				var query = @"Select Count(*) From comm_dt where dt_cd = @Dt_Cd And mt_cd = @Mt_Cd";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Mt_Cd = mt_cd, @Dt_Cd = dt_cd });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> CheckComDT(string mt_cd)
		{
			try
			{
				var query = @"Select Count(*) From comm_dt where mt_cd = @Mt_Cd";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Mt_Cd = mt_cd });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> CheckComMT(string mt_cd)
		{
			try
			{
				var query = @"Select Count(*) from comm_mt where mt_cd = @Mt_Cd";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Mt_Cd = mt_cd });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> InsertIntoComMT(CommMt comm_mt)
		{
			try
			{
				var query = @"Insert into comm_mt(mt_cd, div_cd, mt_nm, mt_exp, use_yn, reg_dt, chg_dt)
							 Values(@mt_cd, @div_cd, @mt_nm, @mt_exp, @use_yn, @reg_dt, @chg_dt)
							 Select Scope_Identity()";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, comm_mt);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}


		}

		public async Task<int> DeleteComDT(string mt_cd, string dt_cd)
		{
			try
			{
				var query = @"Delete comm_dt where mt_cd = @Mt_Cd And dt_cd = @Dt_Cd";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Mt_Cd = mt_cd, @Dt_Cd = dt_cd });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> DeleteComDT(string mt_cd)
		{
			try
			{
				var query = @"Delete comm_dt where mt_cd = @Mt_Cd";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Mt_Cd = mt_cd });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> DeleteComMT(string mt_cd)
		{
			try
			{
				var query = @"Delete comm_mt where mt_cd = @Mt_Cd";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Mt_Cd = mt_cd });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<CommCode> GetCommCodeById(int id)
		{
			try
			{
				var query = @"Select *,(Select a.mt_nm from comm_mt As a where a.mt_cd = b.mt_cd) As mt_nm from comm_dt As b where cdid = @Id";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<CommCode>(query, new { @Id = id });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> UpdateComDT(CommCode comDT)
		{
			try
			{
				var query = @"Update comm_dt Set mt_cd = @mt_cd, dt_cd = @dt_cd, dt_nm = @dt_nm, dt_exp = @dt_exp, dt_order = @dt_order, 
								use_yn = @use_yn, reg_dt = @reg_dt, chg_dt = @chg_dt
								Where cdid = @cdid ";
				var result = await base.DbConnection.ExecuteAsync(query, comDT);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<CommMt> GetCommMTById(int id)
		{
			try
			{
				var query = @"Select * from comm_mt where mt_id = @Id";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<CommMt>(query, new { @Id = id });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> UpdateComMT(CommMt comMT)
		{
			try
			{
				var query = @"Update comm_mt Set mt_cd = @mt_cd, mt_nm = @mt_nm, mt_exp = @mt_exp, use_yn = @use_yn, reg_dt = @reg_dt, chg_dt = @chg_dt
								Where mt_id = @mt_id";
				var result = await base.DbConnection.ExecuteAsync(query, comMT);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IEnumerable<ModelInfo>> GetListModelInfo()
		{
			try
			{
				var query = @"Select * from d_model_info";
				var result = await base.DbConnection.QueryAsync<ModelInfo>(query);
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IEnumerable<CommCode>> GetListCommDT(string mt_cd)
		{
			try
			{
				var query = @"Select * from comm_dt where mt_cd = @Mt_Cd";
				var result = await base.DbConnection.QueryAsync<CommCode>(query, new { @Mt_Cd = mt_cd });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IEnumerable<CommCode>> GetListCommDT(string mt_cd, string use_yn)
		{
			try
			{
				var query = @"Select * from comm_dt where mt_cd = @Mt_Cd And use_yn = @UseYn";
				var result = await base.DbConnection.QueryAsync<CommCode>(query, new { @Mt_Cd = mt_cd, @UseYn = use_yn });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		#endregion

		#region Product Management

		public async Task<IReadOnlyList<StyleInfo>> GetListDStyleInfo()
		{
			var sql = @"SELECT s.sid,s.ssver, s.reg_id,s.chg_id,s.reg_dt,s.chg_dt,s.stamp_code, m.stamp_name,s.bom_type,
						s.style_no, s.style_nm, s.md_cd, s.prj_nm, s.pack_amt,s.expiry_month,s.expiry,part_nm, s.drawingname, s.loss , s.Description , s.productType
						FROM d_style_info AS s 
						LEFT JOIN stamp_master AS m ON s.stamp_code = m.stamp_code
						ORDER BY s.sid desc";
			var result = await base.DbConnection.QueryAsync<StyleInfo>(sql);
			return result.ToList();
		}

		public async Task<IEnumerable<StyleInfo>> searchStyleInfo(string code, string codeName, string modelCode, string projectName, string start, string end)
		{
			try
			{
				//	AND ((@start = '' And @end = '') Or (@start IS NULL And @end IS NULL) Or (a.reg_dt Between @start And @end))
				var query = @"SELECT (SELECT dt_nm FROM comm_dt WHERE a.part_nm = dt_cd and mt_cd = 'DEV003') AS part_name,a.* ,m.stamp_name
								FROM  d_style_info as a 
								LEFT JOIN stamp_master AS m ON a.stamp_code = m.stamp_code
								Where (@code='' OR @code IS NULL Or  a.style_no like '%'+@code+'%')
								AND (@codeName='' OR @codeName IS NULL Or a.style_nm like '%'+ @codeName +'%')
								AND (@modeCode='' OR @modeCode IS NULL Or a.md_cd like '%'+ @modeCode+'%')
								AND (@projectName='' OR @projectName IS NULL Or a.prj_nm like '%'+ @projectName+'%')
								AND (@start='' OR @start IS NULL Or a.reg_dt >= @start)
								AND (@end='' OR @end IS NULL Or a.reg_dt <= @end)
							    ORDER BY a.chg_dt desc";
				var result = await base.DbConnection.QueryAsync<StyleInfo>(query, new
				{
					@code = code,
					@codeName = codeName,
					@modeCode = modelCode,
					@projectName = projectName,
					@start = start,
					@end = end
				});
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<d_style_info> GetStyleInfo(int id)
		{
			try
			{
				var query = @"Select * from d_style_info where sid = @Id";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<d_style_info>(query, new { @Id = id });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> RemoveStyleInfo(int id)
		{
			try
			{
				var query = @"Delete d_style_info where sid = @Id";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Id = id });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> InsertIntoStyleInfo(d_style_info item)
		{
			try
			{
				var query = @"Insert into d_style_info (style_no, style_nm, md_cd, prj_nm, ssver, part_nm, standard, cust_rev, order_num, pack_amt, cav, bom_type, tds_no, item_vcd, 
							qc_range_cd,drawingname, stamp_code, expiry_month, expiry, use_yn, del_yn, reg_id, reg_dt, chg_id, chg_dt,loss,Description,productType)

							Values (@style_no, @style_nm, @md_cd, @prj_nm, @ssver, @part_nm, @standard, @cust_rev, @order_num, @pack_amt, @cav, @bom_type, @tds_no, @item_vcd, 
							@qc_range_cd,@drawingname, @stamp_code, @expiry_month, @expiry, @use_yn, @del_yn, @reg_id, @reg_dt, @chg_id, @chg_dt, @loss, @Description, @productType)
							select scope_identity()";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> UpdateStyleInfo(d_style_info item)
		{
			try
			{
				var query = @"Update d_style_info SET style_no = @style_no, style_nm = @style_nm, md_cd = @md_cd, prj_nm = @prj_nm, ssver = @ssver, part_nm = @part_nm, standard = @standard, 
							cust_rev = @cust_rev, order_num = @order_num, pack_amt = @pack_amt, cav = @cav, bom_type = @bom_type, tds_no = @tds_no, item_vcd = @item_vcd, qc_range_cd = @qc_range_cd, 
							stamp_code = @stamp_code, expiry_month = @expiry_month, expiry = @expiry, use_yn = @use_yn, del_yn = @del_yn, reg_id = @reg_id, reg_dt = @reg_dt, chg_id = @chg_id, chg_dt = @chg_dt ,drawingname=@drawingname,loss=@loss,Description=@Description, productType = @productType
							Where sid = @sid";
				var result = await base.DbConnection.ExecuteAsync(query, item);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<StyleInfo> GetStyleNoFromStyleInfo(string style_no)
		{
			try
			{
				var query = @"Select * from d_style_info where style_no = @StyleNo";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<StyleInfo>(query, new { @StyleNo = style_no });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> InsertProductExcel()
		{
			try
			{
				var query = @" INSERT INTO d_model_info(md_cd, md_nm, reg_dt, chg_dt, use_yn, del_yn) 
							   SELECT a.md_cd, a.md_cd, GetDate(), GetDate(),'Y','N' FROM ( SELECT 
																					d_style_info.md_cd AS md_cd
																				FROM
																					d_style_info
																				WHERE
																					d_style_info.md_cd IN (SELECT d_model_info.md_cd FROM d_model_info)) AS a";
				var result = await base.DbConnection.ExecuteAsync(query);
				return result;

			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IEnumerable<ModelReturnProductExcel>> GetListProductExcel(string style_no)
		{
			try
			{
				var query = @"SELECT s.sid, s.style_no, s.style_nm, s.md_cd, s.prj_nm, s.pack_amt, s.chg_id, s.chg_dt, s.reg_id, s.reg_dt, 'INSERT' AS STATUS
                                 FROM  d_style_info as s 
                                 Left Join comm_dt as m on s.qc_range_cd = m.dt_cd and m.mt_cd = 'COM017' 
								 WHERE s.style_no = @StyleNo
								 Order by s.sid desc";
				var result = await base.DbConnection.QueryAsync<ModelReturnProductExcel>(query, new { @StyleNo = style_no });
				return result;
			}
			catch (Exception e)
			{

				throw e;
			}
		}
		#endregion

		#region Model Management

		public async Task<IEnumerable<ModeCodeResponse>> SearchModeCode(string md_cd, string md_nm)
		{
			try
			{
				var query = @"SELECT md_cd, md_nm FROM d_model_info 
							 Where (@modelCode ='' OR @modelCode IS NULL OR md_cd like '%'+ @modelCode +'%' )
							 AND (@modelName ='' OR @modelName IS NULL OR md_nm like '%'+ @modelName +'%' )";
				var result = await base.DbConnection.QueryAsync<ModeCodeResponse>(query, new { @modelCode = md_cd, @modelName = md_nm });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IEnumerable<ModelReturnModelExcel>> GetListModelExcel(string md_cd)
		{
			try
			{
				var query = @"SELECT a.mdid,a.md_cd,a.md_nm,a.use_yn,a.reg_id,reg_dt,a.chg_id,chg_dt,'INSERT' AS STATUS 
							FROM d_model_info a 
							WHERE a.md_cd = @Md_Cd
							ORDER BY a.chg_dt ";
				var result = await base.DbConnection.QueryAsync<ModelReturnModelExcel>(query, new { @Md_Cd = md_cd });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> CheckModelInfo(string md_cd)
		{
			try
			{
				var query = @"Select Count(*) from d_model_info where md_cd = @ModelCode";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @ModelCode = md_cd });
				return result;
			}
			catch (Exception e)
			{

				throw;
			}
		}

		public async Task<int> InsertModelInfo(ModelInfo item)
		{
			try
			{
				var query = @"Insert into d_model_info(md_cd, md_nm, use_yn, del_yn, reg_id, reg_dt, chg_id, chg_dt)
							Values(@md_cd, @md_nm, @use_yn, @del_yn, @reg_id, @reg_dt, @chg_id, @chg_dt)
							select scope_identity()";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<ModelInfo> GetModelInfoById(int mdid)
		{
			try
			{
				var query = @"Select * from d_model_info where mdid = @Id";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<ModelInfo>(query, new { @Id = mdid });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> RemoveModelInfo(int mdid)
		{
			try
			{
				var query = @"Delete d_model_info where mdid = @Id";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Id = mdid });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> UpdateModelInfo(ModelInfo item)
		{
			try
			{
				var query = @"Update d_model_info SET md_cd = @md_cd, md_nm = @md_nm, use_yn = @use_yn, del_yn = @del_yn, reg_id = @reg_id, 
							reg_dt = @reg_dt, chg_id = @chg_id, chg_dt = @chg_dt Where mdid = @mdid";
				var result = await base.DbConnection.ExecuteAsync(query, item);
				return result;

			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IEnumerable<ModelInfo>> GetListModelInfo(string modelCode, string modelName)
		{
			try
			{
				var query = @"SELECT * FROM d_model_info
							WHERE (@modelCode='' OR @modelCode IS NULL OR md_cd like '%'+ @modelCode +'%')
							AND (@modelName='' OR @modelName IS NULL OR md_nm like '%' + @modelName + '%' )
							ORDER BY chg_dt DESC";
				var result = await base.DbConnection.QueryAsync<ModelInfo>(query, new { @modelCode = modelCode, @modelName = modelName });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IEnumerable<QCItemMaterial>> GetListQCItemMaterial(string item_type, string del_yn)
		{
			try
			{
				var query = @"Select * from qc_item_mt where item_type = @ItemType and del_yn = @DelYn";
				var result = await base.DbConnection.QueryAsync<QCItemMaterial>(query, new { @ItemType = item_type, @DelYn = del_yn });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		#endregion

		#region Routing
		public async Task<int> CheckProductInStyleInfo(string style_no)
		{
			try
			{
				var query = @"Select Count(*) from d_style_info where style_no = @StyleNo";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @StyleNo = style_no });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IEnumerable<DRoutingInfo>> GetListRoutingInfoByStyleNo(string style_no, string process_code)
		{
			try
			{
				var query = @"Select * from d_rounting_info where style_no = @Style_No and process_code = @ProcessCode ";
				var result = await base.DbConnection.QueryAsync<DRoutingInfo>(query, new { @Style_No = style_no, ProcessCode = process_code });
				return result.ToList();
			}
			catch (Exception e)
			{

				throw e;
			}
		}

		public async Task<int> InsertRoutingInfo(DRoutingInfo item)
		{
			try
			{
				var query = @"Insert into d_rounting_info (style_no, name,process_code, level, don_vi_pr, type, item_vcd, description,isFinish, reg_dt, reg_id, chg_id, chg_dt)
								Values (@style_no, @name,@process_code, @level, @don_vi_pr, @type, @item_vcd, @description,@isFinish, @reg_dt, @reg_id, @chg_id, @chg_dt)
							select scope_identity()";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
				return result;
			}
			catch (Exception E)
			{

				throw;
			}
		}

		public async Task<int> UpdateRoutingInfo(DRoutingInfo item, string description, string isFinish)
		{
			try
			{
				var query = @"Update d_rounting_info Set name = @name, level = @level, don_vi_pr = @don_vi_pr, type = @type, 
								item_vcd = @item_vcd, isFinish = @isFinish, description = @description, chg_id = @chg_id, chg_dt = @chg_dt
								Where idr = @idr";
				var result = await base.DbConnection.ExecuteAsync(query, item);
				return result;
			}
			catch (Exception e)
			{

				throw;
			}
		}

		public async Task<int> DeleteRoutingInfo(int idr)
		{
			try
			{
				var query = @"Delete d_rounting_info where idr = @Id";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Id = idr });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<CommCode> GetUnitFromCom_DT(string mt_cd, string unit)
		{
			try
			{
				var query = @"Select * from comm_dt where mt_cd = @Mt_Cd and dt_cd = @Unit";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<CommCode>(query, new { @Mt_Cd = mt_cd, @Unit = unit });
				return result;
			}
			catch (Exception e)
			{

				throw e;
			}
		}

		public async Task<string> GetDetailNameFromCom_DT(string mt_cd, string unit)
		{
			try
			{
				var query = @"Select dt_nm from comm_dt where mt_cd = @Mt_Cd and dt_cd = @Unit And use_yn = 'Y'";
				var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @Mt_Cd = mt_cd, @Unit = unit });
				return result;
			}
			catch (Exception e)
			{

				throw e;
			}
		}

		public async Task<DRoutingInfo> GetRoutingInfoById(int id)
		{
			try
			{
				var query = @"Select * from d_rounting_info where idr = @Id";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<DRoutingInfo>(query, new { @Id = id });
				return result;
			}
			catch (Exception e)
			{

				throw e;
			}
		}

		public async Task<int> CheckExistsProductMaterial1(string style_no, int level, string mt_no, string process_code)
		{
			try
			{
				var query = @"SELECT Count(*) FROM product_material  WHERE style_no = @Style_No and process_code = @ProcessCode and level = @Level AND mt_no = @Mt_No ";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Style_No = style_no, @Level = level, @Mt_No = mt_no , @ProcessCode  = process_code });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<IEnumerable<ProductMaterialDetail>> CheckExistsProductMaterialDetail(string productCode, string materialParent, List<string> materialNo)
		{
            try
            {
                var result = new List<ProductMaterialDetail>();

                foreach (var item in materialNo)
                {
					 var query = @"SELECT * FROM product_material_detail Where ProductCode = @ProductCode and MaterialParent = @MaterialParent AND MaterialNo = @Mt_No";
					 var rs = await base.DbConnection.QueryFirstOrDefaultAsync<ProductMaterialDetail>(query, new { @ProductCode = productCode, @MaterialParent = materialParent, @Mt_No = item });
					if(rs != null)
                    {
						result.Add(rs);
					}

				}
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}


		public async Task<IEnumerable<MaterialBom>> ListExistsMaterialBomDetail(string productCode, string materialParent, List<string> materialNo)
		{
			try
			{
				var result = new List<MaterialBom>();
				foreach (var item in materialNo)
				{
					var query = @"SELECT * FROM materialbom Where ProductCode = @ProductCode and MaterialPrarent = @MaterialParent AND MaterialNo = @Mt_No";
					var rs = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialBom>(query, new { @ProductCode = productCode, @MaterialParent = materialParent, @Mt_No = item });
					if(rs != null)
                    {
						result.Add(rs);
                    }
				}
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}


		public async Task<int> CheckExistsProductMaterial2(string style_no, int level, string mt_no, int Id, string process_code)
		{
			try
			{
				var query = @"SELECT Count(*) FROM product_material  WHERE style_no = @Style_No and level = @Level AND mt_no = @Mt_No And Id != @Id and process_code = @ProcessCode";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Style_No = style_no, @Level = level, @Mt_No = mt_no, @Id = Id, ProcessCode = process_code });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> CheckExistsProductMaterial3(string style_no, string name, string process_code)
		{
			try
			{
				var query = @"SELECT Count(*) FROM product_material  WHERE style_no = @Style_No And name = @Name And process_code = @process_code";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Style_No = style_no, @Name = name, @process_code = process_code });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> UpdateMaterialToCalculatePerformance(string product, string process_code)
		{
			try
			{
				var query = @"Update product_material SET use_yn = 'N' WHERE style_no = @Product and process_code = @ProcessCode;";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Product = product, ProcessCode = process_code });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<string> GetMaterialName(string mt_no)
		{
			try
			{
				var query = @"Select mt_nm from d_material_info where mt_no = @Mt_No";
				var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @Mt_No = mt_no });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<ProductMaterial> GetProductMaterialById(int Id)
		{
			try
			{
				var query = @"SELECT * FROM product_material AS a WHERE a.Id = @Id";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<ProductMaterial>(query, new { @Id = Id });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<d_style_info> GetProductByCode(string code)
		{
			try
			{
				var query = @"SELECT * FROM d_style_info a LEFT JOIN stamp_master AS m ON a.stamp_code = m.stamp_code WHERE a.style_no= @code";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<d_style_info>(query, new { @code = code });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> InsertIntoProductMaterial(ProductMaterial item)
		{
			try
			{
				var query = @"Insert into product_material(style_no,process_code, level, mt_no, name, need_time, cav, need_m, buocdap, use_yn, reg_id, reg_dt, chg_id, chg_dt)
							  Values(@style_no,@process_code, @level, @mt_no, @name, @need_time, @cav, @need_m, @buocdap, @use_yn, @reg_id, @reg_dt, @chg_id, @chg_dt)
							  select scope_identity()";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> UpdateProductMaterial(ProductMaterial item)
		{
			try
			{
				var query = @"Update product_material Set  level = @level, mt_no = @mt_no, need_time = @need_time, cav = @cav, 
								need_m = @need_m, buocdap = @buocdap, use_yn = @use_yn, reg_id = @reg_id, reg_dt = @reg_dt, chg_id = @chg_id, chg_dt = @chg_dt
								Where Id = @Id";
				var result = await base.DbConnection.ExecuteAsync(query, item);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> DeleteProductMaterial(int id)
		{
			try
			{
				var query = @"Delete from product_material where Id = @Id";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Id = id });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> DeleteProductMaterialDetail(string ProductCode, string process_code, string name,string MaterialPrarent)
		{
			try
			{
				var query = @"DELETE FROM product_material_detail WHERE ProductCode = @productCode and process_code = @process_code and name = @name and MaterialParent = @materialPrarent";
				var result = await base.DbConnection.ExecuteAsync(query, new { @productCode = ProductCode, @process_code = process_code, @name= name, @materialPrarent = MaterialPrarent });
				return result;
			}
			catch (Exception e)
			{

				throw e;
			}
		}

		public async Task<int> DeleteMaterialChild(int id)
		{
			try
			{
				var query = @"DELETE FROM product_material_detail WHERE Id = @Id";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Id = id });
				return result;
			}
			catch (Exception e)
			{

				throw e;
			}
		}

		public async Task<IEnumerable<ProductMaterialDetail>> GetListMaterialChild(string ProductCode, string name, string MaterialPrarent, string process_code)
		{
			try
			{
				var query = @"SELECT a.Id As id, a.ProductCode,a.level,a.MaterialParent,a.MaterialNo, a.CreateId, a.ChangeId, a.CreateDate, a.ChangeDate, b.mt_nm AS MaterialName
							FROM product_material_detail a
							Join d_material_info b On a.MaterialNo = b.mt_no
							WHERE ProductCode = @productCode and a.name = @name and a.MaterialParent = @materialPrarent and  a.process_code = @process_code order by a.Id desc";
				var result = await base.DbConnection.QueryAsync<ProductMaterialDetail>(query, new { @productCode = ProductCode, name = name, @materialPrarent = MaterialPrarent, process_code = process_code });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> InsertToMaterialChild(ProductMaterialDetail item)
		{
			try
			{
				var query = @"Insert into product_material_detail
							(ProductCode, level, MaterialParent, MaterialNo, CreateId, CreateDate, ChangeId, ChangeDate,name,process_code)
							VALUES (@ProductCode, @level, @MaterialParent, @MaterialNo, @CreateId, @CreateDate, @ChangeId, @ChangeDate, @name, @process_code)";
				var result = await base.DbConnection.ExecuteAsync(query, item);
				return result;

			}
			catch (Exception e)
			{

				throw e;
			}
		}

		public async Task<IEnumerable<RoutingInfoResponse>> GetRoutinigInfo(string product, string process_code)
		{
			try
			{
				var query = @"Select a.*,(select dt_nm from comm_dt where mt_cd='COM032' and dt_cd=a.don_vi_pr)don_vi_prnm, a.description,
							(select dt_nm from comm_dt where mt_cd='COM007' and dt_cd=a.name) As name_pr 
								from d_rounting_info as a where a.style_no=@Product and a.process_code= @ProcessCode
								order by a.level asc";
				var result = await base.DbConnection.QueryAsync<RoutingInfoResponse>(query, new { @Product = product , @ProcessCode =  process_code  });
				return result;
			}
			catch (Exception e)
			{

				throw e;
			}
		}

		public async Task<IEnumerable<ProductMaterial>> GetProductMaterial(string product, string name, string process_code)
		{
			try
			{
				//var query = @"Select a.*, b.mt_nm As name
				//			From product_material As a Join d_material_info As b On a.mt_no = b.mt_no
				//			where (@StyleNo = '' OR @StyleNo IS NULL OR a.style_no = @StyleNo) 
				//			AND(@Name = '' OR @Name IS NULL OR a.mt_nm = @Name)
				//			AND(@Level = '' OR @Level IS NULL OR a.level = @Level)
				//			AND b.active = 1
				//			Order by a.Id asc";

				var query = @"Select a.*,(select mt_nm from d_material_info where mt_no=a.mt_no AND active = 1) As mt_nm
							From product_material As a 
							where  a.style_no = @StyleNo and a.name = @Name and a.process_code = @process_code
							Order by mt_no desc";
				var result = await base.DbConnection.QueryAsync<ProductMaterial>(query, new { StyleNo = product, Name = name, process_code = process_code });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<DRoutingInfo> GetTop1RoutingInfo()
		{
			try
			{
				var query = @"Select top 1 * From d_rounting_info Order by idr Desc";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<DRoutingInfo>(query);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<ProductMaterial> GetTop1ProductMaterial()
		{
			try
			{
				var query = @"Select top 1 * From product_material Order by Id desc";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<ProductMaterial>(query);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}


		public async Task<int> DeleteMaterialExists(IEnumerable<ProductMaterialDetail> listData)
		{
			try
			{
				int result = 0;
                foreach (var item in listData)
                {
					var query = "Delete product_material_detail where Id = @ID";
					result += await base.DbConnection.ExecuteAsync(query, new { @ID = item.id });
				}
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}


		public async Task<int> DeleteBomMaterialExists(IEnumerable<MaterialBom> listData)
		{
			try
			{
				int result = 0;
				foreach (var item in listData)
				{
					var query = "Delete materialbom where Id = @ID";
					result += await base.DbConnection.ExecuteAsync(query, new { @ID = item.id });
				}
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<ProductProcess> GetcheckProcessCode(string style_no)
		{
			try
			{
				var query = @"Select ISNULL(MAX(process_code), 0) AS process_code  from product_routing where style_no = @Style_no";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<ProductProcess>(query, new { Style_no = style_no });
				return result;
			}
			catch (Exception e)
			{

				throw e;
			}
		}
		public async Task<int> UpdateProcessToApply(string style_no)
		{
			try
			{
				var query = @"Update product_routing SET IsApply='N' WHERE style_no=@Style_no";
				var result = await base.DbConnection.ExecuteAsync(query, new { Style_no = style_no });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<int> InsertToProductProcess(ProductProcess item)
		{
			try
			{
				var query = @"INSERT INTO product_routing
							(style_no,process_code,process_name,description,reg_id,IsApply,reg_dt,chg_dt )
							VALUES (@style_no, @process_code, @process_name, @description, @reg_id,@IsApply, @reg_dt,SYSDATETIME());
						SELECT SCOPE_IDENTITY()";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
				return result;
			}
			catch (Exception e)
			{

				throw e;
			}
		}
		public async Task<IEnumerable<ProductProcess>> GetProductRouting(string product)
		{
			try
			{
				var query = @"SELECT a.* FROM product_routing AS a WHERE a.style_no =@StyleNo";
				var result = await base.DbConnection.QueryAsync<ProductProcess>(query, new { StyleNo = product });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<ProductProcess> GetcheckProcessByStyle(int id)
		{
			try
			{
				var query = @"Select *  from product_routing where id = @Id";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<ProductProcess>(query, new { Id = id });
				return result;
			}
			catch (Exception e)
			{

				throw e;
			}
		}
		public async Task<int> UpdateToProductProcess(ProductProcess item)
		{
			try
			{
				var query = @"Update product_routing SET process_name=@process_name,description=@description,chg_id=@chg_id, IsApply = @IsApply
								WHERE id=@id";
				var result = await base.DbConnection.ExecuteAsync(query, item);
				return result;

			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<int> DeleteProductProcessForId(int id)
		{
			try
			{
				var query = @"Delete product_routing where id = @Id";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Id = id });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		#endregion

		#endregion

		#region TOOL (MMS)

		#region Machine (MMS)
		public async Task<IEnumerable<MachineInfo>> GetListMchine()
		{
			try
			{
				var query = @"Select * From d_machine_info";
				var result = await base.DbConnection.QueryAsync<MachineInfo>(query);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IEnumerable<CommCode>> GetListCommDT()
		{
			try
			{
				var query = @"Select * from comm_dt  where mt_cd = 'COM007'";
				var result = await base.DbConnection.QueryAsync<CommCode>(query);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IEnumerable<MachineInfo>> SearchMachineInfo(string mc_type_search, string mc_no_search, string mc_nm_search, DateTime? start_search, DateTime? end_search)
		{
			try
			{
				var query = @"SELECT a.* 
							FROM  d_machine_info as a 
							Where (@mc_type_search = '' OR @mc_type_search IS NULL OR a.mc_type Like '%'+ @mc_type_search +'%' )
									AND (@mc_no_search = '' OR @mc_no_search IS NULL OR a.mc_no Like '%'+ @mc_no_search +'%' )
									AND (@mc_nm_search = '' OR @mc_nm_search IS NULL OR a.mc_nm Like '%'+ @mc_nm_search +'%')
									AND (@start_search = '' OR @start_search IS NULL OR a.reg_dt >=  @start_search)
									AND (@end_search ='' OR @end_search IS NULL OR a.reg_dt <= @end_search)
							ORDER BY a.chg_dt desc";
				var result = await base.DbConnection.QueryAsync<MachineInfo>(query, new
				{
					@mc_type_search = mc_type_search,
					@mc_no_search = mc_no_search,
					@mc_nm_search = mc_nm_search,
					@start_search = start_search,
					@end_search = end_search
				});
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

        public async Task<int> countMachineInfo(string mc_no)
        {
            try
            {
				var query = @"Select Count(*) From d_machine_info Where mc_no = @Mc_No";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Mc_No = mc_no });
				return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

		public async Task<MachineInfo> checkMachineInfo(int mno)
		{
			try
			{
				var query = @"Select * From d_machine_info Where mno = @Id";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<MachineInfo>(query, new { @Id = mno });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<MachineInfo> getListMachineInfoById(int mno)
		{
			try
			{
				var query = @"Select * From d_machine_info Where mno = @Id";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<MachineInfo>(query, new { @Id = mno });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> InsertIntoMachineInfo(MachineInfo item)
        {
            try
            {
				var query = @"INSERT INTO d_machine_info(mc_type, mc_no, mc_nm, purpose, chg_dt, reg_dt, chg_id, reg_id, barcode)
								VALUES (@mc_type, @mc_no, @mc_nm, @purpose, GetDate(), GetDate(), @chg_id, @reg_id, @mc_no)
								SELECT SCOPE_IDENTITY()";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
				return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<ModelReturnMachineExcelResponse>> GetListMachineExcel(string mc_no)
        {
            try
            {
				var query = @"SELECT mno,mc_type,mc_no,mc_nm,purpose,re_mark,reg_id,chg_id,chg_dt,reg_dt,'INSERT' AS STATUS FROM d_machine_info
								WHERE mc_no = @Mc_No";
				var result = await base.DbConnection.QueryAsync<ModelReturnMachineExcelResponse>(query, new { @Mc_No = mc_no });
				return result.ToList();
			}
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> DeleteMachineInfo(int mno)
        {
            try
            {
				var query = @"Delete d_machine_info WHERE mno = @Id";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Id = mno });
				return result;
			}
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<int> UpdateMachineInfo(MachineInfo item)
        {
            try
            {
				var query = @"Update d_machine_info SET mc_type = @mc_type, mc_no = @mc_no, mc_nm = @mc_nm, purpose = @purpose, re_mark = @re_mark, chg_dt = @chg_dt, reg_dt = @reg_dt, chg_id = @chg_id, 
								reg_id = @reg_id, barcode = @barcode Where mno = @mno";
				var result = await base.DbConnection.ExecuteAsync(query, item);
				return result;
			}
            catch (Exception e)
            {
                throw e;
            }
        }
		public async Task<int> UpdateIsFinishDRoutingInfo(string style_no, string process_code)
		{
			try
			{
				var query = @"Update d_rounting_info SET isFinish='N' WHERE style_no=@Style_no and process_code = @ProcessCode	";
				var result = await base.DbConnection.ExecuteAsync(query, new { Style_no = style_no , ProcessCode = process_code });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}


		#endregion

		#region Bobbin (MMS)
		public async Task<IEnumerable<BobbinInfoResponse>> GetListBobbinInfo(string bb_no, string bb_name)
		{
            try
            {
				var query = @"SELECT si.*,
								(SELECT b.at_no FROM w_material_info_mms AS a JOIN w_actual AS b ON a.id_actual=b.id_actual WHERE a.material_code = si.mt_cd Group by b.at_no)As at_no 
							FROM d_bobbin_info AS si 
							WHERE  si.bb_no NOT LIKE '%AUTO%' And (@bb_no='' OR @bb_no IS NULL OR   si.bb_no like '%' + @bb_no + '%' )
							AND (@bb_nm ='' OR  @bb_nm IS NULL OR si.bb_nm like '%' + @bb_nm + '%')
							ORDER BY si.bb_no asc";
				var result = await base.DbConnection.QueryAsync<BobbinInfoResponse>(query ,new { @bb_no = bb_no, @bb_nm = bb_name });
				return result;
            }
            catch (Exception e)
            {

                throw e;
            }
		}

        public async Task<int> CheckBobbinInfo(string bb_no)
        {
            try
            {
				var query = @"Select Count(*) From d_bobbin_info where bb_no = @Bb_No";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query,new { @Bb_No = bb_no });
				return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> InsertIntoBobbinInfo(BobbinInfo item)
        {
			try
			{
				var query = @"Insert Into d_bobbin_info (mc_type, bb_no, mt_cd,bb_nm, purpose, barcode, re_mark, use_yn, count_number, del_yn, reg_id, reg_dt, chg_id, chg_dt, active)
								Values (@mc_type, @bb_no, @mt_cd, @bb_nm, @purpose, @barcode, @re_mark, @use_yn, @count_number, @del_yn, @reg_id, @reg_dt, @chg_id, @chg_dt, @active)
							Select Scope_Identity() ";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

        public async Task<BobbinInfo> GetBobbinInfoById(int bno)
        {
            try
            {
				var query = @"Select * from d_bobbin_info where bno = @Id";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<BobbinInfo>(query, new { @Id = bno});
				return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

		public async Task<int> DeleteBobbinInfo(int bno)
		{
			try
			{
				var query = @"Delete d_bobbin_info Where bno = @Id";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Id = bno });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> UpdateBobbinInfo(BobbinInfo item)
		{
			try
			{
				var query = @"Update d_bobbin_info SET mc_type = @mc_type, bb_no = @bb_no, mt_cd = @mt_cd, bb_nm = @bb_nm, purpose = @purpose, barcode = @barcode, re_mark = @re_mark, 
								use_yn = @use_yn, count_number = @count_number, del_yn = @del_yn, reg_id = @reg_id, reg_dt = @reg_dt, chg_id = @chg_id, chg_dt = @chg_dt, active = @active 
								Where bno = @bno";
				var result = await base.DbConnection.ExecuteAsync(query, item);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		#endregion

		#region Tray Box Management
		public async Task<int> CreateTrayBox(string bb_cd, string chg_id, string reg_id, string mc_type, string bb_nm)
		{
            try
            {
				var query = @"INSERT INTO d_bobbin_info (bb_no, barcode, mc_type, bb_nm, chg_id, reg_id, reg_dt, chg_dt, use_yn, del_yn) 
							VALUES (@bb_no_v, @bb_no_v, @type_v, @bb_nm_v, @chg_id_v, @reg_id_v, GetDate(), GetDate(),'Y','N')
							Select Scope_Identity()";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @bb_no_v = bb_cd, @type_v = mc_type, @bb_nm_v = bb_nm, @chg_id_v = chg_id, @reg_id_v = reg_id });
				return result;
            }
            catch (Exception e)
            {
                throw e;
            }
		}

		public async Task<BobbinInfo> GetListTrayBox(string bb_no)
		{
			try
			{
				var query = @"SELECT d.bb_no, d.bno, d.mc_type, d.mt_cd, d.bb_nm, d.purpose, d.barcode, d.re_mark FROM d_bobbin_info As d WHERE d.bb_no = @BB_CD ORDER BY d.bno DESC";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<BobbinInfo>(query, new { @BB_CD = bb_no });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IEnumerable<BobbinInfo>> SearchTrayBox(string bb_no, string bb_nm)
		{
            try
            {
				var query = @"SELECT d.bb_no, d.bno, d.mc_type, d.mt_cd, d.bb_nm, d.purpose, d.barcode, d.re_mark,
								(SELECT Top 1 b.at_no FROM w_material_info_mms AS a JOIN w_actual AS b ON a.id_actual=b.id_actual WHERE a.material_code = d.mt_cd) As at_no 
								FROM d_bobbin_info As d 
								WHERE d.bb_no LIKE '%AUTO%' 
								AND (@bb_no = '' OR @bb_no IS NULL OR d.bb_no LIKE '%' + @bb_no + '%') 
								AND (@bb_nm = '' OR @bb_nm IS NULL OR d.bb_nm  LIKE '%'+  @bb_nm + '%')
								Order by d.bno Desc";
				var result = await base.DbConnection.QueryAsync<BobbinInfo>(query, new { @bb_no = bb_no, @bb_nm = bb_nm});
				return result;
            }
            catch (Exception e)
            {
                throw e;
            }
		}

		public async Task<IEnumerable<BobbinInfo>> GetBarCodeOfBobbinInfo(int bno)
		{
            try
            {
				var query = @"Select * From d_bobbin_info where bno = @Id";
				var result = await base.DbConnection.QueryAsync<BobbinInfo>(query, new { @Id = bno });
				return result;
            }
            catch (Exception e)
            {
                throw e;
            }
		}
		#endregion

		#region Staff (MMS)
		public async Task<IEnumerable<MbInfo>> GetListStaff()
		{
            try
            {
				var query = @"Select * from mb_info where lct_cd = 'staff' and del_yn = 'N' Order by chg_dt";
				var result = await base.DbConnection.QueryAsync<MbInfo>(query);
				return result;
            }
            catch (Exception e)
            {
                throw e;
            }
		}

        public async Task<IEnumerable<MbInfo>> SearchStaff(string searchType, string keywordInput, string position)
        {
            try
            {
					var query = @"SELECT a.userid, a.uname, a.barcode, a.birth_dt, a.join_dt, a.depart_cd, a.gender, a.position_cd, 
								a.reg_id, a.reg_dt, a.chg_id, a.chg_dt, a.del_yn
									FROM mb_info a
									WHERE a.del_yn = 'N' AND (@Position = '' OR @Position IS NULL OR a.position_cd LIKE '%'+ @Position +'%')
									And
										((@Keyword = '') Or (@Keyword IS NULL) OR(@Keyword = CASE @SearchType
											when 'userid' then a.userid 
											when 'uname' then a.uname 
											ELSE ''
										End))
									And lct_cd = 'staff'";
				var result = await base.DbConnection.QueryAsync<MbInfo>(query, new { @SearchType = searchType, @Keyword = keywordInput, @Position = position });
				return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<CommCode>> GetPositionStaff()
        {
            try
            {
				var query = @"Select * from comm_dt where mt_cd = 'COM018'";
				var result = await base.DbConnection.QueryAsync<CommCode>(query);
				return result;

            }
            catch (Exception e)
            {
                throw;
            }
        }

		public async Task<IEnumerable<CommCode>> getgender()
		{
			try
			{
				var query = @"Select * from comm_dt where mt_cd = 'COM019'";
				var result = await base.DbConnection.QueryAsync<CommCode>(query);
				return result;

			}
			catch (Exception e)
			{
				throw;
			}
		}

		public async Task<IEnumerable<DepartmentInfo>> GetListDepartmentInfo()
        {
            try
            {
				var query = @"Select * from department_info where use_yn = 'Y'";
				var result = await base.DbConnection.QueryAsync<DepartmentInfo>(query);
				return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> countStaffbyUserId(string UserId)
        {
            try
            {
				var query = @"Select Count(*) From mb_info where userid = @UserId";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @UserId = UserId });
				return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
		public async Task<IReadOnlyList<mb_info>> GetListUserById()
		{
			try
			{
				var query = @"Select * from mb_info";
				var result = await base.DbConnection.QueryAsync<mb_info>(query);
				return result.ToList();
			}
			catch (Exception)
			{

				throw;
			}
		}
		public async Task<MbInfo> GetStaffbyUserId(string UserId)
		{
			try
			{
				var query = @"Select * From mb_info where userid = @UserId";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<MbInfo>(query, new { @UserId = UserId });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IEnumerable<MbInfo>> GetListStaffbyUserId(List<string> listUsserId)
		{
			try
			{
				var rs = new List<MbInfo>();
                foreach (var item in listUsserId)
                {
					var query = @"Select * From mb_info where userid = (@UserId)";
					var result = await base.DbConnection.QueryAsync<MbInfo>(query, new { @UserId = item });
					rs.AddRange(result);
				}
				return rs;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> InsertStaff(MbInfo item)
        {
            try
            {
                var query = @"Insert Into mb_info (userid, uname, gender, lct_cd, barcode, position_cd, birth_dt, depart_cd, join_dt, del_yn, reg_dt, chg_dt, chg_id, reg_id)
								Values (@userid, @uname, @gender, @lct_cd, @barcode, @position_cd, @birth_dt, @depart_cd, @join_dt, @del_yn, @reg_dt, @chg_dt, @chg_id, @reg_id)
							  Select Scope_Identity()";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query,item);
				return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
		public async Task<int> checkStaff(string userId)
		{
			try
			{
				var query = @"select count(*) from mb_info where userid = @userId";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @userId = userId });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<int> DeleteStaffbyUserId(string UserId)
        {
            try
            {
				var query = @"Delete mb_info where userid = @UserId";
				var result = await base.DbConnection.ExecuteAsync(query, new { @UserId = UserId });
				return result;
			}
            catch (Exception e)
            {
                throw e;
            }
        }

		public async Task<int> DeleteMbAuthorInfobyUserId(string UserId)
		{
			try
			{
				var query = @"Delete mb_author_info where userid = @UserId";
				var result = await base.DbConnection.ExecuteAsync(query, new { @UserId = UserId });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<MbAuthorInfo> GetMBAuthorInfobyUserId(string UserId)
        {
			try
			{
				var query = @"Select * From mb_author_info where userid = @UserId";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<MbAuthorInfo>(query, new { @UserId = UserId });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

        public async Task<MbInfo> GetStaffbyId(string Id)
        {
            try
            {
				var query = @"SELECT a.userid, a.uname, a.barcode, a.birth_dt, a.join_dt, a.depart_cd, a.gender, a.position_cd, 
								a.reg_id, a.reg_dt, a.chg_id, a.chg_dt, a.del_yn From mb_info a where userid = @Id";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<MbInfo>(query, new { @Id = Id });
				return result;
			}
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateStaff(MbInfo item)
        {
            try
            {
				var query = @"Update mb_info SET uname = @uname, gender = @gender, lct_cd = @lct_cd, barcode = @barcode, position_cd= @position_cd, birth_dt = @birth_dt, depart_cd = @depart_cd, 
							join_dt = @join_dt, del_yn = @del_yn Where userid = @userid";
				var result = await base.DbConnection.ExecuteAsync(query, item);
				return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> InsertListStaff(string Id, string Name, string Gender, string locationCode, string barCode, string positionCode, DateTime? BirthDate, string DepartmentCode, DateTime? JoinDate, string userInsert, DateTime dateInsert)
        {
            try
            {
                var query = @"Insert Into mb_info (userid, uname, gender, lct_cd, barcode, position_cd, birth_dt, depart_cd, join_dt, reg_id, reg_dt, chg_id, chg_dt)
							Values(@userid, @uname, @gender, @lct_cd, @barcode, @position_cd, @birth_dt, @depart_cd, @join_dt, @reg_id, @reg_dt, @chg_id, @chg_dt)";
				var result = await base.DbConnection.ExecuteAsync(query, new
				{
					@UserId = Id,
					@uname = Name,
					@gender = Gender,
					@lct_cd = locationCode,
					@barcode = barCode,
					@position_cd = positionCode,
					@birth_dt = BirthDate,
					@depart_cd = DepartmentCode,
					@join_dt = JoinDate,
					@reg_id = userInsert,
					@chg_id = userInsert,
					@reg_dt = dateInsert,
					@chg_dt = dateInsert,
				});
				return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

		public async Task<MbInfo> GetBarCodeOfStaffByUserId(string userid)
		{
            try
            {
				var query = @"Select * From mb_info where userid = @Id";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<MbInfo>(query, new { @Id = userid});
				return result;
            }
            catch (Exception e)
            {
                throw e;
            }
		}
		#endregion

		#region Status
		public async Task<IEnumerable<BobbinLctHist>> GetListBobbinLctHist(string bb_no)
		{
            try
            {
				var query = @"Select * from d_bobbin_lct_hist Where bb_no = @BB_NO Order By reg_dt Desc ";
				var result = await base.DbConnection.QueryAsync<BobbinLctHist>(query, new { @BB_NO = bb_no });
				return result;
            }
            catch (Exception e)
            {
                throw e;
            }
		}

		public async Task<BobbinInfo> GetBobbinInfoByBB_No(string bb_no)
		{
			try
			{
				var query = @"Select * from d_bobbin_info where bb_no = @BB_NO";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<BobbinInfo>(query, new { @BB_NO = bb_no });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IEnumerable<MaterialMappingMMS>> GetListMaterialMappingMMS(string mt_cd)
		{
			try
			{
				var query = @"Select * From w_material_mapping_mms where mt_cd = @Mt_Cd Order by reg_date Desc";
				var result = await base.DbConnection.QueryAsync<MaterialMappingMMS>(query, new { @Mt_Cd = mt_cd });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IEnumerable<Actual>> GetListActual(int? id_actual)
		{
			try
			{
				var query = @"Select * From w_actual where id_actual = @Id";
				var result = await base.DbConnection.QueryAsync<Actual>(query, new { @Id = id_actual });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IEnumerable<MaterialInfoMMS>> GetListDataMaterialInfoMMS(string mt_cd)
		{
			try
			{
				var query = @"Select * from w_material_info_mms where material_code = @Mt_Cd";
				var result = await base.DbConnection.QueryAsync<MaterialInfoMMS>(query, new { @Mt_Cd = mt_cd });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IEnumerable<InventoryProduct>> GetListDataInventoryProduct(string mt_cd)
		{
			try
			{
				var query = @"Select * from inventory_products where material_code = @Mt_Cd";
				var result = await base.DbConnection.QueryAsync<InventoryProduct>(query, new { @Mt_Cd = mt_cd });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<ActualPrimary> GetActualPrimaryById(string at_no)
		{
			try
			{
				var query = @"Select * from w_actual_primary where at_no = @PO";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<ActualPrimary>(query, new { @PO = at_no });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IEnumerable<MaterialInfoTam>> GetListDataMaterialInfoTam(string mt_cd)
		{
            try
            {
				var query = @"Select * from w_material_info_tam where mt_cd = @Mt_Cd";
				var result = await base.DbConnection.QueryAsync<MaterialInfoTam>(query, new { @Mt_Cd = mt_cd });
				return result;
			}
            catch (Exception e)
            {
                throw e;
            }
		}
		public async Task<string> GetStaffIdByIdActual(int id_actual)
		{
			try
			{
				var query = @"Select Top 1 staff_id From d_pro_unit_staff where id_actual In (Select id_actual From w_material_info_mms Where id_actual = @Id) Order by reg_dt Desc";
				var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @Id = id_actual });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}


		public async Task<string> GetStaffIdByIdActualForTIMS(int id_actual)
		{
			try
			{
				var query = @"Select Top 1 staff_id From d_pro_unit_staff where id_actual In (Select id_actual From w_material_info_tims Where id_actual = @Id) Order by reg_dt Desc";
				var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @Id = id_actual });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<string> GetDetailNameByDetailCode(string detailCode)
		{
            try
            {
				var query = @"Select dt_nm From comm_dt Where mt_cd = 'WHS005' and dt_cd = @DetailCode And mt_cd = 'WHS005'";
				var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @DetailCode = detailCode });
				return result;
			}
            catch (Exception e)
            {
                throw e;
            }
		}

		public async Task<string> CheckActual(string name)
		{
            try
            {
				var query = @"Select dt_nm From comm_dt Where mt_cd = 'COM007' And dt_cd = @DtCd";
				var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @DtCd = name});
				return result;
            }
            catch (Exception e)
            {
                throw e;
            }
		}

		public async Task<IEnumerable<MaterialInfoTIMS>> GetListMaterialInfoTIMS(string mt_cd)
		{
            try
            {
				var query = @"Select * From w_material_info_tims Where material_code = @Mt_Cd";
				var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(query, new { @Mt_Cd = mt_cd});
				return result;
            }
            catch (Exception e)
            {
                throw e;
            }
		}



		public async Task<IEnumerable<WMaterialTamReponse>> GetListDataMaterialInfoTamByMt_Cd(string mt_cd)
		{
			try
			{
				var query = $"Select a.wmtid, a.mt_cd, a.mt_no, a.gr_qty, a.dt_of_receipt , a.expiry_dt, a.expore_dt, a.lot_no							From w_material_info_tam as a where mt_cd in ({mt_cd}) ";
				var result = await base.DbConnection.QueryAsync<WMaterialTamReponse>(query/*, new { @Mt_Cd = id }*/);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}



		public async Task<IEnumerable<string>> GetListMaterialInfoTam(string mt_cd)
		{
            try
            {
				var query = $"Select mt_cd From w_material_info_tam where  mt_cd in ({mt_cd})";
				var result = await base.DbConnection.QueryAsync<string>(query);
				return result;
			}
            catch (Exception)
            {

                throw;
            }
		}

		#endregion

		#endregion

		#region STANDARD

		#region User Management

		public async Task<IEnumerable<MbInfo>> GetListUser()
		{
            try
            {
				var query = @"Select * from mb_info where active = 1 And (lct_cd = 'user' Or grade = 'Supplier') Order by chg_dt Desc";
				var result = await base.DbConnection.QueryAsync<MbInfo>(query);
				return result;
            }
            catch (Exception e)
            {

                throw e;
            }
		}

        public async Task<IEnumerable<MbInfo>> SearchUser(string searchType, string keywordInput, string department, string position)
        {
			try
			{
				var query = @"SELECT  * FROM mb_info a
									WHERE (@Position = '' OR @Position IS NULL OR a.position_cd LIKE '%'+ @Position +'%')
									AND (@Department = '' OR @Department IS NULL OR a.depart_cd LIKE '%'+ @Department +'%')
									AND
										((@Keyword = '') Or (@Keyword IS NULL) OR(@Keyword = CASE @SearchType
											when 'userid' then a.userid 
											when 'uname' then a.uname 
											ELSE ''
										End))
                                    AND active = 1 And (lct_cd = 'user' Or grade = 'Supplier')
									Order by a.chg_dt Desc";
				var result = await base.DbConnection.QueryAsync<MbInfo>(query, new { @SearchType = searchType, @Keyword = keywordInput, @Department = department, @Position = position });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

        public async Task<IEnumerable<AuthorInfo>> GetListGrande()
        {
            try
            {
				var query = @"Select * from author_info where active =  1 and use_yn = 'y'";
				var result = await base.DbConnection.QueryAsync<AuthorInfo>(query);
				return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
		public async Task<IEnumerable<modelTableMAchine>> PartialView_ResourceMgt(string po, string productCode, string machineCode, string date_end, string date_start)
		{
			try
			{

				var query = @"SELECT n.id_actual, n.mc_no, FORMAT(n.reg_dt , 'yyyy-MM-dd') AS date,m.at_no	,m.product, 
								n.start_dt AS start_dt,  
								n.end_dt AS end_dt 
								FROM d_pro_unit_mc AS n 
								JOIN w_actual AS m ON n.id_actual = m.id_actual
								WHERE (FORMAT(CAST(n.reg_dt as datetime), 'HH:mm:ss') >=   (FORMAT(CAST(@date_start as datetime), 'HH:mm:ss')))
							    and (FORMAT(CAST(n.reg_dt as datetime), 'HH:mm:ss') >=   (FORMAT(CAST(@date_end as datetime), 'HH:mm:ss')))
								AND(@machineCode = '' OR  n.mc_no like @machineCode)
								AND(@po_no = '' OR  m.at_no like @po_no )
								AND(@productCode  = '' OR  m.product like @productCode)  ";
				var result = await base.DbConnection.QueryAsync<modelTableMAchine>(query, new { @po_no = po, @productCode = productCode, @machineCode = machineCode, @date_end = date_end, @date_start = date_start });
				return result;
			}
			catch (Exception e)
			{

				throw e;
			}
		}
		public async Task<IEnumerable<TableMachineDatetime>> PartialView_dialog_Viewdetail(string at_no, string mc_no)
		{
			try
			{

					var query = @"With bang1 as ( 
							 SELECT  8 AS Hour
								 UNION ALL SELECT  9 UNION ALL SELECT  10 UNION ALL SELECT  11
								 UNION ALL SELECT  12 UNION ALL SELECT  13 UNION ALL SELECT  14
								 UNION ALL SELECT  15 UNION ALL SELECT  16 UNION ALL SELECT  17
								 UNION ALL SELECT 18 UNION ALL SELECT 19 UNION ALL SELECT 20
						) 
						, bang2  as ( 
									SELECT FORMAT(CAST(a.reg_date as datetime),'HH' ) AS hourRun, c.at_no, a.gr_qty
									FROM w_material_info_mms AS a
									Join d_pro_unit_mc b on a.id_actual =b.id_actual
									join w_actual c on a.id_actual =c.id_actual
									WHERE c.at_no =@at_no  AND
									b.mc_no = @mc_no
								--	FIND_IN_SET(_mc_no, a.machine_id)
									-- AND DATE_FORMAT(a.reg_dt,'%Y-%m-%d' ) = current_date()
					)
					SELECT bang1.hour, ISNULL(bang2.hourRun,'')  AS hourRun, ISNULL(sum(bang2.gr_qty),0) Quantity, @at_no as at_no,  @mc_no AS mc_no 
					From bang1
					left JOIN bang2 ON bang1.Hour = bang2.hourRun
					GROUP BY bang1.hour,bang2.hourRun;  ";
				var result = await base.DbConnection.QueryAsync<TableMachineDatetime>(query, new { @at_no = at_no, @mc_no = mc_no });
				return result;
			}
			catch (Exception e)
			{

				throw e;
			}
		}

		public async Task<string> GetCodeFromAuthorInfo(string grade)
		{
			try
			{
				var query = @"Select at_cd from author_info where active = 1 and use_yn = 'y' and at_nm = @Grade";
				var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @Grade = grade});
				return result;
			}
			catch (Exception e)
			{

				throw e;
			}
		}


		public async Task<int> CheckMbInfo(string userid)
		{
			try
			{
				var query = @"Select Count(*) from mb_info Where userid = @UserId";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @UserId = userid });
				return result;
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<int> InsertIntoMbInfo(MbInfo item)
        {
            try
            {
				var query = @"Insert Into mb_info (userid, uname, nick_name, upw, grade, depart_cd, gender, lct_cd, barcode, position_cd, cel_nb, e_mail, sms_yn, birth_dt, join_dt, scr_yn, mail_yn, ltacc_dt, mbout_dt, mbout_yn, accblock_yn, session_key, session_limit, memo, del_yn, check_yn, mbjoin_dt, reg_dt, chg_dt, chg_id, reg_id, re_mark)
								Values (@userid, @uname, @nick_name, @upw, @grade, @depart_cd, @gender, @lct_cd, @barcode, @position_cd, @cel_nb, @e_mail, @sms_yn, @birth_dt, @join_dt, @scr_yn, @mail_yn, @ltacc_dt, @mbout_dt, @mbout_yn, @accblock_yn, @session_key, @session_limit, @memo, @del_yn, @check_yn, @mbjoin_dt, @reg_dt, @chg_dt, @chg_id, @reg_id, @re_mark)";

				var result = await base.DbConnection.ExecuteAsync(query, item);
				return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> InsertIntoMbAuthorInfo(MbAuthorInfo item)
        {
            try
            {
				var query = @"Insert Into mb_author_info (userid, at_cd, reg_id, reg_dt, chg_id, chg_dt)
								Values(@userid, @at_cd, @reg_id, @reg_dt, @chg_id, @chg_dt)";
				var result = await base.DbConnection.ExecuteAsync(query, item);
				return result;
			}
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<MbInfo> GetUserById(string userid)
        {
            try
            {
				var query = @"Select * from mb_info Where userid = @Id";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<MbInfo>(query, new { @Id = userid });
				return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

		public async Task<MbInfo> GetUserByUserId(string userId)
		{
			try
			{
				var query = @"Select * from mb_info Where userid = @UserId";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<MbInfo>(query, new { @UserId = userId });
				return result;
			}
			catch (Exception)
			{

				throw;
			}
		}

        public async Task<int> UpdateUseṛ(MbInfo item)
        {
            try
            {
				var query = @"Update mb_info SET  uname = @uname, nick_name = @nick_name, upw = @upw, grade = @grade, depart_cd = @depart_cd, gender = @gender, lct_cd = @lct_cd, barcode = @barcode, position_cd = @position_cd, 
							cel_nb = @cel_nb, e_mail = @e_mail, sms_yn = @sms_yn, birth_dt = @birth_dt, join_dt = @join_dt, scr_yn = @scr_yn, mail_yn = @mail_yn, ltacc_dt = @ltacc_dt, mbout_dt = @mbout_dt, mbout_yn = @mbout_yn, accblock_yn = @accblock_yn, 
							session_key = @session_key, session_limit = @session_limit, @memo = memo, del_yn = @del_yn, check_yn = @check_yn, mbjoin_dt = @mbjoin_dt, reg_dt = @reg_dt, chg_dt = @chg_dt, chg_id = @chg_id, reg_id = @reg_id, re_mark = @re_mark
							Where userid = @userid";
				var result = await base.DbConnection.ExecuteAsync(query, item);
				return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UpdateMbUserInfor(MbAuthorInfo item)
        {
            try
            {
				var query = @"Update mb_author_info SET userid = @userid, at_cd = @at_cd, reg_id = @reg_id, reg_dt = @reg_dt, chg_id = @chg_id, chg_dt = @chg_dt
							Where mano = @mano";
				var result = await base.DbConnection.ExecuteAsync(query, item);
				return result;
            }
            catch (Exception w)
            {

                throw w;
            }
        }


		public async Task<string> GetLcoationNameByLocationCode(string locationCode)
		{
            try
            {
				var query = @"Select lct_nm From lct_info where lct_cd = @locationCode";
				var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @locationCode = locationCode });
				return result;
            }
            catch (Exception e)
            {
                throw e;
            }
		}
		#endregion

		#endregion

		#region TIMS
		#region Create Buyer QR

		public async Task<IEnumerable<StyleInfo>> GetListStyleInfo(string style_no, string style_nm, string md_cd)
		{
            try
            {
                var query = @"SELECT (SELECT dt_nm FROM comm_dt WHERE a.part_nm = dt_cd And mt_cd = 'DEV003') AS part_name,a.* 
							FROM  d_style_info as a 
							Where (@Code = '' OR @Code IS NULL OR  a.style_no Like '%'+ @Code +'%' )
								AND ( a.stamp_code IS NOT NULL  And a.stamp_code <> '' ) 
								AND (@Code_Name = '' OR @Code_Name IS NULL OR  a.style_nm Like '%'+ @Code_Name +'%' )
								AND (@ModeCode = '' OR @ModeCode IS NULL OR  a.md_cd Like '%' + @ModeCode + '%' )
								ORDER BY a.chg_dt desc";
				var result = await base.DbConnection.QueryAsync<StyleInfo>(query, new { @Code = style_no, @Code_Name = style_nm, @ModeCode = md_cd });
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