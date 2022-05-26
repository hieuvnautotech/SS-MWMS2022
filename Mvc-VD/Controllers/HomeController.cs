using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_VD.Models;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Net.Http;
using System.Xml;
using Mvc_VD.Classes;
using System.Security.Cryptography;
//using MySql.Data.MySqlClient;
using Mvc_VD.Services;
using MySql.Data.MySqlClient;
using Mvc_VD.Models.HomeModel;
using Mvc_VD.Models.HomeModel.Request;
using System.Web.Http;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;
using AutoMapper;
using Mvc_VD.Models.HomeModel.Response;
using Mvc_VD.Services.Interface;
using System.Threading.Tasks;
using Mvc_VD.Models.Response;
using HttpGetAttribute = System.Web.Mvc.HttpGetAttribute;
using Mvc_VD.Commons;

namespace Mvc_VD.Controllers
{
    public class HomeController : Controller
    {
        private Entities db = new Entities();
        private string KeyEncrypt = "AutoCompany@2021!@#$%^&*()_+";
        private readonly IUserServices _userServices;
        private readonly IhomeService _homeService;
        protected  override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            GetSetCookieLanguage();
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
        public HttpCookie GetSetCookieLanguage()
        {
            HttpCookie languageCookie = null;
            if (HttpContext.Request.Cookies.Get("language") == null)
            {
                languageCookie = new HttpCookie("language");
                DateTime now = DateTime.Now;
                languageCookie.Value = Commons.language.en;
                languageCookie.Expires = now.AddDays(1);
                languageCookie.Secure = false;
                Response.Cookies.Add(languageCookie);
            }
            else
            {
                languageCookie = HttpContext.Request.Cookies.Get("language");
                Response.Cookies.Add(languageCookie);
            }
            return languageCookie;
        }
        #region DashBoard
        public HomeController(IUserServices userServices,  IhomeService homeService) //IHomeService ihomeService, IHomeService ihome,IHomeService ihome,
        {
            _userServices = userServices;
            _homeService = homeService;
        }
        public ActionResult DashBoard()
        {
            HttpCookie cookie = HttpContext.Request.Cookies["language"];
            if (cookie != null)
            {
                ViewBag.language = cookie.Value;
            }
            return View();
        }

        public async Task<ActionResult> getnoticeMgt()
        {
            var resqheader =Request.Headers;
            String[] resqheaderkey = resqheader.AllKeys;
            String[] resqheaderval = resqheader.GetValues("requestFrom");
            if (Session["authorize"] != null || resqheaderval[0]== "Mob")
            {
            
                var noticeboard = await _userServices.GetNoticeBoard();
                var list = (from a in noticeboard
                            where a.div_cd == "A"
                            select new
                            {
                                bno = a.bno,
                                mn_cd = a.mn_cd,
                                title = a.title,
                                reg_id = a.reg_id,
                                reg_dt = a.reg_dt,
                                chg_dt = a.chg_dt,
                                chg_id = a.chg_id,
                            }).OrderByDescending(x => x.bno).Take(10).ToList();
                return Json(new { rows = list }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var noticeboard = new notice_board();
                return Json(new { result = false, message = Constant.ErrorAuthorize, rows= noticeboard }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> getMessageAll(mb_message mbm)
        {
            var resqheader =Request.Headers;String[] resqheaderkey = resqheader.AllKeys;String[] resqheaderval = resqheader.GetValues("requestFrom");if (Session["authorize"] != null || resqheaderval[0]== "Mob")
            {
            
            var datares = await _userServices.GetMBMessage();
            DateTime parsed;
            var data = datares.Select(x => new
            {
                tid = x.tid,
                message = x.message,
                reg_id = x.reg_id ?? "admin",
                reg_dt = DateTime.TryParse(x.reg_dt.ToString(), out parsed) ? parsed.ToString("yyyy-MM-dd HH:mm:ss") : " ",
            }).OrderByDescending(x => x.reg_dt).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = new mb_message();
                return Json(new { result = false, message = Constant.ErrorAuthorize, data }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult insertMessage(mb_message mbm, string message)
        {
            mbm.message = message;

            DateTime reg_dt = DateTime.Now;
            mbm.reg_dt = reg_dt;
            mbm.reg_id = Session["userid"].ToString();
            mbm.del_yn = "N";

            if (ModelState.IsValid)
            {
                db.mb_message.Add(mbm);
                db.SaveChanges();
                DateTime parsed;
                var data = db.mb_message.ToList().Select(x => new
                {
                    message = x.message,
                    reg_id = x.reg_id ?? "Admin",
                    reg_dt = DateTime.TryParse(x.reg_dt.ToString(), out parsed) ? parsed.ToString("yyyy-MM-dd HH:mm:ss") : " ",
                }).OrderByDescending(x => x.reg_dt).ToList();

                //var data2 = data.Skip(data.Count() - 10).Take(10);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return View(mbm);
        }

        public ActionResult getNotice(int bno)
        {
            var data = db.notice_board.FirstOrDefault(x => x.bno == bno);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //-------------------------------

        #endregion DashBoard

        #region Session

        public async Task<ActionResult> Index()
        {
            if (HttpContext.Request.Cookies["user"] != null && HttpContext.Request.Cookies["password"] != null)
            {
                try
                {
                    string cookieUser = HttpContext.Request.Cookies.Get("user").Value;
                    string cookiePassword = HttpContext.Request.Cookies.Get("password").Value;
                    await Login(cookieUser, cookiePassword, "on");
                }
                catch (Exception ex)
                {
                }
            }
            return View();
        }

        public ActionResult Registry()
        {
            return View();
        }

        public ActionResult ResultRegistry()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registry([System.Web.Http.FromBody] string userid, [System.Web.Http.FromBody] string password, [System.Web.Http.FromBody] string password2, [System.Web.Http.FromBody] string uname, [System.Web.Http.FromBody] string cel_nb, [System.Web.Http.FromBody] string e_mail)
        {
            ViewBag.Message = "Registry";
            mb_info model_user = new mb_info();
            var result = db.mb_info.Count(x => x.userid == userid);

            if (password == password2)
            {
                if ((result == 0) && (userid != "") && (password != ""))
                {
                    model_user.userid = userid;
                    model_user.uname = uname;
                    model_user.nick_name = "";
                    model_user.upw = password;
                    model_user.grade = "";
                    model_user.tel_nb = "";
                    model_user.cel_nb = cel_nb;
                    model_user.e_mail = e_mail;
                    model_user.sms_yn = "";
                    model_user.mail_yn = "";
                    model_user.join_ip = "";
                    model_user.join_domain = "";
                    model_user.ltacc_dt = DateTime.Now;
                    model_user.ltacc_domain = "";
                    model_user.mbout_dt = DateTime.Now;
                    model_user.mbout_yn = "";
                    model_user.accblock_yn = "";
                    model_user.session_key = "";
                    model_user.session_limit = DateTime.Now;
                    model_user.memo = "";
                    model_user.del_yn = "";
                    model_user.check_yn = "";
                    model_user.rem_me = "";
                    model_user.mbjoin_dt = DateTime.Now;
                    model_user.scr_yn = "";
                    model_user.log_ip = "";
                    model_user.lct_cd = "";
                    model_user.reg_id = "";
                    model_user.reg_dt = DateTime.Now;
                    model_user.chg_id = "";
                    model_user.chg_dt = DateTime.Now;
                    model_user.re_mark = "";
                    db.mb_info.Add(model_user);
                    db.SaveChanges();
                    ViewBag.Result = "Registry is success";
                }
                else
                {
                    ViewBag.Result = "Registry is fail";
                }
            }
            else
            {
                ViewBag.Result = "Registry is fail : password unmatch";
            }

            return View("ResultRegistry");
        }

        [HttpPost]
        public async Task<ActionResult> Login([System.Web.Http.FromBody] string userid, [System.Web.Http.FromBody] string password, [System.Web.Http.FromBody] string remmemberme)
        {
            
            string check_userid = userid;
            string check_password = password;
            string check_remmemberme = remmemberme;

            string checkroot_userid = "";
            string checkroot_password = "";
            if(string.IsNullOrEmpty(check_userid) || string.IsNullOrEmpty(check_password))
            {
                return RedirectToAction("Index", "Home");
            }
            if ((checkroot_userid == userid) && (checkroot_password == password))
            {
                #region Remember Password
                if (remmemberme == "on")
                {

                    //Xóa Cookie Lưu Thông Tin User cũ
                    HttpContext.Response.Cookies.Remove("user");
                    //Tạo Cookie Lưu Thông Tin mới
                    HttpCookie cookie = new HttpCookie("user");
                    cookie.Value = userid;
                    cookie.Expires = DateTime.Now.AddYears(1);
                    //Set Cookie Lưu thông Tin User
                    HttpContext.Response.SetCookie(cookie);
                    //Xóa Cookie Lưu Thông Tin User cũ
                    HttpContext.Response.Cookies.Remove("password");
                    //Tạo Cookie Lưu Thông Tin mới
                    cookie = new HttpCookie("password");
                    cookie.Value = password;
                    cookie.Expires = DateTime.Now.AddYears(1);
                    //Set Cookie Lưu thông Tin User
                    HttpContext.Response.SetCookie(cookie);
                }
                #endregion
                var MenuList = db.menu_info.Where(x => x.use_yn == "Y").OrderBy(x => x.mn_cd).ToList();
                Session["authorize"] = "logined";
                Session["userid"] = checkroot_userid;
                Session["authName"] = "Root";
                Session["authList"] = MenuList;
                Session["author_cha"] = MenuList;
                Session["role"] = "";
                Session["authData"] = "";
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                var result = await _userServices.CheckLoginUser(check_userid, check_password);
                if (!string.IsNullOrEmpty(result))
                {
                   
                    //duy close
                    /*int authDataCount = db.mb_author_info.Count(x => x.userid == check_userid);
                    if (authDataCount > 0)
                    {
                        var list = db.mb_author_info.Where(x => x.userid == check_userid).FirstOrDefault();
                        string authData = list.at_cd;*/
                    var authData = await _userServices.GetAuthData(result);
                    GetSetCookieUserLogin(authData);
                    if (!string.IsNullOrEmpty(authData))
                    {
                        //if (db.author_info.Where(x => x.at_cd == authData).FirstOrDefault().role == "RL001")
                        var buyerLogin = await _userServices.GetRoleFromAuthData(authData);
                        if (buyerLogin != null && buyerLogin.role == "RL001")
                        {
                            Session["role"] = "RL001";
                        }
                        else
                        {
                            Session["role"] = "";
                        }
                      
                        var listMenu = await _userServices.GetListMenuByAuthData(authData, GetSetCookieLanguage().Value);
                        var check_wms = listMenu.Where(x => x.mn_cd == "012000000000" || x.mn_cd == "002000000000" || x.mn_cd == "009000000000" || x.mn_cd == "010000000000");
                        Session["wms"] = string.Join(",", check_wms.Select(x => x.mn_cd));
                        Session["authorize"] = "logined";
                        Session["userid"] = check_userid;
                        Session["authName"] = buyerLogin.at_nm;
                        Session["authList"] = listMenu.ToList();
                        Session["author_cha"] = listMenu.ToList();
                        Session["authData"] = authData;
                        if (authData == "015")
                        {
                            return RedirectToAction("Supplier_QR_Management", "Supplier");
                        }
                        //Session.Timeout = 500000;
                    }
                    return RedirectToAction("Dashboard", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            //if ((checkroot_userid == userid) && (checkroot_password == password))
            //{
            //    #region Remember Password
                
            //    #endregion
            //    var MenuList = await _userServices.getListMenuInfo();
            //    Session["authorize"] = "logined";
            //    Session["userid"] = checkroot_userid;
            //    Session["authName"] = "Root";
            //    Session["authList"] = MenuList;
            //    Session["author_cha"] = MenuList;
            //    Session["role"] = "";
            //    return RedirectToAction("Dashboard", "Home");
            //}
            //else
            //{
            //    //duy close 
            //    /*var result = db.mb_info.Count(x => x.userid == check_userid && x.upw == check_password);
            //    if (result > 0)
            //    {*/
                
            //}
        }

        public ActionResult checkFirstLogin()
        {
            var loginFirst = new { loginFirst = Session["loginFirst"] };
            if (Session["loginFirst"] == null)
            {
                Session["loginFirst"] = "N";
            }
            return Json(loginFirst, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult Logout()
        //{
        //    HttpCookie userInfo = new HttpCookie("userInfo");
        //    Session.Clear();
        //    if (HttpContext.Request.Cookies["user"] != null && HttpContext.Request.Cookies["password"] != null)
        //    {
        //        Session.Clear();
        //        Response.Cookies["user"].Expires = DateTime.Now.AddDays(-1);
        //        //Response.Cookies["userid"].Expires = DateTime.Now.AddDays(-1);
        //        Response.Cookies["password"].Expires = DateTime.Now.AddDays(-1);
        //    }
        //    //userInfo.Expires = DateTime.Now.AddHours(1);
        //    //Response.Cookies.Add(userInfo);
        //    //return RedirectToAction("Index", "Home");

        //    userInfo.Expires = DateTime.Now.AddHours(1);
        //    Response.Cookies.Add(userInfo);
        //    HttpContext.Request.Cookies.Clear();
        //    return RedirectToAction("Index", "Home");
        //}
        public ActionResult Logout()
        {
            HttpCookie userInfo = new HttpCookie("userInfo");
            if (HttpContext.Request.Cookies["user"] != null && HttpContext.Request.Cookies["password"] != null)
            {
                Response.Cookies["user"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["password"].Expires = DateTime.Now.AddDays(-1);
            }
            userInfo.Expires = DateTime.Now.AddHours(1);
            Response.Cookies.Add(userInfo);
            HttpContext.Request.Cookies.Clear();
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> GetNumNote(string mn_cd)
        {
            try
            {
                //note for M-WMS
                
                if (mn_cd == "013")
                {
                    var value1 = await _homeService.GetListSdInfo(2);

                    if (value1 > 0)
                    {
                        return Json(new { result = true, data = value1 }, JsonRequestBehavior.AllowGet);
                    }
                }
                //note for WIP
                if (mn_cd == "010")
                {
                    var value2 = await _homeService.GetListSdInfo(1);

                    if (value2 > 0)
                    {
                        return Json(new { result = true, data = value2 }, JsonRequestBehavior.AllowGet);
                    }
                }
                //note for FG
                //if (mn_cd == "009")
                //{
                //    var abc = db.w_ext_info.Count(x => x.alert == 1);

                //    if (abc > 0)
                //    {
                //        return Json(new { result = true, data = abc }, JsonRequestBehavior.AllowGet);
                //    }
                //}
                ////note for TIMS
                //if (mn_cd == "012")
                //{
                //    var abc = db.w_ext_info.Count(x => x.alert == 2);

                //    if (abc > 0)
                //    {
                //        return Json(new { result = true, data = abc }, JsonRequestBehavior.AllowGet);
                //    }
                //}
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetNumNote_detail(string mn_cd)
        {
            try
            {

                var resqheader =Request.Headers;String[] resqheaderkey = resqheader.AllKeys;String[] resqheaderval = resqheader.GetValues("requestFrom");if (Session["authorize"] != null || resqheaderval[0]== "Mob")
                {
                
                    //note for WIP
                    if (mn_cd == "010")
                    {
                        var note = await _homeService.Notification1(1);
                        var data = note.AsEnumerable().Select(w => new 
                        {
                            id = w.sid,
                            code = w.sd_no,
                            name = w.sd_nm,
                            link = "/wipwms/Receving_Scan_Wip",
                            chg_dt = w.chg_dt
                        }).OrderByDescending(x => x.chg_dt).ToList();

                        if (data.Count > 0)
                        {
                            return Json(new { result = true, data }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    //note for WMS
                    if (mn_cd == "013")
                    {
                        var note = await _homeService.Notification1(2);
                        var data = note.AsEnumerable().Select(w => new
                        {
                            id = w.sid,
                            code = w.sd_no,
                            name = w.sd_nm,
                            link = "/ShippingMgt/ShippingPickingScan"

                        }).ToList();

                        if (data.Count > 0)
                        {
                            return Json(new { result = true, data }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    //note for FG
                    if (mn_cd == "009")
                    {
                        var note = await _homeService.Notification2(1);
                        var data = note.AsEnumerable().Select(w => new
                        {
                            id = w.extid,
                            code = w.ext_no,
                            name = w.ext_nm,
                            link = "/fgwms/Receving_Scan"

                        }).ToList();

                        if (data.Count > 0)
                        {
                            return Json(new { result = true, data }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    //note for TIMS
                    if (mn_cd == "012")
                    {
                        var note = await _homeService.Notification2(2);
                        var data = note.AsEnumerable().Select(w => new
                        {
                            id = w.extid,
                            code = w.ext_no,
                            name = w.ext_nm,
                            link = "/TIMS/Shipping_Scan"

                        }).ToList();

                        if (data.Count > 0)
                        {
                            return Json(new { result = true, data }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    return Json(new { result = false }, JsonRequestBehavior.AllowGet);
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

        #endregion Session

        #region Menu

        //public ActionResult GetSessionMenuData()
        //{
        //    try
        //    {
        //        if (Session["check_root"].ToString() == "ok")
        //        {
        //            var SessionData1 = new { userid = Session["userid"], authName = Session["authName"], authList = Session["authList"], authList_cha = Session["authList"], ds_author = Session["authList"], ds_KQ = 0 };
        //            return Json(SessionData1, JsonRequestBehavior.AllowGet);
        //        }

        //        var url = Request["url"];
        //        String varname1 = "";
        //        varname1 = varname1 + "SELECT c.* " + "\n";
        //        varname1 = varname1 + "FROM   author_menu_info AS a " + "\n";
        //        varname1 = varname1 + "       JOIN mb_author_info AS b " + "\n";
        //        varname1 = varname1 + "    on a.at_cd=b.at_cd and b.userid='" + Session["userid"] + "' " + "\n";
        //        varname1 = varname1 + "       join author_info as c on b.at_cd=c.at_cd  ";
        //        varname1 = varname1 + "       WHERE a.url_link = '" + url + "'";
        //        var data = db.Database.SqlQuery<author_info>(varname1).ToList();
        //        if (data.Count() == 1)
        //        {
        //            var role = data.FirstOrDefault().role;
        //            var at_cd = data.FirstOrDefault().at_cd;
        //            String varname2 = "";
        //            varname2 = varname2 + "select b.mn_cd,b.mn_nm,b.up_mn_cd,b.url_link from author_menu_info as a " + "\n";
        //            varname2 = varname2 + "join menu_info as b on concat(substring(a.mn_cd,1,3),'000000000') = b.mn_cd " + "\n";
        //            varname2 = varname2 + "where a.at_cd='"+ at_cd + "' " + "\n";
        //            varname2 = varname2 + "group by  substring(a.mn_cd,1,3) " + "\n";
        //            varname2 = varname2 + "union " + "\n";
        //            varname2 = varname2 + "select b.mn_cd,b.mn_nm,b.up_mn_cd,b.url_link from author_menu_info as a " + "\n";
        //            varname2 = varname2 + "join menu_info as b on concat(substring(a.mn_cd,1,6),'000000') = b.mn_cd " + "\n";
        //            varname2 = varname2 + "where a.at_cd='"+ at_cd + "' " + "\n";
        //            varname2 = varname2 + "group by b.mn_cd";
        //            var data1 = db.Database.SqlQuery<menu>(varname2).ToList();
        //            if (role == "RL001")
        //            {
        //                var ds_author = db.author_action.Where(x => x.url_link == url).ToList();
        //                var SessionData = new { userid = Session["userid"], authName = Session["authName"], authList = Session["authList"], authList_cha = data1, ds_author = ds_author };
        //                return Json(SessionData, JsonRequestBehavior.AllowGet);
        //            }
        //            var SessionData1 = new { userid = Session["userid"], authName = Session["authName"], authList = Session["authList"], authList_cha = data1,ds_author = 0, ds_KQ = 0 };
        //            return Json(SessionData1, JsonRequestBehavior.AllowGet);
        //        }

        //        return RedirectToAction("Index", "Home");
        //    }
        //    catch (Exception e)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //}
        public async Task<ActionResult> GetSessionMenuData()
        {
            try
            {
                var result = await _userServices.GetListMenuByAuthData(GetSetCookieUserLogin("").Value, GetSetCookieLanguage().Value);
                Session["author_cha"] = result;
                Session["authlist"] = result;
             var authData = "" ;

                if (Session["authData"] != null)
                {
                    authData= Session["authData"].ToString();
                }
          
                var url = Request["url"];

          
                string[] words;
                string urlnew = "";
       
                if (url.Contains("/"))
                {
                    words = url.Split(new string[] { "/" }, StringSplitOptions.None);
                    for (int i = 0; i < words.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(words[i]))
                        {
                           string start = words[i].Replace("/", "");
                            urlnew += start;
                        }
                      
                    }
                }
             

                if (Session["authName"] == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                var bien = (Session["authName"].ToString() == "Root" ? "true" : Session["wms"]);
                var check_role = await _userServices.GetListAuthorMenuInfo(authData, urlnew);
                if (check_role == "RL001")
                {
                    var ds_role = await _userServices.GetListAuthorAction(urlnew);

                    var SessionData = new { userid = Session["userid"], authName = Session["authName"], authList = Session["authList"], authList_cha = Session["authList"], ds_author = Session["authList"], ds_role = ds_role, wms = bien };
                    return Json(SessionData, JsonRequestBehavior.AllowGet);
                }

                var SessionData1 = new { userid = Session["userid"], authName = Session["authName"], authList = Session["authList"], authList_cha = Session["authList"], ds_author = Session["authList"], ds_role = 0, wms = bien };
                return Json(SessionData1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public HttpCookie GetSetCookieUserLogin(string usercode)
        {
            HttpCookie UserCookie = null;
            if (string.IsNullOrEmpty(usercode) == false)
            {
                if (HttpContext.Request.Cookies.AllKeys.Contains("usercode"))
                {
                    HttpContext.Request.Cookies.Remove("usercode");
                }
                UserCookie = new HttpCookie("usercode");
                DateTime now = DateTime.Now;
                UserCookie.Value = usercode;
                UserCookie.Expires = now.AddDays(1);
                UserCookie.HttpOnly = false;


                Response.Cookies.Add(UserCookie);
            }
            else
            {
                UserCookie = HttpContext.Request.Cookies.Get("usercode");
                //Response.Cookies.Add(UserCookie);
            }
            return UserCookie;
        }

        #endregion Menu

        #region ApplicationLogin
        [HttpPost]
        public async Task<ActionResult> ApplicationLogin([FromBody] LoginRequest request)
        {
            var result = await _homeService.ApplicationLogin(request);
            if (result > 0)
            {
                Session["authorize"] = request.userName;
                Session["userid"] = request.userName;
                //return Json(new BaseResponse(true,"Đăng nhập thành công"), JsonRequestBehavior.AllowGet);
                return Json(new { result=true,message= "Đăng nhập thành công",Session= Session["authorize"] }, JsonRequestBehavior.AllowGet);
            }
            else     
            {
                return Json(new BaseResponse(false,"Đăng nhập thất bại, vui lòng kiểm tra lại tài khoản và mật khẩu"), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion ApplicationLogin

        #region CheckVersionApp
        [HttpPost]
        public async Task<ActionResult> CheckVersionApp([FromBody] CheckVersionRequest request)
        {

            bool isResult = false;
            string message = "";
            bool isNewVersion = false;
            var latestVersion= await _homeService.GetLatestVersionApp(request.appName);
            if (latestVersion != null)
            {
                isResult = true;
                if (latestVersion.version > request.versionCode)
                {
                    isNewVersion = true;
                    message = "Đang tồn tại phiên bản mới, vui lòng cập nhật để tiếp tục!";
                }
            }
            else
            {
                message = "App không tồn tại trên máy chủ";
            }
            var jsonResponse = new
            {
                result = isResult,
                newVersion = isNewVersion,
                message= message,
                data = new AppVersionResponse(latestVersion.id_app, latestVersion.type, latestVersion.name_file, latestVersion.version, latestVersion.chg_dt.ToString())
            };
            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add_Version(int phienban, string name_file, string name_app, version_app version_app)
        {
            var get_ds_new = db.version_app.Where(x => x.type == name_app).OrderBy(x => x.version).ToList();
            if (get_ds_new.Count < 2)
            {
                version_app.version = phienban;
                version_app.type = name_app;
                db.Entry(version_app).State = EntityState.Added;
                db.SaveChanges();
            }
            else
            {
                //xóa cái đầu tiên 
                var xoa = get_ds_new.FirstOrDefault();
                db.Entry(xoa).State = EntityState.Deleted;
                db.SaveChanges();


                //insert cái tiếp theo
                version_app.version = phienban;
                version_app.type = name_app;
                db.Entry(version_app).State = EntityState.Added;
                db.SaveChanges();
            }
            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }



        #endregion
        
        #region EncryptData

        /// <summary>
        /// Hàm mã hóa chuỗi với key
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Encrypt(string source, string key)
        {
            using (TripleDESCryptoServiceProvider tripleDESCryptoService = new TripleDESCryptoServiceProvider())
            {
                using (MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider())
                {
                    byte[] byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
                    tripleDESCryptoService.Key = byteHash;
                    tripleDESCryptoService.Mode = CipherMode.ECB;
                    byte[] data = Encoding.UTF8.GetBytes(source);
                    return Convert.ToBase64String(tripleDESCryptoService.CreateEncryptor().TransformFinalBlock(data, 0, data.Length));
                }
            }
        }
        /// <summary>
        /// Hàm giải mã chuỗi với key
        /// </summary>
        /// <param name="encrypt"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Decrypt(string encrypt, string key)
        {
            using (TripleDESCryptoServiceProvider tripleDESCryptoService = new TripleDESCryptoServiceProvider())
            {
                using (MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider())
                {
                    byte[] byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
                    tripleDESCryptoService.Key = byteHash;
                    tripleDESCryptoService.Mode = CipherMode.ECB;
                    byte[] data = Convert.FromBase64String(encrypt);
                    byte[] test = tripleDESCryptoService.CreateDecryptor().TransformFinalBlock(data, 0, data.Length);
                    return Encoding.UTF8.GetString(test);
                }
            }
        }
        public string Base64(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/').PadRight(4 * ((s.Length + 3) / 4), '=');
            return s;
        }
        #endregion
    }
}