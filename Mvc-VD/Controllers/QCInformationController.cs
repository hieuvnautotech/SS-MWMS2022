using Mvc_VD.Models;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models.Response;
using Mvc_VD.Services.Interface;
using Mvc_VD.Services.Interface.QMS;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mvc_VD.Controllers
{
    public class QCInformationController : BaseController
    {

        private Entities db = new Entities();
        private IQMSService _IQMSService;
        private readonly IhomeService _homeService;

        public QCInformationController(IQMSService IQMSService, IhomeService ihomeService)
        {
            _IQMSService = IQMSService;
            _homeService = ihomeService;
        }

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
        protected override JsonResult Json(object data, string contentType,
        Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }


        #region QMS_NG
        //public JsonResult GetDayInMonth()
        //{
        //    List<JqGrid> lstJqGrid = new List<JqGrid>();
        //    List<JqGrid2> lstJqGrid2 = new List<JqGrid2>();
        //    int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
        //    JqGrid Jq = new JqGrid();
        //    Jq.label = "FQ No";
        //    Jq.width = 100;
        //    Jq.align = "center";
        //    Jq.hidden = false;
        //    Jq.name = "fq_no";
        //    lstJqGrid.Add(Jq);
        //    Jq = new JqGrid();
        //    Jq.label = "Type";
        //    Jq.width = 100;
        //    Jq.align = "center";
        //    Jq.hidden = false;
        //    Jq.name = "total";
        //    lstJqGrid.Add(Jq);
        //    JqGrid2 Jq2 = new JqGrid2();
        //    Jq = new JqGrid();
        //    Jq.key = "sid";
        //    Jq.label = "N";
        //    Jq.name = "day_" + DateTime.Now.Day + "_n";
        //    Jq.width = 100;
        //    Jq.align = "right";
        //    lstJqGrid.Add(Jq);
        //    Jq = new JqGrid();
        //    Jq.key = "sid";
        //    Jq.label = "D";
        //    Jq.name = "day_" + DateTime.Now.Day + "_d";
        //    Jq.width = 100;
        //    Jq.align = "right";
        //    lstJqGrid.Add(Jq);
        //    Jq2.titleText = DateTime.Now.Year + "-" + (DateTime.Now.Month.ToString().Length == 1 ? ("0" + DateTime.Now.Month) : DateTime.Now.Month.ToString()) + "-" + (DateTime.Now.Day.ToString().Length == 1 ? ("0" + DateTime.Now.Day) : DateTime.Now.Day.ToString());
        //    Jq2.startColumnName = "day_" + DateTime.Now.Day + "_n";
        //    Jq2.numberOfColumns = 2;
        //    lstJqGrid2.Add(Jq2);
        //    return Json(new { data = lstJqGrid, group = lstJqGrid2 });
        //}
        [HttpGet]
        public async Task<JsonResult> GetQMSNG(string productCode, string end_date_ymd, string start_date_ymd, string at_no)
        {
            if (string.IsNullOrEmpty(productCode) || string.IsNullOrEmpty(at_no) || end_date_ymd == null)
                return null;
            var data = await _IQMSService.GetListClassificationNG(start_date_ymd, end_date_ymd, productCode, at_no);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //[HttpGet]
        //public async Task<JsonResult> GetQMSNG(string productCode, DateTime? date_ymd)
        //{
        //    if (string.IsNullOrEmpty(productCode) && date_ymd == null) return null;
        //  //  List<GetQMSNGModel> data = db.Database.SqlQuery<GetQMSNGModel>("CALL `ss_qms_ng`(@date_ymd,@productCode)", new MySqlParameter("@date_ymd", date_ymd), new MySqlParameter("@productCode", productCode)).ToList();
        //    var result = await _IQMSService.GetListDataQMSNG(productCode, date_ymd);
        //    List<GetQMSNGModel> data = result.ToList();
        //    Dictionary<string, object> total = new Dictionary<string, object>();
        //    Dictionary<string, object> ok = new Dictionary<string, object>();
        //    Dictionary<string, object> ng = new Dictionary<string, object>();
        //    int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
        //    foreach (var item in data)
        //    {
        //        int day = item.CreateOn.ToDateTime<DateTime?>().Day;
        //        if (item.Shift == "CN")
        //            item.Shift = "_n";
        //        else
        //            item.Shift = "_d";
        //            total["fq_no"] = item.fq_no;
        //            ng["fq_no"] = item.fq_no;
        //            ok["fq_no"] = item.fq_no;
        //            total["total"] = "Total";
        //            ng["total"] = "NG";
        //            ok["total"] = "OK";
        //            total["day_" + day + item.Shift] = item.Total.Format();
        //            ok["day_" + day + item.Shift] = item.OK.Format();
        //            ng["day_" + day + item.Shift] = item.NG.Format();
        //    }
        //    if (data.Count == 0)
        //    {
        //        return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(new { total = total, ok = ok, ng = ng }, JsonRequestBehavior.AllowGet);
        //}

        //Nát project rồi cho nát luôn
        public class GetQMSNGModel
        {
            public int? Total { get; set; }
            public int? NG { get; set; }
            public string Shift { get; set; }
            public int? OK { get; set; }
            public DateTime? CreateOn { get; set; }
            public string fq_no { get; set; }
        }
        public class JqGrid2
        {
            public string startColumnName { get; set; }
            public int numberOfColumns { get; set; }
            public string titleText { get; set; }
        }
        public class JqGrid
        {
            public string key { get; set; }
            public string label { get; set; }
            public string name { get; set; }
            public int width { get; set; }
            public string align { get; set; }
            public bool hidden { get; set; }
        }
        #endregion

        #region OQC_Infomation
        public ActionResult OQC_Infomation()
        {
            return View();
        }
        public ActionResult Getproduct_qc_info()
        {
            var sql = new StringBuilder();

            sql.Append(" select  a.fo_no, CONCAT(SUBSTR(a.fo_no, 1, 1), CONVERT(SUBSTR(a.fo_no, 2, 11), INT)) as fo_no_hien ,a.po_no,a.line_no,a.item_vcd, ")
                .Append(" (select style_no from d_style_info where a.style_no = style_no) as style_no,  ")
                .Append(" (select style_nm from d_style_info where a.style_no = style_no) as style_nm, a.bom_no, ")
                .Append(" (select lct_nm from lct_info where lct_cd = (select lct_cd from s_order_factory_info where a.fo_no = fo_no)) as lct_nm, ")
                .Append(" sum(a.check_qty) as check_qty, ")

                .Append(" (select sum(sked_qty) from m_order_facline_detail where a.fo_no = fo_no and a.process_no = process_no and a.prounit_cd = prounit_cd and a.line_no = line_no) as sked_qty, ")
                .Append(" CASE WHEN (select sum(sked_qty) from m_order_facline_detail where a.fo_no = fo_no and a.process_no = process_no and a.prounit_cd = prounit_cd and a.line_no = line_no) IS NULL OR (select sum(sked_qty) from m_order_facline_detail where a.fo_no = fo_no and a.process_no = process_no and a.prounit_cd = prounit_cd and a.line_no = line_no) IS NULL THEN CONCAT(ROUND(0,2),' %') ELSE CONCAT(ROUND((SUM(a.check_qty)/(select sum(sked_qty) from m_order_facline_detail where a.fo_no = fo_no and a.process_no = process_no and a.prounit_cd = prounit_cd and a.line_no = line_no))*100,2),' %')  END AS qc_rate, ")

                .Append(" SUM(a.ok_qty) AS ok_qty,")
                .Append(" CASE WHEN SUM(a.check_qty) IS NULL OR SUM(a.ok_qty) IS NULL THEN '0' ELSE (SUM(a.check_qty)-SUM(a.ok_qty)) END AS defective_qty,")
                .Append(" CASE WHEN SUM(a.check_qty) IS NULL OR (SUM(a.check_qty)-SUM(a.ok_qty)) < 0 OR SUM(a.ok_qty) IS NUll THEN CONCAT(ROUND(0,2),' %') ELSE CONCAT(ROUND(((SUM(a.check_qty)-SUM(a.ok_qty))/SUM(a.check_qty))*100,2),' %')  END AS defect_rate")
                .Append(" FROM w_product_qc AS a")

                .Append(" GROUP BY a.fo_no order by a.fo_no desc");

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

        public ActionResult Getdetaipr_qc_info(String item_vcd, String fo_no)
        {
            //var sql = new StringBuilder();

            //sql.Append("select b.pqhno,b.pq_no,a.oldd_no,e.check_subject,b.check_value ,(select sum(check_qty) as qc from w_product_qc_value where b.check_value=check_value) as qc_qty,a.style_no")
            //.Append(" from w_product_qc as a ")
            //.Append(" join w_product_qc_value as b on a.pq_no=b.pq_no and a.item_vcd=b.item_vcd   ")
            //.Append(" left join qc_item_mt as c on b.item_vcd=c.item_vcd ")
            //.Append(" left join qc_itemcheck_dt as d on c.item_vcd=d.item_vcd and b.check_value=d.check_name  ")
            //.Append(" left join qc_itemcheck_mt as e on d.item_vcd=e.item_vcd and d.check_id=e.check_id  ")
            //.Append(" where a.item_vcd='" + item_vcd + "' ")
            //.Append(" and a.fo_no='" + fo_no + "'")
            //.Append(" group by a.fo_no,b.item_vcd,b.check_id,b.check_cd ");

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT SUM(b.check_qty) AS qc_qty, \n");
            varname1.Append("       a.style_no, \n");
            varname1.Append("       b.check_value, \n");

            varname1.Append(" (select check_subject from qc_itemcheck_mt where check_id = b.check_id && item_vcd = b.item_vcd ) as check_subject \n");
            varname1.Append("FROM w_product_qc a \n");
            varname1.Append("JOIN w_product_qc_value b ON a.pq_no=b.pq_no \n");
            varname1.Append("AND a.item_vcd=b.item_vcd \n");


            varname1.Append("WHERE a.item_vcd='" + item_vcd + "' \n");
            varname1.Append("  AND a.fo_no='" + fo_no + "' \n");
            varname1.Append("GROUP BY a.fo_no, \n");
            varname1.Append("         b.check_id, \n");
            varname1.Append("         b.check_cd, \n");
            varname1.Append("         b.check_value;");

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
        public ActionResult Search_pr_qc_info()
        {

            var fo_no = Request["fo_no"];
            var style_no = Request["style_no"];
            var bom_no = Request["bom_no"];

            var sql = new StringBuilder();

            sql.Append("select b.target_qty,a.line_no,a.style_no,c.style_nm,a.bom_no,b.lct_cd,sum(a.check_qty) as qc_rate,")
            .Append(" (select lct_nm from lct_info where lct_cd=b.lct_cd) as lct_nm ,a.item_vcd,a.po_no,a.pq_no,CONCAT(SUBSTR(a.fo_no, 1, 1), CONVERT(SUBSTR(a.fo_no, 2, 11), INT)) as fo_no_hien,a.fo_no, a.ok_qty AS ok_qty, ")

            .Append("CASE WHEN a.check_qty IS NULL OR a.ok_qty IS NULL THEN '0' ELSE (a.check_qty-a.ok_qty) END AS defective_qty,")
            .Append("CASE WHEN a.check_qty IS NULL OR (a.check_qty-a.ok_qty) < 0 OR a.ok_qty IS NUll THEN CONCAT(ROUND(0,2),' %') ELSE CONCAT(ROUND(((a.check_qty-a.ok_qty)/a.check_qty)*100,2),' %')  END AS defect_rate")
            .Append(" from w_product_qc as a ")
            .Append(" join m_order_facline_info as b on a.fo_no=b.fo_no join d_style_info as c on a.style_no=c.style_no ")
            .Append(" Where('" + fo_no + "'='' OR CONCAT(substr(a.fo_no, 1, 1), CONVERT(substr(a.fo_no, 2, 11),INT)) LIKE '%" + fo_no + "%') ")
            .Append("  and ('" + style_no + "'='' OR  c.style_no like '%" + style_no + "%' )")
            .Append("AND ('" + bom_no + "'='' OR  CONCAT(substr(a.bom_no, 1, 1), CONVERT(substr(a.bom_no, 2, 11),INT),substr(a.bom_no, 12, 1)) like '%" + bom_no + "%' ) group by a.fo_no");
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


        //------ ThanhNam ------- start-----//
        private class OQC_Chart_Overview_Model
        {
            //public String target_qty { get; set; }
            //public String sked_qty { get; set; }
            //public String line_no { get; set; }
            //public String style_no { get; set; }
            public String work_dt { get; set; }
            public String ml_no { get; set; }

            public String qc_qty { get; set; } //sum(check_qty)
            public String item_vcd { get; set; }
            public String pq_no { get; set; }
            //public String fo_no { get; set; }
            public String ok_qty { get; set; }
            public String defective_qty { get; set; }
            //public String defect_rate { get; set; }
        }

        public ActionResult OQC_OverviewChart(String item_vcd, String pq_no)
        {
            //String sql_overview = "select"
            //                    + " sum(b.target_qty) AS target_qty,"
            //                    + " sum(b.sked_qty) AS sked_qty,"

            //                    + " a.line_no AS line_no,"
            //                    + " c.style_no AS style_no,"
            //                    + " c.style_nm AS style_name,"
            //                    + " a.bom_no AS bom_no,"
            //                    + " b.lct_cd AS lct_cd,"
            //                    + " (select lct_nm from lct_info where lct_cd=b.lct_cd) as lct_nm,"
            //                    + " sum(a.check_qty) AS qc_qty,"
            //                    + " a.item_vcd AS item_vcd,"
            //                    + " a.po_no AS po_no,"
            //                    + " a.fo_no AS fo_no,"
            //                    + " sum(a.ok_qty) AS ok_qty,"
            //                    + " CASE WHEN sum(a.check_qty) IS NULL OR sum(a.ok_qty) IS NULL THEN '0' ELSE (sum(a.check_qty)-sum(a.ok_qty)) END AS defective_qty,"
            //                    + " CASE WHEN sum(a.check_qty) IS NULL OR (sum(a.check_qty)-sum(a.ok_qty)) < 0 OR sum(a.ok_qty) IS NUll THEN CONCAT(ROUND(0,2),' %') ELSE CONCAT(ROUND(((sum(a.check_qty)-sum(a.ok_qty))/sum(a.check_qty))*100,2),' %')  END AS defect_rate"
            //                    + " from w_product_qc as a"
            //                    + " join m_order_facline_info as b on a.fo_no=b.fo_no join d_style_info as c on a.style_no=c.style_no"
            //                    + " where a.item_vcd = '" + item_vcd + "'"
            //                    + " and a.fo_no = '" + fo_no + "'"
            //                    + " GROUP BY a.fo_no";
            StringBuilder varname1 = new StringBuilder();

            varname1.Append(" SELECT a.item_vcd,a.pq_no,a.pqno,a.work_dt,a.check_qty AS qc_qty,a.ok_qty,a.ml_no, ");
            varname1.Append("       (a.check_qty - a.ok_qty) AS defective_qty, ");
            varname1.Append("       IF(a.ok_qty = 0, 0, Round((a.check_qty - a.ok_qty)/a.check_qty * 100, 2)) defect_qty_qc_rate, ");
            varname1.Append("       IF(a.ok_qty = 0, 0, Round(a.ok_qty/a.check_qty * 100, 2)) ok_qty_qc_rate ");
            varname1.Append(" FROM w_product_qc AS a ");
            varname1.Append("WHERE  ('" + pq_no + "' = '' OR a.pq_no LIKE '%" + pq_no + "%') ");
            varname1.Append("       AND ('" + item_vcd + "' = '' OR a.item_vcd LIKE '%" + item_vcd + "%' ) ");
            try
            {
                var list = db.Database.SqlQuery<OQC_Chart_Overview_Model>(varname1.ToString()).FirstOrDefault();

                if (string.IsNullOrEmpty(list.item_vcd) || string.IsNullOrEmpty(list.pq_no))
                {
                    return Json(new { result = false }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { result = true, data = list }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public class OQC_Chart_Detail_Item
        {
            public int pqhno { get; set; }
            public string item_vcd { get; set; }
            public string ml_no { get; set; }
            public string pq_no { get; set; }
            //public string oldd_no { get; set; }
            public string check_subject { get; set; }
            public string check_value { get; set; }
            //public string style_no { get; set; }
            public int qc_qty { get; set; }
        }

        public class OQC_Chart_Detail_Show
        {

            public string check_subject { get; set; }
            public string ml_no { get; set; }
            [AllowHtml]
            public string datasets { get; set; }
            [AllowHtml]
            public string labels { get; set; }
            [AllowHtml]
            public string datasets_pie { get; set; }
            public List<OQC_Chart_Detail_Item> oqcChartDetailItem { get; set; }
        }

        public ActionResult _PartialView_OQC_Chart(String item_vcd, String pq_no)
        {
            //String sql_detail = "select"
            //                            + " a.item_vcd, CONCAT(SUBSTR(a.fo_no, 1, 1), CONVERT(SUBSTR(a.fo_no, 2, 11), INT)) as fo_no,b.pqhno, b.pq_no, a.oldd_no, a.bom_no,"
            //                            + " b.check_value , a.style_no,"
            //                            + " sum(b.check_qty)  as qc_qty"

            //                            + " from w_product_qc as a"
            //                            + " join w_product_qc_value as b on a.pq_no=b.pq_no and a.item_vcd=b.item_vcd"

            //                            + " where a.item_vcd='" + item_vcd + "' "
            //                            + " and a.pq_no='" + pq_no + "'"
            //                            + " group by a.fo_no,b.check_id,b.check_cd,b.check_value";

            StringBuilder sql_detail = new StringBuilder();
            sql_detail.Append("SELECT b.pqhno,b.pq_no,b.check_value,SUM(b.check_qty) AS qc_qty,a.item_vcd,a.ml_no ");
            sql_detail.Append("FROM w_product_qc a ");
            sql_detail.Append("JOIN w_product_qc_value b  ON a.pq_no=b.pq_no AND a.item_vcd=b.item_vcd ");
            sql_detail.Append("WHERE a.pq_no='" + pq_no + "' AND a.item_vcd='" + item_vcd + "' ");
            sql_detail.Append("group by b.check_id,b.check_cd,b.check_value;");


            var model = db.Database.SqlQuery<OQC_Chart_Detail_Item>(sql_detail.ToString()).ToList();

            var list_OQC_Chart_Detail_Show = new List<OQC_Chart_Detail_Show>();

            string check_subject = "";
            string fo_no = "";

            foreach (var item in model)
            {
                if (fo_no != item.ml_no)
                {
                    check_subject = item.check_subject;
                    fo_no = item.ml_no;

                    var oqcChartDetailShow = new OQC_Chart_Detail_Show();
                    oqcChartDetailShow.check_subject = item.check_subject;
                    oqcChartDetailShow.ml_no = item.ml_no;

                    var oqcChartDetailSItem = new OQC_Chart_Detail_Item();
                    oqcChartDetailSItem = item;
                    oqcChartDetailShow.oqcChartDetailItem = new List<OQC_Chart_Detail_Item>();
                    oqcChartDetailShow.oqcChartDetailItem.Add(oqcChartDetailSItem);

                    list_OQC_Chart_Detail_Show.Add(oqcChartDetailShow);
                }
                else
                {
                    var subject = list_OQC_Chart_Detail_Show.FirstOrDefault(x => x.ml_no == item.ml_no);
                    var oqcChartDetailSItem = new OQC_Chart_Detail_Item();
                    oqcChartDetailSItem = item;
                    subject.oqcChartDetailItem.Add(oqcChartDetailSItem);
                }
            }

            foreach (var item in list_OQC_Chart_Detail_Show)
            {
                item.datasets = "";
                item.labels = "";
                item.datasets_pie = "";
                foreach (var item1 in item.oqcChartDetailItem)
                {
                    item.datasets += "'" + item1.qc_qty + "',";

                    item.labels += "'" + item1.check_value + "',";

                    item.datasets_pie += "'" + item1.qc_qty + "',";
                }
            }

            return PartialView(list_OQC_Chart_Detail_Show);
        }
        //---------Thanh Nam------------ end------//
        #endregion


        #region OQC_List
        public ActionResult OQC_List()
        {
            return SetLanguage("");
        }
        public ActionResult Getproduct_qc_list()
        {
            var sql = new StringBuilder();

            sql.Append("select (a.check_qty) as qc_rate,b.target_qty,a.line_no,a.style_no,c.style_nm,a.bom_no,b.lct_cd,(select lct_nm from lct_info where lct_cd=b.lct_cd) as lct_nm ,a.item_vcd ,a.po_no,a.pq_no, CONCAT(SUBSTR(a.fo_no, 1, 1), CONVERT(SUBSTR(a.fo_no, 2, 11), INT)) as fo_no_hien ,a.fo_no from w_product_qc as a ")
            .Append("  join m_order_facline_info as b on a.fo_no=b.fo_no")
            .Append("  join d_style_info as c on a.style_no=c.style_no");

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


        [HttpGet]
        public async Task<JsonResult> getOQCList(Pageing pageing)
        {
            string pq_no = Request["pq_no"] == null ? "" : Request["pq_no"].Trim();
            string ml_no = Request["ml_no"] == null ? "" : Request["ml_no"].Trim();
            string start = Request["start"] == null ? "" : Request["start"].Trim();
            string end = Request["end"] == null ? "" : Request["end"].Trim();

            var dateConvert = new DateTime();
            if (DateTime.TryParse(end, out dateConvert))
            {
                end = dateConvert.ToString("yyyy-MM-dd");
            }
            if (DateTime.TryParse(start, out dateConvert))
            {
                start = dateConvert.ToString("yyyy-MM-dd");
            }
            var result = await _IQMSService.GetListProductQC(pq_no, ml_no, start, end);
            int totals = result.Count();
            int totalPages = (int)Math.Ceiling((float)totals / pageing.rows);
            var jsonReturn = new
            {
                total = totalPages,
                page = pageing.page,
                records = totals,
                rows = result
            };
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append(" SELECT a.item_vcd,a.pq_no,a.pqno,a.work_dt,a.check_qty ,a.ok_qty,a.ml_no, ");
            //varname1.Append("       (a.check_qty - a.ok_qty) AS defect_qty, ");
            //varname1.Append("       IF(a.ok_qty = 0, 0, Round((a.check_qty - a.ok_qty)/a.check_qty * 100, 2)) defect_qty_qc_rate, ");
            //varname1.Append("       IF(a.ok_qty = 0, 0, Round(a.ok_qty/a.check_qty * 100, 2)) ok_qty_qc_rate ");
            //varname1.Append(" FROM w_product_qc AS a ");
            //varname1.Append("WHERE  ('" + pq_no + "' = '' OR a.pq_no LIKE '%" + pq_no + "%') ");
            //varname1.Append("       AND ('" + ml_no + "' = '' OR a.ml_no LIKE '%" + ml_no + "%' ) ");
            //varname1.Append("       AND ('" + start + "' = '' OR Date_format(a.work_dt, '%Y-%m-%d') >= Date_format('" + start + "', '%Y-%m-%d') )");
            //varname1.Append("       AND ('" + end + "'= ''  OR Date_format(a.work_dt, '%Y-%m-%d') <= Date_format('" + end + "', '%Y-%m-%d') )");
            ////StringBuilder varname1 = new StringBuilder(sqlCount);
            //DataTable dt = new InitMethods().ReturnDataTableNonConstraints(varname1);
            //int total = dt.Rows.Count;
            //var result = dt.AsEnumerable().OrderByDescending(x => x.Field<int>("pqno"));
            //return new InitMethods().ReturnJsonResultWithPaging(pageing, total, result);
        }

        public JsonResult getOQCDTList(Pageing pageing)
        {
            string pq_no = Request["pq_no"] == null ? "" : Request["pq_no"].Trim();
            string item_vcd = Request["item_vcd"] == null ? "" : Request["item_vcd"].Trim();

            StringBuilder varname1 = new StringBuilder();
            varname1.Append(" SELECT a.pqhno,a.pq_no,a.check_value,a.check_qty,e.check_subject ");
            varname1.Append("      FROM w_product_qc_value AS a ");
            varname1.Append("     JOIN w_product_qc AS b ON a.pq_no=b.pq_no AND a.item_vcd=b.item_vcd ");
            varname1.Append("     LEFT JOIN qc_item_mt as c on a.item_vcd=c.item_vcd ");
            varname1.Append(" LEFT JOIN qc_itemcheck_dt as d on c.item_vcd=d.item_vcd AND a.check_cd=d.check_cd AND a.check_id=d.check_id	 ");
            varname1.Append(" JOIN qc_itemcheck_mt as e on d.item_vcd=e.item_vcd and d.check_id=e.check_id	 ");
            varname1.Append(" WHERE a.pq_no='" + pq_no + "' AND a.item_vcd='" + item_vcd + "' ");
            //varname1.Append("       AND ('" + ml_no + "' = '' OR a.ml_no LIKE '%" + ml_no + "%' ) ");

            DataTable dt = new InitMethods().ReturnDataTableNonConstraints(varname1);
            int total = dt.Rows.Count;
            var result = dt.AsEnumerable().OrderByDescending(x => x.Field<int>("pqhno"));
            return new InitMethods().ReturnJsonResultWithPaging(pageing, total, result);
        }
        //public ActionResult Getdetaipr_qc_list(string item_vcd, string pq_no)
        //{
        //    var sql = new StringBuilder();

        //    sql.Append("select b.pqhno, b.pq_no,a.oldd_no,e.check_subject,b.check_value ,b.check_qty as qc_qty,a.style_no")
        //    .Append(" from w_product_qc as a ")
        //    .Append(" join w_product_qc_value as b on a.pq_no=b.pq_no and a.item_vcd=b.item_vcd   ")
        //    .Append(" left join qc_item_mt as c on b.item_vcd=c.item_vcd ")
        //    .Append(" left join qc_itemcheck_dt as d on c.item_vcd=d.item_vcd and b.check_value=d.check_name  ")
        //    .Append(" join qc_itemcheck_mt as e on d.item_vcd=e.item_vcd and d.check_id=e.check_id  ")
        //    .Append(" where a.item_vcd='" + item_vcd + "' and b.pq_no='" + pq_no + "'");

        //    var data = new DataTable();
        //    using (var cmd = db.Database.Connection.CreateCommand())
        //    {
        //        db.Database.Connection.Open();
        //        cmd.CommandText = sql.ToString();
        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            data.Load(reader);
        //        }
        //    }
        //    db.Database.Connection.Close();
        //    var result = GetJsonPersons(data);
        //    return result;
        //}
        public ActionResult searchQCList_pr()
        {
            var fo_no = Request["fo_no"];
            var style_no = Request["style_no"];
            var bom_no = Request.QueryString["bom_no"];

            var sql = new StringBuilder();
            sql.Append("select (a.check_qty) as qc_rate,b.target_qty,a.line_no,a.style_no,c.style_nm,a.bom_no,b.lct_cd,(select lct_nm from lct_info where lct_cd=b.lct_cd) as lct_nm ,a.item_vcd ,a.po_no,a.pq_no,CONCAT(SUBSTR(a.fo_no, 1, 1), CONVERT(SUBSTR(a.fo_no, 2, 11), INT)) as fo_no_hien,a.fo_no from w_product_qc as a ")
           .Append("  join m_order_facline_info as b on a.fo_no=b.fo_no")
           .Append("  join d_style_info as c on a.style_no=c.style_no")
            .Append(" Where('" + fo_no + "'='' OR CONCAT(substr(a.fo_no, 1, 1), CONVERT(substr(a.fo_no, 2, 11),INT)) LIKE '%" + fo_no + "%') ")
            .Append("AND ('" + style_no + "'='' OR  a.style_no like '%" + style_no + "%' )")
            .Append("AND ('" + bom_no + "'='' OR  CONCAT(substr(a.bom_no, 1, 1), CONVERT(substr(a.bom_no, 2, 11),INT),substr(a.bom_no, 12, 1)) like '%" + bom_no + "%' )");
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
        #endregion

        #region PQCInformation

        public JsonResult GetJsonPersons_rows(DataTable data)
        {
            var lstPersons = GetTableRows(data);
            return Json(new { rows = lstPersons }, JsonRequestBehavior.AllowGet);
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
        public ActionResult PQCInformation()
        {
            return View();
        }

        public ActionResult processQc4()
        {
            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("SELECT \n");
            //varname1.Append(" a.fo_no as fo_no2, (SELECT lct_info.lct_nm \n");
            //varname1.Append("   FROM lct_info \n");
            //varname1.Append("   WHERE lct_info.lct_cd=a.lct_cd) AS lct_nm, \n");
            //varname1.Append("       b.style_no, \n");
            //varname1.Append("       b.style_nm, \n");
            //varname1.Append("  (SELECT SUM(b1.check_qty) AS check_qty \n");
            //varname1.Append("   FROM m_order_facline_detail AS a1 \n");
            //varname1.Append("   JOIN m_facline_qc AS b1 ON a1.fo_no =b1.fo_no \n");
            //varname1.Append("   WHERE a.fo_no=a1.fo_no \n");
            //varname1.Append("     AND a.line_no=a1.line_no)/ \n");
            //varname1.Append("  (SELECT SUM(a1.sked_qty) AS sked_qty \n");
            //varname1.Append("   FROM m_order_facline_detail AS a1 \n");
            //varname1.Append("   JOIN m_facline_qc AS b1 ON a1.fo_no =b1.fo_no \n");
            //varname1.Append("   WHERE a.fo_no=a1.fo_no \n");
            //varname1.Append("     AND a.line_no=a1.line_no)*100 as qc_rate, \n");
            //varname1.Append("       a.* \n");
            //varname1.Append("FROM m_order_facline_info AS a \n");
            //varname1.Append("JOIN s_order_info AS c ON c.po_no=a.po_no \n");
            //varname1.Append("JOIN d_style_info AS b ON c.style_no=b.style_no");
            //varname1.Append("  order by a.fo_no desc");

            //var sql = new StringBuilder();

            //sql.Append("SELECT a.fo_no AS fo_no2,a.fo_no AS fo_no,")
            //    .Append(" (SELECT lct_info.lct_nm FROM lct_info WHERE lct_info.lct_cd=a.lct_cd) AS lct_nm, b.style_no AS style_no, b.style_nm AS style_nm,")
            //    .Append(" (SELECT SUM(b1.check_qty) AS check_qty FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no)/")
            //    .Append(" (SELECT SUM(a1.sked_qty) AS sked_qty FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no=a1.fo_no AND a.line_no = a1.line_no) * 100 AS qc_rate,")
            //    .Append(" (SELECT SUM(b1.check_qty) FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no GROUP BY b1.fo_no) AS check_qty,")
            //    .Append(" (SELECT SUM(b1.ok_qty) FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no GROUP BY b1.fo_no) AS ok_qty,")

            //    .Append(" CASE WHEN (SELECT SUM(b1.check_qty) FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no GROUP BY b1.fo_no) IS NULL OR (SELECT SUM(b1.ok_qty) FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no GROUP BY b1.fo_no) IS NULL THEN '0' ELSE (( SELECT SUM(b1.check_qty) FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no GROUP BY b1.fo_no)-(SELECT SUM(b1.ok_qty) FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no GROUP BY b1.fo_no)) END AS defective_qty,")

            //    .Append(" CASE WHEN ( SELECT SUM(b1.check_qty) FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no GROUP BY b1.fo_no) IS NULL OR (( SELECT SUM(b1.check_qty) FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no GROUP BY b1.fo_no)-(SELECT SUM(b1.ok_qty) FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no GROUP BY b1.fo_no)) < 0 OR (SELECT SUM(b1.ok_qty) FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no GROUP BY b1.fo_no) IS NULL THEN CONCAT(ROUND(0,2),' %') ELSE CONCAT(ROUND((((SELECT SUM(b1.check_qty) FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no GROUP BY b1.fo_no)-( SELECT SUM(b1.ok_qty) FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no GROUP BY b1.fo_no))/(SELECT SUM(b1.check_qty) FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no GROUP BY b1.fo_no))*100,2),' %') END AS defect_rate,")
            //    .Append(" a.line_no AS line_no, a.oflno AS oflno, a.po_no AS po_no,  a.bom_no AS bom_no, a.target_qty")

            //    .Append(" FROM m_order_facline_info AS a JOIN s_order_info AS c ON c.po_no = a.po_no JOIN d_style_info AS b ON c.style_no = b.style_no")
            //    .Append(" ORDER BY a.fo_no DESC");


            string sql = " select a.fo_no AS fo_no2,a.fo_no, a.po_no, a.line_no,a.bom_no,b.style_no,"

                          + " (select style_nm  from d_style_info where style_no = b.style_no)  as style_nm,"
                           + " (SELECT lct_nm FROM lct_info WHERE lct_cd=a.lct_cd) AS lct_nm,"
                           + " a.target_qty,"
                           + " (select sum(check_qty)  from m_facline_qc where fo_no = a.fo_no)  as check_qty,"
                           + " (select sum(ok_qty)  from m_facline_qc where fo_no = a.fo_no)  as ok_qty,"
                           + " (select (sum(check_qty)/a.target_qty)*100  from m_facline_qc where fo_no = a.fo_no) as qc_rate,"
                           + " (select sum(check_qty)-sum(ok_qty)  from m_facline_qc where fo_no = a.fo_no) as defective_qty,"

                           + " (select CONCAT(ROUND(((sum(check_qty) - sum(ok_qty))/sum(check_qty))*100,2)) from m_facline_qc where fo_no = a.fo_no) as defect_rate"
                           + "  from m_order_facline_info as a"
                           + "  join s_order_info as b"
                           + "  on a.bom_no = b.bom_no"
                           + " WHERE fo_no IN (SELECT fo_no from m_facline_qc) "
                           + " group by a.fo_no order by a.fo_no desc ";


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

        public ActionResult processQcDt(string fo_no, string line_no)
        {
            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("SELECT CASE \n");
            //varname1.Append("           WHEN c.`level` IS NULL THEN 'Last' \n");
            //varname1.Append("           ELSE c.`level` \n");
            //varname1.Append("       END AS level, \n");


            //varname1.Append("        CASE  WHEN c.`level` IS NULL THEN 'Last' ELSE c.`level`   END AS level2, ");
            //varname1.Append("       CONCAT(b.process_no, '-', b.prounit_cd) AS process_nm, \n");
            //varname1.Append("       a.sked_qty as sked_qty, \n");
            //varname1.Append("       b.check_qty as check_qty, \n");
            //varname1.Append("       b.item_vcd, \n");
            //varname1.Append("     ROUND(b.check_qty/SUM(a.sked_qty)*100,2) AS qc_rate, b.fq_no,a.fo_no \n");
            //varname1.Append("FROM m_order_facline_detail AS a \n");
            //varname1.Append("JOIN m_facline_qc AS b ON a.fo_no =b.fo_no \n");
            //varname1.Append("AND a.line_no=b.line_no \n");
            //varname1.Append("LEFT JOIN d_line_next_process AS c ON a.line_no = c.line_no \n");
            //varname1.Append("AND c.process_no = c.process_no \n");
            //varname1.Append("WHERE a.fo_no='" + fo_no + "'\n");
            //varname1.Append("  AND a.line_no='" + line_no + "' \n");
            //varname1.Append("GROUP BY b.oldhno order by c.`level` = 'last', c.`level` desc");
            var sql = new StringBuilder();
            sql.Append("SELECT")
                .Append(" CASE WHEN c.`level` IS NULL THEN 'Last' ELSE c.`level` END AS level,")
                .Append(" CASE  WHEN c.`level` IS NULL THEN 'Last' ELSE c.`level`   END AS level2,  ")
                .Append(" CONCAT(a.process_no, '-', a.prounit_cd) AS process_nm, ")
                .Append(" a.process_no,a.prounit_cd, b.sked_qty as sked_qty,a.item_vcd, ")
                .Append(" sum(a.check_qty) as check_qty,")
                .Append(" sum(a.ok_qty) as ok_qty, ")
                .Append(" case when a.ok_qty IS NULL AND a.check_qty IS null then '0' ELSE (sum(a.check_qty) - sum(a.ok_qty)) END AS defective_qty, ")
                .Append("  ROUND( (sum(a.check_qty)/b.sked_qty)*100,2) AS qc_rate, ")
                .Append(" case when a.check_qty IS NULL OR a.check_qty <= 0  then CONCAT(ROUND(0*100,2)) ELSE CONCAT(ROUND(((sum(a.check_qty) - sum(a.ok_qty))/sum(a.check_qty))*100,2)) END AS defect_rate ,")
                .Append(" a.fq_no, a.fo_no,  a.line_no ")
                .Append(" from m_facline_qc as a")
                .Append(" left join m_order_facline_detail as b ON a.fo_no =b.fo_no  and a.process_no = b.process_no and a.prounit_cd = b.prounit_cd and a.line_no = b.line_no ")
                .Append(" JOIN d_line_next_process AS c ON a.line_no = c.line_no AND a.process_no = c.process_no  ")
                .Append(" WHERE a.fo_no='" + fo_no + "'")
                .Append(" AND a.line_no='" + line_no + "'")
                .Append("  GROUP BY a.fo_no, a.process_no, a.prounit_cd order by c.`level` = 'last', c.`level` desc");

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

        public ActionResult processQcSub(string fo_no, string process_no, string prounit_cd)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT Concat(a.process_no, '-', a.prounit_cd) AS process_no, \n");
            varname1.Append(" b.check_value, \n");
            varname1.Append(" e.check_subject, \n");
            varname1.Append(" b.fq_no, \n");
            varname1.Append(" SUM(b.check_qty) AS check_qty, \n");
            varname1.Append(" a.fo_no \n");
            varname1.Append(" FROM m_facline_qc AS a \n");
            varname1.Append(" JOIN m_facline_qc_value AS b ON a.fq_no = b.fq_no AND a.item_vcd = b.item_vcd \n");

            varname1.Append(" LEFT JOIN qc_itemcheck_mt AS e ON b.item_vcd=e.item_vcd \n");
            varname1.Append(" AND b.check_id=e.check_id \n");
            varname1.Append(" WHERE a.fo_no= '" + fo_no + "' \n");
            varname1.Append(" AND  a.process_no = '" + process_no + "' \n");
            varname1.Append(" AND  a.prounit_cd = '" + prounit_cd + "' \n");
            varname1.Append(" GROUP BY a.fo_no, a.process_no,a.prounit_cd, b.check_id, b.check_cd, b.check_value \n");

            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("SELECT Concat(b.process_no, '-', b.prounit_cd) AS process_no, \n");
            //varname1.Append("       a.check_value, \n");
            //varname1.Append("       d.check_subject, \n");
            //varname1.Append("       Sum(a.check_qty)                        AS check_qty \n");
            //varname1.Append("FROM   m_facline_qc_value AS a \n");
            //varname1.Append("       JOIN m_facline_qc AS b \n");
            //varname1.Append("         ON a.fq_no = b.fq_no \n");
            //varname1.Append("            AND a.item_vcd = b.item_vcd \n");
            //varname1.Append("       JOIN qc_item_mt AS c \n");
            //varname1.Append("         ON c.item_vcd = a.item_vcd \n");
            //varname1.Append("       JOIN qc_itemcheck_mt AS d \n");
            //varname1.Append("         ON c.item_vcd = d.item_vcd \n");
            //varname1.Append("       JOIN qc_itemcheck_dt AS e \n");
            //varname1.Append("         ON e.check_id = d.check_id \n");
            //varname1.Append("WHERE  a.fq_no = '" + fq_no + "' \n");
            //varname1.Append("AND  a.item_vcd = '" + item_vcd + "' \n");
            //varname1.Append("GROUP  BY b.process_no, \n");
            //varname1.Append("          d.check_subject, \n");
            //varname1.Append("          a.check_value");

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

        public JsonResult searchProcessQc4t()
        {
            var fo_no = Request["fo_no"]; //mpo_no
            var style_no = Request["style_no"];
            var bom_no = Request["bom_no"];

            var sql = new StringBuilder();
            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("SELECT  a.fo_no as fo_no2, \n");
            //varname1.Append("  (SELECT lct_info.lct_nm \n");
            //varname1.Append("   FROM lct_info \n");
            //varname1.Append("   WHERE lct_info.lct_cd=a.lct_cd) AS lct_nm, \n");
            //varname1.Append("       b.style_no, \n");
            //varname1.Append("       b.style_nm, \n");
            //varname1.Append("  (SELECT SUM(b1.check_qty) AS check_qty \n");
            //varname1.Append("   FROM m_order_facline_detail AS a1 \n");
            //varname1.Append("   JOIN m_facline_qc AS b1 ON a1.fo_no =b1.fo_no \n");
            //varname1.Append("   WHERE a.fo_no=a1.fo_no \n");
            //varname1.Append("     AND a.line_no=a1.line_no)/ \n");
            //varname1.Append("  (SELECT SUM(a1.sked_qty) AS sked_qty \n");
            //varname1.Append("   FROM m_order_facline_detail AS a1 \n");
            //varname1.Append("   JOIN m_facline_qc AS b1 ON a1.fo_no =b1.fo_no \n");
            //varname1.Append("   WHERE a.fo_no=a1.fo_no \n");
            //varname1.Append("     AND a.line_no=a1.line_no)*100 as qc_rate, \n");
            //varname1.Append("       a.* \n");
            //varname1.Append("FROM m_order_facline_info AS a \n");
            //varname1.Append("JOIN s_order_info AS c ON c.po_no=a.po_no \n");
            //varname1.Append("JOIN d_style_info AS b ON c.style_no=b.style_no \n");
            //varname1.Append("WHERE ('" + fo_no + "'='' \n");
            //varname1.Append("       OR CONCAT(substr(a.fo_no, 1, 1), CONVERT(substr(a.fo_no, 2, 11),INT)) LIKE '%" + fo_no + "%') \n");
            //varname1.Append("  AND ('" + style_no + "'='' \n");
            //varname1.Append("       OR b.style_no LIKE '%" + style_no + "%') \n");
            //varname1.Append("  AND ('" + bom_no + "'='' \n");
            //varname1.Append("       OR a.bom_no LIKE '%" + bom_no + "%') \n");
            //varname1.Append("ORDER BY a.fo_no DESC");

            sql.Append("SELECT a.fo_no AS fo_no2,a.fo_no AS fo_no,")
                .Append(" (SELECT lct_info.lct_nm FROM lct_info WHERE lct_info.lct_cd=a.lct_cd) AS lct_nm, b.style_no, b.style_nm,")
                .Append(" (SELECT SUM(b1.check_qty) AS check_qty FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no)/")
                .Append(" (SELECT SUM(a1.sked_qty) AS sked_qty FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no=a1.fo_no AND a.line_no = a1.line_no) * 100 AS qc_rate,")
                .Append(" (SELECT SUM(b1.check_qty) FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no GROUP BY b1.fo_no) AS check_qty,")
                .Append(" (SELECT SUM(b1.ok_qty) FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no GROUP BY b1.fo_no) AS ok_qty,")

                .Append(" CASE WHEN (SELECT SUM(b1.check_qty) FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no GROUP BY b1.fo_no) IS NULL OR (SELECT SUM(b1.ok_qty) FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no GROUP BY b1.fo_no) IS NULL THEN '0' ELSE (( SELECT SUM(b1.check_qty) FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no GROUP BY b1.fo_no)-(SELECT SUM(b1.ok_qty) FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no GROUP BY b1.fo_no)) END AS defective_qty,")

                .Append(" CASE WHEN ( SELECT SUM(b1.check_qty) FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no GROUP BY b1.fo_no) IS NULL OR (( SELECT SUM(b1.check_qty) FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no GROUP BY b1.fo_no)-(SELECT SUM(b1.ok_qty) FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no GROUP BY b1.fo_no)) < 0 OR (SELECT SUM(b1.ok_qty) FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no GROUP BY b1.fo_no) IS NULL THEN CONCAT(ROUND(0,2),' %') ELSE CONCAT(ROUND((((SELECT SUM(b1.check_qty) FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no GROUP BY b1.fo_no)-( SELECT SUM(b1.ok_qty) FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no GROUP BY b1.fo_no))/(SELECT SUM(b1.check_qty) FROM m_order_facline_detail AS a1 JOIN m_facline_qc AS b1 ON a1.fo_no = b1.fo_no WHERE a.fo_no = a1.fo_no AND a.line_no = a1.line_no GROUP BY b1.fo_no))*100,2),' %') END AS defect_rate,")
                .Append(" a.*")

                .Append(" FROM m_order_facline_info AS a JOIN s_order_info AS c ON c.po_no = a.po_no JOIN d_style_info AS b ON c.style_no = b.style_no")
                .Append(" WHERE ('" + fo_no + "'='' \n")
                .Append("       OR CONCAT(substr(a.fo_no, 1, 1), CONVERT(substr(a.fo_no, 2, 11),INT)) LIKE '%" + fo_no + "%') \n")
                .Append("  AND ('" + style_no + "'='' \n")
                .Append("       OR b.style_no LIKE '%" + style_no + "%') \n")
                .Append("  AND ('" + bom_no + "'='' \n")
                .Append("       OR CONCAT(substr(a.bom_no, 1, 1), CONVERT(substr(a.bom_no, 2, 11),INT),substr(a.bom_no, 12, 1)) LIKE '%" + bom_no + "%') \n")
                .Append("ORDER BY a.fo_no DESC");
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

        //thanhnam//
        public class PQC_Overview
        {
            public string fq_no { get; set; }
            public string fo_no { get; set; }
            public string level { get; set; }
            public string level2 { get; set; }
            public string process_nm { get; set; }
            public string sked_qty { get; set; }
            public string item_vcd { get; set; }
            public string check_qty { get; set; }
            public string qc_rate { get; set; }
            public string ok_qty { get; set; }
            public string defective_qty { get; set; }
            public string defect_rate { get; set; }
        }

        public class PQC_DefectModel
        {
            public string check_subject { get; set; }
            public string fq_no { get; set; }
            [AllowHtml]
            public string datasets { get; set; }
            [AllowHtml]
            public string labels { get; set; }
            [AllowHtml]
            public string datasets_pie { get; set; }
            public List<PQC_DefectItem> Error { get; set; }
        }

        public class PQC_DefectItem
        {
            public string process_no { get; set; }
            public string fq_no { get; set; }
            public string check_subject { get; set; }
            public string check_value { get; set; }
            public string check_qty { get; set; }
        }

        public ActionResult _PQC_Chart(string fo_no, string process_no, string prounit_cd)
        {
            string sql = "SELECT Concat(a.process_no, '-', a.prounit_cd) AS process_no,"
                                + "b.check_value, "
                                + " e.check_subject, "
                                + " b.fq_no, "
                                + " SUM(b.check_qty) AS check_qty, "
                                + " a.fo_no "
                                + " FROM m_facline_qc AS a"
                                + " JOIN m_facline_qc_value AS b ON a.fq_no = b.fq_no AND a.item_vcd = b.item_vcd "

                                + " LEFT JOIN qc_itemcheck_mt AS e ON b.item_vcd=e.item_vcd "
                                + " AND b.check_id=e.check_id "
                                + " WHERE a.fo_no= '" + fo_no + "' "
                                + " AND  a.process_no = '" + process_no + "' "
                                + " AND  a.prounit_cd = '" + prounit_cd + "' "
                                + " GROUP BY a.fo_no, a.process_no,a.prounit_cd, b.check_cd,b.check_id,b.check_value ";
            var model = db.Database.SqlQuery<PQC_DefectItem>(sql).ToList();

            var data = new List<PQC_DefectModel>();

            string fq_no = "";
            foreach (var item in model)
            {
                if (fq_no != item.fq_no)
                {
                    fq_no = item.fq_no;

                    var error = new PQC_DefectModel();
                    error.check_subject = item.check_subject;
                    error.fq_no = item.fq_no;

                    var error_item = new PQC_DefectItem();
                    error_item = item;
                    error.Error = new List<PQC_DefectItem>();
                    error.Error.Add(error_item);

                    data.Add(error);
                }
                else
                {
                    var subject = data.FirstOrDefault(x => x.fq_no == item.fq_no);

                    var error_item = new PQC_DefectItem();
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
                    item.datasets += item1.check_qty + ",";

                    item.labels += "'" + item1.check_value + "',";

                    item.datasets_pie += item1.check_qty + ",";
                }
            }

            return PartialView(data);
        }

        public ActionResult getOverviewInfoChart(string fo_no, string line_no, string process_no, string prounit_cd)
        {
            try
            {
                string sql = "SELECT Concat(a.process_no, '-', a.prounit_cd) AS process_no , "
                               + "SUM(a.check_qty) AS check_qty,  SUM(a.ok_qty) AS ok_qty,"
                               + " case when a.ok_qty IS NULL AND a.check_qty IS null then '0' ELSE (SUM(a.check_qty) - SUM(a.ok_qty)) END AS defective_qty, CONCAT(SUBSTR(a.fo_no, 1, 1), CONVERT(SUBSTR(a.fo_no, 2, 11), INT)) as fo_no,a.*  "

                               + " FROM m_facline_qc AS a"

                               + " WHERE a.fo_no= '" + fo_no + "' "
                               + " AND a.line_no='" + line_no + "'"
                               + " AND  a.process_no = '" + process_no + "' "
                               + " AND  a.prounit_cd = '" + prounit_cd + "' "
                               + " GROUP BY a.fo_no, a.process_no,a.prounit_cd ";
                var data = db.Database.SqlQuery<PQC_Overview>(sql).FirstOrDefault();

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


        #endregion

        #region PQCList
        public ActionResult PQCList()
        {
            return SetLanguage("");
        }

        public ActionResult getqclist()
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT b.style_no, \n");
            varname1.Append("       c.style_nm, \n");
            varname1.Append("       b.bom_no, CONCAT(a.process_no,'-',a.prounit_cd) as process_no, a.item_vcd,\n");
            varname1.Append("       a.* \n");
            varname1.Append("FROM   m_facline_qc AS a \n");
            varname1.Append("       JOIN s_order_info AS b \n");
            varname1.Append("         ON a.po_no = b.po_no \n");
            varname1.Append("       JOIN d_style_info AS c \n");
            varname1.Append("         ON b.style_no = c.style_no \n");
            varname1.Append("ORDER  BY a.fq_no DESC");

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

        [HttpGet]
        public async Task<ActionResult> getdetail(string fq_no, string item_vcd)
        {
            var result = await _IQMSService.GetListFaclineQCValue(fq_no, item_vcd);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult getPopupwO(m_facline_qc m_facline_qc)
        {

            var vaule = db.m_facline_qc.ToList();
            var Data = new { rows = vaule };
            return Json(Data, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public async Task<JsonResult> searchQClistProcess(Pageing pageing)
        {
            string fq_no = Request["fq_no"] == null ? "" : Request["fq_no"].Trim();
            string ml_no = Request["ml_no"] == null ? "" : Request["ml_no"].Trim();
            string start = Request["start"] == null ? "" : Request["start"].Trim();
            string end = Request["end"] == null ? "" : Request["end"].Trim();

            var dateConvert = new DateTime();
            if (DateTime.TryParse(end, out dateConvert))
            {
                end = dateConvert.ToString("yyyy-MM-dd");
            }
            if (DateTime.TryParse(start, out dateConvert))
            {
                start = dateConvert.ToString("yyyy-MM-dd");
            }

            var result = await _IQMSService.GetListFaclineQC(fq_no, ml_no, start, end);
            int begin = (pageing.page - 1) * pageing.rows;
            int totals = result.Count();
            int totalPages = (int)Math.Ceiling((float)totals / pageing.rows);
            var jsonReturn = new
            {
                total = totalPages,
                page = pageing.page,
                records = totals,
                rows = result
            };
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("SELECT a.item_vcd, a.fqno, a.fq_no, a.work_dt, a.check_qty, a.ok_qty, a.ml_no, ");
            //varname1.Append("       (a.check_qty - a.ok_qty) AS defect_qty, ");
            //varname1.Append("       IF(a.ok_qty = 0, 0, Round((a.check_qty - a.ok_qty)/a.check_qty * 100, 2)) defect_qty_qc_rate, ");
            //varname1.Append("       IF(a.ok_qty = 0, 0, Round(a.ok_qty/a.check_qty * 100, 2)) ok_qty_qc_rate ");
            //varname1.Append("FROM   m_facline_qc AS a ");
            //varname1.Append("WHERE  ('" + fq_no + "' = '' OR a.fq_no LIKE '%" + fq_no + "%') ");
            //varname1.Append("       AND ('" + ml_no + "' = '' OR a.ml_no LIKE '%" + ml_no + "%' ) ");
            //varname1.Append("       AND ('" + start + "' = '' OR Date_format(a.work_dt, '%Y-%m-%d') >= Date_format('" + start + "', '%Y-%m-%d') )");
            //varname1.Append("       AND ('" + end + "'= ''  OR Date_format(a.work_dt, '%Y-%m-%d') <= Date_format('" + end + "', '%Y-%m-%d') )");
            ////StringBuilder varname1 = new StringBuilder(sqlCount);
            //DataTable dt = new InitMethods().ReturnDataTableNonConstraints(varname1);
            //int total = dt.Rows.Count;
            //var result = dt.AsEnumerable().OrderByDescending(x => x.Field<int>("fqno"));
            //return new InitMethods().ReturnJsonResultWithPaging(pageing, total, result);
        }
        #endregion

        #region TQC List
        public ActionResult TQCList()
        {
            HttpCookie cookie = HttpContext.Request.Cookies["language"];
            if (cookie != null)
            {
                ViewBag.language = cookie.Value;
            }
            return View();
        }
        public async Task<ActionResult> PartialView_Phan_loai_QC(string item_vcd)
        {
            //lấy hết tất cả qc_itemcheck_mt
            var qc_itemcheck_mt = new List<QcitemCheckModel>();
            var qc_itemcheck_mt2 = new List<QcitemCheckModel>();

            //qc_itemcheck_mt = db.qc_itemcheck_mt
            //   .Where(x => x.item_vcd == item_vcd && x.del_yn.Equals("N"))
            //   .Select(x => new qc_itemcheck_mt_Model
            //   {
            //       qc_itemcheck_mt__check_subject = x.check_subject,
            //       qc_itemcheck_mt__check_id = x.check_id,
            //       qc_itemcheck_mt__icno = x.icno,
            //   })
            //   .ToList();
            var qc_itemcheck_mt1 =await _IQMSService.Getitemcheck_mt(item_vcd);
            qc_itemcheck_mt = qc_itemcheck_mt1
                                       .Select(x => new QcitemCheckModel
                                       {
                                           qc_itemcheck_mt__check_subject = x.check_subject,
                                           qc_itemcheck_mt__check_id = x.check_id,
                                           qc_itemcheck_mt__icno = x.icno,
                                       }).ToList();
            foreach (var item in qc_itemcheck_mt)
            {
                var view_qc_Model = new List<viewQCModel>();

                //lấy hết tất cả qc_itemcheck_dt
                //var qc_itemcheck_dt = db.qc_itemcheck_dt
                //    .Where(x => x.item_vcd.Equals(item_vcd) && x.check_id.Equals(item.qc_itemcheck_mt__check_id) && (x.del_yn == "N") && (x.defect_yn == "Y")).ToList();
                var qc_itemcheck_dt = await _IQMSService.GetitemcheckDetail(item_vcd,item.qc_itemcheck_mt__check_id);
                if (qc_itemcheck_dt.ToList().Count > 0)
                {
                    foreach (var item1 in qc_itemcheck_dt)
                    {
                        var view_qc_Model_item = new viewQCModel();
                        //add check_name
                        view_qc_Model_item.qc_itemcheck_dt__check_name = item1.check_name;
                        view_qc_Model_item.qc_itemcheck_dt__icdno = item1.icdno;

                        //add view_qc_Model
                        view_qc_Model.Add(view_qc_Model_item);
                    }
                }

                item.view_qc_Model = view_qc_Model;
            }
            qc_itemcheck_mt2.AddRange(qc_itemcheck_mt);

            foreach (var ds in qc_itemcheck_mt2)
            {
                if (ds.view_qc_Model.Count == 0)
                {
                    qc_itemcheck_mt.Remove(ds);
                }
            }
            return PartialView("~/Views/QCInformation/PartialView_Phan_loai_QC.cshtml", qc_itemcheck_mt);

        }
        public async Task<ActionResult> Getfacline_qc_value1(string ProductCode, string at_no, string date_ymd, string shift)
        {
            try
            {
                var list = await _IQMSService.GetFaclineQCValueDetail(ProductCode, at_no, date_ymd, shift);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
         
        }
        public async Task<ActionResult> InsertQcPhanLoai(List<InsertMPOQCModel> model, string at_no, string ml_tims, string productcode, string shift, string date_ymd, int gr_qty, string item_vcd, m_facline_qc MFQC, m_facline_qc_value MFQCV)
        {
            #region insert info

            //var check = db.m_facline_qc.SingleOrDefault(x => x.ml_tims == ml_tims && x.fq_no.StartsWith("TI"));
            var check = await _IQMSService.Get_Facline_Qc(ml_tims);
            DateTime reg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
            String dateString = reg_dt.ToString("yyyy-MM-dd HH:mm:ss.FFF");
            DateTime chg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
            String dateChString = chg_dt.ToString("yyyy-MM-dd HH:mm:ss.FFF");


            if (check == null)
            {
              
                var list = await _IQMSService.GetmfaclineQC("TI");
             
                if (list == null)
                {
                    MFQC.fq_no = "TI000000001";
                }
                else
                {
                    var menuCd = string.Empty;
                    var subMenuIdConvert = 0;
                    //var list1 = list.LastOrDefault();

                    var bien1 = list;
                    var subMenuId = bien1.Substring(bien1.Length - 9, 9);
                    int.TryParse(subMenuId, out subMenuIdConvert);
                    menuCd = "TI" + String.Format("{0, 0:D9}", subMenuIdConvert + 1);
                    MFQC.fq_no = menuCd;
                }

                MFQC.check_qty = gr_qty;
                MFQC.ml_tims = ml_tims;
                MFQC.at_no = at_no;
                MFQC.product_cd = productcode;
                MFQC.shift = shift;
                MFQC.work_dt = DateTime.Now.ToString("yyyyMMddHHmmss");
                MFQC.reg_dt = reg_dt;
                MFQC.chg_dt = chg_dt;
                MFQC.item_nm = "Total Inpection";
                MFQC.item_exp = "Kiểm tra hoàn thiện";
                MFQC.reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                MFQC.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                int id = await _IQMSService.InsertIntoMFaclineQC(MFQC);

                if (model != null)
                {
                    foreach (var item2 in model)
                    {
                        if (item2.objTr != null)
                        {
                            foreach (var item3 in item2.objTr)
                            {
                                if (item3.input > 0)
                                {
                                    //var list_qc_itemcheck_dt = db.qc_itemcheck_dt.Find(Convert.ToInt32(item3.icdno));
                                    var list_qc_itemcheck_dt =await _IQMSService.GetQCItemCheckDetailById(Convert.ToInt32(item3.icdno));

                                    MFQCV.fq_no = MFQC.fq_no;
                                    MFQCV.product = productcode;
                                    MFQCV.shift = shift;
                                    MFQCV.at_no = at_no;
                                    MFQCV.check_value = list_qc_itemcheck_dt.check_name;
                                    MFQCV.check_cd = list_qc_itemcheck_dt.check_cd;
                                    MFQCV.check_qty = Convert.ToInt32(item3.input);
                                    MFQCV.check_id = list_qc_itemcheck_dt.check_id;
                                    MFQCV.date_ymd = date_ymd;
                                    MFQCV.reg_dt = reg_dt;
                                    MFQCV.chg_dt = chg_dt;

                                    MFQCV.reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                                    MFQCV.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                                   await _IQMSService.InsertIntoFaclineQCValue(MFQCV);
                                 
                                }
                            }
                        }
                    }


                    return Json(new { result = true, MFQC }, JsonRequestBehavior.AllowGet);
                }
            }

            else
            {
                //insert m_facline_qc_value
                if (model != null)
                {
                    foreach (var item2 in model)
                    {
                        if (item2.objTr != null)
                        {
                            foreach (var item3 in item2.objTr)
                            {
                                if (item3.input > 0)
                                {
                                    var list_qc_itemcheck_dt = await _IQMSService.GetQCItemCheckDetailById(Convert.ToInt32(item3.icdno));

                                    MFQCV.fq_no = check.fq_no;
                                    MFQCV.product = productcode;
                                    MFQCV.shift = shift;
                                    MFQCV.at_no = at_no;

                                    MFQCV.check_value = list_qc_itemcheck_dt.check_name;
                                    MFQCV.check_cd = list_qc_itemcheck_dt.check_cd;
                                    MFQCV.check_qty = Convert.ToInt32(item3.input);
                                    MFQCV.check_id = list_qc_itemcheck_dt.check_id;
                                    MFQCV.date_ymd = date_ymd;
                                    MFQCV.reg_dt = reg_dt;
                                    MFQCV.chg_dt = chg_dt;

                                    MFQCV.reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                                    MFQCV.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                                    check.reg_dt = DateTime.Now;
                                    await _IQMSService.InsertIntoFaclineQCValue(MFQCV);

                                }
                            }
                        }
                    }

                    //update update check_Qty [m_facline_qc]
                    //var soluongdefect = (db.m_facline_qc_value.Where(x => x.fq_no == MFQCV.fq_no).ToList().Sum(x => x.check_qty));
                    var soluongdefect = await _IQMSService.SunNGMFaclineQC(MFQCV.fq_no);

                    //var isExist_QC = db.m_facline_qc.Where(x => x.fq_no == MFQCV.fq_no).SingleOrDefault();
                   var isExist_QC = await _IQMSService.Top1MFaclineQC(MFQCV.fq_no);
                    isExist_QC.check_qty = soluongdefect;
                    isExist_QC.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                    //db.Entry(isExist_QC).State = EntityState.Modified;
                    await _IQMSService.UpdateMFaclineQC(isExist_QC);
                    //db.SaveChanges();

                    return Json(new { result = true, MFQC }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { result = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);

            #endregion insert info
        }
        public async Task<ActionResult> GetfaclineQCValue(string ProductCode, string datetime, string shift)
        {
            IEnumerable<MFaclineQCValue> data = await _IQMSService.GetFaclineQCValue(ProductCode, datetime, shift);
            var jsonReturn = new
            {
                rows = data
            };
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }
        [HttpDelete]
        public async Task<JsonResult> Delete_Qc_valuePhanLoai(int id)
        {
            try
            {
                if (id > 0)
                {
                    //var isExist_QCValue = db.m_facline_qc_value.Find(id);
                    var isExist_QCValue = await _IQMSService.Top1MFaclineQCValueById(id);
                    if (isExist_QCValue == null)
                    {
                        return Json(new { result = false, message = "Không tìm thấy dữ liệu" }, JsonRequestBehavior.AllowGet);
                    }
                    //db.Entry(isExist_QCValue).State = EntityState.Deleted;
                    //db.SaveChanges();
                 await  _IQMSService.DeleteMFaclineValue(id);

                    var soluongdefect = await _IQMSService.SunNGMFaclineQC(isExist_QCValue.fq_no);
                    //var soluongdefect = (db.m_facline_qc_value.Where(x => x.fq_no == isExist_QCValue.fq_no).ToList().Sum(x => x.check_qty));

                    //update check_Qty [m_facline_qc]
                    //var isExist_QC = db.m_facline_qc.Where(x => x.fq_no == isExist_QCValue.fq_no).SingleOrDefault();
                    var isExist_QC = await _IQMSService.Top1MFaclineQC(isExist_QCValue.fq_no);
                    isExist_QC.check_qty = soluongdefect;
                    isExist_QC.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                    //db.Entry(isExist_QC).State = EntityState.Modified;
                    //db.SaveChanges();
                    await _IQMSService.UpdateMFaclineQC(isExist_QC);

                    //var listp_mpo_qc = (from p in db.m_facline_qc

                    //                    where p.fqno == isExist_QC.fqno
                    //                    select new
                    //                    {
                    //                        fqno = p.fqno,
                    //                        fq_no = p.fq_no,
                    //                        check_qty = p.check_qty,
                    //                        ml_no = p.ml_no,

                    //                    }).FirstOrDefault();

                    return Json(new { result = true, data = soluongdefect }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { result = false, message = "Id rỗng" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
            }
        }
        //public ActionResult GetList_TQC()
        //{
        //    string start = Request.QueryString["start"].Trim();
        //    string product_cd = Request.QueryString["product_cd"];

        //    StringBuilder varname1 = new StringBuilder();
        //    varname1.Append("SELECT a.product_cd,a.at_no, sum(a.check_qty) AS checkqty, (sum(a.check_qty)- sum(b.check_qty)) AS ok,sum(b.check_qty) AS defect, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = '" + start + "' WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CN'  GROUP BY qcvalue.date_ymd), 0) AS day_7_n, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = '" + start + "' WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CD'  GROUP BY qcvalue.date_ymd), 0) AS day_7_d, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = DATE_ADD('" + start + "', INTERVAL -1 DAY) WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CN' GROUP BY qcvalue.date_ymd), 0) AS day_6_n, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = DATE_ADD('" + start + "', INTERVAL -1 DAY) WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CD' GROUP BY qcvalue.date_ymd), 0) AS day_6_d, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = DATE_ADD('" + start + "', INTERVAL -2 DAY) WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CN' GROUP BY qcvalue.date_ymd), 0) AS day_5_n, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = DATE_ADD('" + start + "', INTERVAL -2 DAY) WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CD' GROUP BY qcvalue.date_ymd), 0) AS day_5_d, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = DATE_ADD('" + start + "', INTERVAL -3 DAY) WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CN' GROUP BY qcvalue.date_ymd), 0) AS day_4_n, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = DATE_ADD('" + start + "', INTERVAL -3 DAY) WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CD' GROUP BY qcvalue.date_ymd), 0) AS day_4_d, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = DATE_ADD('" + start + "', INTERVAL -4 DAY) WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CN' GROUP BY qcvalue.date_ymd), 0) AS day_3_n, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = DATE_ADD('" + start + "', INTERVAL -4 DAY) WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CD' GROUP BY qcvalue.date_ymd), 0) AS day_3_d, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = DATE_ADD('" + start + "', INTERVAL -5 DAY) WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CN' GROUP BY qcvalue.date_ymd), 0) AS day_2_n, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = DATE_ADD('" + start + "', INTERVAL -5 DAY) WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CD' GROUP BY qcvalue.date_ymd), 0) AS day_2_d, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = DATE_ADD('" + start + "', INTERVAL -6 DAY) WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CN' GROUP BY qcvalue.date_ymd), 0) AS day_1_n, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = DATE_ADD('" + start + "', INTERVAL -6 DAY) WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CD' GROUP BY qcvalue.date_ymd), 0) AS day_1_d \n");

        //    varname1.Append("FROM m_facline_qc AS a \n");
        //    varname1.Append("JOIN m_facline_qc_value AS b \n");
        //    varname1.Append("ON a.fq_no = b.fq_no \n");
        //    varname1.Append("WHERE a.product_cd = '"+ product_cd + "' ");
        //    varname1.Append(" UNION ALL ");

        //    varname1.Append("SELECT c.product_cd,d.check_value as at_no, (c.check_qty) AS checkqty, (c.check_qty)- (d.check_qty) AS ok,(d.check_qty) AS defect, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = '" + start + "' WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CN'  AND qcvalue.check_cd = d.check_cd), 0) AS day_7_n, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = '" + start + "' WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CD'   AND qcvalue.check_cd = d.check_cd), 0) AS day_7_d, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = DATE_ADD('" + start + "', INTERVAL -1 DAY) WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CN' AND qcvalue.check_cd = d.check_cd), 0) AS day_6_n, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = DATE_ADD('" + start + "', INTERVAL -1 DAY) WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CD' AND qcvalue.check_cd = d.check_cd), 0) AS day_6_d, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = DATE_ADD('" + start + "', INTERVAL -2 DAY) WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CN' AND qcvalue.check_cd = d.check_cd), 0) AS day_5_n, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = DATE_ADD('" + start + "', INTERVAL -2 DAY) WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CD' AND qcvalue.check_cd = d.check_cd), 0) AS day_5_d, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = DATE_ADD('" + start + "', INTERVAL -3 DAY) WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CN' AND qcvalue.check_cd = d.check_cd), 0) AS day_4_n, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = DATE_ADD('" + start + "', INTERVAL -3 DAY) WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CD' AND qcvalue.check_cd = d.check_cd), 0) AS day_4_d, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = DATE_ADD('" + start + "', INTERVAL -4 DAY) WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CN' AND qcvalue.check_cd = d.check_cd), 0) AS day_3_n, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = DATE_ADD('" + start + "', INTERVAL -4 DAY) WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CD' AND qcvalue.check_cd = d.check_cd), 0) AS day_3_d, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = DATE_ADD('" + start + "', INTERVAL -5 DAY) WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CN' AND qcvalue.check_cd = d.check_cd), 0) AS day_2_n, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = DATE_ADD('" + start + "', INTERVAL -5 DAY) WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CD' AND qcvalue.check_cd = d.check_cd), 0) AS day_2_d, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = DATE_ADD('" + start + "', INTERVAL -6 DAY) WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CN' AND qcvalue.check_cd = d.check_cd), 0) AS day_1_n, \n");
        //    varname1.Append("IFNULL((SELECT SUM(qcvalue.check_qty)  FROM m_facline_qc AS qc JOIN  m_facline_qc_value AS qcvalue ON qc.fq_no = qcvalue.fq_no AND qcvalue.date_ymd = DATE_ADD('" + start + "', INTERVAL -6 DAY) WHERE qc.product_cd = '" + product_cd + "' AND qc.shift ='CD' AND qcvalue.check_cd = d.check_cd), 0) AS day_1_d \n");

        //    varname1.Append("FROM m_facline_qc AS c \n");
        //    varname1.Append("JOIN m_facline_qc_value AS d \n");
        //    varname1.Append("ON c.fq_no = d.fq_no \n");
        //    varname1.Append("WHERE c.product_cd = '" + product_cd + "' ");
        //    varname1.Append(" GROUP BY d.date_ymd , d.check_cd ");


        //    var data = new InitMethods().ConvertDataTableToJsonAndReturn(varname1);
        //    return Json(data.Data, JsonRequestBehavior.AllowGet);

        //}
        public ActionResult GetList_TQC()
        {
            string start = Request.QueryString["start"].Trim();
            string product_cd = Request.QueryString["product_cd"];


            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT 'Tổng NG' as totalDefect, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)  FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE  qc.product_cd = a.product_cd and qc.product_cd =  '" + product_cd + "' AND qcValue.date_ymd =  '" + start + "' AND  qc.shift ='CN'  GROUP BY qcValue.date_ymd ), 0) AS day_7_n, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)  FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE  qc.product_cd = a.product_cd and qc.product_cd = '" + product_cd + "' AND qcValue.date_ymd =  '" + start + "' AND  qc.shift ='CD' GROUP BY qcValue.date_ymd ), 0) AS day_7_d, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)  FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE  qc.product_cd = a.product_cd and qc.product_cd = '" + product_cd + "' AND qcValue.date_ymd =   DATE_ADD('" + start + "', INTERVAL -1 DAY) AND  qc.shift ='CN' GROUP BY qcValue.date_ymd  ), 0) AS day_6_n, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)  FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE  qc.product_cd = a.product_cd and qc.product_cd = '" + product_cd + "' AND qcValue.date_ymd =   DATE_ADD('" + start + "', INTERVAL -1 DAY) AND qc.shift ='CD'  GROUP BY qcValue.date_ymd), 0) AS day_6_d, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)  FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE   qc.product_cd = a.product_cd and qc.product_cd = '" + product_cd + "' AND qcValue.date_ymd =   DATE_ADD('" + start + "', INTERVAL -2 DAY) AND qc.shift ='CN'  GROUP BY qcValue.date_ymd), 0) AS day_5_n, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)  FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE  qc.product_cd = a.product_cd and qc.product_cd = '" + product_cd + "' AND qcValue.date_ymd =   DATE_ADD('" + start + "', INTERVAL -2 DAY) AND qc.shift ='CD'  GROUP BY qcValue.date_ymd), 0) AS day_5_d, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)  FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE  qc.product_cd = a.product_cd and qc.product_cd = '" + product_cd + "' AND qcValue.date_ymd =   DATE_ADD('" + start + "', INTERVAL -3 DAY) AND qc.shift ='CN'   GROUP BY qcValue.date_ymd), 0) AS day_4_n, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)  FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE  qc.product_cd = a.product_cd and qc.product_cd = '" + product_cd + "' AND qcValue.date_ymd =   DATE_ADD('" + start + "', INTERVAL -3 DAY) AND qc.shift ='CD'  GROUP BY qcValue.date_ymd), 0) AS day_4_d, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)  FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE  qc.product_cd = a.product_cd and qc.product_cd = '" + product_cd + "' AND qcValue.date_ymd =   DATE_ADD('" + start + "', INTERVAL -4 DAY) AND  qc.shift ='CN'   GROUP BY qcValue.date_ymd), 0) AS day_3_n, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)   FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE  qc.product_cd = a.product_cd and qc.product_cd ='" + product_cd + "' AND qcValue.date_ymd =   DATE_ADD('" + start + "', INTERVAL -4 DAY) AND qc.shift ='CD'  GROUP BY qcValue.date_ymd), 0) AS day_3_d, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)  FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE  qc.product_cd = a.product_cd and qc.product_cd = '" + product_cd + "' AND qcValue.date_ymd =   DATE_ADD('" + start + "', INTERVAL -5 DAY) AND qc.shift ='CN'   GROUP BY qcValue.date_ymd), 0) AS day_2_n, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)   FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE  qc.product_cd = a.product_cd and qc.product_cd = '" + product_cd + "' AND qcValue.date_ymd =   DATE_ADD('" + start + "', INTERVAL -5 DAY) AND  qc.shift ='CD'   GROUP BY qcValue.date_ymd), 0) AS day_2_d, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)  FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE  qc.product_cd = a.product_cd and qc.product_cd = '" + product_cd + "' AND qcValue.date_ymd =   DATE_ADD('" + start + "', INTERVAL -6 DAY) AND qc.shift ='CN'  GROUP BY qcValue.date_ymd), 0) AS day_1_n, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)   FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE  qc.product_cd = a.product_cd and qc.product_cd = '" + product_cd + "' AND qcValue.date_ymd =   DATE_ADD('" + start + "', INTERVAL -6 DAY) AND qc.shift ='CD'  GROUP BY qcValue.date_ymd), 0) AS day_1_d \n");
            varname1.Append("FROM m_facline_qc AS a \n");

            varname1.Append("WHERE a.product_cd = '" + product_cd + "' \n");
            varname1.Append(" GROUP BY a.product_cd  \n");



            varname1.Append(" UNION all \n");

            varname1.Append("SELECT d.check_value AS  totalDefect, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)  FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE qcValue.check_value = d.check_value and qc.product_cd ='" + product_cd + "' AND qcValue.date_ymd = '" + start + "' AND  qc.shift ='CN'  GROUP BY qcValue.date_ymd, qc.shift ), 0) AS day_7_n, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)  FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE qcValue.check_value = d.check_value and qc.product_cd ='" + product_cd + "' AND qcValue.date_ymd = '" + start + "' AND  qc.shift ='CD' GROUP BY qcValue.date_ymd, qc.shift ), 0) AS day_7_d, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)  FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE qcValue.check_value = d.check_value and qc.product_cd ='" + product_cd + "' AND qcValue.date_ymd =   DATE_ADD('" + start + "', INTERVAL -1 DAY) AND  qc.shift ='CN' AND qcValue.check_value =  d.check_value GROUP BY qcValue.date_ymd  ), 0) AS day_6_n, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)  FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE qcValue.check_value = d.check_value and qc.product_cd ='" + product_cd + "' AND qcValue.date_ymd =   DATE_ADD('" + start + "', INTERVAL -1 DAY) AND qc.shift ='CD' AND qcValue.check_value =  d.check_value  GROUP BY qcValue.date_ymd), 0) AS day_6_d, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)  FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE qcValue.check_value = d.check_value and qc.product_cd ='" + product_cd + "' AND qcValue.date_ymd =   DATE_ADD('" + start + "', INTERVAL -2 DAY) AND qc.shift ='CN' AND qcValue.check_value =  d.check_value GROUP BY qcValue.date_ymd), 0) AS day_5_n, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)  FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE qcValue.check_value = d.check_value and qc.product_cd ='" + product_cd + "' AND qcValue.date_ymd =   DATE_ADD('" + start + "', INTERVAL -2 DAY) AND qc.shift ='CD' AND qcValue.check_value =  d.check_value GROUP BY qcValue.date_ymd), 0) AS day_5_d, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)  FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE qcValue.check_value = d.check_value and qc.product_cd ='" + product_cd + "' AND qcValue.date_ymd =   DATE_ADD('" + start + "', INTERVAL -3 DAY) AND qc.shift ='CN' AND qcValue.check_value =  d.check_value  GROUP BY qcValue.date_ymd), 0) AS day_4_n, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)  FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE qcValue.check_value = d.check_value and qc.product_cd ='" + product_cd + "' AND qcValue.date_ymd =   DATE_ADD('" + start + "', INTERVAL -3 DAY) AND qc.shift ='CD' AND qcValue.check_value =  d.check_value GROUP BY qcValue.date_ymd), 0) AS day_4_d, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)  FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE qcValue.check_value = d.check_value and qc.product_cd ='" + product_cd + "' AND qcValue.date_ymd =   DATE_ADD('" + start + "', INTERVAL -4 DAY) AND  qc.shift ='CN' AND qcValue.check_value =  d.check_value  GROUP BY qcValue.date_ymd), 0) AS day_3_n, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)   FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE qcValue.check_value = d.check_value and  qc.product_cd ='" + product_cd + "' AND qcValue.date_ymd =   DATE_ADD('" + start + "', INTERVAL -4 DAY) AND qc.shift ='CD'AND qcValue.check_value =  d.check_value  GROUP BY qcValue.date_ymd), 0) AS day_3_d, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)  FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE qcValue.check_value = d.check_value and qc.product_cd ='" + product_cd + "' AND qcValue.date_ymd =   DATE_ADD('" + start + "', INTERVAL -5 DAY) AND qc.shift ='CN' AND qcValue.check_value =  d.check_value  GROUP BY qcValue.date_ymd), 0) AS day_2_n, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)   FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE qcValue.check_value = d.check_value and qc.product_cd ='" + product_cd + "' AND qcValue.date_ymd =   DATE_ADD('" + start + "', INTERVAL -5 DAY) AND  qc.shift ='CD' AND qcValue.check_value =  d.check_value  GROUP BY qcValue.date_ymd), 0) AS day_2_d, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)  FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE qcValue.check_value = d.check_value and qc.product_cd ='" + product_cd + "' AND qcValue.date_ymd =   DATE_ADD('" + start + "', INTERVAL -6 DAY) AND qc.shift ='CN'  AND qcValue.check_value =  d.check_value GROUP BY qcValue.date_ymd), 0) AS day_1_n, \n");
            varname1.Append("IFNULL((SELECT SUM(qcValue.check_qty)   FROM m_facline_qc AS qc  join m_facline_qc_value as qcValue ON qc.fq_no = qcValue.fq_no WHERE qcValue.check_value = d.check_value and qc.product_cd ='" + product_cd + "' AND qcValue.date_ymd =   DATE_ADD('" + start + "', INTERVAL -6 DAY) AND qc.shift ='CD'AND qcValue.check_value =  d.check_value  GROUP BY qcValue.date_ymd), 0) AS day_1_d \n");
            varname1.Append("FROM m_facline_qc AS c \n");
            varname1.Append("JOIN m_facline_qc_value AS d \n");
            varname1.Append("ON c.fq_no = d.fq_no \n");
            varname1.Append("WHERE c.product_cd LIKE 'LJ63-17308A%' \n");
            varname1.Append("GROUP BY d.date_ymd,  d.check_value;");

            var data = new InitMethods().ConvertDataTableToJsonAndReturn(varname1);
            return Json(data.Data, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Dong Made Product Activition Failed

        [HttpGet]
        public async Task<PartialViewResult> LoadProductActivitionFailed(string productCode, string fromDate, string toDate)
        {
            var monthReport = string.IsNullOrEmpty(fromDate) ? DateTime.Now.ToString("yyyy-MM-dd").Substring(0, 7) : fromDate.Substring(0, 7);
            DateTime date = DateTime.Now;
            var firstDayOfMonthDt = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonthDt = firstDayOfMonthDt.AddMonths(1).AddDays(-1);

            var firstDayOfMonthStr = firstDayOfMonthDt.ToString("yyyy-MM-dd");
            var lastDayOfMonthStr = lastDayOfMonthDt.ToString("yyyy-MM-dd");

            fromDate = string.IsNullOrEmpty(fromDate) ? firstDayOfMonthStr : fromDate;
            toDate = string.IsNullOrEmpty(toDate) ? lastDayOfMonthStr : toDate;

            var result = new ProductActivitionFailedResponse();
            try
            {
                var rs = await _IQMSService.GetProductActivitionFaileds(productCode, fromDate, toDate);
                var productActiveFaileds = rs.ToList();
                if (rs != null)
                {
                    var value = await _IQMSService.GetProductActivitionFailed(productCode, fromDate, toDate);
                    var ProductActivitionFailedDetails = value.ToList();
                    result.ProductActivitionFailedDetails = ProductActivitionFailedDetails;
                }
                result.ProductActivitionFaileds = productActiveFaileds;

            }
            catch (Exception e)
            {

                throw e;
            }
            ViewBag.MonthReport = monthReport;
            return PartialView(result);
        }

        public ActionResult ProductActivitionFailed()
        {
            return SetLanguage("");
        }

        public JsonResult ConvertDataTableToJson(DataTable data)
        {
            return Json(GetTableRows(data), JsonRequestBehavior.AllowGet);
        }

        #endregion

    }

    public class QClistProcess
    {
        public string fqno { get; set; }
        public string fq_no { get; set; }
        public string oldhno { get; set; }
        public string item_vcd { get; set; }
        public string fo_no { get; set; }
        public string po_no { get; set; }
        public string line_no { get; set; }
        public string style_no { get; set; }
        public string style_nm { get; set; }
        public string process_no { get; set; }
        public string bom_no { get; set; }
        public string work_dt { get; set; }
        public string check_qty { get; set; }
        public string RowNum { get; set; }
    }
}
