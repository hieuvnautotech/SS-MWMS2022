using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_VD.Models;
using System.Collections;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Mvc_VD.Services.Interface.QMS;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Services.Interface;
using Mvc_VD.Commons;

namespace Mvc_VD.Controllers
{
    public class QMSController : BaseController
    {
        string exs="Lỗi hệ thống!!!";
        //
        // GET: /QMS/
        private Entities db = new Entities();
        private IQMSService _IQMSService;
        private readonly IhomeService _homeService;
        public QMSController(IQMSService IQMSService,IhomeService ihomeService)
        {
            _homeService = ihomeService;
            _IQMSService = IQMSService;
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
        #region QCMaster


        public ActionResult QCMaster()
        {
            return SetLanguage("");
        }

        [HttpGet]
        public async Task<ActionResult> Getqc_item()
        {
            var list = await _IQMSService.GetListItemMaterial();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> get_item_type()
        {
            var type = await _IQMSService.GetListItemType("COM009");
            return Json(type, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> get_defect()
        {
            var type = await _IQMSService.GetListDefect("COM026");
            return Json(type, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Createqc_item(qc_item_mt QIM, string item_type, string item_exp, string item_nm, string use_yn)
        {
            var list = db.qc_item_mt.Where(x => x.item_type == item_type).OrderBy(x => x.item_cd).ToList();

            if (list.Count == 0)
            {
                QIM.item_cd = item_type + "000001";
                QIM.ver = "A";
                QIM.item_vcd = QIM.item_cd + QIM.ver;
            }
            else
            {
                var menuCd = string.Empty;
                var subMenuIdConvert = 0;
                var list1 = list.LastOrDefault();
                var bien1 = list1.item_cd;
                var subMenuId = new Excute_query().Right(bien1, 6);


                //bien1.Substring(bien1.Length - 5, 5);
                int.TryParse(subMenuId, out subMenuIdConvert);
                menuCd = item_type + string.Format("{0}{1}", menuCd, CreatemT((subMenuIdConvert + 1)));
                QIM.item_cd = menuCd;
                QIM.ver = "A";
                QIM.item_vcd = QIM.item_cd + QIM.ver;
            }
            QIM.del_yn = "N";
            QIM.reg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
            QIM.chg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
            QIM.item_nm = item_nm;
            QIM.use_yn = use_yn;
            QIM.item_type = item_type;
            QIM.item_exp = item_exp;
            DateTime reg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
            String dateString = reg_dt.ToString("yyyy-MM-dd HH:mm:ss.FFF");

            DateTime chg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
            String dateChString = chg_dt.ToString("yyyy-MM-dd HH:mm:ss.FFF");
            QIM.reg_dt = reg_dt;
            QIM.chg_dt = chg_dt;

            if (ModelState.IsValid)
            {
                db.Entry(QIM).State = EntityState.Added;
                db.SaveChanges();
                return Json(QIM, JsonRequestBehavior.AllowGet); ;
            }
            return View(QIM);
        }
        private string CreatemT(int id)
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
        private string CreatId(int id)
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
        public ActionResult Modifyqc_item(string item_exp, string item_nm, string use_yn, int ino)
        {
            qc_item_mt QIM = db.qc_item_mt.Find(ino);
            QIM.item_exp = item_exp;
            QIM.use_yn = use_yn;
            QIM.item_nm = item_nm;
            //QIM.chg_dt = DateTime.Now;
            QIM.del_yn = "N";
            if (ModelState.IsValid) //check what type of extension
            {
                db.Entry(QIM).State = EntityState.Modified;
                db.SaveChanges(); // line that threw exception
                return Json(QIM, JsonRequestBehavior.AllowGet);
            }
            return View(QIM);
        }

        [HttpGet]
        public async Task<ActionResult> searchqc_item(string item_type, string item_cd, string item_nm, string item_exp)
        {
            //var list = db.qc_item_mt.Where(x => (x.del_yn == "N") && (item_nm == "" || x.item_nm.Contains(item_nm)) && (item_exp == "" || x.item_exp.Contains(item_exp)) && (item_cd == "" || x.item_vcd.Contains(item_cd)) && (item_type == "" || x.item_type.Contains(item_type))).ToList();
            var list = await _IQMSService.SearchQCItemMaterial(item_type, item_cd, item_nm, item_exp);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult insertver_vf(qc_item_mt QIM, qc_itemcheck_mt QCMT, qc_itemcheck_dt QCDT, string item_cd, string item_type, string item_nm, string item_exp, string use_yn, string ver1)
        {
            DateTime reg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
            String dateString = reg_dt.ToString("yyyy-MM-dd HH:mm:ss.FFF");

            DateTime chg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
            String dateChString = chg_dt.ToString("yyyy-MM-dd HH:mm:ss.FFF");

            string[] listtring = { "0", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            var list = db.qc_item_mt.Where(x => x.item_cd == item_cd).ToList().LastOrDefault();
            var ver = "";
            if (list.ver != "Z")
            {
                for (int i = 0; i < listtring.Length; i++)
                {
                    var m = listtring[i];
                    if (m == list.ver)
                    {
                        i++;
                        ver = listtring[i];
                        break;
                    }
                }
            }

            //insert have qim
            if (list != null)
            {
                QIM.item_cd = item_cd;
                QIM.item_type = item_type;
                QIM.item_nm = item_nm;
                QIM.item_exp = item_exp;
                QIM.reg_dt = reg_dt;
                QIM.chg_dt = chg_dt;
                QIM.use_yn = use_yn;
                QIM.del_yn = "N";
                QIM.item_vcd = item_cd + ver;
                QIM.ver = ver;
                if (ModelState.IsValid) //check what type of extension
                {
                    db.Entry(QIM).State = EntityState.Added;
                    db.SaveChanges();
                }
            }
            //insert have QCMT
            if (list != null)
            {
                var bien = item_cd + ver1;
                var list2 = db.qc_itemcheck_mt.Where(x => x.item_vcd == bien).ToList();
                foreach (var item in list2)
                {
                    QCMT.check_id = item_cd + ver + item.check_id.Substring(item.check_id.Length - 3, 3);
                    QCMT.item_vcd = item_cd + ver;
                    QCMT.check_type = item.check_type;
                    QCMT.check_subject = item.check_subject;
                    QCMT.order_no = item.order_no;
                    QCMT.re_mark = item.re_mark;
                    QCMT.use_yn = item.use_yn;
                    QCMT.del_yn = "N";
                    QCMT.reg_dt = reg_dt;
                    QCMT.chg_dt = chg_dt;
                    db.Entry(QCMT).State = EntityState.Added;
                    db.SaveChanges();
                    var c_id = item.check_id;
                    var list3 = db.qc_itemcheck_dt.Where(x => x.item_vcd == bien && x.check_id == c_id).ToList();
                    foreach (var item1 in list3)
                    {
                        QCDT.item_vcd = item_cd + ver;
                        QCDT.check_id = item_cd + ver + item.check_id.Substring(item.check_id.Length - 3, 3);
                        QCDT.check_cd = item1.check_cd;
                        QCDT.check_name = item1.check_name;
                        QCDT.order_no = item1.order_no;
                        QCDT.re_mark = item1.re_mark;
                        QCDT.use_yn = item1.use_yn;
                        QCDT.del_yn = "N";
                        QCDT.defect_yn = item1.defect_yn;
                        QCDT.reg_dt = reg_dt;
                        QCDT.chg_dt = chg_dt;
                        db.Entry(QCDT).State = EntityState.Added;
                        db.SaveChanges();
                    }
                }
                return Json(QIM, JsonRequestBehavior.AllowGet);
            }
            //UPDATE CŨ
            return View(QIM);

        }
        public JsonResult deleteitem_3table(int id)
        {
            int count = db.qc_item_mt.Where(x => x.ino == id).ToList().Count();
            var result = new { result = true };
            if (count > 0)
            {
                qc_item_mt qc_item_mt = db.qc_item_mt.Find(id);

                var ds = db.qc_itemcheck_mt.Where(x => x.item_vcd == qc_item_mt.item_vcd).ToList();
                var ds1 = db.qc_itemcheck_dt.Where(x => x.item_vcd == qc_item_mt.item_vcd).ToList();
                foreach (var item in ds)
                {
                    qc_itemcheck_mt qc_itemcheck_mt = db.qc_itemcheck_mt.Find(item.icno);
                    qc_itemcheck_mt.del_yn = "Y";
                    db.SaveChanges();
                }
                foreach (var item in ds1)
                {
                    qc_itemcheck_dt qc_itemcheck_dt = db.qc_itemcheck_dt.Find(item.icdno);
                    qc_itemcheck_dt.del_yn = "Y";
                    db.SaveChanges();
                }
                qc_item_mt.del_yn = "Y";
                db.SaveChanges();
                return Json(new { result, message = "Thành công !!!" }, JsonRequestBehavior.AllowGet);
            }
            result = new { result = false };
            return Json(new { result, message = exs }, JsonRequestBehavior.AllowGet);

        }

        #region wc_item_check_mt

        [HttpGet]
        public async Task<ActionResult> get_check_type()
        {
            var type = await _IQMSService.GetListItemType("COM010");
            return Json(type, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> get_range_type()
        {
            var type = await _IQMSService.GetListItemType("COM024");
            return Json(type, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult>searchqc_pp(string item_cd, string item_nm, string item_exp)
        {
            var list = await _IQMSService.SearchQCItemMaterial(item_cd, item_nm, item_exp);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Getqc_check_mt(string item_vcd)
        {
            if (item_vcd != null)
            {
                var result = await _IQMSService.GetQCCheckMaterial(item_vcd);
                var list = result.ToList();
                //var list = (from a in db.qc_itemcheck_mt
                //            where a.item_vcd == item_vcd && a.del_yn == "N"
                //            select new
                //            {
                //                icno = a.icno,
                //                item_vcd = a.item_vcd,
                //                item_cd = a.item_vcd.Substring(0, 7),
                //                ver = a.item_vcd.Substring(a.item_vcd.Length - 1, 1),
                //                check_id = a.check_id,
                //                check_type = a.check_type,
                //                check_subject = a.check_subject,
                //                order_no = a.order_no,
                //                range_type_nm = (a.range_type != null ? db.comm_dt.Where(x => x.mt_cd == "COM024" && x.dt_cd == a.range_type).FirstOrDefault().dt_nm : ""),
                //                range_type = a.range_type,
                //                re_mark = a.re_mark,
                //                min_value = a.min_value,
                //                max_value = a.max_value,
                //                use_yn = a.use_yn,
                //                reg_id = a.reg_id,
                //                reg_dt = a.reg_dt,
                //                chg_id = a.chg_id,
                //                chg_dt = a.chg_dt,
                //            }).ToList();
                var jsonDataComDt = new
                {
                    rows = list
                };
                return Json(jsonDataComDt, JsonRequestBehavior.AllowGet);
            }
            return View("QCMaster");
        }

        [HttpPost]
        public async Task<ActionResult> Createcheck_mt(QCItemCheckMaterial ICM, QCItemCheckDetail ICD, string item_cd, string check_type, string check_subject, int order_no, string re_mark, string ver, string use_yn, decimal min_value, decimal max_value, string range_type)
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
                    var result = await _IQMSService.CheckQCItemCheckMaterial(item_cd, (item_cd + ver));
                    var list = result.ToList();
                    if (list.Count() == 0)
                    {
                        ICM.check_id = item_cd + ver + "001";
                    }
                    else
                    {
                        var menuCd = string.Empty;

                        var subMenuIdConvert = 0;
                        var list1 = list.LastOrDefault();

                        var bien1 = list1.check_id;
                        //var subMenuId = bien1.Substring(bien1.Length - 3, 3);
                        var subMenuId = new Excute_query().Right(bien1, 3);
                        int.TryParse(subMenuId, out subMenuIdConvert);
                        menuCd = list1.item_vcd + string.Format("{0}{1}", menuCd, CreatId((subMenuIdConvert + 1)));
                        ICM.check_id = menuCd;
                    }

                    ICM.reg_dt = DateTime.Now;
                    ICM.chg_dt = DateTime.Now;
                    ICM.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    ICM.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    ICM.del_yn = "N";
                    ICM.item_vcd = item_cd + ver;
                    ICM.check_type = check_type;
                    ICM.check_subject = check_subject;
                    ICM.order_no = order_no;
                    ICM.re_mark = re_mark;
                    ICM.min_value = min_value;
                    ICM.max_value = max_value;
                    ICM.range_type = range_type;
                    if (ModelState.IsValid)
                    {
                        int icno = await _IQMSService.InsertIntoQCItemCheckMaterial(ICM);

                        if (ICM.check_type == "range")
                        {
                            // var comdt_range = db.comm_dt.Where(x => x.mt_cd == "COM023" && x.use_yn == "Y").ToList();
                            var listDataCommDetail = await _IQMSService.GetListItemType("COM023");
                            var comdt_range = listDataCommDetail.ToList();
                            foreach (var item in comdt_range)
                            {
                                //var list_con = db.qc_itemcheck_dt.Where(m => m.check_id == ICM.check_id).ToList();
                                var listDataQCCheckItemDetail = await _IQMSService.CheckQCItemCheckDetail(ICM.check_id);
                                var list_con = listDataQCCheckItemDetail.ToList();
                                if (list_con.Count == 0)
                                {
                                    ICD.check_cd = "001";
                                }
                                else
                                {
                                    var menuCd = string.Empty;
                                    var subMenuIdConvert = 0;
                                    var list1 = list_con.LastOrDefault();

                                    var bien1 = list1.check_cd;
                                    var subMenuId = bien1.Substring(bien1.Length - 3, 3);
                                    int.TryParse(subMenuId, out subMenuIdConvert);
                                    menuCd = string.Format("{0}{1}", menuCd, CreatId((subMenuIdConvert + 1)));
                                    ICD.check_cd = menuCd;
                                }
                                ICD.reg_dt = DateTime.Now;
                                ICD.chg_dt = DateTime.Now;
                                ICM.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                ICM.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                ICD.del_yn = "N";
                                ICD.check_id = ICM.check_id;
                                ICD.defect_yn = "Y";
                                ICD.check_name = item.dt_nm;
                                ICD.order_no = order_no;
                                ICD.re_mark = re_mark;
                                ICD.use_yn = use_yn;
                                ICD.item_vcd = ICM.check_id.Substring(0, 8);
                                if (ModelState.IsValid)
                                {
                                    int idcno = await _IQMSService.InsertIntoQCItemCheckDetail(ICD);
                                }
                            }
                        }


                        var rs = await _IQMSService.GetQCCheckMaterialByIcNo(icno);
                        var kq = rs.FirstOrDefault();

                        return Json(new { result = true,data = kq, message = Constant.Success }, JsonRequestBehavior.AllowGet);

                    }
                    return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
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

        [HttpPut]
        public async Task<ActionResult> Modifycheck_mt(int icno, string check_type, string check_subject, int order_no, string re_mark, string use_yn, decimal min_value, decimal max_value, string range_type)
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
                    //qc_itemcheck_mt ICM = db.qc_itemcheck_mt.Find(icno);
                    var ICM = await _IQMSService.GetQCItemCheckMaterialById(icno);

                    ICM.check_type = check_type;
                    ICM.check_subject = check_subject;
                    ICM.order_no = order_no;
                    ICM.re_mark = re_mark;
                    ICM.use_yn = use_yn;
                    ICM.min_value = min_value;
                    ICM.max_value = max_value;
                    ICM.range_type = range_type;
                    ICM.reg_dt = DateTime.Now;
                    ICM.chg_dt = DateTime.Now;
                    ICM.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    ICM.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    ICM.del_yn = "N";

                    await _IQMSService.UpdateQCItemCheckMaterial(ICM);
                    var list = await _IQMSService.GetQCCheckMaterialByIcNo(icno);
                    var kq = list.FirstOrDefault();
                    return Json(new { result = true, data = kq, message = Constant.Success }, JsonRequestBehavior.AllowGet);
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

        [HttpDelete]
        public async Task<JsonResult> deleteitem_2table(int id)
        {
            //int count = db.qc_itemcheck_mt.Where(x => x.icno == id).ToList().Count();

            var QCItemCheckMaterial = await _IQMSService.GetQCItemCheckMaterialById(id);

            if (QCItemCheckMaterial != null)
            {
                //QCItemCheckMaterial qc_itemcheck_mt = db.qc_itemcheck_mt.Find(id);

                //var ds1 = db.qc_itemcheck_dt.Where(x => x.item_vcd == QCItemCheckMaterial.item_vcd && x.check_id== QCItemCheckMaterial.check_id).ToList();
                var ds = await _IQMSService.CheckQCItemCheckDetail(QCItemCheckMaterial.item_vcd, QCItemCheckMaterial.check_id);
                var ds1 = ds.ToList();
                foreach (var item in ds1)
                {
                    await _IQMSService.UpdateQCItemCheckDetailForDel_Yn(item.icdno);
                }
                await _IQMSService.UpdateQCItemCheckMaterialForDel_Yn(QCItemCheckMaterial.icno);
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);

        }
        #endregion


        #region qc_itemcheck_dt

        [HttpGet]
        public async Task<ActionResult> Getqc_check_dt(string item_vcd, string check_id)
        {
            if (item_vcd != null)
            {

                //var list = (from a in db.qc_itemcheck_dt
                //            where a.item_vcd == item_vcd && a.check_id == check_id && a.del_yn == "N"
                //            select new
                //            {
                //                icdno = a.icdno,
                //                item_vcd = a.item_vcd,
                //                check_id = a.check_id,
                //                check_cd = a.check_cd,
                //                check_name = a.check_name,
                //                order_no = a.order_no,
                //                re_mark = a.re_mark,
                //                use_yn = a.use_yn,
                //                reg_id = a.reg_id,
                //                reg_dt = a.reg_dt,
                //                chg_id = a.chg_id,
                //                chg_dt = a.chg_dt,
                //                defect_yn = a.defect_yn,
                //            }).ToList();

                var result = await _IQMSService.CheckQCItemCheckDetail(item_vcd, check_id);
                var list = result.ToList();
                var jsonDataComDt = new
                {
                    rows = list
                };
                return Json(jsonDataComDt, JsonRequestBehavior.AllowGet);
            }
            return View("QCMaster");
        }

        [HttpPost]
        public async Task<ActionResult> Createcheck_dt(QCItemCheckDetail ICD, string check_id, string check_name, int order_no, string re_mark,string ver, string use_yn, string item_vcd, string defect_yn)
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
                //var list = db.qc_itemcheck_dt.Where(item => item.check_id == check_id).ToList();
                var rs = await _IQMSService.CheckQCItemCheckDetail(check_id);
                var list = rs.ToList();
                if (list.Count() == 0)
                {
                    ICD.check_cd = "001";
                }
                else
                {
                    var menuCd = string.Empty;
                    var subMenuIdConvert = 0;
                    var list1 = list.LastOrDefault();
                    var bien1 = list1.check_cd;
                    var subMenuId = bien1.Substring(bien1.Length - 3, 3);
                    int.TryParse(subMenuId, out subMenuIdConvert);
                    menuCd = string.Format("{0}{1}", menuCd, CreatId((subMenuIdConvert + 1)));
                    ICD.check_cd = menuCd;
                }

                ICD.reg_dt = DateTime.Now;
                ICD.chg_dt = DateTime.Now;
                ICD.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                ICD.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                ICD.del_yn = "N";
                ICD.check_id = check_id;
                ICD.defect_yn = defect_yn;
                ICD.check_name = check_name;
                ICD.order_no = order_no;
                ICD.re_mark = re_mark;
                ICD.use_yn = use_yn;
                ICD.item_vcd = item_vcd + ver;

                var icdno = await _IQMSService.InsertIntoQCItemCheckDetail(ICD);
                var result = await _IQMSService.GetQCItemCheckDetailById(icdno);
                return Json(new { result = true , data = result, message = "Tạo thành công"}, JsonRequestBehavior.AllowGet);
                //return View(ICD);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Modifycheck_dt(int icdno, string check_cd, string check_name, int order_no, string re_mark, string use_yn, string defect_yn)
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
                //qc_itemcheck_dt ICM = db.qc_itemcheck_dt.Find(icdno);
                    var ICM = await _IQMSService.GetQCItemCheckDetailById(icdno);
                ICM.check_cd = check_cd;
                ICM.check_name = check_name;
                ICM.order_no = order_no;
                ICM.re_mark = re_mark;
                ICM.use_yn = use_yn;
                ICM.chg_dt = DateTime.Now;
                ICM.reg_dt = DateTime.Now;
                ICM.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                ICM.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                ICM.del_yn = "N";
                ICM.defect_yn = defect_yn;

                await _IQMSService.UpdateQCItemCheckDetail(ICM);
                var result = await _IQMSService.GetQCItemCheckDetailById(ICM.icdno);
                return Json(new { result = true , data = ICM }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> Delete_qc_itemcheck_dt(int id)
        {

            //var count = db.qc_itemcheck_dt.Where(x => x.icdno == id).Count();
            var QcItemcheckDetail = await _IQMSService.GetQCItemCheckDetailById(id);
            if (QcItemcheckDetail != null)
            {
                await _IQMSService.UpdateQCItemCheckDetailForDel_Yn(QcItemcheckDetail.icdno);
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region QC List
        public ActionResult QClist()
        {
            return View(db.m_facline_qc.ToList());
        }


        public ActionResult getqclist()
        {


            var sql = new StringBuilder();
            sql.Append(" SELECT a.fqno,a.fq_no,a.oldhno,a.fo_no,a.po_no,a.line_no,a.process_no,DATE_FORMAT(a.work_dt,'%Y-%m-%d') as work_dt,a.item_vcd,a.item_nm,a.item_exp,a.item_exp,a.check_qty,a.reg_id,a.reg_dt,a.chg_id,a.chg_dt")
           .Append(" FROM m_facline_qc   as a ");

            //.Append(" group by a.work_dt ");


            var data = new DataTable();
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                db.Database.Connection.Open();
                cmd.CommandText = sql.ToString();
                using (var reader = cmd.ExecuteReader())
                {
                    data.Load(reader);
                }
            }
            db.Database.Connection.Close();
            var result = GetJsonPersons(data);
            return result;

        }


        public ActionResult getdetail(string fq_no)
        {

            var list = (from a in db.m_facline_qc_value

                        join b in db.qc_item_mt
                        on a.item_vcd equals b.item_vcd
                        join d in db.qc_itemcheck_dt
                        on a.item_vcd equals d.item_vcd
                        join e in db.qc_itemcheck_mt
                       on a.item_vcd equals e.item_vcd
                        where a.fq_no == fq_no


                        select new
                        {
                            fqhno = a.fqhno,
                            fq_no = a.fq_no,
                            item_vcd = a.item_vcd,
                            check_id = d.check_name,
                            check_cd = d.check_cd,
                            check_value = d.order_no,
                            check_qty = a.check_qty,


                        }).ToList();

            var Data = new { rows = list };
            return Json(Data, JsonRequestBehavior.AllowGet);

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

        public JsonResult GetJsonPersons(DataTable data)
        {
            var lstPersons = GetTableRows(data);
            return Json(new { rows = lstPersons }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetJsonPersons_QClist_search(DataTable data)
        {
            var lstPersons = GetTableRows(data);
            return Json(lstPersons, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getPopupwO(m_facline_qc m_facline_qc)
        {

            var vaule = db.m_facline_qc.ToList();
            var Data = new { rows = vaule };
            return Json(Data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getPopupLine(m_facline_qc m_facline_qc)
        {

            var vaule = db.m_facline_qc.ToList();
            var Data = new { rows = vaule };
            return Json(Data, JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /QClist/Details/5
        public JsonResult searchQClist()
        {


            var fo_no = Request["fo_no"]; //mpo_no
            var line_no = Request["line_no"];
            var process_no = Request["process_no"];
            var end = Request.QueryString["end"];



            var dateConvert = new DateTime();
            if (DateTime.TryParse(end, out dateConvert))
            {
                end = dateConvert.ToString("yyyy-MM-dd");
            }


            var sql = new StringBuilder();

            sql.Append(" SELECT a.fqno,a.fq_no,a.oldhno,a.fo_no,a.po_no,a.line_no,a.process_no,DATE_FORMAT(a.work_dt,'%Y-%m-%d') as work_dt,a.item_vcd,a.item_nm,a.item_exp,a.item_exp,a.check_qty,a.reg_id,a.reg_dt,a.chg_id,a.chg_dt")
           .Append(" FROM m_facline_qc   as a ")
           .Append(" Where('" + fo_no + "'='' OR a.fo_no like '%" + fo_no + "%' )")
           .Append("AND ('" + line_no + "'='' OR a.line_no like '%" + line_no + "%' )")
           .Append("AND ('" + process_no + "'='' OR a.process_no like '%" + process_no + "%' )")
           .Append("AND ('" + end + "'='' OR CONVERT(a.work_dt,DATE)=CONVERT('" + end + "',DATE))");
            var data = new DataTable();
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                db.Database.Connection.Open();
                cmd.CommandText = sql.ToString();
                using (var reader = cmd.ExecuteReader())
                {
                    data.Load(reader);
                }


            }
            db.Database.Connection.Close();
            var result = GetJsonPersons_QClist_search(data);
            return result;
        }

        #endregion

        #region NG_OK(Hang)
        public ActionResult View_Ng()
        {
            return View();
        }
        public ActionResult PartialView_NG()
        {
            var month = (Request["month"] != null ? Request["month"] : "0");
            var product = (Request["product"] != null ? Request["product"] : "");
            var po_no = (Request["po_no"] != null ? Request["po_no"] : "");
            var type_kind = (Request["type_kind"] != null ? Request["type_kind"] : "");
            var value_code = (Request["value_code"] != null ? Request["value_code"] : "");
            var value_nm = (Request["value_nm"] != null ? Request["value_nm"] : "");
            var start = (Request["start"] != null ? Request["start"] : "");
            var end = (Request["end"] != null ? Request["end"] : "");
            var mc_type = ((Request["mc_type"] != null && Request["mc_type"] != "undefined") ? Request["mc_type"] : "");
            var time_now = DateTime.Now.AddMonths(int.Parse(month));

            var startOfMonth = new DateTime(time_now.Year, time_now.Month, 1);
            var first = startOfMonth.AddMonths(1);
            var last = first.AddDays(-1);

            var date_start = startOfMonth.ToString("yyyyMMdd");
            var date_end = last.ToString("yyyyMMdd");

            String varname1 = "";
            String varname2 = "";
            String varname3 = "";
            if (type_kind == "mc")
            {
                var List = new ArrayList();
                ViewBag.List = List;
                ViewBag.name = "Machine";
                varname1 = varname1 + "select (a.mc_nm),(a.mc_no),('Machine')kind, " + "\n";
                var count_day = (last - startOfMonth).TotalDays;
                for (int i = 0; i < (count_day + 1); i++)
                {
                    var bien = startOfMonth.AddDays(i);
                    var bien_doi = bien.ToString("dd");
                    List.Add(bien_doi);
                    var text = bien.ToString("yyyyMMdd");
                    varname1 = varname1 + "(case when " + "\n";
                    varname1 = varname1 + "	 (select count(k.pmid) from d_pro_unit_mc as k " + "\n";
                    varname1 = varname1 + "			where '" + text + "' between (Date_format(k.start_dt,'%Y%m%d')) and (Date_format(k.end_dt,'%Y%m%d')) and k.mc_no=a.mc_no)>0 " + "\n";
                    varname1 = varname1 + "		 then 	'bg-warning' else 'bg-white' end)day_" + (i + 1) + " , " + "\n";
                    varname1 = varname1 + "(	SELECT Group_concat(DISTINCT (Concat('PO', Cast(Substr(m.at_no, 3, 11)AS UNSIGNED), ' ' , m.name, ' Start: ', Concat(Substring(n.start_dt, 1, 4), '-', " + "\n";
                    varname1 = varname1 + "       Substring(n.start_dt, 5, 2), '-', Substring(n.start_dt, 7, 2), ' ', Substring(n.start_dt, 9, 2), ':', Substring(n.start_dt, 11, 2), ':', " + "\n";
                    varname1 = varname1 + "       Substring(n.start_dt, 13, 2)),  ' End: ', Concat(Substring(n.end_dt, 1, 4), '-',  Substring(n.end_dt, 5, 2), '-', " + "\n";
                    varname1 = varname1 + "       Substring(n.end_dt, 7, 2), ' ', Substring(n.end_dt, 9, 2), ':',  Substring(n.end_dt, 11, 2), ':', Substring(n.end_dt, 13, 2)) ) )  SEPARATOR ',') " + "\n";
                    varname1 = varname1 + "       cv " + "\n";
                    varname1 = varname1 + "FROM   d_pro_unit_mc AS n " + "\n";
                    varname1 = varname1 + "       JOIN w_actual AS m " + "\n";
                    varname1 = varname1 + "         ON n.id_actual = m.id_actual " + "\n";
                    varname1 = varname1 + "WHERE  n.mc_no = a.mc_no " + "\n";
                    varname1 = varname1 + "       AND '" + text + "' BETWEEN ( Date_format(n.start_dt, '%Y%m%d') ) AND ( " + "\n";
                    varname1 = varname1 + "                              Date_format(n.end_dt, '%Y%m%d') ) ) po" + (i + 1) + ",";
                }
                varname1 = varname1 + " (a.mno) id " + "\n";
                varname1 = varname1 + "from d_machine_info as a " + "\n";
                //where
                varname1 = varname1 + " Where ('" + value_code + "'='' OR  a.mc_no  like '%" + value_code + "%' )";
                varname1 = varname1 + " and ('" + value_nm + "'='' OR  a.mc_nm  like '%" + value_nm + "%' )";
                if (po_no != "" || product != "")
                {
                    varname1 = varname1 + " and a.mc_no in (select v.mc_no from d_pro_unit_mc as v ";
                    varname1 = varname1 + " join w_actual as n on v.id_actual=n.id_actual ";
                    varname1 = varname1 + " join w_actual_primary as l on n.at_no =l.at_no ";
                    varname1 = varname1 + " Where ('" + po_no + "'='' OR  n.at_no  like '%" + po_no + "%' )";
                    varname1 = varname1 + " and ('" + product + "'='' OR l.product  like '%" + product + "%' ))";
                }
                varname1 = varname1 + " and ('" + mc_type + "'='' OR  a.mc_type  like '%" + mc_type + "%' ) group by a.mc_no";

            }
            if (type_kind == "wk")
            {
                var List = new ArrayList();
                ViewBag.List = List;
                ViewBag.name = "Worker";
                varname2 = varname2 + "select (a.uname) mc_nm, " + "\n";
                varname2 = varname2 + " (a.userid) mc_no,('Worker')kind," + "\n";
                var count_day = (last - startOfMonth).TotalDays;
                for (int i = 0; i < (count_day + 1); i++)
                {
                    var bien = startOfMonth.AddDays(i);
                    var bien_doi = bien.ToString("dd");
                    List.Add(bien_doi);
                    var text = bien.ToString("yyyyMMdd");
                    varname2 = varname2 + "(case when " + "\n";
                    varname2 = varname2 + "	 (select count(k.psid) from d_pro_unit_staff as k " + "\n";
                    varname2 = varname2 + "			where '" + text + "' between (Date_format(k.start_dt,'%Y%m%d')) and (Date_format(k.end_dt,'%Y%m%d')) and  k.staff_id=a.userid)>0 " + "\n";
                    varname2 = varname2 + "		 then 	'bg-warning' else 'bg-white' end)day_" + (i + 1) + " , " + "\n";
                    varname2 = varname2 + "(	SELECT Group_concat(DISTINCT (Concat('PO', Cast(Substr(m.at_no, 3, 11)AS UNSIGNED), ' ' , m.name, ' Start: ', Concat(Substring(n.start_dt, 1, 4), '-', " + "\n";
                    varname2 = varname2 + "       Substring(n.start_dt, 5, 2), '-', Substring(n.start_dt, 7, 2), ' ', Substring(n.start_dt, 9, 2), ':', Substring(n.start_dt, 11, 2), ':', " + "\n";
                    varname2 = varname2 + "       Substring(n.start_dt, 13, 2)),  ' End: ', Concat(Substring(n.end_dt, 1, 4), '-',  Substring(n.end_dt, 5, 2), '-', " + "\n";
                    varname2 = varname2 + "       Substring(n.end_dt, 7, 2), ' ', Substring(n.end_dt, 9, 2), ':',  Substring(n.end_dt, 11, 2), ':', Substring(n.end_dt, 13, 2)) ) )  SEPARATOR ',') " + "\n";
                    varname2 = varname2 + "       cv " + "\n";
                    varname2 = varname2 + "FROM   d_pro_unit_staff AS n " + "\n";
                    varname2 = varname2 + "       JOIN w_actual AS m " + "\n";
                    varname2 = varname2 + "         ON n.id_actual = m.id_actual " + "\n";
                    varname2 = varname2 + "WHERE  n.staff_id = a.userid " + "\n";
                    varname2 = varname2 + "       AND '" + text + "' BETWEEN ( Date_format(n.start_dt, '%Y%m%d') ) AND ( " + "\n";
                    varname2 = varname2 + "                              Date_format(n.end_dt, '%Y%m%d') ) ) po" + (i + 1) + ",";
                }
                varname2 = varname2 + " (a.userid) id " + "\n";
                varname2 = varname2 + "from mb_info as a " + "\n";
                varname2 = varname2 + " Where a.lct_cd='staff' and ('" + value_code + "'='' OR  a.userid  like '%" + value_code + "%' )";
                varname2 = varname2 + " and ('" + value_nm + "'='' OR  a.uname  like '%" + value_nm + "%' ) ";
                if (po_no != "" || product != "")
                {
                    varname2 = varname2 + " and a.userid in (select v.staff_id from d_pro_unit_staff as v ";
                    varname2 = varname2 + " join w_actual as n on v.id_actual=n.id_actual ";
                    varname2 = varname2 + " join w_actual_primary as l on n.at_no =l.at_no ";
                    varname2 = varname2 + " Where ('" + po_no + "'='' OR  n.at_no  like '%" + po_no + "%' )";
                    varname2 = varname2 + " and ('" + product + "'='' OR l.product  like '%" + product + "%' ))";
                }
                varname2 = varname2 + " and ('" + mc_type + "'='' OR  a.position_cd  like '%" + mc_type + "%' ) group by a.userid";

            }
            if (type_kind == "mold")
            {

                ViewBag.name = "Mold";
                var List = new ArrayList();
                ViewBag.List = List;
                varname3 = varname3 + "select (a.md_nm) mc_nm,(a.md_no) mc_no,('Mold')kind, " + "\n";
                var count_day = (last - startOfMonth).TotalDays;
                for (int i = 0; i < (count_day + 1); i++)
                {
                    var bien = startOfMonth.AddDays(i);
                    var bien_doi = bien.ToString("dd");
                    List.Add(bien_doi);
                    var text = bien.ToString("yyyyMMdd");
                    varname3 = varname3 + "(case when " + "\n";
                    varname3 = varname3 + "	 (select count(k.pmid) from d_pro_unit_mc as k " + "\n";
                    varname3 = varname3 + "			where '" + text + "' between (Date_format(k.start_dt,'%Y%m%d')) and (Date_format(k.end_dt,'%Y%m%d')) and k.mc_no=a.md_no)>0 " + "\n";
                    varname3 = varname3 + "		 then 	'bg-warning' else 'bg-white' end)day_" + (i + 1) + " , " + "\n";
                    varname3 = varname3 + "(	SELECT Group_concat(DISTINCT (Concat('PO', Cast(Substr(m.at_no, 3, 11)AS UNSIGNED), ' ' , m.name, ' Start: ', Concat(Substring(n.start_dt, 1, 4), '-', " + "\n";
                    varname3 = varname3 + "       Substring(n.start_dt, 5, 2), '-', Substring(n.start_dt, 7, 2), ' ', Substring(n.start_dt, 9, 2), ':', Substring(n.start_dt, 11, 2), ':', " + "\n";
                    varname3 = varname3 + "       Substring(n.start_dt, 13, 2)),  ' End: ', Concat(Substring(n.end_dt, 1, 4), '-',  Substring(n.end_dt, 5, 2), '-', " + "\n";
                    varname3 = varname3 + "       Substring(n.end_dt, 7, 2), ' ', Substring(n.end_dt, 9, 2), ':',  Substring(n.end_dt, 11, 2), ':', Substring(n.end_dt, 13, 2)) ) )  SEPARATOR ',') " + "\n";
                    varname3 = varname3 + "       cv " + "\n";
                    varname3 = varname3 + "FROM   d_pro_unit_mc AS n " + "\n";
                    varname3 = varname3 + "       JOIN w_actual AS m " + "\n";
                    varname3 = varname3 + "         ON n.id_actual = m.id_actual " + "\n";
                    varname3 = varname3 + "WHERE  n.mc_no = a.md_no " + "\n";
                    varname3 = varname3 + "       AND '" + text + "' BETWEEN ( Date_format(n.start_dt, '%Y%m%d') ) AND ( " + "\n";
                    varname3 = varname3 + "                              Date_format(n.end_dt, '%Y%m%d') ) ) po" + (i + 1) + ",";
                }
                varname3 = varname3 + " ANY_VALUE(a.mdno) id " + "\n";
                varname3 = varname3 + "from d_mold_info as a " + "\n";

                //where
                varname3 = varname3 + " Where ('" + value_code + "'='' OR  a.md_no  like '%" + value_code + "%' )";
                if (po_no != "" || product != "")
                {
                    varname3 = varname3 + " and a.md_no in (select v.md_no from d_pro_unit_mc as v ";
                    varname3 = varname3 + " join w_actual as n on v.id_actual=n.id_actual ";
                    varname3 = varname3 + " join w_actual_primary as l on n.at_no =l.at_no ";
                    varname3 = varname3 + " Where ('" + po_no + "'='' OR  n.at_no  like '%" + po_no + "%' )";
                    varname3 = varname3 + " and ('" + product + "'='' OR l.product  like '%" + product + "%' ))";
                }
                varname3 = varname3 + " and ('" + value_nm + "'='' OR  a.md_nm  like '%" + value_nm + "%' ) group by a.md_no";

            }
            var varname_all = varname1 + "   " + varname2 + "   " + varname3;

            var data = db.Database.SqlQuery<model_daymc_wk_mold>(varname_all).ToList();



            ViewBag.start = date_start;
            ViewBag.end = date_end;
            ViewBag.month = time_now.ToString("MMMM");

            return PartialView(data);

        }
        public class model_daymc_wk_mold
        {
            public string id { get; set; }
            public string fo_no { get; set; }
            public string kind { get; set; }
            public string mc_no { get; set; }
            public string mc_nm { get; set; }
            public string mc_type { get; set; }
            public string day_1 { get; set; }
            public string day_2 { get; set; }
            public string day_3 { get; set; }
            public string day_4 { get; set; }
            public string day_5 { get; set; }
            public string day_6 { get; set; }
            public string day_7 { get; set; }
            public string day_8 { get; set; }
            public string day_9 { get; set; }
            public string day_10 { get; set; }
            public string day_11 { get; set; }
            public string day_12 { get; set; }
            public string day_13 { get; set; }
            public string day_14 { get; set; }
            public string day_15 { get; set; }
            public string day_16 { get; set; }
            public string day_17 { get; set; }
            public string day_18 { get; set; }
            public string day_19 { get; set; }
            public string day_20 { get; set; }
            public string day_21 { get; set; }
            public string day_22 { get; set; }
            public string day_23 { get; set; }
            public string day_24 { get; set; }
            public string day_25 { get; set; }
            public string day_26 { get; set; }
            public string day_27 { get; set; }
            public string day_28 { get; set; }
            public string day_29 { get; set; }
            public string day_30 { get; set; }
            public string day_31 { get; set; }



            public string po1 { get; set; }
            public string po2 { get; set; }
            public string po3 { get; set; }
            public string po4 { get; set; }
            public string po5 { get; set; }
            public string po6 { get; set; }
            public string po7 { get; set; }
            public string po8 { get; set; }
            public string po9 { get; set; }
            public string po10 { get; set; }
            public string po11 { get; set; }
            public string po12 { get; set; }
            public string po13 { get; set; }
            public string po14 { get; set; }
            public string po15 { get; set; }
            public string po16 { get; set; }
            public string po17 { get; set; }
            public string po18 { get; set; }
            public string po19 { get; set; }
            public string po20 { get; set; }
            public string po21 { get; set; }
            public string po22 { get; set; }
            public string po23 { get; set; }
            public string po24 { get; set; }
            public string po25 { get; set; }
            public string po26 { get; set; }
            public string po27 { get; set; }
            public string po28 { get; set; }
            public string po29 { get; set; }
            public string po30 { get; set; }
            public string po31 { get; set; }

        }
        #endregion


    }
}
