using Mvc_VD.Models.NewVersion;
using Mvc_VD.Respositories.Irepository;
using Mvc_VD.Services.Interface.QMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Mvc_VD.Models.Response;
using static Mvc_VD.Controllers.DashBoardQCController;
using static Mvc_VD.Controllers.QCInformationController;
using Mvc_VD.Models;

namespace Mvc_VD.Services.Implement.QMS
{
    public class QMSService : DbConnection1RepositoryBase, IQMSService
    {
        public QMSService(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {
        }

        #region DashBoard
        public async Task<IEnumerable<CommCode>> GetListItemType(string mt_cd)
        {
            try
            {
                var query = @"Select * from comm_dt where mt_cd = @Mt_Cd And use_yn = 'Y'";
                var result = await base.DbConnection.QueryAsync<CommCode>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<Get_table_info_PQC_D_Model>> GetListFaclineQC(string start, string end, string ml_no)
        {
            try
            {
                var query = @"SELECT a.fq_no, a.work_dt AS work_dt, 
                            SUM(a.check_qty) AS qc_qty, SUM(a.ok_qty) AS ok_qty, 
                            (SUM(a.check_qty) - SUM(a.ok_qty) )AS def_qty,  a.ml_no 
                            FROM m_facline_qc AS a 
                            WHERE (@Start = '' OR @Start IS NULL OR a.work_dt >= @Start)
                            AND (@End = '' OR @End IS NULL OR a.work_dt <= @End)
                            AND (@Ml_No = '' OR Ml_No IS NULL OR a.ml_no LIKE '%' + @Ml_No + '%')  
                            GROUP BY a.fq_no , a.work_dt, a.ok_qty, a.ml_no";
                var result = await base.DbConnection.QueryAsync<Get_table_info_PQC_D_Model>(query, new { @Start = start, @End = end, @Ml_No = ml_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<myChart_error_Item_Model_PQC>> GetListFaclineQCModel(string start, string end, string ml_no)
        {
            try
            {
                var query = @"SELECT a.fq_no, a.ml_no,e.check_subject, b.check_value, b.check_id, a.work_dt,SUM(b.check_qty) as qc_qty 
                           FROM m_facline_qc AS a 
               		            left JOIN m_facline_qc_value AS b ON a.fq_no=b.fq_no AND a.item_vcd=b.item_vcd 
               		            left join qc_item_mt as c on b.item_vcd=c.item_vcd 
               		            left join qc_itemcheck_dt as d on c.item_vcd=d.item_vcd AND b.check_cd=d.check_cd AND b.check_id=d.check_id	
               		            left join qc_itemcheck_mt as e on d.item_vcd=e.item_vcd and d.check_id=e.check_id 
                           WHERE (@Start = '' OR @Start IS NULL OR a.work_dt >=  @Start) 
               	            AND (@End = '' OR @End IS NULL OR a.work_dt <=  @End) 
                            AND (@Ml_No = '' OR @Ml_No IS NULL OR  a.ml_no LIKE '%'+ @Ml_No + '%' ) 
                           Group by a.fq_no,b.check_id, b.check_cd, b.check_value, a.ml_no, e.check_subject, a.work_dt";
                var result = await base.DbConnection.QueryAsync<myChart_error_Item_Model_PQC>(query, new { @Start = start, @End = end, @Ml_No = ml_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<QCItemMaterial>> GetListItemMaterial()
        {
            try
            {
                var query = @"Select * from qc_item_mt Where del_yn = 'N' Order by item_vcd";
                var result = await base.DbConnection.QueryAsync<QCItemMaterial>(query);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<Get_table_info_OQC_D_Model>> GetListProductQC(string start, string end, string ml_no)
        {
            try
            {
                var query = @"SELECT a.item_vcd, a.pq_no, a.pqno,a.ok_qty,a.ml_no, a.work_dt As work_dt, a.check_qty AS qc_qty, (a.check_qty - a.ok_qty) AS defect_qty,
                                ROUND(((a.check_qty - a.ok_qty)/a.check_qty) * 100, 2) As defect_qty_qc_rate, 
                                ROUND(((a.ok_qty/a.check_qty) * 100), 2) As ok_qty_qc_rate 
                            FROM w_product_qc AS a 
                            WHERE (@ML_no = '' OR @ML_no IS NULL OR a.ml_no LIKE '%' + @ML_no + '%' ) 
                                  AND (@Start = '' OR @Start IS NULL OR a.work_dt >=  @Start)
                                  AND (@End = ''  OR @End IS NULL OR a.work_dt <= @End )";
                var result = await base.DbConnection.QueryAsync<Get_table_info_OQC_D_Model>(query, new { @Start = start, @End = end, @Ml_No = ml_no });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<myChart_error_Item_Model_OQC>> GetListProductQCModel(string start, string end, string ml_no)
        {
            try
            {
                var query = @"SELECT b.pqhno,b.pq_no,b.check_value,SUM(b.check_qty) AS qc_qty,a.item_vcd,a.ml_no,work_dt  
            FROM w_product_qc a 
            JOIN w_product_qc_value b  ON a.pq_no=b.pq_no AND a.item_vcd=b.item_vcd 
             WHERE (@ML_no = '' OR @ML_no IS NULL OR a.ml_no LIKE '%' + @ML_no + '%' ) 
                                  AND (@Start = '' OR @Start IS NULL OR a.work_dt >=  @Start)
                                  AND (@End = ''  OR @End IS NULL OR a.work_dt <= @End )
            Group by b.check_id,b.check_cd,b.check_value, b.pqhno, b.pq_no,a.item_vcd,a.ml_no, a.work_dt";
                var result = await base.DbConnection.QueryAsync<myChart_error_Item_Model_OQC>(query, new { @Start = start, @End = end, @Ml_No = ml_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<CommCode>> GetListDefect(string mt_cd)
        {
            try
            {
                var query = @"Select * from comm_dt where mt_cd = @Mt_Cd And use_yn = 'Y' And dt_cd = 'Y'";
                var result = await base.DbConnection.QueryAsync<CommCode>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region Quanlity Control Management
        public async Task<IEnumerable<QCItemMaterial>> SearchQCItemMaterial(string item_type, string item_cd, string item_nm, string item_exp)
        {
            try
            {
                var query = @"Select * from qc_item_mt Where del_yn = 'N'
                            And (@ItemName = '' OR @ItemName IS NULL OR item_nm Like '%' + @ItemName + '%')
							And (@ItemExp = '' OR @ItemExp IS NULL OR item_exp Like '%' + @ItemExp + '%')
							And (@ItemVcd = '' OR @ItemVcd IS NULL OR item_vcd Like '%' + @ItemVcd + '%')
							And (@ItemType = '' OR @ItemType IS NULL OR item_type Like '%' + @ItemType + '%')";
                var result = await base.DbConnection.QueryAsync<QCItemMaterial>(query, new { @ItemName = item_nm, @ItemExp = item_exp, @ItemVcd = item_cd, @ItemType = item_type });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<QCItemMaterial>> SearchQCItemMaterial(string item_cd, string item_nm, string item_exp)
        {
            try
            {
                var query = @"Select * from qc_item_mt Where (@ItemName = '' OR @ItemName IS NULL OR item_nm Like '%' + @ItemName + '%')
							And (@ItemExp = '' OR @ItemExp IS NULL OR item_exp Like '%' + @ItemExp + '%')
							And (@ItemVcd = '' OR @ItemVcd IS NULL OR item_vcd Like '%' + @ItemVcd + '%')";

                var result = await base.DbConnection.QueryAsync<QCItemMaterial>(query, new { @ItemName = item_nm, @ItemExp = item_exp, @ItemVcd = item_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<QCItemCheckMaterialResponse>> GetQCCheckMaterial(string item_vcd)
        {
            try
            {
                var query = @"Select icno, item_vcd, check_id, check_type, check_subject, min_value, max_value,range_type, order_no, re_mark, use_yn, reg_id, reg_dt, chg_id, chg_dt, active,
                             Substring(item_vcd,0,7) as item_cd,
                             Substring(item_vcd,8,1) as ver,
                             (Select dt_nm from comm_dt where mt_cd = 'COM024' And dt_cd = range_type) As range_type_nm
                            from qc_itemcheck_mt 
                            where item_vcd = @ItemVcd And del_yn = 'N'";

                var result = await base.DbConnection.QueryAsync<QCItemCheckMaterialResponse>(query, new { @ItemVcd = item_vcd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<QCItemCheckMaterial>> CheckQCItemCheckMaterial(string item_vcd, string check_id)
        {
            try
            {
                var query = @"Select * from qc_itemcheck_mt Where item_vcd = @ItemVcd And check_id = @Check_Id";
                var result = await base.DbConnection.QueryAsync<QCItemCheckMaterial>(query, new { @ItemVcd = item_vcd, @Check_Id = check_id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> InsertIntoQCItemCheckMaterial(QCItemCheckMaterial item)
        {
            try
            {
                var query = @"Insert Into qc_itemcheck_mt(item_vcd, check_id, check_type, check_subject, min_value, max_value, range_type, order_no, re_mark, use_yn, del_yn, reg_id, reg_dt, chg_id, chg_dt)
                            Values (@item_vcd, @check_id, @check_type, @check_subject, @min_value, @max_value, @range_type, @order_no, @re_mark, @use_yn, @del_yn, @reg_id, @reg_dt, @chg_id, @chg_dt)
                            Select Scope_Identity()";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<QCItemCheckDetail>> CheckQCItemCheckDetail(string check_id)
        {
            try
            {
                var query = @"Select * from qc_itemcheck_dt where check_id = @Id";
                var result = await base.DbConnection.QueryAsync<QCItemCheckDetail>(query, new { @Id = check_id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<QCItemCheckDetail>> CheckQCItemCheckDetail(string item_vcd, string check_id)
        {
            try
            {
                var query = @"Select * from qc_itemcheck_dt where item_vcd = @ItemVcd And check_id = @Id And del_yn = 'N'";
                var result = await base.DbConnection.QueryAsync<QCItemCheckDetail>(query, new { @ItemVcd = item_vcd, @Id = check_id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> InsertIntoQCItemCheckDetail(QCItemCheckDetail item)
        {
            try
            {
                var query = @"Insert Into qc_itemcheck_dt (item_vcd, check_id, check_cd, defect_yn, check_name, order_no, re_mark, use_yn, del_yn, reg_id, reg_dt, chg_id, chg_dt)
                            Values (@item_vcd, @check_id, @check_cd, @defect_yn, @check_name, @order_no, @re_mark, @use_yn, @del_yn, @reg_id, @reg_dt, @chg_id, @chg_dt)
                            Select Scope_Identity()";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<QCItemCheckMaterialResponse>> GetQCCheckMaterialByIcNo(int icno)
        {
            try
            {
                var query = @"Select icno, item_vcd, check_id, check_type, check_subject, min_value, max_value,range_type, order_no, re_mark, use_yn, reg_id, reg_dt, chg_id, chg_dt, active,
                             Substring(item_vcd,0,7) as item_cd,
                             Substring(item_vcd,8,1) as ver,
                             (Select dt_nm from comm_dt where mt_cd = 'COM024' And dt_cd = range_type) As range_type_nm
                            from qc_itemcheck_mt 
                            where icno = @Id";
                var result = await base.DbConnection.QueryAsync<QCItemCheckMaterialResponse>(query, new { @Id = icno });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<QCItemCheckMaterial> GetQCItemCheckMaterialById(int icno)
        {
            try
            {
                var query = @"Select * from qc_itemcheck_mt where icno = @Id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<QCItemCheckMaterial>(query, new { @Id = icno });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateQCItemCheckDetailForDel_Yn(int icdno)
        {
            try
            {
                var query = @"Update qc_itemcheck_dt SET del_yn = 'Y' where icdno = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Id = icdno });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateQCItemCheckMaterialForDel_Yn(int icno)
        {
            try
            {
                var query = @"Update qc_itemcheck_mt SET del_yn = 'Y' where icno = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Id = icno });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<QCItemCheckDetail> GetQCItemCheckDetailById(int idcno)
        {
            try
            {
                var query = @"Select * from qc_itemcheck_dt where icdno = @Id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<QCItemCheckDetail>(query, new { @Id = idcno });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UpdateQCItemCheckMaterial(QCItemCheckMaterial item)
        {
            try
            {
                var query = @"Update qc_itemcheck_mt SET item_vcd = @item_vcd, check_id = @check_id, check_type = @check_type, check_subject = @check_subject, min_value = @min_value, max_value = @max_value,
                            range_type = @range_type, order_no =  @order_no, re_mark = @re_mark, use_yn = @use_yn, reg_id = @reg_id, reg_dt = @reg_dt, chg_id = @chg_id, chg_dt = @chg_dt
                            Where icno = @icno";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UpdateQCItemCheckDetail(QCItemCheckDetail item)
        {
            try
            {
                var query = @"Update qc_itemcheck_dt Set item_vcd = @item_vcd, check_id = @check_id, check_cd = @check_cd, defect_yn = @defect_yn, check_name = @check_name, order_no = @order_no,
                            re_mark = @re_mark, use_yn = @use_yn, del_yn = @del_yn, reg_id = @reg_id, reg_dt = @reg_dt, chg_id = @chg_id, chg_dt = @chg_dt
                            Where icdno = @icdno";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region QC Infomation
        public async Task<IEnumerable<FaclineQC>> GetListFaclineQC(string fq_no, string ml_no, string start, string end)
        {
            try
            {
                var query = @"SELECT a.item_vcd, a.fqno, a.fq_no, a.work_dt, a.check_qty, a.ok_qty, a.ml_no, 
		                            (a.check_qty - a.ok_qty) AS defect_qty, 
		                            ISNULL(a.ok_qty, Round((a.check_qty - a.ok_qty)/a.check_qty * 100, 2)) AS defect_qty_qc_rate, 
		                            ISNULL(a.ok_qty, Round(a.ok_qty/a.check_qty * 100, 2)) AS ok_qty_qc_rate 
                            FROM   m_facline_qc AS a 
                            WHERE  (@fq_no = '' OR  @fq_no IS NULL OR a.fq_no LIKE '%' + @fq_no + '%') 
                                    AND (@ml_no = '' OR @ml_no IS NULL OR a.ml_no LIKE '%' + @ml_no + '%' ) 
                                    AND (@start = '' OR @start IS NULL OR a.work_dt >= @start)
                                    AND (@end = '' OR @end IS NULL OR a.work_dt <= @end ) 
                            Order by a.fqno";
                var result = await base.DbConnection.QueryAsync<FaclineQC>(query, new { @fq_no = fq_no, @ml_no = ml_no, @start = start, @end = end });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<FaclineQCValue>> GetListFaclineQCValue(string fq_no, string item_vcd)
        {
            try
            {
                var query = @"SELECT a.fq_no, a.Id, a.check_qty, d.check_subject AS check_subject, a.check_value AS check_value
                            FROM m_facline_qc_value AS a
                            JOIN m_facline_qc AS b ON a.item_vcd=b.item_vcd AND a.fq_no = b.fq_no
                            JOIN qc_itemcheck_mt AS d ON a.item_vcd=d.item_vcd AND a.check_id=d.check_id
                            WHERE a.fq_no = @fq_no AND b.item_vcd = @item_vcd ";
                var result = await base.DbConnection.QueryAsync<FaclineQCValue>(query, new { @fq_no = fq_no, @item_vcd = item_vcd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<ProductQC>> GetListProductQC(string pq_no, string ml_no, string start, string end)
        {
            try
            {
                var query = @"SELECT a.item_vcd, a.pqno, a.pq_no, a.work_dt, a.check_qty, a.ok_qty, a.ml_no, 
		                            (a.check_qty - a.ok_qty) AS defect_qty, 
		                            ISNULL(a.ok_qty, Round((a.check_qty - a.ok_qty)/a.check_qty * 100, 2)) AS defect_qty_qc_rate, 
		                            ISNULL(a.ok_qty, Round(a.ok_qty/a.check_qty * 100, 2)) AS ok_qty_qc_rate 
                            FROM   w_product_qc AS a 
                            WHERE  (@pq_no = '' OR  @pq_no IS NULL OR a.pq_no LIKE '%' + @pq_no + '%') 
                                    AND (@ml_no = '' OR @ml_no IS NULL OR a.ml_no LIKE '%' + @ml_no + '%' ) 
                                    AND (@start = '' OR @start IS NULL OR a.work_dt >= @start)
                                    AND (@end = '' OR @end IS NULL OR a.work_dt <= @end ) 
                            Order by a.pqno";
                var result = await base.DbConnection.QueryAsync<ProductQC>(query, new { @pq_no = pq_no, @ml_no = ml_no, @start = start, @end = end });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        //public async Task<IEnumerable<GetQMSNGModel>> GetListDataQMSNG(string productCode, DateTime? date_ymd)
        //{
        //    try
        //    {
        //        var query = @"SELECT ROW_NUMBER() OVER (ORDER BY a.fq_no) AS stt , a.ml_tims, 
        //                     MAX(a.product_cd) As ProductCode,
        //                     SUM(a.check_qty) As Total ,
        //                     SUM(a.ok_qty) As OK ,
        //                     (SUM(a.check_qty)- SUM(a.ok_qty)) AS NG, 
        //                     a.shift As Shift, a.reg_dt As CreateOn
        //                    FROM m_facline_qc AS a
        //                    WHERE (@productCode = '' OR @productCode IS NULL OR a.product_cd = @productCode) 
        //                         AND(@Start_date_ymd = '' OR @Start_date_ymd IS NULL OR a.reg_dt >= @Start_date_ymd )
        //                         AND(@End_date_ymd = '' OR @End_date_ymd IS NULL OR a.reg_dt <= @End_date_ymd )
        //                    GROUP BY a.reg_dt, a.ml_tims, a.fq_no, a.shift";
        //        var result = await base.DbConnection.QueryAsync<GetQMSNGModel>(query, new { @productCode = productCode, @Start_date_ymd = date_ymd, @End_date_ymd = date_ymd });
        //        return result;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        public async Task<IEnumerable<ProductActivitionFailedDetailVm>> GetProductActivitionFailed(string productCode, string fromDate, string toDate)
        {
            try
            {
                var query = @"SELECT p1.check_cd as CheckCode, p1.check_name as CheckName,p2.CheckDate,
							   (case when(p2.shift = 'CD' and p2.check_qty is not null) then check_qty else 0 end) as CDQty,
							   (case when(p2.shift = 'CN' and p2.check_qty is not null) then check_qty else 0 end) as CNQty 
							   from (select distinct check_cd,check_name,del_yn 
							   from qc_itemcheck_dt) p1  
							   left outer join(SELECT check_cd, SUM(check_qty) AS check_qty,
							   date_ymd as CheckDate,
							   max(check_value) check_value,shift 
                       FROM m_facline_qc_value WHERE product = @ProductCode
                       AND((date_ymd >= @fromDate) AND (date_ymd <= @toDate))
                       GROUP BY check_cd,shift,(date_ymd)) As p2
                       ON p1.check_cd = p2.check_cd and p1.check_name =p2.check_value
                       where p1.del_yn = 'N'";
                var result = await base.DbConnection.QueryAsync<ProductActivitionFailedDetailVm>(query, new { @ProductCode = productCode, @fromDate = fromDate, @toDate = toDate });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<ProductActivitionFailedVm>> GetProductActivitionFaileds(string productCode, string fromDate, string toDate)
        {
            try
            {
                var query = @"
                            SELECT CTE_1.ProductCode,FORMAT( CAST( CTE_1.CreateOn AS datetime ),'yyyy-MM-dd') as CreateOn,CTE_1.Shifts as Shift,CTE_1.OK,CTE_2.NG,
                            (CASE WHEN SUM(CTE_1.OK)+SUM(CTE_2.NG) IS NULL THEN 0 ELSE SUM(CTE_1.OK)+SUM(CTE_2.NG) END) As Total
                            From 
                            (SELECT TABLE1.*,
                                    (CASE WHEN TABLE1.ok_qty IS NULL THEN 0 ELSE ISNULL(SUM(TABLE1.ok_qty),0)END) OK,
                                    (CASE WHEN TABLE1.check_qty IS NULL THEN 0 ELSE ISNULL(SUM(TABLE1.check_qty) -SUM(TABLE1.ok_qty),0) END) AS NG
                                    FROM(
				                            SELECT (a.product_cd)ProductCode,a.check_qty, a.ok_qty,
                                                (
                                                    CASE
                                                    WHEN('08:00:00' <= (FORMAT( CAST( a.reg_dt AS datetime ),'HH:mm:ss')) AND (FORMAT( CAST( a.reg_dt AS datetime ),'HH:mm:ss')) < '23:59:00') THEN (a.reg_dt)
                                                    WHEN (FORMAT( CAST( a.reg_dt AS datetime ),'HH:mm:ss')) < '08:00:00' THEN ISNULL((a.reg_dt) - DateAdd(DAY,-1,GetDate()),NULL)
                                                    ELSE '' END) as CreateOn,
                                    
					                            ISNULL(a.shift,'') As Shifts
					                            FROM m_facline_qc AS a
					                            JOIN w_material_info_tims AS b on a.ml_tims = b.material_code
					                            INNER JOIN w_actual AS w ON b.id_actual = w.id_actual AND w.IsFinish = '1'
					                            INNER JOIN d_style_info AS style ON a.product_cd = style.style_no
					                            WHERE a.product_cd = @ProductCode) AS TABLE1
				                            WHERE((TABLE1.CreateOn) >=  @fromDate AND (TABLE1.CreateOn) <= @toDate)
				                            GROUP BY TABLE1.CreateOn, TABLE1.Shifts , TABLE1.ProductCode, TABLE1.check_qty, TABLE1.ok_qty) AS CTE_1   
	                            LEFT JOIN 
	                            (
		                            SELECT TABLE1.*,

                                            (CASE WHEN TABLE1.check_qty IS NULL THEN 0 ELSE SUM(TABLE1.check_qty) -SUM(TABLE1.ok_qty) END) AS NG
				                            FROM(
						                            SELECT (a.product_cd)ProductCode,a.check_qty, a.ok_qty,
						                            (CASE WHEN('08:00:00' <= (FORMAT( CAST( a.reg_dt AS datetime ),'HH:mm:ss')) AND (FORMAT( CAST( a.reg_dt AS datetime ),'HH:mm:ss')) < '23:59:00') THEN (a.reg_dt) 
								                            WHEN (FORMAT( CAST( a.reg_dt AS datetime ),'HH:mm:ss')) < '08:00:00' THEN ISNULL((a.reg_dt) - DateAdd(DAY,-1,GetDate()),NULL)
								                            ELSE '' END) as CreateOn,
						                            a.shift As Shifts
						                            FROM m_facline_qc AS a
						                            INNER JOIN d_style_info AS style ON a.product_cd = style.style_no
						                            WHERE a.product_cd = @ProductCode) AS TABLE1
                                    WHERE(TABLE1.CreateOn >= @fromDate AND TABLE1.CreateOn <=  @toDate)
                                    GROUP BY TABLE1.CreateOn, TABLE1.Shifts, TABLE1.ProductCode, TABLE1.check_qty, TABLE1.ok_qty ) As CTE_2 
                            ON CTE_1.ProductCode = CTE_2.ProductCode AND CTE_1.CreateOn= CTE_2.CreateOn AND CTE_1.Shifts = CTE_2.Shifts 
                            GROUP BY CTE_1.CreateOn, CTE_1.Shifts, CTE_1.ProductCode, CTE_1.OK, CTE_2.NG";
                var result = await base.DbConnection.QueryAsync<ProductActivitionFailedVm>(query, new { @ProductCode = productCode, @fromDate = fromDate, @toDate = toDate });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<qc_itemcheck_mt>> Getitemcheck_mt(string item_vcd)
        {
            try
            {
                var query = @"SELECT *
                            FROM qc_itemcheck_mt AS a 
                            WHERE  item_vcd = @item_vcd and  del_yn = 'N'";
                var result = await base.DbConnection.QueryAsync<qc_itemcheck_mt>(query, new { item_vcd = item_vcd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<IEnumerable<Models.NewVersion.GetQMSNGModel>> GetListClassificationNG(string start_date_ymd, string end_date_ymd, string productCode, string at_no)
        {
            try
            {
                var query = @"
                            With CTE_1 as (
                                 SELECT max(TABLE1.at_no)at_no, max(TABLE1.ItemName)ItemName,max(TABLE1.ProductCode)ProductCode,max(TABLE1.ModelName)ModelName,TABLE1.CreateOn, TABLE1.shift ,
                            (CASE WHEN SUM(TABLE1.ok_qty) IS NULL THEN 0 ELSE SUM(TABLE1.ok_qty) END) OK,
                            (CASE WHEN SUM(TABLE1.ng_qty) IS NULL THEN 0 ELSE SUM(TABLE1.ng_qty) END) AS NG
                            FROM( SELECT(style.style_nm) AS ItemName, (style.md_cd)AS ModelName, (a.product_cd)ProductCode,
                                a.check_qty, a.ok_qty,a.ng_qty, a.at_no,
                                   (
                                        CASE 
                                        WHEN ('08:00:00' <= FORMAT( CAST( a.reg_dt AS datetime ),'HH:mm:ss') AND  FORMAT( CAST( a.reg_dt AS datetime ),'HH:mm:ss')  <  '23:59:00') THEN
                                        FORMAT( CAST( a.reg_dt AS DATETIME ),'yyyy-MM-dd')

                                        when (FORMAT( CAST( a.reg_dt AS datetime ),'HH:mm:ss')  < '08:00:00') THEN FORMAT(DATEADD(day,-1,a.reg_dt),'yyyy-MM-dd')
                                          ELSE ''
                                        END )  as CreateOn,
                                    a.shift Shift


                            FROM m_facline_qc AS a
                            JOIN w_material_info_tims AS b on a.ml_tims = b.material_code
                            INNER JOIN w_actual AS w ON b.product = w.product AND b.id_actual = w.id_actual AND w.IsFinish = '1'
                            INNER JOIN d_style_info AS style ON a.product_cd = style.style_no
                            WHERE a.product_cd = @productCode and   (@at_no='' OR a.at_no like @at_no)) AS TABLE1

                                  
                            GROUP BY TABLE1.CreateOn, TABLE1.shift 
                                    ) 
                                    , CTE_2  as ( 
                                    SELECT max(TABLE1.ProductCode) ProductCode,TABLE1.CreateOn, TABLE1.shift ,

                                    (CASE WHEN SUM(TABLE1.ng_qty) IS NULL THEN 0 ELSE SUM(TABLE1.ng_qty) END) AS NG
                                    FROM(
                                    SELECT (a.product_cd)ProductCode,

                                       a.check_qty, a.ok_qty,a.ng_qty,
                                           (
                                        CASE 
                                        WHEN ('08:00:00' <= FORMAT( CAST( a.reg_dt AS datetime ),'HH:mm:ss') AND  FORMAT( CAST( a.reg_dt AS datetime ),'HH:mm:ss')  <  '23:59:00') THEN
                                        FORMAT( CAST( a.reg_dt AS DATETIME ),'yyyy-MM-dd')

                                        when (FORMAT( CAST( a.reg_dt AS datetime ),'HH:mm:ss')  < '08:00:00') THEN FORMAT(DATEADD(day,-1,a.reg_dt),'yyyy-MM-dd')
                                          ELSE ''
                                        END )  as CreateOn,
                                          a.shift Shift
                                    FROM m_facline_qc AS a
                                    INNER JOIN d_style_info AS style ON a.product_cd = style.style_no
                                    WHERE a.product_cd = @productCode  and  (a.ml_no IS NOT  NULL OR a.ml_no != '')and 
									 (@at_no='' OR a.at_no like @at_no)) AS TABLE1

                                   
                                    GROUP BY TABLE1.CreateOn, TABLE1.shift 
                                    )

                                    SELECT max(CTE_1.ItemName) ItemName, max(CTE_1.ModelName) ModelName, ROW_NUMBER() OVER (ORDER BY CTE_1.CreateOn) AS stt ,
                                    
                                     (CASE WHEN (max(CTE_2.NG)- SUM(qcvalue.check_qty)) IS NULL THEN max(CTE_2.NG) ELSE (max(CTE_2.NG)- SUM(qcvalue.check_qty)) END) 
												 AS chuaphanloai,
                                max(CTE_1.ProductCode)ProductCode,max(CTE_1.CreateOn)CreateOn,max(CTE_1.shift)shift,max(CTE_1.at_no)at_no,
            (CASE WHEN max(CTE_1.OK)+max(CTE_2.NG) IS NULL THEN 0 ELSE SUM(CTE_1.OK)+SUM(CTE_2.NG) END) Total,max(CTE_1.OK)OK,max(CTE_2.NG)NG
                                    From CTE_1
                                    LEFT JOIN CTE_2 ON CTE_1.ProductCode = CTE_2.ProductCode 
												AND CTE_1.CreateOn= CTE_2.CreateOn AND CTE_1.shift= CTE_2.shift 
											LEFT JOIN m_facline_qc_value qcvalue ON  CTE_1.ProductCode = qcvalue.product AND  CTE_1.shift =  qcvalue.shift
												AND CTE_1.CreateOn = qcvalue.date_ymd
                            GROUP BY CTE_1.CreateOn, CTE_1.shift ;
		
";
                var result = await base.DbConnection.QueryAsync<Models.NewVersion.GetQMSNGModel>(query, new { productCode = productCode, start_date_ymd = start_date_ymd, end_date_ymd = end_date_ymd, at_no = at_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
     
        public async Task<IEnumerable<qc_itemcheck_dt>> GetitemcheckDetail(string item_vcd, string check_id)
        {
            try
            {
                var query = @"SELECT *
                            FROM qc_itemcheck_dt AS a 
                            WHERE  item_vcd = @item_vcd and check_id = @check_id and  del_yn = 'N' and defect_yn = 'Y'";
                var result = await base.DbConnection.QueryAsync<qc_itemcheck_dt>(query, new { item_vcd = item_vcd, check_id = check_id });
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<IEnumerable<MFaclineQCValue>> GetFaclineQCValueDetail(string ProductCode, string at_no, string date_ymd, string shift)
        {
            try
            {
                var query = @"SELECT  SUM(check_qty) AS check_qty, max(check_value) check_value, max(fqhno)fqhno, max(date_ymd) date_ymd
                              FROM m_facline_qc_value 
                            WHERE product = @ProductCode 
                            AND at_no = @at_no
                            AND date_ymd = @date_ymd
                            AND shift = @shift
					          GROUP BY check_cd";
                var result = await base.DbConnection.QueryAsync<MFaclineQCValue>(query,
                    new
                    {
                        ProductCode = ProductCode,
                        at_no = at_no,
                        date_ymd = date_ymd,
                        shift = shift
                    });
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<mFaclineQC> Get_Facline_Qc(string ml_tims)
        {
            try
            {
                var query = @"Select top 1 * from m_facline_qc where ml_tims = @ml_tims and fq_no like '%TI%' ";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<mFaclineQC>(query, new { ml_tims = ml_tims });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<string> GetmfaclineQC(string fq_no)
        {
            string sql = @"
           
            SELECT TOP 1  fq_no FROM m_facline_qc WHERE  fq_no LIKE @fq_no  ORDER BY fq_no desc ";

            var result = await base.DbConnection.ExecuteScalarAsync<string>(sql, new { fq_no = "%" + fq_no + "%" });
            return result;
        }
        public async Task<int> InsertIntoMFaclineQC(m_facline_qc item)
        {
            try
            {
                var query = @"Insert Into m_facline_qc(fq_no, ml_no, ml_tims, product_cd, shift, at_no, work_dt, item_vcd, item_nm, item_exp, check_qty, ok_qty, ng_qty, remain_qty,reg_id,reg_dt, chg_id,chg_dt)
                            Values (@fq_no, @ml_no, @ml_tims, @product_cd, @shift, @at_no, @work_dt, @item_vcd, @item_nm, @item_exp, @check_qty, @ok_qty, @ng_qty, @remain_qty, @reg_id,@reg_dt,@chg_id,@chg_dt)
                            Select Scope_Identity()";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<int> InsertIntoFaclineQCValue(m_facline_qc_value item)
        {
            try
            {
                var query = @"Insert Into m_facline_qc_value(fq_no, product,at_no, shift, item_vcd, check_id, check_cd, check_value, check_qty, date_ymd,reg_id,reg_dt, chg_id,chg_dt)
                            Values (@fq_no, @product,@at_no, @shift, @item_vcd, @check_id, @check_cd, @check_value, @check_qty, @date_ymd, @reg_id,@reg_dt,@chg_id,@chg_dt)
                            Select Scope_Identity()";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<int> SunNGMFaclineQC(string fq_no)
        {
            try
            {
                var query = @"SELECT sum(check_qty)
                                FROM m_facline_qc_value 
                                where fq_no = @fq_no";

                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { fq_no = fq_no });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<m_facline_qc> Top1MFaclineQC(string fq_no)
        {
            try
            {
                var query = @"Select top 1 * from m_facline_qc where fq_no = @fq_no  ";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<m_facline_qc>(query, new { fq_no = fq_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<int> UpdateMFaclineQC(m_facline_qc item)
        {
            try
            {
                var query = @"Update m_facline_qc SET check_qty = @check_qty where fq_no = @fq_no";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<IEnumerable<MFaclineQCValue>> GetFaclineQCValue(string ProductCode, string datetime, string shift)
        {
            try
            {
                var query = @"SELECT  SUM(check_qty) AS check_qty, max(check_value) check_value, max(fqhno)fqhno
                              FROM m_facline_qc_value 
                            WHERE product = @ProductCode 
                        
                            AND date_ymd = @datetime
                            AND shift = @shift
					          GROUP BY check_cd";
                var result = await base.DbConnection.QueryAsync<MFaclineQCValue>(query,
                    new
                    {
                        ProductCode = ProductCode,
                        datetime = datetime,
                        shift = shift
                    });
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<m_facline_qc_value> Top1MFaclineQCValueById(int id)
        {
            try
            {
                var query = @"Select top 1 * from m_facline_qc_value where fqhno = @id  ";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<m_facline_qc_value>(query, new { id = id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<int> DeleteMFaclineValue(int id)
        {
            string sqlquery = @"DELETE FROM m_facline_qc_value WHERE fqhno=@id";
            var result = await base.DbConnection.ExecuteAsync(sqlquery, new { id = id });
            return result;
        }
       
        
        #endregion
    }
}