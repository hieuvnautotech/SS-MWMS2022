using Microsoft.AspNet.SignalR.Client;
using Mvc_VD.Commons;
using Mvc_VD.Models;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models.Response;
using Mvc_VD.Models.WIP;
using Mvc_VD.Services;
using Mvc_VD.Services.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace Mvc_VD.Controllers
{
    public class wipwmsController : BaseController
    {

        private readonly IWIPService _IWIPService;
        private readonly IWOService _IWOService;
        private readonly IcommonService _IcommonService;
        private readonly IWMSServices _IWMSServices;
        private readonly IWIPServices _IWIPServices;
        private readonly IhomeService _IHomeService;
        //private HubConnection connection = new HubConnection(Extension.GetAppSetting("Realtime"));
        //IHubProxy chat;
        //2 cai
        public wipwmsController(
            IWIPService IWIPService,
            IcommonService IcommonService,
            IWMSServices IWMSServices,
            IWIPServices IWIPServices,
            IWOService IWOService,
            IhomeService ihomeService,
             IDbFactory DbFactory)
        {
            _IHomeService = ihomeService;
            _IWMSServices = IWMSServices;
            _IWIPService = IWIPService;
            _IcommonService = IcommonService;
            _IWOService = IWOService;
            _IWIPServices = IWIPServices;
            //chat = connection.CreateHubProxy("shinsungHub");
            //connection.Start().ContinueWith(task =>
            //{
            //    if (task.IsFaulted)
            //    {
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


        #region Material_Information(WIP)
        public ActionResult Material_Information_Wip()
        {
            return View();
        }
        #endregion

        #region Receiving_Manual
        public ActionResult Receiving_Manual()
        {
            return View();
        }

        #region Put_in

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
            var a = Json(lstPersons, JsonRequestBehavior.AllowGet);
            return a;
        }

        public JsonResult GetJsonPersons1(DataTable data)
        {
            var lstPersons = GetTableRows(data);
            return Json(lstPersons, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Shipping_Manual(WIP)
        public ActionResult Shipping_Manual()
        {
            return View();
        }
        #endregion

        #region Picking_Scan_F
        public ActionResult Picking_Scan_F()
        {
            return View();
        }
        #endregion

        #region Receiving_Direction_gr
        public ActionResult Receiving_Direction_gr()
        {
            return View();
        }
        #endregion

        #region Receiving_Directions_WIP
        public ActionResult Receiving_Directions_WIP()
        {
            return View();
        }
        #endregion

        #region Shipping_direc_WIP
        public ActionResult SP_direction_WIP()
        {
            return View();
        }
        #endregion

        #region Shippping_direction_gr(WIP)
        public ActionResult Shippping_direction_WIP()
        {
            return View();
        }
        #endregion

        #region   Material_Return_WIP
        public ActionResult Material_Return_WIP()
        {
            return View();
        }

        private string CreateId_slip(int id)
        {
            if (id.ToString().Length < 2)
            {
                return string.Format("00{0}", id);
            }

            if (id.ToString().Length < 3 || (id.ToString().Length == 2))
            {
                return string.Format("0{0}", id);
            }

            if (id.ToString().Length < 4 || (id.ToString().Length == 3))
            {
                return string.Format("{0}", id);
            }
            return string.Empty;
        }

        #endregion

        #region   Material_Return_M_WMS
        public ActionResult Material_Return_M_WMS()
        {
            return View();
        }
        #endregion

        #region Material_Return_History
        public ActionResult Material_Return_History()
        {
            return View();
        }

        #endregion

        #region N_Receving_Scan_Wip
        public ActionResult Receving_Scan_Wip(string code)
        {
            ViewData["Message"] = code;
            return SetLanguage("");
        }
        public async Task<ActionResult> GetPickingScan(string sd_no, string sd_nm, string product_cd,int page ,int rows)
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
                int start = (page - 1) * rows;
                var data = await _IcommonService.GetListPickingScan(sd_no, sd_nm, product_cd,start, rows);
                //return Json(data, JsonRequestBehavior.AllowGet);
                int totalRecords = _IcommonService.TottalRowPickingScan(sd_no, sd_nm, product_cd);
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
                var jsonReturn = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = data
                };
                return Json(jsonReturn, JsonRequestBehavior.AllowGet);
                //var result = new
                //{
                //    total = totalPages,
                //    page = page,
                //    records = totalRecords,
                //    rows = data
                //};
                //return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var kq1 = new List<ListPickingScan>();

                return Json(new { result = false, message = "Xin vui lòng đăng nhập !", data1 = kq1 }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult FinishMaterialWIP( string sid ,string sd_no)
        {
            int resultRow = 0;
            //update lại SD thành alert la 0 , trang thai 001 => cho no vo kho
            if (!string.IsNullOrEmpty(sid)  && !string.IsNullOrEmpty(sd_no))
            {
                resultRow = _IWIPService.UpdateFinishMaterialWIP(sid, sd_no);
                if(resultRow != 0)
                {
                    return Json(new { result = resultRow, message = "finish thành công" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { result = resultRow, message = "finish thất bại" }, JsonRequestBehavior.AllowGet);
        }


        #region PartialView_Receving_Scan_Wip_Missing_M_Popup
        public ActionResult PartialView_Receving_Scan_Wip_Missing_M_Popup(string sid = "", string sd_no = "", string alert = "")
        {
            ViewBag.sid = sid;
            ViewBag.alert = alert;
            ViewBag.sd_no = sd_no;
            return PartialView();
        }

        public ActionResult PartialView_WIP_Recei_SD_Popup(string sd_no)
        {
            ViewBag.sd_no = sd_no;

            return PartialView();
        }
        #endregion
        #endregion

        #region N_API_Receiving_Scan_Wip

        public async Task<ActionResult> ScanML_no_ReceiWIP(string ml_no, string sd_no)
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
                try
                {
                    //Check input
                    if (string.IsNullOrEmpty(sd_no))
                    {
                        return Json(new { result = false, message = Constant.ChooseAgain + " SD No!!!" }, JsonRequestBehavior.AllowGet);
                    }
                    if (string.IsNullOrEmpty(ml_no))
                    {
                        return Json(new { result = false, message = Constant.ScanAgain + " LOT" }, JsonRequestBehavior.AllowGet);
                    }
                    ml_no = ml_no.Trim();

                    var CheckMaterialTam = _IWIPService.GetWMaterialInfoTamwithmtcd(ml_no);
                    if (CheckMaterialTam == null)
                    {

                        var kttt_kho_mms = _IWOService.GetWMaterialInfowithmtcd(ml_no);
                        if (kttt_kho_mms != null)
                        {
                            return Json(new { result = false, message = "Đã được đưa vào kho" }, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new { result = false, message = Constant.DataNoExist }, JsonRequestBehavior.AllowGet);
                    }

                    var KT_SD = _IWIPService.GetWMaterialInfoTamwithSTS(ml_no);

                    if (KT_SD == null)
                    {
                        return Json(new { result = false, message = Constant.Used }, JsonRequestBehavior.AllowGet);
                    }
                    //INSERT w_material_info
                    DateTime dt = DateTime.Now; //Today at 00:00:00
                    string day_now = dt.ToString("yyyyMMdd");

                    var user = Session["userid"] == null ? null : Session["userid"].ToString();


                    var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");



                    var w_material_info = new Models.MaterialInfoMMS();
                    w_material_info.rece_wip_dt = time;
                    w_material_info.picking_dt = CheckMaterialTam.picking_dt;
                    w_material_info.sd_no = sd_no;
                    w_material_info.mt_no = CheckMaterialTam.mt_no;
                    w_material_info.mt_cd = CheckMaterialTam.mt_cd;
                    w_material_info.mt_type = CheckMaterialTam.mt_type;
                    w_material_info.gr_qty = CheckMaterialTam.gr_qty;
                    w_material_info.real_qty = CheckMaterialTam.real_qty;
                    w_material_info.recevice_dt = day_now;
                    w_material_info.date = day_now;
                    w_material_info.expiry_dt = CheckMaterialTam.expiry_dt;
                    w_material_info.expore_dt = CheckMaterialTam.expore_dt;

                    w_material_info.dt_of_receipt = CheckMaterialTam.dt_of_receipt;
                    w_material_info.recevice_dt_tims = CheckMaterialTam.recevice_dt_tims;
                    w_material_info.lot_no = CheckMaterialTam.lot_no;
                    w_material_info.mt_barcode = CheckMaterialTam.mt_barcode;
                    w_material_info.mt_qrcode = CheckMaterialTam.mt_qrcode;
                    w_material_info.mt_sts_cd = "001";
                    w_material_info.lct_cd = CheckMaterialTam.lct_cd;
                    w_material_info.lct_sts_cd = "101";
                    w_material_info.from_lct_cd = CheckMaterialTam.from_lct_cd;
                    w_material_info.reg_id = user;
                    w_material_info.chg_id = user;
                    w_material_info.reg_dt = DateTime.Now;
                    w_material_info.chg_dt = DateTime.Now;

                    int idWMaterialInfo = _IWOService.InsertToWmaterialInfo(w_material_info);





                    _IWIPService.DeleteWMaterialTam(ml_no);


                    int kt_sd = _IWIPService.GetWMaterialInfoTamwithCount(sd_no, "000");
                    if (kt_sd <= 0)
                    {


                        _IWIPService.UpdatedSdInfo(0, "001", user, sd_no, DateTime.Now);

                        //await chat.Invoke<string>("Hello", "010").ContinueWith(task => {
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

                    IEnumerable<SdInfoModel> ddd = _IWIPService.GetListSDInfo(sd_no);


                    WMaterialInfo datalist = _IWIPService.GetListWMaterialInfo(ml_no);

                    return Json(new { result = true, message = Constant.Success, data1 = ddd, data2 = datalist, remain_qty = kt_sd }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var kq1 = new List<SdInfoModel>();
                var kq2 = new WMaterialInfo();
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !", data1 = kq1, data2 = kq2, remain_qty = 0 }, JsonRequestBehavior.AllowGet);
            }

        }
        public async  Task<ActionResult> GetPickingScanMLQRTotal(string sd_no)
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
                var data = await _IWMSServices.GetListMaterialInfoBySdNo(sd_no, "");
                return (Json(data, JsonRequestBehavior.AllowGet));
            }
            else
            {
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }


        }
        public async Task<ActionResult> ScanML_no_ReceiWIP2(string ml_no, string sd_no, string LocationCode)
        {
            //var resqheader = Request.Headers;
            //string[] resqheaderkey = resqheader.AllKeys;
            //string[] resqheaderval = resqheader.GetValues("requestFrom");
            //string resqheadervalmob = "";
            //if (resqheaderval != null)
            //{
            //    resqheadervalmob = resqheaderval[0];
            //}
            //if (Session["authorize"] != null || resqheadervalmob == "Mob")
            //{
                try
                {
                    //Check input
                    if (string.IsNullOrEmpty(sd_no))
                    {
                        return Json(new { result = false, message = Constant.ChooseAgain + " SD No!!!" }, JsonRequestBehavior.AllowGet);
                    }
                    if (string.IsNullOrEmpty(ml_no))
                    {
                        return Json(new { result = false, message = Constant.ScanAgain + " LOT" }, JsonRequestBehavior.AllowGet);
                    }

                    ml_no = ml_no.Trim().ToUpper();

                    //KIỂM TRA XEM SD CÓ TỒN TẠI KHÔNG.
                    var checksdinfo = await _IcommonService.CheckSDInfo(sd_no);
                    if (checksdinfo == null)
                    {
                        return Json(new { result = false, message = "SD này không được kho NVL gửi qua" }, JsonRequestBehavior.AllowGet);
                    }

                    //string[] arrListStr = ml_no.Split(new string[] { "-CP-" }, StringSplitOptions.None);
                    //var mt_no = arrListStr[0];
                    var date_now = DateTime.Now.ToString("yyMMdd");
                    //var check = mt_no + "-CP-" + date_now;

                    var kttt_kho_mms = await _IcommonService.CheckExistMaterialProduct(ml_no);
                    if (kttt_kho_mms != null)
                    {
                        return Json(new { result = false, message = "Đã được đưa vào kho SẢN XUẤT <br/>Thuộc sd:  " + kttt_kho_mms.sd_no + "<br/> Thời gian:  " + kttt_kho_mms.create_date }, JsonRequestBehavior.AllowGet);
                    }
                
                    var CheckMaterialTam = await _IWIPServices.GetDataMaterialInfoTam(ml_no);
                    if (CheckMaterialTam == null)
                    {
                        return Json(new { result = false, message = "Mã vừa quét là: " + ml_no + " <br/> Vui lòng kiểm tra và thử lại" }, JsonRequestBehavior.AllowGet);
                    }
                    var mt_no = CheckMaterialTam.mt_no;
                    //kiểm tra xem nguyên vật liệu này nhận đủ chưa
                    var ktNhanduchua = await _IWMSServices.GetListPickingScanBySdNo(sd_no, CheckMaterialTam.mt_no); //4
                if (ktNhanduchua.Count > 0)
                    {
                        if (ktNhanduchua[0].SoluongConLai == 0)
                        {
                            return Json(new { result = false, message = "Mã " + ktNhanduchua[0].mt_no + " đã nhận đủ" }, JsonRequestBehavior.AllowGet);
                        }

                    }
                    //var MaterialTam = await _IWIPServices.GetDataMaterialInfoTam(check);
                    if (await _IcommonService.CheckMaterialNoShipp(mt_no, sd_no))
                    {
                        return Json(new { result = false, message = "Mã này không thuộc những nguyên vật liệu xuất kho" }, JsonRequestBehavior.AllowGet);
                    }

                    //INSERT Into Inventory
                    InventoryProduct invenproduct = new InventoryProduct();

                    //var user1 = Session["authName"] == null ? null : Session["authName"].ToString();
                    var user = Session["userid"] == null ? null : Session["userid"].ToString();
                    //var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    //kiểm tra xem nguyên vật liệu này đã tồn tại ở trong kho hay chưa ?

                    invenproduct.material_code = ml_no;
                    invenproduct.recei_wip_date = DateTime.Now;
                    invenproduct.sd_no = sd_no;
                    invenproduct.mt_no = CheckMaterialTam.mt_no;
                    invenproduct.mt_type = CheckMaterialTam.mt_type;
                    invenproduct.gr_qty = CheckMaterialTam.gr_qty;
                    invenproduct.real_qty = CheckMaterialTam.real_qty;
                    invenproduct.recei_date = DateTime.Now;
                    invenproduct.expiry_date = Convert.ToDateTime(CheckMaterialTam.expiry_dt);
                    invenproduct.export_date = Convert.ToDateTime(CheckMaterialTam.expore_dt);
                    invenproduct.date_of_receipt = Convert.ToDateTime(CheckMaterialTam.dt_of_receipt);
                    invenproduct.lot_no = CheckMaterialTam.lot_no;
                    invenproduct.from_lct_cd = LocationCode;
                    invenproduct.location_code = LocationCode;
                    invenproduct.location_number = "";
                    invenproduct.status = "001";
                    invenproduct.lct_sts_cd = "101";
                    invenproduct.mt_type = "PMT";
                    invenproduct.create_id = user;
                    invenproduct.change_id = user;
                    invenproduct.create_date = DateTime.Now;
                    invenproduct.change_date = DateTime.Now;
                    int resinsert = await _IcommonService.InsertInventoryProduct(invenproduct);

                    await _IWIPServices.DeleteMaterialInfoTamByCode(ml_no);

                    var kt_sd = await _IWMSServices.GetListMaterialInfoBySdNo(sd_no, "");
                    if (!kt_sd.Any(x => x.SoluongConLai > 0))
                    {
                        await _IWMSServices.UpdateSDinfo(0, "001", user, sd_no, DateTime.Now);
                        //await chat.Invoke<string>("Hello", "010").ContinueWith(task => {
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

                    var data = await _IWMSServices.GetListPickingScanBySdNo(sd_no, invenproduct.mt_no);
                    return (Json(new { result = true, data = data }, JsonRequestBehavior.AllowGet));
                }
                catch (Exception e)
                {
                    return Json(new { result = false, message = e.Message }, JsonRequestBehavior.AllowGet);
                }
            //}
            //else
            //{
            //    var kq = new List<PickingScanResponse>();
            //    return (Json(new { result = false, data = kq, message = "Xin vui lòng đăng nhập" }, JsonRequestBehavior.AllowGet));
            //}

        }

        #endregion

        #region N_Shipping_Scan_WIP
        public ActionResult Shipping_Scan_WIP()
        {
            return SetLanguage("~/Views/wipwms/ShippingScanWIP/Shipping_Scan_WIP.cshtml");
        }

        [HttpGet]
        public async Task<JsonResult> GetEXInfo(Pageing paging, string ex_no, string ex_nm)
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
                var result = await _IWIPServices.GetListDataExInfo(ex_no, ex_nm);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = new List<ExInfo>();
                var jsonReturn = new
                {   result = false,
                    total = 0,
                    page = 0,
                    records = 0,
                    rows = data,
                    message = "Xin vui lòng đăng nhập"
                };
                return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> InsertEXInfo(ExInfo w_ex_info)
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
                try
                {
                    #region Tang tự động

                    String ex_no = "EX1";

                    var ex_no_last = await _IWIPServices.GetLastDataExInfo();
                    if (ex_no_last != null)
                    {
                        var ex_noCode = ex_no_last;
                        ex_no = string.Concat("EX", (int.Parse(ex_noCode.Substring(2)) + 1).ToString());
                    }

                    #endregion Tang tự động

                    w_ex_info.ex_no = ex_no;
                    w_ex_info.lct_cd = "002000000000000000";
                    w_ex_info.alert = 0;

                    DateTime dt = DateTime.Now;
                    string day_now = dt.ToString("yyyy-MM-dd");
                    w_ex_info.work_dt = day_now;

                    w_ex_info.status = "000";
                    w_ex_info.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    w_ex_info.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();

                    w_ex_info.use_yn = "Y";

                    w_ex_info.reg_dt = DateTime.Now;
                    w_ex_info.chg_dt = DateTime.Now;

                    var exid = await _IWIPServices.InsertIntoExInfo(w_ex_info);
                    var result = await _IWIPServices.GetExInfoById(exid);

                    return Json(new { result = true, data = result, message = Constant.Success }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { message = Constant.ErrorSystem, result = false }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var kq = new ExInfo();
                return Json(new { result = false, data = kq, messgae = "Xin vui lòng đăng nhập" }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<ActionResult> ModifyEXInfo(ExInfo w_ex_info)
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
                try
                {
                    //var KTTT = db.w_ex_info.Find(w_ex_info.exid);
                    var KTTT = await _IWIPServices.GetExInfoById(w_ex_info.exid);
                    if (KTTT == null)
                    {
                        return Json(new { result = false, message = Constant.DataNoExist }, JsonRequestBehavior.AllowGet);
                    }
                    KTTT.ex_nm = w_ex_info.ex_nm;
                    KTTT.remark = w_ex_info.remark;
                    KTTT.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    KTTT.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    KTTT.reg_dt = DateTime.Now;
                    KTTT.chg_dt = DateTime.Now;
                    await _IWIPServices.UpdateExInfo(KTTT);
                    return Json(new { result = true, data = KTTT, message = Constant.Success }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { result = false, message =  Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var kq = new ExInfo();
                return Json(new { result = false, data = kq, message = "Xin vui lòng đăng nhập !" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<ActionResult> GetShipping_ScanMLQR_WIP(string mt_cd)
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
                try
                {
                    if (string.IsNullOrEmpty(mt_cd))
                    {
                        return Json(new { result = false, message = Constant.ScanAgain }, JsonRequestBehavior.AllowGet);
                    }


                    var kttt_null = await _IWIPServices.CheckMaterialInventoryProduct(mt_cd);
                    if (kttt_null < 1)// data ko còn không kho sản xuất, kiểm tra kho nguyên vật liệu
                    {

                        var kttt_nvl = await _IWIPServices.CheckMaterialInfoTam(mt_cd);
                        if (kttt_nvl > 0)
                        {
                            return Json(new { result = false, message = "Mã này đã được đưa về kho nguyên vật liệu" }, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new { result = false, message = Constant.DataNoExist }, JsonRequestBehavior.AllowGet);
                    }


                    var KT_ML = await _IWIPServices.GetListInventoryProduct(mt_cd);
                    if (KT_ML.Count() == 0)
                    {
                        return Json(new { result = false, message = Constant.Used }, JsonRequestBehavior.AllowGet);
                    }
                    var sts_nm = await _IWIPServices.GetDetailName("WHS005", KT_ML.FirstOrDefault().status);
                    var mt_type_nm = await _IWIPServices.GetDetailName("WHS005", KT_ML.FirstOrDefault().status);
                    var data = (from a in KT_ML

                                select new
                                {
                                    wmtid = a.materialid,
                                    mt_cd = a.material_code,
                                    mt_no = a.mt_no,
                                    lot_no = a.lot_no,
                                    gr_qty = a.gr_qty,
                                    expiry_dt = a.expiry_date,
                                    dt_of_receipt = a.date_of_receipt,
                                    expore_dt = a.export_date,
                                    sd_sts_cd = a.status,
                                    sts_nm = sts_nm,

                                    mt_type_nm = mt_type_nm,
                                    mt_type = a.mt_type,

                                }
                     ).ToList();
                    //var value = KT_ML.AsEnumerable().Select(async a => new
                    //{
                    //    wmtid = a.materialid,
                    //    mt_cd = a.mt_cd,
                    //    mt_no = a.mt_no,
                    //    lot_no = a.lot_no,
                    //    gr_qty = a.gr_qty,
                    //    expiry_dt = a.expiry_date,
                    //    dt_of_receipt = a.date_of_receipt,
                    //    expore_dt = a.export_date,
                    //    sd_sts_cd = a.status,
                    //    //sts_nm = await _IWIPServices.GetDetailName("WHS005", a.status),
                    //    //mt_type_nm = await _IWIPServices.GetDetailName("COM004", a.mt_type),
                    //    mt_type = a.mt_type,
                    //});

                    return Json(new { result = true, message = Constant.Success, data = data }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var kq = new List<InventoryProduct>();
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !", data = kq }, JsonRequestBehavior.AllowGet);

            }

        }
        public async Task<ActionResult> UpdateShipping_ScanMLQR_WIP(string data, string ex_no)
        {
            try
            {
                //ktra input
                if (string.IsNullOrEmpty(ex_no))
                {
                    return Json(new { result = false, message = Constant.ChooseAgain + " EX No" }, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(data))
                {
                    return Json(new { result = false, message = Constant.CheckData }, JsonRequestBehavior.AllowGet);
                }

                //UPDATE w_material_info  COLUMN ex

                DateTime dt = DateTime.Now;
                string day_now = dt.ToString("yyyyMMdd");
                string full_date = dt.ToString("yyyy-MM-dd HH:mm:ss");
                var user = Session["userid"] == null ? null : Session["userid"].ToString();

                await _IWIPServices.UpdatePartialStatusExInfo(ex_no);
                await _IWIPServices.InsertIntoStatusExInfo(data);
                await _IWIPServices.UpdateShippingMaterialReturn(data, ex_no, full_date, user);


                string[] temp_id = data.TrimStart('[').TrimEnd(']').Split(',');
                List<int> temp = new List<int>();
                for (int i = 0; i < temp_id.Length; i++)
                {
                    temp.Add(int.Parse(temp_id[i]));
                }

                var rs = await _IWIPServices.FindMaterialInventoryProductByListId(temp);
                var dataResponse = rs.AsEnumerable().Select(async a => new
                {
                    wmtid = a.materialid,
                    mt_cd = a.mt_cd,
                    mt_no = a.mt_no,
                    lot_no = a.lot_no,
                    gr_qty = a.gr_qty,
                    expiry_dt = a.expiry_date,
                    dt_of_receipt = a.date_of_receipt,
                    expore_dt = a.export_date,
                    sd_sts_cd = a.status,
                    sts_nm =  await _IWIPServices.GetDetailNameByComm_DT("WHS005", a.status),
                    mt_type_nm = await _IWIPServices.GetDetailNameByComm_DT("COM004", a.mt_type),
                    mt_type = a.mt_type,
                }) ;
                return Json(new { result = true, message = Constant.Success }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> DeleteEXInfo(ExInfo w_ex_info)
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
                try
                {

                    var KTTT = await _IWIPServices.GetExInfoById(w_ex_info.exid);
                    if (KTTT == null)
                    {
                        return Json(new { result = false, message =  Constant.DataNoExist }, JsonRequestBehavior.AllowGet);
                    }
                    if (!(KTTT.status.Equals("000")))
                    {
                        return Json(new { result = false, message = "EX này đã có NVL trả về, Bạn không được xóa nó" }, JsonRequestBehavior.AllowGet);
                    }

                    //UPDATE sd
                    await _IWIPServices.DeleteExInfoById(KTTT.exid);

                    return Json(new { result = true, data = KTTT, message = Constant.Success }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { result = false, message  = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var kq = new ExInfo();
                return Json(new { result = false, message = "Xin vui lòng đăng nhập !", data = kq }, JsonRequestBehavior.AllowGet);
            }

        }

        #region  PartialView_List_ML_NO_Info_Popup_WIP
        public ActionResult PartialView_List_ML_NO_Info_Popup_WIP(string ex_no)
        {
            ViewBag.ex_no = ex_no;
            return PartialView("~/Views/wipwms/ShippingScanWip/PartialView_List_ML_NO_Info_Popup_WIP.cshtml");
        }

        public ActionResult PrintEX_LIST(string ex_no)
        {
            ViewData["Message"] = ex_no;
            return PartialView("~/Views/wipwms/ShippingScanWip/PrintEX_LIST.cshtml");
        }

        #endregion

        #region PartialView_Create_List_Memory_Popup
        public ActionResult PartialView_Create_List_Memory_Popup(string ex_no)
        {
            ViewBag.ex_no = ex_no;
            return PartialView("~/Views/wipwms/ShippingScanWip/PartialView_Create_List_Memory_Popup.cshtml");
        }

        #endregion

        #region PartialView_EX_Info_Popup

        public ActionResult PartialView_EX_Info_Popup(string ex_no)
        {
            ViewBag.ex_no = ex_no;
            return PartialView("~/Views/wipwms/ShippingScanWip/PartialView_EX_Info_Popup.cshtml");

        }

        public async Task<ActionResult> GetExWip_pp(string ex_no)
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
                var result = await _IWIPServices.GetListDataExInfo(ex_no);
                var listdata = result.ToList();
                return Json(new { data = listdata }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var kq = new List<ExInfo>();
                return Json(new { data = kq, message = "Xin vui lòng đăng nhập !", result = false}, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<ActionResult> GetShippingWIPListPP(string ex_no)
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
                try
                {
                    var result = await _IWIPServices.GetShippingWIPListPP(ex_no);
                    var data1 = result.ToList();
                    return Json(new { data = data1 }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { result = false }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var kq = new List<ShippingWIPListPPResponse>();
                return Json(new { data = kq, message = "Xin vui lòng đăng nhập !", result = false }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<ActionResult> GetPickingScanPP_Count_MT_no(string ex_no)
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
                try
                {
                    var result = await _IWIPServices.GetPickingScanPP_Count_MT_no(ex_no);
                    var list2 = result.ToList();
                    return Json(new { data = list2 }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    throw e;
                    return Json(new { result = false }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var kq = new List<PickingScanPPCountMTNoResponse>();
                return Json(new { data = kq, message = "Xin vui lòng đăng nhập !", result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> GetShippingScanPP_Memo(string ex_no)
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
                var result = await _IWIPServices.GetListDataMaterialMemo(ex_no);
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var kq = new List<MaterialInfoMemo>();
                return Json(new { data = kq, message = "Xin vui lòng đăng nhập !", result = false }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #endregion


    }
}
