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
using Mvc_VD.Services.Interface;
using System.Threading.Tasks;
using Mvc_VD.Models.NewVersion;
using System.Web.Http;
using HttpGetAttribute = System.Web.Mvc.HttpGetAttribute;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;
using HttpDeleteAttribute = System.Web.Mvc.HttpDeleteAttribute;
using HttpPutAttribute = System.Web.Mvc.HttpPutAttribute;

namespace Mvc_VD.Controllers
{
    public class standardController : BaseController
    {
        //protected SupplierService _supplierService = new SupplierService();
        private Entities db = new Entities();
        private IStandardServices _standardServices;
        private IhomeService _IHomeService;

        public standardController(IStandardServices standardServices, IhomeService ihomeService)
        {
            _standardServices = standardServices;
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
        //... Supplier INFO
        #region Supplier
        public JsonResult deletesupllier( int id ) {
            int count = db.supplier_info.Where(x => x.spno == id).ToList().Count();
            var result = new { result = true };
            if (count > 0) {
                supplier_info supplier_info = db.supplier_info.Find(id);
                db.supplier_info.Remove(supplier_info);
                db.SaveChanges();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            result = new { result = false };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult supplierInfo() {
            return SetLanguage("");
        }
        public ActionResult a() {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAllCountries() {

            var data = await _standardServices.GetListCountrỵ();
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public async Task<ActionResult> selectSupplierInfo()
        {
            var rs = await _standardServices.GetListSupplierInfo();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }


        //public ActionResult insertSupplierInfo( supplier_info supplier_info, int spno, string sp_cd, string phone_nb, string e_mail, string web_site, string address, string re_mark, string fax_nb, string cell_nb, string changer_id, string bsn_tp ) {
        //    var a = db.supplier_info.Count(x => x.sp_cd == sp_cd);
        //    var result = new { result = a };
        //    if (a == 0) {
        //        supplier_info.bsn_tp = bsn_tp;
        //        supplier_info.changer_id = changer_id;
        //        supplier_info.cell_nb = cell_nb;
        //        supplier_info.fax_nb = fax_nb;
        //        supplier_info.spno = spno;
        //        supplier_info.sp_cd = sp_cd.ToUpper();
        //        supplier_info.phone_nb = phone_nb;
        //        supplier_info.e_mail = e_mail;
        //        supplier_info.web_site = web_site;
        //        supplier_info.address = address;
        //        supplier_info.re_mark = re_mark;
        //        supplier_info.use_yn = "Y";
        //        supplier_info.del_yn = "N";
        //        supplier_info.changer_id = Session["authName"] == null ? null : Session["authName"].ToString();
        //        supplier_info.reg_id = Session["authName"] == null ? null : Session["authName"].ToString();
        //        supplier_info.chg_id = Session["authName"] == null ? null : Session["authName"].ToString();

        //        DateTime reg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
        //        String dateString = reg_dt.ToString("yyyy-MM-dd HH:mm:ss");

        //        DateTime chg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
        //        String dateChString = chg_dt.ToString("yyyy-MM-dd HH:mm:ss");
        //        supplier_info.reg_dt = reg_dt;
        //        supplier_info.chg_dt = chg_dt;

        //        db.Entry(supplier_info).State = EntityState.Added;
        //        db.SaveChanges();

        //        //insert user
        //        mb_info mb_info = new mb_info();
        //        var ds_user = db.mb_info.Count(x => x.userid == sp_cd);
        //        if ((ds_user == 0))
        //        {
        //            mb_info.userid = sp_cd.ToUpper();
        //            mb_info.uname = supplier_info.sp_nm;
        //            mb_info.nick_name = supplier_info.sp_nm;
        //            mb_info.upw = sp_cd.ToUpper();
        //            mb_info.grade = "SUPPLIER";
        //            mb_info.tel_nb = supplier_info.phone_nb;
        //            mb_info.e_mail = supplier_info.e_mail;
        //            mb_info.cel_nb = supplier_info.cell_nb;

        //            mb_info.mail_yn = "N";
        //            mb_info.ltacc_dt = DateTime.Now;
        //            mb_info.mbout_dt = DateTime.Now;
        //            mb_info.session_limit = DateTime.Now;

        //            mb_info.mbjoin_dt = DateTime.Now;

        //            mb_info.reg_dt = DateTime.Now;
        //            mb_info.chg_dt = DateTime.Now;
        //            db.mb_info.Add(mb_info);
        //            db.SaveChanges();

        //            mb_author_info mb_author_info = new mb_author_info();
        //            mb_author_info.userid = sp_cd.ToLower();
        //            mb_author_info.at_cd = db.author_info.Where(x => x.at_nm == mb_info.grade).FirstOrDefault().at_cd == null ? "" : db.author_info.Where(x => x.at_nm == mb_info.grade).FirstOrDefault().at_cd;
        //            mb_author_info.reg_dt = DateTime.Now;
        //            mb_author_info.chg_dt = DateTime.Now;
        //            db.mb_author_info.Add(mb_author_info);
        //            db.SaveChanges();
        //           }
        //        var vaule = (from m in db.supplier_info
        //                     orderby m.chg_dt descending
        //                     where m.spno == supplier_info.spno
        //                     select new {
        //                         spno = m.spno,
        //                         sp_cd = m.sp_cd,
        //                         sp_nm = m.sp_nm,
        //                         bsn_tp = m.bsn_tp,
        //                         changer_id = m.changer_id,
        //                         phone_nb = m.phone_nb,
        //                         cell_nb = m.cell_nb,
        //                         fax_nb = m.fax_nb,
        //                         e_mail = m.e_mail,
        //                         web_site = m.web_site,
        //                         address = m.address,
        //                         re_mark = m.re_mark,
        //                         bsn_tp1 = (db.comm_dt.Where(x => x.dt_cd == m.bsn_tp && x.mt_cd == "COM001").Select(x => x.dt_nm)),
        //                     }).FirstOrDefault();

        //        return Json(new { result = true, vaule }, JsonRequestBehavior.AllowGet);
        //    };
        //    return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public async Task<ActionResult> insertSupplierInfo([FromBody]SupplierInfo item)
        {
            string user = Session["userid"] == null ? null : Session["userid"].ToString();
            if (string.IsNullOrEmpty(user))
            {
                return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
            }
            var count = await _standardServices.CheckSupplierInfo(item.sp_cd);
            if(count > 0)
            {
                return Json(new { result = false, message = "Đã tồn tại " + item.sp_cd + " này trong hệ thống" }, JsonRequestBehavior.AllowGet);
            }
            item.chg_id = Session["authName"] == null ? null : Session["authName"].ToString();
            item.reg_id = Session["authName"] == null ? null : Session["authName"].ToString();
            item.reg_dt = DateTime.Now;
            item.chg_dt = DateTime.Now;
            var spno = await _standardServices.InsertSupplierInfo(item);
            var rs = await _standardServices.GetSupplierInfoById(spno);
            return Json(new {result = true, message = "Thêm dữ liệu vào thành công!", vaule = rs }, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public async Task<ActionResult> updateSupplierInfo([FromBody] SupplierInfo item)
        {
            string user = Session["userid"] == null ? null : Session["userid"].ToString();
            if (string.IsNullOrEmpty(user))
            {
                return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
            }
            var supplier_info = await _standardServices.GetSupplierInfoById(item.spno);
            if (supplier_info == null)
            {
                return Json(new { result = false, message = "Không tồn tại " + item.sp_cd + " này trong hệ thống" }, JsonRequestBehavior.AllowGet);
            }
            supplier_info.sp_nm = item.sp_nm;
            supplier_info.bsn_tp = item.bsn_tp;
            supplier_info.cell_nb = item.cell_nb;
            supplier_info.fax_nb = item.fax_nb;
            supplier_info.phone_nb = item.phone_nb;
            supplier_info.e_mail = item.e_mail;
            supplier_info.web_site = item.web_site;
            supplier_info.address = item.address;
            supplier_info.re_mark = item.re_mark;
            supplier_info.use_yn = "Y";
            supplier_info.del_yn = "N";
            supplier_info.chg_id = Session["authName"] == null ? null : Session["authName"].ToString();
            supplier_info.reg_id = Session["authName"] == null ? null : Session["authName"].ToString();
            supplier_info.reg_dt = DateTime.Now;
            supplier_info.chg_dt = DateTime.Now;

            await _standardServices.UpdateSupplierInfo(supplier_info);
            return Json(new { result = true, message = "Cập nhập dữ liệu vào thành công!", spno = supplier_info.spno }, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public async Task<JsonResult> deleteSupplier(int spno)
        {
            var supplier = await _standardServices.GetSupplierInfoById(spno);
            if (supplier == null) {
                return Json(new { result = false, message = "Không tìm thấy dữ liệu " + spno + " này trong hệ thống" }, JsonRequestBehavior.AllowGet);
            }
            await _standardServices.RemoveSupplierInfo(supplier.spno);
            return Json(new { result = true, message = "Xóa dữ liệu thành công !" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> SearchsupplierInfo(string codeData, string nameData, string bsn_searchData)
        {

            var rs = await _standardServices.SearchSupplierInfo(codeData,nameData,bsn_searchData);
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Buyer
        public ActionResult buyerInfo() {
            return SetLanguage("");
        }

        // GET: /standarinfo/buyerInfo
        [HttpGet]
        public async Task<JsonResult> GetBuyerInfo()
        {
            var data = await _standardServices.GetListBuyerInfo();
            return Json(new { rows = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> InsertBuyer([FromBody]BuyerInfo buyer_info, string buyer_cd, HttpPostedFileBase file )
        {
            string user = Session["userid"] == null ? null : Session["userid"].ToString();
            if (string.IsNullOrEmpty(user))
            {
                return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
            }
            var countBuyerInfo = await _standardServices.CheckBuyerInfo(buyer_info.buyer_cd);
            if(countBuyerInfo > 0)
            {
                return Json(new { result = false, message = "Đã tồn tại " + buyer_cd + " này trong hệ thống" }, JsonRequestBehavior.AllowGet);
            }

            buyer_info.reg_dt = DateTime.Now;
            buyer_info.chg_dt = DateTime.Now;
            buyer_info.del_yn = "N";
            buyer_info.buyer_cd = buyer_cd.ToUpper();
            buyer_info.chg_id = Session["authName"] == null ? null : Session["authName"].ToString();
            buyer_info.reg_id = Session["authName"] == null ? null : Session["authName"].ToString();
            buyer_info.reg_dt = DateTime.Now;
            buyer_info.chg_dt = DateTime.Now;
            // Insert Image
            var profile = Request.Files;
            string imgname = string.Empty;
            string ImageName = string.Empty;

            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any()) {
                var logo1 = System.Web.HttpContext.Current.Request.Files["file"];
                if (logo1.ContentLength > 0) {
                    var profileName = Path.GetFileName(logo1.FileName);
                    var ext = Path.GetExtension(logo1.FileName);

                    ImageName = profileName;
                    var comPath = Server.MapPath("~/Images/BuyerImg/") + ImageName;

                    logo1.SaveAs(comPath);
                    buyer_info.logo = ImageName;
                }
            }
            var byno = await _standardServices.InsertIntoBuyerInfo(buyer_info);
            var rs = await _standardServices.GetBuyerInfoById(byno);
            return Json(new { result = true, message = "Thêm dữ liệu thành công!", value = rs}, JsonRequestBehavior.AllowGet);

        }

        [HttpPut]
        public async Task<ActionResult> Editbuyer([FromBody]BuyerInfo item)
        {
            string user = Session["userid"] == null ? null : Session["userid"].ToString();
            if (string.IsNullOrEmpty(user))
            {
                return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
            }
            var buyer_info = await _standardServices.GetBuyerInfoById(item.byno);
            if(buyer_info == null)
            {
                return Json(new { result = false, message = "Không tồn tại " + item.byno + " này trong hệ thống!" }, JsonRequestBehavior.AllowGet);
            }
            buyer_info.buyer_cd = item.buyer_cd;
            buyer_info.buyer_nm = item.buyer_nm;
            buyer_info.brd_nm = item.brd_nm;
            buyer_info.web_site = item.web_site;
            buyer_info.cell_nb = item.cell_nb;
            buyer_info.address = item.address;
            buyer_info.phone_nb = item.phone_nb;
            buyer_info.e_mail = item.e_mail;
            buyer_info.fax_nb = item.fax_nb;
            buyer_info.use_yn = item.use_yn;
            buyer_info.re_mark = item.re_mark;
            buyer_info.chg_dt = DateTime.Now;
            buyer_info.del_yn = "N";

            var profile = Request.Files;
            string imgname = string.Empty;
            string ImageName = string.Empty;

            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any()) {
                var logo1 = System.Web.HttpContext.Current.Request.Files["file"];
                if (logo1.ContentLength > 0) {
                    var profileName = Path.GetFileName(logo1.FileName);
                    var ext = Path.GetExtension(logo1.FileName);

                    ImageName = profileName;
                    var comPath = Server.MapPath("~/Images/BuyerImg/") + ImageName;

                    logo1.SaveAs(comPath);
                    buyer_info.logo = ImageName;
                }
            }
            await _standardServices.UpdateBuyerInfo(buyer_info);
            return Json(new { result = true, message = "Cập nhập liệu thành công!"}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> searchBuyer(string buyer_cd, string buyer_nm )
        {
            var data = await _standardServices.SearchBuyerInfo(buyer_cd,buyer_nm);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public async Task<JsonResult> deleteBuyer(int m_byno)
        {
            var buyerInfo = await _standardServices.GetBuyerInfoById(m_byno);
            if (buyerInfo == null) {
                return Json(new { result = false, message = "Không tồn tại " + m_byno + " này trong hệ thống!" }, JsonRequestBehavior.AllowGet);
            }
            await _standardServices.RemoveBuyerInfo(buyerInfo.byno);
            return Json(new { result = true, message = "Xóa thành công!" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region BussinessType

        public ActionResult BusinessTypeInfor()
        {
            return SetLanguage("");
        }

        [HttpGet]
        public async Task<ActionResult> getBusInforData(string mt_cd )
        {
          var result = await _standardServices.GetListSupplierType(mt_cd);
          return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public async Task<JsonResult> searchBusInfo(string dt_cd, string dt_nm)
        {
            var data = await _standardServices.SearchSupplierType(dt_cd, dt_nm);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> insertBusInfor(CommCode comm_dt, string dt_cd, string dt_nm, string use_yn, string dt_exp)
        {
            string user = Session["userid"] == null ? null : Session["userid"].ToString();
            if (string.IsNullOrEmpty(user))
            {
                return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
            }
            var count = await _standardServices.CheckSupplierType(dt_cd, "COM001");
            if (count > 0)
            {
                return Json(new { result = false, message = "Đã tồn tại " + dt_cd + " này trong hệ thống" }, JsonRequestBehavior.AllowGet);
            }
            comm_dt.dt_cd = dt_cd.ToUpper();
            comm_dt.dt_nm = dt_nm;
            comm_dt.use_yn = use_yn;
            comm_dt.dt_exp = dt_exp;
            comm_dt.reg_dt = DateTime.Now;
            comm_dt.chg_dt = DateTime.Now;
            comm_dt.chg_id = Session["authName"] == null ? null : Session["authName"].ToString();
            comm_dt.reg_id = Session["authName"] == null ? null : Session["authName"].ToString();
            comm_dt.del_yn = "N";
            comm_dt.dt_order = 1;
            comm_dt.mt_cd = "COM001";

            var cdid = await _standardServices.ÍnertIntoSupplierType(comm_dt);
            var rs = await _standardServices.GetSupplierTypeById(cdid);
            return Json(new { result = true, message = "Thêm dữ liệu vào thành công!", value = rs }, JsonRequestBehavior.AllowGet);

        }

        [HttpPut]
        public async Task<ActionResult> updateBusInfo(int cdid, string dt_cd, string dt_nm, string use_yn, string dt_exp )
        {
            try {
                string user = Session["userid"] == null ? null : Session["userid"].ToString();
                if (string.IsNullOrEmpty(user))
                {
                    return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
                }
                var supplierType = await _standardServices.GetSupplierTypeById(cdid);

                if (supplierType == null)
                {
                    return Json(new { result = false, message = "Không tồn tại " + dt_cd + " này trong hệ thống" }, JsonRequestBehavior.AllowGet);
                }

                supplierType.dt_cd = dt_cd.ToUpper();
                supplierType.dt_nm = dt_nm;
                supplierType.use_yn = use_yn;
                supplierType.dt_exp = dt_exp;
                supplierType.chg_dt = DateTime.Now;
                supplierType.reg_dt = DateTime.Now;
                supplierType.chg_id = Session["authName"] == null ? null : Session["authName"].ToString();
                supplierType.reg_id = Session["authName"] == null ? null : Session["authName"].ToString();

                await _standardServices.UpdateSupplierType(supplierType);
                return Json(new { result = true, message = "Cập nhập thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {

                throw ex;
            }

        }

        [HttpDelete]
        public async Task<JsonResult> deleteBus( int cdid )
        {
            var supplierType = await _standardServices.GetSupplierTypeById(cdid);
            if (supplierType == null) {
                return Json(new { result = false, message = "Không tồn tại " + cdid + " này trong hệ thống"}, JsonRequestBehavior.AllowGet);
            }
            await _standardServices.RemoveSupplierType(supplierType.cdid);
            return Json(new { result = true, message = "Xóa dữ liệu thành công!" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Manufact
        public ActionResult ManufacInfo() {
            return SetLanguage("");
        }



        public ActionResult getManufacInfoData()
        {
            //var mfInfo = db.manufac_info.OrderByDescending(x => x.chg_dt).ToList<manufac_info>();
            return Json(new { rows = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult insertManufacInfo( manufac_info manufac_info, string mf_cd, HttpPostedFileBase file ) {
            string user = Session["userid"] == null ? null : Session["userid"].ToString();
            if (string.IsNullOrEmpty(user))
            {
                return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
            }
            manufac_info.reg_dt = DateTime.Now;
            manufac_info.chg_dt = DateTime.Now;
            manufac_info.del_yn = "N";
            manufac_info.mf_cd = mf_cd.ToUpper();

            var profile = Request.Files;
            string imgname = string.Empty;
            string ImageName = string.Empty;
            var countManufactInfo = db.manufac_info.Count(x => x.mf_cd == mf_cd.ToUpper());
            if (countManufactInfo == 0) {
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any()) {
                    var logo1 = System.Web.HttpContext.Current.Request.Files["file"];
                    if (logo1.ContentLength > 0) {
                        var profileName = Path.GetFileName(logo1.FileName);
                        var ext = Path.GetExtension(logo1.FileName);

                        ImageName = profileName;
                        var comPath = Server.MapPath("~/Images/ManufactImg/") + ImageName;

                        logo1.SaveAs(comPath);


                        manufac_info.logo = ImageName;
                    }
                }
                else
                    manufac_info.logo = "logo.png";

                if (ModelState.IsValid) //check what type of extension
                    {
                    db.Entry(manufac_info).State = EntityState.Added;
                    db.SaveChanges(); // line that threw exception
                    var result = new { result1 = true, message = "Thành công", result = countManufactInfo };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            else {
                var result = new { result = countManufactInfo };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return View(manufac_info);
        }


        public ActionResult updateManufacInfo( int mfno, string mf_cd, string mf_nm, string brd_nm, string web_site, string address,
            string phone_nb, string re_mark, string logo ) {
            var profile = Request.Files;
            string imgname = string.Empty;
            string ImageName = string.Empty;
            var countManufact = db.manufac_info.Count(x => x.mf_cd == mf_cd);
            var manufac_info = db.manufac_info.FirstOrDefault(x => x.mf_cd == mf_cd);
            if (countManufact > 0) {
                manufac_info.mf_nm = mf_nm;
                manufac_info.brd_nm = brd_nm;
                manufac_info.web_site = web_site;
                manufac_info.address = address;
                manufac_info.phone_nb = phone_nb;
                manufac_info.re_mark = re_mark;
                //manufac_info.chg_dt = DateTime.Now;



                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any()) {
                    var logo1 = System.Web.HttpContext.Current.Request.Files["file"];
                    if (logo1.ContentLength > 0) {
                        var profileName = Path.GetFileName(logo1.FileName);
                        var ext = Path.GetExtension(logo1.FileName);

                        ImageName = profileName;
                        var comPath = Server.MapPath("~/Images/ManufactImg/") + ImageName;

                        logo1.SaveAs(comPath);
                        manufac_info.logo = ImageName;
                    }
                    else
                        manufac_info.logo = Server.MapPath("/Images/ManufactImg/") + "logo.png";
                }
                else {
                    manufac_info.logo = logo;
                }

                if (ModelState.IsValid) //check what type of extension
                    {
                    db.Entry(manufac_info).State = EntityState.Modified;
                    db.SaveChanges(); // line that threw exception
                    var result = new { result = countManufact, imgname = ImageName };
                    return Json(result, JsonRequestBehavior.AllowGet);

                }
            }
            else {
                var result = new { result = countManufact, imgname = ImageName };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return View(manufac_info);
        }


        public JsonResult searchManufacInfo() {
            // Get Data from ajax function

            string code = Request.QueryString["mf_cd"];
            string name = Request.QueryString["mf_nm"];
            string brd_nm = Request.QueryString["brd_nm"];
            var sql = new StringBuilder();
            sql.Append(" SELECT mfi.* ")
                .Append("FROM  manufac_info as mfi ")
                 .Append("WHERE ('" + code + "'='' OR   mfi.mf_cd like '%" + code + "%' )")
                 .Append("AND ('" + name + "'='' OR   mfi.mf_nm like '%" + name + "%' )")
                 .Append("AND ('" + brd_nm + "'='' OR   mfi.brd_nm like '%" + brd_nm + "%' )")
                             .Append("ORDER BY mfi.chg_dt DESC ");
            var data = db.manufac_info.SqlQuery(sql.ToString()).ToList<manufac_info>();


            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult deleteManufac( int mfno ) {
            var manufacdata = db.manufac_info.Count(x => x.mfno == mfno);
            if (manufacdata > 0) {
                var manufac_record = db.manufac_info.FirstOrDefault(x => x.mfno == mfno);
                db.manufac_info.Remove(manufac_record);
                db.SaveChanges();
            }
            return Json(manufacdata, JsonRequestBehavior.AllowGet);
        }
        #endregion

        protected override void Dispose( bool disposing ) {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}