using Dapper;
using Mvc_VD.Classes;
using Mvc_VD.Models;
//using Mvc_VD.Models.DMS;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models.Request;
using Mvc_VD.Models.Response;
using Mvc_VD.Models.TIMS;
using Mvc_VD.Models.WOModel;
using Mvc_VD.Respositories.Irepository;
using Mvc_VD.Services.Interface.MMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaterialInfoMMS = Mvc_VD.Models.NewVersion.MaterialInfoMMS;

namespace Mvc_VD.Services.Implement.MMS
{
    public class ActualWOService : DbConnection1RepositoryBase, IActualWOService
    {

        public ActualWOService(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {
        }

        #region Actual WO

        /// <summary>
        ///  Lấy danh sách các công đoạn ở bên Sản xuất (MMS)
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CommCode>> getListActualProccess()
        {
            try
            {
                var sql = "Select dt_cd, dt_nm from comm_dt where mt_cd = 'COM007' and use_yn = 'Y' and (dt_cd like 'ROT%' Or dt_cd like 'STA%')";
                var result = await base.DbConnection.QueryAsync<CommCode>(sql);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<StyleInfo>> getListProduct()
        {
            try
            {
                var sql = @"SELECT s.sid,s.ssver, s.reg_id,s.chg_id,s.reg_dt,s.chg_dt,s.stamp_code, m.stamp_name, c.dt_nm AS partname,
						s.style_no, s.style_nm, s.md_cd, s.prj_nm, s.pack_amt,s.expiry_month,s.expiry,part_nm ,drawingname  
						FROM d_style_info AS s 
						LEFT JOIN stamp_master AS m ON s.stamp_code = m.stamp_code
						LEFT JOIN comm_dt AS c ON s.part_nm =  c.dt_cd AND c.mt_cd ='DEV003'
						ORDER BY s.sid desc";
                var result = await base.DbConnection.QueryAsync<StyleInfo>(sql);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Lấy danh sách các NVL theo mã NVL và tên đồ đựng cho NVL đó
        /// </summary>
        /// <param name="bb_no"></param>
        /// <param name="mt_cd"></param>
        /// <returns></returns>
        public async Task<MaterialInfoMMS> GetMaterial(string bb_no, string mt_cd)
        {
            try
            {
                var query = @"Select * from w_material_info_mms where bb_no = @Bb_No and material_code = @Mt_Cd";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoMMS>(query, new { @Bb_No = bb_no, @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Hiển thị danh sách các PO và có thể tìm kiếm các PO theo điều kiện
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DatawActualPrimaryResponse>> getListPO(DataWActualPrimaryReq item)
        {
            try
            {
                //                var query = @"
                //                                                      select Distinct at_no,CountProcess
                //                                                      into #tmp
                //                                                      from 
                //                                                      (
                //                                                      select  wa.at_no,wa.id_actual,
                //                                                      (case
                //                                                      when ustaff.statusstaff=1 and mim.statusinfo=1 then  1
                //                                                      when ustaff.statusstaff=1 and mim.statusinfo=0 then  2
                //                                                      --when ustaff.statusstaff is null then 0
                //                                                      else 0
                //                                                      end ) CountProcess
                //                                                      from w_actual wa
                //                                                      left join (
                //                                                      select id_actual ,max(statusstaff) as statusstaff
                //                                                      from(
                //                                                      select id_actual,
                //                                                      case
                //                                                      when (GETDATE() between start_dt and end_dt) then 1
                //                                                      else 0
                //                                                      end statusstaff
                //                                                      from d_pro_unit_staff) tmpstaff
                //                                                          group by id_actual) ustaff on ustaff.id_actual=wa.id_actual
                //                                                      left join 
                //                                                      (select id_actual ,max(statusinfo) as statusinfo
                //                                                      from(
                //                                                      select id_actual,
                //                                                      case
                //                                                      when DATEDIFF(HOUR,reg_date,getdate()) <=1 then 1
                //                                                      else 0
                //                                                      end statusinfo
                //                                                      from w_material_info_mms) tmpinfomms
                //                                                          group by id_actual) mim on wa.id_actual=mim.id_actual
                //                                                      where (@At_No = '' OR @At_No IS NULL Or wa.at_no like '%'+@At_No+'%')
                //                                                          ) tmpp

                //                                                      Select 
                //                                                          max(a.id_actualpr) AS id_actualpr
                //                                                      ,max(a.id_actualpr) AS id_actualpr1
                //                                                , max(a.at_no) AS at_no
                //                                                , max(a.type) AS type
                //                                               , tttarget.totalTarget AS totalTarget
                //                                                , max(a.target) AS target
                //                                                , max(a.product) AS product
                //                                                , max(a.md_cd) AS md_cd
                //                                                , max(a.remark) AS remark
                //                                                , max(a.style_nm) AS style_nm
                //                                                , max(a.process_count) AS process_count
                //                                                , tttarget.ttactual AS actual
                //                                                , max (a.count_pr_w) AS count_pr_w
                //                                          ,  subquery.poRun AS poRun
                //                                                , max (a.IsApply) AS IsApply
                //                                                      , MAX(CONVERT(int,active)) as active
                //                                                      , MAX(a.CountProcess) As CountProcess
                //                                                      , MAX(a.bom_type) as bom_type ,tttarget.process_code  as process_code
                //                                From (
                //                                                 SELECT 
                //                                                  a.IsApply AS IsApply,
                //                                                  a.finish_yn AS finish_yn,
                //                                                  a.id_actualpr AS id_actualpr,
                //                                                  a.at_no AS at_no,
                //                                                  b.type AS type,
                //                                                  a.target AS target,
                //                                                  a.product AS product,
                //                                                  a.remark AS remark,
                //                                                  a.reg_id AS reg_id,
                //                                                  a.reg_dt AS reg_dt,
                //                                                  a.chg_id AS chg_id,
                //                                                  a.chg_dt AS chg_dt,
                //                                                  b.name AS processName,
                //                                                  (SELECT Top 1 d_style_info.style_nm FROM d_style_info WHERE (a.product = d_style_info.style_no)) AS style_nm,
                //                                                  (SELECT Top 1 d_style_info.md_cd FROM d_style_info WHERE (a.product = d_style_info.style_no)) AS md_cd,
                //                                                  (SELECT Top 1 d_style_info.bom_type FROM d_style_info WHERE (a.product = d_style_info.style_no)) AS bom_type,
                //                                                  (SELECT COUNT(DISTINCT w_actual.name) FROM w_actual WHERE (w_actual.at_no = a.at_no) and (w_actual.active = 1)) AS process_count,
                //                                                  (SELECT COUNT(DISTINCT w_actual.name) FROM w_actual WHERE ((w_actual.at_no = a.at_no) AND (w_actual.actual > 0) and (w_actual.active = 1))) AS count_pr_w,
                //                                                              (SELECT COUNT(id_actual) FROM w_actual WHERE TYPE ='MMS' AND a.at_no = at_no) As CountProcess,
                //                                                  b.actual AS actual,
                //                                                              a.active As active
                //                                                 FROM
                //                                                  w_actual_primary a
                //                                              LEFT JOIN w_actual b ON a.at_no = b.at_no
                //                                ) as a
                //                                                          LEFT JOIN
                //                                                           (
                //                                                             select a.at_no,count(a.id_actual) poRun 
                //from w_actual a
                //left join (select id_actual,max(start_dt) start_dt,max(end_dt) end_dt  from  d_pro_unit_staff group by id_actual) b on a.id_actual=b.id_actual
                //left join (select id_actual,max(reg_date) reg_date from w_material_info_mms
                //group by id_actual) c on a.id_actual=c.id_actual and (c.reg_date between b.start_dt and b.end_dt)
                //where a.type='SX' and (GETDATE() between b.start_dt and b.end_dt) and (DATEDIFF(HOUR,GETDATE(),c.reg_date)<=1)
                //group by a.at_no) AS subquery ON subquery.at_no = a.at_no
                //                  inner join (
                //                                                 SELECT 
                //                                                   a.at_no AS at_no,
                //                                                  SUM(a.target) AS totalTarget ,

                //                								   SUM(b.actual) AS ttactual,a.process_code
                //                                                             -- a.active As active
                //                                                 FROM
                //                                                  w_actual_primary a
                //                                              LEFT JOIN w_actual b ON a.at_no = b.at_no
                //                							  group by a.at_no,a.process_code) tttarget on tttarget.at_no=a.at_no
                //                                               WHERE  a.finish_yn = 'N' --And a.type = 'SX' 
                //                And a.active = 1
                //                                And (@At_No = '' OR @At_No IS NULL Or a.at_no like '%'+@At_No+'%') And (@Product = '' OR @Product IS NULL Or a.product like '%'+@Product+'%') 
                //                                And (@Style_Name = '' OR @Style_Name IS NULL Or a.style_nm like '%'+@Style_Name+'%') And (@Model_Code = '' OR @Model_Code IS NULL Or a.md_cd like '%'+@Model_Code+'%') 
                //                                And (((@RegStart = '' OR @RegStart IS NULL) And (@EndStart = '' OR @EndStart IS NULL)) Or Convert(date,a.reg_dt) between @RegStart and @EndStart)
                //                                AND (@bom_type ='' OR @bom_type is null or a.bom_type like '%'+@bom_type+'%')
                //                                              Group By a.at_no,a.CountProcess,subquery.poRun,tttarget.ttactual,tttarget.totalTarget,tttarget.process_code
                //                                                  drop table #tmp";

                string query = @"Select 
                                     max(a.id_actualpr) AS id_actualpr
                                    ,max(a.id_actualpr) AS id_actualpr1
		                            , max(a.at_no) AS at_no
		                            , max(a.type) AS type
		                           , tttarget.totalTarget AS totalTarget
		                            , max(a.target) AS target
		                            , max(a.product) AS product
		                            , max(a.md_cd) AS md_cd
		                            , max(a.remark) AS remark
		                            , max(a.style_nm) AS style_nm
		                            , max(a.process_count) AS process_count
		                            , tttarget.ttactual AS actual
		                            , max (a.count_pr_w) AS count_pr_w
				                    ,isnull(subquery.poRun,0) AS poRun
		                            , max (a.IsApply) AS IsApply
                                    , MAX(CONVERT(int,active)) as active
                                    , MAX(a.CountProcess) As CountProcess
,a.bom_type,tttarget.process_code  as process_code
						        From (
			                            SELECT 
				                            a.IsApply AS IsApply,
				                            a.finish_yn AS finish_yn,
				                            a.id_actualpr AS id_actualpr,
				                            a.at_no AS at_no,
				                            b.type AS type,
				                            a.target AS target,
				                            a.product AS product,
				                            a.remark AS remark,
				                            a.reg_id AS reg_id,
				                            a.reg_dt AS reg_dt,
				                            a.chg_id AS chg_id,
				                            a.chg_dt AS chg_dt,
				                            b.name AS processName,
 (SELECT Top 1 d_style_info.bom_type FROM d_style_info WHERE (a.product = d_style_info.style_no)) AS bom_type,
				                            (SELECT Top 1 d_style_info.style_nm FROM d_style_info WHERE (a.product = d_style_info.style_no)) AS style_nm,
				                            (SELECT Top 1 d_style_info.md_cd FROM d_style_info WHERE (a.product = d_style_info.style_no)) AS md_cd,
				                            (SELECT COUNT(DISTINCT w_actual.name) FROM w_actual WHERE (w_actual.at_no = a.at_no)) AS process_count,
				                            (SELECT COUNT(DISTINCT w_actual.name) FROM w_actual WHERE ((w_actual.at_no = a.at_no) AND (w_actual.actual > 0))) AS count_pr_w,
                                            (SELECT COUNT(id_actual) FROM w_actual WHERE type='SX' and a.at_no = at_no) As CountProcess,
				                            b.actual AS actual,
                                            a.active As active
			                            FROM
				                            w_actual_primary a
				                        LEFT JOIN w_actual b ON a.at_no = b.at_no
						        ) as a
 inner join (
                                                                 SELECT 
                                                                   a.at_no AS at_no,
                                                                  SUM(a.target) AS totalTarget ,

                                								   SUM(b.actual) AS ttactual,a.process_code
                                                                             -- a.active As active
                                                                 FROM
                                                                  w_actual_primary a
                                                              LEFT JOIN w_actual b ON a.at_no = b.at_no
                                							  group by a.at_no,a.process_code) tttarget on tttarget.at_no=a.at_no
                                        LEFT JOIN
	                                        (
		                                        select a.at_no,count(a.id_actual) poRun 
from w_actual a
left join (select id_actual,max(start_dt) start_dt,max(end_dt) end_dt  from  d_pro_unit_staff group by id_actual) b on a.id_actual=b.id_actual
left join (select id_actual,max(reg_date) reg_date from w_material_info_mms
where active=1
group by id_actual) c on a.id_actual=c.id_actual and (c.reg_date between b.start_dt and b.end_dt)
where a.type='SX' and (GETDATE() between b.start_dt and b.end_dt) and (DATEDIFF(HOUR,c.reg_date,GETDATE())<=1)
group by a.at_no) AS subquery ON subquery.at_no = a.at_no
 
                                           WHERE  a.finish_yn = 'N' And a.active = 1
                						And (@At_No = '' OR @At_No IS NULL Or a.at_no like '%'+@At_No+'%') And (@Product = '' OR @Product IS NULL Or a.product like '%'+@Product+'%')  And (@bom_type = '' OR @bom_type IS NULL Or a.bom_type like '%'+@bom_type+'%') 
                						And (@Style_Name = '' OR @Style_Name IS NULL Or a.style_nm like '%'+@Style_Name+'%') And (@Model_Code = '' OR @Model_Code IS NULL Or a.md_cd like '%'+@Model_Code+'%') 
                						And (((@RegStart = '' OR @RegStart IS NULL) And (@EndStart = '' OR @EndStart IS NULL)) Or Convert(date,a.reg_dt) between @RegStart and @EndStart)
                                           Group By a.at_no,a.bom_type,tttarget.process_code,tttarget.ttactual,tttarget.totalTarget,subquery.poRun";
                var resultSearch = await base.DbConnection.QueryAsync<DatawActualPrimaryResponse>(query, new
                {
                    @At_No = item.at_no,
                    @Product = item.product,
                    @Style_Name = item.product_name,
                    @Model_Code = item.model,
                    @RegStart = item.regstart,
                    @EndStart = item.regend,
                    @bom_type = item.bom_type
                });

                return resultSearch;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Hiển thị danh sách các PO các trạng thái là Finish rùi
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DatawActualPrimaryResponse>> getListFinishPO(DataWActualPrimaryReq item)
        {
            try
            {
                var query = @"
                        Select 
                            max(a.id_actualpr) AS id_actualpr
		                    , max(a.at_no) AS at_no
		                    , max(a.type) AS type
		                    , SUM(a.target) AS totalTarget
		                    , max(a.target) AS target
		                    , max(a.product) AS product
		                    , max(a.md_cd) AS md_cd
		                    , max(a.remark) AS remark
		                    , max(a.style_nm) AS style_nm
		                    , max(a.process_count) AS process_count
		                    , sum(a.actual) AS actual
		                    , max(a.count_pr_w) AS count_pr_w
		                    , max(a.IsApply) AS IsApply
	                    From (
			                    SELECT 
				                    a.IsApply AS IsApply,
				                    a.finish_yn AS finish_yn,
				                    a.id_actualpr AS id_actualpr,
				                    a.at_no AS at_no,
				                    a.type AS type,
				                    a.target AS target,
				                    a.product AS product,
				                    a.remark AS remark,
				                    a.reg_id AS reg_id,
				                    a.reg_dt AS reg_dt,
				                    a.chg_id AS chg_id,
				                    a.chg_dt AS chg_dt,
				                    b.name AS processName,
	                                a.active,
				                    (SELECT Top 1 d_style_info.style_nm FROM d_style_info WHERE (a.product = d_style_info.style_no)) AS style_nm,
				                    (SELECT Top 1 d_style_info.md_cd FROM d_style_info WHERE (a.product = d_style_info.style_no)) AS md_cd,
				                    (SELECT COUNT(DISTINCT w_actual.name) FROM w_actual WHERE (w_actual.at_no = a.at_no)) AS process_count,
				                    (SELECT COUNT(DISTINCT w_actual.name) FROM w_actual WHERE ((w_actual.at_no = a.at_no) AND (w_actual.actual > 0))) AS count_pr_w,
				                     b.actual AS actual
			                    FROM
				                    (w_actual_primary a
				                     LEFT JOIN w_actual b ON ((a.at_no = b.at_no)))) as a
	                    WHERE a.finish_yn IN ('Y', 'YT')  And a.active = 1
						And (@At_No = '' OR @At_No IS NULL Or a.at_no like '%'+@At_No+'%') And (@Product = '' OR @Product IS NULL Or a.product like '%'+@Product+'%') 
						And (@Style_Name = '' OR @Style_Name IS NULL Or a.style_nm like '%'+@Style_Name+'%') And (@Model_Code = '' OR @Model_Code IS NULL Or a.md_cd like '%'+@Model_Code+'%') 
						And (((@RegStart = '' OR @RegStart IS NULL) And (@EndStart = '' OR @EndStart IS NULL)) Or Convert(date,a.reg_dt) between @RegStart and @EndStart)
	                    Group By a.at_no";
                var resultSearch = await base.DbConnection.QueryAsync<DatawActualPrimaryResponse>(query, new
                {
                    @At_No = item.at_no,
                    @Product = item.product,
                    @Style_Name = item.product_name,
                    @Model_Code = item.model,
                    @RegStart = item.regstart,
                    @EndStart = item.regend
                });

                return resultSearch;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Hiển thị danh sách công đoạn của 1 PO nào đó
        /// </summary>
        /// <param name="atNo"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ActualResponse>> getListActualOfProductOrder(string atNo)
        {
            try
            {
                var sql = @"
					        SELECT
	                            a.id_actual,
	                            a.type,
	                            a.NAME,
	                            a.date,
	                            a.item_vcd,
	                            ( SELECT Top 1 item_nm FROM qc_item_mt WHERE item_vcd = a.item_vcd ) AS QCName,
	                            a.defect,
	                            (bang3.actual_cn +  bang3.actual_cd) As actual,
	                            (bang3.actual_cn),
	                            (bang3.actual_cd),
	                            (SELECT Top 1 dt_nm  FROM comm_dt  WHERE dt_cd = a.don_vi_pr AND mt_cd = 'COM032') AS RollName,
	                            a.reg_id,
	                            a.reg_dt,
	                            a.chg_id,
	                            a.chg_dt,
	                            b.target,
	                            a.level,
	                            bang2.mc_no,
                                a.description,
									bang4.ProcessRun ProcessRun
                            FROM
	                            w_actual a
                            JOIN w_actual_primary b ON a.at_no = b.at_no
                            LEFT JOIN (
	                            SELECT
		                            aa.id_actual,
		                            mc_no 
	                            FROM
		                            ( SELECT pmid, id_actual, mc_no, reg_dt 
                                        FROM d_pro_unit_mc 
                                        WHERE id_actual IN (select id_actual from w_actual where at_no= @at_no  AND type='SX') ) aa
		                            INNER JOIN ( 
                                    SELECT id_actual, MAX( reg_dt ) regdt 
                                    FROM d_pro_unit_mc 
                                    WHERE id_actual IN (select id_actual from w_actual where  at_no = @at_no  AND type='SX') 
                                    GROUP BY id_actual ) bb ON bb.regdt = aa.reg_dt
	
                            ) AS bang2 ON a.id_actual = bang2.id_actual

                            LEFT JOIN (
	                            SELECT
		                            p.id_actual,
		                             ISNULL(SUM( CASE WHEN p.reg_dt = 'CN' THEN p.sl_tru_ng END ),0) AS actual_cn,
		                             ISNULL (SUM( CASE WHEN p.reg_dt = 'CD' THEN p.sl_tru_ng END ),0) AS actual_cd 
	                            FROM
		                            (
		                            SELECT a.id_actual,
			                            (CASE
				                            WHEN ('08:00:00' <= CAST(a.reg_date AS time(0))  AND CAST( a.reg_date AS time(0))  <= '20:00:00' ) THEN 'CN' ELSE 'CD' END ) AS reg_dt,
					                            SUM(ISNULL( a.real_qty - d.sltrung, a.real_qty )) sl_tru_ng 
				                            FROM (SELECT a.id_actual,a.reg_date,a.real_qty,a.material_code FROM w_material_info_mms a Join w_actual b On a.id_actual = b.id_actual
							
								            where material_type='CMT' /* AND bbmp_sts_cd in ('000','001','002') AND orgin_mt_cd IS NULL*/ AND b.at_no =  @at_no ) As a
								
					                            LEFT JOIN ( SELECT ( check_qty - ok_qty ) AS sltrung, ml_no FROM m_facline_qc WHERE ml_tims IS NULL ) d ON d.ml_no = a.material_code 
				                            GROUP BY id_actual,reg_date
				                            ) p 
			                            GROUP BY p.id_actual
			                            ) bang3 ON a.id_actual = bang3.id_actual

								 LEFT JOIN (
										   select distinct a.at_no,a.id_actual,
(case when ((GETDATE() between b.start_dt and b.end_dt) and (DATEDIFF(HOUR,c.reg_date,GETDATE())<=1)) then 2 
when  (GETDATE() between b.start_dt and b.end_dt) and DATEDIFF(HOUR,c.reg_date,GETDATE())>1 or ((GETDATE() between b.start_dt and b.end_dt) and c.reg_date is null)
then 1
else 0 end) ProcessRun
from w_actual a
left join	(select id_actual,max(start_dt) start_dt,max(end_dt) end_dt  from  d_pro_unit_staff where active = 1  group by id_actual) b on a.id_actual=b.id_actual
left join (select id_actual,max(reg_date) reg_date from w_material_info_mms where active = 1 
group by id_actual) c on a.id_actual=c.id_actual and (c.reg_date between b.start_dt and b.end_dt)
										  ) bang4 ON a.id_actual = bang4.id_actual
			             
                            where  a.at_no= @at_no and a.type='SX' And a.active = 1
				            GROUP BY a.id_actual, a.type, a.name, a.date, a.item_vcd, defect, a.actual, bang3.actual_cn, bang3.actual_cd, a.don_vi_pr, 
				            a.reg_dt, a.chg_dt, a.reg_id, a.chg_id, b.target, a.level, bang2.mc_no, a.description,bang4.ProcessRun
                            Order By a.name, a.level";
                var result = await base.DbConnection.QueryAsync<ActualResponse>(sql, new { @at_no = atNo });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Lấy ra Id của 1 PO theo At_No của 1 PO đó để finish PO đó
        /// </summary>
        /// <param name="atNo"></param>
        /// <returns></returns>
        public async Task<int> GetPO(string atNo)
        {
            try
            {
                var query = "Select id_actualpr from w_actual_primary where at_no = @At_No";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @At_No = atNo });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// Lấy ra Id của 1 PO theo Id của PO đó để Redo Finish cho PO đó
        /// </summary>
        /// <param name="atNo"></param>
        /// <returns></returns>
        public async Task<int> GetRedoPO(int id_actualpr)
        {
            try
            {
                var query = @"Select id_actualpr from w_actual_primary where id_actualpr = @Id";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Id = id_actualpr });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        /// <summary>
        /// Cập nhật lại trạng thái của PO khi nó đã finish hay Redo lại Finish
        /// </summary>
        /// <param name="finish_yn"></param>
        /// <param name="id_actualpr"></param>
        /// <returns></returns>
        public async Task<int> UpdatePO(string finish_yn, int id_actualpr)
        {
            try
            {
                var query = @"Update w_actual_primary Set finish_yn = @Finish_Yn Where id_actualpr = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Finish_Yn = finish_yn, @Id = id_actualpr });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Tạo mới 1 công đoạn cho 1 PO nào đó
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<int> insertActualInfo(Actual item)
        {
            try
            {
                string sql = @"INSERT INTO w_actual(at_no,type,actual,defect,name,level,date,don_vi_pr,item_vcd,description,reg_id,reg_dt,chg_id,chg_dt,product,IsFinish)
                              VALUES(@at_no, @type, @actual, @defect, @name, @level, GETDATE(), @don_vi_pr, @item_vcd, @description, @reg_id, GETDATE(), @chg_id, GETDATE(),@product,@IsFinish)
                              Select scope_identity()";
                int result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql,
                    new
                    {
                        @at_no = item.at_no,
                        @type = item.type,
                        @actual = item.actual,
                        @defect = item.defect,
                        @name = item.name,
                        @level = item.level,
                        @date = item.date,
                        @don_vi_pr = item.don_vi_pr,
                        @item_vcd = item.item_vcd,
                        @description = item.description,
                        @reg_id = item.reg_id,
                        @reg_dt = item.reg_dt,
                        @chg_id = item.chg_id,
                        @chg_dt = item.chg_dt,
                        @product = item.product,
                        @IsFinish = item.IsFinish
                    });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// Thêm mới sản phẩm theo product cho từng công đoạn đó
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<int> insertMaterialForActual(MaterialInfo item)
        {
            try
            {
                string sql = "INSERT INTO d_material_info(mt_no,mt_nm,mt_type,reg_dt,bundle_unit,chg_dt,reg_id,chg_id,use_yn,del_yn) " +
                              " VALUES(@mt_no, @mt_nm, @mt_type, @reg_dt, @bundle_unit, @chg_dt, @reg_id, @chg_id, @use_yn, @del_yn);" +
                              "Select scope_identity();";

                int result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql,
                       new
                       {
                           @mt_no = item.mt_no,
                           @mt_nm = item.mt_nm,
                           @mt_type = item.mt_type,
                           @reg_dt = item.reg_dt,
                           @bundle_unit = item.bundle_unit,
                           @chg_dt = item.chg_dt,
                           @reg_id = item.reg_id,
                           @chg_id = item.chg_id,
                           @use_yn = item.use_yn,
                           @del_yn = item.del_yn
                       });
                return result;
            }
            catch (Exception e)
            {

                throw;
            }

        }

        /// <summary>
        /// Xóa 1 công đoạn theo id_actual bên MMS
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> removeActualInfo(int id)
        {
            try
            {
                var result = 0;
                string queryActual = @"Select Count(*) from w_actual where id_actual = @id";
                var findId = await base.DbConnection.ExecuteScalarAsync<int>(queryActual, new { @id = id });
                if (findId > 0)
                {
                    var queryMaterial = @"Select Count(*) from w_material_mapping_mms where mt_lot IN (Select material_code from w_material_info_mms where id_actual = @actual)";
                    var material_info = await base.DbConnection.ExecuteScalarAsync<int>(queryMaterial, new { @actual = id });

                    if (material_info == 0)
                    {
                        var removeStaff = @"Update d_pro_unit_staff Set active = 0 where id_actual = @Staff";
                        var resultStaff = await base.DbConnection.ExecuteAsync(removeStaff, new { @Staff = id });

                        var removeMachine = @"Update d_pro_unit_mc Set active = 0 where id_actual = @Machine";
                        var resultMachine = await base.DbConnection.ExecuteAsync(removeMachine, new { @Machine = id });

                        var removeMaterialMapping = @"Update w_material_info_mms set active = 0 where id_actual = @Material";
                        var resultMaterialMapping = await base.DbConnection.ExecuteAsync(removeMaterialMapping, new { @Material = id });
                    }
                    else
                    {
                        return 0;
                    }

                    var query = @"Update w_actual Set active = 0 where id_actual = @id_actual";
                    result = await base.DbConnection.ExecuteAsync(query, new { @id_actual = id });
                }
                else
                {
                    return -1;
                }

                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Kiểm tra xem sản phẩm đó nó đã tồn tại ở trong bảng d_style_info hay chưa
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<int> checkProductExisted(string product)
        {
            try
            {
                var query = "SELECT Count(*) FROM d_style_info where style_no = @product";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @product = product });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Kiểm tra xem nguyên vật liệu đó có tồn tại ở trong bảng d_material_info hay chưa
        /// </summary>
        /// <param name="mt_no"></param>
        /// <returns></returns>
        public async Task<int> checkMaterialForActuals(string mt_no)
        {
            try
            {
                var query = "SELECT Count(*) FROM d_material_info WHERE mt_no = @material";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @material = mt_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Kiểm tra xem công đoạn đó nó đã tồn tại ở trong bảng w_actual hay chưa
        /// </summary>
        /// <param name="at_no"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<IEnumerable<w_actual>> checkActualExisted(string at_no)
        {
            try
            {
                string query = @"SELECT MIN(name)name, MIN(level) level FROM w_actual Where at_no = @AtNo AND type = 'SX' And active = 1 group by name";
                //var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @at_no = at_no, @type = type });
                var result = await base.DbConnection.QueryAsync<w_actual>(query, new { @AtNo = at_no });
                return result;

            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Xóa PO theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> removeProductOrder(int id)
        {
            try
            {
                var query = "";
                int result = 0;
                var findPO = @"Select * from w_actual_primary where id_actualpr = @Id";
                var productOrder = await base.DbConnection.QueryFirstOrDefaultAsync<ActualPrimary>(findPO, new { @Id = id });
                if (productOrder != null)
                {
                    var checkMaterialMapping = @"Select Count(*) from w_material_mapping_mms where mt_lot IN 
                                                (Select material_code from w_material_info_mms where id_actual IN 
                                                (Select id_actual from w_actual where at_no = @At_No And type = 'SX' and active = 1))";
                    var materialMapp = await base.DbConnection.ExecuteScalarAsync<int>(checkMaterialMapping, new { @At_No = productOrder.at_no });

                    if (materialMapp == 0)
                    {
                        var removeStaff = @"Update d_pro_unit_staff Set active = 0 where id_actual IN (Select id_actual from w_actual where at_no = @Staff)";
                        var resultStaff = await base.DbConnection.ExecuteAsync(removeStaff, new { @Staff = productOrder.at_no });

                        var removeMachine = @"Update d_pro_unit_mc Set active = 0 where id_actual IN (Select id_actual from w_actual where at_no = @Machine)";
                        var resultMachine = await base.DbConnection.ExecuteAsync(removeMachine, new { @Machine = productOrder.at_no });

                        var removeMaterialMapping = @" Update w_material_info_mms set active = 0 where id_actual IN (Select id_actual from w_actual where at_no = @Material)";
                        var resultMaterialMapping = await base.DbConnection.ExecuteAsync(removeMaterialMapping, new { @Material = productOrder.at_no });

                        var removeActual = @"Update w_actual Set active = 0 where id_actual IN (Select id_actual from w_actual where at_no = @Actual and type = 'SX' )";
                        var resultActual = await base.DbConnection.ExecuteAsync(removeActual, new { @Actual = productOrder.at_no });
                    }
                    else
                    {
                        return -1;
                    }

                    query = "Update w_actual_primary set active = 0 where id_actualpr = @Id";
                    result = await base.DbConnection.ExecuteAsync(query, new { @Id = id });
                }
                else return -2;
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Cập nhật PO theo Id
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<int> updateProductOrder(ActualPrimaryModify item)
        {
            try
            {
                var query = "Update w_actual_primary Set target = @Target, remark = @Remark,process_code = @process_code where id_actualpr = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Target = item.target, @Remark = item.remark, @Id = item.id_actualpr, @process_code = item.process_code });
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Lấy ra các công đoạn vừa mới được tạo mới của 1 PO theo At_No
        /// </summary>
        /// <param name="at_no"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DatawActualPrimaryResponse>> GetProcessingActual(string at_no)
        {
            try
            {
                var query = @"SELECT 
                                a.id_actualpr,
                                a.id_actualpr As id_actualpr1,
                                a.at_no, a.type,
                                a.target,
                                a.product,
                                a.remark,
                                0 process_count,
                                a.process_code,
                                b.md_cd AS md_cd,
                                b.style_nm AS style_nm 
                                FROM
	                                w_actual_primary AS a 
				                    LEFT JOIN (Select md_cd,style_nm,style_no from d_style_info )as b
				                    ON a.product = b.style_no
                                WHERE a.at_no = @AtNo";
                var result = await base.DbConnection.QueryAsync<DatawActualPrimaryResponse>(query, new { @AtNo = at_no });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Lấy thông tin của 1 công đoạn theo Id
        /// </summary>
        /// <param name="id_actual"></param>
        /// <returns></returns>
        public async Task<Actual> GetActual(int id_actual)
        {
            try
            {
                var query = @"SELECT * FROM w_actual WHERE id_actual= @Id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<Actual>(query, new { @Id = id_actual });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Cập nhập thông tin 1 công đoạn nào đó theo Id
        /// </summary>
        /// <param name="id_actual"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public async Task<int> UpdateDescriptionActual(int id_actual, string description)
        {
            try
            {
                var query = @"UPDATE w_actual SET description = @Desc WHERE id_actual = @Id ";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Desc = description, @Id = id_actual });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Lấy số PO đã tồn tai để làm PO tự tăng dần lên 1
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<w_actual_primary> CheckPOExisted(string name)
        {
            try
            {
                var query = @"Select at_no from w_actual_primary at_no where at_no like @Name + '%' order by at_no desc";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<w_actual_primary>(query, new { @Name = name });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<int> CheckPOExisted1(string name)
        {
            try
            {
                var query = @"Select Count(*) from w_actual_primary where at_no like @Name";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Name = name });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Tạo ra 1 PO mới
        /// </summary>
        /// <param name="productOrder"></param>
        /// <returns></returns>
        public async Task<int> insertNewProductOrder(ActualPrimary productOrder)
        {
            try
            {
                string query = @"INSERT INTO w_actual_primary(at_no, type, target, product, remark, finish_yn, isapply, reg_id, reg_dt, chg_id, chg_dt,process_code) 
                                 VALUES(@at_no, @type, @target,@product, @remark, @finish_yn, @isapply, @reg_id, @reg_dt, @chg_id, @chg_dt,@process_code)";

                var result = await base.DbConnection.ExecuteAsync(query,
                       new
                       {
                           @at_no = productOrder.at_no,
                           @type = productOrder.type,
                           @target = productOrder.target,
                           @product = productOrder.product,
                           @remark = productOrder.remark,
                           @finish_yn = productOrder.finish_yn,
                           @isapply = productOrder.isapply,
                           @reg_id = productOrder.reg_id,
                           @reg_dt = productOrder.reg_dt,
                           @chg_id = productOrder.chg_id,
                           @chg_dt = productOrder.chg_dt,
                           @process_code = productOrder.process_code
                       });

                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        #endregion

        #region Get Tổng Hợp
        /// <summary>
        ///  Lấy danh sách tổng hợp kết hợp giữa các công nhân, máy và Mold
        /// </summary>
        /// <param name="idActual"></param>
        /// <returns></returns>
        public async Task<IEnumerable<wo_info_mc_wk_mold>> GetInfoMcWkMold(int idActual)
        {
            try
            {
                var query = @"SELECT		
			                    a.id_actual,
                                a.mc_no code,
                                a.use_yn,a.pmid, 
                                '' AS staff_tp, 
                                a.start_dt As start_dt, 
			                    a.end_dt  as end_dt, 
                                (CASE 
                                    WHEN (SELECT COUNT(*) FROM d_mold_info WHERE md_no=a.mc_no) >0 THEN 'mold' 
	                                WHEN (SELECT COUNT(*) FROM d_machine_info WHERE mc_no=a.mc_no) >0 THEN 'machine' 
                                END)type, 
                                (CASE 
                                    WHEN (SELECT COUNT(*) FROM d_mold_info WHERE md_no=a.mc_no) >0 THEN (SELECT md_nm FROM d_mold_info WHERE md_no=a.mc_no) 
	                                WHEN (SELECT COUNT(*) FROM d_machine_info WHERE mc_no=a.mc_no) >0 THEN (SELECT mc_nm FROM d_machine_info WHERE mc_no=a.mc_no) 
                                END)name, 
                                    (case when GETDATE() BETWEEN a.start_dt AND a.end_dt then 'HT' ELSE 'QK' END)het_ca 
                                from d_pro_unit_mc AS a 
                                WHERE a.id_actual=@Id 
                                UNION 
                                SELECT id_actual,
                                staff_id code,
                                use_yn,psid,  
                                staff_tp, 
                                start_dt As start_dt, 
                                end_dt  As end_dt, 
                                ('worker')type,
                                    (SELECT uname FROM mb_info WHERE userid=staff_id) name ,
                                (case when GetDate() BETWEEN  d_pro_unit_staff.start_dt AND d_pro_unit_staff.end_dt then 'HT' ELSE 'QK' END)het_ca 
                                FROM d_pro_unit_staff 
                                WHERE id_actual=@Id";
                var result = await base.DbConnection.QueryAsync<wo_info_mc_wk_mold>(query, new { @Id = idActual });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết của công đoạn theo ca ngày hoặc ca đêm
        /// </summary>
        /// <param name="id_actual"></param>
        /// <param name="mt_type"></param>
        /// <param name="date"></param>
        /// <param name="shift"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WMaterialInfoAPI>> GetDetailActualAPI(int id_actual, string mt_type, string date, string shift)
        {
            try
            {
                var query = @"SELECT * FROM 
						                (SELECT	wmtid, material_code As mt_cd, gr_qty, bb_no, real_qty AS sl_tru_ng,
						                (
							                CASE 
								                WHEN ('08:00:00' <= CAST( reg_date AS  time(0)) AND CAST(reg_date AS time(0))  <=  '20:00:00') THEN N'Ca ngày' 
								                WHEN CAST(reg_date AS time(0)) >= '20:00:00' AND  CAST(reg_date AS time(0)) <= '23:59:59'
                                                	OR   ( CAST( reg_date AS time(0) )  < '08:00:00')
                                                THEN  N'Ca đêm' 
							                ELSE '' END )  as ca,
						                (
							                CASE 
								                WHEN ('08:00:00' <=  CAST( reg_date AS time(0)) AND CAST( reg_date AS time(0))  < '23:59:59') THEN  reg_date
								                WHEN ( CAST( reg_date AS time(0) )  < '08:00:00') THEN CAST(reg_date - 1 as datetime )
							                ELSE '' END)  as reg_date,
  (
							                CASE 
								                WHEN ('08:00:00' <=  CAST( reg_date AS time(0)) AND CAST( reg_date AS time(0))  < '23:59:59') THEN  reg_date
								                WHEN ( CAST( reg_date AS time(0) )  < '08:00:00') THEN CAST(reg_date - 1 as datetime )
							                ELSE '' END)  as reg_date1
						                 FROM w_material_info_mms  
						                 WHERE id_actual = @Id AND material_type = @Mt_Type 
						             ) AS TABLE1  
			                 WHERE   (@Date='' OR   TABLE1.reg_date like  @likeDate)  
			                 AND    (@Shift='' OR   TABLE1.ca like  @Shiftlike)  
                             ORDER BY TABLE1.reg_date desc, TABLE1.ca desc ";
                var result = await base.DbConnection.QueryAsync<WMaterialInfoAPI>(query,
                    new
                    {
                        @Id = id_actual,
                        @Mt_Type = mt_type,
                        @Date = date,
                        @likeDate = "%" + date + "%",
                        @Shift = shift,
                        @Shiftlike = "%" + shift + "%"
                    });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        #endregion

        #region Routinng
        /// <summary>
        /// Cập nhập PO khi người dùng nhấn vào Ô apply
        /// </summary>
        /// <param name="Id_Actualpr"></param>
        /// <param name="IsApply"></param>
        /// <returns></returns>
        public async Task<int> UpdateProductDeApply(int Id_Actualpr, string IsApply)
        {
            try
            {
                var query = @"Update w_actual_primary SET IsApply = @isApply WHERE id_actualpr = @idActualpr";
                var result = await base.DbConnection.ExecuteAsync(query, new { @isApply = IsApply, @idActualpr = Id_Actualpr });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Lấy dách các NVL từ bảng routing
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DRoutingInfo>> GetRoutingMMS(string productCode, string processCode)
        {
            try
            {
                var query = @"SELECT * FROM d_rounting_info a WHERE a.style_no = @ProductCode and a.process_code =@processCode";
                var result = await base.DbConnection.QueryAsync<DRoutingInfo>(query, new { @ProductCode = productCode, @processCode = processCode });
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region Print NG

        /// <summary>
        /// Lấy ra các ds các NVL đạt chỉ tiêu (hư hỏng)
        /// </summary>
        /// <param name="code"></param>
        /// <param name="at_no"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<IEnumerable<NGResponse>> GetListNG(string code, string at_no, string product)
        {
            try
            {
                var query = @"SELECT * 
                              FROM (SELECT 
				                        a.wmtid AS Id,
				                        a.gr_qty AS Qty,
				                        a.material_code AS MaterialCode,
				                        SUBSTRING(a.material_code, 1, 14) AS at_no,
				                        (SELECT product FROM w_actual_primary WHERE at_no = SUBSTRING(a.material_code, 1, 11)) AS product, b.check_qty AS SLCK
				                --        (SELECT Top 1 check_qty FROM m_facline_qc WHERE m_facline_qc.ml_no = a.material_code) AS SLCK
			                        FROM
				                        w_material_info_mms a
	                                    left join m_facline_qc b on b.ml_no = a.material_code
			                        WHERE(a.location_code LIKE '002%') AND (a.status = '003') AND (a.material_code LIKE 'PO%')) AS view_printng
                            WHERE(@MtCode = '' OR @MtCode IS NULL OR view_printng.MaterialCode LIKE '%'+ @MtCode +'%')
                            AND (@Product = '' OR @Product IS NULL OR view_printng.product LIKE '%'+ @Product +'%')
                            AND (@AtNo = '' OR @AtNo IS NULL OR view_printng.at_no LIKE '%' + @AtNo + '%') order by view_printng.Id desc ";
                var result = await base.DbConnection.QueryAsync<NGResponse>(query, new { @MtCode = code, @Product = product, @AtNo = at_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Lấy ra các ds các NVL đã đạt chỉ tiêu
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MaterialInfoMMS>> GetListMaterialOK()
        {
            try
            {
                var query = @"SELECT * FROM w_material_info_mms AS a WHERE a.status = 012 AND a.location_code LIKE '002%'";
                var result = await base.DbConnection.QueryAsync<MaterialInfoMMS>(query);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Kiểm tra và lấy ra các NVL nào ko đạt chỉ tiêu
        /// </summary>
        /// <param name="mt_cd"></param>
        /// <returns></returns>
        public async Task<MaterialInfoMMS> CheckMaterialNG_OK(string mt_cd)
        {
            try
            {
                var query = @"Select * from w_material_info_mms where material_code = @Mt_Cd And status = 003";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoMMS>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        ///  Lấy ra cá ds NVL theo mã NVL
        /// </summary>
        /// <param name="mt_cd"></param>
        /// <returns></returns>
        public async Task<MaterialInfoMMS> CheckMaterialLotNG_OK(string mt_cd)
        {
            try
            {
                var query = @"Select * from w_material_info_mms where material_code = @Mt_Cd";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoMMS>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<InventoryProduct> CheckMaterialLotNG_OK_InvenotryProduct(string mt_cd)
        {
            try
            {
                var query = @"Select * From inventory_products where material_code = @Mt_Cd";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<InventoryProduct>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Lưu các thông tin của các NVL ko đạt chỉ tiêu vào hệ thống
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<int> InsertMaterialDown(MaterialDown item)
        {
            try
            {
                var query = @"Insert into w_material_down (mt_cd, gr_qty, gr_down, reason, status_now, bb_no, use_yn, reg_id, reg_dt, chg_id, chg_dt)
                                Values (@mt_cd, @gr_qty, @gr_down, @reason, @status_now, @bb_no, @use_yn, @reg_id, GETDATE(), @chg_id, GETDATE())";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion


        /// <summary>
        /// Hiển thị danh sách máy đã được tạo ra để mapping NVL
        /// </summary>
        /// <param name="id_actual"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcessMachineunit>> GetListMachineFromIdActual(int id_actual)
        {
            try
            {
                var query = "SELECT pmid,id_actual,start_dt,end_dt,remark,mc_no,use_yn FROM d_pro_unit_mc WHERE id_actual=@Id And active = 1";
                var result = await base.DbConnection.QueryAsync<ProcessMachineunit>(query, new { @Id = id_actual });
                return result;

            }
            catch (Exception e)
            {

                throw e;
            }
        }


        /// <summary>
        /// Hiển thị danh sách máy còn trống slot để chuẩn bị mapping với NVL
        /// </summary>
        /// <param name="mc_type"></param>
        /// <param name="mc_no"></param>
        /// <param name="mc_nm"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DMachineInfoAPI>> GetPopupMachine(string mc_type, string mc_no, string mc_nm, int page, int row)
        {

            try
            {
                var query = "SELECT * FROM " +
                        " (SELECT a.mno, a.mc_type, a.mc_no, a.mc_nm, a.purpose, a.color, a.barcode, a.re_mark, a.use_yn, a.del_yn,a.reg_id, a.reg_dt, a.chg_id, a.chg_dt " +
                        " ,(CASE " +
                                " WHEN  c.end_dt IS NULL THEN 'Unmapping' " +
                                " WHEN  GETDATE() > c.end_dt  THEN 'Unmapping' " +
                                " WHEN GETDATE() < c.end_dt  THEN 'Mapping' " +
                                " ELSE 'Unmapping' " +
                            " END)  uses " +
                        " FROM d_machine_info AS a " +
                        " LEFT JOIN(select b.mc_no, max(b.end_dt) AS end_dt From d_pro_unit_mc b GROUP BY b.mc_no) AS c ON a.mc_no = c.mc_no " +
                        " WHERE(@Mc_Type = '' OR @Mc_Type IS NULL OR   a.mc_type like '%'+@Mc_Type+'%') and(@Mc_No = '' OR @Mc_No IS NULL OR a.mc_no like '%'+@Mc_No+'%') and(@Mc_Nm = '' OR @Mc_Nm IS NULL OR   a.mc_nm like '%'+@Mc_Nm+'%') And a.active = 1 " +
                        " GROUP BY  a.mno, a.mc_type, a.mc_no,a.mc_nm, a.purpose, a.color, a.barcode, a.re_mark,a.use_yn, a.del_yn,a.reg_id,a.reg_dt, a.chg_id,a.chg_dt,c.end_dt) " +
                        " as MyDerivedTable ORDER BY MyDerivedTable.mc_no DESC  OFFSET @intpage ROWS FETCH NEXT @introw ROWS ONLY";
                var result = await base.DbConnection.QueryAsync<DMachineInfoAPI>(query, new { @Mc_Type = mc_type, @Mc_No = mc_no, @Mc_Nm = mc_nm, @intpage = page, @introw = row });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }



        public async Task<int> CountListPopupMachine(string mc_type, string mc_no, string mc_nm)
        {

            try
            {
                var query = "SELECT Count (a.mno) FROM " +
                        " (SELECT a.mno, a.mc_type, a.mc_no, a.mc_nm, a.purpose, a.color, a.barcode, a.re_mark, a.use_yn, a.del_yn,a.reg_id, a.reg_dt, a.chg_id, a.chg_dt " +
                        " ,(CASE " +
                                " WHEN  c.end_dt IS NULL THEN 'Unmapping' " +
                                " WHEN  GETDATE() > c.end_dt  THEN 'Unmapping' " +
                                " WHEN GETDATE() < c.end_dt  THEN 'Mapping' " +
                                " ELSE 'Unmapping' " +
                            " END)  uses " +
                        " FROM d_machine_info AS a " +
                        " LEFT JOIN(select b.mc_no, max(b.end_dt) AS end_dt From d_pro_unit_mc b GROUP BY b.mc_no) AS c ON a.mc_no = c.mc_no " +
                        " WHERE(@Mc_Type = '' OR @Mc_Type IS NULL OR  a.mc_type like '%'+@Mc_Type+'%') and(@Mc_No = '' OR @Mc_No IS NULL OR a.mc_no like '%'+@Mc_No+'%') and(@Mc_Nm = '' OR @Mc_Nm IS NULL OR   a.mc_nm like '%'+@Mc_Nm+'%') " +
                        "And a.active = 1) As a ";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Mc_Type = mc_type, @Mc_No = mc_no, @Mc_Nm = mc_nm });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        /// <summary>
        /// Tạo 1 máy mới để mapping NVL
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<int> InsertProUnitMC(DProUnitMachine item)
        {
            try
            {
                var query = "INSERT INTO d_pro_unit_mc (start_dt, end_dt, remark, mc_no, use_yn, del_yn, reg_id ,reg_dt, chg_dt, id_actual) " +
                " VALUES (@StartDate, @EndDate, @Remark, @Mc_No, @Use_Yn, @Del_Yn, @Reg_Id, @Reg_Date, @Chg_Date, @Id_Actual) " +
                " SELECT SCOPE_IDENTITY() ";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query,
                           new
                           {
                               @StartDate = item.start_dt,
                               @EndDate = item.end_dt,
                               @Remark = item.remark,
                               @Mc_No = item.mc_no,
                               @Use_Yn = item.use_yn,
                               @Del_Yn = item.del_yn,
                               @Reg_Id = item.reg_id,
                               @Reg_Date = item.reg_dt,
                               @Chg_Date = item.chg_dt,
                               @Id_Actual = item.id_actual
                           });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        /// <summary>
        /// Kiểm tra xem thông tin máy đó nó có còn tồn tại ở trong bảng d_machine_info theo mc_no
        /// </summary>
        /// <param name="mc_no"></param>
        /// <returns></returns>
        public async Task<int> CheckMachineInfo(string mc_no)
        {
            try
            {
                var query = "Select count(*) from d_machine_info where mc_no = @Mc_No ";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(query, new { @Mc_No = mc_no });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Kiểm tra xem thông tin máy đó nó có còn tồn tại ở trong bảng d_pro_unit_mc hay chưa ?
        /// </summary>
        /// <param name="mc_no"></param>
        /// <param name="id_actual"></param>
        /// <param name="pmid"></param>
        /// <returns></returns>
        public async Task<int> GetTotalCountMachine(string mc_no, int id_actual, int pmid)
        {
            try
            {
                var query = "SELECT COUNT(*) FROM d_pro_unit_mc WHERE mc_no=@Mc_No AND id_actual=@Id_Actual AND pmid != @Pmid";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(query, new { @Mc_No = mc_no, @Id_Actual = id_actual, @Pmid = pmid });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Kiểm tra trạng thái của máy để chuẩn bị cập nhật cho máy đó
        /// </summary>
        /// <param name="mcno"></param>
        /// <param name="id_actual"></param>
        /// <param name="pmid"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public async Task<int> GetStatusMachineUpdate(string mcno, int id_actual, int pmid, DateTime start, DateTime end)
        {
            try
            {
                var query = @"SELECT COUNT(*) FROM d_pro_unit_mc WHERE mc_no=@Mc_No AND id_actual=@Id_Actual AND pmid != @Pmid 
                             AND((start_dt <= CONVERT(VARCHAR(10), CONVERT(datetime, @StartDate, 105), 23)  AND CONVERT(VARCHAR(10), CONVERT(datetime, @StartDate, 105), 23) <= end_dt) 
                             OR(start_dt <= CONVERT(VARCHAR(10), CONVERT(datetime, @EndDate, 105), 23)  AND CONVERT(VARCHAR(10), CONVERT(datetime, @EndDate, 105), 23) <= end_dt))";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(query, new { @Mc_No = mcno, @Id_Actual = id_actual, @Pmid = pmid, @StartDate = start, @EndDate = end });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        /// <summary>
        /// Kiểm tra trạng thái của máy để chuẩn bị tạo ra máy mới mapping với công đoạn đó
        /// </summary>
        /// <param name="mc_no"></param>
        /// <param name="id_actual"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public async Task<string> GetStatusMachine(string mc_no, int id_actual, string start, string end)
        {
            try
            {
                string query = @"
                SELECT (
		            CASE
		            WHEN NOT  EXISTS(SELECT * from d_pro_unit_mc where mc_no=@mc_no)
		            THEN '0'
		            WHEN  EXISTS(SELECT * from d_pro_unit_mc WHERE mc_no =@mc_no AND id_actual=@id_actual AND start_dt <= @start  AND @start <= end_dt)
		            THEN '1'
		            WHEN  EXISTS(SELECT * from d_pro_unit_mc WHERE mc_no =@mc_no AND id_actual=@id_actual AND start_dt <= @end  AND @end <= end_dt)
		            THEN '1'
		            WHEN  EXISTS(SELECT * from d_pro_unit_mc WHERE mc_no =@mc_no AND start_dt <= @start  AND @start <= end_dt)
		            THEN 'SELECT pmid from d_pro_unit_mc WHERE mc_no =@mc_no   AND start_dt <= @start  AND @start <= end_dt'
		            WHEN EXISTS(SELECT * from d_pro_unit_mc WHERE mc_no =@mc_no AND start_dt <= @end  AND @end <= end_dt)
		            THEN 'SELECT pmid from d_pro_unit_mc WHERE mc_no =@mc_no AND start_dt <= @end  AND @end <= end_dt'
                    ELSE '0'
	                END
                ) AS a ";
                var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @mc_no = mc_no, @id_actual = id_actual, @start = start, @end = end });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<string> GetStatusMold(string md_no, int id_actual, string start, string end)
        {
            try
            {
                string query = @"
                SELECT (
		            CASE
		            WHEN NOT  EXISTS(SELECT * from d_pro_unit_mold where md_no=@md_no)
		            THEN '0'
		            WHEN  EXISTS(SELECT * from d_pro_unit_mold WHERE md_no =@md_no AND id_actual=@id_actual AND start_dt <= @start  AND @start <= end_dt)
		            THEN '1'
		            WHEN  EXISTS(SELECT * from d_pro_unit_mold WHERE md_no =@md_no AND id_actual=@id_actual AND start_dt <= @end  AND @end <= end_dt)
		            THEN '1'
		            WHEN  EXISTS(SELECT * from d_pro_unit_mold WHERE md_no =@md_no AND start_dt <= @start  AND @start <= end_dt)
		            THEN 'SELECT pmid from d_pro_unit_mold WHERE md_no =@md_no   AND start_dt <= @start  AND @start <= end_dt'
		            WHEN EXISTS(SELECT * from d_pro_unit_mold WHERE md_no =@md_no AND start_dt <= @end  AND @end <= end_dt)
		            THEN 'SELECT pmid from d_pro_unit_mold WHERE md_no =@md_no AND start_dt <= @end  AND @end <= end_dt'
                    ELSE '0'
	                END
                ) AS a ";
                var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @md_no = md_no, @id_actual = id_actual, @start = start, @end = end });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Hiển thị ra danh sách máy đã được mapping với công đoan đó
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ProcessMachineunit> GetMachineById(int Id)
        {
            try
            {
                var query = "Select * from d_pro_unit_mc where pmid = @id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<ProcessMachineunit>(query, new { @id = Id });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Cập nhật thông tin của máy đã được mapping với công đoạn đó
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<int> UpdateMachine(ProcessMachineunit item)
        {
            try
            {
                var query = @"UPDATE d_pro_unit_mc SET id_actual = @Id_Actual,start_dt = @StartDate, end_dt = @EndDate, remark = @Remark, mc_no = @Mc_No, use_yn = @Use_Yn  WHERE pmid = @Pmid ";
                int result = await base.DbConnection.ExecuteAsync(query,
                    new
                    {
                        @Id_Actual = item.id_actual,
                        @StartDate = item.start_dt,
                        @EndDate = item.end_dt,
                        @Remark = item.remark,
                        @Mc_No = item.mc_no,
                        @Use_Yn = item.use_yn,
                        @Pmid = item.pmid
                        //@Del_Yn = item.del_yn,
                        //@Reg_Id = item.reg_id,
                        //@RegDate = item.reg_dt,
                        //@Chg_Id = item.chg_id,
                        //@Chg_Date = item.chg_dt,

                    });
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Xóa máy đã được mapping với công đoạn đó
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="timeNow"></param>
        /// <returns></returns>
        public async Task<int> removeMachineMapping(string Id, DateTime timeNow)
        {
            try
            {
                var query = @" DELETE FROM d_pro_unit_mc WHERE pmid = @id and @TimeNow <= end_dt ";
                var result = await base.DbConnection.ExecuteAsync(query, new { @id = Id, @TimeNow = timeNow });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Xóa nhân viên đã được mapping với công đoạn đó
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="timeNow"></param>
        /// <returns></returns>
        public async Task<int> removeStaffMapping(string Id, DateTime timeNow)
        {
            try
            {
                var query = @" DELETE d_pro_unit_staff WHERE psid = @id and @TimeNow <= end_dt ";
                var result = await base.DbConnection.ExecuteAsync(query, new { @id = Id, @TimeNow = timeNow });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Lấy ra tất cả danh sách các nhân viên đang làm việc ở trong nhà máy hay xí nghiệp
        /// </summary>
        /// <param name="mtcd"></param>
        /// <param name="useyn"></param>
        /// <returns></returns>
        public async Task<IEnumerable<comm_dt>> GetStaff(string mtcd, string useyn)
        {
            try
            {
                var query = "select * from comm_dt Where mt_cd = @Mt_Cd AND use_yn = @Use_Yn ORDER BY dt_cd DESC ";
                var result = await base.DbConnection.QueryAsync<comm_dt>(query, new { @Mt_Cd = mtcd, @Use_Yn = useyn });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Hiển thị ra danh sách nhân viên đã được mapping với công đoan đó
        /// </summary>
        /// <param name="Id_Actual"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcessUnitStaff>> GetListProcessStaff(int Id_Actual)
        {
            try
            {
                var query = @"SELECT a.psid,a.staff_id,c.uname,CONVERT(varchar,a.start_dt,120) start_dt ,CONVERT(varchar,a.end_dt,120) end_dt ,a.use_yn, b.dt_nm AS staff_tp_nm  
                            FROM d_pro_unit_staff a 
				            JOIN mb_info c ON c.userid = a.staff_id 
				            Left JOIN comm_dt b ON  b.dt_cd = a.staff_tp AND b.mt_cd = 'COM013' AND b.use_yn = 'Y' 
                            where a.id_actual = @Id And a.active = 1
	                        Order by a.reg_dt Desc";
                var result = await base.DbConnection.QueryAsync<ProcessUnitStaff>(query, new { @Id = Id_Actual });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<int> TotalRecordsSearchStaffpp(string userid, string username, string position_cd)
        {
            try
            {
                var query = " SELECT COUNT(*) FROM " +
                        " (Select cmmdt.dt_nm AS position_cd, " +
                            " a.userid, " +
                            " ROW_NUMBER() OVER(ORDER BY a.userid DESC) AS RowNum, " +
                            " a.uname, " +
                            " a.nick_name, " +
                            " ( SELECT Top 1 c.mc_no " +
                            " FROM d_pro_unit_staff AS b " +
                            " LEFT JOIN d_pro_unit_mc AS c ON b.id_actual = c.id_actual " +
                            " WHERE a.userid = b.staff_id AND c.mc_no IN(SELECT d.mc_no FROM d_machine_info AS d) " +
                            " ORDER BY c.chg_dt DESC) AS mc_no " +
                        " FROM mb_info AS a " +
                        " LEFT JOIN comm_dt cmmdt ON cmmdt.mt_cd = 'COM018' AND cmmdt.dt_cd = a.position_cd " +
                        " Where a.lct_cd = 'staff'  " +
                        " and  (@User_id = '' Or a.userid = @User_id ) AND (@User_Name = '' Or a.uname = @User_Name ) AND (@Position_cd = '' Or a.position_cd = @Position_cd )) as MyDerivedTable";
                int result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(query, new { @User_id = userid, @User_Name = username, @Position_cd = position_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<StaffPP>> GetListSearchStaffpp(string userid, string username, string position_cd, int intpage, int introw)
        {
            try
            {
                var query = @"SELECT *
                                FROM( 
                                    SELECT 
										  ROW_NUMBER() OVER(ORDER BY a.userid DESC) AS RowNum, 
										  cmmdt.dt_nm AS position_cd, 
                                           a.userid, 
                                           a.uname, 
                                           a.nick_name, 
                                           ( SELECT Top 1 c.mc_no FROM d_pro_unit_staff AS b 
                                           JOIN d_pro_unit_mc AS c ON b.id_actual = c.id_actual 
                                           WHERE a.userid = b.staff_id AND c.mc_no IN(SELECT d.mc_no FROM d_machine_info AS d)
                                           ORDER BY c.chg_dt DESC) AS mc_no 
                                        FROM mb_info AS a 
                                        JOIN comm_dt cmmdt ON cmmdt.mt_cd = 'COM018' AND cmmdt.dt_cd = a.position_cd 
                                        Where a.lct_cd = 'staff' And a.active = 1
                                        AND(@User_Id = '' OR @User_Id IS NULL Or a.userid like '%'+@User_Id+'%') 
                                        AND(@User_Name = '' OR @User_Name IS NULL Or a.uname like '%'+@User_Name+'%') 
                                        AND(@Position_CD = '' OR @Position_CD IS NULL Or a.position_cd like '%'+@Position_CD+'%') 
                                        ) as MyDerivedTable
                                        ORDER BY MyDerivedTable.userid Desc OFFSET @intpage ROWS FETCH NEXT @introw ROWS ONLY";

                var result = await base.DbConnection.QueryAsync<StaffPP>(query, new { @User_Id = userid, @User_Name = username, @Position_CD = position_cd, @intpage = intpage, @introw = introw });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }



        public async Task<int> CountListSearchStaffpp(string userid, string username, string position_cd)
        {
            try
            {
                var query = @"SELECT Count(MyDerivedTable.userid)
                                FROM( 
                                    SELECT 
										  ROW_NUMBER() OVER(ORDER BY a.userid DESC) AS RowNum, 
										  cmmdt.dt_nm AS position_cd, 
                                           a.userid, 
                                           a.uname, 
                                           a.nick_name, 
                                           ( SELECT Top 1 c.mc_no FROM d_pro_unit_staff AS b 
                                           JOIN d_pro_unit_mc AS c ON b.id_actual = c.id_actual 
                                           WHERE a.userid = b.staff_id AND c.mc_no IN(SELECT d.mc_no FROM d_machine_info AS d)
                                           ORDER BY c.chg_dt DESC) AS mc_no 
                                        FROM mb_info AS a 
                                        JOIN comm_dt cmmdt ON cmmdt.mt_cd = 'COM018' AND cmmdt.dt_cd = a.position_cd 
                                        Where a.lct_cd = 'staff' And a.active = 1
                                        AND(@User_Id = '' OR @User_Id IS NULL Or a.userid like '%'+@User_Id+'%') 
                                        AND(@User_Name = '' OR @User_Name IS NULL Or a.uname like '%'+@User_Name+'%') 
                                        AND(@Position_CD = '' OR @Position_CD IS NULL Or a.position_cd like '%'+@Position_CD+'%') 
                                        ) as MyDerivedTable";

                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @User_Id = userid, @User_Name = username, @Position_CD = position_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<string> GetStatusStaff(string staff_id, int id_actual, DateTime start, DateTime end)
        {
            try
            {
                string query = @"
                SELECT (
		            CASE
		            WHEN NOT  EXISTS(SELECT * from d_pro_unit_staff where staff_id= @Staff_Id)
		            THEN '0'
		            WHEN  EXISTS(SELECT * from d_pro_unit_staff WHERE staff_id = @Staff_Id AND id_actual = @Id_Actual AND start_dt <= @Start  AND @Start <= end_dt)
		            THEN '1'
		            WHEN  EXISTS(SELECT * from d_pro_unit_staff WHERE staff_id = @Staff_Id AND id_actual = @Id_Actual AND start_dt <= @End  AND @End <= end_dt)
		            THEN '1'
		            WHEN  EXISTS(SELECT * from d_pro_unit_staff WHERE staff_id = @Staff_Id AND start_dt <= @Start  AND @Start <= end_dt)
		            THEN 'SELECT psid from d_pro_unit_staff WHERE staff_id = @Staff_Id AND start_dt <= @Start  AND @Start <= end_dt'
		            WHEN EXISTS(SELECT * from d_pro_unit_staff WHERE staff_id = @Staff_Id AND start_dt <= @End  AND @End <= end_dt)
		            THEN 'SELECT psid from d_pro_unit_staff WHERE staff_id = @Staff_Id AND start_dt <= @End  AND @End <= end_dt'
                    ELSE '0'
	                END
                ) AS a";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<string>(query, new { @Staff_Id = staff_id, @Id_Actual = id_actual, @Start = start, @End = end });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> InsertUnitStaff(DProUnitStaff item)
        {
            try
            {
                string query = @"Insert into d_pro_unit_staff(staff_id,staff_tp,use_yn,del_yn,chg_dt,reg_dt,start_dt,end_dt,id_actual) 
                Values(@Staff_Id, @Staff_Tp, @Use_Yn, @Del_Yn, @Chg_Date, @Reg_Date, @Start_Date, @End_Date, @Id_Actual);
                SELECT SCOPE_IDENTITY()";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query,
                          new
                          {
                              @Staff_Id = item.staff_id,
                              @Staff_Tp = item.staff_tp,
                              @Use_Yn = item.use_yn,
                              @Del_Yn = item.del_yn,
                              @Chg_Date = item.chg_dt,
                              @Reg_Date = item.reg_dt,
                              @Start_Date = item.start_dt,
                              @End_Date = item.end_dt,
                              @Id_Actual = item.id_actual
                          });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<ProcessUnitStaff> GetStaffById(int psid)
        {
            try
            {
                var query = @"SELECT a.psid,a.staff_id,b.dt_nm AS staff_tp_nm,c.uname, a.start_dt ,a.end_dt,a.use_yn " +
                       " FROM d_pro_unit_staff a " +
                       " LEFT JOIN comm_dt b ON  b.dt_cd=a.staff_tp AND b.mt_cd='COM013' AND b.use_yn='Y' " +
                       " LEFT JOIN mb_info c ON c.userid=a.staff_id " +
                       " where a.psid= @Id ";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<ProcessUnitStaff>(query, new { @Id = psid });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public async Task<IEnumerable<d_pro_unit_staff>> checkStaffInActual(int id_actual)
        {
            try
            {
                string query = "Select * from d_pro_unit_staff where id_actual = @Id";
                var result = await base.DbConnection.QueryAsync<d_pro_unit_staff>(query, new { @Id = id_actual });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> CheckUnitStaff(string staffId)
        {
            try
            {
                string query = "Select * from mb_info where userid = @StaffId ";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(query, new { @StaffId = staffId });
                return result;

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> CheckProcessUnitStaff(int psid, string staff_id, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                string query = " SELECT COUNT(psid) FROM d_pro_unit_staff AS a " +
                                                                        " WHERE a.staff_id = @StaffId " +
                                                                        " AND a.start_dt  >= @Start " +
                                                                        " AND a.end_dt <= @End " +
                                                                        " AND a.psid != @Psid ";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(query, new { @StaffId = staff_id, @Start = StartDate.Date, @End = EndDate.Date, @Psid = psid });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<d_pro_unit_staff> GetUnitStaffById(int psid)
        {
            try
            {
                string query = "Select * from d_pro_unit_staff where psid = @Id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<d_pro_unit_staff>(query, new { @Id = psid });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UpdateUnitStaff(d_pro_unit_staff item)
        {
            try
            {
                var sql = @"UPDATE d_pro_unit_staff SET staff_id=@Staff_id, start_dt=@StartDate, end_dt=@EndDate, use_yn=@Use_YN, del_yn=@Del_YN,
                            reg_id=@RegId, reg_dt=@RegDate, chg_id=@ChangeId, chg_dt=@ChangeDate
                            WHERE psid=@Id ";
                var result = await base.DbConnection.ExecuteAsync(sql,
                    new
                    {
                        @Staff_id = item.staff_id,
                        @StartDate = item.start_dt,
                        @EndDate = item.end_dt,
                        @Use_YN = item.use_yn,
                        @Del_YN = item.del_yn,
                        @RegId = item.reg_id,
                        @RegDate = item.reg_dt,
                        @ChangeId = item.chg_id,
                        @ChangeDate = item.chg_dt,
                        @Id = item.psid
                    });

                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<comm_dt>> GetMachineType()
        {
            try
            {
                var query = @"Select * from comm_dt where mt_cd = 'COM007' And active = 1 ";
                var result = await base.DbConnection.QueryAsync<comm_dt>(query);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> CheckNewWorkerAndMachine(int id_actual)
        {
            try
            {
                var queryWorker = " Select Count(*) from d_pro_unit_staff where id_actual = @Id ";
                var queryMachine = " Select Count(*) from d_pro_unit_mc where id_actual = @Id ";

                var checkWorker = await base.DbConnection.QuerySingleOrDefaultAsync<int>(queryWorker, new { @Id = id_actual });
                var checkMachine = await base.DbConnection.QuerySingleOrDefaultAsync<int>(queryMachine, new { @Id = id_actual });

                if (checkWorker == 0 && checkMachine == 0)
                {
                    return 1;
                }
                else if (checkWorker > 0 && checkMachine == 0)
                {
                    return 2;
                }
                else if (checkWorker == 0 && checkMachine > 0)
                {
                    return 3;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<BobbinResponse>> GetListBobbin(string bb_no, string bb_nm, string mt_cd, int? id_actual, int page, int row)
        {
            try
            {
                var query = @"SELECT b.blno,b.mc_type,b.bb_no,b.mt_cd,b.bb_nm,b.use_yn,('')purpose,('')barcode, 
                            ('')re_mark,(0)count_number,('N')del_yn,b.reg_id,b.reg_dt,b.chg_id,b.chg_dt 
                            FROM d_bobbin_lct_hist AS b 
                            JOIN w_material_info_mms AS c ON b.mt_cd=c.material_code 
                            JOIN w_actual AS d ON c.id_actual=d.id_actual 
                            WHERE c.id_actual!= @Id_Actual and c.location_code LIKE '002%' And c.gr_qty> 0 
                            and d.at_no=(select Top 1 at_no from w_actual where id_actual = @Id_Actual)
                            and (@Bb_No='' OR @Bb_No IS NULL OR b.bb_no LIKE '%'+@Bb_No+'%')
                            and (@Bb_Nm='' OR @Bb_Nm IS NULL OR b.bb_nm LIKE '%'+@Bb_Nm+'%')
                            and (@Mt_Cd='' OR @Mt_Cd IS NULL OR b.mt_cd LIKE '%'+@Mt_Cd+'%')
                            UNION 
                            SELECT a.bno,a.mc_type,a.bb_no,a.mt_cd,a.bb_nm,a.use_yn,a.purpose,a.barcode, 
                            a.re_mark,a.count_number,a.del_yn,a.reg_id,a.reg_dt,a.chg_id,a.chg_dt FROM d_bobbin_info AS a 
                            WHERE (a.mt_cd = ''  OR a.mt_cd IS NULL)
                           -- and (@Mt_Cd='' OR @Mt_Cd IS NULL or a.mt_cd LIKE '%'+@Mt_Cd+'%')
                            and  (@Bb_No='' OR @Bb_No IS NULL OR a.bb_no LIKE '%'+@Bb_No+'%')
                            and  (@Bb_Nm='' OR @Bb_Nm IS NULL OR a.bb_nm LIKE '%'+@Bb_Nm+'%') 
                            Order By reg_dt Desc
                            OFFSET @intpage ROWS FETCH NEXT @introw ROWS ONLY";

                var result = await base.DbConnection.QueryAsync<BobbinResponse>(query, new { @Bb_No = bb_no, @Bb_Nm = bb_nm, @Mt_Cd = mt_cd, @Id_Actual = id_actual, @intpage = page, @introw = row });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }



        public async Task<int> CountListBobbin(string bb_no, string bb_nm, string mt_cd, int? id_actual)
        {
            try
            {
                var query = @"
                            SELECT Count(a.bno) 
                            FROM d_bobbin_info AS a 
                            WHERE (a.mt_cd=''  OR a.mt_cd IS NULL)
                            and (@Mt_Cd='' OR @Mt_Cd IS NULL or a.mt_cd LIKE '%'+@Mt_Cd+'%')
                            and  (@Bb_No='' OR @Bb_No IS NULL OR a.bb_no LIKE '%'+@Bb_No+'%')
                            and  (@Bb_Nm='' OR @Bb_Nm IS NULL OR a.bb_nm LIKE '%'+@Bb_Nm+'%')";

                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Bb_No = bb_no, @Bb_Nm = bb_nm, @Mt_Cd = mt_cd, @Id_Actual = id_actual });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> CheckStaffShift(int id_actual)
        {
            try
            {
                string query = "SELECT Count(a.psid) FROM d_pro_unit_staff AS a WHERE a.id_actual = @Id AND (GETDATE() BETWEEN a.start_dt AND a.end_dt)";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(query, new { @Id = id_actual });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> CheckMachineShift(int id_actual)
        {
            try
            {
                string query = "SELECT Count(a.pmid) FROM d_pro_unit_mc AS a WHERE a.id_actual = @Id AND (GETDATE() BETWEEN a.start_dt AND a.end_dt)";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(query, new { @Id = id_actual });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> CountDataMaterialInfo(string mt_cd)
        {
            try
            {
                string query = " SELECT COUNT(wmtid) FROM  w_material_info_mms  WHERE material_code = @Material_Code";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Material_Code = mt_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<string>> GetStaffOnProcess(int id_actual)
        {
            try
            {
                string query = "SELECT staff_id FROM d_pro_unit_staff WHERE id_actual = @Id AND (GETDATE() BETWEEN start_dt AND end_dt)";
                var result = await base.DbConnection.QueryAsync<string>(query, new { @Id = id_actual });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<string>> GetMachineOnProcess(int id_actual)
        {
            try
            {
                string query = "SELECT mc_no FROM d_pro_unit_mc WHERE id_actual = @Id AND (GETDATE() BETWEEN start_dt AND end_dt)";
                var result = await base.DbConnection.QueryAsync<string>(query, new { @Id = id_actual });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> InsertMaterialInfoMMS(MaterialInfoMMS item)
        {
            try
            {
                var query = @"Insert Into w_material_info_mms (id_actual, material_code, material_type, gr_qty, real_qty, status, mt_no, bb_no, location_code,
                                                                    from_lct_code, to_lct_code, receipt_date, reg_date, reg_id, chg_date, chg_id)
                                    Values (@id_actual, @material_code, @material_type, @gr_qty, @real_qty, '002', @mt_no, @bb_no, @location_code, 
                                            @from_lct_code, @to_lct_code, @receipt_date, @reg_date, @reg_id, @chg_date, @chg_id);
                                    Select Scope_Identity()";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<int> checkFullMtCode(string item)
        {
            try
            {
                var query = @"select count(*) from w_material_info_mms where material_code = @item";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @item = item });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<int> InsertMaterialInfoMMSByModel(ChangeBobbinForMaterialChildrenResponse item)
        {
            try
            {
                var query = @"Insert Into w_material_info_mms (id_actual, material_code, gr_qty, status, mt_no, bb_no, reg_date, reg_id, chg_date, chg_id)
                                    Values (@id_actual, @material_code, @gr_qty, '002', @mt_no, @bb_no, @reg_date, @reg_id, @chg_date, @chg_id);
                                    Select Scope_Identity()";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<Models.NewVersion.MaterialInfoMMS> GetDetailMaterialInfoMMS(int Id)
        {
            try
            {
                string query = "SELECT * FROM w_material_info_mms WHERE wmtid = @id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<Models.NewVersion.MaterialInfoMMS>(query, new { @id = Id });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<InventoryProduct> GetDetailInventoryProduct(int Id)
        {
            try
            {
                string query = "SELECT * FROM inventory_products WHERE materialid = @id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<InventoryProduct>(query, new { @id = Id });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateBobbinMTCode(BobbinInfo item)
        {
            try
            {
                string query = @"UPDATE d_bobbin_info SET mc_type = @mc_type, bb_no = @bb_no, mt_cd = @mt_cd, bb_nm = @bb_nm, purpose = @purpose, barcode = @barcode, re_mark = @re_mark,
                                    use_yn = @use_yn, count_number = @count_number, del_yn = @del_yn, reg_id = @reg_id, reg_dt = @reg_dt ,chg_id = @chg_id, chg_dt = @chg_dt    
                                    WHERE bno=@bno";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> InsertToBobbinHistory(BobbinLctHist item)
        {
            try
            {
                string query = @"INSERT INTO d_bobbin_lct_hist(mc_type, bb_no, mt_cd, bb_nm, use_yn, del_yn, reg_id,reg_dt,chg_id,chg_dt)
                               VALUES(@mc_type, @bb_no ,@mt_cd ,@bb_nm, @use_yn, @del_yn, @reg_id, @reg_dt, @chg_id, @chg_dt);
                               SELECT SCOPE_IDENTITY()";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<string> GetMaterialByIdActual(int idActual, string FullMtCode)
        {
            try
            {
                string query = @"SELECT top 1 minfo.material_code from w_material_info_mms minfo WHERE minfo.id_actual = @Id And  minfo.material_type ='CMT' and minfo.material_code <> @materialcode  
                 order by   minfo.reg_date DESC ";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<string>(query,
                    new { @Id = idActual, @materialcode = FullMtCode
                    });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<string> CheckBobbinInheritance(int idActual)
        {
            try
            {
                string query = $"SELECT Top 1 mmapp.mt_lot FROM w_material_mapping_mms mmapp Where mmapp.mt_lot in ({idActual})   ORDER BY mmapp.reg_date DESC";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<string>(query/*, new { @Id = idActual }*/);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<string> CheckBobbinInheritance1(string idActual)
        {
            try
            {
                string query = $"SELECT Top 1 mmapp.mt_lot FROM w_material_mapping_mms mmapp Where mmapp.mt_lot in ('{idActual}')   ORDER BY mmapp.reg_date DESC";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<string>(query/*, new { @Id = idActual }*/);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> InsertMaterialMapping(string mt_cd, string userid, string mt_lot)
        {
            try
            {
                string query = @"INSERT INTO w_material_mapping_mms(mt_lot,mt_no,mt_cd,mapping_dt,use_yn,chg_date,reg_id,chg_id,reg_date)
                    SELECT @Mt_Cd AS mt_lot,
                    mt_no,
                    mt_cd,
                    mapping_dt,
                    use_yn,
                    GETDATE() AS chg_date,
                    @UserId AS reg_id,
                    @UserId AS chg_id,
                    GETDATE()  AS reg_date
                    FROM w_material_mapping_mms WHERE mt_lot=@Mt_Lot AND use_yn='Y'";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Mt_Cd = mt_cd, @UserId = userid, @Mt_Lot = mt_lot });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UpdateMaterialInfoFromMappingOtherType(string userid, string mt_type, string mt_lot)
        {
            try
            {
                string query = $"UPDATE w_material_info_mms SET chg_id = '{userid}' WHERE /*material_type != @Mt_Type AND*/ material_code IN (SELECT mt_cd FROM w_material_mapping_mms WHERE mt_lot = '{mt_lot}' AND use_yn='Y')";
                var result = await base.DbConnection.ExecuteAsync(query/*, new { @UserId = userid, @Mt_Type = mt_type, @Mt_Lot = mt_lot }*/);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateMaterialInfoFromMappingByType(string userid, string mt_type, string mt_lot)
        {
            try
            {
                string query = @"UPDATE w_material_info_mms SET chg_id = @UserId WHERE material_type = @Mt_Type AND material_code IN (SELECT mt_cd FROM w_material_mapping_mms WHERE mt_lot = @Mt_Lot AND use_yn='Y')";
                var result = await base.DbConnection.ExecuteAsync(query, new { @UserId = userid, @Mt_Type = mt_type, @Mt_Lot = mt_lot });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<w_material_info_wo> GetMaterialInfoWo(string mt_no, string mt_cd, int idWMaterialInfo)
        {
            try
            {
                var query = @"SELECT m.wmtid,
                                    m.reg_date as reg_dt,
                                    m.material_code As mt_cd,
                                    @MT_NO AS mt_no,
                                   -- tmpb.dt_nm As bbmp_sts_cd,
                                    m.gr_qty,
                                    m.material_code As mt_qrcode,
                                    tmpc.lct_nm AS lct_cd,
                                    m.bb_no,
                                    m.material_code As mt_barcode,
                                    ISNULL(tmpa.count_table2,0) As count_table2
                            FROM w_material_info_mms As m 
					                LEFT JOIN (
						                select COUNT(wmmid) count_table2, mt_lot 
						                from w_material_mapping_mms
						                WHERE mt_lot = @MT_CD
						                GROUP BY mt_lot
					                ) tmpa ON tmpa.mt_lot=m.material_code
					               -- INNER JOIN(
						              --  Select dt_nm,dt_cd FROM comm_dt where mt_cd='MMS007'
					               -- ) tmpb ON tmpb.dt_cd=m.bbmp_sts_cd
                                    LEFT JOIN (SELECT lct_cd,lct_nm FROM lct_info) tmpc ON m.location_code = tmpc.lct_cd
                            Where m.wmtid = @IdMaterialInfo";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<w_material_info_wo>(query, new { @MT_NO = mt_no, @MT_CD = mt_cd, @IdMaterialInfo = idWMaterialInfo });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> GetSumGroupQty(string material_code)
        {
            try
            {
                string query = $"SELECT SUM(gr_qty) FROM w_material_info_mms WHERE material_code like '%{material_code}%' ";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(query/*, new { @MT_CD = material_code }*/);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<InventoryProduct>> PopupCompositeMaterial(string mt_cd)
        {
            try
            {
                //And('@Mt_Cd' = '' OR @Mt_Cd IS NULL OR  material_code like '%' + @Mt_Cd + '%')
                var query = @"Select * from inventory_products  
				            where status = '001' And gr_qty > 0  And location_code Like'002%' And ExportCode != '' And LoctionMachine = 'Machine' And location_code != '' 
                            And(@Mt_Cd = '' OR @Mt_Cd IS NULL OR  material_code like '%' + @Mt_Cd + '%')";

                var result = await base.DbConnection.QueryAsync<InventoryProduct>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<string> CheckMaxBobbin(string mt_cd, int id_actual)
        {
            try
            {
                var query = @"SELECT a.bb_no
			                FROM w_material_info_mms AS a
			                WHERE a.material_code = @Mt_Cd AND  a.wmtid = (
												                            SELECT MAX(b.wmtid)
												                            FROM w_material_info_mms AS b
												                            WHERE
												                            b.id_actual = @Id_Actual
												                            AND b.material_type = 'CMT'
                                                                            AND b.orgin_mt_cd IS NULL)";
                var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @Mt_Cd = mt_cd, @Id_Actual = id_actual });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<string> CheckBobbinInventoryProduct(string mt_cd, int id_actual)
        {
            try
            {
                var query = @"SELECT a.bb_no
			    FROM w_material_info_mms AS a
			    WHERE  a.material_code = @Mt_Cd AND  a.materialid = (
												                SELECT MAX(b.materialid)
												                FROM inventory_products AS b
												                WHERE
												                b.id_actual = @Id_Actual
												                AND b.mt_type != 'CMT')";
                var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @Mt_Cd = mt_cd, @Id_Actual = id_actual });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<MaterialInfoMMS> GetWMaterialInfoByMaterialCode(string mt_cd)
        {
            try
            {
                var query = @"Select * from w_material_info_mms where material_code = @Mt_Cd";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoMMS>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<int> GetTotalDevideWMaterialInfoByMaterialCode(string mt_cd)
        {
            try
            {
                var query = @"Select count(*) from w_material_info_mms where material_code like @Mt_Cd+'%'";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<string> GetBBNOWMaterialInfoByMaterialCodeMMS(string mt_cd)
        {
            try
            {
                var query = @"Select bb_no from w_material_info_mms where material_code = @Mt_Cd";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<string>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<IReadOnlyList<MaterialInfoMMS>> GetWMaterialInfoByMaterialCd(string mt_cd)
        {
            try
            {
                var query = @"Select * from w_material_info_mms where material_code like @Mt_Cd+ '%'";
                var result = await base.DbConnection.QueryAsync<MaterialInfoMMS>(query, new { @Mt_Cd = mt_cd });
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

         public async Task<IReadOnlyList<MaterialInfoMMS>> GetTIMSInfoByMaterialCd(string mt_cd)
        {
            try
            {
                //var query = @"Select * from w_material_info_tims where material_code = @Mt_Cd";
                var query = @"Select * from w_material_info_tims where material_code like @Mt_Cd+'%'";
                var result = await base.DbConnection.QueryAsync<MaterialInfoMMS>(query, new { @Mt_Cd = mt_cd });
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<int> GetWMTIDWMaterialInfoByMaterialCodeMMS(string mt_cd)
        {
            try
            {
                var query = @"Select wmtid from w_material_info_mms where material_code = @Mt_Cd";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<string> GetBBNOWMaterialInfoByMaterialCodeTIMS(string mt_cd)
        {
            try
            {
                var query = @"Select bb_no from w_material_info_tims where material_code = @Mt_Cd";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<string>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<int> GetWMTIDWMaterialInfoByMaterialCodeTIMS(string mt_cd)
        {
            try
            {
                var query = @"Select wmtid from w_material_info_tims where material_code = @Mt_Cd";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<MaterialInfoMMS> GetWMaterialInfoByMaterialCodeTIMS(string mt_cd)
        {
            try
            {
                var query = @"Select top 1 * from w_material_info_tims where material_code = @Mt_Cd";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoMMS>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> GetTotalWMaterialInfoByMaterialCodeTIMS(string mt_cd)
        {
            try
            {
                var query = @"Select count(*) from w_material_info_tims where material_code like @Mt_Cd+'%'";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<int> GetTotalWMaterialInfoByMaterialCodeMGTIMS(string mt_cd)
        {
            try
            {
                var query = @"Select count(*) from w_material_info_tims where material_code like @Mt_Cd+'%' and material_code like '%MG%'";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<int> GetTotalWMaterialInfoByMaterialCodeMGMMS(string mt_cd)
        {
            try
            {
                var query = @"Select count(*) from w_material_info_mms where material_code like @Mt_Cd+'%' and material_code like '%MG%'";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<InventoryProduct> GetMaterialByInventoryProduct(string mt_cd)

        {
            try
            {
                var query = @"Select * From inventory_products where material_code = @Mt_Cd";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<InventoryProduct>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<InventoryProduct> GetMaterialFromInventoryProducts(string mt_cd)
        {
            try
            {
                //var query = @"Select * From inventory_products where status = '002' And gr_qty > 0 And material_code = @Mt_Cd";
                var query = @"Select materialid, material_code, ExportCode, LoctionMachine, mt_type As material_type, mt_no, status, location_code From inventory_products where /*status = '001' And*/ gr_qty > 0 And material_code = @Mt_Cd";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<InventoryProduct>(query, new { @Mt_Cd = mt_cd.Trim() });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> CheckMaterialMapping(string mt_cd)
        {
            try
            {
                var query = @"SELECT COUNT(*) FROM w_material_mapping_mms WHERE mt_cd = @Mt_Cd";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> CountInventoryProduct(string mt_cd)
        {
            try
            {
                var query = @"SELECT COUNT(*) FROM inventory_products WHERE material_code = @Mt_Cd";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<int> CheckMaterialToMapping(string mt_cd)
        {
            try
            {
                var query = @"SELECT COUNT(*) FROM w_material_mapping_mms WHERE mt_lot = @Mt_Cd";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<BobbinInfo> GetBobbinInfo(string bb_no)
        {
            try
            {
                var query = @"SELECT * FROM d_bobbin_info WHERE bb_no = @BB_NO";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<BobbinInfo>(query, new { @BB_NO = bb_no });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<BobbinLctHist> GetBobbinLctHist(string bb_no)
        {
            try
            {
                var query = @"SELECT * FROM d_bobbin_lct_hist WHERE bb_no = @Bb_No";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<BobbinLctHist>(query, new { @Bb_No = bb_no });
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
                string query = @"UPDATE d_bobbin_info SET mc_type = @mc_type, bb_no = @bb_no, mt_cd = @mt_cd, bb_nm = @bb_nm, purpose = @purpose ,barcode = @barcode, re_mark = @re_mark,
                                    use_yn = @use_yn, count_number = @count_number, del_yn = @del_yn, reg_id = @reg_id, reg_dt = @reg_dt, chg_id = @chg_id ,chg_dt = @chg_dt    
                                    WHERE bno = @bno";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<string> IsCheckApply(string at_no)
        {
            try
            {
                var query = "SELECT isapply FROM w_actual_primary WHERE at_no = @At_no and isapply = 'Y'";
                var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @At_no = at_no });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }



        public async Task<IEnumerable<CommCode>> CheckStatus(string mt_cd, string status)
        {
            try
            {
                var query = "Select * from comm_dt where mt_cd = @Mt_Cd and dt_cd = @Status";
                var result = await base.DbConnection.QueryAsync<CommCode>(query, new { @Mt_Cd = mt_cd, @Status = status });
                return result.ToList();
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> GetCountMaterialMapping(string mt_cd, string mt_lot)
        {
            try
            {
                var query = @"SELECT COUNT(*) FROM w_material_mapping_mms WHERE mt_cd=@Mt_Cd AND mt_lot != @Mt_Lot";
                //var query = @"SELECT COUNT(*) FROM w_material_mapping_mms WHERE mt_cd IN 
                //                (Select material_code From inventory_products Where material_code = @Mt_Cd And status = '002') AND mt_lot != @Mt_Lot";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Mt_Cd = mt_cd, @Mt_Lot = mt_lot });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<string> GetStaffFromActual(int id_actual)
        {
            try
            {
                var query = @"Select staff_id from d_pro_unit_staff where id_actual in (Select id_actual from w_material_info_mms where id_actual = @Id)";
                var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @Id = id_actual });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<string>> GetListStaffFromActual(int id_actual)
        {
            try
            {
                var query = @"Select staff_id from d_pro_unit_staff where id_actual in (Select id_actual from w_material_info_mms where id_actual = @Id)";
                var result = await base.DbConnection.QueryAsync<string>(query, new { @Id = id_actual });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<string>> GetStaffFromActualByInventoryProduct(int id_actual)
        {
            try
            {
                var query = @"Select staff_id from d_pro_unit_staff where id_actual in (Select id_actual from inventory_products where id_actual = @Id)";
                var result = await base.DbConnection.QueryAsync<string>(query, new { @Id = id_actual });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<int> CheckShiftOfStaff(int id_actual, IEnumerable<string> staff_id)
        {
            try
            {
                var result = 0;
                foreach (var item in staff_id)
                {
                    var query = @"SELECT COUNT(*) FROM d_pro_unit_staff AS a WHERE a.id_actual = @Actual AND a.staff_id = @Staff AND (GETDATE() BETWEEN a.start_dt AND a.end_dt)";
                    result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Actual = id_actual, @Staff = item });

                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> CheckHetCaMT(int id_actual, string mt_cd)
        {
            try
            {
                var query = @"Select Count(*) From d_pro_unit_staff a  
                            Join w_material_info_mms b on a.id_actual = b.id_actual And(b.reg_date between a.start_dt and a.end_dt)
                            where a.id_actual = @Id_Actual And b.material_code = @Mt_Cd And (GETDATE() between a.start_dt and a.end_dt)";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(query, new { @Id_Actual = id_actual, @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> GetMaterialMapping(string mt_no, string mt_cd, string mt_lot)
        {
            try
            {
                string query = @"SELECT COUNT(*) FROM w_material_mapping_mms WHERE mt_no = @Mt_No AND mt_cd = @Mt_Cd AND mt_lot = @Mt_Lot";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Mt_No = mt_no, @Mt_Cd = mt_cd, @Mt_Lot = mt_lot });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> InsertMaterialMapping(MaterialMappingMMS item)
        {
            try
            {
                var query = @"INSERT INTO w_material_mapping_mms(mt_lot, mt_no, mt_cd, mapping_dt, use_yn, chg_date, reg_id, chg_id, reg_date, del_yn) 
                            VALUES( @mt_lot, @mt_no, @mt_cd, @mapping_dt, @use_yn, @chg_date, @reg_id, @chg_id, @reg_date, @del_yn);
                            SELECT SCOPE_IDENTITY()";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<DProUnitStaff>> CheckStaff(int id_actual)
        {
            try
            {
                var query = @"SELECT a.staff_id FROM d_pro_unit_staff AS a WHERE a.id_actual = @Id AND (GetDate() BETWEEN a.start_dt AND a.end_dt)";
                var result = await base.DbConnection.QueryAsync<DProUnitStaff>(query, new { @Id = id_actual });
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<DProUnitMachine>> CheckMachine(int id_actual)
        {
            try
            {
                var query = @"Select mc_no from d_machine_info where mc_no IN(Select mc_no from d_pro_unit_mc where id_actual = @Id And(GetDate() BETWEEN start_dt AND end_dt))";
                var result = await base.DbConnection.QueryAsync<DProUnitMachine>(query, new { @Id = id_actual });
                return result.ToList();
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UpdateMaterialInfoMMS(MaterialInfoMMS item)
        {
            try
            {
                var query = @"Update w_material_info_mms SET id_actual = @id_actual, material_type = @material_type, material_code = @material_code,
                            gr_qty = @gr_qty, real_qty = @real_qty, location_code = @location_code, from_lct_code = @from_lct_code ,to_lct_code = @to_lct_code,
                            status = @status, mt_no = @mt_no, bb_no = @bb_no,  receipt_date = @receipt_date,
                            chg_id = @chg_id, chg_date = GETDATE(), number_divide = @number_divide, orgin_mt_cd = @orgin_mt_cd
                            WHERE wmtid = @wmtid";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<int> UpdatemtcdforBobbinInfoForRedo(string bbno, string mtcd)
        {
            try
            {
                var query = @"UPDATE d_bobbin_info SET mt_cd=@mtcd  WHERE bb_no = @bbno";
                var result = await base.DbConnection.ExecuteAsync(query, new { bbno = bbno, mtcd = mtcd });
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<int> InsertBobbinHist(DBobbinLctHist bobbinhist)
        {
            string sql = @"insert into 
                                d_bobbin_lct_hist(bb_no,mt_cd,reg_dt,chg_dt,chg_id,reg_id,use_yn,del_yn) 
                                values(@bbno,@mtcd,GETDATE(),GETDATE(),@chgid,@regid,@useyn,@delyn);SELECT Scope_Identity(); ";
            return await base.DbConnection.ExecuteAsync(sql, new
            {
                bbno = bobbinhist.bb_no,
                mtcd = bobbinhist.mt_cd,
                chgid = bobbinhist.chg_id,
                regid = bobbinhist.reg_id,
                useyn = bobbinhist.use_yn,
                delyn = bobbinhist.del_yn
            });
            //return db.Database.SqlQuery<int>(QuerySQL,
            //	  new MySqlParameter("1", bobbinhist.bb_no),
            //	  new MySqlParameter("2", bobbinhist.mt_cd),
            //	  new MySqlParameter("3", bobbinhist.reg_dt),
            //	  new MySqlParameter("4", bobbinhist.chg_dt),
            //	  new MySqlParameter("5", bobbinhist.chg_id),
            //	  new MySqlParameter("6", bobbinhist.reg_id),
            //	  new MySqlParameter("7", bobbinhist.use_yn),
            //	  new MySqlParameter("8", bobbinhist.del_yn)).FirstOrDefault();
        }
        public async Task<DBobbinInfo> FindOneDBobbinInfo(string bb_no)
        {
            string sql = @"select bno,mc_type,bb_no,mt_cd,bb_nm,purpose,barcode,re_mark,
                           use_yn,del_yn,reg_id,reg_dt,chg_id,chg_dt 
                           from d_bobbin_info where bb_no = @bbno AND active=1";
            var result = await base.DbConnection.QueryFirstOrDefaultAsync<DBobbinInfo>(sql, new { bbno = bb_no });
            return result;
        }

        public async Task<int> InsertMaterialMappingReturn(int count, int quantity, int numberDV, string status, string chg_id, string mt_cd)
        {
            try
            {
                var query = @"INSERT INTO w_material_info_mms(id_actual, material_code, material_type, gr_qty, real_qty, number_divide, status, 
										                    mt_no, bb_no, location_code, location_number, from_lct_code, to_lct_code, receipt_date, reg_date, reg_id, chg_date, chg_id, orgin_mt_cd ) 

                            SELECT id_actual, CONCAT(material_code,'RT','-', @Count) As material_code, material_type, @Quantity, @Quantity, @NumberDV, @Status, 
                                                    mt_no, bb_no, location_code, location_number, from_lct_code, to_lct_code, receipt_date, GetDate(), @Chg_Id, GetDate(), @Chg_Id, @Mt_Cd
                            FROM
	                            w_material_info_mms 
                            WHERE
	                            material_code = @Mt_Cd";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Count = count, @Quantity = quantity, @NumberDV = numberDV, @Status = status, @Chg_Id = chg_id, @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateInventoryProduct(InventoryProduct item)
        {
            try
            {
                var query = @"Update inventory_products SET id_actual = @id_actual, material_code =  @material_code, recei_wip_date = @recei_wip_date, picking_date = @picking_date, sd_no = @sd_no, ex_no = @ex_no,
	                        lct_sts_cd = @lct_sts_cd, mt_no = @mt_no, mt_type = @mt_type, gr_qty = @gr_qty, real_qty = @real_qty, bb_no = @bb_no, orgin_mt_cd = @orgin_mt_cd, recei_date = @recei_date, expiry_date = @expiry_date, export_date = @export_date,
	                        date_of_receipt = @date_of_receipt, lot_no = @lot_no, from_lct_cd = @from_lct_cd, location_code = @location_code, status = @status, create_id = @create_id, create_date = @create_date, change_id = @change_id,
	                        change_date = @change_date, ExportCode = @ExportCode, LoctionMachine = @LoctionMachine
	                        Where materialid = @materialid";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<SaveReturn>> GetDataSaveReturn(int id)
        {
            try
            {//  a.wmmId = @ID
                var query = @"SELECT
		                            a.wmmId,
		                            a.mt_lot,
		                            a.mt_cd,
		                            a.mapping_dt,
		                            a.use_yn,
		                            a.reg_date,
		                            b.real_qty,
		                            b.gr_qty,
		                            b.mt_no,
		                            b.bb_no,
		                            (b.gr_qty) As Used,
		                            (b.real_qty - b.gr_qty) As Remain 
		                            FROM
			                            w_material_mapping_mms AS a
			                            JOIN inventory_products AS b ON a.mt_cd = b.material_code 
		                            Where a.wmmId = @ID
		
		                            UNION ALL

		                            SELECT
		                            a.wmmid,
		                            a.mt_lot,
		                            a.mt_cd,
		                            a.mapping_dt,
		                            a.use_yn,
		                            a.reg_date,
		                            b.real_qty,
                                    b.gr_qty,
		                            b.mt_no,
		                            b.bb_no,
		                            (b.gr_qty) As Used,
		                            (b.real_qty - b.gr_qty) As Remain 
		
		                            FROM
			                            w_material_mapping_mms AS a
			                            JOIN w_material_info_mms AS b ON a.mt_cd = b.material_code 
		                            Where a.wmmId = @ID

		                            ORDER BY
		                            a.use_yn DESC,
		                            a.reg_date DESC";
                var result = await base.DbConnection.QueryAsync<SaveReturn>(query, new { @ID = id });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<SaveReturn>> GetDataMappingMaterial(string mt_cd)
        {
            try
            {
                string query = @"SELECT
		a.wmmid,
		a.mt_lot,
		a.mt_cd,
        a.mt_no,
        a.mapping_dt,
		a.use_yn,
		a.reg_date,
        b.description,
		(CASE WHEN b.real_qty is null or b.real_qty=0 then c.real_qty else b.real_qty end) real_qty,
		(CASE WHEN b.gr_qty is null or b.gr_qty=0 then c.gr_qty else b.gr_qty end) gr_qty,
		(CASE WHEN b.mt_no is null or b.mt_no='' then c.mt_no else b.mt_no end) mt_no,
		(CASE WHEN b.bb_no is null or b.bb_no='' then c.bb_no else b.bb_no end) bb_no,
		--(CASE WHEN b.gr_qty is null or b.gr_qty=0 then c.gr_qty else b.gr_qty end) As Used,
        (CASE WHEN a.use_yn = 'N' THEN c.gr_qty ELSE 0 END )As Used,
		-- (CASE WHEN b.real_qty is null or b.real_qty=0 then c.real_qty - c.gr_qty else b.real_qty - b.gr_qty end) Remain
        (CASE WHEN a.use_yn = 'N' then c.real_qty - c.gr_qty else 0 end) Remain
		FROM
			w_material_mapping_mms AS a
			LEFT JOIN w_material_info_mms AS b ON a.mt_cd = b.material_code 
			LEFT  JOIN inventory_products AS c ON a.mt_cd = c.material_code 

		Where a.mt_lot = @MT_LOT
		ORDER BY
        a.use_yn DESC,
        a.mt_no DESC,
        mapping_dt DESC";
                //var query = @"
                //              SELECT
                //              a.wmmid,
                //              a.mt_lot,
                //              a.mt_cd,
                //                    a.mt_no,
                //                    a.mapping_dt,
                //              a.use_yn,
                //              a.reg_date,
                //              b.real_qty,
                //                    b.gr_qty,
                //              b.mt_no,
                //              b.bb_no,
                //              (b.gr_qty) As Used,
                //              (b.real_qty - b.gr_qty) As Remain 

                //              FROM
                //               w_material_mapping_mms AS a
                //               JOIN w_material_info_mms AS b ON a.mt_lot = b.material_code 
                //              Where a.mt_lot = @MT_LOT
                //              ORDER BY
                //                    a.use_yn DESC,
                //                    a.mt_no DESC,
                //                    mapping_dt DESC";
                var result = await base.DbConnection.QueryAsync<SaveReturn>(query, new { @MT_LOT = mt_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<MtDateWebWO>> GetMtDateWebs(int id_actual)
        {
            try
            {
                //var query = @"
                //                SELECT 
                //                     'PC00004A' AS QC,
                //                     a.id_actual,
                //                     a.staff_id,
                //                     a.reg_date As reg_dt,
                //                     a.wmtid,
                //                     a.date AS DATE ,
                //                     a.material_code As mt_cd,
                //                     a.mt_no,
                //                     a.gr_qty,
                //                     description,
                //                     -- b.dt_nm AS bbmp_sts_cd,
                //                     a.mt_qrcode,
                //                     a.bb_no,
                //                     a.chg_date ,
                //                     ISNULL(a.real_qty - d.sltrung, a.real_qty) sl_tru_ng,
                //                     ISNULL(c.count_table2,0) AS count_table2,
                //                     e.hetca het_ca
                //                     FROM
                //                     (
                //                      select 
                //                      id_actual,
                //                     (Select Top 1 staff_id From d_pro_unit_staff where id_actual IN (Select id_actual From w_material_info_mms where id_actual = @Id ) Order by id_actual Desc) As staff_id,
                //       reg_date , 
                //                      wmtid,
                //                      receipt_date AS date,
                //                      material_code,
                //                      mt_no,
                //                      gr_qty,
                //                      bb_no,
                //       material_code As mt_qrcode,
                //       chg_date AS chg_date,
                //                      -- bbmp_sts_cd,
                //                      description,
                //                      ISNULL( real_qty , 0 ) real_qty
                //                      from w_material_info_mms
                //                      WHERE id_actual = @Id  AND material_type = 'CMT' AND orgin_mt_cd IS NULL
                //                     ) a
                //            INNER JOIN (
                //                         SELECT c.wmtid,SUM(c.ca) As hetca
                //                         FROM(
                //        select DISTINCT b.wmtid,a.staff_id,(CASE WHEN (GetDate() BETWEEN a.start_dt And a.end_dt) THEN 1 ELSE 0 END) ca
                //        From d_pro_unit_staff a 
                //        INNER JOIN(select wmtid,reg_date, s.staff_id From w_material_info_mms m Join d_pro_unit_staff s On m.id_actual = s.id_actual where m.id_actual=@Id  AND m.material_type = 'CMT' AND m.orgin_mt_cd IS NULL) b 
                //       On b.staff_id = a.staff_id And b.reg_date Between a.start_dt And a.end_dt
                //        WHERE a.id_actual=@Id
                //                         ) c
                //                         GROUP BY c.wmtid
                //                     ) e ON e.wmtid = a.wmtid
                //                     -- LEFT JOIN (select dt_cd,dt_nm from comm_dt WHERE mt_cd = 'MMS007') b ON b.dt_cd = a.bbmp_sts_cd
                //                     LEFT JOIN (SELECT COUNT(mt_lot) AS count_table2,mt_lot FROM w_material_mapping_mms GROUP BY mt_lot) c ON c.mt_lot = a.material_code
                //                     LEFT JOIN (SELECT ( check_qty - ok_qty ) AS sltrung,ml_no  FROM m_facline_qc WHERE ml_tims IS NULL) d ON d.ml_no = a.material_code
                //                    --  Where a.bbmp_sts_cd IS NOT NULL
                //                     ORDER BY a.wmtid DESC";
                //var result = await base.DbConnection.QueryAsync<MtDateWebWO>(query, new { @Id = id_actual });
                //return result;

                var query = @"DROP TABLE IF EXISTS #bang1;
                                        DROP TABLE IF EXISTS #bang2;

                                        CREATE TABLE #bang1 (
                                                QC varchar(50),
                                                id_actual varchar(50),
                                                wmtid int  PRIMARY KEY ,
                                                mt_cd  VARCHAR(150),
                                                mt_no VARCHAR(50),
                                                gr_qty int ,
                                                bb_no VARCHAR(100),
                                                chg_dt DateTime,
                                                reg_dt DateTime,
                                                Description VARCHAR(100),
                                                het_ca VARCHAR(10),
	                                            sl_tru_ng int
                                                );
                                       INSERT INTO #bang1
                                                        SELECT 'PC00004A' AS QC,
                                                        a.id_actual,                               
                                                        a.wmtid,
                                                        a.material_code mt_cd,
                                                        a.mt_no,
                                                        a.gr_qty,
                                                        a.bb_no,
                                                        a.chg_dt AS chg_dt,
                                                        a.reg_date as reg_dt,
                                                        a.Description,
                                                        e.hetca het_ca,
                                                        ISNULL(a.real_qty - d.sltrung, a.real_qty) sl_tru_ng
                                                                            FROM
                                                                            (
	                                                                            select 
	                                                                            id_actual,
	                                                                            reg_date, 
	                                                                            wmtid,
	                                  
	                                                                            material_code,
	                                                                            mt_no,
	                                                                            gr_qty,
	                                                                            bb_no,
	                                                                            chg_date AS chg_dt,
	                                                                          real_qty,
                                                                                Description
	                                                                            from w_material_info_mms
	                                                                            WHERE id_actual = @Id AND material_type = 'CMT' AND orgin_mt_cd IS NULL
                                                                            ) a
			                                                                INNER JOIN (
                                                                                SELECT c.wmtid,SUM(c.ca) hetca
                                                                                FROM(
                                                                                select DISTINCT b.wmtid,a.staff_id,(CASE WHEN (GetDate() BETWEEN a.start_dt And a.end_dt) THEN 1 ELSE 0 END) ca
                                                                                from d_pro_unit_staff a
                                                                              INNER JOIN(select wmtid,reg_date, s.staff_id From w_material_info_mms m Join d_pro_unit_staff s On m.id_actual = s.id_actual where m.id_actual=@Id  AND m.material_type = 'CMT' AND m.orgin_mt_cd IS NULL) b 
							                                                        On b.staff_id = a.staff_id And b.reg_date Between a.start_dt And a.end_dt
							                                                         WHERE a.id_actual=@Id
                                  
                                                                                ) c
                                                                                GROUP BY c.wmtid
                                                                            ) e ON e.wmtid =a.wmtid
                                     
                                                                   LEFT JOIN (SELECT ( check_qty - ok_qty ) AS sltrung,ml_no  FROM m_facline_qc WHERE ml_tims IS NULL) d ON d.ml_no = a.material_code
                                        ;
                                         CREATE TABLE #bang2 (
   
	                                        count_table2 VARCHAR(150),
	                                          mt_lot  VARCHAR(150)
                                        );

                                        insert into #bang2
                                         SELECT COUNT(mapp.wmmid) AS count_table2, mapp.mt_lot mt_lot
                                                                 FROM w_material_mapping_mms AS mapp
                                                                 JOIN w_material_info_mms AS winfo ON mapp.mt_lot = winfo.material_code
                                                                WHERE  winfo.id_actual = @Id AND winfo.material_type = 'CMT' AND winfo.orgin_mt_cd IS NULL GROUP BY mapp.mt_lot ;

                                        select   #bang1.*, #bang2.*
                                        from #bang1 
                                        left join #bang2 on #bang1.mt_cd = #bang2.mt_lot
                                        ";
                var result = await base.DbConnection.QueryAsync<MtDateWebWO>(query, new { @Id = id_actual });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> StatusMapping(string mt_cd)
        {
            try
            {
                var query = @"SELECT COUNT(*) FROM w_material_mapping_mms WHERE mt_lot = @MT_CD AND use_yn='N'";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @MT_CD = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<MaterialMappingMMS>> GetMaterialMappingByMaterialCode(string mt_cd)
        {
            try
            {
                var query = @"SELECT * FROM w_material_mapping_mms WHERE mt_lot = @MT_CD";
                var result = await base.DbConnection.QueryAsync<MaterialMappingMMS>(query, new { @MT_CD = mt_cd });
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<MaterialMappingMMS> GetMaterialMappingById(int wmmid)
        {
            try
            {
                var query = @"SELECT * FROM w_material_mapping_mms WHERE wmmid = @Id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialMappingMMS>(query, new { @Id = wmmid });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<MaterialInfoMMS> GetMaterialInfoforBobbin(string mt_cd, string mt_type)
        {
            try
            {
                var query = @"SELECT * FROM w_material_info_mms WHERE material_code = @MT_CD AND material_type != @MT_Type";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoMMS>(query, new { @MT_CD = mt_cd, @MT_Type = mt_type });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<InventoryProduct> GetMaterialInventoryProductforBobbin(string mt_cd, string mt_type)
        {
            try
            {
                var query = @"	SELECT * FROM inventory_products WHERE material_code = @MT_CD AND mt_type != @MT_Type";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<InventoryProduct>(query, new { @MT_CD = mt_cd, @MT_Type = mt_type });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> DeleteMaterialMapping(int id)
        {
            try
            {
                var query = @"DELETE FROM w_material_mapping_mms WHERE wmmId = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Id = id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<string> GetBBNoFromMaterialMMS(string mt_cd, string mt_no)
        {
            try
            {
                var query = @"Select bb_no from w_material_info_mms where material_code In (Select mt_cd from w_material_mapping_mms where mt_cd = @Mt_Cd and mt_no = @Mt_No)";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<string>(query, new { @Mt_Cd = mt_cd, @Mt_No = mt_no });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public async Task<BobbinLctHist> GetBobbinLctHistory(string bb_no, string mt_cd)
        {
            try
            {
                var query = @"Select * from d_bobbin_lct_hist where (@Bb_No = '' OR @Bb_No IS NULL OR bb_no = @Bb_No) 
																	And (@Mt_Cd = '' OR @Mt_Cd IS NULL OR mt_cd = @Mt_Cd)";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<BobbinLctHist>(query, new { @Bb_No = bb_no, @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> DeleteBobbinLctHist(int blno)
        {
            try
            {
                var query = @"DELETE FROM d_bobbin_lct_hist WHERE blno = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Id = blno });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> DeleteMaterialInfoMMs(int wmtid)
        {
            try
            {
                var query = @"DELETE FROM w_material_info_mms WHERE wmtid = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Id = wmtid });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<CommCode>> GetTypeMaterial()
        {
            try
            {
                var query = @"Select * from comm_dt where mt_cd = 'COM004'";
                var result = await base.DbConnection.QueryAsync<CommCode>(query);
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<MbInfo> GetMbInfoGrade(string uname)
        {
            try
            {
                var query = @"SELECT * FROM mb_info WHERE userid=@Name ";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<MbInfo>(query, new { @Name = uname });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> CheckFaclineqc(string mt_cd)
        {
            try
            {
                var query = @"SELECT COUNT(*) FROM m_facline_qc WHERE ml_no = @ML_NO";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @ML_NO = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<CheckUpdateGrty>> GetUpdateGrty(int wmtid)
        {
            try
            {
                var query = @"SELECT a.wmtid,
                                    a.material_code As mt_cd,
                                    a.material_code As mt_barcode,
                                    a.mt_no,
                                    a.gr_qty,
                                    a.bb_no,
                                    a.chg_date,
                                    a.reg_date As date,
                                    ISNULL(b.gr_qty,0) AS sl_tru_ng,
                                    ISNULL(c.count_table2,0) AS count_table2
                                    FROM w_material_info_mms a
                                    LEFT JOIN (SELECT SUM(gr_qty) AS gr_qty,orgin_mt_cd,material_code FROM w_material_info_mms GROUP BY orgin_mt_cd,material_code) b ON b.orgin_mt_cd=a.material_code AND b.material_code LIKE CONCAT('%',a.material_code,'-RT','%')
                                    LEFT JOIN(SELECT count(*) AS count_table2,mt_lot FROM w_material_mapping_mms GROUP BY(mt_lot)) c ON c.mt_lot=a.material_code
                                    WHERE a.wmtid = @Id 
                                    ORDER BY a.reg_date DESC";
                var result = await base.DbConnection.QueryAsync<CheckUpdateGrty>(query, new { @Id = wmtid });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<MaterialToDeviceResponse>> GetMaterialToDevice(int id_actual)
        {
            try
            {
                /*And m.result > 0*/
                var query = @"	Select 
			                        a.wmtid,
			                        a.material_code as mt_cd,
			                        a.mt_no,
			                        a.gr_qty,
			                        a.gr_qty as gr_qty1,
                                    a.real_qty,
			                        -- c.dt_nm,
			                        l.lct_cd,
			                        a.bb_no,
			                        a.chg_date as chg_dt
		                     --       m.result As count_table2
			                   --     Sum(b.gr_qty) as sl_tru_ng
			                        from w_material_info_mms a 
				                       --  -- left join (Select dt_nm,dt_cd,mt_cd from comm_dt) As c on c.dt_cd = a.bbmp_sts_cd And c.mt_cd = 'MMS007'
				                          left join (Select lct_nm, lct_cd from lct_info)  As l on l.lct_cd = a.location_code And lct_nm like 'FAC%'
										  left join (Select Count(*) as result, mt_lot from w_material_mapping_mms m group by m.mt_lot) As m on m.mt_lot = a.material_code 
										--  left join (Select gr_qty, orgin_mt_cd, material_code from w_material_info_mms group by gr_qty, orgin_mt_cd, material_code) As b on b.orgin_mt_cd = a.material_code 
										--																	                       AND b.material_code like (a.material_code + '-RT') + '%'
																												              --     OR b.material_code like (a.material_code + '-DV') --+ '%'
			                        where a.id_actual = @id And a.material_type = 'CMT' -- And dt_nm IS NOT NULL 
			                        group by a.wmtid, a.material_code, a.mt_no, a.gr_qty, a.gr_qty,/* c.dt_nm,*/  a.bb_no, a.chg_date, a.reg_date, l.lct_cd,/* m.result,*/ a.real_qty
			                        Order by a.reg_date desc";
                var result = await base.DbConnection.QueryAsync<MaterialToDeviceResponse>(query, new { @Id = id_actual });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<MaterialInfoMMS> GetMaterialInfoOfDevice(string mt_cd, string status)
        {
            try
            {
                //AND gr_qty > 0
                var query = @"SELECT * FROM w_material_info_mms WHERE material_code Like @Mt_Cd +'%' AND [status] = @Status";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoMMS>(query, new { @Mt_Cd = mt_cd, @Status = status });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> GetUnitStaffforDevice(int id_actual)
        {
            try
            {
                var query = @"SELECT COUNT(*) FROM d_pro_unit_staff AS a WHERE a.id_actual = @Id AND (GetDate() BETWEEN a.start_dt AND a.end_dt)";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Id = id_actual });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UpdatedBobbinInfoForDevice(string bb_no, string mt_cd, string chg_id)
        {
            try
            {
                var query = "UPDATE d_bobbin_info SET chg_id = @Chg_Id, mt_cd = '' WHERE bb_no = @Bb_No AND mt_cd = @Mt_Cd";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Chg_Id = chg_id, @Bb_No = bb_no, @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> DeleteDBobbinLctHistForDevice(string bb_no, string mt_cd)
        {
            try
            {
                var query = @"DELETE FROM d_bobbin_lct_hist WHERE bb_no = @Bb_No AND mt_cd = @Mt_Cd";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Bb_No = bb_no, @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> GetTotalMaterialInfoDV(string mt_cd)
        {
            try
            {
                var query = @"SELECT COUNT(*) FROM w_material_info_mms WHERE material_code Like '%'+@Mt_Cd+'%'";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> GetTotalMaterialInfoDVTims(string mt_cd)
        {
            try
            {
                var query = @"SELECT COUNT(*) FROM w_material_info_tims WHERE material_code Like '%'+@Mt_Cd+'%'";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UpdateMaterialMappingMMS(MaterialMappingMMS item)
        {
            try
            {
                var query = @"Update w_material_mapping_mms Set mt_lot = @mt_lot, mt_cd = @mt_cd, mt_no = @mt_no, mapping_dt = @mapping_dt, 
                                    use_yn = @use_yn, del_yn = @del_yn, chg_date = @chg_date, reg_id = @reg_id, chg_id = @chg_id, reg_date = @reg_date
                                    where wmmid = @wmmId";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<BobbinInfo>> SearchBobbinInfo(string bb_no, string bb_nm)
        {
            try
            {
                var query = @"SELECT Top 5000
	                                a.bno,
	                                a.mc_type,
	                                a.bb_no,
	                                a.mt_cd,
	                                a.bb_nm,
	                                a.use_yn,
	                                a.purpose,
	                                a.barcode,
	                                a.re_mark,
	                                a.count_number,
	                                a.del_yn,
	                                a.reg_id,
	                                a.reg_dt,
	                                a.chg_id,
	                                a.chg_dt 
                                FROM
	                                d_bobbin_info AS a 
                                WHERE
	                                ( a.mt_cd = '' OR a.mt_cd IS NULL ) 
	                                AND (@Bb_No = '' OR @Bb_No IS NULL OR a.bb_no LIKE '%'+@Bb_No+'%') 
	                                AND (@Bb_nm = '' OR @Bb_nm IS NULL OR a.bb_nm LIKE '%'+@Bb_nm+'%')";
                var result = await base.DbConnection.QueryAsync<BobbinInfo>(query, new { @Bb_No = bb_no, @Bb_nm = bb_nm });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<BobbinLctHist> GetBobbinLctHistFrom(string bb_no, string mt_cd)
        {
            try
            {
                var query = @"SELECT * FROM d_bobbin_lct_hist WHERE bb_no = @Bb_No AND mt_cd = @Mt_Cd";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<BobbinLctHist>(query, new { @Bb_No = bb_no, @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UpdateBobbinLctHist(BobbinLctHist item)
        {
            try
            {
                var query = @"UPDATE d_bobbin_lct_hist SET mc_type = @mc_type, bb_no = @bb_no, mt_cd = @mt_cd, bb_nm = @bb_nm, start_dt = @start_dt, end_dt = @end_dt, use_yn = @use_yn
                              ,del_yn = @del_yn, reg_id = @reg_id, reg_dt = @reg_dt, chg_id = @chg_id, chg_dt = @chg_dt WHERE blno = @blno";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> InsertToBobbinLctHistory(BobbinLctHist item)
        {
            try
            {
                var query = @"INSERT INTO d_bobbin_lct_hist (mc_type,bb_no,mt_cd,bb_nm,use_yn,del_yn,reg_id,reg_dt,chg_id,chg_dt)
                              VALUES(@mc_type, @bb_no, @mt_cd, @bb_nm, @use_yn, @del_yn, @reg_id, @reg_dt, @chg_id, @chg_dt);
                              SELECT SCOPE_IDENTITY()";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UpdateMaterialInfoDivideMMS(string bb_no, int wmtid, string type)
        {
            try
            {
                var query = @"Update tmp_w_material_divide_mms Set bb_no = @Bb_No, type = @Type where wmtid = @Id ";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Bb_No = bb_no, @Id = wmtid, @Type = type });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<MaterialInfoMMS>> GetMaterialInfoMMSByDivide(int id_actual, string mt_no)
        {
            try
            {
                var query = @"Select * from w_material_info_mms where id_actual = @Id_Actual and mt_no = @Mt_No";
                var result = await base.DbConnection.QueryAsync<MaterialInfoMMS>(query, new { @Id = id_actual, @Mt_No = mt_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<string> GetAtNoByActual(int id_actual)
        {
            try
            {
                var query = "Select at_no from w_actual where id_actual in (Select id_actual from w_material_info_mms where id_actual = @Id)";
                var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @Id = id_actual });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<int>> GetMaterialMappingNVL(string style_no, string mt_cd , string name, string process_code)
        {
            try
            {
                var query = @"  SELECT Id FROM product_material WHERE style_no = @StyleNo AND name  = @name and process_code =@process_code 
	                                                                AND mt_no NOT IN(select tmpe.MaterialParent From 
											                                                                (SELECT tmpb.MaterialParent 
																                                                                FROM 
																                                                                (SELECT c.mt_no
																		                                                                FROM(
																				                                                                SELECT a.mt_cd, a.mt_no
																				                                                                FROM w_material_mapping_mms  AS a
																				                                                                JOIN w_material_info_mms AS b
																				                                                                ON a.mt_lot = b.material_code
																				                                                                WHERE a.mt_lot = @Mt_Cd) AS TABLE2
																				                                                                JOIN inventory_products AS c
																				                                                                ON TABLE2.mt_cd = c.material_code WHERE c.mt_type = 'PMT') As tmpa
																                                                                JOIN(SELECT b.MaterialParent, b.MaterialNo  FROM  product_material_detail b WHERE  b.ProductCode = @StyleNo AND b.[name] = @name AND b.process_code = @process_code) As
																                                                                tmpb on tmpb.MaterialNo = tmpa.mt_no) As tmpe)
	                                                                AND mt_no  NOT IN (select tmpa.mt_no FROM 
									                                                                (SELECT c.mt_no
									                                                                FROM(
											                                                                SELECT a.mt_cd, a.mt_no
											                                                                FROM w_material_mapping_mms  AS a
											                                                                JOIN w_material_info_mms AS b
											                                                                ON a.mt_lot = b.material_code
											                                                                WHERE a.mt_lot = @Mt_Cd) AS TABLE2
									                                                                JOIN inventory_products AS c
									                                                                ON TABLE2.mt_cd = c.material_code WHERE c.mt_type = 'PMT') As tmpa)";

                var result = await base.DbConnection.QueryAsync<int>(query, new { @StyleNo = style_no, @Mt_Cd = mt_cd, @name = name , @process_code  = process_code });
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<ActualMaxLevelResponse> NameBTP(string at_no, string name)
        {
            try
            {
                var query = @"SELECT MAX(a.name) As NameProcess, MAX(ap.product) As Product
                                FROM  w_actual AS a 
						        JOIN w_actual_primary As ap
						        On a.at_no = ap.at_no
                                WHERE a.at_no =  @At_No AND a.type = 'SX' AND a.name < @Name";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<ActualMaxLevelResponse>(query, new { @At_No = at_no, @Name = name });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<string> IsBTPExistByMapping(string mt_lot, string mt_no)
        {
            try
            {
                var query = @"SELECT CASE 
                                WHEN EXISTS(
                                       SELECT c.mt_no FROM (SELECT a.mt_cd, a.mt_no
                                            FROM w_material_mapping_mms  AS a
                                            JOIN w_material_info_mms AS b
                                            ON a.mt_lot = b.material_code
                                            WHERE a.mt_lot = @Mt_Lot ) AS TABLE1 
                                            JOIN  w_material_info_mms AS c
                                            ON TABLE1.mt_cd = c.material_code AND c.material_type ='CMT' AND c.mt_no = @Mt_no
					                    ) THEN 'true'
                                 ELSE 'false'
                                END";
                var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @Mt_Lot = mt_lot, @Mt_no = mt_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<string> IsMaterialInfoExistByProcess(string productCode, string name, string materialNo, string process_code)
        {
            try
            {
                //AND a.mt_nm = @Name
                var query = @"SELECT CASE 
			                    WHEN EXISTS(
					                    SELECT a.style_no, a.mt_no, b.MaterialNo
					                    FROM product_material a 
						                    LEFT JOIN product_material_detail b ON a.style_no = b.ProductCode AND a.level = b.level
					                    WHERE a.style_no = @ProductCode AND a.process_code = @ProcessCode AND a.name = @Name AND (a.mt_no = @MaterialNo OR b.MaterialNo = @MaterialNo)
					                    ) THEN 'true'
				                    ELSE 'false'
			                    END";
                var result = await base.DbConnection.ExecuteScalarAsync<string>(query,
                    new { @ProductCode = productCode,
                        @MaterialNo = materialNo,
                        @Name = name ,
                        @ProcessCode = process_code
                    });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<string> CheckMaterialRetun(string mt_cd)
        {
            try
            {
                var query = @"SELECT Top 1 a.material_code FROM w_material_info_mms AS a WHERE a.orgin_mt_cd = @Mt_Cd";
                var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<string> CheckInventoryProductRetun(string mt_cd)
        {
            try
            {
                var query = @"SELECT * FROM inventory_products AS a WHERE a.orgin_mt_cd = @Mt_Cd";
                var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> CheckMaterialMappingFinish(string mt_cd, string mt_lot, DateTime mapping_dt)
        {
            try
            {
                var query = @"SELECT COUNT(*) FROM w_material_mapping_mms WHERE mt_cd = @Mt_Cd AND mt_lot != @Mt_Lot AND mapping_dt > @Mapping_Date";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Mt_Cd = mt_cd, @Mt_Lot = mt_lot, @Mapping_Date = mapping_dt });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> GetIdActual()
        {
            try
            {
                var query = @"Select Top 1 id_actual from w_actual Order by id_actual desc";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> CheckExitReturn(string orgin_mt_cd)
        {
            try
            {
                var query = @"SELECT Count(*) FROM inventory_products WHERE orgin_mt_cd = @Orgin_Mt_Cd And status = '004'";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Orgin_Mt_Cd = orgin_mt_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> CheckMaterialMappingForRedo(string mt_cd)
        {
            try
            {
                var query = @"SELECT COUNT(*) FROM w_material_mapping_mms WHERE mt_cd != @Mt_Cd AND mt_lot IN (SELECT mt_cd FROM w_material_info_mms WHERE material_code = @Mt_Cd)";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<MaterialInfoMMS>> CheckMaterialMappingContainer(string mt_cd)
        {
            try
            {
                var query = @"Select * from w_material_info_mms where material_code Like @Mt_Cd + '%'";
                var result = await base.DbConnection.QueryAsync<MaterialInfoMMS>(query, new { @Mt_Cd = mt_cd });
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<MaterialInfoMMS>> CheckMaterialMappingContainer1(string mt_cd)
        {
            try
            {
                var query = @"Select * from w_material_info_mms where material_code Like @Mt_Cd + '%'";
                var result = await base.DbConnection.QueryAsync<MaterialInfoMMS>(query, new { @Mt_Cd = mt_cd + "-DV" });
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<MaterialMappingMMS> GetMaterialMappingReturn(string mt_cd, string mt_lot)
        {
            try
            {
                var query = @"SELECT Top 1 * FROM w_material_mapping_mms WHERE mt_cd = @Mt_Cd AND mt_lot = @Mt_Lot";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialMappingMMS>(query, new { @Mt_Cd = mt_cd, @Mt_Lot = mt_lot });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<IEnumerable<MaterialMappingMMS>> GetMaterialMappingByCode(string mt_cd)
        {
            try
            {
                var query = @"SELECT * FROM w_material_mapping_mms WHERE mt_cd = @Mt_Cd";
                var result = await base.DbConnection.QueryAsync<MaterialMappingMMS>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> CountMaxMapping(string mt_cd, string mt_lot)
        {
            try
            {
                var query = @"SELECT a.wmmid FROM w_material_mapping_mms as a
                                 WHERE a.wmmid = (
                                SELECT  MAX(b.wmmid)
                                FROM w_material_mapping_mms AS b
                                WHERE b.mt_cd = @Mt_Cd ) AND a.mt_cd = @Mt_Cd and   a.mt_lot = @Mt_Lot";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Mt_Cd = mt_cd, @Mt_Lot = mt_lot });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<BobbinInfo> GetBobbinInfoReturn(string bb_no, string mt_cd)
        {
            try
            {
                var query = @"SELECT * FROM d_bobbin_info WHERE bb_no = @Bb_No AND mt_cd = @Mt_Cd";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<BobbinInfo>(query, new { @Bb_No = bb_no, @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UpdateMaterialMappingForDivide(string mt_cd, string use_yn, string useyn, string chg_id)
        {
            try
            {
                var query = @"UPDATE w_material_mapping_mms SET use_yn= @UseYn,chg_id = @Chg_id WHERE mt_cd = @Mt_Cd AND use_yn = @Use_Yn";
                var result = await base.DbConnection.ExecuteAsync(query, new { @UseYn = useyn, @Chg_id = chg_id, @Mt_Cd = mt_cd, @Use_Yn = use_yn });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> InsertMaterialMappingReturn(int count, int quantity, string status, string chg_id, string mt_cd)
        {
            try
            {
                var query = @"
								INSERT INTO inventory_products(material_code, mt_type, gr_qty, real_qty, status, sd_no, ExportCode, LoctionMachine, ShippingToMachineDatetime,
                                expiry_date, export_date, date_of_receipt, lot_no, mt_no, bb_no, location_code, from_lct_cd, recei_date, create_date, create_id, change_date, change_id, orgin_mt_cd, return_date) 

								SELECT CONCAT(material_code,'-RT', @Count) As material_code, mt_type, @Quantity, @Quantity, @Status, sd_no, ExportCode, LoctionMachine, ShippingToMachineDatetime,
														expiry_date, export_date, date_of_receipt, lot_no, mt_no, bb_no, location_code, from_lct_cd, recei_date, GetDate(), @Chg_Id, GetDate(), @Chg_Id, @Mt_Cd, GetDate()
								FROM
									inventory_products 
								WHERE
									material_code = @Mt_Cd";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Count = count, @Quantity = quantity, @Status = status, @Chg_Id = chg_id, @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdatePartialInventoryProduct(int id, string status, double qty, string change_id, DateTime? change_date)
        {
            try
            {
                var query = @"Update inventory_products Set status = @Status, gr_qty = @Qty, change_id = @ChgId, change_date = @ChgDate  where materialid = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Status = status, @Qty = qty, @ChgId = change_id, @ChgDate = change_date, @Id = id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<InventoryProduct> GetDataInventoryProduct(string mt_cd)
        {
            try
            {
                var query = @"Select * from inventory_products where material_code = @Mt_Cd";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<InventoryProduct>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> CheckMaterialMappingOfBobbbin(string mt_mapping)
        {
            try
            {
                var QueryCheckMaterialInfoMMS = @"Select Count(*) from w_material_info_mms Where material_code = @Mt_Mapping";
                var ReusltCheckMaterialInfoMMS = await base.DbConnection.ExecuteScalarAsync<int>(QueryCheckMaterialInfoMMS, new { @Mt_Mapping = mt_mapping });

                var QueryCheckInInventoryProduct = @"Select Count(*) from inventory_products Where material_code = @Mt_Mapping";
                var ReusltCheckInInventoryProduct = await base.DbConnection.ExecuteScalarAsync<int>(QueryCheckInInventoryProduct, new { @Mt_Mapping = mt_mapping });

                var result = 0;
                if (ReusltCheckInInventoryProduct > 0)
                {
                    result = 1;
                }
                if (ReusltCheckMaterialInfoMMS > 0)
                {
                    result = 2;
                }

                return result;

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<ActualPrimary> GetDataActualPrimary(string at_no)
        {
            try
            {
                var query = @"Select * From w_actual_primary where at_no = @AtNo";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<ActualPrimary>(query, new { @AtNo = at_no });
                return result;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<PrintQRMaterialResponse> PrintfQRCode(int id)
        {
            try
            {
                var query = @"
						Select a.material_code, a.mt_no, a.gr_qty, a.export_date, a.expiry_date ,a.date_of_receipt, a.lot_no, 1 As send_qty,
								 b.bundle_qty, b.bundle_unit, b.mt_type, b.width, b.spec, b.width_unit, b.spec_unit, b.mt_nm
							From inventory_products As a
							JOIN d_material_info As b On a.mt_no = b.mt_no
							Where a.materialid = @Id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<PrintQRMaterialResponse>(query, new { @Id = id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Task<IEnumerable<LocationInfo>> GetNameFromLocationInfo(string lct_cd)
        {
            try
            {
                var query = @"Select lct_nm From lct_info where lct_cd = @lct_cd";
                var result = base.DbConnection.QueryAsync<LocationInfo>(query, new { @lct_cd = lct_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<MaterialInfoMMS>> CheckMaterialOKInfoMMS(string mt_cd)
        {
            try
            {
                var query = @"Select * from w_material_info_mms where material_code LIKE @mt_cd + '%' ";
                var result = await base.DbConnection.QueryAsync<MaterialInfoMMS>(query, new { @mt_cd = mt_cd + "-OK" });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<MaterialMappingMMS> CheckwMaterialMappingMax(string mt_cd)
        {
            try
            {
                var query = @"SELECT Top 1 * FROM w_material_mapping_mms WHERE mt_cd = @Mt_Cd ORDER BY wmmid DESC ";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialMappingMMS>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #region QC
        public async Task<IEnumerable<FaclineQC>> GetListFacline_Qc(string item_vcd, string mt_cd)
        {
            try
            {
                var query = @"Select fqno,fq_no,check_qty, ml_no, work_dt, ok_qty, (check_qty - ok_qty) As defect_qty  
                             From m_facline_qc
                             where (@mt_cd = '' OR @mt_cd IS NULL OR ml_no = @mt_cd)
                             --And fq_no like 'FQ%'
                             --And (@item_vcd = '' OR @item_vcd IS NULL OR item_vcd = @item_vcd )
                             Order By fq_no desc ,check_qty desc ";
                var result = await base.DbConnection.QueryAsync<FaclineQC>(query, new { @item_vcd = item_vcd, @mt_cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<FaclineQCValue>> GetListFacline_qc_value(string fq_no)
        {
            try
            {
                var query = @"Select a.fqhno, b.check_subject, a.check_value, a.check_qty, a.date_ymd
                            From m_facline_qc_value As a 
                            JOIN qc_itemcheck_mt As b On a.check_id = b.check_id
                            Where a.fq_no = @Fq_No";
                var result = await base.DbConnection.QueryAsync<FaclineQCValue>(query, new { @Fq_No = fq_no });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<FaclineQC>> GetListDataFacline_Qc(string mt_cd)
        {
            try
            {
                var query = @"Select * From m_facline_qc Where ml_no = @Ml_No And fq_no Like 'FQ%' ";
                var result = await base.DbConnection.QueryAsync<FaclineQC>(query, new { @Ml_No = mt_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> DeleteDataFacline_Qc(string fq_no)
        {
            try
            {
                var query = @"DELETE FROM m_facline_qc WHERE fq_no = @Fq_No";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Fq_No = fq_no });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<FaclineQC>> GetListDataFacline_Qc()
        {
            try
            {
                var query = @"Select * FROM m_facline_qc WHERE fq_no Like 'FQ%'";
                var result = await base.DbConnection.QueryAsync<FaclineQC>(query);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UpdateDataMaterialInfoMMS(string mt_cd, int gr_qty, int real_qty)
        {
            try
            {
                var query = @"Update w_material_info_mms Set gr_qty = @gr_qty, real_qty = @real_qty Where material_code = @Mt_Cd";
                var result = await base.DbConnection.ExecuteAsync(query, new { @gr_qty = gr_qty, @real_qty = real_qty, @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public async Task<int> UpdateDataMaterialInfoMMS(string mt_cd, int gr_qty)
        {
            try
            {
                var query = @"Update w_material_info_mms Set gr_qty = @gr_qty Where material_code = @Mt_Cd";
                var result = await base.DbConnection.ExecuteAsync(query, new { @gr_qty = gr_qty, @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UpdateDataActualInfo(int id_actual, int qty)
        {
            try
            {
                var query = @"Update w_actual Set defect = defect - @Qty Where id_actual = @Id_Actual";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Id_Actual = id_actual, @Qty = qty });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UpdateActualForWActual(int qty, int id_actual)
        {
            try
            {
                var query = @"Update w_actual Set actual = @Qty Where id_actual = @Id_Actual";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Id_Actual = id_actual, @Qty = qty });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<QCItemMaterial> GetDateQcItemMaterial(string item_vcd)
        {
            try
            {
                var query = @"Select * From qc_item_mt Where item_vcd = @item_vcd";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<QCItemMaterial>(query, new { @item_vcd = item_vcd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> InsertIntoDataFaclineQC(FaclineQC item)
        {
            try
            {
                var query = @"Insert Into m_facline_qc (fq_no, ml_no, ml_tims, product_cd, shift, at_no, work_dt, item_vcd, item_nm, item_exp, check_qty, ok_qty, reg_dt, chg_id, chg_dt)
                            Values (@fq_no, @ml_no, @ml_tims, @product_cd, @shift, @at_no, @work_dt, @item_vcd, @item_nm, @item_exp, @check_qty, @ok_qty, @reg_dt, @chg_id, @chg_dt)
                            select scope_identity()";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<MaterialInfoMMS>> GetMaterialInfoMMSByCode(string mt_cd)
        {
            try
            {
                var query = @"Select * from w_material_info_mms where material_code LIKE @mt_cd + '%' ";
                var result = await base.DbConnection.QueryAsync<MaterialInfoMMS>(query, new { @mt_cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> InsertIntoDataMaterialInfoMMS(string Bien, int check_qty, int ok_qty, string reg_id, string chg_id, string mt_cd)
        {
            try
            {
                var query = @"INSERT INTO w_material_info_mms (id_actual, material_type, material_code, mt_no, gr_qty, expiry_date, date_of_receipt, export_date, lot_no, status,
                            reg_id, reg_date, chg_id, chg_date, orgin_mt_cd, location_code, real_qty )

                            SELECT 0,material_type, @bien, mt_no, (@check_qty - @ok_qty), expiry_date, date_of_receipt, export_date, lot_no, '003', 
                            @reg_id, GetDate(), @chg_id, GetDate(), material_code, location_code, (@check_qty - @ok_qty) 
                            FROM   w_material_info_mms
                            WHERE  material_code = @Mt_Cd

                            select scope_identity()";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query,
                    new
                    {
                        @bien = Bien,
                        @check_qty = check_qty,
                        @ok_qty = ok_qty,
                        @reg_id = reg_id,
                        @chg_id = chg_id,
                        @Mt_Cd = mt_cd
                    });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateQtyForMaterialInfoMMS1(string mt_cd, int check_qty, int ok_qty)
        {
            try
            {
                var query = @"Update w_material_info_mms Set gr_qty = gr_qty + (@Check_Qty - @Ok_Qty), real_qty = real_qty + (@Check_Qty - @Ok_Qty)
                            WHERE material_code = @Mt_Cd";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Check_Qty = check_qty, @Ok_Qty = ok_qty, @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UpdateQtyForMaterialInfoMMS2(string mt_cd, int check_qty, int ok_qty)
        {
            try
            {
                var query = @"Update w_material_info_mms Set gr_qty = gr_qty - (@Check_Qty - @Ok_Qty)
                            WHERE material_code = @Mt_Cd";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Check_Qty = check_qty, @Ok_Qty = ok_qty, @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UpdateDecfectQtyMaterialInfoMMS(int id_actual, float defectQty)
        {
            try
            {
                var query = @"Update w_actual Set defect = @DefectQty Where id_actual = @Id_Actual";
                var result = await base.DbConnection.ExecuteAsync(query, new { @DefectQty = defectQty, @Id_Actual = id_actual });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> GetSumQtyDProUnitMachineByIdActual(int id_actual)
        {
            try
            {
                var query = @"SELECT SUM(actual) FROM d_pro_unit_staff WHERE id_actual = @Id_Actual";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Id_Actual = id_actual });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> GetDefectQtyByIdActual(int id_actual)
        {
            try
            {
                var query = @"Select defect From w_actual WHERE id_actual = @Id_Actual";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Id_Actual = id_actual });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> GetRealQtyMaterialInfoMMSByIdActual(int id_actual)
        {
            try
            {
                var query = @"SELECT SUM(real_qty) FROM w_material_info_mms WHERE id_actual = @Id_Actual AND material_type ='CMT' AND orgin_mt_cd IS NULL";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Id_Actual = id_actual });
                return result;

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UpdateQtyActual(int id_actual, double total)
        {
            try
            {
                var query = @"UPDATE w_actual SET actual = ISNULL(@Qty,0) WHERE id_actual=  @Id_Actual";
                var result = await base.DbConnection.ExecuteAsync(query, new { @Qty = total, @Id_Actual = id_actual });
                return result;

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<QCItemCheckMaterial>> GetListDataQCItemCheckMaterial(string item_vcd)
        {
            try
            {
                var query = @"Select * From qc_itemcheck_mt Where item_vcd = @ItemVCD And del_yn = 'N'";
                var result = await base.DbConnection.QueryAsync<QCItemCheckMaterial>(query, new { @ItemVCD = item_vcd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public async Task<IEnumerable<QCItemCheckMaterial>> GetAllDataQCItemCheckMaterial()
        {
            try
            {
                var query = @"Select * From qc_itemcheck_mt";
                var result = await base.DbConnection.QueryAsync<QCItemCheckMaterial>(query);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<QCItemCheckDetail>> GetAllDataQCItemCheckDetail()
        {
            try
            {
                var query = @"Select * From qc_itemcheck_dt";
                var result = await base.DbConnection.QueryAsync<QCItemCheckDetail>(query);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> CountMoldInfo(string md_no, string md_nm)
        {
            try
            {
                var query = @" Select Count(*) From 
							 (
								SELECT a.*,
										ROW_NUMBER() OVER (ORDER BY a.mdno DESC) AS RowNum ,
										(CASE  WHEN b.mc_no IS NOT NULL THEN 'mapping'  ELSE 'Unmapping' END) su_dung
								FROM d_mold_info a  
								 LEFT JOIN d_pro_unit_mc AS b ON a.md_no=b.mc_no  
								WHERE (@Md_No = '' OR @Md_No IS NULL OR   a.md_no like'%' + @Md_No + '%')
									AND (@Md_Nm = '' OR @Md_Nm IS NULL OR   a.md_nm like'%' + @Md_Nm + '%')  
								
								GROUP BY a.md_no, a.mdno, a.md_nm, a.purpose, a.barcode, a.re_mark, a.use_yn, a.del_yn, a.reg_id, a.reg_dt, a.chg_id, a.chg_dt, a.active, b.mc_no
								--ORDER BY su_dung DESC
							 ) As MyDerivedTable";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Md_No = md_no, @Md_Nm = md_nm });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<MoldInfo>> GetListDataMoldInfo(string md_no, string md_nm)
        {
            try
            {
                var query = @"Select * From 
							 (
								SELECT a.*,
										ROW_NUMBER() OVER (ORDER BY a.mdno DESC) AS RowNum ,
										(CASE  WHEN b.mc_no IS NOT NULL THEN 'mapping'  ELSE 'Unmapping' END) su_dung
								FROM d_mold_info a  
								 LEFT JOIN d_pro_unit_mc AS b ON a.md_no=b.mc_no  
								WHERE (@Md_No = '' OR @Md_No IS NULL OR   a.md_no like'%' + @Md_No + '%')
									AND (@Md_Nm = '' OR @Md_Nm IS NULL OR   a.md_nm like'%' + @Md_Nm + '%')  
								GROUP BY a.md_no, a.mdno, a.md_nm, a.purpose, a.barcode, a.re_mark, a.use_yn, a.del_yn, a.reg_id, a.reg_dt, a.chg_id, a.chg_dt, a.active, b.mc_no
								--ORDER BY su_dung DESC
							 ) As MyDerivedTable";
                var result = await base.DbConnection.QueryAsync<MoldInfo>(query, new { @Md_No = md_no, @Md_Nm = md_nm });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        #endregion

        public async Task<IEnumerable<WActualBom>> GetListDataBomMaterial(string style_no, string at_no, string process_code)
        {
            try
            {
                string query = @"Select b.mt_no As MaterialNo,@At_No as at_no,convert(decimal(10,5),b.need_m) need_m,b.style_no As ProductCode, b.process_code,
                                (SELECT mt_nm FROM d_material_info WHERE mt_no = b.mt_no) AS MaterialName,
                                (Case When SUM(gr_qty) IS NULL then (SELECT ISNULL(sum(p.gr_qty),0) FROM inventory_products p JOIN w_actual ac ON p.id_actual = ac.id_actual WHERE ac.at_no = @At_No AND mt_no IN(Select MaterialNo From product_material_detail where ProductCode = @Product and process_code = @process_code )) ELSE ISNULL(sum(gr_qty),0) End) mLieuOLD,

                            (SELECT ISNULL(sum(ac.gr_qty),0) 
								FROM product_material p 
								left JOIN inventory_products ac  ON p.mt_no = ac.mt_no
								JOIN w_actual act ON ac.id_actual = act.id_actual

								WHERE act.at_no = @At_No AND p.mt_no = b.mt_no and  p.style_no = @Product  and p.process_code = @process_code
								) mLieu,
                                isnull(liethaythe.m_lieu,0) lieuthaythe, 
                                isnull(Count(a.mt_cd),0) As SoCuonNVL,
                                isnull((SELECT sum(actual.actual)
                                FROM  process_last_time AS actual
                                WHERE actual.product =@Product AND actual.at_no =@At_No),0) Actual
                                from product_material b 
                                left join 
                                (select distinct a.at_no, a.product
                                ,(case when d.mt_no is null or d.mt_no = ''  then e.mt_no else d.mt_no end) mt_no
                                ,(case when d.material_code is null or d.material_code = ''  then e.material_code else d.material_code end ) mt_cd
                                ,(case when d.gr_qty is null or d.gr_qty = 0  then e.gr_qty else d.gr_qty end ) gr_qty

                                from w_actual a
                                inner join w_material_info_mms b on b.id_actual=a.id_actual
                                inner join w_material_mapping_mms c on c.mt_lot=b.material_code
                                left join inventory_products d on c.mt_cd=d.material_code
                                left join w_material_info_mms e on e.material_code=c.mt_cd
                                where a.at_no=@At_No) a on a.mt_no=b.mt_no
                                left join (Select tbl.style_no,tbl.process_code,tbl.mt_no,isnull(SUM(tbl.m_lieu),0) As m_lieu
                                From (
	                                Select c.style_no,c.process_code, c.mt_no,
		                                isnull(mttt.gr_qty,0) m_lieu
		                                From product_material As c
		                                JOIN product_material_detail As d On c.style_no =  d.ProductCode and c.process_code = d.process_code And c.mt_no = d.MaterialParent
		                                inner join (select distinct a.at_no
				                                ,b.mt_no
				                                ,SUM(b.gr_qty) gr_qty
				                                from w_actual a
				                                inner join inventory_products b on b.id_actual=a.id_actual
				                                where a.at_no = @At_No
				                                group by a.at_no,b.mt_no) mttt on mttt.mt_no=d.MaterialNo
						                                Where c.style_no = @Product and c.process_code = @process_code
		                                ) As tbl
		                                group by tbl.style_no,tbl.process_code,tbl.mt_no) liethaythe on liethaythe.style_no=b.style_no and liethaythe.process_code = b.process_code and liethaythe.mt_no=b.mt_no
                                where b.style_no=@Product and b.process_code = @process_code
                                group by b.mt_no,b.need_m,b.style_no,b.process_code,liethaythe.m_lieu, a.product order by b.mt_no desc ";
                var result = await base.DbConnection.QueryAsync<WActualBom>(query, new { @Product = style_no, @At_No = at_no, process_code = process_code });
                return result;



                //         var query = @"
                //                     SELECT MAX(a.mt_no) AS MaterialNo, @At_No AS at_no , max(a.need_m) AS need_m, max(a.style_no) ProductCode,
                //                  (SELECT mt_nm FROM d_material_info WHERE mt_no = MAX(a.mt_no)) AS MaterialName,
                //                  ((SELECT sum(actual.actual)
                //                         FROM  
                //                   (
                //                    SELECT 

                //				a.product,
                //                      a.actual AS actual,
                //                      a.id_actual AS id_actual,
                //                      a.at_no AS at_no,
                //                      a.type AS type,
                //                      a.name AS name,
                //                      a.level AS level,
                //                      a.date AS date,
                //                      a.don_vi_pr AS don_vi_pr,
                //                      a.item_vcd AS item_vcd,
                //                      a.defect AS defect,
                //                      a.reg_id AS reg_id,
                //                      a.reg_dt AS reg_dt,
                //                      a.chg_id AS chg_id,
                //                      a.chg_dt AS chg_dt
                //                     FROM
                //                      w_actual a
                //                     WHERE
                //                      (a.type = 'SX' AND (a.level = (SELECT MAX(k.level)
                //											  FROM w_actual k
                //											  WHERE k.type = 'SX' AND a.at_no = k.at_no
                //											  GROUP BY k.at_no)))
                //                   ) AS actual
                //                   WHERE actual.at_no = @At_No AND actual.product = @Product)) as Actual,

                //                   (SELECT ISNULL(sum(TABLE1.m_lieu),0) AS m_lieu
                //                    FROM (
                //                     SELECT  
                //                       (Select ISNULL(sum(gr_qty),0) From w_material_info_mms As a JOIN w_actual As b On a.id_actual = b.id_actual
                //					Where b.at_no = @At_No And a.mt_no = (d.MaterialNo)) AS m_lieu
                //                   FROM product_material AS p
                //                   JOIN product_material_detail AS d
                //                   ON p.style_no = d.ProductCode AND p.mt_no = d.MaterialParent
                //                   WHERE p.style_no = @Product AND p.mt_no = MAX(a.mt_no))
                //	AS TABLE1) As lieuthaythe,

                //                  SUM(gr_qty)  AS mLieu,
                //                  COUNT(material_code) AS SoCuonNVL

                //                  FROM product_material AS a
                //JOIN w_material_info_mms As b On a.mt_no = b.mt_no And material_type = 'PMT'
                //JOIN w_actual As c On b.id_actual = c.id_actual
                //                  WHERE a.style_no = @Product And c.at_no = @At_No
                //                  GROUP BY a.mt_no
                //ORDER BY MaterialName Desc ";
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public async Task<IEnumerable<WActualBom>> GetListOfSubstituteMaterials(string ProductCode, string at_no, string mt_no, string process_code)
        {
            try
            {
                var query = @"							
	                        SELECT a.MaterialNo,
	                        (SELECT mt_nm FROM d_material_info WHERE mt_no = (a.MaterialNo)) AS MaterialName,

	                        (SELECT ISNULL(count(material_code),0) FROM inventory_products p JOIN w_actual ac ON p.id_actual = ac.id_actual
	                         WHERE  p.mt_type ='PMT'  and ac.at_no = @At_NO AND mt_no = (a.MaterialNo)) AS SoCuonNVL,

	                        (SELECT sum(gr_qty) FROM inventory_products p JOIN w_actual ac ON p.id_actual = ac.id_actual 
                             WHERE  p.mt_type ='PMT'  and ac.at_no = @At_NO AND mt_no = (a.MaterialNo)) AS mLieu
                        FROM product_material_detail AS a
                        WHERE a.MaterialParent = @Mt_No And a.ProductCode = @ProductCode and a.process_code = @process_code ";
                var result = await base.DbConnection.QueryAsync<WActualBom>(query, new { @ProductCode = ProductCode, @At_NO = at_no, @Mt_No = mt_no, @process_code = process_code });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> GetIdMachineData(string mc_no, string startDate, string endDate)
        {
            try
            {
                var query = @"SELECT pmid from d_pro_unit_mc WHERE mc_no = @Mc_No AND start_dt <= @Start_Date  AND @End_Date <= end_dt";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Start_Date = startDate, @End_Date = endDate, @Mc_No = mc_no });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public async Task<int> GetIdMoldData(string md_no, string startDate, string endDate)
        {
            try
            {
                var query = @"SELECT mdid from d_pro_unit_mold WHERE md_no = @Md_No AND start_dt <= @Start_Date  AND @End_Date <= end_dt";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Start_Date = startDate, @End_Date = endDate, @Md_No = md_no });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<Actual> GetActualDataByMaterialCode(string mt_cd)
        {
            try
            {
                var query = @"Select * From w_actual where id_actual IN (Select id_actual From w_material_info_mms where material_code = @Mt_Cd)";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<Actual>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<BobbinInfo> GetBobbinInfoChangebbdv(string bb_no)
        {
            try
            {
                var query = @"SELECT * FROM d_bobbin_info WHERE bb_no = @Bb_No And (mt_cd IS NULL OR mt_cd='')";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<BobbinInfo>(query, new { @Bb_No = bb_no });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<string> GetBobbinHistoryData(string bb_no)
        {
            try
            {
                var query = @"SELECT bb_no FROM d_bobbin_lct_hist WHERE bb_no = @Bb_No /*And (mt_cd IS NULL OR mt_cd='')*/";
                var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @Bb_No = bb_no });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> Checkwmfaclineqc(string mt_cd)
        {
            try
            {
                var query = @"SELECT COUNT(*) FROM m_facline_qc WHERE ml_no = @Ml_No";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Ml_No = mt_cd });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> Gettwproductqc(string mt_cd)
        {
            try
            {
                var query = @"SELECT COUNT(pqno) FROM w_product_qc WHERE ml_no =@Ml_No";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Ml_No = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> CountMoldInfoData(string md_no)
        {
            try
            {
                var query = @"Select Count(*) From d_mold_info where md_no = @Md_No";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Md_No = md_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> InsertIntoMoldInfoData(MoldInfo item)
        {
            try
            {
                var query = @"Insert Into d_mold_info (md_no, md_nm, purpose, barcode, re_mark, use_yn, del_yn, reg_id, reg_dt, chg_id, chg_dt)
                            Values (@md_no, @md_nm, @purpose, @barcode, @re_mark, @use_yn, @del_yn, @reg_id, @reg_dt, @chg_id, @chg_dt)
                            Select scope_identity()";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<MoldInfo> GetMoldInfoDataById(int Id)
        {
            try
            {
                var query = @"Select * From d_mold_info where mdno = @ID";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<MoldInfo>(query, new { @ID = Id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<MoldInfo> ModifyMoldInfo(int mdno, string md_no, string md_nm, string purpose, string re_mark)
        {
            try
            {
                var query = @"Update d_mold_info set md_no =@md_no, md_nm = @md_nm,purpose =@purpose, re_mark = @re_mark, chg_dt =SYSDATETIME() where  mdno =@mdno
                               select * from d_mold_info order by chg_dt desc ";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<MoldInfo>(query, new { @mdno = mdno,@md_no =md_no, @md_nm =md_nm, @purpose= purpose, @re_mark= re_mark });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<BobbinInfo>> GetListBobbinInfoData(string mt_cd)
        {
            try
            {
                var query = @"select * from d_bobbin_info where mt_cd = @Mt_Cd";
                var result = await base.DbConnection.QueryAsync<BobbinInfo>(query, new { @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<DProUnitMold>> GetListMoldInfoDataByIdActual(int id_actual)
        {
            try
            {
                var query = @"SELECT start_dt, end_dt, remark, md_no, use_yn, mdid 
                            FROM d_pro_unit_mold 
                            WHERE id_actual = @Id_Actual and md_no IN (SELECT md_no FROM d_mold_info)";
                var result = await base.DbConnection.QueryAsync<DProUnitMold>(query, new { @Id_Actual = id_actual });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<int> CheckExistMaterialMapping(string mt_lot, string mt_cd)
        {
            try
            {
                var query = @"Select Count(*) from w_material_mapping_mms where mt_lot = @Mt_Lot and mt_cd = @Mt_Cd";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Mt_Lot = mt_lot, @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<DProUnitMold> CheckMoldInfoData(string md_no, int id_actual)
        {
            try
            {
                var query = @"Select * From d_pro_unit_mold where md_no = @Md_No And id_actual = @Id_Actual";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<DProUnitMold>(query, new { @Md_No = md_no, @Id_Actual = id_actual });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> InsertIntoMoldInfo(DProUnitMold item)
        {
            try
            {
                var query = @"Insert Into d_pro_unit_mold (id_actual, start_dt, end_dt, remark, md_no, use_yn, del_yn, reg_id, reg_dt, chg_id, chg_dt)
                            Values(@id_actual, @start_dt, @end_dt, @remark, @md_no, @use_yn, @del_yn, @reg_id, @reg_dt, @chg_id, @chg_dt)
                            select scope_identity()";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<DProUnitMold> GetDMoldUnitDataById(int mdid)
        {
            try
            {
                var query = @"Select * From d_pro_unit_mold where mdid = @Id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<DProUnitMold>(query, new { @Id = mdid });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateMold(DProUnitMold item)
        {
            try
            {
                var query = @"UPDATE d_pro_unit_mold SET id_actual = @Id_Actual,start_dt = @StartDate, end_dt = @EndDate, remark = @Remark, md_no = @Md_No, use_yn = @Use_Yn  WHERE mdid = @Mdid ";
                int result = await base.DbConnection.ExecuteAsync(query,
                    new
                    {
                        @Id_Actual = item.id_actual,
                        @StartDate = item.start_dt,
                        @EndDate = item.end_dt,
                        @Remark = item.remark,
                        @Mc_No = item.md_no,
                        @Use_Yn = item.use_yn,
                        @Mdid = item.mdid
                    });
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<int> DeleteMold(int mdid)
        {
            try
            {
                var query = @"Delete d_pro_unit_mold where mdid = @Id ";
                int result = await base.DbConnection.ExecuteAsync(query, new { @Id = mdid });
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<int> DeleteMaterialMappingMMSByCode(string mt_cd)
        {
            try
            {
                try
                {
                    var query = @"Delete w_material_mapping_mms where mt_cd = @Mt_Cd ";
                    int result = await base.DbConnection.ExecuteAsync(query, new { @Mt_Cd = mt_cd });
                    return result;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> CheckBobbinHistoryLocation(string bb_no, string mt_cd)
        {
            try
            {
                var query = @"Select Count(*) From d_bobbin_lct_hist where bb_no = @Bb_No And mt_cd = @Mt_Cd";
                int result = await base.DbConnection.ExecuteAsync(query, new { @Bb_No = bb_no, @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> CheckExistedMaterialMappingMMS(string mt_lot, string mt_cd)
        {
            try
            {
                var query = @"Select Count(*) From w_material_mapping_mms where mt_lot = @Mt_Lot And mt_cd = @Mt_Cd";
                int result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Mt_Lot = mt_lot, @Mt_Cd = mt_cd });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> CalQuantityForActual(int id_actual)
        {
            try
            {
                var query = @"Select sum(real_qty) as Actual 
						    From w_material_info_mms 
						    where id_actual = @Id_Actual And (orgin_mt_cd IS NULL OR orgin_mt_cd = '')";
                int result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Id_Actual = id_actual });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<WMaterialInfoNew> GetWMaterialInfo(int id)
        {
            try
            {
                string QuerySQL = "SELECT * FROM w_material_info_mms WHERE wmtid = @wmtid";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<WMaterialInfoNew>(QuerySQL, new { wmtid = id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public async Task<int> UpdateWMaterialInfoDescription(int wmtid, string description)
        {
            try
            {
                string sqlquery = @"UPDATE w_material_info_mms SET description=@description WHERE wmtid=@wmtid";
                var result = await base.DbConnection.ExecuteAsync(sqlquery, new { wmtid = wmtid, description = description });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<IReadOnlyList<ProductProcess>> GetProductProcesses(string product)
        {
            try
            {
                string data = @"SELECT a.process_code, a.process_name FROM product_routing AS a WHERE a.style_no = @product  ORDER BY a.IsApply DESC  ;";

                var result = await base.DbConnection.QueryAsync<ProductProcess>(data, new { product = product });
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<ActualPrimaryModify> GetPoInfo(int id_actualpr)
        {
            //string QuerySQL = "SELECT * FROM w_actual_primary WHERE id_actualpr = @1";
            //var a = _db.Database.SqlQuery<ActualPrimaryModel>(QuerySQL, new MySqlParameter("1", id_actualpr)).FirstOrDefault();

            //return a;

            try
            {
                string data = @"SELECT * FROM w_actual_primary WHERE id_actualpr = @id_actualpr";

                var result = await base.DbConnection.QueryFirstOrDefaultAsync<ActualPrimaryModify>(data, new { id_actualpr = id_actualpr });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> CheckDeletePO(string at_no)
        {
            //string sql = @"SELECT CASE WHEN EXISTS(
            //               SELECT w.id_actual
            //               FROM w_actual w
            //               WHERE w.at_no = @1 AND  (w.actual <> 0  OR w.id_actual
            //               IN (SELECT id_actual
            //               FROM w_material_info 
            //               WHERE  id_actual IN (SELECT a.id_actual
            //               FROM w_actual AS a
            //               WHERE a.at_no = @1 )))) THEN TRUE
            //               ELSE FALSE
            //               END ";

            //bool result = _db.Database.SqlQuery<bool>(sql, new MySqlParameter("1", at_no)).First();
            //return result;

            try
            {
                string sql = @"SELECT CASE WHEN EXISTS(
                           SELECT w.id_actual
                           FROM w_actual w
                           WHERE w.at_no = @at_no AND  (w.actual <> 0  OR w.id_actual
                           IN (SELECT id_actual
                           FROM w_material_info_mms 
                           WHERE  id_actual IN (SELECT a.id_actual
                           FROM w_actual AS a
                           WHERE a.at_no = @at_no )))) THEN 1
                           ELSE 0
                           END ";

                var result = await base.DbConnection.QueryFirstOrDefaultAsync<bool>(sql, new { at_no = at_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public async Task<bool> DeleteMachine(string id_actual)
        {
            //string sql = @"DELETE FROM d_pro_unit_mc 
            //                WHERE id_actual IN (SELECT id_actual
            //                FROM w_actual 
            //                WHERE at_no = '@1');";
            //_db.Database.ExecuteSqlCommand(sql, new MySqlParameter("1", id_actual));


            try
            {
                string sql = @"DELETE FROM d_pro_unit_mc 
                            WHERE id_actual IN (SELECT id_actual
                            FROM w_actual 
                            WHERE at_no = '@id_actual'); ";

                var result = await base.DbConnection.QueryFirstOrDefaultAsync<bool>(sql, new { id_actual = id_actual });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> DeleteStaff(string id_actual)
        {
            //string sql = @"DELETE  FROM d_pro_unit_staff 
            //                 WHERE id_actual IN (SELECT id_actual
            //                 FROM w_actual 
            //                 WHERE at_no = '@1')";
            //_db.Database.ExecuteSqlCommand(sql, new MySqlParameter("1", id_actual));


            try
            {
                string sql = @"DELETE  FROM d_pro_unit_staff 
                             WHERE id_actual IN (SELECT id_actual
                             FROM w_actual 
                             WHERE at_no = '@id_actual'); ";

                var result = await base.DbConnection.QueryFirstOrDefaultAsync<bool>(sql, new { id_actual = id_actual });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<bool> DeleteProcess(string id_actual)
        {
            //string sql = @" DELETE FROM w_actual WHERE at_no = @1";
            //_db.Database.ExecuteSqlCommand(sql, new MySqlParameter("1", id_actual));


            try
            {
                string sql = @"DELETE FROM w_actual WHERE at_no = @id_actual ";

                var result = await base.DbConnection.QueryFirstOrDefaultAsync<bool>(sql, new { id_actual = id_actual });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Phong Vu
        public async Task<DateTime> GetTimeOfMaxContainerByStaffId(int psid)
        {
            try
            {
                var query = @"Select Top 1 reg_date From w_material_info_mms As a JOIN d_pro_unit_staff As b On a.id_actual = b.id_actual And b.psid = @Id
                             where a.orgin_mt_cd IS NULL And a.reg_date Between b.start_dt And b.end_dt
                             Order by a.reg_date Desc";
                DateTime result = await base.DbConnection.ExecuteScalarAsync<DateTime>(query, new { @Id = psid });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<DateTime> GetTimeOfMinContainerByStaffId(int psid)
        {
            try
            {
                var query = @"Select Top 1 reg_date From w_material_info_mms As a JOIN d_pro_unit_staff As b On a.id_actual = b.id_actual And b.psid = @Id
                             where a.orgin_mt_cd IS NULL And a.reg_date Between b.start_dt And b.end_dt
                             Order by a.reg_date Asc";
                DateTime result = await base.DbConnection.ExecuteScalarAsync<DateTime>(query, new { @Id = psid });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<DateTime> GetTimeOfMaxContainerByMachineId(int pmid)
        {
            try
            {
                var query = @"Select Top 1 reg_date From w_material_info_mms As a JOIN d_pro_unit_mc As b On a.id_actual = b.id_actual And b.pmid = @Id
                             where a.orgin_mt_cd IS NULL And a.reg_date Between b.start_dt And b.end_dt
                             Order by a.reg_date Desc";
                DateTime result = await base.DbConnection.ExecuteScalarAsync<DateTime>(query, new { @Id = pmid });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<DateTime> GetTimeOfMinContainerByMachineId(int pmid)
        {
            try
            {
                var query = @"Select Top 1 reg_date From w_material_info_mms As a JOIN d_pro_unit_mc As b On a.id_actual = b.id_actual And b.pmid = @Id
                             where a.orgin_mt_cd IS NULL And a.reg_date Between b.start_dt And b.end_dt
                             Order by a.reg_date Asc";
                DateTime result = await base.DbConnection.ExecuteScalarAsync<DateTime>(query, new { @Id = pmid });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<IEnumerable<WActualBom>> GetListmaterialbomdetail(string product, string at_no, string mt_no, string shift_dt, string shift_name)
        {
            try
            {
                IEnumerable<WActualBom> datas = null;
                var query = @"select ROW_NUMBER() OVER (ORDER BY TABLE1.shift_dt DESC) AS stt, TABLE1.mt_no MaterialNo ,table1.mt_nm MaterialName,sum(table1.gr_qty) mLieu,COUNT(TABLE1.material_code) AS SoCuonNVL, TABLE1.shift_dt, TABLE1.shift_name
                                    FROM  (
                                    SELECT  (a.mt_no) mt_no,dm.mt_nm,(a.material_code)material_code ,a.gr_qty, min(b.mapping_dt) mapping_dt2,
                                     (
                                    CASE 
                                    WHEN ('08:00:00' <= FORMAT( CAST(min(b.mapping_dt) AS datetime ),'HH:mm:ss') AND  FORMAT( CAST( min(b.mapping_dt) AS datetime ),'HH:mm:ss')  <  '23:59:00') THEN
                                    FORMAT( CAST( min(b.mapping_dt) AS DATETIME ),'yyyy-MM-dd')

                                    when (FORMAT( CAST( min(b.mapping_dt) AS datetime ),'HH:mm:ss')  < '08:00:00') THEN  FORMAT(DATEADD(day,-1,min(b.mapping_dt)),'yyyy-MM-dd')
                                        ELSE ''
                                    END )  as shift_dt,
                                     (
                                    CASE 
	                                    WHEN ('08:00:00' <= CAST( min(b.mapping_dt) AS  time(0)) AND CAST(min(b.mapping_dt) AS time(0))  <=  '20:00:00') THEN N'Ca ngay' 
	                                    WHEN CAST(min(b.mapping_dt) AS time(0)) >= '20:00:00' AND  CAST(min(b.mapping_dt) AS time(0)) <= '23:59:00'
                                            OR   ( CAST( min(b.mapping_dt) AS time(0) )  < '08:00:00')
                                        THEN  N'Ca dem' 
                                    ELSE '' END )  as shift_name
                                    FROM inventory_products AS a
                                    join w_actual as w_ac
                                    on a.id_actual = w_ac.id_actual
                                    join d_material_info dm on a.mt_no = dm.mt_no
                                    JOIN w_material_mapping_mms AS b 
                                    ON a.material_code = b.mt_cd
                                    where a.mt_type ='PMT' and w_ac.at_no = @At_No

                                  GROUP BY a.mt_no,dm.mt_nm, a.material_code, a.gr_qty

                                     ) AS TABLE1

                                      GROUP BY TABLE1.mt_no,  TABLE1.mt_nm, TABLE1.shift_dt, TABLE1.shift_name
                                    ORDER BY  TABLE1.shift_dt DESC , TABLE1.shift_name desc,TABLE1.mt_no 

                                ";
                 datas = await base.DbConnection.QueryAsync<WActualBom>(query, new { Product = product, At_No = at_no });

                if (datas.Count() > 0)
                {
                    if (string.IsNullOrEmpty(shift_dt) == false)
                    {
                        datas = datas.Where(item => item.shift_dt == shift_dt);
                    }

                    if (string.IsNullOrEmpty(shift_name) == false)
                    {
                        datas = datas.Where(item => item.shift_name.Contains(shift_name));
                    }
                    if (string.IsNullOrEmpty(mt_no) == false)
                    {
                        datas = datas.Where(item => item.MaterialNo.Contains(mt_no));
                    }
                }
                return datas;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<IReadOnlyList<w_material_down>> GetDetailMaterialNG(string mt_cd)
        {
            try
            {
                var query = @"select FORMAT(CAST(reg_dt as datetime),'yyyy-MM-dd') as reg_date,mt_cd,gr_qty,gr_down,reason,reg_id from w_material_down where mt_cd =@mt_cd";
                var result = await base.DbConnection.QueryAsync<w_material_down>(query, new { @mt_cd = mt_cd });
                return result.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

      
    }
}











