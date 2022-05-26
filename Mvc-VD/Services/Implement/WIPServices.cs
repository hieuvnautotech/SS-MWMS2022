using Mvc_VD.Respositories.Irepository;
using Mvc_VD.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using Mvc_VD.Models.WIP;
using Mvc_VD.Models.Response;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models.WOModel;
using Mvc_VD.Models.DTO;

namespace Mvc_VD.Services.Implement
{
    public class WIPServices: DbConnection1RepositoryBase, IWIPServices
    {
        public WIPServices(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {

        }

        public async Task<IEnumerable<ReturnModelResponse>> GetListDataInventoryReturn(string mt_cd)
        {
            try
            {
                //(b.width * a.gr_qty) As size,
                var query = @"Select a.materialid As wmtid, a.material_code, b.mt_nm, a.mt_no, b.bundle_unit,
		                        CONCAT(a.gr_qty, b.unit_cd) As length,
		                        ISNULL(a.gr_qty,'') As length1,
                             CONCAT(b.width, '*', a.gr_qty) AS size,
		                       
		                        ISNULL(b.spec,0) As spec,
                                a.return_date,
                                (Select Top 1 mc_no From d_pro_unit_mc where id_actual In ( Select id_actual From inventory_products where material_code = a.orgin_mt_cd)) As machine,
		                        CONCAT((CASE 
					                        WHEN b.bundle_unit ='Roll' THEN (a.gr_qty/b.spec)
				                        ELSE a.gr_qty
				                        END),' ',ISNULL(b.bundle_unit, '')) As qty,
                                (SELECT TOP 1 dt_nm FROM comm_dt WHERE comm_dt.dt_cd = a.status AND comm_dt.mt_cd='WHS005') AS sts_nm
                                    FROM inventory_products a 
                                    LEFT JOIN d_material_info b ON a.mt_no=b.mt_no 
                                    WHERE a.status='004' 
                                      AND a.location_code LIKE '002%'
                                      AND a.material_code LIKE '%' + @mtCode + '%'
                             Order by a.materialid";
                var result = await base.DbConnection.QueryAsync<ReturnModelResponse>(query, new { @mtCode = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<ReturnModelResponse> GetListDataToPrintQR(int id)
        {
            try
            {
                var query = @"SELECT a.wmtid,a.material_code,b.mt_nm, 
                           CONCAT(a.gr_qty, b.unit_cd) AS lenght, 
                           ISNULL(a.gr_qty,'') AS lenght1, 
                           CONCAT(b.width,'*',a.gr_qty) AS size, 
                           ISNULL(b.spec,0) AS spec,
			               a.mt_no,  
			               CONCAT((case when b.bundle_unit ='Roll' then  (a.gr_qty/b.spec) ELSE a.gr_qty END),' ',ISNULL(b.bundle_unit, '')) As qty, 
                           b.bundle_unit,
                           (SELECT dt_nm FROM comm_dt WHERE comm_dt.dt_cd=a.status AND comm_dt.mt_cd='WHS005') AS sts_nm , 
                           (SELECT Top 1 w_actual_primary.product  FROM w_actual JOIN w_actual_primary ON w_actual.at_no=w_actual_primary.at_no WHERE a.id_actual = w_actual.id_actual) AS product, 
                           (SELECT Top 1 name FROM w_actual WHERE a.id_actual = w_actual.id_actual) AS name 
                            FROM w_material_info_mms a LEFT JOIN d_material_info  b ON a.mt_no=b.mt_no 
                           WHERE a.wmtid = @Id 
                           Order by a.wmtid";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<ReturnModelResponse>(query, new { @Id = id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<ReturnModelResponse> GetListDataUpdatedToPrintQR(string id)
        {
            try
            {
                var query = $" SELECT a.wmtid,a.material_code,b.mt_nm,  CONCAT(a.gr_qty, b.unit_cd) AS lenght,                       ISNULL(a.gr_qty,'') AS lenght1,  CONCAT(b.width,'*',a.gr_qty) AS size, ISNULL(b.spec,0) AS spec, a.mt_no, CONCAT((case when b.bundle_unit ='Roll' then  (a.gr_qty/b.spec) ELSE a.gr_qty END),' ',ISNULL(b.bundle_unit, '')) As qty,   b.bundle_unit, (SELECT dt_nm FROM comm_dt WHERE comm_dt.dt_cd=a.status AND comm_dt.mt_cd='WHS005') AS sts_nm ,                           (SELECT Top 1 w_actual_primary.product  FROM w_actual JOIN w_actual_primary ON w_actual.at_no=w_actual_primary.at_no WHERE a.id_actual = w_actual.id_actual) AS product,                           (SELECT Top 1 name FROM w_actual WHERE a.id_actual = w_actual.id_actual) AS name FROM w_material_info_mms a LEFT JOIN d_material_info  b ON a.mt_no=b.mt_no WHERE a.wmtid in ({id}) Order by a.wmtid";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<ReturnModelResponse>(query);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> CountMaterialInventoryProduct(int id)
        {
            try
            {
                var query = @"Select Count(*) From inventory_products Where materialid = @Id And status = '001'";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Id = id});
                return result;

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<InventoryProduct> FindMaterialInventoryProductById(int id)
        {
            try
            {
                var query = @"Select * From inventory_products Where materialid = @Id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<InventoryProduct>(query, new { @Id = id });
                return result;

            }
            catch (Exception e)
            {

                throw e;
            }

        }
        public async Task<IReadOnlyList<InventoryProduct>> FindListMaterialInventoryProductById(string id)
        {
            try
            {
                var query = $"Select * From inventory_products Where materialid in ({id})";
                var result = await base.DbConnection.QueryAsync<InventoryProduct>(query);
                return result.ToList();

            }
            catch (Exception e)
            {

                throw e;
            }

        }


        public async Task<IEnumerable<InventoryProduct>> FindMaterialInventoryProductByListId(List<int> listData)
        {
            try
            {
                IEnumerable<InventoryProduct> result = new List<InventoryProduct>();
                foreach (var item in listData)
                {
                    var query = @"Select * From inventory_products Where materialid = @Id";
                    var rs = await base.DbConnection.QueryFirstOrDefaultAsync<InventoryProduct>(query, new { @Id = item });
                    if(rs != null)
                    {
                        result.ToList().Add(rs);
                    }
                }

                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public async Task<IEnumerable<PrintQRResponse>> GetListPrintQRInventory(List<string> Id)
        {
            try
            {
                var result = new PrintQRResponse();
                List<PrintQRResponse> listData = new List<PrintQRResponse>();
                foreach (var id in Id)
                {
                    var query = @"SELECT a.materialid As wmtid, a.material_code, b.mt_nm,
                            CONCAT(a.gr_qty,b.unit_cd) As lenght,
                            ISNULL(a.gr_qty,'') As lenght1,
                            CONCAT(b.width,'MM*',b.spec,'M') AS size,
                            ISNULL(b.spec,0) As spec,
				            a.mt_no,  
                            b.bundle_unit,
                            a.lot_no,
				            a.date_of_receipt As dt_of_receipt,
							a.expiry_date As expiry_dt, 
							a.export_date As expore_dt,
				            CONCAT((case when b.bundle_unit ='Roll' then  (a.gr_qty/b.spec) ELSE a.gr_qty END),'',ISNULL(b.bundle_unit, '')) As qty,
                            (SELECT dt_nm FROM comm_dt WHERE comm_dt.dt_cd=a.status AND comm_dt.mt_cd='WHS005') sts_nm,
                            (SELECT Top 1 w_actual_primary.product  FROM   w_actual JOIN w_actual_primary ON w_actual.at_no=w_actual_primary.at_no WHERE  a.id_actual = w_actual.id_actual) product,
                            (SELECT Top 1 name FROM w_actual WHERE a.id_actual=w_actual.id_actual) AS name
                             FROM inventory_products a LEFT JOIN d_material_info  b ON a.mt_no=b.mt_no
                            WHERE a.status='004' 
				            AND  a.materialid = @Id  
				            ORDER BY a.mt_no, a.material_code";
                     result = await base.DbConnection.QueryFirstOrDefaultAsync<PrintQRResponse>(query, new { @Id = id });
                    listData.Add(result);
                }

                return listData;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> InsertIntoInventoryProduct(InventoryProduct item)
        {
            try
            {
                var query = @"Insert into inventory_products(id_actual, material_code, recei_wip_date, picking_date, sd_no, ex_no, lct_sts_cd, mt_no, mt_type, gr_qty, real_qty, bb_no, orgin_mt_cd,
                            recei_date, expiry_date, export_date, date_of_receipt, lot_no, from_lct_cd, location_code, status, create_id, create_date, change_id, change_date, ExportCode, LoctionMachine)
                            Values(@id_actual, @material_code, @recei_wip_date, @picking_date, @sd_no, @ex_no, @lct_sts_cd, @mt_no, @mt_type, @gr_qty, @real_qty, @bb_no, @orgin_mt_cd,
                            @recei_date, @expiry_date, @export_date, @date_of_receipt, @lot_no, @from_lct_cd, @location_code, @status, @create_id, @create_date, @change_id, @change_date, @ExportCode, @LoctionMachine)
                            Select Scope_Identity()";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<InventoryProduct>> GetListInventoryProduct(string mt_cd)
        {
            try
            {
                // And a.ex_no = ''
                var query = @"SELECT a.materialid, a.material_code, a.mt_no, a.mt_type, a.lot_no, a.expiry_date, a.date_of_receipt, a.export_date, a.status, a.gr_qty,
			                   (SELECT dt_nm FROM comm_dt WHERE mt_cd = 'WHS005' AND dt_cd = a.status) AS sts_nm,
			                   (SELECT dt_nm FROM comm_dt WHERE mt_cd = 'COM004' And dt_cd = a.mt_type ) AS mt_type_nm
			                   FROM inventory_products As a
                               WHERE  a.location_code LIKE '002%' And status = '001' 
							   AND(@MtCd = '' OR @MtCd IS NULL OR a.material_code =  @MtCd)";
                var result = await base.DbConnection.QueryAsync<InventoryProduct>(query, new { @MtCd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateMaterialInventoryProduct(int id_actual, string status, DateTime? create_date, DateTime? change_date, int materialid)
        {
            try
            {
                var query = @"UPDATE inventory_products SET id_actual= @id_actual, status = @Status, create_date = @reg_dt, change_date = @chg_dt WHERE materialid = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @id_actual = id_actual, @Status = status, @reg_dt = create_date, @chg_dt = change_date, @Id = materialid });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<int> UpdateListMaterialInventoryProduct(int id_actual, string status, DateTime? create_date, DateTime? change_date, string materialid)
        {
            try
            {
                var query = $"UPDATE inventory_products SET id_actual= @id_actual, status = @Status, create_date = @reg_dt, change_date = @chg_dt WHERE materialid in ({materialid})";
                var result = await base.DbConnection.ExecuteAsync(query, new { @id_actual = id_actual, @Status = status, @reg_dt = create_date, @chg_dt = change_date});
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public async Task<int> UpdateQuantityInventoryProduct(double gr_qry, int id)
        {
            try
            {
                var query = @"UPDATE inventory_products SET gr_qty = @Quantity WHERE materialid = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Quantity = gr_qry, @Id = id });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public async Task<int> UpdateQuantityInventoryProduct(double gr_qry, double real_qty, int id)
        {
            try
            {
                var query = @"UPDATE inventory_products SET gr_qty = @Quantity, real_qty = @Real_Qty WHERE materialid = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Quantity = gr_qry, @Real_Qty = real_qty, @Id = id });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<InventoryProduct> CheckMaterialsInventoryProduct(string materialCode)
        {
            try
            {
                var query = @"SELECT (SELECT lct_nm FROM lct_info WHERE lct_cd = a.location_code ) AS locationName, a.* 
                            FROM inventory_products as a WHERE a.material_code = @mt_cd";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<InventoryProduct>(query, new { @mt_cd = materialCode });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<InventoryProduct> CheckMaterialsInventoryProductByMaterialId(string materialID)
        {
            try
            {
                var query = @"SELECT (SELECT lct_nm FROM lct_info WHERE lct_cd = a.location_code ) AS locationName, a.* 
                            FROM inventory_products as a WHERE a.materialid = @mt_cd";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<InventoryProduct>(query, new { @mt_cd = materialID });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UpdateMaterialToWIP(InventoryProduct item)
        {
            try
            {
                var query = @"Update inventory_products SET ExportCode=@ExportCode ,LoctionMachine = @LoctionMachine ,change_date= @change_date, 
                              create_id= @create_id, ShippingToMachineDatetime = @ShippingToMachineDatetime
                            WHERE materialid = @materialid";
                var result = await base.DbConnection.ExecuteAsync(query,item);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<PrintMaterialQRInventoryResponse>> PrintMaterialQRInventory(string mt_no)
        {
            try
            {
                var listData = mt_no.Split(',');
                var result = new List<PrintMaterialQRInventoryResponse>();
                foreach (var item in listData)
                {
                    var query = @"SELECT a.materialid As wmtid, a.material_code As mt_cd, b.mt_nm, ISNULL(b.spec,0) As spec, a.mt_no, a.recei_date, a.lot_no, a.export_date As expore_dt, a.date_of_receipt As dt_of_receipt, a.expiry_date As expiry_dt, b.bundle_unit, 
	                                CONCAT(ISNULL(a.gr_qty,''),ISNULL(b.unit_cd,'')) lenght,
	                                CONCAT(ISNULL(b.width,0),'MM*',ISNULL(B.spec,0),'M') AS size,
                                    (case when b.bundle_unit ='Roll' then  ROUND((a.gr_qty/b.spec),2) ELSE ROUND(a.gr_qty,2) END) qty, 
	                                (SELECT dt_nm FROM comm_dt WHERE comm_dt.dt_cd=a.status AND comm_dt.mt_cd='WHS005') sts_nm
                                FROM inventory_products a 
                                LEFT JOIN d_material_info  b ON a.mt_no=b.mt_no 
                                WHERE a.location_code LIKE '002%' AND a.materialid = @Item  ORDER BY a.mt_no, a.material_code";
                    var rs = await base.DbConnection.QueryFirstOrDefaultAsync<PrintMaterialQRInventoryResponse>(query, new { @Item = item });
                    result.Add(rs);
                }
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<CompositeMaterialDetailResponse>> GetCompositeMaterialDetailData(string product, string bb_no, string status)
        {
            try
            {
                var query = @"SELECT MAX(a.wmtid) as wmtid ,
                                    a.material_code As mt_cd,
                                    a.mt_no As mt_nm ,
                                    c.at_no,
					                a.mt_no,
                                    a.reg_date,
					                a.bb_no,
                                   
                                    
                               
                                   CONCAT(a.gr_qty, ' EA') qty
              
                            FROM w_material_info_mms AS a
                            JOIN w_actual AS c ON a.id_actual = c.id_actual
                            WHERE a.location_code LIKE '002%' AND a.material_type = 'CMT'AND a.status = '002' AND c.type='SX'
                            AND c.product = @Product
                            AND (@bb_no ='' OR @bb_no IS NULL OR  a.bb_no like '%' + @bb_no + '%') 
                            Group By c.at_no, a.material_code, a.mt_no, a.reg_date, a.bb_no, a.gr_qty";
                var result = await base.DbConnection.QueryAsync<CompositeMaterialDetailResponse>(query, new { @Product = product, @bb_no = bb_no });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        #region Export Material

        public async Task<int> InsertToExportToMachine(ExportToMachineModel item)
        {
            try
            {
                var query = @"INSERT INTO ExportToMachine (ExportCode,ProductCode,ProductName,MachineCode,IsFinish,Description,CreateId,CreateDate,ChangeId,ChangeDate)
                                    VALUES (@ExportCode, @ProductCode, @ProductName, @MachineCode, @IsFinish, @Description, @CreateId, @CreateDate, @ChangeId, @ChangeDate)
                                    Select Scope_Identity()";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> ModifyToExportToMachine(ExportToMachineModel item)
        {
            try
            {
                var query = @"UPDATE ExportToMachine SET ProductCode = @ProductCode, ProductName = @ProductName, MachineCode = @MachineCode, Description = @Description,
                              CreateId = @CreateId, ChangeId = @ChangeId, ChangeDate = @ChangeDate WHERE ExportCode = @ExportCode";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> CheckMaterialEP(string ExportCode)
        {
            try
            {
                var QuerySQL = "SELECT count(materialid) FROM inventory_products WHERE ExportCode = @ExportCode";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(QuerySQL, new { @exportCode = ExportCode});
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> DeleteToExportToMachine(int id)
        {
            try
            {
                string sqlquery = @"DELETE FROM exporttomachine WHERE Id = @id";
                var result = await base.DbConnection.ExecuteAsync(sqlquery, new { @id = id});
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> TotalRecordsSearchExportToMachine(string ExportCode, string ProductCode, string ProductName, string Description)
        {
            try
            {
                var query = @"SELECT COUNT(*) 
	                        FROM ExportToMachine AS a
	                        Where (@exportCode='' OR @exportCode IS NULL OR a.ExportCode Like '%' +@exportCode+ '%' ) 
                                    AND (@productCode='' OR @productCode IS NULL OR  a.ProductCode Like '%'+@productCode+'%' )
                                    AND (@productName ='' OR @productName IS NULL OR a.ProductName Like '%'+@productName+'%' )
                                    AND (@description='' OR  @description IS NULL OR a.Description Like '%'+@description+'%' ) 
                                    AND a.IsFinish = 'N'";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @exportCode = ExportCode, @productCode = ProductCode, @productName = ProductName, @description = Description });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> TotalRecordSearchExportToMachineFinish(string ExportCode, string ProductCode, string ProductName, string Description)
        {
            try
            {
                var query = @"SELECT COUNT(*) 
	                        FROM ExportToMachine AS a
	                        Where (@exportCode='' OR @exportCode IS NULL OR a.ExportCode Like '%' +@exportCode+ '%' ) 
                                    AND (@productCode='' OR @productCode IS NULL OR  a.ProductCode Like '%'+@productCode+'%' )
                                    AND (@productName ='' OR @productName IS NULL OR a.ProductName Like '%'+@productName+'%' )
                                    AND (@description='' OR  @description IS NULL OR a.Description Like '%'+@description+'%' ) 
                                    AND a.IsFinish = 'Y'";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @exportCode = ExportCode, @productCode = ProductCode, @productName = ProductName, @description = Description });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<IEnumerable<ExportToMachineModel>> GetListExportToMachineFinish(string ExportCode, string ProductCode, string ProductName, string Description)
        {
            try
            {
                var query = @"SELECT *
	                        FROM ExportToMachine AS a
	                        Where a.IsFinish = 'Y'
									AND (@exportCode='' OR @exportCode IS NULL OR a.ExportCode Like '%' +@exportCode+ '%' ) 
                                    AND (@productCode='' OR @productCode IS NULL OR  a.ProductCode Like '%'+@productCode+'%' )
                                    AND (@productName ='' OR @productName IS NULL OR a.ProductName Like '%'+@productName+'%' )
                                    AND (@description='' OR  @description IS NULL OR a.Description Like '%'+@description+'%' ) 
						Order by a.Id Desc";
                var result = await base.DbConnection.QueryAsync<ExportToMachineModel>(query, new { @exportCode = ExportCode, @productCode = ProductCode, @productName = ProductName, @description = Description });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> CheckMaterialEP1(string ExportCode)
        {
            try
            {
                var query = @"SELECT Count(*) FROM inventory_products WHERE ExportCode = @ExpCode";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @ExpCode = ExportCode });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> DeleteToExportToMachine(string ex_cd)
        {
            try
            {
                var query = @"DELETE ExportToMachine WHERE ExportCode = @ExCd";
                var result = await base.DbConnection.ExecuteAsync(query, new { @ExCd = ex_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> FinishExportToMachine(ExportToMachineModel item)
        {
            try
            {
                var query = @"UPDATE ExportToMachine SET IsFinish= @isFinish,ChangeId = @ChangeId,ChangeDate = @ChangeDate WHERE Id = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @isFinish = item.IsFinish, @ChangeId = item.ChangeId, @ChangeDate  = item.ChangeDate,@Id = item.Id});
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<ExportToMachineModel> GetExportToMachineById(int Id)
        {
            var query = @"Select * From ExportToMachine Where Id = @Id";
            var result = await base.DbConnection.QueryFirstOrDefaultAsync<ExportToMachineModel>(query, new { @Id = Id });
            return result;
        }

        public async Task<IEnumerable<ExportToMachineModel>> GetListExportToMachine()
        {
            try
            {
                var query = @"SELECT * FROM ExportToMachine Order by Id desc ";
                var result = await base.DbConnection.QueryAsync<ExportToMachineModel>(query);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<ExportToMachineModel>> GetListSearchExportToMachine(string ExportCode, string ProductCode, string ProductName, string Description)
        {
            try
            {
                var query = @" SELECT a.* 
	                    FROM   ExportToMachine AS a
	                     Where a.IsFinish = 'N'
                                    AND (@exportCode='' OR @exportCode IS NULL OR a.ExportCode Like '%'+@exportCode+'%') 
                                    AND (@productCode = '' OR @productCode IS NULL OR  a.ProductCode Like '%'+@productCode+'%')
                                    AND (@productName ='' OR @productName IS NULL OR a.ProductName Like '%'+@productName+'%' )
                                    AND (@description='' OR  @description IS NULL OR a.Description Like '%'+@description+'%') 
                        Order By a.Id desc ";
                var result = await base.DbConnection.QueryAsync<ExportToMachineModel>(query, new { @exportCode = ExportCode, @productCode = ProductCode, @productName = ProductName, @description = Description });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public async Task<IEnumerable<ExportToMachineModel>> GetListSearchExportToMachinePP(string ExportCode)
        {
            try
            {
                var query = @"SELECT * FROM ExportToMachine Where ExportCode = @ExpCode";
                var result = await base.DbConnection.QueryAsync<ExportToMachineModel>(query, new { @ExpCode = ExportCode });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<WMaterialInfoNewResponse>> GetListExportToMachine(string ExportCode)
        {
            try
            {

                var query1 = @"SELECT MAX(a.materialid) As wmtid, a.mt_no, a.status,  a.material_code As mt_cd, a.gr_qty, a.export_date, a.recei_date, a.expiry_date,
                            (SELECT dt_nm FROM comm_dt WHERE mt_cd = 'WHS005' AND dt_cd = a.status) As statusName, 
							(Select Top 1 mc_no From d_pro_unit_mc Where id_actual = a.id_actual) As machine_id,
                            a.ShippingToMachineDatetime As ShippingToMachineDatetime,
	                        (SELECT lct_nm FROM lct_info WHERE lct_cd = a.location_code ) AS lct_nm,
	                        (SELECT at_no FROM w_actual WHERE id_actual = a.id_actual) AS po
                            FROM inventory_products As a
                            WHERE a.ExportCode = @ExpCode
							Group by a.mt_no, a.status, a.material_code, a.gr_qty, a.export_date, a.recei_date, a.expiry_date, a.id_actual, a.ShippingToMachineDatetime, a.location_code
                            UNION ALL
                            SELECT MAX(b.wmtid) As wmtid, b.mt_no, b.status,  b.material_code As mt_cd, b.gr_qty, b.export_date, b.receipt_date, b.expiry_date,
                            (SELECT dt_nm FROM comm_dt WHERE mt_cd = 'WHS005' AND dt_cd = b.status) As statusName, (Select Top 1 mc_no From d_pro_unit_mc Where id_actual = b.id_actual) As machine_id,
                            b.ShippingToMachineDatetime As ShippingToMachineDatetime,
	                        (SELECT lct_nm FROM lct_info WHERE lct_cd = b.location_code ) AS lct_nm,
	                        (SELECT at_no FROM w_actual WHERE id_actual = b.id_actual) AS po
                            FROM w_material_info_mms As b
                            WHERE b.ExportCode = @ExpCode
			                Group by b.mt_no, b.status, b.material_code, b.gr_qty, b.export_date, b.receipt_date, b.expiry_date, b.id_actual, b.ShippingToMachineDatetime, b.location_code";

                var result = await base.DbConnection.QueryAsync<WMaterialInfoNewResponse>(query1, new { @ExpCode = ExportCode });



                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<ShippingScanPPResponse>> GetListShippingScanPP(string ExportCode)
        {
            try
            {
                var listData = new List<ShippingScanPPResponse>();


                var query1 = @"SELECT CONCAT(max(abc.mt_no),'(',ISNULL(FORMAT(Sum(info.gr_qty),'#,#'),0),'M)') AS mt_no, 
                            --COUNT(info.material_code) As cap,
                            COUNT(CASE WHEN info.mt_no=abc.mt_no And  info.ExportCode = @ExportCode AND info.location_code LIKE '002%' AND info.status <>'004' THEN info.material_code END) As cap,
                            COUNT(CASE WHEN info.mt_no=abc.mt_no And  info.ExportCode = @ExportCode AND info.location_code LIKE '002%' AND info.status ='005' THEN info.material_code END) As dasd,
                            COUNT(CASE WHEN info.mt_no=abc.mt_no And  info.ExportCode = @ExportCode AND info.location_code LIKE '002%' AND info.status ='002' THEN info.material_code END) As dangsd,
                            COUNT(CASE WHEN info.mt_no=abc.mt_no and  info.ExportCode =@ExportCode AND info.location_code LIKE '002%' AND (info.status ='013' OR info.status ='014' OR info.status ='004') THEN info.material_code END) AS trave
                            FROM (SELECT  max(a.ExportCode) AS ExportCode,  MAX(a.mt_no) AS mt_no 
		                            FROM inventory_products AS a
		                            WHERE a.ExportCode = @ExportCode
		                            GROUP BY a.ExportCode, a.mt_no) AS abc
                            JOIN inventory_products info ON info.ExportCode = abc.ExportCode AND abc.mt_no = info.mt_no
                            GROUP BY abc.mt_no";
                var result1 = await base.DbConnection.QueryAsync<ShippingScanPPResponse>(query1, new { @ExportCode = ExportCode });
                listData.AddRange(result1);
         //       var query2 = @"SELECT CONCAT(max(abc.mt_no),'(',sum(ISNULL(info.gr_qty,0)),'M)') AS mt_no, COUNT(info.material_code) As cap,
         //                   COUNT(CASE WHEN info.mt_no=abc.mt_no And  info.ExportCode = @ExportCode AND info.location_code LIKE '002%' AND info.status ='005' THEN info.material_code END) As dasd,
         //                   COUNT(CASE WHEN info.mt_no=abc.mt_no And  info.ExportCode = @ExportCode AND info.location_code LIKE '002%' AND info.status ='002' THEN info.material_code END) As dangsd,
         //                   COUNT(CASE WHEN info.mt_no=abc.mt_no and  info.ExportCode =@ExportCode AND info.location_code LIKE '002%' AND info.status ='013' THEN info.material_code END) AS trave
         //                   FROM (SELECT  max(a.ExportCode) AS ExportCode,  MAX(a.mt_no) AS mt_no 
		       //                     FROM w_material_info_mms AS a
		       //                     WHERE a.ExportCode = @ExportCode
									//GROUP BY a.ExportCode, a.mt_no) AS abc
         //                   JOIN w_material_info_mms info ON info.ExportCode = abc.ExportCode AND abc.mt_no = info.mt_no
         //                   GROUP BY abc.mt_no";
         //       var result2 = await base.DbConnection.QueryAsync<ShippingScanPPResponse>(query2, new { @ExportCode = ExportCode });
               // listData.AddRange(result2);


                return listData;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<LocationInfo>> GetLocationWIP(string lct_cd)
        {
            try
            {
                var query = @"SELECT * FROM lct_info Where lct_cd LIKE '002%' AND (level_cd ='001' OR level_cd ='002' ) AND (@LocationCode = '' OR @LocationCode IS NULL OR  lct_cd Like '%'+ @LocationCode +'%' ) Order by lct_cd ASC";
                var result = await base.DbConnection.QueryAsync<LocationInfo>(query, new { @LocationCode = lct_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<LocationInfo> CheckIsExistLocation(string lct_cd)
        {
            try
            {
                var query = @"SELECT  a.* FROM lct_info as a WHERE a.lct_cd = @LocationCode";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<LocationInfo>(query, new { @LocationCode = lct_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<LocationDetailResponse>> GetListLocationDetail(string lct_cd, string myDate)
        {
            try
            {
                        var query = @"   
                                        SELECT  ROW_NUMBER() OVER (ORDER BY a.location_code,  (SELECT product_cd FROM w_sd_info WHERE sd_no = max(a.sd_no))) AS stt,
								        ISNULL([dbo].TongSoMetNVL(max(a.mt_no), MAX(a.location_code)),0) AS soMet ,
								        ISNULL([dbo].TongSoCuonNVL(max(a.mt_no), MAX(a.location_code)),0) AS soCuon,
								        (SELECT md_cd FROM d_style_info WHERE style_no = (SELECT product_cd FROM w_sd_info WHERE sd_no = max(a.sd_no))) AS md_cd, 
								        (SELECT product_cd FROM w_sd_info WHERE sd_no = max(a.sd_no)) AS product_cd, 
								        (SELECT t2.lct_nm FROM lct_info t2 WHERE t2.lct_cd = max(a.location_code)) AS lct_nm,
                                        a.mt_no, 
                                        --SUM(a.gr_qty) AS soMet, 
								        longez.INPUT  AS input,
								        longez2.OUTPUT AS output
								        FROM inventory_products AS a

							            LEFT JOIN (
												select count(material_code) As INPUT,mt_no,location_code from inventory_products 
												where location_code   Like '%'+ @LocationCode + '%' 
                                                AND (@DateTime = '' OR @DateTime IS NULL OR recei_wip_date >= @DateTime)
												AND (@DateTime = '' OR @DateTime IS NULL OR recei_wip_date <=  DATEADD(day,+1,@DateTime))
												group by mt_no,location_code 
                                            ) AS longez  ON longez.mt_no = a.mt_no and  longez.location_code = a.location_code

								        LEFT JOIN (select count(material_code) As OUTPUT,mt_no,location_code from inventory_products 
											where location_code Like '%'+ @LocationCode + '%'  AND ExportCode IS NOT NULL AND ExportCode != ''
                                            AND (@DateTime = '' OR @DateTime IS NULL OR ShippingToMachineDatetime >= @DateTime)
											AND (@DateTime = '' OR @DateTime IS NULL OR ShippingToMachineDatetime <=  DATEADD(day,+1,@DateTime))
											group by mt_no,location_code 
											) AS longez2 ON longez2.mt_no = a.mt_no and  longez2.location_code = a.location_code
                                           
								        WHERE a.location_code Like '%'+ @LocationCode + '%' 
										AND a.mt_type !='CMT' AND a.mt_type != '' AND a.mt_type IS NOT NULL AND a.status = '001'
                                        AND (a.ExportCode IS NULL OR a.ExportCode = '')
								        GROUP BY a.location_code, a.mt_no,longez.INPUT, longez2.OUTPUT";
                var result = await base.DbConnection.QueryAsync<LocationDetailResponse>(query, new { @LocationCode = lct_cd, @DateTime = myDate });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<InventoryProduct> CheckInInventoryProduct(string materialCode)
        {
            try
            {
                var query = @"SELECT (SELECT lct_nm FROM lct_info WHERE lct_cd = a.location_code ) AS locationName, a.* 
                            FROM inventory_products as a WHERE a.material_code = @MaterialCode";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<InventoryProduct>(query, new { @MaterialCode = materialCode });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<CommCode>> CheckStatus(string status)
        {
            try
            {
                var query = @"Select * from comm_dt Where mt_cd = 'WHS005' and dt_cd = @Status";
                var result = await base.DbConnection.QueryAsync<CommCode>(query, new { @Status = status });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<string> GetDetailName(string mt_cd ,string status)
        {
            try
            {
                var query = @"Select dt_nm from comm_dt Where mt_cd = @Mt_Cd and dt_cd = @Status";
                var result =  await base.DbConnection.ExecuteScalarAsync<string>(query, new { @Mt_Cd = mt_cd, @Status = status });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> DeleteMaterialInfoTam(string ml_no)
        {
            try
            {
                var query = @"Delete w_material_info_tam Where mt_cd = @MlNo";
                var result = await base.DbConnection.ExecuteAsync(query, new { @MlNo = ml_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region Inventory
        public async Task<int> TotalRecordsSearchGeneralMaterialWIP(string mt_no, string product_cd, string mt_nm, DateTime? recevice_dt_start, DateTime? recevice_dt_end, string sts, string lct_cd, string mt_cd)
        {
            try
            {
                var query = @"SELECT  COUNT(*) 
			                                FROM (
					                                Select 
					                                ROW_NUMBER() OVER (ORDER BY a.mt_no) AS wmtid, 
					                                MAX(a.mt_no) As mt_no,
					                                MAX(b.mt_nm) As mt_nm,      
					                                CONCAT((Case WHEN (MAX(b.bundle_unit) = 'Roll') THEN round((sum(a.gr_qty) / max(b.spec)),2) ELSE round(MAX(a.gr_qty),2) END),' ROLL') AS qty, 
					                                SUM( CASE  WHEN a.status='002' THEN a.gr_qty ELSE 0  END) AS 'DSD',
					                                SUM( CASE WHEN (a.status='001' or a.status='004') THEN a.gr_qty ELSE 0  END)  AS 'CSD'
					                                FROM inventory_products AS a 
					                                LEFT JOIN d_material_info AS b ON a.mt_no=b.mt_no 
                                                    LEFT JOIN  w_sd_info info ON info.sd_no = a.sd_no 
					                                WHERE a.location_code  LIKE '002%' AND a.mt_type!='CMT'  AND (a.ExportCode IS NULL OR a.ExportCode = '')
                                                        AND a.status <> '005' AND a.status <> '013'
						                                AND (@Mt_No = '' OR @Mt_No IS NULL OR  a.mt_no Like '%' + @Mt_No + '%' )
                                                        AND (@Mt_Cd = '' OR @Mt_Cd IS NULL OR  a.material_code Like '%' + @Mt_Cd + '%' )
						                                AND (@Mt_Nm = '' OR @Mt_Nm IS NULL OR b.mt_nm Like '%' + @Mt_Nm + '%' ) 
                                                        AND (@ProductCode = '' OR @ProductCode IS NULL OR info.product_cd Like '%' + @ProductCode + '%' ) 
						                                AND (@LocationCode = '' OR @LocationCode IS NULL OR a.location_code Like '%' + @LocationCode + '%' ) 
                                                        AND (a.ExportCode IS NULL OR a.ExportCode = '')
						                                AND (@Start = '' OR @Start IS NULL OR a.recei_date >= @Start)  
						                                AND (@End = '' OR @End IS NULL OR a.recei_date <= @End) 
					                                GROUP BY a.mt_no
					                                ) As MyTable";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Mt_No = mt_no, @ProductCode = product_cd, @Mt_Nm = mt_nm, @Start = recevice_dt_start, @End  = recevice_dt_end, @LocationCode = lct_cd, @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //public async Task<int> TotalRecordsSearchGeneralMachine(string mt_no, string product_cd, string mt_nm, DateTime? recevice_dt_start, DateTime? recevice_dt_end, string sts, string lct_cd, string mt_cd)
        //{
        //    try
        //    {
        //        var query = @"SELECT  COUNT(*) 
			     //                           FROM (
					   //                             Select 
					   //                             ROW_NUMBER() OVER (ORDER BY a.mt_no) AS wmtid, 
					   //                             MAX(a.mt_no) As mt_no,
					   //                             MAX(b.mt_nm) As mt_nm,      
					   //                             CONCAT((Case WHEN (MAX(b.bundle_unit) = 'Roll') THEN round((sum(a.gr_qty) / max(b.spec)),2) ELSE round(MAX(a.gr_qty),2) END),' ROLL') AS qty, 
					   //                             SUM( CASE  WHEN a.status='002' THEN a.gr_qty ELSE 0  END) AS 'DSD',
					   //                             SUM( CASE WHEN (a.status='001' or a.status='004') THEN a.gr_qty ELSE 0  END)  AS 'CSD',
					   //                             SUM( CASE WHEN (a.status='005' or a.status='004') THEN a.gr_qty ELSE 0  END)  AS 'returnMachine'
					   //                             FROM inventory_products AS a 
					   //                             LEFT JOIN d_material_info AS b ON a.mt_no=b.mt_no 
        //                                            LEFT JOIN  w_sd_info info ON info.sd_no = a.sd_no 
					   //                             WHERE a.location_code  LIKE '002%' AND a.mt_type='PMT'  AND a.ExportCode IS not NULL
        //                                                AND a.status = '005' or a.status = '001' or a.status = '002'
						  //                              AND (@Mt_No = '' OR @Mt_No IS NULL OR  a.mt_no Like '%' + @Mt_No + '%' )
        //                                                AND (@Mt_Cd = '' OR @Mt_Cd IS NULL OR  a.material_code Like '%' + @Mt_Cd + '%' )
						  //                              AND (@Mt_Nm = '' OR @Mt_Nm IS NULL OR b.mt_nm Like '%' + @Mt_Nm + '%' ) 
        //                                                AND (@ProductCode = '' OR @ProductCode IS NULL OR info.product_cd Like '%' + @ProductCode + '%' ) 
						  //                              AND (@LocationCode = '' OR @LocationCode IS NULL OR a.location_code Like '%' + @LocationCode + '%' ) 
                                                      
						  //                              AND (@Start = '' OR @Start IS NULL OR a.recei_date >= @Start)  
						  //                              AND (@End = '' OR @End IS NULL OR a.recei_date <= @End) 
					   //                             GROUP BY a.mt_no
					   //                             ) As MyTable";
        //        var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Mt_No = mt_no, @ProductCode = product_cd, @Mt_Nm = mt_nm, @Start = recevice_dt_start, @End = recevice_dt_end, @LocationCode = lct_cd, @Mt_Cd = mt_cd });
        //        return result;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        public async Task<IEnumerable<GeneralWIP>> GetListGeneralMaterialWIP(string mt_no, string product_cd, string mt_nm, DateTime? recevice_dt_start, DateTime? recevice_dt_end, string sts, string lct_cd, string mt_cd)
        {
            try
            {
                var query = @"SELECT *
			                        FROM (
					                        Select 
					                        MAX(a.materialid) AS wmtid, 
					                        MAX(a.mt_no) As mt_no,
                                            max(info.product_cd) as product_cd,
					                        MAX(b.mt_nm) As mt_nm,      
					                        CONCAT((Case 
							                    WHEN (max(b.bundle_unit) = 'Roll')  
							                    THEN concat(round((sum(a.gr_qty) / max(b.spec)),2), ' Roll') 
                                                ELSE concat(round(SUM(a.gr_qty),2) ,'EA')
                                                END),'') AS qty, 
					                        SUM( CASE  WHEN a.status='002' THEN a.gr_qty ELSE 0  END) AS 'DSD',
					                        SUM( CASE WHEN (a.status='001' or a.status='004') THEN a.gr_qty ELSE 0  END)  AS 'CSD'
					                        FROM inventory_products AS a 
					                        LEFT JOIN d_material_info AS b ON a.mt_no = b.mt_no 
                                            LEFT JOIN  w_sd_info info ON info.sd_no = a.sd_no 
					                        WHERE a.location_code LIKE '002%' AND a.mt_type !='CMT' AND a.mt_type != '' AND a.mt_type IS NOT NULL 
                                            -- AND a.active = 1 
                                            AND a.status = '001' AND (a.ExportCode IS NULL OR a.ExportCode = '')
						                        AND (@Mt_No = '' OR @Mt_No IS NULL OR  a.mt_no Like '%' + @Mt_No + '%' )
                                                AND (@Mt_Cd = '' OR @Mt_Cd IS NULL OR  a.material_code Like '%' + @Mt_Cd + '%' )
						                        AND (@Mt_Nm = '' OR @Mt_Nm IS NULL OR b.mt_nm Like '%' + @Mt_Nm + '%' )  
                                                AND (@ProductCode = '' OR @ProductCode IS NULL OR info.product_cd Like '%' + @ProductCode + '%' ) 
						                        AND (@LocationCode = '' OR @LocationCode IS NULL OR a.location_code Like '%' + @LocationCode + '%' )
						                        AND (@Start = '' OR @Start IS NULL OR a.recei_date >= @Start)  
						                        AND (@End = '' OR @End IS NULL OR a.recei_date <= @End) 
					                        GROUP BY a.mt_no 
					                        ) As MyTable order by MyTable.wmtid  ";
                var result = await base.DbConnection.QueryAsync<GeneralWIP>(query, new { @Mt_No = mt_no, @ProductCode = product_cd, @Mt_Nm = mt_nm, @Start = recevice_dt_start, @End = recevice_dt_end, @LocationCode = lct_cd, @Mt_Cd = mt_cd});
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<IEnumerable<GeneralResponse>> GetListGeneral(string at_no, string model, string product, string product_name, string reg_dt_start, string reg_dt_end, string mt_cd, string bb_no, string status)
        {
            try
            {
                var query = @"  Select MAX(a.wmtid) As wmtid, Sum(a.BTP) AS 'BTP', Sum(TP) AS 'TP', product_cd, product_nm,model, ConCat(COUNT(qty), ' ', 'Roll') As qty, max(a.reg_date) as reg_date From
                              (
                              SELECT a.wmtid,
                                  Count(1) As qty, MAX(b.product) As product_cd,
                              (SELECT style_nm FROM d_style_info WHERE style_no = b.product) As product_nm,
                               (SELECT md_cd FROM d_style_info WHERE style_no = b.product) As model,
                                    case when
                                        b.level != (SELECT MAX(level) As level_id FROM w_actual WHERE at_no = b.at_no AND type = 'SX') AND b.type != 'TIMS'
                                      AND(@Product = '' OR @Product IS NULL OR  b.product like '%' + @Product + '%')
                                      AND(@Bb_No = '' OR  @Bb_No IS NULL OR a.bb_no like '%' + @Bb_No + '%')
                                    then SUM(a.gr_qty)
                                    ELSE 0
                                    END AS 'BTP',
                        
                                    case when
                                         b.level = (SELECT MAX(level) As level_id FROM w_actual WHERE at_no = b.at_no AND type = 'SX' ) AND b.type != 'TIMS'
                                      AND(@Product = '' OR @Product IS NULL OR  b.product like '%' + @Product + '%')
                                      AND(@Bb_No = '' OR  @Bb_No IS NULL OR a.bb_no like '%' + @Bb_No + '%')
                                    then SUM(a.real_qty)
                                    ELSE 0
                                    END AS 'TP',max(b.at_no) as at_no ,max(a.material_code) as material_code ,a.reg_date

                        FROM w_material_info_mms a
                        JOIN w_actual AS b ON a.id_actual = b.id_actual


                        WHERE a.status = '002' AND a.material_type = 'CMT' AND a.location_code Like '002%' AND b.type = 'SX'
                        AND(@Product = '' OR @Product IS NULL OR  b.product like '%' + @Product + '%')
                        AND(@Bb_No = '' OR  @Bb_No IS NULL OR a.bb_no like '%' + @Bb_No + '%')
                        GROUP BY  b.product, a.wmtid,  b.at_no, b.level, b.type, a.gr_qty, a.bb_no,a.reg_date
                            ) As a
                        WHERE(@Status = 'BTP' OR @Status = '' OR @Status IS NULL OR @Status = 'TP'  And a.TP > 0)
                              AND(@Status = 'TP' OR @Status = '' OR @Status IS NULL OR @Status = 'BTP' And a.BTP > 0)
                              AND(@at_no = '' OR @at_no = '' OR @at_no IS NULL OR  a.at_no like '%' + @at_no  + '%')
                              AND(@model = '' OR @model = '' OR @model IS NULL OR  a.model like '%' + @model + '%')
                              AND(@product_name = '' OR @product_name = '' OR @product_name IS NULL OR a.product_nm like '%' +  @product_name + '%')
                              AND(@Product = '' OR @Product = '' OR @product_name IS NULL OR a.product_cd like '%' + @Product  + '%' )
                               AND(@mt_cd = '' OR @mt_cd = '' OR @mt_cd IS NULL OR @mt_cd = a.material_code)
                              and(@reg_dt_start = '' OR FORMAT(CAST(a.reg_date AS datetime), 'yyyy-MM-dd') >= @reg_dt_start)
                              and(@reg_dt_end = ''   OR FORMAT(CAST(a.reg_date AS datetime), 'yyyy-MM-dd') <= @reg_dt_end)
                        GROUP BY a.model, a.product_cd, a.product_nm--,a.at_no,
                        Order by a.product_cd";
                var result = await base.DbConnection.QueryAsync<GeneralResponse>(query, new { @Product = product, @Bb_No = bb_no, @Status = status,@at_no=at_no,@model =model,@product_name =product_name,@mt_cd = mt_cd, @reg_dt_end =reg_dt_end,@reg_dt_start =reg_dt_start});
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<ExcelInventoryWIPComposite>> printExcelTab2(string at_no, string model, string product, string product_name, string reg_dt_start, string reg_dt_end, string mt_cd, string bb_no, string status)
        {
            try
            {
                var query = @"WITH GETDATA AS(
SELECT max(table1.wmtid) as wmtid,max(TABLE1.md_cd) model, table1.product product_cd,max(table1.style_nm) product_name, ''as at_no,'' as bb_no,  '' as mt_cd, concat(COUNT(1), ' Roll') quantity ,
	   sum(table1.BTP) AS 'BTP', SUM(table1.gr_qty) Total,                               
       sum(table1.TP) AS 'TP' ,'' create_date
                                     FROM (
									 SELECT a.wmtid, b.product, c.style_nm,c.md_cd,
									  case when
					                  b.level != (SELECT MAX(level) As level_id FROM w_actual WHERE at_no = b.at_no AND type='SX') AND b.type!='TIMS'
                                      AND (@Product = '' OR @Product IS NULL OR  b.product like '%' + @Product + '%' ) 
                                      AND ( @Bb_No ='' OR  @Bb_No IS NULL OR a.bb_no like '%' + @Bb_No + '%' ) 
				                    then SUM(a.gr_qty) 
				                    ELSE 0
				                    END AS 'BTP' ,
									case when
					                  b.level = (SELECT MAX(level) As level_id FROM w_actual WHERE at_no = b.at_no AND type='SX' ) AND b.type!='TIMS'
                                      AND (@Product = '' OR @Product IS NULL OR  b.product like '%' + @Product + '%' ) 
                                      AND ( @Bb_No ='' OR  @Bb_No IS NULL OR a.bb_no like '%' + @Bb_No + '%' ) 
				                    then SUM(a.real_qty)
				                    ELSE 0
				                    END AS 'TP',
									b.at_no,a.gr_qty,a.real_qty,a.material_code mt_cd,a.bb_no,
                                      (
                                        CASE 
                                        WHEN ('08:00:00' <= FORMAT( CAST( a.reg_date AS datetime ),'HH:mm:ss') AND  FORMAT( CAST( a.reg_date AS datetime ),'HH:mm:ss')  <  '23:59:59') THEN
                                        FORMAT( CAST( a.reg_date AS DATETIME ),'yyyy-MM-dd')

                                        when (FORMAT( CAST( a.reg_date AS datetime ),'HH:mm:ss')  < '08:00:00') THEN FORMAT(DATEADD(day,-1,a.reg_date),'yyyy-MM-dd')
                                          ELSE ''
                                        END )  as reg_date
                            FROM  w_material_info_mms a 
                            JOIN w_actual AS b ON a.id_actual=b.id_actual 
                            JOIN d_style_info AS c ON b.product = c.style_no 
                            WHERE (a.location_code = '002002000000000000' OR a.location_code = '002000000000000000') 
                            AND a.material_type='CMT'
                           --  AND ( a.mt_sts_cd NOT IN ('005','003') ) 
                         AND a.status = '002'
   	                        AND	 (@at_no='' OR b.at_no LIKE CONCAT('%',@at_no,'%')) 
   	                        AND	 (@model='' OR c.md_cd LIKE CONCAT('%',@model,'%')) 
   	                        AND	 (@product='' OR b.product LIKE CONCAT('%',@product,'%')) 
   	                        AND	 (@product_name='' OR c.style_nm LIKE CONCAT('%',@product_name,'%')) 
   	                        AND	 (@mt_cd='' OR a.material_code LIKE CONCAT('%',@mt_cd,'%')) 
   	                        AND	 (@bb_no='' OR a.bb_no LIKE CONCAT('%',@bb_no,'%')) 
                       group by a.wmtid, b.product, c.style_nm,c.md_cd,b.at_no,a.gr_qty,a.real_qty,a.material_code,a.bb_no,level,type,a.reg_date  ) table1
	                        WHERE  (@reg_dt_start='' OR FORMAT( CAST( table1.reg_date AS datetime ),'yyyy-MM-dd')  >= @reg_dt_start)
                                   and (@reg_dt_end=''   OR FORMAT( CAST( table1.reg_date AS datetime ),'yyyy-MM-dd') <= @reg_dt_end)
                        GROUP BY table1.product
                        UNION 
                        SELECT  TABLE2.wmtid,'' md_cd,TABLE2.product, '' style_nm,TABLE2.at_no,
                          TABLE2.bb_no, TABLE2.mt_cd, TABLE2.qty,'','','',CONCAT(FORMAT( CAST( TABLE2.reg_date AS DATETIME ),'yyyy-MM-dd'),'')
                        FROM (
                        SELECT a.wmtid,b.at_no at_no,
                          a.bb_no, a.material_code mt_cd, b.product,

                              CONCAT ( ( CASE 
                                   WHEN d.bundle_unit = 'Roll' THEN Round(( a.gr_qty / d.spec ), 2) 
                                   ELSE Round(a.gr_qty, 2) 
                                 END )   , ' EA'  )   qty, 
                               a.reg_date ,
                                (CASE WHEN ('08:00:00' <= FORMAT( CAST( a.reg_date AS datetime ),'HH:mm:ss') AND  FORMAT( CAST( a.reg_date AS datetime ),'HH:mm:ss')  <  '23:59:59') THEN
                                    FORMAT( CAST( a.reg_date AS DATETIME ),'yyyy-MM-dd') when (FORMAT( CAST( a.reg_date AS datetime ),'HH:mm:ss')  < '08:00:00') THEN  FORMAT(DATEADD(day,-1,a.reg_date),'yyyy-MM-dd')
                                      ELSE ''END )  as reg_date1
						  FROM  w_material_info_mms a 
                          LEFT  JOIN w_actual AS b ON a.id_actual=b.id_actual 
                          LEFT  JOIN d_style_info AS c ON b.product = c.style_no 
						  LEFT	JOIN d_material_info AS d ON d.mt_no=a.mt_no 
                        WHERE a.location_code LIKE '002%' AND a.material_type= 'CMT' 
                        -- AND a.mt_sts_cd NOT IN ('005','003') 
                        AND a.status = '002'
	                        AND	 (@at_no='' OR b.at_no LIKE CONCAT('%',@at_no,'%')) 
   	                        AND	 (@model='' OR c.md_cd LIKE CONCAT('%',@model,'%')) 
   	                        AND	 (@Product='' OR b.product LIKE CONCAT('%',@Product,'%')) 
   	                        AND	 (@product_name='' OR c.style_nm LIKE CONCAT('%',@product_name,'%')) 
   	                        AND	 (@mt_cd='' OR a.material_code LIKE CONCAT('%',@mt_cd,'%')) 
   	                        AND	 (@bb_no='' OR a.bb_no LIKE CONCAT('%',@bb_no,'%')) 
                        ) TABLE2
		                        where  (@reg_dt_start='' OR FORMAT( CAST( TABLE2.reg_date AS datetime ),'yyyy-MM-dd')  >= @reg_dt_start)
                                       and (@reg_dt_end=''   OR FORMAT( CAST( TABLE2.reg_date AS datetime ),'yyyy-MM-dd') <= @reg_dt_end)
                        )
SELECT * FROM GETDATA  a order by a.product_cd, a.at_no 
";
               var result = await base.DbConnection.QueryAsync<ExcelInventoryWIPComposite>(query, new { @Product = product, @Bb_No = bb_no, @Status = status,@at_no=at_no,@model =model,@product_name =product_name,@mt_cd = mt_cd, @reg_dt_end =reg_dt_end,@reg_dt_start =reg_dt_start});
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<MemoResponse>> GetListMemoInfo(string MTCode, string memoProductCode)
        {
            try
            {
                var query = @"SELECT 
		                            Max(a.mt_cd) AS mt_cd 
		                            , MAX(a.width) AS width 
		                            , MAX(a.spec) AS spec 
		                            , SUM(a.TX) AS total_roll 
		                            , MAX(a.chg_dt) AS chg_dt 
		                            FROM w_material_info_memo a 
		                            WHERE (@MTCode = '' OR  @MTCode IS NULL OR a.mt_cd LIKE '%' + @MTCode + '%')
		                            AND ( @memoProductCode = '' OR  @memoProductCode IS NULL OR a.style_no LIKE '%'+ @memoProductCode +'%') 
		                            GROUP BY a.width, a.spec, a.mt_cd";
                var result = await base.DbConnection.QueryAsync<MemoResponse>(query, new { @MTCode = MTCode,  @memoProductCode = memoProductCode});
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<GeneralDetailWIP>> GetListGeneralMaterialDetailWIP(string mt_no, string product_cd, string mt_nm, DateTime? recevice_dt_start, DateTime? recevice_dt_end, string sts, string lct_cd, string mt_cd)
        {
            try
            {
                var query = @"SELECT a.materialid As wmtid, a.material_code, b.mt_nm,
                            CONCAT(ISNULL(a.gr_qty,''),ISNULL(b.unit_cd,'')) lenght,
                            CONCAT(ISNULL(b.width,0),'*',ISNULL(a.gr_qty,0)) AS size,
                            ISNULL(b.spec,0) spec,a.mt_no, 
                                (case when b.bundle_unit ='Roll' then  ROUND((a.gr_qty/b.spec),2) ELSE ROUND(a.gr_qty,2) END) qty, 
                                b.bundle_unit, 
                                a.recei_wip_date As receipt_date, 
                                a.sd_no,
                                (SELECT dt_nm FROM comm_dt WHERE comm_dt.dt_cd=a.status AND comm_dt.mt_cd='WHS005') sts_nm,
                                (SELECT lct_nm FROM lct_info WHERE lct_cd = a.location_code) AS lct_nm
                                FROM inventory_products a 
                                LEFT JOIN d_material_info  b ON a.mt_no=b.mt_no 
                                LEFT JOIN  w_sd_info info ON info.sd_no = a.sd_no  
                                WHERE a.location_code LIKE '002%' AND a.mt_type<> 'CMT'
                                -- AND a.active = 1
                                AND a.status = '001' AND (a.ExportCode IS NULL OR a.ExportCode = '')
                                AND (@Mt_No = '' OR @Mt_No IS NULL OR  a.mt_no Like '%' + @Mt_No + '%' )
                                AND (@Mt_Nm = '' OR @Mt_Nm IS NULL OR b.mt_nm Like '%' + @Mt_Nm + '%' ) 
                                AND (@Mt_Cd = '' OR @Mt_Cd IS NULL OR a.material_code Like '%' +  @Mt_Cd + '%' )
                                AND (@ProductCode = '' OR @ProductCode IS NULL OR info.product_cd Like '%' + @ProductCode + '%' ) 
                                AND (@LocationCode = '' OR @LocationCode IS NULL OR a.location_code Like '%' + @LocationCode + '%' )
                                AND (@Start = '' OR @Start IS NULL OR a.recei_date >= @Start)  
                                AND (@End = '' OR @End IS NULL OR a.recei_date <= @End) 
                                Order By a.sd_no Asc";

                var result = await base.DbConnection.QueryAsync<GeneralDetailWIP>(query, new { @Mt_No = mt_no, @ProductCode = product_cd, @Mt_Nm = mt_nm, @Start = recevice_dt_start, @End = recevice_dt_end, @LocationCode = lct_cd, @Mt_Cd = mt_cd, @Status = sts});
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<GeneralDetailWIP>> GetListGeneralMaterialDetail(string mt_no, string product_cd, string mt_nm, DateTime? recevice_dt_start, DateTime? recevice_dt_end, string sts, string mt_cd)
        {
            try
            {
                var query = @"SELECT a.materialid As wmtid,a.ExportCode,a.material_code, b.mt_nm,
                            CONCAT(ISNULL(a.gr_qty,''),ISNULL(b.unit_cd,'')) lenght,
                            CONCAT(ISNULL(b.width,0),'*',ISNULL(a.gr_qty,0)) AS size,
                            ISNULL(b.spec,0) spec,a.mt_no, 
                                (case when b.bundle_unit ='Roll' then  ROUND((a.gr_qty/b.spec),2) ELSE ROUND(a.gr_qty,2) END) qty, b.bundle_unit, 
                                a.recei_wip_date As receipt_date, 
                                a.sd_no,
                                (SELECT dt_nm FROM comm_dt WHERE comm_dt.dt_cd=a.status AND comm_dt.mt_cd='WHS005') sts_nm
                                FROM inventory_products a 
                                LEFT JOIN d_material_info  b ON a.mt_no=b.mt_no 
                                LEFT JOIN  w_sd_info info ON info.sd_no = a.sd_no  
                                WHERE a.location_code LIKE '002%' AND a.mt_type = 'PMT' AND b.active = 1 AND (a.status='001' or a.status='002' or a.status='004' ) and a.ExportCode IS NOT null and a.ExportCode != ''
                                    AND a.status like @Status
                                AND (@Mt_No = '' OR @Mt_No IS NULL OR  a.mt_no Like '%' + @Mt_No + '%' )
                                AND (@Mt_Nm = '' OR @Mt_Nm IS NULL OR b.mt_nm Like '%' + @Mt_Nm + '%' ) 
                                AND (@Mt_Cd = '' OR @Mt_Cd IS NULL OR a.material_code Like '%' +  @Mt_Cd + '%' )
                                AND (@ProductCode = '' OR @ProductCode IS NULL OR info.product_cd Like '%' + @ProductCode + '%' ) 
                               
                                AND (@Start = '' OR @Start IS NULL OR a.recei_date >= @Start)  
                                AND (@End = '' OR @End IS NULL OR a.recei_date <= @End) 
                                Order By a.ExportCode Asc";

                var result = await base.DbConnection.QueryAsync<GeneralDetailWIP>(query, new { @Mt_No = mt_no, @ProductCode = product_cd, @Mt_Nm = mt_nm, @Start = recevice_dt_start, @End = recevice_dt_end, @Mt_Cd = mt_cd, @Status = '%' + sts + '%' });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<IEnumerable<MemoDetailResponse>> GetListMemoDetail(string mtCodeMemo, string memoWidth, string memoSpec, string productCodeMemo)
        {
            try
            {
                var query = @"SELECT a.id, a.md_cd, a.style_no, a.style_nm, a.width, a.width_unit, a.spec, a.spec_unit, a.TX, a.receiving_dt
                            FROM w_material_info_memo a 
                            WHERE (@MTCodeMemo = '' OR @MTCodeMemo IS NULL OR a.mt_cd LIKE '%' + @MTCodeMemo + '%')
                            AND (@MemoWidth = '' OR @MemoWidth IS NULL OR a.width LIKE '%' + @MemoWidth + '%') 
                            AND (@MemoSpec = '' OR @MemoSpec IS NULL OR a.spec LIKE '%' + @MemoSpec + '%')
                            AND (@ProductCodeMemo = '' OR @ProductCodeMemo IS NULL OR a.style_no LIKE '%' + @ProductCodeMemo + '%')";
                var result = await base.DbConnection.QueryAsync<MemoDetailResponse>(query, new { @MTCodeMemo = mtCodeMemo, @MemoWidth = memoWidth, @MemoSpec = memoSpec, @ProductCodeMemo = productCodeMemo });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<GeneralWIP>> GetListGeneralExportToMachine(string mt_no, string product_cd, string mt_nm, DateTime? recevice_dt_start, DateTime? recevice_dt_end, string sts, string mt_cd)
        {
            try
            {
                var query = @"SELECT *
			                        FROM (
					                        Select 
					                        ROW_NUMBER() OVER (ORDER BY a.mt_no) AS wmtid, 
					                        MAX(a.mt_no) As mt_no,
					                        MAX(info.product_cd) As product_cd,
					                        MAX(b.mt_nm) As mt_nm,      
					                        CONCAT((Case 
							                    WHEN (max(b.bundle_unit) = 'Roll')  
							                    THEN concat(round((sum(a.gr_qty) / max(b.spec)),2), ' Roll') 
                                                ELSE concat(round(SUM(a.gr_qty),2) ,'EA')
                                                END),'') AS qty, 
					                        SUM( CASE  WHEN a.status='002' THEN a.gr_qty ELSE 0  END) AS 'DSD',
					                        SUM( CASE WHEN (a.status='001') THEN a.gr_qty ELSE 0  END)  AS 'CSD',
                                            SUM(CASE WHEN (a.status='004' )	THEN a.gr_qty  ELSE 0  END) AS 'returnMachine' 
                                           

					                        FROM inventory_products AS a 
					                        LEFT JOIN d_material_info AS b ON a.mt_no = b.mt_no 
                                            LEFT JOIN  w_sd_info info ON info.sd_no = a.sd_no 
					                        WHERE a.location_code LIKE '002%' AND a.mt_type ='PMT' 

                                                AND (a.status='001' or a.status='002' or a.status='004' ) 
                                                AND a.status like @sts
                                                and a.ExportCode IS NOT null and a.ExportCode != ''
						                        AND (@Mt_No = '' OR @Mt_No IS NULL OR  a.mt_no Like '%' + @Mt_No + '%' )
                                                AND (@Mt_Cd = '' OR @Mt_Cd IS NULL OR  a.material_code Like '%' + @Mt_Cd + '%' )
						                        AND (@Mt_Nm = '' OR @Mt_Nm IS NULL OR b.mt_nm Like '%' + @Mt_Nm + '%' )  
                                                AND (@ProductCode = '' OR @ProductCode IS NULL OR info.product_cd Like '%' + @ProductCode + '%' ) 
						                  
						                        AND (@Start = '' OR @Start IS NULL OR a.recei_date >= @Start)  
						                        AND (@End = '' OR @End IS NULL OR a.recei_date <= @End) 
					                        GROUP BY a.mt_no
					                        ) As MyTable";
                var result = await base.DbConnection.QueryAsync<GeneralWIP>(query, new { @Mt_No = mt_no, @ProductCode = product_cd, @Mt_Nm = mt_nm, @Start = recevice_dt_start, @End = recevice_dt_end, @Mt_Cd = mt_cd, @sts ='%'+ sts +'%'});
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        #endregion

        #region Receiving Scan (WIP-WMS)

        public async Task<MaterialInfoTam> GetDataMaterialInfoTam(string ml_no)
        {
            try
            {
                var query = @"Select Top 1 mt_no, mt_type, gr_qty, real_qty, expiry_dt, expore_dt, dt_of_receipt, lot_no From w_material_info_tam Where mt_cd = @Ml_No And active = 1";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoTam>(query, new { @Ml_No = ml_no });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion

        #region Shipping Scan (WIP-WMS)
        public async Task<IEnumerable<ExInfo>> GetListDataExInfo(string ex_no, string ex_nm)
        {
            try
            {
                var query = @"SELECT a.* 
                            FROM w_ex_info as a 
                            WHERE a.use_yn ='Y'
                            AND (@Ex_No='' OR @Ex_No IS NULL OR  a.ex_no Like '%' + @Ex_No + '%' )
                            AND (@Ex_Nm ='' OR @Ex_Nm IS NULL OR a.ex_nm Like '%' + @Ex_Nm + '%' )
                            Order by exid desc";
                var result = await base.DbConnection.QueryAsync<ExInfo>(query, new { @Ex_No = ex_no, @Ex_Nm = ex_nm});
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<ExInfo>> GetListDataExInfo(string ex_no)
        {
            try
            {
                var query = @"SELECT ex_no, ex_nm, status, remark
                            FROM w_ex_info
                            Where (@Ex_No='' OR @Ex_No IS NULL OR  ex_no = @Ex_No )";
                var result = await base.DbConnection.QueryAsync<ExInfo>(query, new { @Ex_No = ex_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> InsertIntoExInfo(ExInfo item)
        {
            try
            {
                var query = @"Insert Into w_ex_info (ex_no, ex_nm, status, work_dt, lct_cd, alert, remark, use_yn, reg_id, reg_dt, chg_id, chg_dt)
                                Values(@ex_no, @ex_nm, @status, @work_dt, @lct_cd, @alert, @remark, @use_yn, @reg_id, @reg_dt, @chg_id, @chg_dt)
                                Select SCOPE_IDENTITY()";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> InsertIntoExInfo(string data)
        {
            try
            {
                int result = 0;
                var listData = data.Split(',');
                foreach (var item in listData)
                {
                    var query = @"INSERT INTO w_material_info_tam  (mt_type, mt_cd,mt_no, gr_qty,real_qty, status, date, lot_no, bbmp_sts_cd, expiry_dt,dt_of_receipt,expore_dt) 
                            SELECT mt_type, material_code, mt_no, gr_qty, real_qty,'000',GetDate(), lot_no, '000', expiry_date,date_of_receipt,export_date 
                            FROM inventory_products 
                            WHERE materialid = @Data";
                    result = await base.DbConnection.ExecuteAsync(query, new { @Data = item});
                }

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<ExInfo> GetExInfoById(int exid)
        {
            try
            {
                var query = @"Select * from w_ex_info where exid = @Id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<ExInfo>(query, new { @Id = exid});
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateToInvertoryProduct(string ex_no)
        {
            try
            {
                var query = @"UPDATE inventory_products SET ex_no = '' , status ='001' , location_code ='002000000000000000' WHERE (@ExNo = '' OR @ExNo IS NULL OR ex_no = @ExNo)";
                var result = await base.DbConnection.ExecuteAsync(query, new { @ExNo = ex_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> DeleteExInfoById(int exid)
        {
            try
            {
                var query = @"Delete w_ex_info WHERE exid = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Id = exid });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateExInfo(ExInfo item)
        {
            try
            {
                var query = @"update w_ex_info Set ex_no = @ex_no, ex_nm = @ex_nm, status = @status, work_dt = @work_dt, lct_cd = @lct_cd, alert = @alert, remark = @remark, use_yn = @use_yn, 
                            reg_id = @reg_id, reg_dt = @reg_dt, chg_id = @chg_id, chg_dt = @chg_dt
                            Where exid = @exid";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdatePartialStatusExInfo(string data)
        {
            try
            {
                var result = 0;
                var listData = data.Split(',');
                foreach (var item in listData)
                {
                    var query = @"Update w_ex_info SET status = '001' Where ex_no  = @ex_no ";
                    result = await base.DbConnection.ExecuteAsync(query, new { @ex_no =  data});
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<int> InsertIntoStatusExInfo(string data)
        {
            try
            {
                var result = 0;
                var listData = data.Split(',');
                foreach (var item in listData)
                {
                    var query = @"INSERT INTO w_material_info_tam  
                                (mt_type, mt_cd,mt_no, gr_qty,real_qty, status, date, lot_no, bbmp_sts_cd, expiry_dt,dt_of_receipt,expore_dt) 

                                SELECT mt_type, material_code, mt_no, gr_qty, real_qty,'000',GetDate(), lot_no, '000', cast(expiry_date as date),cast(date_of_receipt as date),cast(export_date as date) 
                                FROM inventory_products 
                                WHERE materialid = @data ";
                    result = await base.DbConnection.ExecuteAsync(query, new { @data = item });
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<int> CheckMaterialInventoryProduct(string mt_cd)
        {
            try
            {
                var query = @"Select Count(*) from inventory_products Where material_code = @MtCd";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @MtCd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> CheckMaterialInfoTam(string mt_cd)
        {
            try
            {
                var query = @"Select Count(*) from w_material_info_tam Where mt_cd = @MtCd";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @MtCd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<string> GetLastDataExInfo()
        {
            try
            {
                var query = @"Select Top 1 ex_no from w_ex_info Order by exid Desc";
                var result = await base.DbConnection.ExecuteScalarAsync<string>(query);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<MaterialInfoMemo>> GetListDataMaterialMemo(string sd_no)
        {
            try
            {
                var query = @"Select id, mt_cd, lot_no, memo From w_material_info_memo Where sd_no = @SdNo";
                var result = await base.DbConnection.QueryAsync<MaterialInfoMemo>(query, new { @SdNo = sd_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<PickingScanPPCountMTNoResponse>> GetPickingScanPP_Count_MT_no(string ex_no)
        {
            try
            {
                //SELECT a.mt_no, a.mt_cd, a.status
                //            FROM w_material_info_tam AS a
                //            WHERE a.ex_no = @ExNo AND a.lct_cd LIKE '002%'
                //            UNION ALL
                var query = @"SELECT COUNT(material_code) AS cap, max(abc.mt_no) AS  mt_no, max(abc.status) AS status,  
                            (SELECT COUNT(trave.material_code) FROM inventory_products AS trave WHERE trave.mt_no =  max(abc.mt_no) And trave.ex_no = @ExNo AND trave.location_code LIKE '002%' AND trave.status ='013' ) as trave 
                            FROM(
                                SELECT b.mt_no, b.material_code, b.status
                                FROM inventory_products AS b
                                WHERE b.ex_no = @ExNo AND b.location_code LIKE '002%') AS abc
                            GROUP BY abc.mt_no
                            ";
                var result = await base.DbConnection.QueryAsync<PickingScanPPCountMTNoResponse>(query, new { @ExNo = ex_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<ShippingWIPListPPResponse>> GetShippingWIPListPP(string ex_no)
        {
            try
            {
                var query = @"Select materialid As wmtid, material_code As mt_cd , mt_type, gr_qty, from_lct_cd, status, change_date As recevice_dt_tims,
				            (Select Top 1 lct_nm From lct_info Where lct_cd = lct_cd) As from_lct_nm,
				            (Select dt_nm From comm_dt Where mt_cd = 'WHS002' And dt_cd = lct_sts_cd) As lct_sts_cd,
				            (Select dt_nm From comm_dt Where mt_cd = 'COM004' And dt_cd = mt_type) As mt_type_nm,
				            (Select dt_nm From comm_dt Where mt_cd = 'WHS005' And dt_cd = status) As sts_nm,
				            (Select Top 1 bb_no From w_material_info_mms where wmtid = wmtid) As bb_no
				            From inventory_products 
				            Where ex_no = @ExNo";
                var result = await base.DbConnection.QueryAsync<ShippingWIPListPPResponse>(query, new { @ExNo = ex_no });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UpdateListMaterialToMachine(InventoryProduct item, List<string> data)
        {
            try
            {
                var result = 0;
                foreach (var id in data)
                {
                    var query = @"Update inventory_products SET ExportCode = @ExportCode ,LoctionMachine = @LoctionMachine, change_date = @change_date, ShippingToMachineDatetime = @ShippingDateTime
                              WHERE materialid = @Id";
                    result = await base.DbConnection.ExecuteAsync(query, new { @ExportCode = item.ExportCode, @LoctionMachine = item.LoctionMachine, @change_date = item.change_date, @ShippingDateTime = item.ShippingToMachineDatetime, @Id = id  });
                }

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        public async Task<int> UpdateShippingMaterialReturn(string data, string ex_no, string full_date, string user)
        {
            try
            {
                var result = 0;
                var listData = data.Split(',');
                foreach (var item in listData)
                {
                    var query = @"UPDATE  inventory_products 
                                SET material_code = CONCAT(material_code, '-TRA', [dbo].[GetNumberTra_Wip](material_code)),  ex_no = @Ex_No , status = '013',
                                location_code = '002000000000000000', shipping_wip_dt = @Full_Date, change_id = @User, change_date = @Full_Date  
	                            WHERE materialid = @Data";
                    result = await base.DbConnection.ExecuteAsync(query ,new { @Data = item, @Ex_No = ex_no, @User = user, @Full_Date =  full_date});
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<string> GetDetailNameByComm_DT(string mt_cd, string dt_cd)
        {
            try
            {
                var query = @"Select dt_nm From comm_dt where mt_cd = @Mt_Cd and dt_cd = @Status";
                var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @Status =  dt_cd, @Mt_Cd  =  mt_cd});
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion


        #region Change Rack

        public async Task<IEnumerable<InventoryProduct>> CheckNewMaterialInventoryProduct(string materialCode)
        {
            try
            {
                var query = @"SELECT (SELECT lct_nm FROM lct_info WHERE lct_cd = a.location_code ) AS locationName, a.material_code As mt_cd, a.* 
                              FROM inventory_products as a WHERE a.material_code = @Mt_Cd ";
                var result = await base.DbConnection.QueryAsync<InventoryProduct>(query, new { @Mt_Cd = materialCode});
                return result;
            }
            catch (Exception e )
            {
                throw e;
            }
        }

        public async Task<int> UpdateChangeRackMaterialToMachine(InventoryProduct item, string ListId)
        {
            try
            {
                var result = 0;
                var listData = ListId.Split(',');
                foreach (var id in listData)
                {
                    var query = @"Update inventory_products SET change_date = @ChangeDate, change_id = @ChangeId, location_code = @LocationCode
                                WHERE materialid = @Id";
                    result = await base.DbConnection.ExecuteAsync(query, new {@Id = id, @ChangeDate = item.change_date, @ChangeId = item.change_id,
                                                                                @LocationCode = item.location_code, @ReceWipDate = item.recei_wip_date
                    });
                }

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<int> UpdateReturnMaterialToWIP(InventoryProduct item, string ListId)
        {
            try
            {
                var result = 0;
                var listData = ListId.Split(',');
                foreach (var id in listData)
                {
                    var query = @"
                                Update inventory_products 
                                SET change_date = @ChangeDate, 
                                change_id = @ChangeId, 
                                location_code = @LocationCode, 
                                ShippingToMachineDatetime = @ShippingMachineDatetime,
                                ExportCode = @ExportCode,
                                LoctionMachine = @LoctionMachine                  
                                WHERE materialid = @Id";
                    result = await base.DbConnection.ExecuteAsync(query, new
                    {
                        @Id = id,
                        @ChangeDate = item.change_date,
                        @ChangeId = item.change_id,
                        @LocationCode = item.location_code,
                        @ShippingMachineDatetime = item.ShippingToMachineDatetime,
                        @ExportCode = item.ExportCode,
                        @LoctionMachine = item.LoctionMachine
                    });
                }

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> InsertReturnMaterialToWIP(string ListId)
        {
            try
            {
                var result = 0;
                var listData = ListId.Split(',');
                foreach (var id in listData)
                {
                    var query = @"INSERT INTO inventory_products(mt_no, material_code, mt_type, gr_qty, real_qty, ExportCode, LoctionMachine, ShippingToMachineDatetime, 
                                location_code, a.shipping_wip_dt, status, picking_date)
                                SELECT a.mt_no, concat(a.material_code,'-SP-', Format(GetDate(),'%y%m%d%H%i%s')), a.mt_type,  a.gr_qty, a.real_qty, a.ExportCode, 
		                            a.LoctionMachine, ShippingToMachineDatetime, a.location_code, a.shipping_wip_dt, '014', GetDate()
                                FROM inventory_products  as a
                                WHERE  a.materialid = @Id";
                    result = await base.DbConnection.ExecuteAsync(query, new { @Id = id});
                }

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> DeleteMaterialInfoTamByCode(string mt_cd)
        {
            try
            {
                var query = @"DELETE w_material_info_tam WHERE mt_cd = @Mt_Cd";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<wipgeneralexportexcel>> ExportMaterialByExcel()
        {
            try
            {
                var query = @" SELECT * FROM ( 
                                        SELECT one.product_cd ProductCode, one.mt_no 'CODE', '' AS 'CompositeCode',one.mt_nm 'NAME', CONCAT(ISNULL(one.qty, 0), ' ', 
			                            ISNULL(one.bundle_unit, '')) 'QTY', CONCAT(ISNULL(one.qty2, 0), ' ', ISNULL(one.bundle_unit, '')) stk_qty, 
                                        one.lenght 'LENGTH', one.size SIZE,'' AS STATUS, '' AS 'ReceviceDate',one.mt_no 'MT_NO' 
                                        FROM [dbo].[v_excelwipgeneral_one] As one
                                        UNION 
                                        SELECT '' AS ProductCode,'' AS 'CODE',two.mt_cd 'CompositeCode', two.mt_nm 'NAME', 
                                        two.qty 'QTY', '' stk_qty, two.lenght 'LENGTH', two.size SIZE, two.sts_nm 'STATUS', two.recevice_dt As ReceviceDATE, 
                                        two.mt_no 'MT_NO' 
                                        FROM [dbo].[v_excelwipgeneral_two] As two
                                ) AS RESULTS 
	                        Where RESULTS.STATUS = N'Đang Sử Dụng'";
                var result = await base.DbConnection.QueryAsync<wipgeneralexportexcel>(query);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<WIP_ParentInventoryExport>> PrintMaterialByExcel(string mt_no,string mt_cd,string s_product_cd, string s_locationNAme, string recevice_dt_start, string recevice_dt_end)
        {
            try
            {
                var query = @"
					                         with getdata as (  Select 
					                        MAX(a.materialid) AS wmtid, 
					                        MAX(a.mt_no) As mt_no,
											MAX(b.mt_nm) As mt_nm,''sd_no,
                                            max(info.product_cd) as product_cd,
					                        '' as mt_cd,
					                        SUM( CASE  WHEN a.status='002' THEN a.gr_qty ELSE 0  END) AS 'DSD',
					                        CONCAT(SUM( CASE WHEN (a.status='001' or a.status='004') THEN a.gr_qty ELSE 0  END),'M')  AS 'CSD',
											''size,''spec, CONCAT((Case 
							                    WHEN (max(b.bundle_unit) = 'Roll')  
							                    THEN concat(round((sum(a.gr_qty) / max(b.spec)),2), ' Roll') 
                                                ELSE concat(round(SUM(a.gr_qty),2) ,'EA')
                                                END),'') AS qty,''receipt_date,''sts_nm,''lct_nm
					                        FROM inventory_products AS a 
					                        LEFT JOIN d_material_info AS b ON a.mt_no = b.mt_no 
                                            LEFT JOIN  w_sd_info info ON info.sd_no = a.sd_no 
					                        WHERE a.location_code LIKE '002%' AND a.mt_type !='CMT' AND a.mt_type != '' AND a.mt_type IS NOT NULL 
                                            AND a.status = '001' AND (a.ExportCode IS NULL OR a.ExportCode = '')
										--	and a.mt_no ='CDYT4BASD135-145'
						                        AND (@Mt_No = '' OR @Mt_No IS NULL OR  a.mt_no Like '%' + @Mt_No + '%' )
                                                AND (@Mt_Cd = '' OR @Mt_Cd IS NULL OR  a.material_code Like '%' + @Mt_Cd + '%' )
						                --        AND (@Mt_Nm = '' OR @Mt_Nm IS NULL OR b.mt_nm Like '%' + @Mt_Nm + '%' )  
                                                AND (@ProductCode = '' OR @ProductCode IS NULL OR info.product_cd Like '%' + @ProductCode + '%' ) 
						                        AND (@LocationCode = '' OR @LocationCode IS NULL OR a.location_code Like '%' + @LocationCode + '%' )
						                        AND (@Start = '' OR @Start IS NULL OR a.recei_date >= @Start)  
						                        AND (@End = '' OR @End IS NULL OR a.recei_date <= @End) 

					                        GROUP BY a.mt_no
UNION 
SELECT a.materialid  as wmtid, b.mt_no, b.mt_nm,a.sd_no, info.product_cd, a.material_code as mt_cd,'' DSD,
                            CONCAT(ISNULL(a.gr_qty,0),ISNULL(b.unit_cd,0)) CSD,
                            CONCAT(ISNULL(b.width,0),'*',ISNULL(a.gr_qty,0)) AS size,
                            ISNULL(b.spec,0) spec, 
                             CONCAT(   (case when b.bundle_unit ='Roll' then  ROUND((a.gr_qty/b.spec),2) ELSE ROUND(a.gr_qty,2) END),' Roll') qty, 
                                a.recei_wip_date As receipt_date, 
                                (SELECT dt_nm FROM comm_dt WHERE comm_dt.dt_cd=a.status AND comm_dt.mt_cd='WHS005') sts_nm,
                                (SELECT lct_nm FROM lct_info WHERE lct_cd = a.location_code) AS lct_nm
                                FROM inventory_products a 
                                LEFT JOIN d_material_info  b ON a.mt_no=b.mt_no 
                                LEFT JOIN  w_sd_info info ON info.sd_no = a.sd_no  
                                WHERE a.location_code LIKE '002%' AND a.mt_type<> 'CMT'
							--	and a.mt_no ='CDYT4BASD135-145'
                                AND a.status = '001' AND (a.ExportCode IS NULL OR a.ExportCode = '')
                                AND (@Mt_No = '' OR @Mt_No IS NULL OR  a.mt_no Like '%' + @Mt_No + '%' )
                            --    AND (@Mt_Nm = '' OR @Mt_Nm IS NULL OR b.mt_nm Like '%' + @Mt_Nm + '%' ) 
                                AND (@Mt_Cd = '' OR @Mt_Cd IS NULL OR a.material_code Like '%' +  @Mt_Cd + '%' )
                                AND (@ProductCode = '' OR @ProductCode IS NULL OR info.product_cd Like '%' + @ProductCode + '%' ) 
                                AND (@LocationCode = '' OR @LocationCode IS NULL OR a.location_code Like '%' + @LocationCode + '%' )
                                AND (@Start = '' OR @Start IS NULL OR a.recei_date >= @Start)  
                                AND (@End = '' OR @End IS NULL OR a.recei_date <= @End) 
							) 
select * from getdata order by mt_nm, sd_no  asc
                         ";

                var result = await base.DbConnection.QueryAsync<WIP_ParentInventoryExport>(query, new {@Mt_No =mt_no, @Mt_Cd = mt_cd,@ProductCode = s_product_cd, @LocationCode = s_locationNAme, @Start = recevice_dt_start, @End = recevice_dt_end});
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<WIP_ParentInventoryModelExport>> ExportExcelGeneralMaterialDetail(string mt_no,string mt_nm, string sts, string recevice_dt_start, string recevice_dt_end,string ProductCode)
        {
            try
            {
                var query = @"
					                    with getdata  as  (     Select 
					                   --     ROW_NUMBER() OVER (ORDER BY a.mt_no) AS wmtid, 
					                        MAX(a.mt_no) As mt_no,
					                        MAX(info.product_cd) As product_cd,''ExportCode,''material_code,
					                        MAX(b.mt_nm) As mt_nm,      
					                        CONCAT((Case 
							                    WHEN (max(b.bundle_unit) = 'Roll')  
							                    THEN concat(round((sum(a.gr_qty) / max(b.spec)),2), ' Roll') 
                                                ELSE concat(round(SUM(a.gr_qty),2) ,'EA')
                                                END),'') AS qty, 
					                        SUM( CASE  WHEN a.status='002' THEN a.gr_qty ELSE 0  END) AS 'DSD',
					                        SUM( CASE WHEN (a.status='001') THEN a.gr_qty ELSE 0  END)  AS 'CSD',
                                            SUM(CASE WHEN (a.status='004' )	THEN a.gr_qty  ELSE 0  END) AS 'returnMachine' ,''lenght,''size,''receipt_date,''sd_no,''sts_nm
					                        FROM inventory_products AS a 
					                        LEFT JOIN d_material_info AS b ON a.mt_no = b.mt_no 
                                            LEFT JOIN  w_sd_info info ON info.sd_no = a.sd_no 
					                        WHERE a.location_code LIKE '002%' AND a.mt_type ='PMT'   AND (a.status='001' or a.status='002' or a.status='004' ) and a.ExportCode IS NOT null and a.ExportCode != ''
											--and info.product_cd = 'LJ63-20860A'
										--	and a.mt_no ='C3M#9733S35-20'
						                        AND (@Mt_No = '' OR @Mt_No IS NULL OR  a.mt_no Like '%' + @Mt_No + '%' )
                                                AND (@Mt_Cd = '' OR @Mt_Cd IS NULL OR  a.material_code Like '%' + @Mt_Cd + '%' )
						                     --   AND (@Mt_Nm = '' OR @Mt_Nm IS NULL OR b.mt_nm Like '%' + @Mt_Nm + '%' )  
                                                AND (@ProductCode = '' OR @ProductCode IS NULL OR info.product_cd Like '%' + @ProductCode + '%' ) 
						                  
						                        AND (@Start = '' OR @Start IS NULL OR a.recei_date >= @Start)  
						                        AND (@End = '' OR @End IS NULL OR a.recei_date <= @End) 
					                         GROUP BY info.product_cd, a.mt_no
					                        
union
SELECT a.mt_no,info.product_cd,a.ExportCode,a.material_code, ''mt_nm,(case when b.bundle_unit ='Roll' then CONCAT (ROUND((a.gr_qty/b.spec),2),' ')  ELSE concat(ROUND(a.gr_qty,2),' ') END) qty,''DSD,ISNULL(b.spec,0) CSD,''returnMachine,
                            CONCAT(ISNULL(a.gr_qty,''),ISNULL(b.unit_cd,'')) lenght,
                            CONCAT(ISNULL(b.width,0),'*',ISNULL(a.gr_qty,0)) AS size,
                          
                                CONCAT(a.recei_wip_date,'') As receipt_date, 
                                a.sd_no,
                                (SELECT dt_nm FROM comm_dt WHERE comm_dt.dt_cd=a.status AND comm_dt.mt_cd='WHS005') sts_nm
                                FROM inventory_products a 
                                LEFT JOIN d_material_info  b ON a.mt_no=b.mt_no 
                                LEFT JOIN  w_sd_info info ON info.sd_no = a.sd_no  
                                WHERE a.location_code LIKE '002%' AND a.mt_type = 'PMT' AND b.active = 1 AND (a.status='001' or a.status='002' or a.status='004' ) and a.ExportCode IS NOT null and a.ExportCode != ''
								--and a.mt_no ='C3M#9733S35-20'
                                AND (@Mt_No = '' OR @Mt_No IS NULL OR  a.mt_no Like '%' + @Mt_No + '%' )
                            --    AND (@Mt_Nm = '' OR @Mt_Nm IS NULL OR b.mt_nm Like '%' + @Mt_Nm + '%' ) 
                                AND (@Mt_Cd = '' OR @Mt_Cd IS NULL OR a.material_code Like '%' +  @Mt_Cd + '%' )
                                AND (@ProductCode = '' OR @ProductCode IS NULL OR info.product_cd Like '%' + @ProductCode + '%' ) 
                               
                                AND (@Start = '' OR @Start IS NULL OR a.recei_date >= @Start)  
                                AND (@End = '' OR @End IS NULL OR a.recei_date <= @End) 
                             --   Order By a.ExportCode Asc
								)
select * from getdata order by product_cd,mt_no,mt_nm desc
";

                var result = await base.DbConnection.QueryAsync<WIP_ParentInventoryModelExport>(query, new { @Mt_No = mt_no, @Mt_Cd = mt_nm, @Start = recevice_dt_start, @End = recevice_dt_end, @ProductCode = ProductCode });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<InventoryProduct>> GetInventoryProductByOrginMaterialCode(string orgin_mt_cd)
        {
            try
            {
                var query = @"Select * From inventory_products where material_code = @Mt_Cd";
                var result = await base.DbConnection.QueryAsync<InventoryProduct>(query, new { @Mt_Cd = orgin_mt_cd});
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<w_material_model>> UpdateLengthReturn(string id)
        {
            try
            {
                var query = @"SELECT a.materialid,a.material_code,b.mt_nm, 
                            CONCAT(ISNULL(a.gr_qty,''),ISNULL(b.unit_cd,'')) lenght, 
                            ISNULL(a.gr_qty,'') lenght1, 
                            CONCAT(ISNULL(b.width,0),'*',ISNULL(a.gr_qty,0)) AS size, 
                            ISNULL(b.spec,0) spec,a.mt_no,  CONCAT((case when b.bundle_unit ='Roll' then  (a.gr_qty/b.spec) ELSE a.gr_qty END),' ',b.bundle_unit) qty, 
                            b.bundle_unit,
                            (SELECT dt_nm FROM comm_dt WHERE comm_dt.dt_cd=a.status AND comm_dt.mt_cd='WHS005') sts_nm , 
                             (SELECT Top 1 w_actual_primary.product FROM w_actual JOIN w_actual_primary ON w_actual.at_no=w_actual_primary.at_no  WHERE a.id_actual=w_actual.id_actual) AS product, 
                            (SELECT Top 1 name FROM w_actual WHERE a.id_actual=w_actual.id_actual) AS name 
                             FROM inventory_products a LEFT JOIN d_material_info  b ON a.mt_no=b.mt_no 
                            WHERE a.status='004' and a.location_code LIKE '002%' AND a.materialid = @Id ";
                var result = await base.DbConnection.QueryAsync<w_material_model>(query, new { @Id = id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        #endregion

    }
}