using ClosedXML.Excel;
using Microsoft.AspNet.SignalR.Client;
using Mvc_VD.Commons;
using Mvc_VD.Models;
using Mvc_VD.Models.APIRespones;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models.WIP;
using Mvc_VD.Models.WOModel;
using Mvc_VD.Services;
using Mvc_VD.Services.Interface;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using shippingsdmaterial = Mvc_VD.Models.NewVersion.shippingsdmaterial;

namespace Mvc_VD.Controllers
{
    public class ShippingMgtController : BaseController
    {
        private readonly IWMSServices _IWMSServices;
        private readonly IWIPService _IWIPService;
        private readonly IcommonService _IcommonService;
        private readonly IhomeService _ihomeService;

        //private HubConnection connection = new HubConnection(Extension.GetAppSetting("Realtime"));
        //IHubProxy chat;
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
        public ShippingMgtController(
           IWMSServices IWMSServices,
            IWIPService IWIPService,
            IcommonService IcommonService,
           IDbFactory DbFactory, IhomeService ihomeService)
        {
            _IWMSServices= IWMSServices;
            _IWIPService = IWIPService;
            _IcommonService = IcommonService;
            _ihomeService = ihomeService;

            //db = DbFactory.Init();
            //chat = connection.CreateHubProxy("shinsungHub");
            //connection.Start().ContinueWith(task =>
            //{
            //    if (task.IsFaulted)
            //    {
            //        var sss = task.Exception.GetBaseException();
            //        Console.WriteLine("There was an error opening the connection:{0}",
            //                          task.Exception.GetBaseException());
            //    }
            //    else
            //    {
            //        Console.WriteLine("Connected");
            //    }

            //}).Wait();
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

        #region N_ShippingPicking Scan

        public ActionResult ShippingPickingScan(string code)
        {
            ViewData["Message"] = code;
            return SetLanguage("");
        }
        public async Task<ActionResult> GetPickingScan(string sd_no, string sd_nm, string product_cd, string remark)
        {
            try
            {
                var data = await _IcommonService.GetListSDInfo(sd_no, sd_nm, product_cd, remark);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
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
        public async Task<ActionResult> InsertSDInfo(SdInfo w_sd_info)
        {
            #region Tang tự động
            string sd_no = "SD1";
            //var sd_no_last = db.w_sd_info.ToList().LastOrDefault();
            var sd_no_last = await _IcommonService.GetLastSdNo();
            if (sd_no_last != null)
            {
                var sd_noCode = sd_no_last;
                sd_no = string.Concat("SD", (int.Parse(sd_noCode.Substring(2)) + 1).ToString());
            }
            #endregion
            //int sdno = await _IcommonService.GetSdNo() + 1;
            //string sd_no = "SD" + sdno;
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
            int kqq =await  _IcommonService.InsertSdInfo(w_sd_info);

            var data = await _IcommonService.GetListSDInfo(sd_no,"","","");
            return Json(new { result = true, data = data, message = Constant.Success }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> UpdateSDInfo(SdInfo w_sd_info)
        {
            try
            {
                //var KTTT = db.w_sd_info.Find(w_sd_info.sid);
                var KTTT = await _IWMSServices.GetListSdInfoById(w_sd_info.sid);
                var rs = await _IcommonService.GetListSDInfo(KTTT.sd_no, "", "","");
                var status =  rs.FirstOrDefault();
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
                if (checkSdInfo  == null)
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

                //await chat.Invoke<string>("Hello", "010").ContinueWith(task =>
                //{
                //    if (task.IsFaulted)
                //    {
                //        Console.WriteLine("There was an error calling send: {0}",
                //                          task.Exception.GetBaseException());
                //    }
                //    else
                //    {
                //        Console.WriteLine(task.Result);
                //    }
                //});

                return Json(new { result = true, data = w_sd_info.sid, message = Constant.Success }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> getdsMaterialDetail(string sd_no, string mt_no)
        {
            try
            {
                //IEnumerable<shippingsdmaterial> values = _IWIPService.GetListShippngMaterial(sd_no, mt_no);
                var value =await _IWMSServices.GetListShippngMaterial(sd_no, mt_no);

                return Json(value, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { result = false, message = "Lỗi hệ thống!!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DelMaterialShippng(int id)
        {
            //kiểm tra mã này có trong db không
            //var CheckExist = _IWIPService.GetShippingExist(id);
            var CheckExist = await _IWMSServices.CheckMaterialInfoTam(id);
            if (CheckExist == null)
            {
                return Json(new { result = false, message = "Không tìm thấy mã này đễ xóa" }, JsonRequestBehavior.AllowGet);
            }
            //kiểm tra số lượng gửi >= số lượng nhận mới cho xóa.
            // số lượng gửi:
            //var DanhSachGui = _IWIPService.GetListShippngMaterial(CheckExist.sd_no, CheckExist.mt_no);
            var DanhSachGui = await _IWMSServices.GetListShippngMaterial(CheckExist.sd_no, CheckExist.mt_no);
            double SoLuongGui = DanhSachGui.Where(x => x.id != id).Sum(x => x.quantity);
            // số lượng nhan:
            var DanhSachNhan = await _IWMSServices.GetListWMaterial(CheckExist.sd_no, CheckExist.mt_no);
            double SoLuongNhan = DanhSachNhan.Count();
            //Số lượng gửi ít hơn số lượng nhận nên không được xóa
            if (SoLuongGui - SoLuongNhan < 0)
            {
                return Json(new { result = false, message = "Không xóa được vì đã được nhận ở kho NVL" }, JsonRequestBehavior.AllowGet);
            }
            else//Có thể xóa
            {
                var remove = await _IWMSServices.DeleteShippingMaterial(id);
                if (remove > 0)
                {
                    //kiểm tra xem nguyên vật liệu này nhận đủ chưa
                    //UPDATE SD ALERT = 0, AND STS = 001 // trở về trạng thái tồn kho
                    //ktra xem còn sd nào trong w_material_tam không, nêu ko có thì update
                    //var kt_sd = db.w_material_info_tam.Count(x => x.sd_no == sd_no && x.mt_sts_cd.Equals("000"));
                    //if (kt_sd == 0)
                    //int kt_sd = _IWIPService.GetWMaterialInfoTamwithCount(sd_no, "000");

                    var kt_sd = await _IWMSServices.GetListMaterialInfoBySdNo(CheckExist.sd_no);
                    if (!kt_sd.Any(x => x.SoluongConLai > 0))
                    {
                        var user = Session["userid"] == null ? null : Session["userid"].ToString();

                        //_IWIPService.UpdatedSdInfo(0, "001", user, CheckExist.sd_no, DateTime.Now);
                        await _IWMSServices.UpdateSDinfo(0, "000", user, CheckExist.sd_no, DateTime.Now);
                        //await chat.Invoke<string>("Hello", "010").ContinueWith(task =>
                        //{
                        //    if (task.IsFaulted)
                        //    {
                        //        Console.WriteLine("There was an error calling send: {0}",
                        //                          task.Exception.GetBaseException());
                        //    }
                        //    else
                        //    {
                        //        Console.WriteLine(task.Result);
                        //    }
                        //});
                        return Json(new { result = true, message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
                    }

                }
                return Json(new { result = true, message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> updateSoCuon(int id, int Qty)
        {
            try
            {
                //kiểm tra mã này có trong db không
                //var CheckExist = _IWIPService.GetShippingExist(id);
                var CheckExist = await _IWMSServices.CheckMaterialInfoTam(id);
                if (CheckExist == null)
                {
                    return Json(new { result = false, message = "Không tìm thấy mã này đễ xóa" }, JsonRequestBehavior.AllowGet);
                }
                if (Qty == 0)
                {
                    return Json(new { result = false, message = "Số lượng bắt buộc lớn hơn 0" }, JsonRequestBehavior.AllowGet);
                }
                // số lượng gửi:
                var DanhSachGui = await _IWMSServices.GetListShippngMaterial(CheckExist.sd_no, CheckExist.mt_no);
                int SoLuongGui = DanhSachGui.Where(x => x.id != id).Sum(x => x.quantity).ToInt();
                // số lượng nhan:
                //var DanhSachNhan = _IWIPService.GetListWMaterial(CheckExist.sd_no, CheckExist.mt_no);

                var DanhSachNhan = await _IWMSServices.GetListWMaterial(CheckExist.sd_no, CheckExist.mt_no);
                int SoLuongNhan = DanhSachNhan.Count();
                //Số lượng gửi ít hơn số lượng nhận nên không được update
                if (SoLuongGui + Qty - SoLuongNhan < 0)
                {
                    return Json(new { result = false, message = "Không sửa được vì đã được nhận ở kho NVL" }, JsonRequestBehavior.AllowGet);
                }
                else//Có thể sửa
                {
                    var checkUpdateQty = await _IWMSServices.UpdateshippingMaterial(id, Qty);
                    if (checkUpdateQty > 0)
                    {
                        //kiểm tra xem SoluongConLai == 0 là update SD ALERT= 0, status = 001(tỨC TỒN KHO)
                        var user = Session["userid"] == null ? null : Session["userid"].ToString();
                       // var kt_sd = _IWIPService.GetListMaterialInfoBySdNo(CheckExist.sd_no).ToList();
                        var kt_sd = await _IWMSServices.GetListMaterialInfoBySdNo(CheckExist.sd_no);
                        if (!kt_sd.Any(x => x.SoluongConLai > 0))
                        {
                             await _IWMSServices.UpdateSDinfo(0, "001", user, CheckExist.sd_no, DateTime.Now);

                            //await chat.Invoke<string>("Hello", "010").ContinueWith(task1 =>
                            //{
                            //    if (task1.IsFaulted)
                            //    {
                            //        Console.WriteLine("There was an error calling send: {0}",
                            //                          task1.Exception.GetBaseException());
                            //    }
                            //    else
                            //    {
                            //        Console.WriteLine(task1.Result);
                            //    }
                            //});

                        }
                        //kiểm tra xem SoluongConLai > 1 là update SD ALERT= 1, status = 000(tỨC TỒN KHO)
                        else
                        {
                            await _IWMSServices.UpdateSDinfo(1, "000", user, CheckExist.sd_no, DateTime.Now);
                            //await chat.Invoke<string>("Hello", "010").ContinueWith(task1 =>
                            //{
                            //    if (task1.IsFaulted)
                            //    {
                            //        Console.WriteLine("There was an error calling send: {0}",
                            //                          task1.Exception.GetBaseException());
                            //    }
                            //    else
                            //    {
                            //        Console.WriteLine(task1.Result);
                            //    }
                          //  });
                        }
                        return Json(new { result = true, message = "Sửa thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    else return Json(new { result = false, message = "Không thể sửa mã này" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = "Không thể sửa mã này" }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> updateSoMeter(int id, int Qty)
        {
            try
            {
                //kiểm tra mã này có trong db không
                //var CheckExist = _IWIPService.GetShippingExist(id);
                var CheckExist = await _IWMSServices.CheckMaterialInfoTam(id);
                if (CheckExist == null)
                {
                    return Json(new { result = false, message = "Không tìm thấy mã này đễ xóa" }, JsonRequestBehavior.AllowGet);
                }
                if (Qty == 0)
                {
                    return Json(new { result = false, message = "Số lượng bắt buộc lớn hơn 0" }, JsonRequestBehavior.AllowGet);
                }

                //// số lượng gửi:
                //var DanhSachGui = _IWIPService.GetListShippngMaterial(CheckExist.sd_no, CheckExist.mt_no);
                //int SoLuongGui = DanhSachGui.Where(x => x.id != id).Sum(x => x.meter).ToInt();
                //// số lượng nhan:
                //var DanhSachNhan = _IWIPService.GetListWMaterial(CheckExist.sd_no, CheckExist.mt_no);
                //int SoLuongNhan = DanhSachNhan.Sum(x=>x.gr_qty).ToInt();
                ////Số lượng gửi ít hơn số lượng nhận nên không được update
                //if (SoLuongGui + Qty - SoLuongNhan < 0)
                //{
                //    return Json(new { result = false, message = "Không sửa được vì đã được nhận ở kho NVL" }, JsonRequestBehavior.AllowGet);
                //}
                //else//Có thể sửa
                //{

                //    if (_IWIPService.UpdateshippingMeter(id, Qty) > 0)
                //    {
                //        return Json(new { result = true, message = "Sửa thành công" }, JsonRequestBehavior.AllowGet);
                //    }
                //}

                var checkUpdateQty = await _IWMSServices.UpdateshippingMeterMaterial(id, Qty);
                if (checkUpdateQty > 0)
                {
                    return Json(new { result = true, message = "Sửa thành công" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = false, message = "Không thể sửa" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Không thể sửa mã này" }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> GetPickingScanMLQR(string ml_cd)
        {
            try
            {
                if (string.IsNullOrEmpty(ml_cd))
                {
                    return Json(new { result = false, message = "Mã ML NO trống vui lòng Scan lại" }, JsonRequestBehavior.AllowGet);
                }

                //var kttt_null = db.w_material_info_tam.Where(x => x.mt_cd == ml_cd).SingleOrDefault();
                var kttt_null = await _IWMSServices.CheckMaterialInfoTam(ml_cd);
                if (kttt_null == null)
                {
                    return Json(new { result = false, message = Constant.DataNoExist }, JsonRequestBehavior.AllowGet);
                }

                if (kttt_null.mt_barcode == null)
                {
                    return Json(new { result = false, message = "Mã này là nguyên vật liệu phụ không có barcode!!!" }, JsonRequestBehavior.AllowGet);
                }
                if (!(string.IsNullOrEmpty(kttt_null.sd_no)))
                {
                    return Json(new { result = false, message = Constant.Used + " cho " + kttt_null.sd_no }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = true, message = Constant.DataExist, data = kttt_null }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> InsertMTQRSDList(string data, string sd_no)
        {
            try
            {
                //UPDATE w_material_info_tam  COLUMN SD
                var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //var sql2 = new StringBuilder();
                //sql2.Append(" UPDATE  w_material_info_tam  ")

                //.Append("  SET w_material_info_tam.picking_dt='" + time + "', w_material_info_tam.sd_no = '" + sd_no + "' , w_material_info_tam.lct_cd = '002000000000000000' , w_material_info_tam.from_lct_cd = '002000000000000000' , w_material_info_tam.lct_sts_cd = '000' ")
                //.Append(",  w_material_info_tam.dt_of_receipt= IF(w_material_info_tam.dt_of_receipt IS NULL or w_material_info_tam.dt_of_receipt = '', DATE_FORMAT(NOW(), '%Y%m%d'), w_material_info_tam.dt_of_receipt) ")
                //.Append(" WHERE w_material_info_tam.wmtid in (" + data + ") and w_material_info_tam.mt_barcode is not null");
                //int effect_rows = new Excute_query().Execute_NonQuery(sql2);

                await _IWMSServices.UpdateMaterialInfoTam(data,sd_no,time);

                //UPDATE SD ALERT = 1 và sd_sts_cd = '000'
                //var sql3 = new StringBuilder();
                //sql3.Append(" UPDATE  w_sd_info  ")

                //.Append("  SET w_sd_info.alert = 1 , w_sd_info.sd_sts_cd = '000' ")
                //.Append(" WHERE w_sd_info.sd_no in ('" + sd_no + "') ");
                //int effect_rows2 = new Excute_query().Execute_NonQuery(sql3);

                await _IWMSServices.UpdateSdNoInfo(sd_no, 1, "000");
                //Thông báo


                //await chat.Invoke<string>("Hello", "010").ContinueWith(task =>
                //{
                //    if (task.IsFaulted)
                //    {
                //        Console.WriteLine("There was an error calling send: {0}",
                //                          task.Exception.GetBaseException());
                //    }
                //    else
                //    {
                //        Console.WriteLine(task.Result);
                //    }
                //});
                return Json(new { result = true, message = Constant.Success }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }

        }

        #region SD_Info_popup
        public ActionResult PartialView_SD_Info_Popup(string sd_no)
        {
            ViewBag.sd_no = sd_no;

            return PartialView();
        }
        public async Task< ActionResult> GetPickingScanPP(string sd_no)
        {
            var listdata = await _IWMSServices.GetPickingScanPP(sd_no);
            var result = listdata.ToList();
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetPickingScanPP_Count_MT_no(string sd_no)
        {
            var list2 = await _IWMSServices.GetPickingScanPP_Count_MT_no(sd_no);
            var result = list2.ToList();
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        public async Task< ActionResult> GetPickingScanListPP(string sd_no)
        {
            var listData = await _IWMSServices.GetPickingScanListPP(sd_no);
            var result = listData.ToList();
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetPickingScanPP_Memo(string sd_no)
        {
            //var ds = db.w_material_info_memo.Where(x => x.sd_no.Equals(sd_no)).ToList();
            var ds = await _IWMSServices.GetPickingScanPP_Memo(sd_no);
            var rs = ds.ToList();
            var listdata = (from a in ds
                            select new
                            {
                                id = a.id,
                                md_cd = a.md_cd,
                                style_no = a.style_no,
                                style_nm = a.style_nm,
                                size = a.width + "MM X " + a.spec + "M",
                                mt_cd = a.mt_cd,
                                memo = a.memo,
                                month_excel = a.month_excel,
                                TX = a.TX,
                                total_m = a.total_m,
                                total_m2 = a.total_m2,
                                total_ea = a.total_ea,
                                lot_no = a.lot_no,
                            }).ToList();

            return Json(new { data = listdata }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintSD_LIST(string sd_no)
        {
            ViewData["Message"] = sd_no;

            return View();
        }

        public async Task<ActionResult> DeleteRecortSd(string mt_cd)
        {
            try
            {
                if (string.IsNullOrEmpty(mt_cd))
                {
                    return Json(new { result = false, message = Constant.DataNoExist }, JsonRequestBehavior.AllowGet);
                }

                //var kttt = db.w_material_info_tam.Any(x => x.mt_cd == mt_cd && x.mt_sts_cd.Contains("000"));
                var KTTT = await _IWMSServices.CountMaterialInfoTamByMtCd(mt_cd);
                if (KTTT !=  null)
                {
                    if (KTTT == null)
                    {
                        return Json(new { result = false, message = Constant.DataExist }, JsonRequestBehavior.AllowGet);
                    }

                    //insert lại bảng tạm
                    KTTT.sd_no = null;
                    KTTT.lct_cd = null;
                    KTTT.to_lct_cd = null;
                    KTTT.from_lct_cd = null;
                    KTTT.lct_sts_cd = null;
                    KTTT.remark = "Không tìm thấy mã này";
                    //db.Entry(KTTT).State = EntityState.Modified;
                    //db.SaveChanges();
                    await _IWMSServices.UpdateMaterialInfoTam(KTTT);

                    //ktra xem con sd nào bị remain nữa ko, nêu hết rồi thì update status của sd đó
                    var checkMaterialInfoBySdNo = await _IWMSServices.CountMaterialInfoTamBySdNo(KTTT.sd_no);
                    if (checkMaterialInfoBySdNo == null)
                    {
                        //UPDATE SD ALERT = 0 và sd_sts_cd = '001'
                        //var sql3 = new StringBuilder();
                        //sql3.Append(" UPDATE  w_sd_info  ")

                        //.Append("  SET w_sd_info.alert = 0 , w_sd_info.sd_sts_cd = '001' ")
                        //.Append(" WHERE w_sd_info.sd_no in ('" + KTTT.sd_no + "') ");

                        await _IWMSServices.UpdateSdNoInfo(KTTT.sd_no, 0, "001");

                        //using (var cmd = db.Database.Connection.CreateCommand())
                        //{
                        //    db.Database.Connection.Open();

                        //    cmd.CommandText = sql3.ToString();
                        //    var reader = cmd.ExecuteNonQuery();
                        //    int result1 = Int32.Parse(reader.ToString());
                        //    db.Database.Connection.Close();
                        //}
                    }
                    return Json(new { result = true, message = Constant.Success }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { result = false, message = Constant.DataExist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region  PartialView_List_ML_NO_Info_Popup
        public ActionResult PartialView_List_ML_NO_Info_Popup(string sd_no)
        {
            ViewBag.sd_no = sd_no;

            return PartialView();
        }

        public async Task<JsonResult> Get_List_Material_ShippingPickingPage(Pageing paging)
        {
            string mt_no = Request["mt_no"] == null ? "" : Request["mt_no"].Trim();
            string mt_cd = Request["mt_cd"] == null ? "" : Request["mt_cd"].Trim();

            int start = (paging.page - 1) * paging.rows;
            var rs = await _IWMSServices.GetListDataMaterialInfoTam(mt_no, mt_cd);
            var result = rs.OrderByDescending(x => x.mt_cd);
            int totals = result.Count();
            int totalPages = (int)Math.Ceiling((float)totals / paging.rows);
            var dataactual = result.Skip<MaterialInfoTam>(start).Take(paging.rows);
            var jsonReturn = new
            {
                total = totalPages,
                page = paging.page,
                records = totals,
                rows = dataactual
            };
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);


            //StringBuilder sql = new StringBuilder();
            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append(" SELECT a.wmtid ,a.mt_cd,a.mt_no,a.lot_no,a.gr_qty ,a.expiry_dt ,a.dt_of_receipt,a.expore_dt  ");
            //varname1.Append("FROM w_material_info_tam as a ");
            //varname1.Append(" WHERE  a.mt_sts_cd ='000'  AND (a.sd_no IS NULL OR a.sd_no = '') and (a.mt_barcode IS NULL OR a.mt_barcode = '') ");
            //varname1.Append(" AND ('" + mt_no + "'='' OR  a.mt_no like '%" + mt_no + "%' ) ");
            //varname1.Append(" AND ('" + mt_cd + "'='' OR  a.mt_cd like '%" + mt_cd + "%' ) ");
            ////return new InitMethods().ConvertDataTableToJsonAndReturn(varname1);

            //DataTable dt = new InitMethods().ReturnDataTableNonConstraints(varname1);

            //int total = dt.Rows.Count;
            //var result = dt.AsEnumerable().OrderByDescending(x => x.Field<string>("mt_cd"));
            //return new InitMethods().ReturnJsonResultWithPaging(paging, total, result);
        }

        //public ActionResult Get_List_Material_ShippingPicking(string mt_no = "", string mt_cd = "")
        //{
        //    try
        //    {
        //        var data = db.w_material_info_tam.Where(x => x.mt_no.Contains(mt_no) && x.mt_cd.Contains(mt_cd) && string.IsNullOrEmpty(x.sd_no) && x.mt_sts_cd == "000").ToList();
        //        if (data.Count > 0)
        //        {
        //            var datalist = (from a in data
        //                            select new
        //                            {
        //                                wmtid = a.wmtid,
        //                                mt_cd = a.mt_cd,
        //                                mt_no = a.mt_no,
        //                                lot_no = a.lot_no,
        //                                gr_qty = a.gr_qty,
        //                                expiry_dt = a.expiry_dt,
        //                                dt_of_receipt = a.dt_of_receipt,
        //                                expore_dt = a.expore_dt

        //                            }
        //             ).ToList();

        //            return Json(datalist, JsonRequestBehavior.AllowGet);
        //        }
        //        return Json(new { result = false, message = Constant.NotFound }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        public async Task<ActionResult> ShippingPicking_M(string data, string sd_no)
        {
            try
            {
                if (string.IsNullOrEmpty(data))
                {
                    return Json(new { result = false, message = Constant.ChooseAgain }, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(sd_no))
                {
                    return Json(new { result = false, message = Constant.ChooseAgain + " SD No!!!" }, JsonRequestBehavior.AllowGet);
                }
                var kttt_null = new List<MaterialInfoTam>();
                var rs = new MaterialInfoTam();
                string[] temp_id = data.TrimStart('[').TrimEnd(']').Split(',');
                List<int> temp = new List<int>();
                for (int i = 0; i < temp_id.Length; i++)
                {
                    temp.Add(int.Parse(temp_id[i]));
                    rs = await _IWMSServices.GetMaterialInfoTamById(i);
                    kttt_null.Add(rs);
                }

                //var kttt_null = db.w_material_info_tam.Any(x => temp.Contains(x.wmtid));

                if (kttt_null.Count() < 1)
                {
                    return Json(new { result = false, message = Constant.DataExist }, JsonRequestBehavior.AllowGet);
                }

                //UPDATE w_material_info_tam  COLUMN SD
                //var sql2 = new StringBuilder();
                //sql2.Append(" UPDATE  w_material_info_tam  ")

                //.Append("  SET w_material_info_tam.picking_dt='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', w_material_info_tam.sd_no = '" + sd_no + "' , w_material_info_tam.lct_cd = '002000000000000000' , w_material_info_tam.from_lct_cd = '002000000000000000' , w_material_info_tam.lct_sts_cd = '000' ")
                //.Append(" WHERE w_material_info_tam.wmtid in (" + data + ") ");

                //using (var cmd = db.Database.Connection.CreateCommand())
                //{
                //    db.Database.Connection.Open();

                //    cmd.CommandText = sql2.ToString();
                //    var reader = cmd.ExecuteNonQuery();
                //    int result1 = Int32.Parse(reader.ToString());
                //    db.Database.Connection.Close();
                //}
                await _IWMSServices.UpdateMaterialInfoTam(data, sd_no, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                //UPDATE SD ALERT = 1 và sd_sts_cd = '000'
                //var sql3 = new StringBuilder();
                //sql3.Append(" UPDATE  w_sd_info  ")

                //.Append("  SET w_sd_info.alert = 1 , w_sd_info.sd_sts_cd = '000' ")
                //.Append(" WHERE w_sd_info.sd_no in ('" + sd_no + "') ");

                //using (var cmd = db.Database.Connection.CreateCommand())
                //{
                //    db.Database.Connection.Open();

                //    cmd.CommandText = sql3.ToString();
                //    var reader = cmd.ExecuteNonQuery();
                //    int result1 = Int32.Parse(reader.ToString());
                //    db.Database.Connection.Close();
                //}

                await _IWMSServices.UpdateSdNoInfo(sd_no, 1, "000");

                //await chat.Invoke<string>("Hello", "010").ContinueWith(task =>
                //{
                //    if (task.IsFaulted)
                //    {
                //        Console.WriteLine("There was an error calling send: {0}",
                //                          task.Exception.GetBaseException());
                //    }
                //    else
                //    {
                //        Console.WriteLine(task.Result);
                //    }
                //});

                //get data table 2
                var datalist = (from a in kttt_null
                                where temp.Contains(a.wmtid)
                                select new
                                {
                                    wmtid = a.wmtid,
                                    mt_cd = a.mt_cd,
                                    lot_no = a.lot_no,
                                    gr_qty = a.gr_qty,
                                    expiry_dt = a.expiry_dt,
                                    dt_of_receipt = a.dt_of_receipt,
                                    expore_dt = a.expore_dt

                                }
                        ).ToList();

                return Json(new { result = true, message = Constant.Success, datalist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region  PartialView_Create_List_ML_NO_Info_Popup
        public ActionResult PartialView_Create_List_ML_NO_Info_Popup(string sd_no)
        {
            ViewBag.sd_no = sd_no;

            return PartialView();
        }

        public async Task <JsonResult> GetInfo_memo(Pageing pageing, string sd_no, string md_cd, string mt_cd, string lot_no, string style_no)
        {
            //StringBuilder sql = new StringBuilder($"SELECT a.* FROM w_material_info_memo AS a where a.sd_no='" + sd_no + "'");
            //sql.Append(" and ('" + md_cd + "'='' or a.md_cd LIKE '%" + md_cd + "%')");
            //sql.Append(" and ('" + mt_cd + "'='' or a.mt_cd LIKE '%" + mt_cd + "%')");
            //sql.Append(" and ('" + lot_no + "'='' or a.lot_no LIKE '%" + lot_no + "%')");
            //sql.Append(" and ('" + style_no + "'='' or a.style_no LIKE '%" + style_no + "%')");

            int start = (pageing.page - 1) * pageing.rows;
            var result = await _IWMSServices.GetListMaterialInfoMemo(sd_no, md_cd, lot_no, style_no);
            int totals = result.Count();
            int totalPages = (int)Math.Ceiling((float)totals / pageing.rows);
            var dataactual = result.Skip<MaterialInfoMemo>(start).Take(pageing.rows);
            var jsonReturn = new
            {
                total = totalPages,
                page = pageing.page,
                records = totals,
                rows = dataactual
            };
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);


        }
        public async Task<JsonResult> Upload_ExcelMemory(List<MaterialInfoMemo> insertmaterial_info_memo, MaterialInfoMemo w_material_info_memo)
        {
            var data_update = 0;
            var data_create = 0;
            var data_error = 0;
            foreach (var item in insertmaterial_info_memo)
            {
                try
                {
                    w_material_info_memo.TX = (item.TX != null ? item.TX : 0);
                    w_material_info_memo.total_ea = (item.total_ea != null ? item.total_ea : 0);
                    w_material_info_memo.total_m = (item.total_m != null ? item.total_m : 0);
                    w_material_info_memo.total_m2 = (item.total_m2 != null ? item.total_m2 : 0);
                    w_material_info_memo.md_cd = item.md_cd;
                    w_material_info_memo.style_no = item.style_no;
                    w_material_info_memo.style_nm = item.style_nm;
                    w_material_info_memo.mt_cd = item.mt_cd;
                    w_material_info_memo.lot_no = (item.lot_no != null ? item.lot_no.Replace("\r\n", " ") : "");
                    w_material_info_memo.month_excel = item.month_excel;
                    w_material_info_memo.sd_no = item.sd_no;
                    w_material_info_memo.spec = item.spec;
                    w_material_info_memo.spec_unit = item.spec_unit;
                    w_material_info_memo.width = item.width;
                    w_material_info_memo.width_unit = item.width_unit;
                    w_material_info_memo.status = "001";//stock memo
                    w_material_info_memo.receiving_dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    w_material_info_memo.use_yn = "Y";
                    w_material_info_memo.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                    w_material_info_memo.reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                    w_material_info_memo.reg_dt = DateTime.Now;
                    w_material_info_memo.chg_dt = DateTime.Now;
                    //db.Entry(w_material_info_memo).State = EntityState.Added;
                    //db.SaveChanges();
                    int id = await _IWMSServices.InsertIntoMaterialInfoMemo(w_material_info_memo);
                    data_create++;
                }
                catch (Exception)
                {

                    data_error++;
                }
            }


            return Json(new
            {
                result = true,
                data_update = data_update,
                data_create = data_create,
                data_error = data_error
            }, JsonRequestBehavior.AllowGet);
        }
        public async Task <ActionResult> InsertCreate_memo(MaterialInfoMemo w_material_info_memo)
        {
            try
            {
                //var find_product = db.d_style_info.Where(x => x.style_no == w_material_info_memo.style_no).ToList();
                var find_product = await _IWMSServices.GetListDataStyleInfo(w_material_info_memo.style_no);
                if (find_product.Count() == 0)
                {
                    return Json(new { result = false, message = Constant.NotFound + " Product" }, JsonRequestBehavior.AllowGet);

                }

                w_material_info_memo.TX = (w_material_info_memo.TX != null ? w_material_info_memo.TX : 0);
                w_material_info_memo.width = (w_material_info_memo.width != null ? w_material_info_memo.width : 0);
                w_material_info_memo.spec = (w_material_info_memo.spec != null ? w_material_info_memo.spec : 0);

                w_material_info_memo.total_m = (w_material_info_memo.TX * w_material_info_memo.spec);
                w_material_info_memo.total_m2 = (w_material_info_memo.width / 1000

                    * Convert.ToInt32(w_material_info_memo.spec) * w_material_info_memo.TX);
                w_material_info_memo.total_ea = Math.Round(Convert.ToDecimal(Convert.ToDouble(w_material_info_memo.total_m) / 150.8 * 1000 / 1.1 / 1), 0);
                w_material_info_memo.style_nm = find_product.FirstOrDefault().style_nm;
                w_material_info_memo.md_cd = find_product.FirstOrDefault().md_cd;
                w_material_info_memo.status = "001";
                DateTime dt = DateTime.Now; //Today at 00:00:00
                w_material_info_memo.receiving_dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                w_material_info_memo.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                w_material_info_memo.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                w_material_info_memo.width_unit = "MM";
                w_material_info_memo.spec_unit = "M";
                w_material_info_memo.use_yn = "Y";
                w_material_info_memo.reg_dt = DateTime.Now;
                w_material_info_memo.chg_dt = DateTime.Now;

                int id = await _IWMSServices.InsertIntoMaterialInfoMemo(w_material_info_memo);
                var value = await _IWMSServices.GetMaterialInfoMemoById(id);
                return Json(new { result = true, data = value, message = Constant.Success }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = Constant.DataExist }, JsonRequestBehavior.AllowGet);
            }

        }
        public async Task<ActionResult> UpdateCreate_memo(MaterialInfoMemo w_material_info_memo)
        {
            try
            {
                //var KTTT = db.w_material_info_memo.Find(w_material_info_memo.id);
                var KTTT = await _IWMSServices.GetMaterialInfoMemoById(w_material_info_memo.id);
                if (KTTT == null)
                {
                    return Json(new { result = false, message = Constant.DataExist }, JsonRequestBehavior.AllowGet);
                }
                if (KTTT.style_no != w_material_info_memo.style_no)
                {
                    //var find_product = db.d_style_info.Where(x => x.style_no == w_material_info_memo.style_no).ToList();
                    var find_product = await _IWMSServices.GetListDataStyleInfo(w_material_info_memo.style_no);
                    if (find_product.Count() == 0)
                    {
                        return Json(new { result = false, message = Constant.NotFound + " Product" }, JsonRequestBehavior.AllowGet);
                    }
                    KTTT.style_no = find_product.FirstOrDefault().style_no;
                    KTTT.style_nm = find_product.FirstOrDefault().style_nm;
                    KTTT.md_cd = find_product.FirstOrDefault().md_cd;
                }

                KTTT.width = w_material_info_memo.width;
                KTTT.spec = w_material_info_memo.spec;

                KTTT.TX = (w_material_info_memo.TX != null ? w_material_info_memo.TX : 0);
                KTTT.width = (KTTT.width != null ? KTTT.width : 0);
                KTTT.spec = (KTTT.spec != null ? KTTT.spec : 0);

                KTTT.total_m = (KTTT.TX * KTTT.spec);
                KTTT.total_m2 = (KTTT.width / 1000

                    * Convert.ToInt32(KTTT.spec) * KTTT.TX);
                KTTT.total_ea = Math.Round(Convert.ToDecimal(Convert.ToDouble(KTTT.total_m) / 150.8 * 1000 / 1.1 / 1), 0);




                KTTT.mt_cd = w_material_info_memo.mt_cd;
                KTTT.lot_no = w_material_info_memo.lot_no;
                KTTT.memo = w_material_info_memo.memo;
                KTTT.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                //KTTT.chg_dt = DateTime.Now;

                await _IWMSServices.UpdateMaterialInfoMemo(KTTT);
                //db.Entry(KTTT).State = EntityState.Modified;
                //db.SaveChanges();


                return Json(new { result = true, data = KTTT, message = Constant.Success }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task< ActionResult> DelCreate_memo(MaterialInfoMemo w_material_info_memo)
        {
            try
            {
                var KTTT = await _IWMSServices.GetMaterialInfoMemoById(w_material_info_memo.id);
                if (KTTT == null)
                {
                    return Json(new { result = false, message = Constant.DataExist }, JsonRequestBehavior.AllowGet);
                }

                await _IWMSServices.RemoveMaterialInfoMemo(KTTT.id);
                return Json(new { result = true, data = KTTT, message = Constant.Success }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #endregion

        #region N_API_Shipping_Picking_Scan
        public async Task<ActionResult> SaveShipping_PickingScan(string sd_no, string ml_no)
        {
            try
            {
                if (string.IsNullOrEmpty(sd_no))
                {
                    return Json(new { result = false, message = Constant.ChooseAgain + " SD No!!!" }, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(ml_no))
                {
                    return Json(new { result = false, message = Constant.ScanAgain + " Lot" }, JsonRequestBehavior.AllowGet);
                }
                //var kttt_null = db.w_material_info_tam.Any(x => x.mt_cd == ml_no);
                var kttt_null = await _IWMSServices.CheckMaterialInfoTam(ml_no);
                if (kttt_null == null)
                {
                    return Json(new { result = false, message = Constant.DataExist }, JsonRequestBehavior.AllowGet);
                }
                //var KT_SD = db.w_material_info_tam.Any(x => x.mt_cd == ml_no && string.IsNullOrEmpty(x.sd_no) && x.mt_sts_cd == "000");
                var KT_SD = await _IWMSServices.CountMaterialInfoTamByMtCd(ml_no);
                if (KT_SD == null)
                {
                    return Json(new { result = false, message = Constant.Used }, JsonRequestBehavior.AllowGet);
                }
                //UPDATE w_material_info_tam  COLUMN SD
                //var sql2 = new StringBuilder();
                //sql2.Append(" UPDATE  w_material_info_tam  ")
                //.Append("  SET w_material_info_tam.sd_no = '" + sd_no + "' , w_material_info_tam.lct_cd = '002000000000000000' , w_material_info_tam.from_lct_cd = '002000000000000000' , w_material_info_tam.lct_sts_cd = '000' ")
                //.Append(" WHERE w_material_info_tam.mt_cd in ('" + ml_no + "') ");

                //using (var cmd = db.Database.Connection.CreateCommand())
                //{
                //    db.Database.Connection.Open();
                //    cmd.CommandText = sql2.ToString();
                //    var reader = cmd.ExecuteNonQuery();
                //    int result1 = Int32.Parse(reader.ToString());
                //    db.Database.Connection.Close();
                //}

                await _IWMSServices.UpdateMaterialInfoTam(ml_no, sd_no);

                //UPDATE SD ALERT = 1 và sd_sts_cd = '000'
                //var sql3 = new StringBuilder();
                //sql3.Append(" UPDATE  w_sd_info  ")

                //.Append("  SET w_sd_info.alert = 1 , w_sd_info.sd_sts_cd = '000' ")
                //.Append(" WHERE w_sd_info.sd_no in ('" + sd_no + "') ");
                //using (var cmd = db.Database.Connection.CreateCommand())
                //{
                //    db.Database.Connection.Open();
                //    cmd.CommandText = sql3.ToString();
                //    var reader = cmd.ExecuteNonQuery();
                //    int result1 = Int32.Parse(reader.ToString());
                //    db.Database.Connection.Close();
                //}

                await _IWMSServices.UpdateSdNoInfo(sd_no, 1, "000");

                //get data table 1
                //var data_sd = (from a in db.w_sd_info
                //               where a.sd_no.Equals(sd_no)
                //               select new
                //               {
                //                   sd_no = a.sd_no,
                //                   sd_nm = a.sd_nm,
                //                   sts_nm = (db.comm_dt.Where(x => x.mt_cd == "WHS005" && x.dt_cd == a.sd_sts_cd).Select(x => x.dt_nm)),
                //                   alert = a.alert,
                //                   lct_nm = (db.lct_info.Where(x => x.lct_cd == a.lct_cd).Select(x => x.lct_nm)),
                //                   remark = a.remark,
                //               }
                //        ).FirstOrDefault();

                var data_sd = await _IWMSServices.GetPickingScanPP(sd_no);

                //get data table 2
                //var data_material = (from a in db.w_material_info_tam
                //                     where a.mt_cd.Equals(ml_no)
                //                     select new
                //                     {
                //                         wmtid = a.wmtid,
                //                         mt_cd = a.mt_cd,
                //                         lot_no = a.lot_no,
                //                         gr_qty = a.gr_qty,
                //                         expiry_dt = a.expiry_dt,
                //                         dt_of_receipt = a.dt_of_receipt,
                //                         expore_dt = a.expore_dt

                //                     }
                //        ).FirstOrDefault();

                var data_material = await _IWMSServices.GetMaterialInfoTamByMtCd(ml_no);
                return Json(new { result = true, message = Constant.Success, data_sd, data_material }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        public async Task<ActionResult> getadmin(string id)
        {
            //var admin = db.mb_info.Where(x => x.userid == id).ToList();
            var admin =await _ihomeService.GetlistMbInfo(id);
            return Json(admin, JsonRequestBehavior.AllowGet);
        }
        #region ShowMaterialShipping
        public ActionResult ShowMaterialShipping()
        {
            return SetLanguage("");
        }
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
        public async Task<JsonResult> GetMemo(Pageing paging, string datemonth, string product, string material, string date)
        {

            Dictionary<string, string> list = PagingAndOrderBy(paging, " ORDER BY  a.product_cd  ");

            IEnumerable<MaterialShippingMemo> Data = await _IWMSServices.GetListSearchShowMemo(datemonth, product, material, date);
            int totalRecords = Data.Count();
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



            var result = new
            {
                total = totalPages,
                page = int.Parse(list["index"]),
                records = totalRecords,
                rows = Data
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> getShowMaterialShippingDetail(string style_no, string mt_no, string recei_dt, string datemonth)
        {
            try
            {
                var value = await _IWMSServices.getShowMaterialShippingDetail(style_no, mt_no, recei_dt, datemonth);

                return Json(value, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { result = false, message = "Lỗi hệ thống!!" }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> searchShowMaterialShipping(Pageing pageing, string product, string material, string datemonth, string date)
        {

            Dictionary<string, string> list = PagingAndOrderBy(pageing, " ORDER BY  a.product_cd  ");


            IEnumerable<MaterialShipping> Data = await _IWMSServices.GetListSearchShowMaterialShipping(datemonth, product, material, date);
            //int totalRecords = _IWIPService.TotalRecordsShowMaterialShipping(datemonth, product, material);
            int totalRecords = Data.Count();
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
            var result = new
            {
                total = totalPages,
                page = int.Parse(list["index"]),
                records = totalRecords,
                rows = Data
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region PartialView_ListMaterialNoPopup
        public ActionResult PartialView_ListMaterialNoPopup(string sd_no)
        {
            ViewBag.sd_no = sd_no;
            return PartialView();
        }

        [HttpGet]
        public async Task<JsonResult> Get_ListMaterialNo(Pageing paging)
        {
            string mt_no = Request["mt_no"] == null ? "" : Request["mt_no"].Trim();
            string style_no = Request["style_no"] == null ? "" : Request["style_no"].Trim();

            if(mt_no.Contains('['))
            {
                mt_no = mt_no.Replace('[','%');
            }
            if (mt_no.Contains(']'))
            {
                mt_no = mt_no.Replace(']', '%');
            }
            int start = (paging.page - 1) * paging.rows;
            var data = await _IWMSServices.GetlistMaterialInfo(mt_no, style_no, start, paging.rows);
            var records = await _IWMSServices.CountListMaterialInfo(mt_no, style_no);
            int totalPages = (int)Math.Ceiling((float)records / paging.rows);

            return Json(new
            {
                total = totalPages,
                page = paging.page,
                records = records,
                rows = data
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> insertMaterialShipp(string id, string qty, string meter, string sd_no)
        {
            //insert vào bảng shippingsdmaterial
            try
            {
                if ((id != null) && (qty != null) && (meter != null))
                {
                    var idMaterial = id.TrimStart('[').TrimEnd(']').Split(',');
                    var quantity = qty.TrimStart('[').TrimEnd(']').Split(',');
                    var meters = meter.TrimStart('[').TrimEnd(']').Split(',');
                    bool hasOrder = false;
                    for (int i = 0; i < idMaterial.Length; i++)
                    {

                        var idMaterial1 = int.Parse(idMaterial[i]);
                        //var isExistMT = _IWMSService.GetMaterialInfo(idMaterial1);
                        string mtno = await _IWMSServices.GetDMaterialInfo(idMaterial1);

                        //27/10/2021
                        //int checkduplicateshipping = await _IWMSServices.CheckShippingMaterial(sd_no,mtno);
                        //if(checkduplicateshipping > 0)
                        //{
                        //    await _IWMSServices.UpdateDuplicateShipping(sd_no, mtno, Session["authName"] == null ? null : Session["authName"].ToString(),DateTime.Now);
                        //}

                        if (!string.IsNullOrEmpty(mtno) && Convert.ToDouble(quantity[i]) > 0 && Convert.ToDouble(meters[i]) > 0)
                        {
                            Models.NewVersion.shippingsdmaterial item = new Models.NewVersion.shippingsdmaterial();
                            item.sd_no = sd_no;
                            item.mt_no = mtno;
                            item.quantity = Convert.ToDouble(quantity[i]);
                            item.meter = Convert.ToDouble(meters[i]);
                            item.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                            item.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                            //item.reg_dt = DateTime.Now;
                            hasOrder = true;
                            //int b = _IWMSService.InsertToShippingSDMaterial(item);
                            int kq = await _IWMSServices.InsertShippingdMaterial(item);
                        }
                    }
                    if (hasOrder)
                    {
                        //UPDATE SD ALERT = 1 và sd_sts_cd = '000'
                        var user = Session["userid"] == null ? null : Session["userid"].ToString();
                        //_IWIPService.UpdatedSdInfo(1, "000", user, sd_no, DateTime.Now);
                       int resupdatesd = await _IWMSServices.UpdateSDinfo(1, "000", user, sd_no, DateTime.Now);

                        //await chat.Invoke<string>("Hello", "010").ContinueWith(task =>
                        //{
                        //     if (task.IsFaulted)
                        //     {
                        //         Console.WriteLine("There was an error calling send: {0}",
                        //                           task.Exception.GetBaseException());
                        //     }
                        //     else
                        //     {
                        //         Console.WriteLine(task.Result);
                        //     }
                        //});
                        return Json(new { result = true, data = sd_no }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { result = true, data = sd_no }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = "Vui lòng nhập số lượng để xuất kho" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<ActionResult> Getshippingsdmaterial(string sd_no)
        {
            //var datas = _IWMSService.Getshippingsdmaterial(sd_no);
            //var data = _IWIPService.GetListMaterialInfoBySdNo(sd_no);
            var data = await _IWMSServices.GetListMaterialInfoBySdNo(sd_no,"");
            return (Json(data, JsonRequestBehavior.AllowGet));

        }

        [HttpPost]
        public async Task<ActionResult> UploadExcel(List<shippingsdmaterial> tempList)
        {
            if (tempList == null)
            {
                return Json(new { flag = false, message = "Chưa chọn File excel" }, JsonRequestBehavior.AllowGet);
            }
            List<shippingsdmaterial> listData = new List<shippingsdmaterial>();

            var sd_no = "";
            if (tempList.Count > 0)
            {

                tempList = tempList.Where(m => m.quantity > 0 && m.meter > 0 && !(string.IsNullOrEmpty(m.mt_no))).ToList();

                //string jsonObject = JsonConvert.SerializeObject(tempList);
                int countSS = 0;
                foreach (var item in tempList)
                {
                    var nvl = await _IWMSServices.CheckMaterialInfoToUpload(item.mt_no.Trim());
                   // kieem tra NVL có trong d_material_info khônng / nếu tồn tại thì insert vào bảng shippingsdmaterial
                    if (nvl != null)
                    {
                        shippingsdmaterial itemsd = new shippingsdmaterial();
                        item.sd_no = item.sd_no;
                        item.mt_no = item.mt_no.Trim();
                        item.quantity = Convert.ToDouble(item.quantity);
                        item.meter = Convert.ToDouble(item.meter);
                        item.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        item.reg_dt = DateTime.Now;

                        int b = await _IWMSServices.InsertShippingdMaterial(item);
                        if (b > 0)
                        {
                            countSS += 1;
                        }
                        sd_no = item.sd_no;
                    }
                }
                if (countSS > 0)
                {
                   // UPDATE SD ALERT = 1 và sd_sts_cd = '000'

                    var user = Session["userid"] == null ? null : Session["userid"].ToString();
                   // _IWIPService.UpdatedSdInfo(1, "000", user, sd_no, DateTime.Now);
                    await _IWMSServices.UpdateSDinfo(1, "000", user, sd_no, DateTime.Now);
                    //await chat.Invoke<string>("Hello", "010").ContinueWith(task =>
                    //{
                    //    if (task.IsFaulted)
                    //    {
                    //        Console.WriteLine("There was an error calling send: {0}",
                    //                          task.Exception.GetBaseException());
                    //    }
                    //    else
                    //    {
                    //        Console.WriteLine(task.Result);
                    //    }
                    //});
                }
                return Json(new { result = true, data = sd_no, message = "Số liệu được xuất kho là: " + countSS }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { result = false, message = "File excel không hợp lệ." }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion


    }
}
