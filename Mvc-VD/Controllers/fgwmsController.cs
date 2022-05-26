using ClosedXML.Excel;
using Mvc_VD.Classes;
using Mvc_VD.Models;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models.Request;
using Mvc_VD.Models.Response;
using Mvc_VD.Services;
using Mvc_VD.Services.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

using HttpGetAttribute = System.Web.Mvc.HttpGetAttribute;
using System.Web.Http;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;
using HttpDeleteAttribute = System.Web.Mvc.HttpDeleteAttribute;
using HttpPutAttribute = System.Web.Mvc.HttpPutAttribute;
using Mvc_VD.Models.WOModel;
using Mvc_VD.Commons;
using Mvc_VD.Models.DTO;
using Mvc_VD.Models.FG;

namespace Mvc_VD.Controllers
{

    public class fgwmsController : BaseController
    {
        protected ITimsService _ITimsService;
        protected IFGMWServices _IFGMWServices;
        private readonly IhomeService _homeService;

        #region Constructor
        public fgwmsController(ITimsService ITimsService, IFGMWServices IFGMWServices, IhomeService ihomeService)
        {
            _ITimsService = ITimsService;
            _IFGMWServices = IFGMWServices;
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
        #endregion

        #region Init methods

        private JsonResult ConvertDataTableToJson(DataTable data)
        {
            return Json(GetTableRows(data), JsonRequestBehavior.AllowGet);
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
        #endregion Init methods

        #region Finish_warehouse(M)

        public ActionResult FininshWhlocation()
        {
            return View();
        }

        public async Task<ActionResult> GetFnWh()
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob == "Mob")
            {

                var datalct = await _ITimsService.GetListlctinfo("003G");
                return Json(new { rows = datalct }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }

        private string CreateId_FG(int id)
        {
            if (id.ToString().Length < 2)
            {
                return string.Format("0{0}", id);
            }

            if (id.ToString().Length < 3)
            {
                return string.Format("{0}", id);
            }
            return string.Empty;
        }

        public async Task<ActionResult> insertFnWh(lct_info WHL, string lct_nm, string re_mark, int lctno, string use_yn, string real_use_yn, string mv_yn, string nt_yn)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob == "Mob")
            {

                WHL.mv_yn = mv_yn;
                WHL.nt_yn = nt_yn;
                WHL.use_yn = use_yn;
                WHL.real_use_yn = real_use_yn;
                WHL.sf_yn = WHL.sf_yn;

                if (WHL.sf_yn == null)
                {
                    WHL.sf_yn = "N";
                }
                if (WHL.mv_yn == null)
                {
                    WHL.mv_yn = "N";
                }
                if (WHL.nt_yn == null)
                {
                    WHL.nt_yn = "N";
                }
                string[] listtring = { "0", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                WHL.lct_nm = lct_nm;
                WHL.order_no = 1;
                WHL.re_mark = re_mark;
                string root_yn = Request.QueryString["c_root_yn"];
                var list = await _ITimsService.GetListlctinfo("003G");// db.lct_info.Where(item => item.lct_cd.StartsWith("003G")).
                                                                      //   OrderBy(item => item.lct_cd).ThenBy(item => item.level_cd).ToList();
                var upLevel = await _ITimsService.GetListlctinfowithlctno(lctno);// db.lct_info.FirstOrDefault(item => item.lctno == lctno);
                if (list.Count == 0)
                {
                    WHL.index_cd = "G01";
                    WHL.lct_cd = "003G01000000000000";
                    WHL.shelf_cd = "0";
                    WHL.level_cd = "000";
                    WHL.lct_bar_cd = WHL.index_cd + "-" + WHL.shelf_cd;
                }
                else
                {
                    if (root_yn == "1")
                    {
                        var menuCd = string.Empty;
                        var subMenuIdConvert = 0;
                        var listY = list.Where(item => item.level_cd == "000").ToList().OrderByDescending(item => item.reg_dt).FirstOrDefault();
                        if (listY != null)
                        {
                            var subMenuId = listY.lct_cd.Substring(4, 2);
                            int.TryParse(subMenuId, out subMenuIdConvert);//tra
                        }
                        menuCd = string.Format("{0}{1}", menuCd, CreateId_FG((subMenuIdConvert + 1)));

                        WHL.index_cd = "G" + menuCd;
                        WHL.lct_cd = "003" + WHL.index_cd + "000000000000";
                        WHL.shelf_cd = "0";
                        WHL.level_cd = "000";
                        WHL.lct_bar_cd = WHL.index_cd + "-" + WHL.shelf_cd;
                    }
                    else
                    {
                        WHL.up_lct_cd = upLevel.lct_cd;
                        var Whcd = string.Empty;
                        var subMenuIdConvert = 0;
                        var sublog = new lct_info();
                        var x = "";
                        var l = "";
                        var subnew = "";
                        switch (upLevel.level_cd)
                        {
                            case "000":
                                Whcd = upLevel.lct_cd.Substring(0, 6);
                                WHL.level_cd = "001";
                                sublog = list.Where(item => item.up_lct_cd == upLevel.lct_cd).OrderByDescending(item => item.reg_dt).FirstOrDefault();

                                if (sublog != null)
                                {
                                    subnew = sublog.lct_cd.Substring(8, 1);
                                    for (int i = 0; i < listtring.Length; i++)
                                    {
                                        var m = listtring[i];
                                        if (m == subnew)
                                        {
                                            i++;
                                            subnew = listtring[i];
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    subnew = "A";
                                }
                                Whcd = string.Format("{0}{1}", Whcd, "00" + subnew + "000000000");
                                WHL.lct_cd = Whcd;
                                WHL.index_cd = (Whcd.Substring(3, 3));
                                WHL.shelf_cd = subnew;
                                WHL.lct_bar_cd = WHL.index_cd + "-" + WHL.shelf_cd;
                                WHL.mn_full = upLevel.mn_full + ">" + WHL.lct_nm;
                                break;

                            case "001":
                                Whcd = upLevel.lct_cd.Substring(0, 9);
                                WHL.level_cd = "002";
                                sublog = list.Where(item => item.up_lct_cd == upLevel.lct_cd).OrderByDescending(item => item.reg_dt).FirstOrDefault();
                                if (sublog != null)
                                {
                                    var subMenuId = sublog.lct_cd.Substring(9, 3);
                                    int.TryParse(subMenuId, out subMenuIdConvert);
                                }
                                Whcd = string.Format("{0}{1}000000", Whcd, "0" + CreateId_FG((subMenuIdConvert + 1)));
                                WHL.lct_cd = Whcd;
                                WHL.index_cd = (Whcd.Substring(3, 3));
                                x = ((Whcd.Substring(14, 1) != "0") ? Whcd.Substring(14, 1) : string.Empty);
                                l = (((Whcd.Substring(16, 2) != "0" && Whcd.Substring(16, 2) != "00")) ? Whcd.Substring(16, 2) : string.Empty);
                                WHL.shelf_cd = Whcd.Substring(8, 1) + Whcd.Substring(10, 2) + x + l;
                                WHL.lct_bar_cd = WHL.index_cd + "-" + WHL.shelf_cd;
                                WHL.mn_full = upLevel.mn_full + ">" + WHL.lct_nm;
                                break;

                            case "002":
                                Whcd = upLevel.lct_cd.Substring(0, 12);
                                WHL.level_cd = "003";
                                sublog = list.Where(item => item.up_lct_cd == upLevel.lct_cd).OrderByDescending(item => item.reg_dt).FirstOrDefault();

                                if (sublog != null)
                                {
                                    subnew = sublog.lct_cd.Substring(14, 1);
                                    for (int i = 0; i < listtring.Length; i++)
                                    {
                                        var m = listtring[i];
                                        if (m == subnew)
                                        {
                                            i++;
                                            subnew = listtring[i];
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    subnew = "A";
                                }
                                Whcd = string.Format("{0}{1}000", Whcd, "00" + subnew);
                                WHL.lct_cd = Whcd;
                                WHL.index_cd = (Whcd.Substring(3, 3));
                                x = ((Whcd.Substring(14, 1) != "0") ? Whcd.Substring(14, 1) : string.Empty);
                                l = (((Whcd.Substring(16, 2) != "0" && Whcd.Substring(16, 2) != "00")) ? Whcd.Substring(16, 2) : string.Empty);
                                WHL.shelf_cd = Whcd.Substring(8, 1) + Whcd.Substring(10, 2) + x + l;
                                WHL.lct_bar_cd = WHL.index_cd + "-" + WHL.shelf_cd;
                                WHL.mn_full = upLevel.mn_full + ">" + WHL.lct_nm;
                                break;

                            case "003":
                                Whcd = upLevel.lct_cd.Substring(0, 15);
                                WHL.level_cd = "004";
                                sublog = list.Where(item => item.up_lct_cd == upLevel.lct_cd).OrderByDescending(item => item.reg_dt).FirstOrDefault();
                                if (sublog != null)
                                {
                                    var subMenuId = sublog.lct_cd.Substring(15, 3);
                                    int.TryParse(subMenuId, out subMenuIdConvert);
                                }
                                Whcd = string.Format("{0}{1}", Whcd, "0" + CreateId_FG((subMenuIdConvert + 1)));
                                WHL.lct_cd = Whcd;
                                WHL.index_cd = (Whcd.Substring(3, 3));
                                x = ((Whcd.Substring(14, 1) != "0") ? Whcd.Substring(14, 1) : string.Empty);
                                l = (((Whcd.Substring(16, 2) != "0" && Whcd.Substring(16, 2) != "00")) ? Whcd.Substring(16, 2) : string.Empty);
                                WHL.shelf_cd = Whcd.Substring(8, 1) + Whcd.Substring(10, 2) + x + l;
                                WHL.lct_bar_cd = WHL.index_cd + "-" + WHL.shelf_cd;
                                WHL.mn_full = upLevel.mn_full + ">" + WHL.lct_nm;
                                break;

                            case "004":
                                Whcd = upLevel.lct_cd.Substring(0, 3);
                                WHL.level_cd = "000";
                                sublog = list.Where(item => item.level_cd == "000").OrderByDescending(item => item.reg_dt).FirstOrDefault();
                                if (sublog != null)
                                {
                                    var subMenuId = sublog.lct_cd.Substring(4, 2);
                                    int.TryParse(subMenuId, out subMenuIdConvert);
                                }
                                Whcd = string.Format("{0}{1}", Whcd, "W" + CreateId_FG((subMenuIdConvert + 1)) + "000000000000");

                                WHL.lct_cd = Whcd;
                                WHL.index_cd = (Whcd.Substring(3, 3));
                                WHL.shelf_cd = "0";
                                WHL.lct_bar_cd = WHL.index_cd;
                                WHL.mn_full = upLevel.mn_full + ">" + WHL.lct_nm;
                                break;
                        }
                    }
                }
                DateTime reg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
                String dateString = reg_dt.ToString("yyyy-MM-dd HH:mm:ss.FFF");

                DateTime chg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
                String dateChString = chg_dt.ToString("yyyy-MM-dd HH:mm:ss.FFF");
                WHL.reg_dt = reg_dt;
                WHL.chg_dt = chg_dt;
                if (WHL.lct_cd.Contains("001W0100A"))
                {
                    WHL.real_use_yn = "N";
                }
                await _ITimsService.InsertLctInfo(WHL);
                //if (ModelState.IsValid)
                //{
                //    db.Entry(WHL).State = EntityState.Added;
                //    db.SaveChanges();
                //    return Json(WHL, JsonRequestBehavior.AllowGet);

                //}
                return Json(WHL, JsonRequestBehavior.AllowGet);
                //return View(WHL);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<ActionResult> updateFnWh(string lct_nm, string re_mark, int lctno, string use_yn, string real_use_yn, string sf_yn, string mv_yn, string nt_yn)
        {

            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob == "Mob")
            {

                // Search user = userid fg_yn
                if (nt_yn == null)
                {
                    nt_yn = "N";
                }
                if (sf_yn == null)
                {
                    sf_yn = "N";
                }
                if (mv_yn == null)
                {
                    mv_yn = "N";
                }

                lct_info b = await _ITimsService.GetListlctinfowithlctno(lctno);// db.lct_info.Find(lctno);
                b.sf_yn = sf_yn;
                b.nt_yn = nt_yn;
                b.mv_yn = mv_yn;
                b.use_yn = use_yn;
                b.real_use_yn = real_use_yn;
                b.lct_nm = lct_nm;
                b.mn_full = b.lct_nm;
                b.re_mark = re_mark;
                await _ITimsService.UpdateLctInfo(b);
                return View(b);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> SearchFnWh()
        {

            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob == "Mob")
            {

                // Get Data from ajax function
                var W = Request["whouse"];
                var A = Request["aisle"];
                var B = Request["bay"];

                //var sql = new StringBuilder();
                //sql.Append(" SELECT * ")
                //.Append("FROM  lct_info as a ")
                //.Append("Where a.index_cd like 'G%' and ('" + A + "'='' OR  (SUBSTRING(a.lct_cd,9,1)) like '%" + A + "%' )")
                //.Append("AND ('" + B + "'='' OR  (SUBSTRING(a.lct_cd,11, 2)) like '%" + B + "%' )")
                //.Append("AND ('" + W + "'='' OR  a.index_cd like '%" + W + "%')");

                var data = await _ITimsService.GetListSearchlctinfo(A,B,W);// db.lct_info.SqlQuery(sql.ToString()).ToList<lct_info>();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion Finish_warehouse(M)

        #region selected_finish

        public async Task<ActionResult> fnWarehouse(lct_info lct_info)
        {

            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob == "Mob")
            {


                var lists = await _ITimsService.GetListSearchlctinfo("", "", "");
                //var lists = db.lct_info.Where(item => item.index_cd.StartsWith("G")).
                //  OrderBy(item => item.lct_cd).ThenBy(item => item.level_cd).Select(item => item.index_cd).Distinct().ToList();
                var result = new List<lct_info>();
                foreach (var name in lists)
                {
                    //var item = new lct_info();
                    //item.index_cd = name;
                    result.Add(name);
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> fnAisle(lct_info lct_info)
        {


            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob == "Mob")
            {


                var lists = await _ITimsService.GetListSearchlctinfo("", "", "");
                var result = new List<lct_info>();
                foreach (var name in lists)
                {
                    var item = new lct_info();
                    item.lct_cd = name.lct_cd.Substring(8, 1);
                    result.Add(item);
                }
                var result1 = new List<lct_info>();
                var list2 = result.Select(item => item.lct_cd).Distinct().ToList();
                foreach (var name in list2)
                {
                    var item = new lct_info();
                    item.lct_cd = name;
                    if (item.lct_cd != "0")
                    {
                        result1.Add(item);
                    }
                }
                return Json(result1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> fnBay(lct_info lct_info)
        {

            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob == "Mob")
            {
                var lists = await _ITimsService.GetListSearchlctinfo("", "", "");
                var result = new List<lct_info>();
                foreach (var name in lists)
                {
                    var item = new lct_info();
                    item.lct_cd = name.lct_cd.Substring(10, 2);
                    if (item.lct_cd != "00")
                    {
                        result.Add(item);
                    }
                }
                var result1 = new List<lct_info>();
                var list2 = result.Select(item => item.lct_cd).Distinct().ToList();
                foreach (var name in list2)
                {
                    var item = new lct_info();
                    item.lct_cd = name;
                    if (item.lct_cd != "0")
                    {
                        result1.Add(item);
                    }
                }
                return Json(result1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion selected_finish

        #region N_Receiving_scan_FG

        public ActionResult Receving_Scan(string code)
        {
            ViewData["Message"] = code;
            return SetLanguage("~/Views/fgwms/Receiving_Scan/Receving_Scan.cshtml");
        }

        //public ActionResult GetEXTInfo(string ext_no, string ext_nm)
        //{
        //    var sql = new StringBuilder();
        //    sql.Append(" SELECT a.* ")

        //       .Append(" FROM w_ext_info as a ")
        //        .Append(" WHERE a.use_yn ='Y' and (a.alert > 0 OR  a.ext_sts_cd <> '000' ) ")
        //         .Append("AND ('" + ext_no + "'='' OR  a.ext_no like '%" + ext_no + "%' )")
        //         .Append("AND ('" + ext_nm + "'='' OR  a.ext_nm like '%" + ext_nm + "%' )")

        //        .Append(" order by extid desc ");

        //    return new InitMethods().ConvertDataTableToJsonAndReturn(sql);
        //}

        [HttpGet]
        public async Task<ActionResult> GetReceivingScanMLQR([FromBody]ReceiveScanFGWMSRequest req)
        {

            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob == "Mob")
            {
                int start = (req.page - 1) * req.rows;
                var result = await _ITimsService.GetListDataReceivingScanFG(req.product, req.buyer, req.po, req.lot_date, req.lot_date_end);
                int totals = result.Count();
                int totalPages = (int)Math.Ceiling((float)totals / req.rows);
                var dataactual = result.Skip<ReceivingScanFGResponse>(start).Take(req.rows);
                var jsonReturn = new
                {
                    total = totalPages,
                    page = req.page,
                    records = totals,
                    rows = dataactual
                };
                return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PartialView_Receving_Scan_Wip_Missing_M_Popup_FG(string extid = "", string ext_no = "", string alert = "")
        {
            ViewBag.extid = extid;
            ViewBag.alert = alert;
            ViewBag.ext_no = ext_no;
            return PartialView("~/Views/fgwms/Receiving_Scan/PartialView_Receving_Scan_Wip_Missing_M_Popup_FG.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> finishBuyer([System.Web.Http.FromBody] int[] data)
        {
            try
            {
                var resqheader = Request.Headers;
                string[] resqheaderkey = resqheader.AllKeys;
                string[] resqheaderval = resqheader.GetValues("requestFrom");
                string resqheadervalmob = "";
                if (resqheaderval != null)
                {
                    resqheadervalmob = resqheaderval[0];
                }
                if (Session["authorize"] != null || resqheadervalmob == "Mob")
                {
                    var kiemtra = new List<MaterialInfoTIMS>();
                    string userid= Session["userid"] == null ? null : Session["userid"].ToString();
                    foreach (var wmtid in data)
                    {
                       int kq= await _ITimsService.UpdateRecevingFG(wmtid, "001", "003G01000000000000", "003G01000000000000", "006000000000000000", userid);
                        if (kq == 1)
                        {
                            var daata = await _ITimsService.GetWMaterialInfoTIMS(wmtid);
                            kiemtra.Add(daata);
                        }
                    }
                    if (kiemtra.Count() > 0)
                        return Json(new { result = true, data = kiemtra, message = "Thành Công!!!" }, JsonRequestBehavior.AllowGet);
                    else
                        return Json(new { result = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { result = false,  message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
            }
        }

        public async  Task<ActionResult> InsertMTQR_EXT_List(string buyer_qr)
        {
            try
            {

                var resqheader = Request.Headers;
                string[] resqheaderkey = resqheader.AllKeys;
                string[] resqheaderval = resqheader.GetValues("requestFrom");
                string resqheadervalmob = "";
                if (resqheaderval != null)
                {
                    resqheadervalmob = resqheaderval[0];
                }
                if (Session["authorize"] != null || resqheadervalmob == "Mob")
                {

                    if (string.IsNullOrEmpty(buyer_qr))
                    {
                        return Json(new { result = false, message = "Làm ơn kiểm tra lại mã buyer này rỗng" }, JsonRequestBehavior.AllowGet);
                    }
                    //UPDATE w_material_info
                    var kiemtra = await _ITimsService.GetListMaterialInfoTimsByBuyerQR(buyer_qr);// db.w_material_info.Where(x => x.buyer_qr == buyer_qr).ToList();

                    if (kiemtra.Count()>0)
                    {
                        if (kiemtra.FirstOrDefault().location_code.StartsWith("003"))
                        {
                            return Json(new { result = false, message = "Buyer Qr này đã được đưa vào kho thành phẩm" }, JsonRequestBehavior.AllowGet);
                        }
                        if (kiemtra.FirstOrDefault().location_code.StartsWith("004"))
                        {
                            return Json(new { result = false, message = "Buyer Qr này đã được xuất đến khách hàng" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    var check = kiemtra.Where(x => x.status == "010" && x.location_code.StartsWith("006") && x.buyer_qr != null && x.buyer_qr != "").ToList();
                    var success = 0;
                    var erro = 0;
                    string userid= Session["userid"] == null ? null : Session["userid"].ToString(); ;
                    foreach (var item in check)
                    {
                        try
                        {
                            int kq =await _ITimsService.UpdateRecevingFGwithinputdt(item.wmtid, "001", "003G01000000000000", "003G01000000000000", "006000000000000000", userid, DateTime.Now.ToString("yyyyMMddHHmmss"));
                            if (kq == 1)
                            {
                                success++;
                            }

                        }
                        catch (Exception e)
                        {
                            erro++;
                        }
                    }
                    if (success > 0)
                    {
                        return Json(new { result = true, data = check, message = "Thành Công!!!" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { result = false, data = check, message = "Không tìm thấy!!!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Lỗi hệ thống!!!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> UploadExcelFGRecei(List<FGShippingByExcelModel> tempList)
        {

            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob == "Mob")
            {

                if (tempList == null)
                {
                    return Json(new { flag = false, message = "Chưa chọn File excel" }, JsonRequestBehavior.AllowGet);
                }
                var buyerList = tempList.Select(x => x.Code);

                int success = 0;
                var dataThoaDk = new List<FGReceiveDataResponse>();
                var dataDaTonTai = new List<string>();
                if (tempList.Count > 0)
                {
                    ////lấy ra danh sách đã vào kho FG
                    //var dataf = await _ITimsService.Getwmtinfotimswithlctcd("003G01000000000000");

                    //var dataDaTonTai = dataf.Where(x => buyerList.Contains(x.buyer_qr)).Count();

                    ////lấy danh sách thỏa điều kiện để update
                    //var datas = await _ITimsService.Getwmtinfotimswithlctcd("006");


                    //var dataThoaDk = datas.Where(x => buyerList.Contains(x.buyer_qr) && x.status.Equals("010") && x.gr_qty > 0).Select(x => x.wmtid).ToList();
                    ////danh sách không tồn tại trong kho FG
                    //int listNoExist = tempList.Count() - dataThoaDk.Count() - dataDaTonTai;

                    string userid = Session["userid"] == null ? null : Session["userid"].ToString();
                    foreach (var itm in tempList)
                    {
                        var dataitm = await _ITimsService.CheckBuyerStatus(itm.Code);
                        if (dataitm == null)
                        {
                            //var datastontai = await _ITimsService.Getwmtinfotimswithlctcdandbuyer("003G01000000000000", itm.Code);
                            dataDaTonTai.Add(itm.Code);
                        }
                        else
                        {
                            dataThoaDk.AddRange(dataitm);
                        }
                    }
                    int listNoExist = tempList.Count() - dataThoaDk.Count() - dataDaTonTai.Count();
                    //foreach (var item in dataThoaDk)
                    //{
                    //    try
                    //    {
                    //        //var daata = await _ITimsService.GetWMaterialInfoTIMS(wmtid);
                    //        int kq = await _ITimsService.UpdateRecevingFGwithinputdt(item, "001", "003G01000000000000", "003G01000000000000", "006000000000000000", userid, DateTime.Now.ToString("yyyyMMddHHmmss"));

                    //        if (kq == 1)
                    //        {
                    //            success++;
                    //        }

                    //    }
                    //    catch (Exception e)
                    //    {
                    //        return Json(new { result = false, message = "" }, JsonRequestBehavior.AllowGet);
                    //    }
                    //}
                    //if (success > 0)
                    //{
                    //    //db.SaveChanges();
                    //    return Json(new { result = true, ss = true, ok = dataThoaDk.Count(), dsDatontai = dataDaTonTai, ng = listNoExist, dataThoaDk, message = "Thành Công!!!" }, JsonRequestBehavior.AllowGet);
                    //}
                    bool kqss = true;
                    if (dataThoaDk.Count() < 0)
                    {
                        kqss = false;
                    }
                    return Json(new { result = true, ss = kqss, ok = dataThoaDk.Count(), dsDatontai = dataDaTonTai, ng = listNoExist, dataThoaDk, message = "Thành Công!!!" }, JsonRequestBehavior.AllowGet);


                }
                else
                {
                    return Json(new { result = false, message = "File excel không hợp lệ." }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public async Task<JsonResult> UploadExcelFGRecei1(List<FGShippingByExcelModel> tempList)
        {
            if (tempList.Count() > 100)
            {
                return Json(new { result = false, message = "Chỉ được nhập tối đa 100 dòng." }, JsonRequestBehavior.AllowGet);
            }
            // get productCODE
            var listCode = tempList.Select(x => x.Code).ToArray();

            //lấy product
            var buyer_code = listCode[0].ToUpper();
            string productCode = "";
            if (buyer_code.Contains("DZIH"))
            {
                productCode = listCode[0].Substring(0, listCode[0].IndexOf("DZIH"));
            }
            if (buyer_code.Contains("EA8D"))
            {
                productCode = listCode[0].Substring(0, listCode[0].IndexOf("EA8D"));
            }

            var dataPRoduct = await _IFGMWServices.GetStyleInfoReplace(productCode);
            string model = "";
            string ProductName = "";
            if (dataPRoduct != null)
            {
                //lấy model + product name
                model = dataPRoduct.md_cd;
                ProductName = dataPRoduct.style_nm;
            }
            else
            {
                return Json(new { result = false, message = "Product không có trong hệ thống." }, JsonRequestBehavior.AllowGet);
            }
            // check ds cung product
            var listDataSameProduct = listCode.Where(x => x.StartsWith(productCode)).ToArray();

            //var buyer_qrs = db.generalfgs.AsNoTracking().Select(item => item.buyer_qr).ToArray();
            //var dataChuaNhapKho = await _IFGMWServices.GetAllBuyerQRGenneralFG(listDataSameProduct);
            //if (listDataSameProduct.Length > 100)
            //{
            //    return Json(new { result = false, message = "Chỉ được nhập tối đa 100 dòng." }, JsonRequestBehavior.AllowGet);
            //}
            var dataChuaNhapKho = await _IFGMWServices.GetBuyerQRGenneralFG(listDataSameProduct);

            //var dataChuaNhapKho = listDataSameProduct
            //    .Where(item => buyer_qrs.Contains(item) == false)
            //    .ToArray();
            var soluongdatadanhapkho = listDataSameProduct.Except(dataChuaNhapKho).Count();
            var listDataDiffProduct = listCode.Where(x => !x.StartsWith(productCode)).Select(z => new { BuyerCode = z }).ToArray();

            //string param = "";
            //for (int i = 0; i < dataChuaNhapKho.Count; i++)
            //{
            //    param += "'" + dataChuaNhapKho[i] + "'";
            //    if (i != dataChuaNhapKho.Count - 1)
            //    {
            //        param += ',';
            //    }
            //}


            var resultDataOfMES = await _IFGMWServices.GetListBuyerQRGenneralFG(model, ProductName, dataChuaNhapKho);



            //StringBuilder sql = new StringBuilder();
            //if (!string.IsNullOrEmpty(param))
            //{
            //    sql.AppendLine("SET sql_mode = '';SET @@sql_mode = '';");
            //    sql.AppendLine("WITH FILTER_w_material_info");
            //    sql.AppendLine("AS");
            //    sql.AppendLine(" ( 	SELECT  m.wmtid AS wmtid,");
            //    sql.AppendLine(" m.product ProductNo,");
            //    sql.AppendLine(" m.buyer_qr BuyerCode,");
            //    sql.AppendLine(" m.gr_qty Quantity,");
            //    sql.AppendLine(" 'MES'  TypeSystem , ");
            //    sql.AppendLine(" m.bb_no,  ");
            //    sql.AppendLine($" '{model}' Model, ");
            //    sql.AppendLine($" '{ProductName}' ProductName ");
            //    sql.AppendLine(" FROM	w_material_info m ");
            //    sql.AppendLine($" WHERE m.buyer_qr IN ({param}) ),");
            //    sql.AppendLine(" FILTER_stam_detail AS ");
            //    sql.AppendLine("( SELECT s.id AS wmtid,       ");
            //    sql.AppendLine("  s.product_code ProductNo, ");
            //    sql.AppendLine(" s.buyer_qr BuyerCode,      ");
            //    sql.AppendLine(" s.standard_qty Quantity,   ");
            //    sql.AppendLine(" 'SAP' TypeSystem ,          ");
            //    sql.AppendLine(" '' bb_no ,          ");
            //    sql.AppendLine($" '{model}' Model, ");
            //    sql.AppendLine($" '{ProductName}' ProductName ");
            //    sql.AppendLine("FROM stamp_detail s");
            //    sql.AppendLine($"WHERE s.buyer_qr IN   ( {param}) ),");
            //    sql.AppendLine(" FILTER_RESULT_EXIST AS  (");
            //    sql.AppendLine("    SELECT *");
            //    sql.AppendLine("    FROM    FILTER_w_material_info UNION ");
            //    sql.AppendLine(" SELECT *  FROM    FILTER_stam_detail )");
            //    sql.AppendLine("SELECT  f.wmtid, f.ProductNo, min(f.BuyerCode) BuyerCode,f.Quantity,f.TypeSystem, f.bb_no,f.Model,f.ProductName ");
            //    sql.AppendLine("FROM    FILTER_RESULT_EXIST f ");
            //    sql.AppendLine(" GROUP BY f.BuyerCode  ");
            //}
            if (resultDataOfMES.Count() < 1)
            {
                return Json(new
                {
                    result = true,
                    ss = true,
                    ok = 0,
                    khacproduct = listDataDiffProduct.Count(),
                    dsDatontai = soluongdatadanhapkho,
                    dataThoaDk = 0,
                    dataKhacProduct = listDataDiffProduct,
                    dataSoluongKhongCoTrongHeThong = 0,
                    dataKhongCoTrongHeThong = 0,

                    message = "Thành Công!!!"
                }, JsonRequestBehavior.AllowGet);
            };


            //var resultDataOfMES = db.Database.SqlQuery<FGReceiData>(sql.ToString());

           // return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
           // var resultDataOfMES = db.Database.SqlQuery<FGReceiData>(sql.ToString());
            var dataInDB = resultDataOfMES.Select(item => item.BuyerCode).ToArray();
            var resultNotExist = dataChuaNhapKho
                .Except(dataInDB)
                .Select(z => new { BuyerCode = z })
                .ToArray();

            return Json(new
            {
                result = true,
                ss = true,
                ok = dataInDB.Length,
                khacproduct = listDataDiffProduct.Count(),
                dsDatontai = soluongdatadanhapkho,
                dataThoaDk = resultDataOfMES,
                dataKhacProduct = listDataDiffProduct,
                dataSoluongKhongCoTrongHeThong = resultNotExist.Length,
                dataKhongCoTrongHeThong = resultNotExist,

                message = "Thành Công!!!"
            }, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult Feed_back_EXT_no(string extid)
        //{
        //    try
        //    {

        //        var resqheader =Request.Headers;String[] resqheaderkey = resqheader.AllKeys;String[] resqheaderval = resqheader.GetValues("requestFrom");if (Session["authorize"] != null || resqheaderval[0]== "Mob")
        //        {

        //            if (string.IsNullOrEmpty(extid))
        //            {
        //                return Json(new { result = false, message = "Làm ơn kiểm tra lại mã EXT này rỗng" }, JsonRequestBehavior.AllowGet);
        //            }
        //            var id = Convert.ToInt32(extid);
        //            var Updatesd = db.w_ext_info.Find(id);
        //            if (Updatesd != null && Updatesd.alert != 2)
        //            {
        //                Updatesd.alert = 2;
        //                Updatesd.ext_sts_cd = "000";
        //                //Updatesd.chg_dt = DateTime.Now;
        //                Updatesd.chg_id = Session["authName"] == null ? null : Session["authName"].ToString();

        //                db.Entry(Updatesd).State = EntityState.Modified;
        //                db.SaveChanges();

        //                return Json(new { result = true, data = Updatesd, message = "Thành công!!!" }, JsonRequestBehavior.AllowGet);
        //            }
        //            return Json(new { result = false, message = "Không tồn tại danh sách này!!!" }, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { result = false, message = "Lỗi hệ thống!!!" }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public ActionResult Cancel_EXT_no(string extid)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(extid))
        //        {
        //            return Json(new { result = false, message = "Làm ơn kiểm tra lại mã EXT này rỗng" }, JsonRequestBehavior.AllowGet);
        //        }
        //        var id = Convert.ToInt32(extid);
        //        var Updatesd = db.w_ext_info.Find(id);
        //        if (Updatesd != null && Updatesd.alert == 2)
        //        {
        //            Updatesd.alert = 1;
        //            Updatesd.ext_sts_cd = "000";
        //            //Updatesd.chg_dt = DateTime.Now;
        //            Updatesd.chg_id = Session["authName"] == null ? null : Session["authName"].ToString();

        //            db.Entry(Updatesd).State = EntityState.Modified;
        //            db.SaveChanges();

        //            return Json(new { result = true, data = Updatesd, message = "Thành công!!!" }, JsonRequestBehavior.AllowGet);
        //        }
        //        return Json(new { result = false, message = "Không tồn tại danh sách này!!!" }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { result = false, message = "Lỗi hệ thống!!!" }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpGet]
        public async Task<ActionResult> CheckStatusBuyerToRece(string buyerCode)
        {

            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob == "Mob")
            {

                if (string.IsNullOrEmpty(buyerCode))
                {
                    return Json(new { result = false, message = "Quét mã tem gói để tiếp tục." }, JsonRequestBehavior.AllowGet);

                }

                //var tempData = _iFGWmsService.CheckBuyerStatus(buyerCode).FirstOrDefault();
                var rs = await _ITimsService.CheckBuyerStatus(buyerCode);
                var tempData = rs.FirstOrDefault();
                if (tempData != null)
                {
                    return Json(new { data = tempData, result = true, message = "" }, JsonRequestBehavior.AllowGet);
                }

                //var buyer = db.w_material_info.Where(x => x.buyer_qr == buyerCode).FirstOrDefault();//ko ở trong tồn của MES
                //var buyer = _iFGWmsService.FindOneMaterialInfoById(buyerCode);//ko ở trong tồn của MES
                var buyer = await _ITimsService.FindOneMaterialInfoById(buyerCode);
                if (buyer == null)
                {
                    //add tem gói vào sap
                    //kiểm tra tồn ở sap chưa
                    var general = await _ITimsService.FindOneBuyerInfoById(buyerCode);
                    var isExistGeneral = general.FirstOrDefault();
                    if (isExistGeneral == null)
                    {
                        //nếu chưa có thì kiểm tra bảng stamp_detail
                        var stamp = await _ITimsService.FindStamp(buyerCode);
                        var isExistStampDetail = stamp.FirstOrDefault();
                        if (isExistStampDetail != null)
                        {
                            var data1 = new
                            {
                                wmtid = isExistStampDetail.id,
                                ProductNo = isExistStampDetail.product_code,
                                ProductName = await _ITimsService.GetStyleNameFromStyleInfo(isExistStampDetail.product_code),
                                Model = await _ITimsService.GetModelCodeFromStyleInfo(isExistStampDetail.product_code),
                                BuyerCode = isExistStampDetail.buyer_qr,
                                Quantity = isExistStampDetail.standard_qty,
                                TypeSystem = "SAP",
                                bb_no = "",

                            };
                            return Json(new { data = data1, result = true, message = "" }, JsonRequestBehavior.AllowGet);

                        }
                        return Json(new { flag = false, message = "Tem này không được tạo ở hệ thống" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { flag = false, message = "Tem gói này đã được vào tồn kho" }, JsonRequestBehavior.AllowGet);
                }

                //var tempData = db.w_material_info
                //            .Where(x => x.buyer_qr == buyerCode && x.lct_cd.Equals("006000000000000000") && x.mt_sts_cd.Equals("010"))
                //            .ToList();


                if (tempData == null)
                {
                    if (buyer.location_code.Contains("003"))
                    {
                        //kiểm tra tồn ở sap chưa
                        var isExistGeneral = await _ITimsService.FindOneBuyerInfoById(buyerCode);
                        if (isExistGeneral.Count == 0)

                        {
                            //nếu chưa có thì kiểm tra bảng stamp_detail

                            var tmp = await _ITimsService.FindStamp(buyerCode);
                            var isExistStampDetail = tmp.FirstOrDefault();
                            if (isExistStampDetail != null)
                            {
                                var data1 = new
                                {
                                    wmtid = isExistStampDetail.id,
                                    ProductNo = isExistStampDetail.product_code,
                                    ProductName = await _ITimsService.GetStyleNameFromStyleInfo(isExistStampDetail.product_code),
                                    Model = await _ITimsService.GetModelCodeFromStyleInfo(isExistStampDetail.product_code),
                                    BuyerCode = isExistStampDetail.buyer_qr,
                                    Quantity = isExistStampDetail.standard_qty,
                                    TypeSystem = "SAP",
                                    bb_no = "",
                                };
                                return Json(new { data = data1, result = true, message = "" }, JsonRequestBehavior.AllowGet);

                            }
                            return Json(new { result = false, message = "Mã Buyer này đã được đưa vào kho FG" }, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new { result = false, message = "Mã Buyer này đã được đưa vào kho FG" }, JsonRequestBehavior.AllowGet);
                    }
                    if (buyer.location_code.Contains("004"))
                    {
                        return Json(new { result = false, message = "Buyer Qr này đã được xuất đến khách hàng" }, JsonRequestBehavior.AllowGet);
                    }

                }
                return Json(new { result = false, message = "Mã buyer không đủ điều kiện để Scan" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPut]
        public async Task<ActionResult> UpdateReciFG(FGRecevingScanRequest model)
        {
            try
            {
                var resqheader = Request.Headers;
                string[] resqheaderkey = resqheader.AllKeys;
                string[] resqheaderval = resqheader.GetValues("requestFrom");
                string resqheadervalmob = "";
                if (resqheaderval != null)
                {
                    resqheadervalmob = resqheaderval[0];
                }
                if (Session["authorize"] != null || resqheadervalmob == "Mob")
                {
                    StringBuilder idsListMEs = new StringBuilder();
                    StringBuilder idsListsAP = new StringBuilder();
                    if (model.WmtidMES == null)
                    {
                        if (model.WmtidsAP == null)
                        {
                            return Json(new { flag = false, message = "Vui lòng scan tem gói để tiếp tục." });
                        }
                    }

                    //if (model.WmtidMES != null)
                    //{
                    //    foreach (var item1 in model.WmtidMES)
                    //    {
                    //        idsListMEs.Append($"'{item1}',");
                    //    }
                    //}
                    //if (model.WmtidsAP != null)
                    //{
                    //    foreach (var item1 in model.WmtidsAP)
                    //    {
                    //        idsListsAP.Append($"'{item1}',");

                    //    }
                    //}
                    //string listMes = new InitMethods().RemoveLastComma(idsListMEs);
                    //string listsAP = new InitMethods().RemoveLastComma(idsListsAP);

                    //update mã tem ở buyer
                    var item = new MaterialInfoTIMS();

                    item.location_code = "003G01000000000000";
                    item.from_lct_code = "006000000000000000";
                    item.to_lct_code = "003G01000000000000";
                    item.status = "001";
                    item.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    item.chg_date = DateTime.Now;
                    var html = "'" + "";
                    if (model.WmtidMES != null)
                    {
                        for (int i = 0; i < model.WmtidMES.Count(); i++)
                        {
                            html += model.WmtidMES.ToList()[i];
                            if(i != model.WmtidMES.Count() -1)
                            {
                                 html += "'" + ',' + "'";
                            }
                        }
                        html += "'";
                        await _ITimsService.UpdateReceFGWMaterialInfo(item, html);
                            var userID = Session["userid"] == null ? null : Session["userid"].ToString();

                            //_iFGWmsService.InsertToMesgeneralfg(listMes.Replace("'", ""), model.ModelCode, userID);
                            int aaa = await _ITimsService.InsertToGeneralFGByConditiion(html, model.ModelCode, userID);
                    }


                    //insert vào bảng sap - generalfg
                    //nếu chưa có thì kiểm tra bảng stamp_detail

                    if (model.WmtidsAP != null)
                    {
                        for (int i = 0; i < model.WmtidsAP.Count; i++)
                        {
                            //kiểm tra bảng stamp_detail có tồn tại hay không?
                            //var rs = await _ITimsService.FindOneBuyerInfoById(model.WmtidsAP[i]);

                            var rs = await _IFGMWServices.GetDataStampDetail(model.WmtidsAP[i]);
                            var isExistStampDetail = rs;
                            if (isExistStampDetail != null)
                            {
                                //    kiểm tra mã buyer này đã tồn tại chưa
                                var listStampDetail = await _ITimsService.FindOneBuyerInfoById(isExistStampDetail.buyer_qr);
                                var checkExistStampDetail = listStampDetail.SingleOrDefault();
                                if (checkExistStampDetail == null)
                                {
                                    var itemModel = new Generalfg();
                                    itemModel.buyer_qr = isExistStampDetail.buyer_qr;
                                    itemModel.product_code = isExistStampDetail.product_code;
                                    itemModel.md_cd = model.ModelCode;
                                    itemModel.dl_no = null;
                                    itemModel.qty = isExistStampDetail.standard_qty;
                                    itemModel.lot_no = isExistStampDetail.lot_date;
                                    itemModel.status = "001";
                                    itemModel.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                    itemModel.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                    itemModel.reg_dt = DateTime.Now;
                                    itemModel.chg_dt = DateTime.Now;

                                    await _ITimsService.InsertToGeneralFG(itemModel);
                                }

                            }

                        }
                    }
                    return Json(new { result = true, message = "Nhập kho thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {

                throw e;
            }
        }


        #region EXT_Info_popup

        public ActionResult PartialView_EXT_Info_Popup(string ext_no)
        {
            ViewBag.ext_no = ext_no;
            return PartialView("~/Views/fgwms/Receiving_Scan/PartialView_EXT_Info_Popup.cshtml");
        }

        //public ActionResult GetTimsShippingScanPP(string ext_no)
        //{
        //    var listdata = (from a in db.w_ext_info

        //                    where a.ext_no.Equals(ext_no)

        //                    select new
        //                    {
        //                        ext_no = a.ext_no,
        //                        ext_nm = a.ext_nm,
        //                        ext_sts_cd = a.ext_sts_cd,
        //                        remark = a.remark,
        //                    }).ToList();

        //    return Json(new { data = listdata }, JsonRequestBehavior.AllowGet);
        //}

        public async Task<ActionResult> GetTimsShippingScanListPP(string ext_no)
        {
            try
            {

                var resqheader =Request.Headers;String[] resqheaderkey = resqheader.AllKeys;String[] resqheaderval = resqheader.GetValues("requestFrom");if (Session["authorize"] != null || resqheaderval[0]== "Mob")
                {

                    var data1 = await _ITimsService.GetListMLNOwithextno(ext_no);

                    return Json(new { data = data1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PrintEXT_LIST(string ext_no)
        {
            ViewData["Message"] = ext_no;
            return PartialView("~/Views/TIMS/ShippingScan/PrintEXT_LIST.cshtml");
        }

        #endregion EXT_Info_popup

        public async Task<JsonResult> RecevingBuyerCodeScanning(string buyerCode, string type)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob == "Mob")
            {

                if (string.IsNullOrEmpty(buyerCode))
                {
                    return Json(new { flag = false, message = "Quét mã tem gói để tiếp tục." }, JsonRequestBehavior.AllowGet);
                }
                try
                {
                    var buyer = await _ITimsService.GetListMaterialInfoTimsByBuyerQR(buyerCode);// db.w_material_info.Where(x => x.buyer_qr == buyerCode).FirstOrDefault();//ko ở trong tồn của MES
                    if (buyer.Count() ==0)
                    {
                        return Json(new { flag = false, message = "Mã buyer không tồn tại." }, JsonRequestBehavior.AllowGet);

                    }

                    var tempData = buyer.Where(x => x.buyer_qr == buyerCode && x.location_code.Equals("006000000000000000") && x.status.Equals("010")).ToList();


                    if (tempData.Count < 1)
                    {
                        if (buyer.FirstOrDefault().location_code.Contains("003"))
                        {
                            return Json(new { flag = false, message = "Mã Buyer này đã được đưa vào kho FG" }, JsonRequestBehavior.AllowGet);
                        }
                        if (buyer.FirstOrDefault().location_code.Contains("004"))
                        {
                            return Json(new { result = false, message = "Buyer Qr này đã được xuất đến khách hàng" }, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new { flag = false, message = "Mã buyer không đủ điều kiện để Scan" }, JsonRequestBehavior.AllowGet);
                    }

                    var data = tempData
                                .Select(x => new
                                {
                                    Id = x.wmtid,
                                    PoNo = x.at_no,
                                    ProductNo = x.product,
                                    MaterialCode = x.material_code,
                                    Quantity = x.gr_qty,
                                    BuyerCode = x.buyer_qr,
                                    BobbinNo = x.bb_no,


                                }).FirstOrDefault();
                    return Json(new { data = data, flag = true, message = "" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { flag = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion N_Receiving_scan_FG

        #region N_Shipping_Scan_FG

        public ActionResult FGShipping_Scan()
        {
            return SetLanguage("~/Views/fgwms/Shipping_Scan/FGShipping_Scan.cshtml");
        }


        [HttpGet]
        public async Task<JsonResult> GetDLInfo([FromBody] ShippingScanRequest req)
        {
            try
            {
                var resqheader = Request.Headers;
                string[] resqheaderkey = resqheader.AllKeys;
                string[] resqheaderval = resqheader.GetValues("requestFrom");
                string resqheadervalmob = "";
                if (resqheaderval != null)
                {
                    resqheadervalmob = resqheaderval[0];
                }
                if (Session["authorize"] != null || resqheadervalmob == "Mob")
                {
                    var result = await _IFGMWServices.GetListDileveryInfomation(req.dl_no, req.dl_nm, req.productCode, req.start, req.end);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<JsonResult> GetDeliveryFG(string dl_no)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob == "Mob")
            {

                var listdata = await _IFGMWServices.GetDeliveryFG(dl_no);

                return Json(new { data = listdata }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> DeleteDL(int id)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob == "Mob")
            {

                //var dl = db.w_dl_info.Find(id);
                var dl = await _IFGMWServices.GetLastDileveryInfoById(id);
                if (dl == null)
                {
                    return Json(new { result = false, message = "Không tìm thấy mã DL này." }, JsonRequestBehavior.AllowGet);
                }
                await _IFGMWServices.UpdatePartialDeliveryInfo(id,"N");
                return Json(new { result = true, message = "Mã Đã được xóa.", value = dl.dlid }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> InsertDlInfo(DeliveryInfo w_dl_info, string dl_nm, string remark, string work_dt)
        {
            try
            {
                var resqheader = Request.Headers;
                string[] resqheaderkey = resqheader.AllKeys;
                string[] resqheaderval = resqheader.GetValues("requestFrom");
                string resqheadervalmob = "";
                if (resqheaderval != null)
                {
                    resqheadervalmob = resqheaderval[0];
                }
                if (Session["authorize"] != null || resqheadervalmob == "Mob")
                {
                    DateTime dt = DateTime.Now;
                    string day_now = dt.ToString("yyyy-MM-dd");
                    //nếu là quyền admin thì lấy input date insert ngược lại lấy today
                    var userAdmin = Session["userid"] == null ? null : Session["userid"].ToString();
                    if (string.IsNullOrEmpty(userAdmin))
                    {
                        return Json(new { result = true, message = "Vui lòng đăng nhập tài khoản trước khi tạo" }, JsonRequestBehavior.AllowGet);
                    }
                    if (userAdmin.Equals("Admin"))
                    {
                        w_dl_info.work_dt = w_dl_info.work_dt;
                    }
                    else
                    {
                        w_dl_info.work_dt = day_now;
                    }
                    #region Tang tự động

                    String dl_no = "DL1";

                    var dl_no_last = await _IFGMWServices.GetLastDileveryInfo();
                    if (dl_no_last != null)
                    {
                        var dl_noCode = dl_no_last.dl_no;
                        dl_no = string.Concat("DL", (int.Parse(dl_noCode.Substring(2)) + 1).ToString());
                    }

                    #endregion Tang tự động

                    w_dl_info.dl_no = dl_no;
                    w_dl_info.dl_nm = dl_nm;
                    w_dl_info.lct_cd = "004000000000000000";
                 //   w_dl_info.work_dt = work_dt;
                    w_dl_info.status = "000";
                    w_dl_info.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    w_dl_info.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    w_dl_info.remark = remark;
                    w_dl_info.use_yn = "Y";
                    w_dl_info.reg_dt = DateTime.Now;
                    w_dl_info.chg_dt = DateTime.Now;

                    int dlid = await _IFGMWServices.InsertIntoDeliveryInfo(w_dl_info);
                    var value = await _IFGMWServices.GetLastDileveryInfoById(dlid);
                    return Json(new { result = true, data = value, message = "Thành Công!"}, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { message = "Lỗi hệ thống !", result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPut]
        public async Task<ActionResult> ModifyDlInfo(int dlid, string dl_nm, string remark, string work_dt)
        {
            try
            {
                var resqheader = Request.Headers;
                string[] resqheaderkey = resqheader.AllKeys;
                string[] resqheaderval = resqheader.GetValues("requestFrom");
                string resqheadervalmob = "";
                if (resqheaderval != null)
                {
                    resqheadervalmob = resqheaderval[0];
                }
                if (Session["authorize"] != null || resqheadervalmob == "Mob")
                {

                    DateTime dt = DateTime.Now;
                    string day_now = dt.ToString("yyyy-MM-dd");
                    var KTTT = await _IFGMWServices.GetLastDileveryInfoById(dlid);
                    if (KTTT == null)
                    {
                        return Json(new { result = false, message = "Không tìm thấy !!!" }, JsonRequestBehavior.AllowGet);
                    }

                    var userAdmin = Session["userid"] == null ? null : Session["userid"].ToString();
                    if (string.IsNullOrEmpty(userAdmin))
                    {
                        return Json(new { result = true, message = "Vui lòng đăng nhập tài khoản trước khi sửa" }, JsonRequestBehavior.AllowGet);
                    }
                    if (userAdmin.Equals("Admin"))
                    {
                        KTTT.work_dt = work_dt;
                    }
                    //else
                    //{
                    //    KTTT.work_dt = day_now;
                    //}
                    KTTT.dl_nm = dl_nm;
                    KTTT.remark = remark;
                  //  KTTT.work_dt = work_dt;
                    KTTT.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    KTTT.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    KTTT.chg_dt = DateTime.Now;
                    KTTT.reg_dt = DateTime.Now;

                    await _IFGMWServices.UpdateDeliveryInfo(KTTT);

                    return Json(new { result = true, data = KTTT }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Lỗi hệ thống!!!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetShipping_ScanMLQR_FG(string bx_no)
        {
            try
            {
                var resqheader = Request.Headers;
                string[] resqheaderkey = resqheader.AllKeys;
                string[] resqheaderval = resqheader.GetValues("requestFrom");
                string resqheadervalmob = "";
                if (resqheaderval != null)
                {
                    resqheadervalmob = resqheaderval[0];
                }
                if (Session["authorize"] != null || resqheadervalmob == "Mob")
                {

                    if (string.IsNullOrEmpty(bx_no))
                    {
                        return Json(new { result = false, message = "Vui lòng kiểm tra lại mã tem thùng bị rỗng" }, JsonRequestBehavior.AllowGet);
                    }
                    //nếu thỏa điều kiện tem thùng trong bảng w_box_mapping: sts= 000 là chưa xuất
                    var checkBox = await _IFGMWServices.CheckStampBox(bx_no, "000");
                    if (checkBox != null)
                    {
                        return Json(new { result = true, message = "Thỏa điều kiện xuất hàng", data = checkBox }, JsonRequestBehavior.AllowGet);
                    }
                    //nếu không thỏa điều kiện thì kiểm tra nó có tồn tại trong db khôg
                    var checkBoxExist = await _IFGMWServices.CheckStampBoxExist(bx_no);
                    if (checkBoxExist == null)
                    {
                        return Json(new { result = false, message = "Không tìm thấy mã box này - hoặc tem gói chưa được scan!!!" }, JsonRequestBehavior.AllowGet);
                    }
                    if (checkBoxExist.status.Equals("001"))
                    {
                        return Json(new { result = false, message = "Thùng này đã được xuất kho" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { result = false, message = "Không tìm thấy mã box này - hoặc tem gói chưa được scan!!!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = "Lỗi hệ thống!!!" }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult boxCodeMapped(string boxCodeMapped, string buyerCode)
        {
            StringBuilder sql = new StringBuilder($"CALL spfgwms_GetMappedProducts('{boxCodeMapped}','{buyerCode}');");

            var tempData = new InitMethods().ConvertDataTable<MappedProductModel>(sql);


            var data = tempData
                            .Select(x => new MappedProductModel
                            {
                                Id = x.Id,
                                MaterialCode = x.MaterialCode,
                                ProductNo = x.ProductNo,
                                BuyerCode = x.BuyerCode,
                                Quantity = x.Quantity,
                                lot_date = x.lot_date,

                            })
                            .ToList();
            return Json(new { data, flag = true, message = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> ShowingScanBoxTemporary(List<FGShippingByExcelModel> tempList)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob == "Mob")
            {

                bool check = true;
                List<BoxMapping> listData = new List<BoxMapping>();
                var box_no_issue = "";
                if (tempList == null)
                {
                    return Json(new { result = false, message = "Vui lòng chọn file excel" }, JsonRequestBehavior.AllowGet);
                }
                if (tempList != null)
                {
                    foreach (var item in tempList)
                    {

                        if (string.IsNullOrEmpty(item.Code))
                        {
                            check = false;
                            break;
                        }
                        //nếu không thỏa điều kiện thì kiểm tra nó có tồn tại trong db khôg
                        var checkBoxExist = await _IFGMWServices.CheckStampBoxExist(item.Code);
                        if (checkBoxExist == null)
                        {
                            check = false;
                            box_no_issue = item.Code;
                            break;
                        }
                        var checkBox = await _IFGMWServices.CheckStampBox(item.Code, "000");
                        if (checkBox == null)
                        {
                            check = false;
                            box_no_issue = item.Code;
                            break;
                        }
                        listData.Add(checkBox);
                    }
                    if (check)
                    {
                        return Json(new { result = true, message = "Tạo danh sách thành công!!!", listData }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, message = "Lỗi do không tìm thấy mã tem thùng hoặc tem thùng rỗng" + box_no_issue }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }


        public async Task<ActionResult> GetShipping_ScanBuyerQR_FG(string buyerCode)
        {
            try
            {

                var resqheader = Request.Headers;
                string[] resqheaderkey = resqheader.AllKeys;
                string[] resqheaderval = resqheader.GetValues("requestFrom");
                string resqheadervalmob = "";
                if (resqheaderval != null)
                {
                    resqheadervalmob = resqheaderval[0];
                }
                if (Session["authorize"] != null || resqheadervalmob == "Mob")
                {

                    if (string.IsNullOrEmpty(buyerCode))
                    {
                        return Json(new { result = false, message = "Vui lòng kiểm tra lại mã tem gói này bị rỗng" }, JsonRequestBehavior.AllowGet);
                    }
                    var data = await _ITimsService.GetListShippingScanBuyerQRFG(buyerCode);

                    if (data.Count == 0)
                    {
                        return Json(new { result = false, message = "Không tìm thấy mã tem gói này!!!" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { result = true, message = "Thành công!!", data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Lỗi hệ thống!!!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> ShippingExportToExcel(string dlNo)
        {

            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob == "Mob")
            {

                dlNo = dlNo.TrimStart('[').TrimEnd(']');

                if (dlNo.Length > 0)
                {
                    //StringBuilder sql = new StringBuilder($" Call spFGWMS_ExportShippingScanToExcel('{dlNo}'); ");

                    //string[] keys = Request.Form.AllKeys;

                    //var value = Request.Form[keys[0]];

                    var data = await _IFGMWServices.GetExportShippingScanToExcel(dlNo);// db.Database.SqlQuery<FGShippingScanExportExcelModel>(sql.ToString());

                    var values = data.ToList().AsEnumerable().Select(x => new
                    {
                        product = x.product,
                        lotNo = x.end_production_dt,                        //buyer_qr = x.buyer_qr,
                        //box_code = x.box_code,
                        DeliveryDt = x.work_dt,
                        qty = x.qty,

                    }).ToArray();

                    String[] labelList = new string[4] { "Product", "Lot No", "Delivery Date", "Quantity" };

                    Response.ClearContent();

                    Response.AddHeader("content-disposition", "attachment;filename=DanhSachXuatHang.xls");

                    Response.AddHeader("Content-Type", "application/vnd.ms-excel");

                    new InitMethods().WriteHtmlTable(values, Response.Output, labelList);

                    Response.End();
                }

                return View("~/Views/fgwms/Shipping_Scan/FGShipping_Scan.cshtml");
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> ExportToExcel(string dlNo)
        {

                var resqheader = Request.Headers;
                string[] resqheaderkey = resqheader.AllKeys;
                string[] resqheaderval = resqheader.GetValues("requestFrom");
                string resqheadervalmob = "";
                if (resqheaderval != null)
                {
                    resqheadervalmob = resqheaderval[0];
                }
                if (Session["authorize"] != null || resqheadervalmob == "Mob")
                {
                    dlNo = dlNo.TrimStart('[').TrimEnd(']');

                if (dlNo.Length > 0)
                {
                    //StringBuilder sql = new StringBuilder($" Call spFGWMS_ShippingDL_ExportExcel('{dlNo}'); ");

                    //string[] keys = Request.Form.AllKeys;

                    //var value = Request.Form[keys[0]];

                    var data = await _IFGMWServices.GetShippingDLExportExcel(dlNo);// db.Database.SqlQuery<FGExportExcelModel>(sql.ToString());

                    var values = data.ToList().AsEnumerable().Select(x => new
                    {
                        box_code = x.box_code,
                        buyer_qr = x.buyer_qr,
                        a = "",
                        lot_no = x.lot_no


                    }).ToArray();

                    String[] labelList = new string[4] { "BOX", "BUYER","", "etc/remark" };

                    Response.ClearContent();

                    Response.AddHeader("content-disposition", "attachment;filename=DanhSachMaBuyer.xls");

                    Response.AddHeader("Content-Type", "application/vnd.ms-excel");

                    new InitMethods().WriteHtmlTable(values, Response.Output, labelList);

                    Response.End();
                }

                return View("~/Views/fgwms/Shipping_Scan/FGShipping_Scan.cshtml");
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPut]
        public async Task<ActionResult> UpdateMTQR_DeliveryList(FGBoxMappingModel model)
        {
            try
            {
                var resqheader = Request.Headers;
                string[] resqheaderkey = resqheader.AllKeys;
                string[] resqheaderval = resqheader.GetValues("requestFrom");
                string resqheadervalmob = "";
                if (resqheaderval != null)
                {
                    resqheadervalmob = resqheaderval[0];
                }
                if (Session["authorize"] != null || resqheadervalmob == "Mob")
                {

                    if (string.IsNullOrEmpty(model.DlNo))
                    {
                        return Json(new { result = false, message = "Vui lòng chọn tên danh sách xuất hàng." }, JsonRequestBehavior.AllowGet);
                    }
                    StringBuilder listBox = new StringBuilder();
                    DateTime dt = DateTime.Now;
                    string day_now = dt.ToString("yyyyMMdd");
                    string full_date = dt.ToString("yyyy-MM-dd HH:mm:ss");
                    var user = Session["userid"] == null ? "" : Session["userid"].ToString();

                    var html = "'"+"";

                    for (int i = 0; i < model.ListBoxCode.Count; i++)
                    {
                        html +=  model.ListBoxCode[i];

                        if (i != model.ListBoxCode.Count() - 1)
                        {
                            html += "'" + ',' + "'";
                        }
                    }
                    html = html + "'";
                  //  var html1 = Char.Parse(html);
                   // html = html;
                 //   html =  html.Replace(",", "','");
                    var listStamp = await _IFGMWServices.GetListStampData(html);
                    var getBuyerCode = "'" + "";
                    for (int i = 0; i < listStamp.Count(); i++)
                    {
                        getBuyerCode += listStamp.ToList()[i];

                        if (i != listStamp.Count() - 1)
                        {
                            getBuyerCode += "'" + ',' + "'";
                        }
                    }
                    getBuyerCode = getBuyerCode + "'";
                    //UPDATE w_box_mapping COLUMN status
                    await _IFGMWServices.UpdatePartialBoxMapping(user, html);

                    //UPDATE w_material_info_tims  COLUMN dl_no
                    await _IFGMWServices.UpdatePartialMaterialInfoTIMS(model.DlNo, user, getBuyerCode);

                    //UPDATE generalfg  COLUMN dl_no
                    await _IFGMWServices.UpdatePartialGeneralfg(model.DlNo, user, getBuyerCode);

                    await _IFGMWServices.UpdatePartialStampDetail(user, getBuyerCode);

                    var value = await _IFGMWServices.GetListDileveryInfo(model.DlNo);
                    return Json(new { result = true, message = "Thành công!!!", data = value }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }




        public async Task<JsonResult> Cancel1ThungDelivery(string box_no)
        {

            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob == "Mob")
            {
                if (string.IsNullOrEmpty(box_no))
                {
                    return Json(new { result = false, message = "Không tìm được mã thùng trong cơ sở dữ liệu." }, JsonRequestBehavior.AllowGet);
                }

                //var kttt = await _ITimsService.FindStampwithboxno(box_no);// db.stamp_detail.Any(x => x.box_code == box_no);
                //if (kttt.Count()<=0)
                //{
                //    return Json(new { result = false, message = "Không tìm được mã thùng trong cơ sở dữ liệu." }, JsonRequestBehavior.AllowGet);
                //}
                //StringBuilder sql = new StringBuilder($"CALL spfgwms_Cancel1ThungDelivery('{box_no}');");

                try
                {
                    await _IFGMWServices.UpdateStampDetailCancel1ThungDelivery(box_no);
                    await _IFGMWServices.UpdatewmaterialinfowithBuyerCancel1ThungDelivery(box_no);
                    await _IFGMWServices.UpdategeneralfgCancel1ThungDelivery(box_no);
                    await _IFGMWServices.DeletewboxmappingCancel1ThungDelivery(box_no);
                    //db.Database.ExecuteSqlCommand(sql.ToString());
                    return Json(new { result = true, message = "Xóa thành công." }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception)
                {
                    return Json(new { result = false, message = "Lỗi hệ thống." }, JsonRequestBehavior.AllowGet);

                }
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<JsonResult> CancelDeliveryAll(string dlid)
        {

            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob == "Mob")
            {

                if (string.IsNullOrEmpty(dlid))
                {
                    return Json(new { result = false, message = "Dữ liệu không được tìm thấy." }, JsonRequestBehavior.AllowGet);
                }
                var id = Convert.ToInt32(dlid);
                var kttt = await _IFGMWServices.GetLastDileveryInfoById(id);
                if (kttt == null)
                {
                    return Json(new { result = false, message = "Dữ liệu không được tìm thấy." }, JsonRequestBehavior.AllowGet);
                }
                //StringBuilder sql = new StringBuilder($"CALL spfgwms_CancelDeliveryAll('{kttt.dl_no}');");
                try
                {
                    await _IFGMWServices.DeletewboxmappingdlnoCancelDeliveryAll(kttt.dl_no);
                    await _IFGMWServices.UpdatestampdetailCancelDeliveryAll(kttt.dl_no);
                    await _IFGMWServices.UpdatewmaterialinfowithdlnoCancelDeliveryAll(kttt.dl_no);
                    await _IFGMWServices.DeletewdlinfodlnoCancelDeliveryAll(kttt.dl_no);
                    //db.Database.ExecuteSqlCommand(sql.ToString());
                    return Json(new { result = true, message = "Xóa thành công." }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception)
                {
                    return Json(new { result = false, message = "Lỗi hệ thống." }, JsonRequestBehavior.AllowGet);

                }
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<JsonResult> DeleteDelivery(int id)
        {
            try
            {
                //kiểm tra trong mã này có tồn tại ko
                var CheckIsExist = await _IFGMWServices.CheckDLExist(id);
                if (CheckIsExist == null)
                {
                    return Json(new { result = false, message = "Code này không tồn tại" }, JsonRequestBehavior.AllowGet);
                }
                //kiểm tra xem đã đơn này đã chứa hàng chưa, nếu chưa là cho xóa
                var CheckAnyGenneral = await _IFGMWServices.CheckAnyGenneral(CheckIsExist.dl_no);
                if (!String.IsNullOrEmpty(CheckAnyGenneral))
                {
                    return Json(new { result = false, message = "Code này đã có mã đóng thùng" }, JsonRequestBehavior.AllowGet);
                }
                int IsSS = await _IFGMWServices.DeleteDeliveryForId(id);

                if (IsSS >= 0)
                {
                    return Json(new { result = true, id, message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = false, message = "Xóa thất bại" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json(new { result = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
            }
        }

        #region Delivery_shipping_FG_Info_popup

        public ActionResult PartialView_DL_Info_Popup(string dl_no)
        {
            ViewBag.dl_no = dl_no;
            return PartialView("~/Views/fgwms/Shipping_Scan/PartialView_DL_Info_Popup.cshtml");
        }

        public async  Task<ActionResult> Get_DL_ShippingScanPP(string dl_no)
        {


            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob == "Mob")
            {

                var listdata = await _IFGMWServices.Getwdlinfodlno(dl_no);

                return Json(new { data = listdata }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<ActionResult> Get_DL_ShippingScanListPP(string dl_no)
        {
            try
            {
                var resqheader = Request.Headers;
                string[] resqheaderkey = resqheader.AllKeys;
                string[] resqheaderval = resqheader.GetValues("requestFrom");
                string resqheadervalmob = "";
                if (resqheaderval != null)
                {
                    resqheadervalmob = resqheaderval[0];
                }
                if (Session["authorize"] != null || resqheadervalmob == "Mob")
                {

                    var result = await _IFGMWServices.GetDLShippingScanListPP(dl_no);
                    var rs = result.ToList();
                    return Json(new { data = rs }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PrintDL_ShippingFG_LIST(string dl_no)
        {
            ViewData["Message"] = dl_no;
            return PartialView("~/Views/fgwms/Shipping_Scan/PrintDL_ShippingFG_LIST.cshtml");
        }

        #endregion Delivery_shipping_FG_Info_popup


        #endregion N_Shipping_Scan_FG

        #region FGWMSGeneral

        public ActionResult inventoryGeneral()
        {
            return View("~/Views/fgwms/Inventory/General.cshtml");
        }
        public async Task<JsonResult> Search_BuyergetFGGeneral(string buyerCode)
        {

            var resqheader =Request.Headers;String[] resqheaderkey = resqheader.AllKeys;String[] resqheaderval = resqheader.GetValues("requestFrom");if (Session["authorize"] != null || resqheaderval[0]== "Mob")
            {

                if (string.IsNullOrEmpty(buyerCode))
                {
                    return Json(new { result = false, message = "Vui lòng scan mã Buyer" }, JsonRequestBehavior.AllowGet);
                }
                var kiemtraDt = await _ITimsService.GetListMaterialInfoTimsByBuyerQR(buyerCode);// db.w_material_info.Where(x => x.buyer_qr == buyerCode).SingleOrDefault();
                if (kiemtraDt.Count()<=0)
                {
                    return Json(new { result = false, message = "Mã Buyer không tồn tại" }, JsonRequestBehavior.AllowGet);
                }
                if (kiemtraDt.FirstOrDefault().location_code.Equals("003G01000000000000"))
                {
                    var sts = await _ITimsService.GetNameStatusCommCode(kiemtraDt.FirstOrDefault().status);
                    return Json(new { result = true, message = "Đã được đưa vào KHO THÀNH PHẨM, Trạng thái là:  " + sts }, JsonRequestBehavior.AllowGet);
                }
                if (kiemtraDt.FirstOrDefault().location_code.Equals("006000000000000000"))
                {
                    var sts = await _ITimsService.GetNameStatusCommCode(kiemtraDt.FirstOrDefault().status);
                    return Json(new { result = true, message = "Đang ở kho TIMS, Trạng thái là:  " + sts }, JsonRequestBehavior.AllowGet);
                }
                return null;
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }
        public async Task<JsonResult> getFGGeneral(Pageing paging)
        {

            var resqheader =Request.Headers;String[] resqheaderkey = resqheader.AllKeys;String[] resqheaderval = resqheader.GetValues("requestFrom");if (Session["authorize"] != null || resqheaderval[0]== "Mob")
            {

                string buyerCode = Request["buyerCode"] == null ? "" : Request["buyerCode"].Trim();
                string productCode = Request["productCode"] == null ? "" : Request["productCode"].Trim();
                string poCode = Request["poCode"] == null ? "" : Request["poCode"].Trim();
                string recevice_dt_start = Request["recevice_dt_start"] == null ? "" : Request["recevice_dt_start"].Trim();
                string recevice_dt_end = Request["recevice_dt_end"] == null ? "" : Request["recevice_dt_end"].Trim();

                var dateConvert = new DateTime();
                if (DateTime.TryParse(recevice_dt_end, out dateConvert))
                {
                    recevice_dt_end = dateConvert.ToString("yyyyMMdd");
                }
                else
                {
                    recevice_dt_end = DateTime.MaxValue.ToString("yyyyMMdd");
                }
                if (DateTime.TryParse(recevice_dt_start, out dateConvert))
                {
                    recevice_dt_start = dateConvert.ToString("yyyyMMdd");
                }
                else
                {
                    recevice_dt_start = DateTime.MinValue.ToString("yyyyMMdd");
                }


                //StringBuilder sql = new StringBuilder();

                //sql.Append($"call spFGWMMS_GetFGGeneral('{productCode}','{poCode}','{recevice_dt_start}','{recevice_dt_end}','{buyerCode}')");
                //DataTable dt = new InitMethods().ReturnDataTableNonConstraints(sql);
                var dataa = await _IFGMWServices.spFGWMMSGetFGGeneral(productCode, poCode, recevice_dt_start, recevice_dt_end, buyerCode);
                int total = dataa.Count();
                var dataaa = dataa.AsEnumerable().OrderByDescending(x => x.product).ToList();
                var rowsData = dataaa.Skip((paging.page - 1)).Take(paging.rows);
                int totalPages = (int)Math.Ceiling((float)total / paging.rows);

                var jsonReturn = new
                {
                    total = totalPages,
                    page = paging.page,
                    records = total,
                    rows = rowsData
                };
                return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        //public JsonResult getFGgeneralDetail(Pageing paging)
        //{
        //    try
        //    {
        //        string id = Request["id"] == null ? "" : Request["id"].Trim();
        //        string buyerCode = Request["buyerCode"] == null ? "" : Request["buyerCode"].Trim();
        //        string poCode = Request["poCode"] == null ? "" : Request["poCode"].Trim();
        //        string recevice_dt_start = Request["recevice_dt_start"] == null ? "" : Request["recevice_dt_start"].Trim();
        //        string recevice_dt_end = Request["recevice_dt_end"] == null ? "" : Request["recevice_dt_end"].Trim();

        //        if (string.IsNullOrEmpty(recevice_dt_start))
        //        {
        //            recevice_dt_start = DateTime.MinValue.ToString("yyyyMMdd");
        //        }
        //        else
        //        {
        //            recevice_dt_start = recevice_dt_start.Replace("-", "");
        //        }
        //        if (string.IsNullOrEmpty(recevice_dt_end))
        //        {
        //            recevice_dt_end = DateTime.MaxValue.ToString("yyyyMMdd");
        //        }
        //        else
        //        {
        //            recevice_dt_end = recevice_dt_end.Replace("-", "");
        //        }
        //        if (string.IsNullOrEmpty(id))
        //        {
        //            return Json(new { result = false, st = "error" }, JsonRequestBehavior.AllowGet);
        //        }
        //        var id_actual_pr = Convert.ToInt32(id);
        //        var productCode = db.w_actual_primary.Find(id_actual_pr).product;
        //        StringBuilder sql = new StringBuilder($"CALL spFGWMMS_GetFGgeneralDetail('{productCode}', '{poCode}', '{recevice_dt_start}', '{recevice_dt_end}', '{buyerCode}');");

        //        DataTable dt = new InitMethods().ReturnDataTableNonConstraints(sql);

        //        int total = dt.Rows.Count;
        //        var result = dt.AsEnumerable().OrderBy(x => x.Field<string>("end_production_dt"));
        //        return new InitMethods().ReturnJsonResultWithPaging(paging, total, result);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        [HttpPost]
        public ActionResult PrintFGGeneral()
        {
            string[] keys = Request.Form.AllKeys;

            var value = "";
            value = Request.Form[keys[0]];

            ViewData["Message"] = value;
            return View("~/Views/fgwms/Inventory/PrintGeneral.cshtml");
        }

        //public ActionResult qrGeneral(string mt_no)
        //{
        //    if (mt_no != "")
        //    {
        //        StringBuilder varname1 = new StringBuilder();
        //        varname1.Append("SELECT a.wmtid,a.mt_cd,b.mt_nm,CONCAT(ifnull(a.gr_qty,''),ifnull(b.unit_cd,'')) lenght,CONCAT(ifnull(b.width,0),'*',ifnull(a.gr_qty,0)) AS size,ifnull(b.spec,0) spec,a.mt_no, ");
        //        varname1.Append(" CONCAT((case when b.bundle_unit ='Roll' then  (a.gr_qty/b.spec) ELSE a.gr_qty END),' ',b.bundle_unit) qty, ");
        //        varname1.Append(" a.input_dt recevice_dt, ");
        //        varname1.Append("(SELECT dt_nm FROM comm_dt WHERE comm_dt.dt_cd=a.mt_sts_cd AND comm_dt.mt_cd='WHS005') sts_nm,a.lot_no,a.expore_dt,a.dt_of_receipt,a.expiry_dt ");
        //        varname1.Append("FROM w_material_info a ");
        //        varname1.Append("LEFT JOIN d_material_info  b ON a.mt_no=b.mt_no ");
        //        varname1.Append("WHERE a.lct_cd LIKE '003%' AND a.wmtid IN (" + mt_no + ")  ORDER BY a.mt_no, a.mt_cd ");
        //        return new InitMethods().ConvertDataTableToJsonAndReturn(varname1);
        //    }
        //    return View();
        //}
        public async Task<JsonResult> getbuyer_popup(Pageing paging)
        {
            try
            {
                var resqheader =Request.Headers;String[] resqheaderkey = resqheader.AllKeys;String[] resqheaderval = resqheader.GetValues("requestFrom");if (Session["authorize"] != null || resqheaderval[0]== "Mob")
                {

                    string productCode = Request["productCode"] == null ? "" : Request["productCode"].Trim();
                    string poCode = Request["poCode"] == null ? "" : Request["poCode"].Trim();
                    string recevice_dt_start = Request["recevice_dt_start"] == null ? "" : Request["recevice_dt_start"].Trim();
                    string recevice_dt_end = Request["recevice_dt_end"] == null ? "" : Request["recevice_dt_end"].Trim();

                    if (string.IsNullOrEmpty(recevice_dt_start))
                    {
                        recevice_dt_start = DateTime.MinValue.ToString("yyyyMMdd");
                    }
                    else
                    {
                        recevice_dt_start = recevice_dt_start.Replace("-", "");
                    }
                    if (string.IsNullOrEmpty(recevice_dt_end))
                    {
                        recevice_dt_end = DateTime.MaxValue.ToString("yyyyMMdd");
                    }
                    else
                    {
                        recevice_dt_end = recevice_dt_end.Replace("-", "");
                    }
                    var result = await _IFGMWServices.GetListBuyerQr(productCode, poCode, recevice_dt_start, recevice_dt_end);
                    int start = (paging.page - 1) * paging.rows;
                    int totals = result.Count();
                    int totalPages = (int)Math.Ceiling((float)totals / paging.rows);
                    IEnumerable<ByerQR> dataactual = result.Skip<ByerQR>(start).Take(paging.rows);
                    var jsonReturn = new
                    {
                        total = totalPages,
                        page = paging.page,
                        records = totals,
                        rows = dataactual
                    };
                    return Json(jsonReturn, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }
        //public void ExportFGgeneralToExcel(string productCode, string poCode, string recevice_dt_start, string recevice_dt_end)
        //{

        //    if (string.IsNullOrEmpty(productCode))
        //    {
        //        productCode = "";
        //    }
        //    if (string.IsNullOrEmpty(poCode))
        //    {
        //        poCode = "";
        //    }
        //    if (string.IsNullOrEmpty(recevice_dt_start))
        //    {
        //        recevice_dt_start = DateTime.MinValue.ToString("yyyyMMdd");
        //    }
        //    else
        //    {
        //        recevice_dt_start = recevice_dt_start.Replace("-", "");
        //    }
        //    if (string.IsNullOrEmpty(recevice_dt_end))
        //    {
        //        recevice_dt_end = DateTime.MaxValue.ToString("yyyyMMdd");
        //    }
        //    else
        //    {
        //        recevice_dt_end = recevice_dt_end.Replace("-", "");
        //    }
        //    StringBuilder sql = new StringBuilder($"CALL spFGWMS_ExportFGgeneralToExcel('{productCode}', '{poCode}', '{recevice_dt_start}', '{recevice_dt_end}');");
        //    DataTable dt = new InitMethods().ReturnDataTableNonConstraints(sql);

        //    DataSet ds = new DataSet();
        //    ds.Tables.Add(dt);
        //    ds.Tables[0].TableName = "FG_Inventory";

        //    using (XLWorkbook wb = new XLWorkbook())
        //    {
        //        var ws = wb.AddWorksheet(dt);

        //        ws.Columns().AdjustToContents();
        //        ws.Rows().AdjustToContents();
        //        //ws.Cells("A1").Value = "Product";
        //        //ws.Cells("B1").Value = "Lot Code";
        //        //ws.Cells("C1").Value = "Buyer";
        //        //ws.Cells("D1").Value = "PO";
        //        //ws.Cells("E1").Value = "Composite";
        //        //ws.Cells("F1").Value = "QTy (Roll/EA)";
        //        //ws.Cells("G1").Value = "Status";
        //        //ws.Cells("H1").Value = "Recevied Date";

        //        //ws.Cells("A1").Value = "Product";
        //        ws.Cells("A1").Value = "Lot Code";
        //        ws.Cells("B1").Value = "Buyer";
        //        ws.Cells("C1").Value = "PO";
        //        ws.Cells("D1").Value = "Composite";
        //        ws.Cells("E1").Value = "QTy (Roll/EA)";
        //        ws.Cells("F1").Value = "Status";
        //        ws.Cells("G1").Value = "Recevied Date";
        //        ws.Cells("H1").Value = "Product";
        //        ws.Cells("I1").Value = "Expiry Date";

        //        wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
        //        wb.Style.Alignment.ShrinkToFit = true;
        //        wb.Style.Font.Bold = true;

        //        Response.Clear();
        //        Response.Buffer = true;
        //        Response.Charset = "";
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.AddHeader("content-disposition", "attachment;filename=FG Inventory.xlsx");
        //        using (MemoryStream MyMemoryStream = new MemoryStream())
        //        {
        //            wb.SaveAs(MyMemoryStream);
        //            MyMemoryStream.WriteTo(Response.OutputStream);
        //            Response.Flush();
        //            Response.End();
        //        }
        //    }
        //    //return View("~/Views/WIP/Inventory/General.cshtml");
        //}

        #endregion FGWMSGeneral

        #region FG Mapping

        public ActionResult Mapping_Box()
        {
            return SetLanguage("~/Views/fgwms/Mapping/MappingBox.cshtml");
        }

        [HttpGet]
        public async Task<JsonResult> GetBuyerCode(string buyerCode, string materialCode, string productCode, Pageing paging)
        {
            if (string.IsNullOrEmpty(buyerCode))
            {
                buyerCode = "";
            }
            if (string.IsNullOrEmpty(materialCode))
            {
                materialCode = "";
            }
            if (string.IsNullOrEmpty(productCode))
            {
                productCode = "";
            }

            var stampTypeList = await _IFGMWServices.GetListStyleInfo();

            int start = (paging.page - 1) * paging.rows;
            int end = (paging.page - 1) * paging.rows + paging.rows;

            var tempData = await _IFGMWServices.GetListStampBuyerQr(buyerCode, productCode, start, paging.rows);

            var data = tempData.Select(x => new MappedProductModel
                            {
                                Id = x.id,
                                MaterialNo = x.product_code,
                                MaterialCode = x.md_cd,
                                BuyerCode = x.buyer_qr,
                                Quantity = x.qty.ToString(),
                                StampType = stampTypeList.Where(a => a.style_no.Contains(x.product_code)).Select(a => a.stamp_code).ToList().FirstOrDefault()
                            })
                            .ToList();

            int totals = await _IFGMWServices.CountStampBuyerQr(buyerCode, productCode);
            int totalPages = (int)Math.Ceiling((float)totals / paging.rows);
            var dataaaa = data.Skip<MappedProductModel>(start).Take(paging.rows);

            var jsonReturn = new
            {
                total = totalPages,
                page = paging.page,
                records = totals,
                rows = dataaaa
            };
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }


        //public async Task<JsonResult> BuyerCodeGEtProuct(string buyerCode)
        //{
        //    if (string.IsNullOrEmpty(buyerCode))
        //    {
        //        return Json(new { flag = false, message = "Quét mã tem gói để tiếp tục." }, JsonRequestBehavior.AllowGet);
        //    }
        //    try
        //    {


        //        var buyer = await _ITimsService.FindOneMaterialInfoById(buyerCode);// db.w_material_info.Where(x => x.buyer_qr == buyerCode).ToList();
        //        if (buyer == null)
        //        {
        //            return Json(new { flag = false, message = "Code không tồn tại." }, JsonRequestBehavior.AllowGet);
        //        }
        //        var id_actual = buyer.id_actual;

        //        string sql = " Select "
        //                    + " (SELECT product FROM w_actual_primary AS p  JOIN w_actual AS q  ON p.at_no = q.at_no   WHERE q.id_actual = x.id_actual) AS ProductNo "

        //                    + " FROM w_material_info AS x"

        //                    + " WHERE x.buyer_qr= '" + buyerCode + "' ";
        //        var data = db.Database.SqlQuery<MappedProductModel>(sql).ToList().Select(x => x.ProductNo);





        //        return Json(new { data, flag = true, message = "" }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { flag = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpPost]
        public async Task<JsonResult> BuyerCodeScanning(string buyerCode, string type)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob == "Mob")
            {

                if (string.IsNullOrEmpty(buyerCode))
                {
                    return Json(new { flag = false, message = "Quét mã tem gói để tiếp tục." }, JsonRequestBehavior.AllowGet);
                }
                try
                {
                    string vendor = "";
                    buyerCode = buyerCode.ToUpper();
                    if (buyerCode.Contains("DZIH"))
                    {
                        vendor = "DZIH";
                    }
                    else
                    {
                        vendor = "EA8D";
                    }
                    var data1 = await _IFGMWServices.ReturnGeneralfgByBuyerQR(buyerCode, vendor);

                    if (data1 != null)
                    {
                        if (data1.Status.Equals("001"))
                        {
                            return Json(new { data = data1, flag = true, message = "" }, JsonRequestBehavior.AllowGet);
                        }
                        if (data1.Status.Equals("101"))
                        {
                            return Json(new { flag = false, message = "Mã tem gói đã đưa vào kho" }, JsonRequestBehavior.AllowGet);
                        }

                    }
                    return Json(new { flag = false, message = "Mã tem gói không có trong kho" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { flag = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ShowingScanBuyerTemporary(List<FGShippingByExcelModel> tempList)
        {
            if (tempList == null)
            {
                return Json(new { flag = false, message = "Chưa chọn File excel" }, JsonRequestBehavior.AllowGet);
            }
            List<MappedProductModel> listData = new List<MappedProductModel>();
            bool check = true;
            if (tempList.Count > 0)
            {
                string checkBuyerType = tempList[0].Code;
                //var stampType = db.stamp_detail.Where(x => x.buyer_qr == checkBuyerType).SingleOrDefault().stamp_code;
                //var stampType = await _IFGMWServices.GetDataStampType(checkBuyerType);
                var stampType = await _IFGMWServices.FindGeneralfg(checkBuyerType);



                var html = "'" + "";

                for (int i = 0; i < tempList.Count(); i++)
                {
                    html += tempList[i].Code;

                    if (i != tempList.Count() - 1)
                    {
                        html += "'" + ',' + "'";
                    }
                }
                html = html + "'";


                    // kiểm tra buyer có tồn tại trong kho không
                    var rs1 = await _IFGMWServices.IsCheckBuyerExist(html);

                    //kiểm tra đã đóng gói chưa
                    var checkData = await _IFGMWServices.GetListTempData(html);
                    var buyer = rs1.ToList();
                    if (buyer.Count != tempList.Count() || checkData.Count() > 0)
                    {
                        foreach (var item in tempList)
                        {
                            var checkBuyerExsit = buyer.Where(x => x.BuyerCode.Contains(item.Code)).ToList();
                            if (checkBuyerExsit.Count() <= 0)
                            {
                                return Json(new { flag = false, message = $"Tem gói này chưa được đưa vào kho hoặc nhập sai mã tem <br/> {item.Code}  <br/> " }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                if(checkData!= null)
                                {
                                    var CheckTemDaDongGoi = checkData.Where(x => x.buyer_cd.Contains(item.Code)).FirstOrDefault();
                                    if (CheckTemDaDongGoi != null)
                                    {
                                        return Json(new { flag = false, message = $"tem gói: {CheckTemDaDongGoi.buyer_cd} đã được đóng thùng rồi <br/> Mã thùng : {CheckTemDaDongGoi.bx_no}" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }
                        }
                    }

                var checkStampDetail = await _IFGMWServices.FindGeneralfg1(html);
                    foreach (var item in checkStampDetail)
                    {
                        if (item.product_code != stampType.product_code)
                        {
                            return Json(new { flag = false, message = $"Lỗi tem {item.buyer_qr} gói không cùng loại product {item.product_code}." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    var data = rs1
                            .Select(x => new MappedProductModel
                            {
                                Id = x.Id,
                                //ProductNo = x.product_code,
                                ProductNo = x.ProductNo.Replace("-", ""),

                                //MaterialCode = x.mt_cd,
                                //MaterialNo = x.mt_no,
                                BuyerCode = x.BuyerCode,
                                Quantity = x.Quantity.ToString(),
                                Count = checkData.Count(),
                                StampType = stampType.product_code,
                                lot_date = x.lot_date
                            })
                            .ToList();

                    //var checkData1 = listData.Where(p => p.Id.Equals(data.Id)).ToList();
                    //if (checkData1.Count == 0)
                    //{
                    //    listData.Add(data);
                    //}

                //}
                if (check)
                {
                    return Json(new { listData = data, flag = true, message = "Thành Công !" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { flag = false, message = "Lỗi do không tìm thấy mã sản phẩm hoặc tem gói không cùng loại." }, JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json(new { flag = false, message = "File excel không hợp lệ." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> BoxCodeScanning(string boxCode)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob == "Mob")
            {

                if (string.IsNullOrEmpty(boxCode))
                {
                    return Json(new { flag = false, message = "Quét mã box để tiếp tục." }, JsonRequestBehavior.AllowGet);
                }
                try
                {
                    var boxlist = await _ITimsService.CheckTemGoimappingBoxwithboxno(boxCode);
                    if (boxlist.Count() >0)
                    {
                        return Json(new { flag = false, message = "Mã box này đã được chuyển đi." }, JsonRequestBehavior.AllowGet);
                    }
                    //List<IGrouping<string, w_box_mapping>> boxList = db.w_box_mapping.GroupBy(x => x.bx_no).Select(x => x).ToList();
                    //foreach (var item in boxList)
                    //{
                    //    if (item.Key == boxCode && item.ElementAtOrDefault(0)?.sts == "001")
                    //    {
                    //        return Json(new { flag = false, message = "Mã box này đã được chuyển đi." }, JsonRequestBehavior.AllowGet);
                    //    }
                    //}
                    var list = await _IFGMWServices.GetBoxCodeScanning(boxCode);
                    //StringBuilder sql = new StringBuilder($"CALL spfgwms_BoxCodeScanning('{boxCode}');");
                    //List<ScanBoxModel> list = new InitMethods().ConvertDataTable<ScanBoxModel>(sql);
                    if (list.Count > 0)
                    {
                        return Json(new { data = list[0], flag = true, message = "" }, JsonRequestBehavior.AllowGet);
                    }
                    ScanBoxModel data = new ScanBoxModel();
                    data.bx_no = boxCode;
                    data.totalQty = 0;
                    return Json(new { data, flag = true, message = "" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { flag = false, message = "Lỗi hệ thống." }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> CheckMappingBox(FGBoxMappingModel model)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob == "Mob")
            {

                StringBuilder sql = new StringBuilder();
                StringBuilder idsList = new StringBuilder();
                if (model.Wmtids == null || model.Wmtids.Count == 0)
                {
                    return Json(new { flag = false, message = "Vui lòng scan tem gói để tiếp tục." });

                }

                if (model.Wmtids != null)
                {
                    foreach (var item in model.Wmtids)
                    {
                        idsList.Append($"{item},");
                    }
                }
                string listId = new InitMethods().RemoveLastComma(idsList);

                try
                {

                    var rs = await _IFGMWServices.GetListDataToCheckMappingBox(listId);
                    var listData = rs.ToList();
                    if (listData.Count == 0)
                    {
                        return Json(new { flag = false, message = "Không tìm thấy sản phẩm." });
                    }

                    var minLOTDateInList = listData.LastOrDefault();
                    StringBuilder idStampDetail = new StringBuilder();
                    string proCode = minLOTDateInList.product_code;
                    string stampCode = minLOTDateInList.stamp_code;

                    foreach (var item in listData)
                    {
                        idStampDetail.Append($"{item.id},");
                    }
                    string listIdStampDetail = new InitMethods().RemoveLastComma(idStampDetail);

                    var rs1 = await _IFGMWServices.GetListLotDateFromStampDetail(proCode, stampCode, listIdStampDetail);
                    var str = rs1.ToList();
                    if (str.Count == 0)
                    {
                        return Json(new { flag = true, message = "" });
                    }
                    else
                    {
                        string minLotDate = str.FirstOrDefault().lot_date;
                        if (minLotDate == "" || minLotDate == null)
                        {
                            //return Json(new { result = false, message = "Lot no đang rỗng, vui lòng nhập lot no bên tồn kho FG" });
                            return Json(new { result = true, exist = false, message = "" });
                        }
                        if (DateTime.ParseExact(minLotDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(minLOTDateInList.lot_date, "yyyy-MM-dd", CultureInfo.InvariantCulture))
                        {
                            return Json(new { result = true, exist = false, message = "" });
                        }
                        else
                        {
                            return Json(new { result = true, exist = true, message = "" });
                        }
                    }

                }
                catch (Exception e)
                {
                    return Json(new { result = false, message = "Lỗi hệ thống." });
                }
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> MappingBox(FGBoxMappingModel model)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob == "Mob")
            {

                StringBuilder sql = new StringBuilder();
                StringBuilder ids = new StringBuilder();
                if (model.Wmtids == null || model.Wmtids.Count == 0)
                {
                    return Json(new { flag = false, message = "Vui lòng scan tem gói để tiếp tục." });

                }
                if (model.Wmtids != null)
                {
                    foreach (var item in model.Wmtids)
                    {
                        ids.Append($"{item},");

                    }
                }

                string listId = new InitMethods().RemoveLastComma(ids);
                var rs = await _IFGMWServices.GetListDataToCheckMappingBox(listId);
                var listData = rs.ToList();

                int totalQty = 0;
                string buyerQRCode = listData.LastOrDefault().buyer_qr;
                //var stampdetail = await _IFGMWServices.GetStampDetail(model.ProductCode);
                var product = await _IFGMWServices.GetStyleInfoReplace(model.ProductCode.Replace("-", ""));
                if (product == null)
                {
                    return Json(new { flag = false, message = "Product này chưa được tạo bên DMS" });
                }
                //KIỂM TRA product có phải bất quy tắc
                var typeProduct = product.productType;
                if (typeProduct.Equals("0"))
                {
                    //LẤY PRODUCT Ở TỪ TEM GÓI để tạo tem thùng
                    //int FindPositionDZIH = buyerQRCode.IndexOf("DZIH");

                    //if (FindPositionDZIH < 1)
                    //{
                    //    return Json(new { result = false, message = "Tem gói không hợp lệ vui lòng thử lại" }, JsonRequestBehavior.AllowGet);
                    //}
                    //string ProductNew = buyerQRCode.Substring(0, FindPositionDZIH);

                    if (product == null)
                    {
                        return Json(new { result = false, message = "Product chưa được đăng kí" }, JsonRequestBehavior.AllowGet);
                    }
                    var stampdetail = await _IFGMWServices.GetStampDetail(product.style_no);
                    if (stampdetail == null)
                    {
                        return Json(new { flag = false, message = "Vui lòng tạo một tem gói ở hệ thống MES để làm tiền đề tạo tem thùng" });
                    }
                    if (stampdetail == null)
                    {
                        return Json(new { flag = false, message = "Vui lòng tạo một tem gói ở hệ thống MES để làm tiền đề tạo tem thùng" });
                    }

                    string stampCode = stampdetail.stamp_code;

                    //LẤY THÔNG TIN ĐỂ TẠO MÃ BOX
                    string productCode = stampdetail.product_code;
                    string exp = product.expiry_month;
                    int i;
                    int j = 0;
                    bool success = Int32.TryParse(exp, out i);
                    if (success)
                    {
                        j = i;
                    }

                    string vendorCode = stampdetail.vendor_code;
                    string vendorLine = stampdetail.vendor_line;
                    string labelPrinter = stampdetail.label_printer;
                    string sampleCode = stampdetail.is_sample;
                    string pcnCode = stampdetail.pcn;

                    //CHỌN 1 LOT DATE BẤT KỲ TRONG LIST TEM GÓI ĐƯỢC ĐÓNG THÙNG
                    //string mfgDate = new CreateBuyerQRController().DateFormatByShinsungRule(stampdetail.lot_date); cũ



                    DateTime dt = DateTime.Now; //Today
                    string day_now = dt.ToString("yyyyMMdd");

                    string mfgDate = new InitMethods().DateFormatByShinsungRule(day_now);//lấy ngày tạo mã thùng lớn
                    string tempBoxQR = string.Concat(productCode.Replace("-", ""), vendorCode, vendorLine, labelPrinter, sampleCode, pcnCode, mfgDate);


                    if (vendorCode == null)
                    {
                        var gtri5 = buyerQRCode.Substring(10, 8);
                        tempBoxQR = string.Concat(productCode.Replace("-", ""), gtri5, mfgDate);
                    }

                    //StringBuilder countSql = new StringBuilder();
                    ////countSql.Append($" SELECT MAX(a.bmno) AS id FROM w_box_mapping a WHERE a.bx_no IS NOT NULL AND a.bx_no LIKE '{tempBoxQR}%' GROUP BY a.bx_no;");
                    //countSql.Append($" SELECT SUBSTRING(max(a.bx_no), LENGTH('{tempBoxQR}')+1, 3) AS bientang FROM w_box_mapping a WHERE a.bx_no IS NOT NULL AND a.bx_no LIKE '{tempBoxQR}%' ORDER BY a.bx_no DESC LIMIT 1; ");
                    //List<ListIntModel> listInt = new InitMethods().ConvertDataTableToList<ListIntModel>(countSql);

                    var rs1 = await _IFGMWServices.GetListIntModel(tempBoxQR);
                    var listInt = rs1.ToList();
                    int num = 1;
                    if (listInt.Count() > 0)
                    {
                        if (listInt[0].bientang != null && listInt[0].bientang != "")
                        {
                            num = int.Parse(listInt[0].bientang) + 1;
                        }
                    }

                    foreach (var item in listData)
                    {
                        totalQty += int.Parse(item.standard_qty);
                    }
                    string boxQR = string.Concat(tempBoxQR, new InitMethods().BuyerQRSerialFormat(num), new InitMethods().ProductQuantityFormatForBoxQR(totalQty), new InitMethods().ChangeNumberToCharacter(j).ToString());

                    try
                    {
                        StringBuilder idr = new StringBuilder();
                        foreach (var item in listData)
                        {
                            idr.Append("" + item.buyer_qr + ",");
                        }
                        string idStr = new InitMethods().RemoveLastComma(idr);
                        //StringBuilder executeSql = new StringBuilder($" UPDATE stamp_detail SET box_code = '{boxQR}' WHERE buyer_qr IN ({idStr}) ;");
                        //db.Database.ExecuteSqlCommand(executeSql.ToString());
                        await _IFGMWServices.UpdateStampDetail(idStr, boxQR);
                        StringBuilder listID = new StringBuilder();
                        if (model.Wmtids != null)
                        {
                            foreach (var item in model.Wmtids)
                            {
                                listID.Append(item + ",");

                            }
                        }

                        string IDStr = new InitMethods().RemoveLastComma(listID).Replace("'", "");
                        string userId = Session["userid"] == null ? "" : Session["userid"].ToString();

                        var Id = await _IFGMWServices.InsertIntoBoxMapping(boxQR, userId, IDStr, product.style_no);
                        await _IFGMWServices.UpdatePartialGeneralfg(IDStr);
                        var datareturn = await _IFGMWServices.GetBoxMapping(boxQR);
                        return Json(new { data = datareturn, result = true, message = "Tạo tem thùng thành công." }, JsonRequestBehavior.AllowGet);

                    }
                    catch (Exception e)
                    {
                        return Json(new { result = false, message = "Lỗi hệ thống." }, JsonRequestBehavior.AllowGet);
                        throw e;
                    }
                }
                else
                {
                    //LẤY PRODUCT Ở TỪ TEM GÓI để tạo tem thùng
                    //int FindPositionDZIH = buyerQRCode.IndexOf("DZIH");

                    //if (FindPositionDZIH < 1)
                    //{
                    //    return Json(new { result = false, message = "Tem gói không hợp lệ vui lòng thử lại" }, JsonRequestBehavior.AllowGet);
                    //}
                    var vendor_code = "DZIH";
                    int FindPositionDZIH = buyerQRCode.IndexOf("DZIH");
                    if (FindPositionDZIH < 1)
                    {
                        FindPositionDZIH = buyerQRCode.IndexOf("EA8D");
                        vendor_code = "EA8D";
                    }
                    if (FindPositionDZIH < 1)
                    {
                        return Json(new { result = false, message = "Tem gói không hợp lệ vui lòng thử lại" }, JsonRequestBehavior.AllowGet);
                    }
                    string ProductNew = buyerQRCode.Substring(0, FindPositionDZIH);
                    var CheckProduct = await _IFGMWServices.GetStyleInfoReplace(ProductNew);
                    if (CheckProduct == null)
                    {
                        return Json(new { result = false, message = "Product chưa được đăng kí" }, JsonRequestBehavior.AllowGet);
                    }
                   var stampdetail = await _IFGMWServices.GetStampDetail(CheckProduct.style_no);
                    if (stampdetail == null)
                    {
                        return Json(new { flag = false, message = "Vui lòng tạo một tem gói ở hệ thống MES để làm tiền đề tạo tem thùng" });
                    }
                    //LẤY THÔNG TIN ĐỂ TẠO MÃ BOX
                    string productCode = stampdetail.product_code;
                    string exp = product.expiry_month;
                    int i;
                    int j = 0;
                    bool success = Int32.TryParse(exp, out i);
                    if (success)
                    {
                        j = i;
                    }

                    string vendorCode = vendor_code;
                    string vendorLine = stampdetail.vendor_line;
                    string labelPrinter = stampdetail.label_printer;
                    string sampleCode = stampdetail.is_sample;
                    string pcnCode = stampdetail.pcn;

                    //CHỌN 1 LOT DATE BẤT KỲ TRONG LIST TEM GÓI ĐƯỢC ĐÓNG THÙNG
                    //string mfgDate = new CreateBuyerQRController().DateFormatByShinsungRule(stampdetail.lot_date); cũ



                    //var lastedItem = listData.LastOrDefault();
                    //string day_now = lastedItem != null ? lastedItem.lot_date.Replace("-", "") : string.Empty;

                    DateTime dt = DateTime.Now; //Today
                    string day_now = dt.ToString("yyyyMMdd");

                    string mfgDate = new InitMethods().DateFormatByShinsungRule(day_now);//lấy ngày tạo mã thùng lớn
                    string tempBoxQR = string.Concat(productCode.Replace("-", ""), vendorCode, vendorLine, labelPrinter, sampleCode, pcnCode, mfgDate);


                    if (vendorCode == null)
                    {
                        var gtri5 = buyerQRCode.Substring(10, 8);
                        tempBoxQR = string.Concat(productCode.Replace("-", ""), gtri5, mfgDate);
                    }

                    var rs1 = await _IFGMWServices.GetListIntModel(tempBoxQR);

                    var listInt = rs1.ToList();
                    int num = 1;
                    if (listInt.Count() > 0)
                    {
                        if (listInt[0].bientang != null && listInt[0].bientang != "")
                        {
                            num = int.Parse(listInt[0].bientang) + 1;
                        }
                    }


                    foreach (var item in listData)
                    {
                        totalQty += int.Parse(item.standard_qty);
                    }
                    string boxQR = string.Concat(tempBoxQR, new InitMethods().BuyerQRSerialFormat(num), new InitMethods().ProductQuantityFormatForBoxQR(totalQty), new InitMethods().ChangeNumberToCharacter(j).ToString());

                    try
                    {
                        StringBuilder idr = new StringBuilder();
                        foreach (var item in listData)
                        {
                            idr.Append("" + item.buyer_qr + ",");
                        }
                        string idStr = new InitMethods().RemoveLastComma(idr);
                        //StringBuilder executeSql = new StringBuilder($" UPDATE stamp_detail SET box_code = '{boxQR}' WHERE buyer_qr IN ({idStr}) ;");
                        //db.Database.ExecuteSqlCommand(executeSql.ToString());
                        await _IFGMWServices.UpdateStampDetail(idStr, boxQR);
                        StringBuilder listID = new StringBuilder();
                        if (model.Wmtids != null)
                        {
                            foreach (var item in model.Wmtids)
                            {
                                listID.Append(item + ",");

                            }
                        }

                        string IDStr = new InitMethods().RemoveLastComma(listID).Replace("'", "");
                        string userId = Session["userid"] == null ? "" : Session["userid"].ToString();

                        var Id = await _IFGMWServices.InsertIntoBoxMapping(boxQR, userId, IDStr, product.style_no);
                        await _IFGMWServices.UpdatePartialGeneralfg(IDStr);
                        var datareturn = await _IFGMWServices.GetBoxMapping(boxQR);
                        return Json(new { data = datareturn, result = true, message = "Tạo tem thùng thành công." }, JsonRequestBehavior.AllowGet);

                    }
                    catch (Exception e)
                    {
                        return Json(new { result = false, message = "Lỗi hệ thống." }, JsonRequestBehavior.AllowGet);
                        throw e;
                    }
                }
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> UnMappingBox(string boxCode)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob == "Mob")
            {

                if (string.IsNullOrEmpty(boxCode))
                {
                    return Json(new { flag = false, message = "No box code selected" });
                }
                //StringBuilder sql = new StringBuilder($"CALL spfgwms_UnMappingBox('{boxCode}');");
                try
                {
                    await _IFGMWServices.UnMappingBox(boxCode);
                    return Json(new { flag = true, message = "Un-Mapping Thành công" });
                }
                catch (Exception)
                {
                    return Json(new { flag = false, message = "Lỗi hệ thống" });
                }
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> UnMappingBuyer(string buyerCode)
        {
            if (string.IsNullOrEmpty(buyerCode))
            {
                return Json(new { flag = false, message = "Không tìm được mã tem gói trong cơ sở dữ liệu." });
            }
            //StringBuilder sql = new StringBuilder($"CALL spfgwms_UnMappingBuyer('{buyerCode}');");
            try
            {
                await _IFGMWServices.UnMappingBuyer(buyerCode);
                //db.Database.ExecuteSqlCommand(sql.ToString());
                return Json(new { flag = true, message = "Xóa tem gói thành công." });
            }
            catch (Exception)
            {
                return Json(new { flag = false, message = "Lỗi hệ thống." });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetSumMappedBoxes(Pageing paging, string boxCode, string ProductCode, string sDate, string BuyerCode)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob == "Mob")
            {
                 //var rs = await _IFGMWServices.GetListBoxMapping(boxCode, ProductCode, sDate, BuyerCode);
                //var jsonResult = Json(rs, JsonRequestBehavior.AllowGet);
                //jsonResult.MaxJsonLength = int.MaxValue;
                //return jsonResult;

                int start = (paging.page - 1) * paging.rows;
                var datapoppupStaff = await _IFGMWServices.GetListBoxMapping1(boxCode, ProductCode, sDate, BuyerCode, start/*, paging.rows*/);
              //  int totals = await _IFGMWServices.CountListBoxMapping(boxCode, ProductCode, sDate, BuyerCode);

                int totalPages = (int)Math.Ceiling((float)datapoppupStaff.Count() / paging.rows);
                var rowsData = datapoppupStaff.Skip((paging.page - 1)).Take(paging.rows);
                var result = new
                {
                    total = totalPages,
                    page = paging.page,
                    records = datapoppupStaff.Count(),
                    rows = rowsData
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetMappedProducts(string boxCodeMapped, string buyerCode)
        {
            //StringBuilder sql = new StringBuilder($"CALL spfgwms_GetMappedProducts('{boxCodeMapped}');");

            var tempData = await _IFGMWServices.GetListMappedProducts(boxCodeMapped, buyerCode);
            var data = tempData
                            .Select(x => new MappedProductModel
                            {
                                Id = x.Id,
                                MaterialCode = x.MaterialCode,
                                ProductNo = x.ProductNo,
                                BuyerCode = x.BuyerCode,
                                Quantity = x.Quantity,
                                lot_date =x.lot_date
                            }).ToList();
            return Json(new { data, flag = true, message = "" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMappedProducts_old(string boxCodeMapped)
        {
            StringBuilder sql = new StringBuilder($"CALL spfgwms_GetMappedProducts('{boxCodeMapped}');");

            var tempData = new InitMethods().ConvertDataTable<MappedProductModel>(sql);

            Dictionary<string, int> checkCount = new Dictionary<string, int>();
            Dictionary<string, int> checkTotal = new Dictionary<string, int>();

            var listCount = from x in tempData
                            group x by x.BuyerCode into g
                            let count = g.Count()
                            orderby count descending
                            select new { Value = g.Key, Count = count };
            foreach (var x in listCount)
            {
                checkCount.Add(x.Value, x.Count);
            }

            var listTotal = (from x in tempData
                             group x by x.BuyerCode into g
                             select new
                             {
                                 Value = g.Key,
                                 Total = g.Sum(f => f.Quantity == null ? 0 : int.Parse(f.Quantity))
                             }).ToList();
            foreach (var x in listTotal)
            {
                checkTotal.Add(x.Value, x.Total);
            }

            var data = tempData
                            .Select(x => new MappedProductModel
                            {
                                Id = x.Id,
                                MaterialCode = x.MaterialCode,
                                MaterialNo = x.MaterialNo,
                                BuyerCode = x.BuyerCode,
                                Quantity = x.Quantity,
                                Count = checkCount.Where(a => a.Key == x.BuyerCode).Select(a => a.Value).FirstOrDefault(),
                                Total = checkTotal.Where(a => a.Key == x.BuyerCode).Select(a => a.Value).FirstOrDefault()
                            })
                            .ToList();
            return Json(new { data, flag = true, message = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult exportTemToExcel()
        {

            StringBuilder varname1 = new StringBuilder();

            varname1.Append("SELECT  	SELECT	a.bx_no , a.buyer_cd ");
            varname1.Append("	FROM w_box_mapping a ");
            varname1.Append(" JOIN stamp_detail AS b 	ON a.buyer_cd = b.buyer_qr ");


            varname1.Append("	order by a.bmno DESC ");


            DataTable dt = new InitMethods().ReturnDataTableNonConstraints(varname1);


            List<w_box_mapping> listTotal = new InitMethods().ConvertDataTable<w_box_mapping>(varname1);

            DataTable datatb = new InitMethods().ConvertListToDataTable(listTotal.ToList());


            DataSet ds2 = new DataSet();

            ds2.Tables.Add(datatb);
            ds2.Tables[0].TableName = "Export Tem";


            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.AddWorksheet(datatb);

                ws.Columns().AdjustToContents();



                ws.Cells("B1").Value = "Large Package";
                ws.Cells("C1").Value = "SmallPackage";

                //ws.Cells("I1").Value = "Status";
                //ws.Cells("L1").Value = "Machine";
                ws.Columns("A").Hide(); //an cot I

                ws.Columns("E").Hide(); //an cot I
                ws.Columns("F").Hide(); //an cot I
                ws.Columns("J").Hide(); //an cot I
                ws.Columns("M").Hide(); //an cot I
                ws.Columns("N").Hide(); //an cot I



                ws.Rows().AdjustToContents();
                //ws.Name = "111";
                //Worksheets
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Alignment.ShrinkToFit = true;
                //wb.c

                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=History.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return View("~/Views/fgwms/Mapping/MappingBox.cshtml");
        }

        #endregion FG Mapping

        #region FG_ProductLot

        public ActionResult FGProductLot()
        {
            return SetLanguage("~/Views/fgwms/Inventory/ProductLot.cshtml");
        }

        public JsonResult getFGProductLot(Pageing paging)
        {
            string mt_no = Request["mt_no"] == null ? "" : Request["mt_no"].Trim();
            string buyer_gr = Request["buyer_gr"] == null ? "" : Request["buyer_gr"].Trim();
            string recevice_dt_start = Request["recevice_dt_start"] == null ? "" : Request["recevice_dt_start"].Trim();
            string recevice_dt_end = Request["recevice_dt_end"] == null ? "" : Request["recevice_dt_end"].Trim();

            var dateConvert = new DateTime();

            if (DateTime.TryParse(recevice_dt_end, out dateConvert))
            {
                recevice_dt_end = dateConvert.ToString("yyyy/MM/dd");
            }
            if (DateTime.TryParse(recevice_dt_start, out dateConvert))
            {
                recevice_dt_start = dateConvert.ToString("yyyy/MM/dd");
            }



            StringBuilder varname1 = new StringBuilder();
            varname1.Append(" SELECT a.bmno,a.bx_no, a.gr_qty qty, ");
            varname1.Append(" a.sts_nm, ");
            varname1.Append(" a.buyer_qr,a.mapping_dt ");
            varname1.Append(" FROM view_fgproduct a ");
            //varname1.Append(" GROUP BY a.bx_no ");
            varname1.Append(" WHERE ('" + mt_no + "'='' OR  a.bx_no like '%" + mt_no + "%' ) ");
            varname1.Append(" AND ('" + buyer_gr + "'='' OR  a.buyer_qr like '%" + buyer_gr + "%' ) ");

            varname1.Append(" AND ('" + recevice_dt_start + "'='' OR DATE_FORMAT(a.mapping_dt,'%Y/%m/%d') >= DATE_FORMAT('" + recevice_dt_start + "','%Y/%m/%d')) ");
            varname1.Append(" AND ('" + recevice_dt_end + "'='' OR DATE_FORMAT(a.mapping_dt,'%Y/%m/%d') <= DATE_FORMAT('" + recevice_dt_end + "','%Y/%m/%d')) ");

            //varname1.Append("LEFT JOIN d_material_info  b ON a.mt_no=b.mt_no ");
            DataTable dt = new InitMethods().ReturnDataTableNonConstraints(varname1);

            int total = dt.Rows.Count;
            var result = dt.AsEnumerable().OrderByDescending(x => x.Field<int>("bmno"));
            return new InitMethods().ReturnJsonResultWithPaging(paging, total, result);
        }

        public async Task<JsonResult> get_bieudo_tonghop(string value)
        {
            try
            {

                var resqheader = Request.Headers;
                string[] resqheaderkey = resqheader.AllKeys;
                string[] resqheaderval = resqheader.GetValues("requestFrom");
                string resqheadervalmob = "";
                if (resqheaderval != null)
                {
                    resqheadervalmob = resqheaderval[0];
                }
                if (Session["authorize"] != null || resqheadervalmob == "Mob")
                {

                    //check_tontai trong box
                    var html = "";
                    var box = await _IFGMWServices.Getlistwboxmappingwithboxno(value);// db.w_box_mapping.Where(x => x.bx_no == value).ToList();
                    if (box.Count > 0)
                    {
                        html += "<ul><li><a>" + box.FirstOrDefault().bx_no + "</a><ul>";
                        foreach (var item in box)
                        {
                            html += "<li>";
                            html += "<a>" + item.mt_cd + "</a>";
                            html += "</li>";
                        }
                        html += "</ul></li></ul>";
                        return Json(new { result = false, kq = html }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { result = false, message = "Can not View " }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { result = false, message = "Can not View " }, JsonRequestBehavior.AllowGet);
            }
        }

        //public JsonResult get_bieudo_tonghop6(string value)
        //{
        //    try
        //    {
        //        var html = "";
        //        var mt_lot = "";

        //        var box = db.w_box_mapping.Where(x => x.bx_no == value).ToList();
        //        if (!string.IsNullOrEmpty(value))
        //        {
        //            var id_box = box[0].bmno;
        //            html += "";//cha
        //            html += "<li><span class='caret1'  >" + value + "(Box)</span>";//b1

        //            html += "<ul class='nested'>";
        //            foreach (var item_box in box)
        //            {
        //                html += "<li><span class='caret1'>" + item_box.buyer_cd + " </span>(Buyer)";//b2
        //                var md_cd_buyer = db.w_material_info.Where(x => x.buyer_qr == item_box.buyer_cd).ToList();

        //                html += "<ul class='nested'>";

        //                //gộp các DV lại với nhau
        //                var dspartTolist_OQC = new List<FG_DV>();

        //                foreach (var data_ti222 in md_cd_buyer)
        //                {
        //                    var model = new FG_DV();
        //                    model.id = data_ti222.wmtid;
        //                    model.id_actual = data_ti222.id_actual;
        //                    model.mt_cd = data_ti222.mt_cd;
        //                    dspartTolist_OQC.Add(model);
        //                }
        //                var list_new_OQC = KT_Lot_dv(dspartTolist_OQC);
        //                List<FG_DV> myList_OQC = list_new_OQC.Cast<FG_DV>().ToList();

        //                foreach (var item_cd_buyer in myList_OQC)
        //                {
        //                    mt_lot = KT_MTLot_DV(item_cd_buyer.mt_cd);
        //                    html += "<li><span class='caret1'>" + item_cd_buyer.mt_cd + " </span>(OQC) <span class='detail_fg'  data-id= " + id_box + " data-mt_lot= " + mt_lot + " onclick='PpDetailFGProductLot(this);'><i class='fa fa-info'></i></span>";//b2
        //                                                                                                                                                                                                                                                     //hàm kiểm tra có phải DV không

        //                    //Start b3
        //                    var data_ti = (from a in db.w_material_mapping
        //                                   join b in db.w_material_info
        //                                   on a.mt_cd equals b.mt_cd
        //                                   where a.mt_lot == mt_lot

        //                                   select new
        //                                   {
        //                                       mt_cd = a.mt_cd,
        //                                       id = a.wmmid,
        //                                       id_actual = b.id_actual
        //                                   }).ToList();

        //                    //gộp các DV lại với nhau
        //                    var dspartTolist = new List<FG_DV>();

        //                    foreach (var data_ti222 in data_ti)
        //                    {
        //                        var model = new FG_DV();
        //                        model.id = data_ti222.id;
        //                        model.id_actual = data_ti222.id_actual;
        //                        model.mt_cd = data_ti222.mt_cd;
        //                        dspartTolist.Add(model);
        //                    }
        //                    var list_new = KT_Lot_dv(dspartTolist);
        //                    List<FG_DV> myList = list_new.Cast<FG_DV>().ToList();

        //                    html += "<ul class='nested'>";//b3
        //                    foreach (var itemdata_ti in myList)
        //                    {
        //                        html += "<li><span class='caret1'>" + itemdata_ti.mt_cd + " </span>(TIQC)<span class='detail_fg'  data-id= " + id_box + " data-mt_lot= " + itemdata_ti.mt_cd + " onclick='PpDetailFGProductLot(this);'><i class='fa fa-info'></i></span>";//b3
        //                                                                                                                                                                                                                                                                  //hàm kiểm tra có phải DV không
        //                        mt_lot = KT_MTLot_DV(itemdata_ti.mt_cd);
        //                        //Start b4
        //                        var data_ti2 = (from a in db.w_material_mapping
        //                                        join b in db.w_material_info
        //                                        on a.mt_cd equals b.mt_cd
        //                                        where a.mt_lot == mt_lot

        //                                        select new
        //                                        {
        //                                            mt_cd = a.mt_cd,
        //                                            id = a.wmmid,
        //                                            id_actual = b.id_actual
        //                                        }).ToList();

        //                        var dspartTolist1 = new List<FG_DV>();
        //                        foreach (var data_ti222 in data_ti2)
        //                        {
        //                            var model = new FG_DV();
        //                            model.id = data_ti222.id;
        //                            model.id_actual = data_ti222.id_actual;
        //                            model.mt_cd = data_ti222.mt_cd;
        //                            dspartTolist1.Add(model);
        //                        }
        //                        //gộp các DV lại với nhau

        //                        #region cmt

        //                        //var dsdv_new1 = new List<w_material_info>();

        //                        //foreach (var item_data_ti2 in data_ti2)
        //                        //{
        //                        //    var dsdv_new = new w_material_info();
        //                        //    if (item_data_ti2.mt_cd.Contains("-DV"))
        //                        //    {
        //                        //        var kt_co_dv = dsdv_new1.Where(x => x.mt_cd.Contains("-DV")).ToList();

        //                        //        if (kt_co_dv.Count > 0)
        //                        //        {
        //                        //            var mt_cd_cong_don = kt_co_dv.FirstOrDefault();
        //                        //            if (!mt_cd_cong_don.mt_cd.Contains(item_data_ti2.mt_cd))
        //                        //            {
        //                        //                mt_cd_cong_don.mt_cd = mt_cd_cong_don.mt_cd + "," + item_data_ti2.mt_cd;
        //                        //                break;
        //                        //            }

        //                        //        }
        //                        //        else
        //                        //        {
        //                        //            dsdv_new.mt_cd = item_data_ti2.mt_cd;
        //                        //            dsdv_new.id_actual = item_data_ti2.id_actual;

        //                        //            dsdv_new1.Add(dsdv_new);
        //                        //        }

        //                        //    }
        //                        //    else
        //                        //    {
        //                        //        dsdv_new.mt_cd = item_data_ti2.mt_cd;
        //                        //        dsdv_new.id_actual = item_data_ti2.id_actual;
        //                        //        dsdv_new1.Add(dsdv_new);
        //                        //    }

        //                        //}

        //                        #endregion cmt

        //                        var htht = KT_Lot_dv(dspartTolist1);

        //                        List<FG_DV> myList1 = htht.Cast<FG_DV>().ToList();

        //                        var namecongdonamms = "";
        //                        if (myList1.Count > 0)
        //                        {
        //                            var id_actual_mms = data_ti2[0].id_actual;

        //                            var congdoan_mms = db.w_actual.Where(x => x.id_actual == id_actual_mms).ToList();
        //                            if (congdoan_mms.Count > 0)
        //                            {
        //                                namecongdonamms = congdoan_mms.FirstOrDefault().name;
        //                            }
        //                        }

        //                        html += "<ul class='nested'>";//b4
        //                        foreach (var item_data_ti2 in myList1)
        //                        {
        //                            //hàm kiểm tra có phải DV không
        //                            mt_lot = KT_MTLot_DV(item_data_ti2.mt_cd);

        //                            html += "<li><span class='caret1 buoc4'>" + item_data_ti2.mt_cd + "(" + namecongdonamms + ")</span><span class='detail_fg'  data-id= " + id_box + " data-mt_lot= " + item_data_ti2.mt_cd + " onclick='PpDetailFGProductLot(this);'><i class='fa fa-info'></i></span>";//b4

        //                            //Start b5
        //                            var data_mms1 = (from a in db.w_material_mapping
        //                                             join b in db.w_material_info
        //                                             on a.mt_cd equals b.mt_cd
        //                                             where a.mt_lot == mt_lot

        //                                             select new
        //                                             {
        //                                                 mt_cd = a.mt_cd,
        //                                                 id = a.wmmid,
        //                                                 id_actual = b.id_actual
        //                                             }).ToList();

        //                            var namecongdona = "";
        //                            if (data_mms1.Count > 0)
        //                            {
        //                                var id_actual = data_mms1[0].id_actual;

        //                                var congdoan = db.w_actual.Where(x => x.id_actual == id_actual).ToList();
        //                                if (congdoan.Count > 0)
        //                                {
        //                                    namecongdona = congdoan.FirstOrDefault().name;
        //                                }
        //                            }

        //                            html += "<ul class='nested'>";//b5
        //                            foreach (var item_data_mms1 in data_mms1)
        //                            {
        //                                //hàm kiểm tra có phải DV không
        //                                mt_lot = KT_MTLot_DV(item_data_mms1.mt_cd);
        //                                //start b6
        //                                var data_mms2 = (from a in db.w_material_mapping
        //                                                 join b in db.w_material_info
        //                                                 on a.mt_cd equals b.mt_cd
        //                                                 where a.mt_lot == mt_lot

        //                                                 select new
        //                                                 {
        //                                                     mt_cd = a.mt_cd,
        //                                                     id = a.wmmid,
        //                                                     id_actual = b.id_actual
        //                                                 }).ToList();

        //                                var namecongdona1 = "";
        //                                if (data_mms2.Count > 0)
        //                                {
        //                                    var id_actual1 = data_mms2[0].id_actual;

        //                                    var congdoan1 = db.w_actual.Where(x => x.id_actual == id_actual1).ToList();
        //                                    if (congdoan1.Count > 0)
        //                                    {
        //                                        namecongdona1 = congdoan1.FirstOrDefault().name;
        //                                    }
        //                                }

        //                                if (data_mms2.Count == 0)
        //                                {
        //                                    html += "<li><span class='caret1 nosub'>" + item_data_mms1.mt_cd + "(" + namecongdona + ")</span>";//b5
        //                                }
        //                                else
        //                                {
        //                                    html += "<li><span class='caret1'>" + item_data_mms1.mt_cd + "(" + namecongdona + ")</span>";//b5
        //                                }

        //                                html += "<ul class='nested'>";//b6
        //                                foreach (var itemdata_mms2 in data_mms2)
        //                                {
        //                                    html += "<li><span class='caret1'>" + itemdata_mms2.mt_cd + "(" + namecongdona1 + ")</span>";//b6

        //                                    //hàm kiểm tra có phải DV không
        //                                    mt_lot = KT_MTLot_DV(itemdata_mms2.mt_cd);
        //                                    //Start b7
        //                                    var data_mms3 = (from a in db.w_material_mapping
        //                                                     join b in db.w_material_info
        //                                                     on a.mt_cd equals b.mt_cd
        //                                                     where a.mt_lot == mt_lot

        //                                                     select new
        //                                                     {
        //                                                         mt_cd = a.mt_cd,
        //                                                         id = a.wmmid,

        //                                                         id_actual = b.id_actual
        //                                                     }).ToList();

        //                                    var namecongdona3 = "";
        //                                    if (data_mms3.Count > 0)
        //                                    {
        //                                        var id_actual3 = data_mms3[0].id_actual;

        //                                        var congdoan3 = db.w_actual.Where(x => x.id_actual == id_actual3).ToList();
        //                                        if (congdoan3.Count > 0)
        //                                        {
        //                                            namecongdona3 = congdoan3.FirstOrDefault().name;
        //                                        }
        //                                    }

        //                                    html += "<ul class='nested'>";//b7
        //                                    foreach (var item_mms3 in data_mms3)
        //                                    {
        //                                        //hàm kiểm tra có phải DV không
        //                                        mt_lot = KT_MTLot_DV(item_mms3.mt_cd);
        //                                        var data_mms4 = (from a in db.w_material_mapping
        //                                                         join b in db.w_material_info
        //                                                         on a.mt_cd equals b.mt_cd
        //                                                         where a.mt_lot == mt_lot

        //                                                         select new
        //                                                         {
        //                                                             mt_cd = a.mt_cd,
        //                                                             id = a.wmmid
        //                                                         }).ToList();

        //                                        if (data_mms4.Count > 0)
        //                                        {
        //                                            html += "<li><span class='caret1 '>" + item_mms3.mt_cd + "(" + namecongdona3 + ")</span>";//b7
        //                                                                                                                                      //Start b7
        //                                        }
        //                                        else
        //                                        {
        //                                            html += "<li><span class='caret1 nosub'>" + item_mms3.mt_cd + "(" + namecongdona3 + ")</span>";//b7
        //                                                                                                                                           //Start b7
        //                                        }

        //                                        html += "<ul class='nested'>";//b8
        //                                        foreach (var item_mms4 in data_mms4)
        //                                        {
        //                                            html += "<li><span class=' caret1 nosub'>" + item_mms4.mt_cd + " </span></li>";//b8
        //                                        }
        //                                        html += "</li>";//end b7
        //                                        html += "</ul>";//end b8
        //                                    }

        //                                    html += "</li>";//end b6
        //                                    html += "</ul>";//end b7
        //                                }
        //                                html += " </li>"; //end b5
        //                                html += " </ul>"; //end b6
        //                            }
        //                            html += " </li>"; //end b4
        //                            html += " </ul>"; //end b5
        //                        }
        //                        html += " </li>"; //end b3
        //                        html += " </ul>"; //end b4
        //                    }
        //                    html += " </li>"; //b2
        //                    html += " </ul>";//b3 //end b3
        //                }
        //                html += " </ul>";//end b2
        //                html += " </li>"; //end b1
        //            }
        //            html += " </ul>";//end b2
        //            html += " </li>"; //end b1
        //        }
        //        return Json(new { result = true, kq = html }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { result = false, message = "Can not View " }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        public static IEnumerable KT_Lot_dv(List<FG_DV> list)
        {
            var list_new = new List<FG_DV>();
            foreach (var item in list)
            {
                var dsdv_new = new FG_DV();

                if (item.mt_cd.Contains("-DV"))
                {
                    var mt_lot = "";
                    var fff = item.mt_cd.IndexOf("-DV");
                    mt_lot = item.mt_cd.Remove(fff);

                    var kt_co_dv = list_new.Where(x => x.mt_cd.Contains(mt_lot)).ToList();

                    if (kt_co_dv.Count > 0)
                    {
                        var mt_cd_cong_don = kt_co_dv.FirstOrDefault();
                        if (!mt_cd_cong_don.mt_cd.Contains(item.mt_cd))
                        {
                            mt_cd_cong_don.mt_cd = mt_cd_cong_don.mt_cd + "," + item.mt_cd;
                        }
                    }
                    else
                    {
                        dsdv_new.mt_cd = item.mt_cd;
                        dsdv_new.id_actual = item.id_actual;

                        list_new.Add(dsdv_new);
                    }
                }
                else
                {
                    dsdv_new.mt_cd = item.mt_cd;
                    dsdv_new.id_actual = item.id_actual;
                    list_new.Add(dsdv_new);
                }
            }
            return list_new;
        }

        private string KT_MTLot_DV(string mt_lot)
        {
            var mt_lot1 = "";
            if (mt_lot.Contains("-DV"))
            {
                var fff = mt_lot.IndexOf("-DV");
                mt_lot1 = mt_lot.Remove(fff);
                return mt_lot1;
            }
            if (mt_lot.Contains("-RT"))
            {
                var fff = mt_lot.IndexOf("-RT");
                mt_lot1 = mt_lot.Remove(fff);
                return mt_lot1;
            }
            if (mt_lot.Contains("-MG"))
            {
                var fff = mt_lot.IndexOf("-MG");
                mt_lot1 = mt_lot.Remove(fff);
                return mt_lot1;
            }
            else
            {
                return mt_lot;
            }
        }

        #region PartialView_FG_bb_no
        public ActionResult PartialView_FG_bb_no(string bb_no)
        {
            ViewBag.bb_no = bb_no;
            return PartialView("~/Views/fgwms/Inventory/PartialView_FG_bb_no.cshtml");
        }
        public async Task<JsonResult> GetFGProductLot_bb_no(string bb_no, string mt_lot)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                //string sql="";
                if (!string.IsNullOrEmpty(bb_no))
                {
                    //var check_bb_no = db.d_bobbin_lct_hist.Where(x => x.bb_no == bb_no).ToList();
                    var check_bb_no =await _IFGMWServices.GetBobinHistory(bb_no);
                    var dem = 0;
                    foreach (var item in check_bb_no)
                    {
                        if (dem != 0)
                        {
                            //sql = sql + @"union all";
                            sql.Append("Union all");
                        }
                        var find_lot = await _ITimsService.FindAllMaterialByMtCdLike(item.mt_cd);
                       
                        if (find_lot.Count() >0)
                        {
                            if (!string.IsNullOrEmpty(find_lot.FirstOrDefault().buyer_qr) || find_lot.FirstOrDefault().status == "010")
                            {
                                sql.Append("INSERT @tmpa(order_lv,mapping_dt,reg_dt,id,buyer_qr,cha,mt_lot,cccc,mt_cd,type,bb_no,process,process_cd,congnhan_time,machine,SLLD,mt_no,size,mt_nm,date,expiry_dt,dt_of_receipt,expore_dt,lot_no,mt_type)  ");
                                //sql.Append($"CALL spTesting2('{item.mt_cd}','','" + find_lot.buyer_qr + "');");
                                sql.Append($" EXEC [dbo].[GetListMaterialAndLot] @materialcode = '" +item.mt_cd+$"',@sts='BB',@buyercode='" + find_lot.FirstOrDefault().buyer_qr + $"';");
                            }
                            else
                            {
                                sql.Append("INSERT @tmpa(order_lv,mapping_dt,reg_dt,id,buyer_qr,cha,mt_lot,cccc,mt_cd,type,bb_no,process,process_cd,congnhan_time,machine,SLLD,mt_no,size,mt_nm,date,expiry_dt,dt_of_receipt,expore_dt,lot_no,mt_type)  ");
                                sql.Append($" EXEC [dbo].[GetListMaterialAndLot] @materialcode = '" + item.mt_cd + $"',@sts='BB',@buyercode='';");
                                //sql = sql+ @"CALL spTesting2('{item.mt_cd}','CP','');";
                            }

                        }
                        else
                        {
                            sql.Append("INSERT @tmpa(order_lv,mapping_dt,reg_dt,id,buyer_qr,cha,mt_lot,cccc,mt_cd,type,bb_no,process,process_cd,congnhan_time,machine,SLLD,mt_no,size,mt_nm,date,expiry_dt,dt_of_receipt,expore_dt,lot_no,mt_type)  ");
                            sql.Append($" EXEC [dbo].[GetListMaterialAndLotMMS] @materialcode = '" + item.mt_cd + $"';");
                        }
                        dem++;
                    }
                }
                else
                {
                    var find_lot = await _ITimsService.FindAllMaterialByMtCdLike(mt_lot);
                    ///var find_lot = db.w_material_info.Where(x => x.mt_cd == mt_lot).SingleOrDefault();
                    if (find_lot != null)
                    {
                        if (!string.IsNullOrEmpty(find_lot.FirstOrDefault().buyer_qr) || find_lot.FirstOrDefault().status == "010")
                        {
                            sql.Append("INSERT @tmpa(order_lv,mapping_dt,reg_dt,id,buyer_qr,cha,mt_lot,cccc,mt_cd,type,bb_no,process,process_cd,congnhan_time,machine,SLLD,mt_no,size,mt_nm,date,expiry_dt,dt_of_receipt,expore_dt,lot_no,mt_type) ");
                            //sql.Append($"CALL spTesting2('{mt_lot}','','" + find_lot.buyer_qr + "');");
                            sql.Append($" EXEC [dbo].[GetListMaterialAndLot] @materialcode = '" + mt_lot + $"',@sts='BB',@buyercode='" + find_lot.FirstOrDefault().buyer_qr + $"';");
                        }
                        else
                        {
                            sql.Append("INSERT @tmpa(order_lv,mapping_dt,reg_dt,id,buyer_qr,cha,mt_lot,cccc,mt_cd,type,bb_no,process,process_cd,congnhan_time,machine,SLLD,mt_no,size,mt_nm,date,expiry_dt,dt_of_receipt,expore_dt,lot_no,mt_type)  ");
                            //sql.Append($"CALL spTesting2('{mt_lot}','CP','');");
                            sql.Append($" EXEC [dbo].[GetListMaterialAndLot] @materialcode = '" + mt_lot + $"',@sts='BB',@buyercode='';");
                        }

                    }
                }
                var data = await _IFGMWServices.Truyxuatlistlot(sql);
                return Json(new { data = data, result = true, message = "" }, JsonRequestBehavior.AllowGet);
                //return new InitMethods().JsonResultAndMessageFromQuery(sql, "");
            }
            catch (Exception EX)
            {
                return Json(new { result = false, message = "Data dont has exist!!!", data = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> GetFGProductLot_PO(string po)
        {
            try
            {

                var resqheader = Request.Headers;
                string[] resqheaderkey = resqheader.AllKeys;
                string[] resqheaderval = resqheader.GetValues("requestFrom");
                string resqheadervalmob = "";
                if (resqheaderval != null)
                {
                    resqheadervalmob = resqheaderval[0];
                }
                if (Session["authorize"] != null || resqheadervalmob == "Mob")
                {

                    var datas = await _IFGMWServices.GetListFGProductLotPO(po);
                    if(datas.Count() > 0)
                    {
                        //var daaa = datas.GroupBy(x =>new { x.process,x.congnhan_time,x.machine }).Select(cc=>new FGProductLotPO
                        //{
                        //    process=cc.Key.process,
                        //    congnhan_time = cc.Key.congnhan_time,
                        //    machine = cc.Key.machine,
                        //    FGProductLotPO = cc.ToList()
                        //}).ToList();

                        return Json(new { data = datas, result = true, message = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, message = "Data dont has exist!!!", data = 0 }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }
                //StringBuilder sql = new StringBuilder($"CALL Lot_PO_custom('{po}');");
                //return new InitMethods().JsonResultAndMessageFromQuery(sql, "");
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = "Data dont has exist!!!", data = 0, exception = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> GetFGProductLot_buyer_qr(string buyer_qr)
        {
            try
            {
                var resqheader = Request.Headers;
                string[] resqheaderkey = resqheader.AllKeys;
                string[] resqheaderval = resqheader.GetValues("requestFrom");
                string resqheadervalmob = "";
                if (resqheaderval != null)
                {
                    resqheadervalmob = resqheaderval[0];
                }
                if (Session["authorize"] != null || resqheadervalmob == "Mob")
                {

                    //var List_buyer = db.w_material_info.Where(x => x.buyer_qr == buyer_qr).ToList();
                    var List_buyer =await _IFGMWServices.GetDataBuyersFromMaterialInfoTIMS(buyer_qr);
                    StringBuilder sql = new StringBuilder();
                    var dem = 0;
                    foreach (var item in List_buyer)
                    {
                        if (dem != 0)
                        {
                            sql.Append($"union ");
                        }
                        sql.Append("INSERT @tmpa(order_lv,mapping_dt,reg_dt,id,buyer_qr,cha,mt_lot,cccc,mt_cd,type,bb_no,process,process_cd,congnhan_time,machine,SLLD,mt_no,size,mt_nm,date,expiry_dt,dt_of_receipt,expore_dt,lot_no,mt_type)");
                        sql.Append($" EXEC [dbo].[GetListMaterialAndLot] @materialcode ='" + item.material_code + $"',@sts='buyer',@buyercode='" + buyer_qr + $"';");

                        dem++;
                    }
                    var data = await _IFGMWServices.Truyxuatlistlot(sql);
                    return Json(new { data = data, result = true, message = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = "Data dont has exist!!!", data = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> GetFGProductLot_box_no(string box_no)
        {
            try
            {

                var resqheader = Request.Headers;
                string[] resqheaderkey = resqheader.AllKeys;
                string[] resqheaderval = resqheader.GetValues("requestFrom");
                string resqheadervalmob = "";
                if (resqheaderval != null)
                {
                    resqheadervalmob = resqheaderval[0];
                }
                if (Session["authorize"] != null || resqheadervalmob == "Mob")
                {

                    var danhsach = await _IFGMWServices.GetlistBoxNobyMTCD();

                    return Json(new { result = true, message = "Data dont has exist!!!", data = danhsach }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = "Data dont has exist!!!", data = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion PartialView_FG_bb_no

        #region Popup FG product list

        public ActionResult PartialView_FG_Info_Popup(string sts, string value)
        {
            ViewBag.sts = sts;
            ViewBag.value = value;
            return PartialView("~/Views/fgwms/Inventory/PartialView_FG_Info_Popup.cshtml");
        }
        public ActionResult PartialView_FG_Material(List<NVL_MT> NVL_MT)
        {
            return PartialView("~/Views/fgwms/Inventory/PartialView_FG_Material.cshtml", NVL_MT);
        }
        public async Task<ActionResult> PartialView_FG_PO(string value)
        {


            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob == "Mob")
            {
                ViewBag.value = value;
                var ketqua=await _IFGMWServices.GetListFGPO(value);
                //var csv = string.Join("<br> ", ketqua.Select(x => x.buyer_qr));
                var listBuyers = ketqua.Select(x => x.buyer_qr).ToList();


                string itemTable = "";


                var totalRow = listBuyers.Count() / 5;

                for (int i = 0; i <= totalRow; i++)
                {


                    var item = "";
                    for (int j = i * 5; j  < (i * 5 + 5  < listBuyers.Count() ? i * 5 + 5  : listBuyers.Count()); j++)
                    {
                        item += $"<td>{listBuyers[j]}</td>";
                    }

                    itemTable += $"<tr>{item}</td>";
                }


                string tableHtml =string.Format(
                    @"  <table class=custom-table-buyers>
                            <tr>
                                {0}
                            </tr>
                        </table>", itemTable)
                    ;
                ViewBag.buyer = tableHtml;
                ViewBag.quantity = ketqua.Sum(x => x.gr_qty);
                //var check_value = db.w_actual_primary.Where(x => x.at_no == value).ToList();
                var check_value =await _IFGMWServices.GetProductforPrimary(value);
                //ViewBag.product = (check_value.Count > 0 ? check_value.FirstOrDefault().product : "");
                ViewBag.product = check_value;

                return PartialView("~/Views/fgwms/Inventory/PartialView_FG_PO.cshtml");
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }
        //public ActionResult GetFGProductLotBuyer(int id)
        //{
        //    try
        //    {
        //        var kttt = db.w_box_mapping.Find(id);
        //        if (kttt == null)
        //        {
        //            return Json(new { result = false, message = "Data dont has exist!!!" }, JsonRequestBehavior.AllowGet);
        //        }
        //        var boxx = kttt.bx_no;
        //        var data = (from a in db.w_box_mapping
        //                    where a.bx_no.Equals(boxx)

        //                    select new
        //                    {
        //                        bmno = a.bmno,
        //                        bx_no = a.bx_no,
        //                        buyer_cd = a.buyer_cd,
        //                        gr_qty = a.gr_qty,
        //                        mapping_dt = a.mapping_dt,
        //                        sts = db.comm_dt.Where(x => x.mt_cd == "WHS013" && x.dt_cd == a.sts).Select(x => x.dt_nm),
        //                    }
        //        ).ToList();

        //        return Json(new { result = true, message = "Data has exist!!!", data }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { result = false, message = "Data dont has exist!!!" }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        public ActionResult GetFGProductLot_mt_lot(string mt_lot)
        {
            try
            {
                StringBuilder sql = new StringBuilder($"CALL spTesting2('{mt_lot}');");

                return new InitMethods().JsonResultAndMessageFromQuery(sql, "");
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Data dont has exist!!!" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Pro_GetFGProductLot_Machine(string mt_lot)
        {
            try
            {
                StringBuilder sql = new StringBuilder($"CALL Pro_GetFGProductLot_Machine('{mt_lot}');");

                return new InitMethods().JsonResultAndMessageFromQuery(sql, "");
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Data dont has exist!!!" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Pro_GetFGProductLot_staff(string mt_lot)
        {
            try
            {
                StringBuilder sql = new StringBuilder($"CALL Pro_GetFGProductLot_staff('{mt_lot}');");

                return new InitMethods().JsonResultAndMessageFromQuery(sql, "");
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Data dont has exist!!!" }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion Popup FG product list

        private string vong_lapbd()
        {
            var html = "";
            html += "dsd";
            return html = "";
        }

        //public ActionResult Partial_getTreeDash(string value)
        //{
        //    try
        //    {
        //        //check_tontai trong box
        //        var html = "";
        //        var box = db.w_box_mapping.Where(x => x.bx_no == value).ToList();
        //        if (box.Count > 0)
        //        {
        //            html += "<li><code1>" + box.FirstOrDefault().bx_no + "</code1>";
        //            html += "<ul>";
        //            foreach (var item in box)
        //            {
        //                html += "<li>";

        //                html += "<code1>" + item.mt_cd + "</code1>";
        //                var list_mt_cd_buyer = db.w_material_info.Where(y => y.mt_cd == item.mt_cd).Select(x => x.buyer_qr).ToList();
        //                foreach (var itembuyer in list_mt_cd_buyer)
        //                {
        //                    html += "<ul><li>";
        //                    html += "<code1>" + itembuyer + "</code1>";

        //                    var list_mt_cd = db.w_material_info.Where(y => y.buyer_qr == itembuyer).Select(x => x.mt_cd).ToList();

        //                    var list_qc = db.w_product_qc.Where(x => x.ml_no == item.mt_cd).Select(x => x.pq_no).ToList();
        //                    html += "<ul>";
        //                    foreach (var item_mt_cd in list_qc)
        //                    {
        //                        html += "<li><code1> " + item_mt_cd + " </code1></li> ";
        //                        var list_tqc = db.m_facline_qc.Where(x => x.ml_no == item.mt_cd && x.item_vcd.StartsWith("TI")).Select(x => x.fq_no).ToList();

        //                        html += "<ul>";
        //                        foreach (var item_mt_cd_qc in list_tqc)
        //                        {
        //                            html += "<li><code1> " + item_mt_cd_qc + " </code1></li> ";
        //                        }

        //                        //html += "</ul>";
        //                        html += "</ul>";
        //                    }
        //                    html += "</ul>";
        //                    html += "</ul>";
        //                }

        //                html += "</li>";
        //            }

        //            html += "</ul></li>";
        //            return PartialView("~/Views/fgwms/Inventory/Partial_getTreeDash.cshtml", html);

        //            //new { result = false, message = ex.Message }                    //return PartialView(data);
        //        }

        //        return Json(new { result = false, message = "Can not View " }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { result = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        #endregion FG_ProductLot

        #region FG OK=>NG(TIMS)

        public ActionResult Return()
        {
            return View("~/Views/fgwms/Return/Return.cshtml");
        }

        [HttpGet]
        public JsonResult GetDilevery_Return(Pageing paging, string dl_no, string dl_nm, string buyer)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" SELECT a.wmtid,a.buyer_qr,(c.at_no)po,(select product from w_actual_primary where at_no=c.at_no) product,a.mt_cd,a.gr_qty qty,(select dt_nm from comm_dt where a.mt_sts_cd=dt_cd and mt_cd ='WHS005') sts_nm FROM w_material_info AS a")
                   .Append(" JOIN w_dl_info AS b ON a.dl_no = b.dl_no ")
                   .Append(" JOIN w_actual AS c ON a.id_actual = c.id_actual ")
                   .Append(" WHERE a.lct_cd LIKE '004%' ")
                   .Append("AND ('" + dl_no + "'='' OR  b.dl_no like '%" + dl_no + "%' )")
                   .Append("AND ('" + dl_nm + "'='' OR  b.dl_nm like '%" + dl_nm + "%' )")
                   .Append("AND ('" + buyer + "'='' OR  a.buyer_qr like '%" + buyer + "%' )")
                   .Append(" order by b.dlid desc ");

                DataTable dt = new InitMethods().ReturnDataTableNonConstraints(sql);
                int total = dt.Rows.Count;
                var result = dt.AsEnumerable().OrderByDescending(x => x.Field<int>("wmtid"));
                return new InitMethods().ReturnJsonResultWithPaging(paging, total, result);

                //var result = await _IFGMWServices.GetListDataDilevery(dl_no, dl_nm, buyer);
                //int start = (paging.page - 1) * paging.rows;
                //int end = (paging.page - 1) * paging.rows + paging.rows;
                //int totals = result.Count();
                //int totalPages = (int)Math.Ceiling((float)totals / paging.rows);
                //var dataactual = result.Skip<DeliveryResponse>(start).Take(paging.rows);
                //var jsonReturn = new
                //{
                //    total = totalPages,
                //    page = paging.page,
                //    records = totals,
                //    rows = dataactual
                //};
                //return Json(jsonReturn, JsonRequestBehavior.AllowGet);
                //return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetSortingOK_NG(string buyer_no)
        {
            try
            {
                var resqheader = Request.Headers;
                string[] resqheaderkey = resqheader.AllKeys;
                string[] resqheaderval = resqheader.GetValues("requestFrom");
                string resqheadervalmob = "";
                if (resqheaderval != null)
                {
                    resqheadervalmob = resqheaderval[0];
                }
                if (Session["authorize"] != null || resqheadervalmob == "Mob")
                {

                    //check string null khoong

                    if (string.IsNullOrEmpty(buyer_no))
                    {
                        return Json(new { result = false, message = "Please Scan again" }, JsonRequestBehavior.AllowGet);
                    }
                    //check ton tai khong
                    var check_exits = await _ITimsService.GetMaterialInfoTimsByBuyerQRFg(buyer_no);
                    if (check_exits.Count() == 0)
                    {
                        return Json(new { result = false, message = "Buyer can not found" }, JsonRequestBehavior.AllowGet);
                    }
                    //scan vao bang tam thoi truoc


                    var data = await _ITimsService.GetListShippingScanBuyerQRFGS(buyer_no);
                    if (data.Count == 0)
                    {
                        return Json(new { result = false, message = "Data has not exist!!!" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { result = true, message = "Data has exist!!!", data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> change_sts_fg_tims(string buyer_code)
        {
            //lay danhsach va chuyen trang thai ve NG va tra ve kho tims
            try
            {
                var resqheader = Request.Headers;
                string[] resqheaderkey = resqheader.AllKeys;
                string[] resqheaderval = resqheader.GetValues("requestFrom");
                string resqheadervalmob = "";
                if (resqheaderval != null)
                {
                    resqheadervalmob = resqheaderval[0];
                }
                if (Session["authorize"] != null || resqheadervalmob == "Mob")
                {

                    var data = await _ITimsService.GetMaterialInfoTimsByBuyerQRFg(buyer_code);
                    string userid = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                    foreach (var item in data)
                    {
                        await _ITimsService.Updatechangestsfgtims(item.wmtid, "003", "006000000000000000", userid);

                    }
                    return Json(new { result = true, message = "Successfully!!!", count = data.Count() }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult PrintReturnTims()
        {
            string[] keys = Request.Form.AllKeys;

            var value = "";
            value = Request.Form[keys[0]];

            ViewData["Message"] = value;
            return View("~/Views/fgwms/Return/PrintReturnTims.cshtml");
        }

        public async Task<JsonResult> qrPrintQr(string buyer_code)
        {
            try
            {
                var resqheader = Request.Headers;
                string[] resqheaderkey = resqheader.AllKeys;
                string[] resqheaderval = resqheader.GetValues("requestFrom");
                string resqheadervalmob = "";
                if (resqheaderval != null)
                {
                    resqheadervalmob = resqheaderval[0];
                }
                if (Session["authorize"] != null || resqheadervalmob == "Mob")
                {

                    if (buyer_code != "")
                    {
                       var datarews = await _ITimsService.GetMaterialInfoTimsByBuyerQRFg(buyer_code);
                        var data = (from a in datarews
                                    select new
                                    {
                                        mt_cd = a.material_code,
                                        mt_sts_cd = a.status,
                                        lct_cd = a.location_code,
                                        wmtid = a.wmtid,
                                        buyer_qr = a.buyer_qr,
                                    }
                            ).ToList();
                        return Json(new { result = true, data }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { result = false, message = "Not Found Buyer" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion FG OK=>NG(TIMS)

        #region PrintQR_ma_thung
        public ActionResult PrintQR(string id)
        {
            ViewData["Message"] = id;
            return View("~/Views/fgwms/Mapping/PrintQR.cshtml");
        }

        [HttpGet]
        public async Task<ActionResult> QRbarcodeInfo(string id)
        {
            try
            {
                var multiIDs = id.TrimStart('[').TrimEnd(']').Split(',');
                var row_data = new List<BuyerQRModel>();

                for (int i = 0; i < multiIDs.Length; i++)
                {
                    var id2 = int.Parse(multiIDs[i]);
                    var boxmpp = await _ITimsService.CheckTemGoimappingBoxwithbmno(id2);
                    var bo_no = boxmpp.bx_no;

                    var data = await _IFGMWServices.PrintQRCodeForMappingBox(bo_no);

                    if (data == null)
                    {
                        return Json(new { result = false }, JsonRequestBehavior.AllowGet);
                    }
                    string supplier = "DZIH";
                    //var timDZIH = bo_no.IndexOf("DZIH") + 4; //product+DZIH
                    var timDZIH1 = bo_no.IndexOf("DZIH"); //product
                    if (timDZIH1 < 1)
                    {
                        timDZIH1 = bo_no.IndexOf("EA8D");
                        supplier = "EA8D";
                    }

                    var timDZIH = timDZIH1 + 4; //product+DZIH

                    var vendor_line = bo_no.Substring(timDZIH, 1) == null ? "" : "(" + bo_no.Substring(timDZIH, 1) + ")";

                    var itemBuyer = new BuyerQRModel();
                    itemBuyer.id = data.id;
                    itemBuyer.bx_no = data.bx_no;
                    itemBuyer.model = data.model;
                    itemBuyer.buyer_qr = data.buyer_qr;
                    itemBuyer.stamp_code = data.stamp_code;
                    itemBuyer.part_name = data.part_name;
                    itemBuyer.product_code = data.product_code;
                    itemBuyer.quantity = data.quantity;
                    itemBuyer.lotNo = string.Concat(data.lotNo.Replace("-", ""));

                    if (!string.IsNullOrEmpty(data.lotNo))
                    {
                        var ymd = string.Concat(data.lotNo.Replace("-", ""));
                        string y = ymd.Substring(0, 4);
                        string m = ymd.Substring(4, 2);
                        string d = ymd.Substring(6, 2);

                        itemBuyer.nsx = d + "/" + m + "/" + y;
                    }
                    else
                    {
                        itemBuyer.nsx = "";
                    }
                    itemBuyer.prj_nm = (data.prj_nm == "" || data.prj_nm == null) ? "" : data.prj_nm;
                    itemBuyer.nhietdobaoquan = data.nhietdobaoquan == null ? "" : data.nhietdobaoquan;
                    itemBuyer.ssver = data.ssver == null ? "" : data.ssver;
                    itemBuyer.Description = data.Description == null ? "" : data.Description;
                    itemBuyer.supplier = supplier;
                    itemBuyer.vendor_line = vendor_line;
                    string hsd = "";
                    //if (data.stamp_code == "001")
                    if (data.expiry_month == "0")
                    {
                        hsd = data.hsd;
                    }
                    else
                    {

                        string s = data.lotNo;

                        DateTime dt = DateTime.ParseExact(s, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        if (!string.IsNullOrEmpty(data.expiry_month))
                        {
                            hsd = dt.AddMonths(int.Parse(data.expiry_month)).ToString("dd/MM/yyyy");
                        }
                    }

                    itemBuyer.hsd = hsd;
                    row_data.Add(itemBuyer);
                }
                return Json(new { result = true, row_data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
    }

        public async Task<ActionResult> ExportToExcelReceiFG(string product, string buyer, string po_no, DateTime? lot_date, DateTime? lot_date_end)
        {
            //StringBuilder sql = new StringBuilder($" Call ExportToExcelReceiFG('{po}','{product}','{buyer}','{lot_date}','{lot_date_end}'); ");
            //var data = db.Database.SqlQuery<ExportToExcelReceiFG>(sql.ToString());

            var data = await _ITimsService.ExportToExcelReceiveFG(product, buyer, po_no, lot_date, lot_date_end);
            var values = data.ToList().AsEnumerable().Select(x => new
            {
                po = x.PO,
                product = x.product,
                product_nm = x.product_nm,
                lot_date = x.lot_date,
                buyer_qr = x.buyer_qr,
                bb_no = x.bb_no,
                gr_qty = x.gr_qty,

            }).ToArray();

            String[] labelList = new string[7] { "PO", "Product Code", "Product Name", "Lot date", "Buyer QR", "Bobbin", "Quantity" };

            Response.ClearContent();

            Response.AddHeader("content-disposition", "attachment;filename=DanhSachNhanKhoFG.xls");

            Response.AddHeader("Content-Type", "application/vnd.ms-excel");

            new InitMethods().WriteHtmlTable(values, Response.Output, labelList);

            Response.End();


            return View("~/Views/fgwms/Receiving_Scan/Receving_Scan.cshtml");
        }

        #endregion

        #region Popup PO
        public async  Task<JsonResult> Getdataw_popupactual(Pageing pageing, string product, string at_no, string reg_dt)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob == "Mob")
            {

                var data = await _IFGMWServices.GetlistDataActualPrimary(product, at_no, reg_dt);
                int start = (pageing.page - 1) * pageing.rows;
                int totals = data.Count();
                int totalPages = (int)Math.Ceiling((float)totals / pageing.rows);
                IEnumerable<DatawActualPrimaryResponse> dataactual = data.Skip<DatawActualPrimaryResponse>(start).Take(pageing.rows);
                var jsonReturn = new
                {
                    total = totalPages,
                    page = pageing.page,
                    records = totals,
                    rows = dataactual,

                };
                return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region ShippingSortingFG
        public async Task<ActionResult> DeleteExporttomachine(int id, string ShippingCode)
        {
            try
            {
                if (string.IsNullOrEmpty(ShippingCode))
                {
                    return Json(new { result = false, message = "Vui lòng Chọn một phiếu xuất để xóa" }, JsonRequestBehavior.AllowGet);
                }
                //kiểm tra xem ep này đã xuất bất kì liệu nào ra máy chưa/ nếu chưa mới cho xóa
                var check = await _IFGMWServices.GetListShippingFGSorting(ShippingCode);
                if (check.Count() > 0)
                {
                    return Json(new { result = false, message = "Phiếu này đã có Nguyên Vật Liệu xuất, Bạn không thể xóa" }, JsonRequestBehavior.AllowGet);
                }
                await _IFGMWServices.DeleteToExportToMachine(id);


                return Json(new { result = true, message = "Thành công!!!" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { result = false, message = "Sửa thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> ShippingSortingFG()
        {
            return SetLanguage("~/Views/fgwms/SortingFG/ShippingFG/ShippingSortingFGScan.cshtml");
        }
        public async Task<JsonResult> searchShippingSortingFG(Pageing pageing, string ShippingCode, string ProductCode, string ProductName, string Description)
        {

            Dictionary<string, string> list = PagingAndOrderBy(pageing, " ORDER BY MyDerivedTable.ExportCode DESC ");



            int totalRecords = await _IFGMWServices.TotalRecordsSearchShippingSortingFG(ShippingCode, ProductCode, ProductName, Description);
            int pagesize = int.Parse(list["size"]);
            int totalPages = 0;
            try
            {
                totalPages = (int)Math.Ceiling((float)totalRecords / (float)pagesize);
            }
            catch (Exception e)
            {
                totalPages = totalRecords / pagesize;
            }

            IEnumerable<ShippingFGSortingModel> Data =await  _IFGMWServices.GetListSearchShippingSortingFG(ShippingCode, ProductCode, ProductName, Description);

            var result = new
            {
                total = totalPages,
                page = int.Parse(list["index"]),
                records = totalRecords,
                rows = Data
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        private Dictionary<string, string> PagingAndOrderBy(Pageing pageing, string orderByStr)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            int pageIndex = pageing.page;
            int pageSize = pageing.rows;
            int start_r = pageing.page > 1 ? ((pageIndex - 1) * pageSize) : pageing.page;
            int end_r = (pageIndex * pageSize);
            string order_by = pageing.sidx != null ? (" ORDER BY " + pageing.sidx + " " + pageing.sord) : orderByStr;
            list.Add("index", pageIndex.ToString());
            list.Add("size", pageSize.ToString());
            list.Add("start", start_r.ToString());
            list.Add("end", end_r.ToString());
            list.Add("orderBy", order_by);
            return list;
        }
        public async Task<JsonResult> InsertShippingFGSorting(ShippingFGSortingModel item)
        {
            try
            {
                #region Tang tự động
                String ShippingCode = "SF1";

                var Shippinglast = await _IFGMWServices.GetLastShippingFGSorting();

                if (Shippinglast != null)
                {
                    ShippingCode = Shippinglast.ShippingCode;
                    ShippingCode = string.Concat("SF", (int.Parse(ShippingCode.Substring(2)) + 1).ToString());
                }
                #endregion
                item.ShippingCode = ShippingCode;
                item.IsFinish = "N";
                item.CreateId = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                item.CreateDate = DateTime.Now;
                item.ChangeId = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                item.ChangeDate = DateTime.Now;

                var id_ShippingFG = await _IFGMWServices.InsertToShippingFGSorting(item);
                 var id = await _IFGMWServices.GetLastShippingFGSorting();
                if (id.id > 0)
                {
                    item.id = id.id;
                    return Json(new { result = true, message = "Thành công!!!", data = item }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = false, message = "Tạo thất bại" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = "Tạo thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> ModifyShippingFGSorting(ShippingFGSortingModel item)
        {
            try
            {
                item.ChangeId = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                item.ChangeDate = DateTime.Now;

                   await    _IFGMWServices.ModifyShippingFGSorting(item);


                return Json(new { result = true, message = "Thành công!!!", data = item }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { result = false, message = "Sửa thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> GetExport_ScanBuyer_FG(string BuyerCode)
        {
            try
            {
                //Check input
                if (string.IsNullOrEmpty(BuyerCode))
                {
                    return Json(new { result = false, message = "Mã tem gói trống, Vui lòng Scan lại" }, JsonRequestBehavior.AllowGet);
                }
                BuyerCode = BuyerCode.Trim();
                //Kiểm tra mã materialCode có tồn tại ở kho NVL không
                var IsExistBuyerQr = await _IFGMWServices.CheckIsExistBuyerCode(BuyerCode);
                //kiểm tra xem mã khác 002 là kho khác
                if (IsExistBuyerQr != null)
                {
                    if (IsExistBuyerQr.status.Equals("010"))
                    {
                        return Json(new { result = false, message = "Mã Này Đang Được Mapping với Tem Thùng" }, JsonRequestBehavior.AllowGet);
                    }
                    string trangthai = IsExistBuyerQr.status == "001" ? "Tồn kho" : "Đã giao hàng";
                    IsExistBuyerQr.status = trangthai;
                    return Json(new { result = true, message = "Thành công", data = IsExistBuyerQr }, JsonRequestBehavior.AllowGet);
                }
                {
                    return Json(new { result = false, message = "Mã bạn vừa quét là: " + BuyerCode }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Sửa thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> UpdateShippingSortingFG(string ShippingCode, string ListId)
        {
            try
            {
                //Check input
                if (string.IsNullOrEmpty(ShippingCode))
                {
                    return Json(new { result = false, message = "Vui lòng Chọn một phiếu xuất" }, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(ListId))
                {
                    return Json(new { result = false, message = "Mã Nguyên vật liệu trống, Vui lòng Scan lại" }, JsonRequestBehavior.AllowGet);
                }
                ListId = ListId.Trim();


                //update trạng thái là storting cho tem gói

                var generalfg = new generalfg();
                generalfg.sts_cd = "015";

                await _IFGMWServices.UpdateShippingSortingFG(generalfg, ListId);

                //insert phiếu xuất và tem gói vào bảng shippingfgsortingdetail
                var UserID = Session["userid"] == null ? null : Session["userid"].ToString();
                var result = await _IFGMWServices.InsertShippingSortingFGDetail(ShippingCode, ListId, UserID);
                if (result == -1)
                {
                    return Json(new { result = false, message = "Buyer QR đã tồn tại trong Shipping Code" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = true, message = "Thành công" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { result = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PartialViewShippingFGSortingPP(string ShippingCode, bool edit)
        {
            ViewBag.Deleted = edit;
            ViewBag.ShippingCode = ShippingCode;
            return PartialView("~/Views/fgwms/SortingFG/ShippingFG/PartialViewShippingFGSortingPP.cshtml");
        }
        public async Task<JsonResult> GetShippingSortingFGPP(string ShippingCode)
        {
            var listdata =await _IFGMWServices.GetListSearchShippingSortingFGPP(ShippingCode);

            return Json(new { data = listdata }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetListShippingFGSorting(string ShippingCode)
        {
            var listdata = await _IFGMWServices.GetListShippingFGSorting(ShippingCode);

            return Json(new { data = listdata }, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetShippingScanPP_Countbuyer(string ShippingCode)
        {
            try
            {
                var listdata = await _IFGMWServices.GetShippingScanPPCountbuyer(ShippingCode);

                return Json(new { data = listdata }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<JsonResult> DeleteRecortbuyer(string buyer_qr)
        {
            try
            {
                //Check input
                if (string.IsNullOrEmpty(buyer_qr))
                {
                    return Json(new { result = false, message = "Vui lòng chọn mã cần xóa" }, JsonRequestBehavior.AllowGet);
                }

                buyer_qr = buyer_qr.Trim();

                //Kiểm tra mã buyer_qr có tồn tại ở kho NVL không
                var IsbBuyer = await _IFGMWServices.CheckFGSortingdetail(buyer_qr);
                //kiểm tra xem mã = 006000000000000000 là kho TIMS không được xóa
                if (IsbBuyer != null)
                {
                    if (IsbBuyer.location != null)
                    {
                        if (IsbBuyer.location.Equals("006000000000000000"))
                        {
                            return Json(new { result = false, message = "Mã Này đã được nhận ở kho TIMS " }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        //update bảng genneral status  trước nó
                        var general =await _IFGMWServices.isCheckExistGenneral(buyer_qr);

                        general.sts_cd = (general.dl_no != null && general.dl_no != "") ? "000" : "001";

                        await _IFGMWServices.UpdateBuyerGeneral(general);

                        //delete shippingfgsortingdetail
                        await _IFGMWServices.DeleteFGSotingBuyer(buyer_qr);
                        return Json(new { result = true, message = "Thành công!!!" }, JsonRequestBehavior.AllowGet);
                    }

                }
                return Json(new { result = false, message = "Mã buyer không tìm thấy" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> ScanReceivingFGBuyer(string buyer_qr, string ShippingCode)
        {
            try
            {
                buyer_qr = buyer_qr.Trim();
                //Check input
                if (string.IsNullOrEmpty(ShippingCode))
                {
                    return Json(new { result = false, message = "Vui lòng chọn một phiếu ST!!!" }, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(buyer_qr))
                {
                    return Json(new { result = false, message = "Vui lòng scan một tem gói" }, JsonRequestBehavior.AllowGet);
                }



                //KIỂM TRA XEM SD CÓ TỒN TẠI KHÔNG.
                var check1 = await _IFGMWServices.CheckSTinfo(ShippingCode);
                if (check1.Count()<= 0 )
                {
                    return Json(new { result = false, message = "ST này không tồn tại" }, JsonRequestBehavior.AllowGet);
                }

                //Step1: kiểm tra tem gói có thuộc phiếu xuất đó ko

                var isCheckExistSF =await _IFGMWServices.isCheckExistSF(ShippingCode, buyer_qr);
                if (isCheckExistSF == null)
                {
                    return Json(new { result = false, message = "Không có tem gói được trả về" }, JsonRequestBehavior.AllowGet);
                }
                //Step2: kiểm tra tem gói này đã tồn kho chưa
                var isCheckExistBuyerQRST = await _IFGMWServices.isCheckExistGenneral(buyer_qr);
                if (isCheckExistBuyerQRST == null)
                {
                    return Json(new { result = false, message = "Tem gói không tồn tại" }, JsonRequestBehavior.AllowGet);
                }
                if (isCheckExistBuyerQRST.status != "015")
                {
                    var status = await _IFGMWServices.CheckStatus(isCheckExistBuyerQRST.status);
                    return Json(new { result = false, message = "Tem gói này đang ở Trạng thái:" + status }, JsonRequestBehavior.AllowGet);
                }
                // //Step3: update location shippingtimssortingdetail
                isCheckExistSF.ChangeId = Session["userid"] == null ? null : Session["userid"].ToString();
                isCheckExistSF.location = "003G01000000000000";
                await  _IFGMWServices.UpdateLocationtimSorting(isCheckExistSF);

                //Step3: update w_material_info sts= 001 and lct = 003G01000000000000
                var isCheckExistWmaterialBuyer = await _IFGMWServices.isCheckExistWmaterialBuyer(buyer_qr);
                if (isCheckExistSF == null)
                {
                    return Json(new { result = false, message = "Không có tem gói được trả về" }, JsonRequestBehavior.AllowGet);
                }
                isCheckExistWmaterialBuyer.location_code = "003G01000000000000";
                isCheckExistWmaterialBuyer.status = "001";
                await _IFGMWServices.UpdateWMaterialInfoStatus(isCheckExistWmaterialBuyer);

                //Step4: update generalfg  tem gói sts_cd = 001
                isCheckExistBuyerQRST.sts_cd = "001";
                isCheckExistBuyerQRST.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                await _IFGMWServices.UpdateBuyerGeneral(isCheckExistBuyerQRST);


                return (Json(new { result = true, data = "", message = "Thành công" }, JsonRequestBehavior.AllowGet));

            }
            catch (Exception e)
            {
                return Json(new { result = false, message = e }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region ReceingFG
        public ActionResult RecevingSortingFG()
        {
            return SetLanguage("~/Views/fgwms/SortingFG/RecevingFG/RecevingSortingFG.cshtml");
        }

        #endregion
    }

    public class ProductLotListModel
    {
        public int total_page { get; set; }
    }
}

