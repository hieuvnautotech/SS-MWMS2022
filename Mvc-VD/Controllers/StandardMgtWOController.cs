using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Text;
using Mvc_VD.Models;
using System.Globalization;
using System.Data.Entity;
using Mvc_VD.Services.Interface;
using System.Threading.Tasks;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models.Request;

namespace Mvc_VD.Controllers
{
    public class StandardMgtWOController : BaseController
    {
        private readonly IhomeService _homeService;
        private IStandardManagementWO _IStandardManagement;
        public StandardMgtWOController(IStandardManagementWO IStandardManagement, IhomeService ihomeService)
        {
            _homeService = ihomeService;
            _IStandardManagement = IStandardManagement;
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
        #region WorkPolicyInfo
        public ActionResult WorkPolicyInfo()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> getWorkPolicyInfo()
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            if (Session["authorize"] != null || resqheaderval[0] == "Mob")
            {
                var data = await _IStandardManagement.GetListPolicyMT();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var kq = new List<PolicyMT>();
                return Json(new { result = false, message ="Xin vui lòng đăng nhập !", data = kq }, JsonRequestBehavior.AllowGet);
            }

        }

        public string autoPono(int id)
        {
            if (id.ToString().Length < 2)
            {
                return string.Format("0000{0}", id);
            }

            if ((id.ToString().Length < 3) || (id.ToString().Length == 2))
            {
                return string.Format("000{0}", id);
            }

            if ((id.ToString().Length < 4) || (id.ToString().Length == 3))
            {
                return string.Format("00{0}", id);
            }

            if ((id.ToString().Length < 5) || (id.ToString().Length == 4))
            {
                return string.Format("0{0}", id);
            }
            if ((id.ToString().Length < 6) || (id.ToString().Length == 5))
            {
                return string.Format("{0}", id);
            }
            return string.Empty;
        }

        [HttpPost]
        public async Task<ActionResult> insertWorkPolicyInfo(PolicyMT wpmt, [System.Web.Http.FromBody] PolicyMtResquest request)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            if (Session["authorize"] != null || resqheaderval[0] == "Mob")
            {
                int countlist = await _IStandardManagement.CountListPolicyMT();
                var code_tmp = string.Empty;
                var subcodeconvert = 0;
                if (countlist == 0)
                {
                    code_tmp = "P00001";
                }
                else
                {
                    var listPolicyMT = await _IStandardManagement.GetListPolicyMT();
                    var listlast = listPolicyMT.ToList().LastOrDefault().policy_code;
                    var subbbno = listlast.Substring(listlast.Length - 5, 5);
                    int.TryParse(subbbno, out subcodeconvert);
                    code_tmp = "P" + string.Format("{0}{1}", code_tmp, autoPono((subcodeconvert + 1)));
                }
                wpmt.policy_code = code_tmp;
                wpmt.policy_name = request.policy_name;
                wpmt.work_starttime = request.work_start;
                wpmt.work_endtime = request.work_end;
                wpmt.lunch_start_time = request.lunch_start;
                wpmt.lunch_end_time = request.lunch_end;
                wpmt.dinner_start_time = request.dinner_start;
                wpmt.dinner_end_time = request.dinner_end;
                wpmt.work_hour = Convert.ToDecimal(request.work_hour);
                wpmt.use_yn = request.use_yn;
                wpmt.re_mark = request.re_mark;
                wpmt.reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                wpmt.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                wpmt.reg_dt = DateTime.Now;
                wpmt.chg_dt = DateTime.Now;
                wpmt.policy_start_dt = DateTime.Now;
                wpmt.policy_end_dt = DateTime.Now;
                wpmt.last_yn = "Y";

                await _IStandardManagement.InsertIntoPolicyMT(wpmt);
                return Json(new { result = true, message = "Thêm dữ liệu vào thành công!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpDelete]
        public async Task<JsonResult> deletetWorkPolicyInfo(int id)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            if (Session["authorize"] != null || resqheaderval[0] == "Mob")
            {
                var policy = await _IStandardManagement.GetPolicyMTById(id);
                if (policy == null)
                {
                    return Json(new { result = false, message = "Không tìm thấy dữ liệu trong hệ thống" }, JsonRequestBehavior.AllowGet);
                }
                await _IStandardManagement.DeletePolicyMTById(id);
                return Json(new { result = true, message = "Xóa dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPut]
        public async Task<ActionResult> updateWorkPolicyInfo([System.Web.Http.FromBody] PolicyMtResquest request)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            if (Session["authorize"] != null || resqheaderval[0] == "Mob")
            {
                var wpmt = await _IStandardManagement.GetPolicyMTById(request.wid);

                if (wpmt == null)
                {
                    return Json(new { result = false, message = "Không tìm thấy dữ liệu trong hệ thống" }, JsonRequestBehavior.AllowGet);
                }

                wpmt.policy_name = request.policy_name;
                wpmt.work_starttime = request.work_start;
                wpmt.work_endtime = request.work_end;

                wpmt.lunch_start_time = request.lunch_start;
                wpmt.lunch_end_time = request.lunch_end;
                wpmt.dinner_start_time = request.dinner_start;
                wpmt.dinner_end_time = request.dinner_end;

                wpmt.work_hour = Convert.ToDecimal(request.work_hour);
                wpmt.chg_dt = DateTime.Now;
                wpmt.policy_end_dt = DateTime.Now;
                wpmt.last_yn = "N";
                wpmt.use_yn = request.use_yn;
                wpmt.re_mark = request.re_mark;
                wpmt.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                await _IStandardManagement.UpdatePolicy(wpmt);
                return Json(new { result = true, message = "Cập nhập dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region NoticeManagement
        public ActionResult NoticeManagement()
        {
            return View();
        }


        public ActionResult NoticeCreate()
        {
            return View();
        }

        #endregion

        #region WOCommon(M)

        //Comm_MT
        public ActionResult WOCommon()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> getWHSCommon()
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            if (Session["authorize"] != null || resqheaderval[0] == "Mob")
            {
                var data = await _IStandardManagement.GetListCommMT();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập"}, JsonRequestBehavior.AllowGet);
            }

        }

        public string automtcd(int id)
        {
            if (id.ToString().Length < 2)
            {
                return string.Format("00{0}", id);
            }

            if ((id.ToString().Length < 3) || (id.ToString().Length == 2))
            {
                return string.Format("0{0}", id);
            }

            if ((id.ToString().Length < 4) || (id.ToString().Length == 3))
            {
                return string.Format("{0}", id);
            }
            return string.Empty;
        }

        [HttpPost]
        public async Task<ActionResult> insertWHSCommon(CommMt comm_mt, string mt_cd, string mt_nm, string mt_exp, string use_yn)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            if (Session["authorize"] != null || resqheaderval[0] == "Mob")
            {
                try
                {
                    int countlist = await _IStandardManagement.CheckCommMT();
                    var mt_cd_tmp = string.Empty;
                    var submtcdconvert = 0;

                    var listCommMT = await _IStandardManagement.GetListCommMT();
                    var listlast = listCommMT.OrderBy(item => item.mt_cd).LastOrDefault();
                    if (countlist == 0)
                    {
                        mt_cd = "MMS001";
                    }
                    else
                    {
                        var bien = listlast.mt_cd;
                        var submtcd = bien.Substring(bien.Length - 3, 3);
                        int.TryParse(submtcd, out submtcdconvert);
                        mt_cd_tmp = "MMS" + string.Format("{0}{1}", mt_cd_tmp, automtcd((submtcdconvert + 1)));
                        mt_cd = mt_cd_tmp;
                    }

                    DateTime reg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
                                                    //String dateString = reg_dt.ToString("yyyy-MM-dd HH:mm:ss.FFF");

                    DateTime chg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
                                                    // String dateChString = chg_dt.ToString("yyyy-MM-dd HH:mm:ss.FFF");

                    comm_mt.mt_cd = mt_cd;
                    comm_mt.div_cd = "MMS";
                    comm_mt.mt_nm = mt_nm;
                    comm_mt.mt_exp = mt_exp;
                    comm_mt.use_yn = use_yn;
                    comm_mt.reg_dt = reg_dt;
                    comm_mt.chg_dt = chg_dt;

                    await _IStandardManagement.InsertIntoCommMT(comm_mt);
                    return Json(new { result = true, message = "Thêm dữ liệu vào thành công !", data = comm_mt }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                var kq = new CommMt();
                return Json(new { result = false, message = "Xin vui lòng đăng nhập!", data = kq }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpPut]
        public async Task<ActionResult> updatetWHSCommon(int mt_id, string mt_cd, string mt_nm, string mt_exp, string use_yn)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            if (Session["authorize"] != null || resqheaderval[0] == "Mob")
            {
                try
                {
                    var comm_mt = await _IStandardManagement.GetCommMTById(mt_id);
                    if (comm_mt == null)
                    {
                        return Json(new { result = false, message = "Dữ liệu " + mt_id + " này không tồn tại trong hệ thống" }, JsonRequestBehavior.AllowGet);
                    }

                    DateTime chg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
                    DateTime reg_dt = DateTime.Now;
                    //String dateChString = chg_dt.ToString("yyyy-MM-dd HH:mm:ss.FFF");
                    comm_mt.mt_cd = mt_cd;
                    comm_mt.mt_nm = mt_nm;
                    comm_mt.mt_exp = mt_exp;
                    comm_mt.use_yn = use_yn;
                    comm_mt.chg_dt = chg_dt;
                    comm_mt.reg_dt = reg_dt;
                    await _IStandardManagement.UpdateCommMT(comm_mt);
                    return Json(new { result = true, message = "Cập nhập liệu thành công !" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);

            }

        }

        [HttpDelete]
        public async Task<JsonResult> deleteWHSCommon(int mt_id)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            if (Session["authorize"] != null || resqheaderval[0] == "Mob")
            {
                var checkCommMt = await _IStandardManagement.GetCommMTById(mt_id);
                if (checkCommMt.mt_id < 1)
                {
                    return Json(new { result = false, message = "Không tìm thấy " + mt_id + " này trong hệ thống !" }, JsonRequestBehavior.AllowGet);
                }
                await _IStandardManagement.DeleteCommMT(checkCommMt.mt_id);
                return Json(new { result = true, message = "Xóa thành công !" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }

        }

        //Comm_DT (CommCode)
        [HttpGet]
        public async Task<JsonResult> searchWHSCommon(string mt_cd, string mt_nm, string mt_exp)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            if (Session["authorize"] != null || resqheaderval[0] == "Mob")
            {
                var data = await _IStandardManagement.SearchCommMT(mt_cd, mt_nm, mt_exp);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var kq = new List<CommMt>();
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !", data = kq}, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<ActionResult> getWHSCommonDtData(string mt_cd)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            if (Session["authorize"] != null || resqheaderval[0] == "Mob")
            {
                var data = await _IStandardManagement.GetListComMTDetail(mt_cd);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var kq = new List<Models.Response.WOCommonResponse>();
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !", data = kq }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public async Task<ActionResult> insertWHSCommonDt(CommCode commDt,string dt_cd, string dt_nm, string dt_exp, string use_yn, string mt_cd, int dt_order)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            if (Session["authorize"] != null || resqheaderval[0] == "Mob")
            {
                try
                {
                    var checkCommDT = await _IStandardManagement.checkCommDT(mt_cd, dt_cd);
                    if (checkCommDT > 0)
                    {
                        return Json(new { result = false, message = "Đã tồn tại " + dt_cd + " này trong hệ thống" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        DateTime reg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
                                                        //String dateString = reg_dt.ToString("yyyy-MM-dd HH:mm:ss.FFF");

                        DateTime chg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
                                                        //String dateChString = chg_dt.ToString("yyyy-MM-dd HH:mm:ss.FFF");

                        commDt.mt_cd = mt_cd;
                        commDt.dt_cd = dt_cd;
                        commDt.dt_nm = dt_nm;
                        commDt.dt_exp = dt_exp;
                        commDt.dt_order = dt_order;
                        commDt.use_yn = use_yn;
                        commDt.reg_dt = reg_dt;
                        commDt.chg_dt = chg_dt;
                        commDt.del_yn = "N";

                        await _IStandardManagement.InsertIntoCommDT(commDt);
                        return Json(new { result = true, message = "Thêm dữ liệu vào thành công !", data = commDt }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                var kq = new CommCode();
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !", data = commDt }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPut]
        public async Task<ActionResult> updateWHSCommonDt(int cdid, string dt_nm, string dt_exp, string use_yn, int dt_order)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            if (Session["authorize"] != null || resqheaderval[0] == "Mob")
            {
                try
                {
                    var comm_dt = await _IStandardManagement.GetCommDTById(cdid);
                    if (comm_dt == null)
                    {
                        return Json(new { result = false, message = "Dữ liệu " + dt_nm + " này không tồn tại trong hệ thống" }, JsonRequestBehavior.AllowGet);
                    }

                    DateTime chg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
                    DateTime reg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
                                                    //String dateChString = chg_dt.ToString("yyyy-MM-dd HH:mm:ss.FFF");
                    comm_dt.dt_nm = dt_nm;
                    comm_dt.dt_exp = dt_exp;
                    comm_dt.dt_order = dt_order;
                    comm_dt.use_yn = use_yn;
                    comm_dt.reg_dt = reg_dt;
                    comm_dt.chg_dt = chg_dt;
                    await _IStandardManagement.UpdateCommDT(comm_dt);
                    return Json(new { result = true, message = "Cập nhập liệu thành công !" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpDelete]
        public async Task<JsonResult> deleteWHSCommonDt(int cdid)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            if (Session["authorize"] != null || resqheaderval[0] == "Mob")
            {
                var checkCommDT = await _IStandardManagement.GetCommDTById(cdid);
                if (checkCommDT.cdid < 1)
                {
                    return Json(new { result = false, message = "Không tìm thấy " + cdid + " này trong hệ thống !" }, JsonRequestBehavior.AllowGet);
                }
                await _IStandardManagement.DeleteCommDT(checkCommDT.cdid);
                return Json(new { result = true, message = "Xóa thành công !" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion

        #region Factory_Location
        public ActionResult Factory_Location()
        {
            return SetLanguage("");
        }

        public ActionResult PrintFactory(string id)
        {
            ViewData["Message"] = id;
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> QRbarcodefactory(string id)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            if (Session["authorize"] != null || resqheaderval[0] == "Mob")
            {
                if (id != "")
                {
                    int Id = Int32.Parse(id);
                    var data = await _IStandardManagement.GetDataLocationInfoById(Id);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                return View();
            }
            else
            {
                var kq = new LocationInfo();
                return Json(new { reuslt = false, message = "Xin vui lòng đăng nhập !", data = kq }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpGet]
        public async Task<ActionResult> GetWHfactory_wo()
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            if (Session["authorize"] != null || resqheaderval[0] == "Mob")
            {
                var value = await _IStandardManagement.GetListLocationInfo();
                return Json(new { rows = value }, JsonRequestBehavior.AllowGet);

            }
            else {
                var kq = new List<LocationInfo>();
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !", data = kq }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<JsonResult> get_lct_001(string lct_cd)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            if (Session["authorize"] != null || resqheaderval[0] == "Mob")
            {
                var rs = await _IStandardManagement.GetListLocationInfo(lct_cd);
                var list = rs.ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var kq = new List<LocationInfo>();
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !", data = kq }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public async Task<ActionResult> insertfactory_lct(LocationInfo WHL, string lct_nm, string re_mark, string mn_x, int lctno, string use_yn, string mv_yn, string wp_yn, string ft_yn, string nt_yn, string root_yn)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            if (Session["authorize"] != null || resqheaderval[0] == "Mob")
            {
                WHL.mv_yn = mv_yn;
                WHL.wp_yn = wp_yn;
                WHL.ft_yn = ft_yn;
                WHL.nt_yn = nt_yn;
                if (nt_yn == null)
                {
                    WHL.nt_yn = "N";
                }
                if (wp_yn == null)
                {
                    WHL.wp_yn = "N";
                }
                if (ft_yn == null)
                {
                    WHL.ft_yn = "N";
                }
                if (mv_yn == null)
                {
                    WHL.mv_yn = "N";
                }
                string[] listtring = { "0", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

                WHL.lct_nm = lct_nm;
                WHL.use_yn = use_yn;
                WHL.order_no = 1;
                WHL.re_mark = re_mark;
                //var list = db.lct_info.Where(item => item.lct_cd.StartsWith("002")).
                //   OrderBy(item => item.lct_cd).ThenBy(item => item.level_cd).ToList();
                var rs = await _IStandardManagement.GetListLocationInfo();
                var listData = rs.ToList().OrderBy(item => item.lct_cd).ThenBy(item => item.level_cd);
                var list = listData.ToList();

                var menuCd = string.Empty;
                var subMenuIdConvert = 0;
                var sublog = new LocationInfo();
                if (root_yn == "1")
                {
                    var ds = list.Where(x => x.level_cd == "000").OrderByDescending(item => item.reg_dt).ToList();
                    if (ds.Count() == 0)
                    {
                        WHL.lct_cd = "002001000000000000";
                        WHL.level_cd = "000";
                        WHL.index_cd = "F1";
                        WHL.shelf_cd = "0";
                        WHL.lct_bar_cd = "F1";
                        WHL.up_lct_cd = "002001000000000000";
                        WHL.mn_full = lct_nm;
                    }
                    else
                    {
                        sublog = ds.FirstOrDefault();
                        menuCd = "002";
                        WHL.level_cd = "000";
                        WHL.shelf_cd = "0";

                        if (sublog != null)
                        {
                            var subMenuId = sublog.lct_cd.Substring(3, 3);
                            int.TryParse(subMenuId, out subMenuIdConvert);
                        }
                        var giatri = CreateId_F(subMenuIdConvert + 1);
                        menuCd = string.Format("{0}{1}000000000000", menuCd, giatri);
                        WHL.lct_cd = menuCd;
                        WHL.index_cd = "F" + int.Parse(giatri);
                        WHL.up_lct_cd = menuCd;
                        WHL.mn_full = lct_nm;
                        WHL.lct_bar_cd = WHL.index_cd;
                    }
                }
                else
                {
                    //var upLevel = db.lct_info.FirstOrDefault(item => item.lctno == lctno);
                    var upLevel = await _IStandardManagement.GetDataLocationInfoById(lctno);
                    WHL.up_lct_cd = upLevel.lct_cd;
                    var subnew = "";

                    switch (upLevel.level_cd)
                    {
                        case "000":
                            menuCd = upLevel.lct_cd.Substring(0, 6);
                            WHL.level_cd = "001";
                            sublog = list.Where(item => item.up_lct_cd == upLevel.lct_cd).OrderByDescending(item => item.reg_dt).FirstOrDefault();
                            if (sublog != null)
                            {
                                var subMenuId = sublog.lct_cd.Substring(6, 3);
                                int.TryParse(subMenuId, out subMenuIdConvert);
                            }
                            var giatri = CreateId_F(subMenuIdConvert + 1);
                            menuCd = string.Format("{0}{1}000000000", menuCd, giatri);
                            WHL.lct_cd = menuCd;
                            WHL.shelf_cd = (int.Parse(giatri)).ToString();
                            WHL.index_cd = upLevel.index_cd;
                            WHL.mn_full = list.FirstOrDefault().mn_full + ">" + WHL.lct_nm;
                            WHL.lct_bar_cd = WHL.index_cd + "-" + WHL.shelf_cd;
                            break;
                        case "001":
                            menuCd = upLevel.lct_cd.Substring(0, 9);
                            WHL.level_cd = "002";
                            var test = mn_x;
                            sublog = (mn_x == null || mn_x == "" ?
                             list.Where(item => item.up_lct_cd == upLevel.lct_cd).OrderByDescending(item => item.reg_dt).FirstOrDefault() :
                             list.Where(item => item.up_lct_cd == mn_x).OrderByDescending(item => item.reg_dt).FirstOrDefault());
                            if (sublog != null)
                            {
                                subnew = sublog.lct_cd.Substring(11, 1);
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
                            menuCd = string.Format("{0}{1}", menuCd, "00" + subnew + "000000");

                            WHL.lct_cd = menuCd;
                            var trc = menuCd.Substring(3, 3);
                            WHL.index_cd = "F" + int.Parse(trc);
                            WHL.shelf_cd = subnew;
                            WHL.lct_bar_cd = upLevel.lct_bar_cd + "-" + WHL.shelf_cd;
                            WHL.mn_full = upLevel.mn_full + ">" + WHL.lct_nm;
                            break;
                        case "002":
                            menuCd = upLevel.lct_cd.Substring(0, 12);
                            WHL.level_cd = "003";
                            sublog = list.Where(item => item.up_lct_cd == upLevel.lct_cd).OrderByDescending(item => item.reg_dt).FirstOrDefault();
                            if (sublog != null)
                            {
                                var subMenuId = sublog.lct_cd.Substring(12, 3);
                                int.TryParse(subMenuId, out subMenuIdConvert);
                            }
                            var giatri1 = CreateId_F(subMenuIdConvert + 1);
                            menuCd = string.Format("{0}{1}", menuCd, giatri1) + "000";
                            WHL.lct_cd = menuCd;
                            WHL.index_cd = upLevel.index_cd;
                            WHL.shelf_cd = (int.Parse(giatri1)).ToString();
                            WHL.lct_bar_cd = upLevel.lct_bar_cd + WHL.shelf_cd;
                            WHL.mn_full = upLevel.mn_full + ">" + WHL.lct_nm;
                            break;
                    }
                }

                WHL.reg_dt = DateTime.Now;
                WHL.chg_dt = DateTime.Now;
                int id = await _IStandardManagement.InsertIntoDataLocationInfo(WHL);
                var dataLctInfo = await _IStandardManagement.GetDataLocationInfoById(id);
                if (dataLctInfo != null)
                {
                    return Json(new { result = true, value = dataLctInfo, message = "Thêm dữ liệu thành công !" });
                }
                else return Json(new { result = false, message = "Đã tồn tại " + dataLctInfo.lct_nm + " này ở trong hệ thống !" });
            }
            else
            {
                var kq = new LocationInfo();
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !", data = kq });
            }

        }

        [HttpPut]
        public async Task< ActionResult> updatefactory_lct(string lct_nm, string re_mark, int lctno, string use_yn, string mv_yn, string wp_yn, string ft_yn, string nt_yn)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            if (Session["authorize"] != null || resqheaderval[0] == "Mob")
            {
                var lct_info = await _IStandardManagement.GetDataLocationInfoById(lctno);

                lct_info.mv_yn = mv_yn;
                lct_info.use_yn = use_yn;
                lct_info.wp_yn = wp_yn;
                lct_info.ft_yn = ft_yn;
                lct_info.nt_yn = nt_yn;

                if (mv_yn == null) { lct_info.mv_yn = "N"; }
                if (nt_yn == null) { lct_info.nt_yn = "N"; }
                if (wp_yn == null) { lct_info.wp_yn = "N"; }
                if (ft_yn == null) { lct_info.ft_yn = "N"; }

                //var upLevel = db.lct_info.Where(x => x.lct_cd == lct_info.up_lct_cd).ToList();
                var rs = await _IStandardManagement.GetListLocationInfoByCode(lct_info.up_lct_cd);
                var upLevel = rs.ToList();
                if (upLevel.Count > 0)
                {

                    var mn_trc = upLevel.FirstOrDefault().mn_full;
                    lct_info.mn_full = mn_trc + ">" + lct_nm;
                }


                lct_info.lct_cd = lct_info.lct_cd;
                lct_info.re_mark = re_mark;
                lct_info.lct_nm = lct_nm;
                lct_info.chg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
                lct_info.chg_id = Session["userid"].ToString();

                int kq = await _IStandardManagement.UpdateDataLocationInfo(lct_info);
                if (kq > 0)
                {
                    return Json(new { result = true, value = lct_info, message = "Cập nhập dữ liệu thành công !" });
                }
                else return Json(new { result = false, message = "Không tìm thấy " + lct_info.lct_nm + " này ở trong hệ thống !" });
            }
            else
            {
                var kq = new LocationInfo();
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !", data = kq });
            }

            //return View(lct_info);
        }

        [HttpDelete]
        public async Task<ActionResult> deleteFactoryLocMgt(int lctno)
        {
            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            if (Session["authorize"] != null || resqheaderval[0] == "Mob")
            {
                var item_count = await _IStandardManagement.GetDataLocationInfoById(lctno);
                if (item_count != null)
                {
                    await _IStandardManagement.DeleteDataLocationInfoById(item_count.lctno);
                    return Json(new { result = true, message = "Xoa dữ liệu thành công !" }, JsonRequestBehavior.AllowGet);
                }
                else return Json(new { result = false, message = "Không tìm thấy " + item_count.lct_nm + " này ở trong hệ thống" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var kq = new LocationInfo();
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !", data = kq }, JsonRequestBehavior.AllowGet);
            }

        }

        private string CreateId_F(int id)
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

        [HttpGet]
        public async Task<ActionResult> SearchFactory_Location(string lct_cd, string lct_nm)
        {

            var resqheader = Request.Headers;
            string[] resqheaderkey = resqheader.AllKeys;
            string[] resqheaderval = resqheader.GetValues("requestFrom");
            if (Session["authorize"] != null || resqheaderval[0] == "Mob")
            {
                if (lct_cd == "002001001000000000" ||  lct_cd == "002001002000000000" || lct_cd == "002001003000000000" || lct_cd == "002001004000000000" || lct_cd == "002001005000000000" ||
                   lct_cd == "002001006000000000" || lct_cd == "002001007000000000" || lct_cd == "002001008000000000" || lct_cd == "002001009000000000")
                {
                    var res = lct_cd.Substring(0, 11);
                    lct_cd = res;
                }
                var value = await _IStandardManagement.SearchLocationInfọ(lct_cd, lct_nm);
                return Json(value, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var kq = new List<LocationInfo>();
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !", data = kq }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

    }
}