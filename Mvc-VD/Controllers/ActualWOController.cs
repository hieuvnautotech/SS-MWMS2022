using Mvc_VD.Classes;
using Mvc_VD.Commons;
using Mvc_VD.Models;
using Mvc_VD.Models.TIMS;
using Mvc_VD.Models.WOModel;
using Mvc_VD.Services;
using Mvc_VD.Services.ShinsungNew;
using MySql.Data.MySqlClient;
using PagedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WActual = Mvc_VD.Models.WOModel.WActual;
using AutoMapper;
using Mvc_VD.Services.ShinsungNew.Iservices;
using System.Threading.Tasks;
using Mvc_VD.Services.Interface.MMS;
using Mvc_VD.Models.Request;
using HttpGetAttribute = System.Web.Mvc.HttpGetAttribute;
using System.Web.Http;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;
using HttpDeleteAttribute = System.Web.Mvc.HttpDeleteAttribute;
using HttpPutAttribute = System.Web.Mvc.HttpPutAttribute;
using Mvc_VD.Models.NewVersion;
using MaterialInfoMMS = Mvc_VD.Models.NewVersion.MaterialInfoMMS;
using Mvc_VD.Services.Interface;
using Mvc_VD.Models.Response;
using Newtonsoft.Json;
using System.Web;
using Mvc_VD.Models.DMS;
using System.IO;
using System.Web.UI;
using System.ComponentModel;

namespace Mvc_VD.Controllers
{
    public class ActualWOController : BaseController
    {
        string ec = "Ca làm việc của bobbin đã kết thúc!";
        private readonly IWOService _iWOService;
        private readonly ItestWoServiceNew _itestWoServiceNew;
        private readonly IActualWOService _actualWOService;
        private readonly IcommonService _commonService;
        private readonly IhomeService _homeService;
        private List<MaterialInfoDivideMMS> listData = new List<MaterialInfoDivideMMS>();
        private ITimsService _ITIMSService;
        public ActualWOController(IWOService iWOService, ITimsService ITIMSService, IDbFactory dbFactory, ItestWoServiceNew itestWoServiceNew, IActualWOService actualWOService, IcommonService commonService, IhomeService ihomeService)
        {
            _iWOService = iWOService;
            _ITIMSService = ITIMSService;
            _itestWoServiceNew = itestWoServiceNew;
            _actualWOService = actualWOService;
            _commonService = commonService;
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

        #region Web
        public ActionResult Web()
        {
            return SetLanguage("");
        }
        #region PO
        public async Task<JsonResult> GetActual_Processes()
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
                var rs = await _actualWOService.getListActualProccess();
                var result = rs.AsEnumerable().Select(x => new
                {
                    ProcessCode = x.dt_cd,
                    ProcessName = x.dt_nm
                });
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var listData = new List<CommCode>();
                return Json(new { listData, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> getListProduct()
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
                var rs = await _actualWOService.getListProduct();
                var result = rs.AsEnumerable().Select(x => new
                {
                    sid = x.sid,
                    style_no = x.style_no,
                    style_nm = x.style_nm,
                    md_cd = x.md_cd,
                    prj_nm = x.prj_nm,
                    ssver = x.ssver,
                    part_nm = x.part_nm,
                    standard = x.standard,
                    cust_rev = x.cust_rev,
                    order_num = x.order_num,
                    cav = x.cav,
                    pack_amt = x.pack_amt,
                    bom_type = x.bom_type,
                    tds_no = x.tds_no,
                    use_yn = x.use_yn,
                    del_yn = x.del_yn,
                    reg_id = x.reg_id,
                    reg_dt =  x.reg_dt,
                    chg_id = x.chg_id,
                    chg_dt = x.chg_dt
                });
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var listData = new List<StyleInfo>();
                return Json(new { listData, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Add_w_actual_primary(ActualPrimary actual_primary)
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
                try
                {
                    //kiem tra product co ton tai khong
                    var isChecked = await _actualWOService.checkProductExisted(actual_primary.product);
                    if (isChecked < 1)
                    {
                        return Json(new { result = false, message = "Mã sản phẩm không tồn tại !" }, JsonRequestBehavior.AllowGet);
                    }
                    if (actual_primary.target < 0)
                    {
                        return Json(new { result = false, message = "Nhập target phải lớn hơn 0 !" }, JsonRequestBehavior.AllowGet);
                    }
                    if (actual_primary.target == 0)
                    {
                        return Json(new { result = false, message = "Bạn phải nhập target !" }, JsonRequestBehavior.AllowGet);
                    }
                    string dateChString = DateTime.Now.ToString("yyyyMMdd");
                    var bien_1 = "PO" + dateChString;
                    var check_con = await _actualWOService.CheckPOExisted(bien_1);
                    if (check_con == null)
                    {
                        actual_primary.at_no = "PO" + dateChString + "-001";
                    }
                    else
                    {
                        var num = check_con.at_no.Substring(check_con.at_no.Length - 3, 3);
                        int number = num.ToInt() + 1;
                        var menuCd = string.Empty;
                        var subMenuIdConvert = 0;
                        menuCd = bien_1 + "-" + string.Format("{0}{1}", menuCd, CreatePO((subMenuIdConvert + number)));
                        actual_primary.at_no = menuCd;
                    }

                    actual_primary.finish_yn = "N";
                    actual_primary.isapply = "Y";
                    actual_primary.type = "SX";
                    actual_primary.reg_dt = DateTime.Now;
                    actual_primary.chg_dt = DateTime.Now;
                    actual_primary.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    actual_primary.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();

                    if (ModelState.IsValid)
                    {
                        var checkActualPrimary = await _actualWOService.CheckPOExisted1(actual_primary.at_no);
                        if(checkActualPrimary > 0)
                        {
                            return Json(new { result = false, message = $"PO {actual_primary.at_no} đã tồn tại" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            await _actualWOService.insertNewProductOrder(actual_primary);
                        }
                        var listRouting = await _actualWOService.GetRoutingMMS(actual_primary.product,actual_primary.process_code);
                        foreach (var item in listRouting)
                        {
                            var w_actual = new Actual();
                            w_actual.at_no = actual_primary.at_no;
                            w_actual.product = actual_primary.product;
                            w_actual.actual = 0;
                            w_actual.type = (item.name.StartsWith("ROT") || item.name.StartsWith("STA") ? "SX" : "TIMS"); ;
                            w_actual.name = item.name;
                            w_actual.don_vi_pr = item.don_vi_pr;
                            w_actual.level = item.level;
                            w_actual.item_vcd = item.item_vcd;
                            w_actual.date = DateTime.Now.ToString("yyyy-MM-dd");
                            w_actual.defect = 0;
                            w_actual.description = item.description;
                            bool isF = false;
                            if (item.isFinish == "Y")
                            {
                                isF = true;
                            }

                            w_actual.IsFinish = isF;

                            w_actual.reg_dt = DateTime.Now;
                            w_actual.chg_dt = DateTime.Now;
                            w_actual.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                            w_actual.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                            await _actualWOService.insertActualInfo(w_actual);
                        }
                        IEnumerable<DatawActualPrimaryResponse> result = await _actualWOService.GetProcessingActual(actual_primary.at_no);
                        //return Json(new { result = true, kq = aaa.FirstOrDefault() }, JsonRequestBehavior.AllowGet);
                        return Json(new { result = true, kq = result.FirstOrDefault(), message = "Tạo PO thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { result = false, message = "Cann't create PO" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { result = false, kq = e }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var kq = new List<DatawActualPrimaryResponse>();
                return Json(new { result = false, data = kq.FirstOrDefault(), message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }
        }

        private string CreatePO(int id)
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

        [HttpPost]
        public async Task<JsonResult> Add_w_actual(string style_no, string at_no, string name, Actual w_actual)
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
                try
                {
                    w_actual.date = DateTime.Now.ToString("yyyy-MM-dd");
                    var check_data = await _actualWOService.checkActualExisted(at_no);
                    w_actual.type = "SX";
                    w_actual.at_no = at_no;
                    w_actual.actual = 0;
                    w_actual.name = name;
                    w_actual.defect = 0;
                    var check_con = check_data.Where(x => x.name == name).ToList();


                    if (check_con.Count() > 0)
                    {
                        w_actual.level = check_con.FirstOrDefault().level;
                    }
                    else
                    {

                        w_actual.level = (check_data.Count() + 1);
                    }

                    w_actual.product = style_no;

                    w_actual.reg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
                    w_actual.chg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
                    w_actual.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    w_actual.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    w_actual.item_vcd = "PC00004A";

                    //Code mới
                    int idactual = await _actualWOService.insertActualInfo(w_actual);
                    w_actual.id_actual = idactual;
                    //check tồn tại nguyên vật liệu composite

                    var vlieu = style_no + "-" + name;

                    var isMaterialExist = await _actualWOService.checkMaterialForActuals(vlieu);

                    if (isMaterialExist < 1)
                    {
                        MaterialInfo b = new MaterialInfo();
                        b.mt_no = vlieu;
                        b.mt_nm = style_no;
                        b.mt_type = "CMT";
                        b.reg_dt = DateTime.Now;
                        b.bundle_unit = "EA";
                        b.chg_dt = DateTime.Now;
                        b.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        b.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        b.use_yn = "Y";
                        b.del_yn = "N";

                        //Code mới
                        int idmt = await _actualWOService.insertMaterialForActual(b);


                    }


                    return Json(new { result = true, message = "Thêm công đoạn thành công!!!", kq = w_actual }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var kq = new Actual();
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !", data = kq }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPut]
        public async Task<ActionResult> updateDescriptionWActual(WActual item)
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
                try
                {
                    //kiểm tra mã này có trong db không
                    var ISCheckExist = await _actualWOService.GetActual(item.id_actual);
                    if (ISCheckExist == null)
                    {
                        return Json(new { result = false, message = "Không tìm thấy mã này để sửa nội dung." }, JsonRequestBehavior.AllowGet);
                    }
                    if (string.IsNullOrEmpty(item.description))
                    {
                        return Json(new { result = false, message = "Vui lòng nhập nội dung cần thay đổi." }, JsonRequestBehavior.AllowGet);
                    }
                    await _actualWOService.UpdateDescriptionActual(item.id_actual, item.description);

                    return Json(new { result = true, message = "Sửa thành công" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { result = false, message = "Không thể sửa mã này" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var kq = new WActual();
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !", data = kq }, JsonRequestBehavior.AllowGet);
            }

        }

        //[HttpPut]
        //public async Task<JsonResult> Modify_w_actualprimary(ActualPrimaryModify item)
        //{
        //    var resqheader = Request.Headers;
        //    string[] resqheaderkey = resqheader.AllKeys;
        //    string[] resqheaderval = resqheader.GetValues("requestFrom");
        //    if (Session["authorize"] != null || resqheaderval[0] == "Mob")
        //    {
        //        var result = await _actualWOService.updateProductOrder(item);
        //        if (result <= 0) return Json(new { result = false, message = "Không thể chỉnh sửa đổi!!" }, JsonRequestBehavior.AllowGet);
        //        else return Json(new { result = true, kq = item, message = "Bạn đã thay đổi thành công" }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        var kq = new ActualPrimaryModify();
        //        return Json(new { result = false, message = "Xin vui lòng đăng nhập !", data = kq }, JsonRequestBehavior.AllowGet);
        //    }

        //}
        public async Task<ActionResult> Modify_w_actualprimary(ActualPrimaryModify w_actual_primary)
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
                    // var find = db.w_actual_primary.Find(w_actual_primary.id_actualpr);
                    var find = await _actualWOService.GetPoInfo(w_actual_primary.id_actualpr);
                    if (find != null)
                    {

                        //kiểm tra xem các công đoạn của PO đó đã mapping chưa nếu chưa thì cho update process lại
                        // TRUE CAN'T DELETE/FALSE CAN DELETE
                        if (await _actualWOService.CheckDeletePO(find.at_no))
                        {



                        }
                        else
                        {
                            //nếu công đoạn hiện tại giống như công đoạn đang update thì không cần sửa

                            if (find.process_code != w_actual_primary.process_code)
                            {

                                //delete các công đoạn
                                await _actualWOService.DeleteMachine(find.at_no);
                                await _actualWOService.DeleteStaff(find.at_no);
                                await _actualWOService.DeleteProcess(find.at_no);

                                //Insert các công đoạn mới được chọn

                                var danhsach = await _actualWOService.GetRoutingMMS(find.product, w_actual_primary.process_code);
                                foreach (var item in danhsach)
                                {
                                    var w_actual = new Actual();
                                    w_actual.description = item.description;
                                    w_actual.at_no = find.at_no;
                                    w_actual.actual = 0;
                                    w_actual.type = item.type;
                                    w_actual.product = find.product;
                                    w_actual.name = item.name;
                                    w_actual.don_vi_pr = item.don_vi_pr;
                                    w_actual.level = item.level;
                                    w_actual.item_vcd = item.item_vcd;
                                    w_actual.date = DateTime.Now.ToString("yyyy-MM-dd");
                                    w_actual.defect = 0;
                                    bool isF = false;
                                    if (item.isFinish == "Y")
                                    {
                                        isF = true;
                                    }

                                    w_actual.IsFinished = isF;
                                    w_actual.reg_dt = DateTime.Now;
                                    w_actual.chg_dt = DateTime.Now;
                                    w_actual.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                    w_actual.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();

                                    await _actualWOService.insertActualInfo(w_actual);
                                }
                            }

                        }



                        find.target = w_actual_primary.target;
                        find.remark = w_actual_primary.remark;
                        find.process_code = w_actual_primary.process_code;
                        find.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        await _actualWOService.updateProductOrder(find);

                        return Json(new { result = true, kq = find }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { result = false, kq = "Không thể chỉnh sửa đổi!!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { result = false, kq = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetDataWActualPrimary([FromBody] DataWActualPrimaryReq req)
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
                string type = Request["type"] == null ? "" : Request["type"];
                var sql = new StringBuilder();

                if (string.IsNullOrEmpty(type))
                {
                    int start = (req.page - 1) * req.rows;
                    int end = (req.page - 1) * req.rows + req.rows;
                    //IEnumerable<DatawActualPrimary> aaa = _iWOService.GetDatawActualPrimary(product, product_name, model, at_no, regstart, regend, start, end).OrderByDescending(x => x.id_actualpr);
                    IEnumerable<DatawActualPrimaryResponse> aaa = await _actualWOService.getListPO(req);
                    var result = aaa.OrderByDescending(x => x.id_actualpr);
                    int totals = result.Count();
                    int totalPages = (int)Math.Ceiling((float)totals / req.rows);
                    IEnumerable<DatawActualPrimaryResponse> dataactual = result.Skip<DatawActualPrimaryResponse>(start).Take(req.rows);
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
                    //IEnumerable<DatawActualPrimary> dataactuals = _iWOService.GetDatawActualPrimaryFromSP(product, product_name, model, at_no, regstart, regend, "MMS").OrderByDescending(x => x.id_actualpr);
                    IEnumerable<DatawActualPrimaryResponse> dataactuals = await _actualWOService.getListPO(req);
                    var result = dataactuals.OrderByDescending(x => x.id_actualpr);
                    int start = (req.page - 1) * req.rows;
                    int totals = result.Count();
                    int totalPages = (int)Math.Ceiling((float)totals / req.rows);
                    IEnumerable<DatawActualPrimaryResponse> dataactual = result.Skip<DatawActualPrimaryResponse>(start).Take(req.rows);
                    var jsonReturn = new
                    {
                        total = totalPages,
                        page = req.page,
                        records = totals,
                        rows = dataactual
                    };
                    return Json(jsonReturn, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var data = new List<DatawActualPrimaryResponse>();
                var jsonReturn = new
                {
                    result = false,
                    total = 0,
                    page = 0,
                    records = 0,
                    rows = data,
                    message = "Xin vui lòng đăng nhập."
                };
                return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetFinishPO(Pageing pageing, [FromBody] DataWActualPrimaryReq req)
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
                IEnumerable<DatawActualPrimaryResponse> dataactuals = await _actualWOService.getListFinishPO(req);
                var result = dataactuals.OrderByDescending(x => x.id_actualpr);
                int start = (req.page - 1) * req.rows;
                int totals = result.Count();
                int totalPages = (int)Math.Ceiling((float)totals / req.rows);
                IEnumerable<DatawActualPrimaryResponse> dataactual = result.Skip<DatawActualPrimaryResponse>(start).Take(req.rows);
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
                var data = new List<DatawActualPrimaryResponse>();
                var jsonReturn = new
                {
                    result = false,
                    total = 0,
                    page = 0,
                    records = 0,
                    rows = data,
                    message = "Xin vui lòng đăng nhập !"
                };
                return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> FinishPO(string po)
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
                var id_actualpr = await _actualWOService.GetPO(po);
                if (ModelState.IsValid)
                {
                    await _actualWOService.UpdatePO("Y", id_actualpr);
                    return Json(new { result = true, id = id_actualpr, message = "Finish PO thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = "Finish PO thất bại" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, id = 0, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<JsonResult> Redo_Finish(int id_actualpr)
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
                var authData = "";

                if (Session["authData"] != null)
                {
                    authData = Session["authData"].ToString();
                }


                if (Session["authName"] == null)
                {
                    return Json(new { result = false, message = "Vui lòng đăng nhập!!!" }, JsonRequestBehavior.AllowGet);
                }
                var bien = (Session["authName"].ToString() == "Root" ? "true" : Session["wms"]);
                var check_role = await _homeService.GetListAuthorMenuInfo(authData, "FGWmsGeneralGeneral");
                if (check_role == "RL001")
                {
                    return Json(new { result = false, message = "Tài khoản của bạn chỉ để xem, không được sửa nội dung này!!!" }, JsonRequestBehavior.AllowGet);
                }

                var Id = await _actualWOService.GetRedoPO(id_actualpr);
                if (ModelState.IsValid)
                {
                    await _actualWOService.UpdatePO("N", Id);
                    return Json(new { result = true, id = Id, message = "Redo PO thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = "Redo PO thất bại" }, JsonRequestBehavior.AllowGet);
                }
            }

            else
            {
                return Json(new { result = false, id = 0, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetDataWActual([FromBody] GetDataWActualReq request)
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
                var dataActuals = await _actualWOService.getListActualOfProductOrder(request.at_no);

                int start = (request.page - 1) * request.rows;
                int totals = dataActuals.Count();
                int totalPages = (int)Math.Ceiling((float)totals / request.rows);
                var dataActual = dataActuals.Skip<ActualResponse>(start).Take(request.rows).OrderBy(x => x.level);
                var jsonReturn = new
                {
                    total = totalPages,
                    page = request.page,
                    records = totals,
                    rows = dataActual
                };
                return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = new List<ActualResponse>();
                var jsonReturn = new
                {
                    result = false,
                    total = 0,
                    page = 0,
                    records = 0,
                    rows = data,
                    message = "Xin vui lòng đăng nhập !"
                };
                return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            }

        }

        // TODO: check is use
        [HttpDelete]
        public async Task<JsonResult> xoa_wactual_con(int id)
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
                int rs = 0;
                if (id == 0)
                {
                    rs = await _actualWOService.GetIdActual();
                    id = rs;
                }
                var result = await _actualWOService.removeActualInfo(id);
                if (result == 0) return Json(new { result = false, message = "Công đoạn này đã được mapping với NVL rồi !" }, JsonRequestBehavior.AllowGet);
                else if (result == -1) return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                else return Json(new { result = true, message = "Xóa công đoạn thành công !" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetDetailActual(int id, string date, string shift)
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
                try
                {
                    var data2 = await _actualWOService.GetDetailActualAPI(id, "CMT", date, shift);
                    return Json(data2, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(e, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var kq = new List<WMaterialInfoAPI>();
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !", data = kq }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpDelete]
        public async Task<JsonResult> Deletew_actual_primary(int id)
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
                var result = await _actualWOService.removeProductOrder(id);
                if (result == -1) return Json(new { result = false, message = "Có công đoạn đang hoạt động!" }, JsonRequestBehavior.AllowGet);
                else if (result == -2) return Json(new { result = false, message = "Không tìm thấy PO đó!" }, JsonRequestBehavior.AllowGet);
                else return Json(new { result = true, message = "Xóa PO thành công !!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpDelete]
        public async Task<ActionResult> DeleteMoldMcWkActual([FromBody] DeleteMoldMcWkActualReq req)
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
                if (!string.IsNullOrEmpty(req.id))
                {
                    //StringBuilder varname1 = new StringBuilder();
                    int del = 0;
                    var time_now = DateTime.Now;
                    if (req.type == "wk")
                    {
                        int result = await _commonService.checkStaffIsWorking(req.id);
                        if (result > 0)
                        {
                            return Json(new { result = false, message = "LOT đã tồn tại!!!" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {

                            del = await _actualWOService.removeStaffMapping(req.id, time_now);
                        }
                    }
                    else if (req.type == "mc")
                    {
                        int result = await _commonService.checkMachineOrMoldIsWorking(req.id);
                        if (result > 0)
                        {
                            return Json(new { result = false, message = "LOT đã tồn tại!!!" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {

                            del = await _actualWOService.removeMachineMapping(req.id, time_now);
                        }

                    }
                    else if (req.type == "mold")
                    {
                        var result = await _actualWOService.GetDMoldUnitDataById(Convert.ToInt32(req.id));
                        if (result == null)
                        {
                            return Json(new { result = false, message = "Không tìm thấy LOT này !" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {

                            del = await _actualWOService.DeleteMold(result.mdid);
                        }
                    }
                    if (del > 0)
                    {
                        return Json(new { result = true, message = "Bạn đã xóa thành công!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, message = "Bạn đã xóa thất bại vui lòng kiểm tra lại!" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { result = false, message = "Vui lòng chọn mã cần xóa!" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<ActionResult> PartialView_dialog_Viewdetaildatetime(int id_actual)
        {
            ViewBag.id_actual = id_actual;
            return PartialView();
        }
        public ActionResult PartialView_dialog_ViewDetailMaterial(string product, string at_no)
        {
            ViewBag.product = product;
            ViewBag.at_no = at_no;
            return PartialView();
        }
        [HttpGet]
        public async Task<ActionResult> getBomdsMaterial(string style_no, string at_no, string process_code)
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
                var result = await _actualWOService.GetListDataBomMaterial(style_no, at_no, process_code);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> LayDanhsachLieuThaythe(string ProductCode, string at_no, string mt_no, string process_code)
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
                try
                {
                    var value = await _actualWOService.GetListOfSubstituteMaterials(ProductCode, at_no, mt_no, process_code);
                    return Json(value, JsonRequestBehavior.AllowGet);

                }
                catch (Exception e)
                {
                    return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> getBomdsMaterialDetail(string product, string at_no, string mt_no, string shift_dt, string shift_name)
        {
            try
            {
                IEnumerable<WActualBom> value = await _actualWOService.GetListmaterialbomdetail(product, at_no, mt_no, shift_dt, shift_name);

                return Json(value, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }
        //public ActionResult getBomdsMaterialDetailReplace(string product, string at_no, string mt_no, string shift_dt, string shift_name)
        //{
        //    try
        //    {
        //        IEnumerable<WActualBom> value = await _actualWOService.GetListmaterialbomdetailReplace(product, at_no, mt_no, shift_dt, shift_name);

        //        return Json(value, JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        #endregion

        #endregion Web

        #region Mold

        [HttpGet]
        public async Task<ActionResult> MoldMgtData(Pageing pageing)
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
                var md_no = Request["md_no"];
                var md_nm = Request["md_nm"];
                var listData = await _actualWOService.GetListDataMoldInfo(md_no, md_nm);
                int start = (pageing.page - 1) * pageing.rows;
                int totals = await _actualWOService.CountMoldInfo(md_no, md_nm);
                int totalPages = (int)Math.Ceiling((float)totals / pageing.rows);
                IEnumerable<MoldInfo> data = listData.Skip<MoldInfo>(start).Take(pageing.rows);
                var result = new
                {
                    total = totalPages,
                    page = pageing.page,
                    records = totals,
                    rows = data
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<ActionResult> Getprocess_moldunit(int id_actual)
        {
            var result = await _actualWOService.GetListMoldInfoDataByIdActual(id_actual);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public async Task<ActionResult> CreateProcessMoldUnit(string md_no, string use_yn, string remark, int id_actual)
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
                var checkMold = await _actualWOService.CheckMoldInfoData(md_no, id_actual);
                if (checkMold != null)
                {
                    return Json(new { result = 4, message = "Đã tồn tại dữ liệu này ở trong hệ thống !" }, JsonRequestBehavior.AllowGet);
                }
                var start1 = DateTime.Now;
                float timenow = float.Parse(start1.ToString("HH")) + float.Parse(DateTime.Now.ToString("mm")) / 60;
                var end = "";

                var check = await _commonService.GetEndDateProcessUnit(timenow);
                end = check.ToString("yyyy-MM-dd HH:mm:ss");

                var start = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                var dateConvert = new DateTime();
                if (DateTime.TryParse(start, out dateConvert))
                {
                    start = dateConvert.ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (DateTime.TryParse(end, out dateConvert))
                {
                    end = dateConvert.ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (DateTime.Compare(Convert.ToDateTime(start), Convert.ToDateTime(end)) > 0)
                {
                    return Json(new { result = 2, message = "Ngày bắt đầu phải nhỏ hơn ngày kết thúc !" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string status = await _actualWOService.GetStatusMold(md_no, id_actual, start, end);
                    if (status == "0")
                    {
                        var a = new DProUnitMold();
                        a.id_actual = id_actual;
                        a.start_dt = Convert.ToDateTime(start);
                        a.end_dt = Convert.ToDateTime(end);
                        a.remark = remark;
                        a.md_no = md_no;
                        a.use_yn = use_yn;
                        a.del_yn = "N";
                        a.reg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                        a.reg_dt = DateTime.Now;
                        a.chg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                        a.chg_dt = DateTime.Now;
                        var mdid = await _actualWOService.InsertIntoMoldInfo(a);
                        var data = await _actualWOService.GetDMoldUnitDataById(mdid);
                        return Json(new { result = 0, message = "Thành Công !", kq = data }, JsonRequestBehavior.AllowGet);
                    }

                    else if (status == "1")
                    {
                        return Json(new { result = 1, md_no = md_no, message = "Đơn vị Máy xử lý đã thiết lập ngày trùng lặp !" }, JsonRequestBehavior.AllowGet);
                    }

                    else
                    {
                        int id = await _actualWOService.GetIdMoldData(md_no, start, end);
                        return Json(new { result = 3, update = id, start = start, end = end }, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public async Task<ActionResult> Createprocessmold_duplicate(string md_no, string use_yn, string remark, int id_update, string start, string end, int id_actual)
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
                var dataMold = await _actualWOService.GetDMoldUnitDataById(id_update);
                if (dataMold != null)
                {
                    dataMold.end_dt = Convert.ToDateTime(start);
                    await _actualWOService.UpdateMold(dataMold);

                    var a = new DProUnitMold();
                    a.start_dt = Convert.ToDateTime(start);
                    a.end_dt = Convert.ToDateTime(end);
                    a.md_no = md_no;
                    a.use_yn = use_yn;
                    a.remark = remark;
                    a.id_actual = id_actual;
                    a.del_yn = "N";
                    a.chg_dt = DateTime.Now;
                    a.reg_dt = DateTime.Now;
                    int mdid = await _actualWOService.InsertIntoMoldInfo(a);
                    var kq = await _actualWOService.GetDMoldUnitDataById(mdid);
                    return Json(new { result = true, data = kq, message = "Thành Công !" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = false, message =  Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập" }, JsonRequestBehavior.AllowGet);
            }

        }
        public async Task<ActionResult> UpdateMold(int mdno, string md_no, string md_nm, string purpose, string re_mark)
        {
            try
            {
                var data = await _actualWOService.ModifyMoldInfo(mdno, md_no, md_nm, purpose, re_mark);
                return Json(new { result =true, data = data, message = "Thành Công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new { result = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
            }

        }
        public async Task<ActionResult> ExportToExcelMold(string md_no, string md_nm)
        {
            var listData = await _actualWOService.GetListDataMoldInfo(md_no, md_nm);

            var records = listData.ToList().AsEnumerable().Select(x => new
            {
                md_no = x.md_no,
                md_nm = x.md_nm,
                purpose = x.purpose,
                barcode = x.barcode,
                re_mark = x.re_mark,
                reg_id = x.reg_id,
                reg_dt = x.reg_dt,
                chg_id = x.chg_id,
                chg_dt = x.chg_dt
            }).ToArray();

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=MoldManagement.xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            WriteMoldHtmlTable(records, Response.Output);
            Response.End();
            return View("MoldMgt");
        }
        private void WriteMoldHtmlTable<T>(IEnumerable<T> data, TextWriter output)
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

                    foreach (PropertyDescriptor prop in props)
                    {
                        TableHeaderCell hcell = new TableHeaderCell();
                        hcell.Text = prop.Name;
                        hcell.BackColor = System.Drawing.Color.Yellow;
                        row.Cells.Add(hcell);
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
                            cell.BorderStyle = BorderStyle.Solid;
                            row.Cells.Add(cell);
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
        [HttpPost]
        public async Task<ActionResult> InsertMold(MoldInfo d_mold_info, string md_no, string md_nm, string purpose, string re_mark)
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
                //int countlist = db.d_mold_info.Count(x => x.md_no == md_no);
                int countlist = await _actualWOService.CountMoldInfoData(md_no);
                if (countlist > 0)
                {
                    return Json(new { result = false, message = "Đã tồn tại dữ liệu này ở trong hệ thống !" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    d_mold_info.md_no = md_no;
                    d_mold_info.md_nm = md_nm;
                    d_mold_info.barcode = md_no;
                    d_mold_info.purpose = purpose;
                    d_mold_info.re_mark = re_mark;
                    d_mold_info.reg_dt = DateTime.Now;
                    d_mold_info.chg_dt = DateTime.Now;
                    d_mold_info.reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                    d_mold_info.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                    d_mold_info.del_yn = "N";
                    d_mold_info.use_yn = "Y";

                    int mdid = await _actualWOService.InsertIntoMoldInfoData(d_mold_info);
                    var data = await _actualWOService.GetMoldInfoDataById(mdid);
                    return Json(new { result = true, message = "Thành Công !", kq = data }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập" }, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion

        #region Staff

        [HttpGet]
        public async Task<JsonResult> CheckNewWorkerAndMachine(int id_actual)
        {
            var mess = "";
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
                var result = await _actualWOService.CheckNewWorkerAndMachine(id_actual);
                if (result == 1)
                {
                    mess += " Công đoạn này chưa có mapping với công nhân và máy";
                    return Json(new { result = false, message = mess }, JsonRequestBehavior.AllowGet);
                }
                else if (result == 2)
                {
                    mess += " Công đoạn này chưa có mapping với máy";
                    return Json(new { result = false, message = mess }, JsonRequestBehavior.AllowGet);
                }
                else if (result == 3)
                {
                    mess += " Công đoạn này chưa có mapping với công nhân";
                    return Json(new { result = false, message = mess }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập" }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpGet]
        public async Task<ActionResult> GetStaff()
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
                var result = await _actualWOService.GetStaff("COM013", "Y");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<ActionResult> Getprocess_staff(int id_actual)
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
                int Id = 0;
                var result = new List<ProcessUnitStaff>();
                if (id_actual == 0)
                {
                    Id = await _actualWOService.GetIdActual();
                    result = (List<ProcessUnitStaff>)await _actualWOService.GetListProcessStaff(Id);
                }
                else
                {
                    result = (List<ProcessUnitStaff>)await _actualWOService.GetListProcessStaff(id_actual);
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var kq = new List<ProcessUnitStaff>();
                return Json(new { result = false, message = "Xin vui lòng đăng nhập", data = kq }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<ActionResult> SearchStaffPp([FromBody] SearchStaffPpReq req, Pageing paging)
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
                int start = (paging.page - 1) * paging.rows;
                var datapoppupStaff = await _actualWOService.GetListSearchStaffpp(req.userid, req.uname, req.position_cd, start, paging.rows);
                int totals = await _actualWOService.CountListSearchStaffpp(req.userid, req.uname, req.position_cd);
                int totalPages = (int)Math.Ceiling((float)totals / paging.rows);

                var result = new
                {
                    total = totalPages,
                    page = req.page,
                    records = totals,
                    rows = datapoppupStaff
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = new List<StaffPP>();
                var result = new
                {
                    result = false,
                    total = 0,
                    page = 0,
                    records = 0,
                    rows = data,
                    message = "Xin vui lòng đăng nhập !"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Getposition_staff()
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
                var position = await _actualWOService.GetStaff("COM018", "Y");
                return Json(position, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public async Task<ActionResult> CreateProcessUnitStaff([FromBody] CreateProcessUnitStaffReq req, [FromBody] DProUnitStaff a)
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
                try
                {
                    var start1 = DateTime.Now; //DateTime.ParseExact
                    float timenow = float.Parse(start1.ToString("HH")) + float.Parse(DateTime.Now.ToString("mm")) / 60;
                    var start = DateTime.Now;
                    var end = await _commonService.GetEndDateProcessUnit(timenow);

                    string status = await _actualWOService.GetStatusStaff(req.staff_id, a.id_actual, start, end);

                    if (status == "1")
                    {
                        return Json(new { result = 1, staff_tp = req.staff_tp, staff_id = req.staff_id, message = "Nhân viên đã thiết lập thời gian trùng lặp !" }, JsonRequestBehavior.AllowGet);
                    }

                    if (a.start_dt > a.end_dt)
                    {
                        return Json(new { result = 2, message = "Ngày bắt đầu phải nhỏ hơn ngày kết thúc !" }, JsonRequestBehavior.AllowGet);
                    }

                    a.start_dt = start;
                    a.end_dt = end;
                    a.staff_id = req.staff_id;
                    a.staff_tp = req.staff_tp;
                    a.use_yn = req.use_yn;
                    a.del_yn = "N";
                    a.chg_dt = DateTime.Now;
                    a.reg_dt = DateTime.Now;
                    int Id = await _actualWOService.InsertUnitStaff(a);
                    var result = await _actualWOService.GetStaffById(Id);
                    return Json(new { result = 0, kq = result, message = "Bạn đã thêm công nhân thành công !" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { result = -1, message = "Bạn đã thêm công nhân không thành công !" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var kq = new ProcessUnitStaff();
                return Json(new { result = 0, data = kq, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPut]
        public async Task<ActionResult> ModifyProcessUnitStaff([FromBody] ModifyProcessUnitStaffReq req)
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
                var checkStaff = await _actualWOService.CheckUnitStaff(req.staff_id);
                if (checkStaff < 0)
                {
                    return Json(new { result = 3, message = "Không tìm thấy nhân viên này" }, JsonRequestBehavior.AllowGet);
                }
                if (Convert.ToDateTime(req.start) >= Convert.ToDateTime(req.end))
                {
                    return Json(new { result = 2, staff_id = req.staff_id, message = "Lỗi Thời gian bắt đầu lớn hơn thời gian kết thúc!" }, JsonRequestBehavior.AllowGet);
                }

                DateTime maxDate = await _actualWOService.GetTimeOfMaxContainerByStaffId(req.psid);
                if (maxDate.Date != DateTime.Parse("01/01/0001"))
                {
                    if (DateTime.Compare(req.end, maxDate) < 0)
                    {
                        return Json(new { result = 3, message = "Thời gian này đã có tồn tại bán thành phẩm. Không thể chỉnh sửa ngày" }, JsonRequestBehavior.AllowGet);
                    }
                }

                DateTime minDate = await _actualWOService.GetTimeOfMinContainerByStaffId(req.psid);
                if (minDate.Date != DateTime.Parse("01/01/0001"))
                {
                    if (DateTime.Compare(req.start, minDate) > 0)
                    {
                        return Json(new { result = 2, message = "Thời gian này đã có tồn tại bán thành phẩm. Không thể chỉnh sửa ngày" }, JsonRequestBehavior.AllowGet);
                    }
                }

                var item = await _actualWOService.GetUnitStaffById(req.psid);
                item.start_dt = Convert.ToDateTime(req.start);
                item.end_dt = Convert.ToDateTime(req.end);
                item.staff_id = req.staff_id;
                item.use_yn = req.use_yn;
                item.del_yn = "N";
                item.chg_dt = DateTime.Now;
                //_iWOService.Updatedprounitstaff(item);
                await _actualWOService.UpdateUnitStaff(item);
                return Json(new { result = 0, data = item, message = "Bạn đã thay đổi công nhân thành công !" }, JsonRequestBehavior.AllowGet);
               // return Json(new { result = -1, message = "Bạn đã thay đổi công nhân không thành công !" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var kq = new DProUnitStaff();
                return Json(new { result = false, data = kq, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }

        }


        #endregion Staff

        #region Machine

        [HttpGet]
        public async Task<ActionResult> get_mc_type()
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
                var result = await _actualWOService.GetMachineType();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPut]
        public async Task<JsonResult> ModifyProcessMachineUnit([FromBody] ModifyProcessMachineUnit req, [FromBody] DProUnitMachine machineDetail)
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
                var rs = 0;
                var checkMachine = await _actualWOService.CheckMachineInfo(req.mc_no);
                if (checkMachine == 0)
                {
                    return Json(new { result = 4, message = "Máy này không tồn tại !" }, JsonRequestBehavior.AllowGet);
                }
                if (Convert.ToDateTime(req.start) >= Convert.ToDateTime(req.end))
                {
                    return Json(new { result = 2, staff_id = req.pmid, message = "Lỗi Thời gian bắt đầu lớn hơn thời gian kết thúc!" }, JsonRequestBehavior.AllowGet);
                }
                //if (req.start < DateTime.Now || req.end < DateTime.Now)
                //{
                //    return Json(new { result = 5, message = "Máy này đã được thêm vào ca hiện tại,nên bạn không thể sửa nó !" }, JsonRequestBehavior.AllowGet);
                //}


                DateTime  minDate = await _actualWOService.GetTimeOfMinContainerByMachineId(req.pmid);
                if (minDate.Date != DateTime.Parse("01/01/0001"))
                {
                   if (DateTime.Compare(req.start,minDate) > 0)
                    {
                        return Json(new { result = 2, message = "Thời gian này đã có tồn tại bán thành phẩm. Không thể chỉnh sửa" }, JsonRequestBehavior.AllowGet);
                    }
                }


                DateTime maxDate = await _actualWOService.GetTimeOfMaxContainerByMachineId(req.pmid);
                if (maxDate.Date != DateTime.Parse("01/01/0001"))
                {
                    if (DateTime.Compare(req.end, maxDate) < 0)
                    {
                        return Json(new { result = 3, message = "Thời gian này đã có tồn tại bán thành phẩm. Không thể chỉnh sửa" }, JsonRequestBehavior.AllowGet);
                    }

                }

                //DateTime minDate = await _actualWOService.GetTimeOfMinContainerByMachineId(req.pmid);
                //if (DateTime.Compare(req.start, minDate) > 0)
                //{
                //    return Json(new { result = 2, message = "Thời gian này đã có tồn tại bán thành phẩm. Không thể chỉnh sửa" }, JsonRequestBehavior.AllowGet);
                //}

                //DateTime maxDate = await _actualWOService.GetTimeOfMaxContainerByMachineId(req.pmid);
                //if (DateTime.Compare(req.end, maxDate) < 0)
                //{
                //    return Json(new { result = 3, message = "Thời gian này đã có tồn tại bán thành phẩm. Không thể chỉnh sửa" }, JsonRequestBehavior.AllowGet);
                //}

                int total = await _actualWOService.GetTotalCountMachine(req.mc_no, machineDetail.id_actual, req.pmid);

                var item = await _actualWOService.GetMachineById(req.pmid);
                if (item == null)
                {
                    return Json(new { result = 3, message = "Không tìm thấy máy có trong công đoạn để mapping  !" }, JsonRequestBehavior.AllowGet);
                }

                if (total > 0)
                {

                    int status = await _actualWOService.GetStatusMachineUpdate(req.mc_no, machineDetail.id_actual, req.pmid, req.start, req.end);
                    if (status == 0)
                    {

                        item.start_dt = req.start;
                        item.end_dt = req.end;
                        item.mc_no = req.mc_no;
                        item.use_yn = req.use_yn;

                        rs = await _actualWOService.UpdateMachine(item);
                        if (rs > 0)
                        {

                            return Json(new { result = 0, item, message = "Bạn đã thay đổi máy thành công ! " }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { result = -1, item, message = "Bạn đã thay đổi máy không thành công ! " }, JsonRequestBehavior.AllowGet);
                        }

                    }
                    else
                    {
                        return Json(new { result = 1, mc_no = req.mc_no, message = "Đơn vị máy xử lý đã thiết lập ngày trùng lặp !" }, JsonRequestBehavior.AllowGet);
                    }
                }


                item.start_dt = req.start;
                item.end_dt = req.end;
                item.mc_no = req.mc_no;
                item.use_yn = req.use_yn;
                item.remark = machineDetail.remark;

                rs = await _actualWOService.UpdateMachine(item);
                if (rs > 0)
                {

                    return Json(new { result = 0, item, message = "Bạn đã thay đổi máy thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = -1, item, message = "Bạn đã thay đổi máy không thành công ! " }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var kq = new ProcessMachineunit();
                return Json(new { result = false, data = kq, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateProcessMachineUnit(string mc_no, string use_yn, string remark, int id_actual)
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
                try
                {
                    //var checkStaffInActual = await _actualWOService.checkStaffInActual(id_actual);
                    //if (checkStaffInActual.Count() <= 0)
                    //{
                    //    return Json(new { result = 4, message = "Bạn phải mapping với công nhân trước !" }, JsonRequestBehavior.AllowGet);
                    //}
                    var start1 = DateTime.Now;
                    float timenow = float.Parse(start1.ToString("HH")) + float.Parse(DateTime.Now.ToString("mm")) / 60;
                    var end = "";

                    var check = await _commonService.GetEndDateProcessUnit(timenow);
                    end = check.ToString("yyyy-MM-dd HH:mm:ss");

                    var start = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    var dateConvert = new DateTime();
                    if (DateTime.TryParse(start, out dateConvert))
                    {
                        start = dateConvert.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (DateTime.TryParse(end, out dateConvert))
                    {
                        end = dateConvert.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (DateTime.Compare(Convert.ToDateTime(start), Convert.ToDateTime(end)) > 0)
                    {
                        return Json(new { result = 2, message = "Ngày bắt đầu phải nhỏ hơn ngày kết thúc !" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        string status = await _actualWOService.GetStatusMachine(mc_no, id_actual, start, end);
                        int machineId = 0;
                        if (status == "0")
                        {
                            var a = new DProUnitMachine();

                            a.id_actual = id_actual;
                            a.start_dt = Convert.ToDateTime(start);
                            a.end_dt = Convert.ToDateTime(end);
                            a.mc_no = mc_no;
                            a.use_yn = use_yn;
                            a.remark = remark;
                            a.del_yn = "N";
                            a.chg_dt = DateTime.Now;
                            a.reg_dt = DateTime.Now;
                            a.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                            machineId = await _actualWOService.InsertProUnitMC(a);
                            var rs = await _actualWOService.GetMachineById(machineId);
                            return Json(new { result = 0, kq = rs, message = "Bạn đã thêm máy thành công !" }, JsonRequestBehavior.AllowGet);
                        }
                        else if (status == "1")
                        {
                            return Json(new { result = 1, mc_no = mc_no, message = "Đơn vị Máy xử lý đã thiết lập ngày trùng lặp !" }, JsonRequestBehavior.AllowGet);
                        }

                        else
                        {
                            int id = await _actualWOService.GetIdMachineData(mc_no, start, end);
                            return Json(new { result = 3, update = id, start = start, end = end }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                catch (Exception e)
                {
                    return Json(new { result = -1, message = "Bạn đã thêm máy không thành công !" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var kq = new ProcessMachineunit();
                return Json(new { result = false, data = kq, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<ActionResult> SearchPopupMachine([FromBody] SearchPopupMachineReq req)
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
                var datapopupmachine = await _actualWOService.GetPopupMachine(req.mc_type, req.mc_no, req.mc_nm, start, req.rows);
                int totals = await _actualWOService.CountListPopupMachine(req.mc_type, req.mc_no, req.mc_nm);
                int totalPages = (int)Math.Ceiling((float)totals / req.rows);

                var result = new
                {
                    total = totalPages,
                    page = req.page,
                    records = totals,
                    rows = datapopupmachine
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = new List<DMachineInfoAPI>();
                var result = new
                {
                    result = false,
                    total = 0,
                    page = 0,
                    records = 0,
                    rows = data,
                    message = "Xin vui lòng đăng nhập !"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Getprocess_machineunit(int id_actual)
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
                int Id = 0;
                var result = new List<ProcessMachineunit>();
                if (id_actual == 0)
                {
                    Id = await _actualWOService.GetIdActual();
                    result = (List<ProcessMachineunit>)await _actualWOService.GetListMachineFromIdActual(Id);
                }
                else
                {
                    result = (List<ProcessMachineunit>)await _actualWOService.GetListMachineFromIdActual(id_actual);
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var kq = new List<ProcessMachineunit>();
                return Json(new { result = false, data = kq, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Createprocessmachine_duplicate(string mc_no, string use_yn, string remark, int id_update, string start, string end, DProUnitMachine a)
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
                var dataMachine = await _actualWOService.GetMachineById(id_update);
                if (dataMachine != null)
                {
                    dataMachine.end_dt = Convert.ToDateTime(start);
                    await _actualWOService.UpdateMachine(dataMachine);

                    a.start_dt = Convert.ToDateTime(start);
                    a.end_dt = Convert.ToDateTime(end);
                    a.mc_no = mc_no;
                    a.use_yn = use_yn;
                    a.remark = remark;
                    a.del_yn = "N";
                    a.chg_dt = DateTime.Now;
                    a.reg_dt = DateTime.Now;
                    int pmid = await _actualWOService.InsertProUnitMC(a);
                    var kq = await _actualWOService.GetMachineById(pmid);
                    return Json(new { result = true, data = kq, message = "Thành Công !" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion Machine

        #region get tổng hợp

        [HttpGet]
        public async Task<JsonResult> GetInfoMcWkMold(int idActual)
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
                bool isResult = false;
                var data = await _actualWOService.GetInfoMcWkMold(idActual);

                foreach (var item in data)
                {
                    item.new_start_dt = item.start_dt.ToString("yyyy-MM-dd HH:mm:ss");
                    item.new_end_dt = item.end_dt.ToString("yyyy-MM-dd HH:mm:ss");
                }

                if (data.Count() > 0)
                    isResult = true;
                return Json(new { data = data, result = isResult, message = isResult ? "Thành công" : "Không có dữ liệu" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var kq = new List<wo_info_mc_wk_mold>();
                return Json(new { data = kq, result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion get tổng hợp

        #region QC

        [HttpGet]
        public async Task<ActionResult> Getfacline_qc(string item_vcd, string mt_cd)
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
                var result = await _actualWOService.GetListFacline_Qc(item_vcd, mt_cd);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var kq = new List<FaclineQC>();
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !", data = kq }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<ActionResult> Getfacline_qc_value(string fq_no)
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
                var result = await _actualWOService.GetListFacline_qc_value(fq_no);
                var list = result.ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var kq = new List<FaclineQCValue>();
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !", data = kq }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<ActionResult> Insert_w_product_qc(string name, string style_no, string item_vcd, int check_qty,
                                                            int ok_qty, int ng_qty, string mt_cd, string date, int id_actual, FaclineQC MFQC)
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
                #region insert info
                try
                {

                    var check = await _actualWOService.GetListDataFacline_Qc(mt_cd);
                    var bien = "";
                    var check_mt_cd = await _actualWOService.GetWMaterialInfoByMaterialCode(mt_cd);
                    if (check_mt_cd == null)
                    {
                        return Json(new { result = false, message = "Lot không tìm thấy!!!" }, JsonRequestBehavior.AllowGet);
                    }
                    //kiểm tra lấy PO
                    var check_po = await _actualWOService.GetActual(check_mt_cd.id_actual);
                    var check_pr = await _actualWOService.GetDataActualPrimary(check_po.at_no);
                    var ca = "";
                    var ca_ngay = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 08:00:00"));
                    var ca_dem = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 20:00:00"));
                    var ca_dem_hs = ca_ngay.AddDays(1);
                    //tạo mã PO ca ngày hoặc ca đêm bên mms
                    if (ca_ngay <= DateTime.Now && DateTime.Now <= ca_dem)
                    {
                        ca = "CN";
                    }
                    else
                    {
                        ca = "CD";
                    }
                    bien = check_po.at_no + "-" + check_pr.product + "-" + ca + "-MMS-NG";
                    int defec_t = 0;
                    if (check.Count() > 0)
                    {

                        var fq = check.SingleOrDefault();
                        var listMaterialMapping = await _actualWOService.GetMaterialMappingByCode(fq.ml_no);
                        if (listMaterialMapping.Count() > 0)
                        {
                            return Json(new { result = false, message = "Lot đã qua công đoạn khác " }, JsonRequestBehavior.AllowGet);
                        }
                        var so_luongng = fq.check_qty - fq.ok_qty;
                        defec_t = so_luongng;

                        //xoa bảng chính
                        await _actualWOService.DeleteDataFacline_Qc(fq.fq_no);

                        //tru Mã Ng đã tạo ra trc đó
                        await _actualWOService.UpdateDataMaterialInfoMMS(bien, so_luongng, so_luongng);

                        //update số lượng về ban đầu
                        await _actualWOService.UpdateDataMaterialInfoMMS(fq.ml_no, so_luongng);

                        //update defect của công đoạn
                        await _actualWOService.UpdateDataActualInfo(id_actual, so_luongng);

                    }

                    //insert bảng m_facline_qc
                    var listData = await _actualWOService.GetListDataFacline_Qc();
                    var list = listData.ToList();
                    if (list.Count == 0)
                    {
                        MFQC.fq_no = "FQ000000001";
                    }
                    else
                    {
                        var menuCd = string.Empty;
                        var subMenuIdConvert = 0;
                        var list1 = list.LastOrDefault();

                        var bien1 = list1.fq_no;
                        var subMenuId = bien1.Substring(bien1.Length - 9, 9);
                        int.TryParse(subMenuId, out subMenuIdConvert);
                        menuCd = "FQ" + string.Format("{0}{1}", menuCd, CreateFQ((subMenuIdConvert + 1)));
                        MFQC.fq_no = menuCd;
                    }
                    var item = await _actualWOService.GetDateQcItemMaterial(item_vcd);
                    var getDataIdActual = await _actualWOService.GetActual(id_actual);

                    MFQC.at_no = getDataIdActual.at_no;
                    MFQC.check_qty = check_qty;
                    MFQC.ok_qty = ok_qty;
                    MFQC.ml_no = mt_cd;
                    MFQC.work_dt =(DateTime.Now).ToString();
                    MFQC.reg_dt = DateTime.Now;
                    MFQC.chg_dt = DateTime.Now;
                    MFQC.item_nm = item.item_nm;
                    MFQC.item_exp = item.item_exp;
                    MFQC.reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                    MFQC.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                    list.Add(MFQC);

                    var fqno = await _actualWOService.InsertIntoDataFaclineQC(MFQC);
                    //neu la ma chua check mms

                    if (bien != "")
                    {
                        var checkMaterialInfoMMSByCode = await _actualWOService.GetMaterialInfoMMSByCode(bien);
                        if (checkMaterialInfoMMSByCode.Count() < 1)
                        {
                            //nếu chưa tồn tại thì insert mã vào DB
                            int wmtid = await _actualWOService.InsertIntoDataMaterialInfoMMS(bien, check_qty, ok_qty, MFQC.reg_id, MFQC.chg_id, mt_cd);
                        }
                        else
                        {
                            //nếu tồn tại rồi thì update số lượng củ
                            //varname1.Append("UPDATE w_material_info \n");
                            //varname1.Append("SET gr_qty=(gr_qty+ " + (check_qty - ok_qty) + "),real_qty=(real_qty+ " + (check_qty - ok_qty) + ") \n");
                            //varname1.Append("WHERE mt_cd='" + bien + "';");a NG PO
                            await _actualWOService.UpdateQtyForMaterialInfoMMS1(bien, check_qty, ok_qty);

                        }
                        //mã gốc trừ NG
                        //varname1.Append("UPDATE w_material_info \n");
                        //varname1.Append("SET gr_qty=(gr_qty- " + (check_qty - ok_qty) + ") \n");
                        //varname1.Append("WHERE mt_cd='" + mt_cd + "';");
                        await _actualWOService.UpdateQtyForMaterialInfoMMS2(mt_cd, check_qty, ok_qty);

                        //update defect của công đoạn
                        //var update_defect = db.w_actual.Find(id_actual);
                        var update_defect = await _actualWOService.GetActual(id_actual);
                        update_defect.defect = (update_defect.defect - defec_t) + (check_qty - ok_qty);

                        await _actualWOService.UpdateDecfectQtyMaterialInfoMMS(update_defect.id_actual, update_defect.defect);

                        double rs = 0;
                        if (update_defect.type != "SX")
                        {
                            rs = await _actualWOService.GetSumQtyDProUnitMachineByIdActual(id_actual);
                        }
                        else
                        {
                            var quantityDefect = await _actualWOService.GetDefectQtyByIdActual(id_actual);
                            var realQty = await _actualWOService.GetRealQtyMaterialInfoMMSByIdActual(id_actual);
                            rs = realQty - quantityDefect;
                        }

                        await _actualWOService.UpdateQtyActual(id_actual, rs);
                        //MFQC,
                        return Json(new { result = true, message = "Thành Công !" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    throw e;
                }
                #endregion insert info
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }
        }
        private string CreateFQ(int id)
        {
            if (id.ToString().Length < 2)
            {
                return string.Format("00000000{0}", id);
            }

            if ((id.ToString().Length < 3) || (id.ToString().Length == 2))
            {
                return string.Format("0000000{0}", id);
            }

            if ((id.ToString().Length < 4) || (id.ToString().Length == 3))
            {
                return string.Format("000000{0}", id);
            }
            if ((id.ToString().Length < 5) || (id.ToString().Length == 4))
            {
                return string.Format("00000{0}", id);
            }
            if ((id.ToString().Length < 6) || (id.ToString().Length == 5))
            {
                return string.Format("0000{0}", id);
            }
            if ((id.ToString().Length < 7) || (id.ToString().Length == 6))
            {
                return string.Format("000{0}", id);
            }
            if ((id.ToString().Length < 8) || (id.ToString().Length == 7))
            {
                return string.Format("00{0}", id);
            }
            if ((id.ToString().Length < 9) || (id.ToString().Length == 8))
            {
                return string.Format("0{0}", id);
            }
            if ((id.ToString().Length < 10) || (id.ToString().Length == 9))
            {
                return string.Format("{0}", id);
            }
            return string.Empty;
        }

        // Warning: Api này chỉ dành cho mobile call và sử dụng
        [HttpGet]
        public async Task<ActionResult> Popup_Qc_Check_API(string item_vcd)
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
                var rs = await _actualWOService.GetListDataQCItemCheckMaterial(item_vcd);
                var ds_item_vcd = rs.ToList();
                if (ds_item_vcd.Count > 0)
                {
                    //lấy hết tất cả qc_itemcheck_mt
                    var qc_itemcheck_mt = new List<qc_itemcheck_mt_Model>();
                    var qc_itemcheck_mt2 = new List<qc_itemcheck_mt_Model>();

                    var allData = await _actualWOService.GetAllDataQCItemCheckMaterial();
                    var listData = allData.Where(x => x.item_vcd == item_vcd && x.del_yn.Equals("N"))
                                        .Select(x => new qc_itemcheck_mt_Model
                                        {
                                            qc_itemcheck_mt__check_subject = x.check_subject,
                                            qc_itemcheck_mt__check_id = x.check_id,
                                            qc_itemcheck_mt__icno = x.icno,
                                        }).ToList();
                    qc_itemcheck_mt = listData;

                    foreach (var item in qc_itemcheck_mt)
                    {
                        var view_qc_Model = new List<view_qc_Model>();

                        //lấy hết tất cả qc_itemcheck_dt
                        var allDataDetail = await _actualWOService.GetAllDataQCItemCheckDetail();
                        var listDataDetail = allDataDetail.Where(x => x.item_vcd.Equals(item_vcd) && x.check_id.Equals(item.qc_itemcheck_mt__check_id) && (x.del_yn == "N") && (x.defect_yn == "Y")).ToList();
                        var qc_itemcheck_dt = listDataDetail;
                        if (qc_itemcheck_dt.Count > 0)
                        {
                            foreach (var item1 in qc_itemcheck_dt)
                            {
                                var view_qc_Model_item = new view_qc_Model();

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

                    return Json(new { result = true, data = qc_itemcheck_mt }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var kq = new List<qc_itemcheck_mt_Model>();
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !", data = kq }, JsonRequestBehavior.AllowGet);
            }
        }

        // Warning:  Api này chỉ dành cho mobile call và sử dụng
        [HttpGet]
        public async Task<ActionResult> Insert_FaclineQc_API(FaclineQC MFQC, int check_qty, string check_qty_error, int ok_qty, string item_vcd, string mt_cd)
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
                try
                {
                    var ng_qty = check_qty_error;

                    var check = await _actualWOService.GetListDataFacline_Qc(mt_cd);
                    var bien = "";
                    var check_mt_cd = await _actualWOService.GetWMaterialInfoByMaterialCode(mt_cd);
                    if (check_mt_cd == null)
                    {
                        return Json(new { result = false, message = "Lot không tìm thấy!!!" }, JsonRequestBehavior.AllowGet);
                    }
                    var id_actual = check_mt_cd.id_actual;
                    //kiểm tra lấy PO
                    var check_po = await _actualWOService.GetActual(check_mt_cd.id_actual);
                    var check_pr = await _actualWOService.GetDataActualPrimary(check_po.at_no);
                    var ca = "";
                    var ca_ngay = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 08:00:00"));
                    var ca_dem = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 20:00:00"));
                    var ca_dem_hs = ca_ngay.AddDays(1);
                    //tạo mã PO ca ngày hoặc ca đêm bên mms
                    if (ca_ngay <= DateTime.Now && DateTime.Now <= ca_dem)
                    {
                        ca = "CN";
                    }
                    else
                    {
                        ca = "CD";
                    }
                    bien = check_po.at_no + "-" + check_pr.product + "-" + ca + "-MMS-NG";
                    int defec_t = 0;
                    if (check.Count() > 0)
                    {

                        var fq = check.SingleOrDefault();
                        var listMaterialMapping = await _actualWOService.GetMaterialMappingByCode(fq.ml_no);
                        if (listMaterialMapping.Count() > 0)
                        {
                            return Json(new { result = false, message = "Lot đã qua công đoạn khác " }, JsonRequestBehavior.AllowGet);
                        }
                        var so_luongng = fq.check_qty - fq.ok_qty;
                        defec_t = so_luongng;

                        //xoa bảng chính
                        await _actualWOService.DeleteDataFacline_Qc(fq.fq_no);

                        //tru Mã Ng đã tạo ra trc đó
                        await _actualWOService.UpdateDataMaterialInfoMMS(bien, so_luongng, so_luongng);

                        //update số lượng về ban đầu
                        await _actualWOService.UpdateDataMaterialInfoMMS(fq.ml_no, so_luongng);

                        //update defect của công đoạn
                        await _actualWOService.UpdateDataActualInfo(id_actual, so_luongng);

                    }

                    //insert bảng m_facline_qc
                    var listData = await _actualWOService.GetListDataFacline_Qc();
                    var list = listData.ToList();
                    if (list.Count == 0)
                    {
                        MFQC.fq_no = "FQ000000001";
                    }
                    else
                    {
                        var menuCd = string.Empty;
                        var subMenuIdConvert = 0;
                        var list1 = list.LastOrDefault();

                        var bien1 = list1.fq_no;
                        var subMenuId = bien1.Substring(bien1.Length - 9, 9);
                        int.TryParse(subMenuId, out subMenuIdConvert);
                        menuCd = "FQ" + string.Format("{0}{1}", menuCd, CreateFQ((subMenuIdConvert + 1)));
                        MFQC.fq_no = menuCd;
                    }

                    var item = await _actualWOService.GetDateQcItemMaterial(item_vcd);
                    var getDataIdActual = await _actualWOService.GetActual(id_actual);

                    MFQC.check_qty = check_qty;
                    MFQC.ok_qty = ok_qty;
                    MFQC.ml_no = mt_cd;
                    MFQC.work_dt = (DateTime.Now).ToString();
                    MFQC.reg_dt = DateTime.Now;
                    MFQC.chg_dt = DateTime.Now;
                    MFQC.item_nm = item.item_nm;
                    MFQC.item_exp = item.item_exp;
                    MFQC.reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                    MFQC.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                    list.Add(MFQC);
                    var fqno = await _actualWOService.InsertIntoDataFaclineQC(MFQC);

                    //neu la ma chua check mms

                    if (bien != "")
                    {
                        var checkMaterialInfoMMSByCode = await _actualWOService.GetMaterialInfoMMSByCode(bien);
                        if (checkMaterialInfoMMSByCode.Count() < 1)
                        {
                            //nếu chưa tồn tại thì insert mã vào DB
                            int wmtid = await _actualWOService.InsertIntoDataMaterialInfoMMS(bien, check_qty, ok_qty, MFQC.reg_id, MFQC.chg_id, mt_cd);
                        }
                        else
                        {
                            //nếu tồn tại rồi thì update số lượng củ
                            //varname1.Append("UPDATE w_material_info \n");
                            //varname1.Append("SET gr_qty=(gr_qty+ " + (check_qty - ok_qty) + "),real_qty=(real_qty+ " + (check_qty - ok_qty) + ") \n");
                            //varname1.Append("WHERE mt_cd='" + bien + "';");a NG PO
                            await _actualWOService.UpdateQtyForMaterialInfoMMS1(bien, check_qty, ok_qty);

                        }

                        //mã gốc trừ NG
                        //varname1.Append("UPDATE w_material_info \n");
                        //varname1.Append("SET gr_qty=(gr_qty- " + (int.Parse(check_qty) - int.Parse(ok_qty)) + ") \n");
                        //varname1.Append("WHERE mt_cd='" + mt_cd + "';");
                        await _actualWOService.UpdateQtyForMaterialInfoMMS2(mt_cd, check_qty, ok_qty);


                        //update defect của công đoạn
                        //var update_defect = db.w_actual.Find(id_actual);
                        var update_defect = await _actualWOService.GetActual(id_actual);
                        update_defect.defect = (update_defect.defect - defec_t) + (check_qty - ok_qty);

                        await _actualWOService.UpdateDecfectQtyMaterialInfoMMS(update_defect.id_actual, update_defect.defect);

                        return Json(new { result = true, message = "Thành Công !" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }

            #region insert info

            #endregion insert info
        }

        #endregion QC

        #region mapping

        #region container

        [HttpGet]
        public async Task<ActionResult> searchbobbinPopup(Pageing paging, string bb_no, string bb_nm, string mt_cd, int? id_actual)
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
                int start = (paging.page - 1) * paging.rows;
                var data = await _actualWOService.GetListBobbin(bb_no, bb_nm, mt_cd, id_actual, start, paging.rows);

                var totalRecords = await _actualWOService.CountListBobbin(bb_no, bb_nm, mt_cd, id_actual);
                int totalPages = (int)Math.Ceiling((float)totalRecords / paging.rows);
                return Json(new
                {
                    total = totalPages,
                    page = paging.page,
                    records = totalRecords,
                    rows = data
                }, JsonRequestBehavior.AllowGet);

                //var data = await _actualWOService.GetListBobbin(bb_no, bb_nm, mt_cd, id_actual, 0, 0);
                //var rs = Json(data, JsonRequestBehavior.AllowGet);
                //rs.MaxJsonLength = int.MaxValue;
                //return rs;
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> searchbobbinPopupDV(string bb_no, string bb_nm, string mt_cd)
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
                try
                {
                    var data = await _actualWOService.SearchBobbinInfo(bb_no, bb_nm);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return Json(e, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion container

        [HttpPost]
        public async Task<JsonResult> AddTrayAndBobbin([FromBody] Models.NewVersion.MaterialInfoMMS a, [FromBody] MaterialMappingMMS c, int id_actual, string name, string bb_no)
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
                try
                {
                    string style_no = Request["style_no"] == null ? "" : Request["style_no"].Trim();
                    if(style_no==null || style_no == "")
                    {
                        return Json(new { result = false, message = "Product không tồn tại!" }, JsonRequestBehavior.AllowGet);
                    }
                    var currentBobbin = await _actualWOService.GetBobbinInfo(bb_no);

                    //Kiểm tra xem dồ đựng đó có tồn tại ở trong hệ thống hay không
                    if (currentBobbin == null)
                    {
                        return Json(new { result = false, message = "Đồ đựng này không tồn tại!" }, JsonRequestBehavior.AllowGet);
                    }
                    //Kiểm tra xem dồ đựng đó đã được sử dụng hay chưa
                    if (currentBobbin.mt_cd == "")
                    {
                        currentBobbin.mt_cd = null;
                    }
                    if (currentBobbin.mt_cd != null)
                    {
                        return Json(new { result = false, message = "Đồ đựng này đã được sử dụng" }, JsonRequestBehavior.AllowGet);
                    }

                    int checkStaff = await _actualWOService.CheckStaffShift(id_actual);
                    if (checkStaff == 0)
                    {
                        return Json(new { result = false, message = "Ca này của công nhân đã kết thúc !" }, JsonRequestBehavior.AllowGet);
                    }

                    int checkMachine = await _actualWOService.CheckMachineShift(id_actual);
                    if (checkMachine == 0)
                    {
                        return Json(new { result = false, message = "Máy hết ca!Gia hạn hoặc tạo thêm máy!!" }, JsonRequestBehavior.AllowGet);
                    }

                    var check_exit_vo = await _actualWOService.GetActual(id_actual);
                    //var check_exit_vo = db.w_actual.Find(id_actual);
                    if (check_exit_vo == null)
                    {
                        return Json(new { result = false, message = "Không tìm thấy Mã PO với mã lot này" }, JsonRequestBehavior.AllowGet);
                    }

                    //string GetStaffOfProcess = string.Join(",", _iWOService.GetStaffOfProcess(id_actual));
                    //string GetStaffProcess = string.Join(",", await _actualWOService.GetStaffOnProcess(id_actual));
                    //string GetMachineProcess = string.Join(",", await _actualWOService.GetMachineOnProcess(id_actual));

                    DateTime dateTime = DateTime.Now; //Today at 00:00:00

                    string Hour = dateTime.ToString("HHmmss");
                    string Date = dateTime.ToString("yyyyMMdd");
                    string FulldateTime = dateTime.ToString("yyyyMMddHHmmss");
                    string MtNo = style_no + "-" + check_exit_vo.name;


                    string CreateUser = Session["userid"] == null ? null : Session["userid"].ToString();

                    string MTCode = MtNo + FulldateTime;
                    int CountMTCode = await _actualWOService.CountDataMaterialInfo(MTCode) + 1;
                    string MtCodeAuto = UTL.ConvertToString(CountMTCode, 6);
                    string FullMtCode = MTCode + UTL.ConvertToString(CountMTCode, 6);

                    //bắt đầu insert vào bảng w_material_info_mms
                    //a.product = check_exit_vo.product;
                    //a.at_no = check_exit_vo.at_no;

                    a.material_type = "CMT";
                    a.bb_no = bb_no;
                    a.mt_no = MtNo;
                    //a.bbmp_sts_cd = "000";
                    a.status = "002";
                    a.location_code = "002002000000000000";
                    a.from_lct_code = "002002000000000000";
                    a.id_actual = id_actual;
                    a.real_qty = 0;
                    a.gr_qty = 0;
                    a.chg_date = DateTime.Now;
                    a.chg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                    a.reg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                    a.reg_date = DateTime.Now;
                    a.material_code = FullMtCode;
                    var checkFullMtCode = await _actualWOService.checkFullMtCode(FullMtCode);
                    if(checkFullMtCode > 0)
                    {
                        return Json(new {  result = false, message = "Ml No này đã tồn tại" }, JsonRequestBehavior.AllowGet);
                    }
                    int idWMaterialInfo = await _actualWOService.InsertMaterialInfoMMS(a);
                    //var wmaterialinfo = await _actualWOService.GetDetailMaterialInfoMMS(idWMaterialInfo);
                    //mapping mt_cd với bobin

                    currentBobbin.mt_cd = FullMtCode;
                    currentBobbin.bb_no = bb_no;
                    currentBobbin.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    //_iWOService.UpdateBobbinMTCode(currentBobbin);
                    await _actualWOService.UpdateBobbinMTCode(currentBobbin);

                    //check lai function InsertToBobbinLctHistory
                    var checkBobbinLctHst = await _actualWOService.CheckBobbinHistoryLocation(currentBobbin.bb_no, FullMtCode);
                    if(checkBobbinLctHst < 1)
                    {
                        var d_bobbin_lct_hist = new BobbinLctHist();
                        d_bobbin_lct_hist.bb_no = currentBobbin.bb_no;
                        d_bobbin_lct_hist.bb_nm = currentBobbin.bb_nm;
                        d_bobbin_lct_hist.mc_type = currentBobbin.mc_type;
                        d_bobbin_lct_hist.mt_cd = FullMtCode;
                        d_bobbin_lct_hist.use_yn = "Y";
                        d_bobbin_lct_hist.del_yn = "N";
                        d_bobbin_lct_hist.reg_dt = DateTime.Now;
                        d_bobbin_lct_hist.chg_dt = DateTime.Now;
                        d_bobbin_lct_hist.reg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                        d_bobbin_lct_hist.chg_id = Session["userid"] == null ? "" : Session["userid"].ToString();

                        int currentBobbinInfo = await _actualWOService.InsertToBobbinHistory(d_bobbin_lct_hist);
                    }

                    var take = style_no + name;
                    //var mt_lot_con = _iWOService.Getmtlot(id_actual);
                    // Kế thừa nguyên vật liệu
                    var mt_lot_con = await _actualWOService.GetMaterialByIdActual(id_actual, FullMtCode);
                    //var mt_lot_con = await _actualWOService.CheckBobbinInheritance1(Mt_lot);

                    if (!string.IsNullOrEmpty(mt_lot_con))
                    {
                        await _actualWOService.InsertMaterialMapping(FullMtCode, Session["userid"] == null ? null : Session["userid"].ToString(), mt_lot_con);

                        await _actualWOService.UpdateMaterialInfoFromMappingOtherType(Session["userid"] == null ? null : Session["userid"].ToString(), "CMT", FullMtCode);


                    }
                    var ds1 = await _actualWOService.GetMaterialInfoWo(MtNo, FullMtCode, idWMaterialInfo);
                    var mt_cd = MTCode.Remove(MTCode.Length - 12, 12);

                    int gtri = await _actualWOService.GetSumGroupQty(mt_cd);
                    return Json(new { ds1 = ds1, gtri = gtri, result = true, message = "Bạn đã thêm đồ đựng thành công" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var kq = new w_material_info_wo();
                return Json(new { data = kq, gtri = 0, result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<string> check_staff(int id_actual)
        {
            var data = await _actualWOService.CheckStaff(id_actual);
            var ketqua = data.Select(x => x.staff_id);
            var csv = string.Join(", ", ketqua);
            return csv;
        }

        public async Task<string> machine_sx(int id_actual)
        {
            var data = await _actualWOService.CheckMachine(id_actual);
            var ketqua = data.Select(x => x.mc_no);
            var csv = string.Join(", ", ketqua);
            return csv;
        }

        #region table1
        //xoa container

        [HttpDelete]
        public async Task<ActionResult> Xoa_mt_pp_composite(int id)
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
                var a = await _actualWOService.GetDetailMaterialInfoMMS(id);
                if (id > 0)
                {
                    //kiêm tra hết ca chưa
                    //sửa được

                    var staff_id = await _actualWOService.GetListStaffFromActual(a.id_actual);
                    var data1 = await _actualWOService.CheckShiftOfStaff(a.id_actual, staff_id);
                    if (data1 == 0)
                    {
                        return Json(new { result = false, message = ec }, JsonRequestBehavior.AllowGet);
                    }
                    //kiểm tra đã qua công đoạn khác chưa
                    var checkMaterialMapping = await _actualWOService.CheckMaterialMapping(a.material_code);
                    if (checkMaterialMapping > 0)
                    {
                        return Json(new { result = false, message = "Bobbin này đã được chuyển qua ở công đoạn khác" }, JsonRequestBehavior.AllowGet);
                    }

                    if (a.gr_qty > 0 || a.real_qty > 0)
                    {
                        return Json(new { result = false, message = "Đồ đựng này đã được nhập sản lượng, Bạn không thể xóa nó!!!" }, JsonRequestBehavior.AllowGet);
                    }

                    int countmapping = await _actualWOService.StatusMapping(a.material_code);
                    if (countmapping > 0)
                    {
                        return Json(new { result = false, message = "Đã Tồn tại mã Kết thúc!! Không thể Xóa!" }, JsonRequestBehavior.AllowGet);
                    }

                    //Update NVL về trang thái ban đầu
                    var ds_mapping = await _actualWOService.GetMaterialMappingByMaterialCode(a.material_code);
                    foreach (var item in ds_mapping)
                    {

                        var b = await _actualWOService.GetMaterialMappingById(item.wmmId);

                        //kiểm trả về trạng thái cho nguyên vật liệu
                        var check_mt = await _actualWOService.GetMaterialInventoryProductforBobbin(b.mt_cd, "CMT");
                        if (check_mt != null)
                        {
                            //update active về bằng 0
                            int count = await _actualWOService.GetCountMaterialMapping(b.mt_cd, b.mt_lot);
                            if (count <= 0)
                            {
                                check_mt.status = "001";
                                check_mt.id_actual = 0;
                            }
                            else
                            {
                                check_mt.status = "002";
                            }

                            await _actualWOService.UpdateInventoryProduct(check_mt);
                        }
                        //xóa bảng w_material_mapping_mms
                        await _actualWOService.DeleteMaterialMapping(b.wmmId);
                        ////xã bobin
                        //var bb_no = await _actualWOService.GetBBNoFromMaterialMMS(b.mt_cd, b.mt_no);
                        //var find_bb1 = await _actualWOService.GetBobbinInfo(bb_no);
                        //if (find_bb1 != null)
                        //{
                        //    find_bb1.mt_cd = null;
                        //    find_bb1.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        //    await _actualWOService.UpdateBobbinMTCode(find_bb1);
                        //}
                        ////xóa histor
                        //var find_history1 = await _actualWOService.GetBobbinLctHistory(bb_no, b.mt_cd);
                        //if (find_history1 != null)
                        //{
                        //    await _actualWOService.DeleteBobbinLctHist(find_history1.blno);

                        //}
                    }

                    await _actualWOService.DeleteMaterialInfoMMs(a.wmtid);
                    //xã bobin
                    var find_bb = await _actualWOService.GetBobbinInfo(a.bb_no);
                    if (find_bb != null)
                    {


                        find_bb.mt_cd = null;
                        find_bb.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        await _actualWOService.UpdateBobbinMTCode(find_bb);
                    }
                    //xóa histor
                    var find_history = await _actualWOService.GetBobbinLctHistory(a.bb_no, a.material_code);
                    if (find_history != null)
                    {
                        await _actualWOService.DeleteBobbinLctHist(find_history.blno);
                        return Json(new { result = true, message = "Bạn đã xóa thành công !" }, JsonRequestBehavior.AllowGet);

                    }
                }
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetMtDateWeb(Pageing Pageing, int id_actual)
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
                try
                {
                    var rs = await _actualWOService.GetMtDateWebs(id_actual);
                    var datas = rs.OrderByDescending(x => x.wmtid);
                    int totalcounts = datas.Count();
                    IEnumerable<MtDateWebWO> data = datas.Skip((Pageing.page - 1) * Pageing.rows).Take(Pageing.rows);
                    var aa = data.ToList();
                    int totalPages = (int)Math.Ceiling((float)totalcounts / Pageing.rows);
                    var jsonReturn = new
                    {
                        total = totalPages,
                        page = Pageing.page,
                        records = totalcounts,
                        rows = data
                    };
                    return Json(jsonReturn, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> check_update_grty(string mt_cd, int wmtid, int value)
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
                try
                {
                    //check  không qua công đoạn trước  và chưa kiểm tra PQC
                    var userAcount = Session["userid"] == null ? null : Session["userid"].ToString();
                    var userWIP = "";
                    if (!string.IsNullOrEmpty(userAcount))
                    {

                        var dsMbInfo = await _actualWOService.GetMbInfoGrade(userAcount);
                        userWIP = dsMbInfo.grade;
                    }

                    var checkFaclineqc = await _actualWOService.CheckFaclineqc(mt_cd);
                    if (checkFaclineqc > 0 && userWIP != "Admin")
                    {
                        return Json(new { result = false, message = "Đã kiểm tra PQC", wmtid = wmtid }, JsonRequestBehavior.AllowGet);
                    }

                    var data = await _actualWOService.GetDetailMaterialInfoMMS(wmtid);

                    var staff_id = await _actualWOService.GetListStaffFromActual(data.id_actual);
                    int data1 = await _actualWOService.CheckShiftOfStaff(data.id_actual, staff_id);

                    if (data1 == 0 && userWIP != "Admin")
                    {
                        return Json(new { result = false, message = ec }, JsonRequestBehavior.AllowGet);
                    }


                    var at_no = await _actualWOService.GetAtNoByActual(data.id_actual);
                    var checkYorN = await _actualWOService.IsCheckApply(at_no);
                    if (checkYorN == "Y")
                    {
                        //Phải cho nhập NVL vào trước rồi sau đó mới cho nhập sản lượng
                        var checkNVLToMapping = await _actualWOService.CheckMaterialToMapping(mt_cd);
                        if (checkNVLToMapping < 1)
                        {
                            return Json(new { result = false, message = "Vui lòng mapping với nguyên vật liệu trước khi nhập sản lượng" }, JsonRequestBehavior.AllowGet);
                        }
                        //kiểm tra đã scan NVL và BTP chưa
                        //nếu là level= 1 thì chỉ cần kiểm tra NVL thôi, còn level 2 trở lên là phải có BTP
                        // Lấy ra danh sách cần scan NVL cho công đoạn này

                        //lấy ra danh sách đã scan NVL không tồn tại trong NVL đã đăng kí thì return false
                        var listProcess = await _actualWOService.GetActual(data.id_actual);
                        var actualPrimary = await _actualWOService.GetDataActualPrimary(listProcess.at_no);
                        var listMapping = await _actualWOService.GetMaterialMappingNVL(actualPrimary.product, mt_cd, listProcess.name, actualPrimary.process_code);
                        if (listMapping.Count() > 0)
                        {
                            return Json(new { result = false, message = "Bạn scan thiếu NVL" }, JsonRequestBehavior.AllowGet);
                        }
                        if (listProcess.level > 1)
                        {
                            //LẤY TÊN BTP CỦA CÔNG ĐOẠN TRƯỚC NÓ
                            var TenBTP = await _actualWOService.NameBTP(at_no, listProcess.name);
                            if (TenBTP != null)
                            {
                                var mt_no = TenBTP.Product + "-" + TenBTP.NameProcess;
                                var isBTPExistInMapping = await _actualWOService.IsBTPExistByMapping(mt_cd, mt_no);
                                if (isBTPExistInMapping == "false")
                                {
                                    return Json(new { result = false, message = "Đồ đựng này chưa scan Bán Thành Phẩm" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                    }
                    data.gr_qty = value;
                    data.real_qty = value;
                    data.number_divide = 0;
                    data.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();

                    await _actualWOService.UpdateMaterialInfoMMS(data);
                    var data_ds = await _actualWOService.GetUpdateGrty(data.wmtid);

                    var real_qty = await _actualWOService.CalQuantityForActual(data.id_actual);
                    await _actualWOService.UpdateActualForWActual(real_qty, data.id_actual);

                    return Json(new { result = true, kq = data_ds, wmtid = wmtid, message = "Bạn đã nhập sản lượng thành công !" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { result = false, kq = e, wmtid = wmtid, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var kq = new List<CheckUpdateGrty>();
                return Json(new { result = false, data = kq, wmtid = 0, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }
        }

        #region popupmaterial_mapping

        [HttpGet]
        public async Task<ActionResult> GetType_Marterial()
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
                var type = await _actualWOService.GetTypeMaterial();
                return Json(type, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" });
            }

        }

        [HttpGet]
        public async Task<JsonResult> Popup_composite_mt(Pageing Pageing, string type, string mt_no, string mt_cd, int id_actual)
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
                var status = "";
                var data = await _actualWOService.PopupCompositeMaterial(mt_cd);
                var count = data.Count();
                int pageIndex = Pageing.page;
                int pageSize = Pageing.rows;
                int startRow = (pageIndex * pageSize) + 1;
                int totalRecords = count;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var result = new
                {
                    total = totalPages,
                    page = pageIndex,
                    records = count,
                    rows = data.ToArray().ToPagedList(pageIndex, pageSize)
                };
                return Json(result, status = "success", JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion popupmaterial_mapping

        #endregion table1

        #region table2

        [HttpGet]
        public async Task<ActionResult> ds_mapping_w(string mt_cd)
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
                var data = await _actualWOService.GetDataMappingMaterial(mt_cd);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> insertw_material_mping(string bb_no, string mt_cd, string mt_mapping, int id_actual)
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
                try
                {
                    //check nó đang tồn tại trong bảng nào=> inventory_product hay w_material_info_mms
                    //mt_cd: mã lot đầu ra
                    //mt_mapping: mã mt_cd đầu vào
                    int checkMachine = await _actualWOService.CheckMachineShift(id_actual);
                    if (checkMachine == 0)
                    {
                        return Json(new { result = false, message = "Máy hết ca!Gia hạn hoặc tạo thêm máy!!" }, JsonRequestBehavior.AllowGet);
                    }

                    //Nếu đầu ra là bb là cái cũ nhất thì khong cho mapping
                    //lấy được giá trị nếu là true thì return(tức db bằng rông or null )
                    var MaxBobbin = await _actualWOService.CheckMaxBobbin(mt_cd, id_actual);
                    if (MaxBobbin == null)
                    {
                        return Json(new { result = false, message = "Bobbin này đã cũ rồi vui lòng chọn Bobbin mới nhất." }, JsonRequestBehavior.AllowGet);
                    }
                    var checkMaterialMapping = await _actualWOService.CheckMaterialMapping(mt_cd);
                    if (checkMaterialMapping > 0)
                    {
                        return Json(new { result = false, message = "Bobbin này đã được chuyển qua ở công đoạn khác!!" }, JsonRequestBehavior.AllowGet);
                    }

                    var qr = await _actualWOService.GetWMaterialInfoByMaterialCode(mt_cd);
                    if (qr == null)
                    {
                        return Json(new { result = false, message = "Vui Lòng Chọn Đồ Đựng Đầu Ra!!" }, JsonRequestBehavior.AllowGet);
                    }

                    var materialId = 0;
                    var ds_bb = await _actualWOService.GetBobbinInfo(bb_no);
                    var ds_bb_his = await _actualWOService.GetBobbinLctHist(bb_no);


                    if (string.IsNullOrEmpty(mt_mapping))
                    {

                        // nếu k mapping mã nguyên vât liệu sẽ mapping mã bobbin để lấy
                        if (ds_bb == null)
                        {
                            return Json(new { result = false, message = "Đồ Đựng này không tồn tại trên hệ thống!!" }, JsonRequestBehavior.AllowGet);
                        }
                        mt_mapping = ds_bb.mt_cd;


                        if (ds_bb_his == null)
                        {
                            return Json(new { result = false, message = "Đồ Đựng này không tồn tại trên hệ thống!!" }, JsonRequestBehavior.AllowGet);
                        }
                        mt_mapping = ds_bb_his.mt_cd;

                        if (!string.IsNullOrEmpty(mt_mapping))
                        {
                            // mapping bobin
                            ds_bb.count_number = 1;
                            ds_bb.bb_no = bb_no;
                            ds_bb.mt_cd = mt_mapping;
                            ds_bb.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                            await _actualWOService.UpdateBobbinInfo(ds_bb);
                        }
                        else
                        {
                            return Json(new { result = false, message = "Vui Lòng Chọn Đồ Đựng Đầu Ra!!" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        //kiểm tra xem mã nvl có được đưa ra máy chưa, nếu cột ExportCode,LoctionMachine có dữ liệu thì tiếp tục, ngược lại return false
                        var NVLTaiMay = await _actualWOService.GetMaterialFromInventoryProducts(mt_mapping);

                        if (NVLTaiMay == null)
                        {
                            return Json(new { result = false, message = "NVL này chưa được nhập kho sản xuất!!!" }, JsonRequestBehavior.AllowGet);
                        }
                        if (string.IsNullOrEmpty(NVLTaiMay.LoctionMachine) && string.IsNullOrEmpty(NVLTaiMay.ExportCode))
                        {
                            return Json(new { result = false, message = "NVL này chưa được xuất ra Máy" }, JsonRequestBehavior.AllowGet);
                        }
                        materialId = NVLTaiMay.materialid;
                    }


                    var checkMaterial = await _actualWOService.CheckMaterialMappingOfBobbbin(mt_mapping);
                    if (checkMaterial == 0)
                    {
                        return Json(new { result = false, message = "Không tìm thấy NVL " + mt_mapping + " này ở trong hệ thống" }, JsonRequestBehavior.AllowGet);
                    }

                    else if (checkMaterial == 1)
                    {
                        var mt = await _actualWOService.CheckMaterialLotNG_OK_InvenotryProduct(mt_mapping);
                        //kiểm tra mã này đủ tiêu chuẩn để mapping không
                        if (mt != null)
                        {
                            //kiểm tra xem Product này chọn Y/N

                            //lấy producct
                            var Dsproduct = await _actualWOService.GetActual(id_actual);
                            var ActualPrimary = await _actualWOService.GetDataActualPrimary(Dsproduct.at_no);
                            //var checkYorN = await _actualWOService.IsCheckApply(Dsproduct.at_no);

                            //tức là product này tồn tại apply =Y và nó là nguyên vật liệu
                            // nếu chọn N không cần kiểm tra
                            if (ActualPrimary != null && mt.mt_type.Equals("PMT"))
                            {
                                if (ActualPrimary.isapply.Contains("Y"))
                                {
                                    //Nếu là Y thì kiểm tra NVL có thuộc NVL trong product đó không
                                    // nếu chọn N không cần kiểm tra
                                    var isMaterialExistInProcess = await _actualWOService.IsMaterialInfoExistByProcess(ActualPrimary.product, Dsproduct.name, mt.mt_no, ActualPrimary.process_code);

                                    if (isMaterialExistInProcess.Equals("false"))
                                    {
                                        return Json(new { result = false, message = "NVL này chưa được đăng kí ở tại Product: " + ActualPrimary.product + " Công đoạn:  " + ActualPrimary.process_code + " - " + Dsproduct.name }, JsonRequestBehavior.AllowGet);
                                    }
                                }

                            }

                            if (mt.status != "002" && mt.status != "001")
                            {
                                var checkStatus = await _actualWOService.CheckStatus("WHS005", mt.status);
                                var trangthai = string.Join(", ", checkStatus.Select(x => x.dt_nm));


                                return Json(new { result = false, message = "Trạng Thái mã này đang là: " + trangthai }, JsonRequestBehavior.AllowGet);
                            }

                            int check_exits_mapping = await _actualWOService.GetCountMaterialMapping(mt_mapping, mt_cd);
                            if (check_exits_mapping > 0)
                            {
                                return Json(new { result = false, message = "Mã này đang được sử dụng ở công đoạn khác!!! " }, JsonRequestBehavior.AllowGet);
                            }

                            if (!mt.location_code.StartsWith("002"))
                            {
                                var VITRI = await checkvitri(mt.location_code);
                                return Json(new { result = false, message = "Mã Này Đang ở KHO " + VITRI }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { result = false, message = "NVL này chưa được nhập kho sản xuất!!!" }, JsonRequestBehavior.AllowGet);
                        }

                        if (qr != null && mt != null)
                        {
                            var id_lot = qr.id_actual;
                            ////kiêm tra hết ca chưa
                            int data = await _actualWOService.CheckHetCaMT(id_lot, qr.material_code);
                            if (data < 1)
                            {
                                return Json(new { result = false, message = ec }, JsonRequestBehavior.AllowGet);
                            }

                            var a = new MaterialMappingMMS();
                            a.mt_no = mt.mt_no;

                            int mapp = await _actualWOService.GetMaterialMapping(a.mt_no, mt_mapping, mt_cd);
                            if (mapp == 0)
                            {
                                if (mt_cd == mt_mapping)
                                {
                                    return Json(new { result = false, message = "Làm ơn kiểm tra lại mã lot này" }, JsonRequestBehavior.AllowGet);
                                }

                                a.mt_cd = mt_mapping;
                                a.mt_lot = mt_cd;
                                //a.bb_no = bb_no;
                                a.use_yn = "Y";
                                //a.bb_no = qr.bb_no;
                                a.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                a.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                a.reg_date = DateTime.Now;
                                a.chg_date = DateTime.Now;
                                a.mapping_dt = DateTime.Now;
                                int wmmid = await _actualWOService.InsertMaterialMapping(a);

                                var update1 = await _actualWOService.GetDetailInventoryProduct(materialId);
                                if (update1.mt_type != "CMT")
                                {
                                    update1.id_actual = id_actual;
                                    update1.status = "002";
                                    update1.change_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                    update1.create_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                    update1.change_date = DateTime.Now;
                                    update1.create_date = DateTime.Now;
                                    await _actualWOService.UpdateInventoryProduct(update1);
                                }

                                bool checkres = false;
                                var datares = await _actualWOService.GetDataSaveReturn(wmmid);
                                if (datares.Count() > 0)
                                    checkres = true;
                                return Json(new { data = datares, result = checkres, message = "Mapping nguyên vật liệu thành công !" }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { result = false, message = "Dữ liệu đã được mapping" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }

                    else if (checkMaterial == 2)
                    {

                        var checkMapping = await _actualWOService.CheckExistMaterialMapping(mt_cd, mt_mapping);
                        if (checkMapping > 0)
                        {
                            return Json(new { result = false, message = "NVL này đã được mapping rồi! " }, JsonRequestBehavior.AllowGet);
                        }

                        var mt = await _actualWOService.CheckMaterialLotNG_OK(mt_mapping);
                        //kiểm tra mã này đủ tiêu chuẩn để mapping không
                        if (mt != null)
                        {
                            //kiểm tra xem Product này chọn Y/N

                            //lấy producct
                            var Dsproduct = await _actualWOService.GetActual(id_actual);
                            var ActualPrimary = await _actualWOService.GetDataActualPrimary(Dsproduct.at_no);
                            //var checkYorN = await _actualWOService.IsCheckApply(Dsproduct.at_no);

                            //tức là product này tồn tại apply =Y và nó là nguyên vật liệu
                            // nếu chọn N không cần kiểm tra
                            if (ActualPrimary != null && mt.material_type.Equals("PMT"))
                            {
                                if (ActualPrimary.isapply.Contains("Y"))
                                {
                                    //Nếu là Y thì kiểm tra NVL có thuộc NVL trong product đó không
                                    // nếu chọn N không cần kiểm tra
                                    var isMaterialExistInProcess = await _actualWOService.IsMaterialInfoExistByProcess(ActualPrimary.product, Dsproduct.name, mt.mt_no, ActualPrimary.process_code);

                                    if (isMaterialExistInProcess.Equals("false"))
                                    {
                                        return Json(new { result = false, message = "NVL này chưa được đăng kí ở tại Product: " + ActualPrimary.product + " Công đoạn:  " + ActualPrimary.process_code +" - " +  Dsproduct.name }, JsonRequestBehavior.AllowGet);
                                    }
                                }

                            }

                            if (mt.status != "002" && mt.status != "001")
                            {
                                var checkStatus = await _actualWOService.CheckStatus("WHS005", mt.status);
                                var trangthai = string.Join(", ", checkStatus.Select(x => x.dt_nm));

                                return Json(new { result = false, message = "Trạng Thái mã này đang là: " + trangthai }, JsonRequestBehavior.AllowGet);
                            }

                            int check_exits_mapping = await _actualWOService.GetCountMaterialMapping(mt_mapping, mt_cd);
                            if (check_exits_mapping > 0)
                            {
                                return Json(new { result = false, message = "Mã này đang được sử dụng ở công đoạn khác!!! " }, JsonRequestBehavior.AllowGet);
                            }

                            if (!mt.location_code.StartsWith("002"))
                            {
                                var VITRI = await checkvitri(mt.location_code);
                                return Json(new { result = false, message = "Mã Này Đang ở KHO " + VITRI }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { result = false, message = "NVL này chưa được nhập kho sản xuất!!!" }, JsonRequestBehavior.AllowGet);
                        }

                        if (qr != null && mt != null)
                        {
                            var id_lot = qr.id_actual;
                            ////kiêm tra hết ca chưa
                            int data = await _actualWOService.CheckHetCaMT(id_lot, qr.material_code);
                            if (data < 1)
                            {
                                return Json(new { result = false, message = ec }, JsonRequestBehavior.AllowGet);
                            }

                            var qrAT_no = await _actualWOService.GetActual(qr.id_actual);
                            var mtAT_no = await _actualWOService.GetActual(mt.id_actual);

                            var a = new MaterialMappingMMS();
                            a.mt_no = mt.mt_no;
                            var check_po = "";
                            if (mt.material_type == "CMT")
                            {
                                if (qrAT_no.at_no == mtAT_no.at_no)
                                {
                                    check_po = "OK";
                                }
                                if (check_po == "")
                                {
                                    return Json(new { result = false, message = "Chọn Sai PO! Xin vui lòng chọn lại!" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {

                            }


                            int mapp = await _actualWOService.GetMaterialMapping(a.mt_no, mt_mapping, mt_cd);
                            if (mapp == 0)
                            {
                                if (mt_cd == mt_mapping)
                                {
                                    return Json(new { result = false, message = "Làm ơn kiểm tra lại mã lot này" }, JsonRequestBehavior.AllowGet);
                                }

                                a.mt_cd = mt_mapping;
                                a.mt_lot = mt_cd;
                                a.use_yn = "Y";
                                a.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                a.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                a.reg_date = DateTime.Now;
                                a.chg_date = DateTime.Now;
                                a.mapping_dt = DateTime.Now;
                                int wmmid = await _actualWOService.InsertMaterialMapping(a);

                                var update1 = await _actualWOService.GetDetailMaterialInfoMMS(mt.wmtid);
                                if (update1.material_type != "CMT")
                                {
                                    update1.id_actual = id_actual;
                                    var staff_sx = check_staff(id_actual);
                                    var machinesx = machine_sx(id_actual);
                                }

                                update1.status = "002";
                                update1.id_actual = update1.id_actual;
                                update1.chg_date = DateTime.Now;
                                update1.chg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                                await _actualWOService.UpdateMaterialInfoMMS(update1);

                                bool checkres = false;
                                var datares = await _actualWOService.GetDataSaveReturn(wmmid);
                                if (datares.Count() > 0)
                                    checkres = true;
                                return Json(new { data = datares, result = checkres, message = "Mapping nguyên vật liệu thành công !" }, JsonRequestBehavior.AllowGet);
                            }
                        }

                    }

                    return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                var kq = new List<SaveReturn>();
                return Json(new { data = kq, result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProductDeApply(int id_actualpr, string IsApply)
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
                await _actualWOService.UpdateProductDeApply(id_actualpr, IsApply);
                return Json(new { result = true, message = "Thành Công !" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<string> checkvitri(string lct_Cd)
        {

            var rs = await _actualWOService.GetNameFromLocationInfo(lct_Cd);
            var checkvitri = rs.ToList();
            var csv = string.Join(", ", checkvitri.Select(x => x.lct_nm));
            return csv;
        }

        [HttpGet]
        public async Task<JsonResult> Finish_back(int wmmid)
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
                try
                {
                    var data = await _actualWOService.GetMaterialMappingById(wmmid);
                    var checkMaterialMappingOfBobbin = await _actualWOService.CheckMaterialMappingOfBobbbin(data.mt_cd);
                    if (checkMaterialMappingOfBobbin == 0)
                    {
                        return Json(new { result = false, message = "Không tìm thấy mã này" }, JsonRequestBehavior.AllowGet);
                    }
                    if (checkMaterialMappingOfBobbin == 1)
                    {
                        #region CHECK BOBIN
                        //Nếu đầu ra là bb là cái cũ nhất thì khong cho mapping
                        //lấy được giá trị nếu là true thì return(tức db bằng rông or null )
                        var rs = await _actualWOService.CheckMaterialLotNG_OK(data.mt_lot);
                        int id_actual = rs.id_actual;
                        var IsMaxBobbin = await _actualWOService.CheckMaxBobbin(data.mt_lot, id_actual);
                        if (IsMaxBobbin == "" || IsMaxBobbin == null)
                        {
                            return Json(new { result = false, message = "NVL này đang được kế thừa nên không thể kết thúc." }, JsonRequestBehavior.AllowGet);
                        }
                        #endregion

                        var checkReturn = await _actualWOService.CheckInventoryProductRetun(data.mt_cd);
                        if (checkReturn != null)
                        {
                            return Json(new { result = false, message = "Marterial đã được trả về kho rồI, thao tác này không được thực hiện" }, JsonRequestBehavior.AllowGet);
                        }

                        var userAcount = Session["userid"] == null ? "" : Session["userid"].ToString();
                        var userWIP = "";
                        if (!string.IsNullOrEmpty(userAcount))
                        {
                            var dsMbInfo = await _actualWOService.GetMbInfoGrade(userAcount);
                            userWIP = dsMbInfo.grade;
                        }
                        //int checklotmapping1 = await _actualWOService.CheckMaterialMappingFinish(data.mt_cd, data.mt_lot, data.mapping_dt);
                        //int checklotmapping2 = await _actualWOService.CheckMaterialMappingFinish(data.mt_lot, data.mt_cd, data.mapping_dt);

                        //if (userWIP != "Admin" && (checklotmapping1 > 0 || checklotmapping2 > 0))
                        //{
                        //    return Json(new { result = false, message = "Mã Code này đã được kế thừa qua Lot khác!!!" }, JsonRequestBehavior.AllowGet);
                        //}

                        if (data.use_yn == "Y")
                        {
                            var ds = await _actualWOService.GetMaterialByInventoryProduct(data.mt_cd);
                            var ds_2 = await _actualWOService.GetMaterialByInventoryProduct(data.mt_lot);

                            if (ds != null)
                            {
                                if (ds_2 != null)
                                {
                                    var listStaffId = await _actualWOService.GetStaffFromActualByInventoryProduct(ds_2.id_actual);
                                    int data2 = await _actualWOService.CheckShiftOfStaff(ds_2.id_actual, listStaffId);
                                    if (data2 == 0 && userWIP != "Admin")
                                    {
                                        return Json(new { result = false, message = ec }, JsonRequestBehavior.AllowGet);
                                    }
                                }

                                data.use_yn = "N";
                                data.chg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                                await _actualWOService.UpdateMaterialMappingMMS(data);


                                var id_fo1 = ds.materialid;
                                var update1 = await _actualWOService.GetDetailInventoryProduct(id_fo1);
                                update1.status = "005";
                                update1.change_id = Session["userid"] == null ? "" : Session["userid"].ToString();

                                await _actualWOService.UpdateInventoryProduct(update1);

                                await _actualWOService.UpdatedBobbinInfoForDevice(update1.bb_no, update1.material_code, Session["userid"] == null ? "" : Session["userid"].ToString());

                                await _actualWOService.DeleteDBobbinLctHistForDevice(update1.bb_no, update1.material_code);

                                return Json(new { result = true, message = "Kết thúc mã thành công!!!", use_yn = data.use_yn }, JsonRequestBehavior.AllowGet);
                            }
                            return Json(new { result = false, message = "Không tìm thấy mã này" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //trở về trạng thái đầu
                            //kiểm tra trạng thái có đủ điều kiện để trở về không
                            data.del_yn = "N";
                            data.use_yn = "Y";
                            data.chg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                            await _actualWOService.UpdateMaterialMappingMMS(data);

                            var ds = await _actualWOService.GetMaterialByInventoryProduct(data.mt_cd);
                            if (ds != null)
                            {
                                var id_fo1 = ds.materialid;
                                var update1 = await _actualWOService.GetDetailInventoryProduct(id_fo1);
                                update1.status = "002";
                                update1.change_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                                //_iWOService.UpdateMaterialInfo(update1);
                                await _actualWOService.UpdateInventoryProduct(update1);

                                if (update1.bb_no != "" && update1.bb_no != null)
                                {
                                    var check_bb = await _actualWOService.GetBobbinInfo(update1.bb_no);
                                    if (check_bb != null)
                                    {
                                        check_bb.chg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                                        check_bb.mt_cd = update1.material_code;
                                        check_bb.count_number = 1;
                                        await _actualWOService.UpdateBobbinInfo(check_bb);
                                        // //add history đã xóa
                                        var history = new BobbinLctHist();
                                        history.bb_no = check_bb.bb_no;
                                        history.bb_nm = check_bb.bb_nm;
                                        history.mt_cd = data.mt_cd;
                                        history.mc_type = check_bb.mc_type;
                                        history.reg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                                        history.chg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                                        history.reg_dt = DateTime.Now;
                                        history.chg_dt = DateTime.Now;
                                        history.del_yn = "N";
                                        history.use_yn = "Y";
                                        //_iWOService.InsertToBobbinLctHistory(history);
                                        await _actualWOService.InsertToBobbinHistory(history);
                                    }
                                }
                                return Json(new { result = true, message = "Trở về trạng thái cũ thành công", use_yn = data.use_yn }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    if (checkMaterialMappingOfBobbin == 2)
                    {
                        #region CHECK BOBIN
                        //Nếu đầu ra là bb là cái cũ nhất thì khong cho mapping
                        //lấy được giá trị nếu là true thì return(tức db bằng rông or null )
                        var rs = await _actualWOService.CheckMaterialLotNG_OK(data.mt_lot);
                        int id_actual = rs.id_actual;
                        var IsMaxBobbin = await _actualWOService.CheckMaxBobbin(data.mt_lot, id_actual);
                        if (IsMaxBobbin == "" || IsMaxBobbin == null)
                        {
                            return Json(new { result = false, message = "NVL này đang được kế thừa nên không thể kết thúc." }, JsonRequestBehavior.AllowGet);
                        }
                        #endregion

                        //Nếu NVL NÀY ĐÃ BỊ RETURN THÌ KHÔNG ĐƯỢC FINISH
                        var checkReturn = await _actualWOService.CheckMaterialRetun(data.mt_cd);
                        if (checkReturn != null)
                        {
                            return Json(new { result = false, message = "Marterial đã được trả về kho rồI, thao tác này không được thực hiện" }, JsonRequestBehavior.AllowGet);
                        }

                        var userAcount = Session["userid"] == null ? "" : Session["userid"].ToString();
                        var userWIP = "";
                        if (!string.IsNullOrEmpty(userAcount))
                        {
                            var dsMbInfo = await _actualWOService.GetMbInfoGrade(userAcount);
                            userWIP = dsMbInfo.grade;
                        }

                        //int checklotmapping1 = await _actualWOService.CheckMaterialMappingFinish(data.mt_cd, data.mt_lot, data.mapping_dt);
                        //int checklotmapping2 = await _actualWOService.CheckMaterialMappingFinish(data.mt_lot, data.mt_cd, data.mapping_dt);

                        //if (userWIP != "Admin" && (checklotmapping1 > 0 || checklotmapping2 > 0))
                        //{
                        //    return Json(new { result = false, message = "Mã Code này đã được kế thừa qua Lot khác!!!" }, JsonRequestBehavior.AllowGet);
                        //}

                        if (data.use_yn == "Y")
                        {
                            var ds = await _actualWOService.GetWMaterialInfoByMaterialCode(data.mt_cd);
                            var ds_2 = await _actualWOService.GetWMaterialInfoByMaterialCode(data.mt_lot);

                            if (ds != null)
                            {
                                if (ds_2 != null)
                                {
                                    var listStaffId = await _actualWOService.GetListStaffFromActual(ds_2.id_actual);
                                    int data2 = await _actualWOService.CheckShiftOfStaff(ds_2.id_actual, listStaffId);
                                    if (data2 == 0 && userWIP != "Admin")
                                    {
                                        return Json(new { result = false, message = ec }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                data.use_yn = "N";
                                data.chg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                                await _actualWOService.UpdateMaterialMappingMMS(data);


                                var id_fo1 = ds.wmtid;
                                var update1 = await _actualWOService.GetDetailMaterialInfoMMS(id_fo1);
                                update1.status = "005";
                                update1.chg_id = Session["userid"] == null ? "" : Session["userid"].ToString();

                                await _actualWOService.UpdateMaterialInfoMMS(update1);

                                await _actualWOService.UpdatedBobbinInfoForDevice(update1.bb_no, update1.material_code, Session["userid"] == null ? "" : Session["userid"].ToString());

                                await _actualWOService.DeleteDBobbinLctHistForDevice(update1.bb_no, update1.material_code);

                                return Json(new { result = true, message = "Kết thúc mã thành công!!!", use_yn = data.use_yn }, JsonRequestBehavior.AllowGet);
                            }
                            return Json(new { result = false, message = "Không tìm thấy mã này" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //trở về trạng thái đầu
                            //kiểm tra trạng thái có đủ điều kiện để trở về không
                            data.del_yn = "N";
                            data.use_yn = "Y";
                            data.chg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                            await _actualWOService.UpdateMaterialMappingMMS(data);

                            var ds = await _actualWOService.GetWMaterialInfoByMaterialCode(data.mt_cd);
                            if (ds != null)
                            {
                                var id_fo1 = ds.wmtid;
                                var update1 = await _actualWOService.GetDetailMaterialInfoMMS(id_fo1);
                                update1.status = "002";
                                update1.chg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                                //_iWOService.UpdateMaterialInfo(update1);
                                await _actualWOService.UpdateMaterialInfoMMS(update1);

                                if (update1.bb_no != "" && update1.bb_no != null)
                                {
                                    var check_bb = await _actualWOService.GetBobbinInfo(update1.bb_no);
                                    if (check_bb != null)
                                    {
                                        check_bb.chg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                                        check_bb.mt_cd = update1.material_code;
                                        check_bb.count_number = 1;
                                        await _actualWOService.UpdateBobbinInfo(check_bb);
                                        // //add history đã xóa
                                        var history = new BobbinLctHist();
                                        history.bb_no = check_bb.bb_no;
                                        history.bb_nm = check_bb.bb_nm;
                                        history.mt_cd = data.mt_cd;
                                        history.mc_type = check_bb.mc_type;
                                        history.reg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                                        history.chg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                                        history.reg_dt = DateTime.Now;
                                        history.chg_dt = DateTime.Now;
                                        history.del_yn = "N";
                                        history.use_yn = "Y";
                                        //_iWOService.InsertToBobbinLctHistory(history);
                                        await _actualWOService.InsertToBobbinHistory(history);
                                    }
                                }
                                return Json(new { result = true, message = "Trở về trạng thái cũ thành công", use_yn = data.use_yn }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }

                    return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<JsonResult> savereturn_lot(int soluong, string mt_cd, string mt_lot)
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
                try
                {
                    var checkMaterialMappingOfBobbin = await _actualWOService.CheckMaterialMappingOfBobbbin(mt_cd);
                    if (checkMaterialMappingOfBobbin == 0)
                    {
                        return Json(new { result = false, message = "Không tìm thấy mã !!" }, JsonRequestBehavior.AllowGet);
                    }
                    if (checkMaterialMappingOfBobbin == 1)
                    {
                        #region CHECK BOBIN
                        //Nếu đầu ra là bb là cái cũ nhất thì khong cho mapping
                        //lấy được giá trị nếu là true thì return(tức db bằng rông or null )
                        var rs = await _actualWOService.CheckMaterialLotNG_OK(mt_lot);
                        int id_actual = rs.id_actual;
                        var IsMaxBobbin = await _actualWOService.CheckMaxBobbin(mt_lot, id_actual);
                        if (IsMaxBobbin == "" || IsMaxBobbin == null)
                        {
                            return Json(new { result = false, message = "Mã này đang được kế thừa nên không thể trả về." }, JsonRequestBehavior.AllowGet);
                        }
                        #endregion

                        //mt_cd:mã đầu vào
                        //mt_lot:mã đầu ra
                        var check_exit = await _actualWOService.GetDataInventoryProduct(mt_cd);
                        if (check_exit == null)
                        {
                            return Json(new { result = false, message = "Mã này không tồn tại ở trong hệ thống!" }, JsonRequestBehavior.AllowGet);
                        }

                        if (check_exit.mt_type == "CMT")
                        {
                            return Json(new { result = false, message = "Mã này là bán thành phẩm, không được trả về." }, JsonRequestBehavior.AllowGet);
                        }

                        var check_exit1 = await _actualWOService.GetMaterialMappingReturn(mt_cd, mt_lot);
                        if (check_exit1 == null)
                        {
                            return Json(new { result = false, message = "Mã này không tìm thấy!!" }, JsonRequestBehavior.AllowGet);
                        }


                        //kiểm tra đã finish chưa
                        if (check_exit1.use_yn == "N")
                        {
                            return Json(new { result = false, message = "Mã này đã được kết thúc trước đó!" }, JsonRequestBehavior.AllowGet);
                        }
                        if (soluong == 0)
                        {
                            return Json(new { result = false, message = "Làm ơn kiểm tra lại số lượng trả về!" }, JsonRequestBehavior.AllowGet);
                        }
                        //kiểm tra số lượng có vượt quá k

                        if (soluong > check_exit.gr_qty)
                        {
                            return Json(new { result = false, message = "Số lượng trả về phải ít hơn số lượng tồn tại!" }, JsonRequestBehavior.AllowGet);
                        }

                        //kiem tra da mapping voi cong doan khac chua
                        //var checkMaterialMapping1 = await _actualWOService.CheckMaterialMapping(mt_lot);
                        //if (checkMaterialMapping1 > 0)
                        //{
                        //    return Json(new { result = false, message = "Mã này là bán thành phẩm, không thể trả về!" }, JsonRequestBehavior.AllowGet);
                        //}
                        //tách số lượng

                        var soluongcl_mt_cd = check_exit.gr_qty - (soluong);
                        //finish
                        check_exit1.use_yn = "N";
                        check_exit1.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        await _actualWOService.UpdateMaterialMappingMMS(check_exit1);

                        var change_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        await _actualWOService.UpdatePartialInventoryProduct(check_exit.materialid, "005", soluongcl_mt_cd, change_id, DateTime.Now);

                        //tạo mã mới cho số lượng còn dư và có trạng thái là return
                        //mt_sts_cd=004,sts_update='return',soluong=input,orgin=mt_cd
                        int count_return = await _actualWOService.CheckExitReturn(mt_cd);
                        await _actualWOService.InsertMaterialMappingReturn(count_return + 1, soluong, "004", check_exit.change_id, mt_cd);

                        if (check_exit.bb_no != "" && check_exit.bb_no != null)
                        {
                            //xã bobin
                            var check_bb = await _actualWOService.GetBobbinInfoReturn(check_exit.bb_no, check_exit.material_code);
                            if (check_bb != null)
                            {
                                check_bb.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                check_bb.mt_cd = mt_cd + "-" + "RT" + (count_return + 1);
                                await _actualWOService.UpdateBobbinMTCode(check_bb);

                                //find history để xóa
                                var history = await _actualWOService.GetBobbinLctHistory(check_bb.bb_no, mt_cd);
                                if (history != null)
                                {
                                    history.mt_cd = check_bb.mt_cd;
                                    await _actualWOService.UpdateBobbinLctHist(history);
                                }
                            }
                        }

                        //kiem tra co bao nhieu mt_cd da qua cong doan ma chua finish
                        await _actualWOService.UpdateMaterialMappingForDivide(check_exit.material_code, "Y", "N", Session["userid"] == null ? null : Session["userid"].ToString());
                        var data = await _actualWOService.GetDataSaveReturn(check_exit1.wmmId);
                        bool checkc = false;
                        if (data != null)
                            checkc = true;
                        return Json(new { data = data.FirstOrDefault(), result = checkc, message = "Thành Công!" }, JsonRequestBehavior.AllowGet);
                    }
                    if (checkMaterialMappingOfBobbin == 2)
                    {
                        //mt_cd:mã đầu vào
                        //mt_lot:mã đầu ra
                        var check_exit = await _actualWOService.GetWMaterialInfoByMaterialCode(mt_cd);
                        if (check_exit == null)
                        {
                            return Json(new { result = false, message = "Mã này không tồn tại ở trong hệ thống!" }, JsonRequestBehavior.AllowGet);
                        }

                        if (check_exit.material_type == "CMT")
                        {
                            return Json(new { result = false, message = "Mã này là bán thành phẩm, không được trả về." }, JsonRequestBehavior.AllowGet);
                        }
                        var check_exit1 = await _actualWOService.GetMaterialMappingReturn(mt_cd, mt_lot);
                        if (check_exit1 == null)
                        {
                            return Json(new { result = false, message = "Mã này không tìm thấy!!" }, JsonRequestBehavior.AllowGet);
                        }
                        //kiểm tra đã finish chưa
                        if (check_exit1.use_yn == "N")
                        {
                            return Json(new { result = false, message = "Mã này đã được kết thúc trước đó!" }, JsonRequestBehavior.AllowGet);
                        }
                        if (soluong == 0)
                        {
                            return Json(new { result = false, message = "Làm ơn kiểm tra lại số lượng trả về!" }, JsonRequestBehavior.AllowGet);
                        }
                        //kiểm tra số lượng có vượt quá k

                        if (soluong > check_exit.gr_qty)
                        {
                            return Json(new { result = false, message = "Số lượng trả về phải ít hơn số lượng tồn tại!" }, JsonRequestBehavior.AllowGet);
                        }
                        #region CHECK BOBIN
                        //Nếu đầu ra là bb là cái cũ nhất thì khong cho mapping
                        //lấy được giá trị nếu là true thì return(tức db bằng rông or null )
                        var rs = await _actualWOService.CheckMaterialLotNG_OK(check_exit1.mt_lot);
                        int id_actual = rs.id_actual;
                        var IsMaxBobbin = await _actualWOService.CheckMaxBobbin(check_exit1.mt_lot, id_actual);
                        if (IsMaxBobbin == "" || IsMaxBobbin == null)
                        {
                            return Json(new { result = false, message = "NVL này đang được kế thừa nên không thể trả về." }, JsonRequestBehavior.AllowGet);
                        }
                        #endregion

                        //tách số lượng
                        var soluongcl_mt_cd = check_exit.gr_qty - (soluong);
                        //finish
                        check_exit1.use_yn = "N";
                        check_exit1.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        await _actualWOService.UpdateMaterialMappingMMS(check_exit1);

                        // chuyển trạng thái về 005 và update số lượng đã sử dụng
                        check_exit.status = "004";
                        check_exit.gr_qty = soluongcl_mt_cd;
                        check_exit.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        await _actualWOService.UpdateMaterialInfoMMS(check_exit);

                        //tạo mã mới cho số lượng còn dư và có trạng thái là return
                        //mt_sts_cd=004,sts_update='return',soluong=input,orgin=mt_cd
                        int count_return = await _actualWOService.CheckExitReturn(mt_cd);
                        await _actualWOService.InsertMaterialMappingReturn(count_return + 1, soluong, 0, "004", check_exit.chg_id, mt_cd);

                        if (check_exit.bb_no != "" && check_exit.bb_no != null)
                        {
                            //xã bobin
                            var check_bb = await _actualWOService.GetBobbinInfoReturn(check_exit.bb_no, check_exit.material_code);
                            if (check_bb != null)
                            {
                                check_bb.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                check_bb.mt_cd = mt_cd + "-" + "RT" + (count_return + 1);
                                await _actualWOService.UpdateBobbinMTCode(check_bb);

                                //find history để xóa
                                var history = await _actualWOService.GetBobbinLctHistory(check_bb.bb_no, mt_cd);
                                if (history != null)
                                {
                                    history.mt_cd = check_bb.mt_cd;
                                    await _actualWOService.UpdateBobbinLctHist(history);
                                }
                            }
                        }

                        //kiem tra co bao nhieu mt_cd da qua cong doan ma chua finish
                        await _actualWOService.UpdateMaterialMappingForDivide(check_exit.material_code, "Y", "N", Session["userid"] == null ? null : Session["userid"].ToString());

                        var data = await _actualWOService.GetDataSaveReturn(check_exit1.wmmId);

                        bool checkc = false;
                        if (data != null)
                            checkc = true;
                        return Json(new { data = data, result = checkc, message = "Thành Công!" }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                var kq = new List<SaveReturn>();
                return Json(new { data = kq, result = false, message = "Xin vui lòng đăng nhập!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> Cancel_mapping(int wmmid)
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
                try
                {
                    var check_exits = await _actualWOService.GetMaterialMappingById(wmmid);
                    var checkMaterialMappingOfBobbin = await _actualWOService.CheckMaterialMappingOfBobbbin(check_exits.mt_cd);

                    if (checkMaterialMappingOfBobbin == 0)
                    {
                        return Json(new { result = false, message = "Mã này không tìm thấy!!" }, JsonRequestBehavior.AllowGet);
                    }
                    if (checkMaterialMappingOfBobbin == 1)
                    {
                        #region CHECK BOBIN
                        //Nếu đầu ra là bb là cái cũ nhất thì khong cho mapping
                        //lấy được giá trị nếu là true thì return(tức db bằng rông or null )
                        var rs = await _actualWOService.CheckMaterialLotNG_OK(check_exits.mt_lot);
                        int id_actual = rs.id_actual;
                        var IsMaxBobbin = await _actualWOService.CheckMaxBobbin(check_exits.mt_lot, id_actual);
                        if (IsMaxBobbin == "" || IsMaxBobbin == null)
                        {
                            return Json(new { result = false, message = "NVL này đang được kế thừa nên không thể hủy bỏ." }, JsonRequestBehavior.AllowGet);
                        }
                        #endregion

                        var find_mt_cd = await _actualWOService.GetDataInventoryProduct(check_exits.mt_cd);
                        if (find_mt_cd == null)
                        {
                            return Json(new { result = false, message = "Mã này không tìm thấy!!" }, JsonRequestBehavior.AllowGet);
                        }

                        if (find_mt_cd.status.Equals("005"))
                        {
                            return Json(new { result = false, message = "Mã này đã kết thúc rồi, bạn không thể hủy nó!!" }, JsonRequestBehavior.AllowGet);
                        }

                        int check_exits_mapping = await _actualWOService.GetCountMaterialMapping(check_exits.mt_cd, check_exits.mt_lot);
                        if (check_exits_mapping > 0)
                        {
                            return Json(new { result = false, message = "Mã này là mã kế thừa không được xóa!!! " }, JsonRequestBehavior.AllowGet);
                        }

                        var checkMaterialMapping = await _actualWOService.CheckMaterialMapping(check_exits.mt_lot);
                        if (checkMaterialMapping > 0)
                        {
                            return Json(new { result = false, message = "Mã này đã được kết thúc hoặc đã qua công đoạn khác !" }, JsonRequestBehavior.AllowGet);
                        }


                        int check_exits_return = await _actualWOService.CheckExitReturn(check_exits.mt_cd);
                        if (check_exits_return > 0)
                        {
                            return Json(new { result = false, message = "Mã này đã được bắt đầu sản xuất!!" }, JsonRequestBehavior.AllowGet);
                        }


                        //kiêm tra hết ca chưa
                        if (find_mt_cd.mt_type != "CMT")
                        {
                            var find_lot = await _actualWOService.GetDataInventoryProduct(check_exits.mt_cd);
                            var listStaffId = await _actualWOService.GetStaffFromActualByInventoryProduct(find_lot.id_actual);
                            int data = await _actualWOService.CheckShiftOfStaff(find_lot.id_actual, listStaffId);
                            if (data == 0)
                            {
                                return Json(new { result = false, message = ec }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        //tra về trạng thái cũ

                        //kiểm trả về trạng thái cho nguyên vật liệu
                        var sts = "002";
                        var checkMaterialMapping2 = await _actualWOService.CheckMaterialMapping(find_mt_cd.material_code);
                        if (checkMaterialMapping2 > 0 && find_mt_cd.mt_type != "CMT")
                        {
                            sts = "001";
                        }
                        //neu nhu la nguyen vat lieu thi xoa id_actual

                        find_mt_cd.id_actual = find_mt_cd.mt_type != "CMT" ? 0 : find_mt_cd.id_actual;
                        find_mt_cd.status = sts;
                        find_mt_cd.change_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        await _actualWOService.UpdateInventoryProduct(find_mt_cd);
                        //trả lại trạng thái cũ và xóa khỏi bảng mapping
                        await _actualWOService.DeleteMaterialMapping(check_exits.wmmId);
                        return Json(new { result = true, message = "Trở về thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    if (checkMaterialMappingOfBobbin == 2)
                    {
                        #region CHECK BOBIN
                        //Nếu đầu ra là bb là cái cũ nhất thì khong cho mapping
                        //lấy được giá trị nếu là true thì return(tức db bằng rông or null )
                        var rs = await _actualWOService.CheckMaterialLotNG_OK(check_exits.mt_lot);
                        int id_actual = rs.id_actual;
                        var IsMaxBobbin = await _actualWOService.CheckMaxBobbin(check_exits.mt_lot, id_actual);
                        if (IsMaxBobbin == "" || IsMaxBobbin == null)
                        {
                            return Json(new { result = false, message = "NVL này đang được kế thừa nên không thể hủy bỏ." }, JsonRequestBehavior.AllowGet);
                        }
                        #endregion

                        var find_mt_cd = await _actualWOService.GetWMaterialInfoByMaterialCode(check_exits.mt_cd);
                        if (find_mt_cd == null)
                        {
                            return Json(new { result = false, message = "Mã này không tìm thấy!!" }, JsonRequestBehavior.AllowGet);
                        }

                        if (find_mt_cd.status.Equals("005"))
                        {
                            return Json(new { result = false, message = "Mã này đã kết thúc rồi, bạn không thể hủy nó!!" }, JsonRequestBehavior.AllowGet);
                        }

                        int check_exits_mapping = await _actualWOService.GetCountMaterialMapping(check_exits.mt_cd, check_exits.mt_lot);
                        if (check_exits_mapping > 0)
                        {
                            return Json(new { result = false, message = "Mã này đang được sử dụng!!! " }, JsonRequestBehavior.AllowGet);
                        }

                        int check_exits_return = await _actualWOService.CheckExitReturn(check_exits.mt_cd);
                        if (check_exits_return > 0)
                        {
                            return Json(new { result = false, message = "Mã này đã được bắt đầu sản xuất!!" }, JsonRequestBehavior.AllowGet);
                        }

                        var checkMaterialMapping = await _actualWOService.CheckMaterialMapping(check_exits.mt_lot);
                        if (checkMaterialMapping > 0)
                        {
                            return Json(new { result = false, message = "Mã này đã được kết thúc hoặc đã qua công đoạn khác!!!" }, JsonRequestBehavior.AllowGet);
                        }

                        //kiêm tra hết ca chưa
                        if (find_mt_cd.material_type != "CMT")
                        {
                            var find_lot = await _actualWOService.GetWMaterialInfoByMaterialCode(check_exits.mt_lot);
                            var staff_id = await _actualWOService.GetListStaffFromActual(find_lot.id_actual);
                            int data = await _actualWOService.CheckShiftOfStaff(find_lot.id_actual, staff_id);
                            if (data == 0)
                            {
                                return Json(new { result = false, message = ec }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        //tra về trạng thái cũ

                        //kiểm trả về trạng thái cho nguyên vật liệu
                        var sts = "002";
                        var checkMaterialMapping2 = await _actualWOService.CheckMaterialMapping(find_mt_cd.material_code);
                        if (checkMaterialMapping2 > 0 && find_mt_cd.material_type != "CMT")
                        {
                            sts = "001";
                        }
                        //neu nhu la nguyen vat lieu thi xoa id_actual
                        find_mt_cd.id_actual = find_mt_cd.material_type != "CMT" ? 0 : find_mt_cd.id_actual;


                        find_mt_cd.status = sts;
                        find_mt_cd.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        await _actualWOService.UpdateMaterialInfoMMS(find_mt_cd);

                        //trả lại trạng thái cũ và xóa khỏi bảng mapping
                        await _actualWOService.DeleteMaterialMapping(check_exits.wmmId);
                        return Json(new { result = true, message = "Trở về thành công" }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { result = false, message =  Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }

        }


        #endregion table2

        #endregion mapping

        #region Device

        [HttpGet]
        public async Task<ActionResult> getmt_date_web_auto(int id_actual)
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
                var data = await _actualWOService.GetMaterialToDevice(id_actual);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" });
            }

        }

        [HttpGet]
        public async Task<JsonResult> Decevice_sta(string mt_cd, string number_dv)
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
                try
                {
                    var find_mtcd = await _actualWOService.GetMaterialInfoOfDevice(mt_cd, "002");

                    if (find_mtcd == null)
                    {
                        return Json(new { result = false, message = "Mã này không tìm thấy hoặc mã này không đủ điều kiện để chia ra !!" }, JsonRequestBehavior.AllowGet);
                    }
                    if (String.IsNullOrEmpty(number_dv))
                    {
                        return Json(new { result = false, message = "Số lượng chia rỗng!!!" }, JsonRequestBehavior.AllowGet);
                    }

                    int data1 = await _actualWOService.GetUnitStaffforDevice(find_mtcd.id_actual);
                    if (data1 == 0)
                    {
                        return Json(new { result = false, message = ec }, JsonRequestBehavior.AllowGet);
                    }
                    // INSert DATA
                    double rs = Convert.ToDouble(find_mtcd.gr_qty) / Convert.ToDouble(number_dv);


                    var sl_chia = number_dv;

                    find_mtcd.reg_date = DateTime.Now;
                    find_mtcd.status = "005";
                    find_mtcd.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    find_mtcd.chg_date = DateTime.Now;
                    find_mtcd.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    //find_mtcd.material_code = mt_cd;
                    find_mtcd.number_divide = Convert.ToInt32(sl_chia);
                    find_mtcd.gr_qty = 0;
                    // update material_infor_mms
                    await _actualWOService.UpdateMaterialInfoMMS(find_mtcd);

                    await _actualWOService.UpdatedBobbinInfoForDevice(find_mtcd.bb_no, find_mtcd.material_code, Session["userid"] == null ? null : Session["userid"].ToString());
                    await _actualWOService.DeleteDBobbinLctHistForDevice(find_mtcd.bb_no, find_mtcd.material_code);


                    // }

                    return Json(new { result = true, message = "Thành công!!!", kq = find_mtcd }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var kq = new MaterialInfoMMS();
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !", data = kq }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<JsonResult> DestroyBobbinMgt(string bobin, string mt_cd)
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
                var check_mt_cd = await _actualWOService.GetWMaterialInfoByMaterialCode(mt_cd);
                if (check_mt_cd.material_code != null)
                {
                    var kho = (check_mt_cd.location_code == "002000000000000000" ? "MMS" : "TIMS");

                    var bobin_primary = await _actualWOService.GetBobbinInfoReturn(bobin, mt_cd);
                    var bobin_history = await _actualWOService.GetBobbinLctHistory(bobin, mt_cd);
                    var mtCode = await _actualWOService.GetMaterial(bobin, mt_cd);

                    if (mtCode != null)
                    {
                        mtCode.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        mtCode.status = "005";
                        await _actualWOService.UpdateMaterialInfoMMS(mtCode);
                    }
                    if (bobin_primary != null)
                    {
                        bobin_primary.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        bobin_primary.mt_cd = null;
                        await _actualWOService.UpdateBobbinInfo(bobin_primary);

                    }
                    if (bobin_history != null)
                    {
                        await _actualWOService.DeleteBobbinLctHist(bobin_history.blno);
                    }
                    else
                    {
                        return Json(new { result = false, message = "Không tìm thấy " + bobin_history.bb_nm + " này trong hệ thống !" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { result = true, message = "Thành công!!!" }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { result = true, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetListMappingStaTims(string mt_cd)
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
                var find_mtcd = new MaterialInfoMMS();
                int numberdevide = 0;
                int merge = 0;
                bool tims = true;
                IReadOnlyList<MaterialInfoDivideMMS> mttmerge = new List<MaterialInfoDivideMMS>();
                if (mt_cd.Contains("STA") || mt_cd.Contains("ROT"))
                {
                    tims = false;
                    find_mtcd = await _actualWOService.GetWMaterialInfoByMaterialCode(mt_cd);

                    mttmerge = await _ITIMSService.FindAllMaterialByMtCdMGMMSLike(mt_cd);
                    //numberdevide = await _actualWOService.GetTotalDevideWMaterialInfoByMaterialCode(mt_cd + "-DV");
                    //merge = await _actualWOService.GetTotalWMaterialInfoByMaterialCodeMGMMS(mt_cd + "-DV");
                    //numberdevide += await _actualWOService.GetTotalDevideWMaterialInfoByMaterialCode(mt_cd + "-MG");
                }
                else
                {
                    find_mtcd = await _actualWOService.GetWMaterialInfoByMaterialCodeTIMS(mt_cd);
                    mttmerge = await _ITIMSService.FindAllMaterialByMtCdMGLike(mt_cd);
                    //numberdevide = await _actualWOService.GetTotalWMaterialInfoByMaterialCodeTIMS(mt_cd+"-DV");
                    //merge = await _actualWOService.GetTotalWMaterialInfoByMaterialCodeMGTIMS(mt_cd+"-DV");
                    //numberdevide += await _actualWOService.GetTotalWMaterialInfoByMaterialCodeTIMS(mt_cd+"-MG");
                }
                numberdevide = mttmerge.Count();
                if (find_mtcd == null)
                {
                    listData = new List<MaterialInfoDivideMMS>();
                    return Json(listData, JsonRequestBehavior.AllowGet);
                }

                if (Convert.ToDouble(find_mtcd.number_divide) == 0 && numberdevide == 0)
                {
                    listData = new List<MaterialInfoDivideMMS>();
                    return Json(listData, JsonRequestBehavior.AllowGet);
                }

                double rs = Convert.ToDouble(find_mtcd.real_qty) / Convert.ToDouble(find_mtcd.number_divide);
                var so_luong = find_mtcd.number_divide == 0 ? mttmerge.Where(x => !x.material_code.Contains("-MG")).Count() : Math.Ceiling(rs);
                int grpqtydevide = find_mtcd.number_divide;
                int totalqty = find_mtcd.real_qty;
                int ttqtydevied = 0;
                for (int i = 0; i < so_luong; i++)
                {
                    int wmtiid = i + 1;
                    int wmtiiid = 0;
                    int realqty = 0;
                    int grpqty = 0;
                    string bbno = "";
                    string mtcd = mt_cd + "-DV" + (i + 1);
                    int qtydevide = 0;
                    var MaterialInfo = new MaterialInfoMMS();
                    if (grpqtydevide > totalqty)
                    {
                        qtydevide = totalqty;

                    }
                    //else if ((ttqtydevied+ grpqtydevide) > totalqty)
                    //{
                    //    qtydevide = totalqty - ttqtydevied;
                    //}
                    else
                    {
                        qtydevide = grpqtydevide;
                        //ttqtydevied += grpqtydevide;
                    }
                    var aa = mttmerge.Where(x => x.material_code == mtcd).FirstOrDefault();
                    if (aa != null)
                    {
                        totalqty = totalqty - aa.gr_qty;
                        //bbno = aa.bb_no;
                        //wmtiiid = aa.wmtid;
                        //wmtiid = aa.wmtid;
                        //realqty = aa.real_qty;
                        //grpqty = aa.gr_qty;
                        listData.Add(aa);
                    }
                    else
                    {
                        grpqty = qtydevide;
                        totalqty = totalqty - grpqtydevide;
                        var result = new MaterialInfoDivideMMS()
                        {
                            wmtid = wmtiid,
                            material_code_parent = find_mtcd.material_code,
                            material_code = mtcd,
                            id_actual = find_mtcd.id_actual,
                            real_qty = realqty,
                            mt_cd = mtcd,
                            gr_qty = grpqty,
                            status = find_mtcd.status,
                            mt_no = find_mtcd.mt_no,
                            bb_no = bbno,
                            chg_id = find_mtcd.chg_id,
                            chg_date = find_mtcd.chg_date,
                            reg_id = find_mtcd.reg_id,
                            reg_date = find_mtcd.reg_date
                        };


                        listData.Add(result);
                    }
                    //if (!tims)
                    //{
                    //    var aa = await _actualWOService.CheckMaterialMappingContainer(mtcd);
                    //    MaterialInfo = aa.FirstOrDefault();
                    //    if (MaterialInfo != null)
                    //    {
                    //        bbno = MaterialInfo.bb_no;
                    //        wmtiiid = MaterialInfo.wmtid;
                    //        wmtiid = MaterialInfo.wmtid;
                    //        realqty = MaterialInfo.real_qty;
                    //        grpqty = MaterialInfo.gr_qty;
                    //    }
                    //    else
                    //    {
                    //        grpqty = qtydevide;
                    //    }
                    //    //bbno = await _actualWOService.GetBBNOWMaterialInfoByMaterialCodeMMS(mtcd);
                    //    //wmtiiid = await _actualWOService.GetWMTIDWMaterialInfoByMaterialCodeMMS(mtcd);

                    //}
                    //else
                    //{
                    //    MaterialInfo = await _actualWOService.GetWMaterialInfoByMaterialCodeTIMS(mtcd);
                    //    if(MaterialInfo != null)
                    //    {
                    //        bbno = MaterialInfo.bb_no;
                    //        wmtiiid = MaterialInfo.wmtid;
                    //        wmtiid = MaterialInfo.wmtid;
                    //        realqty = MaterialInfo.real_qty;
                    //        grpqty = MaterialInfo.gr_qty;
                    //    }
                    //    else{
                    //        grpqty = qtydevide;
                    //    }
                    //    //string bbnooo = await _actualWOService.GetBBNOWMaterialInfoByMaterialCodeTIMS(mtcd);
                    //    //int wmtiiidd = await _actualWOService.GetWMTIDWMaterialInfoByMaterialCodeTIMS(mtcd);

                    //}
                    //if (MaterialInfo != null)
                    //{
                    //    wmtiid = MaterialInfo.wmtid;
                    //    realqty = MaterialInfo.real_qty;
                    //    grpqty = MaterialInfo.gr_qty;
                    //}
                    //else
                    //{
                    //    grpqty = qtydevide;
                    //    //realqty = qtydevide;
                    //}

                    //count++;
                }
                listData.AddRange(mttmerge.Where(x => x.material_code.Contains("MG")));

                return Json(listData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }




        }

        [HttpGet]
        public async Task<JsonResult> destroyDevide(string mt_cd)
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
                //Lấy ra thông tin của cái rổ đang chứa nguyên vật liệu cha
                var check_mt_cd = await _actualWOService.GetWMaterialInfoByMaterialCode(mt_cd);
                if (check_mt_cd == null)
                {
                    return Json(new { result = false, message = Constant.NotFound }, JsonRequestBehavior.AllowGet);
                }
                //Kiểm tra xem mã này có trạng thái = 005 chưa và không tồn tại đồ đựng này trong history nhé
                if (check_mt_cd.status != "005")
                {
                    return Json(new { result = false, message = Constant.Status }, JsonRequestBehavior.AllowGet);
                }

                var check_bobbin_info = await _actualWOService.GetBobbinInfo(check_mt_cd.bb_no);
                var check_bobbin_history = await _actualWOService.GetBobbinLctHist(check_mt_cd.bb_no);
                if (check_bobbin_info == null)
                {
                    return Json(new { result = false, message = "Không tìm thấy đồ đựng này" }, JsonRequestBehavior.AllowGet);
                }
                if (check_bobbin_info.mt_cd != null && check_bobbin_history != null)
                {
                    return Json(new { result = false, message = "Bobbin " + check_mt_cd.bb_no + " này đang được mapping với NVL rồi không thể Redo lại được!" }, JsonRequestBehavior.AllowGet);
                }

                //lấy ra danh sách các nguyên vật liệu con đã được add vào container
                var listChildMaterialInfoMMS = await _actualWOService.CheckMaterialMappingContainer1(mt_cd);

                foreach (var item in listChildMaterialInfoMMS)
                {
                    //Xóa lịch sử của các bobbin chứa nguyên vật liệu con
                    await _actualWOService.DeleteDBobbinLctHistForDevice(item.bb_no, item.material_code);

                    //xã bobin đang chứa các nguyên vật liệu con
                    var check_bb = await _actualWOService.GetBobbinInfo(item.bb_no);
                    if (check_bb != null)
                    {
                        check_bb.mt_cd = "";
                        check_bb.chg_dt = DateTime.Now;
                        check_bb.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        await _actualWOService.UpdateBobbinMTCode(check_bb);
                    }

                    // xóa các thông tin rổ đang chứa các NVL con ở trong bảng MMS
                    await _actualWOService.DeleteMaterialInfoMMs(item.wmtid);
                }

                // xóa các thông tin các NVL con ở trong bảng Mapping_MMS
                await _actualWOService.DeleteMaterialMappingMMSByCode(mt_cd);

                check_mt_cd.status = "002";
                check_mt_cd.gr_qty = check_mt_cd.real_qty;
                check_mt_cd.number_divide = 0;
                await _actualWOService.UpdateMaterialInfoMMS(check_mt_cd);
                await GetListMappingStaMMS(mt_cd);
                await _actualWOService.UpdatemtcdforBobbinInfoForRedo(check_mt_cd.bb_no, check_mt_cd.material_code);

                var bbinfo = await _actualWOService.FindOneDBobbinInfo(check_mt_cd.bb_no);
                DBobbinLctHist d_bobbin_lct_hist = new DBobbinLctHist();
                d_bobbin_lct_hist.bb_no = check_mt_cd.bb_no;
                d_bobbin_lct_hist.bb_nm = bbinfo.bb_nm;
                d_bobbin_lct_hist.mc_type = bbinfo.mc_type;
                d_bobbin_lct_hist.mt_cd = check_mt_cd.material_code;
                d_bobbin_lct_hist.use_yn = "Y";
                d_bobbin_lct_hist.del_yn = "N";
                d_bobbin_lct_hist.reg_dt = DateTime.Now;
                d_bobbin_lct_hist.chg_dt = DateTime.Now;
                d_bobbin_lct_hist.reg_id = Session["userid"] == null ? "Mob" : Session["userid"].ToString(); ;
                d_bobbin_lct_hist.chg_id = Session["userid"] == null ? "Mob" : Session["userid"].ToString(); ;
                await _actualWOService.InsertBobbinHist(d_bobbin_lct_hist);
                return Json(new { result = true, message = Constant.Success, gr_qty = check_mt_cd.gr_qty, wmtid = check_mt_cd.wmtid }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !", gr_qty = 0, wmtid = 0 }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public async Task<JsonResult> Changebb_dv(ChangeBobbinForMaterialChildrenResponse item)
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
                try
                {
                    string[] arrListStr = item.material_code.Split(new string[] { "-DV" }, StringSplitOptions.None);
                    var parentCode = arrListStr[0];

                    var check_bb_new = await _actualWOService.GetBobbinInfoChangebbdv(item.bb_no);
                    if (check_bb_new == null)
                    {
                        return Json(new { result = false, message = "Không tìm thấy đồ đựng này" }, JsonRequestBehavior.AllowGet);
                    }

                    //Kiểm tra xem đồ đựng đó đã được sử dụng hay chưa
                    //Nếu đồ đựng đã đc đem đi sử dụng ở chỗ khác rồi thì thông báo
                    // Còn nếu đồ đựng đó đang trống thì add NVL vào đồ đựng đó
                    if (check_bb_new.mt_cd == null)
                    {
                        check_bb_new.mt_cd = "";
                    }
                    if (check_bb_new.mt_cd != "")
                    {
                        return Json(new { result = false, message = "Đồ đựng đã được sử dụng" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var listBoobbin = await _actualWOService.GetListBobbinInfoData(item.material_code);
                        if (listBoobbin.Count() < 1)
                        {
                            //add bobbbin mới vào
                            check_bb_new.mt_cd = item.material_code;
                            check_bb_new.chg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                            await _actualWOService.UpdateBobbinMTCode(check_bb_new);
                        }
                        else
                        {
                            foreach (var x in listBoobbin)
                            {
                                if (x.bb_no != item.bb_no)
                                {
                                    //xã bobbin cũ đi
                                    await _actualWOService.DeleteDBobbinLctHistForDevice(x.bb_no, x.mt_cd);
                                    x.mt_cd = "";
                                    x.chg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                                    await _actualWOService.UpdateBobbinMTCode(x);


                                    //add bobbbin mới vào
                                    check_bb_new.mt_cd = item.material_code;
                                    check_bb_new.chg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                                    await _actualWOService.UpdateBobbinMTCode(check_bb_new);
                                }
                            }
                        }
                    }

                    //Kiểm tra lịch sử đồ đựng có hay chưa
                    //Nếu có rùi thì update
                    //Nếu chưa có thì add NVL vào
                    var check_bb_histoty = await _actualWOService.GetBobbinLctHistory(item.bb_no, item.material_code);
                    if (check_bb_histoty != null)
                    {
                        //change bobin in history bobin
                        check_bb_histoty.bb_no = item.bb_no;
                        check_bb_histoty.chg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                        await _actualWOService.UpdateBobbinLctHist(check_bb_histoty);
                    }
                    else
                    {
                        //add vào bb_history
                        var history = new BobbinLctHist();
                        history.bb_no = item.bb_no;
                        history.bb_nm = check_bb_new.bb_nm;
                        history.mc_type = check_bb_new.mc_type;
                        history.mt_cd = item.material_code;
                        history.use_yn = "Y";
                        history.del_yn = "N";
                        history.chg_dt = DateTime.Now;
                        history.chg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                        history.reg_dt = DateTime.Now;
                        history.reg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                        await _actualWOService.InsertToBobbinLctHistory(history);
                    }


                    var check = await _actualWOService.CheckMaterialLotNG_OK(item.material_code);
                    if (check == null)
                    {
                        int wmtid = await _actualWOService.InsertMaterialInfoMMSByModel(item);
                        check = await _actualWOService.GetDetailMaterialInfoMMS(wmtid);
                    }

                    var parentItem = await _actualWOService.GetWMaterialInfoByMaterialCode(parentCode);

                    //Kiểm tra material_code_divide
                    if (check == null)
                    {
                        return Json(new { result = false, message = "Không tìm thấy mã lot này" }, JsonRequestBehavior.AllowGet);
                    }

                    var checkMaterialMapping = await _actualWOService.CheckMaterialMapping(check.material_code);
                    if (checkMaterialMapping > 0)
                    {
                        return Json(new { result = false, message = "Mã lot này đã được chuyển qua công đoạn khác" }, JsonRequestBehavior.AllowGet);
                    }

                    if (check.status == "009" || check.status == "010")
                    {
                        return Json(new { result = false, message = "Mã lot này đã được chuyển qua công đoạn OQC" }, JsonRequestBehavior.AllowGet);
                    }

                    //đã kiểm tra chưa
                    var checkMaterialWithNG = await _actualWOService.GetTotalMaterialInfoDV(check.material_code + "-NG");
                    if (checkMaterialWithNG > 0)
                    {
                        return Json(new { result = false, message = "Mã lot này đã được chuyển qua công đoạn OQC" }, JsonRequestBehavior.AllowGet);
                    }

                    //Kiểm tra đã check bất cứ TQC OQC ANY Chưa
                    var a = await _actualWOService.Checkwmfaclineqc(check.material_code);
                    var b = await _actualWOService.Gettwproductqc(check.material_code);
                    if (a > 0 || b > 0)
                    {
                        return Json(new { result = false, message = "Mã lot này đã được kiểm tra" }, JsonRequestBehavior.AllowGet);
                    }

                    //Updaqte lại thông tin cho Rổ đã chia ra rồi
                    var material = await _actualWOService.GetMaterialInfoOfDevice(check.material_code, "002");
                    material.id_actual = item.id_actual;
                    material.real_qty = item.gr_qty;
                    material.real_qty = item.gr_qty;
                    material.bb_no = item.bb_no;
                    material.status = "002";
                    material.location_code = "002002000000000000";
                    material.from_lct_code = "002002000000000000";
                    material.material_type = "CMT";
                    material.number_divide = parentItem.number_divide;
                    material.chg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                    material.reg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                    material.reg_date = DateTime.Now;
                    material.chg_date = DateTime.Now;
                    material.orgin_mt_cd = parentItem.material_code;
                    await _actualWOService.UpdateMaterialInfoMMS(material);


                    // thay đổi số lượng(gr_qty) trên w_material_info_mms sau khi chia NVL vào Bobbin hoặc Tray mới
                    var materialParent = await _actualWOService.GetWMaterialInfoByMaterialCode(parentItem.material_code);
                    materialParent.status = "005";
                    materialParent.chg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                    materialParent.chg_date = DateTime.Now;
                    await _actualWOService.UpdateMaterialInfoMMS(materialParent);


                    var c = new MaterialMappingMMS();
                    c.mt_cd = item.material_code_parent;
                    c.mt_lot = item.material_code;
                    c.mt_no = item.mt_no;
                    c.mapping_dt = DateTime.Now;
                    c.use_yn = "Y";
                    c.chg_date = DateTime.Now;
                    c.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    c.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    c.reg_date = DateTime.Now;

                    int checkMaterialMappingMMS = await _actualWOService.CheckExistedMaterialMappingMMS(c.mt_lot, c.mt_cd);
                    if (checkMaterialMappingMMS > 0)
                    {
                        await _actualWOService.UpdateMaterialMappingMMS(c);
                    }
                    else
                    {
                        await _actualWOService.InsertMaterialMapping(c);
                    }

                    await GetListMappingStaMMS(parentCode);
                    return Json(new { result = true, message = "Thành công!!" }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception e)
                {
                    //throw e;
                    return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetListMappingStaMMS(string mt_cd)
        {
            try
            {
                var resqheader = Request.Headers;
                string mtcd_dv = string.Concat(mt_cd, "-DV");
                string[] resqheaderkey = resqheader.AllKeys;
                string[] resqheaderval = resqheader.GetValues("requestFrom");
                string resqheadervalmob = "";
                //    string mergeCodeList = "";
                if (resqheaderval != null)
                {
                    resqheadervalmob = resqheaderval[0];
                }
                if (Session["authorize"] != null || resqheadervalmob == "Mob")
                {
                    var find_mtcd = new MaterialInfoMMS();
                    int numberdevide = 0;
                    int merge = 0;
                //    bool tims = true;
                    if (mt_cd.Contains("STA") || mt_cd.Contains("ROT"))
                    {
                       // tims = false;
                        find_mtcd = await _actualWOService.GetWMaterialInfoByMaterialCode(mt_cd);
                        numberdevide = await _actualWOService.GetTotalDevideWMaterialInfoByMaterialCode(mt_cd + "-DV");
                     //   merge = await _actualWOService.GetTotalWMaterialInfoByMaterialCodeMGMMS(mt_cd + "-DV");
                        //numberdevide += await _actualWOService.GetTotalDevideWMaterialInfoByMaterialCode(mt_cd + "-MG");
                    }
                    else
                    {
                        find_mtcd = await _actualWOService.GetWMaterialInfoByMaterialCodeTIMS(mt_cd);
                        numberdevide = await _actualWOService.GetTotalWMaterialInfoByMaterialCodeTIMS(mt_cd + "-DV");
                        merge = await _actualWOService.GetTotalWMaterialInfoByMaterialCodeMGTIMS(mt_cd + "-DV");
                        //numberdevide += await _actualWOService.GetTotalWMaterialInfoByMaterialCodeTIMS(mt_cd+"-MG");
                    }

                    if (find_mtcd == null)
                    {
                        listData = new List<MaterialInfoDivideMMS>();
                        return Json(listData, JsonRequestBehavior.AllowGet);
                    }

                    if (Convert.ToDouble(find_mtcd.number_divide) == 0 && numberdevide == 0)
                    {
                        listData = new List<MaterialInfoDivideMMS>();
                        return Json(listData, JsonRequestBehavior.AllowGet);
                    }

                    double rs = Convert.ToDouble(find_mtcd.real_qty) / Convert.ToDouble(find_mtcd.number_divide);
                    var so_luong = find_mtcd.number_divide == 0 ? numberdevide : Math.Ceiling(rs);
                    int grpqtydevide = find_mtcd.number_divide;
                    int totalqty = find_mtcd.real_qty;
                    //bbno = await _actualWOService.GetBBNOWMaterialInfoByMaterialCodeMMS(mtcd);

                    //MaterialInfoMMS material = new MaterialInfoMMS();
                    //var material = await _actualWOService.GetWMaterialInfoByMaterialCd(mt_cd);
                   // var TIMS = await _actualWOService.GetTIMSInfoByMaterialCd(mt_cd);
                    var aa = await _actualWOService.CheckMaterialMappingContainer(mtcd_dv);
                    for (int i = 0; i < so_luong; i++)
                    {
                        int wmtiid = i + 1;
                        int wmtiiid = 0;
                        int realqty = 0;
                        int grpqty = 0;
                        string bbno = "";
                        string mtcd = mtcd_dv + (i + 1);
                        //string mtcd = String.Concat(mtcd_dv, (i + 1).ToString());
                        // mergeCodeList = string.Concat(mergeCodeList, ",", mtcd, "-MG");
                        int qtydevide = 0;
                        var MaterialInfo = new MaterialInfoMMS();
                        if (grpqtydevide > totalqty)
                        {
                            qtydevide = totalqty;
                        }
                        else
                        {
                            qtydevide = grpqtydevide;
                        }
                       // if (!tims)
                       // {
                            var check1 = aa.Where(x => x.material_code == mtcd);
                            // var aa = await _actualWOService.CheckMaterialMappingContainer(mtcd);
                            MaterialInfo = check1.FirstOrDefault();
                            if (MaterialInfo != null)
                            {
                                bbno = MaterialInfo.bb_no;
                                wmtiiid = MaterialInfo.wmtid;
                                wmtiid = MaterialInfo.wmtid;
                                realqty = MaterialInfo.real_qty;
                                grpqty = MaterialInfo.gr_qty;
                            }
                            else
                            {
                                grpqty = qtydevide;
                            }
                            //bbno = await _actualWOService.GetBBNOWMaterialInfoByMaterialCodeMMS(mtcd);
                            //wmtiiid = await _actualWOService.GetWMTIDWMaterialInfoByMaterialCodeMMS(mtcd);

                       // }
                      //  else
                     //   {
                            //MaterialInfo = await _actualWOService.GetWMaterialInfoByMaterialCodeTIMS(mtcd);
                            //if (MaterialInfo != null)
                            //{
                            //    bbno = MaterialInfo.bb_no;
                            //    wmtiiid = MaterialInfo.wmtid;
                            //    wmtiid = MaterialInfo.wmtid;
                            //    realqty = MaterialInfo.real_qty;
                            //    grpqty = MaterialInfo.gr_qty;
                            //}
                            //else
                            //{
                            //    grpqty = qtydevide;
                            //}
                            //string bbnooo = await _actualWOService.GetBBNOWMaterialInfoByMaterialCodeTIMS(mtcd);
                            //int wmtiiidd = await _actualWOService.GetWMTIDWMaterialInfoByMaterialCodeTIMS(mtcd);

                      //  }
                        //if (MaterialInfo != null)
                        //{
                        //    wmtiid = MaterialInfo.wmtid;
                        //    realqty = MaterialInfo.real_qty;
                        //    grpqty = MaterialInfo.gr_qty;
                        //}
                        //else
                        //{
                        //    grpqty = qtydevide;
                        //    //realqty = qtydevide;
                        //}
                        totalqty = totalqty - grpqtydevide;
                        var result = new MaterialInfoDivideMMS()
                        {
                            wmtid = wmtiid,
                            material_code_parent = find_mtcd.material_code,
                            material_code = mtcd,
                            id_actual = find_mtcd.id_actual,
                            real_qty = realqty,
                            mt_cd = mtcd,
                            gr_qty = grpqty,
                            status = find_mtcd.status,
                            mt_no = find_mtcd.mt_no,
                            bb_no = bbno,
                            chg_id = find_mtcd.chg_id,
                            chg_date = find_mtcd.chg_date,
                            reg_id = find_mtcd.reg_id,
                            reg_date = find_mtcd.reg_date
                        };

                        //var mttmerge = await _ITIMSService.FindAllMaterialByMtCdLike(mtcd + "-MG");
                        //if (mttmerge.Count() > 0)
                        //{
                        //    foreach (var itm in mttmerge)
                        //    {
                        //        var resultmg = new MaterialInfoDivideMMS()
                        //        {
                        //            wmtid = itm.wmtid,
                        //            material_code_parent = find_mtcd.material_code,
                        //            material_code = itm.material_code,
                        //            id_actual = itm.id_actual,
                        //            real_qty = itm.real_qty,
                        //            mt_cd = itm.material_code,
                        //            gr_qty = itm.gr_qty,
                        //            status = itm.status,
                        //            mt_no = itm.mt_no,
                        //            bb_no = itm.bb_no,
                        //            chg_id = itm.chg_id,
                        //            chg_date = itm.chg_date,
                        //            reg_id = itm.reg_id,
                        //            reg_date = itm.reg_date
                        //        };
                        //        listData.Add(resultmg);
                        //    }
                        //}

                        listData.Add(result);
                        //count++;
                    }
                    //if (mergeCodeList.Length > 0)
                    //{
                    //    mergeCodeList = mergeCodeList.Substring(1);
                    //}
                    //var mttmerge = await _ITIMSService.GetListMappingStaTims_FindMerge(mergeCodeList);
                    //if (mttmerge.Count() > 0)
                    //{
                    //    foreach (var itm in mttmerge)
                    //    {
                    //        var resultmg = new MaterialInfoDivideMMS()
                    //        {
                    //            wmtid = itm.wmtid,
                    //            material_code_parent = find_mtcd.material_code,
                    //            material_code = itm.material_code,
                    //            id_actual = itm.id_actual,
                    //            real_qty = itm.real_qty,
                    //            mt_cd = itm.material_code,
                    //            gr_qty = itm.gr_qty,
                    //            status = itm.status,
                    //            mt_no = itm.mt_no,
                    //            bb_no = itm.bb_no,
                    //            chg_id = itm.chg_id,
                    //            chg_date = itm.chg_date,
                    //            reg_id = itm.reg_id,
                    //            reg_date = itm.reg_date
                    //        };
                    //        listData.Add(resultmg);
                    //    }
                    //}

                    return Json(listData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        #endregion Device

        #region SỬ DỤNG CHUNG
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

        #endregion SỬ DỤNG CHUNG

        #region Print NG

        public ActionResult Print_NG()
        {
            return SetLanguage("~/Views/ActualWO/PrintNG/PrintNG.cshtml");
        }

        [HttpGet]
        public async Task<JsonResult> GetMaterialNG([FromBody] PrintNGRequest req)
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
                var data = await _actualWOService.GetListNG(req.code, req.at_no, req.product);
                int start = (req.page - 1) * req.rows;
                int totals = data.Count();
                int totalPages = (int)Math.Ceiling((float)totals / req.rows);
                var dataactual = data.Skip<NGResponse>(start).Take(req.rows);
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
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult PrintMaterialNG()
        {
            string[] keys = Request.Form.AllKeys;
            StringBuilder listValues = new StringBuilder();
            for (int i = 0; i < keys.Length; i++)
            {
                listValues.Append(Request.Form[keys[i]] + ",");
            }
            ViewData["Message"] = listValues.ToString();
            return View("~/Views/ActualWO/PrintNG/QRPrintNG.cshtml");
        }

        [HttpGet]
        public async Task<JsonResult> Change_OK_NG(string mt_cd, int gr_qty, string reason)
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
                try
                {
                    var find_ng = await _actualWOService.CheckMaterialNG_OK(mt_cd);
                    if (find_ng == null)
                    {
                        return Json(new { result = false, message = "Không tìm thấy mã này" }, JsonRequestBehavior.AllowGet);
                    }
                    if (find_ng.gr_qty < gr_qty)
                    {
                        return Json(new { result = false, message = "Số lượng này lớn hơn số lượng tồn tại!!" }, JsonRequestBehavior.AllowGet);
                    }
                    //trừ mã cũ

                    //insert bảng giảm có lí do

                    var w_material_down = new MaterialDown();
                    w_material_down.gr_down = find_ng.gr_qty - gr_qty;
                    w_material_down.mt_cd = find_ng.material_code;
                    w_material_down.reason = reason;
                    w_material_down.bb_no = find_ng.bb_no;
                    w_material_down.status_now = find_ng.status;
                    w_material_down.gr_qty = Convert.ToDouble(find_ng.gr_qty);
                    w_material_down.chg_dt = DateTime.Now.ToString();
                    w_material_down.reg_dt = DateTime.Now.ToString();
                    w_material_down.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    w_material_down.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();

                    await _actualWOService.InsertMaterialDown(w_material_down);

                    find_ng.gr_qty = find_ng.gr_qty - gr_qty;
                    find_ng.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();

                    if (ModelState.IsValid)
                    {
                        //db.Entry(find_ng).State = EntityState.Modified;
                        //db.SaveChanges();
                        await _actualWOService.UpdateMaterialInfoMMS(find_ng);

                        //tạo mã ok mới dựa trên mã ng
                        var check = await _actualWOService.CheckMaterialOKInfoMMS(find_ng.material_code);
                        var dem = (check.Count() > 0 ? check.Count().ToString() : "");
                        var ok = new MaterialInfoMMS();
                        ok.id_actual = find_ng.id_actual;
                        ok.orgin_mt_cd = find_ng.material_code;
                        ok.gr_qty = gr_qty;
                        ok.real_qty = ok.gr_qty;
                        ok.material_code = find_ng.material_code + "-OK" + dem;
                        //ok.mt_barcode = ok.mt_cd;
                        //ok.mt_qrcode = ok.mt_cd;
                        ok.location_code = find_ng.location_code;
                        ok.material_type = find_ng.material_type;
                        ok.mt_no = find_ng.mt_no;
                        ok.status = "012";
                        //ok.use_yn = "Y";
                        ok.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        ok.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        ok.chg_date = DateTime.Now;
                        ok.reg_date = DateTime.Now;
                        await _actualWOService.InsertMaterialInfoMMS(ok);

                        return Json(new { result = true, kq = ok, message = "Thành Công !" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception e)
                {
                    return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var kq = new MaterialInfoMMS();
                return Json(new { result = false, data = kq, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<JsonResult> Cancel_NG_OK(string mt_cd)
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
                try
                {
                    //check mã tồn tại không
                    var find_lot = await _actualWOService.CheckMaterialLotNG_OK(mt_cd);
                    if (find_lot == null)
                    {
                        return Json(new { result = false, message = "Không tìm thấy mã này" }, JsonRequestBehavior.AllowGet);
                    }
                    if (find_lot.status != "012")
                    {
                        return Json(new { result = false, message = "Trạng thái đã thay đổi!!" }, JsonRequestBehavior.AllowGet);
                    }
                    //kiếm con NG cũ
                    var find_lotng = await _actualWOService.CheckMaterialLotNG_OK(find_lot.orgin_mt_cd);
                    if (find_lot == null)
                    {
                        return Json(new { result = false, message = "Không tìm thấy mã này" }, JsonRequestBehavior.AllowGet);
                    }
                    //updtae lại số lượng ng
                    find_lotng.gr_qty = find_lotng.gr_qty + find_lot.gr_qty;
                    find_lotng.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    //db.Entry(find_lotng).State = EntityState.Modified;
                    //db.SaveChanges();
                    await _actualWOService.UpdateMaterialInfoMMS(find_lotng);

                    //xóa mã ok

                    await _actualWOService.DeleteMaterialInfoMMs(find_lot.wmtid);

                    return Json(new { result = true, message = "Thành Công !!" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !!" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetTIMSMaterialOK(Pageing paging)
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
                //Code mới
                var data = await _actualWOService.GetListMaterialOK();
                int start = (paging.page - 1) * paging.rows;
                int totals = data.Count();
                int totalPages = (int)Math.Ceiling((float)totals / paging.rows);
                var dataactual = data.Skip<MaterialInfoMMS>(start).Take(paging.rows);
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
                var data = new List<MaterialInfoMMS>();
                var jsonReturn = new
                {
                    result = false,
                    total = 0,
                    page = 0,
                    records = 0,
                    rows = data,
                    message = "Xin vui lòng đăng nhập"
                };
                return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<ActionResult> PartialView_Phan_loai_QC(string item_vcd)
        {
            var qc_itemcheck_mt = new List<QCItemCheck_Mt_Model>();
            var qc_itemcheck_mt2 = new List<QCItemCheck_Mt_Model>();

            var rs = await _ITIMSService.GetQCItemCheckMaterial(item_vcd);
            qc_itemcheck_mt = rs.Select(x => new QCItemCheck_Mt_Model
            {
                qc_itemcheck_mt__check_subject = x.check_subject,
                qc_itemcheck_mt__check_id = x.check_id,
                qc_itemcheck_mt__icno = x.icno,
            }).ToList();

            foreach (var item in qc_itemcheck_mt)
            {
                var view_qc_Model = new List<ViewQCModel>();

                //lấy hết tất cả qc_itemcheck_dt
                var qc_itemcheck_dt = await _ITIMSService.GetQCItemCheckMaterialDetail(item_vcd, item.qc_itemcheck_mt__check_id);
                //var qc_itemcheck_dt = db.qc_itemcheck_dt
                //    .Where(x => x.item_vcd.Equals(item_vcd) && x.check_id.Equals(item.qc_itemcheck_mt__check_id) && (x.del_yn == "N") && (x.defect_yn == "Y")).ToList();
                if (qc_itemcheck_dt.Count() > 0)
                {
                    foreach (var item1 in qc_itemcheck_dt)
                    {
                        var view_qc_Model_item = new ViewQCModel();
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
            return PartialView("~/Views/ActualWO/PrintNG/PartialView_Phan_loai_QC.cshtml", qc_itemcheck_mt);
        }


        #endregion insert info

        #region ATM

        public ActionResult ATM()
        {
            return View();
        }

        #endregion ATM

        #region ExportToMachineScan
        public ActionResult ExportToMachineScan()
        {
            return SetLanguage("~/Views/ActualWO/ExportToMachine/ExportToMachineScan.cshtml");
        }
        #endregion

        #region STATUS
        public async Task<JsonResult> XaMaterial(string materialcode)
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
                try
                {
                    if (string.IsNullOrEmpty(materialcode))
                    {
                        return Json(new { result = false, message = "Nguyên vật liệu bị rỗng, vui lòng scan lại" }, JsonRequestBehavior.AllowGet);
                    }
                    //kiểm tra có phải tài khoản admin không?
                    string user = Session["userid"] == null ? null : Session["userid"].ToString();
                    var userWIP = "";
                    if (!string.IsNullOrEmpty(user))
                    {
                        //var sqlquery = @"SELECT * FROM mb_info WHERE userid=@1 ";
                        //var dsMbInfo = db.Database.SqlQuery<mb_info>(sqlquery,
                        //    new MySqlParameter("1", user)).FirstOrDefault();
                        var dsMbInfo = await _actualWOService.GetMbInfoGrade(user);

                        userWIP = dsMbInfo.grade;
                    }
                    if (userWIP != "Admin")
                    {
                        return Json(new { result = false, message = "Tài khoản thuộc quyền Admin mới được chỉnh sửa" }, JsonRequestBehavior.AllowGet);
                    }
                    // kiêm tra tồn tại
                    var isExist = await _actualWOService.GetDataInventoryProduct(materialcode);
                    if (isExist == null)
                    {
                        return Json(new { result = false, message = "Liệu này không tồn tại trong hệ thống" }, JsonRequestBehavior.AllowGet);
                    }
                    //liệu ở trạng thái đang sử dụng mới cho xả
                    if (isExist.status.Equals("002"))
                    {
                        //update bảng w_material_mapping_mms. lấy id mới nhất của liệu
                        var checkLieuMaping = await _actualWOService.CheckwMaterialMappingMax(materialcode);
                        if (checkLieuMaping == null)
                        {
                            return Json(new { result = false, message = "Liệu chưa sử dụng không thể xả" }, JsonRequestBehavior.AllowGet);
                        }
                        checkLieuMaping.use_yn = "N";
                        checkLieuMaping.chg_id = user;
                        await _actualWOService.UpdateMaterialMappingMMS(checkLieuMaping);
                        //update bảng w_material_info

                        isExist.status = "005";
                        isExist.change_id = user;
                        await _actualWOService.UpdateInventoryProduct(isExist);



                        return Json(new { result = true, message = "Xả thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    if (isExist.status.Equals("001"))
                    {
                        return Json(new { result = false, message = "Liệu này đang tồn kho, không được xả" }, JsonRequestBehavior.AllowGet);
                    }
                    if (isExist.status.Equals("005"))
                    {
                        return Json(new { result = false, message = "Liệu này đã bị xả rồi" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { result = false, message = "Không thể xả" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }


        }
        #endregion
        public async Task<JsonResult> GetReasonDown(string mt_cd)
        {
            try
            {
                var result = await _actualWOService.GetDetailMaterialNG(mt_cd);
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        //print QRcode from wmtid in table w_material_info
        public async Task<ActionResult> PrintfQRCode(int code)
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
                var data = await _actualWOService.PrintfQRCode(code);
                if (data == null)
                {
                    return new ContentResult
                    {
                        ContentType = "text/html",
                        Content = "lỗi không tìm thấy nguyên vật liệu",
                    };
                }
                else
                {
                    ViewBag.MaterialCode = data.material_code;
                    ViewBag.MaterialNo = data.mt_no;
                    ViewBag.Length = data.gr_qty;

                    ViewBag.DateExport = "";
                    if (!string.IsNullOrEmpty(data.export_date))
                    {
                        var abc = data.export_date.Substring(11, 8);
                        ViewBag.DateExport = data.export_date.Substring(6, 4) + '-' + data.expiry_date.Substring(0, 2) + '-' + data.expiry_date.Substring(3, 2);
                    }
                    ViewBag.DateExpired = "";
                    if (!string.IsNullOrEmpty(data.expiry_date))
                    {
                        ViewBag.DateExpired = data.expiry_date.Substring(6, 4) + '-' + data.expiry_date.Substring(0, 2) + '-' + data.expiry_date.Substring(3, 2);
                    }
                    ViewBag.DateReceived = "";
                    if (!string.IsNullOrEmpty(data.date_of_receipt))
                    {
                        ViewBag.DateReceived = data.date_of_receipt.Substring(6, 4) + '-' + data.date_of_receipt.Substring(0, 2) + '-' + data.date_of_receipt.Substring(3, 2);
                    }

                    //ViewBag.DateExport = data.export_date;
                    //ViewBag.DateExpired = data.expiry_date;
                    //ViewBag.DateReceived = data.date_of_receipt;

                    ViewBag.LotNo = data.lot_no;
                    ViewBag.SendQuality = data.send_qty;
                    ViewBag.BundleQuality = data.bundle_qty;
                    ViewBag.BundleUnit = data.bundle_unit;
                    ViewBag.MaterialType = data.mt_type;
                    ViewBag.Width = data.width;
                    ViewBag.WidthUnit = data.width_unit;
                    ViewBag.OriginalLength = data.spec;
                    ViewBag.OriginalLengthUnit = data.spec_unit;
                    ViewBag.MaterialName = data.mt_nm;
                    return View("~/Views/ActualWO/PrintfQR/PrintfQR.cshtml");
                }
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" });
            }
        }
        public async Task<ActionResult> updateDescriptionWMaterialInfo(int? wmtid, string description)
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
                    //kiểm tra mã này có trong db không
                    var ISCheckExist = await _actualWOService.GetWMaterialInfo((int)wmtid);
                    if (ISCheckExist == null)
                    {
                        return Json(new { result = false, message = "Không tìm thấy mã này để sửa nội dung." }, JsonRequestBehavior.AllowGet);
                    }
                    if (string.IsNullOrEmpty(description))
                    {
                        return Json(new { result = false, message = "Vui lòng nhập nội dung cần thay đổi." }, JsonRequestBehavior.AllowGet);
                    }
                    await _actualWOService.UpdateWMaterialInfoDescription((int)wmtid, description);

                    return Json(new { result = true, message = "Sửa thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = "Xin vui lòng đăng nhập !" });
                }

            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Không thể sửa mã này" }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> GetProductProcesses(string productCode)
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
                var data1 = await _actualWOService.GetProductProcesses(productCode);
                return Json(data1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" });
            }
        }


    }
}