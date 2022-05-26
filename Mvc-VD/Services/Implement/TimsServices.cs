using Mvc_VD.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Mvc_VD.Respositories.Irepository;
using Mvc_VD.Models.Request;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models.TIMS;
using Mvc_VD.Models.WOModel;
using Mvc_VD.Models;
using Mvc_VD.Models.DTO;
using Mvc_VD.Models.Response;
using System.Web.Http;
using System.Text;
using Mvc_VD.Classes;
using Mvc_VD.Models.FG;
using Mvc_VD.Models.WIP;

namespace Mvc_VD.Services.Implement
{
	public class TimsServices : DbConnection1RepositoryBase, ITimsService
	{
		public TimsServices(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
		{

		}
		public async Task<IReadOnlyList<DatawActualPrimaryResponse>> getListPO(DataWActualPrimaryReq item)
		{
            try
            {



                string sql = @"
                								select Distinct at_no,CountProcess
                into #tmp
                from 
                (
                select  wa.at_no,wa.id_actual,
                (case
                when ustaff.statusstaff=1 then  1
                when ustaff.statusstaff=0  then  0
                --when ustaff.statusstaff is null then 0
                else 0
                end ) CountProcess
                from w_actual wa
                left join (
                select id_actual ,max(statusstaff) as statusstaff
                from(
                select id_actual,
                case
                when (GETDATE() between start_dt and end_dt) then 1
                else 0
                end statusstaff
                from d_pro_unit_staff) tmpstaff
                 group by id_actual) ustaff on ustaff.id_actual=wa.id_actual
                where wa.type='TIMS' And (@atno = '' OR @atno IS NULL OR wa.at_no LIKE '%'+@atno+'%')
                 ) tmpp
                				select 
                					max(a.id_actualpr) AS id_actualpr
                					, max(a.at_no) AS at_no
                					, max(a.type) AS type
                					, SUM(a.target) AS totalTarget
                					, max(a.target) AS target
                					, max(a.product) AS product
                					, max(a.md_cd) AS md_cd
                					, max(a.remark) AS remark
                				    , max(a.bom_type) as bom_type
                					, max(a.style_nm) AS style_nm
                					, sum(a.actual) AS actual
                					, max(a.process_count) AS process_count
                					,a.count_pr_w AS count_pr_w
                ,a.poRun

                				FROM(
                						SELECT
                							tmpa.id_actualpr AS id_actualpr,
                							tmpa.at_no AS at_no,
                							tmpb.type AS type,
                							tmpa.target AS totalTarget,
                							tmpa.target AS target,
                							tmpa.product AS product,
                							tmpc.md_cd AS md_cd,
                							tmpa.remark AS remark,
                							tmpc.style_nm AS style_nm,
                							tmpc.bom_type as bom_type,
                							tmpb.actual AS actual,
                							tmpb.name
                							,ISNULL(tmpd.process_count,0) process_count
                							,ISNULL(tmpe.count_pr_w,0) count_pr_w
                							,tmpr.poRun AS poRun
                						FROM
                							(
                							SELECT id_actualpr,finish_yn,at_no,target,product,remark,reg_id,reg_dt,chg_id,chg_dt ,IsApply 
                							FROM w_actual_primary
                							WHERE active=1 and finish_yn IN ('N','Y') AND (@atno = '' OR @atno IS NULL OR at_no LIKE '%'+@atno+'%') AND (@product ='' OR @product IS NULL OR product LIKE '%'+@product+'%') 
                							AND  ((@regdt='' AND  @enddt='') OR (@regdt IS NULL AND  @enddt IS NULL) OR (CONVERT(date, reg_dt ) BETWEEN CONVERT(date, @regdt ) AND CONVERT(date, @enddt )))
                							) tmpa 
                						LEFT JOIN(
                						 SELECT type,name,at_no,actual
                						 FROM w_actual 
                						WHERE active=1 and  (@atno = '' OR @atno IS NULL OR at_no LIKE '%'+@atno+'%') 
                						) tmpb ON tmpa.at_no = tmpb.at_no
                						LEFT JOIN (			 
                						 select a.at_no,count(a.id_actual) poRun 
from w_actual a
inner join (select id_actual,max(start_dt) start_dt,max(end_dt) end_dt  from  d_pro_unit_staff where active = 1  group by id_actual) b on a.id_actual=b.id_actual
where  a.type='TIMS'  And a.active = 1 and (GETDATE() BETWEEN b.start_dt and b.end_dt)
group by a.at_no
                						) tmpr ON tmpr.at_no=tmpa.at_no
                						INNER JOIN(SELECT style_nm,md_cd,style_no,bom_type FROM d_style_info WHERE active=1 and (@stylename ='' OR @stylename IS NULL OR style_nm LIKE '%'+@stylename+'%') and (@bom_type ='' OR @bom_type IS NULL OR bom_type LIKE '%'+@bom_type+'%') AND (@modelcode = '' OR @modelcode IS NULL OR md_cd LIKE '%'+@modelcode+'%')) tmpc ON tmpa.product=tmpc.style_no
                						LEFT JOIN (select COUNT(DISTINCT id_actual) process_count,at_no from w_actual WHERE type='TIMS'  and active=1 and (@atno = '' OR @atno IS NULL OR at_no LIKE '%'+@atno+'%') GROUP BY at_no) tmpd ON tmpd.at_no=tmpa.at_no
                						LEFT JOIN (select COUNT(DISTINCT name) count_pr_w,at_no from w_actual WHERE active=1 and actual>0  AND (@atno = '' OR @atno IS NULL OR at_no LIKE '%'+@atno+'%') GROUP BY at_no ) tmpe ON tmpe.at_no=tmpa.at_no
                				) a
                				group by a.at_no,a.count_pr_w,a.poRun--,tmpr.poorun,tmpr.CountProcess
                				order by id_actualpr DESC
                				drop table #tmp";


//                string sql = @"select 
//					max(a.id_actualpr) AS id_actualpr
//					, max(a.at_no) AS at_no
//					, max(a.type) AS type
//					, SUM(a.target) AS totalTarget
//					, max(a.target) AS target
//					, max(a.product) AS product
//					, max(a.md_cd) AS md_cd
//					, max(a.remark) AS remark
//					, max(a.style_nm) AS style_nm
//					, sum(a.actual) AS actual
//					, max(a.process_count) AS process_count
//					,a.count_pr_w AS count_pr_w
//					,a.poRun
//				FROM(
//						SELECT
//							tmpa.id_actualpr AS id_actualpr,
//							tmpa.at_no AS at_no,
//							tmpb.type AS type,
//							tmpa.target AS totalTarget,
//							tmpa.target AS target,
//							tmpa.product AS product,
//							tmpc.md_cd AS md_cd,
//							tmpa.remark AS remark,
//							tmpc.style_nm AS style_nm,
//							tmpb.actual AS actual,
//							tmpb.name
//							,ISNULL(tmpd.process_count,0) process_count
//							,ISNULL(tmpe.count_pr_w,0) count_pr_w
//							,isnull(tmpr.poRun,0) poRun
//						FROM
//							(
//							SELECT id_actualpr,finish_yn,at_no,target,product,remark,reg_id,reg_dt,chg_id,chg_dt ,IsApply 
//							FROM w_actual_primary
//							WHERE active=1 and finish_yn IN ('N','Y') AND (@atno = '' OR @atno IS NULL OR at_no LIKE '%'+@atno+'%') AND (@product ='' OR @product IS NULL OR product LIKE '%'+@product+'%') 
//							AND  ((@regdt='' AND  @enddt='') OR (@regdt IS NULL AND  @enddt IS NULL) OR (CONVERT(date, reg_dt ) BETWEEN CONVERT(date, @regdt ) AND CONVERT(date, @enddt )))
//							) tmpa 
//						LEFT JOIN(
//						 SELECT type,name,at_no,actual
//						 FROM w_actual 
//						WHERE active=1 and  (@atno = '' OR @atno IS NULL OR at_no LIKE '%'+@atno+'%') 
//						) tmpb ON tmpa.at_no = tmpb.at_no
//						LEFT JOIN (			 
//						  select a.at_no,count(a.id_actual) poRun 
//from w_actual a
//inner join (select id_actual,max(start_dt) start_dt,max(end_dt) end_dt  from  d_pro_unit_staff group by id_actual) b on a.id_actual=b.id_actual
//where  a.type='tims' --and a.at_no='PO20211129-002'
//group by a.at_no
//						) tmpr ON tmpr.at_no=tmpa.at_no
//						INNER JOIN(SELECT style_nm,md_cd,style_no FROM d_style_info WHERE active=1 and (@stylename ='' OR @stylename IS NULL OR style_nm LIKE '%'+@stylename+'%') AND (@modelcode = '' OR @modelcode IS NULL OR md_cd LIKE '%'+@modelcode+'%')) tmpc ON tmpa.product=tmpc.style_no
//						LEFT JOIN (select COUNT(DISTINCT name) process_count,at_no from w_actual WHERE active=1 and (@atno = '' OR @atno IS NULL OR at_no LIKE '%'+@atno+'%') GROUP BY at_no) tmpd ON tmpd.at_no=tmpa.at_no
//						LEFT JOIN (select COUNT(DISTINCT name) count_pr_w,at_no from w_actual WHERE active=1 and actual>0  AND (@atno = '' OR @atno IS NULL OR at_no LIKE '%'+@atno+'%') GROUP BY at_no ) tmpe ON tmpe.at_no=tmpa.at_no
//				) a
			
//				group by a.at_no,a.count_pr_w,a.poRun
//				order by id_actualpr DESC";
                //var resultSearch = await base.DbConnection.QueryAsync<DatawActualPrimary>(sql, new { whereacprimary = WhereAcPrimary,whereactual = WhereActual,wherecountproces = WhereCountProces,wherestyleinfo = WhereStyleInfo,wherecountprw = WhereCountprw});
                var result = await base.DbConnection.QueryAsync<DatawActualPrimaryResponse>(sql, new { atno = item.at_no, product = item.product, stylename = item.product_name, modelcode = item.model, regdt = item.regstart, enddt = item.regend , bom_type = item.bom_type });

			//if (!string.IsNullOrWhiteSpace(item.at_no))
			//{
			//	result = result.Where(x=>x.at_no.Contains(item.at_no));
			//}
			//if (!string.IsNullOrWhiteSpace(item.product))
			//{
			//	AcPrimary += " AND product LIKE '%" + item.product.Trim() + "%'";
			//	WhereActual += " AND product LIKE '%" + item.product.Trim() + "%'";
			//	WhereCountProces += " AND product LIKE '%" + item.product.Trim() + "%'";
			//	WhereCountprw += " AND product LIKE '%" + item.product.Trim() + "%'";
			//}
			//if (!string.IsNullOrWhiteSpace(item.product_name))
			//{

			//	WhereStyleInfo += " AND style_nm LIKE '%" + item.product_name.Trim() + "%'";

			//}
			//if (!string.IsNullOrWhiteSpace(item.model))
			//{

			//	WhereStyleInfo += " AND md_cd LIKE '%" + item.model.Trim() + "%'";
			//}
			//if (!string.IsNullOrWhiteSpace(item.reg_end) && !string.IsNullOrWhiteSpace(item.reg_start))
			//{
			//	AcPrimary += " AND CONVERT (DATE,reg_dt,101) BETWEEN CONVERT (DATE,'" + item.reg_start.Trim() + "',101) AND CONVERT (DATE,'"+item.reg_end.Trim()+"',101)";
			//}
			return result.ToList();
			}
			catch (Exception ex)
			{

				throw;
			}

		}
		public async Task<IReadOnlyList<WActualAPI>> GetActualInfo(string AtNo)
		{
            //			string sql = @"SELECT max(b.id_actual) Id,
            //						b.at_no,
            //						b.type,
            //						b.name Name,
            //						b.IsFinish,
            //						 CONVERT(DATETIME,b.date,120) Date,
            //						b.item_vcd QCCode,
            //						b.defect Defective,
            //						b.actual ActualQty,
            //						b.don_vi_pr as RollCode,
            //						a.target,c.dt_nm Name_View,case when d.dt_nm is null then 'EA' else d.dt_nm end RollName ,
            //e.item_nm QCName,
            //isnull(bang4.CountProcess,2)ProcessRun
            //								FROM (SELECT at_no,target FROM w_actual_primary WHERE at_no=@atno and active=1) a
            //								INNER JOIN (SELECT id_actual,at_no,type,name,date,item_vcd,defect,don_vi_pr,actual,IsFinish FROM w_actual where type='TIMS' and active=1 AND at_no=@atno) b ON b.at_no=a.at_no
            //								left JOIN (SELECT dt_cd,dt_nm FROM  comm_dt WHERE active=1 AND mt_cd ='COM007') c ON c.dt_cd =b.name
            //								left JOIN (SELECT dt_cd,dt_nm FROM  comm_dt WHERE active=1 AND mt_cd ='COM032') d ON d.dt_cd = b.don_vi_pr
            //								LEFT JOIN (SELECT  item_vcd,item_nm FROM qc_item_mt) e ON e.item_vcd = b.item_vcd
            //left join ( select a.at_no,a.id_actual,
            // (case when b.id_actual is null then 0 else 1 end) CountProcess 
            //from w_actual a
            //left join (select id_actual,max(start_dt) start_dt,max(end_dt) end_dt  from  d_pro_unit_staff where active = 1 group by id_actual) b on a.id_actual=b.id_actual
            //where  a.type='TIMS'  And a.active = 1 and (GETDATE() BETWEEN b.start_dt and b.end_dt)
            //) bang4 on bang4.id_actual=b.id_actual
            //						ORDER BY b.id_actual DESC";


            string sql = @"SELECT max(b.id_actual) Id,
									max(b.at_no) ,
									max(b.type),
									max(b.name) Name,
									max(b.IsFinish) as IsFinish,
															 max(CONVERT(DATETIME,b.date,120)) Date,
						max(b.item_vcd) QCCode,
						max(b.defect) Defective,
						max(b.actual) ActualQty,
						max(b.don_vi_pr) as RollCode,
						max(a.target) as target,max(c.dt_nm) Name_View,case when max(d.dt_nm) is null then 'EA' else max(d.dt_nm) end RollName ,
max(e.item_nm) QCName,
	isnull(max(bang4.CountProcess),2)ProcessRun
											FROM (SELECT at_no,target FROM w_actual_primary WHERE at_no=@atno and active=1) a
											INNER JOIN (SELECT id_actual,at_no,type,name,date,item_vcd,defect,don_vi_pr,actual,IsFinish FROM w_actual where type='TIMS' and active=1 AND at_no=@atno) b ON b.at_no=a.at_no
											left JOIN (SELECT dt_cd,dt_nm FROM  comm_dt WHERE active=1 AND mt_cd ='COM007') c ON c.dt_cd =b.name
											left JOIN (SELECT dt_cd,dt_nm FROM  comm_dt WHERE active=1 AND mt_cd ='COM032') d ON d.dt_cd = b.don_vi_pr
											LEFT JOIN (SELECT  item_vcd,item_nm FROM qc_item_mt) e ON e.item_vcd = b.item_vcd
			left join ( select a.at_no,a.id_actual,
			 (case when b.id_actual is null then 0 else 1 end) CountProcess 
			from w_actual a
			left join (select id_actual,max(start_dt) start_dt,max(end_dt) end_dt  from  d_pro_unit_staff where active = 1 group by id_actual) b on a.id_actual=b.id_actual
			where  a.type='TIMS'  And a.active = 1 and (GETDATE() BETWEEN b.start_dt and b.end_dt)
			) bang4 on bang4.id_actual=b.id_actual
group by b.id_actual
									ORDER BY b.id_actual DESC";
            var result = await base.DbConnection.QueryAsync<WActualAPI>(sql, new { @atno = AtNo });
			return result.ToList();
		}
		public async Task<IReadOnlyList<WMaterialnfo>> GetDataActualSX(string AtNo)
		{

			//string sql = @"SELECT a.wmtid,
			//            a.mt_cd mt_cd,
			//            a.bb_no,
			//            a.gr_qty,
			//            a.real_qty,
			//            c.dt_nm as mt_sts_cd,
		 //               CONVERT(DATETIME,a.recevicedttims,120) As recevice_dt_tims, 
			//            b.type
   //         FROM(		
			//select bbhist.bb_no,bbhist.mt_cd,
			//(case when infomms.id_actual is null then infotims.id_actual else infomms.id_actual end ) id_actual,
			//(case when infomms.gr_qty is null then infotims.gr_qty else infomms.gr_qty end ) gr_qty,
			//(case when infomms.real_qty is null then infotims.real_qty else infomms.real_qty end ) real_qty,
			//(case when infomms.chg_date is null then infotims.chg_date else infomms.chg_date end ) recevicedttims,
			//(case when infomms.wmtid is null then infotims.wmtid else infomms.wmtid end ) wmtid,
			//(case when infomms.status is null then infotims.status else infomms.status end ) status
			//from d_bobbin_lct_hist bbhist
			//left join w_material_info_mms infomms on infomms.material_code=bbhist.mt_cd and infomms.active = 1 AND infomms.status IN ('008','002') AND infomms.location_code LIKE '006%'
			//left join w_material_info_tims infotims on infotims.material_code=bbhist.mt_cd and infotims.active = 1 AND infotims.status IN ('008','002') AND infotims.location_code LIKE '006%'
			//--where bbhist.bb_no='AUTO-TRAY-20210519202603000009'
			//) a
			//INNER JOIN (SELECT id_actual,type FROM w_actual WHERE active=1 and at_no=@atno) b ON a.id_actual = b.id_actual
   //         INNER JOIN (SELECT dt_cd,dt_nm FROM comm_dt WHERE  mt_cd = 'WHS005') c ON c.dt_cd = a.status";

			string sql = @"SELECT 
			            c.dt_nm as mt_sts_cd,
		                (case when infomms.chg_date is null then CONVERT(DATETIME,infotims.chg_date,120) else CONVERT(DATETIME,infomms.chg_date,120)  end ) As recevice_dt_tims, 
			            a.type,
			(case when infomms.bb_no is null then infotims.bb_no else infomms.bb_no end ) bb_no,
			(case when infomms.material_code is null then infotims.material_code else infomms.material_code end ) mt_cd,
						a.id_actual,
						(case when infomms.id_actual is null then infotims.id_actual else infomms.id_actual end ) id_actual,
			(case when infomms.gr_qty is null then infotims.gr_qty else infomms.gr_qty end ) gr_qty,
			(case when infomms.real_qty is null then infotims.real_qty else infomms.real_qty end ) real_qty,
			(case when infomms.chg_date is null then infotims.chg_date else infomms.chg_date end ) recevicedttims,
			(case when infomms.wmtid is null then infotims.wmtid else infomms.wmtid end ) wmtid,
			(case when infomms.status is null then infotims.status else infomms.status end ) status
            FROM(	SELECT id_actual,type FROM w_actual WHERE active=1 and at_no=@atno	
			
			) a
			LEFT JOIN w_material_info_mms infomms on infomms.active = 1 AND infomms.status IN ('008','002') AND infomms.location_code LIKE '006%' AND a.id_actual = infomms.id_actual
			LEFT JOIN w_material_info_tims infotims on  infotims.active = 1 AND infotims.status IN ('008','002') AND infotims.location_code LIKE '006%' AND a.id_actual = infotims.id_actual
            INNER JOIN (SELECT dt_cd,dt_nm FROM comm_dt WHERE  mt_cd = 'WHS005') c ON (c.dt_cd = infomms.status OR c.dt_cd = infotims.status)";
			var result = await base.DbConnection.QueryAsync<WMaterialnfo>(sql, new { atno = AtNo });
			return result.ToList();

		}
		public async Task<DBobbinInfo> FindOneDBobbinInfo(string bb_no)
		{
			string sql = @"select bno,mc_type,bb_no,mt_cd,bb_nm,purpose,barcode,re_mark,
                           use_yn,del_yn,reg_id,reg_dt,chg_id,chg_dt 
                           from d_bobbin_info where bb_no = @bbno AND active=1";
			var result = await base.DbConnection.QueryFirstOrDefaultAsync<DBobbinInfo>(sql, new { bbno = bb_no });
			return result;
		}
		public async Task<DBobbinInfo> FindOneDBobbinInfomtcd(string bb_no,string mtcd)
		{
			string sql = @"select bno,mc_type,bb_no,mt_cd,bb_nm,purpose,barcode,re_mark,
                           use_yn,del_yn,reg_id,reg_dt,chg_id,chg_dt 
                           from d_bobbin_info where bb_no = @bbno and mt_cd=@mtcd AND active=1";
			var result = await base.DbConnection.QueryFirstOrDefaultAsync<DBobbinInfo>(sql, new { bbno = bb_no , mtcd = mtcd });
			return result;
		}
		public async Task<Models.NewVersion.MaterialInfoMMS> FindOneMaterialInfoByMTCdBBNo(string mt_cd, string bb_no)
		{
			string sql = @"Select * from w_material_info_mms where material_code =@mtcd AND bb_no=@bbno AND active=1";
			var Result = await base.DbConnection.QueryFirstOrDefaultAsync<Models.NewVersion.MaterialInfoMMS>(sql, new { mtcd = mt_cd, bbno = bb_no });
			return Result;
		}
		public async Task<int> UpdateMTQR_RDList(int wmtid, string user)
		{
			string sql = @"UPDATE w_material_info_mms  SET status = '008', location_code = '006000000000000000' ,chg_id =@chgid  ,chg_date =GETDATE() ,recevice_dt_tims =GETDATE()
                WHERE wmtid = @wmtid";
			return await base.DbConnection.ExecuteAsync(sql, new { wmtid = wmtid, chgid = user });
		}
		public async Task<IReadOnlyList<TimsReceivingScanModel>> GetListMaterialTimsReceivingPO(string po_no, string product, string input_dt, string bb_no)
		{
			try
			{
				string sql = @"select dt_cd,dt_nm into #tmpdt from comm_dt where mt_cd in('WHS005','COM004')
				SELECT
					a.wmtid AS wmtid,
					a.id_actual AS id_actual,
					a.material_code AS mt_cd,
					a.bb_no AS bb_no,
					a.gr_qty AS gr_qty,
					a.from_lct_code AS from_lct_cd,
					'101' AS lct_sts_cd,
					a.status AS mt_sts_cd,
					tmpa.at_no AS at_no,
					tmpa.product AS product,
					tmpsts.dt_nm AS sts_nm,
					tmpnm.dt_nm AS mt_type_nm,
					convert(datetime,a.chg_date,120) input_dt,
					--'2021/06/22 15:46:27.000' AS input_dt,
					--convert(varchar, a.chg_date, 23) input_dt,
					(SELECT x.lct_nm FROM lct_info x WHERE( x.lct_cd = a.location_code )) AS from_lct_nm 
				FROM
					w_material_info_mms a 
				inner join (select c.id_actual,c.at_no,d.product
				from w_actual c 
				inner join(
				select a.at_no,a.product,MAX(b.level) lvmax
				from w_actual_primary a
				inner join w_actual b ON a.at_no=b.at_no
				where b.type='SX' AND (@pono='' OR @pono IS NULL OR a.at_no LIKE '%'+@pono+'%') AND (@product='' OR @product IS NULL OR a.product LIKE '%'+@product+'%') AND b.active=1
				group by a.at_no,a.product) d on c.at_no=d.at_no and c.level=d.lvmax
				where c.type='SX') tmpa on tmpa.id_actual=a.id_actual
				left join #tmpdt tmpsts on tmpsts.dt_cd= a.status
				left join #tmpdt tmpnm on tmpnm.dt_cd= a.material_type
				WHERE
					(( a.location_code LIKE '002%' ) 
						AND ((a.status = '001' ) OR ( a.status = '002' )) 
						AND EXISTS (
						SELECT
							d_bobbin_lct_hist.mt_cd 
						FROM
							d_bobbin_lct_hist 
						WHERE ( d_bobbin_lct_hist.mt_cd = a.material_code )) AND ( a.gr_qty > 0 )
						AND (@bbno='' OR @bbno IS NULL OR a.bb_no LIKE '%'+@bbno+'%') AND (@inputdt='' OR @inputdt IS NULL OR convert(varchar, a.chg_date, 23)=@inputdt))
order by a.wmtid DESC
				drop table  #tmpdt";
				var result = await base.DbConnection.QueryAsync<TimsReceivingScanModel>(sql, new { bbno = bb_no, inputdt = input_dt, pono = po_no, product = product });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<IReadOnlyList<ListWorker>> GetListWorker(String UserId, string UserName, string PositionCode)
		{
			//string sql = @"SELECT
			//	tmpa.userid UserId,
			//	tmpa.uname UserName,
			//	tmpc.dt_nm PositionName,
			//	tmpa.position_cd PositionCode,
			//	tmpa.nick_name
			//	,tmpb.mc_no
			//FROM mb_info tmpa 
			//LEFT JOIN (	select a.staff_id,b.mc_no
			//	from d_pro_unit_staff a
			//	inner join d_pro_unit_mc b on a.id_actual=b.id_actual
			//	where a.active = 1 AND (GETDATE() between a.start_dt and a.end_dt) AND (GETDATE() between b.start_dt and b.end_dt) AND (@userid='' OR @userid IS NULL OR a.staff_id LIKE ''+@userid+'')) tmpb ON tmpb.staff_id=tmpa.userid
			//INNER JOIN (SELECT dt_nm,dt_cd FROM comm_dt WHERE mt_cd = 'COM018' AND active=1) tmpc ON tmpc.dt_cd=tmpa.position_cd
			//WHERE
			//	tmpa.lct_cd = 'staff'  AND tmpa.active=1
			//	AND ( @userid = '' OR @userid IS NULL OR tmpa.userid LIKE '%'+@userid+'%' ) 
			//	AND ( @uname = '' OR @uname IS NULL OR tmpa.uname LIKE '%'+@uname+'%' ) 
			//	AND (@postioncode = '' OR @postioncode IS NULL OR tmpa.position_cd LIKE '%'+@postioncode+'%' )
			//ORDER BY tmpa.userid DESC";

			string sql = @"SELECT * FROM ( SELECT (SELECT dt_nm 
        FROM   comm_dt 
        WHERE  mt_cd = 'COM018' 
               AND dt_cd = a.position_cd) AS PositionName, 
       a.userid UserId,ROW_NUMBER() OVER(
ORDER BY a.userid DESC) AS RowNum ,
       a.uname UserName, a.position_cd PositionCode, 
       a.nick_name, 
       (SELECT top 1 c.mc_no 
        FROM   d_pro_unit_staff AS b 
               LEFT JOIN d_pro_unit_mc AS c 
                      ON b.id_actual = c.id_actual 
        WHERE  a.userid = b.staff_id 
               AND c.mc_no IN (SELECT d.mc_no 
                               FROM   d_machine_info AS d) 
        ORDER  BY c.chg_dt DESC 
        )                         AS mc_no
FROM   mb_info AS a Where a.lct_cd='staff' and  (@userid='' OR @userid is null OR  a.userid like '%'+@userid+'%' ) AND (@uname='' OR @uname is null OR  a.uname like '%'+@uname+'%' ) AND (@postioncode='' OR @postioncode is null OR  a.position_cd like '%'+@postioncode+'%' ) ) MyDerivedTable  
WHERE MyDerivedTable.RowNum BETWEEN 1 AND 50  ORDER BY MyDerivedTable.userid DESC";
			var result = await base.DbConnection.QueryAsync<ListWorker>(sql, new { userid = UserId, uname = UserName, postioncode = PositionCode });
			return result.ToList();
		}
		public async Task<IReadOnlyList<DProUnitStaffAPI>> FindDProUnitStaffByStaffIdIdActual(int id_actual, string staff_id)
		{
			string sql = @"SELECT 
                            psid,staff_id,actual,defect,id_actual,staff_tp,Convert(varchar(23),convert(datetime,start_dt,121),121) start_dt,convert(varchar(23),convert(datetime,end_dt,121),121) end_dt,
                            use_yn,del_yn,reg_id,reg_dt,chg_id,chg_dt
                            FROM d_pro_unit_staff
                            Where id_actual = @idactual AND staff_id= @staffid and active=1";
			var result = await base.DbConnection.QueryAsync<DProUnitStaffAPI>(sql, new { idactual = id_actual, staffid = staff_id });
			return result.ToList();
		}
		public async Task<IReadOnlyList<ListStaffTims>> GetTIMSListStaff(int id_actual, string staff_id, string StartDate, string EndDate,string staffName)
		{
			string sql = @"SELECT
				w.at_no,
				a.psid,
				a.staff_id,
				a.actual ActualQty,
				a.use_yn,
				a.staff_tp,
				b.uname uname,
				( a.defect ) Defective,
				 a.reg_dt ,
				a.chg_dt ,
				convert(varchar, a.start_dt, 120) start_dt,
				convert(varchar, a.end_dt, 120) end_dt ,
				(CASE
						WHEN GETDATE() BETWEEN a.start_dt AND a.end_dt
						THEN 'HT' 
						ELSE 'QK' 
					END ) het_ca 
				FROM d_pro_unit_staff AS a
				LEFT JOIN w_actual w ON w.active=1 AND w.id_actual = a.id_actual 
				LEFT JOIN mb_info b ON B.active=1 AND b.userid = a.staff_id 
				WHERE a.id_actual= @idactual AND a.active=1
					AND (@staffid='' OR @staffid IS NULL OR a.staff_id LIKE '%'+@staffid+'%' )
					AND (@staffname='' OR @staffname IS NULL OR b.uname LIKE '%'+@staffname+'%' )
					AND (@startdt='' OR @startdt IS NULL OR a.start_dt >= @startdt )
					AND (@endt='' OR @endt IS NULL OR a.end_dt <= @endt )
					order by a.reg_dt desc";
			var result = await base.DbConnection.QueryAsync<ListStaffTims>(sql, new { idactual = id_actual, staffid = staff_id, startdt = StartDate, endt = EndDate, staffname = staffName });
			return result.ToList();
		}
		public async Task<bool> CheckExistsWorker(string StaffId)
		{
			string sql = @"SELECT userid FROM mb_info WHERE active = 1 AND userid = @staffid";
			string userid = await base.DbConnection.QueryFirstOrDefaultAsync<string>(sql, new { staffid = StaffId });
			return string.IsNullOrEmpty(userid);
		}
		public async Task<string> FindDProUnitStaffByStaffId(int psid, string staff_id, int id_actual)
		{
			try
			{
				string sql = @" SELECT TOP 1
								convert(varchar, start_dt, 121) start_dt
								FROM d_pro_unit_staff
								Where psid != @psid AND staff_id= @staffid AND id_actual=@idactual";
				return await base.DbConnection.QueryFirstOrDefaultAsync<string>(sql, new { psid = psid, staffid = staff_id, idactual = id_actual });
			}
			catch (Exception e)
			{

				throw;
			}
		}
		public async Task<DProUnitStaffAPI> FindOneDProUnitStaffById(int psid)
		{
			try
			{
				string sql = @" SELECT 
								psid as psid,staff_id as staff_id,actual as actual,defect as defect,id_actual as id_actual,staff_tp as staff_tp,convert(varchar, start_dt, 121) start_dt,
								convert(varchar, end_dt, 121) end_dt ,
								use_yn as use_yn,del_yn as del_yn ,reg_id as reg_id,reg_dt as reg_dt,chg_id as chg_id,chg_dt as chg_dt
								FROM d_pro_unit_staff
								Where psid = @psid";
				return await base.DbConnection.QueryFirstOrDefaultAsync<DProUnitStaffAPI>(sql, new { psid = psid });
			}
			catch (Exception e)
			{

				throw;
			}

		}
		public async Task<bool> CheckExistsDuplicateTime(string staff_id, string start, string end, int psid)
		{
            try
            {


				string sql = @"SELECT psid 
					FROM d_pro_unit_staff AS a 
					WHERE a.staff_id = @staffid
					AND (CONVERT(datetime,a.start_dt,120) >= CONVERT(datetime,@startdt,120) )
					AND( CONVERT(datetime,a.start_dt,120) <= CONVERT(datetime,@enddt,120))
					AND a.psid != @psid and a.active=1";
				return string.IsNullOrEmpty(await base.DbConnection.QueryFirstOrDefaultAsync<string>(sql, new { staffid = staff_id, startdt = start, enddt = end, psid = psid }));
			}
			catch (Exception EX)
			{

				throw;
			}
		}
		//public async Task<bool> CheckExistsDuplicateTimeOQC(string staff_id, string start, string end, int psid)
		//{
		//	string sql = @"SELECT psid 
		//		FROM d_pro_unit_staff AS a 
		//		WHERE a.staff_id = @staffid
		//		AND (CONVERT(datetime,a.start_dt,120) >= CONVERT(datetime,@startdt,120) )
		//		AND( CONVERT(datetime,a.start_dt,120) <= CONVERT(datetime,@enddt,120))
		//		AND a.psid = @psid ";
		//	return string.IsNullOrEmpty(await base.DbConnection.QueryFirstOrDefaultAsync<string>(sql, new { staffid = staff_id, startdt = start, enddt = end, psid = psid }));
		//}
		public async Task<int> UpdateDProUnitStaff(DProUnitStaffAPI item)
		{
            try
            {
				string sql = @"Update d_pro_unit_staff SET start_dt=@startdt, end_dt=@enddt, staff_id=@staffid,use_yn=@useyn,del_yn=@delyn ,id_actual=@idactual,chg_id=@chgid,chg_dt=GETDATE() where  psid = @psid  ";
				return await base.DbConnection.ExecuteAsync(sql, new
				{
					startdt = item.start_dt,
					enddt = item.end_dt,
					staffid = item.staff_id,
					useyn = item.use_yn,
					delyn = item.del_yn,
					idactual = item.id_actual,
					chgid = item.chg_id,
					psid = item.psid
				});
			}
            catch (Exception ex)
            {

                throw;
            }

			//return db.Database.ExecuteSqlCommand(QuerySQL,
			//	new MySqlParameter("1", item.start_dt),
			//	new MySqlParameter("2", item.end_dt),
			//	new MySqlParameter("3", item.staff_id),
			//	new MySqlParameter("4", item.use_yn),
			//	new MySqlParameter("5", item.del_yn),
			//	new MySqlParameter("6", item.psid),
			//	new MySqlParameter("7", item.id_actual)
			//	);
		}
		public async Task<Models.TIMS.WActual> FindOneWActual(int id_actual)
		{
            try
            {
				string sql = @"select id_actual ,at_no,type,
                         actual,defect,name,level,date,don_vi_pr,item_vcd,reg_id,reg_dt,chg_id,chg_dt,product
                         from w_actual where id_actual = @idactual";
				 var result = await base.DbConnection.QueryFirstOrDefaultAsync<Models.TIMS.WActual>(sql, new { idactual = id_actual });
				return result;
			}
			catch(Exception e)
            {
				throw e;
            }

		}
		public async Task<IReadOnlyList<WMaterialInfoTIMSAPIRec>> GetDetailActualAPIOQC(int id_actual, string date, string shift)
		{
            try {
				string sqlquery = @" 
                                SELECT * FROM ( 
                                SELECT c.psid stt, c.id_actual ,mb.uname staff_name, c.actual AS realQty, c.defect AS defectQty, c.staff_id,
                                     FORMAT( CAST( c.start_dt AS datetime ),'yyyy-MM-dd HH:mm:ss') start_dt,
                                      FORMAT( CAST( c.end_dt AS datetime ),'yyyy-MM-dd HH:mm:ss') end_dt, 
 
                                                     (
                                                    CASE 
                                                    WHEN ('08:00:00' <= FORMAT( CAST( c.start_dt AS datetime ),'HH:mm:ss') AND  FORMAT( CAST( c.start_dt AS datetime ),'HH:mm:ss')  <  '23:59:00') THEN
                                                    FORMAT( CAST( c.start_dt AS DATETIME ),'yyyy-MM-dd')

                                                    when (FORMAT( CAST( c.start_dt AS datetime ),'HH:mm:ss')  < '08:00:00') THEN  FORMAT(DateAdd(DAY,-1,c.start_dt), 'yyyy-MM-dd')
                                                      ELSE ''
                                                    END )  as shift_dt,
                                                    (
                                                    CASE 
                                                    WHEN ('08:00:00' <= FORMAT( CAST( c.start_dt AS datetime ),'HH:mm:ss') AND  FORMAT( CAST( c.start_dt AS datetime ),'HH:mm:ss')  <  '20:00:00') THEN
                                                    'Ca ngay'
                                                    WHEN
                                                    (FORMAT( CAST( c.start_dt AS datetime ),'HH:mm:ss') >= '20:00:00' AND FORMAT( CAST( c.start_dt AS datetime ),'yyyy-MM-dd') <= '23:59:00' OR 

                                                    FORMAT( CAST( c.start_dt AS datetime ),'HH:mm:ss')  < '08:00:00')
                                                     THEN  'Ca dem'
                                                      ELSE ''
                                                    END )  as shift_name

                                    FROM d_pro_unit_staff AS c
                                    LEFT JOIN mb_info AS mb ON c.staff_id = mb.userid

                                    WHERE c.id_actual =@id_actual
                                   
                                     ) AS TABLE1  
                WHERE   (@date='' OR   TABLE1.shift_dt like @date)  
                AND   (@shift='' OR   TABLE1.shift_name like @shift)  

                                 GROUP BY TABLE1.staff_id, TABLE1.shift_dt,TABLE1.stt,TABLE1.id_actual,TABLE1.staff_name,TABLE1.realQty,TABLE1.defectQty,TABLE1.start_dt,TABLE1.end_dt,TABLE1.shift_name
                     ORDER BY  TABLE1.shift_dt DESC ,TABLE1.shift_name    ";


				//var result = db.Database.SqlQuery<WMaterialInfoTIMSAPI>(sqlquery, new MySqlParameter("1", id_actual));
				var result = await base.DbConnection.QueryAsync<Models.TIMS.WMaterialInfoTIMSAPIRec>(sqlquery, new { id_actual = id_actual, date = date, shift = shift });

				return result.ToList();
			}
			catch(Exception e)
            {
				throw e;
            }

		}



		public async Task<int> CheckShiftWithPisd(int psid)
		{
			string sql = @"SELECT count(psid) FROM d_pro_unit_staff WHERE  
                            psid= @psid AND datediff(MINUTE,CONVERT(DATETIME,start_dt,120),CONVERT(DATETIME,GETDATE(),120))>=0 and datediff(MINUTE,CONVERT(DATETIME,GETDATE(),120),CONVERT(DATETIME,end_dt,120)) >=0";
			return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { psid = psid });
		}
		public async Task<int> CheckMaterialforTims(string StaffId, int IdActual)
		{
			string sql = @"select COUNT(a.wmtid) from w_material_info_mms a
				INNER JOIN d_pro_unit_staff b on b.id_actual=@idactual and b.staff_id=@staffid and a.id_actual=b.id_actual
				WHERE a.id_actual=@idactual ";
			return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { idactual = IdActual, staffid = StaffId });
		}
		public async Task<int> CheckWMaterialInfoByStaffIdOQC(string StaffId, int IdActual)
		{
			string sql = @"SELECT 
				COUNT(a.wmtid)  
				from w_material_info_tims a
				inner join d_pro_unit_staff b on b.id_actual=@idactual and b.staff_id=@staffid and a.id_actual=b.id_actual
				Where
				a.id_actual = @idactual";
			return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { idactual = IdActual, staffid = StaffId });
		}
		public async Task<int> DeleteDProUnitStaff(int id_actual, int psid, string staff_id, string ChgId)
		{
			string sql = @"Update d_pro_unit_staff set active=0,chg_id=@chgid,chg_dt=GETDATE() where id_actual =@idactual AND psid =@psid AND staff_id = @staffid";
			return await base.DbConnection.ExecuteAsync(sql, new { idactual = id_actual, psid = psid, staffid = staff_id, chgid = ChgId });
		}
		public async Task<IReadOnlyList<d_bobbin_info>> GetListBoBinPopup(string bb_no, string bb_nm, string mt_cd, int id_actual,int intpage,int introw)
		{
            try
            {


			string sql = @"SELECT * FROM (SELECT
				a.bno,
				a.mc_type,
				a.bb_no,
				a.mt_cd,
				--(case when b.material_code is null then c.material_code
				--else b.material_code end)mt_cd,
				a.bb_nm,
				a.use_yn,
				a.purpose,
				('') barcode,
				a.re_mark,
				a.count_number,
				a.del_yn,
				a.reg_id,
				a.reg_dt,
				a.chg_id,
				a.chg_dt 
				--into #tmpaa
			FROM
				d_bobbin_info a
				--left JOIN w_material_info_mms b ON a.mt_cd = b.material_code AND b.location_code LIKE '006%' and b.active=1 and a.active=1  AND b.status IN ( '002', '008' ) AND b.gr_qty > 0
				--left JOIN w_material_info_tims c ON a.mt_cd = c.material_code AND b.location_code LIKE '006%' and b.active=1 and a.active=1  AND b.status IN ( '002', '008' ) AND b.gr_qty > 0
				where a.mt_cd in(select material_code from w_material_info_mms where  id_actual!=@idactual and location_code LIKE '006%' and active=1  AND status IN ( '002', '008' ) AND gr_qty > 0 and id_actual in(
				SELECT
					tmpa.id_actual 
				FROM
					w_actual tmpa
					inner join (select at_no from w_actual where id_actual=@idactual and active=1) tmpb on tmpb.at_no=tmpa.at_no
				where tmpa.type='SX' and tmpa.active=1 
				))
				or a.mt_cd in(select material_code from w_material_info_tims where  id_actual!=@idactual and  location_code LIKE '006%' and active=1  AND status IN ( '002', '008' ) AND gr_qty > 0 and id_actual in(
				SELECT
					tmpa.id_actual 
				FROM
					w_actual tmpa
					inner join (select at_no from w_actual where id_actual=@idactual and active=1) tmpb on tmpb.at_no=tmpa.at_no
				where tmpa.type='TIMS' and tmpa.active=1
				))
				
			
			UNION ALL
			SELECT
				a.bno,
				a.mc_type,
				a.bb_no,
				a.mt_cd,
				a.bb_nm,
				a.use_yn,
				a.purpose,
				('') barcode,
				a.re_mark,
				a.count_number,
				a.del_yn,
				a.reg_id,
				a.reg_dt,
				a.chg_id,
				a.chg_dt 
			FROM
				d_bobbin_info AS a 
			WHERE ( a.mt_cd = '' OR a.mt_cd IS NULL  )   and active=1
				
) bbb
Where ( @bbno = ''OR @bbno IS NULL OR bbb.bb_no LIKE '%'+@bbno+'%' ) 
				AND ( @bbnm = ''OR @bbnm IS NULL OR bbb.bb_nm LIKE '%'+@bbnm+'%' ) 
				AND ( @mtcd = ''OR @mtcd IS NULL OR bbb.mt_cd LIKE '%'+@mtcd+'%' ) 
			order by bbb.mt_cd DESC,bbb.reg_dt DESC
				OFFSET @intpage ROWS FETCH NEXT @introw ROWS ONLY";
			var result = await base.DbConnection.QueryAsync<d_bobbin_info>(sql, new { bbno = bb_no, bbnm = bb_nm, mtcd = mt_cd, idactual = id_actual, intpage= intpage, introw= introw });
			return result.ToList();
			}
			catch (Exception ex)
			{

				throw;
			}

		}
		public int getTotalRecordBobbinInfo()
        {
			string sql = "select count(bno) FROM d_bobbin_info where use_yn = 'Y' and active = 1";
			return base.DbConnection.QueryFirstOrDefault<int>(sql);
        }
		public async Task<DBobbinLctHist> FindOneBobbin_lct_hist(string bb_no, string mt_cd)
		{
			string sql = @"select blno, mc_type,bb_no,bb_nm,mt_cd,start_dt,end_dt,use_yn,del_yn,reg_id,reg_dt,chg_id,chg_dt 
                            from d_bobbin_lct_hist
                            where bb_no = @bbno and (@mtcd ='' OR @mtcd IS NULL or mt_cd=@mtcd) AND active=1 order by reg_dt DESC";
			return await base.DbConnection.QueryFirstOrDefaultAsync<DBobbinLctHist>(sql, new { bbno = bb_no, mtcd = mt_cd });
		}
		public async Task<int> UpdateBobbinInfo(string chg_id, string mt_cd, int bno)
		{
			string sql = @"Update d_bobbin_info set 
                                    chg_id = @chgid,
                                    mt_cd = @mtcd,
									chg_dt=GETDATE()
                                    where bno = @bno";
			return await base.DbConnection.ExecuteAsync(sql, new { chgid = chg_id, mtcd = mt_cd, bno = bno });

		}
		public async Task<WMaterialnfo> FindOneWMaterialInfoLike(string mt_cd)
		{
			string sql = @"select wmtid , id_actual ,material_type,material_code,mt_no,status mt_sts_cd,
			gr_qty,real_qty,location_code lct_cd
			from w_material_info_mms 
			where  location_code like '006%' and material_code = @mtcd";
			return await base.DbConnection.QueryFirstOrDefaultAsync<WMaterialnfo>(sql, new { mtcd = mt_cd });
			//return db.Database.SqlQuery<WMaterialnfo>(sql, new MySqlParameter("1", mt_cd)).SingleOrDefault();
		}
		public async Task<WMaterialnfo> FindOneWMaterialInfoLikeTIMS(string mt_cd)
		{
			string sql = @"select wmtid , id_actual ,material_type,material_code,mt_no,status mt_sts_cd,
			gr_qty,real_qty,location_code lct_cd
			from w_material_info_tims
			where  location_code like '006%' and material_code = @mtcd";
			return await base.DbConnection.QueryFirstOrDefaultAsync<WMaterialnfo>(sql, new { mtcd = mt_cd });
			//return db.Database.SqlQuery<WMaterialnfo>(sql, new MySqlParameter("1", mt_cd)).SingleOrDefault();
		}
		public async Task<int> UpdateMaterialStaffQty(string status, string ChgId, int wmtid, int IdActualOqc)
		{
			string sql = @"Update w_material_info_mms 
                                    SET status=@status,chg_id=@chgid,chg_date=GETDATE()
                                    Where wmtid = @wmtid";
			return await base.DbConnection.ExecuteAsync(sql, new { status = status, chgid = ChgId, wmtid = wmtid, idactualoqc = IdActualOqc });

		}
		public async Task<ActualPrimary> FindOneWActualPrimaryByAtNo(string at_no)
		{
			string sql = @"select id_actualpr,at_no,type,target,product,remark,finish_yn,reg_id,reg_dt,chg_id,chg_dt
                         from w_actual_primary  where at_no = @atno";
			return await base.DbConnection.QueryFirstOrDefaultAsync<ActualPrimary>(sql, new { atno = at_no });
		}
		public async Task<IReadOnlyList<Models.NewVersion.MaterialInfoTIMS>> FindAllMaterialByMtCdLike(string bien_first)
		{
			var sql = @"Select * from w_material_info_tims where material_code like '%'+@mtcd+'%' order by reg_date DESC";
			var result = await base.DbConnection.QueryAsync<Models.NewVersion.MaterialInfoTIMS>(sql, new { mtcd = bien_first });
			return result.ToList();
			//return db.Database.SqlQuery<MaterialInfoMMS>(sql_ds, new MySqlParameter("1", "%" + bien_first + "%"));
		}

		public async Task<IReadOnlyList<Models.NewVersion.MaterialInfoTIMS>> GetListMappingStaTims_FindMerge(string code)
		{
			var sql = @"EXEC [dbo].[usp_GetListMappingStaTims_FindMerge] @mtcd";
			var result = await base.DbConnection.QueryAsync<Models.NewVersion.MaterialInfoTIMS>(sql, new { mtcd = code });
			return result.ToList();
			//return db.Database.SqlQuery<MaterialInfoMMS>(sql_ds, new MySqlParameter("1", "%" + bien_first + "%"));
		}


		public async Task<IReadOnlyList<MaterialInfoDivideMMS>> FindAllMaterialByMtCdMGLike(string bien_first)
		{
			var sql = @"Select wmtid,@mtcd material_code_parent,material_code,material_code mt_cd,id_actual,real_qty,material_code,gr_qty,status,mt_no,bb_no,chg_id,chg_date,reg_id,reg_date 
						From w_material_info_tims where (material_code like @mtcd+'-DV%') 
						OR (material_code like @mtcd+'%' and material_code like '%MG%') order by reg_date DESC";
			var result = await base.DbConnection.QueryAsync<MaterialInfoDivideMMS>(sql, new { mtcd = bien_first });
			return result.ToList();
			//return db.Database.SqlQuery<MaterialInfoMMS>(sql_ds, new MySqlParameter("1", "%" + bien_first + "%"));
		}
		public async Task<IReadOnlyList<MaterialInfoDivideMMS>> FindAllMaterialByMtCdMGMMSLike(string bien_first)
		{
			var sql = @"Select wmtid,@mtcd material_code_parent,material_code,material_code mt_cd,id_actual,real_qty,material_code,gr_qty,status,mt_no,bb_no,chg_id,chg_date,reg_id,reg_date 
from w_material_info_mms where (material_code like @mtcd+'-DV%') OR (material_code like @mtcd+'%' and material_code like '%MG%') order by reg_date DESC";
			var result = await base.DbConnection.QueryAsync<MaterialInfoDivideMMS>(sql, new { mtcd = bien_first });
			return result.ToList();
			//return db.Database.SqlQuery<MaterialInfoMMS>(sql_ds, new MySqlParameter("1", "%" + bien_first + "%"));
		}

		public async Task<int> InsertMaterialInfoTIMMS(Models.NewVersion.MaterialInfoTIMS item)
		{
			try
			{
				var query = @"Insert Into w_material_info_tims (id_actual,staff_id, material_code, material_type, gr_qty, real_qty, status, mt_no, bb_no, location_code, from_lct_code, orgin_mt_cd, receipt_date, reg_date, reg_id, chg_date, chg_id,sts_update,at_no,product)
                                    Values (@id_actual,@staff_id, @material_code, @material_type, @gr_qty, @real_qty, @status, @mt_no, @bb_no, @location_code, 
                                            @from_lct_code, @orgin_mt_cd, GETDATE(), GETDATE(), @reg_id, GETDATE(), @chg_id,@sts_update,@at_no,@product);
                                    Select Scope_Identity()";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<int> InsertMaterialMapping(MaterialMappingTIMS data)
		{
			string sql = @"INSERT INTO w_material_mapping_tims (mt_cd,mt_lot,mt_no,mapping_dt,bb_no,use_yn,del_yn,chg_id,reg_id,reg_dt,chg_dt,remark) VALUES (@mt_cd,@mt_lot,@mt_no,GETDATE(),@bb_no,@use_yn,@del_yn,@chg_id,@reg_id,GETDATE(),GETDATE(),@remark);
                                 Select Scope_Identity();";
			return await base.DbConnection.ExecuteAsync(sql, data);

		}
		public async Task<int> UpdateMtCdBobbinHistInfo(string mt_cd, int blno)
		{
			string sql = @" Update d_bobbin_lct_hist set mt_cd = @mtcd where blno = @blno ";
			return await base.DbConnection.ExecuteAsync(sql, new { mtcd = mt_cd, blno = blno });
			//return db.Database.ExecuteSqlCommand(sql,
			//	new MySqlParameter("1", mt_cd),
			//	new MySqlParameter("2", blno));
		}
		public async Task<int> InsertBobbinHist(DBobbinLctHist bobbinhist)
		{
			string sql = @"Insert Into d_bobbin_lct_hist(bb_no,mt_cd,reg_dt,chg_dt,chg_id,reg_id,use_yn,del_yn) 
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
		public async Task<int> CheckMaterialMappingTims(string MaterialNo, string MaterialCode, string MaterilaLot)
		{
			string sql = @"SELECT COUNT(wmmid) FROM w_material_mapping_tims WHERE mt_no=@mtno AND mt_cd=@mtcd AND mt_lot=@mtlot";
			return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { mtno = MaterialNo, mtcd = MaterialCode, mtlot = MaterilaLot });
		}
		public async Task<int> CheckShift(int psid, int id_actual)
		{
			string sql = @"SELECT count(a.psid) FROM d_pro_unit_staff AS a WHERE a.psid=@psid AND a.id_actual=@idactual AND datediff(HOUR,CONVERT(DATETIME,a.start_dt,120),CONVERT(DATETIME,GETDATE(),120))>=0 and datediff(HOUR,CONVERT(DATETIME,GETDATE(),120),CONVERT(DATETIME,a.end_dt,120)) >=0";
			return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { psid = psid, idactual = id_actual });
		}
		public async Task<string> GetAtNoFromIdActual(int IdActual)
		{
			string sql = @"SELECT at_no FROM w_actual WHERE id_actual=@idactual";
			return await base.DbConnection.QueryFirstOrDefaultAsync<string>(sql, new { idactual = IdActual });
		}
		public async Task<WMaterialnfo> FindOneWMaterialInfoTims(int wmtid)
		{
			string sql = @"select 
				a.wmtid as wmtid,
				a.reg_date date,
				'' as bbmp_sts_cd,
				a.material_code as mt_cd,
				a.mt_no as mt_no,
				a.gr_qty as gr_qty,
				a.gr_qty as gr_qty1,
				-- com_dt.dt_nm ,com_dt.mt_cd as test_mt_cd,
				a.material_code mt_qrcode,
				a.location_code lct_cd ,
				l.lct_nm,a.bb_no,
				a.material_code mt_barcode,
				a.chg_date chg_dt
				,a.gr_qty as sl_tru_ng
				,a.real_qty
				from w_material_info_tims a 
				-- left join comm_dt com_dt on com_dt.dt_cd = a.bbmp_sts_cd and com_dt.mt_cd ='MMS007'
				left join lct_info l on l.lct_cd = a.location_code  
				where wmtid = @wmtid";
			return await base.DbConnection.QueryFirstOrDefaultAsync<WMaterialnfo>(sql, new { wmtid = wmtid });
		}
		public async Task<IReadOnlyList<WMaterialnfo>> FindAllWMaterialInfoByStaffIdOQC(string staff_id_oqc, int id_actual, string start_dt, string end_dt)
		{
			string sql = @"SELECT 
                                a.wmtid  as wmtid   ,
                                a.reg_date as date    ,
                                a.material_code  as mt_cd   ,
                                a.mt_no  as mt_no   ,
                                a.real_qty  as real_qty ,
                                a.gr_qty  as gr_qty     ,
                                a.gr_qty  as gr_qty1    ,
                                a.material_code as mt_qrcode ,
                                a.bb_no  as bb_no    ,
                                a.material_code as mt_barcode,
                                a.chg_date   as chg_dt 
                            from w_material_info_tims a
							inner join d_pro_unit_staff b on a.id_actual=b.id_actual 
                            WHERE status != '003'
                            and b.staff_id = '@1' AND a.id_actual =0  anda.active=1
							AND ((SELECT CONVERT(datetime,k.reg_dt,121) FROM w_material_mapping_tims k  WHERE k.mt_lot IS NULL AND a.material_code=k.mt_cd) between CONVERT(datetime,@3,121) andCONVERT(datetime,@4, 121))
							Order by reg_dt desc";
			var result = await base.DbConnection.QueryAsync<WMaterialnfo>(sql);
			return result.ToList();
		}
		public async Task<IReadOnlyList<MaterialMappingsTims>> GetListMaterialMapping(string MtLot)
		{
			try
			{
				string sql = @"SELECT
                    a.wmmid,
	                a.mt_lot,
	                a.mt_cd,
					--a.active,
	                a.sts_share,
	                a.reg_dt mapping_dt,
	                a.use_yn,
					--(case when a.active =0 then 'N'
					--else 'Y' end 
					--)use_yn,
	                a.reg_dt,
	                 (
                    CASE
                            WHEN  tmpa.gr_qty >0 THEN tmpa.gr_qty
							WHEN  tmpb.gr_qty >0 THEN tmpb.gr_qty
                            ELSE 0
                        END
		                ) 
						gr_qty,
	                b.mt_no,
	                (
                    CASE
                            WHEN  tmpa.bb_no IS NULL THEN tmpb.bb_no
							WHEN  tmpb.bb_no IS NULL THEN tmpa.bb_no
                            ELSE b.bb_no
                        END
		                ) bb_no,
	                (CASE WHEN a.use_yn = 'N' THEN b.gr_qty ELSE 0 END ) Used,
	                (
                    CASE
                            WHEN  tmpa.real_qty >0 THEN tmpa.real_qty
							WHEN  tmpb.real_qty >0 THEN tmpb.real_qty
                            ELSE 0
                        END
		                ) real_qty
                    FROM

                        w_material_mapping_tims a
                        JOIN w_material_info_tims b ON a.mt_lot = b.material_code
						left join (select real_qty,gr_qty,material_code,bb_no from w_material_info_mms) tmpb on tmpb.material_code= a.mt_cd
		                left join (select real_qty,gr_qty,material_code,bb_no from w_material_info_tims) tmpa on tmpa.material_code= a.mt_cd
                    WHERE
                        a.mt_lot = @mtlot
                    order by a.reg_dt DESC";
				var result = await base.DbConnection.QueryAsync<MaterialMappingsTims>(sql, new { @mtlot = MtLot });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}

		}
		public async Task<IReadOnlyList<WMaterialnfo>> FindAllWMaterialInfoByStaffId(string staff_id, int id_actual, string start_dt, string end_dt)
		{
			string sql = @"SELECT DISTINCT
                                a.wmtid  as wmtid   ,                            
                               a.material_code  as mt_cd ,
                                a.mt_no  as mt_no   ,
                                a.real_qty  as real_qty ,
                                a.gr_qty  as gr_qty,
                                a.gr_qty  as gr_qty1,
                                a.material_code as mt_qrcode ,
                                a.bb_no  as bb_no    ,
                                a.material_code as mt_barcode,
                                a.chg_date,
                               CONVERT(DATETIME,a.reg_date,120) as date ,
                                CONVERT(DATETIME,a.reg_date,120) as reg_dt 
                            from w_material_info_tims a
							inner join d_pro_unit_staff b on a.id_actual=b.id_actual and b.staff_id=@staffid and b.active=1 and a.staff_id=b.staff_id
                            WHERE a.status != '003' AND a.id_actual =@idactual And a.active=1
							and ( (a.reg_date between convert(datetime, @regdate,120) and convert(datetime, @enddate,120)))
                            --AND (datediff(HOUR,convert(datetime, a.reg_date,120),convert(datetime, @regdate,120))>=0 and datediff(HOUR,CONVERT(DATETIME,a.reg_date,120),convert(datetime, @enddate,120)) >=0 ) 
						and (a.orgin_mt_cd IS  NULL or a.orgin_mt_cd='')
                            --Order by reg_date desc";
			var result = await base.DbConnection.QueryAsync<WMaterialnfo>(sql, new { staffid = staff_id, idactual = id_actual, regdate = start_dt, enddate = end_dt });
			return result.ToList();
		}
		public async Task<BobbinInfo> GetOneDBobbinInfoWithMtCdIsNULL(string bb_no)
		{
			string sql = @"select * from d_bobbin_info where bb_no = @bbno AND ( mt_cd is null OR mt_cd ='' ) AND active=1";
			return await base.DbConnection.QueryFirstOrDefaultAsync<BobbinInfo>(sql, new { bbno = bb_no });
			//return db.Database.SqlQuery<d_bobbin_info>(sql_ds_bb, new MySqlParameter("1", bb_no)).SingleOrDefault();
		}
		public async Task<string> CheckMaterialMappingForTimsRoll(int IdActual, string StaffId, string material_code)
		{
			//string sql = @"	select b.mt_lot from w_material_info_tims a 
			//inner join w_material_mapping_tims b on b.mt_lot=a.material_code 
			//inner join d_pro_unit_staff c on c.id_actual=a.id_actual and a.staff_id=c.staff_id and c.active=1
			//where a.id_actual=@idactual and a.staff_id=@staffid  and b.use_yn='Y'
			//order by b.wmmid DESC";
			string sql = @"select a.material_code
				from w_material_info_tims a 
	
			where a.id_actual=@idactual and a.staff_id=@staffid  and a.material_type ='CMT' and a.material_code <> @material_code
			order by a.wmtid DESC";
			return await base.DbConnection.QueryFirstOrDefaultAsync<string>(sql, new { idactual = IdActual, staffid = StaffId, material_code= material_code },commandTimeout:60);
		}
		public async Task<int> InsertMultiMaterialMppingTims(string mtcd,string MtLot, string User, string material_code)
		{
            try
            {
				string sql = @"insert w_material_mapping_tims(mt_lot,mt_cd,mt_no,mapping_dt,bb_no,remark,sts_share,use_yn,del_yn,reg_id,reg_dt,chg_id,chg_dt,status)
				select @mtcd,a.mt_cd,a.mt_no,GETDATE(),a.bb_no,a.remark,a.sts_share,a.use_yn,a.del_yn,@user,GETDATE() ,@user,GETDATE(),a.status
				from w_material_mapping_tims a
				where mt_lot =@mtlot and mt_cd = @material_code and use_yn='Y'";
				return await base.DbConnection.ExecuteAsync(sql, new { mtcd= mtcd, mtlot = MtLot, user = User, @material_code=  material_code }, commandTimeout: 60);
			}
            catch (Exception ex)
            {

                throw;
            }

		}
		public async Task<int> UpdateMultiWmaterialInfoMMS(string MtLot, string User, int IdActualOqc)
		{
			string sql = @"update w_material_info_mms set status='002',chg_id=@user,chg_date=GETDATE(),id_actual_oqc=@idactualoqc where material_code in(SELECT mt_cd from w_material_mapping_tims where mt_lot=@mtlot and use_yn='Y' and active=1)";
			return await base.DbConnection.ExecuteAsync(sql, new { mtlot = MtLot, user = User, idactualoqc = IdActualOqc });
		}
		public async Task<int> SumGroupQtyWMaterialInfo(string mt_cd)
		{
			string sql = @"SELECT case when SUM(gr_qty) is null then 0 else SUM(gr_qty) end FROM w_material_info_tims WHERE material_code LIKE '%'+@mtcd+'%'";
			return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { mtcd = mt_cd });
		}
		public  Models.NewVersion.MaterialInfoMMS FindWMaterialInfoByMTQRCode(string mt_qrcode)
		{
            try
            {
				string sql = @"select *
								from w_material_info_tims
								where material_code = @mtcd ";
				return base.DbConnection.QueryFirstOrDefault<Models.NewVersion.MaterialInfoMMS>(sql,new { mtcd= mt_qrcode });
			}
			catch (Exception ex)
			{

				throw;
			}
		}
		public async Task<DBobbinLctHist> FindAllBobbin_lct_histByBBNo(string bb_no)
		{
			string sql = @"select top 1 blno, mc_type,bb_no,bb_nm,mt_cd,start_dt,end_dt,use_yn,del_yn,reg_id,reg_dt,chg_id,chg_dt 
                            from d_bobbin_lct_hist
                            where bb_no = @bbno AND active = 1 order by reg_dt DESC";
			return await base.DbConnection.QueryFirstOrDefaultAsync<DBobbinLctHist>(sql, new { bbno = bb_no });
			//return result.ToList();
		}
		public async Task<WMaterialnfo> FindOneWMaterialInfotypeRoll(string mt_cd)
		{
			string sql = @"select wmtid , id_actual ,material_type,material_code,mt_no,status mt_sts_cd,
			gr_qty,real_qty,location_code lct_cd
			from w_material_info_mms 
			where  location_code like '006%' and material_code = @mtcd and status in('008','002') and gr_qty >0";
			return await base.DbConnection.QueryFirstOrDefaultAsync<WMaterialnfo>(sql, new { mtcd = mt_cd });
			//return db.Database.SqlQuery<WMaterialnfo>(sql, new MySqlParameter("1", mt_cd)).SingleOrDefault();
		}

		public async Task<WMaterialnfo> FindOneWMaterialInfotypeRollFromTIMS(string mt_cd)
		{
			string sql = @"select wmtid , id_actual ,material_type,material_code,mt_no,status mt_sts_cd,
			gr_qty,real_qty,location_code lct_cd
			from w_material_info_tims 
			where  location_code like '006%' and material_code = @mtcd and status in('008','002') and gr_qty > 0";
			return await base.DbConnection.QueryFirstOrDefaultAsync<WMaterialnfo>(sql, new { mtcd = mt_cd });
			//return db.Database.SqlQuery<WMaterialnfo>(sql, new MySqlParameter("1", mt_cd)).SingleOrDefault();
		}

		public async Task<WMaterialnfo> FindOneWMaterialInfoLikeForRoll(string mt_cd)
		{
			string sql = @"select wmtid , id_actual ,material_type,material_code,mt_no,status mt_sts_cd,
			gr_qty,real_qty,location_code lct_cd
			from w_material_info_tims 
			where  location_code like '006%' and material_code = @mtcd";
			return await base.DbConnection.QueryFirstOrDefaultAsync<WMaterialnfo>(sql, new { mtcd = mt_cd });
			//return db.Database.SqlQuery<WMaterialnfo>(sql, new MySqlParameter("1", mt_cd)).SingleOrDefault();
		}
		public async Task<Models.NewVersion.MaterialInfoTIMS> GetWMaterialInfowithmtcd(string mt_lot)
		{
			string QuerySQL = "SELECT * FROM w_material_info_tims WHERE material_code = @Mt_Cd";
			return await base.DbConnection.QuerySingleOrDefaultAsync<Models.NewVersion.MaterialInfoTIMS>(QuerySQL, new { @Mt_Cd = mt_lot });
		}

		public async Task<Models.NewVersion.MaterialInfoMMS> GetWMaterialInfoMMS(string mt_cd)
		{
			string QuerySQL = "SELECT * FROM w_material_info_mms WHERE material_code = @mtcd";
			return await base.DbConnection.QuerySingleOrDefaultAsync<Models.NewVersion.MaterialInfoMMS>(QuerySQL, new { mtcd = mt_cd });
		}


		public async Task<int> CheckMaterialMapping(string mt_cd, string mt_lot)
		{
			string sqlcheckmapping = @"SELECT COUNT(a.wmmid) FROM w_material_mapping_tims a 
            WHERE a.mt_cd = @mtcd AND a.mt_lot !=@mtlot AND  a.mapping_dt >(SELECT b.mapping_dt FROM w_material_mapping_tims b WHERE b.mt_cd =@mtcd AND b.mt_lot = @mtlot)";
			return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sqlcheckmapping, new { mtcd = mt_cd, mtlot = mt_lot });
		}
		public async Task<w_material_mapping> Getmaterialmappingreturn(string mt_cd, string mt_lot)
		{
			string sqlquery = @"SELECT * FROM w_material_mapping_tims WHERE mt_cd=@mtcd AND mt_lot=@mtlot";
			return await base.DbConnection.QueryFirstOrDefaultAsync<w_material_mapping>(sqlquery, new { mtcd = mt_cd, mtlot = mt_lot });

		}
		public async Task<Actual> GetWActual(int id_actual)
		{
			string sql = @"SELECT * FROM w_actual WHERE id_actual=@idactual";
			return await base.DbConnection.QueryFirstOrDefaultAsync<Actual>(sql, new { idactual = id_actual });
		}
		public async Task<w_actual_primary> GetwactualprimaryFratno(string at_no)
		{
			string sqlquery = @"SELECT * FROM w_actual_primary WHERE at_no=@atno";
			return await base.DbConnection.QueryFirstOrDefaultAsync<w_actual_primary>(sqlquery, new { atno = at_no });
		}
		public async Task<IReadOnlyList<m_facline_qc>> Getmfaclineqc(string ml_no, string fq_no, string ml_tims)
		{
			string sqlquery = @"SELECT * FROM m_facline_qc WHERE ml_no=@mlno AND fq_no LIKE '%'+@fqno+'%' AND ml_tims=@mltims";
			var result = await base.DbConnection.QueryAsync<m_facline_qc>(sqlquery, new { mlno = ml_no, fqno = fq_no, mltims = ml_tims });
			return result.ToList();
		}
		public async Task<int> CheckExistMaterialMappingById(string ml_tims)
		{
			// if return 1 => co ton tai , 0 la ko ton tai
			string sql = @"SELECT COUNT(wmmid) FROM w_material_mapping_tims WHERE mt_cd = @mtlot ";
			return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { mtlot = ml_tims });

		}
		public async Task<int> UpdateGroupQTYRealQTYTims(int GroupQTY, int RealQTY, string MaterialCode, string ChangeID)
		{
			try
			{
				string sql = @"UPDATE w_material_info_tims SET gr_qty=@grpqty,real_qty=@realqty,chg_date=GETDATE(),chg_id=@chgid WHERE material_code=@Mt_Cd";
				var result = await base.DbConnection.ExecuteAsync(sql, new { grpqty = GroupQTY, realqty = RealQTY, chgid = ChangeID, @Mt_Cd = MaterialCode });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}

		}
		public async Task<int> UpdateFaclineQty(int checkQty, int okQty, int fqNo, string changeId, int ng_qty, int remain_qty)
		{
			string sql = @"Update m_facline_qc set check_qty= @checkqty , ok_qty = @okqty, ng_qty = @ng_qty , remain_qty = @remain_qty, chg_dt=GETDATE(),chg_id=@chgid  where fqno = @fqno";
			return await base.DbConnection.ExecuteAsync(sql,
				new { checkqty = checkQty,
					okqty = okQty,
					chgid = changeId,
					fqno = fqNo,
					ng_qty = ng_qty,
					remain_qty = remain_qty
				});
		}
		public async Task<int> GetSumQtyFacline(string ml_tims)
		{
			string sql = @"SELECT SUM(b.ok_qty) AS qty FROM m_facline_qc b WHERE b.ml_tims=@mltims GROUP BY b.ml_tims";
			return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { mltims = ml_tims });
		}
		public async Task<BobbinLctHist> GetdbobbinlcthistFrbbno(string bb_no)
		{
			string sqlquery = @"SELECT * FROM d_bobbin_lct_hist WHERE bb_no=@bbno order by reg_dt DESC";
			return await base.DbConnection.QueryFirstOrDefaultAsync<BobbinLctHist>(sqlquery, new { bbno = bb_no });
		}
		public async Task<BobbinInfo> GetBobbinInfo(string bb_no)
		{
			string QuerySQL = "SELECT * FROM d_bobbin_info WHERE bb_no = @bbno";
			return await base.DbConnection.QueryFirstOrDefaultAsync<BobbinInfo>(QuerySQL, new { bbno = bb_no });
		}
		public async Task<int> UpdateRemainQTYTims(int GroupQTY, string statusCode, string MaterialCode, string ChangeID)
		{
			string sql = @"UPDATE w_material_info_tims SET gr_qty=@grpqty,status=@mtstscode,chg_date=GETDATE(),chg_id=@chgid WHERE material_code=@mtcd";
			return await base.DbConnection.ExecuteAsync(sql, new { grpqty = GroupQTY, mtstscode = statusCode, chgid = ChangeID, mtcd = MaterialCode });
		}
		public async Task<int> UpdateRemainQTYInfoTims(int GroupQTY, string statusCode, string MaterialCode, string ChangeID, int alertng)
		{
			string sql = @"UPDATE w_material_info_tims SET gr_qty=@grpqty,status=@mtstscode,chg_date=GETDATE(),chg_id=@chgid,alert_ng=@alertng WHERE material_code=@mtcd";
			return await base.DbConnection.ExecuteAsync(sql, new { grpqty = GroupQTY, mtstscode = statusCode, chgid = ChangeID, mtcd = MaterialCode, alertng = alertng });
		}
		public async Task<int> UpdateRemainQTYmms(int GroupQTY, string statusCode, string MaterialCode, string ChangeID)
		{
			string sql = @"UPDATE w_material_info_mms SET gr_qty=@grpqty,status=@mtstscode,chg_date=GETDATE(),chg_id=@chgid WHERE material_code=@mtcd";
			return await base.DbConnection.ExecuteAsync(sql, new { grpqty = GroupQTY, mtstscode = statusCode, chgid = ChangeID, mtcd = MaterialCode });
		}
		public async Task<MaterialInfoTimsDTO> GetWMaterialInfowithmtcdNew(string mt_cd)
		{
			string QuerySQL = @"SELECT wmtid,id_actual,material_code AS mt_cd,bb_no,gr_qty,status AS mt_sts_cd,mt_no,material_type AS mc_type 
FROM w_material_info_tims WHERE material_code = @mtcd";
			return await base.DbConnection.QuerySingleOrDefaultAsync<MaterialInfoTimsDTO>(QuerySQL, new { mtcd = mt_cd });
		}

		public async Task<MaterialInfoTimsDTO> GetWMaterialInfowithmtcdNewMMS(string mt_cd)
		{
			string QuerySQL = @"SELECT wmtid,id_actual,material_code AS mt_cd,bb_no,gr_qty,status AS mt_sts_cd,mt_no,material_type AS mc_type 
FROM w_material_info_mms WHERE material_code = @mtcd";
			return await base.DbConnection.QuerySingleOrDefaultAsync<MaterialInfoTimsDTO>(QuerySQL, new { mtcd = mt_cd });
		}
		public async Task<MaterialInfoTimsDTO> GetWMaterialInfowithmtcdTims(string mt_cd)
		{
			string QuerySQL = @"SELECT wmtid,id_actual,material_code AS mt_cd,bb_no,gr_qty,status AS mt_sts_cd,mt_no,material_type AS mc_type 
FROM w_material_info_tims WHERE material_code = @mtcd";
			return await base.DbConnection.QuerySingleOrDefaultAsync<MaterialInfoTimsDTO>(QuerySQL, new { mtcd = mt_cd });
		}
		public async Task<int> UpdateUseYnMaterialMapping(string UseYn, string ChangeId, int wmmid)
		{
			string sql = @"UPDATE w_material_mapping_tims SET use_yn=@useyn,chg_dt=GETDATE(), chg_id=@chgid WHERE wmmid=@wmmid";
			return await base.DbConnection.ExecuteAsync(sql, new { useyn = UseYn, chgid = ChangeId, wmmid = wmmid });
		}
		public async Task<int> GetDefactActual(int id_actual)
		{
			string sql = @"SELECT SUM(k.check_qty-k.ok_qty)
            FROM m_facline_qc AS k 
            WHERE k.ml_tims IN (
            SELECT l.material_code FROM w_material_info_tims AS l 
            WHERE  l.id_actual=@idactual)";
			return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { idactual = id_actual });
		}
		public async Task<int> UpdateDefectActual(double? defect, int id_actual)
		{
			string sql = @"Update w_actual SET defect = @defect  where id_actual = @idactual ";
			return await base.DbConnection.ExecuteAsync(sql, new { defect = defect, idactual = id_actual });
		}
		public async Task<int> UpdateTotalQTYActual(int totalQty, int id_actual)
		{
			string sql = @"Update w_actual SET actual = @actual  where id_actual = @idactual ";
			return await base.DbConnection.ExecuteAsync(sql, new { actual = totalQty, idactual = id_actual });
		}
		public async Task<string> GetmfaclineqcSearch(string fq_no)
		{
			string sqlquery = @"SELECT top 1 fq_no FROM m_facline_qc WHERE  fq_no LIKE '%'+@fqno+'%'  ORDER BY fq_no ASC";
			return await base.DbConnection.QueryFirstOrDefaultAsync<string>(sqlquery, new { fqno = fq_no });
		}
		public async Task<qc_item_mt> Getqcitemmt(string item_vcd)
		{
			string sql = @"SELECT * FROM qc_item_mt WHERE item_vcd=@itemvcd";
			return await base.DbConnection.QueryFirstOrDefaultAsync<qc_item_mt>(sql, new { itemvcd = item_vcd });
		}
		public async Task<int> InsertMFaclineQC(m_facline_qc mFaclineQC)
		{
			string sqlquery = @"insert into m_facline_qc(fq_no,ml_no,ml_tims,work_dt,reg_dt,chg_dt,item_nm,item_exp,reg_id,chg_id,check_qty,ok_qty,item_vcd,product_cd,shift,at_no,ng_qty,remain_qty) 
                                        values(@fq_no,@ml_no,@ml_tims,@work_dt,GETDATE(),GETDATE(),@item_nm,@item_exp,@reg_id,@chg_id,@check_qty,@ok_qty,@item_vcd,@product_cd,@shift,@at_no,@ng_qty,@remain_qty) ; Select Scope_Identity();";
			return await base.DbConnection.ExecuteAsync(sqlquery, mFaclineQC);
		}
		public async Task<int> CheckMaterialByMtCdLike(string bien_first)
		{
			var sql = @"Select COUNT(wmtid) from w_material_info_tims where material_code like '%'+@mtcd+'%' ";
			return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { mtcd = bien_first });

		}
		public async Task<int> InsertMaterialInfoTims(string materialCode, int grpQTy, int realQty, string reg_id, string materialLot)
		{
			string sql = @"INSERT INTO w_material_info_tims(at_no,product, id_actual,material_type,material_code,
                                            alert_ng,mt_no,gr_qty,
                                           status,active,reg_id,reg_date,chg_id,chg_date, orgin_mt_cd,location_code,real_qty)
                                            SELECT at_no,product, 0,material_type,@matercode,
                                            1,mt_no,@grqty,
                                            '003',active,@userid,GETDATE(),@userid,GETDATE(),material_code,location_code,@realqty
                                            FROM   w_material_info_tims
                                            where material_code = @materiallot ; Select Scope_Identity();";
			return await base.DbConnection.ExecuteAsync(sql, new { matercode = materialCode, grqty = grpQTy, userid = reg_id, realqty = realQty, materiallot = materialLot });
		}
		public async Task<int> UpdateUseYnMaterialMapping(string useyn, int wmmid, string userId)
		{
			string sql = "Update w_material_mapping_tims set use_yn = @use_yn,chg_id=@chgid,chg_dt=GETDATE() where wmmid = @wmmid";
			return await base.DbConnection.ExecuteAsync(sql, new { use_yn = useyn, wmmid = wmmid, chgid = userId });
		}
		public async Task<IReadOnlyList<StampDetail>> GetListStamDetail(string product,string lotNo,string shift, string buyer_qr)
		{
			try
			{
				var query = @"SELECT a.id, a.buyer_qr , a.product_code ,  (SELECT style_nm FROM d_style_info WHERE style_no = a.product_code) AS product_name, 
							(SELECT md_cd FROM d_style_info WHERE style_no = a.product_code) AS model, 
							(SELECT stamp_name FROM stamp_master WHERE stamp_code = a.stamp_code) AS stamp_name,
							a.standard_qty AS quantity,
							REPLACE(a.lot_date, '-','') AS lotNo, reg_id, reg_dt, chg_id, chg_dt
							FROM stamp_detail AS a
							WHERE
								(@buyer_qr = '' OR @buyer_qr IS NULL OR a.buyer_qr LIKE '%' + @buyer_qr + '%') 
							-- a.buyer_qr NOT IN (SELECT buyer_qr FROM w_material_info_tims WHERE buyer_qr IS NOT NULL )
							AND a.product_code = case when @product = '' then a.product_code else @product end
							And a.lot_date = case when @lotDate ='' then a.lot_date else @lotDate end
					And	(@shift = '' OR @shift IS NULL OR a.shift LIKE '%' + @shift + '%') ";
				var result = await base.DbConnection.QueryAsync<StampDetail>(query, new { @Product = product, @lotDate = lotNo, @shift = shift , @buyer_qr = buyer_qr });
				return result.ToList();
			}
			catch (Exception e)
			{

				throw e;
			}
		}
		public async Task<int> UpdateActualNDefectForStaff(int idActual, int psId, int actual, int defect, string userId)
		{
			string sql = @"UPDATE d_pro_unit_staff
				SET actual =@sanluong,defect=@defectreturn,chg_dt=GETDATE(),chg_id=@chgid
				WHERE id_actual=@idactual AND  psid=@psid;";
			return await base.DbConnection.ExecuteAsync(sql, new { idactual = idActual, psid = psId, sanluong = actual, defectreturn = defect, chgid = userId });
		}
		public async Task<int> UpdateActualForOQC(int actual, int idActual, int psId, string userId)
		{
			string sql = @"UPDATE d_pro_unit_staff SET actual =actual+@sanluong ,chg_dt=GETDATE(),chg_id=@chgid
			WHERE id_actual=@idactual AND psid=@psid;";
			return await base.DbConnection.ExecuteAsync(sql, new { sanluong = actual, idactual = idActual, psid = psId, chgid = userId });
		}
		public async Task<int> getActualRealQTY(int IdActual, string StaffId, string StartDate, string EndDate)
		{
			string sql = @"SELECT SUM(l.real_qty)
				FROM w_material_info_tims AS l
				WHERE l.status!='003' AND 
				(l.reg_date BETWEEN CONVERT(datetime,@startdt) AND CONVERT(datetime,@enddt)) AND
				 l.staff_id=@staffid AND l.id_actual=@idactual AND l.orgin_mt_cd IS  NULL";
			return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { idactual = IdActual, staffid = StaffId, startdt = StartDate, enddt = EndDate });

		}
		public async Task<int> getActualQTYForActualTims(int IdActual)
		{
			string sql = @"SELECT CASE WHEN  SUM(actual) IS NULL THEN 0 ELSE SUM(actual) END FROM d_pro_unit_staff WHERE id_actual =@idactual";
			return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { idactual = IdActual });

		}
		public async Task<int> getActualDefectforTims(int IdActual, string StaffId, string StartDate, string EndDate)
		{
			string sql = @" SELECT  SUM(ng_qty)
				FROM m_facline_qc AS k 
				WHERE k.ml_tims IN (

				SELECT l.material_code FROM w_material_info_tims AS l
				WHERE l.staff_id=@staffid AND l.reg_date 
				 BETWEEN CONVERT(datetime,@startdt) AND CONVERT(datetime,@enddt)
				AND l.id_actual=@idactual
				)";
			return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { idactual = IdActual, staffid = StaffId, startdt = StartDate, enddt = EndDate });
		}
		public async Task<int> getActualrealOQC(int IdActual, string StaffId, string StartDate, string EndDate)
		{
			string sql = @"SELECT SUM(l.gr_qty) FROM w_material_info_tims as l WHERE l.status!='003' and 
			((SELECT k.reg_dt FROM w_material_mapping_tims AS k  WHERE k.mt_lot IS NULL AND l.material_code=k.mt_cd ) 
			 BETWEEN CONVERT(datetime,@startdt) AND CONVERT(datetime,@enddt)) and
			 l.staf_id=@staffid AND l.id_actual=@idactual";
			return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { idactual = IdActual, staffid = StaffId, startdt = StartDate, enddt = EndDate });
		}
		public async Task<int> updateQtyforStaff(int idActual, int actualQty, int defectQty, int psId)
		{
			string sql = @" UPDATE d_pro_unit_staff
			SET actual =@actual,defect=@defect
			WHERE id_actual=@idactual AND  psid=@psid;";
			return await base.DbConnection.ExecuteAsync(sql, new { idactual = idActual, psid = psId, actual = actualQty, defect = defectQty });
		}
		public async Task<int> updateQtyOqcforStaff(int idActual, int actualQty, int psId)
		{
			string sql = @" UPDATE d_pro_unit_staff
			SET actual =@actual
			WHERE id_actual=@idactual AND  psid=@psid;";
			return await base.DbConnection.ExecuteAsync(sql, new { idactual = idActual, psid = psId, actual = actualQty });
		}
		public async Task<string> getNameActual(int idActual)
		{
			string sql = @" SELECT [name] FROM w_actual WHERE id_actual=@idactual";
			return await base.DbConnection.QueryFirstOrDefaultAsync<string>(sql, new { idactual = idActual });
		}
		public async Task<string> getStaffId(int psId)
		{
			string sql = @"SELECT staff_id FROM d_pro_unit_staff WHERE psid=@psid";
			return await base.DbConnection.QueryFirstOrDefaultAsync<string>(sql, new { psid = psId });
		}
		public async Task<int> UpdateNGPO(int realQty, string materialCode) //bien = mt_cd
		{
			string sql = @"UPDATE w_material_info_tims SET gr_qty=(gr_qty+ @grpqty),
                                                      real_qty=(real_qty+ @realqty)
                                                      WHERE material_code=@materialcode ";
			return await base.DbConnection.ExecuteAsync(sql, new { materialcode = materialCode, grpqty = realQty, realqty = realQty });
		}
		public async Task<int> UpdateContainerOutput(string mt_cd, int qty)
		{
			string sql = @"UPDATE w_material_info_tims
                        SET gr_qty=@grpqty,real_qty=@realqty,chg_date=GETDATE()
                        WHERE material_code=@mtcd";
			return await base.DbConnection.ExecuteAsync(sql, new { mtcd = mt_cd, grpqty = qty, realqty = qty });
		}
		public async Task<int> UpdateMaterialmappinguseyn(string mt_lot, string mt_cd, string userid, bool active, string useyn)
		{
			string sql = @"UPDATE w_material_mapping_tims SET active=@active,chg_dt=GETDATE(),chg_id=@userid,use_yn=@useyn where mt_lot=@mtlot and mt_cd=@mtcd;";
			return await base.DbConnection.ExecuteAsync(sql, new { mtlot = mt_lot, mtcd = mt_cd, userid = userid, active = active, useyn= useyn });
		}

		public async Task<int> UpdateMaterialmappingMMSuseyn(string mt_lot, string mt_cd, string userid, bool active)
		{
			string sql = @"UPDATE w_material_mapping_mms SET active=@active,chg_date=GETDATE(),chg_id=@userid where mt_lot=@mtlot and mt_cd=@mtcd;";
			return await base.DbConnection.ExecuteAsync(sql, new { mtlot = mt_lot, mtcd = mt_cd, userid = userid, active = active });
		}
		public async Task<int> UpdateGrqtyMaterialInfoTims(string materialCode, int grpQty, string statusCode, string userId)
		{
			string sql = @"UPDATE w_material_info_tims SET gr_qty=@grpqty,[status]=@status,chg_id=@userid,chg_date=GETDATE() WHERE material_code=@mtcd";
			return await base.DbConnection.ExecuteAsync(sql, new { grpqty = grpQty, status = statusCode, userid = userId, mtcd = materialCode });
		}

		public async Task<int> UpdateGrqtyMaterialInfoMMS(string materialCode, int grpQty, string statusCode, string userId)
		{
			string sql = @"UPDATE w_material_info_mms SET gr_qty=@grpqty,[status]=@status,chg_id=@userid,chg_date=GETDATE() WHERE material_code=@mtcd";
			return await base.DbConnection.ExecuteAsync(sql, new { grpqty = grpQty, status = statusCode, userid = userId, mtcd = materialCode });
		}

		public async Task<int> CheckGrqtyMaterialInfo(string mt_cd, string mt_sts_cd)
		{
			string sql = "SELECT COUNT(wmtid) FROM w_material_info_tims WHERE material_code=@mtcd AND status=@status AND gr_qty<=0";
			return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { mtcd = mt_cd, status = mt_sts_cd });
		}
		public async Task<int> DeleteDBobbinLctHistforDevice(string mt_cd, string bbno)
		{
			string sql = @"DELETE FROM d_bobbin_lct_hist WHERE bb_no=@bbno AND mt_cd=@mtcd";
			return await base.DbConnection.ExecuteAsync(sql, new { mtcd = mt_cd, bbno = bbno });
		}
		public async Task<int> UpdateBobbinInfowithmtcd(string mt_cd, string bbno, string userid)
		{
            try
            {
				string sql = "UPDATE d_bobbin_info SET mt_cd=NULL,chg_id=@chgid,chg_dt=GETDATE() WHERE bb_no=@bbno and mt_cd = @mtcd";
				return await base.DbConnection.ExecuteAsync(sql, new { mtcd = mt_cd, chgid = userid, bbno = bbno });
			}
			catch (Exception e)
			{
				throw;
			}
		}
		public async Task<int> GetDefactActualTimss(int id_actual)
		{
			string sql = @"SELECT SUM(k.check_qty-k.ok_qty)
            FROM m_facline_qc AS k 
            WHERE k.ml_tims IN (

            SELECT l.material_code FROM w_material_info_tims AS l 
            WHERE  l.id_actual=@idactual)";
			return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { idactual = id_actual });
		}
		public async Task<int> UpdateDefectActualTims(int defect, int id_actual, string userid)
		{
			string sql = @"
                    Update w_actual SET defect = @defect,chg_dt=GETDATE(),chg_id=@chgid 
                    where id_actual = @idactual ;  ";
			return await base.DbConnection.ExecuteAsync(sql, new { idactual = id_actual, defect = defect, chgid = userid });
		}
		public async Task<WMaterialInfoTmp> FindOneMaterialInfoByMTLot(string mt_lot)
		{
			string sql = @"SELECT wmtid,id_actual,mt_no,bb_no,gr_qty,material_code AS mt_cd, status as mt_sts_cd FROM w_material_info_tims WHERE material_code= @mtcd";
			return await base.DbConnection.QueryFirstOrDefaultAsync<WMaterialInfoTmp>(sql, new { @mtcd = mt_lot });
		}
		public async Task<IReadOnlyList<OKReason>> GetOKReason(string mt_lot, string PrimaryProduct, string don_vi_pr, string mt_cd, string product, string mt_no, string bb_no)
		{

			string sqlquery = @"SELECT a.wmtid,a.material_code mt_cd,a.mt_no,a.gr_qty,a.bb_no,b.at_no 
								FROM w_material_info_tims AS a
								JOIN w_actual AS b ON a.id_actual = b.id_actual
								JOIN w_actual_primary AS c ON b.at_no = c.at_no
								JOIN d_bobbin_lct_hist AS d ON a.material_code = d.mt_cd
								WHERE a.location_code LIKE '006%' AND a.gr_qty > 0  AND c.product = @PrimaryProduct AND b.don_vi_pr = @donvipr AND a.material_code != @mt_lot
								AND(@mtcd = '' OR @mtcd IS NULL OR a.material_code LIKE '%' + @mtcd + '%') 
								AND(@product = '' OR @product IS NULL OR c.product LIKE '%' + @product + '%')
								AND(@mtno = '' OR @mtno IS NULL OR a.mt_no LIKE '' + @mtno + '') 
								AND(@bbno = '' OR @bbno IS NULL OR a.bb_no LIKE '%' + @bbno + '%')
								ORDER BY a.wmtid DESC";
			var result = await base.DbConnection.QueryAsync<OKReason>(sqlquery,
								new { @mt_lot = mt_lot, @mtcd = mt_cd, @PrimaryProduct = PrimaryProduct, @donvipr = don_vi_pr, @mtno = mt_no, @bbno = bb_no, @product = product });
			return result.ToList();
		}
		public async Task<FaclineQc> GetFaclineQC(string item_vcd, string mt_cd, string mt_lot)
		{
			//string sql = @"SELECT b.fqno fqno,b.fq_no,
			//CONCAT(SUBSTRING(b.work_dt,1,4),'-',SUBSTRING(b.work_dt,5,2),'-',SUBSTRING(b.work_dt,7,2),' ',SUBSTRING(b.work_dt,9,2),':',SUBSTRING(b.work_dt,11,2) ) work_dt,
			//			b.check_qty,(b.ok_qty),(b.check_qty)-(b.ok_qty) as defect_qty
			//from m_facline_qc as b 
			//where b.item_vcd=@itemvcd and b.ml_no=@mtcd and b.ml_tims=@mtlot and fq_no like 'TI%' 
			//order by fq_no desc ,check_qty";
			//var result = await base.DbConnection.QueryAsync<FaclineQc>(sql, new { itemvcd = item_vcd, mtcd = mt_cd, mtlot = mt_lot }); ;
			//return result.ToList();

			string sql = @"SELECT b.fqno,convert(varchar, b.reg_dt, 23) as work_dt,b.check_qty,b.ok_qty, b.ng_qty, b.remain_qty
			from m_facline_qc as b 
			where b.item_vcd=@itemvcd and b.ml_no=@mtcd and b.ml_tims=@mtlot";

			var result = await base.DbConnection.QueryFirstOrDefaultAsync<FaclineQc>(sql, new { itemvcd = item_vcd, mtcd = mt_cd, mtlot = mt_lot });
			return result;
		}

		public async Task<IReadOnlyList<WMaterialInfoOQCAPI>> GetListLotOQCpp(int id_actual_oqc, string staff_id_oqc)
		{
			string viewSql = @" SELECT  wmtid,bb_no,mt_no,material_code mt_cd,gr_qty,'0' as count_ng
               FROM w_material_info_tims 
	                   WHERE id_actual_oqc = @idacutal AND status = '009' AND staff_id_oqc = @staffid";
			var result = await base.DbConnection.QueryAsync<WMaterialInfoOQCAPI>(viewSql, new { idacutal = id_actual_oqc, staffid = staff_id_oqc });

			return result.ToList();
		}

		public async Task<IReadOnlyList<WMaterialInfoTmp>> GetListMaterial(int wmtid, string mt_cd, string mt_no)
		{
            try
            {
				string sqlquery = @"SELECT
										wmtid,
										at_no,
										product,
										id_actual,
										staff_id,
										material_type As mt_type,
										material_code As mt_cd,
										mt_no,
										gr_qty,
										real_qty,
										reg_date As DATE,
										[status] As mt_sts_cd,
										bb_no,
										--bbmp_sts_cd,
										location_code As lct_cd,
										from_lct_code As from_lct_cd,
										reg_date As input_dt,
										orgin_mt_cd,
										sts_update,
										active As use_yn,
										reg_id,
										reg_date As reg_dt,
										chg_id,
										chg_date As chg_dt 
									FROM
										w_material_info_tims 
									WHERE
										wmtid= @wmtid  AND
										 gr_qty > 0 
										AND material_code != @mtcd
										AND mt_no = @mtno
									ORDER BY
										gr_qty DESC";
				var result = await base.DbConnection.QueryAsync<WMaterialInfoTmp>(sqlquery, new { @wmtid = wmtid, @mtcd = mt_cd, @mtno = mt_no });
				return result.ToList();
			}
            catch (Exception e)
            {

                throw e;
            }

		}
		public async Task<MaterialMappingTIMS> FindOneWMaterialMappingById(int wmmid)
		{
			try
			{
				var query = @"select 
                                    wmmid  ,
                                    mt_lot ,
                                    mt_cd  ,
                                    mt_no  ,
                                    mapping_dt,
                                    bb_no     ,
                                    remark   , 
                                    sts_share ,
                                    use_yn    ,
                                    del_yn    ,
                                    reg_id   , 
                                        reg_dt,
                                        chg_id,
                                        chg_dt
                                from w_material_mapping_tims where wmmid = @Id";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialMappingTIMS>(query, new { @Id = wmmid });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<int> UpdateWMaterialMappingById(string sts_share, int wmmid)
		{
			try
			{
				var query = @"Update w_material_mapping_tims SET sts_share = @Status where wmmid = @Id";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Status = sts_share, @Id = wmmid });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<int> CheckShift(int psid)
		{
			try
			{
				var query = @"SELECT count(a.psid) FROM d_pro_unit_staff AS a  WHERE a.psid = @Id AND (GETDATE() BETWEEN a.start_dt AND a.end_dt)";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Id = psid });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<IReadOnlyList<DProUnitStaff>> GetListStaff(int id, int psid)
		{
			try
			{
				var query = @"SELECT * FROM d_pro_unit_staff AS a WHERE a.id_actual = @Id_Actual and a.psid= @PSID AND (GETDATE() BETWEEN a.start_dt AND a.end_dt)";
				var result = await base.DbConnection.QueryAsync<DProUnitStaff>(query, new { @Id_Actual = id, @PSID = psid });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<MaterialInfoTIMS> GetMaterialInfoTIMSById(int id)
		{
			try
			{
				var query = @"Select * from w_material_info_tims where id_actual = @Id";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoTIMS>(query, new { @Id = id });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<MaterialInfoTIMS> GetMaterialInfoTIMSByOQC(int id)
		{
			try
			{
				var query = @"Select * from w_material_info_tims where id_actual_oqc = @OQC";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoTIMS>(query, new { @OQC = id });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<MaterialMappingTIMS> CheckMaterialMappingTIMS(string mt_cd)
		{
			try
			{
				var query = @"Select * From w_material_mapping_tims Where mt_cd = @MaterialCode";
				var result = base.DbConnection.Query<MaterialMappingTIMS>(query, new { MaterialCode = mt_cd });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<IReadOnlyList<MaterialMappingTIMS>> GetMaterialMappingTIMSByLot(string mt_lot)
		{
			try
			{
				var query = @"Select * From w_material_mapping_tims Where mt_cd = @mtlot";
				var result = await base.DbConnection.QueryAsync<MaterialMappingTIMS>(query, new { mtlot = mt_lot },commandTimeout:60,commandType:System.Data.CommandType.Text);
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<IReadOnlyList<MaterialMappingTIMS>> GetMaterialMappingTIMSByLotCancel(string mt_lot)
		{
			try
			{
				var query = @"Select * From w_material_mapping_tims Where mt_lot = @Mt_Lot order by reg_dt DESC";
				var result = await base.DbConnection.QueryAsync<MaterialMappingTIMS>(query, new { @Mt_Lot = mt_lot });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<IReadOnlyList<BobbinLctHist>> GetListDataBobbinLctHist(string bb_no, string mt_cd)
		{
			try
			{
				var query = @"Select * From d_bobbin_lct_hist where bb_no = @Bb_no And mt_cd = @Mt_Cd order by reg_dt DESC";
				var result = await base.DbConnection.QueryAsync<BobbinLctHist>(query, new { @Bb_no = bb_no, @Mt_Cd = mt_cd });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public int InsertIntoBobbinLctHist(BobbinLctHist item)
		{
			try
			{
				var query = @"Insert into d_bobbin_lct_hist (mc_type, bb_no, mt_cd, bb_nm, start_dt, end_dt, use_yn, del_yn, reg_id, reg_dt, chg_id, chg_dt)
							Values (@mc_type, @bb_no, @mt_cd, @bb_nm,GETDATE() , GETDATE(), @use_yn, @del_yn, @reg_id, GETDATE(), @chg_id, GETDATE())";
				return base.DbConnection.Execute(query, item);
				//return result;
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
				var query = @"Update d_bobbin_info set mc_type = @mc_type, bb_no = @bb_no, mt_cd = @mt_cd, bb_nm = @bb_nm, purpose = @purpose, barcode = @barcode, re_mark = @re_mark, 
							use_yn = @use_yn, count_number = @count_number, del_yn = @del_yn, chg_id = @chg_id, chg_dt = Getdate()
							Where bno = @bno";
				var result = await base.DbConnection.ExecuteAsync(query, item);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<int> RemoveBobbinLctHist(int blno)
		{
			try
			{
				var query = @"Delete d_bobbin_lct_hist where blno = @Id";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Id = blno });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> RemoveBobbinLctHistwithbbno(string bbno)
		{
			try
			{
				var query = @"Delete d_bobbin_lct_hist where bb_no = @bbno";
				var result = await base.DbConnection.ExecuteAsync(query, new { bbno = bbno });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<int> RemoveMaterialInfoTims(int wmtid)
		{
			try
			{
				var query = @"Delete w_material_info_tims where wmtid = @Id";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Id = wmtid });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<MaterialMappingTIMS> GetMaterialMappingTimsById(int wmmid)
		{
			try
			{
				var query = @"Select * From w_material_mapping_tims where  wmmid = @Id";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialMappingTIMS>(query, new { @Id = wmmid });
				return result;

			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<MaterialMappingTIMS> GetListMaterialMappingReturn(string mt_cd, string mt_lot)
		{
			try
			{
				var query = @"SELECT * FROM w_material_mapping_tims WHERE mt_cd = @Mt_Cd AND mt_lot = @Mt_Lot Order by mapping_dt Desc";
				var result=  base.DbConnection.Query<MaterialMappingTIMS>(query, new { @Mt_Cd = mt_cd, @Mt_Lot = mt_lot });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IReadOnlyList<MaterialInfoTIMS>> GetListDataMaterialInfoTIMS(string orgin_mt_cd)
		{
			try
			{
				var query = @"Select * From w_material_info_tims where orgin_mt_cd = @Orgin And sts_update = 'return'";
				var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(query, new { @Orgin = orgin_mt_cd });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IReadOnlyList<FaclineQC>> GetListDataFaclineQc(string ml_no, string ml_tims)
		{
			try
			{
				var query = @"Select * From m_facline_qc Where ml_no = @MlNo And ml_tims = @MlTims And fq_no Like 'TI%' order by reg_dt DESC";
				var result = await base.DbConnection.QueryAsync<FaclineQC>(query, new { MlNo = ml_no, MlTims = ml_tims });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<IReadOnlyList<FaclineQC>> GetListDataFaclineQcwithfqno(string fq_no)
		{
			try
			{
				var query = @"Select * From m_facline_qc Where fq_no Like '%'+@fqno+'%' order by reg_dt DESC";
				var result = await base.DbConnection.QueryAsync<FaclineQC>(query, new { @fqno = fq_no });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<IReadOnlyList<FaclineQC>> GetListDataFaclineQcwithmltims(string ml_tims)
		{
			try
			{
				var query = @"Select * From m_facline_qc Where ml_tims = @MlTims And fq_no Like 'TI%'";
				var result = await base.DbConnection.QueryAsync<FaclineQC>(query, new { @MlTims = ml_tims });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<IReadOnlyList<FaclineQCValue>> GetListDataFaclineQcValue(string fq_no)
		{
			try
			{
				var query = @"Select * From m_facline_qc_value Where fq_no = @Fq_No";
				var result = await base.DbConnection.QueryAsync<FaclineQCValue>(query, new { @Fq_No = fq_no });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> RemoveFaclineQC(int id)
		{
			try
			{
				var query = @"Delete m_facline_qc Where fqno = @Id";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Id = id });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> RemoveFaclineQCValue(int id)
		{
			try
			{
				var query = @"Delete m_facline_qc_value Where fqhno = @Id";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Id = id });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<FaclineQCValue> GetListDataFaclineQcValuewithId(int id)
		{
			try
			{
				var query = @"Select * From m_facline_qc_value Where Id = @id";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<FaclineQCValue>(query, new { id = id });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IReadOnlyList<m_facline_qc>> GetmfaclineqcwithId(int id)
		{
			string sqlquery = @"SELECT * FROM m_facline_qc WHERE Id = @Id";
			var result = await base.DbConnection.QueryAsync<m_facline_qc>(sqlquery, new { Id = id });
			return result.ToList();
		}
		public async Task<IReadOnlyList<MaterialInfoTIMS>> GetListMaterialInfoTIMS(string mt_cd)
		{
			try
			{
				var query = @"Select * From w_material_info_tims where material_code Like @Mt_Cd + '-NG%' And location_code Like '006%' ";
				var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(query, new { @Mt_Cd = mt_cd });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> RemoveMaterialMappingsTims(int id)
		{
			try
			{
				var query = @"Delete w_material_mapping_tims Where wmmid = @Id";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Id = id });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> UpdateMaterial(int quantity, int wmtid, string mt_sts_cd, string userid)
		{
			string sqlquery = "UPDATE w_material_info_tims SET gr_qty=@grpqty,[status]=@status,chg_date=GETDATE(),chg_id=@chgid WHERE wmtid=@wtmid";
			return await base.DbConnection.ExecuteAsync(sqlquery, new { grpqty = quantity, status = mt_sts_cd, chgid = userid, wtmid = wmtid });
			//db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", quantity), new MySqlParameter("2", wmtid), new MySqlParameter("3", mt_sts_cd));
		}
		public async Task<int> UpdateQtyMaterialTims(int quantity, int wmtid, string userid)
		{
			string sqlquery = "UPDATE w_material_info_tims SET gr_qty=@grpqty,chg_date=GETDATE(),chg_id=@chgid WHERE wmtid=@wtmid";
			return await base.DbConnection.ExecuteAsync(sqlquery, new { grpqty = quantity, chgid = userid, wtmid = wmtid });
			//db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", quantity), new MySqlParameter("2", wmtid), new MySqlParameter("3", mt_sts_cd));
		}
		public async Task<int> CountMaterialInfo(string LikeCondition)
		{
			string sqlquery = "SELECT count(wmtid) FROM w_material_info_tims WHERE material_code LIKE '%'+@mtcd+'%'";
			return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sqlquery, new { mtcd = LikeCondition });
			//return db.Database.SqlQuery<int>(sqlquery, new MySqlParameter("1", LikeCondition)).FirstOrDefault();
		}
		//		public async Task<int> InsertMergeMaterial(MaterialInfoTam data)
		//		{
		//			string sqlquery = @"INSERT INTO w_material_info_tims
		//(id_actual, staff_id, material_type, material_code, mt_no, gr_qty, real_qty,[status], bb_no, bbmp_sts_cd, location_code, from_lct_code, orgin_mt_cd, sts_update, active, reg_id, reg_date, chg_id, chg_date)
		//VALUES(@idactual, @staffid, @mttype, @mtcd, @mtno, @grpqty, @realqty, @status, @bbno, @bbmpstscd, @lctcd, @frlctcd, @orgmtcd, @stsupdate, @active, @regid, @regdate, @chgid, @chgdate); ";
		//			return await base.DbConnection.ExecuteAsync(sqlquery,new { idactual= data.id_actual ,
		//				staffid=data.staff_id,
		//				mttype=data.mt_type,
		//				mtcd=data.mt_cd,
		//				mtno=data.mt_no,
		//				grpqty=data.gr_qty,
		//				realqty=data.real_qty,
		//				status=data.mt_sts_cd,
		//				bbno=data.bb_no,
		//				bbmpstscd=data.bbmp_sts_cd,
		//				lctcd=data.lct_cd,
		//				frlctcd=data.from_lct_cd,
		//				orgmtcd=data.orgin_mt_cd,
		//				stsupdate=data.sts_update,
		//				active=data.use_yn,
		//				regid=data.reg_id,
		//				regdate=data.reg_dt,
		//				chgid=data.chg_id,
		//				chgdate=data.chg_dt
		//			});
		//			//db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", data.id_actual), new MySqlParameter("2", data.id_actual_oqc), new MySqlParameter("3", data.at_no), new MySqlParameter("4", data.product), new MySqlParameter("5", data.staff_id), new MySqlParameter("6", data.staff_id_oqc), new MySqlParameter("7", data.mt_type), new MySqlParameter("8", data.mt_cd), new MySqlParameter("9", data.mt_no), new MySqlParameter("10", data.gr_qty), new MySqlParameter("11", data.real_qty), new MySqlParameter("12", data.date), new MySqlParameter("13", data.end_production_dt), new MySqlParameter("14", data.mt_barcode), new MySqlParameter("15", data.mt_qrcode), new MySqlParameter("16", data.mt_sts_cd), new MySqlParameter("17", data.bb_no), new MySqlParameter("18", data.bbmp_sts_cd), new MySqlParameter("19", data.lct_cd), new MySqlParameter("20", data.lct_sts_cd), new MySqlParameter("21", data.from_lct_cd), new MySqlParameter("22", data.input_dt), new MySqlParameter("23", data.orgin_mt_cd), new MySqlParameter("24", data.sts_update), new MySqlParameter("25", data.use_yn), new MySqlParameter("26", data.reg_id), new MySqlParameter("27", data.reg_dt), new MySqlParameter("28", data.chg_id), new MySqlParameter("29", data.chg_dt));
		//		}
		public async Task<int> InsertMergeMaterial(WMaterialInfoTmp data)
		{
			string sqlquery = @"INSERT INTO w_material_info_tims
							(id_actual, staff_id, material_type, material_code, mt_no, gr_qty, real_qty,[status], bb_no,  location_code, from_lct_code, orgin_mt_cd, sts_update, active, reg_id, reg_date, chg_id, chg_date,at_no,product)
							VALUES(@idactual, @staffid, @mttype, @mtcd, @mtno, @grpqty, @realqty, @status, @bbno,@lctcd, @frlctcd, @orgmtcd, @stsupdate, @active, @regid, @regdate, @chgid, @chgdate,@atno,@product); ";
			return await base.DbConnection.ExecuteAsync(sqlquery, new
			{
				idactual = data.id_actual,
				staffid = data.staff_id,
				mttype = data.mt_type,
				mtcd = data.mt_cd,
				mtno = data.mt_no,
				grpqty = data.gr_qty,
				realqty = data.real_qty,
				status = data.mt_sts_cd,
				bbno = data.bb_no,
				bbmpstscd = data.bbmp_sts_cd,
				lctcd = data.lct_cd,
				frlctcd = data.from_lct_cd,
				orgmtcd = data.orgin_mt_cd,
				stsupdate = data.sts_update,
				active = data.use_yn,
				regid = data.reg_id,
				regdate = data.reg_dt,
				chgid = data.chg_id,
				chgdate = data.chg_dt,
				atno = data.at_no,
				product = data.product
			});
			//db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", data.id_actual), new MySqlParameter("2", data.id_actual_oqc), new MySqlParameter("3", data.at_no), new MySqlParameter("4", data.product), new MySqlParameter("5", data.staff_id), new MySqlParameter("6", data.staff_id_oqc), new MySqlParameter("7", data.mt_type), new MySqlParameter("8", data.mt_cd), new MySqlParameter("9", data.mt_no), new MySqlParameter("10", data.gr_qty), new MySqlParameter("11", data.real_qty), new MySqlParameter("12", data.date), new MySqlParameter("13", data.end_production_dt), new MySqlParameter("14", data.mt_barcode), new MySqlParameter("15", data.mt_qrcode), new MySqlParameter("16", data.mt_sts_cd), new MySqlParameter("17", data.bb_no), new MySqlParameter("18", data.bbmp_sts_cd), new MySqlParameter("19", data.lct_cd), new MySqlParameter("20", data.lct_sts_cd), new MySqlParameter("21", data.from_lct_cd), new MySqlParameter("22", data.input_dt), new MySqlParameter("23", data.orgin_mt_cd), new MySqlParameter("24", data.sts_update), new MySqlParameter("25", data.use_yn), new MySqlParameter("26", data.reg_id), new MySqlParameter("27", data.reg_dt), new MySqlParameter("28", data.chg_id), new MySqlParameter("29", data.chg_dt));
		}
		public async Task<IReadOnlyList<PrintNGTimsReponse>> GetListDataPrintNGTims(string mt_cd, string at_no, string product)
		{
			try
			{
				var query = @"Select *
									From( SELECT 
									a.wmtid AS Id,
									a.gr_qty AS Qty,
									a.material_code AS WMaterialCode,
									a.at_no AS at_no,
									a.product AS product,
									(CASE
										WHEN ((a.gr_qty - qc.check_qty) IS NULL) THEN a.gr_qty
										ELSE (a.gr_qty - qc.check_qty)
									END) AS SLCK,
									a.buyer_qr AS buyer_qr,
									a.reg_date AS reg_dt
								FROM
									(w_material_info_tims a
									LEFT JOIN m_facline_qc qc ON ((a.material_code = qc.ml_tims)))
								WHERE
									(((a.location_code LIKE '003%')
										OR (a.location_code LIKE '006%')
										OR (a.location_code LIKE '004%'))
										AND (a.status = '003')
										AND (a.location_code LIKE 'PO%')) 
								UNION 

								SELECT 
									a.wmtid AS Id,
									a.gr_qty AS Qty,
									a.material_code AS WMaterialCode,
									a.at_no AS at_no,
									a.product AS product,
									(CASE
										WHEN ((a.gr_qty - qc.check_qty) IS NULL) THEN a.gr_qty
										ELSE (a.gr_qty - qc.check_qty)
									END) AS SLCK,
									a.buyer_qr AS buyer_qr,
									a.reg_date AS reg_dt
								FROM
									((w_material_info_tims a
									LEFT JOIN m_facline_qc qc ON ((a.material_code = qc.ml_no))))
								WHERE
									((a.status = '003') AND (a.buyer_qr <> ''OR a.buyer_qr IS NOT NULL)))
								AS a
								WHERE (@mtCode ='' OR @mtCode IS NULL OR a.WMaterialCode LIKE '%' + @mtCode +'%')
									AND (@at_no = '' OR @at_no IS NULL OR a.at_no LIKE '%'+ @at_no +'%')
									AND (@product = '' OR @product IS NULL OR a.product LIKE '%'+ @product +'%')

								Order by a.buyer_qr Desc";
				var result = await base.DbConnection.QueryAsync<PrintNGTimsReponse>(query, new { @mtCode = mt_cd, @at_no = at_no, @product = product });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IReadOnlyList<MaterialInfoTIMS>> GetListDataPrintNGOK()
		{
			try
			{
				var query = @"SELECT * FROM w_material_info_tims AS a WHERE a.status = '012' AND a.location_code LIKE '006%' Order by wmtid Desc";
				var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(query);
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IReadOnlyList<MaterialInfoTIMS>> GetMaterialInfoTimsByCode(string mt_cd)
		{
			try
			{
				var query = @"Select * From w_material_info_tims Where material_code = @Code And status = '003'";
				var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(query, new { @Code = mt_cd });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IReadOnlyList<MaterialInfoTIMS>> GetMaterialInfoTimsByBuyerQR(string buyer_qr)
		{
			try
			{
				var query = @"Select * From w_material_info_tims Where buyer_code = @BuyerCode And status = '003'";
				var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(query, new { @BuyerCode = buyer_qr });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<IReadOnlyList<MaterialInfoTIMS>> GetListMaterialInfoTimsByBuyerQR(string buyer_qr)
		{
			try
			{
				var query = @"Select * From w_material_info_tims Where buyer_code = @BuyerCode order by wmtid DESC";
				var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(query, new { @BuyerCode = buyer_qr });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<int> InsertIntoMaterialDown(MaterialDown item)
		{
			try
			{
				var query = @"Insert Into w_material_down(mt_cd, gr_qty, gr_down, reason, status_now, bb_no, use_yn, reg_dt, chg_id, chg_dt)
								Values (@mt_cd, @gr_qty, @gr_down, @reason, @status_now, @bb_no, @use_yn, @reg_dt, @chg_id, @chg_dt)
								Select Scope_Identity()";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IReadOnlyList<MaterialInfoTIMS>> GetMaterialWithStatusOk(string mt_cd)
		{
			try
			{
				var query = @"Select * From w_material_info_tims Where material_code Like @Code + '-OK' + '%'";
				var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(query, new { @Code = mt_cd });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IReadOnlyList<MaterialInfoTIMS>> GetMaterialInfoTimsByBuyerCode(string buyer_code)
		{
			try
			{
				var query = @"Select * From w_material_info_tims Where buyer_code Like '%'+ @Code +'%'";
				var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(query, new { @Code = buyer_code });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> UpdateMaterialInfoTimsScan(string materialCode, int grpQty, string statusCode, string userId)
		{
			string sql = @"UPDATE w_material_info_tims SET gr_qty = @Grp_Qty, [status] = @Status, location_code = '006000000000000000', chg_date = GETDATE(), chg_id = @UserId WHERE material_code=@mtcd";
			return await base.DbConnection.ExecuteAsync(sql, new { @Grp_Qty = grpQty, @Status = statusCode, @mtcd = materialCode, @UserId = userId });
		}

		public async Task<int> UpdateMaterialInfoTims(string materialCode, int grpQty, int realQty, string userId)
		{
			string sql = @"UPDATE w_material_info_tims SET gr_qty = @Grp_Qty, real_qty = @Real, location_code = '006000000000000000', chg_date = GETDATE(), chg_id = @UserId WHERE material_code = @mtcd";
			return await base.DbConnection.ExecuteAsync(sql, new { @Grp_Qty = grpQty, @Real = realQty, @mtcd = materialCode, @UserId = userId });
		}

		public async Task<IReadOnlyList<MaterialInfoTIMS>> GetMaterialInfoMMSByOrgin(string orgin_mt_cd)
		{
			try
			{
				var query = @"Select * From w_material_info_tims where material_code = @Orgin order by reg_date DESC";
				var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(query, new { @Orgin = orgin_mt_cd });
				return result.ToList();
			}
			catch (Exception e)
			{

				throw e;
			}
		}

		public async Task<IReadOnlyList<MaterialDown>> GetMaterialDownByCode(string mt_cd)
		{
			try
			{
				var query = @"Select wmtid,mt_cd,gr_qty,gr_down,reason,status_now,bb_no,use_yn,reg_id,CONVERT(VARCHAR(23),convert(datetimE,reg_dt,121),121) reg_dt,chg_id,CONVERT(VARCHAR(23),convert(datetimE,chg_dt,121),121) chg_dt from w_material_down where mt_cd = @Mt_Cd";
				var result = await base.DbConnection.QueryAsync<MaterialDown>(query, new { @Mt_Cd = mt_cd });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IReadOnlyList<QCItemCheckMaterial>> GetQCItemCheckMaterial(string item_vcd)
		{
			try
			{
				var query = @"Select * From qc_itemcheck_mt Where item_vcd = @Item And del_yn = 'N'";
				var result = await base.DbConnection.QueryAsync<QCItemCheckMaterial>(query, new { @Item = item_vcd });
				return result.ToList();
			}
			catch (Exception e)
			{

				throw e;
			}
		}

		public async Task<IReadOnlyList<QCItemCheckDetail>> GetQCItemCheckMaterialDetail(string item_vcd, string check_id)
		{
			try
			{
				var query = @"Select * From qc_itemcheck_dt where item_vcd = @Item And check_cd = @Code And del_yn = 'N' And defect_yn = 'Y'";
				var result = await base.DbConnection.QueryAsync<QCItemCheckDetail>(query, new { @Item = item_vcd, @Code = check_id });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IReadOnlyList<FaclineQCValueResponse>> Getfacline_qc_value(string fq_no)
		{
			try
			{
				var query = @"Select m.Id, q.check_subject, m.check_value, m.check_qty, m.date_ymd
							From m_facline_qc_value As m JOIN qc_itemcheck_mt q On m.check_id = q.check_id 
							where m.fq_no = @Fq_No";
				var result = await base.DbConnection.QueryAsync<FaclineQCValueResponse>(query, new { @Fq_No = fq_no });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<int> Updatemfaclineqcvalue(int checkqty, string userid, int id)
		{
			string sql = @"Update m_facline_qc_value set check_qty=@checkqty,check_id=@userid,chg_dt=GETDATE() WHERE Id=@id";
			return await base.DbConnection.ExecuteAsync(sql, new { checkqty = checkqty, userid = userid, id = id });
		}

		public async Task<IReadOnlyList<Getfacline_qc_PhanLoaiReponse>> Getfacline_qc_PhanLoai(string item_vcd, string mt_lot)
		{
			try
			{
				var query = @"SELECT b.Id,b.fq_no,b.check_qty, b.ml_tims,
							CONCAT(SUBSTRING(b.work_dt,1,4),'-',SUBSTRING(b.work_dt,5,2),'-',SUBSTRING(b.work_dt,7,2),' ',SUBSTRING(b.work_dt,9,2),':',SUBSTRING(b.work_dt,11,2) ) As work_dt,
							(b.ok_qty),(b.check_qty)-(b.ok_qty) as defect_qty
							From m_facline_qc As b 
							where b.item_vcd = @item_vcd AND (b.ml_no='' or b.ml_no IS NULL) and b.ml_tims = @mt_lot And fq_no Like 'TI%' 
							Order by fq_no desc, check_qty,Id";
				var result = await base.DbConnection.QueryAsync<Getfacline_qc_PhanLoaiReponse>(query, new { @item_vcd = item_vcd, @mt_lot = mt_lot });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IReadOnlyList<NGPO>> GetNotGoodPO(string product, string mt_cd, string mt_no, string PO)
		{
			try
			{
				var query = @"SELECT wmtid,status ,mt_no, material_code As mt_cd,mt_no,gr_qty,(CASE WHEN material_code NOT LIKE 'po%' THEN (SELECT Top 1 at_no FROM w_actual WHERE id_actual=id_actual ) ELSE (substring(material_code,1,11)) END) at_no 
							--product
							FROM w_material_info_tims 
							WHERE status='012' AND gr_qty> 0 
							 AND (@Product = '' OR @Product IS NULL OR mt_no LIKE '%' + @Product + '%') 
							 AND (@MaterialCode ='' OR @MaterialCode IS NULL OR material_code LIKE '%' + @MaterialCode + '%') 
							 AND (@Mt_No = '' OR @Mt_No IS NULL OR mt_no LIKE '%' + @Mt_No + '%') 
							 AND (@PO = '' OR @PO IS NULL OR material_code LIKE '%' + @PO + '%') 
							ORDER BY wmtid";
				var result = await base.DbConnection.QueryAsync<NGPO>(query, new { @Product = product, @MaterialCode = mt_cd, @Mt_No = mt_no, @PO = PO });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IReadOnlyList<MaterialInfoTIMS>> GetListDataMaterialInfoTIMSById(List<int> listId)
		{
			try
			{
				var result = new List<MaterialInfoTIMS>();
				foreach (var id in listId)
				{
					var query = @"SELECT * FROM w_material_info_tims Where wmtid = @Id and gr_qty> 0 and status = '012' order by gr_qty Desc";
					var rs = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoTIMS>(query, new { @Id = id });
					result.Add(rs);
				}
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IReadOnlyList<MaterialInfoTIMS>> GetListNumberForTIMS(string mt_cd)
		{
			try
			{
				var query = @"Select * From w_material_info_tims where material_code Like @Mt_Cd + '-MG%'";
				var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(query, new { @Mt_Cd = mt_cd });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<bool> CheckExistMaterialMapping(string mt_cd, string mt_lot)
		{
			string sqlquery = "SELECT wmmid FROM w_material_mapping_tims WHERE mt_cd=@mtcd AND mt_lot=@mtlot";
			//return db.Database.SqlQuery<int?>(sqlquery, new MySqlParameter("1", mt_cd), new MySqlParameter("2", mt_lot)).FirstOrDefault().HasValue;
			string wmmid = await base.DbConnection.QueryFirstOrDefaultAsync<string>(sqlquery, new { mtcd = mt_cd, mtlot = mt_lot });
			return string.IsNullOrEmpty(wmmid);
        }

		public async Task<int> InsertMaterialMappingTims(w_material_mapping data)
		{
			try
			{
				string sqlquery = @"INSERT INTO w_material_mapping_tims(mt_cd,mt_lot,mt_no,mapping_dt,bb_no,use_yn,del_yn,chg_id,reg_id,reg_dt,chg_dt,remark) 
									VALUES (@mtcd,@mtlot,@mtno,@mappingdt,@bbno,@useyn,@delyn,@chgid,@regid,@regdt,@chgdt,@remark);
									Select Scope_Identity()";
				return await base.DbConnection.ExecuteAsync(sqlquery, new
				{
					mtcd = data.mt_cd,
					mtlot = data.mt_lot,
					mtno = data.mt_no,
					mappingdt = Convert.ToDateTime(data.mapping_dt),
					bbno = data.bb_no,
					useyn = data.use_yn,
					delyn = data.use_yn,
					chgid = data.chg_id,
					regid = data.reg_id,
					regdt = data.reg_dt,
					chgdt = data.chg_dt,
					remark = data.remark
				});
			}


			catch (Exception e)
			{

				throw e;
			}
		}

		public async Task<IReadOnlyList<WMaterialInfoTmp>> Getdbobbinlcthist(int id_actual, string at_no, string bb_nm, string bb_no)
		{
			try
			{
				string sqlquery = @"SELECT wmtid,id_actual,id_actual id_actual_oqc,staff_id,staff_id staff_id_oqc,
				material_type mt_type,material_code mt_cd,mt_no,gr_qty,real_qty,reg_date AS date,
				[status] mt_sts_cd,bb_no,
				location_code lct_cd,
				from_lct_code from_lct_cd,reg_date input_dt,orgin_mt_cd,
				sts_update,active use_yn,reg_id,reg_date reg_dt,chg_id,chg_date chg_dt 
				FROM w_material_info_tims 
				WHERE bb_no is not NULL AND gr_qty> 0 AND [status] in ('002','008') 
				AND location_code='006000000000000000' 
				AND id_actual=(SELECT TOP 1 id_actual FROM w_actual WHERE id_actual !=@idactual AND at_no=@atno AND type='TIMS' AND active=1 and IsFinish = '1' ORDER BY level DESC);";
				if (string.IsNullOrEmpty(bb_no))
				{
					var result = await base.DbConnection.QueryAsync<WMaterialInfoTmp>(sqlquery, new { idactual = id_actual, atno = at_no });
					return result.ToList();
				}
				else
				{
					var result = await base.DbConnection.QueryAsync<WMaterialInfoTmp>(sqlquery, new { idactual = id_actual, atno = at_no });
					return result.ToList();
					//new MySqlParameter("2", at_no)).Where(i => i.bb_no.Contains(bb_no));
				}
			}
			catch (Exception ex)
			{

				throw;
			}
		}

		public async Task<bool> CheckStaffShift(int id_actual, string staff_id)
		{
			string QuerySQL = @"SELECT a.psid FROM d_pro_unit_staff AS a 
								WHERE a.id_actual = @idactual AND a.staff_id = @staffid
								AND datediff(HOUR,CONVERT(DATETIME,a.start_dt,120),CONVERT(DATETIME,GETDATE(),120))>=0 and datediff(HOUR,CONVERT(DATETIME,GETDATE(),120),CONVERT(DATETIME,a.end_dt,120)) >=0";
			string psid = await base.DbConnection.QueryFirstOrDefaultAsync<string>(QuerySQL, new { staffid = staff_id, idactual = id_actual });
			return String.IsNullOrWhiteSpace(psid);
		}

		public async Task<string> GetNameStatusCommCode(string mt_sts_cd)
		{
			string sql = @"SELECT dt_nm FROM comm_dt WHERE mt_cd='WHS005' AND dt_cd=@dtcd";
			return await base.DbConnection.QueryFirstOrDefaultAsync<string>(sql, new { dtcd = mt_sts_cd });
		}
		//public async Task<dt_nm> GetNameStatusCommCode(string mt_sts_cd)
		//{
		//	string sql = @"SELECT dt_nm FROM comm_dt WHERE mt_cd='WHS005' AND dt_cd=@dtcd";
		//	return await base.DbConnection.QueryFirstOrDefaultAsync<string>(sql, new { dtcd = mt_sts_cd });
		//}

		public async Task<Models.TIMS.WActual> FindOneWActual(int? id_actual)
		{
			string sql = @"select id_actual ,at_no,type,product,
                         actual,defect,name,level,date,don_vi_pr,item_vcd,reg_id,reg_dt,chg_id,chg_dt
                         from w_actual where id_actual = @idactual";
			return await base.DbConnection.QueryFirstOrDefaultAsync<Models.TIMS.WActual>(sql, new { idactual = id_actual });
		}

		public async Task<IReadOnlyList<w_actual>> GetListWActualForProcess(string at_no)
		{
			string sqlquery = @"SELECT * FROM w_actual WHERE at_no=@atno AND type = 'TIMS' AND name != 'OQC' AND active=1";
			var result = await base.DbConnection.QueryAsync<w_actual>(sqlquery, new { atno = at_no });
			return result.ToList();
		}

		#region
		public async Task<IReadOnlyList<ReceivingScanFGResponse>> GetListDataReceivingScanFG(string product, string buyer, string po, string lot_date, string lot_date_end)
		{
			try
			{

				//DateTime? lotdate = null;
				//DateTime? lotdateend = null;

				//if (!string.IsNullOrEmpty(lot_date))
    //            {
				//	lotdate = DateTime.ParseExact(lot_date, "yyyy-MM-dd HH:mm tt", null);

				//}

    //            if (!string.IsNullOrEmpty(lot_date_end))
    //            {
				//	lotdateend = DateTime.ParseExact(lot_date_end, "yyyy-MM-dd HH:mm tt", null);

				//}

                //var query = @"SELECT a.at_no po,a.product,product.style_nm product_nm, a.wmtid,a.mt_no,a.gr_qty, a.status,comm.dt_nm sts_nm,  a.material_code, a.bb_no, a.buyer_qr, stamp.lot_date 
                //			FROM w_material_info_tims AS a 
                //				LEFT JOIN stamp_detail AS stamp ON  a.buyer_qr = stamp.buyer_qr 
                //				JOIN d_style_info AS product ON a.product = product.style_no 
                //				JOIN comm_dt AS comm ON a.status = comm.dt_cd AND comm.mt_cd='WHS005' 
                //			WHERE a.status='010' AND a.location_code LIKE '006%'  AND a.buyer_qr IS NOT null 
                //			AND (@Product ='' OR @Product IS NULL OR a.product LIKE '%' + @Product + '%')
                //			AND (@Buyer ='' OR @Buyer IS NULL OR a.buyer_qr LIKE '%' + @Buyer + '%')
                //			AND (@PO ='' OR @PO IS NULL OR a.at_no LIKE '%' + @PO + '%')
                //			AND (@LotDate ='' OR @LotDate IS NULL OR CONVERT(DATETIME,stamp.lot_date,120) >=CONVERT(DATETIME, @LotDate,120))
                //			AND (@LotDateEnd ='' OR @LotDateEnd IS NULL OR stamp.lot_date <= CONVERT(DATETIME,@LotDateEnd,120))";
                string query = @"SELECT a.at_no po,a.product,product.style_nm product_nm, a.wmtid,a.mt_no,a.gr_qty, a.status,comm.dt_nm sts_nm,  a.material_code, a.bb_no, a.buyer_qr, stamp.lot_date 
							FROM (select at_no,product,wmtid,mt_no,gr_qty,status,material_code,bb_no,buyer_qr from w_material_info_tims 
							where status='010' AND location_code LIKE '006%'  AND buyer_qr IS NOT null 
							AND (@Product ='' OR @Product IS NULL OR product LIKE '%' + @Product + '%') 
							AND (@Buyer ='' OR @Buyer IS NULL OR buyer_qr LIKE '%' + @Buyer + '%')
							AND (@PO ='' OR @PO IS NULL OR at_no LIKE '%' + @PO + '%')
							) AS a 
							inner JOIN (select lot_date,buyer_qr from stamp_detail where (@LotDate ='' OR @LotDate IS NULL OR lot_date >=@LotDate )
						AND (@LotDateEnd ='' OR @LotDateEnd IS NULL OR lot_date <=@LotDateEnd )
							) AS stamp ON  a.buyer_qr = stamp.buyer_qr 
								JOIN d_style_info AS product ON a.product = product.style_no 
								JOIN comm_dt AS comm ON a.status = comm.dt_cd AND comm.mt_cd='WHS005' 
						order by a.product, stamp.lot_date DESC";
				var result = await base.DbConnection.QueryAsync<ReceivingScanFGResponse>(query, new { @Product = product, @Buyer = buyer, @PO = po, @LotDate = lot_date, @LotDateEnd = lot_date_end });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<int> InsertMultiMaterialMppingTimsDV(string mtcd, string MtLot, string User, string bb_bo)
		{
			try
			{
				string sql = @"insert w_material_mapping_tims(mt_lot,mt_cd,mt_no,mapping_dt,bb_no,remark,sts_share,use_yn,del_yn,reg_id,reg_dt,chg_id,chg_dt,status)
				select @mtcd,material_code,a.mt_no,GETDATE(),@bbno,a.remark,null,'Y','N',@user,GETDATE() ,@user,GETDATE(),a.status
				from w_material_info_tims a
				where material_code =@mtlot";
				return await base.DbConnection.ExecuteAsync(sql, new { mtcd = mtcd, mtlot = MtLot, user = User, bbno = bb_bo }, commandTimeout: 60);
			}
			catch (Exception ex)
			{

				throw;
			}

		}
		public int GetRealQTYParent(string MaterialCodePar)
		{
			string sql = @"select real_qty from w_material_info_tims where material_code=@MaterialCodePar";
			return base.DbConnection.QueryFirstOrDefault<int>(sql, new { MaterialCodePar = MaterialCodePar });
		}
		public int GetRealQTYMaterialCodeDevie(string MaterialCodeDV)
		{
			string sql = @"if exists(select gr_qty from w_material_info_tims where material_code like  @MaterialCodeDV+'%')
							begin
							 select gr_qty from w_material_info_tims where material_code like @MaterialCodeDV+'%'
							 end
							else select 0";
			return base.DbConnection.QueryFirstOrDefault<int>(sql, new { MaterialCodeDV = MaterialCodeDV });

		}
		public int UpdateStatusMaterialParent(string MaterialCodePar)
		{
			string sql = @"update from w_material_info_tims set use_yn='N' where material_code = @MaterialCodePar";
			return base.DbConnection.Execute(sql, new { MaterialCodePar = MaterialCodePar });
		}
		public async Task<IReadOnlyList<FGReceiveDataResponse>> CheckBuyerStatus(string buyerCode)
		{
			try
			{
				var query = @"SELECT  a.wmtid,a.product ProductNo, d.style_nm ProductName, d.md_cd AS Model, a.bb_no, a.gr_qty As Quantity,a.buyer_qr As BuyerCode
								FROM w_material_info_tims AS a
								JOIN w_actual b On a.id_actual = b.id_actual
								JOIN w_actual_primary c ON c.at_no = b.at_no
								JOIN d_style_info AS d ON c.product = d.style_no
								WHERE a.buyer_qr = @BuyerQR
								and	a.location_code = '006000000000000000'
								AND a.status = '010'";
				var result = await base.DbConnection.QueryAsync<FGReceiveDataResponse>(query, new { @BuyerQR = buyerCode });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<MaterialInfoTIMS> FindOneMaterialInfoById(string buyerCode)
		{
			try
			{
				var query = @"Select Top 1 * from  w_material_info_tims where buyer_qr = @BuyerCode";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoTIMS>(query, new { @BuyerCode = buyerCode });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IReadOnlyList<Generalfg>> FindOneBuyerInfoById(string buyerCode)
		{
			try
			{
				var query = @"Select Top 1 * From  generalfg where buyer_qr = @BuyerCode";
				var result = await base.DbConnection.QueryAsync<Generalfg>(query, new { @BuyerCode = buyerCode });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IReadOnlyList<StampDetail>> FindStamp(string buyerCode)
		{
			try
			{
				var query = @"Select * From stamp_detail WHERE buyer_qr = @BuyerCode";
				var result = await base.DbConnection.QueryAsync<StampDetail>(query, new { @BuyerCode = buyerCode });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<string> GetStyleNameFromStyleInfo(string buyerCode)
		{
			try
			{
				var query = @"Select style_nm From d_style_info Where style_no = @ProductCode ";
				var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @ProductCode = buyerCode });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<string> GetModelCodeFromStyleInfo(string buyerCode)
		{
			try
			{
				var query = @"Select md_cd From d_style_info Where style_no = @ProductCode ";
				var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @ProductCode = buyerCode });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> UpdateReceFGWMaterialInfo(MaterialInfoTIMS item, string buyerCode)
		{
			try
			{
				var wmtid = Convert.ToString(item.wmtid);
				//var query = @"UPDATE w_material_info_tims SET location_code = @location_code, from_lct_code = @from_lct_code, to_lct_code = @to_lct_code, 
				//			status = @status, chg_id = @chg_id, chg_date = GETDATE() WHERE buyer_qr=@ListBuyerCode";

				//var result = await base.DbConnection.ExecuteAsync(query, new
				//{
				//	@location_code = item.location_code,
				//	@from_lct_code = item.from_lct_code,
				//	@to_lct_code = item.to_lct_code,
				//	@status = item.status,
				//	@chg_id = item.chg_id,
				//	@chg_dt = item.chg_date,
				//	@Id = wmtid,
				//	@ListBuyerCode = buyerCode
				//});
				var query = $"UPDATE w_material_info_tims SET location_code = '{item.location_code}', from_lct_code = {item.from_lct_code}, to_lct_code = '{item.to_lct_code}',status = '{item.status}', chg_id ='{item.chg_id}', chg_date = GETDATE() WHERE buyer_qr in ({buyerCode})";
				var result = await base.DbConnection.ExecuteAsync(query);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> InsertToGeneralFGByConditiion(string ListBuyerCode, string modelCode, string userID)
		{
			try
			{
				//var query = @"INSERT INTO generalfg (buyer_qr, product_code, qty, status, reg_id, reg_dt, chg_id, chg_dt, type, md_cd, at_no)
				//			SELECT  b.buyer_qr, c.product, b.gr_qty, b.status, @userID, GetDate(), @userID, GetDate(),'MES', @modelCode, a.at_no
				//			FROM w_material_info_tims AS b
				//			JOIN w_actual As a On a.id_actual = b.id_actual
				//			JOIN w_actual_primary As c On a.at_no = c.at_no
				//			WHERE wmtid = @ListId AND NOT EXISTS (SELECT * FROM generalfg WHERE buyer_qr = b.buyer_qr)";


//-------------------BAO FIX----------------------------------
				//var query = @"INSERT INTO generalfg  (buyer_qr,product_code,qty,lot_no,status,reg_id,reg_dt,chg_id,chg_dt,type,md_cd,at_no)
				//			SELECT  b.buyer_qr, b.product, b.gr_qty, stam.lot_date, b.status, @userID, GetDate(), @userID, GetDate(),'MES', @modelCode, b.at_no
				//			FROM w_material_info_tims AS b
				//			JOIN stamp_detail AS stam ON b.buyer_qr = stam.buyer_qr
				//			WHERE a.buyer_qr = @ListBuyerCode
				//			AND NOT EXISTS (SELECT * FROM generalfg WHERE buyer_qr = b.buyer_qr)";
				//var result = await base.DbConnection.ExecuteAsync(query, new { @ListBuyerCode = ListBuyerCode, @userID = userID, @modelCode = modelCode });

				var query = $"INSERT INTO generalfg  (buyer_qr,product_code,qty,lot_no,status,reg_id,reg_dt,chg_id,chg_dt,type,md_cd,at_no)	" +
				$" SELECT  b.buyer_qr, b.product, b.gr_qty, stam.lot_date, b.status, '{userID}', GetDate(), '{userID}', GetDate(),'MES', '{modelCode}', b.at_no " +
				$" FROM w_material_info_tims AS b JOIN stamp_detail AS stam ON b.buyer_qr = stam.buyer_qr " +
				$" WHERE b.buyer_qr in  ({ListBuyerCode})	AND NOT EXISTS (SELECT * FROM generalfg WHERE buyer_qr = b.buyer_qr)";
				var result = await base.DbConnection.ExecuteAsync(query);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> InsertMaterialMappingTIMS(MaterialMappingTIMS data)
		{
			try
			{
				var query = @"Insert Into w_material_mapping_tims (mt_lot, mt_cd, mt_no, mapping_dt, bb_no, remark, sts_share, use_yn, del_yn, 
								reg_id, reg_dt, chg_id, chg_dt, status)
							Values (@mt_lot, @mt_cd, @mt_no, @mapping_dt, @bb_no, @remark, @sts_share, @use_yn, @del_yn, 
							@reg_id, @reg_dt, @chg_id, @chg_dt, @status)
							Select Scope_Identity()";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, data);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> UpdateMaterial1(double? quantity, int wmtid, string status)
		{
			try
			{
				var query = @"UPDATE w_material_info_tims SET gr_qty = @Quantity, status = @Status WHERE wmtid = @Id";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Quantity = quantity, @Status = status, @Id = wmtid });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> UpdateMaterial2(double? quantity, int wmtid)
		{
			try
			{
				var query = @"UPDATE w_material_info_tims SET gr_qty = @Quantity WHERE wmtid = @Id";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Quantity = quantity, @Id = wmtid });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> InsertTmpMaterialInfo(MaterialInfoTam item)
		{
			try
			{
				var query = @"Insert Into w_material_info_tam (id_actual, id_actual_oqc, staff_id, staff_id_oqc, mt_type, mt_cd, mt_no, gr_qty, real_qty, date, alert_ng, expiry_dt, dt_of_receipt, expore_dt, recevice_dt_tims, lot_no, mt_barcode, mt_qrcode,
							status, bbmp_sts_cd,bb_no, use_yn, reg_id, reg_dt, chg_id, chg_dt)
							Values (@id_actual, @id_actual_oqc, @staff_id, @staff_id_oqc, @mt_type, @mt_cd, @mt_no, @gr_qty, @real_qty, @date, a2lert_ng, @expiry_dt, @dt_of_receipt, @expore_dt, @recevice_dt_tims, @lot_no, @mt_barcode, @mt_qrcode,
							@status, @bbmp_sts_cd, @bb_no, @use_yn, @reg_id, @reg_dt, @chg_id, @chg_dt)
							Select Scope_Identity()";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> InsertToGeneralFG(Generalfg item)
		{
			try
			{
				var query = @"INSERT INTO generalfg(buyer_qr, product_code, md_cd, dl_no, qty, lot_no, status, reg_id, reg_dt, chg_id, chg_dt, type)
							Values(@buyer_qr, @product_code, @md_cd, @dl_no, @qty, @lot_no, '001', @reg_id, @reg_dt, @chg_id, @chg_dt, 'SAP'); 
							Select Scope_Identity()";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IReadOnlyList<ExportToExcelReceiveFGResponse>> ExportToExcelReceiveFG(string product, string buyer, string po_no, DateTime? lot_date, DateTime? lot_date_end)
		{
			try
			{
				var query = @"SELECT b.at_no As PO,c.product,product.style_nm product_nm, 
								a.wmtid,a.mt_no,a.gr_qty, a.status,comm.dt_nm sts_nm, 
								a.material_code, a.bb_no, a.buyer_qr, stamp.lot_date

								FROM w_material_info_tims AS a 
								JOIN stamp_detail AS stamp ON  a.buyer_qr = stamp.buyer_qr
								JOIN w_actual As b On a.id_actual = b.id_actual
								JOIN w_actual_primary As c On c.at_no = b.at_no
								JOIN d_style_info AS product ON c.product = product.style_no
								JOIN comm_dt AS comm ON a.status = comm.dt_cd AND comm.mt_cd='WHS005'
								WHERE a.status='010' AND a.location_code = '006000000000000000' AND a.buyer_qr IS NOT null 

								AND (@Product = '' OR @Product IS NULL OR c.product LIKE '%' + @Product + '%')
								AND (@Buyer = '' OR @Buyer IS NULL OR a.buyer_qr LIKE '%' + @Buyer + '%')
								AND (@Po_No = '' OR @Po_No IS NULL OR b.at_no LIKE '%' + @Po_No + '%')
								AND (@lot_date = '' OR @lot_date IS NULL OR stamp.lot_date >= @lot_date)
								AND(@lot_date_end = '' OR @lot_date_end IS NULL OR stamp.lot_date  <= @lot_date_end)";
				var result = await base.DbConnection.QueryAsync<ExportToExcelReceiveFGResponse>(query, new { @Product = product, @Buyer = buyer, @Po_No = po_no, @lot_date = lot_date, @lot_date_end = lot_date_end });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}


		#endregion

		public async Task<IReadOnlyList<Models.NewVersion.MaterialInfoTIMS>> FindAllMeterialChangestPackingweb(int wmtid)
		{
			string QuerySQL = @"SELECT * 
			FROM w_material_info_tims
			WHERE wmtid = @wmtid  AND status = '009' AND location_code = '006000000000000000'";
			var result = await base.DbConnection.QueryAsync<Models.NewVersion.MaterialInfoTIMS>(QuerySQL, new { wmtid = wmtid });
			return result.ToList();
			//return db.Database.SqlQuery<MaterialInfoMMS>(QuerySQL, new MySqlParameter("1", wmtid));
		}
		public async Task<int> UpdateWMaterialInfoByIdMultiple(string mt_sts_cd, string chg_id, string end_production_dt, int wmtid)
		{
			string sql = @"UPDATE w_material_info_tims 
SET status =@status,chg_id =@chgid,sts_update =@chgdate,end_production_dt=GETDATE(),chg_date =GETDATE() WHERE wmtid =@wmtid ";
			//return db.Database.ExecuteSqlCommand(sql, new MySqlParameter("1", mt_sts_cd),
			//										  new MySqlParameter("2", chg_id),
			//										  new MySqlParameter("3", end_production_dt),
			//										  new MySqlParameter("4", wmtid));
			return await base.DbConnection.ExecuteAsync(sql, new { status = mt_sts_cd, chgid = chg_id, chgdate = end_production_dt, wmtid = wmtid });
		}
		public async Task<int> UpdateWMaterialInfoByIdMultipleforReturn(string mt_sts_cd, string chg_id, string end_production_dt, int wmtid)
		{
			string sql = @"UPDATE w_material_info_tims 
SET status =@status,chg_id =@chgid,sts_update =@chgdate,end_production_dt=null,chg_date =GETDATE() WHERE wmtid =@wmtid ";
			//return db.Database.ExecuteSqlCommand(sql, new MySqlParameter("1", mt_sts_cd),
			//										  new MySqlParameter("2", chg_id),
			//										  new MySqlParameter("3", end_production_dt),
			//										  new MySqlParameter("4", wmtid));
			return await base.DbConnection.ExecuteAsync(sql, new { status = mt_sts_cd, chgid = chg_id, chgdate = end_production_dt, wmtid = wmtid });
		}
		public async Task<Models.NewVersion.MaterialInfoTIMS> FindAllMeterialChangestPacking(int wmtid)
		{
			string QuerySQL = "SELECT * FROM w_material_info_tims WHERE wmtid = @wtmid AND status='009' AND location_code='006000000000000000';";
			return await base.DbConnection.QuerySingleOrDefaultAsync<Models.NewVersion.MaterialInfoTIMS>(QuerySQL, new { wtmid = wmtid });
			//return db.Database.SqlQuery<MaterialInfoMMS>(QuerySQL, new MySqlParameter("1", wmtid)).FirstOrDefault();
		}


		//****************************************************************************************
		#region Divide
		// Đ.Phong
		public async Task<IReadOnlyList<CommCode>> GetTIMSProcesses()
		{
			try
			{
				var query = @"Select * From comm_dt Where mt_cd = 'COM007' And use_yn = 'Y' And dt_cd Not Like 'STA%' And dt_cd Not Like 'ROT%'";
				var result = await base.DbConnection.QueryAsync<CommCode>(query);
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<DBobbinLctHist> FindBobbinLctHist(string bb_no)
		{
			try
			{
				var query = @"Select blno, mc_type,bb_no,bb_nm,mt_cd,start_dt,end_dt,use_yn,del_yn,reg_id,reg_dt,chg_id,chg_dt 
                            From d_bobbin_lct_hist Where bb_no = @BB_NO";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<DBobbinLctHist>(query, new { @BB_NO = bb_no });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<MaterialInfoTIMS> CheckMaterialForDiv(string mt_cd)
		{
			try
			{
				var query = @"SELECT Top 1 * FROM w_material_info_tims WHERE material_code = @MtCd AND sts_update ='composite' AND status = '002' AND gr_qty > 0";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoTIMS>(query, new { @MtCd = mt_cd });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> CheckMaterialMappingTims(string mt_cd)
		{
			try
			{
				var query = @"SELECT COUNT(*) FROM w_material_mapping_tims WHERE mt_cd = @mtcdd";
				return await base.DbConnection.QueryFirstOrDefaultAsync<int>(query, new { mtcdd = mt_cd });
				//return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}


		public async Task<int> CheckStaffShiftForId(int id_actual, int psid)
		{
			try
			{
				var query = @"SELECT COUNT(*) FROM d_pro_unit_staff AS a WHERE a.id_actual = @Actual AND a.psid = @Psid AND (GETDATE() BETWEEN a.start_dt AND a.end_dt)";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @Actual = id_actual, @Psid = psid });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> UpdateMaterialInfoTims(MaterialInfoTIMS item)
		{
			try
			{
				var query = @"Update w_material_info_tims SET id_actual= @id_actual, staff_id = @staff_id, material_code = @material_code, material_type = @material_type, gr_qty = @gr_qty, real_qty = @real_qty, status = @status,
							sts_update = @sts_update, mt_no = @mt_no, bb_no = @bb_no, location_code = @location_code,
							from_lct_code = @from_lct_code, orgin_mt_cd = @orgin_mt_cd, to_lct_code = @to_lct_code, alert_ng = @alert_ng, receipt_date = Getdate(),number_divide=@number_divide,
							 chg_date = GETDATE(), chg_id = @chg_id
							WHERE wmtid= @wmtid";
				var result = await base.DbConnection.ExecuteAsync(query, item);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> UpdatePartialBobbinInfo(string mt_cd)
		{
			try
			{
				var query = @"UPDATE d_bobbin_info SET mt_cd=NULL WHERE  mt_cd = @MtCd";
				var result = await base.DbConnection.ExecuteAsync(query, new { @MtCd = mt_cd });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> GetTotalwMaterialInfoDV(string mt_cd)
		{
			try
			{
				var query = @"SELECT COUNT(*) FROM w_material_info_tims WHERE material_code LIKE '%' + @MtCd + '%'";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @MtCd = mt_cd });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> DeleteBobbinHistoryDevideSta(string mt_cd)
		{
			try
			{
				var query = @"DELETE FROM d_bobbin_lct_hist WHERE  mt_cd=@MtCd";
				var result = await base.DbConnection.ExecuteAsync(query, new { @MtCd = mt_cd });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> InsertIntoMappingDevideSta(string bobbin, string mt_cd)
		{
			try
			{
				var query = @" INSERT INTO w_material_mapping_tims (mt_lot, mt_cd, mt_no, mapping_dt, bb_no, sts_share, use_yn, del_yn, reg_id, reg_dt, chg_id, chg_dt) 
							SELECT  material_code,@MtCd, mt_no, GetDate() , @Bobbin, NULL, 'Y', 'N', reg_id, GetDate(), chg_id, GetDate()
							FROM w_material_info_tims WHERE orgin_mt_cd = @MtCd AND material_code  LIKE @MtCd+'%' GROUP BY material_code";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Bobbin = bobbin, MtCd = mt_cd+ "-DV" });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> CheckWMaterialMapForDV(string mt_cd)
		{
			try
			{
				var query = @"SELECT wmmid FROM w_material_mapping_tims WHERE mt_cd Like @MtCd + '%'";
				var result = await base.DbConnection.ExecuteAsync(query, new { @MtCd = mt_cd + "-DV" });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> CheckWMaterialMappingForRedo(string mt_cd)
		{
			try
			{
				var query = @"SELECT count(wmmid)  FROM w_material_mapping_tims WHERE mt_cd != @MtCd1 AND mt_lot IN
(SELECT material_code FROM w_material_info_tims WHERE material_code LIKE @MtCd2 + '%')";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @MtCd1 = mt_cd, @MtCd2 = mt_cd + "-DV" });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IReadOnlyList<WMaterialnfoDV>> DSWMaterialDV(string mt_cd)
		{
			try
			{
				var query = @"SELECT Bobbin.bno, BobbinHis.blno
							FROM w_material_info_tims AS Material
							JOIN d_bobbin_info AS Bobbin ON Material.bb_no = Bobbin.bb_no
							JOIN d_bobbin_lct_hist AS BobbinHis  ON Material.bb_no = BobbinHis.bb_no
							WHERE Material.material_code LIKE @MtCd +'%'";
				var result = await base.DbConnection.QueryAsync<WMaterialnfoDV>(query, new { @MtCd = mt_cd + "-DV" });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> SumGrqtyDSWMaterialDV(string mt_cd)
		{
			try
			{
				var query = @"
							SELECT CASE
									WHEN SUM(gr_qty) > 0 THEN SUM(gr_qty)
										ELSE 0
										END as tinhtong
									FROM w_material_info_tims  WHERE material_code LIKE @MtCd + '%'";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @MtCd = mt_cd + "-DV" });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> UpdateMaterinInfoTims(MaterialMappingTIMS item)
		{
			try
			{
				var query = @"
							Update w_material_mapping_tims Set mt_cd = @mt_cd, mt_lot = @mt_lot, mt_no = @mt_no, mapping_dt = @mapping_dt, bb_no = @bb_no, remark = @remark, sts_share = @sts_share,
							use_yn = @use_yn, del_yn = @del_yn, chg_id = @chg_id, chg_dt = getdate(), status = @status
							Where wmmid = @wmmid";
				var result = await base.DbConnection.ExecuteAsync(query, item);
				return result;
			}
			catch (Exception)
			{

				throw;
			}
		}
		public async Task<int> UpdateBobbinMaterinInfoTimsDV(string mtlot,string mtcd,string userid,string bbno)
		{
			try
			{
				var query = @"
							Update w_material_mapping_tims Set bb_no = @bbno, chg_id = @chg_id, chg_dt = getdate()
							Where  mt_lot = @mt_lot and mt_cd=@mt_cd";
				var result = await base.DbConnection.ExecuteAsync(query, new { bbno=bbno, chg_id=userid, mt_lot =mtlot, mt_cd =mtcd});
				return result;
			}
			catch (Exception)
			{

				throw;
			}
		}
		public async Task<int> UpdateWMaterialQtyForRedo(double? gr_qty, string mt_cd)
		{
			try
			{
				var query = @"UPDATE w_material_info_tims SET status='002',gr_qty= @Qty,number_divide=0  WHERE material_code= @MtCd";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Qty = gr_qty, @MtCd = mt_cd });
				return result;
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<int> DeleteWMaterialQtyForRedo(string mt_cd)
		{
			try
			{
				var query = @"DELETE FROM w_material_info_tims WHERE material_code LIKE  @MtCd +'%'";
				var result = await base.DbConnection.ExecuteAsync(query, new { @MtCd = mt_cd + "-DV" });
				return result;
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<int> DeleteWMaterialMappingForRedo(string mt_cd)
		{
			try
			{
				var query = @"DELETE FROM w_material_mapping_tims  WHERE mt_cd = @MtCd";
				var result = await base.DbConnection.ExecuteAsync(query, new { @MtCd = mt_cd });
				return result;
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<int> DeleteWMaterialMappingWithMtLot(string mt_cd)
		{
			try
			{
				var query = @"DELETE FROM w_material_mapping_tims  WHERE mt_lot = @MtCd";
				var result = await base.DbConnection.ExecuteAsync(query, new { @MtCd = mt_cd });
				return result;
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<int> DeleteBoBbinHisForRedo(int blno)
		{
			try
			{
				var query = @"DELETE FROM d_bobbin_lct_hist  WHERE blno=@BlNo";
				var result = await base.DbConnection.ExecuteAsync(query, new { @BlNo = blno });
				return result;
			}
			catch (Exception ex)
			{

				throw;
			}
		}

		public async Task<int> UpdateBobbinInfoForRedo(int bno)
		{
			try
			{
				var query = @"UPDATE d_bobbin_info SET  mt_cd=NULL  WHERE bno=@BNo ";
				var result = await base.DbConnection.ExecuteAsync(query, new { @BNo = bno });
				return result;
			}
			catch (Exception ex)
			{

				throw;
			}
		}
		public async Task<int> UpdatemtcdforBobbinInfoForRedo(string bbno,string mtcd)
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

		public async Task<int> CheckMt_cdInTims(string mtcd)
		{
			try
			{
				var query = $"select count(*) from w_material_info_mms a Where  a.material_code = '{mtcd}' and a.status = '008' and a.location_code ='006000000000000000' ";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query/*, new { @mtcd = mtcd }*/);
				return result;
			}
			catch (Exception)
			{

				throw;
			}
		}
		public async Task<MaterialInfoTIMS> GetWMaterialInfoTIMS(int id)
		{
			try
			{
				var query = @"SELECT * FROM w_material_info_tims WHERE wmtid = @Id";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoTIMS>(query, new { @Id = id });
				return result;
			}
			catch (Exception)
			{

				throw;
			}
		}
		public async Task<IReadOnlyList<MaterialInfoTIMS>> GetWMaterialInfoWithmtcdLike(string mt_cd)
		{
			try
			{
				var query = @"SELECT * FROM w_material_info_tims WHERE material_code LIKE '%' + @MtCd + '%' order by reg_date DESC";
				var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(query, new { @MtCd = mt_cd });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<string> CheckBobbinHistory(string bb_no)
		{
			try
			{
				var query = @"SELECT bb_no FROM d_bobbin_lct_hist WHERE bb_no = @BbNo and active=1 and use_yn='Y'";
				var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @BbNo = bb_no });
				return result;
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<string> CheckWMaterialHasNG(string mt_cd)
		{
			try
			{
				var query = @"SELECT material_code FROM w_material_info_tims WHERE material_code = @MtCd AND location_code ='006000000000000000'";
				var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { MtCd = mt_cd });
				return result;
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<string> CheckFaclineQCHasNG(string ml_no)
		{
			try
			{
				var query = @"SELECT ml_no FROM m_facline_qc WHERE ml_no = @MlNo";
				var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @MlNo = ml_no });
				return result;
			}
			catch (Exception)
			{

				throw;
			}
		}
		public async Task<IReadOnlyList<m_facline_qc>> GetListDataFaclineQcwithmlno(string ml_no)
		{
			try
			{
				var query = @"Select * From m_facline_qc Where ml_no = @MlNo order by reg_dt DESC";
				var result = await base.DbConnection.QueryAsync<m_facline_qc>(query, new { @MlNo = ml_no });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<string> CheckWProductQCHasNG(string ml_no)
		{
			try
			{
				var query = @"SELECT ml_no FROM w_product_qc WHERE ml_no = @MlNo";
				var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @MlNo = ml_no });
				return result;
			}
			catch (Exception)
			{

				throw;
			}
		}


		public async Task<MaterialInfoTIMS> GetMaterialInfoTimsOfDevice(string mt_cd, string status)
		{
			try
			{
				var query = @"SELECT * FROM w_material_info_tims WHERE material_code Like '%' +@Mt_Cd+'%' AND [status] = @Status";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoTIMS>(query, new { @Mt_Cd = mt_cd, @Status = status });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<MaterialInfoTIMS> GetWMaterialInfoTIMSwithstatuswmtid(int id, string status)
		{
			try
			{
				var query = @"SELECT * FROM w_material_info_tims WHERE wmtid = @Id AND [status] = @Status";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoTIMS>(query, new { @Id = id, Status = status });
				return result;
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<MaterialInfoTIMS> GetMaterialInfoTimsOfDevice(string mt_cd)
		{
			try
			{
				var query = @"SELECT * FROM w_material_info_tims WHERE material_code = @Mt_Cd AND real_qty > 0";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoTIMS>(query, new { @Mt_Cd = mt_cd });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<MaterialInfoTIMS> GetMaterialInfoTimsOfDeviceGrpQTY(string mt_cd)
		{
			try
			{
				var query = @"SELECT * FROM w_material_info_tims WHERE material_code = @Mt_Cd AND gr_qty > 0";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoTIMS>(query, new { @Mt_Cd = mt_cd });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}


		public async Task<Models.NewVersion.MaterialInfoMMS> GetMaterialInfoTimsOfDeviceGrpQTYMMS(string mt_cd)
		{
			try
			{
				var query = @"SELECT * FROM w_material_info_mms WHERE material_code = @Mt_Cd AND gr_qty > 0";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<Models.NewVersion.MaterialInfoMMS>(query, new { @Mt_Cd = mt_cd });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<MaterialMappingTIMS> GetMaterialMappingTIMSById(int wmmid)
		{
			try
			{
				var query = @"Select * from w_material_mapping_tims where wmmid = @Id";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialMappingTIMS>(query, new { @Id = wmmid });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IReadOnlyList<MaterialMappingTIMS>> GetListDataMaterialMappingTIMSById(string mt_cd, string mt_lot)
		{
			try
			{
				var query = @"Select * from w_material_mapping_tims where mt_cd = @MtCd And mt_lot != @MtLot";
				var result = await base.DbConnection.QueryAsync<MaterialMappingTIMS>(query, new { @MtCd = mt_cd, @MtLot = mt_lot });
				return result.ToList();
			}
			catch (Exception e)
			{

				throw e;
			}
		}

		public async Task<IReadOnlyList<BobbinInfo>> SearchBobbinInfo(string bb_no, string bb_nm,string mt_cd, int intpage, int introw)
		{
			try
			{
				var query = @"SELECT a.bno,a.mc_type,a.bb_no,a.mt_cd,a.bb_nm,a.use_yn,a.purpose,a.barcode, a.re_mark,a.count_number,a.del_yn,
							  a.reg_id,a.reg_dt,a.chg_id,a.chg_dt FROM d_bobbin_info AS a 
							  WHERE 
							  (a.mt_cd = '' OR a.mt_cd IS NULL)
							  AND (@bb_no ='' OR @bb_no IS NULL OR a.bb_no LIKE '%' + @bb_no + '%')
							  AND (@bb_nm ='' OR @bb_nm  IS NULL OR a.bb_nm LIKE '%' + @bb_nm + '%')
							Order By a.bno DESC
							OFFSET @intpage ROWS FETCH NEXT @introw ROWS ONLY";
				var result = await base.DbConnection.QueryAsync<BobbinInfo>(query, new { @bb_no = bb_no, @bb_nm = bb_nm, @mt_cd = mt_cd , @intpage =intpage, @introw =introw});
				return result.ToList();
			}
			catch (Exception e)
			{

				throw e;
			}
		}
		public int GettotalRowSearchBobbinInfo(string bb_no, string bb_nm, string mt_cd)
		{
			try
			{
				var query = @"SELECT Count(a.bno) FROM d_bobbin_info AS a 
							  WHERE (a.mt_cd = '' OR a.mt_cd IS NULL)
							  AND (@bb_no ='' OR @bb_no IS NULL OR a.bb_no LIKE '%' + @bb_no + '%')
							  AND (@bb_nm ='' OR @bb_nm  IS NULL OR a.bb_nm LIKE '%' + @bb_nm + '%')";
				return base.DbConnection.QueryFirst<int>(query, new { @bb_no = bb_no, @bb_nm = bb_nm, @mt_cd = mt_cd});
				//return result.ToList();
			}
			catch (Exception e)
			{

				throw e;
			}
		}
		public async Task<IReadOnlyList<comm_dt>> GetStaff(string mtcd, string useyn)
		{
			try
			{
				var query = "select * from comm_dt Where mt_cd = @Mt_Cd AND use_yn = @Use_Yn AND dt_cd NOT LIKE '%ROT%' AND dt_cd NOT LIKE '%STA%' ORDER BY dt_cd DESC ";
				var result = await base.DbConnection.QueryAsync<comm_dt>(query, new { @Mt_Cd = mtcd, @Use_Yn = useyn });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IReadOnlyList<comm_dt>> GetCommon(string code)
		{
			try
			{
				var query = "Select * from comm_dt where mt_cd = @Code";
				var result = await base.DbConnection.QueryAsync<comm_dt>(query, new { @Code = code });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IReadOnlyList<Actual>> FindAllWActualByAtNo(string at_no)
		{
			try
			{
				var query = @"Select id_actual , at_no,type,actual,defect,name,level,date,don_vi_pr,item_vcd,reg_id,chg_id,chg_dt, active
							From w_actual where at_no = @AtNo ";
				var result = await base.DbConnection.QueryAsync<Actual>(query, new { @AtNo = at_no });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> CheckDMaterialInfo(string mt_no)
		{
			try
			{
				var query = @"Select * from d_material_info where mt_no = @MtNo";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @MtNo = mt_no });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		#endregion


		public async Task<int> DeleteActualForTIMS(int id)
		{
			try
			{
				var query = @"UPDATE w_actual SET active=0 where id_actual = @Id";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Id = id });
				return result;
			}
			catch (Exception e)
			{

				throw e;
			}
		}

		public List<truyxuatlot> Truyxuatlistlot(string mt_cd, string tentam, string buyer_qr, string atno)
		{
            try
            {

				string sqlquery = @"
					declare  @tmpb table(mapping_dt datetime,
					reg_dt datetime,
					wmmid int,
					mt_cd nvarchar(50),
					cccc nvarchar(50),
					mt_lot nvarchar(50),
					type nvarchar(50),
					bb_no nvarchar(50),
					process nvarchar(50),
					process_cd nvarchar(50),
					congnhan_time nvarchar(max),
					machine nvarchar(max),
					SLLD int,
					mt_no nvarchar(50),
					date datetime,
					lot_no nvarchar(50),
					mt_tpye nvarchar(50));

select *
into #tmpactual
from w_actual where at_no=@atno --and type='SX'
select *
into #tmpmmsinfo
from w_material_info_mms where id_actual in (select id_actual from #tmpactual where type='SX')

select * 
into #timptimsinfo
from w_material_info_tims where id_actual in (select id_actual from #tmpactual where type='TIMS')


	insert @tmpb
	select b.* 
	from (SELECT  mpptims.mapping_dt,mpptims.reg_dt,mpptims.wmmid,
	mpptims.mt_cd,mpptims.mt_cd cccc,mpptims.mt_lot,wactual.type,
	(CASE WHEN mtinfotims.bb_no IS NULL THEN mtinfomms.bb_no
	ELSE mtinfotims.bb_no END) bb_no,
	(CASE
						WHEN (mtinfotims.end_production_dt IS NOT  NULL)  THEN 	'OQC'	
						ELSE (SELECT dt_nm FROM comm_dt WHERE mt_cd ='COM007' and dt_cd=wactual.name)
					END
					)process,wactual.NAME process_cd,
					(STUFF((SELECT TOP 1 '<br>'+ CONCAT(n.userid,' - ',n.uname,' Start: ',CONVERT(datetime, staff.start_dt, 121 ),' End: ',CONVERT(datetime, staff.end_dt,121 ))
					FROM
						d_pro_unit_staff AS staff
						JOIN mb_info AS n ON n.userid = staff.staff_id 
					WHERE
						staff.id_actual = wactual.id_actual 
						AND CONVERT(datetime, mpptims.mapping_dt,121 ) BETWEEN CONVERT(datetime, staff.start_dt, 121 ) 
						AND CONVERT(datetime, staff.end_dt, 121 )  
						FOR XML PATH('')
					),1,1,'' )) congnhan_time,'' machine,
					(
					CASE
						
							WHEN ( wactual.type = 'TIMS' AND mtinfotims.real_qty IS NULL) THEN
							ISNULL(( SELECT TOP 1 check_qty FROM m_facline_qc WHERE ml_tims = mpptims.mt_lot AND ml_no = mpptims.mt_cd ), mtinfomms.real_qty ) 
							WHEN ( wactual.type = 'TIMS' AND mtinfotims.real_qty IS NOT NULL) THEN
							ISNULL(( SELECT TOP 1 check_qty FROM m_facline_qc WHERE ml_tims = mpptims.mt_lot AND ml_no = mpptims.mt_cd ), mtinfotims.real_qty ) 
							WHEN ( mtinfomms.material_type IS NULL) THEN
							(CASE 
							WHEN  mtinfomms.material_type!= 'CMT' THEN mtinfomms.gr_qty 
							WHEN ( mtinfomms.id_actual_oqc IS NULL OR mtinfomms.id_actual_oqc = '' OR mtinfomms.id_actual_oqc = 0 ) 
							THEN 
							ISNULL((
								SELECT TOP 1
									check_qty 
								FROM
									m_facline_qc 
								WHERE
									ml_tims = mpptims.mt_lot 
									AND ml_no = mpptims.mt_cd),
								ISNULL((
										mtinfomms.real_qty -(
										SELECT TOP 1
											( check_qty - ok_qty ) 
										FROM
											m_facline_qc 
										WHERE
											ml_no = mpptims.mt_cd 
										ORDER BY
											reg_dt ASC 
										)),
									mtinfomms.real_qty 
								))
						
							END)

							ELSE mtinfomms.real_qty 
						END 
						) SLLD,
					(CASE
					WHEN mtinfotims.mt_no IS NULL THEN mtinfomms.mt_no
					ELSE mtinfotims.mt_no END ) mt_no,
					(CASE
					WHEN mtinfotims.reg_date IS NULL THEN mtinfomms.reg_date
					ELSE mtinfotims.reg_date END ) AS  date,
				
				
			
					(CASE
					WHEN mtinfotims.lot_no IS NULL THEN mtinfomms.lot_no
					ELSE mtinfotims.lot_no END ) lot_no,
					(CASE
					WHEN mtinfotims.material_type IS NULL THEN mtinfomms.material_type
					ELSE mtinfotims.material_type END ) mt_type
			
	FROM w_material_mapping_tims mpptims
	LEFT JOIN #timptimsinfo mtinfotims ON mtinfotims.material_code = mpptims.mt_cd
	LEFT JOIN #tmpmmsinfo mtinfomms ON mtinfomms.material_code = mpptims.mt_cd
	INNER JOIN #timptimsinfo infotims ON infotims.material_code = mpptims.mt_lot
	INNER JOIN #tmpactual wactual  ON wactual.id_actual=infotims.id_actual
	--WHERE wactual.at_no=@atno
	) b;

	with tree as(
	select *
	From @tmpb where mt_lot=@mtcd
	UNION ALL
	select b.*
	From @tmpb b
	inner join tree t on (t.mt_cd like concat(b.mt_lot,'-DV%') or t.mt_cd=b.mt_lot)
	)
	select * from tree";
				var result = base.DbConnection.Query<truyxuatlot>(sqlquery, new { mtcd = mt_cd , atno = atno },commandTimeout:180);
				return result.ToList();
			}
			catch (Exception ex)
			{

				throw;
			}

		}

		public async Task<int> UpdateWMaterialInfoById(int id_actual_oqc, string staff_id_oqc, string mt_sts_cd, string chg_id, int wmtid)
		{
			string sqlUpdate = @"Update w_material_info_tims SET id_actual_oqc=@idactualoqc ,
                                                            staff_id_oqc=@staffidoqc,
                                                            status=@status,
                                                            chg_id=@chgid
                                    where 
                            status <> '009' AND location_code like '006%' AND wmtid=@wmtid ";
			return await base.DbConnection.ExecuteAsync(sqlUpdate, new { idactualoqc = id_actual_oqc, staffidoqc = staff_id_oqc, status = mt_sts_cd, chgid = chg_id, wmtid = wmtid });


		}
		public async Task<IReadOnlyList<w_product_qc>> Checkwproductqc(string material_code)
		{
			string sql = @"select * from w_product_qc where ml_no=@materialcode";
			var result = await base.DbConnection.QueryAsync<w_product_qc>(sql, new { materialcode = material_code });
			return result.ToList();
		}

		public async Task<IReadOnlyList<MaterialInfoTIMS>> GetListmtmappingOQC(string product, string bb_bo, string at_no)
		{
			string sql = @"select  a.wmtid, bb.at_no,a.bb_no,cc.product, a.gr_qty,a.material_code mt_cd,a.buyer_qr, Convert(nvarchar(50),a.end_production_dt,120) input_dt
from w_material_info_tims as a 
INNER JOIN w_actual bb on a.id_actual=bb.id_actual and bb.active=1
inner join w_actual_primary cc on cc.at_no=bb.at_no and cc.active=1
join d_bobbin_lct_hist as b on a.bb_no=b.bb_no and a.material_code=b.mt_cd  

WHERE  a.buyer_qr is null and a.status='010' and  a.gr_qty>0 and a.active=1 and
(@mtno='' OR @mtno is null or a.mt_no LIKE '%'+@mtno+'%')and (@bbno='' or @bbno is null or a.bb_no LIKE '%'+@bbno+'%')and (@atno='' OR @atno is null OR bb.at_no LIKE '%'+@atno+'%')
order by wmtid DESC";
			var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(sql, new { mtno = product, bbno = bb_bo, atno = at_no });
			return result.ToList();
		}
		public async Task<bool> CheckQRBuyer(string bb_no)
		{
			string sql = @"SELECT buyer_qr FROM w_material_info_tims WHERE bb_no=@bbno and active=1";
			string buyerqr = await base.DbConnection.QueryFirstOrDefaultAsync<string>(sql, new { bbno = bb_no });
			return string.IsNullOrEmpty(buyerqr);
		}
		public async Task<IReadOnlyList<MaterialInfoTIMS>> CheckwmaterialinfoMappingbuyer(string mt_cd, string mt_sts_cd, string lct_cd)
		{
			string sql = @"Select a.wmtid,a.material_code mt_cd,a.bb_no,c.product,a.buyer_qr buyer_code,b.at_no,a.gr_qty,a.status mt_sts_cd,a.staff_id,a.staff_id_oqc,a.real_qty ,end_production_dt
from w_material_info_tims a
            inner join w_actual b on a.id_actual = b.id_actual
            inner join w_actual_primary c on b.at_no = c.at_no where a.material_code=@mtcd and a.status=@status and a.location_code = @lctcd";
			var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(sql, new { mtcd = mt_cd, status = mt_sts_cd, lctcd = lct_cd });
			return result.ToList();
			//return db.Database.SqlQuery<MaterialInfoMMS>(sql, new MySqlParameter("@1", mt_cd), new MySqlParameter("@2", mt_sts_cd), new MySqlParameter("@3", lct_cd));
		}

		public async Task<string> GetProductWactualPrimary(string mt_cd, string mt_sts_cd, string lct_cd)
		{
			string sql = @"Select c.product from w_material_info_tims a
            inner join w_actual b on a.id_actual = b.id_actual
            inner join w_actual_primary c on b.at_no = c.at_no
            where  a.material_code=@mtcd and a.status=@status and a.location_code = @lctcd";
			return await base.DbConnection.QueryFirstOrDefaultAsync<string>(sql, new { mtcd = mt_cd, status = mt_sts_cd, lctcd = lct_cd });
		}

		public async Task<stamp_detail> Getstampdetail(string buyer_qr)
		{
			string sql = @"SELECT * FROM stamp_detail WHERE buyer_qr=@buyerqr";
			return await base.DbConnection.QueryFirstOrDefaultAsync<stamp_detail>(sql, new { buyerqr = buyer_qr });
			//return db.Database.SqlQuery<stamp_detail>(sql, new MySqlParameter("@1", buyer_qr)).FirstOrDefault();
		}

		public async Task<d_style_info> GetStyleNo(string style_no)
		{
			string sql = @"Select * From d_style_info Where style_no=@styleno";
			return await base.DbConnection.QueryFirstOrDefaultAsync<d_style_info>(sql, new { styleno = style_no });
			//return db.Database.SqlQuery<d_style_info>(sql, new MySqlParameter("@1", style_no)).FirstOrDefault();
		}

		public async Task<int> Insertstampdetail(stamp_detail item)
		{
			string sql = @"Insert into stamp_detail(buyer_qr,stamp_code,product_code, ssver, vendor_code,vendor_line,label_printer,is_sample,pcn,lot_date,serial_number,machine_line,shift,standard_qty,is_sent,box_code,reg_id,reg_dt,chg_id,chg_dt, active)

            Values(@buyer_qr,@stamp_code,@product_code,@ssver, @vendor_code,@vendor_line,@label_printer,@is_sample,@pcn,@lot_date,@serial_number,@machine_line,@shift,@standard_qty,@is_sent,@box_code,
@reg_id,GETDATE(),@chg_id,GETDATE(),'1' )";
			return await base.DbConnection.ExecuteAsync(sql, item);
			//db.Database.ExecuteSqlCommand(sql, new MySqlParameter("@1", item.buyer_qr)
			//	, new MySqlParameter("@2", item.stamp_code), new MySqlParameter("@3", item.product_code), new MySqlParameter("@4", item.vendor_code)
			//	, new MySqlParameter("@5", item.vendor_line), new MySqlParameter("@6", item.label_printer), new MySqlParameter("@7", item.is_sample)
			//	, new MySqlParameter("@8", item.pcn), new MySqlParameter("@9", item.lot_date), new MySqlParameter("@10", item.serial_number)
			//	, new MySqlParameter("@11", item.machine_line), new MySqlParameter("@12", item.shift), new MySqlParameter("@13", item.standard_qty)
			//	, new MySqlParameter("@14", item.is_sent), new MySqlParameter("@15", item.box_code), new MySqlParameter("@16", item.reg_id)
			//	, new MySqlParameter("@17", item.chg_id));
		}
		public async Task<int> UpdateBuyerCodeforTims(string buyer_code, int wmtid, string chgid)
		{
			string sql = @"UPDATE w_material_info_tims SET buyer_qr =@buyercode,chg_id=@chgid,chg_date=GETDATE() where wmtid=@wmtid";
			return await base.DbConnection.ExecuteAsync(sql, new { buyercode = buyer_code, chgid = chgid, wmtid = wmtid });
		}

		public async Task<int> Deletedbobbininfo(int bno)
		{
			string sql = "Delete From d_bobbin_info where bno=@bno";
			return await base.DbConnection.ExecuteAsync(sql, new { bno = bno });
			//db.Database.ExecuteSqlCommand(sql, new MySqlParameter("@1", bno));
		}
		public async Task<int> GetQrcodeBuyer(string buyer_qr)
		{
			string sql = @"SELECT COUNT(wmtid) FROM w_material_info_tims WHERE buyer_qr=@buyrqr";
			return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { buyrqr = buyer_qr });
			//return db.Database.SqlQuery<int>(sql, new MySqlParameter("@1", buyer_qr)).FirstOrDefault();
		}

		public async Task<IReadOnlyList<InventoryGeneralResponse>> GetInventoryInfoGenerals(string mtCode, string sVBobbinCd, string prd_cd, string recDateStart, string recDateEnd, string mtNoSpecific, string status,string bom_type, string po,string model)
		{
			try
			{
				//var query = @"  SELECT * FROM(
				//SELECT a.product product_cd,c.style_nm product_nm,c.md_cd,a.at_no,
				//	ISNULL(SUM (CASE WHEN a.status='008' 
    //           				AND	 (@mtCode = '' OR @mtCode IS NULL OR a.material_code LIKE '%' + @mtCode + '%')
    //           				AND (@sVBobbinCd = '' OR @sVBobbinCd IS NULL OR a.bb_no LIKE '%'+ @sVBobbinCd + '%')
    //           				AND (@po ='' OR @po IS NULL OR a.at_no LIKE '%' + @po + '%')
    //           				AND (@prd_cd = '' OR @prd_cd IS NULL OR a.product LIKE '%'+ @prd_cd + '%') 
    //           				AND (@recDateStart = '' OR @recDateStart IS NULL OR a.end_production_dt >= @recDateStart) 
    //           				AND (@recDateEnd = '' OR  @recDateEnd IS NULL OR a.end_production_dt <=  @recDateEnd) 
				//			THEN a.gr_qty  ELSE 0  END ),0) AS 'HCK',

				//	ISNULL(Sum(CASE WHEN a.status='010'
    //           				AND	 (@mtCode = '' OR @mtCode IS NULL OR a.material_code LIKE '%' + @mtCode + '%')
    //           				AND (@sVBobbinCd = '' OR @sVBobbinCd IS NULL OR a.bb_no LIKE '%'+ @sVBobbinCd + '%')
    //           				AND (@po ='' OR @po IS NULL OR a.at_no LIKE '%' + @po + '%')
    //           				AND (@prd_cd = '' OR @prd_cd IS NULL OR a.product LIKE '%'+ @prd_cd + '%') 
    //           				AND (@recDateStart = '' OR @recDateStart IS NULL OR a.end_production_dt >= @recDateStart) 
    //           				AND (@recDateEnd = '' OR  @recDateEnd IS NULL OR a.end_production_dt <=  @recDateEnd) 
    //           THEN a.gr_qty  ELSE 0  END),0) AS 'HDG',

				//	ISNULL(CASE WHEN a.status='002' 
	   //        				AND	 (@mtCode = '' OR @mtCode IS NULL OR a.material_code LIKE '%' + @mtCode + '%')
    //           				AND (@sVBobbinCd = '' OR @sVBobbinCd IS NULL OR a.bb_no LIKE '%'+ @sVBobbinCd + '%')
    //           				AND (@po ='' OR @po IS NULL OR a.at_no LIKE '%' + @po + '%')
    //           				AND (@prd_cd = '' OR @prd_cd IS NULL OR a.product LIKE '%'+ @prd_cd + '%') 
    //           				AND (@recDateStart = '' OR @recDateStart IS NULL OR a.end_production_dt >= @recDateStart) 
    //          				AND (@recDateEnd = '' OR  @recDateEnd IS NULL OR a.end_production_dt <=  @recDateEnd) 
				//			AND a.id_actual Not In (SELECT Max(v.id_actual) As id_actual FROM w_actual AS v WHERE v.at_no = a.at_no AND v.type='TIMS' AND v.name!='OQC') 
				//			THEN a.gr_qty  ELSE 0  END,0) AS 'DKT',

				//	ISNULL(CASE WHEN a.status='002' 
    //            				AND	 (@mtCode = '' OR @mtCode IS NULL OR a.material_code LIKE '%' + @mtCode + '%')
    //          				AND (@sVBobbinCd = '' OR @sVBobbinCd IS NULL OR a.bb_no LIKE '%'+ @sVBobbinCd + '%')
    //          				AND (@po ='' OR @po IS NULL OR a.at_no LIKE '%' + @po + '%')
    //          				AND (@prd_cd = '' OR @prd_cd IS NULL OR a.product LIKE '%'+ @prd_cd + '%') 
    //           				AND (@recDateStart = '' OR @recDateStart IS NULL OR a.end_production_dt >= @recDateStart) 
    //           				AND (@recDateEnd = '' OR  @recDateEnd IS NULL OR a.end_production_dt <=  @recDateEnd) 
				//			AND a.id_actual In (SELECT MAX(v.id_actual) AS id_actual FROM w_actual AS v WHERE v.at_no = a.at_no AND v.type='TIMS' AND v.name!='OQC') 
				//			THEN a.gr_qty  ELSE 0  END,0) AS 'CKT' 
				//FROM w_material_info_tims As a
				//			LEFT JOIN d_material_info b ON  b.mt_no = a.mt_no
    //       					JOIN d_style_info AS c ON a.product=c.style_no 
				//WHERE  a.location_code = '006000000000000000' 
				//		AND ( a.status!= '000'  and a.status!= '003' and a.status!= '005' ) 
    //       				AND (@mtCode ='' OR @mtCode IS NULL OR a.location_code LIKE '%' + @mtCode + '%') 
    //       				AND (@sVBobbinCd ='' OR @sVBobbinCd IS NULL OR a.bb_no LIKE '%'+ @sVBobbinCd +'%')
    //       				AND (@po ='' OR @po IS NULL OR a.at_no LIKE '%'+ @po +'%') 
    //       				AND (@prd_cd ='' OR @prd_cd IS NULL OR a.product LIKE '%'+ @prd_cd +'%') 
    //       				AND (@recDateStart ='' OR @recDateStart IS NULL OR a.end_production_dt >=  @recDateStart ) 
    //       				AND (@recDateEnd ='' OR @recDateEnd IS NULL OR a.end_production_dt <=  @recDateEnd ) 
    //            GROUP BY a.product, a.at_no,c.style_nm, c.md_cd, a.status, a.id_actual, a.gr_qty, a.material_code,a.bb_no, a.end_production_dt) As table_view";
				string sql = @"select product product_cd,c.style_nm product_nm, c.md_cd,c.bom_type ,sum(HCK) AS 'HCK',sum(DKT) AS 'DKT',sum(HDG) AS 'HDG',sum(MAPPINGBUYER) as 'MAPPINGBUYER',sum(CKT) AS 'CKT',Sum(SORTING) AS 'SORTING'
from(

select cc.product,cc.at_no,(CASE WHEN a.status='008' THEN a.gr_qty  ELSE 0  END )AS 'HCK', 
                            (CASE WHEN a.status='002' AND a.id_actual !=( 
                            SELECT top 1  v.id_actual id_actual FROM w_actual AS v 
                            WHERE v.at_no=bb.at_no AND v.type='TIMS' AND v.name!='OQC' order by v.id_actual DESC
                            ) 
                            THEN (a.gr_qty)  ELSE 0  END) AS 'DKT', 

                            (CASE WHEN a.status='010' and (a.buyer_qr IS NULL or a.buyer_qr = '') THEN (a.gr_qty)  ELSE 0  END) AS 'HDG', 
							 (CASE WHEN a.status='010' and (a.buyer_qr IS NOT NULL or a.buyer_qr != '') THEN (a.gr_qty)  ELSE 0  END) AS 'MAPPINGBUYER', 
                            (CASE WHEN (a.status='002' or a.status='009')AND a.id_actual =( 
                            SELECT top 1  v.id_actual id_actual FROM w_actual AS v 
                            WHERE v.at_no=bb.at_no AND v.type='TIMS' AND v.name!='OQC' order by v.id_actual DESC
                            ) 
                            THEN (a.gr_qty)  ELSE 0  END) AS 'CKT' ,

                            (CASE WHEN a.status='015'  THEN (a.gr_qty)  ELSE 0  END )AS 'SORTING' 
							 
 
from w_material_info_tims  a
inner join w_actual bb on a.id_actual=bb.id_actual
inner join w_actual_primary cc on bb.at_no=cc.at_no
WHERE   a.location_code  = '006000000000000000' 
                           AND      a.status  in ('002','015','009','010','008')
AND (@status ='' OR @status IS NULL OR CHARINDEX(a.status,@status)  >0) 
								AND (@mtCode ='' OR @mtCode IS NULL OR a.location_code LIKE '%' + @mtCode + '%') 
           				AND (@sVBobbinCd ='' OR @sVBobbinCd IS NULL OR a.bb_no LIKE '%'+ @sVBobbinCd +'%')
           				AND (@po ='' OR @po IS NULL OR cc.at_no LIKE '%'+ @po +'%') 
           				AND (@prd_cd ='' OR @prd_cd IS NULL OR cc.product LIKE '%'+ @prd_cd +'%') 
           				AND (@recDateStart ='' OR @recDateStart IS NULL OR a.end_production_dt >=  @recDateStart ) 
           				AND (@recDateEnd ='' OR @recDateEnd IS NULL OR a.end_production_dt <=  @recDateEnd )   
union all
select cc.product,cc.at_no,(CASE WHEN a.status='008' THEN (a.gr_qty)  ELSE 0  END )AS 'HCK', 
                            (CASE WHEN a.status='009' 
                            THEN (a.gr_qty)  ELSE 0  END) AS 'DKT', 
                            (CASE WHEN a.status='010' THEN (a.gr_qty)  ELSE 0  END) AS 'HDG', 
							'' as 'MAPPINGBUYER',
                            (CASE WHEN a.status='002' AND a.id_actual =( 
                            SELECT top 1  v.id_actual id_actual FROM w_actual AS v 
                            WHERE v.at_no=bb.at_no AND v.type='TIMS' AND v.name!='OQC' order by v.id_actual DESC
                            ) 
                            THEN (a.gr_qty)  ELSE 0  END) AS 'CKT' ,
                            (CASE WHEN a.status='015'  THEN (a.gr_qty)  ELSE 0  END )AS 'SORTING' 

							from w_material_info_mms  a
inner join w_actual bb on a.id_actual=bb.id_actual
inner join w_actual_primary cc on bb.at_no=cc.at_no
WHERE  a.location_code = '006000000000000000'
 AND     a.status in ('008','002')  
AND (@status ='' OR @status IS NULL OR  CHARINDEX(a.status,@status)  >0 )
AND (@mtCode ='' OR @mtCode IS NULL OR a.location_code LIKE '%' + @mtCode + '%') 
           				AND (@sVBobbinCd ='' OR @sVBobbinCd IS NULL OR a.bb_no LIKE '%'+ @sVBobbinCd +'%')
           				AND (@po ='' OR @po IS NULL OR cc.at_no LIKE '%'+ @po +'%') 
           				AND (@prd_cd ='' OR @prd_cd IS NULL OR cc.product LIKE '%'+ @prd_cd +'%') 
           				AND (@recDateStart ='' OR @recDateStart IS NULL OR a.date_of_receipt >=  @recDateStart ) 
           				AND (@recDateEnd ='' OR @recDateEnd IS NULL OR a.date_of_receipt <=  @recDateEnd )                           
) cccc 
JOIN d_style_info AS c ON cccc.product=c.style_no 
where   (@bom_type ='' OR @bom_type is null or  c.bom_type like '%' + @bom_type+'%' )
   and  (@model ='' OR @model is null or  c.md_cd like '%' + @model+'%' )

group by product,c.style_nm,c.md_cd,c.bom_type
order by c.md_cd ASC,c.bom_type ASC ";
				var result = await base.DbConnection.QueryAsync<InventoryGeneralResponse>(sql, new { @mtCode = mtCode, @sVBobbinCd = sVBobbinCd, @po = po, @prd_cd = prd_cd, @recDateStart = recDateStart, @recDateEnd = recDateEnd , @status =status,bom_type =bom_type,@model = model});
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IReadOnlyList<GetInventoryGeneralDetailResponse>> GetInventoryDetail(string prd_cd, string mtCode, string sVBobbinCd, string recDateStart, string recDateEnd, string ProSpecific, string status, string po)
		{
			try
			{
				string query = "";
				//if (status.Contains("002") && !status.Contains("008"))
				//{
				//var giatri = " not in ";

				query = @"SELECT b.at_no,b.Id, b.WMaterialCode AS MaterialCode, b.WMaterialNo AS MaterialNo, b.WMaterialLength AS 'Length', b.WMaterialSize AS 'Size', b.WMaterialQty AS 'Qty', 
							b.WMaterialUnit AS 'Unit', b.WMaterialStatusCode AS StatusCode, b.WMaterialStatusName AS StatusName, b.WMaterialReceivedDate AS ReceivedDate,b.VBobbinCd, b.product_cd,b.buyer_qr ,convert(varchar(23),b.WMaterialReceivedDate) AS ReceivedDates
							FROM 
							(
								SELECT 
									a.id_actual AS id_actual,
									d.product AS product_cd,
									(SELECT d_style_info.style_nm FROM d_style_info WHERE(d.product = d_style_info.style_no)) AS product_nm,
									(SELECT d_style_info.md_cd FROM d_style_info WHERE(d.product = d_style_info.style_no)) AS md_cd,
									c.at_no AS at_no,
									a.wmtid AS Id,
									a.material_code AS WMaterialCode,
									a.mt_no AS WMaterialNo,
									a.material_type AS WMaterialType,
									a.status AS WMaterialStatusCode,
									a.gr_qty AS WMaterialGrQty,
									a.bb_no AS VBobbinCd,
									CONCAT(ISNULL(a.gr_qty, 0),ISNULL(b.unit_cd, '')) AS WMaterialLength,
									a.buyer_qr AS buyer_qr,
									(SELECT comm_dt.dt_nm FROM comm_dt WHERE ((comm_dt.dt_cd = a.status) AND (comm_dt.mt_cd = 'WHS005'))) AS WMaterialStatusName,
									CONCAT(ISNULL(b.width, 0),'*',ISNULL(a.gr_qty, 0)) AS WMaterialSize,
									(SELECT (CASE WHEN (b.bundle_unit = 'Roll') THEN
											(CASE WHEN((a.gr_qty IS NULL) OR (b.spec IS NULL)) THEN 0 ELSE (a.gr_qty / b.spec) END) ELSE ISNULL(a.gr_qty, 0) END)
										) AS WMaterialQty,
									ISNULL(b.bundle_unit, 'EA') AS WMaterialUnit,
									(case when  a.end_production_dt='' or a.end_production_dt is null then convert(datetime,a.reg_date,120) else  convert(datetime,a.end_production_dt,120) end) AS WMaterialReceivedDate
							
								FROM
									w_material_info_tims As a
									LEFT JOIN d_material_info b ON b.mt_no = a.mt_no
									JOIN w_actual c ON a.id_actual = c.id_actual
									JOIN w_actual_primary d ON c.at_no = d.at_no
								WHERE a.location_code LIKE '006%' --AND a.status NOT IN ('000' , '003', '005') 
									AND (@mtCode ='' OR @mtCode IS NULL OR a.location_code LIKE '%' + @mtCode + '%') 
									AND (@recDateStart ='' OR @recDateStart IS NULL OR a.end_production_dt >=  @recDateStart ) 
           						AND (@recDateEnd ='' OR @recDateEnd IS NULL OR a.end_production_dt <=  @recDateEnd ) 
							) AS b
								left join w_actual c on c.id_actual = b.id_actual and c.type ='TIMS'and  c.name != 'OQC'
								left join w_actual_primary d on b.at_no = d.at_no and d.product = @ProSpecific
								WHERE (@ProSpecific ='' OR @ProSpecific IS NULL OR b.product_cd = @ProSpecific)
							--	AND b.id_actual not in  (SELECT MAX(a.id_actual) FROM w_actual AS a WHERE a.type = 'TIMS'  AND a.name != 'OQC')
							--	AND b.at_no in  (SELECT a.at_no FROM w_actual_primary As a  WHERE a.product = @ProSpecific GROUP BY a.at_no)  
								
           						AND (@sVBobbinCd ='' OR @sVBobbinCd IS NULL OR b.VBobbinCd LIKE '%'+ @sVBobbinCd +'%')
           						AND (@po ='' OR @po IS NULL OR b.at_no LIKE '%'+ @po +'%') 
           				--		AND (@prd_cd ='' OR @prd_cd IS NULL OR b.product_cd = @prd_cd) 
           						-- AND (@status Like '%'+ b.WMaterialStatusCode + '%')
";
				query += $" AND b.WMaterialStatusCode in ({@status})  AND b.WMaterialQty >0";
				query += @" 
UNION ALL
SELECT b.at_no,b.Id, b.WMaterialCode AS MaterialCode, b.WMaterialNo AS MaterialNo, b.WMaterialLength AS 'Length', b.WMaterialSize AS 'Size', b.WMaterialQty AS 'Qty', 
							b.WMaterialUnit AS 'Unit', b.WMaterialStatusCode AS StatusCode, b.WMaterialStatusName AS StatusName, b.WMaterialReceivedDate AS ReceivedDate,b.VBobbinCd, b.product_cd,b.buyer_qr ,convert(varchar(23),b.WMaterialReceivedDate) AS ReceivedDates
							FROM 
							(
								SELECT 
									a.id_actual AS id_actual,
									d.product AS product_cd,
									(SELECT d_style_info.style_nm FROM d_style_info WHERE(d.product = d_style_info.style_no)) AS product_nm,
									(SELECT d_style_info.md_cd FROM d_style_info WHERE(d.product = d_style_info.style_no)) AS md_cd,
									c.at_no AS at_no,
									a.wmtid AS Id,
									a.material_code AS WMaterialCode,
									a.mt_no AS WMaterialNo,
									a.material_type AS WMaterialType,
									a.status AS WMaterialStatusCode,
									a.gr_qty AS WMaterialGrQty,
									a.bb_no AS VBobbinCd,
									CONCAT(ISNULL(a.gr_qty, 0),ISNULL(b.unit_cd, '')) AS WMaterialLength,
									'' AS buyer_qr,
									(SELECT comm_dt.dt_nm FROM comm_dt WHERE ((comm_dt.dt_cd = a.status) AND (comm_dt.mt_cd = 'WHS005'))) AS WMaterialStatusName,
									CONCAT(ISNULL(b.width, 0),'*',ISNULL(a.gr_qty, 0)) AS WMaterialSize,
									(SELECT (CASE WHEN (b.bundle_unit = 'Roll') THEN
											(CASE WHEN((a.gr_qty IS NULL) OR (b.spec IS NULL)) THEN 0 ELSE (a.gr_qty / b.spec) END) ELSE ISNULL(a.gr_qty, 0) END)
										) AS WMaterialQty,
									ISNULL(b.bundle_unit, 'EA') AS WMaterialUnit,
									(case when  a.date_of_receipt='' or a.date_of_receipt is null then convert(datetime,a.chg_date,120) else convert(datetime,a.date_of_receipt,120) end) AS WMaterialReceivedDate
							
								FROM
									w_material_info_mms As a
									LEFT JOIN d_material_info b ON b.mt_no = a.mt_no
									JOIN w_actual c ON a.id_actual = c.id_actual
									JOIN w_actual_primary d ON c.at_no = d.at_no
								WHERE a.location_code LIKE '006%' --AND a.status NOT IN ('000' , '003', '005') 
									AND (@mtCode ='' OR @mtCode IS NULL OR a.location_code LIKE '%' + @mtCode + '%') 
									AND (@recDateStart ='' OR @recDateStart IS NULL OR a.date_of_receipt >=  @recDateStart ) 
           						AND (@recDateEnd ='' OR @recDateEnd IS NULL OR a.date_of_receipt <=  @recDateEnd ) 
							) AS b
								left join w_actual c on c.id_actual = b.id_actual and c.type ='TIMS'and  c.name != 'OQC'
							    left join w_actual_primary d on b.at_no = d.at_no and d.product = @ProSpecific
								WHERE (@ProSpecific ='' OR @ProSpecific IS NULL OR b.product_cd = @ProSpecific)
							--	AND b.id_actual not in  (SELECT MAX(a.id_actual) FROM w_actual AS a WHERE a.type = 'TIMS'  AND a.name != 'OQC')
							--	AND b.at_no in  (SELECT a.at_no FROM w_actual_primary As a  WHERE a.product = @ProSpecific GROUP BY a.at_no)  
								
           						AND (@sVBobbinCd ='' OR @sVBobbinCd IS NULL OR b.VBobbinCd LIKE '%'+ @sVBobbinCd +'%')
           						AND (@po ='' OR @po IS NULL OR b.at_no LIKE '%'+ @po +'%') 
           					--	AND (@prd_cd ='' OR @prd_cd IS NULL OR b.product_cd = @prd_cd)
							--	AND (@status Like '%'+ b.WMaterialStatusCode + '%')
           						
";
				query += $" AND b.WMaterialStatusCode in ({@status})  AND b.WMaterialQty >0 order by b.at_no,b.VBobbinCd";
					if (status.Contains("HCOQC"))
					{
					query = @"SELECT b.at_no,b.Id, b.WMaterialCode AS MaterialCode, b.WMaterialNo AS MaterialNo, b.WMaterialLength AS 'Length', b.WMaterialSize AS 'Size', b.WMaterialQty AS 'Qty', 
							b.WMaterialUnit AS 'Unit', b.WMaterialStatusCode AS StatusCode, b.WMaterialStatusName AS StatusName, b.WMaterialReceivedDate AS ReceivedDate,b.VBobbinCd, b.product_cd,b.buyer_qr ,convert(varchar(23),b.WMaterialReceivedDate) AS ReceivedDates
							FROM 
							(
								SELECT 
									a.id_actual AS id_actual,
									d.product AS product_cd,
									(SELECT d_style_info.style_nm FROM d_style_info WHERE(d.product = d_style_info.style_no)) AS product_nm,
									(SELECT d_style_info.md_cd FROM d_style_info WHERE(d.product = d_style_info.style_no)) AS md_cd,
									c.at_no AS at_no,
									a.wmtid AS Id,
									a.material_code AS WMaterialCode,
									a.mt_no AS WMaterialNo,
									a.material_type AS WMaterialType,
									a.status AS WMaterialStatusCode,
									a.gr_qty AS WMaterialGrQty,
									a.bb_no AS VBobbinCd,
									CONCAT(ISNULL(a.gr_qty, 0),ISNULL(b.unit_cd, '')) AS WMaterialLength,
									a.buyer_qr AS buyer_qr,
									(SELECT comm_dt.dt_nm FROM comm_dt WHERE ((comm_dt.dt_cd = a.status) AND (comm_dt.mt_cd = 'WHS005'))) AS WMaterialStatusName,
									CONCAT(ISNULL(b.width, 0),'*',ISNULL(a.gr_qty, 0)) AS WMaterialSize,
									(SELECT (CASE WHEN (b.bundle_unit = 'Roll') THEN
											(CASE WHEN((a.gr_qty IS NULL) OR (b.spec IS NULL)) THEN 0 ELSE (a.gr_qty / b.spec) END) ELSE ISNULL(a.gr_qty, 0) END)
										) AS WMaterialQty,
									ISNULL(b.bundle_unit, 'EA') AS WMaterialUnit,
									(case when  a.end_production_dt='' or a.end_production_dt is null then convert(datetime,a.reg_date,120) else  convert(datetime,a.end_production_dt,120) end) AS WMaterialReceivedDate
							
								FROM
									w_material_info_tims As a
									LEFT JOIN d_material_info b ON b.mt_no = a.mt_no
									JOIN w_actual c ON a.id_actual = c.id_actual
									JOIN w_actual_primary d ON c.at_no = d.at_no
								WHERE a.location_code LIKE '006%' AND a.status  IN ('002' , 'HCOQC') 
									AND (@mtCode ='' OR @mtCode IS NULL OR a.location_code LIKE '%' + @mtCode + '%') 
									AND (@recDateStart ='' OR @recDateStart IS NULL OR a.end_production_dt >=  @recDateStart ) 
           						AND (@recDateEnd ='' OR @recDateEnd IS NULL OR a.end_production_dt <=  @recDateEnd ) 
							) AS b
							left join w_actual c on c.id_actual = b.id_actual and c.type ='TIMS'and  c.name != 'OQC'
							left join w_actual_primary d on b.at_no = d.at_no and d.product = @ProSpecific
								WHERE (@ProSpecific ='' OR @ProSpecific IS NULL OR b.product_cd = @ProSpecific )
							--	AND b.id_actual in  (SELECT MAX(a.id_actual) FROM w_actual AS a WHERE a.type = 'TIMS'  AND a.name != 'OQC')
							--	AND b.at_no in  (SELECT a.at_no FROM w_actual_primary As a  WHERE a.product = @ProSpecific GROUP BY a.at_no)  
								
           						AND (@sVBobbinCd ='' OR @sVBobbinCd IS NULL OR b.VBobbinCd LIKE '%'+ @sVBobbinCd +'%')
           						AND (@po ='' OR @po IS NULL OR b.at_no LIKE '%'+ @po +'%') 
           					--	AND (@prd_cd ='' OR @prd_cd IS NULL OR b.product_cd = @prd_cd ) 
           						--	AND (@status Like '%'+ b.WMaterialStatusCode + '%') 
								";
					query += $"  AND b.WMaterialStatusCode in ({@status})  AND b.WMaterialQty >0 ";
					query += @"
union all
SELECT b.at_no,b.Id, b.WMaterialCode AS MaterialCode, b.WMaterialNo AS MaterialNo, b.WMaterialLength AS 'Length', b.WMaterialSize AS 'Size', b.WMaterialQty AS 'Qty', 
							b.WMaterialUnit AS 'Unit', b.WMaterialStatusCode AS StatusCode, b.WMaterialStatusName AS StatusName, b.WMaterialReceivedDate AS ReceivedDate,b.VBobbinCd, b.product_cd,b.buyer_qr ,convert(varchar(23),b.WMaterialReceivedDate,21) AS ReceivedDates
							FROM 
							(
								SELECT 
									a.id_actual AS id_actual,
									d.product AS product_cd,
									(SELECT d_style_info.style_nm FROM d_style_info WHERE(d.product = d_style_info.style_no)) AS product_nm,
									(SELECT d_style_info.md_cd FROM d_style_info WHERE(d.product = d_style_info.style_no)) AS md_cd,
									c.at_no AS at_no,
									a.wmtid AS Id,
									a.material_code AS WMaterialCode,
									a.mt_no AS WMaterialNo,
									a.material_type AS WMaterialType,
									a.status AS WMaterialStatusCode,
									a.gr_qty AS WMaterialGrQty,
									a.bb_no AS VBobbinCd,
									CONCAT(ISNULL(a.gr_qty, 0),ISNULL(b.unit_cd, '')) AS WMaterialLength,
									'' AS buyer_qr,
									(SELECT comm_dt.dt_nm FROM comm_dt WHERE ((comm_dt.dt_cd = a.status) AND (comm_dt.mt_cd = 'WHS005'))) AS WMaterialStatusName,
									CONCAT(ISNULL(b.width, 0),'*',ISNULL(a.gr_qty, 0)) AS WMaterialSize,
									(SELECT (CASE WHEN (b.bundle_unit = 'Roll') THEN
											(CASE WHEN((a.gr_qty IS NULL) OR (b.spec IS NULL)) THEN 0 ELSE (a.gr_qty / b.spec) END) ELSE ISNULL(a.gr_qty, 0) END)
										) AS WMaterialQty,
									ISNULL(b.bundle_unit, 'EA') AS WMaterialUnit,
									(case when  a.date_of_receipt='' or a.date_of_receipt is null then convert(datetime,a.chg_date,120) else convert(datetime,a.date_of_receipt,120) end) AS WMaterialReceivedDate
							
								FROM
									w_material_info_mms As a
									LEFT JOIN d_material_info b ON b.mt_no = a.mt_no
									JOIN w_actual c ON a.id_actual = c.id_actual
									JOIN w_actual_primary d ON c.at_no = d.at_no
								WHERE a.location_code LIKE '006%' and  a.status  IN ('002' , 'HCOQC') 
									AND (@mtCode ='' OR @mtCode IS NULL OR a.location_code LIKE '%' + @mtCode + '%') 
									AND (@recDateStart ='' OR @recDateStart IS NULL OR a.date_of_receipt >=  @recDateStart ) 
           						AND (@recDateEnd ='' OR @recDateEnd IS NULL OR a.date_of_receipt <=  @recDateEnd ) 
							) AS b
							left join w_actual c on c.id_actual = b.id_actual and c.type ='TIMS'and  c.name != 'OQC'
							left join w_actual_primary d on b.at_no = d.at_no and d.product = @ProSpecific
								WHERE (@ProSpecific ='' OR @ProSpecific IS NULL OR b.product_cd = @ProSpecific )
							--	AND b.id_actual in  (SELECT MAX(a.id_actual) FROM w_actual AS a WHERE a.type = 'TIMS'  AND a.name != 'OQC')
							--	AND b.at_no in  (SELECT a.at_no FROM w_actual_primary As a  WHERE a.product = @ProSpecific GROUP BY a.at_no)  
								
           						AND (@sVBobbinCd ='' OR @sVBobbinCd IS NULL OR b.VBobbinCd LIKE '%'+ @sVBobbinCd +'%')
           						AND (@po ='' OR @po IS NULL OR b.at_no LIKE '%'+ @po +'%') 
           					--	AND (@prd_cd ='' OR @prd_cd IS NULL OR b.product_cd = @prd_cd ) 
           						
							--	AND (@status Like '%'+ b.WMaterialStatusCode + '%') 
								AND b.WMaterialQty > 0 ";
					query += $" AND b.WMaterialStatusCode in ({@status}) order by b.at_no,b.VBobbinCd";

					}
					var result = await base.DbConnection.QueryAsync<GetInventoryGeneralDetailResponse>(query, new { @ProSpecific = ProSpecific,  @mtCode = mtCode, @sVBobbinCd = sVBobbinCd, @po = po, @prd_cd = prd_cd, @recDateStart = recDateStart, @recDateEnd = recDateEnd, @status = status });
					return result.ToList();
				//}
				//else return null;

			}
			catch (Exception e)
			{

				throw e;
			}
		}


		#region WareHouse Common
		public async Task<IReadOnlyList<CommMt>> GetDataComCode()
		{
			try
			{
				var query = @"Select * From comm_mt Where div_cd = 'WHS' And mt_cd Like 'WHS%'";
				var result = await base.DbConnection.QueryAsync<CommMt>(query);
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IReadOnlyList<CommMt>> SearchDataCommCode(string mt_cd, string mt_nm, string mt_exp)
		{
			try
			{
				var query = @"SELECT * FROM  comm_mt 
											WHERE div_cd='WHS'
											AND (@MtCd = '' OR @MtCd IS NULL OR  mt_cd Like '%' + @MtCd + '%' )
											AND (@MtNm = '' OR @MtNm IS NULL OR  mt_nm Like '%' + @MtNm + '%' )
											AND (@MtExp = '' OR @MtExp IS NULL OR   mt_exp Like '%' + @MtExp + '%' )";
				var result = await base.DbConnection.QueryAsync<CommMt>(query, new { @MtCd = mt_cd, @MtNm = mt_nm, @MtExp = mt_exp });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IReadOnlyList<WarehouseCommonResponse>> getWHSCommonDetailData(string mt_cd)
		{
			try
			{
				var query = @"Select d.cdid, m.mt_cd, m.mt_nm, d.dt_cd, d.dt_nm, d.dt_exp,d.dt_order, d.use_yn 
							From comm_mt  As m Join comm_dt As d On m.mt_cd = d.mt_cd where d.mt_cd = @MtCd";
				var result = await base.DbConnection.QueryAsync<WarehouseCommonResponse>(query, new { @MtCd = mt_cd });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> CountComCode()
		{
			try
			{
				var query = @"Select Count(*) From comm_mt where div_cd = 'WHS'";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<CommMt> GetLastDataComCode()
		{
			try
			{
				var query = @"Select Top 1 * From comm_mt where div_cd = 'WHS' Order by mt_cd Desc";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<CommMt>(query);
				return result;
			}
			catch (Exception e)
			{

				throw e;
			}
		}

		public async Task<int> InsertIntoCommMt(CommMt item)
		{

			try
			{
				var query = @"Insert into comm_mt(mt_cd, div_cd, mt_nm, mt_exp, memo, use_yn, reg_id, reg_dt, chg_id, chg_dt)
								Values(@mt_cd, @div_cd, @mt_nm, @mt_exp, @memo, @use_yn, @reg_id, @reg_dt, @chg_id, @chg_dt)
							    Select Scope_Identity()";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<CommMt> GetCommMtById(int Id)
		{
			try
			{
				var query = @"Select * From comm_mt Where mt_id = @Id";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<CommMt>(query, new { @Id = Id });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> RemoveCommMt(int Id)
		{
			try
			{
				var query = @"Delete comm_mt Where mt_id = @Id";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Id = Id });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IReadOnlyList<CommCode>> GetListDataCommDTByMtCd(string mt_cd)
		{
			try
			{
				var query = @"Select * From comm_dt where mt_cd = @Mt_Cd";
				var result = await base.DbConnection.QueryAsync<CommCode>(query, new { @Mt_Cd = mt_cd });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<CommCode> GetCommCodeById(int Id)
		{
			try
			{
				var query = @"Select * From comm_dt where cdid = @Id";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<CommCode>(query, new { @Id = Id });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> RemoveCommDT(int Id)
		{
			try
			{
				var query = @"Delete comm_dt Where cdid = @Id";
				var result = await base.DbConnection.ExecuteAsync(query, new { @Id = Id });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> UpdateCommMt(CommMt item)
		{
			try
			{
				var query = @"Update comm_mt Set div_cd = @div_cd, mt_cd = @mt_cd, mt_nm = @mt_nm, mt_exp = @mt_exp, memo = @memo, use_yn = @use_yn, 
							 reg_id = @reg_id, reg_dt = @reg_dt, chg_dt = @chg_dt, chg_id = @chg_id Where mt_id = @mt_id";
				var result = await base.DbConnection.ExecuteAsync(query, item);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> CountCommDT(string dt_cd, string mt_cd)
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


		public async Task<CommCode> GetDataCommDT(int Id, string mt_cd)
		{
			try
			{
				var query = @"Select * From comm_dt where mt_cd = @Mt_Cd And cdid =  @Id";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<CommCode>(query, new { @Mt_Cd = mt_cd, @Id = Id });
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
				var query = @"Insert Into comm_dt(mt_cd, dt_cd, dt_nm, dt_exp, dt_order, use_yn, del_yn, reg_id, reg_dt, chg_dt, chg_id )
							 Values(@mt_cd, @dt_cd, @dt_nm, @dt_exp, @dt_order, @use_yn, @del_yn, @reg_id, @reg_dt, @chg_dt, @chg_id)
							 Select Scope_Identity()";
				var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<CommCode> GetDataCommDTById(int Id)
		{
			try
			{
				var query = @"Select *, m.mt_nm From comm_dt As d JOIN comm_mt As m On d.mt_cd = m.mt_cd 
							Where d.cdid = @Id";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<CommCode>(query, new { @Id = Id });
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
				var query = @"Update comm_dt Set mt_cd = @mt_cd, dt_cd = @dt_cd, dt_nm = @dt_nm, dt_exp = @dt_exp, dt_order = @dt_order, del_yn = @del_yn,
								use_yn = @use_yn, reg_id = @reg_id, reg_dt = @reg_dt, chg_dt = @chg_dt, chg_id = @chg_id
								Where cdid = @cdid ";
				var result = await base.DbConnection.ExecuteAsync(query, item);
				return result;
			}
			catch (Exception e)
			{

				throw e;
			}
		}


		#endregion
		public async Task<DProUnitStaffAPIRespond> FindOneDProUnitStaffReturnByPsid(int psid)
		{
			string sql = @"SELECT a.psid,a.staff_id,a.staff_tp,a.id_actual
                            ,b.dt_nm AS staff_tp_nm,
                           c.uname, CONVERT(DATETIME,a.start_dt,120) AS start_dt,
                                    CONVERT(DATETIME,a.end_dt  ,120) AS end_dt,
                            a.use_yn
                FROM d_pro_unit_staff a
                LEFT JOIN comm_dt b ON  b.dt_cd=a.staff_tp AND b.mt_cd='COM013' AND b.use_yn='Y'
                LEFT JOIN mb_info c ON c.userid=a.staff_id
                where a.psid= @psid ";

			var result = await base.DbConnection.QueryFirstOrDefaultAsync<DProUnitStaffAPIRespond>(sql, new { psid = psid });
			return result;
		}
		public async Task<IReadOnlyList<MaterialInfoTIMS>> GetListOKPO(string mt_cd, string mt_no, string bb_no, string product, string at_no, string staff_id)
		{
			string sql = @"
SELECT a.wmtid,a.material_code mt_cd,a.mt_no,a.gr_qty,a.bb_no,b.at_no,''  staff_id 
			FROM w_material_info_mms AS a 
			JOIN w_actual AS b ON a.id_actual=b.id_actual 
			JOIN w_actual_primary AS c ON b.at_no=c.at_no 
			JOIN d_bobbin_lct_hist AS d ON a.material_code=d.mt_cd 
			WHERE a.location_code LIKE '006%' AND  a.gr_qty>0 AND a.status ='008' 
and (@atno='' or @atno IS NULL  or b.at_no LIKE '%'+@atno+'%')
			and (@mtcd='' or @mtcd IS NULL or a.material_code LIKE '%'+@mtcd+'%')and (@product='' or @product is null or c.product LIKE '%'+@product+'%')
			and (@mtno='' or @mtno is null  or a.mt_no LIKE '%'+@mtno+'%')and (@bbno='' or @bbno is null or a.bb_no LIKE '%'+@bbno+'%')
union all

SELECT a.wmtid,a.material_code mt_cd,a.mt_no,a.gr_qty,a.bb_no,b.at_no,a.staff_id  
			FROM w_material_info_tims AS a 
			JOIN w_actual AS b ON a.id_actual=b.id_actual 
			JOIN w_actual_primary AS c ON b.at_no=c.at_no 
			JOIN d_bobbin_lct_hist AS d ON a.material_code=d.mt_cd 
			WHERE a.location_code LIKE '006%' AND  a.gr_qty>0 AND b.type!='SX' and (@atno='' or @atno IS NULL  or b.at_no LIKE '%'+@atno+'%')
			and (@staffid='' OR @staffid IS NULL or a.staff_id LIKE '%'+@staffid+'%')and (@mtcd='' or @mtcd IS NULL or a.material_code LIKE '%'+@mtcd+'%')and (@product='' or @product is null or c.product LIKE '%'+@product+'%')
			and (@mtno='' or @mtno is null  or a.mt_no LIKE '%'+@mtno+'%')and (@bbno='' or @bbno is null or a.bb_no LIKE '%'+@bbno+'%')
order by a.wmtid DESC";
			var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(sql, new { mtcd = mt_cd, atno = at_no, staffid = staff_id, product = product, mtno = mt_no, bbno=bb_no });
			return result.ToList();
		}
		public async Task<IReadOnlyList<DatawActualPrimaryResponse>> GetAllFinishProducts(string product, string product_name, string model, string at_no, string regstart, string regend)
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
				                    (SELECT Top 1 d_style_info.style_nm FROM d_style_info WHERE (a.product = d_style_info.style_no)) AS style_nm,
				                    (SELECT Top 1 d_style_info.md_cd FROM d_style_info WHERE (a.product = d_style_info.style_no)) AS md_cd,
				                    (SELECT COUNT(DISTINCT w_actual.name) FROM w_actual WHERE (w_actual.at_no = a.at_no)) AS process_count,
				                    (SELECT COUNT(DISTINCT w_actual.name) FROM w_actual WHERE ((w_actual.at_no = a.at_no) AND (w_actual.actual > 0))) AS count_pr_w,
				                     b.actual AS actual
			                    FROM
				                    (w_actual_primary a
				                     LEFT JOIN w_actual b ON ((a.at_no = b.at_no)))) as a
	                    WHERE (@At_No = '' OR @At_No IS NULL Or a.at_no like '%'+@At_No+'%') And (@Product = '' OR @Product IS NULL Or a.product like '%'+@Product+'%') 
                                    And (@Style_Name = '' OR @Style_Name IS NULL Or a.style_nm like '%'+@Style_Name+'%') And (@Model_Code = '' OR @Model_Code IS NULL Or a.md_cd like '%'+@Model_Code+'%') 
                                    And (((@RegStart = '' OR @RegStart IS NULL) And (@EndStart = '' OR @EndStart IS NULL)) Or Convert(date,a.reg_dt) between @RegStart and @EndStart)
                                    And  a.finish_yn = 'YT'
Group By a.at_no,a.id_actualpr
						order by a.id_actualpr DESc";
				var resultSearch = await base.DbConnection.QueryAsync<DatawActualPrimaryResponse>(query, new
				{
					@At_No = at_no,
					@Product = product,
					@Style_Name = product_name,
					@Model_Code = model,
					@RegStart = regstart,
					@EndStart = regend
				});

				return resultSearch.ToList();
			}
			catch (Exception e)
			{

				throw e;
			}

			//string sql = @"";
			//var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(sql, new { product = product, productname = product_name, model = model, at_no = atno, regstart = regstart, regend= regend });
			//return result.ToList();
		}
		public async Task<int> ChecklistMatialTIMS(string mt_cd)
		{
			string sql = @"select COUNT(wmtid) from w_material_info_tims where material_code = @mtcd and location_code like '%002%' and status in ('001', '002') and gr_qty> 0";
			var result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { mtcd = mt_cd });
			return result;
		}

		public async Task<IReadOnlyList<MaterialInfoTIMS>> CheckTimsReceiScanMLQR(string mt_cd, int rowpan)
		{
			string sql = @"select wmtid, material_code mt_cd,bb_no,gr_qty,input_dt recevice_dt_tims,from_lct_code from_lct_cd,material_type mt_type,status mt_sts_cd,
@rowpan rowpan,dsCommomlctstscd.dt_nm lct_sts_cd,dsCommommttypenm.dt_nm mt_type_nm,dsCommommtstsnm.dt_nm sts_nm

from w_material_info_tims a
left join (select dt_cd,dt_nm from comm_dt where mt_cd='WHS002') AS dsCommomlctstscd on dsCommomlctstscd.dt_cd=a.bbmp_sts_cd 
left join (select dt_cd,dt_nm from comm_dt where mt_cd='COM004') AS dsCommommttypenm on dsCommomlctstscd.dt_cd=a.material_type 
left join (select dt_cd,dt_nm from comm_dt where mt_cd='WHS005') AS dsCommommtstsnm on dsCommomlctstscd.dt_cd=a.status 
where material_code = @mtcd and location_code like '%002%' and status in ('001', '002') and gr_qty> 0";
			var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(sql, new { mtcd = mt_cd, rowpan = rowpan });
			return result.ToList();
		}
		public async Task<int> CheckRecevingScanM(string wtmid)
		{
			string sql = @"select COUNT(wmtid) from w_material_info_tims where charindex(convert(varchar(50),wmtid), @wmtid)>0";
			var result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { wmtid = wtmid });
			return result;

		}

		public async Task<int> UpdateRecevingFG(int wtmid, string status, string locationcode, string tolctcode, string fromlctcd, string userid)
		{
			string sql = @"Update w_material_info_tims set status=@status,location_code=@location_code,from_lct_code=@fromlctcd,receipt_date=GETDATE(),to_lct_code=@to_lct_code,chg_id=@userid,chg_date=GETDATE() where wmtid= @wmtid";
			var result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { wmtid = wtmid, status = status, fromlctcd= fromlctcd, location_code = locationcode, to_lct_code = tolctcode, userid = userid });
			return result;

		}
		// Task<int> UpdateRecevingScanM(string wtmid, string status, string locationcode, string tolctcode, string userid)
		public async Task<int> UpdateRecevingFGwithinputdt(int wtmid, string status, string locationcode, string tolctcode, string fromlctcd, string userid,string inputdt)
		{
			string sql = @"Update w_material_info_tims set status=@status,location_code=@location_code,input_dt=@inputdt,from_lct_code=@fromlctcd,receipt_date=GETDATE(),to_lct_code=@to_lct_code,chg_id=@userid,chg_date=GETDATE() where wmtid= @wmtid";
			var result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { wmtid = wtmid, status = status, inputdt= inputdt, fromlctcd = fromlctcd, location_code = locationcode, to_lct_code = tolctcode, userid = userid });
			return result;

		}
		public async Task<int> UpdateRecevingScanM(string wtmid, string status, string locationcode, string tolctcode, string userid)
		{
			string sql = @"Update w_material_info_tims set status=@status,location_code=@location_code,receipt_date=GETDATE(),to_lct_code=@to_lct_code,chg_id=@userid,chg_date=GETDATE() where wmtid= @wmtid";
			var result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { wmtid = wtmid, status = status, location_code = locationcode, to_lct_code = tolctcode, userid = userid });
			return result;

		}
		public async Task<int> UpdateRecevingFG(string wtmid, string status, string locationcode, string tolctcode, string userid)
		{
			string sql = @"Update w_material_info_tims set status=@status,location_code=@location_code,to_lct_code=@to_lct_code,chg_id=@userid,chg_date=GETDATE() where charindex(convert(varchar(50),wmtid), @wmtid)>0";
			var result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { wmtid = wtmid, status = status, location_code = locationcode, to_lct_code = tolctcode, userid = userid });
			return result;

		}
		public async Task<IReadOnlyList<DatawActualPrimaryResponse>> GetAllRecevingScanM(string wtmid)
		{

			try
			{
				var query = @"
                        select wmtid, material_code mt_cd,bb_no,gr_qty,input_dt recevice_dt_tims,from_lct_code from_lct_cd,material_type mt_type,status mt_sts_cd,
			@rowpan rowpan,dsCommomlctstscd.dt_nm lct_sts_cd,dsCommommttypenm.dt_nm mt_type_nm,dsCommommtstsnm.dt_nm sts_nm

			from w_material_info_tims a
			left join (select dt_cd,dt_nm from comm_dt where mt_cd='WHS002') AS dsCommomlctstscd on dsCommomlctstscd.dt_cd=a.bbmp_sts_cd 
			left join (select dt_cd,dt_nm from comm_dt where mt_cd='COM004') AS dsCommommttypenm on dsCommomlctstscd.dt_cd=a.material_type 
			left join (select dt_cd,dt_nm from comm_dt where mt_cd='WHS005') AS dsCommommtstsnm on dsCommomlctstscd.dt_cd=a.status 
			where  charindex(convert(varchar(50),wmtid), @wmtid)>0
	                    Group By a.at_no";
				var resultSearch = await base.DbConnection.QueryAsync<DatawActualPrimaryResponse>(query, new
				{
					wmtid = wtmid
				});

				return resultSearch.ToList();
			}
			catch (Exception e)
			{

				throw e;
			}

			//string sql = @"";
			//var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(sql, new { product = product, productname = product_name, model = model, at_no = atno, regstart = regstart, regend= regend });
			//return result.ToList();
		}

		public async Task<IReadOnlyList<DatawActualPrimaryResponse>> GetAllShippingScanMLQR(string buyerqr, string status, string lctcd)
		{

			try
			{
				var query = @"
                       select wmtid, material_code mt_cd,bb_no,gr_qty,buyer_qr,input_dt recevice_dt_tims,from_lct_code from_lct_cd,material_type mt_type,status mt_sts_cd,
			dsCommomlctstscd.dt_nm lct_sts_cd,dsCommommttypenm.dt_nm mt_type_nm,dsCommommtstsnm.dt_nm sts_nm

			from w_material_info_tims a
			left join (select dt_cd,dt_nm from comm_dt where mt_cd='WHS002') AS dsCommomlctstscd on dsCommomlctstscd.dt_cd=a.bbmp_sts_cd 
			left join (select dt_cd,dt_nm from comm_dt where mt_cd='COM004') AS dsCommommttypenm on dsCommomlctstscd.dt_cd=a.material_type 
			left join (select dt_cd,dt_nm from comm_dt where mt_cd='WHS005') AS dsCommommtstsnm on dsCommomlctstscd.dt_cd=a.status 
			where  buyer_qr=@buyerqr and status like '%'+@status+'%' and location_code like '%'+lctcd+'%'
	                    Group By a.at_no";
				var resultSearch = await base.DbConnection.QueryAsync<DatawActualPrimaryResponse>(query, new
				{
					buyerqr = buyerqr,
					status = status,
					lctcd = lctcd
				});

				return resultSearch.ToList();
			}
			catch (Exception e)
			{

				throw e;
			}

		}
		public async Task<int> CheckStatusBuyer(string buyerqr)
		{
			try
			{
				var query = @"Select count(wmtid) from w_material_info_tims where  buyer_qr=@buyerqr";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(query, new { buyerqr = buyerqr });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<IReadOnlyList<ListStatusBuyer>> GetlistBuyer(string buyerqr)
		{
			try
			{
				var query = @"SELECT a.gr_qty,a.real_qty gr_qty_bf
				, (SELECT dt_nm FROM comm_dt WHERE mt_cd = 'WHS005' AND a.status = dt_cd) AS mt_sts_nm,
				(SELECT product FROM w_actual_primary WHERE at_no=b.at_no) product, b.at_no po
				FROM w_material_info_tims AS a
				JOIN w_actual AS b ON a.id_actual =b.id_actual
				WHERE a.buyer_qr=@buyerCode;";
				var result = await base.DbConnection.QueryAsync<ListStatusBuyer>(query, new { buyerCode = buyerqr });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<int> ChecStatusBobbin(string bbno)
		{
			var sql = @"Select COUNT(wmtid) from w_material_info_tims where bb_no = @bbno AND buyer_qr IS NOT NULL and active=1";
			return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { bbno = bbno });

		}
		public async Task<Models.NewVersion.MaterialInfoMMS> FindWMaterialInfoByWmtid(int wmtid)
		{
			string sql = @"select *
                            from w_material_info_tims
                            where wmtid = @wmtid ";
			var result = await base.DbConnection.QueryFirstOrDefaultAsync<Models.NewVersion.MaterialInfoMMS>(sql, new { wmtid = wmtid });
			return result;
		}
		public async Task<int> UpdateUseYnMaterialMappingwithMtCd(string UseYn, string ChangeId, string mtcd)
		{
			string sql = @"UPDATE w_material_mapping_tims SET use_yn=@useyn,chg_dt=GETDATE(), chg_id=@chgid WHERE mt_lot=@mtlot";
			return await base.DbConnection.ExecuteAsync(sql, new { useyn = UseYn, chgid = ChangeId, mtcd = mtcd });
		}

		public async Task<int> UpdateUseYnMaterialIndfowithMtCd(string UseYn, string ChangeId, string mtcd)
		{
			string sql = @"UPDATE w_material_info_tims SET active=@useyn,chg_date=GETDATE(), chg_id=@chgid WHERE material_code=@mtlot";
			return await base.DbConnection.ExecuteAsync(sql, new { useyn = UseYn, chgid = ChangeId, mtlot = mtcd });
		}
		public async Task<int> UpdateBobbinInfowitbbno(string bbno, string userid)
		{
			string sql = "UPDATE d_bobbin_info SET mt_cd=NULL,chg_id=@chgid,chg_dt=GETDATE() WHERE bb_no=@bbno ";
			return await base.DbConnection.ExecuteAsync(sql, new { chgid = userid, bbno = bbno });
		}
		public async Task<IReadOnlyList<StatusBobbin>> GetProGetInfoBobin(string bbno)
		{
            try
            {


			string sql = @"select sum(a.gr_qty) gr_qty,
SUM( a.real_qty ) AS gr_qty_bf,
(CASE
				    WHEN (a.id_actual_oqc IS NOT  NULL or a.id_actual_oqc >0)  THEN 	'OQC'	
				    ELSE (SELECT dt_nm FROM comm_dt WHERE mt_cd ='COM007' and dt_cd=b.name)
				END
				)process,
				a.material_type mt_type,
				a.receipt_date,
				( SELECT top 1 dt_nm FROM comm_dt WHERE mt_cd = 'WHS005' AND a.status = dt_cd ) AS mt_sts_nm,
		( SELECT top 1 lct_nm FROM lct_info WHERE lct_cd = a.location_code ) AS lct_nm,
--( SELECT top 1 product FROM w_actual_primary WHERE at_no = b.at_no ) product,
b.product product,
		a.material_code mt_cd,
		a.at_no po,
		(Case when a.staff_id is null then
		(STUFF((SELECT TOP 1 staff.staff_id
				FROM
					d_pro_unit_staff AS staff
					
				WHERE
					staff.id_actual = a.id_actual 
					FOR XML PATH('')
				),1,1,'' )) else a.staff_id  end) staff_id,

				(CASE WHEN a.staff_id is null then 
		
		(STUFF((SELECT TOP 1  n.uname
				FROM
					d_pro_unit_staff AS staff
					JOIN mb_info AS n ON n.userid = staff.staff_id 
				WHERE
					staff.id_actual = a.id_actual 
					FOR XML PATH('')
				),1,1,'' )) 
				else ( SELECT uname FROM mb_info WHERE userid = a.staff_id )
							END) staff_nm 

from w_material_info_tims a
inner join w_actual b on  b.id_actual = a.id_actual 
WHERE
	a.material_code IN ( SELECT mt_cd FROM d_bobbin_lct_hist AS a WHERE bb_no = @bbno )
group by b.name,a.id_actual_oqc,a.material_type,a.receipt_date,a.status,a.location_code,a.material_code,a.at_no,b.product,a.id_actual,a.staff_id
";
			var result = await base.DbConnection.QueryAsync<StatusBobbin>(sql, new { bbno = bbno });
			return result.ToList();
			}
			catch (Exception e)
			{

				throw e;
			}
		}


		public async Task<IReadOnlyList<StatusBobbin>> GetProGetInfoBobinMMS(string bbno)
		{
			string sql = @"select SUM(a.gr_qty) gr_qty,
SUM(a.real_qty)  AS gr_qty_bf,
(SELECT dt_nm FROM comm_dt WHERE mt_cd ='COM007' and dt_cd=b.name) process,
				a.material_type mt_type,
				a.date_of_receipt recevice_dt_tims,
				( SELECT top 1 dt_nm FROM comm_dt WHERE mt_cd = 'WHS005' AND a.status = dt_cd ) AS mt_sts_nm,
		( SELECT top 1 lct_nm FROM lct_info WHERE lct_cd = a.location_code ) AS lct_nm,
		( SELECT top 1 product FROM w_actual_primary WHERE at_no = b.at_no ) product,
		a.material_code mt_cd,
		b.at_no po,
		(Case when c.staff_id is null then
		(STUFF((SELECT TOP 1 staff.staff_id
				FROM
					d_pro_unit_staff AS staff
					
				WHERE
					staff.id_actual = a.id_actual 
					FOR XML PATH('')
				),1,1,'' )) else c.staff_id  end) staff_id,

				(CASE WHEN c.staff_id is null then 
		
		(STUFF((SELECT TOP 1  n.uname
				FROM
					d_pro_unit_staff AS staff
					JOIN mb_info AS n ON n.userid = staff.staff_id 
				WHERE
					staff.id_actual = a.id_actual 
					FOR XML PATH('')
				),1,1,'' )) 
				else ( SELECT uname FROM mb_info WHERE userid = c.staff_id )
							END) staff_nm 

from w_material_info_mms a
inner join w_actual b on  b.id_actual = a.id_actual
left join d_pro_unit_staff c on c.id_actual=b.id_actual and (a.reg_date between c.start_dt and c.end_dt)
WHERE
	a.material_code IN ( SELECT mt_cd FROM d_bobbin_lct_hist AS a WHERE bb_no = @bbno )
group by b.name,a.id_actual_oqc,a.material_type,a.date_of_receipt,a.status,a.location_code,a.material_code,b.at_no,a.id_actual,c.staff_id
	--select * from w_material_info_mms


		
";
			var result = await base.DbConnection.QueryAsync<StatusBobbin>(sql, new { bbno = bbno });
			return result.ToList();
		}
		public async Task<int> UpdateExtnoMaterialIndfowithWMTID(string extno, string status, string lctcd, string tolctcd, string ChangeId, string wmtid)
		{
			string sql = @"UPDATE w_material_info_tims SET ext_no=@extno,status=@status,location_code=@lctcd,receipt_date=GETDATE(),to_lct_code=@tolctcd,chg_date=GETDATE(), chg_id=@chgid WHERE  charindex(convert(varchar(50),wmtid), @wmtid)>0";
			return await base.DbConnection.ExecuteAsync(sql, new { extno = extno, lctcd = lctcd, tolctcd = tolctcd, chgid = ChangeId, wmtid = wmtid });
		}

		public async Task<int> Updatewextinfo(string alert, string extno)
		{
			string sql = @"update w_ext_info set alert=@alert where ext_no in (@extno)";
			return await base.DbConnection.ExecuteAsync(sql, new { alert = alert, extno = extno });
		}

		public async Task<IReadOnlyList<qc_item_mt>> GetqcitemmtWithQCTYpe(string delyn, string itemtype, string qctype, string itemvcd, string itemnm, string itemexp)
		{
			string sql = @"SELECT * FROM qc_item_mt WHERE  del_yn=@delyn and item_type!=@itemtype and  charindex(item_type,@qctype)>0
						 and  charindex(item_vcd,@itemvcd)>0
						  and  charindex(item_nm,@itemnm)>0
						   and  charindex(item_exp,@itemexp)>0
						order item_vcd DESC";
			var result = await base.DbConnection.QueryAsync<qc_item_mt>(sql, new { delyn = delyn, itemvcd = itemvcd, itemtype = itemtype, qctype = qctype, itemnm = itemnm, itemexp = itemexp });
			return result.ToList();
		}
		public async Task<int> UpdateStatusWMaterialInfoById(string mt_sts_cd, string chg_id, int wmtid)
		{
			string sql = @"UPDATE w_material_info_tims 
SET status =@status,chg_id =@chgid,chg_date = GETDATE() WHERE wmtid =@wmtid ";
			//return db.Database.ExecuteSqlCommand(sql, new MySqlParameter("1", mt_sts_cd),
			//										  new MySqlParameter("2", chg_id),
			//										  new MySqlParameter("3", end_production_dt),
			//										  new MySqlParameter("4", wmtid));
			return await base.DbConnection.ExecuteAsync(sql, new { status = mt_sts_cd, chgid = chg_id, wmtid = wmtid });
		}
		public async Task<MaterialInfoTIMS> GetListDataMaterialInfoTIMSBywmtid(int wmtid)
		{
			try
			{


				var query = @"SELECT * FROM w_material_info_tims Where wmtid = @Id ";
				var rs = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoTIMS>(query, new { @Id = wmtid });
				return rs;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<int> InsertWMaterialInfoTims(int countreturn, int soluong, string userid, string mtcd)
		{
			string sql = @"INSERT INTO w_material_info_tims (
					staff_id,
					staff_id_oqc,
					id_actual,
					material_type,
					material_code,
					mt_no,
					gr_qty,
					rd_no,
					sd_no,
					recevice_dt,
					date,
					return_date,
					alert_NG,
					expiry_dt,
					date_of_receipt,
					expore_dt,
					receipt_date,
					lot_no,
					[status],
					bb_no,
					bbmp_sts_cd,
					location_code,
					lct_sts_cd,
					from_lct_code,
					to_lct_code,
					input_dt,
					orgin_mt_cd,
					remark,
					sts_update,
					use_yn,
					reg_id,
					reg_date,
					chg_id,
					chg_date,
					real_qty 
				) 
				SELECT
				staff_id,
				staff_id_oqc,
				id_actual,
				material_type,
				CONCAT(material_code,'-','RT',CONVERT ( VARCHAR ( 10 ), @countreturn )),
				mt_no,
				@soluong,
				rd_no,
				sd_no,
				recevice_dt,
				date,
				GETDATE (),
				alert_NG,
				expiry_dt,
				date_of_receipt,
				expore_dt,
				receipt_date,
				lot_no,
				'004',
				bb_no,
				bbmp_sts_cd,
				location_code,
				lct_sts_cd,
				from_lct_code,
				to_lct_code,
				input_dt,
				material_code,
				remark,
				'return',
				use_yn,
				@userid,
				GETDATE (),
				@userid,
				GETDATE (),
				@soluong 
				FROM w_material_info_tims WHERE mt_cd = @mtcd";
			return await base.DbConnection.ExecuteAsync(sql, new { countreturn = countreturn, soluong = soluong, userid = userid, mtcd = mtcd });
		}

		public async Task<int> UpdateMaterialmappingwithmtcd(string mt_cd, string userid, bool active, string useyn)
		{
			string sql = @"UPDATE w_material_mapping_tims SET use_yn=@useyn,active=@active,chg_dt=GETDATE(),chg_id=@userid where mt_cd=@mtcd;";
			return await base.DbConnection.ExecuteAsync(sql, new { mtcd = mt_cd, userid = userid, active = active, useyn = useyn });
		}

		public async Task<IReadOnlyList<QCItemCheck_Mt_Model>> GetQCItemCheckMaterialwithitemvcd(string item_vcd)
		{
			try
			{
				var query = @"Select check_subject as qc_itemcheck_mt__check_subject,
					check_id as qc_itemcheck_mt__check_id,
					icno as qc_itemcheck_mt__icno
					From qc_itemcheck_mt Where item_vcd = @Item And del_yn = 'N'";
				var result = await base.DbConnection.QueryAsync<QCItemCheck_Mt_Model>(query, new { @Item = item_vcd });
				return result.ToList();
			}
			catch (Exception e)
			{

				throw e;
			}
		}

		public async Task<IReadOnlyList<ViewQCModel>> GetQCItemCheckMaterialDetailwithcheckcd(string item_vcd, string check_id)
		{
			try
			{
				var query = @"Select check_name as qc_itemcheck_dt__check_name,
icdno as qc_itemcheck_dt__icdno
From qc_itemcheck_dt where item_vcd = @Item And check_cd = @Code And del_yn = 'N' And defect_yn = 'Y'";
				var result = await base.DbConnection.QueryAsync<ViewQCModel>(query, new { @Item = item_vcd, @Code = check_id });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<qc_itemcheck_dt> Getqcitemcheckdt(int icdno)
		{
			string sql = @"select * from qc_itemcheck_dt where icdno=@icdno";
			return await base.DbConnection.QueryFirstOrDefaultAsync<qc_itemcheck_dt>(sql, new { icdno = icdno });
		}


		public async Task<IReadOnlyList<WMaterialInfoQRCode>> PrintQRCodeMaterial(List<string> listId)
		{
			try
			{
				var result = new List<WMaterialInfoQRCode>();
				foreach (var item in listId)
				{
					var query = @"SELECT a.wmtid AS Id, a.material_code AS WMaterialCode, a.mt_no AS WMaterialNumber, b.mt_nm AS WMaterialName, a.material_type AS WMaterialType, a.alert_NG AS WAlertNG, a.status AS WMaterialStatusCode,
									 a.gr_qty AS WMaterialGrQty, a.reg_date AS CreatedDate, b.spec AS DMaterialSpec, b.spec_unit AS DMaterialSpecUnit,ISNULL(b.bundle_unit,'EA') AS DMaterialBundleUnit,
									 d.product AS Product, c.name AS Process, (a.staff_id) AS WorkerId, (SELECT mb_info.uname FROM mb_info WHERE mb_info.userid = a.staff_id) AS WorkerName,
									(SELECT CASE WHEN b.bundle_unit = 'Roll' THEN CASE WHEN (a.gr_qty IS NULL OR b.spec IS NULL) THEN 0 ELSE a.gr_qty / b.spec END ELSE ISNULL(a.gr_qty,0) END) AS Qty
							FROM (((w_material_info_tims a
							LEFT JOIN d_material_info b ON(b.mt_no = a.mt_no))
							LEFT JOIN w_actual c ON(a.id_actual = c.id_actual))
							LEFT JOIN w_actual_primary d ON(c.at_no = d.at_no))
							WHERE a.status IN ('003','012') AND a.wmtid IN (@Id)  AND a.location_code LIKE '006%'";
					var data = await base.DbConnection.QueryAsync<WMaterialInfoQRCode>(query, new { @Id = item });
					result = data.ToList();
				}
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<IReadOnlyList<DProUnitStaffAPI>> FindDProUnitStaffByIdActual(int id_actual, string staff_id, string datee)
		{
			string sql = @"SELECT
                            psid,staff_id,actual,defect,id_actual,staff_tp,start_dt,end_dt,
                            use_yn,del_yn,reg_id,reg_dt,chg_id,chg_dt
                            FROM d_pro_unit_staff
                            Where id_actual = @idactual AND staff_id= @staffid and (convert(datetime,@datee,120) between convert(datetime,start_dt,121) and convert(datetime,end_dt,121))
order by reg_dt DESC";
			var result = await base.DbConnection.QueryAsync<DProUnitStaffAPI>(sql, new { idactual = id_actual, staffid = staff_id, datee = datee });
			return result.ToList();
		}
		public async Task<IReadOnlyList<FaclineQc>> GetFaclineQCWithfqno(string fqno)
		{
			string sql = @"SELECT b.fqno fqno,b.fq_no,
			CONCAT(SUBSTRING(b.work_dt,1,4),'-',SUBSTRING(b.work_dt,5,2),'-',SUBSTRING(b.work_dt,7,2),' ',SUBSTRING(b.work_dt,9,2),':',SUBSTRING(b.work_dt,11,2) ) work_dt,
						b.check_qty,(b.ok_qty),(b.check_qty)-(b.ok_qty) as defect_qty
			from m_facline_qc as b 
			where b.fqno =@fqno
			order by fq_no desc ,check_qty";
			var result = await base.DbConnection.QueryAsync<FaclineQc>(sql, new { fqno = fqno });
			return result.ToList();


		}
		public async Task<IReadOnlyList<m_facline_qc>> GetFaclineQCWithfqnos(int fqno)
		{
			string sql = @"SELECT *
			from m_facline_qc as b 
			where b.fqno =@fqno
			order by fq_no desc ,check_qty";
			var result = await base.DbConnection.QueryAsync<m_facline_qc>(sql, new { fqno = fqno });
			return result.ToList();
		}
		public async Task<int> GetQTYmfaclineqcvalue(string fqno)
		{
			string sql = @"select SUM(check_qty) from m_facline_qc_value where fq_no=@fqno";
			return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { fqno = fqno });
		}
		public async Task<int> GetDefetcActual(int idacutal, string status)
		{


			string sql = @" SELECT SUM(gr_qty) FROM w_material_info_tims WHERE id_actual = @idacutal AND status = @status";
			return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { idacutal = idacutal, status = status });
		}

		public async Task<MaterialInfoTIMS> Getwmtinfotimswithwmtid(int wmtid, string status, string locationcode, string MaterialCode)
		{


			string sql = @" SELECT * FROM w_material_info_tims WHERE wmtid = @wmtid AND status = @status and location_code like '%'+@lctcd+'%' and material_code = @MaterialCode";
			return await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoTIMS>(sql,
				new { wmtid = wmtid, status = status, lctcd = locationcode , MaterialCode  = MaterialCode });
		}
		public async Task<IReadOnlyList<MaterialInfoTIMS>> CheckwmaterialinfoMappingbuyerwmtid(int wmtid,string MaterialCode, string lct_cd)
		{
			string sql = @"Select * from w_material_info_tims where wmtid=@wmtid and status in('008','009','002','003','010') 
						and location_code like '%'+@lctcd+'%'   and material_code = @materialcode ";
			var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(sql, new { wmtid = wmtid, lctcd = lct_cd , materialcode = MaterialCode });
			return result.ToList();
			//return db.Database.SqlQuery<MaterialInfoMMS>(sql, new MySqlParameter("@1", mt_cd), new MySqlParameter("@2", mt_sts_cd), new MySqlParameter("@3", lct_cd));
		}
		public async Task<IReadOnlyList<MaterialInfoTIMS>> GetWmtInfoTimsWithbbno(string bb_no)
		{
			string sql = @"SELECT * FROM w_material_info_tims WHERE bb_no=@bbno and active=1 order by reg_date DESC";
			var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(sql, new { bbno = bb_no });
			return result.ToList();
		}
		public async Task<int> SPUpdateDestroy(int id, string status, string mt_cd, string bb_no,string userid)
		{
			
			string sqldelete = @"DELETE FROM d_bobbin_lct_hist WHERE bb_no = @bb_no  and mt_cd = @mt_cd ;
									update d_bobbin_info set mt_cd= null  where bb_no =@bb_no  and mt_cd = @mt_cd;";
			 await base.DbConnection.ExecuteAsync(sqldelete,new { wmtid =id, bb_no , mt_cd });


			string sql = @"UPDATE w_material_info_tims
			SET	chg_date=GETDATE(),status='011',chg_id=@regid,sts_update=@stsupdate
			WHERE wmtid = @wmtid and material_code = @mt_cd;";
			return await base.DbConnection.ExecuteAsync(sql, new { wmtid = id, stsupdate = status, regid = userid, mt_cd });
		}

		public async Task<IReadOnlyList<viewtimsinventorygeneral>> Getviewtimsinventorygeneral(int wmtid)
		{
			string sql = @"SELECT 
        a.id_actual AS id_actual,
        d.product AS product_cd,
        (SELECT 
                d_style_info.style_nm
            FROM
                d_style_info
            WHERE
                (d.product = d_style_info.style_no)) AS product_nm,
        (SELECT 
                d_style_info.md_cd
            FROM
                d_style_info
            WHERE
                (d.product = d_style_info.style_no)) AS md_cd,
        c.at_no AS at_no,
        a.wmtid AS Id,
        a.material_code AS WMaterialCode,
        a.mt_no AS WMaterialNo,
        a.material_type AS WMaterialType,
        a.status AS WMaterialStatusCode,
        a.gr_qty AS WMaterialGrQty,
        a.bb_no AS VBobbinCd,
        CONCAT(ISNULL(a.gr_qty, 0),
                ISNULL(b.unit_cd, '')) AS WMaterialLength,
        a.buyer_qr AS buyer_qr,
        (SELECT 
                comm_dt.dt_nm
            FROM
                comm_dt
            WHERE
                comm_dt.dt_cd = a.status
                    AND comm_dt.mt_cd = 'WHS005') AS WMaterialStatusName,
        CONCAT(ISNULL(b.width, 0),
                '*',
                ISNULL(a.gr_qty, 0)) AS WMaterialSize,
        (SELECT 
                (CASE
                        WHEN
                            (b.bundle_unit = 'Roll')
                        THEN
                            (CASE
                                WHEN
                                    ((a.gr_qty IS NULL)
                                        OR (b.spec IS NULL))
                                THEN
                                    0
                                ELSE (a.gr_qty / b.spec)
                            END)
                        ELSE ISNULL(a.gr_qty, 0)
                    END)
            ) AS WMaterialQty,
        ISNULL(b.bundle_unit, 'EA') AS WMaterialUnit,
        ISNULL(CONVERT(DATETIME,a.receipt_date,120),
                a.end_production_dt) AS WMaterialReceivedDate
    FROM w_material_info_tims a
        LEFT JOIN d_material_info b ON b.mt_no = a.mt_no
        JOIN w_actual c ON a.id_actual = c.id_actual
        JOIN w_actual_primary d ON c.at_no = d.at_no
    WHERE (a.location_code LIKE '006%') AND a.status NOT IN ('000' , '003', '005') AND a.wmtid=@wmtid";
			var result = await base.DbConnection.QueryAsync<viewtimsinventorygeneral>(sql,new { wmtid = wmtid });
			return result.ToList();
		}
		public async Task<int> SPUpdatesRedo(int id,string sts_update, string userid)
		{
			string sql = @"UPDATE w_material_info_tims
					SET
					chg_date=GETDATE(),
					status=@sts_update,
					sts_update = 'composite',
					chg_id=@regid
					WHERE wmtid = @wmtid;";
			return await base.DbConnection.ExecuteAsync(sql,new { wmtid=id, sts_update= sts_update, regid= userid });
		}
		public async Task<lct_info> Getlctinfo(string lct_cd)
		{
			string sql = @"Select * from lct_info where lct_cd=@lctcd";
			return await base.DbConnection.QueryFirstOrDefaultAsync<lct_info>(sql, new { lctcd = lct_cd });
		}
		public async Task<IReadOnlyList<lct_info>> GetListlctinfo(string lct_cd)
		{
			string sql = @"Select * from lct_info where lct_cd Like '%'+@lctcd+'%' order by lct_cd,level_cd";
			var result= await base.DbConnection.QueryAsync<lct_info>(sql, new { lctcd = lct_cd });
			return result.ToList();
		}
		public async Task<IReadOnlyList<lct_info>> GetListSearchlctinfo(string lct_cd, string lct_cds,string index_cd)
		{
			string sql = @"Select * from lct_info where index_cd like 'G%'  
				and (@lctcd='' or @lctcd is null or lct_cd Like '%'+@lctcd+'%') 
				and (@lctcds='' or @lctcds is null or lct_cd Like '%'+@lctcds+'%' )
				and (@indexcd='' or @indexcd is null or index_cd Like '%'+@indexcd+'%' )
				order by lct_cd,level_cd";
			var result = await base.DbConnection.QueryAsync<lct_info>(sql, new { lctcd = lct_cd, lctcds=lct_cds, indexcd =index_cd});
			return result.ToList();
		}
		public async Task<lct_info> GetListlctinfowithlctno(int lctno)
		{
			string sql = @"Select * from lct_info where lctno =@lctno order by lct_cd,level_cd";
			return await base.DbConnection.QueryFirstOrDefaultAsync<lct_info>(sql, new { lctno = lctno });
			//return result.ToList();
		}
		public async Task<int> InsertLctInfo(lct_info item)
        {
			string sql = @"INSERT INTO [dbo].[lct_info]
           ([lct_cd]
           ,[lct_nm]
           ,[up_lct_cd]
           ,[level_cd]
           ,[index_cd]
           ,[shelf_cd]
           ,[order_no]
           ,[real_use_yn]
           ,[re_mark]
           ,[use_yn]
           ,[lct_rfid]
           ,[lct_bar_cd]
           ,[sf_yn]
           ,[is_yn]
           ,[mt_yn]
           ,[mv_yn]
           ,[ti_yn]
           ,[fg_yn]
           ,[rt_yn]
           ,[ft_yn]
           ,[wp_yn]
           ,[nt_yn]
           ,[pk_yn]
           ,[manager_id]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt]
           ,[mn_full]
           ,[sap_lct_cd]
           ,[userid]
           ,[selected]
           ,[active])
     VALUES
           (@lct_cd
           ,@lct_nm
           ,@up_lct_cd
           ,@level_cd
           ,@index_cd
           ,@shelf_cd
           ,@order_no
           ,@real_use_yn
           ,@re_mark
           ,@use_yn
           ,@lct_rfid
           ,@lct_bar_cd
           ,@sf_yn
           ,@is_yn
           ,@mt_yn
           ,@mv_yn
           ,@ti_yn
           ,@fg_yn
           ,@rt_yn
           ,@ft_yn
           ,@wp_yn
           ,@nt_yn
           ,@pk_yn
           ,@manager_id
           ,@reg_id
           ,GETDATE()
           ,@chg_id
           ,GETDATE()
           ,@mn_full
           ,@sap_lct_cd
           ,@userid
           ,@selected)";
			return await base.DbConnection.ExecuteAsync(sql, item);
        }
		public async Task<int> UpdateLctInfo(lct_info item)
		{
			string sql = @"UPDATE [dbo].[lct_info]
			   SET [lct_cd] = @lct_cd
				  ,[lct_nm] = @lct_nm
				  ,[up_lct_cd] = @up_lct_cd
				  ,[level_cd] = @level_cd
				  ,[index_cd] = @index_cd
				  ,[shelf_cd] = @shelf_cd
				  ,[order_no] = @order_no
				  ,[real_use_yn] = @real_use_yn
				  ,[re_mark] = @re_mark
				  ,[use_yn] = @use_yn
				  ,[lct_rfid] = @lct_rfid
				  ,[lct_bar_cd] = @lct_bar_cd
				  ,[sf_yn] = @sf_yn
				  ,[is_yn] = @is_yn
				  ,[mt_yn] = @mt_yn
				  ,[mv_yn] = @mv_yn
				  ,[ti_yn] = @ti_yn
				  ,[fg_yn] = @fg_yn
				  ,[rt_yn] = @rt_yn
				  ,[ft_yn] = @ft_yn
				  ,[wp_yn] = @wp_yn
				  ,[nt_yn] = @nt_yn
				  ,[pk_yn] = @pk_yn
				  ,[manager_id] = @manager_id
				  ,[chg_id] = @chg_id
				  ,[chg_dt] =GETDATE()
				  ,[mn_full] = @mn_full
				  ,[sap_lct_cd] = @sap_lct_cd
				  ,[userid] = @userid
				  ,[selected] = @selected
			 WHERE lctno =@lctno";
			return await base.DbConnection.ExecuteAsync(sql, new
			{
				lct_cd = item.lct_cd,
				lct_nm = item.lct_nm,
				up_lct_cd = item.up_lct_cd,
				level_cd = item.level_cd,
				index_cd = item.index_cd,
				shelf_cd = item.shelf_cd,
				order_no = item.order_no,
				real_use_yn = item.real_use_yn,
				re_mark = item.re_mark,
				use_yn = item.use_yn,
				lct_rfid = item.lct_rfid,
				lct_bar_cd = item.lct_bar_cd,
				sf_yn = item.sf_yn,
				is_yn = item.is_yn,
				mt_yn = item.mt_yn,
				mv_yn = item.mv_yn,
				ti_yn = item.ti_yn,
				fg_yn = item.fg_yn,
				rt_yn = item.rt_yn,
				ft_yn = item.ft_yn,
				wp_yn = item.wp_yn,
				nt_yn = item.nt_yn,
				pk_yn = item.pk_yn,
				manager_id = item.manager_id,
				chg_id = item.chg_id,
				mn_full = item.mn_full,
				sap_lct_cd = item.sap_lct_cd,
				userid = item.userid,
				selected = item.selected,
				lctno = item.lctno
			});
		}
		//public async Task<IReadOnlyList<lct_info>> GetListlctinfo(string lct_cd)
		//{
		//	string sql = @"Select * from lct_info where lct_cd Like '%'+@lctcd+'%' order by lct_cd,level_cd";
		//	var result= await base.DbConnection.QueryAsync<lct_info>(sql, new { lctcd = lct_cd });
		//	return result.ToList();
		//}
		public async Task<int> inertmfaclineqcvalue(m_facline_qc_value item)
		{
			string sql = @"INSERT INTO [dbo].[m_facline_qc_value]
           ([product]
           ,[shift]
           ,[item_vcd]
           ,[check_id]
           ,[check_cd]
           ,[check_value]
           ,[check_qty]
           ,[date_ymd]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
     VALUES
           (@product
           ,@shift
           ,@item_vcd
           ,@check_id
           ,@check_cd
           ,@check_value
           ,@check_qty
           ,GETDATE()
           ,@reg_id
           ,GETDATE()
           ,@chg_id
           ,GETDATE());
Select Scope_Identity()";
			return await base.DbConnection.ExecuteScalarAsync<int>(sql, item);
		}
		public async Task<IReadOnlyList<w_material_model>> Getwmaterialmodel(string wmtid)
		{
			string sql = @"select a.wmtid,a.material_code mt_cd,b.mt_nm,
				CONCAT(isnull(a.gr_qty,''),isnull(b.unit_cd,'')) lenght, 
				isnull(a.gr_qty,'') lenght1,
				CONCAT(ISNULL(b.width,0),'*',ISNULL(a.gr_qty,0)) AS size,
				ISNULL(b.spec,0) spec,a.mt_no,
				CONCAT((case when b.bundle_unit ='Roll' then  (a.gr_qty/b.spec) ELSE a.gr_qty END),' ',isnull(b.bundle_unit,'')) qty,
				b.bundle_unit, a.return_date,
				(SELECT dt_nm FROM comm_dt WHERE comm_dt.dt_cd=a.status AND comm_dt.mt_cd='WHS005') sts_nm ,
				(SELECT TOP 1 w_actual_primary.product  FROM   w_actual JOIN w_actual_primary ON w_actual.at_no=w_actual_primary.at_no WHERE  a.id_actual = w_actual.id_actual) product, 
				(SELECT TOP 1 name FROM w_actual WHERE a.id_actual=w_actual.id_actual ) AS name 
				from w_material_info_tims a
				LEFT JOIN d_material_info  b ON a.mt_no=b.mt_no
				WHERE charindex(convert(varchar(50),wmtid), @wmtid)>0 ";
			var result = await base.DbConnection.QueryAsync<w_material_model>(sql, new { wmtid = wmtid });
			return result.ToList();
		}
		public async Task<IReadOnlyList<w_material_model>> GetwmaterialmodelPrintQR(string mtno, string returndate, string recevicedtstart, string recevicedtend)
		{
			string sql = @"select a.wmtid,a.material_code mt_cd,b.mt_nm,
				CONCAT(isnull(a.gr_qty,''),isnull(b.unit_cd,'')) lenght, 
				isnull(a.gr_qty,'') lenght1,
				CONCAT(ISNULL(b.width,0),'*',ISNULL(a.gr_qty,0)) AS size,
				ISNULL(b.spec,0) spec,a.mt_no,
				CONCAT((case when b.bundle_unit ='Roll' then  (a.gr_qty/b.spec) ELSE a.gr_qty END),' ',isnull(b.bundle_unit,'')) qty,
				b.bundle_unit, a.return_date,
				(SELECT dt_nm FROM comm_dt WHERE comm_dt.dt_cd=a.status AND comm_dt.mt_cd='WHS005') sts_nm ,
				(SELECT TOP 1 w_actual_primary.product  FROM   w_actual JOIN w_actual_primary ON w_actual.at_no=w_actual_primary.at_no WHERE  a.id_actual = w_actual.id_actual) product, 
				(SELECT TOP 1 name FROM w_actual WHERE a.id_actual=w_actual.id_actual ) AS name 
				from w_material_info_tims a
				LEFT JOIN d_material_info  b ON a.mt_no=b.mt_no
				WHERE (@mtno ='' OR @mtno is null or a.mt_no like '%'+@mtno+'%') 
				and (@returndate ='' OR @returndate is null or convert(date,a.return_date,110) =convert(date,@returndate,110))
				and (@recevicedtstart ='' OR @recevicedtstart is null or or convert(date,a.recevice_dt,110) >=convert(date,@recevicedtstart,110))
				and (@recevicedtend ='' OR @recevicedtend is null or or convert(date,a.recevice_dt,110) <=convert(date,@recevicedtend,110))";
			var result = await base.DbConnection.QueryAsync<w_material_model>(sql, new { mtno = mtno, returndate = returndate, recevicedtstart = recevicedtstart, recevicedtend = recevicedtend });
			return result.ToList();
		}

		public async Task<IReadOnlyList<w_material_model>> GetwmaterialmodelPrintQRDetail(string mtno)
		{
			string sql = @"select a.wmtid,a.material_code mt_cd,b.mt_nm,
				CONCAT(isnull(a.gr_qty,''),isnull(b.unit_cd,'')) lenght, 
				isnull(a.gr_qty,'') lenght1,
				CONCAT(ISNULL(b.width,0),'*',ISNULL(a.gr_qty,0)) AS size,
				ISNULL(b.spec,0) spec,a.mt_no,
				CONCAT((case when b.bundle_unit ='Roll' then  (a.gr_qty/b.spec) ELSE a.gr_qty END),' ',isnull(b.bundle_unit,'')) qty,
				b.bundle_unit, a.return_date,
				(SELECT dt_nm FROM comm_dt WHERE comm_dt.dt_cd=a.status AND comm_dt.mt_cd='WHS005') sts_nm ,
				(SELECT TOP 1 w_actual_primary.product  FROM   w_actual JOIN w_actual_primary ON w_actual.at_no=w_actual_primary.at_no WHERE  a.id_actual = w_actual.id_actual) product, 
				(SELECT TOP 1 name FROM w_actual WHERE a.id_actual=w_actual.id_actual ) AS name 
				from w_material_info_tims a
				LEFT JOIN d_material_info  b ON a.mt_no=b.mt_no
				WHERE a.status='004' and a.location_code LIKE '006%' AND a.sts_update='Return' (@mtno ='' OR @mtno is null or a.mt_no like '%'+@mtno+'%') 
				";
			var result = await base.DbConnection.QueryAsync<w_material_model>(sql, new { mtno = mtno });
			return result.ToList();
		}

		public async Task<IReadOnlyList<w_material_model>> GetwmaterialmodelqrPrintQr(string mtno)
		{
			string sql = @"select a.wmtid,a.material_code mt_cd,b.mt_nm,
				CONCAT(isnull(a.gr_qty,''),isnull(b.unit_cd,'')) lenght, 
				isnull(a.gr_qty,'') lenght1,
				CONCAT(ISNULL(b.width,0),'*',ISNULL(a.gr_qty,0)) AS size,
				ISNULL(b.spec,0) spec,a.mt_no,
				CONCAT((case when b.bundle_unit ='Roll' then  (a.gr_qty/b.spec) ELSE a.gr_qty END),' ',isnull(b.bundle_unit,'')) qty,
				b.bundle_unit, a.return_date,
				(SELECT dt_nm FROM comm_dt WHERE comm_dt.dt_cd=a.status AND comm_dt.mt_cd='WHS005') sts_nm ,
				(SELECT TOP 1 w_actual_primary.product  FROM   w_actual JOIN w_actual_primary ON w_actual.at_no=w_actual_primary.at_no WHERE  a.id_actual = w_actual.id_actual) product, 
				(SELECT TOP 1 name FROM w_actual WHERE a.id_actual=w_actual.id_actual ) AS name 
				from w_material_info_tims a
				LEFT JOIN d_material_info  b ON a.mt_no=b.mt_no
				WHERE a.status='004'  AND a.sts_update='Return' (@mtno ='' OR @mtno is null or a.mt_no like '%'+@mtno+'%') 
				";
			var result = await base.DbConnection.QueryAsync<w_material_model>(sql, new { mtno = mtno });
			return result.ToList();
		}

		public async Task<IReadOnlyList<WMaterialnfo>> FindDetailActualStaffOQC(string staff_id_oqc, int id_actual, string end_dt, string start_dt)
		{
            try
            {


			string sql = @"SELECT 
					wmtid  as wmtid   ,
					reg_date as date    ,
					material_code  as mt_cd   ,
					mt_no  as mt_no   ,
					real_qty  as real_qty ,
					gr_qty  as gr_qty     ,
					gr_qty  as gr_qty1    ,
					material_code as mt_qrcode ,
					bb_no  as bb_no    ,
					material_code as mt_barcode,
					chg_date   as chg_dt ,
					--reg_date reg_dt,
					Convert(datetime, end_production_dt, 120 ) as reg_dt,
					(
							CASE
							WHEN 8<= DATEPART(HOUR,w.end_production_dt) AND DATEPART(HOUR,w.end_production_dt)<=20 
							THEN  'Ca Ngày'
							WHEN 20 < DATEPART(HOUR,w.end_production_dt) AND DATEPART(HOUR,w.end_production_dt)<= 24
							THEN 'Ca Đêm'
							WHEN 0 <= DATEPART(HOUR,w.end_production_dt) AND DATEPART(HOUR,w.end_production_dt)< 8
							THEN 'Ca Đêm'
							ELSE ''  END
						) as ShiftName
				from w_material_info_tims w
				WHERE status != '003' 
				AND  Convert(datetime, w.end_production_dt, 120 )   >=  Convert(datetime, @startdt, 120 ) 
				AND Convert(datetime, w.end_production_dt, 120 ) <=  Convert(datetime, @endproductionDt, 120 ) 
				and staff_id_oqc = @staffidoqc AND id_actual_oqc =@idactualoqc  
					Order by reg_dt desc";


			var result = await base.DbConnection.QueryAsync<WMaterialnfo>(sql, new { staffidoqc = staff_id_oqc, idactualoqc = id_actual, startdt = start_dt, endproductionDt = end_dt });
			return result.ToList();
			}
			catch (Exception ex)
			{

				throw;
			}
		}
		public async Task<IReadOnlyList<WMaterialnfo>> GetTIMSActualDetailByStaff(string staff_id, int id_actual, string start_dt, string end_dt)
		{
			string sql1 = @"SELECT 
					wmtid  as wmtid   ,
					reg_date as date    ,
					material_code  as mt_cd   ,
					mt_no  as mt_no   ,
					real_qty  as real_qty ,
					gr_qty  as gr_qty     ,
					gr_qty  as gr_qty1    ,
					material_code as mt_qrcode ,
					bb_no  as bb_no    ,
					material_code as mt_barcode,
					chg_date   as chg_dt ,
					--reg_date reg_dt,
					Convert(datetime, reg_date, 120 ) as reg_dt,
					(
							CASE
							WHEN 8<= DATEPART(HOUR,w.reg_date) AND DATEPART(HOUR,w.reg_date)<=20 
							THEN  'Ca Ngày'
							WHEN 20 < DATEPART(HOUR,w.reg_date) AND DATEPART(HOUR,w.reg_date)<= 24
							THEN 'Ca Đêm'
							WHEN 0 <= DATEPART(HOUR,w.reg_date) AND DATEPART(HOUR,w.reg_date)< 8
							THEN 'Ca Đêm'
							ELSE ''  END
						) as ShiftName
				from w_material_info_tims w
                            WHERE status != '003'
                            and staff_id = @staffid AND id_actual =@idactual 
                            AND (reg_date between Convert(datetime, @startdt, 120 ) and Convert(datetime, @enddt, 120 ) ) and 
                    material_code NOT IN (SELECT material_code FROM w_material_info_tims WHERE id_actual=@idactual AND orgin_mt_cd IS NOT null and status!='003'  AND material_code LIKE CONCAT(orgin_mt_cd,'-MG%')  
UNION SELECT orgin_mt_cd FROM w_material_info_tims WHERE orgin_mt_cd IS NOT NULL
                    AND id_actual=@idactual AND material_code LIKE CONCAT(orgin_mt_cd,'-DV%') AND status!='003' AND staff_id=@staffid)
                    ";
			//return db.Database.SqlQuery<WMaterialnfo>(sql1,
			//	new MySqlParameter("1", staff_id),
			//	new MySqlParameter("2", id_actual),
			//	new MySqlParameter("3", start_dt),
			//	new MySqlParameter("4", end_dt),
			//	new MySqlParameter("5", id_actual),
			//	new MySqlParameter("6", id_actual),
			//new MySqlParameter("7", staff_id)
			//);
			var result = await base.DbConnection.QueryAsync<WMaterialnfo>(sql1, new { staffid = staff_id, idactual = id_actual, startdt = start_dt, enddt = end_dt });
			return result.ToList();
		}
		public async Task<IReadOnlyList<TIMSInventoryModel>> GetspTIMSInventoryGeneralUnion(string mtCode, string mtNo, string sVBobbinCd, string recDate, string prd_cd)
		{
			string sql = @"select '' AS Id,
		'' AS MaterialCode, 
		tmpa.WMaterialNo AS MaterialNo, 
		'' AS MaterialName,
		SUM(tmpa.WMaterialLength) AS Length,
		'' AS Size,
		SUM(tmpa.WMaterialQty) AS Qty,
		tmpa.WMaterialUnit AS Unit,
		tmpa.WMaterialStatusCode AS StatusCode,
		'' AS StatusCode, '' AS StatusName, 
		'' AS ReceivedDate,
		'' VBobbinCd,
		tmpa.product_cd
				from(
				SELECT 
				a.id_actual AS id_actual,
				d.product AS product_cd,
				(SELECT 
						d_style_info.style_nm
					FROM
						d_style_info
					WHERE
						(d.product = d_style_info.style_no)) AS product_nm,
				(SELECT 
						d_style_info.md_cd
					FROM
						d_style_info
					WHERE
						(d.product = d_style_info.style_no)) AS md_cd,
				c.at_no AS at_no,
				a.wmtid AS Id,
				a.material_code AS WMaterialCode,
				a.mt_no AS WMaterialNo,
				a.material_type AS WMaterialType,
				a.mt_sts_cd AS WMaterialStatusCode,
				a.gr_qty AS WMaterialGrQty,
				a.bb_no AS VBobbinCd,
				CONCAT(ISNULL(a.gr_qty, 0),
						ISNULL(b.unit_cd, 0)) AS WMaterialLength,
				a.buyer_qr AS buyer_qr,
				(SELECT 
						comm_dt.dt_nm
					FROM
						comm_dt
					WHERE
						comm_dt.dt_cd = a.mt_sts_cd
							AND comm_dt.mt_cd = 'WHS005') AS WMaterialStatusName,
				CONCAT(ISNULL(b.width, 0),
						'*',
						ISNULL(a.gr_qty, 0)) AS WMaterialSize,
				(SELECT 
						(CASE
								WHEN
									(b.bundle_unit = 'Roll')
								THEN
									(CASE
										WHEN
											((a.gr_qty IS NULL)
												OR (b.spec IS NULL))
										THEN
											0
										ELSE (a.gr_qty / b.spec)
									END)
								ELSE ISNULL(a.gr_qty, 0)
							END)
					) AS WMaterialQty,
				ISNULL(b.bundle_unit, 'EA') AS WMaterialUnit,
				ISNULL(CONVERT(DATETIME,a.receipt_date,120),
						a.end_production_dt) AS WMaterialReceivedDate
			FROM w_material_info_tims a
				LEFT JOIN d_material_info b ON b.mt_no = a.mt_no
				JOIN w_actual c ON a.id_actual = c.id_actual
				JOIN w_actual_primary d ON c.at_no = d.at_no
			WHERE (a.location_code LIKE '006%') AND a.mt_sts_cd NOT IN ('000' , '003', '005') 
			AND (@mtno='' or @mtno is null or a.mt_no=@mtno)
			AND (@bbno='' or @mtcd is null or a.bb_no=@bbno)
			AND (@procd='' or @procd is null or d.product=@procd)

			) tmpa
			where  (  @recDate = '' OR @recDate IS NULL ORDATEDIFF(day, CONVERT(DATETIME,tmpa.WMaterialReceivedDate,101), CONVERT(DATE,@recDate,101))=0)
			
			union
			select tmpb.Id,
			tmpb.WMaterialCode AS MaterialCode,
			tmpb.WMaterialNo AS MaterialNo,
			tmpb.VBobbinCd AS MaterialName, 
			tmpb.WMaterialLength AS Length, 
			tmpb.WMaterialSize AS Size,
			tmpb.WMaterialQty AS Qty, 
			tmpb.WMaterialUnit AS Unit, 
			tmpb.WMaterialStatusCode AS StatusCode,
			tmpb.WMaterialStatusName AS StatusName,
			tmpb.WMaterialReceivedDate AS ReceivedDate,
			tmpb.VBobbinCd, 
			tmpb.product_cd 
			from (
			SELECT 
				a.id_actual AS id_actual,
				d.product AS product_cd,
				(SELECT 
						d_style_info.style_nm
					FROM
						d_style_info
					WHERE
						(d.product = d_style_info.style_no)) AS product_nm,
				(SELECT 
						d_style_info.md_cd
					FROM
						d_style_info
					WHERE
						(d.product = d_style_info.style_no)) AS md_cd,
				c.at_no AS at_no,
				a.wmtid AS Id,
				a.material_code AS WMaterialCode,
				a.mt_no AS WMaterialNo,
				a.material_type AS WMaterialType,
				a.mt_sts_cd AS WMaterialStatusCode,
				a.gr_qty AS WMaterialGrQty,
				a.bb_no AS VBobbinCd,
				CONCAT(ISNULL(a.gr_qty, 0),
						ISNULL(b.unit_cd, 0)) AS WMaterialLength,
				a.buyer_qr AS buyer_qr,
				(SELECT 
						comm_dt.dt_nm
					FROM
						comm_dt
					WHERE
						comm_dt.dt_cd = a.mt_sts_cd
							AND comm_dt.mt_cd = 'WHS005') AS WMaterialStatusName,
				CONCAT(ISNULL(b.width, 0),
						'*',
						ISNULL(a.gr_qty, 0)) AS WMaterialSize,
				(SELECT 
						(CASE
								WHEN
									(b.bundle_unit = 'Roll')
								THEN
									(CASE
										WHEN
											((a.gr_qty IS NULL)
												OR (b.spec IS NULL))
										THEN
											0
										ELSE (a.gr_qty / b.spec)
									END)
								ELSE ISNULL(a.gr_qty, 0)
							END)
					) AS WMaterialQty,
				ISNULL(b.bundle_unit, 'EA') AS WMaterialUnit,
				ISNULL(CONVERT(DATETIME,a.receipt_date,120),
						a.end_production_dt) AS WMaterialReceivedDate
			FROM w_material_info_tims a
				LEFT JOIN d_material_info b ON b.mt_no = a.mt_no
				JOIN w_actual c ON a.id_actual = c.id_actual
				JOIN w_actual_primary d ON c.at_no = d.at_no
			WHERE (a.location_code LIKE '006%') AND a.mt_sts_cd NOT IN ('000' , '003', '005')
			AND (@mtno='' or @mtno is null or a.mt_no=@mtno)
			AND (@mtcd='' or @mtcd is null or a.material_code=@mtcd)
			AND (@bbno='' or @mtcd is null or a.bb_no=@bbno)
			AND (@procd='' or @procd is null or d.product=@procd)
			) tmpb
			where  (  @recDate = '' OR @recDate IS NULL ORDATEDIFF(day, CONVERT(DATETIME,tmpb.WMaterialReceivedDate,101), CONVERT(DATE,@recDate,101))=0)";
			var result = await base.DbConnection.QueryAsync<TIMSInventoryModel>(sql,new { mtno = mtNo, mtcd = mtCode  , bbno=sVBobbinCd, procd = prd_cd, recDate= recDate });//string recDate, string prd_cd
			return result.ToList();
		}
		public async Task<IReadOnlyList<TIMSInventoryExcel>> GetspTIMSInventoryGeneralExcel(string status, string po, string model, string prd_cd, string bom_type, string mt_cd, string VBobbinCd, string endDate, string startDate)
		{
			try
			{
				string sql = @"with table1 as (
select product product_cd,c.style_nm product_nm, c.md_cd,c.bom_type ,sum(HCK) AS 'HCK',sum(DKT) AS 'DKT',sum(HDG) AS 'HDG',sum(MAPPINGBUYER) as 'MAPPINGBUYER',sum(CKT) AS 'CKT',Sum(SORTING) AS 'SORTING'
from(

select cc.product,cc.at_no,(CASE WHEN a.status='008' THEN a.gr_qty  ELSE 0  END )AS 'HCK', 
                            (CASE WHEN a.status='002' AND a.id_actual !=( 
                            SELECT top 1  v.id_actual id_actual FROM w_actual AS v 
                            WHERE v.at_no=bb.at_no AND v.type='TIMS' AND v.name!='OQC' order by v.id_actual DESC
                            ) 
                            THEN (a.gr_qty)  ELSE 0  END) AS 'DKT', 

                            (CASE WHEN a.status='010' and (a.buyer_qr IS NULL or a.buyer_qr = '') THEN (a.gr_qty)  ELSE 0  END) AS 'HDG', 
							 (CASE WHEN a.status='010' and (a.buyer_qr IS NOT NULL or a.buyer_qr != '') THEN (a.gr_qty)  ELSE 0  END) AS 'MAPPINGBUYER', 
                            (CASE WHEN (a.status='002' or a.status='009')AND a.id_actual =( 
                            SELECT top 1  v.id_actual id_actual FROM w_actual AS v 
                            WHERE v.at_no=bb.at_no AND v.type='TIMS' AND v.name!='OQC' order by v.id_actual DESC
                            ) 
                            THEN (a.gr_qty)  ELSE 0  END) AS 'CKT' ,

                            (CASE WHEN a.status='015'  THEN (a.gr_qty)  ELSE 0  END )AS 'SORTING' 
							 
 
from w_material_info_tims  a
inner join w_actual bb on a.id_actual=bb.id_actual
inner join w_actual_primary cc on bb.at_no=cc.at_no
WHERE   a.location_code  = '006000000000000000' 
                           AND      a.status  in ('002','015','009','010','008')
AND (@status ='' OR @status IS NULL OR CHARINDEX(a.status,@status)  >0) 
								AND (@mtCode ='' OR @mtCode IS NULL OR a.location_code LIKE '%' + @mtCode + '%') 
           				AND (@sVBobbinCd ='' OR @sVBobbinCd IS NULL OR a.bb_no LIKE '%'+ @sVBobbinCd +'%')
           				AND (@po ='' OR @po IS NULL OR cc.at_no LIKE '%'+ @po +'%') 
           				AND (@prd_cd ='' OR @prd_cd IS NULL OR cc.product LIKE '%'+ @prd_cd +'%') 
           				AND (@startDate ='' OR @startDate IS NULL OR a.end_production_dt >=  @startDate ) 
           				AND (@endDate ='' OR @endDate IS NULL OR a.end_production_dt <=  @endDate )   
union all
select cc.product,cc.at_no,(CASE WHEN a.status='008' THEN (a.gr_qty)  ELSE 0  END )AS 'HCK', 
                            (CASE WHEN a.status='009' 
                            THEN (a.gr_qty)  ELSE 0  END) AS 'DKT', 
                            (CASE WHEN a.status='010' THEN (a.gr_qty)  ELSE 0  END) AS 'HDG', 
							'' as 'MAPPINGBUYER',
                            (CASE WHEN a.status='002' AND a.id_actual =( 
                            SELECT top 1  v.id_actual id_actual FROM w_actual AS v 
                            WHERE v.at_no=bb.at_no AND v.type='TIMS' AND v.name!='OQC' order by v.id_actual DESC
                            ) 
                            THEN (a.gr_qty)  ELSE 0  END) AS 'CKT' ,
                            (CASE WHEN a.status='015'  THEN (a.gr_qty)  ELSE 0  END )AS 'SORTING' 
							from w_material_info_mms  a
inner join w_actual bb on a.id_actual=bb.id_actual
inner join w_actual_primary cc on bb.at_no=cc.at_no
WHERE  a.location_code = '006000000000000000'
 AND     a.status in ('008','002')  
AND (@status ='' OR @status IS NULL OR  CHARINDEX(a.status,@status)  >0 )
AND (@mtCode ='' OR @mtCode IS NULL OR a.location_code LIKE '%' + @mtCode + '%') 
           				AND (@sVBobbinCd ='' OR @sVBobbinCd IS NULL OR a.bb_no LIKE '%'+ @sVBobbinCd +'%')
           				AND (@po ='' OR @po IS NULL OR cc.at_no LIKE '%'+ @po +'%') 
           				AND (@prd_cd ='' OR @prd_cd IS NULL OR cc.product LIKE '%'+ @prd_cd +'%') 
           				AND (@startDate ='' OR @startDate IS NULL OR a.date_of_receipt >=  @startDate ) 
           				AND (@endDate ='' OR @endDate IS NULL OR a.date_of_receipt <=  @endDate )                           
) cccc 
JOIN d_style_info AS c ON cccc.product=c.style_no 
--where c.md_cd ='AMB136ZH06'
--and c.bom_type ='RollType'
where   (@bom_type ='' OR @bom_type is null or  c.bom_type like '%' + @bom_type+'%' )
   and  (@model ='' OR @model is null or  c.md_cd like '%' + @model+'%' )

group by product,c.style_nm,c.md_cd,c.bom_type
--order by c.md_cd ASC,c.bom_type ASC
)
--select * from table1
,
table2 as(
select b.product_cd, b.Id,b.VBobbinCd,b.at_no,b.buyer_qr, b.WMaterialCode AS MaterialCode, b.WMaterialLength AS 'Length', b.WMaterialStatusName AS StatusName, b.WMaterialReceivedDate AS ReceivedDate,b.WMaterialStatusCode,b.md_cd
							FROM 
							(
								SELECT 
									a.id_actual AS id_actual,
									d.product AS product_cd,
									(SELECT d_style_info.style_nm FROM d_style_info WHERE(d.product = d_style_info.style_no)) AS product_nm,
									(SELECT d_style_info.md_cd FROM d_style_info WHERE(d.product = d_style_info.style_no)) AS md_cd,
									c.at_no AS at_no,
									a.wmtid AS Id,
									a.material_code AS WMaterialCode,
									a.mt_no AS WMaterialNo,
									a.material_type AS WMaterialType,
									a.status AS WMaterialStatusCode,
									a.gr_qty AS WMaterialGrQty,
									a.bb_no AS VBobbinCd,
									CONCAT(ISNULL(a.gr_qty, 0),ISNULL(b.bundle_unit, ' EA')) AS WMaterialLength,
									a.buyer_qr AS buyer_qr,
									(SELECT comm_dt.dt_nm FROM comm_dt WHERE ((comm_dt.dt_cd = a.status) AND (comm_dt.mt_cd = 'WHS005'))) AS WMaterialStatusName,
									CONCAT(ISNULL(b.width, 0),'*',ISNULL(a.gr_qty, 0)) AS WMaterialSize,
								--	(SELECT (CASE WHEN (b.bundle_unit = 'Roll') THEN
								--			(CASE WHEN((a.gr_qty IS NULL) OR (b.spec IS NULL)) THEN 0 ELSE (a.gr_qty / b.spec) END) ELSE ISNULL(a.gr_qty, 0) END)
								--		) AS WMaterialQty,
								--	ISNULL(b.bundle_unit, 'EA') AS WMaterialUnit,
									(case when  a.end_production_dt='' or a.end_production_dt is null then convert(datetime,a.reg_date,120) else  convert(datetime,a.end_production_dt,120) end) AS WMaterialReceivedDate
							
								FROM
									w_material_info_tims As a
									LEFT JOIN d_material_info b ON b.mt_no = a.mt_no
									JOIN w_actual c ON a.id_actual = c.id_actual
									JOIN w_actual_primary d ON c.at_no = d.at_no
								WHERE a.location_code = '006000000000000000' and a.gr_qty > 0 and a.status  in ('002','015','009','010','008') 
									AND (@mtCode ='' OR @mtCode IS NULL OR a.location_code LIKE '%' + @mtCode + '%') 
									AND (@startDate ='' OR @startDate IS NULL OR a.end_production_dt >=  @startDate ) 
            						AND (@endDate =''  OR @endDate IS NULL OR a.end_production_dt <=  @endDate ) 
							) AS b
								left join w_actual_primary c on c.at_no =b.at_no
							--	left join w_actual d on d.id_actual != b.id_actual and d.type ='TIMS' and d.name !='OQC' 
								WHERE (@prd_cd ='' OR @prd_cd IS NULL OR b.product_cd Like '%'+ @prd_cd + '%')
							--	AND 
							--	b.id_actual not in  (SELECT MAX(a.id_actual) FROM w_actual AS a WHERE a.type = 'TIMS'  AND a.name != 'OQC')
							--	AND b.at_no in  (SELECT a.at_no FROM w_actual_primary As a  
							--	WHERE  (@prd_cd ='' OR @prd_cd IS NULL OR a.product LIKE '%'+ @prd_cd +'%') 
							--	GROUP BY a.at_no)  
								--and b.at_no ='PO20211018-011'
								--and b.VBobbinCd ='AUTO-BOB-211021225223008'
           						AND (@sVBobbinCd ='' OR @sVBobbinCd IS NULL OR b.VBobbinCd LIKE '%'+ @sVBobbinCd +'%')
           						AND (@po ='' OR @po IS NULL OR b.at_no LIKE '%'+ @po +'%') 
           						AND (@prd_cd ='' OR @prd_cd IS NULL OR b.product_cd LIKE '%'+ @prd_cd +'%') 
								AND (@status ='' OR @status IS NULL OR b.WMaterialStatusCode LIKE '%'+ @status +'%')
								AND (@model ='' OR @model IS NULL OR b.md_cd LIKE '%'+ @model +'%')
								and b.WMaterialStatusCode NOT IN(005,011)
							--Order by b.at_no
UNION ALL
SELECT b.product_cd, b.Id,b.VBobbinCd,b.at_no,b.buyer_qr, b.WMaterialCode AS MaterialCode, b.WMaterialLength AS 'Length', b.WMaterialStatusName AS StatusName, b.WMaterialReceivedDate AS ReceivedDate,b.WMaterialStatusCode,b.md_cd
							FROM 
							(
								SELECT 
									a.id_actual AS id_actual,
									d.product AS product_cd,
									(SELECT d_style_info.style_nm FROM d_style_info WHERE(d.product = d_style_info.style_no)) AS product_nm,
									(SELECT d_style_info.md_cd FROM d_style_info WHERE(d.product = d_style_info.style_no)) AS md_cd,
									c.at_no AS at_no,
									a.wmtid AS Id,
									a.material_code AS WMaterialCode,
									a.mt_no AS WMaterialNo,
									a.material_type AS WMaterialType,
									a.status AS WMaterialStatusCode,
									a.gr_qty AS WMaterialGrQty,
									a.bb_no AS VBobbinCd,
									CONCAT(ISNULL(a.gr_qty, 0),ISNULL(b.bundle_unit, ' EA')) AS WMaterialLength,
									'' AS buyer_qr,
									(SELECT comm_dt.dt_nm FROM comm_dt WHERE ((comm_dt.dt_cd = a.status) AND (comm_dt.mt_cd = 'WHS005'))) AS WMaterialStatusName,
									CONCAT(ISNULL(b.width, 0),'*',ISNULL(a.gr_qty, 0)) AS WMaterialSize,
								--	 (SELECT (CASE WHEN (b.bundle_unit = 'Roll') THEN
								--			(CASE WHEN((a.gr_qty IS NULL) OR (b.spec IS NULL)) THEN 0 ELSE (a.gr_qty / b.spec) END) ELSE ISNULL(a.gr_qty, 0) END)
								--		) AS WMaterialQty,
								--	ISNULL(b.bundle_unit, 'EA') AS WMaterialUnit,
									(case when  a.date_of_receipt='' or a.date_of_receipt is null then convert(datetime,a.chg_date,120) else convert(datetime,a.date_of_receipt,120) end) AS WMaterialReceivedDate
							
								FROM
									w_material_info_mms As a
									LEFT JOIN d_material_info b ON b.mt_no = a.mt_no
									JOIN w_actual c ON a.id_actual = c.id_actual
									JOIN w_actual_primary d ON c.at_no = d.at_no
								WHERE a.location_code = '006000000000000000'  AND a.gr_qty > 0
									AND (@mtCode ='' OR @mtCode IS NULL OR a.location_code LIKE '%' + @mtCode + '%') 
									AND (@startDate ='' OR @startDate IS NULL OR a.date_of_receipt >=  @startDate ) 
           						    AND (@endDate ='' OR @endDate IS NULL OR a.date_of_receipt <=  @endDate ) 
							) AS b
						--	left join w_actual_primary c on c.at_no =b.at_no
							left join w_actual d on d.id_actual != b.id_actual and d.type ='TIMS' and d.name !='OQC' 
							--where  b.at_no ='PO20211018-011'
							--and b.VBobbinCd ='AUTO-BOB-211021225223008'
								WHERE 
								(@prd_cd ='' OR @prd_cd IS NULL OR b.product_cd Like '%'+ @prd_cd + '%')
							--	AND b.id_actual not in  (SELECT MAX(a.id_actual) FROM w_actual AS a WHERE a.type = 'TIMS'  AND a.name != 'OQC')
								AND b.at_no in  (SELECT a.at_no FROM w_actual_primary As a  WHERE (@prd_cd ='' OR @prd_cd IS NULL OR b.product_cd LIKE '%'+ @prd_cd +'%')  GROUP BY a.at_no)  
								
           						AND (@sVBobbinCd ='' OR @sVBobbinCd IS NULL OR b.VBobbinCd LIKE '%'+ @sVBobbinCd +'%')
           						AND (@po ='' OR @po IS NULL OR b.at_no LIKE '%'+ @po +'%') 
           						AND (@prd_cd ='' OR @prd_cd IS NULL OR b.product_cd LIKE '%'+ @prd_cd +'%') 
           						
						    	AND (@status ='' OR @status IS NULL OR b.WMaterialStatusCode LIKE '%'+ @status +'%')
								and b.WMaterialStatusCode NOT IN(005,011) 
								AND (@model ='' OR @model IS NULL OR b.md_cd LIKE '%'+ @model +'%')
							--Order by b.at_no
						
) 
select * from
(select a.md_cd,a.product_cd, a.product_nm,a.bom_type,a.HCK,a.DKT,a.HDG,a.MAPPINGBUYER,a.CKT,a.SORTING,''Id,''VBobbinCd
,''at_no,''buyer_qr,''MaterialCode,''Length,''StatusName,''ReceivedDate
from table1 a
union
select a.md_cd,a.product_cd,''product_nm,''bom_type,''HCK,''DKT,''HDG,''MAPPINGBUYER,''CKT,''SORTING,a.Id,a.VBobbinCd
,a.at_no,a.buyer_qr,a.MaterialCode,a.Length,a.StatusName,CONCAT(FORMAT( CAST( a.ReceivedDate AS DATETIME ),'yyyy-MM-dd'),'')  from table2 a ) a
order by a.product_cd,a.product_nm desc,a.at_no,a.VBobbinCd";
				var result = await base.DbConnection.QueryAsync<TIMSInventoryExcel>(sql, new { @status = status, @po = po, @model = model, @prd_cd = prd_cd, @bom_type = bom_type, @mtCode = mt_cd, @sVBobbinCd = VBobbinCd, @endDate = endDate, @startDate = startDate });//string recDate, string prd_cd
				return result.ToList();
			}
			catch(Exception e)
            {
				throw e;
            }
		}
		public async Task<WMaterialInfoStamp> ViewStatusTemGoi(string buyerCode)
		{
			string QuerySQL = @" SELECT a.wmtid,a.material_code mt_cd,a.chg_date chg_dt,a.chg_id, a.buyer_qr, a.gr_qty,a.status mt_sts_cd, a.location_code lct_cd,a.product,
                (SELECT dt_nm FROM comm_dt WHERE mt_cd = 'WHS005' and a.status = dt_cd) AS NameStatus,
                (SELECT lct_nm FROM lct_info WHERE a.location_code = lct_cd) AS locationName
                FROM w_material_info_tims AS a
				WHERE a.buyer_qr =@buyerqr  ";
			//return db.Database.SqlQuery<WMaterialInfoStamp>(QuerySQL, new MySqlParameter("1", buyerCode)).FirstOrDefault();
			var result = await base.DbConnection.QueryFirstOrDefaultAsync<WMaterialInfoStamp>(QuerySQL,new { buyerqr= buyerCode });
			return result;
		}
		public async Task<w_box_mapping> CheckTemGoimappingBox(string buyerCode)
		{
			string QuerySQL = "SELECT * FROM w_box_mapping WHERE buyer_cd = @buyer  ";
			//return db.Database.SqlQuery<w_box_mapping>(QuerySQL, new MySqlParameter("1", buyerCode)).FirstOrDefault();
			return await base.DbConnection.QueryFirstOrDefaultAsync<w_box_mapping>(QuerySQL,new { buyer = buyerCode });
		}
		public async Task<IReadOnlyList<w_ext_info>> GetListwextinfo(string extno,string extnm)
        {
			string sql = @" SELECT a.* FROM w_ext_info WHERE use_yn ='Y' AND ( @extno '=' OR @extno IS NULL OR  ext_no like '%' + @extno + ''%' ) 
			AND ( @extnm '=' OR @extnm IS NULL OR  ext_nm like '%' + @extnm + ''%' )
order by extid desc";
			var result = await base.DbConnection.QueryAsync<w_ext_info>(sql,new { extno= extno, extnm= extnm });
			return result.ToList();

        }
		public async Task<IReadOnlyList<WMaterialnfo>> GetListMLNO(string buyer_qr,string mt_cd,string mt_no)
        {
			string sql = @"SELECT
		a.wmtid,a.material_code mt_cd,a.mt_no, a.lot_no, a.gr_qty,a.buyer_qr ,a.bb_no, a.receipt_date recevice_dt_tims,a.from_lct_code from_lct_cd
		
		, (SELECT top 1 lct_nm FROM lct_info WHERE lct_cd = a.location_code) AS from_lct_nm 
		, (SELECT top 1 dt_nm FROM comm_dt WHERE mt_cd = 'WHS002' and dt_cd = a.lct_sts_cd) AS lct_sts_cd 
		
		, (SELECT top 1 dt_nm FROM comm_dt WHERE  mt_cd = 'WHS005' and  dt_cd = a.mt_sts_cd) AS sts_nm,a.mt_sts_cd

		,(SELECT top 1 dt_nm FROM comm_dt WHERE dt_cd = a.material_type and mt_cd = 'COM004') AS mt_type_nm, a.material_type mt_type 
		
		,a.reg_id,CONVERT(DATETIME,a.reg_date,120) reg_dt,a.chg_id,CONVERT(DATETIME,a.chg_date,120) chg_dt
		
	FROM w_material_info_tims a
	WHERE ( a.buyer_qr is not null and a.buyer_qr <> '')
	AND a.status ='010'
	AND a.location_code  LIKE '006%'
	AND a.gr_qty > 0
	AND ( a.ext_no IS NULL 
       OR a.ext_no = ' ')
	AND a.buyer_qr LIKE'%'+@buyerqr+'%'
	AND a.material_code LIKE '%'+@mtcd+'%'
	AND a.mt_no LIKE '%'+@mtno+'%'
order by a.wmtid DESC";
			var result = await base.DbConnection.QueryAsync<WMaterialnfo>(sql,new { buyerqr=buyer_qr, mtcd =mt_cd, mtno =mt_no});
			return result.ToList();
        }

		public async Task<IReadOnlyList<WMaterialnfo>> GetListShippingScanBuyerQRFG(string buyer_qr)
		{
			string sql = @"SELECT
		a.wmtid,a.material_code mt_cd,a.material_type mt_type,a.expiry_dt,a.mt_no, a.lot_no,
		a.gr_qty,a.buyer_qr ,a.bb_no, a.receipt_date recevice_dt_tims,a.from_lct_code from_lct_cd,
		a.expore_dt,a.status sd_sts_cd,a.status mt_sts_cd
		, (SELECT top 1 lct_nm FROM lct_info WHERE lct_cd = a.location_code) AS from_lct_nm 
		, (SELECT top 1 dt_nm FROM comm_dt WHERE mt_cd = 'WHS002' and dt_cd = a.lct_sts_cd) AS lct_sts_cd 
		
		, (SELECT top 1 dt_nm FROM comm_dt WHERE  mt_cd = 'WHS005' and  dt_cd = a.mt_sts_cd) AS sts_nm,a.mt_sts_cd

		,(SELECT top 1 dt_nm FROM comm_dt WHERE dt_cd = a.material_type and mt_cd = 'COM004') AS mt_type_nm, a.material_type mt_type 
		
		,a.reg_id,CONVERT(DATETIME,a.reg_date,120) reg_dt,a.chg_id,CONVERT(DATETIME,a.chg_date,120) chg_dt
		
	FROM w_material_info_tims a
	WHERE a.status ='001'
	AND a.location_code  LIKE '003%'
	AND a.buyer_qr LIKE'%'+@buyerqr+'%'

order by a.wmtid DESC";
			var result = await base.DbConnection.QueryAsync<WMaterialnfo>(sql, new { buyerqr = buyer_qr });
			return result.ToList();
		}
		public async Task<IReadOnlyList<d_style_info>> GetListdstyleinfo(string productCode,string productName,string modelCode)
        {
			string sql = @"SELECT  m.dt_nm as qc_range_nm, s.sid, s.style_no, s.style_nm, s.md_cd, s.prj_nm, s.ssver, s.standard, s.cust_rev, s.order_num, s.pack_amt, s.cav, s.bom_type, s.tds_no, s.item_vcd, s.qc_range_cd
	FROM  d_style_info as s
	LEFT JOIN comm_dt as m on s.qc_range_cd = m.dt_cd and m.mt_cd = 'COM017'
	WHERE s.use_yn = 'Y'
	AND s.style_no LIKE '%'+@productCode+'%'
	AND s.style_nm LIKE '%'+@productName+'%'
	AND s.md_cd LIKE '%'+@modelCode+'%'
	ORDER BY s.sid DESC";
			var result = await base.DbConnection.QueryAsync<d_style_info>(sql,new { productCode = productCode , productName = productName , modelCode = modelCode });
			return result.ToList();
        }
		public async Task<string> GetUname(string userid)
		{
			string sql = @"SELECT uname FROM mb_info WHERE userid=@userid";
			return await base.DbConnection.QueryFirstOrDefaultAsync<string>(sql, new { userid = userid });
		}
		public async Task<IReadOnlyList<WMaterialMapping>> FindAllWMaterialMappingByMtcdUseYn(string mt_cd, string use_yn)
		{
			string getvalue = @"select 
                                        wmmid  ,
                                        mt_lot ,
                                        mt_cd  ,
                                        mt_no  ,
                                        mapping_dt,
                                        bb_no     ,
                                        remark   , 
                                        sts_share ,
                                        use_yn    ,
                                        del_yn    ,
                                        reg_id   , 
                                         reg_dt,
                                         chg_id,
                                         chg_dt
                                    from w_material_mapping_tims where mt_cd = @mtcd AND use_yn =@useyn
                                    ";
			var result=  await base.DbConnection.QueryAsync<WMaterialMapping>(getvalue, new { mtcd = mt_cd , useyn = use_yn });
			return result.ToList();
		}
		public async Task<IReadOnlyList<MaterialInfoTIMS>> Getwmtinfotimswithlctcd(string locationcode)
		{


			string sql = @" SELECT * FROM w_material_info_tims WHERE  location_code like '%'+@lctcd+'%'";
			var result= await base.DbConnection.QueryAsync<MaterialInfoTIMS>(sql, new { lctcd = locationcode });
			return result.ToList();

		}
		public async Task<MaterialInfoTIMS> Getwmtinfotimswithlctcdandbuyer(string locationcode ,string buyerqr)
		{


			string sql = @" SELECT * FROM w_material_info_tims WHERE  location_code like '%'+@lctcd+'%' and buyer_qr=@buyerqr";
			return await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoTIMS>(sql, new { lctcd = locationcode , buyerqr= buyerqr });
			//return result.ToList();
		}
		public async Task<IReadOnlyList<WMaterialnfo>> GetListMLNOwithextno(string ext_no)
		{
			string sql = @"SELECT
		a.wmtid,a.material_code mt_cd,a.mt_no, a.lot_no, a.gr_qty,a.buyer_qr ,a.bb_no, a.receipt_date recevice_dt_tims,a.from_lct_code from_lct_cd
		
		, (SELECT top 1 lct_nm FROM lct_info WHERE lct_cd = a.location_code) AS from_lct_nm 
		, (SELECT top 1 dt_nm FROM comm_dt WHERE mt_cd = 'WHS002' and dt_cd = a.lct_sts_cd) AS lct_sts_cd 
		
		, (SELECT top 1 dt_nm FROM comm_dt WHERE  mt_cd = 'WHS005' and  dt_cd = a.mt_sts_cd) AS sts_nm,a.mt_sts_cd

		,(SELECT top 1 dt_nm FROM comm_dt WHERE dt_cd = a.material_type and mt_cd = 'COM004') AS mt_type_nm, a.material_type mt_type 
		
		,a.reg_id,CONVERT(DATETIME,a.reg_date,120) reg_dt,a.chg_id,CONVERT(DATETIME,a.chg_date,120) chg_dt
		
	FROM w_material_info_tims a
	WHERE ( a.buyer_qr is not null and a.buyer_qr <> '')
	AND a.mt_sts_cd ='010'
	AND a.location_code  LIKE '006%'
	AND a.gr_qty > 0
	AND  a.ext_no LIKE'%'+@extno+'%'
      
order by a.wmtid DESC";
			var result = await base.DbConnection.QueryAsync<WMaterialnfo>(sql, new { extno = ext_no });
			return result.ToList();
		}
		public async Task<IReadOnlyList<StampDetail>> FindStampwithboxno(string boxno)
		{
			try
			{
				var query = @"Select * From stamp_detail where box_code = @boxno";
				var result = await base.DbConnection.QueryAsync<StampDetail>(query, new { boxno = boxno });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<IReadOnlyList<w_box_mapping>> CheckTemGoimappingBoxwithboxno(string boxno)
		{
			string QuerySQL = "SELECT * FROM w_box_mapping WHERE bx_no = @bxno and status='001'  ";
			//return db.Database.SqlQuery<w_box_mapping>(QuerySQL, new MySqlParameter("1", buyerCode)).FirstOrDefault();
			var result= await base.DbConnection.QueryAsync<w_box_mapping>(QuerySQL, new { bxno = boxno });
			return result.ToList();
		}
		public async Task<w_box_mapping> CheckTemGoimappingBoxwithbmno(int bmno)
		{
			string QuerySQL = "SELECT * FROM w_box_mapping WHERE bmno=@bmno  ";
			//return db.Database.SqlQuery<w_box_mapping>(QuerySQL, new MySqlParameter("1", buyerCode)).FirstOrDefault();
			return await base.DbConnection.QueryFirstOrDefaultAsync<w_box_mapping>(QuerySQL, new { bmno = bmno });
		}
		public async Task<IReadOnlyList<MaterialInfoTIMS>> GetMaterialInfoTimsByBuyerQRFg(string buyer_qr)
		{
			try
			{
				var query = @"Select * From w_material_info_tims Where buyer_code = @BuyerCode and (location_code like'003%' or location_code like'004%')";
				var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(query, new { @BuyerCode = buyer_qr });
				return result.ToList() ;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<IReadOnlyList<WMaterialnfo>> GetListShippingScanBuyerQRFGS(string buyer_qr)
		{
			string sql = @"SELECT
		a.wmtid,a.material_code mt_cd,a.material_type mt_type,a.expiry_dt,a.mt_no, a.lot_no,
		a.gr_qty,a.buyer_qr ,a.bb_no, a.receipt_date recevice_dt_tims,a.from_lct_code from_lct_cd,
		a.expore_dt,a.status sd_sts_cd,a.status mt_sts_cd
		, (SELECT top 1 lct_nm FROM lct_info WHERE lct_cd = a.location_code) AS from_lct_nm 
		, (SELECT top 1 dt_nm FROM comm_dt WHERE mt_cd = 'WHS002' and dt_cd = a.lct_sts_cd) AS lct_sts_cd 
		
		, (SELECT top 1 dt_nm FROM comm_dt WHERE  mt_cd = 'WHS005' and  dt_cd = a.mt_sts_cd) AS sts_nm,a.mt_sts_cd

		,(SELECT top 1 dt_nm FROM comm_dt WHERE dt_cd = a.material_type and mt_cd = 'COM004') AS mt_type_nm, a.material_type mt_type 
		
		,a.reg_id,CONVERT(DATETIME,a.reg_date,120) reg_dt,a.chg_id,CONVERT(DATETIME,a.chg_date,120) chg_dt
		
	FROM w_material_info_tims a
	WHERE (a.location_code  LIKE '003%' OR a.location_code  LIKE '004%')
	AND a.buyer_qr LIKE'%'+@buyerqr+'%'

order by a.wmtid DESC";
			var result = await base.DbConnection.QueryAsync<WMaterialnfo>(sql, new { buyerqr = buyer_qr });
			return result.ToList();
		}

		public async Task<int> Updatechangestsfgtims(int wtmid, string status, string locationcode, string userid)
		{
			string sql = @"Update w_material_info_tims set status=@status,location_code=@location_code,,chg_id=@userid,chg_date=GETDATE() where wmtid= @wmtid";
			var result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { wmtid = wtmid, status = status, location_code = locationcode, userid = userid });
			return result;

		}

		public async Task<Models.NewVersion.MaterialInfoTIMS> GetMaterialInfoTIMS(string mt_cd)
		{
			string sql = @"SELECT Top 1 * FROM w_material_info_tims WHERE material_code = @Mt_Cd AND active=1 Order by reg_date Desc";
			return await base.DbConnection.QueryFirstOrDefaultAsync<Models.NewVersion.MaterialInfoTIMS>(sql, new { @Mt_Cd = mt_cd });
		}


		public async Task<IEnumerable<MaterialMappingTIMS>> GetListMaterialMappingTIMS(string mt_cd)
		{
			try
			{
				var query = @"Select * From w_material_mapping_tims where mt_cd = @Mt_Cd Order by reg_dt Desc";
				var result = await base.DbConnection.QueryAsync<MaterialMappingTIMS>(query, new { @Mt_Cd = mt_cd });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}


		public async Task<IEnumerable<MaterialInfoTIMS>> GetListDataMaterialInfoTIMSStatus(string mt_cd)
		{
			try
			{
				var query = @"Select * from w_material_info_tims where material_code = @Mt_Cd";
				var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(query, new { @Mt_Cd = mt_cd });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<ATMTIMS> SearchATMTims(string product_id, string name_id, string at_id, string reg_dt, string reg_dt_end, string bom_type)
		{
			try
			{
				string sql = @"SELECT [waiting]
							  ,[model]
							  ,[unit_cd]
							  ,a.prj_nm
							  ,[waitold]
							  ,[at_no]
							  ,[product]
							  ,[product_nm]
							  ,convert(varchar(23),a.reg_dt,121) reg_dt
							  ,[m_lieu]
							  ,convert(decimal(10,2),[need_m])need_m
							  ,[actual]
							  ,[id_actual]
							  ,[HS]
							  ,[mt_no]
							  ,[mt_nm]
							  ,[actual_lt]
							  ,[NG]
							  ,[target]
							  , b.bom_type as bom_type
						FROM [atm_tims] a
							join d_style_info b on b.style_no = a.product
						WHERE (@productid='' or @productid is null or product like '%'+@productid+'%')
						and (@nameid='' or @nameid is null or product_nm like '%'+@nameid+'%')
						and (@atid='' or @atid is null or at_no like '%'+@atid+'%')
						and (@bom_type='' or @bom_type is null or b.bom_type like '%'+@bom_type+'%')
						and (@regdt='' or @regdt is null or CONVERT(DATETIME, a.reg_dt,121) >=CONVERT(DATETIME, @regdt,121))
						and (@regdtend='' or @regdtend is null or CONVERT(DATETIME, a.reg_dt,121) <= CONVERT(DATETIME, @regdtend,121))
						order by reg_dt DESC

	";
				var result = DbConnection.Query<ATMTIMS>(sql, new { productid = product_id, nameid = name_id, atid = at_id, regdt = reg_dt, regdtend = reg_dt_end, bom_type = bom_type});
				return result.ToList();
			}
			catch(Exception e)
            {
				throw e;
            }
        }
		public List<ATMTIMS> SearchATMTims1(string product_id, string name_id, string at_id, string reg_dt, string reg_dt_end,string bom_type,int intpage,int introw)
		{
			try
			{
				string sql = @"SELECT [waiting]
							  ,[model]
							  ,[unit_cd]
							  ,a.prj_nm
							  ,[waitold]
							  ,[at_no]
							  ,[product]
							  ,[product_nm]
							  ,convert(varchar(23),a.reg_dt,121) reg_dt
							  ,[m_lieu]
							  ,convert(decimal(10,2),[need_m])need_m
							  ,[actual]
							  ,[id_actual]
							  ,[HS]
							  ,[mt_no]
							  ,[mt_nm]
							  ,[actual_lt]
							  ,[NG]
							  ,[target]
							  , b.bom_type as bom_type
						FROM [atm_tims] a
							join d_style_info b on b.style_no = a.product
						WHERE (@productid='' or @productid is null or product like '%'+@productid+'%')
						and (@nameid='' or @nameid is null or product_nm like '%'+@nameid+'%')
						and (@atid='' or @atid is null or at_no like '%'+@atid+'%')
						and (@bom_type='' or @bom_type is null or b.bom_type like '%'+@bom_type+'%')
						and (@regdt='' or @regdt is null or CONVERT(DATETIME, a.reg_dt,121) >=CONVERT(DATETIME, @regdt,121))
						and (@regdtend='' or @regdtend is null or CONVERT(DATETIME, a.reg_dt,121) <= CONVERT(DATETIME, @regdtend,121))
						order by reg_dt DESC

						OFFSET @intpage ROWS FETCH NEXT @introw ROWS ONLY

	";
				var result = DbConnection.Query<ATMTIMS>(sql, new { productid = product_id, nameid = name_id, atid = at_id, regdt = reg_dt, regdtend = reg_dt_end, bom_type = bom_type, introw= introw, intpage = intpage });
				return result.ToList();
			}
			catch(Exception e)
            {
				throw e;
            }
        }
		public List<ATMTIMS> SearchATMTimsS1(string product_id, string name_id, string at_id, string reg_dt, string reg_dt_end, string bom_type,int intpage,int introw)
		{
			string sql = "";
			if (at_id != "")
            {
				sql = @"SELECT * 
							from  atm_tims as b join d_style_info c on c.style_no = b.product  WHERE (@productid='' or @productid is null or product like '%'+@productid+'%')
							and (@nameid='' or @nameid is null or product_nm like '%'+@nameid+'%')
							and (@bom_type='' or @bom_type is null or product_nm like '%'+@nameid+'%')
							and (@atid='' or @atid is null or at_no like '%'+@atid+'%')
							and (@regdt='' or @regdt is null or CONVERT(DATETIME, b.reg_dt,121) >=CONVERT(DATETIME, @regdt,121))
							and (@regdtend='' or @regdtend is null or CONVERT(DATETIME, b.reg_dt,121) <= CONVERT(DATETIME, @regdtend,121))
							order by b.at_no DESC
						--	OFFSET @intpage ROWS FETCH NEXT @introw ROWS ONLY
				";
			}
            else
            {
				sql = @"SELECT * 
							from  atm_tims as b join d_style_info c on c.style_no = b.product  WHERE (@productid='' or @productid is null or product like '%'+@productid+'%')
							and (@nameid='' or @nameid is null or product_nm like '%'+@nameid+'%')
							and (@bom_type='' or @bom_type is null or product_nm like '%'+@nameid+'%')
							and (@atid='' or @atid is null or at_no like '%'+@atid+'%')
							and (@regdt='' or @regdt is null or CONVERT(DATETIME, b.reg_dt,121) >=CONVERT(DATETIME, @regdt,121))
							and (@regdtend='' or @regdtend is null or CONVERT(DATETIME, b.reg_dt,121) <= CONVERT(DATETIME, @regdtend,121))
							order by b.at_no DESC
						--	OFFSET @intpage ROWS FETCH NEXT @introw ROWS ONLY
				";
			}

			var result = DbConnection.Query<ATMTIMS>(sql, new { productid = product_id, nameid = name_id, atid = at_id, regdt = reg_dt, regdtend = reg_dt_end , bom_type = bom_type, intpage= intpage, introw = introw });
			return result.ToList();
		}
		public List<ATMTIMS> SearchATMTimsS(string product_id, string name_id, string at_id, string reg_dt, string reg_dt_end, string bom_type)
		{
			string sql = @"SELECT * 
							from  atm_mms as b join d_style_info c on c.style_no = b.product  WHERE (@productid='' or @productid is null or product like '%'+@productid+'%')
							and (@nameid='' or @nameid is null or product_nm like '%'+@nameid+'%')
							and (@bom_type='' or @bom_type is null or product_nm like '%'+@nameid+'%')
							and (@atid='' or @atid is null or at_no like '%'+@atid+'%')
							and (@regdt='' or @regdt is null or CONVERT(DATETIME, b.reg_dt,121) >=CONVERT(DATETIME, @regdt,121))
							and (@regdtend='' or @regdtend is null or CONVERT(DATETIME, b.reg_dt,121) <= CONVERT(DATETIME, @regdtend,121))
							order by b.reg_dt DESC

 ";
			var result = DbConnection.Query<ATMTIMS>(sql, new { productid = product_id, nameid = name_id, atid = at_id, regdt = reg_dt, regdtend = reg_dt_end, bom_type = bom_type });
			return result.ToList();
		}
		public bool CheckPOMove(string at_no)
		{
			string sql = @"SELECT IsMove FROM w_actual_primary WHERE at_no = @atno ";
			return DbConnection.QueryFirstOrDefault<bool>(sql, new { atno = at_no });

		}
		public List<ATMTIMS> SearchATMTimsOQC(string product_id, string name_id, string at_id, string reg_dt, string reg_dt_end)
		{
			string sql = @"SELECT [waiting]
						  ,[model]
						  ,[unit_cd]
						  ,[prj_nm]
						  ,[waitold]
						  ,[at_no]
						  ,[product]
						  ,[product_nm]
						  ,convert(varchar(23),[reg_dt],121) reg_dt
						  ,[m_lieu]
						  ,convert(decimal(10,2),[need_m])need_m
						  ,[actual]
						  ,[id_actual]
						  ,[HS]
						  ,[mt_no]
						  ,[mt_nm]
						  ,[actual_lt]
						  ,[NG]
						  ,[target] FROM [atm_timsoqc] WHERE (@productid='' or @productid is null or product like '%'+@productid+'%')
					and (@nameid='' or @nameid is null or product_nm like '%'+@nameid+'%')
					and (@atid='' or @atid is null or at_no like '%'+@atid+'%')
					and (@regdt='' or @regdt is null or CONVERT(DATETIME, reg_dt,121) >=CONVERT(DATETIME, @regdt,121))
					and (@regdtend='' or @regdtend is null or CONVERT(DATETIME, reg_dt,121) <= CONVERT(DATETIME, @regdtend,121))";
			var result = DbConnection.Query<ATMTIMS>(sql, new { productid = product_id, nameid = name_id, atid = at_id, regdt = reg_dt, regdtend = reg_dt_end });
			return result.ToList();
		}
		public async Task<string> GetGrade(string userid)
		{
			string sql = @"SELECT grade FROM mb_info WHERE userid=@userid";
			return await base.DbConnection.QueryFirstOrDefaultAsync<string>(sql, new { userid = userid });
		}
		public async Task<int> UpdateCompositeShipping(int w_mtid, string user_id, string output_date)
		{
			try
			{
				var query = @"Update w_material_info_tims SET
									status = '000',
									location_code = '004000000000000000',
									output_dt = @output_dt,
									to_lct_code = '004000000000000000',
									chg_id = @userid

								WHERE wmtid in (@wmtid)";
				var result = await base.DbConnection.ExecuteAsync(query, new { wmtid = w_mtid, userid = user_id, output_dt = output_date });
				return result;
			}
			catch (Exception e)
			{

				throw;
			}
		}
		public async Task<int> UpdateProcessToHieuSuat(string at_no)
		{
			try
			{
				var query = @"Update w_actual SET IsFinish= 0 WHERE type= 'TIMS' and at_no= @at_no ";
				var result = await base.DbConnection.ExecuteAsync(query, new { at_no = at_no});
				return result;
			}
			catch (Exception e)
			{
				throw;
			}
		}
		public async Task<int> UpdateActualIsFinish(int id_actual, bool IsFinished)
		{
			try
			{
				var query = @"Update w_actual Set IsFinish = @IsFinished where id_actual=@id_actual  ";
				var result = await base.DbConnection.ExecuteAsync(query, new { id_actual = id_actual, IsFinished = IsFinished });
				return result;
			}
			catch (Exception e)
			{
				throw;
			}
		}
		public async Task<IReadOnlyList<WMaterialInfoTIMSAPIReceing>> GetDetailActualAPIReceiving(string receive_date, string product, string shift)
		{
			try {
				string sqlquery = @" SELECT * FROM ( 
									SELECT a.bb_no, a.material_code as mt_cd, a.gr_qty, a.real_qty, a.recevice_dt_tims, b.product,b.at_no, 
										(
										CASE 
										WHEN ('08:00:00' <= CAST( a.recevice_dt_tims AS TIME ) AND  CAST( a.recevice_dt_tims AS TIME  )  <  '23:59:59') THEN
										FORMAT(CAST( a.recevice_dt_tims AS DATETIME ),'yyyy-MM-dd')

										when (  CAST( a.recevice_dt_tims AS TIME ) < '08:00:00') THEN   FORMAT(DateAdd(DAY,-1,a.recevice_dt_tims), 'yyyy-MM-dd')
										ELSE ''
										END )  as  receive_date,
										(
										CASE 
										WHEN ('08:00:00' <=  CAST( a.recevice_dt_tims AS TIME) AND  CAST( a.recevice_dt_tims AS TIME )  <  '20:00:00') THEN
										'Ca ngày'
										WHEN
										(CAST( a.recevice_dt_tims AS TIME ) >= '20:00:00' AND  CAST( a.recevice_dt_tims AS TIME ) <= '23:59:59' OR 

										 CAST( a.recevice_dt_tims AS TIME ) < '08:00:00')
										THEN  'Ca Đêm'
										ELSE ''
										END )  as shift
										FROM w_material_info_mms AS a
										join w_actual b on b.id_actual =a.id_actual
									   where a.location_code = '006000000000000000' 
									   ) AS TABLE1
 ";
				sqlquery += $" where (@shift ='' or TABLE1.shift like '{shift}') ";


				sqlquery += @"AND (@product ='' or TABLE1.product = @product) 
							AND ( @receive_date ='' or TABLE1.receive_date = @receive_date) 
					order by table1.at_no,table1.bb_no ";
				var result = await base.DbConnection.QueryAsync<WMaterialInfoTIMSAPIReceing>(sqlquery, new { @product = product, @shift = shift, @receive_date = receive_date });
			return result.ToList();
			}
            catch (Exception e)
			{
				throw e;
			}
			//return db.Database.SqlQuery<WMaterialInfoTIMSAPIReceing>(sqlquery,
			//  new MySqlParameter("1", product),

			// new MySqlParameter("2", "%" + product + "%"),
			//	new MySqlParameter("3", date),
			//	new MySqlParameter("4", "%" + date + "%"),
			//	new MySqlParameter("5", shift),
			//	new MySqlParameter("6", "%" + shift + "%")
			//  );

		}

		public async Task<int> TotalRecordsSearchShippingSortingTims(string ShippingCode, string productCode, string productName, string description)
		{
			try
			{
				string countSql = @"SELECT COUNT(*) 
	                    FROM   shippingtimssorting AS a
	                   Where  a.IsFinish='N' AND ((@ShippingCode='' OR  a.ShippingCode like @ShippingCode ) 
                                AND (@productCode='' OR  a.ProductCode like @productCode )
                                AND (@productName='' OR  a.ProductName like @productName )
                                AND (@description='' OR  a.Description like @description ))
                ";
				var result = await base.DbConnection.ExecuteAsync(countSql, new { ShippingCode = ShippingCode, productCode = productCode, productName = productName, description= description });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<IReadOnlyList<ShippingTIMSSortingModel>> GetListSearchShippingSortingTIMS(string ShippingCode, string ProductCode, string ProductName, string Description)
		{
			try
			{
				string viewSql = @" SELECT a.* 
              
	                    FROM   shippingtimssorting AS a
	                    Where  a.IsFinish='N' AND ((@ShippingCode='' OR  a.ShippingCode like @ShippingCode ) 
                                AND (@productCode='' OR  a.ProductCode like @productCode )
                                AND (@productName='' OR  a.ProductName like @productName )
                                AND (@description='' OR  a.Description like @description ))
	           
                    order by a.id desc ";
				var result = await base.DbConnection.QueryAsync<ShippingTIMSSortingModel>(viewSql, new { ShippingCode = ShippingCode, productCode = ProductCode, productName = ProductName, description = Description });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IReadOnlyList<ShippingTIMSSortingModel>> GetListSearchShippingSortingTIMSPP(string ShippingCode)
		{
			try
			{
				string viewSql = @" SELECT a.* 
              
	                    FROM   shippingtimssorting AS a
	                    Where  a.ShippingCode = @ShippingCode
	                        ";
				var result = await base.DbConnection.QueryAsync<ShippingTIMSSortingModel>(viewSql, new { ShippingCode = ShippingCode});
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IReadOnlyList<ShippingFGSortingDetailModel>> GetListShippingTIMSSorting(string ShippingCode)
		{
			try
			{
				string QuerySQL = @"SELECT a.*,( CASE 
                                         WHEN (a.location  =  '006000000000000000' ) THEN 'TIMS'
                                        WHEN (a.location IS NOT NULL AND a.location  !=  '006000000000000000' )then 'FG' END) AS locationname
                        
                        FROM shippingtimssortingdetail as a WHERE ShippingCode = @ShippingCode";
				var result = await base.DbConnection.QueryAsync<ShippingFGSortingDetailModel>(QuerySQL, new { ShippingCode = ShippingCode });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<IReadOnlyList<ShippingFGSortingDetailModel>> GetShippingScanPPCountbuyer(string ShippingCode)
		{
			try
			{
				string QuerySQL = @"SELECT max(a.productCode) ProductCode ,MAX(a.Model) Model, sum(a.Quantity) sumQuantity, COUNT(a.buyer_qr) countBuyer
								  FROM shippingtimssortingdetail AS a
								  WHERE a.ShippingCode =@ShippingCode
								  GROUP BY a.ShippingCode;";
				var result = await base.DbConnection.QueryAsync<ShippingFGSortingDetailModel>(QuerySQL, new { @ShippingCode = ShippingCode });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<IReadOnlyList<ShippingFGSortingDetailModel>> CheckSFinfo(string ShippingCode)
		{
			try
			{
				string QuerySQL = "SELECT top 1 ShippingCode FROM shippingfgsorting WHERE ShippingCode = @ShippingCode";
				var result = await base.DbConnection.QueryAsync<ShippingFGSortingDetailModel>(QuerySQL, new { @ShippingCode = ShippingCode });
				return result.ToList();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<ShippingFGSortingDetailModel> isCheckExistSF(string ShippingCode, string buyer_qr)
		{
			try
			{
				string getvalue = @"SELECT *
                            FROM shippingfgsortingdetail 
                            WHERE ShippingCode = @ShippingCode and buyer_qr = @buyer_qr";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<ShippingFGSortingDetailModel>(getvalue, new { @ShippingCode = ShippingCode, @buyer_qr= buyer_qr });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<WMaterialnfo> isCheckExistBuyerQRSF(string buyer_qr)
		{
			try
			{
				string getvalue = @"SELECT top 1 * 
                            FROM w_material_info_tims
                            WHERE buyer_qr = @buyer_qr ";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<WMaterialnfo>(getvalue, new { @buyer_qr = buyer_qr });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<int> UpdateBuyerFGWMaterialInfo(WMaterialnfo item)
		{
			try
			{
				string sql = @"UPDATE w_material_info_tims set status= @mt_sts_cd,chg_id=@chg_id, location_code = @lct_cd
                      WHERE buyer_qr = @buyer_qr  ";
				var result = await base.DbConnection.ExecuteAsync(sql, new { @buyer_qr = item.buyer_qr, @mt_sts_cd = "015", @chg_id =item.chg_id, @lct_cd = "006000000000000000" });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

        public async Task<int> updateShippingFGSorting(ShippingFGSortingDetailModel item)
        {
            try
            {
                string sql = @"UPDATE shippingfgsortingdetail set location= @location
                      WHERE buyer_qr = @buyer_qr and ShippingCode = @ShippingCode ";
                var result = await base.DbConnection.ExecuteAsync(sql, new { @buyer_qr = item.buyer_qr, @ShippingCode = item.ShippingCode , @location = "006000000000000000" });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
		public async Task<ShippingTIMSSortingModel> GetLastShippingTIMSSorting()
		{
			try
			{
				string viewSql = @" SELECT top 1 a.ShippingCode
              
	                    FROM   shippingtimssorting AS a
	           
                    order by a.id desc  ";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<ShippingTIMSSortingModel>(viewSql);
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<int> InsertToShippingTIMSSorting(ShippingTIMSSortingModel item)
		{
			try
			{
				string QuerySQL = @"INSERT INTO shippingtimssorting (ShippingCode,ProductCode,ProductName,IsFinish,Description,CreateId,CreateDate,ChangeId,ChangeDate)
            VALUES (@ShippingCode,@ProductCode,@ProductName, @IsFinish, @Description, @CreateId, @CreateDate, @ChangeId, @ChangeDate);
			";

				var result = await base.DbConnection.ExecuteAsync(QuerySQL, new { @ShippingCode = item.ShippingCode,
					@ProductCode=item.ProductCode,
					@ProductName = item.ProductName,
					@IsFinish = item.IsFinish,
					@Description = item.Description,
					@CreateId = item.CreateId,
					@CreateDate = item.CreateDate,
					@ChangeId = item.ChangeId,
					@ChangeDate = item.ChangeDate });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<int> ModifyShippingTIMSSorting(ShippingTIMSSortingModel item)
		{
			try
			{
				string QuerySQL = @"UPDATE shippingtimssorting SET 
     ProductCode=@ProductCode,ProductName= @ProductName,Description=@Description,CreateId=@CreateId,ChangeId=@ChangeId,ChangeDate=@ChangeDate
            WHERE id=@Id";
				var result = await base.DbConnection.ExecuteAsync(QuerySQL, new
				{
					@ProductCode = item.ProductCode,
					@ProductName = item.ProductName,
					@Id = item.id,
					@Description = item.Description,
					@CreateId = item.CreateId,
					@CreateDate = item.CreateDate,
					@ChangeId = item.ChangeId,
					@ChangeDate = item.ChangeDate
				});
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<WMaterialnfo> CheckIsExistBuyerCode(string BuyerCode)
		{
			try
			{
				string QuerySQL = @"SELECT top 1 a.*
                                FROM w_material_info_tims as a WHERE a.buyer_qr = @buyer_qr";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<WMaterialnfo>(QuerySQL, new
				{
					@buyer_qr= BuyerCode
				});
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<int> UpdateShippingSortingTIMS(ShippingTIMSSortingDetailModel item, string data)
		{
			try
			{
				int result = 0;
				var listData = data.Split(',');
				foreach (var it in listData)
				{
					string sqlupdate = @"Update shippingtimssortingdetail SET location=@data
                            WHERE  id = @id ";
					result = await base.DbConnection.ExecuteAsync(sqlupdate, new
					{
						@data = item.location,
						@id = it
					});
				}
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> InsertShippingSortingTIMSDetail(string ShippingCode, string ListId, string UserID)
		{
			try
			{
				int result = 0;
				var listData = ListId.Split(',');
				foreach (var item in listData)
				{
					string QuerySQL = @"INSERT INTO shippingtimssortingdetail(ShippingCode,buyer_qr,CreateId,Model,productCode,lot_no,Quantity,location)
			        SELECT @ShippingCode, a.buyer_qr, @UserID, '',a.product,a.lot_no, a.real_qty,a.location_code
                    FROM w_material_info_tims  as a
				    WHERE  a.wmtid = @id ;
					";
					result = await base.DbConnection.ExecuteAsync(QuerySQL, new
					{
						@id = item,
						@ShippingCode = ShippingCode,
						@UserID = UserID
					});
				}
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<IReadOnlyList<ShippingTIMSSortingDetailModel>> GetListShippingFGSorting(string ShippingCode)
		{
			try
			{
				string QuerySQL = @"SELECT a.*,( CASE 
                                         WHEN (a.location  =  '006000000000000000' ) THEN 'TIMS'
                                        WHEN (a.location IS NOT NULL AND a.location  !=  '006000000000000000' )then a.location 
                                       else 'FG' END) AS locationname
                        
                        FROM shippingtimssortingdetail as a WHERE ShippingCode = @ShippingCode";
				var result = await base.DbConnection.QueryAsync<ShippingTIMSSortingDetailModel>(QuerySQL, new
				{
					ShippingCode = ShippingCode
				});
				return result.ToList();
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
				string sqlquery = @"DELETE FROM shippingtimssorting WHERE Id = @id";
				var result = await base.DbConnection.ExecuteAsync(sqlquery, new { @id = id });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<int> DeleteMfaclineQC(string ml_no, string ml_tims)
		{
			string sql = @"DELETE FROM m_facline_qc WHERE ml_no=@ml_no AND ml_tims=@ml_tims";
			return await base.DbConnection.ExecuteAsync(sql, new { ml_no = ml_no, ml_tims = ml_tims });
		}
		public async Task<IReadOnlyList<MaterialInfoTIMS>> CheckBobbinDestroyMMS(int wmtid,string MaterialCode, string lct_cd)
		{
			string sql = @"select a.wmtid, a.material_code, a.status, a.location_code, a.bb_no
							from w_material_info_mms as a
							Where wmtid = @wmtid and status in('008','009','002','003','010') and material_code = @MaterialCode ";
			var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(sql, new { wmtid = wmtid, lctcd = lct_cd , MaterialCode  = MaterialCode });
			return result.ToList();
			//return db.Database.SqlQuery<MaterialInfoMMS>(sql, new MySqlParameter("@1", mt_cd), new MySqlParameter("@2", mt_sts_cd), new MySqlParameter("@3", lct_cd));
		}
		public async Task<int> SPUpdateDestroyMMS(int id, string status, string mt_cd, string bb_no, string userid)
		{
			string sql = @"UPDATE w_material_info_mms
			SET	chg_date=GETDATE(),status='011',chg_id=@regid,sts_update=@stsupdate
			WHERE wmtid = @wmtid and material_code = @mt_cd ;";
			await base.DbConnection.ExecuteAsync(sql, new { wmtid = id, stsupdate = status, regid = userid, mt_cd = mt_cd });
			string sqldelete = @"DELETE FROM d_bobbin_lct_hist WHERE bb_no = @bb_no  and mt_cd = @mt_cd ;
									update d_bobbin_info set mt_cd= null  where bb_no =@bb_no  and mt_cd = @mt_cd";
			return await base.DbConnection.ExecuteAsync(sqldelete, new { wmtid = id, bb_no, mt_cd });
		}
		public async Task<Models.WOModel.WMaterialInfoNew> CheckBobbinRedoMMS(int wmtid, string status, string MaterialCode)
		{
			string sql = @"select  a.wmtid, a.material_code as mt_cd, a.status as mt_sts_cd, a.bb_no, a.sts_update
							from w_material_info_mms as a
							Where a.wmtid = @wmtid and a.status = @status and a.material_code = @MaterialCode ";
			return await base.DbConnection.QueryFirstOrDefaultAsync <Models.WOModel.WMaterialInfoNew> (sql, new { wmtid = wmtid, status = status, MaterialCode = MaterialCode });


		}
		public async Task<int> SPUpdatesRedoMMS(int id, string status,  string userid)
		{
			string sql = @"UPDATE w_material_info_mms
					SET
					chg_date=GETDATE(),
					status=@sts_update,
					sts_update = 'composite',
					chg_id=@regid
					WHERE wmtid = @wmtid;";
			return await base.DbConnection.ExecuteAsync(sql, new { wmtid = id, sts_update = status, regid = userid });
		}
		public async Task<string> ChecktypeProduct(string style_no)
		{
            try
            {
				string sql = @"Select a.productType 
					from d_style_info a
          
					where   a.style_no = @style_no";
				var resuilt =  await base.DbConnection.ExecuteScalarAsync<string>(sql, new { style_no = style_no });
				return resuilt;
			}
            catch (Exception e)
            {

                throw;
            }
		}
		public async Task<MaterialInfoTIMS> FindOneMaterialInfoByIdBuyer(int wmtid)
		{
			try
			{
				var query = @"Select Top 1 * from  w_material_info_tims where wmtid = @wmtid";
				var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoTIMS>(query, new { wmtid = wmtid });
				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<Generalfg> FindOneBuyerInfoByBuyer(string buyer_code)
		{
			string sql = @"SELECT * FROM generalfg WHERE buyer_qr=@buyerqr";
			return await base.DbConnection.QueryFirstOrDefaultAsync<Generalfg>(sql, new { buyerqr = buyer_code });
		}
		public async Task<int> UpdateBuyerQRGeneral(Generalfg item)
		{
			string sql = @"UPDATE generalfg set buyer_qr = @buyer_qr ,chg_id=@chg_id, chg_dt =GETDATE() 
                WHERE id = @id";
			return await base.DbConnection.ExecuteAsync(sql, new { id = item.id, buyer_qr = item.buyer_qr, chg_id = item.chg_id });
		}
		public async Task<int> UpdateMaterialInfoTimsBuyer(int wmtid, string buyer_qr, string chg_id)
		{
			string sql = @"UPDATE w_material_info_tims SET buyer_qr = @buyer_qr, chg_date = GETDATE(), chg_id = @chg_id WHERE wmtid = @wmtid";
			return await base.DbConnection.ExecuteAsync(sql, new { wmtid = wmtid, chg_id = chg_id, buyer_qr = buyer_qr });
		}
		public async Task<int> CheckCountExistFacline(string material_code)
		{
			string sql = "SELECT COUNT(fqno) FROM m_facline_qc WHERE ml_tims=@mtcd";
			return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { mtcd = material_code });
		}
		public async Task<MaterialInfoTIMS> GetwmtinfotimswithBobbin(string status, string bb_no)
		{
			string sql = @" SELECT top 1 * 
							FROM w_material_info_tims
							WHERE status = @status and bb_no = @bb_no  ORDER BY wmtid DESC ";
			return await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoTIMS>(sql,
				new { status = status, bb_no = bb_no });
		}
		public async Task<Models.WOModel.WMaterialInfoNew> GetwmtinfoMMSwithBobbin(string status, string bb_no)
		{
			string sql = @"select top 1 *
							from w_material_info_mms
							Where  status = @status and bb_no = @bb_no ORDER BY wmtid DESC  ";
			return await base.DbConnection.QueryFirstOrDefaultAsync<Models.WOModel.WMaterialInfoNew>(sql, new { status = status, bb_no = bb_no });


		}
		public async Task<BobbinInfo> GetBobbinInfoByMaterialCode(string mt_cd)
		{
			string QuerySQL = "SELECT * FROM d_bobbin_info WHERE mt_cd = @mt_cd";
			return await base.DbConnection.QueryFirstOrDefaultAsync<BobbinInfo>(QuerySQL, new { mt_cd = mt_cd });
		}
		public async Task<int> UpdateMaterialBBMMS(int wmtid, string bb_no)
		{
			string sql = @"UPDATE w_material_info_mms
					SET
					chg_date=GETDATE(),
					bb_no=@bb_no
					WHERE wmtid = @wmtid;";
			return await base.DbConnection.ExecuteAsync(sql, new { wmtid = wmtid, bb_no = bb_no });
		}
		public async Task<int> UpdateMaterialBBTIMS(int wmtid, string bb_no)
		{
			string sql = @"UPDATE w_material_info_tims
					SET
					chg_date=GETDATE(),
					bb_no=@bb_no
					WHERE wmtid = @wmtid;";
			return await base.DbConnection.ExecuteAsync(sql, new { wmtid = wmtid, bb_no = bb_no });
		}
		public async Task<int> CheckCountExistFaclineRoll(string material_code, string material_lot)
		{
			string sql = "SELECT COUNT(fqno) FROM m_facline_qc WHERE ml_no = @mtcd and  ml_tims=@mtlot";
			return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { mtcd = material_code ,mtlot = material_lot });
		}
		public async Task<IReadOnlyList<ATMTIMSEFF>> getHieuSuat(string ProductCode, string ProductName, string PO, string Remark, string bom_type)
        {
            try
			{
				string sql = @"SELECT  b.remark,a.at_no, a.product, c.style_nm product_nm, isnull(a.actual_lt,0)	 AS SanLuongLyThuyet, a.m_lieu AS SoMetLyThuyet, a.mt_no,
							   c.loss, concat(isnull(ROUND((b.actual / a.actual_lt) * 100, 2),0), '%') HieuSuatSanXuat,
						       b.actual AS OKSanXuat,b.defect AS NgSanXuat,
							   concat(isnull(ROUND((a.actual / (a.actual_lt - a.waiting)) * 100, 2),0), '%') HieusuatOQC,
                               a.actual OkThanhPham, a.NG NGThanhPham,
							   a.waiting HangChoKiem,
							   c.bom_type
							   FROM atm_tims AS a
							   JOIN atm_mms AS b
							   ON a.at_no = b.at_no
							   left JOIN d_style_info AS c
							   ON a.product = c.style_no
							   WHERE(@ProductCode = '' OR a.product LIKE CONCAT('%', @ProductCode, '%')) 
								AND(@ProductName = '' OR a.product_nm LIKE CONCAT('%', @ProductName, '%'))
								AND(@bom_type = '' OR c.bom_type LIKE CONCAT('%', @bom_type, '%'))
								AND(@PO = '' OR a.at_no LIKE CONCAT('%', @PO, '%'))
								AND(@Remark = '' OR b.remark LIKE CONCAT('%', @Remark, '%'))
								order by a.at_no desc
								; ";
				var resutl = await base.DbConnection.QueryAsync<ATMTIMSEFF>(sql, new { @ProductCode = ProductCode, @ProductName = ProductName, @PO =PO , @Remark= Remark, bom_type = bom_type });
				return resutl.ToList();
			}
			catch(Exception e)
            {
				throw e;
            }

        }
		public async Task<IReadOnlyList<w_material_mapping>> GetmaterialmappingStsShare(string mt_lot)
		{
			string sqlquery = @"SELECT * FROM w_material_mapping_tims WHERE  mt_lot=@mtlot and use_yn = 'Y' ";
			var result =  await base.DbConnection.QueryAsync<w_material_mapping>(sqlquery, new { @mtlot = mt_lot });

			return result.ToList();
		}
		public async Task<Models.WOModel.WMaterialInfoNew> GetWMaterialInfoWithmtcdLikemms(string mt_cd)
		{
			try
			{
				var query = @"SELECT  a.wmtid, a.material_code , a.bb_no, a.status mt_sts_cd, a.gr_qty FROM w_material_info_mms a WHERE material_code  = @mt_cd order by reg_date DESC";
				return await base.DbConnection.QueryFirstOrDefaultAsync<Models.WOModel.WMaterialInfoNew>(query, new {mt_cd = mt_cd });
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public async Task<int> UpdateQtyMaterialMMS(int quantity, int wmtid, string userid)
		{
			string sqlquery = "UPDATE w_material_info_mms SET gr_qty=@grpqty,chg_date=GETDATE(),chg_id=@chgid WHERE wmtid=@wtmid";
			return await base.DbConnection.ExecuteAsync(sqlquery, new { grpqty = quantity, chgid = userid, wtmid = wmtid });
			//db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", quantity), new MySqlParameter("2", wmtid), new MySqlParameter("3", mt_sts_cd));
		}

        public List<truyxuatlot> TruyxuatlistlotOQC(string mt_cd, string tentam, string buyer_qr, string atno)
        {
			var sql = @"EXEC [dbo].[CheckProcessOQC] @material_code,@at_no";
			//var result = await base.DbConnection.QueryAsync<truyxuatlot>(sql, new { @material_code = mt_cd, @at_no = atno });
			//return result.ToList();


			var result = base.DbConnection.Query<truyxuatlot>(sql, new { @material_code = mt_cd, @at_no = atno }, commandTimeout: 180);
			return result.ToList();
		}
    }
}
