using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_VD.Models;
using System.Collections;
using System.Text;
using Mvc_VD.Extensions;
using PagedList;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using Mvc_VD.Classes;
using Mvc_VD.Services.Interface.QMS;
using System.Threading.Tasks;
using Mvc_VD.Services.Interface;
using Mvc_VD.Commons;

namespace Mvc_VD.Controllers
{
    public class DashBoardQCController : BaseController
    {
        private IQMSService _iQMSService;
        private readonly IhomeService _homeService;
        public DashBoardQCController(IQMSService iQMSService,IhomeService ihomeService)
        {
            _homeService = ihomeService;
            _iQMSService = iQMSService;
        }

        private Entities db = new Entities();
      
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var lang = HttpContext.Request.Cookies.AllKeys.Contains("language")
                  ? HttpContext.Request.Cookies["language"].Value
                  : "en";
            var router = this.ControllerContext.RouteData.Values.Values.ElementAt(0).ToString();
            var result = _homeService.GetLanguage(lang, router);
            foreach (var item in result)
            {
                if (lang == "en")
                {
                    ViewData.Add(new KeyValuePair<string, object>(item.keyname, item.en));
                }
                else if (lang == "vi")
                {
                    ViewData.Add(new KeyValuePair<string, object>(item.keyname, item.vi));
                }
                else if (lang == "kr")
                {
                    ViewData.Add(new KeyValuePair<string, object>(item.keyname, item.kr));
                }
            }

        }
        #region OQC

        public ActionResult OQCDash()
        {
            return SetLanguage("");
        }

        public ActionResult getcolum_prqc2(string pq_no)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select a.fo_no,a.pq_no, b.pqhno,a.oldd_no,e.check_subject,b.check_value ,(select sum(check_qty) as qc from w_product_qc_value where b.check_value=check_value) as qc_qty, a.style_no \n");
            varname1.Append("from w_product_qc as a \n");
            varname1.Append("LEFT join w_product_qc_value as b on a.pq_no=b.pq_no and a.item_vcd=b.item_vcd \n");
            varname1.Append("left join qc_item_mt as c on b.item_vcd=c.item_vcd \n");
            varname1.Append("left join qc_itemcheck_dt as d on c.item_vcd=d.item_vcd and b.check_value=d.check_name \n");
            varname1.Append("join qc_itemcheck_mt as e on d.item_vcd=e.item_vcd and d.check_id=e.check_id \n");
            varname1.Append("where a.pq_no='" + pq_no + "' \n");
            varname1.Append("group by a.pq_no,a.oldd_no,d.item_vcd,d.check_id,d.check_cd");
            var data = new DataTable();
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                db.Database.Connection.Open();
                cmd.CommandText = varname1.ToString();
                using (var reader = cmd.ExecuteReader())
                {
                    data.Load(reader);
                }
            }
            db.Database.Connection.Close();
            var result = GetJsonPersons(data);
            return result;
        }

        public JsonResult GetJsonPersons(DataTable data)
        {
            var lstPersons = GetTableRows(data);
            return Json(lstPersons, JsonRequestBehavior.AllowGet);
        }

        public List<Dictionary<string, object>> GetTableRows(DataTable data)
        {
            var lstRows = new List<Dictionary<string,
             object>>();
            Dictionary<string, object> dictRow = null;

            foreach (DataRow row in data.Rows)
            {
                dictRow = new Dictionary<string, object>();
                foreach (DataColumn column in data.Columns)
                {
                    dictRow.Add(column.ColumnName, row[column]);
                }
                lstRows.Add(dictRow);
            }
            return lstRows;
        }

        public ActionResult getcolum_prqc(string pq_no)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select a.fo_no,a.pq_no, b.pqhno,a.oldd_no,e.check_subject,b.check_value ,(select sum(check_qty) as qc from w_product_qc_value where b.check_value=check_value) as qc_qty, a.style_no \n");
            varname1.Append("from w_product_qc as a \n");
            varname1.Append("LEFT join w_product_qc_value as b on a.pq_no=b.pq_no and a.item_vcd=b.item_vcd \n");
            varname1.Append("left join qc_item_mt as c on b.item_vcd=c.item_vcd \n");
            varname1.Append("left join qc_itemcheck_dt as d on c.item_vcd=d.item_vcd and b.check_value=d.check_name \n");
            varname1.Append("join qc_itemcheck_mt as e on d.item_vcd=e.item_vcd and d.check_id=e.check_id \n");
            varname1.Append("where a.pq_no='" + pq_no + "' \n");
            varname1.Append("group by a.pq_no,a.oldd_no,d.item_vcd,d.check_id,d.check_cd");

            var data = new DataTable();
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                db.Database.Connection.Open();
                cmd.CommandText = varname1.ToString();
                using (var reader = cmd.ExecuteReader())
                {
                    data.Load(reader);
                }
            }
            db.Database.Connection.Close();
            var result = GetJsonPersons(data);
            return result;
        }

        public ActionResult gettable_prqc()
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select a.fo_no,a.pq_no, b.pqhno,a.oldd_no,e.check_subject,b.check_value ,(select sum(check_qty) as qc from w_product_qc_value where b.check_value=check_value) as qc_qty, a.style_no \n");
            varname1.Append("from w_product_qc as a \n");
            varname1.Append("LEFT join w_product_qc_value as b on a.pq_no=b.pq_no and a.item_vcd=b.item_vcd \n");
            varname1.Append("left join qc_item_mt as c on b.item_vcd=c.item_vcd \n");
            varname1.Append("left join qc_itemcheck_dt as d on c.item_vcd=d.item_vcd and b.check_value=d.check_name \n");
            varname1.Append("join qc_itemcheck_mt as e on d.item_vcd=e.item_vcd and d.check_id=e.check_id \n");
            varname1.Append(" where Convert(substring(a.work_dt,1,8), DATE) = Convert(now(), DATE) ");
            varname1.Append("group by a.pq_no,a.oldd_no,d.item_vcd,d.check_id");

            var data = new DataTable();
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                db.Database.Connection.Open();
                cmd.CommandText = varname1.ToString();
                using (var reader = cmd.ExecuteReader())
                {
                    data.Load(reader);
                }
            }
            db.Database.Connection.Close();
            var result = GetJsonPersons(data);
            return result;
        }

        public ActionResult searchPrductQcchart()
        {
            var start = Request["start"];
            var end = Request["end"];

            var style_no = Request["style_no"];

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select a.fo_no,a.pq_no, b.pqhno,a.oldd_no,e.check_subject,b.check_value ,(select sum(check_qty) as qc from w_product_qc_value where b.check_value=check_value) as qc_qty, a.style_no \n");
            varname1.Append("from w_product_qc as a \n");
            varname1.Append("LEFT join w_product_qc_value as b on a.pq_no=b.pq_no and a.item_vcd=b.item_vcd \n");
            varname1.Append("left join qc_item_mt as c on b.item_vcd=c.item_vcd \n");
            varname1.Append("left join qc_itemcheck_dt as d on c.item_vcd=d.item_vcd and b.check_value=d.check_name \n");
            varname1.Append("join qc_itemcheck_mt as e on d.item_vcd=e.item_vcd and d.check_id=e.check_id \n");
            varname1.Append("where ('" + style_no + "'='' or a.style_no LIKE  '%" + style_no + "%' ) and \n");
            varname1.Append(" (Convert(substring(a.work_dt,1,8), DATE) >= Convert('" + start + "', DATE) or '" + start + "'='') \n");
            varname1.Append("and (Convert(substring(a.work_dt,1,8), DATE) <= Convert('" + end + "', DATE) or '" + end + "'='') \n");
            varname1.Append("group by a.pq_no,a.oldd_no,d.item_vcd,d.check_id");
            var data = new DataTable();
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                db.Database.Connection.Open();
                cmd.CommandText = varname1.ToString();
                using (var reader = cmd.ExecuteReader())
                {
                    data.Load(reader);
                }
            }
            db.Database.Connection.Close();
            var result = GetJsonPersons(data);
            return result;
        }

        public ActionResult OverviewOQCInfo()
        {
            //string sql = "SELECT"
            //                + " a.fo_no,"
            //                + " a.pq_no,"
            //                + " b.pqhno,"
            //                + " a.oldd_no,"
            //                + " e.check_subject,"
            //                + " SUM(a.check_qty) AS qc_qty,"
            //                + " SUM(a.ok_qty) AS ok_qty,"
            //                + " (SUM(a.check_qty)-SUM(a.ok_qty)) AS defect_qty,"
            //                + " a.item_vcd,"
            //                + " a.work_dt,"
            //                + " a.style_no"
            //                + " from w_product_qc as a"
            //                + " LEFT join w_product_qc_value as b on a.pq_no = b.pq_no and a.item_vcd = b.item_vcd"
            //                + " left join qc_item_mt as c on b.item_vcd = c.item_vcd"
            //                + " left join qc_itemcheck_dt as d on c.item_vcd = d.item_vcd and b.check_value = d.check_name"
            //                + " join qc_itemcheck_mt as e on d.item_vcd  =e.item_vcd and d.check_id = e.check_id"
            //                + " GROUP BY a.work_dt, b.check_value, e.check_subject, a.item_vcd "
            //                ;
            return PartialView();
        }

        //public ActionResult Get_table_OQC_info()
        //{
        //    try
        //    {
        //        var start = Request.QueryString["start"];
        //        var end = Request.QueryString["end"];
        //        var style_no = Request.QueryString["style_no"];

        //        string sql = " select a.fo_no,a.pq_no,a.oldd_no,a.style_no, "
        //        + " sum(a.check_qty) as qc_qty, "
        //        + " sum(a.ok_qty) as ok_qty, " +
        //         " (sum(a.check_qty)-sum(a.ok_qty)) as def_qty " +
        //       "  from w_product_qc as a  " +
        //       " WHERE ('" + style_no + "'='' OR a.style_no LIKE '%" + style_no + "%')" +
        //        " AND (Convert(substring(a.work_dt, 1, 8), DATE) >= Convert('" + start + "', DATE)  OR '" + start + "'='')" +
        //        " AND (Convert(substring(a.work_dt, 1, 8), DATE) <= Convert('" + end + "', DATE)  OR '" + end + "'='')";

        //        var data = db.Database.SqlQuery<Get_table_info_OQC_D_Model>(sql).FirstOrDefault();

        //        if (string.IsNullOrEmpty(data.fo_no))
        //        {
        //            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        //        }

        //        return Json(new { result = true, data = data }, JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        public class Get_table_info_OQC_D_Model
        {
            public String work_dt { get; set; }
            public String ml_no { get; set; }

            public String qc_qty { get; set; } //sum(check_qty)
            public String item_vcd { get; set; }
            public String pq_no { get; set; }

            //public String fo_no { get; set; }
            public String ok_qty { get; set; }

            public String defect_qty { get; set; }
            //public String defect_rate { get; set; }
        }

        public async Task<ActionResult> _OQCDash_Bar()
        {
            var ml_no = Request["ml_no"];
            var start = Request.QueryString["start"];
            var end = Request.QueryString["end"];

            StringBuilder sql_info = new StringBuilder();

            //sql_info.Append(" SELECT a.item_vcd,a.pq_no,a.pqno,Date_format(a.work_dt, '%Y-%m-%d') work_dt,a.check_qty AS qc_qty,a.ok_qty,a.ml_no, ");
            //sql_info.Append("       (a.check_qty - a.ok_qty) AS defect_qty, ");
            //sql_info.Append("       IF(a.ok_qty = 0, 0, Round((a.check_qty - a.ok_qty)/a.check_qty * 100, 2)) defect_qty_qc_rate, ");
            //sql_info.Append("       IF(a.ok_qty = 0, 0, Round(a.ok_qty/a.check_qty * 100, 2)) ok_qty_qc_rate ");
            //sql_info.Append(" FROM w_product_qc AS a ");
            //sql_info.Append("WHERE ('" + ml_no + "' = '' OR a.ml_no LIKE '%" + ml_no + "%' ) ");
            //sql_info.Append("       AND ('" + start + "' = '' OR Date_format(a.work_dt, '%Y-%m-%d') >= Date_format('" + start + "', '%Y-%m-%d') )");
            //sql_info.Append("       AND ('" + end + "'= ''  OR Date_format(a.work_dt, '%Y-%m-%d') <= Date_format('" + end + "', '%Y-%m-%d') )");

            var data_info = await _iQMSService.GetListProductQC(start, end, ml_no);


            StringBuilder sql_detail = new StringBuilder();

            //sql_detail.Append("SELECT b.pqhno,b.pq_no,b.check_value,SUM(b.check_qty) AS qc_qty,a.item_vcd,a.ml_no,Date_format(a.work_dt, '%Y-%m-%d') work_dt ");
            //sql_detail.Append("FROM w_product_qc a ");
            //sql_detail.Append("JOIN w_product_qc_value b  ON a.pq_no=b.pq_no AND a.item_vcd=b.item_vcd ");
            //sql_detail.Append("WHERE ('" + ml_no + "' = '' OR a.ml_no LIKE '%" + ml_no + "%' ) ");
            //sql_detail.Append("       AND ('" + start + "' = '' OR Date_format(a.work_dt, '%Y-%m-%d') >= Date_format('" + start + "', '%Y-%m-%d') )");
            //sql_detail.Append("       AND ('" + end + "'= ''  OR Date_format(a.work_dt, '%Y-%m-%d') <= Date_format('" + end + "', '%Y-%m-%d') )");
            //sql_detail.Append("group by b.check_id,b.check_cd,b.check_value;");

            //var model = db.Database.SqlQuery<myChart_error_Item_Model_OQC>(sql_detail.ToString()).ToList();
            var model = await _iQMSService.GetListProductQCModel(start, end, ml_no);

            var data = new List<myChart_error_Model_OQC>();
            if (data_info.Count() == 0)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
            string fo_no = "";
            string total_defect_qty = "";

            foreach (var item in model)
            {
                if (fo_no != item.pq_no)
                {
                    fo_no = item.pq_no;

                    var error = new myChart_error_Model_OQC();
                    error.ml_no = item.ml_no;
                    error.work_dt = item.work_dt;
                    error.pq_no = item.pq_no;
                    //error.line_no = item.line_no;
                    //error.bom_no = item.bom_no;
                    //error.style_no = item.style_no;
                    //error.po_no = item.po_no;
                    //error.style_nm = item.style_nm;

                    var dsdsds = data_info.Where(x => x.ml_no == item.ml_no).ToList();
                    if (dsdsds.Count == 0)
                    {
                        total_defect_qty = "0";
                    }
                    else
                    {
                        total_defect_qty = dsdsds.FirstOrDefault().defect_qty;
                    }

                    error.total_def_qty = total_defect_qty;

                    var error_item = new myChart_error_Item_Model_OQC();
                    error_item = item;
                    error.Error = new List<myChart_error_Item_Model_OQC>();
                    error.Error.Add(error_item);

                    data.Add(error);
                }
                else
                {
                    var subject = data.FirstOrDefault(x => x.pq_no == item.pq_no);

                    var error_item = new myChart_error_Item_Model_OQC();
                    error_item = item;
                    subject.Error.Add(error_item);
                }
            }

            foreach (var item in data)
            {
                item.datasets = "";
                item.labels = "";
                item.datasets_pie = "";
                foreach (var item1 in item.Error)
                {
                    item.datasets += item1.qc_qty + ",";

                    item.labels += "'" + item1.check_value + "',";

                    item.datasets_pie += "'" + item1.qc_qty + "',";
                }
                item.chart_info = data_info.ToList();
            }

            return PartialView(data);
        }

        public async Task<ActionResult> _OQCDash_Pie()
        {
            var ml_no = Request["ml_no"];

            var start = Request.QueryString["start"];
            var end = Request.QueryString["end"];
            //info
            //string sql_info = " select a.fo_no,a.pq_no,a.oldd_no,a.style_no,a.line_no, a.bom_no,(CONCAT(SUBSTRING(a.work_dt,1,4),'-',SUBSTRING(a.work_dt,5,2) ,'-',SUBSTRING(a.work_dt,7,2)) ) as work_dt, "
            //+ " sum(a.check_qty) as qc_qty, "
            //+ " sum(a.ok_qty) as ok_qty, " +
            // " (sum(a.check_qty)-sum(a.ok_qty)) as def_qty " +
            // "  from w_product_qc as a  " +
            // " WHERE ('" + style_no + "'='' OR a.style_no LIKE '%" + style_no + "%')" +
            //" AND (Convert(substring(a.work_dt, 1, 8), DATE) >= Convert('" + start + "', DATE)  OR '" + start + "'='')" +
            //" AND (Convert(substring(a.work_dt, 1, 8), DATE) <= Convert('" + end + "', DATE)  OR '" + end + "'='') group by a.fo_no";

            //string sql_info = " select CONCAT(SUBSTR(a.fo_no, 1, 1), CONVERT(SUBSTR(a.fo_no, 2, 11), INT)) as fo_no,a.pq_no,a.oldd_no,a.style_no,a.line_no, a.bom_no, CONCAT(SUBSTR(a.po_no, 1, 1), CONVERT(SUBSTR(a.po_no, 2, 11), INT)) as po_no, b.style_nm,(CONCAT(SUBSTRING(a.work_dt,1,4),'-',SUBSTRING(a.work_dt,5,2) ,'-',SUBSTRING(a.work_dt,7,2)) ) as work_dt, "
            //+ " sum(a.check_qty) as qc_qty, "
            //+ " sum(a.ok_qty) as ok_qty, "
            //+ " (sum(a.check_qty)-sum(a.ok_qty)) as def_qty "
            //+ "  from w_product_qc as a JOIN d_style_info AS b ON a.style_no = b.style_no  "
            //+ " WHERE ('" + style_no + "'='' OR a.style_no LIKE '%" + style_no + "%')"
            //+ " AND (Convert(substring(a.work_dt, 1, 8), DATE) >= Convert('" + start + "', DATE)  OR '" + start + "'='')"
            //+ " AND (Convert(substring(a.work_dt, 1, 8), DATE) <= Convert('" + end + "', DATE)  OR '" + end + "'='') group by a.fo_no";

            //StringBuilder sql_info = new StringBuilder();

            //sql_info.Append(" SELECT a.item_vcd,a.pq_no,a.pqno,Date_format(a.work_dt, '%Y-%m-%d') work_dt,a.check_qty AS qc_qty,a.ok_qty,a.ml_no, ");
            //sql_info.Append("       (a.check_qty - a.ok_qty) AS defect_qty, ");
            //sql_info.Append("       IF(a.ok_qty = 0, 0, Round((a.check_qty - a.ok_qty)/a.check_qty * 100, 2)) defect_qty_qc_rate, ");
            //sql_info.Append("       IF(a.ok_qty = 0, 0, Round(a.ok_qty/a.check_qty * 100, 2)) ok_qty_qc_rate ");
            //sql_info.Append(" FROM w_product_qc AS a ");
            //sql_info.Append("WHERE ('" + ml_no + "' = '' OR a.ml_no LIKE '%" + ml_no + "%' ) ");
            //sql_info.Append("       AND ('" + start + "' = '' OR Date_format(a.work_dt, '%Y-%m-%d') >= Date_format('" + start + "', '%Y-%m-%d') )");
            //sql_info.Append("       AND ('" + end + "'= ''  OR Date_format(a.work_dt, '%Y-%m-%d') <= Date_format('" + end + "', '%Y-%m-%d') )");
            //var data_info = db.Database.SqlQuery<Get_table_info_OQC_D_Model>(sql_info.ToString()).ToList();

            var data_info = await _iQMSService.GetListProductQC(start, end, ml_no);


            if (data_info.Count() == 0)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
            //var sql = " select a.pq_no,a.style_no,a.fo_no,b.check_value ,a.line_no, a.bom_no,  " +
            //" sum(b.check_qty) as qc_qty , (CONCAT(SUBSTRING(a.work_dt,1,4),'-',SUBSTRING(a.work_dt,5,2) ,'-',SUBSTRING(a.work_dt,7,2)) ) as work_dt " +
            //" from w_product_qc as a  " +
            //" left join w_product_qc_value as b on a.pq_no=b.pq_no and a.item_vcd=b.item_vcd   " +

            //" where ('" + style_no + "'='' or a.style_no like '%" + style_no + "%') " +

            //" AND (Convert(substring(a.work_dt, 1, 8), DATE) >= Convert('" + start + "', DATE)  OR '" + start + "'='')" +
            //" AND (Convert(substring(a.work_dt, 1, 8), DATE) <= Convert('" + end + "', DATE)  OR '" + end + "'='')" +
            //"  group by a.fo_no,b.check_id,b.check_cd,b.check_value ";

            //var sql = " select a.pq_no,a.style_no,a.fo_no,b.check_value ,a.line_no, a.bom_no, CONCAT(SUBSTR(a.po_no, 1, 1), CONVERT(SUBSTR(a.po_no, 2, 11), INT)) as po_no, c.style_nm, " +
            //" sum(b.check_qty) as qc_qty , (CONCAT(SUBSTRING(a.work_dt,1,4),'-',SUBSTRING(a.work_dt,5,2) ,'-',SUBSTRING(a.work_dt,7,2)) ) as work_dt " +
            //" from w_product_qc as a  " +
            //" left join w_product_qc_value as b on a.pq_no=b.pq_no and a.item_vcd=b.item_vcd LEFT JOIN d_style_info c ON a.style_no = c.style_no  " +

            //" where ('" + style_no + "'='' or a.style_no like '%" + style_no + "%') " +

            //" AND (Convert(substring(a.work_dt, 1, 8), DATE) >= Convert('" + start + "', DATE)  OR '" + start + "'='')" +
            //" AND (Convert(substring(a.work_dt, 1, 8), DATE) <= Convert('" + end + "', DATE)  OR '" + end + "'='')" +
            //" group by a.fo_no,b.check_id,b.check_cd,b.check_value ";

            //StringBuilder sql_detail = new StringBuilder();

            //sql_detail.Append("SELECT b.pqhno,b.pq_no,b.check_value,SUM(b.check_qty) AS qc_qty,a.item_vcd,a.ml_no,Date_format(a.work_dt, '%Y-%m-%d') work_dt ");
            //sql_detail.Append("FROM w_product_qc a ");
            //sql_detail.Append("JOIN w_product_qc_value b  ON a.pq_no=b.pq_no AND a.item_vcd=b.item_vcd ");
            //sql_detail.Append("WHERE ('" + ml_no + "' = '' OR a.ml_no LIKE '%" + ml_no + "%' ) ");
            //sql_detail.Append("       AND ('" + start + "' = '' OR Date_format(a.work_dt, '%Y-%m-%d') >= Date_format('" + start + "', '%Y-%m-%d') )");
            //sql_detail.Append("       AND ('" + end + "'= ''  OR Date_format(a.work_dt, '%Y-%m-%d') <= Date_format('" + end + "', '%Y-%m-%d') )");
            //sql_detail.Append("group by b.check_id,b.check_cd,b.check_value;");

            //var model = db.Database.SqlQuery<myChart_error_Item_Model_OQC>(sql_detail.ToString()).ToList();
            var model = await _iQMSService.GetListProductQCModel(start, end, ml_no);

            var data = new List<myChart_error_Model_OQC>();

            string fo_no = "";

            string total_defect_qty = "";

            foreach (var item in model)
            {
                if (fo_no != item.pq_no)
                {
                    fo_no = item.pq_no;

                    var error = new myChart_error_Model_OQC();
                    error.ml_no = item.ml_no;
                    error.work_dt = item.work_dt;
                    error.pq_no = item.pq_no;

                    var dsdsds = data_info.Where(x => x.ml_no == item.ml_no).ToList();
                    if (dsdsds.Count == 0)
                    {
                        total_defect_qty = "0";
                    }
                    else
                    {
                        total_defect_qty = dsdsds.FirstOrDefault().defect_qty;
                    }
                    //var total_defect_qty = data_info.Where(x => x.fo_no == item.fo_no).FirstOrDefault().def_qty;
                    error.total_def_qty = total_defect_qty;

                    var error_item = new myChart_error_Item_Model_OQC();
                    error_item = item;
                    error.Error = new List<myChart_error_Item_Model_OQC>();
                    error.Error.Add(error_item);
                    data.Add(error);
                }
                else
                {
                    var subject = data.FirstOrDefault(x => x.pq_no == item.pq_no);

                    var error_item = new myChart_error_Item_Model_OQC();
                    error_item = item;
                    subject.Error.Add(error_item);
                }
            }

            foreach (var item in data)
            {
                item.datasets = "";
                item.labels = "";
                item.datasets_pie = "";
                foreach (var item1 in item.Error)
                {
                    item.datasets += item1.qc_qty + ",";

                    item.labels += "'" + item1.check_value + "',";

                    item.datasets_pie += "'" + item1.qc_qty + "',";
                }
                item.chart_info = data_info.ToList();
            }

            return PartialView(data);
        }

        public class myChart_error_Item_Model_OQC
        {
            public int pqhno { get; set; }
            public string item_vcd { get; set; }
            public string ml_no { get; set; }
            public string pq_no { get; set; }
            public string work_dt { get; set; }
            public string check_subject { get; set; }
            public string check_value { get; set; }

            //public string style_no { get; set; }
            public int qc_qty { get; set; }
        }

        public class myChart_error_Model_OQC
        {
            public string total_def_qty { get; set; }
            public string work_dt { get; set; }
            public string check_subject { get; set; }
            public string ml_no { get; set; }

            public string pq_no { get; set; }

            //public string bom_no { get; set; }
            //public string line_no { get; set; }
            //public string po_no { get; set; }
            //public string style_nm { get; set; }
            [AllowHtml]
            public string datasets { get; set; }

            [AllowHtml]
            public string labels { get; set; }

            [AllowHtml]
            public string datasets_pie { get; set; }

            public List<myChart_error_Item_Model_OQC> Error { get; set; }
            public List<Get_table_info_OQC_D_Model> chart_info { get; set; }
        }

        #endregion OQC

        #region PQC_Dash

        public ActionResult PQCDash()
        {
            return SetLanguage("");
        }

        public ActionResult getcolum_proqc2(string fq_no)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select a.fq_no, b.fqhno,a.oldhno,e.check_subject,b.check_value ,SUM(b.check_qty) as qc_qty, CONCAT(a.process_no,'-',a.prounit_cd) as process \n");
            varname1.Append("from m_facline_qc as a \n");
            varname1.Append("left join m_facline_qc_value as b on a.fq_no=b.fq_no and a.item_vcd=b.item_vcd \n");
            varname1.Append("left join qc_item_mt as c on b.item_vcd=c.item_vcd \n");
            varname1.Append("left join qc_itemcheck_dt as d on c.item_vcd=d.item_vcd and b.check_value=d.check_name \n");
            varname1.Append("join qc_itemcheck_mt as e on d.item_vcd=e.item_vcd and d.check_id=e.check_id \n");
            varname1.Append("where a.fq_no='" + fq_no + "' \n");
            varname1.Append("group by a.fq_no,a.oldhno,d.item_vcd,d.check_id,d.check_cd");

            var data = new DataTable();
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                db.Database.Connection.Open();
                cmd.CommandText = varname1.ToString();
                using (var reader = cmd.ExecuteReader())
                {
                    data.Load(reader);
                }
            }
            db.Database.Connection.Close();
            var result = GetJsonPersons(data);
            return result;
        }

        public ActionResult getcolum_proqc(string fq_no)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select a.fq_no, b.fqhno,a.oldhno,e.check_subject,b.check_value ,SUM(b.check_qty) as qc_qty, CONCAT(a.process_no,'-',a.prounit_cd) as process \n");
            varname1.Append("from m_facline_qc as a \n");
            varname1.Append("left join m_facline_qc_value as b on a.fq_no=b.fq_no and a.item_vcd=b.item_vcd \n");
            varname1.Append("left join qc_item_mt as c on b.item_vcd=c.item_vcd \n");
            varname1.Append("left join qc_itemcheck_dt as d on c.item_vcd=d.item_vcd and b.check_value=d.check_name \n");
            varname1.Append("join qc_itemcheck_mt as e on d.item_vcd=e.item_vcd and d.check_id=e.check_id \n");
            varname1.Append("where a.fq_no='" + fq_no + "' \n");
            varname1.Append("group by a.fq_no,a.oldhno,d.item_vcd,d.check_id,d.check_cd");

            var data = new DataTable();
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                db.Database.Connection.Open();
                cmd.CommandText = varname1.ToString();
                using (var reader = cmd.ExecuteReader())
                {
                    data.Load(reader);
                }
            }
            db.Database.Connection.Close();
            var result = GetJsonPersons(data);
            return result;
        }

        public ActionResult gettable_proqc()
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select a.fq_no, b.fqhno,a.oldhno,e.check_subject,b.check_value ,(select sum(check_qty) as qc from m_facline_qc_value where b.check_value=check_value) as qc_qty, CONCAT(a.process_no,'-',a.prounit_cd) as process \n");
            varname1.Append("from m_facline_qc as a \n");
            varname1.Append("left join m_facline_qc_value as b on a.fq_no=b.fq_no and a.item_vcd=b.item_vcd \n");
            varname1.Append("left join qc_item_mt as c on b.item_vcd=c.item_vcd \n");
            varname1.Append("left join qc_itemcheck_dt as d on c.item_vcd=d.item_vcd and b.check_value=d.check_name \n");
            varname1.Append("join qc_itemcheck_mt as e on d.item_vcd=e.item_vcd and d.check_id=e.check_id \n");
            varname1.Append(" where Convert(substring(a.work_dt, 1, 8), DATE) = Convert(now(), DATE) \n");
            varname1.Append("group by a.fq_no,a.oldhno,d.item_vcd,d.check_id ");

            var data = new DataTable();
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                db.Database.Connection.Open();
                cmd.CommandText = varname1.ToString();
                using (var reader = cmd.ExecuteReader())
                {
                    data.Load(reader);
                }
            }
            db.Database.Connection.Close();
            var result = GetJsonPersons(data);
            return result;
        }

        public JsonResult searchStyle()
        {
            string bom_no = Request.QueryString["bom_no"].Trim(),
                style_no = Request.QueryString["style_no"].Trim(),
                model_no = Request.QueryString["model_no"].Trim();

            var sql = new StringBuilder();

            sql.Append(" SELECT b.style_no,b.style_nm,b.md_cd, a.*")
             .Append(" FROM  d_bom_info as a ")
             .Append(" LEFT JOIN d_style_info as b on a.style_no = b.style_no ")
             .Append(" Where  a.del_yn='N' and ('" + bom_no + "'='' OR a.bom_no like '%" + bom_no + "%' )")
             .Append("AND ('" + style_no + "'='' OR b.style_no like '%" + style_no + "%' )")
             .Append("AND ('" + model_no + "'='' OR b.md_cd like '%" + model_no + "%' )")
             //.Append("AND ('" + process_no_s + "'='' OR a.line_no like '%" + process_no_s + "%' )")

             .Append(" group by a.style_no ");

            var data = new DataTable();
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                db.Database.Connection.Open();
                cmd.CommandText = sql.ToString();
                using (var reader = cmd.ExecuteReader())
                {
                    data.Load(reader);
                }
            }
            db.Database.Connection.Close();
            var result = GetJsonPersons(data);
            return result;
        }

        public ActionResult searchProcessQcchart()
        {
            var start = Request["start"];
            var end = Request["end"];

            var process = Request["process"];

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT a.fq_no,b.fqhno, a.oldhno,e.check_subject, b.check_value, \n");
            varname1.Append("(SELECT sum(check_qty) AS qc \n");
            varname1.Append("   FROM m_facline_qc_value \n");
            varname1.Append("   WHERE b.check_value=check_value) AS qc_qty, \n");
            varname1.Append("       CONCAT(a.process_no, '-', a.prounit_cd) AS process \n");
            varname1.Append("FROM m_facline_qc AS a \n");
            varname1.Append("LEFT JOIN m_facline_qc_value AS b ON a.fq_no=b.fq_no \n");
            varname1.Append("AND a.item_vcd=b.item_vcd \n");
            varname1.Append("LEFT JOIN qc_item_mt AS c ON b.item_vcd=c.item_vcd \n");
            varname1.Append("LEFT JOIN qc_itemcheck_dt AS d ON c.item_vcd=d.item_vcd \n");
            varname1.Append("AND b.check_value=d.check_name \n");
            varname1.Append("JOIN qc_itemcheck_mt AS e ON d.item_vcd=e.item_vcd \n");
            varname1.Append("AND d.check_id=e.check_id \n");
            varname1.Append("WHERE ('" + process + "'='' OR (CONCAT(a.process_no, '-', a.prounit_cd)) LIKE '%" + process + "%') \n");
            varname1.Append("  AND (Convert(substring(a.work_dt, 1, 8), DATE) >= Convert('" + start + "', DATE)  OR '" + start + "'='') \n");
            varname1.Append("  AND (Convert(substring(a.work_dt, 1, 8), DATE) <= Convert('" + end + "', DATE) OR '" + end + "'='') \n");
            varname1.Append("GROUP BY a.fq_no, a.oldhno, d.item_vcd,d.check_id");

            var data = new DataTable();
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                db.Database.Connection.Open();
                cmd.CommandText = varname1.ToString();
                using (var reader = cmd.ExecuteReader())
                {
                    data.Load(reader);
                }
            }
            db.Database.Connection.Close();
            var result = GetJsonPersons(data);
            return result;
        }

        public ActionResult Get_table_PQC_info()
        {
            try
            {
                var start = Request.QueryString["start"];
                var end = Request.QueryString["end"];
                var prounit_cd = Request.QueryString["prounit_cd"];

                var sql = " select a.fq_no, " +
                            " sum(a.check_qty) as qc_qty, " +
                            " sum(a.ok_qty) as ok_qty, " +
                            " (sum(a.check_qty)-sum(a.ok_qty)) as def_qty, " +
                            " CONCAT(a.process_no,'-',a.prounit_cd) as process " +
                            " from m_facline_qc as a  " +
                            " WHERE ('" + prounit_cd + "'='' OR (CONCAT(a.process_no, '-', a.prounit_cd)) LIKE '%" + prounit_cd + "%')" +
                            " AND (Convert(substring(a.work_dt, 1, 8), DATE) >= Convert('" + start + "', DATE)  OR '" + start + "'='')" +
                           " AND (Convert(substring(a.work_dt, 1, 8), DATE) <= Convert('" + end + "', DATE)  OR '" + end + "'='')";

                var data = db.Database.SqlQuery<Get_table_info_PQC_D_Model>(sql).FirstOrDefault();

                if (string.IsNullOrEmpty(data.fq_no))
                {
                    return Json(new { result = false }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { result = true, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public class Get_table_info_PQC_D_Model
        {
            //public string process { get; set; }
            //public string process_no { get; set; }
            //public string prounit_cd { get; set; }
            //public string fo_no { get; set; }
            public string fq_no { get; set; }

            public string work_dt { get; set; }
            public string mt_no { get; set; }

            public string ok_qty { get; set; }
            public string def_qty { get; set; }
            public string qc_qty { get; set; }
            public string ml_no { get; set; }
        }

        public async Task<ActionResult> PartialView_PQCDash_Bar()
        {
            try
            {
                var ml_no = Request["ml_no"] == null ? "" : Request["ml_no"].Trim();
                var start = Request.QueryString["start"];
                var end = Request.QueryString["end"];

                #region MyRegion

                //info
                //var info = " select a.fq_no,CONCAT(SUBSTR(a.fo_no, 1, 1), CONVERT(SUBSTR(a.fo_no, 2, 11), INT)) as fo_no,a.fo_no as fo_no1, a.process_no, a.prounit_cd ,(CONCAT(SUBSTRING(a.work_dt,1,4),'-',SUBSTRING(a.work_dt,5,2) ,'-',SUBSTRING(a.work_dt,7,2)) ) as work_dt, " +
                //            " sum(a.check_qty) as qc_qty, " +
                //            " sum(a.ok_qty) as ok_qty, " +
                //            " (sum(a.check_qty)-sum(a.ok_qty)) as def_qty, " +
                //            " CONCAT(a.process_no,'-',a.prounit_cd) as process, " +
                //            " (SELECT mt_no FROM d_bom_part_info WHERE bom_no = (SELECT bom_no FROM d_line_info WHERE line_no = a.line_no) AND part_no = a.process_no) AS mt_no,a.ml_no " +
                //            " from m_facline_qc as a  " +
                //            //" WHERE ('" + prounit_cd + "'='' OR (CONCAT(a.process_no, '-', a.prounit_cd)) LIKE '%" + prounit_cd + "%')" +
                //            " WHERE (Convert(substring(a.work_dt, 1, 8), DATE) >= Convert('" + start + "', DATE)  OR '" + start + "'='')" +
                //           " AND (Convert(substring(a.work_dt, 1, 8), DATE) <= Convert('" + end + "', DATE)  OR '" + end + "'='')" +
                //                " AND ('" + ml_no + "' = '' OR a.ml_no LIKE '%" + ml_no + "%') " +
                //            "  group by a.fo_no, a.process_no, a.prounit_cd  ";

                #endregion MyRegion

                //StringBuilder info = new StringBuilder();
                //info.Append("SELECT a.fq_no, ( Concat(Substring(a.work_dt, 1, 4), '-', Substring(a.work_dt, 5, 2), '-' , Substring(a.work_dt, 7, 2)) )AS work_dt, ");
                //info.Append("       SUM(a.check_qty) AS qc_qty, SUM(a.ok_qty) AS ok_qty, ");
                //info.Append("       (SUM(a.check_qty) - SUM(a.ok_qty) )AS def_qty,  a.ml_no ");
                //info.Append("FROM   m_facline_qc AS a ");
                //info.Append("WHERE  (Convert(Substring(a.work_dt, 1, 8), DATE) >= Convert('" + start + "', DATE) OR '" + start + "' = '' ) ");
                //info.Append("       AND (Convert(Substring(a.work_dt, 1, 8), DATE) <= Convert('" + end + "', DATE) OR '" + end + "' = '' ) ");
                //info.Append("       AND ('" + ml_no + "' = '' OR a.ml_no LIKE '%" + ml_no + "%')  ");
                //info.Append("GROUP  BY a.fq_no ");

                //var data_info = db.Database.SqlQuery<Get_table_info_PQC_D_Model>(info.ToString());
                var data_info = await _iQMSService.GetListFaclineQC(start,end,ml_no);

                #region MyRegion

                //var sql = " SELECT a.fq_no,b.fqhno, a.ml_no,a.oldhno,e.check_subject, b.check_value,CONCAT(SUBSTR(a.fo_no, 1, 1), CONVERT(SUBSTR(a.fo_no, 2, 11), INT)) as fo_no,a.prounit_cd,a.process_no, (CONCAT(SUBSTRING(a.work_dt,1,4),'-',SUBSTRING(a.work_dt,5,2) ,'-',SUBSTRING(a.work_dt,7,2)) ) as work_dt," +
                //" sum(b.check_qty) as qc_qty,CONCAT(a.process_no, '-', a.prounit_cd) AS process " +
                //" FROM m_facline_qc AS a left JOIN m_facline_qc_value AS b ON a.fq_no=b.fq_no AND a.item_vcd=b.item_vcd  " +
                ////" left join qc_item_mt as c on b.item_vcd=c.item_vcd  " +
                ////" left join qc_itemcheck_dt as d on c.item_vcd=d.item_vcd and b.check_value=d.check_name  " +
                //" LEFT JOIN qc_itemcheck_mt AS e ON b.item_vcd=e.item_vcd AND b.check_id=e.check_id " +
                ////" WHERE ('" + prounit_cd + "'='' OR (CONCAT(a.process_no, '-', a.prounit_cd)) LIKE '%" + prounit_cd + "%') " +
                //"  WHERE (Convert(substring(a.work_dt, 1, 8), DATE) >= Convert('" + start + "', DATE)  OR '" + start + "'='') " +
                //"  AND (Convert(substring(a.work_dt, 1, 8), DATE) <= Convert('" + end + "', DATE) OR '" + end + "'='')" +
                //               " AND ('" + ml_no + "' = '' OR a.ml_no LIKE '%" + ml_no + "%') " +
                //"  group by a.fo_no, a.process_no,a.prounit_cd, b.check_id, b.check_cd, b.check_value  ";

                #endregion MyRegion

                //StringBuilder varname1 = new StringBuilder();
                //varname1.Append("SELECT a.fq_no,b.fqhno, a.ml_no,e.check_subject, b.check_value, ");
                //varname1.Append("	(CONCAT(SUBSTRING(a.work_dt,1,4),'-',SUBSTRING(a.work_dt,5,2) ,'-',SUBSTRING(a.work_dt,7,2)) ) as work_dt, ");
                //varname1.Append("	SUM(b.check_qty) as qc_qty ");
                //varname1.Append("FROM m_facline_qc AS a ");
                //varname1.Append("		left JOIN m_facline_qc_value AS b ON a.fq_no=b.fq_no AND a.item_vcd=b.item_vcd ");
                //varname1.Append("		left join qc_item_mt as c on b.item_vcd=c.item_vcd ");
                ////varname1.Append("		left join qc_itemcheck_dt as d on c.item_vcd=d.item_vcd and b.check_value=d.check_name ");14/10
                //varname1.Append("		left join qc_itemcheck_dt as d on c.item_vcd=d.item_vcd AND b.check_cd=d.check_cd AND b.check_id=d.check_id	");
                //varname1.Append("		left join qc_itemcheck_mt as e on d.item_vcd=e.item_vcd and d.check_id=e.check_id ");
                //varname1.Append("WHERE (Convert(substring(a.work_dt, 1, 8), DATE) >= Convert('" + start + "', DATE)  OR '" + start + "'='') ");
                //varname1.Append("	AND (Convert(substring(a.work_dt, 1, 8), DATE) <= Convert('" + end + "', DATE) OR '" + end + "'='') ");
                //varname1.Append("	AND ('" + ml_no + "' = '' OR a.ml_no LIKE '%" + ml_no + "%' ) ");
                //varname1.Append("group by a.fq_no,b.check_id, b.check_cd, b.check_value");

                //var model = db.Database.SqlQuery<myChart_error_Item_Model_PQC>(varname1.ToString()).ToList();
                var model = await _iQMSService.GetListFaclineQCModel(start, end, ml_no);

                //StringBuilder varname2 = new StringBuilder(sql);
                //var model = new InitMethods().ConvertDataTableToList<myChart_error_Item_Model_PQC>(varname2);

                var data = new List<myChart_error_Model_PQC>();

                string check_subject = "";
                string fo_no = "";

                string fq_no = "";

                string total_defect_qty = "";

                foreach (var item in model)
                {
                    if (fo_no != item.fq_no)
                    {
                        check_subject = item.check_subject;
                        fo_no = item.fq_no;
                        //process_no = item.process_no;
                        //prounit_cd = item.prounit_cd;
                        fq_no = item.fq_no;
                        var error = new myChart_error_Model_PQC();
                        error.check_subject = item.check_subject;
                        error.fq_no = item.fq_no;
                        //error.process_no = item.process_no;
                        //error.prounit_cd = item.prounit_cd;
                        error.mt_no = item.ml_no;
                        error.ml_no = item.ml_no;

                        var dsdsds = data_info.Where(x => x.fq_no == item.fq_no).ToList();
                        if (dsdsds.Count == 0)
                        {
                            total_defect_qty = "0";
                        }
                        else
                        {
                            total_defect_qty = dsdsds.FirstOrDefault().def_qty;
                        }

                        //var total_defect_qty = data_info.Where(x => x.fo_no == item.fo_no).FirstOrDefault().def_qty;
                        error.total_def_qty = total_defect_qty;

                        var error_item = new myChart_error_Item_Model_PQC();
                        error_item = item;
                        error.Error = new List<myChart_error_Item_Model_PQC>();
                        error.Error.Add(error_item);

                        data.Add(error);
                    }
                    else
                    {
                        var subject = data.FirstOrDefault(x => x.fq_no == item.fq_no);

                        var error_item = new myChart_error_Item_Model_PQC();
                        error_item = item;
                        subject.Error.Add(error_item);
                    }
                }

                foreach (var item in data)
                {
                    item.datasets = "";
                    item.labels = "";
                    item.datasets_pie = "";
                    foreach (var item1 in item.Error)
                    {
                        item.datasets += item1.qc_qty + ",";

                        item.labels += "'" + item1.check_value + "',";

                        item.datasets_pie += "'" + item1.qc_qty + "',";
                    }

                    item.chart_info = data_info.ToList();
                }

                return PartialView(data);
            }
            catch (Exception ex)
            {
                return PartialView("~/DashBoardQC/PQCDash", new
                {
                    result = false,
                    message= Constant.ErrorSystem
                });
            }
        }

        public async Task<ActionResult> PartialView_PQCDash_Pie()
        {
            try
            {
                var ml_no = Request["ml_no"] == null ? "" : Request["ml_no"].Trim();
                var start = Request.QueryString["start"];
                var end = Request.QueryString["end"];

                #region MyRegion

                //info
                //var info2 = " select a.fq_no,  CONCAT(SUBSTR(a.fo_no, 1, 1), CONVERT(SUBSTR(a.fo_no, 2, 11), INT)) as fo_no, a.process_no, a.prounit_cd ,(CONCAT(SUBSTRING(a.work_dt,1,4),'-',SUBSTRING(a.work_dt,5,2) ,'-',SUBSTRING(a.work_dt,7,2)) ) as work_dt, " +
                //            " sum(a.check_qty) as qc_qty, " +
                //            " sum(a.ok_qty) as ok_qty, " +
                //            " (sum(a.check_qty)-sum(a.ok_qty)) as def_qty, " +
                //            " CONCAT(a.process_no,'-',a.prounit_cd) as process, " +
                //            " (SELECT mt_no FROM d_bom_part_info WHERE bom_no = (SELECT bom_no FROM d_line_info WHERE line_no = a.line_no) AND part_no = a.process_no) AS mt_no, a.ml_no " +
                //            " from m_facline_qc as a  " +
                //            //" WHERE ('" + prounit_cd + "'='' OR (CONCAT(a.process_no, '-', a.prounit_cd)) LIKE '%" + prounit_cd + "%')" +
                //            " WHERE (Convert(substring(a.work_dt, 1, 8), DATE) >= Convert('" + start + "', DATE)  OR '" + start + "'='')" +
                //           " AND (Convert(substring(a.work_dt, 1, 8), DATE) <= Convert('" + end + "', DATE)  OR '" + end + "'='')" +
                //                                  " AND ('" + ml_no + "' = '' OR a.ml_no LIKE '%" + ml_no + "%') " +
                //            "  group by a.fo_no, a.process_no, a.prounit_cd  ";
                //String info = ""
                //+ "SELECT a.fq_no, Concat(Substr(a.fo_no, 1, 1), Convert(Substr(a.fo_no, 2, 11), INT)) AS fo_no, "
                //+ "a.fo_no AS fo_no1, a.process_no, a.prounit_cd, "
                //+ "( Concat(Substring(a.work_dt, 1, 4), '-', Substring(a.work_dt, 5, 2), '-' , Substring(a.work_dt, 7, 2)) )AS work_dt, "
                //+ "SUM(a.check_qty) AS qc_qty, SUM(a.ok_qty) AS ok_qty, "
                //+ "       (SUM(a.check_qty) - SUM(a.ok_qty) )AS def_qty, "
                //+ "       Concat(a.process_no, '-', a.prounit_cd) AS process, "
                //+ "       (SELECT mt_no FROM   d_bom_part_info WHERE  bom_no = (SELECT bom_no FROM   d_line_info WHERE  line_no = a.line_no) AND part_no = a.process_no) AS  mt_no, a.ml_no "
                //+ " FROM   m_facline_qc AS a "
                //+ " WHERE  (Convert(Substring(a.work_dt, 1, 8), DATE) >= Convert('" + start + "'', DATE) OR '" + start + "' = '' ) "
                //+ "       AND ( Convert(Substring(a.work_dt, 1, 8), DATE) <= Convert('" + end + "', DATE) OR '" + end + "' = '' ) "
                //+ "      AND ('" + ml_no + "' = '' OR a.ml_no LIKE '%" + ml_no + "%') "
                //+ " GROUP  BY a.fo_no, a.process_no, a.prounit_cd ";

                #endregion MyRegion

                //StringBuilder info = new StringBuilder();
                //info.Append("SELECT a.fq_no,  ( Concat(Substring(a.work_dt, 1, 4), '-', Substring(a.work_dt, 5, 2), '-' , Substring(a.work_dt, 7, 2)) ) AS work_dt, ");
                //info.Append("       SUM(a.check_qty) AS qc_qty, SUM(a.ok_qty) AS ok_qty, ");
                //info.Append("       (SUM(a.check_qty) - SUM(a.ok_qty) )AS def_qty, a.ml_no ");
                //info.Append("FROM   m_facline_qc AS a ");
                //info.Append("WHERE  (Convert(Substring(a.work_dt, 1, 8), DATE) >= Convert('" + start + "', DATE) OR '" + start + "' = '' ) ");
                //info.Append("       AND (Convert(Substring(a.work_dt, 1, 8), DATE) <= Convert('" + end + "', DATE) OR '" + end + "' = '' ) ");
                //info.Append("       AND ('" + ml_no + "' = '' OR a.ml_no LIKE '%" + ml_no + "%')  ");
                //info.Append("GROUP  BY a.fq_no ");
                //var data_info = db.Database.SqlQuery<Get_table_info_PQC_D_Model>(info.ToString());
                var data_info = await _iQMSService.GetListFaclineQC(start, end, ml_no);

                #region MyRegion

                //StringBuilder varname1 = new StringBuilder(info);
                //var data_info = new InitMethods().ConvertDataTableToList<Get_table_info_PQC_D_Model>(varname1);

                //info

                //var sql2 = " SELECT a.fq_no,b.fqhno, a.ml_no, a.oldhno,e.check_subject, b.check_value,a.fo_no,a.prounit_cd,a.process_no, (CONCAT(SUBSTRING(a.work_dt,1,4),'-',SUBSTRING(a.work_dt,5,2) ,'-',SUBSTRING(a.work_dt,7,2)) ) as work_dt," +
                //  " sum(b.check_qty) as qc_qty,CONCAT(a.process_no, '-', a.prounit_cd) AS process, "
                //  + " (select mt_no from d_bom_part_info where bom_no = (select bom_no from d_line_info where line_no = a.line_no) and part_no = a.process_no) AS mt_no"
                //  + " FROM m_facline_qc AS a left JOIN m_facline_qc_value AS b ON a.fq_no=b.fq_no AND a.item_vcd=b.item_vcd  "
                //  + " left join qc_item_mt as c on b.item_vcd=c.item_vcd  "
                //  + " left join qc_itemcheck_dt as d on c.item_vcd=d.item_vcd and b.check_value=d.check_name  "
                //  + " left join qc_itemcheck_mt as e on d.item_vcd=e.item_vcd and d.check_id=e.check_id  "
                //  ////" WHERE ('" + prounit_cd + "'='' OR (CONCAT(a.process_no, '-', a.prounit_cd)) LIKE '%" + prounit_cd + "%') " +
                //  + "  WHERE (Convert(substring(a.work_dt, 1, 8), DATE) >= Convert('" + start + "', DATE)  OR '" + start + "'='') "
                //  + "  AND (Convert(substring(a.work_dt, 1, 8), DATE) <= Convert('" + end + "', DATE) OR '" + end + "'='')"
                //  + " AND ('" + ml_no + "' = '' OR a.ml_no LIKE '%" + ml_no + "%') "

                //  + "  group by a.fo_no, a.process_no,a.prounit_cd, b.check_id, b.check_cd, b.check_value  ";

                #endregion MyRegion

                //StringBuilder varname1 = new StringBuilder();
                //varname1.Append("SELECT ");
                //varname1.Append("	a.fq_no,b.fqhno, a.ml_no, e.check_subject, b.check_value, ");
                //varname1.Append("	(CONCAT(SUBSTRING(a.work_dt,1,4),'-',SUBSTRING(a.work_dt,5,2) ,'-',SUBSTRING(a.work_dt,7,2)) ) as work_dt, ");
                //varname1.Append("	SUM(b.check_qty) as qc_qty ");
                //varname1.Append("FROM m_facline_qc AS a ");
                //varname1.Append("		left JOIN m_facline_qc_value AS b ON a.fq_no=b.fq_no AND a.item_vcd=b.item_vcd ");
                //varname1.Append("		left join qc_item_mt as c on b.item_vcd=c.item_vcd ");
                ////varname1.Append("		left join qc_itemcheck_dt as d on c.item_vcd=d.item_vcd and b.check_value=d.check_name "); 14/10
                //varname1.Append("		left join qc_itemcheck_dt as d on c.item_vcd=d.item_vcd AND b.check_cd=d.check_cd AND b.check_id=d.check_id	");
                //varname1.Append("		left join qc_itemcheck_mt as e on d.item_vcd=e.item_vcd and d.check_id=e.check_id ");
                //varname1.Append("WHERE (Convert(substring(a.work_dt, 1, 8), DATE) >= Convert('" + start + "', DATE)  OR '" + start + "'='') ");
                //varname1.Append("	AND (Convert(substring(a.work_dt, 1, 8), DATE) <= Convert('" + end + "', DATE) OR '" + end + "'='') ");
                //varname1.Append("	AND ('" + ml_no + "' = '' OR a.ml_no LIKE '%" + ml_no + "%' ) ");
                //varname1.Append("group by  b.check_id, b.check_cd, b.check_value");
                //var model = db.Database.SqlQuery<myChart_error_Item_Model_PQC>(varname1.ToString()).ToList();
                //var data = new List<myChart_error_Model_PQC>();
                var model = await _iQMSService.GetListFaclineQCModel(start, end, ml_no);
                var data = new List<myChart_error_Model_PQC>();
                string check_subject = "";
                string fo_no = "";
                string fq_no = "";
                //string process_no = "";
                string total_defect_qty = "";
                foreach (var item in model)
                {
                    if (fo_no != item.fq_no)
                    {
                        check_subject = item.check_subject;
                        fo_no = item.fq_no;
                        fq_no = item.fq_no;
                        //process_no = item.process_no;
                        //prounit_cd = item.prounit_cd;
                        var error = new myChart_error_Model_PQC();
                        error.check_subject = item.check_subject;
                        error.fq_no = item.fq_no;
                        //error.process_no = item.process_no;
                        //error.prounit_cd = item.prounit_cd;
                        error.mt_no = item.mt_no;
                        error.ml_no = item.ml_no;

                        //fo_no = item.ml_no;
                        //process_no = item.process_no;
                        //prounit_cd = item.prounit_cd;

                        var dsdsds = data_info.Where(x => x.fq_no == item.fq_no).ToList();
                        if (dsdsds.Count == 0)
                        {
                            total_defect_qty = "0";
                        }
                        else
                        {
                            total_defect_qty = dsdsds.FirstOrDefault().def_qty;
                        }
                        //var total_defect_qty = data_info.Where(x => x.fo_no == item.fo_no).FirstOrDefault().def_qty;
                        error.total_def_qty = total_defect_qty;

                        var error_item = new myChart_error_Item_Model_PQC();
                        error_item = item;
                        error.Error = new List<myChart_error_Item_Model_PQC>();
                        error.Error.Add(error_item);

                        data.Add(error);
                    }
                    else
                    {
                        var subject = data.FirstOrDefault(x => x.fq_no == item.fq_no);

                        var error_item = new myChart_error_Item_Model_PQC();
                        error_item = item;
                        subject.Error.Add(error_item);
                    }
                }

                foreach (var item in data)
                {
                    item.datasets = "";
                    item.labels = "";
                    item.datasets_pie = "";
                    foreach (var item1 in item.Error)
                    {
                        item.datasets += item1.qc_qty + ",";

                        item.labels += "'" + item1.check_value + "',";

                        item.datasets_pie += "'" + item1.qc_qty + "',";
                    }
                    item.chart_info = data_info.ToList();
                }

                return PartialView(data);
            }
            catch (Exception ex)
            {
                return PartialView("~/DashBoardQC/PQCDash", new
                {
                    result = false,
                    message= Constant.ErrorSystem
                });
            }
        }

        public class myChart_error_Item_Model_PQC
        {
            public string fqhno { get; set; }
            public string fq_no { get; set; }
            public string check_subject { get; set; }
            public string check_value { get; set; }
            public string qc_qty { get; set; }
            public string work_dt { get; set; }

            //public string fo_no { get; set; }
            //public string prounit_cd { get; set; }
            //public string process_no { get; set; }
            public string mt_no { get; set; }

            public string total_def_qty { get; set; }

            public string ml_no { get; set; }
        }

        public class myChart_error_Model_PQC
        {
            public string total_def_qty { get; set; }
            public string mt_no { get; set; }
            public string fqhno { get; set; }
            public string fq_no { get; set; }
            public string work_dt { get; set; }

            ////public string fo_no { get; set; }
            //public string prounit_cd { get; set; }
            //public string process_no { get; set; }
            public string check_subject { get; set; }

            public string check_value { get; set; }
            public string qc_qty { get; set; }

            [AllowHtml]
            public string datasets { get; set; }

            [AllowHtml]
            public string labels { get; set; }

            [AllowHtml]
            public string datasets_pie { get; set; }

            public List<myChart_error_Item_Model_PQC> Error { get; set; }

            public List<Get_table_info_PQC_D_Model> chart_info { get; set; }

            public string ml_no { get; set; }
        }

        #endregion PQC_Dash

        public ActionResult ATM()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private void WriteHtmlTable<T>(IEnumerable<T> data, TextWriter output, List<string> labelList)
        {
            //Writes markup characters and text to an ASP.NET server control output stream. This class provides formatting capabilities that ASP.NET server controls use when rendering markup to clients.
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    //  Create a form to contain the List
                    Table table = new Table();
                    TableRow row = new TableRow();
                    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

                    // table header from IEnumerable
                    //foreach (PropertyDescriptor prop in props) {
                    //    TableHeaderCell hcell = new TableHeaderCell();
                    //    hcell.Text = prop.Name;
                    //    hcell.BackColor = System.Drawing.Color.Yellow;
                    //    row.Cells.Add(hcell);
                    //}

                    foreach (String label in labelList)
                    {
                        TableHeaderCell hcell = new TableHeaderCell();
                        hcell.Text = label;
                        //hcell.BackColor = System.Drawing.Color.Yellow;
                        hcell.Font.Bold = true;
                        row.Cells.Add(hcell);
                        row.BorderStyle = BorderStyle.Solid;
                    }
                    table.Rows.Add(row);

                    //  add each of the data item to the table
                    foreach (T item in data)
                    {
                        row = new TableRow();
                        foreach (PropertyDescriptor prop in props)
                        {
                            TableCell cell = new TableCell();
                            cell.Text = prop.Converter.ConvertToString(prop.GetValue(item));
                            //cell.BorderStyle = BorderStyle.Solid;
                            row.Cells.Add(cell);
                            row.BorderStyle = BorderStyle.Solid;
                        }
                        table.Rows.Add(row);
                    }

                    //  render the table into the htmlwriter
                    table.RenderControl(htw);

                    //  render the htmlwriter into the response
                    output.Write(sw.ToString());
                }
            }
        }
    }
}