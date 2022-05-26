using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Mvc_VD.Models;
using System.Text;
using Mvc_VD.Classes;

//using OfficeOpenXml;
using System.Web.UI.WebControls;
using System.IO;
using ClosedXML.Excel;
using System.Collections;
using Mvc_VD.Commons;
using Mvc_VD.Services;
using MySql.Data.MySqlClient;
using Mvc_VD.Services.TIMS;
using Mvc_VD.Models.TIMS;
using System.Data.Entity;
using System.Web.Http;
using Mvc_VD.Models.APIRequest;
using HttpGetAttribute = System.Web.Mvc.HttpGetAttribute;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;
using Mvc_VD.Services.Interface;
using System.Threading.Tasks;
using Mvc_VD.Models.Request;
using Mvc_VD.Models.DTO;
using Mvc_VD.Services.Interface.MMS;
using Mvc_VD.Models.NewVersion;
using Microsoft.AspNetCore.Mvc;
using ActionResult = System.Web.Mvc.ActionResult;
using JsonResult = System.Web.Mvc.JsonResult;
using Mvc_VD.Models.Response;
using Mvc_VD.Models.WOModel;
using Mvc_VD.Services.Implement;
using System.Net.Http.Headers;
using System.Web.UI;
using System.ComponentModel;

namespace Mvc_VD.Controllers
{
    public class TIMSController : BaseController
    {
        private readonly IWOService _IWOService;
        private readonly IWorkRequestService _WorkRequestServices;
        private readonly ITimsService _ITimsService;
        private readonly IcommonService _IcommonService;
        private readonly IActualWOService _IActualWOService;
        private readonly IhomeService _IHomeService;

        private readonly ActualWOController _actualController;

        #region Override JsonResult

        public TIMSController(IWOService IWOService,
                IWorkRequestService WorkRequestServices,
                ITimsService ITimsService,
                IcommonService IcommonService,
                IActualWOService IActualWOService,
                IhomeService ihomeService,
                ActualWOController actualController
                )
        {
            _ITimsService = ITimsService;
            _IWOService = IWOService;
            _WorkRequestServices = WorkRequestServices;
            _IcommonService = IcommonService;
            _IActualWOService = IActualWOService;
            _actualController = actualController;
            _IHomeService = ihomeService;
            //db = DbFactory.Init();
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
            var result = _IHomeService.GetLanguage(lang, router);
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

        #region N_Receiving_Scan(TIMS)
        public ActionResult Receving_Scan()
        {
            return View("~/Views/TIMS/ReceivingScan/Receving_Scan.cshtml");
        }

        public async Task<JsonResult> GetListMaterialTimsReceivingPO(string po_no, string product, string input_dt, string bb_no)
        {
            bool check = false;
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                IEnumerable<TimsReceivingScanModel> data = await _ITimsService.GetListMaterialTimsReceivingPO(po_no, product, input_dt, bb_no);
                //bool check = true;
                if (data.Count() > 0)
                    check = true;
                return Json(new { data = data, result = check, message = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<TimsReceivingScanModel> data = new List<TimsReceivingScanModel>();
                return Json(new { data = data, result = check, message = "Xin vui lòng đăng nhập." }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<ActionResult> GetTimsReceiScanMLQR(string bb_no)
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
                    if (string.IsNullOrEmpty(bb_no))
                    {
                        return Json(new { result = false, message = "Làm ơn kiểm tra lại đồ đựng rỗng" }, JsonRequestBehavior.AllowGet);
                    }

                    var kttt_null = await _ITimsService.GetdbobbinlcthistFrbbno(bb_no);
                    if (kttt_null == null)
                    {
                        return Json(new { result = false, message = "Không tìm thấy đồ đựng này!!!" }, JsonRequestBehavior.AllowGet);
                    }

                    var L_ml_no_bb = kttt_null.mt_cd;
                    var count_rowpan = await _ITimsService.ChecklistMatialTIMS(L_ml_no_bb);

                    if (count_rowpan == 0)
                    {
                        return Json(new { result = false, message = "Đồ đựng này đã được sử dụng!!!" }, JsonRequestBehavior.AllowGet);
                    }
                    var data = await _ITimsService.CheckTimsReceiScanMLQR(L_ml_no_bb, count_rowpan);

                    return Json(new { result = true, message = Constant.Success, data }, JsonRequestBehavior.AllowGet);
                }
                else {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public async Task<ActionResult> UpdateMTQRRDList(string bb_no)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {
                    if (string.IsNullOrEmpty(bb_no))
                    {
                        return Json(new { result = false, message = Constant.ScanAgain }, JsonRequestBehavior.AllowGet);
                    }
                    var bobin = await _ITimsService.FindOneDBobbinInfo(bb_no);
                    if (bobin == null)
                    {
                        return Json(new { result = false, message = "Đồ đựng này đã bị xóa khỏi hệ thống!Vui lòng kiểm tra lại!!!" }, JsonRequestBehavior.AllowGet);
                    }


                    var mt_cd = await _ITimsService.FindOneMaterialInfoByMTCdBBNo(bobin.mt_cd, bb_no);
                    if (mt_cd == null)
                    {
                        return Json(new { result = false, message = "Không tìm thấy hàng chứa trong đồ đựng này!!!" }, JsonRequestBehavior.AllowGet);
                    }
                    if (mt_cd.location_code.StartsWith("006"))
                    {
                        return Json(new { result = false, message = "Đồ Đựng này đã được chuyển vào kho TIMS!!!" }, JsonRequestBehavior.AllowGet);
                    }
                    if (mt_cd.location_code.StartsWith("003"))
                    {
                        return Json(new { result = false, message = "Đồ Đựng này đã được chuyển vào kho FG!!!" }, JsonRequestBehavior.AllowGet);
                    }
                    if ((mt_cd.status == "001" && mt_cd.status == "002") && mt_cd.status.StartsWith("002") && mt_cd.gr_qty > 0)
                    {
                        return Json(new { result = false, message = "Không tồn tại trong danh sách này!!!" }, JsonRequestBehavior.AllowGet);
                    }

                    var data = await _ITimsService.GetListMaterialTimsReceivingPO("", "", "", bb_no);
                    int total = data.Count;
                    if (total == 0)
                    {
                        return Json(new { result = false, message = "Không tìm thấy đồ đựng này!!!" }, JsonRequestBehavior.AllowGet);
                    }
                    var user = Session["userid"] == null ? null : Session["userid"].ToString();
                    int result = await _ITimsService.UpdateMTQR_RDList(data[0].wmtid, user);
                    return Json(new { result = true, message = "Thành công!!!", Data = data }, JsonRequestBehavior.AllowGet);
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

        #region RD_Info_popup

        public ActionResult PartialView_RD_Info_Popup(string rd_no)
        {
            ViewBag.rd_no = rd_no;
            return PartialView("~/Views/TIMS/ReceivingScan/PartialView_RD_Info_Popup.cshtml");
        }

        public ActionResult PrintRD_LIST(string rd_no)
        {
            ViewData["Message"] = rd_no;
            return PartialView("~/Views/TIMS/ReceivingScan/PrintRD_LIST.cshtml");
        }

        #endregion RD_Info_popup

        #region N_PartialView_List_ML_NO_Info_Popup
        public ActionResult PartialView_List_ML_NO_Info_Popup(string rd_no)
        {
            ViewBag.rd_no = rd_no;
            return PartialView("~/Views/TIMS/ReceivingScan/PartialView_List_ML_NO_Info_Popup.cshtml");
        }

        public async Task<ActionResult> Receving_Scan_M(string data, string rd_no)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {
                    if (string.IsNullOrEmpty(rd_no))
                    {
                        return Json(new { result = false, message = Constant.ChooseAgain + " RD No" }, JsonRequestBehavior.AllowGet);
                    }
                    if (string.IsNullOrEmpty(data))
                    {
                        return Json(new { result = false, message = Constant.ChooseAgain + " Lot" }, JsonRequestBehavior.AllowGet);
                    }
                    string wtmid = data.Replace('[', '(').Replace(']', ')');
                    var kttt_null = await _ITimsService.CheckRecevingScanM(wtmid);
                    if (kttt_null > 0)
                    {
                        return Json(new { result = false, message = Constant.DataExist }, JsonRequestBehavior.AllowGet);
                    }
                    var user = Session["userid"] == null ? null : Session["userid"].ToString();
                    await _ITimsService.UpdateRecevingScanM(wtmid, "008", "006000000000000000", "006000000000000000", user);
                    var datalist = await _ITimsService.GetAllRecevingScanM(wtmid);

                    return Json(new { result = true, message = Constant.Success, datalist }, JsonRequestBehavior.AllowGet);
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

        #endregion N_PartialView_List_ML_NO_Info_Popup

        #endregion N_Receiving_Scan(TIMS)

        #region N_API_Receiving_Scan

        public async Task<ActionResult> Scan_Receiving_Tims(string bb_no, string rd_no)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {
                    if (string.IsNullOrEmpty(bb_no))
                    {
                        return Json(new { result = false, message = Constant.ScanAgain + " Container" }, JsonRequestBehavior.AllowGet);
                    }
                    if (string.IsNullOrEmpty(rd_no))
                    {
                        return Json(new { result = false, message = Constant.ChooseAgain + " RD No!!!" }, JsonRequestBehavior.AllowGet);
                    }
                    var kttt_null = await _ITimsService.GetdbobbinlcthistFrbbno(bb_no);
                    if (kttt_null == null)
                    {
                        return Json(new { result = false, message = Constant.DataExist }, JsonRequestBehavior.AllowGet);
                    }
                    var L_ml_no_bb = kttt_null.mt_cd;

                    var datraKT_ML = await _ITimsService.GetMaterialInfoMMSByOrgin(L_ml_no_bb);
                    var KT_ML = datraKT_ML.FirstOrDefault(x => x.location_code.Contains("002") && (x.status == "001" || x.status == "002") && x.gr_qty > 0);
                    if (KT_ML == null)
                    {
                        return Json(new { result = false, message = Constant.Used }, JsonRequestBehavior.AllowGet);
                    }
                    var user = Session["userid"] == null ? null : Session["userid"].ToString();

                    await _ITimsService.UpdateRecevingScanM(KT_ML.wmtid.ToString(), "008", "006000000000000000", "006000000000000000", user);

                    var data = await _ITimsService.CheckTimsReceiScanMLQR(KT_ML.material_code, 0);

                    return Json(new { result = true, message = Constant.Success, data }, JsonRequestBehavior.AllowGet);
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

        #endregion N_API_Receiving_Scan

        #region WorkRequest

        public ActionResult DeliveryPlan()
        {
            return View();
        }




        #endregion WorkRequest

        #region N_Shipping_Scan(TIMS)

        public ActionResult Shipping_Scan(string code)
        {
            ViewData["Message"] = code;
            return View("~/Views/TIMS/ShippingScan/Shipping_Scan.cshtml");
        }

        public async Task<ActionResult> GetEXTInfo(string ext_no, string ext_nm)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                var data = await _ITimsService.GetListwextinfo(ext_no, ext_nm);
                return Json(new { data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = new List<w_ext_info>();
                return Json(new { result = false, data, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> getdetail_RecordReceing(string date, string product, string shift)
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
                IEnumerable<WMaterialInfoTIMSAPIReceing> data = await _ITimsService.GetDetailActualAPIReceiving(date, product, shift);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> GetTimsShippingScanMLQR(string buyer_qr)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {
                    if (string.IsNullOrEmpty(buyer_qr))
                    {
                        return Json(new { result = false, message = Constant.ScanAgain }, JsonRequestBehavior.AllowGet);
                    }

                    var data = await _ITimsService.GetAllShippingScanMLQR(buyer_qr, "010", "006");
                    if (data.Count == 0)
                    {
                        return Json(new { result = false, message = Constant.Used }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { result = true, message = Constant.DataExist, data }, JsonRequestBehavior.AllowGet);
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

        [HttpGet]
        public async Task<JsonResult> GetStatusBobbin(string bb_no)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {
                    if (string.IsNullOrEmpty(bb_no))
                    {
                        return Json(new { result = false, message = Constant.ScanAgain }, JsonRequestBehavior.AllowGet);
                    }
                    var bb_noo = bb_no.Trim();

                    //var KIEMTRABUYER = await _ITimsService.ChecStatusBobbin(bb_noo);
                    ////var KIEMTRABUYER = _TIMSService.GetOneDBWithBuyer(bb_noo);

                    //if (KIEMTRABUYER > 0)
                    //{
                    //    return Json(new { result = false, message = Constant.MappingBuyerSuccess }, JsonRequestBehavior.AllowGet);
                    //}

                    var ds_bb = await _ITimsService.GetBobbinInfo(bb_noo);
                    if (ds_bb == null)
                    {
                        return Json(new { result = false, message = Constant.DataExist + " " + Constant.ScanAgain }, JsonRequestBehavior.AllowGet);
                    }
                    var bbinfo = await _ITimsService.FindOneDBobbinInfo(bb_noo);
                    if (string.IsNullOrEmpty(bbinfo.mt_cd))
                    {
                        return Json(new { result = false, message = "Bobbin này đã được xả!"}, JsonRequestBehavior.AllowGet);
                    }
                    else if (bbinfo.mt_cd.Contains("ROT") || bbinfo.mt_cd.Contains("STA"))
                    {
                        var data = await _ITimsService.GetProGetInfoBobinMMS(bb_noo);
                        data[0].ketluan = "Đang ở công đoạn : " + data[0].process;
                        return Json(new { result = true, message = Constant.Success, data = data }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var data = await _ITimsService.GetProGetInfoBobin(bb_noo);
                        data[0].ketluan = "Đang ở công đoạn : " + data[0].process;
                        return Json(new { result = true, message = Constant.Success, data = data }, JsonRequestBehavior.AllowGet);
                    }

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

        public async Task<JsonResult> GetStatusBuyer(string buyerCode)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {
                    if (string.IsNullOrEmpty(buyerCode))
                    {
                        return Json(new { result = false, message = Constant.ScanAgain }, JsonRequestBehavior.AllowGet);
                    }
                    var bb_noo = buyerCode.Trim();
                    //kt exist
                    int checkbuyer = await _ITimsService.CheckStatusBuyer(buyerCode);
                    if (checkbuyer == 0)
                    {
                        return Json(new { result = false, message = Constant.DataExist + " " + Constant.ScanAgain }, JsonRequestBehavior.AllowGet);
                    }

                    //StringBuilder sql = new StringBuilder($"CALL Pro_GetInfoBuyer('{buyerCode}');");

                    var data = await _ITimsService.GetlistBuyer(buyerCode);

                    return Json(new { result = true, message = Constant.Success, Data = data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { result = false, message="Lỗi hệ Thống" }, JsonRequestBehavior.AllowGet);
            }
        }

        #region EXT_Info_popup

        public ActionResult PartialView_EXT_Info_Popup(string ext_no)
        {
            ViewBag.ext_no = ext_no;
            return PartialView("~/Views/TIMS/ShippingScan/PartialView_EXT_Info_Popup.cshtml");
        }

        public ActionResult PrintEXT_LIST(string ext_no)
        {
            ViewData["Message"] = ext_no;
            return PartialView("~/Views/TIMS/ShippingScan/PrintEXT_LIST.cshtml");
        }

        #endregion EXT_Info_popup

        #region N_PartialView_GetList_ML_NO_Tims_Shipping_PP

        public ActionResult PartialView_GetList_ML_NO_Tims_Shipping_PP(string ext_no)
        {
            ViewBag.ext_no = ext_no;
            return PartialView("~/Views/TIMS/ShippingScan/PartialView_GetList_ML_NO_Tims_Shipping_PP.cshtml");
        }

        public async Task<ActionResult> Get_List_Material_TimsShipping(Pageing paging, string mt_no, string mt_cd, string buyer_qr)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {
                    var data = await _ITimsService.GetListMLNO(buyer_qr, mt_cd, mt_no);
                    var records = data.Count();
                    int totalPages = (int)Math.Ceiling((float)records / paging.rows);

                    int start = (paging.page - 1) * paging.rows;
                    var rowsData = data.Skip(start).Take(paging.rows);
                    return Json(new { total = totalPages, page = paging.page, records = records, rows = rowsData }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var data = new List<WMaterialnfo>();
                    return Json(new { result = false, total = 0, page = 0, records = 0, rows = data, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> TimsShipping_Scan_M(string data, string ext_no)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {
                    if (string.IsNullOrEmpty(ext_no))
                    {
                        return Json(new { result = false, message = Constant.ChooseAgain + " EXT No" }, JsonRequestBehavior.AllowGet);
                    }
                    if (string.IsNullOrEmpty(data))
                    {
                        return Json(new { result = false, message = Constant.CheckData }, JsonRequestBehavior.AllowGet);
                    }

                    var user = Session["userid"] == null ? null : Session["userid"].ToString();

                    await _ITimsService.UpdateExtnoMaterialIndfowithWMTID(ext_no, "000", "003G01000000000000", "003G01000000000000", user, data);

                    await _ITimsService.Updatewextinfo("1", ext_no);

                    string wtmid = data.Replace('[', '(').Replace(']', ')');
                    var datalist = await _ITimsService.GetAllRecevingScanM(wtmid);

                    return Json(new { result = true, message = Constant.Success, datalist }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var datas = new List<DatawActualPrimaryResponse>();
                    return Json(new { result = false, message = Constant.ErrorAuthorize, datas }, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion N_PartialView_GetList_ML_NO_Tims_Shipping_PP

        #endregion N_Shipping_Scan(TIMS)

        public JsonResult GetJsonPersons(DataTable data)
        {
            var lstPersons = GetTableRows(data);
            var a = Json(lstPersons, JsonRequestBehavior.AllowGet);
            return a;
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

        #region TIMS Web

        public ActionResult Web()
        {
            return SetLanguage("~/Views/TIMS/Web/Web.cshtml");
        }

        public async Task<JsonResult> GetProducts(string productNo, string productName, string modelCode)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                var data = await _ITimsService.GetListdstyleinfo(productNo, productName, modelCode);
                return Json(new { result = true, message = "", data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PartialView_dialog_Viewdetaildatetime(int id_actual)
        {
            ViewBag.id_actual = id_actual;

            return PartialView("~/Views/TIMS/Web/PartialView_dialog_Viewdetaildatetime.cshtml");

        }

        public async Task<JsonResult> UpdateProcessIsFinish(int id_actual, bool IsFinished)
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
                    var checkexist = await _ITimsService.GetWActual(id_actual);
                    if (checkexist == null)
                    {
                        return Json(new { result = false, message = "Sửa lỗi" }, JsonRequestBehavior.AllowGet);
                    }
                    if (IsFinished)
                    {
                        await _ITimsService.UpdateProcessToHieuSuat(checkexist.at_no);
                    }
                    int update = await _ITimsService.UpdateActualIsFinish(id_actual, IsFinished);
                    if (update > 0)
                    {
                        return Json(new { result = true, message = "Sửa thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { result = false, message = "Sửa lỗi" }, JsonRequestBehavior.AllowGet);
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

        public async Task<JsonResult> GetQCs(string qcType, string qcCode, string qcName, string qcExp)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                if (string.IsNullOrEmpty(qcType))
                {
                    qcType = "";
                }
                if (string.IsNullOrEmpty(qcCode))
                {
                    qcCode = "";
                }
                if (string.IsNullOrEmpty(qcName))
                {
                    qcName = "";
                }
                if (string.IsNullOrEmpty(qcExp))
                {
                    qcExp = "";
                }
                var list = await _ITimsService.GetqcitemmtWithQCTYpe("N", "PC", qcType, qcCode, qcName, qcExp);

                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = new List<qc_item_mt>();
                return Json(new { result = false, message = Constant.ErrorAuthorize, data }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetTIMSProcesses()
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                var result = await _ITimsService.GetStaff("COM007", "Y");
                var position = result.AsEnumerable().Select(x => new {
                    ProcessCode = x.dt_cd,
                    ProcessName = x.dt_nm
                }).ToList();
                return Json(position, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = new List<comm_dt>();
                return Json(new { result = false, message = Constant.ErrorAuthorize, data }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetTIMSRolls()
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                var list = await _ITimsService.GetCommon("COM032");
                var values = list.AsEnumerable().Select(x => new {
                    Code = x.dt_cd,
                    Name = x.dt_nm,
                    UseYN = x.use_yn
                }).ToList();
                return Json(values, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = new List<comm_dt>();
                return Json(new { result = false, message = Constant.ErrorAuthorize, data }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetDataWActualPrimary([System.Web.Http.FromBody] DataWActualPrimaryReq item)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || (resqheadervalmob == "Mob"))
            {

                string type = item.type;
                if (type == "")
                {
                    var data = await _ITimsService.getListPO(item);
                    var records = data.Count();
                    int totalPages = (int)Math.Ceiling((float)records / item.rows);

                    int start = (item.page - 1) * item.rows;
                    var rowsData = data.Skip(start).Take(item.rows);
                    return Json(new { total = totalPages, page = item.page, records = records, rows = rowsData }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    var data = await _ITimsService.getListPO(item);
                    var records = data.Count();
                    int totalPages = (int)Math.Ceiling((float)records / item.rows);

                    int start = (item.page - 1) * item.rows;
                    var rowsData = data.Skip(start).Take(item.rows);
                    return Json(new { total = totalPages, page = item.page, records = records, rows = rowsData }, JsonRequestBehavior.AllowGet);

                }
            }
            else
            {
                var data = new List<DatawActualPrimaryResponse>();
                return Json(new { result = false, total = 0, page = 0, records = 0, rows = data, message = "Xin vui lòng đăng nhập." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetFinishPO(Pageing pageing, string product, string product_name, string model, string at_no, string regstart, string regend)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                var data = await _ITimsService.GetAllFinishProducts(product, product_name, model, at_no, regstart, regend);
                var records = data.Count();
                int totalPages = (int)Math.Ceiling((float)records / pageing.rows);
                var rowsData = data.Skip((pageing.page - 1)).Take(pageing.rows);
                return Json(new { total = totalPages, page = pageing.page, records = records, rows = rowsData }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
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
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                var poModified = await _ITimsService.FindOneWActualPrimaryByAtNo(po);
                await _IActualWOService.UpdatePO("YT", poModified.id_actualpr);
                poModified.finish_yn = "YT";
                return Json(new { result = true, id = poModified.id_actualpr }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> Getdataw_actual_SX(Pageing pageing, string at_no)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                var data = await _ITimsService.GetDataActualSX(at_no);
                var records = data.Count();
                int totalPages = (int)Math.Ceiling((float)records / pageing.rows);
                var rowsData = data.Skip((pageing.page - 1)).Take(pageing.rows);
                return Json(new { result = true, total = totalPages, page = pageing.page, records = records, rows = rowsData }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var rowsData = new List<WMaterialnfo>();
                return Json(new { result = false, total = 0, page = 0, records = 0, rows = rowsData, message = "Xin vui lòng đăng nhập." }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<JsonResult> GetListStaffTims(Pageing pageing, int id_actual)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                string staff_id = Request["staff_id"] == null ? "" : Request["staff_id"].Trim();
                string StartDate = Request["StartDate"] == null ? "" : Request["StartDate"].Trim();
                string EndDate = Request["EndDate"] == null ? "" : Request["EndDate"].Trim();
                string staffName = Request["staff_name"] == null ? "" : Request["staff_name"].Trim();

                var data = await _ITimsService.GetTIMSListStaff(id_actual, staff_id, StartDate, EndDate, staffName);
                var records = 0;
                if (data != null)
                {
                    records = data.Count();
                }

                int totalPages = (int)Math.Ceiling((float)records / pageing.rows);
                var rowsData = data.Skip((pageing.page - 1)).Take(pageing.rows);
                return Json(new { result = true, total = totalPages, page = pageing.page, records = records, rows = rowsData }, JsonRequestBehavior.AllowGet);
            } else
            {
                var rowsData = new List<ListStaffTims>();
                return Json(new { result = false, total = 0, page = 0, records = 0, rows = rowsData, message = "Xin vui lòng đăng nhập." }, JsonRequestBehavior.AllowGet);
            }

        }

        //public async Task<JsonResult> Getprocess_staff(int id_actual, int psid)
        public async Task<JsonResult> GetProcessStaffTims(int id_actual, int psid)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {

                    string user = Session["userid"] == null ? null : Session["userid"].ToString();
                    //var find = _TIMSService.FindOneWActual(id_actual);
                    var find = await _ITimsService.FindOneWActual(id_actual);
                    if (id_actual == 0)
                    {
                        return Json(new { result = false, message = "Vui Lòng Kiểm tra lại" }, JsonRequestBehavior.AllowGet);
                    }

                    //var delete = _TIMSService.FindOneDProUnitStaffById(psid);
                    var delete = await _ITimsService.FindOneDProUnitStaffById(psid);
                    //int ca = _TIMSService.CheckShift(psid);
                    int ca = await _ITimsService.CheckShiftWithPisd(psid);
                    if (ca == 0)
                    {
                        return Json(new { result = false, message = Constant.EndShift }, JsonRequestBehavior.AllowGet);
                    }
                    var type = "";
                    if (find != null)
                    {
                        type = find.name;
                    }
                    if (type != "OQC")
                    {
                        //if (db.w_material_info.Any(x => x.id_actual == id_actual && x.staff_id == delete.staff_id))
                        //{
                        //    return Json(new { result = false, message = "MT LOT " + Constant.Exist }, JsonRequestBehavior.AllowGet);
                        //}

                        var dataExist = await _ITimsService.FindAllWMaterialInfoByStaffId(delete.staff_id, id_actual, delete.start_dt.ToString().Substring(0, delete.start_dt.ToString().Length - 5), delete.end_dt.ToString().Substring(0, delete.end_dt.ToString().Length - 5));
                        if (dataExist.ToList().Count > 0)
                        {
                            return Json(new { result = false, message = "MT LOT " + Constant.Exist }, JsonRequestBehavior.AllowGet);
                        }


                    }
                    else
                    {
                        var check = await _ITimsService.CheckWMaterialInfoByStaffIdOQC(delete.staff_id, id_actual);
                        if (check > 0)
                        {
                            return Json(new { result = false, message = "MT LOT " + Constant.Exist }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    //var type = "";
                    //if (find != null)
                    //{
                    //    type = find.name;
                    //}

                    //int check = await _ITimsService.CheckWMaterialInfoByStaffIdOQC(delete.staff_id, id_actual);
                    //if (check > 0)
                    //{
                    //    return Json(new { result = false, message = "MT LOT " + Constant.Exist }, JsonRequestBehavior.AllowGet);
                    //}

                    if (delete != null)
                    {
                        //int result = _TIMSService.DeleteDProUnitStaff(delete.id_actual, delete.psid, delete.staff_id);
                        await _ITimsService.DeleteDProUnitStaff(delete.id_actual, delete.psid, delete.staff_id, user);
                        return Json(new { result = true, psid = delete.psid }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { result = false, message = Constant.CannotDelete }, JsonRequestBehavior.AllowGet);
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

        public async Task<JsonResult> GetInfoActualTims(Pageing paging, string at_no)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                //var data = _TIMSService.GetTIMSActualInfoBySP(at_no);
                var data = await _ITimsService.GetActualInfo(at_no);
                var records = data.Count();
                int totalPages = (int)Math.Ceiling((float)records / paging.rows);
                var rowsData = data.Skip((paging.page - 1)).Take(paging.rows);
                return Json(new { result = true, total = totalPages, page = paging.page, records = records, rows = rowsData }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = new List<WActualAPI>();
                return Json(new { result = false, total = 0, page = 0, records = 0, rows = data, message = "Xin vui lòng đăng nhập." }, JsonRequestBehavior.AllowGet);
            }

        }

        //public JsonResult xoa_wactual_con(int id)
        [System.Web.Http.HttpDelete]
        public async Task<JsonResult> DeleteActualChild(int id)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {

                    //var find_id = db.w_actual.Find(id);
                    var find_id = await _ITimsService.GetWActual(id);
                    if (find_id != null)
                    {
                        if (find_id.name.StartsWith("OQC"))
                        {
                            var check1 = await _ITimsService.GetMaterialInfoTIMSByOQC(find_id.id_actual);
                            //if (db.w_material_info.Any(x => x.id_actual_oqc == find_id.id_actual))
                            if (check1 != null)
                            {
                                return Json(new { result = false, message = "Already Exits MT LOT" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            var check2 = await _ITimsService.GetMaterialInfoTIMSById(find_id.id_actual);
                            //if (db.w_material_info.Any(x => x.id_actual == find_id.id_actual))
                            if (check2 != null)
                            {
                                return Json(new { result = false, message = "Already Exits MT LOT" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        await _ITimsService.DeleteActualForTIMS(find_id.id_actual);
                        return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { result = false, message = "Can not Find" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = e }, JsonRequestBehavior.AllowGet);
            }
        }

        //public JsonResult GetTIMSActualDetail(int id_actual, int psid)
        public async Task<JsonResult> GetListActualDetailTims(int id_actual, int psid)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {

                    //var check_vo = _TIMSService.FindOneWActual(id_actual);
                    var check_vo = await _ITimsService.FindOneWActual(id_actual);
                    //var check_stff = _TIMSService.FindOneDProUnitStaffById(psid);
                    var check_stff = await _ITimsService.FindOneDProUnitStaffById(psid);
                    StringBuilder sql = new StringBuilder();

                    if (check_vo.name == "OQC")
                    {
                        var data1 = await _ITimsService.FindDetailActualStaffOQC(check_stff.staff_id, id_actual, check_stff.end_dt.ToString().Substring(0, check_stff.end_dt.ToString().Length - 5), check_stff.start_dt.ToString().Substring(0, check_stff.start_dt.ToString().Length - 5));
                        return Json(new { result = true, data = data1 }, JsonRequestBehavior.AllowGet);
                    }

                    var data = await _ITimsService.GetTIMSActualDetailByStaff(check_stff.staff_id, id_actual, check_stff.start_dt.ToString().Substring(0, check_stff.start_dt.ToString().Length - 5), check_stff.end_dt.ToString().Substring(0, check_stff.end_dt.ToString().Length - 5));
                    return Json(new { result = true, data = data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { result = false, data = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        //public async Task<JsonResult> Add_w_actual(string style_no, string at_no, string name, string roll, w_actual w_actual)
        [HttpPost]
        public async Task<JsonResult> AddActualForTims(string style_no, string at_no, string name, string roll, Actual w_actual)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {

                    //w_actual.date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    var check_data = await _ITimsService.FindAllWActualByAtNo(at_no);
                    var nameExist = check_data.Where(x => x.name == name && x.active == true).ToList();
                    if (nameExist.Count() > 0 )
                    {
                        w_actual.level = nameExist.FirstOrDefault().level;
                    }
                    else
                    {
                        w_actual.level = check_data.Count() + 1;
                    }

                    w_actual.type = "TIMS";
                    w_actual.actual = 0;
                    w_actual.at_no = at_no;
                    w_actual.product = style_no;
                    w_actual.name = name;
                    w_actual.don_vi_pr = roll;
                    w_actual.defect = 0;
                    w_actual.reg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
                    w_actual.chg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
                    w_actual.reg_id = Session["userid"] == null ? "Admin" : Session["userid"].ToString();
                    w_actual.chg_id = Session["userid"] == null ? "Admin" : Session["userid"].ToString();
                    if (name == "OQC")
                    {
                        w_actual.item_vcd = "OC000001A";
                    }
                    else
                    {
                        w_actual.item_vcd = "TI000001A";
                    }

                    if (ModelState.IsValid)
                    {

                        int idInserted = await _IActualWOService.insertActualInfo(w_actual);
                        var vlieu = style_no + "-" + name;
                        var check = await _ITimsService.CheckDMaterialInfo(vlieu);
                        //if (!(db.d_material_info.Any(x => x.mt_no == vlieu)))
                        if (check < 1)
                        {
                            var b = new MaterialInfo();
                            b.mt_no = vlieu;
                            b.mt_nm = vlieu;
                            b.mt_type = "CMT";
                            b.reg_dt = DateTime.Now;
                            b.bundle_unit = "EA";
                            b.chg_dt = DateTime.Now;
                            b.reg_id = Session["userid"] == null ? "Admin" : Session["userid"].ToString();
                            b.chg_id = Session["userid"] == null ? "Admin" : Session["userid"].ToString();
                            b.use_yn = "Y";
                            b.del_yn = "N";
                            await _IActualWOService.insertMaterialForActual(b);
                            //db.Entry(b).State = EntityState.Added;
                            //db.SaveChanges(); // line that threw exception
                        }
                        //StringBuilder sql = new StringBuilder($"CALL spTIMS_GetTIMSActualInfo('{at_no}','','','{w_actual.id_actual}');");
                        //var result2 = _TIMSService.GetTIMSActualInfoBySP(at_no);
                        var result2 = await _ITimsService.GetActualInfo(at_no);
                        return Json(new { result = true, message = "Thêm công đoạn thành công!", kq = result2 }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { result = false, message = Constant.Duplicate }, JsonRequestBehavior.AllowGet);
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

        public JsonResult GetTIMSWorkerMappingList()
        {
            return null;
        }
        [HttpGet]
        public async Task<JsonResult> GetWorkers(Pageing pageing, string userId, string userName, string positionCode)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {

                var datas = await _ITimsService.GetListWorker(userId, userName, positionCode);
                int totalRecords = datas.Count();
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageing.rows);
                var data = datas.Skip<ListWorker>((pageing.page - 1) * (pageing.rows)).Take(pageing.rows);
                var result = new
                {

                    total = totalPages,
                    page = pageing.page,
                    records = totalRecords,
                    rows = data

                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = new List<ListWorker>();
                var result = new
                {
                    result = false,
                    total = 0,
                    page = 0,
                    records = 0,
                    rows = data,
                    message = Constant.ErrorAuthorize
                };
                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetAllWorkerPositions()
        {
            var list = await _ITimsService.GetCommon("COM018");
            var values = list.AsEnumerable().Select(x => new {
                Code = x.dt_cd,
                Name = x.dt_nm,
                UseYN = x.use_yn
            }).ToList();
            return Json(values, JsonRequestBehavior.AllowGet);
            //return Json(new InitMethods().GetCommon("COM018"), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetStaffTypes()
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {

                var list = await _ITimsService.GetCommon("COM013");
                var values = list.AsEnumerable().Select(x => new {
                    Code = x.dt_cd,
                    Name = x.dt_nm,
                    UseYN = x.use_yn
                }).ToList();
                return Json(values, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var dataa = new List<comm_dt>();
                return Json(new { result = false, message = Constant.ErrorAuthorize, dataa }, JsonRequestBehavior.AllowGet);
            }

        }

        //-------------------HẰNG CODE
        public async Task<ActionResult> Createprocess_unitstaff(string staff_id, string use_yn, Models.NewVersion.DProUnitStaff a)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                try
                {
                    var start1 = DateTime.Now;
                    //var dsach_ca = db.w_policy_mt.ToList();
                    float timenow = float.Parse(start1.ToString("HH")) + float.Parse(DateTime.Now.ToString("mm")) / 60;
                    //var end = _IWOService.GetEndDateProcessUnitStaff(timenow);
                    var end = await _IcommonService.GetEndDateProcessUnit(timenow);
                    var start = DateTime.Now;
                    var dateConvert = new DateTime();

                    var item_count = await _ITimsService.FindDProUnitStaffByStaffIdIdActual(a.id_actual, staff_id);
                    if (item_count.Count() == 0)
                    {
                        a.start_dt = start;
                        a.end_dt = end;
                        a.staff_id = staff_id;
                        a.use_yn = use_yn;
                        a.del_yn = "N";
                        a.chg_dt = DateTime.Now;
                        a.reg_dt = DateTime.Now;
                        a.id_actual = a.id_actual;
                        int idReturn = await _IActualWOService.InsertUnitStaff(a);
                        a.psid = idReturn;
                        var vaule = await _ITimsService.FindOneDProUnitStaffReturnByPsid(idReturn);
                        return Json(new { result = 0, kq = vaule }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var list = item_count.ToList();
                        if (item_count.Count() >= 1)
                        {
                            bool duplicate_time = false;
                            foreach (var item in list)
                            {
                                var item_start_dt = item.start_dt;
                                var item_end_dt = item.end_dt;
                                string starts = "";
                                string ends = "";
                                string itemstartdt = "";
                                string itemenddt ="";
                                //var dateConverts = new DateTime();
                                if (DateTime.TryParse(item_start_dt, out dateConvert))
                                {
                                    itemstartdt = dateConvert.ToString("yyyyMMddHHmmss");
                                }
                                if (DateTime.TryParse(item_end_dt, out dateConvert))
                                {
                                    itemenddt = dateConvert.ToString("yyyyMMddHHmmss");
                                }
                                //string stast = start.ToString("MM/dd/yyyy HH:mm:ss");
                                 starts = start.ToString("yyyyMMddHHmmss");
                                //DateTime myDate = DateTime.ParseExact(stast, "yyyy-MM-dd HH:mm:ss,fff",
                                //       System.Globalization.CultureInfo.InvariantCulture);
                                //if (DateTime.TryParse(stast, out dateConvert))
                                //{
                                //    starts = dateConvert;
                                //}
                                //string endd = end.ToString("MM/dd/yyyy HH:mm:ss");
                                 ends = end.ToString("yyyyMMddHHmmss");
                                //if (DateTime.TryParse(endd, out dateConvert))
                                //{
                                //    ends = dateConvert;
                                //}

                                bool condition_after = false;
                                bool condition_before = false;
                                //if (DateTime.Compare(starts, itemstartdt) > 0 && DateTime.Compare(ends, itemstartdt) > 0)
                                if (Convert.ToInt64(starts)> Convert.ToInt64(itemstartdt)&& Convert.ToInt64(ends) > Convert.ToInt64(itemstartdt))
                                {
                                    condition_after = true;
                                }
                                //if (DateTime.Compare(itemstartdt, starts) > 0 && DateTime.Compare(ends, itemstartdt) > 0)
                                if (Convert.ToInt64(itemenddt) < Convert.ToInt64(starts) && Convert.ToInt64(ends) > Convert.ToInt64(itemstartdt))
                                {
                                    condition_before = true;
                                }
                                if (!condition_before || !condition_after)
                                {
                                    duplicate_time = true;
                                    if (item.id_actual != a.id_actual)
                                    {
                                        if (ModelState.IsValid)
                                        {
                                            return Json(new { result = 3, update = item.psid, start = start, end = end }, JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                }
                            }
                            if (duplicate_time == false && list.Count > 0)
                            {
                                a.start_dt = start;
                                a.end_dt = end;
                                a.staff_id = staff_id;
                                a.use_yn = use_yn;
                                a.del_yn = "N";
                                a.chg_dt = DateTime.Now;
                                a.reg_dt = DateTime.Now;
                                a.id_actual = a.id_actual;
                                int idReturn = await _IActualWOService.InsertUnitStaff(a);
                                a.psid = idReturn;
                                var vaule = await _ITimsService.FindOneDProUnitStaffReturnByPsid(idReturn);
                                return Json(new { result = 0, kq = vaule }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { result = 1, staff_id = staff_id }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }
        //public async Task<ActionResult> Modifyprocess_unitstaff(int psid, string staff_id, string use_yn, d_pro_unit_staff a, string end, string start)

        public async Task<ActionResult> ModifyProcessUnitStaffTims(int psid, string staff_id, string use_yn, d_pro_unit_staff a, string end, string start)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {

                    bool checkUserExist = await _ITimsService.CheckExistsWorker(staff_id);

                    if (checkUserExist)
                    {
                        return Json(new { result = false, message = "Staff " + Constant.NotFound }, JsonRequestBehavior.AllowGet);
                    }

                    DProUnitStaffAPI item1 = await _ITimsService.FindOneDProUnitStaffById(psid);
                    //if (Convert.ToInt64(start) >= Convert.ToInt64(end) || Convert.ToInt64(item1.start_dt) < Convert.ToInt64(start))
                    if (Convert.ToDateTime(start) >= Convert.ToDateTime(end) || Convert.ToDateTime(item1.start_dt) < Convert.ToDateTime(start))
                    {
                        return Json(new { result = 2, staff_id = staff_id, message = "Lỗi Thời gian bắt đầu lớn hơn thời gian kết thúc!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        string time_cuoi = await _ITimsService.FindDProUnitStaffByStaffId(psid, staff_id, item1.id_actual);
                        if (string.IsNullOrEmpty(time_cuoi))
                        {
                            item1.uname = await _ITimsService.GetUname(staff_id);
                            item1.start_dt = start;
                            item1.end_dt = end;
                            item1.staff_id = staff_id;
                            item1.use_yn = use_yn;
                            item1.del_yn = "N";
                            await _ITimsService.UpdateDProUnitStaff(item1);

                            return Json(new { result = 0, kq = item1, message = "Bạn đã thay đổi công nhân thành công!" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            if (DateTime.Parse(time_cuoi) >= DateTime.Parse(end))
                            {
                                return Json(new { result = 2, staff_id = staff_id, message = "Lỗi Thời gian bắt đầu lớn hơn thời gian kết thúc!" }, JsonRequestBehavior.AllowGet);
                            }


                            //bool duplicate_time = false;
                            bool duplicate_time = await _ITimsService.CheckExistsDuplicateTime(staff_id, start, end, psid);

                            //var data1 = _TIMSService.CheckExistDuplicateTime(staff_id, start, end, psid);

                            //if (data1)
                            //{
                            //    duplicate_time = true;
                            //}
                            if (duplicate_time == true)
                            {

                                DProUnitStaffAPI item = await _ITimsService.FindOneDProUnitStaffById(psid);
                                item.uname = await _ITimsService.GetUname(staff_id);
                                item.start_dt = start;
                                item.end_dt = end;
                                item.staff_id = staff_id;
                                item.use_yn = use_yn;
                                item.del_yn = "N";
                                //_TIMSService.UpdateDProUnitStaff(item);
                                //item.start_dt = item.start_dt.Substring(0, 4) + "-" + item.start_dt.Substring(4, 2) + "-" + item.start_dt.Substring(6, 2) + " " + item.start_dt.Substring(8, 2) + ":" + item.start_dt.Substring(10, 2) + ":" + item.start_dt.Substring(12, 2);
                                //item.end_dt = item.end_dt.Substring(0, 4) + "-" + item.end_dt.Substring(4, 2) + "-" + item.end_dt.Substring(6, 2) + " " + item.end_dt.Substring(8, 2) + ":" + item.end_dt.Substring(10, 2) + ":" + item.end_dt.Substring(12, 2);
                                //return Json(new { result = 0, kq = item }, JsonRequestBehavior.AllowGet);
                                int kq = await _ITimsService.UpdateDProUnitStaff(item);

                                return Json(new { result = 0, kq = item, message = "Bạn đã thay đổi công nhân thành công!" }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { result = 1, staff_id = staff_id, message = "Lỗi thời gian đã bị trùng!" }, JsonRequestBehavior.AllowGet);
                            }
                        }

                    }
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

        public async Task<ActionResult> getdetail_actual(int id_actual, string date, string shift, int staff_id = -1)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {
                    var check_vo = await _ITimsService.FindOneWActual(id_actual);
                    IEnumerable<WMaterialInfoTIMSAPIRec> data1 = await _ITimsService.GetDetailActualAPIOQC(id_actual, date, shift);
                    return Json(data1, JsonRequestBehavior.AllowGet);
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

        public async Task<ActionResult> searchShippingSortingTIMS(Pageing pageing, string ShippingCode, string ProductCode, string ProductName, string Description)
        {
            Dictionary<string, string> list = PagingAndOrderBy(pageing, " ORDER BY MyDerivedTable.ExportCode DESC ");
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                int totalRecords =await _ITimsService.TotalRecordsSearchShippingSortingTims(ShippingCode, ProductCode, ProductName, Description);
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

                IEnumerable<ShippingTIMSSortingModel> Data =await _ITimsService.GetListSearchShippingSortingTIMS(ShippingCode, ProductCode, ProductName, Description);

                var result = new
                {
                    total = totalPages,
                    page = int.Parse(list["index"]),
                    records = totalRecords,
                    rows = Data
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var rs = new List<ShippingTIMSSortingModel>();
                return Json(new { result = rs, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PartialViewShippingTIMSSortingPP(string ShippingCode, bool edit)
        {
            ViewBag.Deleted = edit;
            ViewBag.ShippingCode = ShippingCode;
            return PartialView("~/Views/TIMS/SortingTims/ShippingTims/PartialViewShippingTIMSSortingPP.cshtml");
        }
        public async Task<ActionResult> GetShippingSortingTIMSPP(string ShippingCode)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                var listdata = await _ITimsService.GetListSearchShippingSortingTIMSPP(ShippingCode);
                return Json(new { data = listdata }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var rs = new List<ShippingTIMSSortingModel>();
                return Json(new { result = rs, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> GetListShippingFGSorting(string ShippingCode)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {
                    var listdata = await _ITimsService.GetListShippingTIMSSorting(ShippingCode);
                    return Json(new { data = listdata }, JsonRequestBehavior.AllowGet);
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
        public async Task<ActionResult> GetShippingScanPP_Countbuyer(string ShippingCode)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {
                    var listdata = await _ITimsService.GetShippingScanPPCountbuyer(ShippingCode);
                    return Json(new { data = listdata }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }

        }
        //-------------------HẰNG CODE - END

        #region ROll Nomal

        public async Task<ActionResult> searchbobbinPopup(Pageing pageing,string bb_no, string bb_nm, string mt_cd, int id_actual)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                int start = (pageing.page - 1) * pageing.rows;
                var data = await _ITimsService.GetListBoBinPopup(bb_no, bb_nm, mt_cd, id_actual, start, pageing.rows);

                int totalRecords = _ITimsService.getTotalRecordBobbinInfo();
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageing.rows);
                var result = new
                {
                    total = totalPages,
                    page = pageing.page,
                    records = totalRecords,
                    rows = data
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = new List<WMaterialnfo>();
                return Json(new { result = false, message = Constant.ErrorAuthorize, data }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public async Task<JsonResult> InsertMLNoWithSelectedBobin(Models.NewVersion.MaterialInfoTIMS a, w_material_mapping c, int id_actual, string name, string bb_no,
             string staff_id, int psid, string style_no)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {

                    string styleno = Request["style_no"] == null ? style_no : Request["style_no"].Trim();
                    var ds_bb = await _ITimsService.GetOneDBobbinInfoWithMtCdIsNULL(bb_no);
                    if (string.IsNullOrEmpty(staff_id))
                    {
                        return Json(new { result = false, message = Constant.CheckData + " Staff" }, JsonRequestBehavior.AllowGet);
                    }
                    else if (ds_bb == null)
                    {
                        return Json(new { result = false, message = "Container" + Constant.DataExist }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if ((ds_bb.mt_cd != "" && ds_bb.mt_cd != null) && ds_bb != null)
                        {
                            return Json(new { result = false, message = Constant.Used }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    var check_exit_vo = await _ITimsService.FindOneWActual(id_actual);
                    if (check_exit_vo == null)
                    {
                        return Json(new { result = false, message = "PO " + Constant.DataExist }, JsonRequestBehavior.AllowGet);
                    }
                    var ca = await _ITimsService.CheckShift(psid, id_actual);
                    if (ca == 0)   //het ca
                    {
                        return Json(new { result = false, message = Constant.EndShift }, JsonRequestBehavior.AllowGet);
                    }
                    string user = Session["userid"] == null ? "Mob" : Session["userid"].ToString();
                    //Models.NewVersion.MaterialInfoMMS item =new Models.NewVersion.MaterialInfoMMS;
                    //DateTime dt1 = DateTime.Now; //Today at 00:00:00
                    string time_now = DateTime.Now.ToString("HHmmss");
                    string time_now1 = DateTime.Now.ToString("yyyyMMdd");
                    time_now = time_now + Guid.NewGuid().ToString("N").Substring(0, 3).ToUpper();
                    a.material_type = "CMT";
                    a.bb_no = bb_no;
                    a.real_qty = 0;
                    a.mt_no = styleno + "-" + check_exit_vo.name;
                    //a.bbmp_sts_cd = "000";
                    a.status = "002";
                    a.location_code = "006000000000000000";
                    a.id_actual = id_actual;
                    a.from_lct_code = "006000000000000000";
                    a.gr_qty = 0;
                    a.chg_date = DateTime.Now;
                    a.reg_date = DateTime.Now;
                    a.reg_id = user;
                    a.chg_id = user;

                    a.staff_id = staff_id;

                    var bien_first = styleno + "-"  + check_exit_vo.name + time_now1 + time_now;

                    var menuCd = string.Empty;

                    menuCd = string.Format("{0, 0:D3}", 1);

                    a.material_code = bien_first + menuCd;
                    a.staff_id = staff_id;
                    a.product = check_exit_vo.product;
                    a.at_no = check_exit_vo.at_no;

                    int idReturn = await _ITimsService.InsertMaterialInfoTIMMS(a);
                    //int idReturn = _TIMSService.InsertWMaterialInfoToBobin(a);
                    //mapping mt_cd với bobin
                    ds_bb.mt_cd = a.material_code;
                    ds_bb.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    ds_bb.count_number = 1;
                    int count_number = int.Parse(ds_bb.count_number.ToString());
                    await _ITimsService.UpdateBobbinInfo(user, ds_bb.mt_cd, ds_bb.bno);
                    var history = await _ITimsService.FindOneBobbin_lct_hist(bb_no, ds_bb.mt_cd);
                    if (history == null) //ko co trong do thi insert // if (data.Count() < 0 )
                    {
                        DBobbinLctHist d_bobbin_lct_hist = new DBobbinLctHist();
                        d_bobbin_lct_hist.bb_no = ds_bb.bb_no;
                        d_bobbin_lct_hist.bb_nm = ds_bb.bb_nm;
                        d_bobbin_lct_hist.mc_type = ds_bb.mc_type;
                        d_bobbin_lct_hist.mt_cd = ds_bb.mt_cd;
                        d_bobbin_lct_hist.use_yn = "Y";
                        d_bobbin_lct_hist.del_yn = "N";
                        d_bobbin_lct_hist.reg_dt = DateTime.Now;
                        d_bobbin_lct_hist.chg_dt = DateTime.Now;
                        d_bobbin_lct_hist.reg_id = user;
                        d_bobbin_lct_hist.chg_id = user;
                        await _ITimsService.InsertBobbinHist(d_bobbin_lct_hist);
                    }
                    string checkmtlotmpping = await _ITimsService.CheckMaterialMappingForTimsRoll(id_actual, staff_id, a.material_code);
                    if (!string.IsNullOrEmpty(checkmtlotmpping))
                    {
                        //kiểm tra xem mã đầu vào cột sts_share = N thì không insert w_materail_mapping_mms
                        var list_con = await _ITimsService.GetmaterialmappingStsShare(checkmtlotmpping);
                        foreach (var item in list_con)
                        {
                            if (item.sts_share == null)
                            {
                                int result = await _ITimsService.InsertMultiMaterialMppingTims(a.material_code, checkmtlotmpping, user, item.mt_cd);
                            }
                        }

                        //await _ITimsService.UpdateMultiWmaterialInfoMMS(checkmtlotmpping, user, id_actual);
                    }

                    var ds1 = await _ITimsService.FindOneWMaterialInfoTims(idReturn);
                    var mt_cd = a.material_code.Remove(a.material_code.Length - 12, 12);
                    int giatri = await _ITimsService.SumGroupQtyWMaterialInfo(mt_cd);

                    return Json(new { result = true, message = "", ds1 = ds1, gtri = giatri }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }

        //public async Task<ActionResult> insertw_material_mping(w_material_mapping a, string bb_no, string mt_cd, int id_actual)
        [HttpPost]
        public async Task<ActionResult> InsertMaterialMappingRollTims(MaterialMappingTIMS a, string bb_no, string mt_cd, int id_actual)
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

                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {
                    string user = Session["userid"] == null ? null : Session["userid"].ToString();
                    //var qr = _TIMSService.FindWMaterialInfoByMTQRCode(mt_cd);
                    var qr = await _ITimsService.FindOneWMaterialInfoLikeForRoll(mt_cd);
                    if(qr == null)
                    {
                        return Json(new { result = false, message = "Mã đầu ra này đã xuất ra kho." }, JsonRequestBehavior.AllowGet);
                    }
                    int checkmapping = await _ITimsService.CheckExistMaterialMappingById(mt_cd);

                    if (checkmapping > 0)
                    {
                        return Json(new { result = false, message = Constant.Other }, JsonRequestBehavior.AllowGet);
                    }
                    //var ds_bb = _TIMSService.FindAllBobbin_lct_histByBBNo(bb_no).ToList();
                    var ds_bb = await _ITimsService.FindAllBobbin_lct_histByBBNo(bb_no);
                    if (ds_bb == null)
                    {
                        return Json(new { result = false, message = "Container " + Constant.DataExist }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {

                        //var ds_bb_for = ds_bb;
                        //var mt = qr.Where(x => x.mt_cd == ds_bb_for.mt_cd && (x.mt_sts_cd == "008" || x.mt_sts_cd == "002")
                        //&& x.lct_cd.StartsWith("006") && (x.lct_sts_cd == "101") && (x.gr_qty > 0)
                        //&& (id_actual != x.id_actual || id_actual == x.id_actual && x.id_actual_oqc != null && x.id_actual_oqc != 0)).ToList();
                        string mtcd = ds_bb.mt_cd;
                        WMaterialnfo mt = new WMaterialnfo();
                        if (mtcd.Contains("STA") || mtcd.Contains("ROT"))
                        {
                            mt = await _ITimsService.FindOneWMaterialInfotypeRoll(mtcd);
                        }
                        else
                        {
                            mt = await _ITimsService.FindOneWMaterialInfotypeRollFromTIMS(mtcd);
                        }
                        if (mt == null)
                        {
                            return Json(new { result = false, message = "Đồ đựng này chưa được nhập ở kho TIMS" }, JsonRequestBehavior.AllowGet);
                        }
                        if (mt.id_actual == id_actual)
                        {
                            return Json(new { result = false, message = "Cùng một công đoạn không thể mapping" }, JsonRequestBehavior.AllowGet);
                        }
                        var id_lot = qr.id_actual;
                        var id_cd = mt.id_actual;
                        //var check_lot = db.w_actual.Find(id_lot);
                        //var check_cd = db.w_actual.Find(id_cd);
                        string check_lot = await _ITimsService.GetAtNoFromIdActual(id_lot);
                        //var check_cd = db.w_actual.Find(id_cd);
                        string check_cd = await _ITimsService.GetAtNoFromIdActual(id_cd);
                        var check_po = "";
                        if (check_lot == check_cd)
                        {
                            check_po = "OK";
                        }
                        if (check_po == "")
                        {
                            return Json(new { result = false, message = "Chọn Sai PO! Xin vui lòng chọn lại!" }, JsonRequestBehavior.AllowGet);
                        }

                        //a.mapping_dt = DateTime.Now.ToString("yyyyMMddHHmmss");
                        a.mapping_dt = DateTime.Now;
                        a.mt_no = mt.mt_no;
                        int checkmpp = await _ITimsService.CheckMaterialMappingTims(mt.mt_no, ds_bb.mt_cd, mt_cd);
                        //if ((!db.w_material_mapping.Any(x => x.mt_no == a.mt_no && x.mt_cd == ds_bb.mt_cd && x.mt_lot == mt_cd)) && (mt_cd != ds_bb.mt_cd))
                        if (checkmpp <= 0 && (mt_cd != ds_bb.mt_cd))
                        {
                            a.mt_cd = ds_bb.mt_cd;
                            a.mt_lot = mt_cd;
                            a.bb_no = ds_bb.bb_no;
                            a.use_yn = "Y";
                            //a.bb_no = qr.bb_no;
                            //a.bb_no = bb_no;
                            //a.remark = qr.FirstOrDefault().remark;
                            a.remark = "";
                            a.reg_id = user;
                            a.chg_id = user;
                            a.reg_date = DateTime.Now;
                            a.chg_date = DateTime.Now;
                            //int resultMapping = _TIMSService.InsertMaterialMapping(a);
                            int resultMapping = await _ITimsService.InsertMaterialMapping(a);
                            var id_fo1 = mt.wmtid;
                            if (mt.mt_no.Contains("STA") || mt.mt_no.Contains("ROT"))
                            {
                                mt.mt_sts_cd = "008";
                            }
                            else
                            {
                                mt.mt_sts_cd = "002";
                            }

                            await _ITimsService.UpdateMaterialStaffQty(mt.mt_sts_cd, user, mt.wmtid, id_actual);
                            //var update1 = _TIMSService.FindOneWMaterialInfo(id_fo1);
                            //update1.mt_sts_cd = "002";
                            //update1.id_actual = update1.id_actual;
                            //update1.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                            //int effectRows = _TIMSService.UpdateWMaterialInfoById(update1.mt_sts_cd, update1.chg_id, update1.id_actual, update1.wmtid);
                        }
                        var data = await _ITimsService.GetListMaterialMapping(mt_cd);
                        data = data.Where(s => s.mt_cd == a.mt_cd).ToList();
                        var result = new { result = true, message="Thành công", list = data, count_erro = 0 };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
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
        //public async Task<ActionResult> insertw_materialEA_mping(w_material_mapping a, Models.NewVersion.MaterialInfoMMS b, string bb_no, int id_actual, string staff_id, int psid)
        [HttpPost]
        public async Task<ActionResult> InsertMaterialMappingEATims(MaterialMappingTIMS a, Models.NewVersion.MaterialInfoTIMS b, string bb_no, int id_actual, string staff_id, int psid)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {
                    string CreateUser = Session["userid"] == null ? null : Session["userid"].ToString();
                    var ds_bb = await _ITimsService.FindOneDBobbinInfo(bb_no);

                    //var ds_mapping = await _ITimsService.FindOneBobbin_lct_hist(bb_no, null);

                    if (string.IsNullOrEmpty(staff_id))
                    {
                        return Json(new { result = false, message = Constant.CheckData + " Staff" }, JsonRequestBehavior.AllowGet);
                    }
                    if (ds_bb == null)
                    {
                        return Json(new { result = false, message = "Container Không tìm thấy!!!" }, JsonRequestBehavior.AllowGet);
                    }
                    //kiểm tra mấy mã lot cũ có đạt tiêu chuẩn k
                    string mtcd = ds_bb.mt_cd;
                    WMaterialnfo mt = new WMaterialnfo();
                    if (mtcd != null)
                    {
                        if (mtcd.Contains("STA") || mtcd.Contains("ROT"))
                        {
                            mt = await _ITimsService.FindOneWMaterialInfoLike(ds_bb.mt_cd);
                        }
                        else
                        {
                            mt = await _ITimsService.FindOneWMaterialInfoLikeTIMS(ds_bb.mt_cd);
                        }
                    }
                    else
                    {
                        return Json(new { result = false, message = "Đồ đựng này đã xã" }, JsonRequestBehavior.AllowGet);
                    }
                    if (mt != null)
                    {
                        //var check_thong_bao = mt;
                        if (mt.mt_sts_cd != "008" && mt.mt_sts_cd != "002" )
                        {
                            var trangthai =await checktrangthai(mt.mt_sts_cd);
                            return Json(new { result = false, message = "Trạng Thái đang là " + trangthai }, JsonRequestBehavior.AllowGet);
                        }
                        if (mt.gr_qty == 0)
                        {
                            return Json(new { result = false, message = "Sản Lượng Không có để Scan!!!" }, JsonRequestBehavior.AllowGet);
                        }
                        if (id_actual == mt.id_actual)
                        {
                            return Json(new { result = false, message = "Cùng Công Đoạn Vui Lòng Không Scan vào!!!" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { result = false, message = "Không đủ điều kiện để scan" }, JsonRequestBehavior.AllowGet);
                    }

                    //var data1 = _TIMSService.CheckShift(psid, id_actual);
                    var data1 = await _ITimsService.CheckShift(psid, id_actual);

                    if (data1 == 0)
                    {
                        return Json(new { result = false, message = Constant.EndShift }, JsonRequestBehavior.AllowGet);
                    }

                    var check_exit_vo = await _ITimsService.FindOneWActual(id_actual);

                    if (check_exit_vo == null)
                    {
                        return Json(new { result = false, message = "PO " + Constant.NotFound }, JsonRequestBehavior.AllowGet);
                    }

                    //tạo mã lot mới nhưng vẫn là container đó
                    DateTime dt1 = DateTime.Now; //Today at 00:00:00
                    string time_now = dt1.ToString("HHmmss");
                    string time_now1 = dt1.ToString("yyyyMMdd");
                    time_now = time_now + Guid.NewGuid().ToString("N").Substring(0, 3).ToUpper();
                    b.material_type = "CMT";
                    b.bb_no = bb_no;
                    b.real_qty = 0;
                    b.staff_id = staff_id;

                    //var get_primary = await _ITimsService.FindOneWActualPrimaryByAtNo(check_exit_vo.at_no);

                    b.mt_no = check_exit_vo.product + "-" + check_exit_vo.name;
                    var bien_first = b.mt_no + time_now1 + time_now;
                    //b.bbmp_sts_cd = "000";
                    b.status = "002";
                    b.location_code = "006000000000000000";
                    b.from_lct_code = "006000000000000000";
                    //b.lct_sts_cd = "101";
                    b.id_actual = id_actual;
                    b.active = true;
                    b.sts_update = "composite";
                    b.gr_qty = 0;
                    b.chg_date = DateTime.Now;
                    b.chg_id = CreateUser;
                    b.reg_id = CreateUser;
                    b.reg_date = DateTime.Now;
                    b.at_no = check_exit_vo.at_no;
                    b.product = check_exit_vo.product;
                    DateTime dt = DateTime.Now; //Today at 00:00:00
                    string input_dt = dt.ToString("yyyyMMddHHmmss");
                    //b.input_dt = input_dt;

                    var menuCd = string.Empty;

                    menuCd = string.Format("{0, 0:D3}", 1);
                    b.material_code = bien_first + menuCd;
                    b.staff_id = staff_id;

                    //string check_lot = await _ITimsService.GetAtNoFromIdActual(id_actual);

                    string check_cd = await _ITimsService.GetAtNoFromIdActual(mt.id_actual);
                    var check_po = "";
                    if (check_exit_vo.at_no == check_cd)
                    {
                        check_po = "OK";
                    }
                    if (check_po == "")
                    {
                        return Json(new { result = false, message = "Chọn Sai PO! Xin vui lòng chọn lại!" }, JsonRequestBehavior.AllowGet);
                    }
                    int idReturn = await _ITimsService.InsertMaterialInfoTIMMS(b);

                    a.mapping_dt = DateTime.Now;
                    a.mt_no = mt.mt_no;
                    int checkmpp = await _ITimsService.CheckMaterialMappingTims(mt.mt_no, ds_bb.mt_cd, b.material_code);
                    // kiểm tra xem đã có mã lot mới và lot cũ đã tồn tại mapping chưa

                    if (checkmpp <= 0 && b.material_code != ds_bb.mt_cd)
                    {
                        a.mt_cd = ds_bb.mt_cd;
                        a.mt_lot = b.material_code;
                        a.bb_no = ds_bb.bb_no;
                        a.use_yn = "Y";
                        a.bb_no = ds_bb.bb_no;
                        a.remark = "";
                        a.reg_id = CreateUser;
                        a.chg_id = CreateUser;
                        a.reg_date = DateTime.Now;
                        a.chg_date = DateTime.Now;

                        int result = await _ITimsService.InsertMaterialMapping(a);
                    }

                    var history = await _ITimsService.FindOneBobbin_lct_hist(bb_no, null);
                    if (history != null)
                    {
                        history.mt_cd = b.material_code;
                        int updatedHistory = await _ITimsService.UpdateMtCdBobbinHistInfo(history.mt_cd, history.blno);

                    }
                    else
                    {

                        var d_bobbin_lct_hist = new DBobbinLctHist();
                        //add vào bb_history
                        d_bobbin_lct_hist.bb_no = bb_no;
                        d_bobbin_lct_hist.bb_nm = ds_bb.bb_nm;
                        d_bobbin_lct_hist.mc_type = ds_bb.mc_type;
                        d_bobbin_lct_hist.mt_cd = b.material_code;
                        d_bobbin_lct_hist.use_yn = ds_bb.use_yn;
                        d_bobbin_lct_hist.chg_dt = DateTime.Now;
                        d_bobbin_lct_hist.chg_id = CreateUser;
                        d_bobbin_lct_hist.reg_dt = DateTime.Now;
                        d_bobbin_lct_hist.reg_id = CreateUser;
                        //_TIMSService.InsertBobbinHist(d_bobbin_lct_hist);
                        await _ITimsService.InsertBobbinHist(d_bobbin_lct_hist);
                    }
                    ds_bb.mt_cd = b.material_code;
                    ds_bb.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    await _ITimsService.UpdateBobbinInfo(ds_bb.chg_id, ds_bb.mt_cd, ds_bb.bno);

                    var ds1 = await _ITimsService.FindOneWMaterialInfoTims(idReturn);

                    //tra ve enumerable nen phai to list
                    return Json(new { result = true, message = "", ds1 = ds1 }, JsonRequestBehavior.AllowGet);
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
        [HttpGet]
        public async Task<ActionResult> Getfacline_qc_PhanLoai(string item_vcd, string mt_lot)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                var result = await _ITimsService.Getfacline_qc_PhanLoai(item_vcd, mt_lot);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = new List<Getfacline_qc_PhanLoaiReponse>();
                return Json(new { result = false, message = Constant.ErrorAuthorize, data }, JsonRequestBehavior.AllowGet);
            }

        }
        public async Task<ActionResult> Getfacline_qc(string item_vcd, string mt_cd, string mt_lot)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                var data = await _ITimsService.GetFaclineQC(item_vcd, mt_cd, mt_lot);
                if (data == null)
                {
                    return Json(new { result = false }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = true, data = data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = new List<FaclineQc>();
                return Json(new { result = false, message = Constant.ErrorAuthorize, data }, JsonRequestBehavior.AllowGet);
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
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                var result = await _ITimsService.Getfacline_qc_value(fq_no);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = new List<FaclineQCValueResponse>();
                return Json(new { result = false, message = Constant.ErrorAuthorize, data }, JsonRequestBehavior.AllowGet);
            }

        }
        //public async Task<ActionResult> getmt_date_web_auto(int id_actual, int psid)
        public async Task<ActionResult> GetListMaterialWebAuto(int id_actual, int psid)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                var check_vo = await _ITimsService.FindOneWActual(id_actual);

                //var check_stff = _TIMSService.FindOneDProUnitStaffById(psid);
                var check_stff = await _ITimsService.FindOneDProUnitStaffById(psid);
                if (check_stff == null)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                //StringBuilder sql = new StringBuilder();
                if (check_vo.name == "OQC")
                {
                    var result = await _ITimsService.FindAllWMaterialInfoByStaffIdOQC(check_stff.staff_id, id_actual, check_stff.start_dt, check_stff.end_dt);
                    return Json(new { result = true, data = result }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var results = await _ITimsService.FindAllWMaterialInfoByStaffId(check_stff.staff_id, id_actual, check_stff.start_dt.Substring(0, check_stff.start_dt.Length - 4), check_stff.end_dt.Substring(0, check_stff.end_dt.Length - 4));

                    return Json(results.OrderByDescending(x => x.reg_dt), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var results = new List<WMaterialnfo>();
                return Json(results, JsonRequestBehavior.AllowGet);
            }

        }
        public async Task<ActionResult> Xoa_mt_pp_composite(int id)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {

                    if (id != 0)
                    {
                        string userid = Session["userid"] == null ? null : Session["userid"].ToString();
                        //string sql = @"select * from w_material_info where wmtid = @1";
                        //Models.MaterialInfoMMS a = db.Database.SqlQuery<Models.MaterialInfoMMS>(sql, new MySqlParameter("@1", id)).FirstOrDefault();
                        var a = await _ITimsService.FindWMaterialInfoByWmtid(id);
                        //kiem tra chua qua cong doan
                        if (a != null)
                        {
                            //bool check = _TIMSService.CheckExistMaterialMappingById(a.material_code);
                            int check = await _ITimsService.CheckExistMaterialMappingById(a.material_code);
                            if (check > 0)
                            {
                                return Json(new { result = false, message = Constant.Exist + "mapping" }, JsonRequestBehavior.AllowGet);
                            }
                            if (a.gr_qty > 0 || a.real_qty > 0)
                            {
                                return Json(new { result = false, message = Constant.CannotDelete }, JsonRequestBehavior.AllowGet);
                            }

                            //kiểm tra hàng đã kiểm rồi
                            var check_facline = await _ITimsService.CheckCountExistFacline(a.material_code);
                            //var check_facline = await _ITimsService.GetSumQtyFacline(a.material_code);
                            if (check_facline > 0)
                            {
                                return Json(new { result = false, message = Constant.QCPass }, JsonRequestBehavior.AllowGet);
                            }
                            var mt_cd = a.material_code.Remove(a.material_code.Length - 12, 12);
                            //var ds_mapping = db.w_material_mapping.Where(x => x.mt_lot == a.mt_cd).ToList();
                            //foreach (var item in ds_mapping)
                            //{
                            //    w_material_mapping b = db.w_material_mapping.Find(item.wmmid);
                            //    db.Entry(b).State = EntityState.Deleted;
                            //}
                            //Thắc mắc sao xóa 1 caí
                            await _ITimsService.DeleteWMaterialMappingWithMtLot(a.material_code);
                            //db.Entry(a).State = EntityState.Deleted;
                            await _ITimsService.RemoveMaterialInfoTims(a.wmtid);
                            //await _ITimsService.UpdateUseYnMaterialIndfowithMtCd("0", userid, a.material_code);
                            await _ITimsService.UpdateBobbinInfowitbbno(a.bb_no, userid);
                            //var ds = db.w_material_info.Where(x => x.mt_cd.StartsWith(mt_cd)).ToList().Sum(x => x.gr_qty);
                            var ds = await _ITimsService.SumGroupQtyWMaterialInfo(mt_cd);

                            //xã bobin
                            //var find_bb = db.d_bobbin_info.Where(x => x.bb_no == a.bb_no).SingleOrDefault();
                            //var find_bb = db.d_bobbin_info.Where(x => x.bb_no == a.bb_no).SingleOrDefault();
                            //if (find_bb != null)
                            //{
                            //    find_bb.mt_cd = "";
                            //    find_bb.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                            //    db.Entry(find_bb).State = EntityState.Modified;
                            //}
                            //await _ITimsService.UpdateBobbinInfowitbbno(a.bb_no,userid);
                            ////xóa history
                            //var find_history = db.d_bobbin_lct_hist.Where(x => x.bb_no == a.bb_no && x.mt_cd == a.mt_cd).SingleOrDefault();
                            //if (find_history != null)
                            //{
                            //    db.Entry(find_history).State = EntityState.Deleted;
                            //}
                            //db.SaveChanges(); // line that threw exception
                            await _ITimsService.DeleteDBobbinLctHistforDevice(a.material_code, a.bb_no);
                            return Json(new { result = true, message = "", ds = ds }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    return Json(new { result = false, message = "Công đoạn đã được xóa hoặc không tồn tại!" }, JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        public async Task<ActionResult> CancelEA(int id, int psid)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {

                if (id > 0)
                {
                    //Tìm ID của mã đầu ra
                    var IdOutput = await _ITimsService.GetWMaterialInfoTIMS(id);
                    if (IdOutput == null)
                    {
                        return Json(new { result = false, message = Constant.ErrorCancelEASearch }, JsonRequestBehavior.AllowGet);
                    }
                    //Kiểm tra đã hết ca chưa
                    var data = await _ITimsService.GetListStaff(IdOutput.id_actual, psid);
                    if (data.Count() < 1)
                    {
                        return Json(new { result = false, message = Constant.EndShift + "Bạn không thể xóa!!!" }, JsonRequestBehavior.AllowGet);
                    }
                    //Kiểm tra mã đầu ra đã sang công đoạn khác chưa
                    var checkMaterialMapping = _ITimsService.CheckMaterialMappingTIMS(IdOutput.material_code);
                    if (checkMaterialMapping.Count() > 0)
                    {
                        return Json(new { result = false, message = Constant.Other }, JsonRequestBehavior.AllowGet);
                    }
                    //kiểm tra hàng đã kiểm rồi
                    var check_facline = await _ITimsService.CheckCountExistFacline(IdOutput.material_code);
                    if (check_facline > 0)
                    {
                        return Json(new { result = false, message = Constant.QCPass }, JsonRequestBehavior.AllowGet);
                    }
                    if (IdOutput != null)
                    {
                        //Nếu số lượng lớn hơn không tức là mã đầu ra đã nhập sản lượng , không cho trở về
                        if (IdOutput.gr_qty > 0 || IdOutput.real_qty > 0)
                        {
                            return Json(new { result = false, message = Constant.Tested }, JsonRequestBehavior.AllowGet);
                        }
                        //bobbin đang chưa mã đầu ra
                        var find_bb = await _ITimsService.GetBobbinInfo(IdOutput.bb_no);
                        if (find_bb == null)
                        {
                            return Json(new { result = false, message = Constant.NotFound + " Container" }, JsonRequestBehavior.AllowGet);
                        }
                        //chỗ này thắc mắc dạng EA đầu vào khi nào cũng 1 hàng thì cần gì foreach
                        var ds_mapping = await _ITimsService.GetMaterialMappingTIMSByLotCancel(IdOutput.material_code);
                        foreach (var item in ds_mapping)
                        {
                            //kiểm tra mã đầu vào trạng thái gì, nếu sts_share = N thì return
                            var check_exits_Input = await _ITimsService.GetMaterialMappingTimsById(item.wmmid);
                            if (check_exits_Input.sts_share == "N")
                            {
                                return Json(new { result = false, message ="Mã đầu vào đã bị dừng kế thừa." }, JsonRequestBehavior.AllowGet);
                            }
                            //kiểm tra mã đầu đã qua công đoạn khác chưa
                            var check_exits_mapping =  _ITimsService.CheckMaterialMappingTIMS(check_exits_Input.mt_lot);
                            if (check_exits_mapping.Count() > 0)
                            {
                                return Json(new { result = false, message = Constant.Other }, JsonRequestBehavior.AllowGet);
                            }

                            //Xóa mã đầu vào trong bảng w_material_mapping_tims
                            await _ITimsService.RemoveMaterialMappingsTims(check_exits_Input.wmmid);

                            //Nếu đầu vào đã add bobbin thì xả luôn, sau đó mới add đồ đựng ban đầu vào
                            var CheckBobbinDauVao = await _ITimsService.GetBobbinInfoByMaterialCode(item.mt_cd);
                            if (CheckBobbinDauVao != null)
                            {
                                //XẢ bảng d_bobbin_info
                                await _ITimsService.UpdatePartialBobbinInfo(item.mt_cd);
                                //Xả bảng d_bobbin_lct_history
                                await _ITimsService.DeleteDBobbinLctHistforDevice(item.mt_cd, CheckBobbinDauVao.bb_no);

                                //update mã bobbin cho mã đầu vào nếu là STA/Rot thì update bảng w_material_info_mms /ngược lại w_material_info_tims
                                if (item.mt_cd.Contains("STA") || item.mt_cd.Contains("ROT"))
                                {
                                    var checkMaterailMMS = await _ITimsService.GetWMaterialInfoMMS(item.mt_cd);

                                    await _ITimsService.UpdateMaterialBBMMS(checkMaterailMMS.wmtid, IdOutput.bb_no);
                                }
                                else
                                {
                                    var checkMaterailTIMS = await _ITimsService.FindOneWMaterialInfoLikeForRoll(item.mt_cd);

                                    await _ITimsService.UpdateMaterialBBTIMS(checkMaterailTIMS.wmtid, IdOutput.bb_no);
                                }

                            }

                            //add history trở về nguyên trạng thái ban đầu
                            //add vào bb_history
                            var history = new BobbinLctHist();
                            var checkBobbinLctHist = await _ITimsService.GetListDataBobbinLctHist(find_bb.bb_no, item.mt_cd);
                            if (checkBobbinLctHist.Count() < 1)
                            {
                                history.bb_no = find_bb.bb_no;
                                history.bb_nm = find_bb.bb_nm;
                                history.mc_type = find_bb.mc_type;
                                history.mt_cd = item.mt_cd;
                                history.use_yn = find_bb.use_yn;
                                history.chg_dt = DateTime.Now;
                                history.chg_id = Session["userid"] == null ? "Mob" : Session["userid"].ToString();
                                history.reg_dt = DateTime.Now;
                                history.reg_id = Session["userid"] == null ? "Mob" : Session["userid"].ToString();

                                //thiếu await
                                int resss = _ITimsService.InsertIntoBobbinLctHist(history);

                            }
                            find_bb.mt_cd = item.mt_cd;
                            find_bb.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                            await _ITimsService.UpdateBobbinInfo(find_bb);
                        }

                        //xóa history
                        var listBobbinLctHists = await _ITimsService.GetListDataBobbinLctHist(IdOutput.bb_no, IdOutput.material_code);
                        var find_history = listBobbinLctHists.SingleOrDefault();

                        if (find_history != null)
                        {
                            await _ITimsService.RemoveBobbinLctHist(find_history.blno);
                        }
                        await _ITimsService.RemoveMaterialInfoTims(IdOutput.wmtid);

                        return Json(new { result = true, message = "Trở về thành công !" }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { result = false, message = Constant.CannotCancel }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }
        //public async Task<ActionResult> ds_mapping_w(string mt_cd)
        public async Task<ActionResult> GetListMaterialMappingwTims(string mt_cd)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {

                    var kq = await _ITimsService.GetListMaterialMapping(mt_cd);
                    return Json(kq, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception e)
            {
                Console.WriteLine("Disconected");
                return Json(e, JsonRequestBehavior.AllowGet);
            }
        }
        //public JsonResult Finish_back(int wmmid)
        public async Task<JsonResult> FinishBack(int wmmid)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {

                    //không sử dụng nữa cho về flase
                    if (wmmid > 0)
                    {
                        return Json(new { result = false, message = "Chức năng này đã bị hủy" }, JsonRequestBehavior.AllowGet);
                    }
                    string userid = Session["userid"] == null ? null : Session["userid"].ToString();

                    var data = await _ITimsService.GetMaterialMappingTimsById(wmmid);
                    if (data.use_yn == "Y")
                    {


                        var ds = await _ITimsService.GetMaterialInfoMMSByOrgin(data.mt_cd);
                        //ds = ds.First();
                        if (ds.Count() > 0)
                        {
                            data.use_yn = "N";
                            data.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                            await _ITimsService.UpdateUseYnMaterialMapping(data.use_yn, data.chg_id, data.wmmid);


                            var id_fo1 = ds.FirstOrDefault().wmtid;

                            var update1 = await _ITimsService.GetListDataMaterialInfoTIMSBywmtid(ds.FirstOrDefault().wmtid);
                            update1.status = "005";
                            update1.chg_id = userid;

                            await _ITimsService.UpdateStatusWMaterialInfoById("005", userid, ds.FirstOrDefault().wmtid);
                            if (!string.IsNullOrEmpty(update1.bb_no))
                            {
                                //xã bobin

                                var check_bb = await _ITimsService.FindOneDBobbinInfomtcd(update1.bb_no, update1.mt_cd);
                                if (check_bb != null)
                                {
                                    //find history để xóa _TIMSService
                                    await _ITimsService.DeleteDBobbinLctHistforDevice(update1.bb_no, update1.mt_cd);

                                    await _ITimsService.UpdateBobbinInfowitbbno(check_bb.bb_no, userid);

                                }
                            }
                            var check_finish = await _ITimsService.FindAllWMaterialMappingByMtcdUseYn(update1.mt_cd, "Y");
                            foreach (var item in check_finish)
                            {

                                await _ITimsService.UpdateUseYnMaterialMapping("N", userid, item.wmmid);


                            }

                            return Json(new { result = true, message = Constant.EndLotSuccess, use_yn = data.use_yn }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        //kiem tra da mapping voi cong doan khac chua


                        //var check_exit2 = _TIMSService.FindAllMaterialMappingByMtCd(data.mt_lot);
                        var check_exit2 = _ITimsService.CheckMaterialMappingTIMS(data.mt_lot);
                        if (check_exit2.Count() > 0)
                        {

                            return Json(new { result = false, message = Constant.Other }, JsonRequestBehavior.AllowGet);
                        }

                        var ds = await _ITimsService.FindAllMaterialByMtCdLike(data.mt_cd);
                        if (ds.Any(x => x.mt_cd.StartsWith(x.orgin_mt_cd + "-MG")))
                        {
                            return Json(new { result = false, message = Constant.MergeCodeCannotCancel, use_yn = data.use_yn }, JsonRequestBehavior.AllowGet);
                        }
                        if (ds.Count == 1)
                        {
                            //trở về trạng thái đầu

                            data.use_yn = "Y";
                            data.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();

                            await _ITimsService.UpdateUseYnMaterialMapping("Y", userid, data.wmmid);

                            var id_fo1 = ds.FirstOrDefault().wmtid;

                            var update1 = await _ITimsService.GetWMaterialInfoTIMS(id_fo1);
                            update1.status = "002";
                            update1.chg_id = userid;

                            await _ITimsService.UpdateStatusWMaterialInfoById(update1.status, update1.chg_id, update1.wmtid);
                            if (!string.IsNullOrEmpty(update1.bb_no))
                            {
                                //xã bobin


                                var check_bb = await _ITimsService.FindOneDBobbinInfo(update1.bb_no);
                                if (check_bb != null)
                                {

                                    check_bb.chg_id = userid;
                                    check_bb.mt_cd = update1.mt_cd;

                                    await _ITimsService.UpdateBobbinInfo(check_bb.chg_id, check_bb.mt_cd, check_bb.bno);
                                    //add history đã xóa

                                    var listbobin = await _ITimsService.FindOneBobbin_lct_hist(check_bb.bb_no, data.mt_cd);

                                    if (listbobin == null)
                                    {
                                        var history = new DBobbinLctHist();
                                        history.bb_no = check_bb.bb_no;
                                        history.bb_nm = check_bb.bb_nm;
                                        history.mt_cd = data.mt_cd;
                                        history.mc_type = check_bb.mc_type;
                                        history.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                        history.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                        history.reg_dt = DateTime.Now;
                                        history.chg_dt = DateTime.Now;
                                        history.del_yn = "N";
                                        history.use_yn = "Y";

                                        await _ITimsService.InsertBobbinHist(history);
                                    }
                                }
                            }

                            return Json(new { result = true, message = Constant.BackSuccess, use_yn = data.use_yn }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                        }
                    }
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
        public async Task<JsonResult> StopCp(int wmmid, int psid)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {
                    //kiểm tra coi n đã hết thời gian làm việc chưa
                    int ca = await _ITimsService.CheckShift(psid);
                    if (ca == 0)
                    {
                        return Json(new { result = false, message = Constant.EndShift }, JsonRequestBehavior.AllowGet);
                    }
                    var data = await _ITimsService.FindOneWMaterialMappingById(wmmid);
                    data.sts_share = "N";
                    await _ITimsService.UpdateWMaterialMappingById(data.sts_share, wmmid);
                    return Json(new { result = true, message = Constant.Success }, JsonRequestBehavior.AllowGet);
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
        public async Task<JsonResult> savereturn_lot(int soluong, string mt_cd, string mt_lot)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {
                    string userid = Session["userid"] == null ? null : Session["userid"].ToString();
                    //var check_exit = db.w_material_info.Where(x => x.mt_cd == mt_cd).SingleOrDefault();
                    var check_exit = _ITimsService.FindWMaterialInfoByMTQRCode(mt_cd);
                    if (check_exit == null)
                    {
                        return Json(new { result = false, message = "MT Code DONT Exits" }, JsonRequestBehavior.AllowGet);
                    }
                    //var check_exit1 = db.w_material_mapping.Where(x => x.mt_lot == mt_lot && x.mt_cd == mt_cd).SingleOrDefault();
                    var check_exit1 = await _ITimsService.Getmaterialmappingreturn(mt_lot, mt_cd);
                    if (check_exit1 == null)
                    {
                        return Json(new { result = false, message = "MT LOT DONT Exits" }, JsonRequestBehavior.AllowGet);
                    }
                    //kiểm tra đã finish chưa
                    if (check_exit1.use_yn == "N")
                    {
                        return Json(new { result = false, message = "MT LOT Already Finish" }, JsonRequestBehavior.AllowGet);
                    }

                    if (soluong == 0)
                    {
                        return Json(new { result = false, message = "Length is not valid" }, JsonRequestBehavior.AllowGet);
                    }

                    //kiểm tra số lượng có vượt quá k

                    if (soluong > check_exit.gr_qty)
                    {
                        return Json(new { result = false, message = "Length is not valid" }, JsonRequestBehavior.AllowGet);
                    }

                    //tách số lượng
                    var soluongcl_mt_cd = check_exit.gr_qty - (soluong);
                    //finish
                    check_exit1.use_yn = "N";
                    check_exit1.chg_id = userid;
                    await _ITimsService.UpdateUseYnMaterialMapping("Y", userid, check_exit1.wmmid);
                    //db.Entry(check_exit1).State = EntityState.Modified;
                    //db.SaveChanges();
                    // chuyển trạng thái về 005 và update số lượng đã sử dụng
                    check_exit.status = "005";
                    check_exit.gr_qty = soluongcl_mt_cd;
                    check_exit.chg_id = userid;
                    await _ITimsService.UpdateGrqtyMaterialInfoTims(check_exit.material_code, soluongcl_mt_cd, "005", userid);
                    var countreturn = await _ITimsService.GetListDataMaterialInfoTIMS(mt_cd);
                    int count_return = countreturn.Count();

                    await _ITimsService.InsertWMaterialInfoTims(count_return + 1, soluong, userid, mt_cd);
                    if (string.IsNullOrEmpty(check_exit.bb_no))
                    {
                        //xã bobin
                        //var check_bb = db.d_bobbin_info.Where(x => x.bb_no == check_exit.bb_no && x.mt_cd == check_exit.mt_cd).ToList().SingleOrDefault();
                        var check_bb = await _ITimsService.FindOneBobbin_lct_hist(check_exit.bb_no, check_exit.material_code);
                        if (check_bb != null)
                        {
                            check_bb.chg_id = userid;
                            check_bb.mt_cd = mt_cd + "-" + "RT" + (count_return + 1);
                            await _ITimsService.UpdateBobbinInfo(check_bb.chg_id, check_bb.mt_cd, check_bb.blno);
                            var history = await _ITimsService.GetListDataBobbinLctHist(check_bb.bb_no, mt_cd);
                            if (history.Count() > 0)
                            {
                                history.FirstOrDefault().mt_cd = check_bb.mt_cd;
                                await _ITimsService.UpdateMtCdBobbinHistInfo(history.FirstOrDefault().mt_cd, history.FirstOrDefault().blno);
                            }
                        }
                    }
                    //kiem tra co bao nhieu mt_cd da qua cong doan ma chua finish

                    await _ITimsService.UpdateMaterialmappingwithmtcd(check_exit.material_code, userid, false, "N");
                    var data = await _ITimsService.GetListMaterialMapping(mt_lot);

                    return Json(new { result = true, message = "Success", kq = data }, JsonRequestBehavior.AllowGet);
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
        public async Task<JsonResult> Cancel_mapping(int wmmid, int psid)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {
                    //Kiểm tra mã đầu vào đã finish chưa BẢNG w_material_mapping_tims
                    var check_exits = await _ITimsService.GetMaterialMappingTimsById(wmmid);
                    if (check_exits.sts_share == "N")
                    {
                        return Json(new { result = false, message = Constant.Divide }, JsonRequestBehavior.AllowGet);
                    }
                    //KIỂM TRA MÃ ĐẦU RA TỒN TẠI KHÔNG
                    var find_lot = await _ITimsService.GetWMaterialInfowithmtcd(check_exits.mt_lot);
                    if (check_exits == null || find_lot == null)
                    {
                        return Json(new { result = false, message = "MT Code " + Constant.NotFound }, JsonRequestBehavior.AllowGet);
                    }
                    //KIỂM TRA MÃ ĐẦU RA ĐÃ QUA CÔNG ĐOẠN KHAC CHƯA
                    var check_exits_mapping = _ITimsService.CheckMaterialMappingTIMS(check_exits.mt_lot);
                    if (check_exits_mapping.Count() > 0)
                    {
                        return Json(new { result = false, message = Constant.Other }, JsonRequestBehavior.AllowGet);
                    }

                    //KIỂM TRA CÔNG NHÂN TẠI CÔNG ĐOẠN NÀY CÒN THỜI GIAN LÀM VIỆC KHÔNG
                    var data = await _ITimsService.GetListStaff(find_lot.id_actual, psid);
                    if (data.Count() < 1)
                    {
                        return Json(new { result = false, message = Constant.EndShift }, JsonRequestBehavior.AllowGet);
                    }

                    //kIỂM TRA CUỘN ĐẦU VÀO ĐÃ QUA CÔNG ĐOẠN KHÁC CHƯA TRƯỜNG HỢP NÓ CÒN DƯA
                    var check_mapping1 =await _ITimsService.GetListDataMaterialMappingTIMSById(check_exits.mt_cd, check_exits.mt_lot);
                    foreach (var item in check_mapping1)
                    {
                        //so sánh thời gian mapping
                        int compareTime = DateTime.Compare(item.mapping_dt, check_exits.mapping_dt);
                        if (compareTime > 0)
                        {
                            return Json(new { result = false, message = Constant.Other }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    //kiểm tra đã check QC chưa và xóa

                    var check_facline = await _ITimsService.CheckCountExistFaclineRoll(check_exits.mt_cd, check_exits.mt_lot);
                    if (check_facline > 0)
                    {
                        return Json(new { result = false, message = Constant.QCPass }, JsonRequestBehavior.AllowGet);
                    }
                    //Xóa đầu vào ra khỏi bảng mapping
                    await _ITimsService.RemoveMaterialMappingsTims(check_exits.wmmid);
                    return Json(new { result = true, message = Constant.CancelSuccess }, JsonRequestBehavior.AllowGet);
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
        public async Task<ActionResult> PartialView_View_QC_WEB(string item_vcd)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                //lấy hết tất cả qc_itemcheck_mt

                var qc_itemcheck_mt2 = new List<QCItemCheck_Mt_Model>();
                var qc_itemcheck_mt = await _ITimsService.GetQCItemCheckMaterialwithitemvcd(item_vcd);



                foreach (var item in qc_itemcheck_mt)
                {
                    //var view_qc_Model = new List<QCItemCheck_Mt_Model>();

                    var qc_itemcheck_dt = await _ITimsService.GetQCItemCheckMaterialDetailwithcheckcd(item_vcd, item.qc_itemcheck_mt__check_id);
                    if (qc_itemcheck_dt.Count() > 0)
                    {
                        item.view_qc_Model = qc_itemcheck_dt.ToList();
                        qc_itemcheck_mt2.Add(item);

                    }
                }

                return PartialView(qc_itemcheck_mt2);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> Insert_w_product_qc(string item_vcd, int check_qty, int ok_qty, string mt_cd,
            string mt_lot, int remain, int check_qty_error, int psid, m_facline_qc MFQC, m_facline_qc_value MFQCV)
        {
            #region insert info
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {
                    //kiem tra check co dung so luong k
                    //kiểm tra coi n đã hết thời gian làm việc chưa
                    if (check_qty < ok_qty)
                    {
                        return Json(new { result = false, message = "Vui lòng kiểm tra lại sản lượng của bạn nhập !!!" }, JsonRequestBehavior.AllowGet);
                    }
                    if (remain < 0)
                    {
                        return Json(new { result = false, message = "Remain number phải là số dương!" }, JsonRequestBehavior.AllowGet);
                    }
                    var userAcount = Session["userid"] == null ? null : Session["userid"].ToString();
                    var userWIP = "";
                    if (!string.IsNullOrEmpty(userAcount))
                    {
                        var dsMbInfo = await _IActualWOService.GetMbInfoGrade(userAcount);
                        userWIP = dsMbInfo.grade;
                    }

                    int dem = await _ITimsService.CheckShiftWithPisd(psid);

                    if (dem == 0 && userWIP != "Admin")
                    {
                        return Json(new { result = false, message = Constant.EndShift }, JsonRequestBehavior.AllowGet);
                    }

                    MaterialInfoTimsDTO check_mtcd = new MaterialInfoTimsDTO();
                    if (mt_cd.Contains("STA") || mt_cd.Contains("ROT"))
                    {
                        check_mtcd = await _ITimsService.GetWMaterialInfowithmtcdNewMMS(mt_cd);
                    }
                    else
                    {
                        check_mtcd = await _ITimsService.GetWMaterialInfowithmtcdNew(mt_cd);
                    }
                    if (check_mtcd == null)
                    {
                        return Json(new { result = false, message = "MT Code " + Constant.NotFound }, JsonRequestBehavior.AllowGet);
                    }
                    if (string.IsNullOrEmpty(mt_lot))
                    {
                        return Json(new { result = false, message = "Composite " + Constant.NotFound }, JsonRequestBehavior.AllowGet);
                    }

                    //kiem tra mapping đo chua
                    var check_mapping2 = await _ITimsService.Getmaterialmappingreturn(mt_cd, mt_lot);

                    var bien = "";

                    var check_lot = await _ITimsService.GetWMaterialInfowithmtcdTims(mt_lot);
                    if (check_lot == null && userWIP != "Admin")
                    {
                        return Json(new { result = false, message = "Lot không tìm thấy!!!" }, JsonRequestBehavior.AllowGet);
                    }
                    //kiểm tra lấy PO
                    var check_po = await _ITimsService.GetWActual(check_lot.id_actual);

                    //var check_pr = await _ITimsService.GetwactualprimaryFratno(check_po.at_no);
                    var ca = "";
                    var ca_ngay = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 08:00:00"));
                    var ca_dem = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 20:00:00"));
                    var check_again = await _ITimsService.Getmfaclineqc(mt_cd, "TI", mt_lot);

                    DateTime reg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
                    DateTime chg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
                    var defec_t = 0;
                    int id_actual = check_lot.id_actual;
                    var trang_thai = 0;
                    string NameActual = await _ITimsService.getNameActual(id_actual);
                    var unitStaff = await _ITimsService.FindOneDProUnitStaffById(psid);
                    if (check_again.Count() > 0)
                    {
                        trang_thai = 1;
                        var fq = check_again.SingleOrDefault();
                        int checkexistmpp = await _ITimsService.CheckExistMaterialMappingById(fq.ml_tims);
                        if (checkexistmpp > 0 && userWIP != "Admin")
                        {
                            return Json(new { result = false, message = "Lot đã qua công đoạn khác " }, JsonRequestBehavior.AllowGet);
                        }
                        if (check_lot.mt_sts_cd == "005")
                        {
                            return Json(new { result = false, message = "Mã đầu ra  đã được kết thúc !!! " }, JsonRequestBehavior.AllowGet);
                        }
                        defec_t = Convert.ToInt32(fq.ng_qty);

                        if (ca_ngay <= fq.reg_dt && fq.reg_dt <= ca_dem)
                        {
                            ca = "CN";
                        }
                        else
                        {
                            ca = "CD";
                        }
                        bien = check_po.at_no + "-" + check_po.product + "-" + ca + "-TIMS-NG";

                        //tru Mã Ng đã tạo ra trc đó
                        var ma_NG = await _ITimsService.GetWMaterialInfowithmtcd(bien);
                        if (ma_NG != null)
                        {
                            //int grQty = ((int)ma_NG.gr_qty - defec_t) + (check_qty - ok_qty);
                            int grQty = ((int)ma_NG.gr_qty - defec_t) + (check_qty_error);
                            //int realQty = ((int)ma_NG.real_qty - defec_t) + (check_qty - ok_qty);
                            int realQty = ((int)ma_NG.real_qty - defec_t) + (check_qty_error);
                            await _ITimsService.UpdateGroupQTYRealQTYTims(grQty, realQty, bien, userAcount);
                        }
                        //update bảng chính m_facline_qc
                        fq.check_qty = check_qty;
                        fq.ok_qty = ok_qty;

                        await _ITimsService.UpdateFaclineQty(fq.check_qty, fq.ok_qty, fq.fqno, userAcount, check_qty_error, remain);
                        //update số lượng đầu ra
                        int kqqtyfq = await _ITimsService.GetSumQtyFacline(mt_lot);
                        await _ITimsService.UpdateGroupQTYRealQTYTims(kqqtyfq, kqqtyfq, mt_lot, userAcount);

                        //thay doi san luong dau vao neu nhu co remain
                        //neu co remain
                        int reamain_int = remain;

                        var bobin_xa = await _ITimsService.GetdbobbinlcthistFrbbno(check_mtcd.bb_no);
                        //var find_bb = _IWOService.GetBobbinInfo(check_mtcd.bb_no);
                        var find_bb = await _ITimsService.GetBobbinInfo(check_mtcd.bb_no);

                        // db.Entry(fq).State = EntityState.Modified;
                        if (reamain_int > 0)
                        {
                            //update lại sản lượng của đầu vào nếu có remain
                            //NẾU MÃ ĐẦU VÀO LÀ STA OR ROT THÌ STATUS = 008
                            if (check_mtcd.mt_no.Contains("STA") || check_mtcd.mt_no.Contains("ROT"))
                            {
                                check_mtcd.mt_sts_cd = "008";
                            }
                            else
                            {
                                check_mtcd.mt_sts_cd = "002";
                            }
                            check_mtcd.gr_qty = reamain_int;
                            //check_mtcd.mt_sts_cd = "002";
                            if (check_mtcd.mt_cd.Contains("STA") || check_mtcd.mt_cd.Contains("ROT"))
                            {
                                await _ITimsService.UpdateRemainQTYmms(check_mtcd.gr_qty, check_mtcd.mt_sts_cd, check_mtcd.mt_cd, userAcount);
                            }
                            else
                            {
                                await _ITimsService.UpdateRemainQTYTims(check_mtcd.gr_qty, check_mtcd.mt_sts_cd, check_mtcd.mt_cd, userAcount);
                            }
                            //kiem tra do dung da được sử dụng chưa
                            if (bobin_xa == null)
                            {
                                //insert lại nha
                                DBobbinLctHist d_bobbin_lct_hist = new DBobbinLctHist();
                                d_bobbin_lct_hist.bb_no = check_mtcd.bb_no;
                                d_bobbin_lct_hist.bb_nm = find_bb.bb_nm;
                                d_bobbin_lct_hist.mc_type = check_mtcd.mc_type;
                                d_bobbin_lct_hist.mt_cd = check_mtcd.mt_cd;
                                d_bobbin_lct_hist.use_yn = "Y";
                                d_bobbin_lct_hist.del_yn = "N";
                                d_bobbin_lct_hist.reg_dt = DateTime.Now;
                                d_bobbin_lct_hist.chg_dt = DateTime.Now;
                                d_bobbin_lct_hist.reg_id = userAcount;
                                d_bobbin_lct_hist.chg_id = userAcount;


                                await _ITimsService.InsertBobbinHist(d_bobbin_lct_hist);
                                find_bb.mt_cd = check_mtcd.mt_cd;
                                find_bb.chg_id = userAcount;
                                await _IcommonService.UpdateBobbinInfo(find_bb.bb_no, check_mtcd.mt_cd, userAcount, DateTime.Now);

                            }
                            //maping đổi về trạng thái
                            check_mapping2.use_yn = "Y";
                            await _ITimsService.UpdateUseYnMaterialMapping(check_mapping2.use_yn, check_mapping2.wmmid, userAcount);
                        }
                        else
                        {
                            //xa do dung
                            //xóa bobin info
                            //update lại sản lượng của đầu vào nếu có remain=0
                            check_mtcd.gr_qty = reamain_int;
                            check_mtcd.mt_sts_cd = "005";
                            if (check_mtcd.mt_cd.Contains("STA") || check_mtcd.mt_cd.Contains("ROT"))
                            {
                                await _ITimsService.UpdateRemainQTYmms(check_mtcd.gr_qty, check_mtcd.mt_sts_cd, check_mtcd.mt_cd, userAcount);
                            }
                            else
                            {
                                await _ITimsService.UpdateRemainQTYTims(check_mtcd.gr_qty, check_mtcd.mt_sts_cd, check_mtcd.mt_cd, userAcount);
                            }
                            await _ITimsService.DeleteDBobbinLctHistforDevice(mt_cd, check_mtcd.bb_no);
                            await _ITimsService.UpdateBobbinInfowithmtcd(mt_cd, check_mtcd.bb_no, userAcount);
                            check_mapping2.use_yn = "N";
                            int ress = await _ITimsService.UpdateUseYnMaterialMapping(check_mapping2.use_yn, userAcount, check_mapping2.wmmid);
                        }
                        check_po.defect = await _ITimsService.GetDefactActual((int)check_po.id_actual);

                        await _ITimsService.UpdateDefectActual(check_po.defect, check_po.id_actual);


                        if (NameActual == "OQC")
                        {
                            int ActualOQC = await _ITimsService.getActualrealOQC(id_actual, unitStaff.staff_id, unitStaff.start_dt.Substring(0, unitStaff.start_dt.Length - 4), unitStaff.end_dt.Substring(0, unitStaff.end_dt.Length - 4));
                            await _ITimsService.updateQtyOqcforStaff(id_actual, ActualOQC, psid);
                        }
                        else
                        {
                            int ActualQty = await _ITimsService.getActualRealQTY(id_actual, unitStaff.staff_id, unitStaff.start_dt.Substring(0, unitStaff.start_dt.Length - 4), unitStaff.end_dt.Substring(0, unitStaff.end_dt.Length - 4));
                            int ActualDefectQty = await _ITimsService.getActualDefectforTims(id_actual, unitStaff.staff_id, unitStaff.start_dt.Substring(0, unitStaff.start_dt.Length - 4), unitStaff.end_dt.Substring(0, unitStaff.end_dt.Length - 4));
                            await _ITimsService.updateQtyforStaff(id_actual, ActualQty, ActualDefectQty, psid);
                        }
                        int totalQTYforActual = await _ITimsService.getActualQTYForActualTims(id_actual);
                        await _ITimsService.UpdateTotalQTYActual(totalQTYforActual, id_actual);
                        //_TIMSService.UpdateActualStaff(id_actual, psid);


                        if (ok_qty == 0 && check_qty_error == 0)
                        {
                            //ml_no mã đầu vào

                            //ml_tims mã đầu ra
                          await _ITimsService.DeleteMfaclineQC(mt_cd, mt_lot);
                        }

                        return Json(new { result = true, remain = reamain_int, MFQC }, JsonRequestBehavior.AllowGet);


                    }
                    else
                    {
                        //tạo mã PO ca ngày hoặc ca đêm bên mms
                        if (ca_ngay <= DateTime.Now && DateTime.Now <= ca_dem)
                        {
                            ca = "CN";
                        }
                        else
                        {
                            ca = "CD";
                        }
                        bien = check_po.at_no + "-" + check_po.product + "-" + ca + "-TIMS-NG";
                        var list = await _ITimsService.GetmfaclineqcSearch("TI");

                        if (list == null)
                        {
                            MFQC.fq_no = "TI000000001";
                        }
                        else
                        {
                            var menuCd = string.Empty;
                            var subMenuIdConvert = 0;
                            //var list1 = list;

                            var bien1 = list;
                            var subMenuId = bien1.Substring(bien1.Length - 9, 9);
                            int.TryParse(subMenuId, out subMenuIdConvert);
                            menuCd = "TI" + string.Format("{0}{1}", menuCd, CreateFQ((subMenuIdConvert + 1)));
                            MFQC.fq_no = menuCd;
                        }
                        MFQC.ml_tims = mt_lot;
                        MFQC.ml_no = mt_cd;
                        MFQC.work_dt = DateTime.Now.ToString("yyyyMMddHHmmss");
                        MFQC.reg_dt = reg_dt;
                        MFQC.chg_dt = chg_dt;
                        MFQC.product_cd = check_po.product;
                        MFQC.shift = ca;
                        MFQC.at_no = check_po.at_no;
                        var item = await _ITimsService.Getqcitemmt(item_vcd);
                        MFQC.item_nm = item.item_nm;
                        MFQC.item_exp = item.item_exp;
                        MFQC.ng_qty = check_qty_error;
                        MFQC.remain_qty = remain;

                        if (ModelState.IsValid)
                        {
                            MFQC.reg_id = userAcount;
                            MFQC.chg_id = userAcount;
                            //int result = _TIMSService.InsertMFaclineQC(MFQC);
                            int result = await _ITimsService.InsertMFaclineQC(MFQC);

                            //neu la ma chua check mms
                            if (bien != "")
                            {
                                int ttmaterialdv = await _ITimsService.CheckMaterialByMtCdLike(bien);
                                //int grptqy = check_qty - ok_qty;
                                int grptqy =check_qty_error;
                                if (ttmaterialdv <= 0)
                                {
                                    await _ITimsService.InsertMaterialInfoTims(bien, grptqy, grptqy, MFQC.reg_id, mt_lot);
                                }
                                else
                                {
                                    await _ITimsService.UpdateNGPO(grptqy, bien);
                                }

                                //công vào thùng đầu ra

                                int totalQTYfactline = await _ITimsService.GetSumQtyFacline(mt_lot);
                                await _ITimsService.UpdateContainerOutput(mt_lot, totalQTYfactline);

                                if (remain > 0)
                                {

                                    var bobin_history = await _IActualWOService.GetBobbinLctHist(check_mtcd.bb_no);
                                    //if (trang_thai == 1 && check_po.don_vi_pr != "200" && (!db.d_bobbin_lct_hist.Any(x => x.bb_no == check_mtcd.bb_no)))
                                    if (trang_thai == 1 && check_po.don_vi_pr != "200" && bobin_history != null)
                                    {
                                        //insert history lại

                                        var bobin_primary = await _IActualWOService.GetBobbinInfo(check_mtcd.bb_no);
                                        if (bobin_primary != null)
                                        {
                                            bobin_primary.chg_id = userAcount;
                                            bobin_primary.mt_cd = check_mtcd.mt_cd;
                                            await _ITimsService.UpdateBobbinInfo(userAcount, check_mtcd.mt_cd, bobin_primary.bno);

                                        }

                                        if (bobin_history == null)
                                        {
                                            //insert data đã xóa trước đó

                                            var d_bobbin_lct_hist = new DBobbinLctHist();
                                            d_bobbin_lct_hist.bb_no = check_mtcd.bb_no;
                                            d_bobbin_lct_hist.mt_cd = check_mtcd.mt_cd;
                                            d_bobbin_lct_hist.reg_dt = DateTime.Now;
                                            d_bobbin_lct_hist.chg_dt = DateTime.Now;
                                            d_bobbin_lct_hist.chg_id = userAcount;
                                            d_bobbin_lct_hist.reg_id = userAcount;
                                            d_bobbin_lct_hist.use_yn = "Y";
                                            d_bobbin_lct_hist.del_yn = "N";
                                            await _ITimsService.InsertBobbinHist(d_bobbin_lct_hist);

                                        }
                                    }

                                    if (mt_cd.Contains("ROT") || mt_cd.Contains("STA"))
                                    {
                                        await _ITimsService.UpdateGrqtyMaterialInfoMMS(mt_cd, (int)remain, "008", userAcount);
                                        await _ITimsService.UpdateMaterialmappinguseyn(mt_lot, mt_cd, userAcount, false, "Y");
                                        //await _ITimsService.UpdateMaterialmappingMMSuseyn(mt_lot, mt_cd, userAcount, true);
                                    }
                                    else
                                    {
                                        await _ITimsService.UpdateGrqtyMaterialInfoTims(mt_cd, (int)remain, "002", userAcount);
                                        await _ITimsService.UpdateMaterialmappinguseyn(mt_lot, mt_cd, userAcount, true, "Y");

                                    }

                                }
                                else if (remain == 0)
                                {
                                    if (mt_cd.Contains("ROT") || mt_cd.Contains("STA"))
                                    {
                                        await _ITimsService.UpdateGrqtyMaterialInfoMMS(mt_cd, (int)remain, "005", userAcount);
                                        await _ITimsService.UpdateMaterialmappinguseyn(mt_lot, mt_cd, userAcount, false, "N");
                                    }
                                    else
                                    {
                                        await _ITimsService.UpdateGrqtyMaterialInfoTims(mt_cd, (int)remain, "005", userAcount);
                                        await _ITimsService.UpdateMaterialmappinguseyn(mt_lot, mt_cd, userAcount, false, "N");
                                    }
                                    await _ITimsService.DeleteDBobbinLctHistforDevice(mt_cd, check_mtcd.bb_no);
                                    await _ITimsService.UpdateBobbinInfowithmtcd(mt_cd, check_mtcd.bb_no, userAcount);
                                }
                                //tổng hàng defect của 1 cộng đoạn
                                int defect = await _ITimsService.GetDefactActualTimss((int)id_actual);
                                await _ITimsService.UpdateDefectActualTims(defect, id_actual, userAcount);
                                //tổng hàng actual của 1 cộng đoạn
                                int ActualQty = await _ITimsService.getActualRealQTY(id_actual, unitStaff.staff_id, unitStaff.start_dt.Substring(0, unitStaff.start_dt.Length - 4), unitStaff.end_dt.Substring(0, unitStaff.end_dt.Length - 4));
                                int ActualDefectQty = await _ITimsService.getActualDefectforTims(id_actual, unitStaff.staff_id, unitStaff.start_dt.Substring(0, unitStaff.start_dt.Length - 4), unitStaff.end_dt.Substring(0, unitStaff.end_dt.Length - 4));
                                await _ITimsService.updateQtyforStaff(id_actual, ActualQty, ActualDefectQty, psid);

                                int totalQTYforActual = await _ITimsService.getActualQTYForActualTims(id_actual);
                                await _ITimsService.UpdateTotalQTYActual(totalQTYforActual, id_actual);

                                if (ok_qty == 0 && check_qty_error == 0)
                                {
                                    //ml_tims mã đầu ra
                                    await _ITimsService.DeleteMfaclineQC(mt_cd, mt_lot);
                                }
                                return Json(new { result = true, remain = remain, MFQC }, JsonRequestBehavior.AllowGet);
                            }
                            return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = "Lôĩ hệ thống!!!" }, JsonRequestBehavior.AllowGet);
            }
            #endregion insert info
        }
        //public async Task<ActionResult> update_Ng_qty(string fqno, string defect_qty)
        //{
        //    try
        //    {
        //        var resqheader = Request.Headers; string[] resqheaderkey = resqheader.AllKeys; string[] resqheaderval = resqheader.GetValues("requestFrom"); if (Session["authorize"] != null || resqheadervalmob  == "Mob")
        //        {

        //            string userid = (Session["userid"] == null) ? "" : Session["userid"].ToString();
        //            //update check qty bảng m_facline_qc/ tông defect+ok = check_qty
        //            if (string.IsNullOrEmpty(fqno))
        //            {
        //                return Json(new { result = false, message = "Id rỗng" }, JsonRequestBehavior.AllowGet);
        //            }
        //            if (string.IsNullOrEmpty(defect_qty))
        //            {
        //                return Json(new { result = false, message = "Số Ng rỗng, vui lòng nhập số Ng" }, JsonRequestBehavior.AllowGet);
        //            }
        //            var id = int.Parse(fqno);
        //            //var ktExist = db.m_facline_qc.Find(id);
        //            var ktExist = await _ITimsService.GetmfaclineqcwithId(id);
        //            if (ktExist == null)
        //            {
        //                return Json(new { result = false, message = "Id không tồn tại" }, JsonRequestBehavior.AllowGet);
        //            }
        //            var ng_cu = (ktExist.FirstOrDefault().check_qty - ktExist.FirstOrDefault().ok_qty);
        //            var updateCheckqty = ktExist.FirstOrDefault().ok_qty + int.Parse(defect_qty);
        //            ktExist.FirstOrDefault().check_qty = updateCheckqty;
        //            await _ITimsService.UpdateFaclineQty(ktExist.FirstOrDefault().check_qty, ktExist.FirstOrDefault().ok_qty, ktExist.FirstOrDefault().fqno, userid);
        //            //tim mã PO000000022-LJ63-19611A-CN-TIMS-NG update gr_qty
        //            //kiểm tra lấy PO
        //            var ExistId_actual = await _ITimsService.GetMaterialInfoMMSByOrgin(ktExist.FirstOrDefault().ml_tims);

        //            var Exist_po = await _ITimsService.FindOneWActual(ExistId_actual.FirstOrDefault().id_actual);
        //            var check_pr = await _ITimsService.FindOneWActualPrimaryByAtNo(Exist_po.at_no);
        //            var ca = "";
        //            var ca_ngay = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 08:00:00"));
        //            var ca_dem = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 20:00:00"));

        //            //tạo mã PO ca ngày hoặc ca đêm bên mms
        //            if (ca_ngay.TimeOfDay <= ExistId_actual.FirstOrDefault().reg_date.TimeOfDay && ExistId_actual.FirstOrDefault().chg_date.TimeOfDay <= ca_dem.TimeOfDay)
        //            {
        //                ca = "CN";
        //            }
        //            else
        //            {
        //                ca = "CD";
        //            }
        //            var bien = Exist_po.at_no + "-" + check_pr.product + "-" + ca + "-TIMS-NG";

        //            var Exist_MaNg = await _ITimsService.GetWMaterialInfoWithmtcdLike(bien);
        //            if (Exist_MaNg != null)
        //            {
        //                if (Exist_MaNg.FirstOrDefault().gr_qty == Exist_MaNg.FirstOrDefault().real_qty)
        //                {
        //                    Exist_MaNg.FirstOrDefault().real_qty = (Exist_MaNg.FirstOrDefault().real_qty - ng_cu) + int.Parse(defect_qty);

        //                }
        //                Exist_MaNg.FirstOrDefault().gr_qty = (Exist_MaNg.FirstOrDefault().gr_qty - ng_cu) + int.Parse(defect_qty);
        //                await _ITimsService.UpdateGroupQTYRealQTYTims(Exist_MaNg.FirstOrDefault().gr_qty, Exist_MaNg.FirstOrDefault().real_qty, Exist_MaNg.FirstOrDefault().material_code, userid);
        //                //db.Entry(Exist_MaNg).State = EntityState.Modified;
        //            }

        //            update_defect(ktExist.FirstOrDefault().ml_tims);
        //            var data = await _ITimsService.GetFaclineQCWithfqno(fqno);
        //            return Json(new { result = true, message = "Sửa thành công", data = data }, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { result = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);

        //    }
        //}
        public async Task<string> update_defect(string mt_cd)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {

                    string userid = (Session["userid"] == null) ? "" : Session["userid"].ToString(); ;
                    //var danhsach = db.w_material_info.Where(x => x.mt_cd == mt_cd).SingleOrDefault();
                    var danhsach = await _ITimsService.GetWMaterialInfoWithmtcdLike(mt_cd);
                    if (danhsach != null)
                    {
                        var id_actual = danhsach.FirstOrDefault().id_actual;
                        var staff_id = danhsach.FirstOrDefault().staff_id;
                        var thoigian = danhsach.FirstOrDefault().reg_date.ToString("yyyy-MM-dd HH:mm:ss");
                        var stata = await _ITimsService.FindDProUnitStaffByIdActual(id_actual, staff_id, thoigian);
                        int defectt = await _ITimsService.getActualDefectforTims(id_actual, staff_id, stata.FirstOrDefault().start_dt, stata.FirstOrDefault().end_dt);
                        await _ITimsService.UpdateActualNDefectForStaff(id_actual, stata.FirstOrDefault().psid, (int)stata.FirstOrDefault().actual, defectt, userid);
                        return "ok";
                    }

                    return "";
                }
                else
                {
                    return "";
                }

            }
            catch (Exception e)
            {
                return "";
            }


        }
        //public async Task<ActionResult> ModifyFaclineQcDetail(string check_qty, string id, int fqno)
        //{
        //    var resqheader = Request.Headers; string[] resqheaderkey = resqheader.AllKeys; string[] resqheaderval = resqheader.GetValues("requestFrom"); if (Session["authorize"] != null || resqheadervalmob  == "Mob")
        //    {
        //        var fq_no = "";
        //        string userid = Session["userid"] == null ? null : Session["userid"].ToString();
        //        if ((check_qty != null) && (id != null) && (fqno > 0))
        //        {

        //            //var list_m_facline_qc = db.m_facline_qc.Find(fqno);
        //            var list_m_facline_qc = await _ITimsService.GetFaclineQCWithfqnos(fqno);
        //            fq_no = list_m_facline_qc.FirstOrDefault().fq_no;
        //            var list = new ArrayList();
        //            var a2 = id.TrimStart('[').TrimEnd(']').Split(',');
        //            var m1 = check_qty.TrimStart('[').TrimEnd(']').Split(',');

        //            int sumcheck_qty = 0;
        //            foreach (var item in m1)
        //            {
        //                sumcheck_qty += Convert.ToInt32(item);
        //            }
        //            var defect_qty = list_m_facline_qc.FirstOrDefault().check_qty - list_m_facline_qc.FirstOrDefault().ok_qty;
        //            if (sumcheck_qty > defect_qty)
        //            {
        //                return Json(new { result = false, message = Constant.OverloadError }, JsonRequestBehavior.AllowGet);
        //            }

        //            for (int i = 0; i < a2.Length; i++)
        //            {
        //                var id2 = int.Parse(a2[i]);
        //                var find = await _ITimsService.GetListDataFaclineQcValuewithId(id2);

        //                // var list_m_facline_qc = await _ITimsService.GetFaclineQCWithfqnos(fqno);
        //                if (Convert.ToInt32(m1[i]) <= defect_qty)
        //                {
        //                    find.check_qty = Convert.ToInt32(m1[i]);
        //                    find.chg_id = userid;
        //                    await _ITimsService.Updatemfaclineqcvalue(find.check_qty, userid, id2);
        //                    //db.Entry(find).State = EntityState.Modified;
        //                    //db.SaveChanges();
        //                }
        //                var listdata = await _ITimsService.Getfacline_qc_value(find.fq_no);

        //                list.Add(listdata);
        //            }

        //            int soluongdefect = await _ITimsService.GetQTYmfaclineqcvalue(fq_no);

        //            list_m_facline_qc.FirstOrDefault().ok_qty = list_m_facline_qc.FirstOrDefault().check_qty - soluongdefect;
        //            list_m_facline_qc.FirstOrDefault().chg_dt = DateTime.Now;
        //            list_m_facline_qc.FirstOrDefault().chg_id = userid;
        //            await _ITimsService.UpdateFaclineQty(list_m_facline_qc.FirstOrDefault().check_qty, list_m_facline_qc.FirstOrDefault().ok_qty, list_m_facline_qc.FirstOrDefault().fqno, userid);
        //            //db.Entry(list_m_facline_qc).State = EntityState.Modified;
        //            //db.SaveChanges();
        //            //update w_material_info

        //            var w_material_info = _ITimsService.FindWMaterialInfoByMTQRCode(list_m_facline_qc.FirstOrDefault().ml_no);// db.w_material_info.Where(x => x.mt_cd == list_m_facline_qc.ml_no).SingleOrDefault();
        //            if (w_material_info != null)
        //            {
        //                var check_ng = await _ITimsService.GetListDataFaclineQcwithmlno(list_m_facline_qc.FirstOrDefault().ml_no);
        //                w_material_info.gr_qty = w_material_info.gr_qty - (check_ng.Sum(x => x.check_qty) - check_ng.Sum(x => x.ok_qty));
        //                w_material_info.chg_id = userid;
        //                await _ITimsService.UpdateRemainQTYTims(w_material_info.gr_qty, w_material_info.status, w_material_info.material_code, userid);
        //                // var w_material_info2 = db.w_material_info.Where(x => x.mt_cd.StartsWith(list_m_facline_qc.ml_no + "-NG") && x.lct_cd.StartsWith("006")).SingleOrDefault();
        //                var w_material_info2 = await _ITimsService.GetListMaterialInfoTIMS(list_m_facline_qc.FirstOrDefault().ml_no);
        //                if (w_material_info2 != null)
        //                {
        //                    w_material_info2.FirstOrDefault().gr_qty = (check_ng.Sum(x => x.check_qty) - check_ng.Sum(x => x.ok_qty));
        //                    w_material_info2.FirstOrDefault().alert_NG = 1;
        //                    w_material_info2.FirstOrDefault().chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
        //                    await _ITimsService.UpdateRemainQTYInfoTims(w_material_info2.FirstOrDefault().gr_qty, w_material_info2.FirstOrDefault().status, w_material_info2.FirstOrDefault().material_code, userid, w_material_info2.FirstOrDefault().alert_NG);
        //                    //db.Entry(w_material_info2).State = EntityState.Modified;
        //                    //db.SaveChanges();
        //                }
        //                //update w_actual
        //                var w_actual = await _ITimsService.FindOneWActual(w_material_info.id_actual);
        //                w_actual.defect = await _ITimsService.GetDefetcActual(w_material_info.id_actual, "003");
        //                w_actual.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
        //                await _ITimsService.UpdateDefectActual(w_actual.defect, w_actual.id_actual);
        //                //db.Entry(w_actual).State = EntityState.Modified;
        //                //db.SaveChanges();
        //            }

        //            var listp_mpo_qc = await _ITimsService.GetFaclineQCWithfqnos(fqno);
        //            list.Add(listp_mpo_qc);
        //            return Json(new { result = true, list = list }, JsonRequestBehavior.AllowGet);
        //        }
        //        return View();
        //    }
        //    else
        //    {
        //        return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        #endregion ROll Nomal

        #region Devide

        [HttpGet]
        //public async Task<ActionResult> searchbobbinPopupDV(Pageing pageing, string bb_no, string bb_nm, string mt_cd)
        public async Task<ActionResult> searchbobbinPopupDV(PopUpDVrequest request)
        {
            try
            {
                int row = request.rows;
                var resqheader = Request.Headers;
                string[] resqheaderkey = resqheader.AllKeys;
                string[] resqheaderval = resqheader.GetValues("requestFrom");
                string resqheadervalmob = "";
                if (resqheaderval != null)
                {
                    resqheadervalmob = resqheaderval[0];
                }
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {
                    int start = (request.page - 1) * request.rows;
                    var data = await _ITimsService.SearchBobbinInfo(request.bb_no, request.bb_nm, request.mt_cd, start, row);
                    //return Json(data, JsonRequestBehavior.AllowGet);
                    int totalRecords = _ITimsService.GettotalRowSearchBobbinInfo(request.bb_no, request.bb_nm, request.mt_cd);
                    int totalPages = (int)Math.Ceiling((float)totalRecords / (float)request.rows);
                    var result = new
                    {
                        total = totalPages,
                        page = request.page,
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(e, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> DevideStaTims(string bb_no, int number_dv, int psid)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {
                    var check_bb = await _ITimsService.FindBobbinLctHist(bb_no);
                    if (check_bb == null)
                    {
                        return Json(new { result = false, message = "Container " + Constant.NotFound }, JsonRequestBehavior.AllowGet);
                    }
                    var mt_cd = check_bb.mt_cd;
                    if (number_dv == 0)
                    {
                        return Json(new { result = false, message = Constant.QuantityInvalid }, JsonRequestBehavior.AllowGet);
                    }

                    var find_mtcd = await _ITimsService.CheckMaterialForDiv(mt_cd);
                    if (find_mtcd == null)
                    {
                        return Json(new { result = false, message = Constant.FindDK }, JsonRequestBehavior.AllowGet);
                    }
                    //kiểm tra đã qua công đoạn khác chưa
                    //xử lý lại chổ này
                    var checkMaterialMappingTims = await _ITimsService.CheckMaterialMappingTims(mt_cd);
                    if (checkMaterialMappingTims > 0)
                    {
                        return Json(new { result = false, message = Constant.Other }, JsonRequestBehavior.AllowGet);
                    }
                    //kiểm tra coi n đã hết thời gian làm việc chưa
                    //cần optimize
                    var checkHetCaMT = await _ITimsService.CheckStaffShiftForId(find_mtcd.id_actual, psid);
                    if (checkHetCaMT == 0)
                    {
                        return Json(new { result = false, message = Constant.EndShift }, JsonRequestBehavior.AllowGet);
                    }

                    double sss = Convert.ToDouble(find_mtcd.gr_qty) / Convert.ToDouble(number_dv);
                    var so_luong = Math.Ceiling(sss);
                    var so_cl = so_luong;
                    int gr_cl = (int)find_mtcd.gr_qty;
                    double total = find_mtcd.real_qty / so_luong;


                    //update sô lượng về 0 và trang thái về 005
                    find_mtcd.status = "005";
                    find_mtcd.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    find_mtcd.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    find_mtcd.gr_qty = 0;
                    find_mtcd.number_divide = number_dv;
                    find_mtcd.chg_date = DateTime.Now;
                    find_mtcd.reg_date = DateTime.Now;
                    find_mtcd.material_code = mt_cd;
                    //kiểm tra chỉ update những field cần thiết
                    await _ITimsService.UpdateMaterialInfoTims(find_mtcd);
                    await _ITimsService.UpdatePartialBobbinInfo(find_mtcd.material_code);
                    //int count = await _ITimsService.GetTotalwMaterialInfoDV(find_mtcd.material_code + "-DV"); // 20%
                    await _ITimsService.DeleteBobbinHistoryDevideSta(find_mtcd.material_code);

                    return Json(new { result = true, message = Constant.Success, kq = find_mtcd }, JsonRequestBehavior.AllowGet);
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
        public async Task<JsonResult> DestroyDevideTims(string mt_cd)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                //kiểm tra mã lot này tồn tại không
                var result = _ITimsService.FindWMaterialInfoByMTQRCode(mt_cd); //1%
                var check_mt_cd = result;
                if (check_mt_cd == null)
                {
                    return Json(new { result = false, message = Constant.NotFound }, JsonRequestBehavior.AllowGet);
                }
                //mã này có bằng 0 và trạng thái = 005 chưa và không tồn tại đồ đựng này trong history nhé
                if (check_mt_cd.gr_qty != 0)
                {
                    return Json(new { result = false, message = "Sản Lượng Vẫn Còn Không thể trở về" }, JsonRequestBehavior.AllowGet);
                }
                if (check_mt_cd.status != "005")
                {
                    return Json(new { result = false, message = Constant.Status }, JsonRequestBehavior.AllowGet);
                }

                var checkMappingForDV = await _ITimsService.CheckWMaterialMapForDV(mt_cd);//2%
                if (checkMappingForDV > 0)
                {
                    return Json(new { result = false, message = Constant.Other }, JsonRequestBehavior.AllowGet);
                }
                //kiểm tra đã gộp chưa
                // xử lí
                StringBuilder varname1 = new StringBuilder();
                int CheckExistMappig = await _ITimsService.CheckWMaterialMappingForRedo(mt_cd); //2%
                if (CheckExistMappig > 0)
                {
                    return Json(new { result = false, message = "Đã được gộp lot nên không thể trở về !! " }, JsonRequestBehavior.AllowGet);
                }

                IEnumerable<WMaterialnfoDV> dsach = await _ITimsService.DSWMaterialDV(mt_cd); //2%
                foreach (var ittm in dsach)
                {
                    if (ittm.blno > 0)
                    {
                        await _ITimsService.DeleteBoBbinHisForRedo(ittm.blno);

                    }
                    if (ittm.bno > 0)
                    {
                        await _ITimsService.UpdateBobbinInfoForRedo(ittm.bno);

                    }
                }
                var gr_qty = check_mt_cd.real_qty;

                await _ITimsService.UpdateWMaterialQtyForRedo(gr_qty, check_mt_cd.material_code); //1%

                //xoa mã devide
                await _ITimsService.DeleteWMaterialQtyForRedo(check_mt_cd.material_code); //2%

                //Xóa mã devedie mapping
                await _ITimsService.DeleteWMaterialMappingForRedo(check_mt_cd.material_code); //1%

                //XÓA BOBIN HISTORY DEVIDE

                await _ITimsService.UpdatemtcdforBobbinInfoForRedo(check_mt_cd.bb_no, check_mt_cd.material_code); //1%
                var bbinfo = await _ITimsService.FindOneDBobbinInfo(check_mt_cd.bb_no);//1%

                DBobbinLctHist d_bobbin_lct_hist = new DBobbinLctHist();
                d_bobbin_lct_hist.bb_no = check_mt_cd.bb_no;
                d_bobbin_lct_hist.bb_nm = bbinfo.bb_nm;
                d_bobbin_lct_hist.mc_type = bbinfo.mc_type;
                d_bobbin_lct_hist.mt_cd = check_mt_cd.material_code;
                d_bobbin_lct_hist.use_yn = "Y";
                d_bobbin_lct_hist.del_yn = "N";
                d_bobbin_lct_hist.reg_dt = DateTime.Now;
                d_bobbin_lct_hist.chg_dt = DateTime.Now;
                d_bobbin_lct_hist.reg_id = Session["userid"] == null ? "Mob" : Session["userid"].ToString();
                d_bobbin_lct_hist.chg_id = Session["userid"] == null ? "Mob" : Session["userid"].ToString();
                await _ITimsService.InsertBobbinHist(d_bobbin_lct_hist);
                return Json(new { result = true, message = Constant.Success, gr_qty = gr_qty, wmtid = check_mt_cd.wmtid }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }

        private int ToNumber(string input)
        {
            string stringNumber = "";
            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsDigit(input[i]))
                    stringNumber += input[i];
            }
            if (stringNumber.Length > 0)
                return Convert.ToInt32(stringNumber);
            else
                return 0;
        }

        [HttpPost]
        //public JsonResult change_gr_dv(string value_new, string value_old, string wmtid, int psid)
        public JsonResult ChangeGroupDevide(string value_new, string value_old, string wmtid, int psid)
        {
            return Json(new { result = false, message = "Đã hủy chức năng này khỏi hệ thống!!!" }, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> Changebb_dvEA(string bb_no, int wmmid)
        {//chức năng thay đổi ở nhưng cuộn EA còn dư
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {
                    //kiểm tra bobbin có tồn tại không
                    string userid = Session["userid"] == null ? "Mob" : Session["userid"].ToString(); ;
                    var CheckBobbinExist = await _ITimsService.GetBobbinInfo(bb_no);

                    if (CheckBobbinExist == null)
                    {
                        return Json(new { result = false, message = "Bobbin này không tồn tại, Mã bobbin bạn vừa quét: " + bb_no }, JsonRequestBehavior.AllowGet);
                    }
                    //string CheckBobbinUsing = _TIMSService.GetBobbinUsing(bb_no);
                    var CheckBobbinUsing = await _ITimsService.GetdbobbinlcthistFrbbno(bb_no);
                    //var check_bb_new_his = db.d_bobbin_lct_hist.Where(x => x.bb_no == bb_no).ToList();
                    if (CheckBobbinUsing != null)
                    {
                        return Json(new { result = false, message = Constant.Used }, JsonRequestBehavior.AllowGet);
                    }
                    var check_mapping2 = await _ITimsService.FindOneWMaterialMappingById(wmmid);// db.w_material_mapping.Find(wmmid);
                    var check_mapping1 = await _ITimsService.GetListDataMaterialMappingTIMSById(check_mapping2.mt_cd, check_mapping2.mt_lot);

                    if (check_mapping2 != null)
                    {
                        foreach (var item in check_mapping1)
                        {
                            //so sánh thời gian mapping
                            var mapping_hientai = check_mapping2.mapping_dt;
                            if (item.mapping_dt > mapping_hientai)
                            {
                                return Json(new { result = false, message = Constant.Other }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    if (check_mapping2.mt_cd.Contains("ROT") || check_mapping2.mt_cd.Contains("STA"))
                    {
                        //var check = db.w_material_info.Where(x => x.mt_cd == check_mapping2.mt_cd && x.gr_qty > 0).SingleOrDefault();
                        var check = await _ITimsService.GetMaterialInfoTimsOfDeviceGrpQTYMMS(check_mapping2.mt_cd);
                        if (check == null)
                        {
                            return Json(new { result = false, message = "Lot " + Constant.NotFound }, JsonRequestBehavior.AllowGet);
                        }
                        if (check.status == "009" || check.status == "010")
                        {
                            return Json(new { result = false, message = Constant.Other }, JsonRequestBehavior.AllowGet);
                        }
                        if (check.gr_qty == 0)
                        {
                            return Json(new { result = false, message = "Đã sử dụng hết" }, JsonRequestBehavior.AllowGet);
                        }
                        //await _ITimsService.UpdateBobbinInfowitbbno(check.bb_no, userid);
                        //await _ITimsService.RemoveBobbinLctHistwithbbno(check.bb_no);
                        await _ITimsService.UpdatemtcdforBobbinInfoForRedo(bb_no, check.material_code);
                        DBobbinLctHist bbhist = new DBobbinLctHist();
                        bbhist.bb_no = bb_no;
                        bbhist.mt_cd = check.material_code;
                        bbhist.use_yn = "Y";
                        bbhist.del_yn = "N";
                        bbhist.chg_id = userid;
                        bbhist.reg_id = userid;
                        await _ITimsService.InsertBobbinHist(bbhist);
                        check.bb_no = bb_no;
                        var ress = await _IActualWOService.UpdateMaterialInfoMMS(check);
                    }
                    else
                    {
                        //var check = db.w_material_info.Where(x => x.mt_cd == check_mapping2.mt_cd && x.gr_qty > 0).SingleOrDefault();
                        var check = await _ITimsService.GetMaterialInfoTimsOfDeviceGrpQTY(check_mapping2.mt_cd);
                        if (check == null)
                        {
                            return Json(new { result = false, message = "Lot " + Constant.NotFound }, JsonRequestBehavior.AllowGet);
                        }
                        if (check.status == "009" || check.status == "010")
                        {
                            return Json(new { result = false, message = Constant.Other }, JsonRequestBehavior.AllowGet);
                        }
                        if (check.gr_qty == 0)
                        {
                            return Json(new { result = false, message = "Đã sử dụng hết" }, JsonRequestBehavior.AllowGet);
                        }
                        //await _ITimsService.UpdateBobbinInfowitbbno(check.bb_no, userid);
                        //await _ITimsService.RemoveBobbinLctHistwithbbno(check.bb_no);
                        await _ITimsService.UpdatemtcdforBobbinInfoForRedo(bb_no, check.material_code);
                        DBobbinLctHist bbhist = new DBobbinLctHist();
                        bbhist.bb_no = bb_no;
                        bbhist.mt_cd = check.material_code;
                        bbhist.use_yn = "Y";
                        bbhist.del_yn = "N";
                        bbhist.chg_id = userid;
                        bbhist.reg_id = userid;
                        await _ITimsService.InsertBobbinHist(bbhist);
                        check.bb_no = bb_no;
                        var ress = await _ITimsService.UpdateMaterialInfoTims(check);
                    }

                    //var wmtid = await _ITimsService.InsertMaterialInfoTIMMS(material);
                    return Json(new { result = true, message = Constant.Success }, JsonRequestBehavior.AllowGet);
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
        //public JsonResult Changebb_dvEA(string bb_no, int wmmid)
        public async Task<JsonResult> ChangeBobbinDevideEA(string bb_no, string material_code, string material_code_child)
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

                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {
                    string userid = Session["userid"] == null ? "Mob" : Session["userid"].ToString();
                    //chức năng thay đổi ở nhưng cuộn EA còn dư
                    //var check_mapping2 = db.w_material_mapping.Find(wmmid);
                    var check_mapping2 = await _ITimsService.GetMaterialMappingTIMSByLotCancel(material_code);
                    var result = await _ITimsService.GetListDataMaterialMappingTIMSById(check_mapping2.FirstOrDefault().mt_cd, check_mapping2.FirstOrDefault().mt_lot);
                    //var check_mapping1 = db.w_material_mapping.Where(x => x.mt_cd == check_mapping2.mt_cd && x.mt_lot != check_mapping2.mt_lot).OrderByDescending(x => x.mapping_dt).ToList();
                    var check_mapping1 = result.ToList();
                    if (check_mapping2 != null)
                    {
                        foreach (var item in check_mapping1)
                        {
                            //so sánh thời gian mapping
                            var mapping_hientai = check_mapping2.FirstOrDefault().mapping_dt;
                            if (Convert.ToInt64(item.mapping_dt.ToString("yyyyMMddHHmmss")) > Convert.ToInt64(mapping_hientai.ToString("yyyyMMddHHmmss")))
                            {
                                return Json(new { result = false, message = Constant.Other }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }

                    //var check = db.w_material_info.Where(x => x.mt_cd == check_mapping2.mt_cd && x.gr_qty > 0).SingleOrDefault();
                    var check = await _ITimsService.GetMaterialInfoTimsOfDevice(check_mapping2.FirstOrDefault().mt_cd);
                    if (check == null)
                    {
                        return Json(new { result = false, message = "Lot " + Constant.NotFound }, JsonRequestBehavior.AllowGet);
                    }
                    if (check.status == "009" || check.status == "010")
                    {
                        return Json(new { result = false, message = Constant.Other }, JsonRequestBehavior.AllowGet);
                    }
                    if (check.gr_qty == 0)
                    {
                        return Json(new { result = false, message = "Đã sử dụng hết" }, JsonRequestBehavior.AllowGet);
                    }

                    //var check_bb_new_his = db.d_bobbin_lct_hist.Where(x => x.bb_no == bb_no).ToList();
                    var check_bb_new_his = await _ITimsService.GetdbobbinlcthistFrbbno(bb_no);
                    if (check_bb_new_his != null)
                    {
                        return Json(new { result = false, message = Constant.Used }, JsonRequestBehavior.AllowGet);
                    }

                    //await _IActualWOService.UpdateMaterialInfoDivideMMS(bb_no, wmmid, "TIMS");

                    var check_bb = await _IActualWOService.GetBobbinInfo(bb_no);
                    if (check_bb != null)
                    {
                        check_bb.mt_cd = "";
                        check_bb.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        await _IActualWOService.UpdateBobbinMTCode(check_bb);
                    }
                    else
                    {
                        //add bobin mới
                        check_bb_new_his.mt_cd = check.material_code;
                        check_bb_new_his.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        await _IActualWOService.UpdateBobbinLctHist(check_bb_new_his);
                    }
                    DBobbinLctHist bbhist = new DBobbinLctHist();
                    bbhist.bb_no = bb_no;
                    bbhist.mt_cd = check.material_code;
                    bbhist.use_yn = "Y";
                    bbhist.del_yn = "N";
                    bbhist.chg_id = userid;
                    bbhist.reg_id = userid;
                    await _ITimsService.InsertBobbinHist(bbhist);
                    //int tinhtoan = _TIMSService.ChangeBobinTimsDv(bb_no, wmtid, check.bb_no, check.material_code);

                    //var material = await _ITimsService.GetMaterialInfoTimsOfDevice(check.material_code, "002");
                    Models.NewVersion.MaterialInfoTIMS material = new Models.NewVersion.MaterialInfoTIMS();
                    //int subtraction = material.real_qty - check.gr_qty;
                    material.material_code = material_code_child;
                    material.real_qty = check.number_divide;
                    material.gr_qty = check.number_divide;
                    material.id_actual = check.id_actual;
                    material.staff_id = check.staff_id;
                    material.id_actual_oqc = check.id_actual_oqc;
                    material.orgin_mt_cd = check.material_code;
                    material.product = check.product;
                    material.end_production_dt = check.end_production_dt;
                    material.mt_no = check.mt_no;
                    //material.real_qty = check.gr_qty;
                    material.bb_no = bb_no;
                    material.status = "002";
                    material.location_code = "006000000000000000";
                    material.from_lct_code = "006000000000000000";
                    material.material_type = "CMT";
                    //material.number_divide = parentItem.number_divide;
                    material.chg_id = userid;
                    material.reg_id = userid;
                    material.reg_date = DateTime.Now;
                    material.chg_date = DateTime.Now;
                    //Models.NewVersion.MaterialInfoMMS itemm = new Models.NewVersion.MaterialInfoMMS();
                    //itemm.material_code = material_code_child;
                    //itemm.bb_no = bb_no;
                    var wmtid = await _ITimsService.InsertMaterialInfoTIMMS(material);
                    await _ITimsService.InsertMultiMaterialMppingTimsDV(material_code_child, check.material_code, userid, bb_no);

                    //await _ITimsService.UpdateMaterialInfoTims(material);
                    //await _actualController.GetListMappingStaTims(check.material_code);



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

        //public JsonResult Changebb_dv(string bb_no, int wmtid)
        public async Task<JsonResult> ChangeBobinDevideTims(string bb_no, string material_code, string material_code_child, int gr_qty)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {
                    string userid = Session["userid"] == null ? "Mob" : Session["userid"].ToString();
                    //chức năng thay đổi ở sau khi chia ra hộp nhỏ//chức năng thay đổi ở sau khi chia ra hộp nhỏ
                    //var check = _IWOService.GetWMaterialInfo(wmtid);
                    //var check = await _ITimsService.GetWMaterialInfoTIMS(wmtid);GetMaterialInfoTimsOfDevice
                    var check = await _ITimsService.GetMaterialInfoTimsOfDevice(material_code); //1%
                    //var check_mapping2 = await _ITimsService.GetMaterialMappingTIMSByLotCancel(material_code);
                    if (check == null)
                    {
                        return Json(new { result = false, message = Constant.NotFound + " MT NO" }, JsonRequestBehavior.AllowGet);
                    }

                    var CheckMaterialMappingTims = await _ITimsService.CheckMaterialMappingTims(check.material_code);
                    int qtyparent = _ITimsService.GetRealQTYParent(check.material_code);
                    int qtydv = _ITimsService.GetRealQTYMaterialCodeDevie(check.material_code + "-DV");
                    if (CheckMaterialMappingTims > 0 && qtyparent== qtydv)
                    {
                        return Json(new { result = false, message = Constant.Other }, JsonRequestBehavior.AllowGet);
                    }
                    if (check.status == "009" || check.status == "010")
                    {
                        return Json(new { result = false, message = Constant.Other }, JsonRequestBehavior.AllowGet);
                    }

                    var check_bb_new = await _ITimsService.GetOneDBobbinInfoWithMtCdIsNULL(bb_no);

                    var check_bb_new_his = await _ITimsService.CheckBobbinHistory(bb_no);

                    if (check_bb_new_his != null)// nếu = false chạy vào đây (mã BObin này chưa tồn tại trong bảng d_bobbin_info)
                    {
                        return Json(new { result = false, message = Constant.Used }, JsonRequestBehavior.AllowGet);
                    }
                    if (check_bb_new == null)
                    {
                        return Json(new { result = false, message = Constant.NotFound + " Container" }, JsonRequestBehavior.AllowGet);
                    }
                    //đã kiểm tra chưa
                    //var CheckWMaterialHasNG = await _ITimsService.CheckWMaterialHasNG(check.material_code);
                    //if (CheckWMaterialHasNG != null)
                    //{
                    //    return Json(new { result = false, message = Constant.Other }, JsonRequestBehavior.AllowGet);
                    //}

                    var CheckFaclineQCHasNG = await _ITimsService.CheckFaclineQCHasNG(check.material_code);
                    var CheckWProductQCHasNG = await _ITimsService.CheckWProductQCHasNG(check.material_code);
                    //đã check bất cứ TQC OQC ANY Chưa
                    if (CheckFaclineQCHasNG != null || CheckWProductQCHasNG != null)
                    {
                        return Json(new { result = false, message = Constant.Tested }, JsonRequestBehavior.AllowGet);
                    }

                    //await _IActualWOService.UpdateMaterialInfoDivideMMS(bb_no, wmtid,"TIMS");

                    var check_bb = await _IActualWOService.GetBobbinInfo(bb_no);
                    if (check_bb != null)
                    {
                        check_bb.mt_cd = "";
                        check_bb.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        await _IActualWOService.UpdateBobbinMTCode(check_bb);
                    }
                    else
                    {
                        //add bobin mới
                        check_bb_new.mt_cd = check.material_code;
                        check_bb_new.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        await _IActualWOService.UpdateBobbinMTCode(check_bb_new);
                    }
                    var check2 = await _ITimsService.GetMaterialInfoTimsOfDevice(material_code_child);
                    if (check2 != null)
                    {
                        await _ITimsService.UpdateBobbinInfowitbbno(check2.bb_no, userid);
                        await _ITimsService.RemoveBobbinLctHistwithbbno(check2.bb_no);
                        await _ITimsService.UpdatemtcdforBobbinInfoForRedo(bb_no, material_code_child);
                        check2.bb_no = bb_no;
                        await _ITimsService.UpdateMaterialInfoTims(check2);
                        await _ITimsService.UpdateBobbinMaterinInfoTimsDV(material_code_child, check.material_code,userid,bb_no);

                    }
                    else
                    {

                        //check.bb_no = bb_no;
                        Models.NewVersion.MaterialInfoTIMS material = new Models.NewVersion.MaterialInfoTIMS();
                        //int subtraction = material.real_qty - check.gr_qty;
                        material.material_code = material_code_child;
                        material.real_qty = gr_qty;
                        material.gr_qty = gr_qty;
                        material.at_no = check.at_no;
                        material.product = check.product;
                        material.id_actual = check.id_actual;
                        material.staff_id = check.staff_id;
                        material.id_actual_oqc = check.id_actual_oqc;
                        material.orgin_mt_cd = check.material_code;
                        material.product = check.product;
                        material.end_production_dt = check.end_production_dt;
                        material.mt_no = check.mt_no;
                        //material.real_qty = check.gr_qty;
                        material.bb_no = bb_no;
                        material.status = "002";
                        material.location_code = "006000000000000000";
                        material.from_lct_code = "006000000000000000";
                        material.material_type = "CMT";
                        material.sts_update = "composite";
                        material.chg_id = userid;
                        material.reg_id = userid;
                        material.reg_date = DateTime.Now;
                        material.chg_date = DateTime.Now;

                        var wmtid = await _ITimsService.InsertMaterialInfoTIMMS(material);
                      int kqinsert=  await _ITimsService.InsertMultiMaterialMppingTimsDV(material_code_child, check.material_code, userid, bb_no); //1%
                    }
                    DBobbinLctHist bbhist = new DBobbinLctHist();
                    bbhist.bb_no = bb_no;
                    bbhist.mt_cd = material_code_child;
                    bbhist.use_yn = "Y";
                    bbhist.del_yn = "N";
                    bbhist.chg_id = userid;
                    bbhist.reg_id = userid;
                    await _ITimsService.InsertBobbinHist(bbhist);
                    await _ITimsService.UpdatemtcdforBobbinInfoForRedo(bb_no, material_code_child);

                    if(qtyparent== qtydv)
                    {
                        _ITimsService.UpdateStatusMaterialParent(check.material_code);
                    }
                    //await _ITimsService.UpdateBobbinInfowitbbno(check.bb_no, userid);
                    //await _ITimsService.RemoveBobbinLctHistwithbbno(check.bb_no);
                    //await _ITimsService.UpdatemtcdforBobbinInfoForRedo(bb_no, material_code_child);

                    //var material = await _ITimsService.GetMaterialInfoTimsOfDevice(check.material_code, "002");
                    //int subtraction = material.real_qty - check.gr_qty;
                    //material.real_qty = subtraction;
                    //material.real_qty = check.gr_qty;
                    //material.real_qty = check.gr_qty;
                    //material.bb_no = bb_no;
                    //material.status = "002";
                    //material.location_code = "006000000000000000";
                    //material.from_lct_code = "006000000000000000";
                    //material.material_type = "CMT";
                    //material.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    //material.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    //material.reg_date = DateTime.Now;
                    //material.chg_date = DateTime.Now;
                    //await _ITimsService.UpdateMaterialInfoTims(material);
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

        #endregion Devide

        #region Process OQC

        public async Task<ActionResult> GetbobinOqc(Pageing pageing, int id_actual, string at_no)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                var bb_nm = Request["bb_nm"];
                var bb_no = Request["bb_no"];
                //var datatt = _TIMSService.Getdbobbinlcthist(id_actual, at_no, bb_nm, bb_no);
                var datatt = await _ITimsService.Getdbobbinlcthist(id_actual, at_no, bb_nm, bb_no);
                if (!string.IsNullOrEmpty(bb_no))
                {
                    IEnumerable<WMaterialInfoTmp> enumerable = datatt.Where(i => i.bb_no.Contains(bb_no));
                    datatt = enumerable.ToList();
                    //.Where(i => i.bb_no.Contains(bb_no)
                }
                int start = (pageing.page - 1) * pageing.rows;
                int totals = datatt.Count();
                int totalPages = (int)Math.Ceiling((float)totals / pageing.rows);
                IEnumerable<WMaterialInfoTmp> dataactual = datatt.Skip<WMaterialInfoTmp>(start).Take(pageing.rows);
                var jsonReturn = new
                {
                    total = totalPages,
                    page = pageing.page,
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

        //public JsonResult getListQR_oqc(string bb_no, int id_actual, string staff_id)
        public async Task<JsonResult> GetListQrOqcTims(string bb_no, int id_actual, string staff_id)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {

                    //kiem tra do dung

                    //var history = _TIMSService.FindOneBobbin_lct_hist(bb_no);
                    var history = await _ITimsService.FindAllBobbin_lct_histByBBNo(bb_no);
                    if (history == null)
                    {
                        return Json(new { result = false, message = "Đồ đựng này " + Constant.Used }, JsonRequestBehavior.AllowGet);
                    }
                    //kiểm tra mã lot đó có không
                    if (history.mt_cd.Contains("ROT") || history.mt_cd.Contains("STA"))
                    {
                        return Json(new { result = false, message = "Đồ đựng này chưa kiểm tra công đoạn trước khi vào OQC" }, JsonRequestBehavior.AllowGet);
                    }

                    var find = await _ITimsService.GetWMaterialInfowithmtcd(history.mt_cd);

                    if (find == null)
                    {
                        return Json(new { result = false, message = "Đồ đựng này " + Constant.Used }, JsonRequestBehavior.AllowGet);
                    }


                    if (find.gr_qty == 0)
                    {
                        return Json(new { result = false, message = "Sản lượng đang đang bằng 0 !! Vui lòng kiểm tra lại" }, JsonRequestBehavior.AllowGet);
                    }
                    if (find.status.Equals("009"))
                    {
                        return Json(new { result = false, message = "Hộp hàng này đã có người khác kiểm tra rồi. " + find.staff_id_oqc }, JsonRequestBehavior.AllowGet);
                    }
                    if (find.status != "002" && find.status != "008")
                    {
                        //var trangthai = checktrangthai(find.status);
                        var trangthai = await _ITimsService.GetNameStatusCommCode(find.status);
                        return Json(new { result = false, message = "Trạng Thái đang là " + trangthai }, JsonRequestBehavior.AllowGet);
                    }
                    //kiểm tra xem coi công nhân này còn ca không
                    bool checkstaffshift = await _ITimsService.CheckStaffShift(id_actual, staff_id);
                    if (checkstaffshift)
                    {
                        return Json(new { result = false, message = Constant.EndShift }, JsonRequestBehavior.AllowGet);
                    }

                    var id_lot = id_actual;
                    var id_cd = find.id_actual;

                    //var check_lot = _TIMSService.FindOneWActual(id_lot);
                    var check_lot = await _ITimsService.FindOneWActual(id_lot);


                    //var check_cd = _IWOService.GetWActual((int)id_cd);
                    var check_cd = await _ITimsService.GetWActual((int)id_cd);

                    var check_po = "";
                    if (check_lot.at_no == check_cd.at_no)
                    {
                        check_po = "OK";
                    }
                    if (check_po == "")
                    {
                        return Json(new { result = false, message = "Chọn Sai PO! Xin vui lòng chọn lại!" }, JsonRequestBehavior.AllowGet);
                    }
                    //kiểm tra trạng thái hiện tại và đã đủ công đoạn chưa
                    //kiểm tra bao nhiêu công đoạn
                    //IEnumerable<w_actual> process = _TIMSService.GetListWActualForProcess(check_cd.at_no).ToList();
                    var process = await _ITimsService.GetListWActualForProcess(check_cd.at_no);

                    //truy xuất lot
                    IEnumerable<truyxuatlot> txlot;
                    string mtcd = "";
                    if (find.material_code.Contains("-DV"))
                    {
                        var dvv = find.material_code.Substring(find.material_code.IndexOf("-DV"));
                        //int indexoof = find.material_code.IndexOf(dvv);
                        mtcd = find.material_code.Substring(0, find.material_code.Length- dvv.Length);
                        //mtcd = mtcds[0];
                    }
                    else
                    {
                        mtcd = find.material_code;
                    }
                    if (!string.IsNullOrEmpty(find.buyer_code) || find.status == "010")
                    {
                        //txlot = _ITimsService.Truyxuatlistlot(mtcd, "", find.buyer_code, check_cd.at_no);
                        txlot = _ITimsService.TruyxuatlistlotOQC(mtcd, "", find.buyer_code, check_cd.at_no);
                    }
                    else
                    {
                        //txlot = _ITimsService.Truyxuatlistlot(mtcd, "CP", find.buyer_code, check_cd.at_no);
                        txlot = _ITimsService.TruyxuatlistlotOQC(mtcd, "", find.buyer_code, check_cd.at_no);
                    }
                    //kiem tra ton tai đầy đủ process không

                    var khongton_tai = (from c in process
                                        where !(from o in txlot
                                                select o.process_cd)
                                        .Contains(c.name)
                                        select new { process = c.name }).ToList();
                    if (khongton_tai.Count() > 0)
                    {
                        var html = "Bạn chưa trải qua những công đoạn này :" + string.Join("<br> ", khongton_tai.Select(x => x.process)); ;
                        return Json(new { result = false, message = html }, JsonRequestBehavior.AllowGet);

                    }
                    if (find.status != "009" && find.location_code.StartsWith("006"))
                    {
                        //update trạng thái và đưa vào võ
                        find.id_actual = id_actual;
                        find.staff_id = staff_id;
                        find.status = "009";
                        find.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                        // int result = _TIMSService.UpdateWMaterialInfoById(find.id_actual, find.staff_id, find.status, find.chg_id, find.wmtid);
                        int result = await _ITimsService.UpdateWMaterialInfoById(find.id_actual, find.staff_id, find.status, find.chg_id, find.wmtid);

                    }
                    return Json(new
                    {
                        result = true,
                        kq = new
                        {
                            wmtid = find.wmtid,
                            bb_no = find.bb_no,
                            mt_no = find.mt_no,
                            bb_nm = "",
                            mt_cd = find.material_code,
                            gr_qty = find.gr_qty,
                            id_actual = find.id_actual,
                            message = "",
                            count_ng = 0,
                        }
                    }, JsonRequestBehavior.AllowGet);
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
        public async Task<string> checktrangthai(string mt_sts_cd)
        {
            var csv = await _ITimsService.GetNameStatusCommCode(mt_sts_cd);
            return csv;
        }
        public async Task<ActionResult> PartialView_View_OQC_WEB(string item_vcd)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                //lấy hết tất cả qc_itemcheck_mt
                //var qc_itemcheck_mt = new List<QCItemCheck_Mt_Model>();
                var qc_itemcheck_mt2 = new List<QCItemCheck_Mt_Model>();
                var qc_itemcheck_mt = await _ITimsService.GetQCItemCheckMaterialwithitemvcd(item_vcd);

                foreach (var item in qc_itemcheck_mt)
                {
                    //var view_qc_Model = new List<view_qc_Model>();
                    //lấy hết tất cả qc_itemcheck_dt
                    var qc_itemcheck_dt = await _ITimsService.GetQCItemCheckMaterialDetailwithcheckcd(item_vcd, item.qc_itemcheck_mt__check_id);
                    if (qc_itemcheck_dt.Count() > 0)
                    {
                        item.view_qc_Model = qc_itemcheck_dt.ToList();
                        qc_itemcheck_mt2.Add(item);

                    }
                }
                return PartialView(qc_itemcheck_mt2);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> GetMeterialOqcTims(int id_actual, string staff_id)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                var Data = await _ITimsService.GetListLotOQCpp(id_actual, staff_id);
                return Json(Data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> Get_OKPO(Pageing pageing, string mt_cd, string mt_no, string bb_no, string product, string at_no, string staff_id)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {

                    var listdata = await _ITimsService.GetListOKPO(mt_cd, mt_no, bb_no, product, at_no, staff_id);
                    int start = (pageing.page - 1) * pageing.rows;
                    int totals = listdata.Count();
                    int totalPages = (int)Math.Ceiling((float)totals / pageing.rows);
                    IEnumerable<MaterialInfoTIMS> dataactual = listdata.Skip<MaterialInfoTIMS>(start).Take(pageing.rows);
                    var jsonReturn = new
                    {
                        total = totalPages,
                        page = pageing.page,
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
            catch (Exception e)
            {
                return Json(new { result = false, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //public JsonResult Changests_packing(int wmtid, int psid)
        //public async Task<JsonResult> ChangestsPacking(int wmtid, int psid)
        //{
        //    try
        //    {
        //        var resqheader = Request.Headers; string[] resqheaderkey = resqheader.AllKeys; string[] resqheaderval = resqheader.GetValues("requestFrom"); if (Session["authorize"] != null || resqheadervalmob  == "Mob")
        //        {
        //            string userid = (Session["userid"] == null) ? "" : Session["userid"].ToString();
        //            //var data = _TIMSService.FindAllMeterialChangestPacking(wmtid);
        //            var data = await _ITimsService.FindAllMeterialChangestPacking(wmtid);
        //            var Sucess = 0;
        //            var Not_Enough = 0;
        //            var id_actual = 0;
        //            if (data != null)
        //            {
        //                //id_actual = data.id_actual_oqc ?? 0;
        //                id_actual = data.id_actual;
        //                int checkshiftstaff = await _ITimsService.CheckShift(psid, id_actual);
        //                //if (_TIMSService.FindDProUnitStaffChangestsPacking(data.id_actual_oqc, psid) == 0)
        //                if (checkshiftstaff == 0)
        //                {
        //                    return Json(new { result = false, message = Constant.EndShift }, JsonRequestBehavior.AllowGet);
        //                }
        //                //_TIMSService.UpdateWMaterialInfoByIdSingle("010", (Session["userid"] == null) ? "" : Session["userid"].ToString(), DateTime.Now, wmtid);
        //                await _ITimsService.UpdateWMaterialInfoByIdMultiple("010", userid, DateTime.Now.ToString(), wmtid);
        //                //_TIMSService.UpdateactualDprounitstaffChangestspacking(data.gr_qty, id_actual, psid);
        //                await _ITimsService.UpdateActualForOQC(data.gr_qty, id_actual, psid, userid);
        //                //int SumActual = _TIMSService.GetSumactualChangestspacking(id_actual);
        //                int SumActual = await _ITimsService.getActualQTYForActualTims(id_actual);
        //                //_TIMSService.UpdatewactualChangestspacking(id_actual, SumActual);
        //                await _ITimsService.UpdateTotalQTYActual(id_actual, SumActual);
        //                return Json(new { result = true, message = Constant.Success, Not_Enough, Sucess }, JsonRequestBehavior.AllowGet);
        //            }
        //            else
        //            {
        //                return Json(new { result = false, message = Constant.CannotPass, Not_Enough, Sucess }, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        else
        //        {
        //            return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { result = false, message = Constant.ErrorSystem, Not_Enough = Constant.ErrorSystem, Sucess = "0" }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpPost]
        //public async Task<JsonResult> ChangestspackingWeb(List<int> wmtid, int psid)
        public async Task<JsonResult> ChangestspackingWeb([System.Web.Http.FromBody] ChangestspackingWebRequest request)
        {
            try
            {
                int ss = 0;
                int psid = request.psid;
                foreach (int wmtid in request.Listwmtid)
                {
                    var resqheader = Request.Headers;
                    string[] resqheaderkey = resqheader.AllKeys;
                    string[] resqheaderval = resqheader.GetValues("requestFrom");
                    string resqheadervalmob = "";
                    if (resqheaderval != null)
                    {
                        resqheadervalmob = resqheaderval[0];
                    }
                    if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                    {

                        //var data = _TIMSService.FindAllMeterialChangestPackingweb(wmtid).ToList();
                        string userid = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                        //var data = await _ITimsService.FindAllMeterialChangestPackingweb(int.Parse(wmtid));
                        var data = await _ITimsService.FindAllMeterialChangestPackingweb(wmtid);
                        var Not_Enough = 0;
                        int id_actual = 0;
                        var Sucess = 0;
                        if (data.Count() > 0)
                        {
                            //id_actual = data[0].id_actual_oqc ?? 0;
                            id_actual = data[0].id_actual_oqc;
                            int checkshiftstaff = await _ITimsService.CheckShift(psid, id_actual);
                            //if (_TIMSService.FindDProUnitStaffChangestsPacking(id_actual, psid) == 0)
                            if (checkshiftstaff == 0)
                            {
                                return Json(new { result = false, message = Constant.EndShift }, JsonRequestBehavior.AllowGet);
                            }
                            //_TIMSService.UpdateWMaterialInfoByIdMultiple("010", userid, DateTime.Now, wmtid);
                            //await _ITimsService.UpdateWMaterialInfoByIdMultiple("010", userid, DateTime.Now.ToString(),int.Parse( wmtid));
                            await _ITimsService.UpdateWMaterialInfoByIdMultiple("010", userid, DateTime.Now.ToString(), wmtid);
                            int sanluong = 0;
                            foreach (var item in data)
                            {
                                sanluong += item.gr_qty;
                            }
                            // _TIMSService.UpdateactualDprounitstaffChangestspacking(sanluong ,id_actual, psid);
                            await _ITimsService.UpdateActualForOQC(sanluong, id_actual, psid, userid);
                            //int SumActual = _TIMSService.GetSumactualChangestspacking(id_actual);
                            int SumActual = await _ITimsService.getActualQTYForActualTims(id_actual);
                            //_TIMSService.UpdatewactualChangestspacking(id_actual, SumActual);
                            await _ITimsService.UpdateTotalQTYActual(SumActual, id_actual);
                            ss++;
                        }
                        else
                        {
                            return Json(new { result = false, message = Constant.CannotPass, Not_Enough }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                    }
                }
                if(ss > 0)
                {
                    return Json(new { result = true, message = Constant.Success }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.CannotPass }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { result = false, message = Constant.ErrorSystem, Not_Enough = Constant.ErrorSystem, Sucess = "0" }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> ChangestsPacking(int wmtid, int psid)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {

                    //var data = _TIMSService.FindAllMeterialChangestPackingweb(wmtid).ToList();
                    string userid = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                    //var data = await _ITimsService.FindAllMeterialChangestPackingweb(int.Parse(wmtid));
                    var data = await _ITimsService.FindAllMeterialChangestPackingweb(wmtid);
                    var Not_Enough = 0;
                    int id_actual = 0;
                    if (data.Count() > 0)
                    {
                        //id_actual = data[0].id_actual_oqc ?? 0;
                        id_actual = data[0].id_actual_oqc;
                        int checkshiftstaff = await _ITimsService.CheckShift(psid, id_actual);
                        //if (_TIMSService.FindDProUnitStaffChangestsPacking(id_actual, psid) == 0)
                        if (checkshiftstaff == 0)
                        {
                            return Json(new { result = false, message = Constant.EndShift }, JsonRequestBehavior.AllowGet);
                        }
                        //_TIMSService.UpdateWMaterialInfoByIdMultiple("010", userid, DateTime.Now, wmtid);
                        //await _ITimsService.UpdateWMaterialInfoByIdMultiple("010", userid, DateTime.Now.ToString(),int.Parse( wmtid));
                        await _ITimsService.UpdateWMaterialInfoByIdMultiple("010", userid, DateTime.Now.ToString(), wmtid);
                        int sanluong = 0;
                        foreach (var item in data)
                        {
                            sanluong += item.gr_qty;
                        }
                        // _TIMSService.UpdateactualDprounitstaffChangestspacking(sanluong ,id_actual, psid);
                        await _ITimsService.UpdateActualForOQC(sanluong, id_actual, psid, userid);
                        //int SumActual = _TIMSService.GetSumactualChangestspacking(id_actual);
                        int SumActual = await _ITimsService.getActualQTYForActualTims(id_actual);
                        //_TIMSService.UpdatewactualChangestspacking(id_actual, SumActual);
                        await _ITimsService.UpdateTotalQTYActual(SumActual, id_actual);
                        //ss++;
                        return Json(new { result = true, message = Constant.Success, Not_Enough }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, message = Constant.CannotPass, Not_Enough }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { result = false, message = Constant.ErrorSystem, Not_Enough = Constant.ErrorSystem, Sucess = "0" }, JsonRequestBehavior.AllowGet);
            }
        }
        //public JsonResult Returnsts_packing(string wmtid)

        [HttpPost]
        public async Task<JsonResult> ReturnPackingWeb([System.Web.Http.FromBody] List<int> listwmtid)
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

                    var Sucess = 0;
                    foreach (int wmtid in listwmtid)
                    {
                        //start
                        //var data = _TIMSService.FindAllMaterialInfoReturn(wmtid).ToList();
                        var data = await _ITimsService.FindAllMeterialChangestPackingweb(wmtid);
                        var check_oqc = 0;



                        foreach (var item in data)
                        {
                            var checkproqc = await _ITimsService.Checkwproductqc(item.material_code);
                            //if (!db.w_product_qc.Any(x => x.ml_no == item.material_code))
                            if (checkproqc == null)
                            {
                                check_oqc++;
                            }
                            //tra ve nguyen
                            item.status = "008";
                            item.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                            //int effectRow = _TIMSService.UpdateWMaterialInfoById(item.mt_sts_cd, item.chg_id, item.wmtid);
                            int effectRow = await _ITimsService.UpdateWMaterialInfoByIdMultipleforReturn(item.status, item.chg_id, DateTime.Now.ToString(), item.wmtid);

                            //var id_oqc = _TIMSService.FindOneWActual(item.id_actual_oqc);
                            var id_oqc = await _ITimsService.FindOneWActual(item.id_actual);
                            if (id_oqc != null)
                            {
                                id_oqc.defect = id_oqc.defect + item.gr_qty;
                                id_oqc.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                                //_TIMSService.UpdateDefectActual(id_oqc.defect, id_oqc.chg_id, id_oqc.id_actual);
                                await _ITimsService.UpdateDefectActualTims((int)(id_oqc.defect), id_oqc.id_actual, id_oqc.chg_id);
                            }
                            Sucess++;
                        }
                    }

                    if (Sucess > 0)
                    {
                        return Json(new { result = true, message = Constant.Success }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { result = false, message = Constant.CannotPass }, JsonRequestBehavior.AllowGet);
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
        public async Task<JsonResult> ReturnPacking(int wmtid)
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

                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {
                    var Sucess = 0;
                    //start
                    //var data = _TIMSService.FindAllMaterialInfoReturn(wmtid).ToList();
                    var data = await _ITimsService.FindAllMeterialChangestPackingweb(wmtid);
                    var check_oqc = 0;



                    foreach (var item in data)
                    {
                        var checkproqc = await _ITimsService.Checkwproductqc(item.material_code);
                        //if (!db.w_product_qc.Any(x => x.ml_no == item.material_code))
                        if (checkproqc == null)
                        {
                            check_oqc++;
                        }
                        //tra ve nguyen
                        item.status = "008";
                        item.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                        //int effectRow = _TIMSService.UpdateWMaterialInfoById(item.mt_sts_cd, item.chg_id, item.wmtid);
                        int effectRow = await _ITimsService.UpdateWMaterialInfoByIdMultipleforReturn(item.status, item.chg_id, DateTime.Now.ToString(), item.wmtid);

                        //var id_oqc = _TIMSService.FindOneWActual(item.id_actual_oqc);
                        var id_oqc = await _ITimsService.FindOneWActual(item.id_actual);
                        if (id_oqc != null)
                        {
                            id_oqc.defect = id_oqc.defect + item.gr_qty;
                            id_oqc.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                            //_TIMSService.UpdateDefectActual(id_oqc.defect, id_oqc.chg_id, id_oqc.id_actual);
                            await _ITimsService.UpdateDefectActualTims((int)(id_oqc.defect), id_oqc.id_actual, id_oqc.chg_id);
                        }
                        Sucess++;
                    }


                    if (Sucess > 0)
                    {
                        return Json(new { result = true, message = Constant.Success }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { result = false, message = Constant.CannotPass }, JsonRequestBehavior.AllowGet);
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
        public async void ReleaseContainer(string bobbin, string mt_cd, string mt_sts_cd)
        {
            //kiểm tra xem gr_qty=0 chưa
            if (mt_sts_cd == "005")
            {

                //_TIMSService.UpdateBobbinInfo(Session["userid"] == null ? null : Session["userid"].ToString(), bobbin, mt_cd);
                await _ITimsService.UpdateBobbinInfowithmtcd(mt_cd, bobbin,Session["userid"] == null ? null : Session["userid"].ToString());
                await _ITimsService.DeleteDBobbinLctHistforDevice(mt_cd,bobbin);
                //_TIMSService.DeleteBobbinHistory(bobbin, mt_cd);

            }
        }

        public async Task<JsonResult> Gop_OK(string wmtid, string mt_cd, int soluong, int psid)
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

                    string userid = Session["userid"] == null ? null : Session["userid"].ToString();
                    //WMaterialInfoTmp currentMaterial = _TIMSService.FindOneMaterialInfoByMTLot(mt_cd);
                    WMaterialInfoTmp currentMaterial = await _ITimsService.FindOneMaterialInfoByMTLot(mt_cd);
                    if (currentMaterial == null)
                    {
                        return Json(new { result = false, message = "MT LOT " + Constant.NotFound }, JsonRequestBehavior.AllowGet);
                    }
                    if (string.IsNullOrEmpty(currentMaterial.bb_no))
                    {
                        return Json(new { result = false, message = "Vui Lòng Thêm đồ đựng !!!!" }, JsonRequestBehavior.AllowGet);
                    }
                    if (currentMaterial.mt_sts_cd.Equals("005"))
                    {
                        return Json(new { result = false, message = "Đồ đựng này đã bị xả rồi, mã đồ đựng: " + currentMaterial.bb_no + " !!!!" }, JsonRequestBehavior.AllowGet);
                    }
                    //kiểm tra coi n đã hết thời gian làm việc chưa
                    int checkShiftStaff = await _ITimsService.CheckShift(psid, currentMaterial.id_actual.Value);
                    //if (_TIMSService.CheckShift(psid, currentMaterial.id_actual.Value) == 0)
                    if (checkShiftStaff == 0)
                    {
                        return Json(new { result = false, message = Constant.EndShift }, JsonRequestBehavior.AllowGet);
                    }
                    //IEnumerable<WMaterialInfoTmp> listMaterial = _TIMSService.GetListMaterial(wmtid, mt_cd, currentMaterial.mt_no).ToList();
                    var listMaterial = new List<WMaterialInfoTmp>();
                    string[] lstwmtid = wmtid.Split(',');
                    foreach(string wmtidd in lstwmtid)
                    {
                        var lstMaterial = await _ITimsService.GetListMaterial(int.Parse(wmtidd), mt_cd, currentMaterial.mt_no);
                        listMaterial.AddRange(lstMaterial);
                    }


                    double ChosenNG = listMaterial.Sum(x => (double)x.gr_qty);
                    if (ChosenNG < soluong)
                    {
                        return Json(new { result = false, message = Constant.QuantityNGInvalid }, JsonRequestBehavior.AllowGet);
                    }
                    int Count = 0;
                    int RemainingNumber = soluong;
                    bool Success = false;
                    foreach (WMaterialInfoTmp item in listMaterial)
                    {
                        if (RemainingNumber <= 0)
                        {
                            break;
                        }
                        if ((item.gr_qty >= soluong) && Count == 0)
                        {
                            ////trừ số lượng còn lại của ng và update
                            item.gr_qty = item.gr_qty - soluong;
                            ////nếu số lượng hết thì cho n đã sử dụng hết

                            if (item.gr_qty == 0)
                            {
                                //_TIMSService.UpdateMaterial(item.gr_qty, item.wmtid, "005");
                                await _ITimsService.UpdateMaterial((int)item.gr_qty, item.wmtid, "005", userid);
                                //ReleaseContainer(item.bb_no, item.mt_cd, "005");
                                await _ITimsService.UpdateBobbinInfowithmtcd(item.mt_cd, item.bb_no, userid);
                                await _ITimsService.DeleteDBobbinLctHistforDevice(item.mt_cd, item.bb_no);
                            }
                            else
                            {
                                //_TIMSService.UpdateMaterial(item.gr_qty, item.wmtid);
                                await _ITimsService.UpdateQtyMaterialTims((int)item.gr_qty, item.wmtid, userid);
                            }
                            //var infoactual = await _ITimsService.GetWActual(item.id_actual);
                            //add mapping
                            //ADD mã mới
                            int CountMaterialInfo = await _ITimsService.CountMaterialInfo(item.mt_cd + "-MG");
                            int IncrementNumber = CountMaterialInfo + 1;
                            item.orgin_mt_cd = item.mt_cd;
                            item.mt_cd = item.mt_cd + "-MG" + IncrementNumber;
                            item.gr_qty = 0;
                            item.real_qty = soluong;
                            item.mt_sts_cd = "005";
                            //_TIMSService.InsertMergeMaterial(item);
                            await _ITimsService.InsertMergeMaterial(item);
                            bool checkExistsMtMpp = await _ITimsService.CheckExistMaterialMapping(item.mt_cd, currentMaterial.mt_cd);
                            if (currentMaterial.mt_cd != item.mt_cd && checkExistsMtMpp)
                            {
                                w_material_mapping addmapping = new w_material_mapping();
                                addmapping.mt_cd = item.mt_cd;
                                addmapping.mt_lot = currentMaterial.mt_cd;
                                addmapping.mt_no = currentMaterial.mt_no;
                                addmapping.mapping_dt = DateTime.Now.ToString();
                                addmapping.bb_no = currentMaterial.bb_no;
                                addmapping.use_yn = "N";
                                addmapping.del_yn = "N";
                                addmapping.chg_id = userid;
                                addmapping.reg_id = userid;
                                addmapping.reg_dt = DateTime.Now;
                                addmapping.chg_dt = DateTime.Now;
                                //_TIMSService.InsertMaterialMapping(addmapping);
                                await _ITimsService.InsertMaterialMappingTims(addmapping);
                                //await _ITimsService.InsertMaterialMapping(addmapping);
                                Success = true;
                                //await _ITimsService.UpdateBobbinInfowithmtcd(item.mt_cd, item.bb_no, userid);
                                //await _ITimsService.DeleteDBobbinLctHistforDevice(item.mt_cd, item.bb_no);
                                //ReleaseContainer(item.bb_no, item.mt_cd, "005");
                                //break;
                            }

                        }
                        else
                        {
                            var GrQuantity = item.gr_qty;

                            item.gr_qty = (RemainingNumber - item.gr_qty > 0 ? 0 : (-1 * (RemainingNumber - item.gr_qty)));

                            if (item.gr_qty == 0)
                            {
                                await _ITimsService.UpdateMaterial((int)item.gr_qty, item.wmtid, "005", userid);
                                //_TIMSService.UpdateMaterial(item.gr_qty, item.wmtid, "005");
                                // ReleaseContainer(item.bb_no, item.mt_cd, "005");
                                await _ITimsService.UpdateBobbinInfowithmtcd(item.mt_cd, item.bb_no, userid);
                                await _ITimsService.DeleteDBobbinLctHistforDevice(item.mt_cd, item.bb_no);
                            }
                            else
                            {
                                await _ITimsService.UpdateQtyMaterialTims((int)item.gr_qty, item.wmtid, userid);
                            }
                            // ADD mã mới
                            int IncrementNumber = await _ITimsService.CountMaterialInfo(item.mt_cd + "-MG");
                            //int IncrementNumber = _TIMSService.CountMaterialInfo(item.mt_cd + "-MG%") + 1;
                            item.orgin_mt_cd = item.mt_cd;
                            item.mt_cd = item.mt_cd + "-MG" + IncrementNumber;
                            item.real_qty = item.gr_qty;
                            item.mt_sts_cd = "005";
                            //_TIMSService.InsertMergeMaterial(item);
                            await _ITimsService.InsertMergeMaterial(item);
                            //add mapping
                            //ADD mã mới
                            bool checkExistsMtMpp = await _ITimsService.CheckExistMaterialMapping(item.mt_cd, currentMaterial.mt_cd);
                            if (currentMaterial.mt_cd != item.mt_cd && !checkExistsMtMpp)
                            {

                                var addmapping = new w_material_mapping();
                                addmapping.mt_cd = item.mt_cd;
                                addmapping.mt_lot = currentMaterial.mt_cd;
                                addmapping.mt_no = currentMaterial.mt_no;
                                addmapping.mapping_dt = DateTime.Now.ToString();
                                addmapping.bb_no = currentMaterial.bb_no;
                                addmapping.use_yn = "N";
                                addmapping.del_yn = "N";
                                addmapping.chg_id = userid;
                                addmapping.reg_id = userid;
                                addmapping.reg_dt = DateTime.Now;
                                addmapping.chg_dt = DateTime.Now;
                                //await _ITimsService.InsertMaterialMapping(addmapping);
                                await _ITimsService.InsertMaterialMappingTims(addmapping);
                                //_TIMSService.InsertMaterialMapping(addmapping);

                            }
                            Success = true;
                            RemainingNumber = RemainingNumber - int.Parse(GrQuantity.ToString());
                        }
                        Count++;
                    }
                    if (Success)
                    {
                        currentMaterial.gr_qty = currentMaterial.gr_qty + soluong;
                        await _ITimsService.UpdateQtyMaterialTims((int)currentMaterial.gr_qty, currentMaterial.wmtid, userid);
                        //_TIMSService.UpdateMaterial(currentMaterial.gr_qty, currentMaterial.wmtid);
                        return Json(new { result = true, message = Constant.Success }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
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

        //public JsonResult Get_NGPO(Pageing pageing, int id_actual, string mt_cd, string mt_no, string bb_no, string PO)
        public async Task<JsonResult> GetNotGoodPoTims(Pageing pageing, int id_actual, string mt_cd, string mt_no, string bb_no, string PO)
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

                    //WActual FindPO = _TIMSService.FindOneWActual(id_actual);
                    var FindPO = await _IActualWOService.GetActual(id_actual);
                    if (FindPO == null)
                    {
                        return Json(new { result = false, message = "PO " + Constant.NotFound }, JsonRequestBehavior.AllowGet);
                    }

                    var FindPrimary = await _ITimsService.FindOneWActualPrimaryByAtNo(FindPO.at_no);

                    //var data = _TIMSService.GetNotGoodPO(FindPrimary.product, mt_cd, mt_no, PO);
                    var data = await _ITimsService.GetNotGoodPO(FindPrimary.product, mt_cd, mt_no, PO);
                    var records = data.Count();
                    int totalPages = (int)Math.Ceiling((float)records / pageing.rows);
                    var rowsData = data.Skip((pageing.page - 1)).Take(pageing.rows);
                    return Json(new { total = totalPages, page = pageing.page, records = records, rows = rowsData }, JsonRequestBehavior.AllowGet);
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

        [HttpGet]
        public async Task<JsonResult> Gop_NG(List<int> wmtid, string mt_cd, int soluong, int psid)
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

                //var chekck_mtcd = db.w_material_info.Where(x => x.mt_cd == mt_cd).SingleOrDefault();
                var rs = await _ITimsService.FindAllMaterialByMtCdLike(mt_cd);
                var chekck_mtcd = rs.SingleOrDefault();
                if (chekck_mtcd == null)
                {
                    return Json(new { result = false, message = " MT LOT" + Constant.NotFound }, JsonRequestBehavior.AllowGet);
                }

                //kiểm tra coi n đã hết thời gian làm việc chưa
                var listStaff = await _ITimsService.GetListStaff(chekck_mtcd.id_actual, psid);
                var data1 = listStaff.ToList().Count();
                if (data1 == 0)
                {
                    return Json(new { result = false, message = Constant.EndShift }, JsonRequestBehavior.AllowGet);
                }

                var gr_cu = chekck_mtcd.gr_qty;
                var materialInfoTims = await _ITimsService.GetListDataMaterialInfoTIMSById(wmtid);
                var data = materialInfoTims.ToList();
                var so_luong_ngchoose = data.Sum(x => x.gr_qty);

                if (so_luong_ngchoose < soluong)
                {
                    return Json(new { result = false, message = Constant.QuantityNGInvalid }, JsonRequestBehavior.AllowGet);
                }

                var dem = 0;
                var so_luongcl = soluong;
                var sucess = 0;
                foreach (var item in data)
                {
                    if (so_luongcl <= 0)
                    {
                        break;
                    }
                    var addmapping = new MaterialMappingTIMS();
                    if ((item.gr_qty >= soluong) && dem == 0)
                    {
                        //trừ số lượng còn lại của ng và update
                        var gr_qty = item.gr_qty - soluong;
                        //nếu số lượng hết thì cho n đã sử dụng hết
                        if (item.gr_qty == 0)
                        {
                            addmapping.mt_cd = item.material_code;
                        }
                        //item.location_code = "006000000000000000";
                        // db.Entry(item).State = EntityState.Modified;
                        var status = "005";
                        var chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        await _ITimsService.UpdateMaterialInfoTimsScan(item.material_code, gr_qty, status, chg_id);

                        //add mapping
                        if (item.gr_qty > 0)
                        {
                            // ADD mã mới
                            var list_number = await _ITimsService.GetListNumberForTIMS(item.material_code);
                            var so_tang = list_number.ToList().Count() + 1;
                            item.orgin_mt_cd = item.material_code;
                            item.material_code = item.material_code + "-MG" + so_tang;
                            item.status = "005";
                            item.real_qty = item.gr_qty;
                            var id = await _ITimsService.InsertMaterialInfoTIMMS(item);
                            addmapping.mt_cd = item.material_code;
                        }
                        if(chekck_mtcd.material_code != item.material_code)
                        {
                            var ListMaterialMapping = _ITimsService.GetListMaterialMappingReturn(item.material_code, chekck_mtcd.material_code);
                            if(ListMaterialMapping.Count() < 1)
                            {
                                addmapping.mt_lot = chekck_mtcd.material_code;
                                addmapping.mt_no = chekck_mtcd.mt_no;
                                addmapping.mapping_dt = DateTime.Now;
                                addmapping.bb_no = chekck_mtcd.bb_no;
                                addmapping.use_yn = "N";
                                addmapping.del_yn = "N";
                                addmapping.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                addmapping.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                addmapping.reg_date = DateTime.Now;
                                addmapping.chg_date = DateTime.Now;
                                var wmmid = await _ITimsService.InsertMaterialMapping(addmapping);
                                sucess++;
                            }
                        }

                        break;
                    }
                    else
                    {
                        var so_luongcu = item.gr_qty;
                        var gr_qty = (so_luongcl - item.gr_qty > 0 ? 0 : (-1 * (so_luongcl - item.gr_qty)));
                        var status = "";
                        if (item.gr_qty == 0)
                        {
                            status = "005";
                        }
                        //item.location_code = "006000000000000000";
                        var chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        //db.Entry(item).State = EntityState.Modified;
                        await _ITimsService.UpdateMaterialInfoTimsScan(item.material_code, gr_qty, status, chg_id);

                        if (item.gr_qty > 0)
                        {
                            // ADD mã mới
                            var list_so_tang = await _ITimsService.GetListNumberForTIMS(item.material_code);
                            //var so_tang = db.w_material_info.Where(x => x.mt_cd.StartsWith(item.material_code + "-MG")).ToList().Count() + 1;
                            var so_tang = list_so_tang.ToList().Count() + 1;
                            item.orgin_mt_cd = item.material_code;
                            item.material_code = item.material_code + "-MG" + so_tang;
                            item.status = "005";
                            item.real_qty = item.gr_qty;
                            //db.Entry(item).State = EntityState.Added;
                            var key = await _ITimsService.InsertMaterialInfoTIMMS(item);
                            addmapping.mt_cd = item.material_code;
                        }
                        if (chekck_mtcd.material_code != item.material_code)
                        {
                                var ListMaterialMapping = _ITimsService.GetListMaterialMappingReturn(item.material_code, chekck_mtcd.material_code);
                            if (ListMaterialMapping.Count() < 1)
                            {
                                addmapping.mt_lot = chekck_mtcd.material_code;
                                addmapping.mt_no = chekck_mtcd.mt_no;
                                addmapping.mapping_dt = DateTime.Now;
                                addmapping.bb_no = chekck_mtcd.bb_no;
                                addmapping.use_yn = "N";
                                addmapping.del_yn = "N";
                                addmapping.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                addmapping.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                addmapping.reg_date = DateTime.Now;
                                addmapping.chg_date = DateTime.Now;

                                var wmmid = await _ITimsService.InsertMaterialMapping(addmapping);
                                sucess++;
                            }
                            so_luongcl = so_luongcl - int.Parse(so_luongcu.ToString());
                        }
                    }
                    dem++;
                }
                if (sucess > 0)
                {
                    var gr_qty = chekck_mtcd.gr_qty + soluong;
                    var real_qty = chekck_mtcd.real_qty + soluong;
                    var chg_id = Session["userid"] == null ? null : Session["userid"].ToString();

                    await _ITimsService.UpdateMaterialInfoTims(mt_cd, gr_qty, real_qty, chg_id);
                    return Json(new { result = true, message = Constant.Success }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion Process OQC

        #endregion TIMS Web

        #region TIMS_MAPPINGBUYER

        public ActionResult Scan_Buyer()
        {
            return SetLanguage("");
        }

        public async Task<JsonResult> Getmt_mappingOQC(Pageing pageing, string product, string bb_bo, string at_no)
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

                var result = await _ITimsService.GetListmtmappingOQC(product, bb_bo, at_no);
                int start = (pageing.page - 1) * pageing.rows;
                int end = (pageing.page - 1) * pageing.rows + pageing.rows;
                int totals = result.Count();
                int totalPages = (int)Math.Ceiling((float)totals / pageing.rows);
                IEnumerable<MaterialInfoTIMS> dataactual = result.Skip<MaterialInfoTIMS>(start).Take(pageing.rows);
                var jsonReturn = new
                {
                    total = totalPages,
                    page = pageing.page,
                    records = totals,
                    rows = dataactual
                };
                return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = new List<MaterialInfoTIMS>();
                var jsonReturn = new
                {
                    result=false,
                    total = 0,
                    page = 0,
                    records = 0,
                    rows = data,
                    message = Constant.ErrorAuthorize
                };
                return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            }

        }

        //public JsonResult mapping_buyer(string bb_no, string buyer_qr)
        public async Task<JsonResult> MappingBuyerTims(string bb_no, string buyer_qr)
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

                    if (string.IsNullOrEmpty(bb_no))
                    {
                        return Json(new { result = false, message = "Vui lòng scan mã Container" }, JsonRequestBehavior.AllowGet);
                    }
                    if (string.IsNullOrEmpty(buyer_qr))
                    {
                        return Json(new { result = false, message = "Vui lòng scan mã Buyer" }, JsonRequestBehavior.AllowGet);
                    }
                    //check tồn tại buyer này chưa

                    int checkqr=await _ITimsService.GetQrcodeBuyer(buyer_qr);
                    if (checkqr > 0)
                    {
                        return Json(new { result = false, message = "Buyer Qr đã được sử dụng." }, JsonRequestBehavior.AllowGet);
                    }

                    //bool checkbuyer = _TIMSService.CheckQRBuyer(bb_no);
                    bool checkbuyer =await _ITimsService.CheckQRBuyer(bb_no);
                    if (!checkbuyer)
                    {
                        return Json(new { result = false, message = "Đồ đựng này đã được mapping với buyer!!!!." }, JsonRequestBehavior.AllowGet);

                    }

                    //var check_bobin = _TIMSService.FindOneDBobbinInfo(bb_no);
                    var check_bobin =await _ITimsService.FindOneDBobbinInfo(bb_no);
                    if (check_bobin == null)
                    {
                        return Json(new { result = false, message = "Container " + Constant.NotFound }, JsonRequestBehavior.AllowGet);
                    }
                    //var check_bobin_history = _TIMSService.GetdbobbinlcthistFrbbno(bb_no);
                    var check_bobin_history =await _ITimsService.FindAllBobbin_lct_histByBBNo(bb_no);

                    if (check_bobin_history == null)
                    {
                        return Json(new { result = false, message = "Container " + Constant.NotFound }, JsonRequestBehavior.AllowGet);
                    }

                    var sucess = 0;

                    //var check_mtcd_con = _TIMSService.CheckwmaterialinfoMappingbuyer(check_bobin_history.mt_cd, "010" , "006000000000000000").ToList();
                    var check_mtcd_con =await _ITimsService.CheckwmaterialinfoMappingbuyer(check_bobin_history.mt_cd, "010" , "006000000000000000");
                    if (check_mtcd_con.Count() == 1)
                    {
                        var check_mtcd_1 = check_mtcd_con.SingleOrDefault();
                        //kiểm tra có cùng product không

                        //string check_product = _TIMSService.GetProductWactualPrimary(check_bobin_history.mt_cd, "010", "006000000000000000");
                        string check_product =await _ITimsService.GetProductWactualPrimary(check_bobin_history.mt_cd, "010", "006000000000000000");
                        string trimmed = check_product.Replace(" ","");

                        //check product bất quy tắt

                        var typeProduct = await _ITimsService.ChecktypeProduct(check_product);
                        if (typeProduct.Equals("0"))
                        {
                            if (buyer_qr.StartsWith(trimmed.Replace("-", "")))
                            {
                                check_mtcd_1.buyer_qr = buyer_qr;
                                check_mtcd_1.buyer_code = buyer_qr;
                                //check_mtcd_1.end_production_dt = DateTime.Now;
                                check_mtcd_1.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();

                                // stamp_detail isExisted = _TIMSService.Getstampdetail(buyer_qr);
                                stamp_detail isExisted = await _ITimsService.Getstampdetail(buyer_qr);
                                if (isExisted == null)
                                {
                                    //ktra stamp_cd
                                    var stamp_cd = "";

                                    //var ktra_stamp_cd = _TIMSService.GetStyleNo(check_product);
                                    var ktra_stamp_cd = await _ITimsService.GetStyleNo(check_product);

                                    if (ktra_stamp_cd != null)
                                    {
                                        stamp_cd = ktra_stamp_cd.stamp_code;
                                        if (string.IsNullOrEmpty(stamp_cd))
                                        {
                                            return Json(new { result = false, message = "Vui Lòng Chọn kiểu tem cho Product(DMS)!!! " }, JsonRequestBehavior.AllowGet);
                                        }
                                    }

                                    //lấy ra lot_date bằng cách trừ đi product và 8 kí tự sẽ tìm được mã lot_date
                                    var prd = trimmed.Replace(" ", "");
                                    var prd1 = trimmed.Replace("-", "");
                                    //var startBien = prd1.Length + 8;
                                    //var timDZIH = buyer_qr.IndexOf("DZIH") + 4; //product+DZIH

                                    //if (timDZIH <= 0)
                                    //{
                                    //    timDZIH = buyer_qr.IndexOf("EA8D") + 4;
                                    //}
                                    var timDZIH1 = buyer_qr.IndexOf("DZIH"); //product
                                    if (timDZIH1 < 1)
                                    {
                                        timDZIH1 = buyer_qr.IndexOf("EA8D");
                                    }

                                    var timDZIH = timDZIH1 + 4; //product+DZIH

                                    var vendor_line = buyer_qr.Substring(timDZIH, 1);
                                    var label_printer = buyer_qr.Substring(timDZIH + 1, 1);
                                    var is_sample = buyer_qr.Substring(timDZIH + 2, 1);
                                    var PCN = buyer_qr.Substring(timDZIH + 3, 1);
                                    var date = buyer_qr.Substring(timDZIH + 4, 3);
                                    var lot_date = DateFormatByShinsungRule(date);

                                    var serial_number = buyer_qr.Substring(timDZIH + 7, 3); //gắn 001
                                    var machine_line = buyer_qr.Substring(timDZIH + 10, 2); //gắn 01

                                    var shift = buyer_qr.Substring(timDZIH + 12, 1);


                                    //insert stamp_detail
                                    var stamp_detail = new stamp_detail()
                                    {
                                        buyer_qr = buyer_qr,
                                        stamp_code = stamp_cd,
                                        product_code = check_product,
                                        vendor_code = ktra_stamp_cd.cust_rev,
                                        vendor_line = vendor_line,
                                        label_printer = label_printer,
                                        is_sample = is_sample,
                                        pcn = PCN,
                                        lot_date = lot_date,
                                        serial_number = serial_number,
                                        machine_line = machine_line,
                                        shift = shift,
                                        standard_qty = ktra_stamp_cd.pack_amt.HasValue ? ktra_stamp_cd.pack_amt.Value : 0,
                                        reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString(),
                                        chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString(),
                                    };

                                    //_TIMSService.Insertstampdetail(stamp_detail);
                                    await _ITimsService.Insertstampdetail(stamp_detail);

                                }
                                await _ITimsService.UpdateBuyerCodeforTims(check_mtcd_1.buyer_code, check_mtcd_1.wmtid, check_mtcd_1.chg_id);
                                //_IWOService.UpdateMaterialInfo(check_mtcd_1);
                                sucess++;
                            }
                            else
                            {
                                return Json(new { result = false, message = "Mã tem gói này không thuộc với Product của Container. " }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            check_mtcd_1.buyer_qr = buyer_qr;
                            check_mtcd_1.buyer_code = buyer_qr;
                            //check_mtcd_1.end_production_dt = DateTime.Now;
                            check_mtcd_1.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();

                            // stamp_detail isExisted = _TIMSService.Getstampdetail(buyer_qr);
                            stamp_detail isExisted = await _ITimsService.Getstampdetail(buyer_qr);
                            if (isExisted == null)
                            {
                                //ktra stamp_cd
                                var stamp_cd = "";

                                //var ktra_stamp_cd = _TIMSService.GetStyleNo(check_product);
                                var ktra_stamp_cd = await _ITimsService.GetStyleNo(check_product);

                                if (ktra_stamp_cd != null)
                                {
                                    stamp_cd = ktra_stamp_cd.stamp_code;
                                    if (string.IsNullOrEmpty(stamp_cd))
                                    {
                                        return Json(new { result = false, message = "Vui Lòng Chọn kiểu tem cho Product(DMS)!!! " }, JsonRequestBehavior.AllowGet);
                                    }
                                }

                                //lấy ra lot_date bằng cách trừ đi product và 8 kí tự sẽ tìm được mã lot_date
                                var prd = trimmed.Replace(" ", "");
                                var prd1 = trimmed.Replace("-", "");
                                //var startBien = prd1.Length + 8;
                                //var timDZIH = buyer_qr.IndexOf("DZIH") + 4; //product+DZIH

                                //if (timDZIH <= 0)
                                //{
                                //    timDZIH = buyer_qr.IndexOf("EA8D") + 4;
                                //}
                                var timDZIH1 = buyer_qr.IndexOf("DZIH"); //product
                                if (timDZIH1 < 1)
                                {
                                    timDZIH1 = buyer_qr.IndexOf("EA8D");
                                }

                                var timDZIH = timDZIH1 + 4; //product+DZIH

                                var vendor_line = buyer_qr.Substring(timDZIH, 1);
                                var label_printer = buyer_qr.Substring(timDZIH + 1, 1);
                                var is_sample = buyer_qr.Substring(timDZIH + 2, 1);
                                var PCN = buyer_qr.Substring(timDZIH + 3, 1);
                                var date = buyer_qr.Substring(timDZIH + 4, 3);
                                var lot_date = DateFormatByShinsungRule(date);

                                var serial_number = buyer_qr.Substring(timDZIH + 7, 3); //gắn 001
                                var machine_line = buyer_qr.Substring(timDZIH + 10, 2); //gắn 01

                                var shift = buyer_qr.Substring(timDZIH + 12, 1);


                                //insert stamp_detail
                                var stamp_detail = new stamp_detail()
                                {
                                    buyer_qr = buyer_qr,
                                    stamp_code = stamp_cd,
                                    product_code = check_product,
                                    vendor_code = ktra_stamp_cd.cust_rev,
                                    vendor_line = vendor_line,
                                    label_printer = label_printer,
                                    is_sample = is_sample,
                                    pcn = PCN,
                                    lot_date = lot_date,
                                    serial_number = serial_number,
                                    machine_line = machine_line,
                                    shift = shift,
                                    standard_qty = ktra_stamp_cd.pack_amt.HasValue ? ktra_stamp_cd.pack_amt.Value : 0,
                                    reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString(),
                                    chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString(),
                                };

                                //_TIMSService.Insertstampdetail(stamp_detail);
                                await _ITimsService.Insertstampdetail(stamp_detail);

                            }
                            await _ITimsService.UpdateBuyerCodeforTims(check_mtcd_1.buyer_code, check_mtcd_1.wmtid, check_mtcd_1.chg_id);
                            //_IWOService.UpdateMaterialInfo(check_mtcd_1);
                            sucess++;
                        }
                    }
                    //xóa bobin
                    //kiểm tra bobin
                    if (check_bobin != null && sucess > 0)
                    {
                        //_TIMSService.DeleteBobbinHist(check_bobin_history.blno);
                        await _ITimsService.RemoveBobbinLctHist(check_bobin_history.blno);
                        await _ITimsService.Deletedbobbininfo(check_bobin.bno);
                        //_TIMSService.Deletedbobbininfo(check_bobin.bno);
                        //if (resqheaderval[0] == "Mob")
                        if (Session["authorize"] != null)
                        {
                            return Json(new { result = true, message = Constant.Success, data = check_bobin_history, dataWeb = check_mtcd_con }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var lstres = new List<DBobbinLctHist>();

                            lstres.Add(check_bobin_history);
                            return Json(new { result = true, message = Constant.Success, data = lstres, dataWeb = check_mtcd_con }, JsonRequestBehavior.AllowGet);

                        }

                    }
                    return Json(new { result = false, message = "Không thể mapping vì chưa qua công đoạn OQC" }, JsonRequestBehavior.AllowGet);

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

        public string DateFormatByShinsungRule(string input)
        {
            var y = input.Substring(0, 1);
            var m = input.Substring(1, 1);
            var d = input.Substring(2, 1);
            string year = ChangeCharacterToNumber(y).ToString();
            string month = ChangeCharacterToNumber(m).ToString();
            string date = ChangeCharacterToNumber(d).ToString();
            StringBuilder result = new StringBuilder();
            year = "20" + year + "-";

            if (month.Length < 2)
            {
                month = "0" + month + "-";
            }
            else
            {
                month = month + "-";
            }
            if (date.Length < 2)
            {
                date = "0" + date;
            }

            result.Append(year);
            result.Append(month);
            result.Append(date);
            return result.ToString();
        }

        public int ChangeCharacterToNumber(string number)
        {
            int temp;
            if (int.TryParse(number, out temp))
            {

                return int.Parse(number);
            }

            char c = 'Z';
            char a = char.Parse(number);
            int i = 35;
            do
            {

                c--;
                i--;
            }
            while (c != a);
            return i;


        }

        public async Task<JsonResult> ShippingContainer(string wmtid)
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

                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {
                    string userid = Session["userid"] == null ? null : Session["userid"].ToString();
                    var userWIP = "";
                    if (string.IsNullOrEmpty(userid))
                    {
                        return Json(new { result = false, message = "Vui lòng đăng nhập tài khoản" }, JsonRequestBehavior.AllowGet);
                    }
                    userWIP = await _ITimsService.GetGrade(userid);
                    if (userWIP != "Admin")
                    {
                        return Json(new { result = false, message = "Tài khoản thuộc quyền Admin mới được chỉnh sửa" }, JsonRequestBehavior.AllowGet);
                    }

                    string[] temp_id = wmtid.TrimStart('[').TrimEnd(']').Split(',');

                    for (int i = 0; i < temp_id.Length; i++)
                    {
                        int aaa = await _ITimsService.UpdateCompositeShipping(int.Parse(temp_id[i]), userid, DateTime.Now.ToString("yyyyMMddHHmmss"));
                    }


                    return Json(new { result = true, message = "Xuất kho thành công" }, JsonRequestBehavior.AllowGet);
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
        #endregion TIMS_MAPPINGBUYER

        #region TIMS_DownReason

        public ActionResult DownReason()
        {
            return SetLanguage("");
        }

        //public JsonResult Get_OKReason(Pageing pageing, string mt_lot, string mt_cd, string mt_no, string bb_no, string product)
        public async Task<JsonResult> GetOkeReasonTims(Pageing pageing, string mt_lot, string mt_cd, string mt_no, string bb_no, string product)
        {
            try
            {
                OKReason resfail = new OKReason();
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

                    //WMaterialInfoTmp currentMaterial = _TIMSService.FindOneMaterialInfoByMTLot(mt_lot);
                    WMaterialInfoTmp currentMaterial =await _ITimsService.FindOneMaterialInfoByMTLot(mt_lot);
                    if (currentMaterial == null)
                    {
                        return Json(new { result = false, message = Constant.NotFound,rows= resfail }, JsonRequestBehavior.AllowGet);
                    }

                    //WActual findPO = _TIMSService.FindOneWActual(currentMaterial.id_actual);
                    var findPO =await _ITimsService.FindOneWActual((int)currentMaterial.id_actual);
                    if (findPO == null)
                    {
                        return Json(new { result = false, message = Constant.NotFound + "PO ", rows = resfail }, JsonRequestBehavior.AllowGet);
                    }

                    //WActualPrimary findPrimary = _TIMSService.FindOneWActualPrimaryByAtNo(findPO.at_no);
                    ActualPrimary findPrimary =await _ITimsService.FindOneWActualPrimaryByAtNo(findPO.at_no);
                    //var data = _TIMSService.GetOKReason(mt_lot, findPrimary.product, findPO.don_vi_pr, mt_cd, product, currentMaterial.mt_no, bb_no);
                    var data =await _ITimsService.GetOKReason(mt_lot, findPrimary.product, findPO.don_vi_pr, mt_cd, product, currentMaterial.mt_no, bb_no);
                    var records = data.Count();
                    int totalPages = (int)Math.Ceiling((float)records / pageing.rows);
                    var rowsData = data.Skip((pageing.page - 1)).Take(pageing.rows);
                    return Json(new { total = totalPages, page = pageing.page, records = records, rows = rowsData }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize, rows = resfail }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ActionResult> check_update_grty(string mt_cd, int value, string reason)
        {
            //check  không qua công đoạn trước  và chưa kiểm tra PQC
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
                    if (mt_cd.Contains("ROT") || mt_cd.Contains("STA"))
                    {
                        var checkedTims = await _ITimsService.CheckMt_cdInTims(mt_cd);
                        if (checkedTims <= 0)
                        {
                            return Json(new { result = false, message = "Liệu Chưa được nhận ở kho Tims" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    var datatims = await _ITimsService.GetWMaterialInfoWithmtcdLike(mt_cd);
                    var datamms = await _ITimsService.GetWMaterialInfoWithmtcdLikemms(mt_cd);
                    //if (datatims == null)
                    //{

                    //    return Json(new { result = false, kq = Constant.NotFound, wmtid = datatims.FirstOrDefault().wmtid }, JsonRequestBehavior.AllowGet);
                    //}
                    //var mtmpp = await _ITimsService.GetMaterialMappingTIMSByLot(mt_cd);
                    //if (mtmpp.Count() <0)
                    //{
                    //    return Json(new { result = false, kq = Constant.Other, wmtid = data.FirstOrDefault().wmtid }, JsonRequestBehavior.AllowGet);
                    //}
                    if (datatims.Count > 0)
                    {
                        //insert bảng giảm có lí do
                        var w_material_down = new MaterialDown();
                        w_material_down.gr_down = value;
                        w_material_down.mt_cd = datatims.FirstOrDefault().material_code;
                        w_material_down.reason = reason;
                        w_material_down.bb_no = datatims.FirstOrDefault().bb_no;
                        w_material_down.status_now = datatims.FirstOrDefault().status;
                        w_material_down.gr_qty = (double)datatims.FirstOrDefault().gr_qty;
                        w_material_down.chg_dt = DateTime.Now.ToString();
                        w_material_down.reg_dt = DateTime.Now.ToString();
                        w_material_down.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        w_material_down.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();

                        await _IActualWOService.InsertMaterialDown(w_material_down);

                        datatims.FirstOrDefault().gr_qty = value;
                        datatims.FirstOrDefault().chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        await _ITimsService.UpdateQtyMaterialTims(datatims.FirstOrDefault().gr_qty, datatims.FirstOrDefault().wmtid, datatims.FirstOrDefault().chg_id);
                        return Json(new { result = true, wmtid = datatims.FirstOrDefault().wmtid }, JsonRequestBehavior.AllowGet);
                    }
                    if (datamms != null)
                    {
                        //insert bảng giảm có lí do
                        var w_material_down = new MaterialDown();
                        w_material_down.gr_down = value;
                        w_material_down.mt_cd = datamms.material_code;
                        w_material_down.reason = reason;
                        w_material_down.bb_no = datamms.bb_no;
                        w_material_down.status_now = datamms.mt_sts_cd;
                        w_material_down.gr_qty = (double)datamms.gr_qty;
                        w_material_down.chg_dt = DateTime.Now.ToString();
                        w_material_down.reg_dt = DateTime.Now.ToString();
                        w_material_down.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        w_material_down.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();

                        await _IActualWOService.InsertMaterialDown(w_material_down);

                        datamms.gr_qty = value;
                        datamms.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        await _ITimsService.UpdateQtyMaterialMMS(Convert.ToInt32(datamms.gr_qty), datamms.wmtid, datamms.chg_id);
                        return Json(new { result = true, wmtid = datamms.wmtid }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, kq = Constant.NotFound}, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<ActionResult> CheckUpdateGrtyApi(string container, int value, string reason)
        {
            //check  không qua công đoạn trước  và chưa kiểm tra PQC
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

                    var data = await _ITimsService.FindAllBobbin_lct_histByBBNo(container);// db.d_bobbin_lct_hist.Where(x => x.bb_no == container).ToList();
                    if (data == null)
                    {
                        return Json(new { result = false, message = Constant.NotFound + " Container " }, JsonRequestBehavior.AllowGet);
                    }
                    var mt_cd = data.mt_cd;
                    if (mt_cd.Contains("ROT") || mt_cd.Contains("STA"))
                    {
                        var checkedTims = await _ITimsService.CheckMt_cdInTims(mt_cd);
                        if (checkedTims <= 0)
                        {
                            return Json(new { result = false, message = "Liệu Chưa được nhận ở kho Tims" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    var datatims = await _ITimsService.GetWMaterialInfoWithmtcdLike(mt_cd);
                    var datamms = await _ITimsService.GetWMaterialInfoWithmtcdLikemms(mt_cd);
                    if (datatims.Count > 0)
                    {
                        if (datatims.FirstOrDefault().real_qty <= 0)
                        {
                            return Json(new { result = false, message = "Mã này chưa có số lượng để thay đổi", wmtid = datatims.FirstOrDefault().wmtid }, JsonRequestBehavior.AllowGet);
                        }
                        //insert bảng giảm có lí do
                        var w_material_down = new MaterialDown();
                        w_material_down.gr_down = value;
                        w_material_down.mt_cd = datatims.FirstOrDefault().material_code;
                        w_material_down.reason = reason;
                        w_material_down.bb_no = datatims.FirstOrDefault().bb_no;
                        w_material_down.status_now = datatims.FirstOrDefault().status;
                        w_material_down.gr_qty = (double)datatims.FirstOrDefault().gr_qty;
                        w_material_down.chg_dt = DateTime.Now.ToString();
                        w_material_down.reg_dt = DateTime.Now.ToString();
                        w_material_down.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        w_material_down.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();

                        await _IActualWOService.InsertMaterialDown(w_material_down);

                        datatims.FirstOrDefault().gr_qty = value;
                        datatims.FirstOrDefault().chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        await _ITimsService.UpdateQtyMaterialTims(datatims.FirstOrDefault().gr_qty, datatims.FirstOrDefault().wmtid, datatims.FirstOrDefault().chg_id);
                        return Json(new { result = true, message = Constant.Success, wmtid = datatims.FirstOrDefault().wmtid }, JsonRequestBehavior.AllowGet);
                    }
                    if (datamms != null)
                    {
                        if (datamms.real_qty <= 0)
                        {
                            return Json(new { result = false, message = "Mã này chưa có số lượng để thay đổi", wmtid = datamms.wmtid }, JsonRequestBehavior.AllowGet);
                        }
                        //insert bảng giảm có lí do
                        var w_material_down = new MaterialDown();
                        w_material_down.gr_down = value;
                        w_material_down.mt_cd = datamms.material_code;
                        w_material_down.reason = reason;
                        w_material_down.bb_no = datamms.bb_no;
                        w_material_down.status_now = datamms.mt_sts_cd;
                        w_material_down.gr_qty = (double)datamms.gr_qty;
                        w_material_down.chg_dt = DateTime.Now.ToString();
                        w_material_down.reg_dt = DateTime.Now.ToString();
                        w_material_down.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        w_material_down.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();

                        await _IActualWOService.InsertMaterialDown(w_material_down);

                        datamms.gr_qty = value;
                        datamms.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        await _ITimsService.UpdateQtyMaterialMMS(Convert.ToInt32(datamms.gr_qty), datamms.wmtid, datamms.chg_id);
                        return Json(new { result = true, message = Constant.Success, wmtid = datamms.wmtid }, JsonRequestBehavior.AllowGet);
                    }

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

        //[HttpGet]
        public async Task<JsonResult> GetReasonDown(string mt_cd)
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
                var result = await _ITimsService.GetMaterialDownByCode(mt_cd);
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion TIMS_DownReason

        #region General

        public ActionResult General()
        {
            return SetLanguage("~/Views/TIMS/Inventory/General.cshtml");
        }

        public async Task<JsonResult> destroyLotCode(int id, string MaterialCode)
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

                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {

                    string user = Session["userid"] == null ? null : Session["userid"].ToString();
                    var userWIP = "";
                    if (!string.IsNullOrEmpty(user))
                    {
                        userWIP = await _ITimsService.GetGrade(user);
                        if (userWIP != "Admin")
                        {
                            return Json(new { result = false, message = "Tài khoản thuộc quyền Admin mới được chỉnh sửa" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
                    }
                    var checkTIMS = await _ITimsService.CheckwmaterialinfoMappingbuyerwmtid(id, MaterialCode, "006");
                    var checkMMS = await _ITimsService.CheckBobbinDestroyMMS(id, MaterialCode, "002");


                    if (checkTIMS.Count() > 0)
                    {
                        var chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                        var sts_update = checkTIMS.FirstOrDefault().status;
                        var mt_cd = checkTIMS.FirstOrDefault().material_code;
                        var bb_no = checkTIMS.FirstOrDefault().bb_no;
                        int result = await _ITimsService.SPUpdateDestroy(id, sts_update, mt_cd, bb_no, chg_id);

                        if (result > 0)
                        {
                            var list2 = await _ITimsService.Getviewtimsinventorygeneral(id);
                            return Json(new { result = true, message = Constant.Success, data = list2 }, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new { result = false, message = Constant.CannotCancel }, JsonRequestBehavior.AllowGet);
                    }
                    if (checkMMS.Count() > 0)
                    {
                        var chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();

                        var sts_update = checkMMS.FirstOrDefault().status;
                        var mt_cd = checkMMS.FirstOrDefault().material_code;
                        var bb_no = checkMMS.FirstOrDefault().bb_no;
                        int result = await _ITimsService.SPUpdateDestroyMMS(id, sts_update, mt_cd, bb_no, chg_id);

                        if (result > 0)
                        {
                            var list2 = await _ITimsService.Getviewtimsinventorygeneral(id);
                            return Json(new { result = true, message = Constant.Success, data = list2 }, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new { result = false, message = Constant.CannotCancel }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> redoLotCode(int id, string MaterialCode)
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

                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {
                    var checkTIMS = await _ITimsService.Getwmtinfotimswithwmtid(id, "011", "006", MaterialCode);
                    var checkMMS = await _ITimsService.CheckBobbinRedoMMS(id, "011", MaterialCode);
                    //+ Chức năng trở về trạng thái cũ

                    if (checkTIMS != null)
                    {
                        var chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                        var sts_update = checkTIMS.sts_update;
                        var bb_no = checkTIMS.bb_no;
                        var material_code = checkTIMS.material_code;
                        int result = await _ITimsService.SPUpdatesRedo(id, sts_update, chg_id);
                        if (result > 0)
                        {
                            //var list2 = await _ITimsService.Getviewtimsinventorygeneral(id);
                            var checkbbhist = await _ITimsService.FindAllBobbin_lct_histByBBNo(bb_no);
                            if (checkbbhist == null && !(string.IsNullOrEmpty(bb_no)))
                            {
                                var list_data_bobin = await _ITimsService.GetBobbinInfo(bb_no);

                                var history = new BobbinLctHist();

                                history.bb_no = checkTIMS.bb_no;
                                history.bb_nm = list_data_bobin.bb_nm;
                                history.mc_type = list_data_bobin.mc_type;
                                history.mt_cd = checkTIMS.material_code;
                                history.use_yn = "Y";
                                history.del_yn = "N";
                                history.chg_dt = DateTime.Now;
                                history.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                history.reg_dt = DateTime.Now;
                                history.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                int resss = _ITimsService.InsertIntoBobbinLctHist(history);
                                await _ITimsService.UpdatemtcdforBobbinInfoForRedo(checkTIMS.bb_no, checkTIMS.material_code);
                            }
                            return Json(new { result = true, message = Constant.Success }, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new { result = false, message = Constant.CannotRedo }, JsonRequestBehavior.AllowGet);
                    }

                    if (checkMMS != null)
                    {
                        var chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                        var sts_update = checkMMS.sts_update;
                        var bb_no = checkMMS.bb_no;
                        var mt_cd = checkMMS.mt_cd;
                        int result = await _ITimsService.SPUpdatesRedoMMS(id, sts_update, chg_id);
                        if (result > 0)
                        {
                            //var sql_insert = new StringBuilder();
                            //var list2 = await _ITimsService.Getviewtimsinventorygeneral(id);
                            var checkbbhist = await _ITimsService.FindAllBobbin_lct_histByBBNo(bb_no);
                            if (checkbbhist == null && !(string.IsNullOrEmpty(bb_no)))
                            {
                                var list_data_bobin = await _ITimsService.GetBobbinInfo(bb_no);

                                var history = new BobbinLctHist();

                                history.bb_no = checkMMS.bb_no;
                                history.bb_nm = list_data_bobin.bb_nm;
                                history.mc_type = list_data_bobin.mc_type;
                                history.mt_cd = checkMMS.mt_cd;
                                history.use_yn = "Y";
                                history.del_yn = "N";
                                history.chg_dt = DateTime.Now;
                                history.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                history.reg_dt = DateTime.Now;
                                history.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                int resss = _ITimsService.InsertIntoBobbinLctHist(history);
                                await _ITimsService.UpdatemtcdforBobbinInfoForRedo(checkMMS.bb_no, checkMMS.mt_cd);
                            }
                            return Json(new { result = true, message = Constant.Success }, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new { result = false, message = Constant.CannotRedo }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { result = false, message = Constant.CannotRedo }, JsonRequestBehavior.AllowGet);
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

        public async Task<JsonResult> redoLotCodeDMS(int id,string bobbin)
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

                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {
                    var checkTIMS = await _ITimsService.GetwmtinfotimswithBobbin("011", bobbin);
                    var checkMMS = await _ITimsService.GetwmtinfoMMSwithBobbin("011", bobbin);
                    //+ Chức năng trở về trạng thái cũ

                    if (checkTIMS != null)
                    {
                        var chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                        var sts_update = checkTIMS.sts_update;
                        var bb_no = checkTIMS.bb_no;
                        var material_code = checkTIMS.material_code;
                        int result = await _ITimsService.SPUpdatesRedo(id, sts_update, chg_id);
                        if (result > 0)
                        {
                            if (sts_update.Equals("010"))
                            {
                                return Json(new { result = true, message = Constant.Success }, JsonRequestBehavior.AllowGet);
                            }
                            //var list2 = await _ITimsService.Getviewtimsinventorygeneral(id);
                            var checkbbhist = await _ITimsService.FindAllBobbin_lct_histByBBNo(bb_no);
                            if (checkbbhist == null && !(string.IsNullOrEmpty(bb_no)))
                            {

                                var list_data_bobin = await _ITimsService.GetBobbinInfo(bb_no);

                                var history = new BobbinLctHist();
                                if (list_data_bobin != null)
                                {
                                    history.bb_nm = list_data_bobin.bb_nm;
                                    history.mc_type = list_data_bobin.mc_type;
                                }
                                history.bb_no = checkTIMS.bb_no;
                               
                                history.mt_cd = checkTIMS.material_code;
                                history.use_yn = "Y";
                                history.del_yn = "N";
                                history.chg_dt = DateTime.Now;
                                history.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                history.reg_dt = DateTime.Now;
                                history.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                int resss = _ITimsService.InsertIntoBobbinLctHist(history);
                                await _ITimsService.UpdatemtcdforBobbinInfoForRedo(checkTIMS.bb_no, checkTIMS.material_code);
                            }
                            return Json(new { result = true, message = Constant.Success }, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new { result = false, message = Constant.CannotRedo }, JsonRequestBehavior.AllowGet);
                    }

                    if (checkMMS != null)
                    {
                        var chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                        var sts_update = checkMMS.sts_update;
                        var bb_no = checkMMS.bb_no;
                        var mt_cd = checkMMS.material_code;
                        int result = await _ITimsService.SPUpdatesRedoMMS(id, sts_update, chg_id);
                        if (result > 0)
                        {
                            //var sql_insert = new StringBuilder();
                            //var list2 = await _ITimsService.Getviewtimsinventorygeneral(id);
                            var checkbbhist = await _ITimsService.FindAllBobbin_lct_histByBBNo(bb_no);
                            if (checkbbhist == null && !(string.IsNullOrEmpty(bb_no)))
                            {
                                var list_data_bobin = await _ITimsService.GetBobbinInfo(bb_no);

                                var history = new BobbinLctHist();

                                history.bb_no = checkMMS.bb_no;
                                history.bb_nm = list_data_bobin.bb_nm;
                                history.mc_type = list_data_bobin.mc_type;
                                history.mt_cd = mt_cd;
                                history.use_yn = "Y";
                                history.del_yn = "N";
                                history.chg_dt = DateTime.Now;
                                history.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                history.reg_dt = DateTime.Now;
                                history.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                int resss = _ITimsService.InsertIntoBobbinLctHist(history);
                                await _ITimsService.UpdatemtcdforBobbinInfoForRedo(checkMMS.bb_no, mt_cd);
                            }
                            return Json(new { result = true, message = Constant.Success }, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new { result = false, message = Constant.CannotRedo }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { result = false, message = Constant.CannotRedo }, JsonRequestBehavior.AllowGet);
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

        public async Task<JsonResult> GetInventory(string mtCode, string mtNo, string sVBobbinCd, string recDate, int view, string mtNoSpecific, string prd_cd, string recDateStart, string recDateEnd)
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

                //StringBuilder sql = new StringBuilder($"Call spTIMS_InventoryGeneral_Union('{mtCode}','{mtNo}','{sVBobbinCd}','{recDate}','{prd_cd}');");
                var listTotals = await _ITimsService.GetspTIMSInventoryGeneralUnion(mtCode, mtNo, sVBobbinCd, recDate, prd_cd);
                List<TIMSInventoryModel> listTotal = listTotals.ToList();
                List<TIMSInventoryModel> listInfo = new List<TIMSInventoryModel>();
                List<TIMSInventoryModel> listDetailAll = new List<TIMSInventoryModel>();
                List<TIMSInventoryModel> listDetailSpecific = new List<TIMSInventoryModel>();
                string mtNoTemp = "";
                foreach (var item in listTotal)
                {
                    if (string.IsNullOrEmpty(item.MaterialCode))
                    {
                        listInfo.Add(item);
                    }
                    else
                    {
                        listDetailAll.Add(item);
                    }
                }
                if (view == 1 && string.IsNullOrEmpty(mtCode))
                {
                    return Json(new { listInfo, mtNoTemp }, JsonRequestBehavior.AllowGet);
                }

                if (view == 1 && (!string.IsNullOrEmpty(mtCode)))
                {
                    foreach (var item in listDetailAll)
                    {
                        if (item.MaterialCode.Contains(mtCode))
                        {
                            listDetailSpecific.Add(item);
                        }
                    }
                    if (listDetailSpecific.Count > 0)
                    {
                        mtNoTemp = listDetailSpecific[0].MaterialNo;
                    }
                    return Json(new { listInfo, mtNoTemp }, JsonRequestBehavior.AllowGet);
                }

                //if view = 2
                foreach (var item in listDetailAll)
                {
                    if (item.MaterialNo == mtNoSpecific)
                    {
                        listDetailSpecific.Add(item);
                    }
                }
                return Json(listDetailSpecific, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> getInventoryInfo(string mtCode, string sVBobbinCd, string prd_cd, string recDateStart, string recDateEnd, string mtNoSpecific, string status,string bom_type, string model, string po)
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
                if(status== "008,002,010,009,012")
                {
                    status = null;
                }
                var result = await _ITimsService.GetInventoryInfoGenerals(mtCode, sVBobbinCd, prd_cd, recDateStart,recDateEnd, mtNoSpecific, status,bom_type, po,model);
                return Json(new { listInfo = result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetInventoryDetail(string prd_cd, string mtCode, string sVBobbinCd, string recDateStart, string recDateEnd, string ProSpecific, string status, string po)
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

                var result = await _ITimsService.GetInventoryDetail(prd_cd, mtCode, sVBobbinCd, recDateStart, recDateEnd, ProSpecific, status, po);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> PrintExcelFile(string status, string po, string model, string prd_cd, string bom_type, string mt_cd, string VBobbinCd, string endDate, string startDate)
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
                if(status== "008,002,010,009,012")
                {
                    status = "";
                }
                var listTotals = await _ITimsService.GetspTIMSInventoryGeneralExcel(status, po,  model,  prd_cd,  bom_type,  mt_cd,  VBobbinCd,  endDate,  startDate);

                var values = listTotals.ToList().AsEnumerable().Select(x => new
                {
                    md_cd = x.md_cd,
                    product_cd = x.product_cd,
                    product_nm = x.product_nm,
                    bom_type = x.bom_type,
                    HCK = x.HCK,
                    DKT = x.DKT,
                    HDG = x.HDG,
                    MAPPINGBUYER = x.MAPPINGBUYER, /*((x.bundle_unit != null || x.bundle_unit != "") ? _IdevManagementService.GetDetailNameFromCom_DT("COM027", x.bundle_unit): null),*/
                    CKT = x.CKT,
                    SORTING = x.SORTING,
                    VBobbinCd = x.VBobbinCd,
                    at_no = x.at_no,
                    buyer_qr = x.buyer_qr,
                    MaterialCode = x.MaterialCode,
                    Length = x.Length,
                    StatusName = x.StatusName,
                    ReceivedDate = x.ReceivedDate,
                }).ToArray();

                String[] labelList = new string[17] { "Model", "Product Code", "product name", "BOM type", "Hàng CHờ kiểm", "Hàng đang kiểm tra","Hàng Đóng gói", "Hàng Mapping Buyer", "Hàng chờ vào QQC", "SORTING", "Container", "PO", "buyer_qr", "Composite Code", "Length", "Status Name", "Received Date" };

                //String[] labelList = new string[7] { "PO", "Product Code", "Product Name", "Lot date", "Buyer QR", "Bobbin", "Quantity" };

                Response.ClearContent();

                Response.AddHeader("content-disposition", "attachment;filename=TIMS_Inventory_General.xls");

                Response.AddHeader("Content-Type", "application/vnd.ms-excel");

                new InitMethods().WriteHtmlTable(values, Response.Output, labelList);

                Response.End();


                return View("~/Views/TIMS/Inventory/General.cshtml");
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }
        private void WriteHtmlTable<T>(IEnumerable<T> data, TextWriter output, String[] labelList)
        {
            //Writes markup characters and text to an ASP.NET server control output stream. This class provides formatting capabilities that ASP.NET server controls use when rendering markup to clients.
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {

                    Table table = new Table();
                    TableRow row = new TableRow();
                    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

                    foreach (String label in labelList)
                    {
                        TableHeaderCell hcell = new TableHeaderCell();
                        hcell.Text = label;

                        hcell.Font.Bold = true;
                        row.Cells.Add(hcell);
                        row.BorderStyle = BorderStyle.Solid;
                    }
                    table.Rows.Add(row);

                    foreach (T item in data)
                    {
                        row = new TableRow();
                        foreach (PropertyDescriptor prop in props)
                        {
                            TableCell cell = new TableCell();
                            cell.Text = prop.Converter.ConvertToString(prop.GetValue(item));
                            row.Cells.Add(cell);
                            row.BorderStyle = BorderStyle.Solid;
                        }
                        table.Rows.Add(row);
                    }

                    table.RenderControl(htw);

                    output.Write(sw.ToString());
                }
            }
        }
        public async Task<ActionResult> Search_bbTimsGeneral(string bb_no)
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

                    if (string.IsNullOrEmpty(bb_no))
                    {

                        return Json(new { result = false, message = "Vui lòng SCan lại" }, JsonRequestBehavior.AllowGet);
                    }
                    var kttt = await _ITimsService.GetWmtInfoTimsWithbbno(bb_no);// db.w_material_info.Where(x => x.bb_no == bb_no).ToList();
                    if (kttt.Count == 0)
                    {
                        return Json(new { result = false, message = "Không tìm thấy mã Container" }, JsonRequestBehavior.AllowGet);
                    }
                    var kt_tims = kttt.Where(x => x.bb_no == bb_no && x.location_code.StartsWith("006")).ToList();
                    if (kt_tims.Count == 0)//ko co trong phong tims
                    {
                        var ktStatus = kttt.Where(x => x.bb_no == bb_no).FirstOrDefault();
                        var lctinfo = await _ITimsService.Getlctinfo(ktStatus.location_code);
                        var lctName = lctinfo.lct_nm;

                        var StatusName = await _ITimsService.GetNameStatusCommCode(ktStatus.mt_sts_cd);

                        return Json(new { result = false, message = "Mã này đang ở trong kho" + lctName + "Trạng thái là:" + StatusName }, JsonRequestBehavior.AllowGet);
                    }
                    var data = kt_tims.Where(x => x.bb_no == bb_no).FirstOrDefault();
                    return Json(new { result = true, data = data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                return Json(new { result = false, message = "Không tồn tại" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion General

        #region PRINT NG (TIMS)

        public ActionResult Print_NG()
        {
            return SetLanguage("~/Views/TIMS/Actual/PrintNG/PrintNG.cshtml");
        }

        [HttpGet]
        public async Task<JsonResult> GetTIMSMaterialNG(Pageing paging, string code, string at_no, string product)
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

            var result = await _ITimsService.GetListDataPrintNGTims(code,at_no,product);
            return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = new List<PrintNGTimsReponse> ();
                var jsonReturn = new
                {
                    result = false,
                    total = 0,
                    page = 0,
                    records = 0,
                    rows = data,
                    message = Constant.ErrorAuthorize
                };
                return Json(jsonReturn, JsonRequestBehavior.AllowGet);
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

                var result = await _ITimsService.GetListDataPrintNGOK();

                int start = (paging.page - 1) * paging.rows;
                int totals = result.Count();
                int totalPages = (int)Math.Ceiling((float)totals / paging.rows);
                IEnumerable<MaterialInfoTIMS> dataactual = result.Skip<MaterialInfoTIMS>(start).Take(paging.rows);
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
                var data = new List<MaterialInfoTIMS>();
                var jsonReturn = new
                {
                    result = false,
                    total = 0,
                    page = 0,
                    records = 0,
                    rows = data,
                    message = Constant.ErrorAuthorize
                };
                return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> Change_OK_NG(string mt_cd, double gr_qty, string reason, string buyer_qr)
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

                var type = "CP";
                var result = await _ITimsService.GetMaterialInfoTimsByCode(mt_cd);
                //var find_ng = db.w_material_info.Where(x => x.mt_cd == mt_cd && x.mt_sts_cd == "003").SingleOrDefault();
                if (!string.IsNullOrEmpty(buyer_qr))
                {
                    //find_ng = db.w_material_info.Where(x => x.buyer_qr == buyer_qr && x.mt_sts_cd == "003").SingleOrDefault();
                    result = await _ITimsService.GetMaterialInfoTimsByBuyerQR(buyer_qr);
                    type = "BR";
                }
                var find_ng = result.SingleOrDefault();
                if (find_ng == null)
                {
                    return Json(new { result = false, message = " NG" + Constant.NotFound }, JsonRequestBehavior.AllowGet);
                }
                if (find_ng.gr_qty < gr_qty)
                {
                    return Json(new { result = false, message = Constant.QuantityNGInvalid }, JsonRequestBehavior.AllowGet);
                }
                //trừ mã cũ

                //insert bảng giảm có lí do
                var w_material_down = new MaterialDown();
                w_material_down.gr_down = (double)find_ng.gr_qty - gr_qty;
                w_material_down.mt_cd = find_ng.material_code;
                w_material_down.reason = reason;
                w_material_down.bb_no = find_ng.bb_no;
                w_material_down.status_now = find_ng.status;
                w_material_down.gr_qty = (double)find_ng.gr_qty;
                w_material_down.chg_dt = DateTime.Now.ToString();
                w_material_down.reg_dt = DateTime.Now.ToString();
                w_material_down.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                w_material_down.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();

                var wmtid = await _ITimsService.InsertIntoMaterialDown(w_material_down);

                var result_gr_qty = find_ng.gr_qty - Convert.ToInt32(gr_qty);
                var result_chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                if (ModelState.IsValid)
                {
                    await _ITimsService.UpdateGrqtyMaterialInfoTims(find_ng.material_code, result_gr_qty, find_ng.status, result_chg_id);
                    //db.Entry(find_ng).State = EntityState.Modified;
                    //db.SaveChanges();

                    var ok = new Models.NewVersion.MaterialInfoTIMS();
                    if (type == "CP")
                    {
                        //tạo mã ok mới dựa trên mã ng
                        //var check = db.w_material_info.Where(x => x.mt_cd.StartsWith(find_ng.mt_cd + "-OK")).ToList();
                        var check = await _ITimsService.GetMaterialWithStatusOk(find_ng.material_code);
                        var dem = (check.Count() > 0 ? check.Count().ToString() : "");
                        ok.material_code = find_ng.material_code + "-OK" + dem;
                    }
                    else
                    {
                        // tao ra ma moi
                        var buyer_new = find_ng.buyer_code + "-BROK";
                        //var danhsach = db.w_material_info.Where(x => x.mt_cd.StartsWith(buyer_new)).ToList();
                        var danhsach = await _ITimsService.GetMaterialInfoTimsByBuyerCode(buyer_new);
                        buyer_new = danhsach.Count() == 0 ? buyer_new : buyer_new + danhsach.Count();
                        ok.material_code = buyer_new;
                    }

                    ok.id_actual = find_ng.id_actual;
                    ok.orgin_mt_cd = find_ng.material_code;
                    ok.gr_qty = Convert.ToInt32(gr_qty);
                    ok.real_qty = ok.gr_qty;
                    // ok.mt_barcode = ok.mt_cd;
                    // ok.mt_qrcode = ok.mt_cd;
                    ok.location_code = "006000000000000000";
                    ok.material_type = find_ng.material_type;
                    ok.mt_no = find_ng.mt_no;
                    ok.status = "012";
                    //ok.use_yn = "Y";
                    ok.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    ok.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    ok.chg_date = DateTime.Now;
                    ok.reg_date = DateTime.Now;

                    var id = await _ITimsService.InsertMaterialInfoTIMMS(ok);

                    Log.Info("TIMS=>Change_OK_NG add mt_cd ok: " + ok.material_code + " SLok: " + ok.gr_qty + "NG: " + find_ng.material_code + " SLok: " + find_ng.gr_qty);
                    return Json(new { result = true, kq = ok, message = "Scan NVL thành công!" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
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

                //check mã tồn tại không
                //var find_lot = db.w_material_info.Where(x => x.mt_cd == mt_cd).SingleOrDefault();
                var find_lot = await _ITimsService.GetWMaterialInfowithmtcd(mt_cd);

                if (find_lot == null)
                {
                    return Json(new { result = false, message = "LOT " + Constant.NotFound }, JsonRequestBehavior.AllowGet);
                }
                if (find_lot.status != "012")
                {
                    return Json(new { result = false, message = Constant.Status }, JsonRequestBehavior.AllowGet);
                }
                //kiếm con NG cũ
                var rs = await _ITimsService.GetMaterialInfoMMSByOrgin(find_lot.orgin_mt_cd);
                var find_lotng = rs.SingleOrDefault();
                if (find_lot == null)
                {
                    return Json(new { result = false, message = "LOT " + Constant.NotFound }, JsonRequestBehavior.AllowGet);
                }
                //updtae lại số lượng ng
                var gr_qty = find_lotng.gr_qty + find_lot.gr_qty;
                var chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                await _ITimsService.UpdateGroupQTYRealQTYTims(gr_qty,find_lot.real_qty,find_lotng.material_code,chg_id);
                Log.Info("TIMS=>Change_OK_NG THAY DOI mt_cd : " + find_lotng.material_code + " SL: " + find_lotng.gr_qty);

                //xóa mã ok
                await _ITimsService.RemoveMaterialInfoTims(find_lot.wmtid);
                return Json(new { result = true, message = "Cancel thành công !!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
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
            return View("~/Views/TIMS/Actual/PrintNG/QRPrintNG.cshtml");
        }

        [HttpGet]
        public async Task<JsonResult> QRMaterialNGPrint(string id)
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

                var rs = id.Split(',');
                var listId = rs.ToList();
                var result = await _ITimsService.PrintQRCodeMaterial(listId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }


        //public async Task<ActionResult> InsertQcPhanLoai(List<Insert_MPO_QC_Model> model, string at_no, string ml_tims, string date_ymd, int gr_qty, string item_vcd, m_facline_qc MFQC, m_facline_qc_value MFQCV)
        //{
        //    var resqheader =Request.Headers;string[] resqheaderkey = resqheader.AllKeys;string[] resqheaderval = resqheader.GetValues("requestFrom"); if (Session["authorize"] != null || resqheaderval[0]== "Mob")
        //    {

        //        #region insert info

        //        var checkk = await _ITimsService.GetListDataFaclineQcwithmltims(ml_tims);
        //        var check = checkk.FirstOrDefault();
        //        DateTime reg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
        //        string datestring = reg_dt.ToString("yyyy-MM-dd HH:mm:ss.FFF");
        //        DateTime chg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
        //        string dateChstring = chg_dt.ToString("yyyy-MM-dd HH:mm:ss.FFF");
        //        if (check == null)
        //        {
        //            //var list = db.m_facline_qc.Where(x => x.fq_no.StartsWith("TI")).OrderBy(x => x.fq_no).ToList();
        //            var list = await _ITimsService.GetListDataFaclineQcwithfqno("TI");
        //            if (list.Count() == 0)
        //            {
        //                MFQC.fq_no = "TI000000001";
        //            }
        //            else
        //            {
        //                var menuCd = string.Empty;
        //                var subMenuIdConvert = 0;
        //                var list1 = list.FirstOrDefault();

        //                var bien1 = list1.fq_no;
        //                var subMenuId = bien1.Substring(bien1.Length - 9, 9);
        //                int.TryParse(subMenuId, out subMenuIdConvert);
        //                menuCd = "TI" + string.Format("{0}{1}", menuCd, CreateFQ((subMenuIdConvert + 1)));
        //                MFQC.fq_no = menuCd;
        //            }
        //            //lấy product
        //            var prd_cd = "";
        //            var ktprd_cd = await _ITimsService.FindOneWActualPrimaryByAtNo(at_no);
        //            if (ktprd_cd != null)
        //            {
        //                prd_cd = ktprd_cd.product;
        //            }
        //            // LẤY sHILF
        //            var ca = "";
        //            if (ml_tims.Contains("-CN"))
        //            {
        //                ca = "CN";
        //            }
        //            if (ml_tims.Contains("-CD"))
        //            {
        //                ca = "CD";
        //            }


        //            MFQC.check_qty = gr_qty;
        //            MFQC.ml_tims = ml_tims;
        //            MFQC.at_no = at_no;
        //            MFQC.product_cd = prd_cd;
        //            MFQC.shift = ca;
        //            MFQC.work_dt = DateTime.Now.ToString("yyyyMMddHHmmss");
        //            MFQC.reg_dt = reg_dt;
        //            MFQC.chg_dt = chg_dt;
        //            var item = await _ITimsService.Getqcitemmt(item_vcd);
        //            MFQC.item_nm = item.item_nm;
        //            MFQC.item_exp = item.item_exp;

        //            MFQC.reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
        //            MFQC.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
        //            await _ITimsService.InsertMFaclineQC(MFQC);
        //                //db.Entry(MFQC).State = EntityState.Added;
        //                //list.Add(MFQC);
        //                //db.SaveChanges();


        //            if (model != null)
        //            {
        //                foreach (var item2 in model)
        //                {
        //                    if (item2.objTr != null)
        //                    {
        //                        foreach (var item3 in item2.objTr)
        //                        {
        //                            if (item3.input > 0)
        //                            {
        //                                var list_qc_itemcheck_dt = await _ITimsService.Getqcitemcheckdt(Convert.ToInt32(item3.icdno));

        //                                MFQCV.fq_no = MFQC.fq_no;
        //                                MFQCV.check_value = list_qc_itemcheck_dt.check_name;
        //                                MFQCV.check_cd = list_qc_itemcheck_dt.check_cd;
        //                                MFQCV.check_qty = Convert.ToInt32(item3.input);
        //                                MFQCV.check_id = list_qc_itemcheck_dt.check_id;
        //                                MFQCV.date_ymd = date_ymd;
        //                                MFQCV.reg_dt = reg_dt;
        //                                MFQCV.chg_dt = chg_dt;

        //                                MFQCV.reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
        //                                MFQCV.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();

        //                                await _ITimsService.inertmfaclineqcvalue(MFQCV);
        //                            }
        //                        }
        //                    }
        //                }


        //                return Json(new { result = true, MFQC }, JsonRequestBehavior.AllowGet);
        //            }
        //        }

        //        else
        //        {
        //            //insert m_facline_qc_value
        //            if (model != null)
        //            {
        //                foreach (var item2 in model)
        //                {
        //                    if (item2.objTr != null)
        //                    {
        //                        foreach (var item3 in item2.objTr)
        //                        {
        //                            if (item3.input > 0)
        //                            {
        //                                //var list_qc_itemcheck_dt = db.qc_itemcheck_dt.Find(Convert.ToInt32(item3.icdno));
        //                                var list_qc_itemcheck_dt = await _ITimsService.Getqcitemcheckdt(Convert.ToInt32(item3.icdno));
        //                                MFQCV.fq_no = check.fq_no;
        //                                MFQCV.check_value = list_qc_itemcheck_dt.check_name;
        //                                MFQCV.check_cd = list_qc_itemcheck_dt.check_cd;
        //                                MFQCV.check_qty = Convert.ToInt32(item3.input);
        //                                MFQCV.check_id = list_qc_itemcheck_dt.check_id;
        //                                MFQCV.date_ymd = date_ymd;
        //                                MFQCV.reg_dt = reg_dt;
        //                                MFQCV.chg_dt = chg_dt;

        //                                MFQCV.reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
        //                                MFQCV.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
        //                                check.reg_dt = DateTime.Now;
        //                                //MFQCV.id=await _ITimsService.inertmfaclineqcvalue(MFQCV);

        //                            }
        //                        }
        //                    }
        //                }

        //                //update update check_Qty [m_facline_qc]
        //                var soluongdefect = await _ITimsService.GetQTYmfaclineqcvalue(MFQCV.fq_no);

        //                var isExist_QC = await _ITimsService.GetListDataFaclineQcwithfqno(MFQCV.fq_no);
        //                isExist_QC.FirstOrDefault().check_qty = soluongdefect;
        //                isExist_QC.FirstOrDefault().chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
        //                await _ITimsService.UpdateFaclineQty(isExist_QC.FirstOrDefault().check_qty, isExist_QC.FirstOrDefault().ok_qty, isExist_QC.FirstOrDefault().fqno, isExist_QC.FirstOrDefault().chg_id);


        //                return Json(new { result = true, MFQC }, JsonRequestBehavior.AllowGet);
        //            }
        //        }

        //        return Json(new { result = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);

        //        #endregion insert info
        //    }
        //    else
        //    {
        //        return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        #endregion PRINT NG (TIMS)

        #region Sử dụng chung
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
        public string autobarcode(int id)
        {
            if (id.ToString().Length < 2)
            {
                return string.Format("00000{0}", id);
            }

            if ((id.ToString().Length < 3) || (id.ToString().Length == 2))
            {
                return string.Format("0000{0}", id);
            }

            if ((id.ToString().Length < 4) || (id.ToString().Length == 3))
            {
                return string.Format("000{0}", id);
            }

            if ((id.ToString().Length < 5) || (id.ToString().Length == 4))
            {
                return string.Format("00{0}", id);
            }
            if ((id.ToString().Length < 6) || (id.ToString().Length == 5))
            {
                return string.Format("0{0}", id);
            }
            if ((id.ToString().Length < 7) || (id.ToString().Length == 6))
            {
                return string.Format("{0}", id);
            }
            return string.Empty;
        }

        #endregion Sử dụng chung

        #region TIMS_ATM
        public ActionResult TIMS_ATM()
        {
            return SetLanguage("");
        }
        public ActionResult ATM()
        {
            return SetLanguage("~/Views/TIMS/ATM/ATM.cshtml");
        }
        public ActionResult searchAtm(Pageing paging, bool type)
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

                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {

                    string product_id = Request["product"] == null ? "" : Request["product"].Trim();
                    string name_id = Request["name"] == null ? "" : Request["name"].Trim();
                    string at_id = Request["at_no"] == null ? "" : Request["at_no"].Trim();
                    string reg_dt = Request["reg_dt"] == null ? "" : Request["reg_dt"].Trim();
                    string reg_dt_end = Request["reg_dt_end"] == null ? "" : Request["reg_dt_end"].Trim();
                    string bom_type = Request["bom_type"] == null ? "" : Request["bom_type"].Trim();
                    var dateConvert = new DateTime();
                    if (DateTime.TryParse(reg_dt, out dateConvert))
                    {
                        reg_dt = dateConvert.ToString("yyyy/MM/dd");
                    }

                    if (DateTime.TryParse(reg_dt_end, out dateConvert))
                    {
                        reg_dt_end = dateConvert.ToString("yyyy/MM/dd");
                    }
                    StringBuilder varname1 = new StringBuilder();
                    if (type != true)
                    {
                        //varname1.Append(" SET sql_mode = '';SET @@sql_mode = ''; SELECT any_value(`v`.`id_actual`)id_actual, ");
                        //varname1.Append("  SUM(`v`.`m_lieu`)m_lieu,SUM(`v`.`actual`)actual,MAX(`v`.`product`)product, ");
                        //varname1.Append("  COUNT(`v`.`reg_dt`) AS `count_day`,MAX(`v`.`product_nm`) product_nm,");
                        //varname1.Append("  MIN(`v`.`reg_dt`)min_day,MAX(`v`.`reg_dt`)max_day,");
                        //varname1.Append("  ROUND((((SUM(`v`.`actual`) * any_value(`v`.`need_m`)) / SUM(`v`.`m_lieu`)) * 100),2) AS `HS` ");

                        //varname1.Append(" from (  ");
                        //varname1.Append(" SELECT b.id_actual,b.m_lieu,b.actual,b.product,b.reg_dt,b.product_nm,b.need_m from atm_tims as b  ");
                        //varname1.Append("where ('" + product_id + "'='' OR b.product LIKE '%" + product_id + "%') ");
                        //varname1.Append("AND ('" + name_id + "'='' OR b.product_nm LIKE '%" + name_id + "%') ");
                        //varname1.Append("AND ('" + at_id + "'='' OR b.at_no LIKE '%" + at_id + "%') ");
                        //varname1.Append("AND ('" + reg_dt + "'='' OR DATE_FORMAT(b.reg_dt,'%Y/%m/%d') >= DATE_FORMAT('" + reg_dt + "','%Y/%m/%d') )");
                        //varname1.Append("AND ('" + reg_dt_end + "'='' OR DATE_FORMAT(b.reg_dt,'%Y/%m/%d') <= DATE_FORMAT('" + reg_dt_end + "','%Y/%m/%d') ))v GROUP BY `v`.`product`");
                        //string aa = varname1.ToString();
                        //DataTable dt = new InitMethods().ReturnDataTableNonConstraints(varname1);

                        //int total = dt.Rows.Count;
                        //var result = dt.AsEnumerable().OrderByDescending(x => x.Field<string>("product_nm"));
                        //return new InitMethods().ReturnJsonResultWithPaging(paging, total, result);
                        var daata = _ITimsService.SearchATMTimsS1(product_id, name_id, at_id, reg_dt, reg_dt_end, bom_type,paging.page,paging.rows);
                        int start = (paging.page - 1) * paging.rows;
                        int totals = daata.Count();
                        int totalPages = (int)Math.Ceiling((float)totals / paging.rows);
                        IEnumerable<ATMTIMS> dataactual = daata.Skip<ATMTIMS>(start).Take(paging.rows);
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
                        var daata = _ITimsService.SearchATMTims1(product_id, name_id, at_id, reg_dt, reg_dt_end, bom_type,paging.page, paging.rows);
                        //varname1.Append("SET sql_mode = ''; SET @@sql_mode = ''; SELECT b.* FROM atm_tims AS b ");
                        //varname1.Append(" where ('" + product_id + "'='' OR b.product LIKE '%" + product_id + "%') ");
                        //varname1.Append("AND ('" + name_id + "'='' OR b.product_nm LIKE '%" + name_id + "%') ");
                        //varname1.Append("AND ('" + at_id + "'='' OR b.at_no LIKE '%" + at_id + "%') ");
                        //varname1.Append("AND ('" + reg_dt + "'='' OR DATE_FORMAT(b.reg_dt,'%Y/%m/%d') >= DATE_FORMAT('" + reg_dt + "','%Y/%m/%d') )");
                        //varname1.Append("AND ('" + reg_dt_end + "'='' OR DATE_FORMAT(b.reg_dt,'%Y/%m/%d') <= DATE_FORMAT('" + reg_dt_end + "','%Y/%m/%d') )   GROUP BY b.at_no  ");
                        //string aa = varname1.ToString();
                        //DataTable dt = new InitMethods().ReturnDataTableNonConstraints(varname1);

                        //int total = dt.Rows.Count;
                        //var result = dt.AsEnumerabl e().OrderByDescending(x => x.Field<string>("at_no"));
                        //return new InitMethods().ReturnJsonResultWithPaging(paging, total, result);
                        int start = (paging.page - 1) * paging.rows;
                        int totals = daata.Count();
                        int totalPages = (int)Math.Ceiling((float)totals / paging.rows);
                        IEnumerable<ATMTIMS> dataactual = daata.Skip<ATMTIMS>(start).Take(paging.rows);
                        var jsonReturn = new
                        {
                            total = totalPages,
                            page = paging.page,
                            records = totals,
                            rows = dataactual
                        };
                        return Json(jsonReturn, JsonRequestBehavior.AllowGet);
                    }
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
        public ActionResult TIMSDaily()
        {
            string[] keys = Request.Form.AllKeys;
            if (keys.Length >= 5)
            {
                var value = Request.Form[keys[0]];
                var value2 = Request.Form[keys[1]];
                var value3 = Request.Form[keys[2]];
                var value4 = Request.Form[keys[3]];
                var value5 = Request.Form[keys[4]];
                ViewBag.at_no = (value == "undefined" ? "" : value);
                ViewBag.tieude = (value == "undefined" ? "" : value + "-") + value3;
                ViewBag.id_actual = value2;
                ViewBag.reg_dt = value4;
                ViewBag.reg_dt_end = value5;
            }
            else
            {
                ViewBag.Error = "Không có dữ liệu được gửi lên";
            }
            return View("~/Views/TIMS/ATM/TIMSDaily.cshtml");
        }
        public JsonResult getData(string at_no, string product, string reg_dt, string reg_dt_end,string bom_type)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                try
                {

                    var dateConvert = new DateTime();
                    if (DateTime.TryParse(reg_dt, out dateConvert))
                    {
                        reg_dt = dateConvert.ToString("yyyy/MM/dd");
                    }

                    if (DateTime.TryParse(reg_dt_end, out dateConvert))
                    {
                        reg_dt_end = dateConvert.ToString("yyyy/MM/dd");
                    }

                    var type = "";
                    var daata = new List<ATMTIMS>();
                    var dataa1 = new List<ATMTIMS>();
                    if (at_no == "")
                    {
                        type = "product";
                        //varname1.Append(" SET sql_mode = '';SET @@sql_mode = ''; SELECT any_value(`v`.`id_actual`)id_actual, ");
                        //varname1.Append("  SUM(`v`.`m_lieu`)m_lieu,SUM(`v`.`actual`)actual,MAX(`v`.`product`)product, ");
                        //varname1.Append("  COUNT(`v`.`reg_dt`) AS `count_day`,MAX(`v`.`product_nm`) product_nm,");
                        //varname1.Append("  MIN(`v`.`reg_dt`)min_day,MAX(`v`.`reg_dt`)max_day,");
                        //varname1.Append("  ROUND((((SUM(`v`.`actual`) * any_value(`v`.`need_m`)) / SUM(`v`.`m_lieu`)) * 100),2) AS `HS` ");

                        //varname1.Append(" from (  ");
                        //varname1.Append("SELECT b.id_actual,b.m_lieu,b.actual,b.product,b.reg_dt,b.product_nm,b.need_m from atm_tims as b  ");
                        //varname1.Append("where ('" + product + "'='' OR b.product LIKE '%" + product + "%') ");
                        //varname1.Append("AND ('" + reg_dt + "'='' OR DATE_FORMAT(b.reg_dt,'%Y/%m/%d') >= DATE_FORMAT('" + reg_dt + "','%Y/%m/%d') )");
                        //varname1.Append("AND ('" + reg_dt_end + "'='' OR DATE_FORMAT(b.reg_dt,'%Y/%m/%d') <= DATE_FORMAT('" + reg_dt_end + "','%Y/%m/%d') ))v GROUP BY `v`.`product`");
                        daata = _ITimsService.SearchATMTims(product, "", at_no, reg_dt, reg_dt_end, bom_type);

                    }
                    else
                    {
                        type = "at_no";

                        //kiểm tra po đã được chuyển sang w_material_info01
                        //var Checkdata = _ITimsService.CheckPOMove(at_no);
                        //if (Checkdata)
                        //{
                        //    //varname1.Append(" SET sql_mode = '';SET @@sql_mode = ''; SELECT * from atm_tims01 as b  ");
                        //    //varname1.Append("where ('" + at_no + "'='' OR b.at_no LIKE '%" + at_no + "%') ");
                        //    //varname1.Append("AND ('" + reg_dt + "'='' OR DATE_FORMAT(b.reg_dt,'%Y/%m/%d') >= DATE_FORMAT('" + reg_dt + "','%Y/%m/%d') )");
                        //    //varname1.Append("AND ('" + reg_dt_end + "'='' OR DATE_FORMAT(b.reg_dt,'%Y/%m/%d') <= DATE_FORMAT('" + reg_dt_end + "','%Y/%m/%d') )");
                        //    dataa1 = _ITimsService.SearchATMTimsOQC("","", at_no, reg_dt, reg_dt_end);
                        //}
                        //else
                        //{
                        //    //varname1.Append(" SET sql_mode = '';SET @@sql_mode = ''; SELECT * from atm_tims as b  ");
                        //    //varname1.Append("where ('" + at_no + "'='' OR b.at_no LIKE '%" + at_no + "%') ");
                        //    //varname1.Append("AND ('" + reg_dt + "'='' OR DATE_FORMAT(b.reg_dt,'%Y/%m/%d') >= DATE_FORMAT('" + reg_dt + "','%Y/%m/%d') )");
                        //    //varname1.Append("AND ('" + reg_dt_end + "'='' OR DATE_FORMAT(b.reg_dt,'%Y/%m/%d') <= DATE_FORMAT('" + reg_dt_end + "','%Y/%m/%d') )");
                        //     dataa1 = _ITimsService.SearchATMTims("", "", at_no, reg_dt, reg_dt_end);
                        //}
                        daata = _ITimsService.SearchATMTims(product, "", at_no, reg_dt, reg_dt_end, bom_type);

                    }
                    //StringBuilder varname2 = new StringBuilder();
                    //varname2.Append("SELECT * from atm_mms as b  ");
                    //varname2.Append("where ('" + at_no + "'='' OR b.at_no LIKE '%" + at_no + "%') ");
                    //varname2.Append("AND ('" + reg_dt + "'='' OR DATE_FORMAT(b.reg_dt,'%Y/%m/%d') >= DATE_FORMAT('" + reg_dt + "','%Y/%m/%d') )");
                    //varname2.Append("AND ('" + reg_dt_end + "'='' OR DATE_FORMAT(b.reg_dt,'%Y/%m/%d') <= DATE_FORMAT('" + reg_dt_end + "','%Y/%m/%d') )");

                    dataa1 = _ITimsService.SearchATMTimsS(product, "", at_no, reg_dt, reg_dt_end, bom_type);

                    //var data = new InitMethods().ConvertDataTableToJsonAndReturn(varname1);
                    //var data1 = new InitMethods().ConvertDataTableToJsonAndReturn(varname2);

                    return Json(new { result = true, data = daata, data1 = dataa1, type }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { result = false, message = "Lỗi hệ Thống!" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }



        }

        #endregion TIMS_ATM

        #region Return
        public ActionResult PrintQR_Tims()
        {
            return View("~/Views/TIMS/PrintQR/PrintQR_Tims.cshtml");
        }

        public async Task<JsonResult> getPrintQR(Pageing paging)
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
                var datatt = await _ITimsService.GetwmaterialmodelPrintQR(mt_no, return_date, recevice_dt_start, recevice_dt_end);
                int start = (paging.page - 1) * paging.rows;
                int totals = datatt.Count();
                int totalPages = (int)Math.Ceiling((float)totals / paging.rows);
                IEnumerable<w_material_model> dataactual = datatt.Skip<w_material_model>(start).Take(paging.rows);
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
                var data = new List<w_material_model>();
                var jsonReturn = new
                {
                    result = false,
                    total = 0,
                    page = 0,
                    records = 0,
                    rows = data,
                    message = Constant.ErrorAuthorize
                };
                return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> getPrintQRDetail(Pageing paging)
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
                int id = Convert.ToInt32(Request["wmtid_DT"] == null ? "" : Request["wmtid_DT"].Trim());
                var wmtinfo = await _ITimsService.GetWMaterialInfoTIMS(id);
                var mt_no = wmtinfo.mt_no;
                var datatt = await _ITimsService.GetwmaterialmodelPrintQRDetail(mt_no);
                int start = (paging.page - 1) * paging.rows;
                int totals = datatt.Count();
                int totalPages = (int)Math.Ceiling((float)totals / paging.rows);
                IEnumerable<w_material_model> dataactual = datatt.Skip<w_material_model>(start).Take(paging.rows);
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
                var data = new List<w_material_model>();
                var jsonReturn = new
                {
                    result = false,
                    total = 0,
                    page = 0,
                    records = 0,
                    rows = data,
                    message = Constant.ErrorAuthorize
                };
                return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> printQRConfirm(string id)
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

                    var list = new ArrayList();
                    var a2 = id.TrimStart('[').TrimEnd(']').Split(',');
                    for (int i = 0; i < a2.Length; i++)
                    {
                        var id2 = int.Parse(a2[i]);

                        //var flag = db.w_material_info.Where(x => x.wmtid == id2 && x.mt_sts_cd == "001").Count();
                        var flagg = await _ITimsService.GetWMaterialInfoTIMSwithstatuswmtid(id2, "001");
                        if (flagg != null)
                        {
                            continue;
                        }
                        string userid = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                        //Models.MaterialInfoMMS db_w_material = db.w_material_info.Find(id2);

                        //var gr_qty_tmp = db_w_material.gr_qty;
                        //db_w_material.mt_sts_cd = "008";
                        await _ITimsService.UpdateStatusWMaterialInfoById("008", userid, id2);
                        //db.Entry(db_w_material).State = EntityState.Modified;

                        //db.SaveChanges();
                    }

                    var list2 = await _ITimsService.Getwmaterialmodel(id);
                    return Json(new
                    {
                        result = true,
                        message = Constant.Success,
                        data = list2
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult btnPrintQR()
        {
            string[] keys = Request.Form.AllKeys;
            var value = "";
            value = Request.Form[keys[0]];
            ViewData["Message"] = value;
            return View("~/Views/TIMS/PrintQR/BtnPrintQr.cshtml");
        }

        public async Task<ActionResult> qrPrintQr(string mt_no)
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
                if (Session["authorize"] != null || resqheadervalmob  == "Mob")
                {

                    if (mt_no != "")
                    {
                        var data = await _ITimsService.GetwmaterialmodelqrPrintQr(mt_no);
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    return View();
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

        #endregion Return

        #region searchTemGoi
        [HttpGet]
        public async Task<JsonResult> GetdataTemGoi(Pageing pageing, string product,string lotNo, string shift, string buyer_qr)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                if (!string.IsNullOrEmpty(buyer_qr))
                {
                    product = "";
                    lotNo = "";
                    shift = "";
                }
                var data = await _ITimsService.GetListStamDetail(product, lotNo, shift, buyer_qr);
                var records = data.Count();
                int totalPages = (int)Math.Ceiling((float)records / pageing.rows);
                var rowsData = data.Skip((pageing.page - 1)).Take(pageing.rows);
                return Json(new { total = totalPages, page = pageing.page, records = records, rows = rowsData }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = new List<StampDetail>();
                var jsonReturn = new
                {
                    result = false,
                    total = 0,
                    page = 0,
                    records = 0,
                    rows = data,
                    message = Constant.ErrorAuthorize
                };
                return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region ChangeTemGoi
        public ActionResult PartialView_ViewStatusTemGoiPopup()
        {
            return PartialView();
        }
        public async Task<ActionResult> ViewStatusTemGoi(string buyerCode)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }

            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                try
                {
                    //kiểm tra trong bảng stamp_detail có tồn tại không
                    var IsExist = await _ITimsService.Getstampdetail(buyerCode);
                    if (IsExist == null)
                    {
                        return Json(new { result = false, message = "Tem gói này không tồn tại." }, JsonRequestBehavior.AllowGet);
                    }
                    var data = await _ITimsService.ViewStatusTemGoi(buyerCode);
                    if (data != null)
                    {
                        //kiểm tra w_material_info(status = 010, lct = '006', buyer_qr: có)
                        if (data.mt_sts_cd.Equals("010") && data.lct_cd.Equals("006000000000000000"))
                        {
                            return Json(new { result = true, data, message = "Thành công" }, JsonRequestBehavior.AllowGet);
                        }
                        //kiểm tra mã tem gói đã được đóng thùng chưa
                        var isBoxMapping = await _ITimsService.CheckTemGoimappingBox(buyerCode);
                        if (isBoxMapping != null)
                        {
                            return Json(new { result = false, message = "Tem gói này đã được cho vào thùng trong FG." }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { result = true, data, message = "" }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    //kiểm tra xem đã đưa vào Kho FG chưa

                    return Json(new { result = false, message = "Tem gói này chưa được mapping với Container" }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }
        public async Task<ActionResult> ChangStamp(string stampOld, string stampNew, string ProductCode, int wmtid)
        {

            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }

            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                try
                {
                    if (string.IsNullOrEmpty(ProductCode))
                    {
                        return Json(new { result = false, message = "Product Rỗng" }, JsonRequestBehavior.AllowGet);
                    }
                    if (string.IsNullOrEmpty(stampOld))
                    {
                        return Json(new { result = false, message = "Tem gói rỗng, vui lòng scan lại" }, JsonRequestBehavior.AllowGet);
                    }
                    if (string.IsNullOrEmpty(stampNew))
                    {
                        return Json(new { result = false, message = "Tem gói thay thế rỗng, vui lòng scan lại" }, JsonRequestBehavior.AllowGet);
                    }
                    if (!stampNew.Contains("DZIH"))
                    {
                        return Json(new { result = false, message = "Tem gói thay thế không đúng tiêu chuẩn, phải có DZIH" }, JsonRequestBehavior.AllowGet);
                    }
                    int checkqrbuyer = await _ITimsService.GetQrcodeBuyer(stampNew);
                    //if (_TIMSService.GetQrcodeBuyer(stampNew) > 0)
                    if (checkqrbuyer > 0)
                    {
                        return Json(new { result = false, message = "Buyer Qr đã được sử dụng." }, JsonRequestBehavior.AllowGet);
                    }
                    // nếu tem thay thế chưa mapping với contanier nào thì cho thay thế
                    // kiểm tra tem gói đã tồn tại trong stamp_detail. không có thì insert
                    string trimmed = ProductCode.Replace(" ", "");
                    // kiểm tra xem product theo quy tắc(0) hay bất quy tắc(1). có nghĩa  là nếu bất quy tắc thì không cần khác product vẫn cho mapping với tem gói
                    var typeProduct = await _ITimsService.ChecktypeProduct(trimmed);
                    if (typeProduct.Equals("1"))
                    {

                        var item = await _ITimsService.FindOneMaterialInfoByIdBuyer(wmtid);
                        item.buyer_qr = stampNew;
                        item.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                        item.chg_date = DateTime.Now;

                        var isExisted = await _ITimsService.FindStamp(stampNew);
                        if (isExisted == null)
                        {
                            //ktra stamp_cd
                            var stamp_cd = "";

                            var ktra_stamp_cd = await _ITimsService.GetStyleNo(ProductCode);

                            if (ktra_stamp_cd != null)
                            {
                                stamp_cd = ktra_stamp_cd.stamp_code;
                                if (string.IsNullOrEmpty(stamp_cd))
                                {
                                    return Json(new { result = false, message = "Vui Lòng Chọn kiểu tem cho Product(DMS)!!! " }, JsonRequestBehavior.AllowGet);
                                }
                            }

                            //lấy ra lot_date bằng cách trừ đi product và 8 kí tự sẽ tìm được mã lot_date
                            var prd = trimmed.Replace(" ", "");
                            var prd1 = trimmed.Replace("-", "");

                            var timDZIH = stampNew.IndexOf("DZIH") + 4; //product+DZIH
                            var vendor_line = stampNew.Substring(timDZIH, 1);
                            var label_printer = stampNew.Substring(timDZIH + 1, 1);
                            var is_sample = stampNew.Substring(timDZIH + 2, 1);
                            var PCN = stampNew.Substring(timDZIH + 3, 1);
                            var date = stampNew.Substring(timDZIH + 4, 3);
                            var lot_date = DateFormatByShinsungRule(date);

                            var serial_number = stampNew.Substring(timDZIH + 7, 3); //gắn 001
                            var machine_line = stampNew.Substring(timDZIH + 10, 2); //gắn 01

                            var shift = stampNew.Substring(timDZIH + 12, 1);


                            // kiểm tra tem gói đã qua FG chưa nếu rồi thì update luôn
                            var isExist = await _ITimsService.FindOneBuyerInfoByBuyer(stampOld);
                            if (isExist != null)
                            {
                                if (isExist.status != "001")
                                {
                                    return Json(new { result = false, message = "Tem gói này có thể đã đóng thùng hoặc đã giao hàng" }, JsonRequestBehavior.AllowGet);
                                }

                                isExist.buyer_qr = stampNew;

                                isExist.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();

                                await _ITimsService.UpdateBuyerQRGeneral(isExist);
                            }


                            //insert stamp_detail
                            var stamp_detail = new stamp_detail()
                            {
                                buyer_qr = stampNew.ToUpper(),
                                stamp_code = stamp_cd,
                                product_code = ProductCode,
                                vendor_code = "DZIH",
                                vendor_line = vendor_line,
                                label_printer = label_printer,
                                is_sample = is_sample,
                                pcn = PCN,
                                lot_date = lot_date,
                                serial_number = serial_number,
                                machine_line = machine_line,
                                shift = shift,
                                standard_qty = ktra_stamp_cd.pack_amt.HasValue ? ktra_stamp_cd.pack_amt.Value : 0,
                                reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString(),
                                chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString(),
                            };
                            await _ITimsService.Insertstampdetail(stamp_detail);

                        }
                        await _ITimsService.UpdateMaterialInfoTimsBuyer(item.wmtid, item.buyer_qr, item.chg_id);
                        //await _ITimsService.InsertMaterialInfoTIMMS(item);
                        //_IWOService.UpdateMaterialInfo(item);
                        return Json(new { result = true, message = Constant.Success }, JsonRequestBehavior.AllowGet);


                    }
                    if (stampNew.StartsWith(trimmed.Replace("-", "")))
                    {
                        //var WmaterialStampChange = await _ITimsService.ViewStatusTemGoi(stampOld);
                        var item = await _ITimsService.FindOneMaterialInfoByIdBuyer(wmtid);
                        item.buyer_qr = stampNew;
                        item.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();

                        var isExisted = await _ITimsService.FindStamp(stampNew);
                        if (isExisted.Count() == 0)
                        {
                            //ktra stamp_cd
                            var stamp_cd = "";

                            var ktra_stamp_cd = await _ITimsService.GetStyleNo(ProductCode);

                            if (ktra_stamp_cd != null)
                            {
                                stamp_cd = ktra_stamp_cd.stamp_code;
                                if (string.IsNullOrEmpty(stamp_cd))
                                {
                                    return Json(new { result = false, message = "Vui Lòng Chọn kiểu tem cho Product(DMS)!!! " }, JsonRequestBehavior.AllowGet);
                                }
                            }

                            //lot_date
                            //var timDZIH = stampNew.IndexOf("DZIH") + 4; //product+DZIH
                            //if (timDZIH <= 0)
                            //{
                            //     timDZIH = stampNew.IndexOf("EA8D") + 4;
                            //}

                            var timDZIH1 = stampNew.IndexOf("DZIH"); //product
                            if (timDZIH1 < 1)
                            {
                                timDZIH1 = stampNew.IndexOf("EA8D");
                            }

                            var timDZIH = timDZIH1 + 4; //product+DZIH
                            var vendor_line = stampNew.Substring(timDZIH, 1);
                            var label_printer = stampNew.Substring(timDZIH + 1, 1);
                            var is_sample = stampNew.Substring(timDZIH + 2, 1);
                            var PCN = stampNew.Substring(timDZIH + 3, 1);
                            var date = stampNew.Substring(timDZIH + 4, 3);
                            var lot_date = DateFormatByShinsungRule(date);

                            var serial_number = stampNew.Substring(timDZIH + 7, 3); //gắn 001
                            var machine_line = stampNew.Substring(timDZIH + 10, 2); //gắn 01

                            var shift = stampNew.Substring(timDZIH + 12, 1);


                            //insert stamp_detail
                            var stamp_detail = new stamp_detail()
                            {
                                buyer_qr = stampNew.ToUpper(),
                                stamp_code = stamp_cd,
                                product_code = ProductCode,
                                vendor_code = "DZIH",
                                vendor_line = vendor_line,
                                label_printer = label_printer,
                                is_sample = is_sample,
                                pcn = PCN,
                                lot_date = lot_date,
                                serial_number = serial_number,
                                machine_line = machine_line,
                                shift = shift,
                                standard_qty = ktra_stamp_cd.pack_amt.HasValue ? ktra_stamp_cd.pack_amt.Value : 0,
                                reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString(),
                                chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString(),
                            };
                            await _ITimsService.Insertstampdetail(stamp_detail);
                            //_TIMSService.Insertstampdetail(stamp_detail);



                        }

                        // kiểm tra tem gói đã qua FG chưa nếu rồi thì update luôn
                        var isExist =await _ITimsService.FindOneBuyerInfoByBuyer(stampOld);
                        if (isExist != null)
                        {
                            if (isExist.status != "001")
                            {
                                return Json(new { result = false, message = "Tem gói này không có thể đã đóng thùng hoặc đã giao hàng" }, JsonRequestBehavior.AllowGet);
                            }

                            isExist.buyer_qr = stampNew;

                            isExist.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();

                           await _ITimsService.UpdateBuyerQRGeneral(isExist);
                        }

                        await _ITimsService.UpdateMaterialInfoTimsBuyer(item.wmtid, item.buyer_qr, item.chg_id);


                        return Json(new { result = true, message = Constant.Success }, JsonRequestBehavior.AllowGet);
                    }

                    else
                    {
                        return Json(new { result = false, message = "Mã tem gói này không thuộc với Product của Container" }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region TIMReceving
        public ActionResult RecevingSortingTims()
        {
            return SetLanguage("~/Views/Tims/SortingTims/RecevingTims/RecevingSortingTims.cshtml");
        }
        public async Task<ActionResult> ScanReceivingBuyer(string buyer_qr, string ShippingCode)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }

            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                try
                {
                    buyer_qr = buyer_qr.Trim();
                    //Check input
                    if (string.IsNullOrEmpty(ShippingCode))
                    {
                        return Json(new { result = false, message = "Vui lòng chọn một phiếu SF!!!" }, JsonRequestBehavior.AllowGet);
                    }
                    if (string.IsNullOrEmpty(buyer_qr))
                    {
                        return Json(new { result = false, message = "Vui lòng scan một tem gói" }, JsonRequestBehavior.AllowGet);
                    }



                    //KIỂM TRA XEM SD CÓ TỒN TẠI KHÔNG.
                    var check = await _ITimsService.CheckSFinfo(ShippingCode);
                    if (check.Count <= 0)
                    {
                        return Json(new { result = false, message = "SF này không tồn tại" }, JsonRequestBehavior.AllowGet);
                    }

                    //kiểm tra tem gói có thuộc phiếu xuất đó ko

                    var isCheckExistSF = await _ITimsService.isCheckExistSF(ShippingCode, buyer_qr);
                    if (isCheckExistSF == null)
                    {
                        return Json(new { result = false, message = "Không có tem gói được trả về" }, JsonRequestBehavior.AllowGet);
                    }
                    //Step1: update w_material_info_tims tem gói  lct= 006000000000000000, sts_cd = 015

                    var isCheckExistBuyerQRSF = await _ITimsService.isCheckExistBuyerQRSF(buyer_qr);
                    if (isCheckExistBuyerQRSF == null)
                    {
                        return Json(new { result = false, message = "Tem gói này không được mapping với OQC" }, JsonRequestBehavior.AllowGet);
                    }
                    isCheckExistBuyerQRSF.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    int updateWMaterial = await _ITimsService.UpdateBuyerFGWMaterialInfo(isCheckExistBuyerQRSF);

                    //Step2: update shippingfgsortingdetail  location= 006000000000000000

                    int updateShippingFGSorting = await _ITimsService.updateShippingFGSorting(isCheckExistSF);

                    return (Json(new { result = true, data = "", message = "Thành công" }, JsonRequestBehavior.AllowGet));

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region TIMShipping
        public ActionResult ShippingSortingTims()
        {
            return SetLanguage("~/Views/Tims/SortingTims/ShippingTims/ShippingSortingTims.cshtml");
        }

        public async Task<JsonResult> InsertShippingTIMSSorting(ShippingTIMSSortingModel item)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                try
                {
                    #region Tang tự động
                    string ShippingCode = "ST1";

                    var Shippinglast = await _ITimsService.GetLastShippingTIMSSorting();

                    if (Shippinglast != null)
                    {
                        ShippingCode = Shippinglast.ShippingCode;
                        ShippingCode = string.Concat("ST", (int.Parse(ShippingCode.Substring(2)) + 1).ToString());
                    }
                    #endregion
                    item.ShippingCode = ShippingCode;
                    item.IsFinish = "N";
                    item.CreateId = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                    item.CreateDate = DateTime.Now;
                    item.ChangeId = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                    item.ChangeDate = DateTime.Now;

                    int id_ShippingFG = await _ITimsService.InsertToShippingTIMSSorting(item);
                    var GetLastShippingTIMSSorting = await _ITimsService.GetLastShippingTIMSSorting();
                    if (GetLastShippingTIMSSorting != null)
                    {
                        item.id = GetLastShippingTIMSSorting.id;
                        return Json(new { result = true, message = "Thành công!!!", data = item }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { result = false, message = "Tạo thất bại" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<JsonResult> ModifyShippingTIMSSorting(ShippingTIMSSortingModel item)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }

            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                try
                {
                    item.ChangeId = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                    item.ChangeDate = DateTime.Now;

                    await _ITimsService.ModifyShippingTIMSSorting(item);


                    return Json(new { result = true, message = "Thành công!!!", data = item }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<ActionResult> GetExport_ScanBuyer_TIMS(string BuyerCode)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                try
                {
                    //Check input
                    if (string.IsNullOrEmpty(BuyerCode))
                    {
                        return Json(new { result = false, message = "Mã tem gói trống, Vui lòng Scan lại" }, JsonRequestBehavior.AllowGet);
                    }
                    BuyerCode = BuyerCode.Trim();
                    //Kiểm tra mã TEM GOI có tồn tại ở kho TIMS không
                    var IsExistBuyerQr = await _ITimsService.CheckIsExistBuyerCode(BuyerCode);
                    //kiểm tra xem mã khác 015 là kho khác
                    if (IsExistBuyerQr != null)
                    {
                        if (!IsExistBuyerQr.status.Equals("015"))
                        {
                            return Json(new { result = false, message = "Mã Này không được sorting" }, JsonRequestBehavior.AllowGet);
                        }
                        string trangthai = IsExistBuyerQr.status == "015" ? "Sorting" : "";
                        IsExistBuyerQr.status = trangthai;
                        return Json(new { result = true, message = "Thành công", data = IsExistBuyerQr }, JsonRequestBehavior.AllowGet);
                    }
                    {
                        return Json(new { result = false, message = "Mã bạn vừa quét là: " + BuyerCode }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> UpdateShippingSortingTIMS(string ShippingCode, string ListId)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }
            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
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

                    var item = new ShippingTIMSSortingDetailModel();
                    item.location = "003G01000000000000";

                    await _ITimsService.UpdateShippingSortingTIMS(item, ListId);

                    //insert phiếu xuất và tem gói vào bảng shippingfgsortingdetail
                    var UserID = Session["userid"] == null ? null : Session["userid"].ToString();
                    await _ITimsService.InsertShippingSortingTIMSDetail(ShippingCode, ListId, UserID);


                    return Json(new { result = true, message = "Thành công" }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<ActionResult> DeleteFGShipping(int id, string ShippingCode)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            string resqheadervalmob = "";
            if (resqheaderval != null)
            {
                resqheadervalmob = resqheaderval[0];
            }

            if (Session["authorize"] != null || resqheadervalmob  == "Mob")
            {
                try
                {
                    if (string.IsNullOrEmpty(ShippingCode))
                    {
                        return Json(new { result = false, message = "Vui lòng Chọn một phiếu xuất để xóa" }, JsonRequestBehavior.AllowGet);
                    }

                    //kiểm tra xem ep này đã xuất bất kì liệu nào ra máy chưa/ nếu chưa mới cho xóa
                    var check = await _ITimsService.GetListShippingFGSorting(ShippingCode);
                    if (check.Count() > 0)
                    {
                        return Json(new { result = false, message = "Phiếu này đã có Nguyên Vật Liệu xuất, Bạn không thể xóa" }, JsonRequestBehavior.AllowGet);
                    }
                    await _ITimsService.DeleteToExportToMachine(id);


                    return Json(new { result = true, message = "Thành công!!!" }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion
        #region
        public ActionResult EffATM()
        {
            return SetLanguage("~/Views/TIMS/ATM/EffATM.cshtml");
        }
        public async Task<ActionResult> searchEffATM(Pageing paging)
        {
            try
            {
                string ProductCode = Request["SearchProductCode"] == null ? "" : Request["SearchProductCode"].Trim();
                string ProductName = Request["SearchProductName"] == null ? "" : Request["SearchProductName"].Trim();
                string PO = Request["SearchPO"] == null ? "" : Request["SearchPO"].Trim();
                string Remark = Request["SearchRemark"] == null ? "" : Request["SearchRemark"].Trim();
                string bom_type = Request["bom_type"] == null ? "" : Request["bom_type"].Trim();

                var dt =  await _ITimsService.getHieuSuat(ProductCode, ProductName, PO, Remark, bom_type);
                int start = (paging.page - 1) * paging.rows;
                int totals = dt.Count();
                int totalPages = (int)Math.Ceiling((float)totals / paging.rows);
                IEnumerable<ATMTIMSEFF> dataactual = dt.Skip<ATMTIMSEFF>(start).Take(paging.rows);
                var jsonReturn = new
                {
                    total = totalPages,
                    page = paging.page,
                    records = totals,
                    rows = dataactual
                };
                return Json(jsonReturn, JsonRequestBehavior.AllowGet);

            }
            catch(Exception e)
            {
                throw e;
            }

        }
        public async  Task<ActionResult> ExportsearchEffATM()
        {
                var resqheader = Request.Headers; String[] resqheaderkey = resqheader.AllKeys; String[] resqheaderval = resqheader.GetValues("requestFrom"); if (Session["authorize"] != null || resqheaderval[0] == "Mob")
                {
                string ProductCode = Request["SearchProductCode"] == null ? "" : Request["SearchProductCode"].Trim();
                string ProductName = Request["SearchProductName"] == null ? "" : Request["SearchProductName"].Trim();
                string PO = Request["SearchPO"] == null ? "" : Request["SearchPO"].Trim();
                string Remark = Request["SearchRemark"] == null ? "" : Request["SearchRemark"].Trim();
                string bom_type = Request["bom_type"] == null ? "" : Request["bom_type"].Trim();
                var listTotals = await _ITimsService.getHieuSuat(ProductCode, ProductName, PO, Remark, bom_type);
                //var listTotals = await _ITimsService.GetspTIMSInventoryGeneralUnion(mtCode, mtNo, mtName, recDate, "");
                    //StringBuilder sql = new StringBuilder($"Call spTIMS_InventoryGeneral_Union('{mtCode}','{mtNo}','{mtName}','{recDate}');");
                    List<ATMTIMSEFF> listTotal = listTotals.ToList();
                    List<ATMTIMSEFF> listOrderBy = new List<ATMTIMSEFF>();
                    listOrderBy = listTotal.OrderBy(x => x.at_no).ToList();

                    DataTable dt = new InitMethods().ConvertListToDataTable(listOrderBy);
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);
                    ds.Tables[0].TableName = "PerformanceTIMS";

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
                        Response.AddHeader("content-disposition", "attachment;filename= Performance_TIMS.xlsx");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                    return View("~/Views/TIMS/ATM/EffATM.cshtml");
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }
        }
        #endregion
    }
}