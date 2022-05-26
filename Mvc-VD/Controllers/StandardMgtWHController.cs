using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_VD.Models;
using System.Text;
using System.Globalization;
using System.Collections;
using Mvc_VD.Services.Interface;
using System.Threading.Tasks;
using Mvc_VD.Models.NewVersion;

namespace Mvc_VD.Controllers
{
    public class StandardMgtWHController : Controller
    {
        private Entities db = new Entities();
        private readonly ITimsService _ITimsService;
        private readonly IhomeService _homeService;

        public StandardMgtWHController(ITimsService ITimsService, IhomeService ihomeService)
        {
            _homeService = ihomeService;
            _ITimsService = ITimsService;
        }
        #region WMS COMMON
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
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
        [HttpGet]
        public async Task<ActionResult> WarehouseCommon()
        {
            HttpCookie cookie = HttpContext.Request.Cookies["language"];
            if (cookie != null)
            {
                ViewBag.language = cookie.Value;
            }
            var data = await _ITimsService.GetDataComCode();
            return View(data);

        }

        [HttpGet]
        public async Task<ActionResult> getWHSCommon(CommMt commMT)
        {
            try
            {
                var result = await _ITimsService.GetDataComCode();
                var data = new { rows = result };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
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
            try
            {

                int countlist = await _ITimsService.CountComCode();
                var mt_cd_tmp = string.Empty;
                var submtcdconvert = 0;
                var listlast = await _ITimsService.GetLastDataComCode();
                if (countlist == 0)
                {
                    mt_cd = "WHS001";
                }
                else
                {
                    var bien = listlast.mt_cd;
                    var submtcd = bien.Substring(bien.Length - 3, 3);
                    int.TryParse(submtcd, out submtcdconvert);

                    mt_cd_tmp = "WHS" + string.Format("{0}{1}", mt_cd_tmp, automtcd((submtcdconvert + 1)));
                    mt_cd = mt_cd_tmp;
                }


                comm_mt.mt_cd = mt_cd;
                comm_mt.div_cd = "WHS";
                comm_mt.mt_nm = mt_nm;
                comm_mt.mt_exp = mt_exp;
                comm_mt.use_yn = use_yn;
                comm_mt.reg_dt = DateTime.Now;
                comm_mt.chg_dt = DateTime.Now;
                comm_mt.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                comm_mt.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();

                var mt_id = await _ITimsService.InsertIntoCommMt(comm_mt);
                var value = await _ITimsService.GetCommMtById(mt_id);
                return Json(new { result = true, kq = value, message = "Thêm dữ liệu vào thành công !" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPut]
        public async Task<ActionResult> updatetWHSCommon(int mt_id, string mt_cd, string mt_nm, string mt_exp, string use_yn)
        {
            try
            {
                var commMT = await _ITimsService.GetCommMtById(mt_id);
                commMT.mt_cd = mt_cd;
                commMT.mt_nm = mt_nm;
                commMT.mt_exp = mt_exp;
                commMT.use_yn = use_yn;
                commMT.reg_dt = DateTime.Now;
                commMT.chg_dt = DateTime.Now;
                commMT.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                commMT.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();

                await _ITimsService.UpdateCommMt(commMT);
                return Json(new { result = true, kq = commMT, message = "Chỉnh sửa dữ liệu thành công !" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpDelete]
        public async Task<JsonResult> deleteWHSCommon(int mt_id)
        {
            var commonMT = await _ITimsService.GetCommMtById(mt_id);
            if (commonMT != null)
            {
                await _ITimsService.RemoveCommMt(commonMT.mt_id);

                var rs = await _ITimsService.GetListDataCommDTByMtCd(commonMT.mt_cd);
                var value2 = rs.ToList();
                foreach (var item in value2)
                {
                    var commonDT = await _ITimsService.GetCommCodeById(item.cdid);
                    if (commonDT != null)
                    {
                        await _ITimsService.RemoveCommDT(commonDT.cdid);
                    }

                }
                return Json(new { result = true, message = "Xóa dữ liệu thành công !" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Không tìm thấy dữ liệu này !" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<JsonResult> searchWHSCommon(string mt_cd, string mt_nm, string mt_exp)
        {
            var data = await _ITimsService.SearchDataCommCode(mt_cd, mt_nm, mt_exp);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> getWHSCommonDtData(string mt_cd)
        {
            try
            {
                var data = await _ITimsService.getWHSCommonDetailData(mt_cd);
                var value = data.ToList();
                return Json(new { rows = value}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        public async Task<ActionResult> insertWHSCommonDt(CommCode comm_dt, string dt_cd, string dt_nm, string dt_exp, string use_yn, string mt_cd, int dt_order)
        {
            try
            {
                //int count = db.comm_dt.Where(item => item.dt_cd == dt_cd && item.mt_cd == mt_cd).ToList().Count();
                int count = await _ITimsService.CountCommDT(dt_cd, mt_cd);
                if (count > 0)
                {
                    return Json(new { result = false, message = "Đã tồn tại dữ liệu này ở trong hệ thống!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    comm_dt.mt_cd = mt_cd;
                    comm_dt.dt_cd = dt_cd;
                    comm_dt.dt_nm = dt_nm;
                    comm_dt.dt_exp = dt_exp;
                    comm_dt.dt_order = dt_order;
                    comm_dt.use_yn = use_yn;
                    comm_dt.reg_dt = DateTime.Now;
                    comm_dt.chg_dt = DateTime.Now;
                    comm_dt.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    comm_dt.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    comm_dt.del_yn = "N";
                    var cdid = await _ITimsService.InsertIntoCommDT(comm_dt);
                    var result = await _ITimsService.GetDataCommDTById(cdid);
                    return Json(new { result = true, kq = result, message = "Thêm dữ liệu vào thành công !" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        [HttpPut]
        public async Task<ActionResult> updateWHSCommonDt(int cdid, string dt_nm, string dt_exp, string use_yn, int dt_order)
        {
            try
            {
                var commDT = await _ITimsService.GetDataCommDTById(cdid);
                commDT.dt_nm = dt_nm;
                commDT.dt_exp = dt_exp;
                commDT.dt_order = dt_order;
                commDT.use_yn = use_yn;
                commDT.reg_dt = DateTime.Now;
                commDT.chg_dt = DateTime.Now;
                commDT.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                commDT.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                await _ITimsService.UpdateCommDT(commDT);
                return Json(new { result = true, kq = commDT, message = "Chỉnh sửa dữ liệu thành công !" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        public async Task<JsonResult> deleteWHSCommonDt(string mt_cd, int cdid)
        {
            var WHSCommonDt = await _ITimsService.GetDataCommDT(cdid, mt_cd);
            if (WHSCommonDt == null)
            {
                return Json(new { result = false, message = "Không tìm thấy dữ liệu này !" }, JsonRequestBehavior.AllowGet);
            }
            await _ITimsService.RemoveCommDT(WHSCommonDt.cdid);
            return Json(new { result = true, message = "Xóa dữ liệu thành công !"}, JsonRequestBehavior.AllowGet);
        }


        //        mt_cd: $('#m_mt_cd').val(),
        //cdid: $('#dm_cdid').val(),
        #endregion


        #region Warehouse Location(M)
        public ActionResult PrintWahouse(string id)
        {
            ViewData["Message"] = id;
            return View();
        }
        public ActionResult QRbarcodewwh(string id)
        {
            if (id != "")
            {
                var sql = new StringBuilder();
                sql.Append(" SELECT * ")
                    .Append("FROM  lct_info as a ")
                     .Append("where a.lctno in (" + id + ")");
                var data = db.lct_info.SqlQuery(sql.ToString()).ToList<lct_info>();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return View();

        }
        public ActionResult WarehouseLocation()
        {
            return View();
        }
        // location
        public ActionResult locMgt()
        {
            return View();
        }
        public ActionResult GetWHlocMgt()
        {
            var vaule = db.lct_info.Where(item => item.index_cd.StartsWith("W")).OrderBy
              (item => item.lct_cd).ToList();
            return Json(new { rows = vaule }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult insertlocMgt(lct_info WHL, string lct_nm, string re_mark, int lctno, string use_yn, string real_use_yn, string mv_yn, string nt_yn)
        {
            WHL.mv_yn = mv_yn;
            WHL.nt_yn = nt_yn;
            WHL.use_yn = use_yn;
            WHL.real_use_yn = real_use_yn;
            WHL.sf_yn = WHL.sf_yn;
            if (WHL.nt_yn == null)
            {
                WHL.nt_yn = "N";
            }
            if (WHL.sf_yn == null)
            {
                WHL.sf_yn = "N";
            }
            if (WHL.mv_yn == null)
            {
                WHL.mv_yn = "N";
            }

            string[] listtring = { "0", "A", "B", "C", "D", "E", "F", "G", "H","I","J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            WHL.lct_nm = lct_nm;
            WHL.order_no = 1;
            WHL.re_mark = re_mark;
            string root_yn = Request.QueryString["c_root_yn"];
            var list = db.lct_info.Where(item => item.index_cd.StartsWith("W")).
                  OrderBy(item => item.lct_cd).ThenBy(item => item.level_cd).ToList();
            var upLevel = db.lct_info.FirstOrDefault(item => item.lctno == lctno);
            if (list.Count == 0)
            {
                WHL.index_cd = "W01";
                WHL.lct_cd = "001W01000000000000";
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
                    menuCd = string.Format("{0}{1}", menuCd, CreateId((subMenuIdConvert + 1)));


                    WHL.index_cd = "W" + menuCd;
                    WHL.lct_cd = "001" + WHL.index_cd + "000000000000";
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
                            Whcd = string.Format("{0}{1}000000", Whcd, "0" + CreateId((subMenuIdConvert + 1)));
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
                            Whcd = string.Format("{0}{1}", Whcd, "0" + CreateId((subMenuIdConvert + 1)));
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
                            Whcd = string.Format("{0}{1}", Whcd, "W" + CreateId((subMenuIdConvert + 1)) + "000000000000");

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
            WHL.reg_id = Session["userid"].ToString();
            WHL.chg_id = Session["userid"].ToString();

            if (WHL.lct_cd.Contains("001W0100A"))
            {
                WHL.real_use_yn = "N";

            }
            if (ModelState.IsValid)
            {
                db.Entry(WHL).State = EntityState.Added;
                db.SaveChanges();
                return Json(WHL, JsonRequestBehavior.AllowGet); ;
            }

            return View(WHL);
        }
        public ActionResult updatewhlocMgt(string mv_yn, string lct_nm, string lct_cd, int lctno, string sf_yn, string nt_yn ,string use_yn, string real_use_yn, string re_mark)
        {
            var count_b = db.lct_info.Count(x => x.lctno == lctno);
            if (count_b != 0)
            {
                if (sf_yn == null)
                {
                    sf_yn = "N";
                }
                if (mv_yn == null)
                {
                    mv_yn = "N";
                }
                if (nt_yn == null)
                {
                    nt_yn = "N";
                }
                lct_info b = db.lct_info.Find(lctno);
                b.lct_nm = lct_nm;
                b.use_yn = use_yn;
                b.real_use_yn = real_use_yn;
                b.mn_full = b.lct_nm;
                b.re_mark = re_mark;
                b.sf_yn = sf_yn;
                b.nt_yn = nt_yn;
                b.mv_yn = mv_yn;
                b.chg_id = Session["userid"].ToString();
                b.chg_id = Session["userid"].ToString();
                DateTime chg_dt = DateTime.Now;
                String dateChString = chg_dt.ToString("yyyy-MM-dd HH:mm:ss.FFF");
                b.chg_dt = chg_dt;
                db.SaveChanges();
                return Json(new { result = count_b, lct_cd = b.lct_cd }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = count_b, lct_cd = db.lct_info.FirstOrDefault(x => x.lctno == lctno).lct_cd }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult deleteWHlocMgt(int lctno)
        {
            lct_info lct_info = db.lct_info.Find(lctno);
            if (lct_info != null)
            {
                db.lct_info.Remove(lct_info);
                db.SaveChanges();
                var result = new { result = 1 };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new { result = 0 };
                return Json(result, JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult SearchWhLocation()
        {

            // Get Data from ajax function
            var W = Request["whouse"];
            var A = Request["aisle"];
            var B = Request["bay"];

            var sql = new StringBuilder();
            sql.Append(" SELECT * ")
            .Append("FROM  lct_info as a ")
            .Append("Where a.index_cd like 'W%' and ('" + A + "'='' OR  (SUBSTRING(a.lct_cd,9,1)) like '%" + A + "%' )")
            .Append("AND ('" + B + "'='' OR  (SUBSTRING(a.lct_cd,11, 2)) like '%" + B + "%' )")
            .Append("AND ('" + W + "'='' OR  a.index_cd like '%" + W + "%')");

            var data = db.lct_info.SqlQuery(sql.ToString()).ToList<lct_info>();
            return Json(data, JsonRequestBehavior.AllowGet); ;
        }
        public ActionResult Warehouse(lct_info lct_info)
        {
               var sql = new StringBuilder();
               sql.Append(" SELECT * ")
               .Append("FROM  lct_info as a ")
               .Append("Where a.lct_cd like '001%' GROUP  by a.index_cd");
            var data = db.lct_info.SqlQuery(sql.ToString()).ToList<lct_info>();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Aisle(lct_info lct_info)
        {
            var lists = db.lct_info.Where(item => item.index_cd.StartsWith("W")).
              OrderBy(item => item.lct_cd).ThenBy(item => item.level_cd).ToList();
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

        public ActionResult Bay(lct_info lct_info)
        {
            var lists = db.lct_info.Where(item => item.index_cd.StartsWith("W")).
              OrderBy(item => item.lct_cd).ThenBy(item => item.level_cd).ToList();
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
        //// GET: /system/
        private string CreateId(int id)
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
        #endregion


    }
}
