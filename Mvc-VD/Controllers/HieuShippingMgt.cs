using Mvc_VD.Services;
using Mvc_VD.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mvc_VD.Controllers
{
    public class HieuShippingMgtController : BaseController
    {
        private readonly IWMSServices _IWMSServices;
        private readonly IWIPService _IWIPService;
        private readonly IcommonService _IcommonService;
        private readonly IhomeService _ihomeService;

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
        // GET: HieuShippingMgt
        public ActionResult HieuShippingPickingScan(string code)
        {
            return View();
        }

        public async Task<ActionResult> GetPickingScan(string sd_no, string sd_nm, string product_cd, string remark)
        {
            try
            {
                var data = await _IcommonService.GetListSDInfo(sd_no, sd_nm, product_cd, remark);
                return Json(data, JsonRequestBehavior.AllowGet);
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
    }
}