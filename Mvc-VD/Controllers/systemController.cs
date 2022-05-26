using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Mvc_VD.Models;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Net.Http;
using Mvc_VD.Services.Interface;
using System.Threading.Tasks;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Commons;

namespace Mvc_VD.Controllers
{
    public class systemController : BaseController
    {
        private Entities db = new Entities();

        private IMenuServices _menuServices;
        private IStandardServices _standardServices;
        private IdevManagementService _IdevManagementService;
        private IhomeService _IHomeService;


        string exs = "Lỗi hệ thống!!!";
        string ss = "Thành công!!!";

        public systemController(IMenuServices menuServices, IdevManagementService IdevManagementService, IStandardServices standardServices,IhomeService ihomeService)
        {
            _menuServices = menuServices;
            _standardServices = standardServices;
            _IdevManagementService = IdevManagementService;
            _IHomeService = ihomeService;
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

        #region User Mgt
        //Begin Layout
        public ActionResult Index()
        {
            //ViewBag.userid = Session["userid"];
            //ViewBag.authData = Session["authData"] + "@";
            //ViewBag.authList = Session["authList"];
            return View();
        }

        public ActionResult userMgt()
        {
            return SetLanguage("");
        }

        //public ActionResult searchpp_staff(string uname)
        //{
        //    var sql = new StringBuilder();
        //    sql.Append(" SELECT * ")
        //        .Append("FROM  mb_info as a ")
        //        .Append("LEFT JOIN comm_dt as b ON ")
        //        .Append("( a.position_cd = b.dt_cd )")
        //        .Append("Where ('" + uname + "'='' OR  a.uname like '%" + uname + "%' )");

        //    var data = new DataTable();
        //    using (var cmd = db.Database.Connection.CreateCommand())
        //    {
        //        db.Database.Connection.Open();
        //        cmd.CommandText = sql.ToString();
        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            data.Load(reader);
        //        }
        //    }
        //    db.Database.Connection.Close();
        //    var result = GetJsonPersons_search(data);
        //    return result;
        //}

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

        public JsonResult GetJsonPersons_search(DataTable data)
        {
            var lstPersons = GetTableRows(data);
            return Json(lstPersons, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> getgrade()
        {
            if (Session["authorize"] != null)
            {

                var list =await _IdevManagementService.GetListGrande();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> userInfoData()
        {
            try
            {
                if (Session["authorize"] != null)
                {

                    var mb_info_list =await _IdevManagementService.GetListUserById();
                    var jsonData = new
                    {
                        rows = mb_info_list
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public JsonResult insertUser(string userid, string uname, string upw, string nick_name, string cel_nb, string e_mail, string memo, string grade, string scr_yn, string mail_yn)
        {
            mb_info mb_info = new mb_info();
            var result = db.mb_info.Count(x => x.userid == userid);
            if ((result == 0) && (userid != "") && (upw != ""))
            {
                mb_info.userid = userid;
                mb_info.uname = uname;
                mb_info.nick_name = nick_name;
                mb_info.upw = upw;
                mb_info.grade = grade;
                mb_info.cel_nb = cel_nb;
                mb_info.e_mail = e_mail;
                mb_info.mail_yn = mail_yn;
                mb_info.ltacc_dt = DateTime.Now;
                mb_info.mbout_dt = DateTime.Now;
                mb_info.session_limit = DateTime.Now;
                mb_info.memo = memo;
                mb_info.mbjoin_dt = DateTime.Now;
                mb_info.scr_yn = scr_yn;
                mb_info.reg_dt = DateTime.Now;
                mb_info.chg_dt = DateTime.Now;
                db.mb_info.Add(mb_info);
                db.SaveChanges();

                mb_author_info mb_author_info = new mb_author_info();
                mb_author_info.userid = userid;
                mb_author_info.at_cd = db.author_info.FirstOrDefault(x => x.at_nm == grade).at_cd;
                mb_author_info.reg_dt = DateTime.Now;
                mb_author_info.chg_dt = DateTime.Now;
                db.mb_author_info.Add(mb_author_info);
                db.SaveChanges();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult modifyUser(string userid, string uname, string upw, string nick_name, string cel_nb, string e_mail, string memo, string grade, string scr_yn, string mail_yn)
        {

            var result = db.mb_info.Count(x => x.userid == userid);
            if ((result != 0) && (userid != "") && (upw != ""))
            {
                var mb_info = db.mb_info.FirstOrDefault(x => x.userid == userid);
                var at_cd = db.author_info.FirstOrDefault(x => x.at_nm == grade).at_cd;
                mb_info.uname = uname;
                mb_info.nick_name = nick_name;
                mb_info.upw = upw;
                mb_info.grade = grade;
                mb_info.cel_nb = cel_nb;
                mb_info.e_mail = e_mail;
                mb_info.mail_yn = mail_yn;
                mb_info.memo = memo;
                mb_info.scr_yn = scr_yn;
                mb_info.chg_dt = DateTime.Now;
                db.SaveChanges();

                int CountMbAuthor = db.mb_author_info.Count(x => x.userid == userid);
                if (CountMbAuthor > 0)
                {
                    var mb_author_info = db.mb_author_info.FirstOrDefault(x => x.userid == userid);
                    db.mb_author_info.Remove(mb_author_info);
                    db.SaveChanges();
                }
                var insert_mb_author_info = new mb_author_info();
                insert_mb_author_info.userid = userid;
                insert_mb_author_info.at_cd = at_cd;
                insert_mb_author_info.reg_dt = DateTime.Now;
                insert_mb_author_info.chg_dt = DateTime.Now;
                db.mb_author_info.Add(insert_mb_author_info);
                db.SaveChanges();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpDelete]
        public async Task<JsonResult> deleteUser(string userid)
        {
            var resultauth = await _IdevManagementService.GetMBAuthorInfobyUserId(userid);
            if (resultauth != null)
            {
                await _IdevManagementService.DeleteMbAuthorInfobyUserId(resultauth.userid);
            }

            var result = await _IdevManagementService.GetStaffbyUserId(userid);
            if (result == null)
            {
                return Json(new { result = false, message = "không tìm thấy " + userid + " này trong hệ thống !" }, JsonRequestBehavior.AllowGet);
            }
            await _IdevManagementService.DeleteStaffbyUserId(result.userid);
            return Json(new { result = true, message = "Xóa Thành Công !" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region CommonMgt
        public async Task<ActionResult> Common()
        {
            try
            {
                var data = await _standardServices.GetListCommonMT();
                return View(data);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        public ActionResult Create(comm_mt comm_mt)
        {
            if (ModelState.IsValid)
            {
                db.comm_mt.Add(comm_mt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(comm_mt);
        }

        [HttpPost]
        public ActionResult Edit(comm_mt comm_mt)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(comm_mt).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(comm_mt);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpGet]
        public async Task<ActionResult> GetJqGridDataCommDt(string mt_cd)
        {
            var result = await _standardServices.GetListDataCommDt(mt_cd);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> insertCommonDetail(CommCode data, string dc_mt_cd, string dc_dt_cd, string dc_dt_nm, string dc_dt_exp, int dc_dt_order, string dc_use_yn)
        {
            int count = await _IdevManagementService.CheckComDT(dc_dt_cd, dc_mt_cd);
            if (count < 1)
            {
                data.mt_cd = dc_mt_cd;
                data.dt_cd = dc_dt_cd;
                data.dt_nm = dc_dt_nm;
                data.dt_exp = dc_dt_exp;
                data.dt_order = dc_dt_order;
                data.use_yn = dc_use_yn;
                data.reg_dt = DateTime.Now;
                data.chg_dt = DateTime.Now;
                data.reg_id = Session["authName"] == null ? null : Session["authName"].ToString();
                data.chg_id = Session["authName"] == null ? null : Session["authName"].ToString();
                data.del_yn = "N";

                var cdid = await _IdevManagementService.InsertIntoComDT(data);
                var rs = await _IdevManagementService.GetCommCodeById(cdid);
                return Json(new { result = true, message = ss, value = rs }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = "Không tồn tại " + dc_mt_cd + " này trong hệ thống!" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public async Task<ActionResult> updateCommonDetail(string dm_mt_cd, string dm_dt_cd, string dm_dt_nm, string dm_dt_exp, int dm_dt_order, string dm_use_yn, int dm_cdid)
        {
            //int countcommondetail = db.comm_dt.Count(x => x.cdid == dm_cdid);
            var datacommondetail = await _IdevManagementService.GetCommCodeById(dm_cdid);
            if (datacommondetail == null)
            {
                return Json(new { result = false,message = "Không tồn tại " + dm_mt_cd + " này trong hệ thống!" }, JsonRequestBehavior.AllowGet);
            }
            datacommondetail.mt_cd = dm_mt_cd;
            datacommondetail.dt_cd = dm_dt_cd;
            datacommondetail.dt_nm = dm_dt_nm;
            datacommondetail.dt_exp = dm_dt_exp;
            datacommondetail.dt_order = dm_dt_order;
            datacommondetail.use_yn = dm_use_yn;
            datacommondetail.reg_dt = DateTime.Now;
            datacommondetail.chg_dt = DateTime.Now;
            datacommondetail.reg_id = Session["authName"] == null ? null : Session["authName"].ToString();
            datacommondetail.chg_id = Session["authName"] == null ? null : Session["authName"].ToString();

            await _IdevManagementService.UpdateComDT(datacommondetail);
            return Json(new { result = true, message = ss }, JsonRequestBehavior.AllowGet);


        }

        [HttpGet]
        public async Task<ActionResult> GetJqGridData()
        {
            var result = await _standardServices.GetListCommonMT();
            return Json(result, JsonRequestBehavior.AllowGet);
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

        public string automtcd2(int id)
        {
            if (id.ToString().Length < 2)
            {
                return string.Format("000{0}", id);
            }

            if ((id.ToString().Length < 3) || (id.ToString().Length == 2))
            {
                return string.Format("00{0}", id);
            }

            if ((id.ToString().Length < 4) || (id.ToString().Length == 3))
            {
                return string.Format("0{0}", id);
            }
            if ((id.ToString().Length < 5) || (id.ToString().Length == 4))
            {
                return string.Format("{0}", id);
            }
            return string.Empty;
        }

        [HttpPost]
        public async Task<ActionResult> insertCommon(CommMt comm_mt, string mt_cd, string mt_nm, string mt_exp, string use_yn, string div_cd)
        {
            var countname = await _standardServices.CountCommMTByName(mt_nm);
            if(countname > 0)
            {
                return Json(new { result = false, message = "Đã tồn tại " + mt_cd + " này trong hệ thống!" }, JsonRequestBehavior.AllowGet);
            }

            int countlist = await _standardServices.CountCommMtByCode();
            var mt_cd_tmp = string.Empty;
            var submtcdconvert = 0;

            var rs = await _standardServices.GetListCommMtByCode();
            var listlast = rs.ToList().LastOrDefault();
            if (countlist == 0)
            {
                mt_cd = div_cd + "COM001";
            }
            else
            {
                var bien = listlast.mt_cd;
                var submtcd = bien.Substring(bien.Length - 3, 3);
                int.TryParse(submtcd, out submtcdconvert);

                mt_cd_tmp = "COM" + string.Format("{0}{1}", mt_cd_tmp, automtcd((submtcdconvert + 1)));
                mt_cd = mt_cd_tmp;
            }

            comm_mt.mt_cd = mt_cd;
            comm_mt.mt_nm = mt_nm;
            comm_mt.mt_exp = mt_exp;
            comm_mt.use_yn = use_yn;
            comm_mt.reg_dt = DateTime.Now;
            comm_mt.chg_dt = DateTime.Now;
            comm_mt.reg_id = Session["authName"] == null ? null : Session["authName"].ToString();
            comm_mt.chg_id = Session["authName"] == null ? null : Session["authName"].ToString();

            var mt_id = await _IdevManagementService.InsertIntoComMT(comm_mt);
            var Comm_Mt = await _IdevManagementService.GetCommMTById(mt_id);
            return Json(new { result = true, message = ss, value = Comm_Mt, mt_cd = Comm_Mt.mt_cd }, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public async Task<ActionResult> updateCommon(int mt_id, string mt_cd, string mt_nm, string mt_exp, string use_yn)
        {
            var data = await _IdevManagementService.GetCommMTById(mt_id);
            if (data == null)
            {
                return Json(new { result = false, message = "Không tìm thấy " + mt_cd + " này trong hệ thống!" }, JsonRequestBehavior.AllowGet);

            }

            data.mt_nm = mt_nm;
            data.mt_exp = mt_exp;
            data.use_yn = use_yn;
            data.reg_dt = DateTime.Now;
            data.chg_dt = DateTime.Now;
            data.reg_id = Session["authName"] == null ? null : Session["authName"].ToString();
            data.chg_id = Session["authName"] == null ? null : Session["authName"].ToString();

            await _IdevManagementService.UpdateComMT(data);
            return Json(new { result = true, message = ss }, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public async Task<JsonResult> deleteCommon(int mt_id)
        {
            var result = await _IdevManagementService.GetCommMTById(mt_id);
            if (result == null)
            {
                return Json(new { result = false, message = "Không tồn tại " + result.mt_cd + " này trong hệ thống !" }, JsonRequestBehavior.AllowGet);
            }

            await _standardServices.RemoveCommMt(result.mt_id);
            var listCommDT = await _standardServices.GetListCommDT(result.mt_cd);
            var rs = listCommDT.ToList();
            if(rs.Count() > 0)
            {
                foreach (var item in rs)
                {
                    await _standardServices.RemoveCommDT(item.cdid);
                }
            }
            return Json(new { result = true, message = ss }, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public async Task<JsonResult> deleteCommon_dt(string commondt_manincode, string commondt_dtcd)
        {
            var data = await _standardServices.GetCommCodeById(commondt_dtcd, commondt_manincode);
            if (data == null)
            {
                return Json(new { result = false, message = "Không tìm thấy " + commondt_manincode + " này ở trong hệ thống" }, JsonRequestBehavior.AllowGet);
            }
            await _IdevManagementService.DeleteComDT(data.mt_cd, data.dt_cd);
            return Json(new { result = true, message = ss }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region MenuMgt
        public ActionResult menuMgt()
        {
            return SetLanguage("");
        }
        //get giữ liệu _cho db menu_info MenuMgt

        [HttpGet]
        public async Task<ActionResult> GetDataMenu(string mn_cd, string mn_nm, string full_nm)
        {
            var result = await _menuServices.GetListMenuInfo(mn_cd,mn_nm,full_nm);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Creat giữ liệu đưa vào bảng menu
        [HttpPost]
        public async Task<ActionResult> insertMenu(MenuInfo WHL, string root_yn, string mn_nm, string url_link, int mnno, string use_yn, string re_mark)
        {
            string user = Session["userid"] == null ? null : Session["userid"].ToString();
            if (string.IsNullOrEmpty(user))
            {
                return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
            }
            var list = await _menuServices.GetListMenuInfo();
            var menuCd = string.Empty;
            var subMenuIdConvert = 0;
            var sublog = new MenuInfo();
            if (root_yn == "1")
            {
                var ds = list.Where(x => x.level_cd == "000").OrderByDescending(item => item.reg_dt).ToList();
                if (ds.Count() == 0)
                {
                    WHL.mn_cd = "001000000000";
                    WHL.level_cd = "000";
                    WHL.up_mn_cd = "001000000000";
                    WHL.mn_full = WHL.mn_nm;
                    WHL.order_no =0;
                }
                else
                {
                    sublog = ds.FirstOrDefault();
                    menuCd = "002";
                    WHL.level_cd = "000";
                    if (sublog != null)
                    {
                        var subMenuId = sublog.mn_cd.Substring(0, 3);
                        int.TryParse(subMenuId, out subMenuIdConvert);
                    }
                    var giatri = CreateId(subMenuIdConvert + 1);
                    menuCd = string.Format("{0}{1}000000000", menuCd, giatri);
                    WHL.mn_cd = menuCd;
                    WHL.up_mn_cd = menuCd;
                    WHL.mn_full = WHL.mn_nm;
                    WHL.order_no = 1;
                }
            }
            else
            {
                var upLevel = list.FirstOrDefault(item => item.mnno == mnno);
                WHL.up_mn_cd = upLevel.mn_cd;

                switch (upLevel.level_cd)
                {
                    case "000":
                        menuCd = upLevel.mn_cd.Substring(0,3);
                        WHL.level_cd = "001";
                        sublog = list.Where(item => item.up_mn_cd == upLevel.mn_cd).OrderByDescending(item => item.reg_dt).FirstOrDefault();
                        if (sublog != null)
                        {
                            var subMenuId = sublog.mn_cd.Substring(3, 3);
                            int.TryParse(subMenuId, out subMenuIdConvert);
                        }
                        var giatri = CreateId(subMenuIdConvert + 1);
                        menuCd = string.Format("{0}{1}000000", menuCd, giatri);
                        WHL.mn_cd = menuCd;
                        WHL.mn_full = upLevel.mn_full + ">" + WHL.mn_nm;
                        break;
                    case "001":
                        menuCd = upLevel.mn_cd.Substring(0, 6);
                        WHL.level_cd = "002";
                        sublog = list.Where(item => item.up_mn_cd == upLevel.mn_cd).OrderByDescending(item => item.reg_dt).FirstOrDefault();
                        if (sublog != null)
                        {
                            var subMenuId = sublog.mn_cd.Substring(6, 3);
                            int.TryParse(subMenuId, out subMenuIdConvert);
                        }
                        giatri = CreateId(subMenuIdConvert + 1);
                        menuCd = string.Format("{0}{1}000", menuCd, giatri);
                        WHL.mn_cd = menuCd;
                        WHL.mn_full = upLevel.mn_full + ">" + WHL.mn_nm;
                        break;
                }
            }
            WHL.mn_nm = mn_nm;
            WHL.reg_dt = DateTime.Now;
            WHL.chg_dt = DateTime.Now;
            WHL.reg_id = Session["authName"] == null ? null : Session["authName"].ToString();
            WHL.chg_id = Session["authName"] == null ? null : Session["authName"].ToString();
            WHL.sub_yn = "N";
            WHL.use_yn = use_yn;
            WHL.url_link = url_link;
            WHL.re_mark = re_mark;

            var id = await _menuServices.InsertIntoMenuInfo(WHL);
            var rs = await _menuServices.GetMenuInfoById(id);
            return Json(new { result =  true, message = ss, value = rs});
        }

        private string CreateId(int id)
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

        [HttpPut]
        public async Task<ActionResult> updateMenuMgt(string mn_nm, int mnno, string url_link, string use_yn, string re_mark)
        {
            string user = Session["userid"] == null ? null : Session["userid"].ToString();
            if (string.IsNullOrEmpty(user))
            {
                return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
            }
            var data = await _menuServices.GetMenuInfoById(mnno);
            if (data == null)
            {
                return Json(new { result = false, message = "Không tồn tại " + mnno + " này trong hệ thống!" }, JsonRequestBehavior.AllowGet);
            }
            data.mn_nm = mn_nm;
            data.url_link = url_link;
            data.use_yn = use_yn;
            data.re_mark = re_mark;
            data.reg_dt = DateTime.Now;
            data.chg_dt = DateTime.Now;
            data.reg_id = Session["authName"] == null ? null : Session["authName"].ToString();
            data.chg_id = Session["authName"] == null ? null : Session["authName"].ToString();

            await _menuServices.UpdateMenuInfo(data);
            return Json(new { result = true, message = ss }, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public async Task<JsonResult> deleteMenu(int mnno)
        {
            var data = await _menuServices.GetMenuInfoById(mnno);
            if (data == null)
            {
                return Json(new { result = false, message = "Không tồn tại " + mnno + " này trong hệ thống!" }, JsonRequestBehavior.AllowGet);
            }

            await _menuServices.RemoveMenuInfo(data.mnno);
            return Json(new { result = true, message = ss }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region AuthorityMgt
        public ActionResult authMgt()
        {
            return SetLanguage("");
        }

        [HttpGet]
        public async Task<JsonResult> getRole()
        {
            var list = await _standardServices.GetRole();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult memberAuthMgt()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetAuthor(string code, string name)
        {
            var result = await _standardServices.GetListAuthorInfo(code, name);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> saveAuthMbInfo(string selRowIdMbJSON)
        {
            string user = Session["userid"] == null ? null : Session["userid"].ToString();
            if (string.IsNullOrEmpty(user))
            {
                return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
            }
            JavaScriptSerializer jsdata = new JavaScriptSerializer();
            var obj = jsdata.Deserialize<dynamic>(selRowIdMbJSON);
            string at_cd = obj["at_cd"];
            string at_nm = obj["at_nm"];
            var lengdata = obj["selRowIdUserArr"].Length;

            //int CountRows = db.mb_author_info.Count();
            var listData = await _standardServices.GetMbAuthorInfoByCode(at_cd);
            int CountRows = listData.Count();

            for (var i = 0; i <= lengdata - 1; i++)
            {
                string userid = obj["selRowIdUserArr"][i];
                for (var j = 1; j <= CountRows; j++)
                {
                    int resultCount = await _standardServices.CountMbAuthorInfo(userid);
                    if (resultCount > 0)
                    {
                        var CountRowsIteam = await _standardServices.GetMbAuthorInfo(userid);
                        await _standardServices.RemoveMbMenuInfo(CountRowsIteam.mano);
                    }
                }
                var mb_info = await _IdevManagementService.GetStaffbyUserId(userid);
                mb_info.grade = await _standardServices.GetNameFromAuthorInfo(at_cd);
            }

            var mb_author_info = new MbAuthorInfo();
            for (var i = 0; i <= lengdata - 1; i++)
            {
                string userid = obj["selRowIdUserArr"][i];
                mb_author_info.at_cd = at_cd;
                mb_author_info.userid = userid;
                mb_author_info.reg_dt = DateTime.Now;
                mb_author_info.chg_dt = DateTime.Now;

                await _IdevManagementService.InsertIntoMbAuthorInfo(mb_author_info);
            };

            return Json(new { result = true, value = lengdata }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetAuMember(string userid, string uname)
        {
            var data = await _standardServices.GetListMbInfo(userid, uname);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> getAuthorMbData(string at_cd)
        {
            try
            {
                List<string> userid = new List<string>();
                var mb_author_info = await _standardServices.GetMbAuthorInfoByCode(at_cd);
                foreach (var item in mb_author_info)
                {
                    userid.Add(item.userid);
                }

                return Json(userid, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetAuthorMenu(string mn_cd, string mn_full, string at_cd)
        {
            var data = await _standardServices.GetListMenuInfo(mn_cd, mn_full, at_cd);
            var SS = Session["userid"].ToString();
            if (SS != "root")
            {
                var authMenuItem = data.FirstOrDefault(x => x.url_link == "/system/menuMgt");
              data.ToList().Remove(authMenuItem);
                //await _standardServices.RemoveMenuInfo(authMenuItem.mnno);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> getAuthorMenuData(string at_cd)
        {
            try
            {
                List<string> mn_cd = new List<string>();
                List<string> role = new List<string>();
                var author_menu_list = await _standardServices.GetListAuthorMenuInfo(at_cd);
                //foreach (var item in author_menu_list)
                //{
                //    mn_cd.Add(item.mn_cd);
                //    role.Add(item.role);
                //}

                return Json(new { result = true,data = author_menu_list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult delAuthMemberInfo(mb_author_info mb_author_info)
        {
            var at_cd_Id = mb_author_info.at_cd;

            var AuthorMemberInfos = db.mb_author_info.Where(item => item.at_cd == at_cd_Id).ToList();
            if (ModelState.IsValid)
            {
                foreach (var item in AuthorMemberInfos)
                {
                    db.Entry(item).State = EntityState.Deleted;
                }
                db.SaveChanges();
            }
            var jsonDataComDt = new
            {
                rows = AuthorMemberInfos
            };
            return Json(jsonDataComDt, JsonRequestBehavior.AllowGet);
        }//End Delete

        public ActionResult saveAuthMemberInfo(author_info author_info, mb_info mb_info, mb_author_info mb_author_info, string userid, string at_cd, string re_mark, string use_yn)
        {
            var userId = mb_info.userid;
            var authorcd = author_info.at_cd;

            mb_author_info.reg_dt = DateTime.Now;
            mb_author_info.chg_dt = DateTime.Now;

            var a = db.mb_author_info.Any(item => ((item.userid == userId) && (item.at_cd == authorcd)));
            if (db.mb_author_info.Any(item => ((item.userid == userId) && (item.at_cd == authorcd))) == false)
            {
                //INSERT INTO mb_author_info (userid, lct_cd)
                var InsertMbLct = (new mb_author_info()
                {
                    userid = mb_info.userid,
                    at_cd = author_info.at_cd,
                    re_mark = author_info.re_mark,
                    use_yn = author_info.use_yn,
                    reg_dt = mb_author_info.reg_dt,
                    chg_dt = mb_author_info.chg_dt,

                });

                if (ModelState.IsValid)
                {
                    db.Entry(InsertMbLct).State = EntityState.Added;
                    db.SaveChanges();
                }
                var jsonDataComDt = new
                {
                    rows = InsertMbLct
                };
                return Json(jsonDataComDt, JsonRequestBehavior.AllowGet);
            }
            else
                return View("GetAuMember");
        }


        //public async Task<ActionResult> saveAuthMenuInfo_addHome()
        //{
        //    string at_cd = Request.QueryString["at_cd"];
        //    string mn_id = Request.QueryString["mn_id"];
        //    string bo_check = Request.QueryString["bo_check"];
        //    var kq = mn_id + "," + "011001001000";
        //    var time = DateTime.Now;
        //    //var dataa = await _standardServices.getmenuinfo(at_cd, kq);
        //    var deleteauthor = await _standardServices.RemoveAuthorMenuInfoWithAtCd(at_cd);
        //    var rs = await _standardServices.InsertIntoAuthorMenuInfo(at_cd, time, kq);

        //    if (bo_check != null && bo_check != "")
        //    {
        //        await _standardServices.RemoveAuthorMenuInfo(bo_check, at_cd);
        //    }
        //    if (rs > 0)
        //    {
        //        return Json(new { result = true, message = ss }, JsonRequestBehavior.AllowGet);
        //    }else
        //    {
        //        return Json(new { result = false, message = exs }, JsonRequestBehavior.AllowGet);
        //    }

        //}
        public class objTr
        {
            public string listMn_cd { get; set; }
            public int at_cd { get; set; }
        }

        [HttpPost]
        public async Task<ActionResult> saveAuthMenuInfo_addHome()
        {
            try
            {
                string user = Session["userid"] == null ? null : Session["userid"].ToString();
                if (string.IsNullOrEmpty(user))
                {
                    return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
                }
                string at_cd = Request.QueryString["at_cd"];
                string mn_id = Request.QueryString["mn_id"];
                string bo_check = Request.QueryString["bo_check"];
                string list_role = Request.QueryString["list_role"];


                var deleteauthor = await _standardServices.RemoveAuthorMenuInfoWithAtCd(at_cd);



                var list_rolePartString = list_role.TrimStart('[').TrimEnd(']').Split(',');
                var list_mn_cdPartString = mn_id.TrimStart('[').TrimEnd(']').Split(',');
                for (int i = 0; i < list_rolePartString.Length; i++)
                {
                    var itemRole = list_rolePartString[i];
                    var itemMn_cd = list_mn_cdPartString[i];
                    var rs = await _standardServices.InsertIntoAuthorMenuInfo(at_cd, DateTime.Now, itemRole, itemMn_cd);
                }

                return Json(new { result = true, message = ss }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new { result = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateAuthor(AuthorInfo authorInfo, string at_cd, string at_nm)
        {
            string user = Session["userid"] == null ? null : Session["userid"].ToString();
            if (string.IsNullOrEmpty(user))
            {
                return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
            }
            var menuCd = string.Empty;
            var subMenuIdConvert = 0;
            var listAuthorInfo = await _IdevManagementService.GetListGrande();
            var data = listAuthorInfo.ToList().LastOrDefault();
            if (data == null)
            {
                authorInfo.at_cd = "001";
            }
            else
            {
                var bien = data.at_cd;
                var subMenuId = bien.Substring(bien.Length - 3, 3);
                int.TryParse(subMenuId, out subMenuIdConvert);
                menuCd = string.Format("{0}{1}", menuCd, CreateId((subMenuIdConvert + 1)));
                authorInfo.at_cd = menuCd;

            }
            authorInfo.at_nm = at_nm;
            authorInfo.reg_dt = DateTime.Now;
            authorInfo.chg_dt = DateTime.Now;
            authorInfo.reg_id = Session["authName"] == null ? null : Session["authName"].ToString();
            authorInfo.chg_id = Session["authName"] == null ? null : Session["authName"].ToString();

            var rs = new AuthorInfo();
            var CountAuth = listAuthorInfo.Count(x => x.at_nm == at_nm);
            if (CountAuth == 0)
            {
                var atno = await _standardServices.InsertIntoAuthorInfo(authorInfo);
                rs = await _standardServices.GetAuthorInfoById(atno);
            }
            return Json(new { result = true, message = ss, value = rs}, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public async Task<ActionResult> EditAuthor(string at_cd, string at_nm, string re_mark, string use_yn,string role)
        {
            string user = Session["userid"] == null ? null : Session["userid"].ToString();
            if (string.IsNullOrEmpty(user))
            {
                return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
            }
            var data = await _standardServices.GetAuthorInfoByAt_Code(at_cd);
            if (data != null)
            {

                data.re_mark = re_mark;
                data.use_yn = use_yn;
                data.at_nm = at_nm;
                data.role = role;
                data.reg_dt = DateTime.Now;
                data.chg_dt = DateTime.Now;
                data.reg_id = Session["authName"] == null ? null : Session["authName"].ToString();
                data.chg_id = Session["authName"] == null ? null : Session["authName"].ToString();
                await _standardServices.UpdateAuthorInfo(data);
                return Json(new { result = true, message = ss, value = data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Không tồn tại " + at_cd + " này trong hệ thống!"}, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpDelete]
        public async Task<JsonResult> deleteAuthor(string at_cd)
        {
            var dataAuthorInfo = await _standardServices.GetAuthorInfoByAt_Code(at_cd);
            if (dataAuthorInfo != null)
            {
                await _standardServices.RemoveAuthorInfo(dataAuthorInfo.atno);
            }

            var authorMenu = await _standardServices.GetListAuthorMenuInfo(at_cd);
            var listAuthorMenuInfo = authorMenu.ToList();
            if (listAuthorMenuInfo.Count() > 0)
            {
                for (var i = 0; i < listAuthorMenuInfo.Count(); i++)
                {
                    await _standardServices.RemoveAuthorMenuInfo(listAuthorMenuInfo[i].amno);
                }
            }

            var authorUser = await _standardServices.GetMbAuthorInfoByCode(at_cd);
            var listDataAuthorInfo = authorUser.ToList();
            if (listDataAuthorInfo.Count() > 0)
            {
                for (var i = 0; i < listDataAuthorInfo.Count(); i++)
                {
                    await _standardServices.RemoveMbAuthorInfo(listDataAuthorInfo[i].mano); ;
                }

            }
            return Json(new { result = true, message = ss}, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region UserAuthorityMgt
        public ActionResult GetMemberInfo()
        {
            var aumember = db.mb_info.ToList();
            var jsonaumember = new
            {

                rows = aumember
            };
            return Json(jsonaumember, JsonRequestBehavior.AllowGet);

        }  //End list Member info

        public ActionResult GetMemberLct(string selected, string userid = "")
        {
            var getLct = string.IsNullOrEmpty(userid) ? null : db.lct_info.ToList();
            var memberLocaltionInfos = db.mb_lct_info.Where(item => item.userid == userid).ToList();
            if (getLct != null)
            {
                foreach (var item in getLct)
                {
                    var c = memberLocaltionInfos.Any(mbItem => mbItem.lct_cd == item.lct_cd);
                    if (c == true)
                    {
                        item.selected = "true";
                    }
                    else
                    {
                        item.selected = "false";
                    }
                }
            }
            var jsonDataLct = new
            {
                rows = getLct
            };

            return Json(jsonDataLct, JsonRequestBehavior.AllowGet);
        }
        //End list2 Member & Location Authority Connection

        public ActionResult delMemberLocationInfo(mb_lct_info mb_lct_info, string userid)
        {
            var userId = mb_lct_info.userid;

            var AuthorMenuInfos = db.mb_lct_info.Where(item => item.userid == userId).ToList();
            if (ModelState.IsValid)
            {
                foreach (var item in AuthorMenuInfos)
                {
                    db.Entry(item).State = EntityState.Deleted;
                }
                db.SaveChanges();
            }
            var jsonDataComDt = new
            {
                rows = AuthorMenuInfos
            };
            return Json(jsonDataComDt, JsonRequestBehavior.AllowGet);
        }
        //End Delete

        public ActionResult saveMemberLocationInfo(mb_info mb_info, lct_info lct_info, mb_lct_info mb_lct_info, int lctno, string userid, string lct_cd, string re_mark, string use_yn)
        {
            var userId = mb_info.userid;
            var localtionCd = lct_info.lct_cd;

            mb_lct_info.reg_dt = DateTime.Now;
            mb_lct_info.chg_dt = DateTime.Now;
            var a = db.mb_lct_info.Any(item => ((item.userid == userId) && (item.lct_cd == localtionCd)));
            //if (db.mb_lct_info.Any(item => ((item.userid == userId) && (item.lct_cd == localtionCd))) == false)
            //{
            //INSERT INTO mb_lct_info (userid, lct_cd)
            var InsertMbLct = (new mb_lct_info()
            {
                userid = mb_info.userid,
                lct_cd = lct_info.lct_cd,
                re_mark = lct_info.re_mark,
                use_yn = lct_info.use_yn,
                reg_dt = mb_lct_info.reg_dt,
                chg_dt = mb_lct_info.chg_dt,

            });

            if (ModelState.IsValid)
            {
                db.Entry(InsertMbLct).State = EntityState.Added;
                db.SaveChanges();
            }
            var jsonDataComDt = new
            {
                rows = InsertMbLct
            };
            return Json(jsonDataComDt, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region location
        public ActionResult locMgt()
        {
            return View();
        }
        public ActionResult Getlocation()
        {

            var vaule = db.lct_info.Where(item => !(item.lct_cd.StartsWith("002")) && !(item.lct_cd.StartsWith("001")) && !(item.lct_cd.StartsWith("004")) && !(item.lct_cd.StartsWith("003")) && !(item.lct_cd.StartsWith("005"))).OrderBy
               (item => item.lct_cd).ToList();
            return Json(new { rows = vaule }, JsonRequestBehavior.AllowGet);

        }



        public ActionResult insertlocMgt(lct_info lct_info, string lct_nm, string mv_yn, string root_yn, string re_mark, int lctno, string use_yn)
        {
            lct_info.lct_nm = lct_nm;
            lct_info.order_no = 1;
            lct_info.mv_yn = mv_yn;
            lct_info.re_mark = re_mark;
            var list = db.lct_info.Where(item => !(item.lct_cd.StartsWith("002")) && !(item.lct_cd.StartsWith("001")) && !(item.lct_cd.StartsWith("004")) && !(item.lct_cd.StartsWith("003")) && !(item.lct_cd.StartsWith("005"))).
               OrderBy(item => item.lct_cd).ThenBy(item => item.level_cd).ToList();
            var upLevel1 = list.FirstOrDefault(item => item.lctno == lctno);
            if (root_yn == "1")
            {
                //if (root_yn == null)
                //{
                //    lct_info.lctno = 1;
                //    lct_info.lct_cd = "001000000000000000";
                //    lct_info.level_cd = "000";
                //    lct_info.up_lct_cd = "000000000000000000";
                //}
                //lct_info.up_lct_cd = "000000000000000000";
                lct_info.level_cd = "000";
                var menuCd1 = string.Empty;
                var subMenuIdConvert = 0;
                var listY = db.lct_info.Where(item => item.level_cd == "000").ToList().OrderByDescending(item => item.lct_cd).FirstOrDefault();

                if (listY != null)
                {
                    var subMenuId = listY.lct_cd.Substring(0, 3);
                    int.TryParse(subMenuId, out subMenuIdConvert);//tra
                    if (subMenuIdConvert == 4 || subMenuIdConvert == 2 || subMenuIdConvert == 1)
                    {
                        subMenuIdConvert = subMenuIdConvert + 1;
                    }
                }


                menuCd1 = string.Format("{0}{1}000000000000000", menuCd1, CreateId((subMenuIdConvert + 1)));
                lct_info.lct_cd = menuCd1;
                lct_info.mn_full = lct_nm;
                //lct_info.index_cd = "F1";
            }

            else
            {
                if (upLevel1 != null)
                {
                    lct_info.up_lct_cd = upLevel1.lct_cd;
                    var menuCd = string.Empty;
                    var subMenuIdConvert = 0;

                    var sublog = new lct_info();
                    switch (upLevel1.level_cd)
                    {
                        case "000":
                            menuCd = upLevel1.lct_cd.Substring(0, 3);
                            lct_info.level_cd = "001";
                            sublog = db.lct_info.ToList().Where(item => item.up_lct_cd == upLevel1.lct_cd).OrderByDescending(item => item.reg_dt).FirstOrDefault();

                            if (sublog != null)
                            {
                                var subMenuId = sublog.lct_cd.Substring(3, 3);
                                int.TryParse(subMenuId, out subMenuIdConvert);
                            }
                            menuCd = string.Format("{0}{1}000000000000", menuCd, CreateId((subMenuIdConvert + 1)));
                            lct_info.lct_cd = menuCd;
                            lct_info.mn_full = upLevel1.mn_full + ">" + lct_info.lct_nm;
                            var x = CreateId(subMenuIdConvert + 1);
                            //lct_info.index_cd = "F" + x.ToString();
                            break;

                        case "001":
                            menuCd = upLevel1.lct_cd.Substring(0, 6);
                            lct_info.level_cd = "002";
                            sublog = db.lct_info.ToList().Where(item => item.up_lct_cd == upLevel1.lct_cd).OrderByDescending(item => item.reg_dt).FirstOrDefault();

                            if (sublog != null)
                            {
                                var subMenuId = sublog.lct_cd.Substring(6, 3);
                                int.TryParse(subMenuId, out subMenuIdConvert);
                            }
                            menuCd = string.Format("{0}{1}000000000", menuCd, CreateId((subMenuIdConvert + 1)));
                            lct_info.lct_cd = menuCd;
                            lct_info.mn_full = upLevel1.mn_full + ">" + lct_info.lct_nm;
                            x = CreateId(subMenuIdConvert + 1);
                            //lct_info.index_cd = "F" + x.ToString();
                            break;
                        case "002":
                            menuCd = upLevel1.lct_cd.Substring(0, 9);
                            lct_info.level_cd = "003";
                            sublog = db.lct_info.ToList().Where(item => item.up_lct_cd == upLevel1.lct_cd).OrderByDescending(item => item.reg_dt).FirstOrDefault();

                            if (sublog != null)
                            {
                                var subMenuId = sublog.lct_cd.Substring(9, 3);
                                int.TryParse(subMenuId, out subMenuIdConvert);
                            }
                            menuCd = string.Format("{0}{1}000000", menuCd, CreateId((subMenuIdConvert + 1)));
                            lct_info.lct_cd = menuCd;
                            lct_info.mn_full = upLevel1.mn_full + ">" + lct_info.lct_nm;
                            x = CreateId(subMenuIdConvert + 1);
                            //lct_info.index_cd = "F" + x.ToString();
                            break;
                        case "003":
                            menuCd = upLevel1.lct_cd.Substring(0, 12);
                            lct_info.level_cd = "004";
                            sublog = db.lct_info.ToList().Where(item => item.up_lct_cd == upLevel1.lct_cd).OrderByDescending(item => item.reg_dt).FirstOrDefault();

                            if (sublog != null)
                            {
                                var subMenuId = sublog.lct_cd.Substring(12, 3);
                                int.TryParse(subMenuId, out subMenuIdConvert);
                            }
                            menuCd = string.Format("{0}{1}000", menuCd, CreateId((subMenuIdConvert + 1)));
                            lct_info.lct_cd = menuCd;
                            lct_info.mn_full = upLevel1.mn_full + ">" + lct_info.lct_nm;
                            x = CreateId(subMenuIdConvert + 1);
                            //lct_info.index_cd = "F" + x.ToString();
                            break;
                        default:
                            menuCd = upLevel1.lct_cd.Substring(0, 15);
                            if (sublog != null)
                            {
                                var subMenuId = sublog.lct_cd.Substring(15, 3);
                                int.TryParse(subMenuId, out subMenuIdConvert);
                            }
                            menuCd = string.Format("{0}{1}000", menuCd, CreateId((subMenuIdConvert + 1)));
                            lct_info.lct_cd = menuCd;
                            lct_info.mn_full = upLevel1.mn_full + ">" + lct_info.lct_nm;
                            x = CreateId(subMenuIdConvert + 1);
                            //lct_info.index_cd = "F" + x.ToString();
                            break;
                    }
                }

            }


            DateTime reg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
            String dateString = reg_dt.ToString("yyyy-MM-dd HH:mm:ss.FFF");

            DateTime chg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
            String dateChString = chg_dt.ToString("yyyy-MM-dd HH:mm:ss.FFF");
            lct_info.reg_dt = reg_dt;
            lct_info.chg_dt = chg_dt;
            lct_info.reg_id = Session["userid"].ToString();
            lct_info.chg_id = Session["userid"].ToString();
            lct_info.use_yn = use_yn;

            if (ModelState.IsValid)
            {
                db.Entry(lct_info).State = EntityState.Added;
                db.SaveChanges();
                return Json(lct_info, JsonRequestBehavior.AllowGet);
            }
            return View();
        }
        public ActionResult Searchloc_mgt(string lct_cd, string lct_nm)
        {
            var list = db.lct_info.Where(item => (item.lct_nm == "" || item.lct_nm.Contains(lct_nm)) && !(item.lct_cd.StartsWith("002")) && !(item.lct_cd.StartsWith("001")) && !(item.lct_cd.StartsWith("004")) && !(item.lct_cd.StartsWith("003")) && !(item.lct_cd.StartsWith("005")) && (lct_cd == "" || item.lct_cd.Contains(lct_cd))).
                  OrderBy(item => item.lct_cd).ThenBy(item => item.level_cd).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        protected string RenderRazorViewToString(string viewName, object model)
        {
            if (model != null)
            {
                ViewData.Model = model;
            }
            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);

                return sw.GetStringBuilder().ToString();
            }
        }


        public ActionResult updatelocMgt(string lct_nm, string mv_yn, string re_mark, int lctno, string use_yn)
        {
            // Search user = userid
            lct_info lct_info = db.lct_info.Find(lctno);
            lct_info.use_yn = use_yn;
            lct_info.mv_yn = mv_yn;
            var name_cu = lct_info.lct_nm.Length;
            var chuoi_full_name = lct_info.mn_full;
            var md = chuoi_full_name.Remove(chuoi_full_name.Length - name_cu);

            var full_name_new = md + lct_nm;
            lct_info.mn_full = full_name_new;
            lct_info.lct_cd = lct_info.lct_cd;
            lct_info.re_mark = re_mark;
            lct_info.lct_nm = lct_nm;

            DateTime chg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
            String dateChString = chg_dt.ToString("yyyy-MM-dd HH:mm:ss.FFF");
            //lct_info.chg_dt = chg_dt;
            lct_info.chg_id = Session["userid"].ToString();
            if (ModelState.IsValid)
            {
                db.Entry(lct_info).State = EntityState.Modified;
                db.SaveChanges();
                return Json(lct_info, JsonRequestBehavior.AllowGet); ;
            }
            return View();
        }
        public ActionResult deletelocMgt(int lctno)
        {
            var item_count = db.lct_info.Count(x => x.lctno == lctno);
            if (item_count != 0)
            {
                var item = db.lct_info.FirstOrDefault(x => x.lctno == lctno);
                db.lct_info.Remove(item);
                db.SaveChanges();
            }
            return Json(new { result = item_count }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region NoticeMgt
        public ActionResult noticeRead(int bno)
        {
            notice_board notice_board = db.notice_board.Find(bno);
            if (notice_board == null)
            {
                return HttpNotFound();
            }
            return View(notice_board);
        }

        public ActionResult noticeInfoData()
        {
            return View();
        }

        public ActionResult Getnotice()
        {

            var list = (from a in db.notice_board
                            //where a.div_cd == "A"
                        select new
                        {
                            bno = a.bno,
                            mn_cd = a.mn_cd,
                            title = a.title,
                            reg_id = a.reg_id,
                            reg_dt = a.reg_dt,
                            chg_dt = a.chg_dt,
                            chg_id = a.chg_id,
                        }).OrderByDescending(x => x.bno).ToList();
            return Json(new { rows = list }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult searchnoticeinfodata()
        {

            // Get Data from ajax function
            string searchType1 = Request.QueryString["searchType1Data"];
            string keywordInput1 = Request.QueryString["keywordInput1Data"];
            string start = Request.QueryString["start1Data"];
            string end = Request.QueryString["end1Data"];
            var html = "";
            if (searchType1 == "title") { html += "AND ('" + keywordInput1 + "'='' OR a.title like '%" + keywordInput1 + "%' )"; }
            if (searchType1 == "content") { html += " AND ('" + keywordInput1 + "'='' OR a.content like '%" + keywordInput1 + "%' )"; }
            if (searchType1 == "create_name") { html += " AND ('" + keywordInput1 + "'='' OR a.reg_id like '%" + keywordInput1 + "%' )"; }
            // Get data

            var sql = new StringBuilder();
            sql.Append(" SELECT * ")
            .Append("FROM  notice_board as a ")
            .Append("Where a.mn_cd is null and ('" + start + "'='' OR DATE_FORMAT(a.reg_dt,'%Y/%m/%d') >= DATE_FORMAT('" + start + "','%Y/%m/%d'))")
            .Append("AND ('" + end + "'='' OR DATE_FORMAT(a.reg_dt,'%Y/%m/%d') <= DATE_FORMAT('" + end + "','%Y/%m/%d'))  " + html + "");
            var data = db.notice_board.SqlQuery(sql.ToString()).ToList<notice_board>().AsEnumerable().Select(x => new
            {
                bno = x.bno,
                title = x.title,
                //content = item.content,
                reg_id = x.reg_id,
                reg_dt = Convert.ToString(x.reg_dt),
                chg_id = x.chg_id,
                chg_dt = Convert.ToString(x.chg_dt),
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult noticeRegister()
        {
            return View();
        }

        public ActionResult noticeModify(int bno)
        {
            notice_board notice_board = db.notice_board.Find(bno);
            if (notice_board == null)
            {
                return HttpNotFound();
            }
            return View(notice_board);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult updatenotice(int bno, string content, string title)
        {
            notice_board notice_board = db.notice_board.Find(bno);
            notice_board.title = title;
            notice_board.content = content;
            DateTime chg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
            String dateChString = chg_dt.ToString("yyyy-MM-dd HH:mm:ss.FFF");
            //notice_board.chg_dt = chg_dt;
            if (ModelState.IsValid)
            {
                db.Entry(notice_board).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("noticeInfoData");
            }
            return View(notice_board);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult insertnotice(notice_board notice_board)
        {
            if (ModelState.IsValid)
            {
                notice_board.chg_dt = DateTime.Now;
                notice_board.reg_dt = DateTime.Now;
                notice_board.reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                notice_board.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                notice_board.div_cd = "A";

                db.notice_board.Add(notice_board);
                db.SaveChanges();
                return RedirectToAction("noticeInfoData");
            }

            return View(notice_board);
        }

        #endregion

        #region Manual_Management
        public ActionResult Manual()
        {
            return SetLanguage("");
        }

        [HttpGet]
        public async Task<ActionResult> Getnotice_menu()
        {
            var data = await _standardServices.GetListNoticeBoard();
            var list = data.Select(a => new
            {
                bno = a.bno,
                mn_cd = a.mn_cd,
                lng_cd = ((a.lng_cd == "EN") ? "English" : "Vietnamese"),
                title = a.title,
                reg_id = a.reg_id,
                reg_dt = a.reg_dt,
                chg_dt = a.chg_dt,
                chg_id = a.chg_id,
            }).ToList();

            return Json(new { rows = list }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ManualCreate()
        {
            return View();
        }


        [HttpGet]
        [ValidateInput(false)]
        public async Task<ActionResult> check_exist_manual(string mn_cd, string lng_cd)
        {
            var data = await _standardServices.CheckDataNoticeBoard(mn_cd, lng_cd);
            return Json(new { result = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> insertnotice_manu(NoticeBoard notice_board)
        {

            notice_board.div_cd = "B";
            notice_board.chg_dt = DateTime.Now;
            notice_board.reg_dt = DateTime.Now;
            notice_board.chg_id = Session["userid"].ToString();
            notice_board.reg_id = Session["userid"].ToString();
            var rs = await _standardServices.InsertIntoNoticeBoard(notice_board);
            return RedirectToAction("Manual");
        }


        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> updatemanu(NoticeBoard notice_board)
        {
            var item_count = await _standardServices.GetListDataNoticeBoard(notice_board.mn_cd, notice_board.lng_cd);
            if (item_count.Count() > 0)
            {
                var item = item_count.FirstOrDefault();

                item.title = notice_board.title;
                item.mn_cd = notice_board.mn_cd;
                item.content = notice_board.content;
                DateTime chg_dt = DateTime.Now;
                item.chg_dt = DateTime.Now;
                item.chg_id = Session["userid"].ToString();
                await _standardServices.UpdateNoticeBoard(item);
                return RedirectToAction("Manual");
            }
            else
            {
                notice_board.div_cd = "B";
                notice_board.reg_dt = DateTime.Now;
                notice_board.reg_id = Session["userid"].ToString();
                await _standardServices.InsertIntoNoticeBoard(notice_board);
                return RedirectToAction("Manual");
            }

        }

        [HttpGet]
        public async Task <ActionResult> Searchmanu(string title, string mn_cd, string lng_cd)
        {
            if (lng_cd == "All")
            {
                var data = await _standardServices.SearchDataNoticeBoard1(title, mn_cd);
                var rs = data.Select(a => new
                {
                    bno = a.bno,
                    lng_cd = ((a.lng_cd == "EN") ? "English" : "Vietnamese"),
                    mn_cd = a.mn_cd,
                    title = a.title,
                    reg_id = a.reg_id,
                    reg_dt = a.reg_dt,
                    chg_dt = a.chg_dt,
                    chg_id = a.chg_id,
                }).ToList();
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = await _standardServices.SearchDataNoticeBoard2(title, mn_cd, lng_cd);
                var rs = data.Select(a => new
                {
                    bno = a.bno,
                    lng_cd = ((a.lng_cd == "EN") ? "English" : "Vietnamese"),
                    mn_cd = a.mn_cd,
                    title = a.title,
                    reg_id = a.reg_id,
                    reg_dt = a.reg_dt,
                    chg_dt = a.chg_dt,
                    chg_id = a.chg_id,
                }).ToList();
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> viewdetaillangue(int bno, string lng_cd)
        {
            var a = await _standardServices.GetNoticeBoardById(bno);
            var notice_board = await _standardServices.GetListDataNoticeBoard(a.mn_cd, lng_cd);
            var result = notice_board.ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> viewdetailnotice(int bno)
        {
            var notice_board = await _standardServices.GetNoticeBoardById(bno);
            return Json(new { mn_cd = notice_board.mn_cd, lng_cd = notice_board.lng_cd, title = notice_board.title, content = notice_board.content }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> viewdetail_notice(int bno)
        {
            var notice_board = await _standardServices.GetNoticeBoardById(bno);
            return Json(notice_board, JsonRequestBehavior.AllowGet);
        }

        public ActionResult viewdetailmanual(string menu_nm, string lang_cd)
        {
            notice_board notice_board = new notice_board();
            var mn_cd = (db.menu_info.FirstOrDefault(x => x.mn_full == menu_nm) == null) ? "" : db.menu_info.FirstOrDefault(x => x.mn_full == menu_nm).mn_cd;
            notice_board = db.notice_board.FirstOrDefault(item => item.mn_cd == mn_cd && ((item.mn_cd != null) || (item.mn_cd != "")) && (item.lng_cd == lang_cd));
            return Json(notice_board, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public async Task<ActionResult> viewdetaildetailmanu(string mn_cd)
        {
            var notice_board = await _standardServices.GetListNoticeBoardByMenuCode(mn_cd);
            return Json(notice_board, JsonRequestBehavior.AllowGet);
        }


        [HttpDelete]
        public async Task<JsonResult> deleteManual(int bno)
        {
            var count_item = await _standardServices.GetNoticeBoardById(bno);
            if (count_item != null)
            {
                await _standardServices.DeleteNoticeBoard(bno);
            }

            return Json(new { result = count_item }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult updatemanu2(int bno, string content, string title)
        {
            notice_board notice_board = db.notice_board.Find(bno);
            notice_board.title = title;
            notice_board.content = content;
            DateTime chg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
            String dateChString = chg_dt.ToString("yyyy-MM-dd HH:mm:ss.FFF");
            //notice_board.chg_dt = chg_dt;
            if (ModelState.IsValid)
            {
                db.Entry(notice_board).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("noticeInfoData");
            }
            return View(notice_board);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public int keywordInput1 { get; set; }
    }
}