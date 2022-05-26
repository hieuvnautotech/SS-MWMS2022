using ClosedXML.Excel;
using Mvc_VD.Commons;
using Mvc_VD.Models;
using Mvc_VD.Models.DTO;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models.Request;
using Mvc_VD.Models.Response;
using Mvc_VD.Models.WIP;
using Mvc_VD.Services;
using Mvc_VD.Services.Interface;
using Mvc_VD.Services.Interface.MMS;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mvc_VD.Controllers
{
    public class WIPController : BaseController
    {
        private readonly IWIPServices _IWIPServices;
        private readonly IActualWOService _actualWOServices;
        private readonly IhomeService _homeService;
        private readonly Entities db;

        string ss = "Thành công!!!";

        public WIPController(
            IWIPServices IWIPServices,
            IActualWOService actualWOServices,
            IhomeService ihomeService,

        IDbFactory DbFactory)
        {

            _IWIPServices = IWIPServices;
            _actualWOServices = actualWOServices;
            _homeService = ihomeService;
            db = DbFactory.Init();
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
        #region General

        #region tab1_Material
        [HttpGet]
        public async Task<JsonResult> getGeneral_Material(Pageing pageing)
        {
            if (Session["authorize"] != null)
            {
                DateTime? start = new DateTime();
                DateTime? end = new DateTime();
                string mt_no = Request["mt_no"] == null ? "" : Request["mt_no"].Trim();
                string mt_cd = Request["mt_cd"] == null ? "" : Request["mt_cd"].Trim();
                string mt_nm = Request["mt_nm"] == null ? "" : Request["mt_nm"].Trim();
                string product_cd = Request["product_cd"] == null ? "" : Request["product_cd"].Trim();
                string lct_cd = Request["lct_cd"] == null ? "" : Request["lct_cd"].Trim();
                string recevice_dt_start = Request["recevice_dt_start"] == null ? "" : Request["recevice_dt_start"].Trim();
                string recevice_dt_end = Request["recevice_dt_end"] == null ? "" : Request["recevice_dt_end"].Trim();
                string sts = Request["sts"] == null ? "" : Request["sts"].Trim();


                if (mt_no.Contains('#'))
                {
                    mt_no = mt_no.Replace("#", "%#");
                }

                if (mt_no.Contains('['))
                {
                    mt_no = mt_no.Replace("[", "%");
                }

                if (mt_no.Contains(']'))
                {
                    mt_no = mt_no.Replace("]", "%");
                }
                if (recevice_dt_start == "")
                {
                    start = null;
                }
                else
                {
                    start = DateTime.Parse(recevice_dt_start);
                }

                if (recevice_dt_end == "")
                {
                    end = null;
                }
                else
                {
                    end = DateTime.Parse(recevice_dt_end);
                }

                if(lct_cd == "002001001000000000" ||  lct_cd  == "002001002000000000" || lct_cd == "002001003000000000" || lct_cd == "002001004000000000" || lct_cd == "002001005000000000" ||
                    lct_cd == "002001006000000000" || lct_cd == "002001007000000000"  || lct_cd == "002001008000000000" || lct_cd == "002001009000000000")
                {
                    var res = lct_cd.Substring(0,11);
                    lct_cd = res;
                }

                Dictionary<string, string> list = PagingAndOrderBy(pageing, " ORDER BY MyTable.mt_no DESC ");

                //int totalRecords = await _IWIPServices.TotalRecordsSearchGeneralMaterialWIP(mt_no, product_cd, mt_nm, start, end, sts, lct_cd, mt_cd);

                var data = await _IWIPServices.GetListGeneralMaterialWIP(mt_no, product_cd, mt_nm, start, end, sts, lct_cd, mt_cd);
                int totalRecords = data.Count();
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)int.Parse(list["size"]));
                var result = new
                {
                    total = totalPages,
                    page = int.Parse(list["index"]),
                    records = totalRecords,
                    rows = data
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetgeneralDetail_List(string mt_no, string mt_cd, string mt_nm, string product_cd, string lct_cd, string recevice_dt_start, string recevice_dt_end, string sts)
        {
            if (Session["authorize"] != null)
            {
                DateTime? start = new DateTime();
                DateTime? end = new DateTime();

                if (mt_no.Contains('['))
                {
                    mt_no = mt_no.Replace("[", "%");
                }

                if (mt_no.Contains(']'))
                {
                    mt_no = mt_no.Replace("]", "%");
                }

                if (recevice_dt_start == "")
                {
                    start = null;
                }
                else
                {
                    start = DateTime.Parse(recevice_dt_start);
                }

                if (recevice_dt_end == "")
                {
                    end = null;
                }
                else
                {
                    end = DateTime.Parse(recevice_dt_end);
                }

                if (lct_cd == "002001001000000000" || lct_cd == "002001002000000000" || lct_cd == "002001003000000000" || lct_cd == "002001004000000000" || lct_cd == "002001005000000000" ||
                   lct_cd == "002001006000000000" || lct_cd == "002001007000000000" || lct_cd == "002001008000000000" || lct_cd == "002001009000000000")
                {
                    var res = lct_cd.Substring(0, 11);
                    lct_cd = res;
                }

                var result = await _IWIPServices.GetListGeneralMaterialDetailWIP(mt_no, product_cd, mt_nm, start, end, sts, lct_cd, mt_cd);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }

        public Dictionary<string, string> PagingAndOrderBy(Pageing pageing, string orderByStr)
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

        #endregion tab1_Material


        public async Task<ActionResult> PrintExcelFile()
        {
            if (Session["authorize"] != null)
            {

                string mt_no = Request["mt_no"] == null ? "" : Request["mt_no"].Trim();
                //string mt_nm = Request["mt_nm"] == null ? "" : Request["mt_nm"].Trim();
                string mt_cd = Request["mt_cd"] == null ? "" : Request["mt_cd"].Trim();
                string s_product_cd = Request["s_product_cd"] == null ? "" : Request["s_product_cd"].Trim();
                string s_locationNAme = Request["s_locationNAme"] == null ? "" : Request["s_locationNAme"].Trim();
                string recevice_dt_start = Request["Rece_start"] == null ? "" : Request["Rece_start"].Trim();
                string recevice_dt_end = Request["Rece_end"] == null ? "" : Request["Rece_end"].Trim();
                string sts_search = "";
                //switch (sts)
                //{
                //    case "002":
                //        sts_search = " AND DSD >0";
                //        break;

                //    case "001,004":
                //        sts_search = "AND CSD >0";
                //        break;
                //}
                var dateConvert = new DateTime();

                if (DateTime.TryParse(recevice_dt_end, out dateConvert))
                {
                    recevice_dt_end = dateConvert.ToString("yyyy/MM/dd");
                }
                if (DateTime.TryParse(recevice_dt_start, out dateConvert))
                {
                    recevice_dt_start = dateConvert.ToString("yyyy/MM/dd");
                }
                //StringBuilder varname1 = new StringBuilder();
                //varname1.Append("SELECT ''mt_cd,a.mt_no,a.mt_nm,a.qty,concat(a.DSD,'m')DSD,concat(a.CSD,'m')CSD,(a.DSD + a.CSD)TK, \n");
                //varname1.Append(" ''lenght,''size,''recevice_dt,''sts_nm \n");
                //varname1.Append("FROM getgeneral_material AS a \n");
                //varname1.Append(" WHERE ('" + mt_no + "'='' OR a.mt_no like '%" + mt_no + "%' ) ");
                //varname1.Append(" AND ('" + mt_nm + "'='' OR  a.mt_nm like '%" + mt_nm + "%' ) ");
                //varname1.Append("" + sts_search + " ");
                //varname1.Append(" AND ('" + recevice_dt_start + "'='' OR DATE_FORMAT(a.recevice_dt,'%Y/%m/%d') >= DATE_FORMAT('" + recevice_dt_start + "','%Y/%m/%d')) ");
                //varname1.Append(" AND ('" + recevice_dt_end + "'='' OR DATE_FORMAT(a.recevice_dt,'%Y/%m/%d') <= DATE_FORMAT('" + recevice_dt_end + "','%Y/%m/%d')) ");

                //varname1.Append("UNION \n");
                //varname1.Append(" \n");
                //varname1.Append("SELECT a.mt_cd,a.mt_no,''mt_nm,CONCAT((CASE WHEN b.bundle_unit ='Roll' THEN ROUND((a.gr_qty/b.spec),2) ELSE ROUND(a.gr_qty,2) END),b.bundle_unit)qty, \n");
                //varname1.Append("'' DSD,''CSD,''TK, \n");
                //varname1.Append("CONCAT(IFNULL(a.gr_qty,''), IFNULL(b.unit_cd,'')) lenght, CONCAT(IFNULL(b.width,0),'*', IFNULL(a.gr_qty,0)) AS size,a.recevice_dt, ( \n");
                //varname1.Append("SELECT dt_nm \n");
                //varname1.Append("FROM comm_dt \n");
                //varname1.Append("WHERE comm_dt.dt_cd=a.mt_sts_cd AND comm_dt.mt_cd='WHS005') sts_nm \n");
                //varname1.Append("FROM w_material_info a \n");
                //varname1.Append("LEFT JOIN d_material_info b ON a.mt_no=b.mt_no \n");
                //varname1.Append("WHERE a.lct_cd LIKE '002%'  and a.mt_type<> 'CMT' AND a.mt_sts_cd!='005'  ");
                //varname1.Append(" AND ('" + mt_nm + "'='' OR  b.mt_nm like '%" + mt_nm + "%' ) ");
                //varname1.Append(" AND ('" + sts + "'='' OR  a.mt_sts_cd in (" + sts + ") ) ");
                //varname1.Append(" AND ('" + recevice_dt_start + "'='' OR DATE_FORMAT(a.recevice_dt,'%Y/%m/%d') >= DATE_FORMAT('" + recevice_dt_start + "','%Y/%m/%d')) ");
                //varname1.Append(" AND ('" + recevice_dt_end + "'='' OR DATE_FORMAT(a.recevice_dt,'%Y/%m/%d') <= DATE_FORMAT('" + recevice_dt_end + "','%Y/%m/%d')) ");

                //DataTable dt = new InitMethods().ReturnDataTableNonConstraints(varname1);
                //List<WIP_ParentInventoryModel> listTotal = new InitMethods().ConvertDataTable<WIP_ParentInventoryModel>(varname1);
                //listTotal = listTotal.OrderBy(x => x.mt_no).ThenBy(x => x.mt_cd).ToList();

                var listData = await _IWIPServices.PrintMaterialByExcel(mt_no, mt_cd, s_product_cd, s_locationNAme, recevice_dt_start, recevice_dt_end);
                var listTotal = listData.ToList();

                DataTable datatb = new InitMethods().ConvertListToDataTable(listTotal);

                DataSet ds2 = new DataSet();

                ds2.Tables.Add(datatb);
                ds2.Tables[0].TableName = "WIP_General";

                using (XLWorkbook wb = new XLWorkbook())
                {
                    var ws = wb.AddWorksheet(datatb);

                    ws.Columns().AdjustToContents();
                    //ws.Cells("A1").Value = "MT Code";
                    //ws.Cells("B1").Value = "MT Name";
                    //ws.Cells("C1").Value = "SD No";

                    //ws.Cells("D1").Value = "Product Code";
                    //ws.Cells("E1").Value = "mt_cd";
                    //ws.Cells("F1").Value = "Đang sử dụng Qty Length(M)";
                    //ws.Cells("G1").Value = "Chưa sử dụng Qty Length(M)";
                    //ws.Cells("H1").Value = "Size";
                    //ws.Cells("I1").Value = "spec";
                    //ws.Cells("J1").Value = "Quantity";
                    //ws.Cells("K1").Value = "receipt_date";
                    //ws.Cells("L1").Value = "trạng thái";
                    //ws.Cells("M1").Value = "kho";
                    //ws.Columns("K").Hide(); //an cot I

                    ws.Rows().AdjustToContents();
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Alignment.ShrinkToFit = true;

                    wb.Style.Font.Bold = true;
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=WIP_General.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
                return View("~/Views/WIP/Inventory/General.cshtml");
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult General()
        {
            return SetLanguage("~/Views/WIP/Inventory/General.cshtml");
        }

        [HttpGet]
        public async Task<JsonResult> getGeneral(Pageing paging)
        {
            string at_no = Request["at_no"] == null ? "" : Request["at_no"].Trim();
            string model = Request["model"] == null ? "" : Request["model"].Trim();
            string product = Request["product"] == null ? "" : Request["product"].Trim();
            string product_name = Request["product_name"] == null ? "" : Request["product_name"].Trim();
            string reg_dt_start = Request["reg_dt_start"] == null ? "" : Request["reg_dt_start"].Trim();
            string reg_dt_end = Request["reg_dt_end"] == null ? "" : Request["reg_dt_end"].Trim();
            string mt_cd = Request["mt_cd"] == null ? "" : Request["mt_cd"].Trim();
            string bb_no = Request["bb_no"] == null ? "" : Request["bb_no"].Trim();
            string status = Request["status_tab2"] == null ? "" : Request["status_tab2"].Trim();
            if (Session["authorize"] != null)
            {

                var result = await _IWIPServices.GetListGeneral(at_no, model, product, product_name, reg_dt_start, reg_dt_end, mt_cd, bb_no, status);

                int totals = result.Count();
                int totalPages = (int)Math.Ceiling((float)totals / paging.rows);
                int start = (paging.page - 1) * paging.rows;
                var dataactual = result.Skip<GeneralResponse>(start).Take(paging.rows);
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

        public async Task<ActionResult> PrintExcelTab2()
        {
            var resqheader = Request.Headers; String[] resqheaderkey = resqheader.AllKeys; String[] resqheaderval = resqheader.GetValues("requestFrom"); if (Session["authorize"] != null || resqheaderval[0] == "Mob")
            {
                string at_no = Request["at_no"] == null ? "" : Request["at_no"].Trim();
            string model = Request["model"] == null ? "" : Request["model"].Trim();
            string product = Request["product"] == null ? "" : Request["product"].Trim();
            string product_name = Request["product_name"] == null ? "" : Request["product_name"].Trim();
            string reg_dt_start = Request["reg_dt_start"] == null ? "" : Request["reg_dt_start"].Trim();
            string reg_dt_end = Request["reg_dt_end"] == null ? "" : Request["reg_dt_end"].Trim();
            string mt_cd = Request["mt_cd"] == null ? "" : Request["mt_cd"].Trim();
            string bb_no = Request["bb_no"] == null ? "" : Request["bb_no"].Trim();
            string status = Request["status_tab2"] == null ? "" : Request["status_tab2"].Trim();
            var listTotals = await _IWIPServices.printExcelTab2(at_no, model, product, product_name, reg_dt_start, reg_dt_end, mt_cd, bb_no, status);
            List<ExcelInventoryWIPComposite> listTotal = listTotals.ToList();
            List<ExcelInventoryWIPComposite> listOrderBy = new List<ExcelInventoryWIPComposite>();
            listOrderBy = listTotal.OrderBy(x => x.product_cd).ToList();

            DataTable dt = new InitMethods().ConvertListToDataTable(listOrderBy);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            ds.Tables[0].TableName = "WIP_Inventory_General";

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.AddWorksheet(dt);
                ws.Columns().AdjustToContents();
                ws.Rows().AdjustToContents();
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Alignment.ShrinkToFit = true;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= TIMS_Inventory_General.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }

            return View("~/Views/WIP/Inventory/General.cshtml");
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetgeneralDetail(Pageing paging, string product, string bb_no, string sts)
        {
            if (Session["authorize"] != null)
            {
                var result = await _IWIPServices.GetCompositeMaterialDetailData(product, bb_no, sts);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<ActionResult> qrGeneral(string mt_no)
        {
            if (Session["authorize"] != null)
            {
                if (mt_no != "")
                {
                    var result = await _IWIPServices.PrintMaterialQRInventory(mt_no);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                return View();
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }


        public async Task<ActionResult> ExportgeneralToExcel()
        {

            if (Session["authorize"] != null)
            {
                //StringBuilder varname1 = new StringBuilder();
                //varname1.Append("SELECT * FROM ( ");
                //varname1.Append("SELECT v_excelwipgeneral_one.product_cd ProductCode, v_excelwipgeneral_one.mt_no 'CODE', '' AS 'CompositeCode',v_excelwipgeneral_one.mt_nm 'NAME', CONCAT(IFNULL(v_excelwipgeneral_one.qty, 0), ' ', IFNULL(v_excelwipgeneral_one.bundle_unit, '')) 'QTY', CONCAT(IFNULL(v_excelwipgeneral_one.qty2, 0), ' ', IFNULL(v_excelwipgeneral_one.bundle_unit, '')) stk_qty, ");
                //varname1.Append("v_excelwipgeneral_one.lenght 'LENGTH', v_excelwipgeneral_one.size SIZE,'' AS STATUS, '' AS 'ReceviceDate',v_excelwipgeneral_one.mt_no 'MT_NO' ");
                //varname1.Append("FROM v_excelwipgeneral_one ");
                //varname1.Append("UNION ");
                //varname1.Append("SELECT '' AS ProductCode,'' AS 'CODE',v_excelwipgeneral_two.mt_cd 'CompositeCode', v_excelwipgeneral_two.mt_nm 'NAME', ");
                //varname1.Append("v_excelwipgeneral_two.qty 'QTY', '' stk_qty, v_excelwipgeneral_two.lenght 'LENGTH', v_excelwipgeneral_two.size SIZE, v_excelwipgeneral_two.sts_nm 'STATUS', IFNULL(DATE_FORMAT(v_excelwipgeneral_two.recevice_dt, '%Y-%m-%d'),'') 'ReceviceDATE', ");
                //varname1.Append("v_excelwipgeneral_two.mt_no 'MT_NO' ");
                //varname1.Append("FROM v_excelwipgeneral_two) AS RESULTS");

                //DataTable dt = new InitMethods().ReturnDataTableNonConstraints(varname1);

                //List<wipgeneralexportexcel> listTotal = new InitMethods().ConvertDataTable<wipgeneralexportexcel>(varname1);

                //listTotal = listTotal.OrderBy(x => x.MT_NO).ThenBy(x => x.CompositeCode).ToList();

                var data = await _IWIPServices.ExportMaterialByExcel();
                var listTotal = data.OrderBy(x => x.MT_NO).ThenBy(x => x.CompositeCode).ToList();

                DataTable datatb = new InitMethods().ConvertListToDataTable(listTotal);

                DataSet ds2 = new DataSet();

                ds2.Tables.Add(datatb);
                ds2.Tables[0].TableName = "WIP_General";
                //ds2.Tables[1].TableName = "DETAIL";
                //ds2.Tables.Add(data3);
                //ds.Tables.Add(dtEmpOrder);

                using (XLWorkbook wb = new XLWorkbook())
                {
                    //var ws = wb.AddWorksheet("ds2");
                    //ws.Columns().AdjustToContents();
                    //ws.Rows().AdjustToContents();

                    //wb.Worksheets.Add(ds2);

                    var ws = wb.AddWorksheet(datatb);

                    ws.Columns().AdjustToContents();
                    ws.Cells("C1").Value = "Composite Code";
                    ws.Cells("E1").Value = "QTY (Roll/EA)";
                    ws.Cells("F1").Value = "QTY STOCK (Roll/EA)";
                    ws.Cells("H1").Value = "Recevice Date";
                    ws.Columns("K").Hide(); //an cot I


                    ws.Rows().AdjustToContents();

                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Alignment.ShrinkToFit = true;

                    wb.Style.Font.Bold = true;
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=WIP_General.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
                return View("~/Views/WIP/Inventory/General.cshtml");
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        public ActionResult PrintGeneral()
        {
            string[] keys = Request.Form.AllKeys;

            var value = "";
            value = Request.Form[keys[0]];

            ViewData["Message"] = value;
            return View("~/Views/WIP/Inventory/PrintGeneral.cshtml");
        }

        #region PrintQr

        public ActionResult PrintQR()
        {
            return SetLanguage("~/Views/WIP/PrintQR/PrintQR.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> btnPrintQR()
        {
            string[] keys = Request.Form.AllKeys;
            var value = "";
            value = Request.Form[keys[0]];
            ViewData["Message"] = value;
            return View("~/Views/WIP/PrintQR/BtnPrintQr.cshtml");
        }

        [HttpGet]
        public async Task<ActionResult> qrPrintQr(string mt_no)
        {
            if (Session["authorize"] != null)
            {
                var rs = mt_no.Split(',');
                var listId = rs.ToList();
                var result = await _IWIPServices.GetListPrintQRInventory(listId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getPrintQR(Pageing paging)
        {
            if (Session["authorize"] != null)
            {

                string mt_no = Request["mt_no"] == null ? "" : Request["mt_no"].Trim();
                string return_date = Request["return_date"] == null ? "" : Request["return_date"].Trim();
                string recevice_dt_start = Request["recevice_dt_start"] == null ? "" : Request["recevice_dt_start"].Trim();
                string recevice_dt_end = Request["recevice_dt_end"] == null ? "" : Request["recevice_dt_end"].Trim();
                //getpicMpaging

                var dateConvert = new DateTime();
                if (DateTime.TryParse(return_date, out dateConvert))
                {
                    return_date = dateConvert.ToString("yyyy/MM/dd");
                }
                if (DateTime.TryParse(recevice_dt_end, out dateConvert))
                {
                    recevice_dt_end = dateConvert.ToString("yyyy/MM/dd");
                }
                if (DateTime.TryParse(recevice_dt_start, out dateConvert))
                {
                    recevice_dt_start = dateConvert.ToString("yyyy/MM/dd");
                }

                StringBuilder varname1 = new StringBuilder();
                varname1.Append("SELECT a.wmtid,a.mt_cd,b.mt_nm, ");
                varname1.Append("CONCAT(SUM(a.gr_qty),ifnull(b.unit_cd,'')) lenght, ");
                varname1.Append("CONCAT(ifnull(b.width,0),'*',ifnull(a.gr_qty,0)) AS size, ");
                varname1.Append("ifnull(b.spec,0) spec,a.mt_no, SUM((case when b.bundle_unit ='Roll' then  ROUND((a.gr_qty/b.spec),2) ELSE a.gr_qty END)) qty, ");
                varname1.Append("b.bundle_unit, a.return_date, ");
                varname1.Append("(SELECT dt_nm FROM comm_dt WHERE comm_dt.dt_cd=a.mt_sts_cd AND comm_dt.mt_cd='WHS005' LIMIT 1) sts_nm , ");
                varname1.Append(" (SELECT w_actual_primary.product FROM w_actual JOIN w_actual_primary ON w_actual.at_no=w_actual_primary.at_no WHERE  a.id_actual = w_actual.id_actual LIMIT 1) product, ");
                varname1.Append("(SELECT name FROM w_actual WHERE a.id_actual=w_actual.id_actual LIMIT 1) AS name ");
                varname1.Append("FROM w_material_info a ");
                varname1.Append("LEFT JOIN d_material_info  b ON a.mt_no=b.mt_no  ");
                varname1.Append("WHERE a.mt_sts_cd='004' AND a.sts_update='Return' and a.lct_cd LIKE '002%'  ");
                varname1.Append(" AND ('" + mt_no + "'='' OR  a.mt_no like '%" + mt_no + "%' ) ");
                varname1.Append(" AND ('" + return_date + "'='' OR DATE_FORMAT(a.return_date,'%Y/%m/%d') = DATE_FORMAT('" + return_date + "','%Y/%m/%d')) ");
                varname1.Append(" AND ('" + recevice_dt_start + "'='' OR DATE_FORMAT(a.recevice_dt,'%Y/%m/%d') >= DATE_FORMAT('" + recevice_dt_start + "','%Y/%m/%d')) ");
                varname1.Append(" AND ('" + recevice_dt_end + "'='' OR DATE_FORMAT(a.recevice_dt,'%Y/%m/%d') <= DATE_FORMAT('" + recevice_dt_end + "','%Y/%m/%d')) ");
                varname1.Append("GROUP BY a.mt_no");

                DataTable dt = new InitMethods().ReturnDataTableNonConstraints(varname1);

                int total = dt.Rows.Count;
                var result = dt.AsEnumerable().OrderByDescending(x => x.Field<int>("wmtid"));
                return new InitMethods().ReturnJsonResultWithPaging(paging, total, result);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<JsonResult> getPrintQRDetail(Pageing paging)
        {
            if (Session["authorize"] != null)
            {
                string mtCode = Request["mtCode"] == null ? "" : Request["mtCode"].Trim();
                var rs = await _IWIPServices.GetListDataInventoryReturn(mtCode);
                var result = rs.ToList();
                result = rs.ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }


        public async Task<JsonResult> update_lenght_qty(string wmtid, string lenght_qty)
        {
            if (Session["authorize"] != null)
            {

                var id = int.Parse(wmtid);

                var flag = await _IWIPServices.FindMaterialInventoryProductById(id); // con

                if (flag != null)
                {
                    InventoryProduct db_w_material = flag;
                    //không để n âm nhé
                    var vaule1 = await _IWIPServices.GetInventoryProductByOrginMaterialCode(db_w_material.orgin_mt_cd);
                    if (vaule1.Count() == 1)
                    {
                        var gr_qty_tmp = db_w_material.gr_qty; // mã con
                        var item = vaule1.FirstOrDefault();
                        var vaule2 = item.gr_qty; // mã cha
                        if (int.Parse(lenght_qty) <= item.real_qty)
                        {
                            //update con
                            DateTime dt = DateTime.Now;
                            db_w_material.gr_qty = int.Parse(lenght_qty);
                            db_w_material.real_qty = int.Parse(lenght_qty);
                            await _IWIPServices.UpdateQuantityInventoryProduct(db_w_material.gr_qty, db_w_material.real_qty, db_w_material.materialid);

                            //update cha
                            InventoryProduct db_w_material_rm = await _IWIPServices.FindMaterialInventoryProductById(item.materialid);
                            db_w_material_rm.gr_qty = vaule2 - (int.Parse(lenght_qty) - gr_qty_tmp);
                            await _IWIPServices.UpdateQuantityInventoryProduct(db_w_material_rm.gr_qty, db_w_material_rm.materialid);

                            var list2 = await _IWIPServices.UpdateLengthReturn(wmtid);
                            return Json(new
                            {
                                result = true,
                                message = Constant.Success,
                                data = list2.ToList()
                            }, JsonRequestBehavior.AllowGet);
                        }

                        return Json(
                              new
                              {
                                  result = false,
                                  message = "Số lượng đã vượt quá số lượng cuộn đầu!!!",
                              }, JsonRequestBehavior.AllowGet);
                    }

                }

                return Json(
                      new
                      {
                          result = false,
                          message = Constant.ErrorSystem,
                      }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public async Task<ActionResult> printQRConfirm(string id)
        {
            try
            {
                if (Session["authorize"] != null)
                {

                    var list2 = new List<ReturnModelResponse>();
                    var list = new ArrayList();
                    var a2 = id.TrimStart('[').TrimEnd(']').Split(',');
                    //for (int i = 0; i < a2.Length; i++)
                    //{
                    //    var id2 = int.Parse(a2[i]);

                        var flag = await _IWIPServices.FindListMaterialInventoryProductById(id);

                        //if (flag.Count() == 1)
                        //{
                        //    continue;
                        //}

                        // var db_w_material = await _IWIPServices.FindMaterialInventoryProductById(id2);
                        //if (db_w_material.mt_type != "CMT")
                        //{
                        //    db_w_material.id_actual = 0;
                        //}
                        //db_w_material.status = "001";
                        //db_w_material.create_date = DateTime.Now;
                        //db_w_material.change_date = DateTime.Now;
                        var wmtid = await _IWIPServices.UpdateListMaterialInventoryProduct(0, "001", DateTime.Now, DateTime.Now, id);
                       // var rs = await _IWIPServices.GetListDataUpdatedToPrintQR(id);
                      //  list2.Add(rs);
                   // }
                    return Json(new
                    {
                        result = true,
                        message = Constant.Success,
                        //data = rs
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(
                new
                {
                    result = false,
                    message = Constant.ErrorSystem
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetMemo(Pageing paging, string MTCode, string memoProductCode)
        {
            if (Session["authorize"] != null)
            {

                var result = await _IWIPServices.GetListMemoInfo(MTCode, memoProductCode);
                var rs = result.ToList().OrderByDescending(x => x.chg_dt);
                int totals = rs.Count();
                int totalPages = (int)Math.Ceiling((float)totals / paging.rows);
                int start = (paging.page - 1) * paging.rows;
                var dataactual = rs.Skip<MemoResponse>(start).Take(paging.rows);
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

        [HttpGet]
        public async Task<JsonResult> GetMemoDetail(Pageing paging, string mtCodeMemo, string memoWidth, string memoSpec, string productCodeMemo)
        {
            if (Session["authorize"] != null)
            {

                var result = await _IWIPServices.GetListMemoDetail(mtCodeMemo, memoWidth, memoSpec, productCodeMemo);
                var rs = result.ToList().OrderByDescending(x => x.id);
                int totals = rs.Count();
                int totalPages = (int)Math.Ceiling((float)totals / paging.rows);
                int start = (paging.page - 1) * paging.rows;
                var dataactual = rs.Skip<MemoDetailResponse>(start).Take(paging.rows);
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

        #endregion PrintQr

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        #endregion General

        #region Check_inventory
        public ActionResult Check_inventory()
        {
            return View();
        }
        public JsonResult GetCheckInvenSchedule(Pageing pageing, string vn_nm, string start, string end)
        {
            try
            {
                if (Session["authorize"] != null)
                {

                    StringBuilder sql = new StringBuilder($"SELECT * from  w_vt_mt as a ");
                    sql.Append(" where   ('" + vn_nm + "'='' or a.vn_nm LIKE '%" + vn_nm + "%')")
                    .Append(" AND ('" + start + "'='' OR DATE_FORMAT(a.start_dt,'%Y/%m/%d') >= DATE_FORMAT('" + start + "','%Y/%m/%d') )")
                    .Append(" AND ('" + end + "'='' OR DATE_FORMAT(a.end_dt,'%Y/%m/%d') <= DATE_FORMAT('" + end + "','%Y/%m/%d') )");
                    DataTable dt = new InitMethods().ReturnDataTableNonConstraints(sql);

                    int total = dt.Rows.Count;
                    var result = dt.AsEnumerable().OrderByDescending(x => x.Field<int>("vno"));
                    return new InitMethods().ReturnJsonResultWithPaging(pageing, total, result);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                throw;
            }
        }
        public ActionResult insertSchedule(w_vt_mt w_vt_mt, string vn_cd)
        {
            try
            {
                if (Session["authorize"] != null)
                {

                    String dateChString = DateTime.Now.ToString("yyMMdd");

                    var bien_1 = "IV" + dateChString;
                    var check_con = db.w_vt_mt.Where(x => x.vn_cd.StartsWith(bien_1)).ToList();
                    if (check_con.Count == 0)
                    {
                        w_vt_mt.vn_cd = "IV" + dateChString + "001";
                    }
                    else
                    {
                        var menuCd = string.Empty;
                        var subMenuIdConvert = 0;
                        var list1 = check_con.OrderBy(x => x.vn_cd).LastOrDefault();
                        var bien1 = list1.vn_cd;
                        var subMenuId = bien1.Substring(bien1.Length - 3, 3);
                        int.TryParse(subMenuId, out subMenuIdConvert);
                        menuCd = bien_1 + string.Format("{0}{1}", menuCd, CreateIV((subMenuIdConvert + 1)));
                        w_vt_mt.vn_cd = menuCd;
                    }

                    w_vt_mt.use_yn = "Y";
                    w_vt_mt.del_yn = "N";
                    w_vt_mt.reg_dt = DateTime.Now;
                    w_vt_mt.chg_dt = DateTime.Now;
                    w_vt_mt.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();

                    if (ModelState.IsValid)
                    {
                        db.Entry(w_vt_mt).State = EntityState.Added;
                        db.SaveChanges(); // line that threw exception
                    }
                    return Json(new { result = true, kq = w_vt_mt, message = "Thành công!!!" }, JsonRequestBehavior.AllowGet);
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
        private string CreateIV(int id)
        {
            if (id.ToString().Length < 2)
            {
                return string.Format("00{0}", id);
            }

            if (id.ToString().Length < 3)
            {
                return string.Format("0{0}", id);
            }

            if (id.ToString().Length < 4)
            {
                return string.Format("{0}", id);
            }

            return string.Empty;
        }
        public ActionResult updateSchedule(int vno, string vn_nm, string start_dt, string end_dt, string re_mark, string use_yn)
        {
            try
            {
                if (Session["authorize"] != null)
                {
                    w_vt_mt w_vt_mt = db.w_vt_mt.Find(vno);
                    w_vt_mt.vn_nm = vn_nm;
                    w_vt_mt.start_dt = start_dt;
                    w_vt_mt.end_dt = end_dt;
                    w_vt_mt.re_mark = re_mark;
                    w_vt_mt.use_yn = "Y";
                    w_vt_mt.del_yn = "N";

                    w_vt_mt.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    w_vt_mt.chg_dt = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        db.Entry(w_vt_mt).State = EntityState.Modified;
                        db.SaveChanges();
                        return Json(new { result = w_vt_mt }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetInventorySX(Pageing paging)
        {
            if (Session["authorize"] != null)
            {
                string mt_no = Request["mt_no"] == null ? "" : Request["mt_no"].Trim();
                string mt_nm = Request["mt_nm"] == null ? "" : Request["mt_nm"].Trim();
                string recevice_dt_start = Request["recevice_dt_start"] == null ? "" : Request["recevice_dt_start"].Trim();
                string recevice_dt_end = Request["recevice_dt_end"] == null ? "" : Request["recevice_dt_end"].Trim();
                string sts = Request["sts"] == null ? "" : Request["sts"].Trim();
                string vd_cd = Request["vd_cd"] == null ? "" : Request["vd_cd"].Trim();
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
                varname1.Append("SELECT a.wmtid,a.mt_cd,b.mt_nm,CONCAT(ifnull(a.gr_qty,''),ifnull(b.unit_cd,'')) lenght,CONCAT(ifnull(b.width,0),'*',ifnull(a.gr_qty,0)) AS size,ifnull(b.spec,0) spec,a.mt_no, ");
                varname1.Append(" (case when b.bundle_unit ='Roll' then  ROUND((a.gr_qty/b.spec),2) ELSE ROUND(a.gr_qty,2) END) qty, b.bundle_unit, ");
                varname1.Append("a.recevice_dt, ");
                varname1.Append("(SELECT dt_nm FROM comm_dt WHERE comm_dt.dt_cd=a.mt_sts_cd AND comm_dt.mt_cd='WHS005') sts_nm ");
                varname1.Append("FROM w_material_info a ");
                varname1.Append("LEFT JOIN d_material_info  b ON a.mt_no=b.mt_no ");
                varname1.Append("WHERE a.lct_cd LIKE '002%'  and a.mt_type<> 'CMT' AND a.mt_sts_cd not in ('005','013')  AND a.mt_cd NOT IN (SELECT mt_cd FROM w_vt_dt WHERE vn_cd='" + vd_cd + "') ");
                varname1.Append(" AND ('" + mt_nm + "'='' OR  b.mt_nm like '%" + mt_nm + "%' ) ");
                varname1.Append(" AND ('" + recevice_dt_start + "'='' OR DATE_FORMAT(a.recevice_dt,'%Y/%m/%d') >= DATE_FORMAT('" + recevice_dt_start + "','%Y/%m/%d')) ");
                varname1.Append(" AND ('" + recevice_dt_end + "'='' OR DATE_FORMAT(a.recevice_dt,'%Y/%m/%d') <= DATE_FORMAT('" + recevice_dt_end + "','%Y/%m/%d')) ");

                DataTable dt = new InitMethods().ReturnDataTableNonConstraints(varname1);

                int total = dt.Rows.Count;
                var result = dt.AsEnumerable().OrderByDescending(x => x.Field<int>("wmtid"));
                return new InitMethods().ReturnJsonResultWithPaging(paging, total, result);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult GetPickingScanMLQR(string ml_cd, string vn_cd)
        {
            try
            {
                if (Session["authorize"] != null)
                {

                    if (string.IsNullOrEmpty(ml_cd))
                    {
                        return Json(new { result = false, message = "Làm ơn scan lại!!" }, JsonRequestBehavior.AllowGet);
                    }

                    var kttt_null = db.w_material_info.Where(x => x.mt_cd == ml_cd).SingleOrDefault();
                    if (kttt_null == null)
                    {
                        return Json(new { result = false, message = "Mã Này Không có trong tồn kho !!!" }, JsonRequestBehavior.AllowGet);
                    }

                    if (!kttt_null.lct_cd.StartsWith("002"))
                    {
                        var vitri = checkvitri(kttt_null.lct_cd);
                        return Json(new { result = false, message = "Mã Này đang ở kho " + vitri + " !!!" }, JsonRequestBehavior.AllowGet);
                    }

                    if (kttt_null.mt_type == "CMT")
                    {
                        return Json(new { result = false, message = "Mã Này là mã bán thành phẩm!!!" }, JsonRequestBehavior.AllowGet);
                    }

                    if (kttt_null.mt_sts_cd != "001")
                    {
                        var trangthai = checktrangthai(kttt_null.mt_sts_cd);
                        return Json(new { result = false, message = "Trạng Thái mã này đang là " + trangthai + " Không phải tồn kho" }, JsonRequestBehavior.AllowGet);
                    }
                    if (db.w_vt_dt.Any(x => x.mt_cd == ml_cd && x.vn_cd == vn_cd))
                    {
                        return Json(new { result = false, message = "Mã này đã được vào danh sách " + vn_cd + " rồi !!!" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { result = true, message = "Thành công!!!", data = kttt_null }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult InsertMTQRIVList(string data, string vn_cd)
        {
            try
            {
                if (Session["authorize"] != null)
                {
                    //UPDATE w_material_info_tam  COLUMN SD
                    var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    var people = Session["userid"] == null ? null : Session["userid"].ToString();

                    StringBuilder varname1 = new StringBuilder();
                    varname1.Append("INSERT INTO `w_vt_dt` ( `vn_cd`, `mt_cd`, `wmtid`, `id_actual`, `id_actual_oqc`, `staff_id`, \n");
                    varname1.Append(" `staff_id_oqc`, `machine_id`, `mt_type`, `mt_no`, `gr_qty`, `real_qty`, `staff_qty`, `sp_cd`, `rd_no`, \n");
                    varname1.Append(" `sd_no`, `ext_no`, `ex_no`, `dl_no`, `recevice_dt`, `date`, `return_date`, `alert_NG`, `expiry_dt`, \n");
                    varname1.Append(" `dt_of_receipt`, `expore_dt`, `recevice_dt_tims`, `rece_wip_dt`, `picking_dt`, `shipping_wip_dt`, \n");
                    varname1.Append(" `end_production_dt`, `lot_no`, `mt_barcode`, `mt_qrcode`, `mt_sts_cd`, `bb_no`, `bbmp_sts_cd`, \n");
                    varname1.Append(" `lct_cd`, `lct_sts_cd`, `from_lct_cd`, `to_lct_cd`, `output_dt`, `input_dt`, `buyer_qr`, `orgin_mt_cd`, `reg_id`, `reg_dt`, `chg_id`, `chg_dt`) \n");
                    varname1.Append("SELECT '" + vn_cd + "',`mt_cd`, `wmtid`, `id_actual`, `id_actual_oqc`, `staff_id`, `staff_id_oqc`, `machine_id`, \n");
                    varname1.Append("`mt_type`, `mt_no`, `gr_qty`, `real_qty`, `staff_qty`, `sp_cd`, `rd_no`, `sd_no`, `ext_no`, `ex_no`, \n");
                    varname1.Append("`dl_no`, `recevice_dt`, `date`, `return_date`, `alert_NG`, `expiry_dt`, `dt_of_receipt`, `expore_dt`, \n");
                    varname1.Append(" `recevice_dt_tims`, `rece_wip_dt`, `picking_dt`, `shipping_wip_dt`, `end_production_dt`, `lot_no`, \n");
                    varname1.Append("  `mt_barcode`, `mt_qrcode`, `mt_sts_cd`, `bb_no`, `bbmp_sts_cd`, `lct_cd`, `lct_sts_cd`, `from_lct_cd`, \n");
                    varname1.Append("   `to_lct_cd`, `output_dt`, `input_dt`, `buyer_qr`, `orgin_mt_cd`,'" + people + "',NOW(),'" + people + "',NOW() \n");
                    varname1.Append("FROM w_material_info \n");
                    varname1.Append("WHERE wmtid IN (" + data + ");");
                    int effect_rows = new Excute_query().Execute_NonQuery(varname1);

                    return Json(new { result = true, message = Constant.Success }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }

        }
        public string checkvitri(string lct_Cd)
        {
            if (Session["authorize"] != null)
            {

                var checkvitri = db.lct_info.Where(x => x.lct_cd == lct_Cd).ToList();
                var csv = string.Join(", ", checkvitri.Select(x => x.lct_nm));
                return csv;
            }
            else
            {
                return Constant.ErrorAuthorize ;
            }

        }
        public string checktrangthai(string mt_sts_cd)
        {
            if (Session["authorize"] != null)
            {

                var check_trangthai = db.comm_dt.Where(x => x.mt_cd == "WHS005" && x.dt_cd == mt_sts_cd).ToList();
                var csv = string.Join(", ", check_trangthai.Select(x => x.dt_nm));

                return csv;
            }
            else
            {
                return  Constant.ErrorAuthorize;
            }

        }
        public string checkcondoan(string name)
        {
            if (Session["authorize"] != null)
            {

                var check_trangthai = db.comm_dt.Where(x => x.mt_cd == "COM007" && x.dt_cd == name).ToList();
                var csv = string.Join(", ", check_trangthai.Select(x => x.dt_nm));

                return csv;
            }
            else
            {
                return  Constant.ErrorAuthorize ;
            }

        }
        #endregion
    }
}