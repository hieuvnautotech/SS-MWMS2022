using Mvc_VD.Commons;
using Mvc_VD.Models;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models.Request;
using Mvc_VD.Services;
using Mvc_VD.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace Mvc_VD.Controllers
{
    public class HieuShippingMgtController : BaseController
    {
        private readonly IWMSServices _IWMSServices;
        private readonly IWIPService _IWIPService;
        private readonly IcommonService _IcommonService;
        private readonly IHieuCommonServices _IHieuCommonServices;
        private readonly IhomeService _ihomeService;

        #region nay dùng để cheat code max lenght json để đổ db lớn từ db
        //protected override JsonResult Json(object data, string contentType,
        //Encoding contentEncoding, JsonRequestBehavior behavior)
        //{
        //    return new JsonResult()
        //    {
        //        Data = data,
        //        ContentType = contentType,
        //        ContentEncoding = contentEncoding,
        //        JsonRequestBehavior = behavior,
        //        MaxJsonLength = Int32.MaxValue
        //    };
        //}
        #endregion
        public HieuShippingMgtController(
          IWMSServices IWMSServices,
           IWIPService IWIPService,
           IcommonService IcommonService,
           IHieuCommonServices IHieuCommonServices,
          IDbFactory DbFactory, IhomeService ihomeService)
        {
            _IWMSServices = IWMSServices;
            _IWIPService = IWIPService;
            _IcommonService = IcommonService;
            _IHieuCommonServices = IHieuCommonServices;
            _ihomeService = ihomeService;


        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var lang = HttpContext.Request.Cookies.AllKeys.Contains("language")
                  ? HttpContext.Request.Cookies["language"].Value
                  : "en";
            var router = this.ControllerContext.RouteData.Values.Values.ElementAt(0).ToString();
            var result = _ihomeService.GetLanguage(lang, router);
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
        // GET: HieuShippingMgt
        public ActionResult HieuShippingPickingScan(string code)
        {
            ViewData["Message"] = code;
            return SetLanguage("");
        }

        public async Task<ActionResult> GetPickingScan(string sd_no, string sd_nm, string product_cd, string remark, Pageing commPag)// có kết hợp phân trang
        {
            try
            {
                
                //var data = await _IcommonService.GetListSDInfo(sd_no, sd_nm, product_cd, remark);

                IEnumerable<SdInfos> data = await _IHieuCommonServices.GetListSDInfo(sd_no, sd_nm, product_cd, remark);
                int start = (commPag.page - 1) * commPag.rows;
                int end = (commPag.page - 1) * commPag.rows + commPag.rows;
                int totals = data.Count();
                int totalPages = (int)Math.Ceiling((float)totals / commPag.rows);
                IEnumerable<SdInfos> dataSkip = data.Skip<SdInfos>(start).Take(commPag.rows);
                var jsonReturn = new
                {
                    total = totalPages,
                    page = commPag.page,
                    records = totals,
                    rows = dataSkip
                };
                return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new
                {
                    Status = -1,
                    Message = ex.Message,
                    Data = ""
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public async Task<ActionResult> InsertSDInfo(SdInfo w_sd_info) {
            string sd_no = "SD1";
            var sd_no_last = await _IcommonService.GetLastSdNo();
            if (sd_no_last != null) {
                var sd_noCode = sd_no_last;
                sd_no = string.Concat("SD", (int.Parse(sd_noCode.Substring(2)) + 1).ToString());
            }
            string trimmed = String.Concat(w_sd_info.product_cd.Where(c => !Char.IsWhiteSpace(c)));
            w_sd_info.product_cd = trimmed.ToUpper();
            w_sd_info.sd_no = sd_no;
            w_sd_info.alert = 0;
            w_sd_info.lct_cd = "002000000000000000";
            w_sd_info.status = "000";
            w_sd_info.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
            w_sd_info.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();

            w_sd_info.use_yn = "Y";
            w_sd_info.del_yn = "N";
            w_sd_info.reg_dt = DateTime.Now;
            w_sd_info.chg_dt = DateTime.Now;
            int kqq = await _IcommonService.InsertSdInfo(w_sd_info);
            var data = await _IcommonService.GetListSDInfo(sd_no, "", "", "");
            return Json(new { result = true, data = data, message = Constant.Success }, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult PartialView_SD_Info_Popup(string sd_no)
        //{
        //    ViewBag.sd_no = sd_no;

        //    return PartialView();
        //}

        //public async Task<ActionResult> GetPickingScanPP(string sd_no)
        //{
        //    var listdata = await _IWMSServices.GetPickingScanPP(sd_no);
        //    var result = listdata.ToList();
        //    return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        //}

        public async Task<ActionResult> UpdateSDInfo(SdInfo w_sd_info) 
        {
            try
            {
                //var KTTT = db.w_sd_info.Find(w_sd_info.sid);
                var KTTT = await _IWMSServices.GetListSdInfoById(w_sd_info.sid);
                var rs = await _IcommonService.GetListSDInfo(KTTT.sd_no, "", "", "");
                var status = rs.FirstOrDefault();
                if (KTTT == null)
                {
                    return Json(new { result = false, message = Constant.DataNoExist }, JsonRequestBehavior.AllowGet);
                }
                string trimmed = String.Concat(w_sd_info.product_cd.Where(c => !Char.IsWhiteSpace(c)));
                KTTT.product_cd = trimmed.ToUpper();
                KTTT.sd_nm = w_sd_info.sd_nm;
                KTTT.sts_nm = status.sts_nm;
                KTTT.remark = w_sd_info.remark;
                KTTT.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                KTTT.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                KTTT.chg_dt = DateTime.Now;
                KTTT.reg_dt = DateTime.Now;
                await _IWMSServices.UpdateSdInfoTable(KTTT);
                return Json(new { result = true, data = KTTT, message = Constant.Success }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<ActionResult> DeleteSDInfo(SdInfo w_sd_info) 
        {
            try
            {
                var checkSdInfo = await _IWMSServices.GetListSdInfoById(w_sd_info.sid);
                if (checkSdInfo == null)
                {
                    return Json(new { result = false, message = Constant.DataNoExist }, JsonRequestBehavior.AllowGet);
                }
                //kiểm tra xem sd này bên wip đã nhận chưa nếu nhận rồi là không được xóa

                var checkSdInfoBySd_No = await _IWMSServices.GetListDatabySdNo(w_sd_info.sd_no);
                if (checkSdInfoBySd_No != null)
                {
                    return Json(new { result = false, message = "SD này đã được kho sản xuất nhận rồi " }, JsonRequestBehavior.AllowGet);
                }
                await _IWMSServices.DeleteSdInfo(w_sd_info.sid);
                await _IWMSServices.DeleteShippingSDInfo(w_sd_info.sd_no);
                return Json(new { result = true, data = w_sd_info.sid, message = Constant.Success }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> Getshippingsdmaterial(string sd_no)
        {
            //var datas = _IWMSService.Getshippingsdmaterial(sd_no);
            //var data = _IWIPService.GetListMaterialInfoBySdNo(sd_no);
            var data = await _IWMSServices.GetListMaterialInfoBySdNo(sd_no, "");
            return (Json(data, JsonRequestBehavior.AllowGet));

        }
    }


}