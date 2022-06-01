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

namespace Mvc_VD.Controllers
{
    public class HieuShippingMgtController : BaseController
    {
        private readonly IWMSServices _IWMSServices;
        private readonly IWIPService _IWIPService;
        private readonly IcommonService _IcommonService;
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
          IDbFactory DbFactory, IhomeService ihomeService)
        {
            _IWMSServices = IWMSServices;
            _IWIPService = IWIPService;
            _IcommonService = IcommonService;
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

        public async Task<ActionResult> GetPickingScan(string sd_no, string sd_nm, string product_cd, string remark, Pageing commPag)
        {
            try
            {
                
                //var data = await _IcommonService.GetListSDInfo(sd_no, sd_nm, product_cd, remark);

                IEnumerable<SdInfos> data = await _IcommonService.GetListSDInfo(sd_no, sd_nm, product_cd, remark);
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
        #region phan trang
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
        #endregion
    }
}