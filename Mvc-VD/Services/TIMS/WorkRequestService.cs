using Mvc_VD.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Mvc_VD.Services.TIMS
{
    public interface IWorkRequestService
    {
        DataTable Search_Delivery_Detaill(string style_no, object start, object end);
        DataTable GetWorkRequest(string po_no, string delivery_dt);
        DataTable Search_Delivery_Detail(string style_no, object start, object end);
    }
    public class WorkRequestService  : IWorkRequestService
    {
        public DataTable Search_Delivery_Detail(string style_no, object start, object end)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT 	c.start_dt, c.end_dt,b.fm_no, b.line_no, 	a.soid AS sid,  a.style_no, 	b.po_no,b.fo_no, \n");
            varname1.Append("			(SELECT i.md_cd FROM d_style_info AS i WHERE i.style_no = a.style_no LIMIT 1) as md_cd, \n");
            varname1.Append("			(SELECT i.style_nm FROM d_style_info AS i WHERE i.style_no = a.style_no LIMIT 1) as style_nm \n");
            varname1.Append("	FROM s_order_info AS a \n");
            varname1.Append("	 	 JOIN s_order_factory_info AS b ON a.po_no =b.po_no \n");
            varname1.Append("		  LEFT JOIN m_order_facline_info AS c ON c.po_no = a.po_no \n");
            varname1.Append("		  WHERE a.style_no ='" + style_no + "' ");
            varname1.Append("  AND ('" + start + "'='' \n");
            varname1.Append("       OR CONVERT(c.start_dt,DATE)>=CONVERT('" + start + "',DATE)) \n");
            varname1.Append("  AND ('" + end + "'='' \n");
            varname1.Append("       OR CONVERT(c.start_dt,DATE)<=CONVERT('" + end + "',DATE)) \n");
            varname1.Append(" ORDER BY b.fo_no DESC");
            DataTable data = new Excute_query().get_data_from_data_base(varname1);
            return data;
        }   
        public DataTable GetWorkRequest(string po_no, string delivery_dt )
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select result.*,so.bom_no, \n");
            varname1.Append("(select \n");
            varname1.Append("			(select sum(ifnull(cast(gr_qty as int ),0)) \n");
            varname1.Append("					from w_material_info as i \n");
            varname1.Append("					where (   SUBSTRING_INDEX(TRIM(LEADING concat(i.mt_no,'-LOT') FROM i.mt_cd ), 'F', 1)     = \n");
            varname1.Append("										 CONCAT(substr(a.fo_no, 1, 1), CONVERT(substr(a.fo_no, 2, 11),INT))  and i.mt_sts_cd ='008' and i.mt_type='CMT') \n");
            varname1.Append("						 group by i.mt_sts_cd) \n");
            varname1.Append("						 					as total_qty_008 \n");
            varname1.Append("			  \n");
            varname1.Append("	 from s_order_factory_info as a where a.po_no = result.po_no having total_qty_008   is not null limit 1) as total_008, \n");
            varname1.Append("	 (select \n");
            varname1.Append("			(select sum(ifnull(cast(gr_qty as int ),0)) \n");
            varname1.Append("					from w_material_info as i \n");
            varname1.Append("					where (   SUBSTRING_INDEX(TRIM(LEADING concat(i.mt_no,'-LOT') FROM i.mt_cd ), 'F', 1)     = \n");
            varname1.Append("										 CONCAT(substr(a.fo_no, 1, 1), CONVERT(substr(a.fo_no, 2, 11),INT))  and i.mt_sts_cd ='009' and i.mt_type='CMT') \n");
            varname1.Append("						 group by i.mt_sts_cd) \n");
            varname1.Append("						 					as total_qty_008 \n");
            varname1.Append("			  \n");
            varname1.Append("	 from s_order_factory_info as a where a.po_no = result.po_no having total_qty_008 is not null limit 1) as total_009 \n");
            varname1.Append(" 	from s_order_ship_info as result \n");
            varname1.Append(" 	left join s_order_factory_info as so \n");
            varname1.Append(" 		on so.po_no = result.po_no");
            varname1.Append(" WHERE ('" + po_no + "'=''  OR result.po_no LIKE '%" + po_no + "%')");
            varname1.Append(" AND ('" + delivery_dt + "'=''  OR result.delivery_dt LIKE '%" + delivery_dt + "%') ");

            var data = new Excute_query().get_data_from_data_base(varname1);
            return data;
        }

        public DataTable Search_Delivery_Detaill(string style_no, object start, object end)
        {
            throw new NotImplementedException();
        }
    }
}