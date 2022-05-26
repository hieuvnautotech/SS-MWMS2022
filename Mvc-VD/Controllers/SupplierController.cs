using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_VD.Models;
using System.Collections;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using PagedList;
using Mvc_VD.Extensions;
using System.ComponentModel;
using System.Threading.Tasks;
using Mvc_VD.Services.Interface;
using System;


namespace Mvc_VD.Controllers
{
    public class SupplierController : BaseController
    {
        private Entities db = new Entities();
        private ISupplierQRServiecs _supplierServiecs;
        private readonly IhomeService _homeService;

        public SupplierController(ISupplierQRServiecs supplierServiecs,IhomeService ihomeService)
        {
            _homeService = ihomeService;
            _supplierServiecs = supplierServiecs;
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
        //
        // GET: /Supplier/
        #region Supplier_QR_Management
        public ActionResult Supplier_QR_Management()
        {
            return SetLanguage("");
        }

        [HttpGet]
        public async Task<ActionResult> Getpopupmaterial(string mt_cd, string mt_no)
        {
            //var list = db.d_material_info.Where(x => x.del_yn == "N" && x.mt_cd.StartsWith(mt_cd) && x.mt_type == "MMT" && x.mt_no != mt_no).ToList();

            var data = await _supplierServiecs.GetListMaterialInfo(mt_cd,mt_no);
            return Json(new { rows = data }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public async Task<JsonResult> searchmaterial(Pageing pageing, string type, string name, string code, string start, string end)
        {
            // Get Data from ajax function
            var dateConvert = new DateTime();
            if (DateTime.TryParse(end, out dateConvert))
            {
                end = dateConvert.ToString("yyyy/MM/dd");
            }
            if (DateTime.TryParse(start, out dateConvert))
            {
                start = dateConvert.ToString("yyyy/MM/dd");
            }
            var user = (Session["userid"] == null) ? "" : Session["userid"].ToString().Trim();
      
            var result = await _supplierServiecs.SearchListMaterialInfo(user,type,name,code,start,end);
            int totals = result.Count();
            int totalPages = (int)Math.Ceiling((float)totals / pageing.rows);

            var data = new
            {
                total = totalPages,
                page = pageing.page,
                records = totals,
                rows = result
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchAndPaging(string countSql, string viewSql, int pageIndex, int pageSize)
        {
            int totalRecords = db.Database.SqlQuery<int>(countSql).FirstOrDefault();
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            var data = new InitMethods().ReturnDataTableNonConstraints(new StringBuilder(viewSql));
            var values = ConvertDataTableToJson(data);
            var result = new
            {
                total = totalPages,
                page = pageIndex,
                records = totalRecords,
                rows = values.Data
            };
            return Json(result, JsonRequestBehavior.AllowGet);
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
        public JsonResult ConvertDataTableToJson(DataTable data)
        {
            return Json(GetTableRows(data), JsonRequestBehavior.AllowGet);
        }
        public List<Dictionary<string, object>> GetTableRows(DataTable data)
        {
            var lstRows = new List<Dictionary<string, object>>();
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

        #endregion



        #region Supplier_MachiningQR
        //public ActionResult MachiningQR()
        //{
        //    return View();
        //}
        public ActionResult MachiningQR()
        {
            return SetLanguage("");
        }

        public async Task<JsonResult> searchMachiningQR(Pageing pageing, string type, string name, string code, string start, string end)
        {
            // Get Data from ajax function
            var dateConvert = new DateTime();
            if (DateTime.TryParse(end, out dateConvert))
            {
                end = dateConvert.ToString("yyyy/MM/dd");
            }
            if (DateTime.TryParse(start, out dateConvert))
            {
                start = dateConvert.ToString("yyyy/MM/dd");
            }
            var user = "";
            var result =  await _supplierServiecs.searchMachiningQR(type, name, code, start, end);
            int totals = result.Count();
            int totalPages = (int)Math.Ceiling((float)totals / pageing.rows);

            var data = new
            {
                total = totalPages,
                page = pageing.page,
                records = totals,
                rows = result
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
