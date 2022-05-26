using ClosedXML.Excel;
using Mvc_VD.Commons;
using Mvc_VD.Models.DTO;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models.WIP;
using Mvc_VD.Models.WOModel;
using Mvc_VD.Services;
using Mvc_VD.Services.Interface;
using Mvc_VD.Services.Interface.MMS;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Mvc_VD.Controllers
{
    public class ExportToMachineController : BaseController
    {
        //private readonly IWIPService _IWIPService;
        //private Entities db;
        private readonly IcommonService _IcommonService;
        private readonly IWIPServices _IWIPServices;
        private readonly IhomeService _homeService;
        public ExportToMachineController(IcommonService IcommonService, IWIPServices IWIPServices, IDbFactory DbFactory,IhomeService ihomeService)
        {
            _IcommonService = IcommonService;
            _IWIPServices = IWIPServices;
            _homeService = ihomeService;
          //  db = DbFactory.Init();
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

        #region danh_sach_bang_xuat_lieu_cho_may
        public ActionResult ExportToMachineScan()
        {
            return SetLanguage("~/Views/wipwms/ExportToMachine/ExportToMachineScan.cshtml");
        }


        [HttpGet]
        public async Task<ActionResult> listExportMaterial(Pageing pageing, string ExportCode, string ProductCode, string ProductName, string Description)
        {

            int totalRecords = await _IWIPServices.TotalRecordSearchExportToMachineFinish(ExportCode, ProductCode, ProductName, Description);
            int totalPages = 0;
            try
            {
                totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageing.rows);
            }
            catch (Exception e)
            {
                totalPages = totalRecords / pageing.rows;
            }

            var data = await _IWIPServices.GetListExportToMachineFinish(ExportCode, ProductCode, ProductName, Description);

            var result = new
            {
                total = totalPages,
                page = pageing.page,
                records = totalRecords,
                rows = data
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public async Task<ActionResult> searchExportMaterial(Pageing pageing, string ExportCode, string ProductCode, string ProductName, string Description)
        {

            int totalRecords = await _IWIPServices.TotalRecordsSearchExportToMachine(ExportCode, ProductCode, ProductName, Description);
           // int pagesize = int.Parse(list["size"]);
            int totalPages = 0;
            try
            {
                totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageing.rows);
            }
            catch (Exception e)
            {
                totalPages = totalRecords / pageing.rows;
                throw e;
            }
            int pageIndex = pageing.page;
            int pageSize = pageing.rows;
            var data = await _IWIPServices.GetListSearchExportToMachine(ExportCode, ProductCode, ProductName, Description);
            var result = new
            {
                total = totalPages,
                page = pageing.page,
                records = totalRecords,
                rows = data.ToPagedList(pageIndex, pageSize)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public async Task<JsonResult> InsertExporttomachine(ExportToMachineModel item)
        {
            #region Tang tự động
            String ExportCode = "EP1";

            var listExport = await _IWIPServices.GetListExportToMachine();
            var Exportlast = listExport.FirstOrDefault();

            if (Exportlast != null)
            {
                ExportCode = Exportlast.ExportCode;
                ExportCode = string.Concat("EP", (int.Parse(ExportCode.Substring(2)) + 1).ToString());
            }
            #endregion
            item.ExportCode = ExportCode;

            item.CreateId = (Session["userid"] == null) ? "" : Session["userid"].ToString();
            item.CreateDate = DateTime.Now;
            item.ChangeId = (Session["userid"] == null) ? "" : Session["userid"].ToString();
            item.ChangeDate = DateTime.Now;
            item.IsFinish = "N";

            int id = await _IWIPServices.InsertToExportToMachine(item);
            if(id > 0)
            {
                var data = await _IWIPServices.GetExportToMachineById(id);
                return Json(new { result = true, message = "Thành công!!!", value = data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Tạo dữ liệu thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPut]
        public async Task<ActionResult> ModifyExporttomachine(ExportToMachineModel item)
        {
            try
            {
                item.ChangeId = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                item.CreateId = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                item.ChangeDate = DateTime.Now;
                item.CreateDate = DateTime.Now;

                await _IWIPServices.ModifyToExportToMachine(item);
                var Data = await _IWIPServices.GetListSearchExportToMachine(item.ExportCode, "", "", "");
                return Json(new { result = true, data = Data, message = "Thành công!"}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = "Sửa thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteExporttomachine(string ExportCode)
        {

            if (string.IsNullOrEmpty(ExportCode))
            {
                return Json(new { result = false, message = "Vui lòng Chọn một phiếu xuất để xóa" }, JsonRequestBehavior.AllowGet);
            }
            //kiểm tra xem ep này đã xuất bất kì liệu nào ra máy chưa/ nếu chưa mới cho xóa
            var checkExpCode = await _IWIPServices.CheckMaterialEP(ExportCode);
            if (checkExpCode > 0)
            {
                return Json(new { result = false, message = "Phiếu này đã có Nguyên Vật Liệu xuất, Bạn không thể xóa" }, JsonRequestBehavior.AllowGet);
            }

            await _IWIPServices.DeleteToExportToMachine(ExportCode);
            return Json(new { result = true, message = "Thành công!!!" }, JsonRequestBehavior.AllowGet);
        }

        //them chuc nang finish
        [HttpPut]
        public async Task<JsonResult> FinishExtoMachine(ExportToMachineModel item)
        {
            try
            {
                item.ChangeId = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                item.ChangeDate = DateTime.Now;
                if(item.IsFinish == "true")
                {
                    item.IsFinish = "Y";
                }
                else
                {
                    item.IsFinish = "N";
                }
                await _IWIPServices.FinishExportToMachine(item);
                return Json(new { result = true, message = "Kết Thúc Thành Công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = "Sửa thất bại", ErrorMessage = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetExport_ScanMLQR_WIP(string materialCode)
        {
            try
            {
                //Check input
                if (string.IsNullOrEmpty(materialCode))
                {
                    return Json(new { result = false, message = "Mã Nguyên vật liệu trống, Vui lòng Scan lại" }, JsonRequestBehavior.AllowGet);
                }
                materialCode = materialCode.Trim();
                //Kiểm tra mã materialCode có tồn tại ở kho NVL không
                var IsWmaterial = await _IWIPServices.CheckInInventoryProduct(materialCode);
                if (IsWmaterial != null)
                {
                    //kiểm tra xem mã khác 002 là kho khác
                    if (!IsWmaterial.location_code.StartsWith("002"))
                    {
                        var VITRI = await checkvitri(IsWmaterial.location_code);
                        return Json(new { result = false, message = "Mã Này Đang ở " + VITRI }, JsonRequestBehavior.AllowGet);
                    }
                    //kiểm tra mã này có trạng thái gì, nếu != 001 thì return
                    if (IsWmaterial.status != "001")
                    {
                        var VITRI = await checktrangthai(IsWmaterial.status);
                        return Json(new { result = false, message = "Mã Này Đang " + VITRI }, JsonRequestBehavior.AllowGet);
                    }
                    //kiểm tra mã này đã đưa lên kệ nào chưa
                    if (!string.IsNullOrEmpty(IsWmaterial.ExportCode) && !string.IsNullOrEmpty(IsWmaterial.LoctionMachine))
                    {
                        //var VITRI = checkvitri(IsWmaterial.lct_cd);
                        return Json(new { result = false, message = "Mã này đang ở MÁY, PHIẾU XUẤT " + IsWmaterial.ExportCode }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { result = true, message = "Thành công", data = IsWmaterial }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { result = false, message = "NVL này chưa được nhập kho sản xuất!!!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Sửa thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateMaterialToMachine(string ExportCode, string MachineCode, string ListId)
        {
            try
            {
                var list = JsonConvert.DeserializeObject<List<string>>(ListId);
                //Check input
                if (string.IsNullOrEmpty(ExportCode))
                {
                    return Json(new { result = false, message = "Vui lòng Chọn một phiếu xuất" }, JsonRequestBehavior.AllowGet);
                }
                if (list.Count < 1)
                {
                    return Json(new { result = false, message = "Mã Nguyên vật liệu trống, Vui lòng Scan lại" }, JsonRequestBehavior.AllowGet);
                }
                //ListId = ListId.Trim();


                //update kệ và máy cho mã nguyên vật liệu này

                var IsWmaterial = new InventoryProduct();
                IsWmaterial.ExportCode = ExportCode;
                IsWmaterial.LoctionMachine = MachineCode;
                //IsWmaterial.status = "002";
                IsWmaterial.ShippingToMachineDatetime = DateTime.Now;
                IsWmaterial.change_date = DateTime.Now;

                await _IWIPServices.UpdateListMaterialToMachine(IsWmaterial, list);
                //_IWIPService.UpdateMaterialToMachine(IsWmaterial, ListId);

                return Json(new { result = true, message = "Thành công" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { result = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<string> checkvitri(string lct_cd)
        {
            var checkvitri = await _IWIPServices.CheckIsExistLocation(lct_cd);
            var csv = checkvitri.lct_nm;
            return csv;
        }

        public async Task<string> checktrangthai(string mt_sts_cd)
        {
            var checktrangthai = await _IWIPServices.CheckStatus(mt_sts_cd);
            var csv = string.Join(", ", checktrangthai.Select(x => x.dt_nm));
            return csv;
        }
        #endregion

        #region PartialViewExportToMachinePP
        public ActionResult PartialViewExportToMachinePP(string ExportCode, bool edit)
        {
            ViewBag.Deleted = edit;
            ViewBag.ExportCode = ExportCode;
            return PartialView("~/Views/wipwms/ExportToMachine/PartialViewExportToMachinePP.cshtml");
        }

        //public ActionResult GetShippingtoMachine(string ExportCode)
        //{
        //    var listdata = _IWIPService.GetListSearchExportToMachine(ExportCode, "");

        //    return Json(new { data = listdata }, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public async Task<ActionResult> GetListExportToMachine(string ExportCode)
        {
            var listdata = await _IWIPServices.GetListExportToMachine(ExportCode);
            return Json(new { data = listdata }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetShippingScanPP_Count_MT_no(string ExportCode)
        {
            var result = await _IWIPServices.GetListShippingScanPP(ExportCode);
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintExport_LIST(string ExportCode)
        {
            ViewData["Message"] = ExportCode;
            return PartialView("~/Views/wipwms/ExportToMachine/PrintExport_LIST.cshtml");
        }
        public ActionResult PrintShippingSortingFGExport_LIST(string ExportCode)
        {
            ViewData["Message"] = ExportCode;
            return PartialView("~/Views/fgwms/SortingFG/ShippingFG/PrintShippingSortingFG.cshtml");
        }

        [HttpGet]
        public async Task<ActionResult> GetShippingtoMachinePP(string ExportCode)
        {
            var listdata = await _IWIPServices.GetListSearchExportToMachinePP(ExportCode);
            return Json(new { data = listdata }, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteRecortEP(string mt_cd)
        {
            try
            {
                //Check input
                if (string.IsNullOrEmpty(mt_cd))
                {
                    return Json(new { result = false, message = "Vui lòng chọn mã cần xóa" }, JsonRequestBehavior.AllowGet);
                }

                mt_cd = mt_cd.Trim();

                //Kiểm tra mã materialCode có tồn tại ở kho NVL không
                var IsWmaterial = await _IWIPServices.CheckMaterialsInventoryProduct(mt_cd);
                //kiểm tra xem mã khác 002 là kho khác
                if (IsWmaterial != null)
                {
                    if (IsWmaterial.status.Equals("001"))
                    {
                        //xóa liệu này ra khỏi phiếu xuất
                        IsWmaterial.ExportCode = "";
                        IsWmaterial.LoctionMachine = "";
                        IsWmaterial.ShippingToMachineDatetime = null;
                        IsWmaterial.change_date = DateTime.Now;
                        IsWmaterial.create_id = Session["userid"] == null ? null : Session["userid"].ToString();

                        await _IWIPServices.UpdateMaterialToWIP(IsWmaterial);
                        return Json(new { result = true, message = "Thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    var VITRI = checktrangthai(IsWmaterial.status);
                    return Json(new { result = false, message = "Mã Này Đang " + VITRI }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteRecortEP1(string mt_cd)
        {
            try
            {
                //Check input
                if (string.IsNullOrEmpty(mt_cd))
                {
                    return Json(new { result = false, message = "Vui lòng chọn mã cần xóa" }, JsonRequestBehavior.AllowGet);
                }

                mt_cd = mt_cd.Trim();

                //Kiểm tra mã materialCode có tồn tại ở kho NVL không
                var IsWmaterial = await _IWIPServices.CheckMaterialsInventoryProductByMaterialId(mt_cd);
                //kiểm tra xem mã khác 002 là kho khác
                if (IsWmaterial != null)
                {
                    if (IsWmaterial.status.Equals("001"))
                    {
                        //xóa liệu này ra khỏi phiếu xuất
                        IsWmaterial.ExportCode = "";
                        IsWmaterial.LoctionMachine = "";
                        IsWmaterial.ShippingToMachineDatetime = null;
                        IsWmaterial.change_date = DateTime.Now;
                        IsWmaterial.create_id = Session["userid"] == null ? null : Session["userid"].ToString();

                        await _IWIPServices.UpdateMaterialToWIP(IsWmaterial);
                        return Json(new { result = true, message = "Thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    var VITRI = checktrangthai(IsWmaterial.status);
                    return Json(new { result = false, message = "Mã Này Đang " + VITRI }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region FActory_location
        public ActionResult Factory_Location()
        {
            return SetLanguage("~/Views/wipwms/FactionWip/Factory_Location.cshtml");
        }

        [HttpGet]
        public async Task<ActionResult> GetLocation(string lct_cd)
        {
            try
            {
                var listdata = await _IWIPServices.GetLocationWIP(lct_cd);
                return Json(listdata, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new { result = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> ViewLoactionDetail()
        {
            string[] keys = Request.Form.AllKeys;
            if (keys.Length >= 1)
            {
                var value = Request.Form[keys[0]];
                var result = await checkvitri(value);
                var LocationName = result.ToString();
                ViewBag.lct_cd = (value == "undefined" ? "" : value);
                ViewBag.LocationName = (LocationName == "undefined" ? "" : LocationName);
            }
            else
            {
                ViewBag.Error = "Không có dữ liệu được gửi lên";
            }
            return View("~/Views/wipwms/FactionWip/ViewLocationDetail.cshtml");
        }

        public async Task<ActionResult> DetailRackWIP(string lct_cd)
        {
              //kiểm tra kệ này có tồn tại hay không
                var IsExistLocation = await _IWIPServices.CheckIsExistLocation(lct_cd);
                if (IsExistLocation == null)
                {
                    return Json(new { result = false, message = "kệ này không tồn tại" }, JsonRequestBehavior.AllowGet);
                }

                //nếu level = 000 thì search like 002
                if (IsExistLocation.level_cd.Equals("000"))
                {
                    lct_cd = "002";
                }
                //kiểm tra location có rỗng ko và nếu level = 002 thì search bằng chính nó, ngược lại search like

                if (!string.IsNullOrEmpty(lct_cd) && IsExistLocation.level_cd.Equals("001"))
                {
                    lct_cd = lct_cd.Substring(0, 9);
                }
                var locationCode = lct_cd;
                DateTime datetime = DateTime.Now;

                TimeSpan timestart = new TimeSpan(00, 0, 0); //00 o'clock
                TimeSpan timeend = new TimeSpan(08, 0, 0); //08 o'clock
                TimeSpan timenow = DateTime.Now.TimeOfDay;

                if ((timenow > timestart) && (timenow < timeend))
                {
                    datetime = datetime.AddDays(-1);
                }

            var myDate = datetime.Year + "-" + datetime.Month + "-" + datetime.Day + " 08:00:00";
            //var myDate = "2021-10-18 08:00:00";
            var list2 = await _IWIPServices.GetListLocationDetail(locationCode, myDate);
                return Json(new { data = list2 }, JsonRequestBehavior.AllowGet);

        }

        //public async Task<ActionResult> searchfactory_location(string lct_cd = null, string lct_nm = null)
        //{
        //    var data = db.lct_info.where(item => (item.lct_cd.startswith("002")

        //   && item.lct_cd.contains(lct_cd)
        //   && item.lct_nm.contains(lct_nm)
        //   && item.level_cd.equals("002")

        //    )).orderby(item => item.lct_cd).thenby(item => item.level_cd).tolist();


        //    return Json(data, JsonRequestBehavior.allowget);

        //}

        [HttpGet]
        public async Task<ActionResult> GetFactory_Location()
        {
            var data = await _IcommonService.GetLocationFactory();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Return Material from machine to Wip
        public ActionResult ReturnMaterialToWip()
        {
            return SetLanguage("~/Views/wipwms/ExportToMachine/ReturnMaterialToWip.cshtml");
        }

        [HttpGet]
        public async Task<ActionResult> GetReturn_ScanMLQR_WIP(string materialCode)
        {
            try
            {
                //Check input
                if (string.IsNullOrEmpty(materialCode))
                {
                    return Json(new { result = false, message = "Mã Nguyên vật liệu trống, Vui lòng Scan lại" }, JsonRequestBehavior.AllowGet);
                }
                materialCode = materialCode.Trim();
                //Kiểm tra mã materialCode có tồn tại ở kho NVL không
                var rs = await _IWIPServices.CheckNewMaterialInventoryProduct(materialCode);
                var IsWmaterial = rs.FirstOrDefault();
                //kiểm tra xem mã khác 002 là kho khác
                if (IsWmaterial != null)
                {
                    if (!IsWmaterial.location_code.StartsWith("002"))
                    {
                        var VITRI = await checkvitri(IsWmaterial.location_code);
                        return Json(new { result = false, message = "Mã Này Đang ở " + VITRI }, JsonRequestBehavior.AllowGet);
                    }

                    //kiểm tra mã này đã đưa lên kệ nào chưa
                    if (string.IsNullOrEmpty(IsWmaterial.ExportCode) && string.IsNullOrEmpty(IsWmaterial.LoctionMachine))
                    {
                        var VITRI = await checkvitri(IsWmaterial.location_code);

                        return Json(new { result = false, message = "Mã này đã được đang ở " + VITRI }, JsonRequestBehavior.AllowGet);
                    }
                    //kiểm tra mã này có trạng thái gì, nếu != 001 thì return
                    if (IsWmaterial.status != "001")
                    {
                        var VITRI = await checktrangthai(IsWmaterial.status);
                        return Json(new { result = false, message = "Mã Này Đang " + VITRI }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { result = true, message = "Thành công", data = IsWmaterial }, JsonRequestBehavior.AllowGet);
                }

                {
                    return Json(new { result = false, message = "NVL này chưa được nhập kho sản xuất!!!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Sửa thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }


        public async Task<ActionResult> UpdateReturnMaterialToMachine(string SelectRack, string ListId)
        {
            try
            {
                //Check input
                if (string.IsNullOrEmpty(SelectRack))
                {
                    return Json(new { result = false, message = "Vui lòng Chọn một kệ" }, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(ListId))
                {
                    return Json(new { result = false, message = "Mã Nguyên vật liệu trống, Vui lòng Scan lại" }, JsonRequestBehavior.AllowGet);
                }
                ListId = ListId.Trim();

                //INSERT MÃ BỊ TRẢ TỪ KHO MÁY VỀ LẠI KHO SẢN XUẤT
                await _IWIPServices.InsertReturnMaterialToWIP(ListId);

                //update kệ và máy cho mã nguyên vật liệu này
                var IsWmaterial = new InventoryProduct();
                IsWmaterial.location_code = SelectRack;
                IsWmaterial.ExportCode = "";
                IsWmaterial.LoctionMachine = "";
                IsWmaterial.ShippingToMachineDatetime = null;
                //IsWmaterial.recei_wip_date = DateTime.Now;
                IsWmaterial.change_date = DateTime.Now;
                IsWmaterial.change_id = Session["userid"] == null ? null : Session["userid"].ToString();

                //_IWIPService.UpdateReturnMaterialToWIP(IsWmaterial, ListId);
                int rs = await _IWIPServices.UpdateReturnMaterialToWIP(IsWmaterial, ListId);

                if (rs > 0)
                {
                    return Json(new { result = true, message = "Thành công!" }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { result = false, message = "Thất bại!" }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { result = true, message = "Thành công" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { result = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region ChangeRack
        public ActionResult ChangeRack()
        {
            return SetLanguage("~/Views/wipwms/ExportToMachine/ChangeRack.cshtml");
        }

        [HttpGet]
        public async Task<ActionResult> GetRack_ScanMLQR_WIP(string materialCode)
        {
            try
            {
                //Check input
                if (string.IsNullOrEmpty(materialCode))
                {
                    return Json(new { result = false, message = "Mã Nguyên vật liệu trống, Vui lòng Scan lại" }, JsonRequestBehavior.AllowGet);
                }
                materialCode = materialCode.Trim();
                //Kiểm tra mã materialCode có tồn tại ở kho NVL không
                //var IsWmaterial = _IWIPService.CheckWMaterialInfo(materialCode);
                var rs = await _IWIPServices.CheckNewMaterialInventoryProduct(materialCode);
                var IsWmaterial = rs.FirstOrDefault();

                //kiểm tra xem mã khác 002 là kho khác
                if (IsWmaterial != null)
                {
                    if (!IsWmaterial.location_code.StartsWith("002"))
                    {
                        var VITRI = await checkvitri(IsWmaterial.location_code);
                        return Json(new { result = false, message = "Mã Này Đang ở " + VITRI }, JsonRequestBehavior.AllowGet);
                    }
                    //kiểm tra mã này có trạng thái gì, nếu != 001 thì return
                    if (IsWmaterial.status != "001")
                    {
                        var VITRI = await checktrangthai(IsWmaterial.status);
                        return Json(new { result = false, message = "Mã Này Đang " + VITRI }, JsonRequestBehavior.AllowGet);
                    }
                    //kiểm tra mã này đã đưa lên kệ nào chưa
                    if (!string.IsNullOrEmpty(IsWmaterial.ExportCode) && !string.IsNullOrEmpty(IsWmaterial.LoctionMachine))
                    {
                        var VITRI = IsWmaterial.LoctionMachine;

                        return Json(new { result = false, message = "Mã này đã ở Máy: " + VITRI }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { result = true, message = "Thành công", data = IsWmaterial }, JsonRequestBehavior.AllowGet);
                }
                {
                    return Json(new { result = false, message = "NVL này chưa được nhập kho sản xuất!!!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Sửa thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateChangeRackMaterialToMachine(string SelectRack, string ListId)
        {
            try
            {
                //Check input
                if (string.IsNullOrEmpty(SelectRack))
                {
                    return Json(new { result = false, message = "Vui lòng Chọn một kệ" }, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(ListId))
                {
                    return Json(new { result = false, message = "Mã Nguyên vật liệu trống, Vui lòng Scan lại" }, JsonRequestBehavior.AllowGet);
                }
                ListId = ListId.Trim();


                //update kệ và máy cho mã nguyên vật liệu này
                var IsWmaterial = new InventoryProduct();

                IsWmaterial.location_code = SelectRack;
                //IsWmaterial.recei_wip_date = DateTime.Now;
                IsWmaterial.change_date = DateTime.Now;
                IsWmaterial.change_id = Session["userid"] == null ? null : Session["userid"].ToString();

                //_IWIPService.UpdateChangeRackMaterialToMachine(IsWmaterial, ListId);
                int rs = await _IWIPServices.UpdateChangeRackMaterialToMachine(IsWmaterial, ListId);

                if(rs > 0)
                {
                    return Json(new { result = true, message = "Thành công!" }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { result = false, message = "Thất bại!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { result = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


        protected override void Dispose(bool disposing)
        {
            //db.Dispose();
            base.Dispose(disposing);
        }

        #region general
        public ActionResult GenaralExportMaterial()
        {
            return SetLanguage("~/Views/wipwms/ExportToMachine/GenaralExportMaterial.cshtml");
        }

        [HttpGet]
        public async Task<JsonResult> getGeneral_Material(Pageing pageing)
        {
            if (Session["authorize"] != null)
            {
                DateTime? start = new DateTime();
                DateTime? end = new DateTime();
                string mt_no = Request["mt_no"] == null ? "" : Request["mt_no"].Trim();
                string mt_cd = Request["mt_cd"] == null ? "" : Request["mt_cd"].Trim();
                string mt_nm = Request["mt_nm"] == null ? "" : Request["mt_nm"].Trim();
                string product_cd = Request["product_cd"] == null ? "" : Request["product_cd"].Trim();
                string recevice_dt_start = Request["recevice_dt_start"] == null ? "" : Request["recevice_dt_start"].Trim();
                string recevice_dt_end = Request["recevice_dt_end"] == null ? "" : Request["recevice_dt_end"].Trim();
                string sts = Request["sts"] == null ? "" : Request["sts"].Trim();

                if (recevice_dt_start == "")
                {
                    start = null;
                }
                else
                {
                    start = DateTime.Parse(recevice_dt_start);
                }

                if (recevice_dt_end == "")
                {
                    end = null;
                }
                else
                {
                    end = DateTime.Parse(recevice_dt_end);
                }
                Dictionary<string, string> list = PagingAndOrderBy(pageing, " ORDER BY MyTable.mt_no DESC ");
                var data = await _IWIPServices.GetListGeneralExportToMachine(mt_no, product_cd, mt_nm, start, end, sts/*, lct_cd*/, mt_cd);
                int totalRecords = data.Count();
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)int.Parse(list["size"]));
                var result = new
                {
                    total = totalPages,
                    page = int.Parse(list["index"]),
                    records = totalRecords,
                    rows = data
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetgeneralDetail_List(string mt_no, string mt_cd, string mt_nm, string product_cd,/* string lct_cd,*/ string recevice_dt_start, string recevice_dt_end, string sts)
        {
            if (Session["authorize"] != null)
            {
                DateTime? start = new DateTime();
                DateTime? end = new DateTime();
                if (recevice_dt_start == "")
                {
                    start = null;
                }
                else
                {
                    start = DateTime.Parse(recevice_dt_start);
                }

                if (recevice_dt_end == "")
                {
                    end = null;
                }
                else
                {
                    end = DateTime.Parse(recevice_dt_end);
                }

                //if (lct_cd == "002001001000000000" || lct_cd == "002001002000000000" || lct_cd == "002001003000000000" || lct_cd == "002001004000000000" || lct_cd == "002001005000000000" ||
                //   lct_cd == "002001006000000000" || lct_cd == "002001007000000000" || lct_cd == "002001008000000000" || lct_cd == "002001009000000000")
                //{
                //    var res = lct_cd.Substring(0, 11);
                //    lct_cd = res;
                //}

                var result = await _IWIPServices.GetListGeneralMaterialDetail(mt_no, product_cd, mt_nm, start, end, sts/*, lct_cd*/, mt_cd);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }



        public async Task<ActionResult> PrintExcelFile(string mt_no, string mt_nm, string recevice_dt_start, string recevice_dt_end, string sts,string ProductCode)
        {


            string sts_search = "";
            switch (sts)
            {
                case "002":
                    sts_search = " AND DSD >0";
                    break;

                case "001,004":
                    sts_search = "AND CSD >0";
                    break;
            }
            string[] subs = sts.Split(' ');

            var dateConvert = new DateTime();

            if (DateTime.TryParse(recevice_dt_end, out dateConvert))
            {
                recevice_dt_end = dateConvert.ToString("yyyy/MM/dd");
            }
            if (DateTime.TryParse(recevice_dt_start, out dateConvert))
            {
                recevice_dt_start = dateConvert.ToString("yyyy/MM/dd");
            }
            // var listTotals = await _ITimsService.GetspTIMSInventoryGeneralUnion(mtCode, mtNo, mtName, recDate, "");
            var result = await _IWIPServices.ExportExcelGeneralMaterialDetail(mt_no,mt_nm, sts , recevice_dt_start, recevice_dt_end, ProductCode);
            //StringBuilder sql = new StringBuilder($"Call spTIMS_InventoryGeneral_Union('{mtCode}','{mtNo}','{mtName}','{recDate}');");
            List<WIP_ParentInventoryModelExport> listOrderBy = new List<WIP_ParentInventoryModelExport>();
            var listTotal = result.OrderBy(x => x.mt_no).ThenBy(x => x.mt_no).ToList();

            var values = listTotal.ToList().AsEnumerable().Select(x => new
            {
                product_cd = x.product_cd,
                mt_no = x.mt_no,
                mt_nm = x.mt_nm,
                ExportCode = x.ExportCode,
                material_code = x.material_code,

                qty = x.qty,
                DSD = x.DSD,
                CSD = x.CSD, /*((x.bundle_unit != null || x.bundle_unit != "") ? _IdevManagementService.GetDetailNameFromCom_DT("COM027", x.bundle_unit): null),*/
                returnMachine = x.returnMachine,
                SORTIlenghtNG = x.lenght,
                size = x.size,
                receipt_date = x.receipt_date,
                sd_no = x.sd_no,
                sts_nm = x.sts_nm,
            }).ToArray();

            String[] labelList = new string[14] { "Product Code", "MT No", "MT Name", "Export Code", "material code", "quantity", "Đang sử dụng", "Chưa sử dụng", "Trả về chưa xác nhận", "Tồn kho", "Size", "receipt date", "sd no", "status" };

            //String[] labelList = new string[7] { "PO", "Product Code", "Product Name", "Lot date", "Buyer QR", "Bobbin", "Quantity" };

            Response.ClearContent();

            Response.AddHeader("content-disposition", "attachment;filename=ExportToMachineGeneral.xls");

            Response.AddHeader("Content-Type", "application/vnd.ms-excel");

            new InitMethods().WriteHtmlTable(values, Response.Output, labelList);

            Response.End();

            return View("~/Views/wipwms/ExportToMachine/GenaralExportMaterial.cshtml");

        }
        #endregion
    }
}
