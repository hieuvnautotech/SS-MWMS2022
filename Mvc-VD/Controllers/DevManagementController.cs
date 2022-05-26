using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_VD.Models;
using System.Collections;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using PagedList;
using Mvc_VD.Extensions;
using System.ComponentModel;
using ClosedXML.Excel;
using Mvc_VD.Classes;
using Mvc_VD.Services;
using MySql.Data.MySqlClient;
using Mvc_VD.Services.Interface;
using System.Threading.Tasks;
using Mvc_VD.Models.WMS;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models.APIRequest;
using System.Web.Http;
using Mvc_VD.Models.Request;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPutAttribute = System.Web.Http.HttpPutAttribute;
using HttpDeleteAttribute = System.Web.Http.HttpDeleteAttribute;
using Mvc_VD.Models.Response;
using Mvc_VD.Services.Interface.MMS;
using Newtonsoft.Json;
using Mvc_VD.Models.WOModel;
using Mvc_VD.Commons;
using System.Globalization;

namespace Mvc_VD.Controllers
{
    public class DevManagementController : BaseController
    {
        private readonly IDMSService _IDomMS;
        private readonly IdevManagementService _IdevManagementService;
        private readonly IcommonService _IcommonService;
        private readonly ITimsService _ITimsService;
        private readonly ISupplierQRServiecs _ISupplierQRServiecs;
        private readonly IActualWOService _actualWOService;
        private readonly IhomeService _homeService;
        public DevManagementController(ITimsService ITimsService, IDbFactory dbFactory, IDMSService DomMS, IdevManagementService idevManagementService, IcommonService icommonService, ISupplierQRServiecs ISupplierQRServiecs, IActualWOService actualWOService, IhomeService ihomeService)
        {
            _IDomMS = DomMS;
            _IdevManagementService = idevManagementService;
            _IcommonService = icommonService;
            _ISupplierQRServiecs = ISupplierQRServiecs;
            _actualWOService = actualWOService;
            _ITimsService = ITimsService;
            _homeService = ihomeService;
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
        #region popupmachine

        public ActionResult PopupMachine()
        {
            return PartialView();
        }

        #endregion popupmachine

        #region Material
        public JsonResult add_month(string date, int month)
        {
            DateTime SearchDate = DateTime.Parse(date);
            var kq = SearchDate.AddMonths(month).ToString("yyyy-MM-dd");
            return Json(kq, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public async Task<ActionResult> getpopupsuplier(string suplier_no, string suplier_nm)
        {
            var data = await _IdevManagementService.GetListSupplierInfoForPopup(suplier_no, suplier_nm);
            foreach (var item in data)
            {
                var comDetail = await _IdevManagementService.GetListComDetailForPopup(item.bsn_tp);
                item.bsn_tp = ((comDetail != null) ? comDetail.dt_nm : string.Empty);
            }
            var jsondatacomdt = new
            {
                rows = data
            };
            return Json(jsondatacomdt, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetPopupManu()
        {
            var rs = await _IdevManagementService.GetListManuFacInfoForPopup();
            var list = rs.ToList();
            return Json(new { rows = list }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Material()
        {
            return SetLanguage("");
        }

        [HttpGet]
        public async Task<ActionResult> Getpp_qc_type_mt()
        {
            var list = await _IdevManagementService.GetListItemMaterial("MT", "N");
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetType_Marterial(comm_mt comm_dt)
        {
            var type = await _IdevManagementService.GetListCommDT("COM004");
            return Json(type, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetWidth_Marterial(comm_mt comm_dt)
        {
            var width = await _IdevManagementService.GetListCommDT("COM005");
            return Json(width, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetSpec_Marterial(comm_mt comm_dt)
        {
            var spec = await _IdevManagementService.GetListCommDT("COM006");
            return Json(spec, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetPrice_Marterial(comm_mt comm_dt)
        {
            var price = await _IdevManagementService.GetListCommDT("COM002");
            return Json(price, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetArea_Marterial(comm_mt comm_dt)
        {
            var price = await _IdevManagementService.GetListCommDT("COM011");
            return Json(price, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetStickness(comm_mt comm_dt)
        {
            var price = await _IdevManagementService.GetListCommDT("COM028");
            return Json(price, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Getprice_least_unit(comm_mt comm_dt)
        {
            var price = await _IdevManagementService.GetListCommDT("COM029");
            return Json(price, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetQCRange_Marterial(comm_mt comm_dt)
        {
            var qc = await _IdevManagementService.GetListCommDT("COM017", "Y");
            return Json(qc, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Get_Getbundle(comm_mt comm_dt)
        {
            var qc = await _IdevManagementService.GetListCommDT("COM027", "Y");
            return Json(qc, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetQCRange(comm_mt comm_dt)
        {
            var QCRange = await _IdevManagementService.GetListCommDT("COM017");
            return Json(QCRange, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> getQC_gs(qc_item_mt qc_item_mt)
        {
            var Qc_gs = await _IdevManagementService.GetListQCItemMaterial("GS", "N");
            return Json(Qc_gs, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetUnit_qty(comm_mt comm_dt)
        {
            var qc = await _IdevManagementService.GetListCommDT("COM025", "Y");
            return Json(qc, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public async Task<JsonResult> deleteMaterial(int id)
        {
            if (!ModelState.IsValid) {
                return null;
        }
                string user = Session["userid"] == null ? null : Session["userid"].ToString();
                if (string.IsNullOrEmpty(user))
                {
                    return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
                }
                var material_info = await _IdevManagementService.GetDMaterialInfoById(id);
            var check = await _IdevManagementService.CheckExistMaterialInBom(material_info.mt_no);
            if (check > 0)
            {
                return Json(new { result = false,  message = "NVL này đã tồn tại ở trong BOM rồi!" }, JsonRequestBehavior.AllowGet);
            }
            //liệu chính
            int isCheckRouting = await _IdevManagementService.CheckIsExistMaterialRouting(material_info.mt_no);
             if (isCheckRouting > 0)
            {
                return Json(new { result = false, message = "Liệu này đã được thêm vào Routing" }, JsonRequestBehavior.AllowGet);
            }
            //liệu thay thế
            int isCheckRouting2 = await _IdevManagementService.CheckIsExistMaterialRouting2(material_info.mt_no);
            if(isCheckRouting2 > 0)
            {
                return Json(new { result = false, message = "Liệu này đã được thêm vào Routing" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                int result = await _IdevManagementService.DeleteMaterialInfo(material_info.mtid);
                return Json(new { result = true, message = "Xóa dữ liệu thành công !" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public async Task<ActionResult> insertmaterial(MaterialInfo d_material_info, string sts_create)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return null;
                }
                string user = Session["userid"] == null ? null : Session["userid"].ToString();
                if (string.IsNullOrEmpty(user))
                {
                    return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
                }
                if (d_material_info.mt_type == "PMT")
                {
                    d_material_info.mt_no = "C" + d_material_info.mt_no + "-" + d_material_info.width;
                }
                var menuCd1 = string.Empty;
                var sublog1 = new MaterialInfo();
                var rs = await _IdevManagementService.GetListDataMaterialNoFromMaterialInfo(d_material_info.mt_no);
                var list1 = rs.ToList();
                if (list1.Count == 0)
                {
                    {
                        //var profile = Request.Files;
                        //string imgname = string.Empty;
                        //string ImageName = string.Empty;

                        //if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                        //{
                        //    var logo1 = System.Web.HttpContext.Current.Request.Files["file"];
                        //    if (logo1.ContentLength > 0)
                        //    {
                        //        var profileName = Path.GetFileName(logo1.FileName);
                        //        var ext = Path.GetExtension(logo1.FileName);

                        //        ImageName = profileName;
                        //        var comPath = Server.MapPath("~/Images/MarterialImg/") + ImageName;

                        //        logo1.SaveAs(comPath);
                        //        d_material_info.photo_file = ImageName;
                        //    }
                        //}

                        d_material_info.reg_dt = DateTime.Now;
                        d_material_info.chg_dt = DateTime.Now;
                        d_material_info.del_yn = "N";
                        d_material_info.use_yn = "Y";
                        d_material_info.reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                        d_material_info.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();

                        int new_mtid = await _IdevManagementService.InsertDMaterialInfo(d_material_info);
                        var item = await _IdevManagementService.GetDMaterialInfoById(new_mtid);
                        var result = await _IdevManagementService.GetListDMaterialInfo(item.mt_type, item.mt_nm, item.mt_no, null, null, item.sp_cd);
                        var rs1 = result.ToList();
                        return Json(new { result = true, message = "Thêm nguyên vật mới thành công !", kq = rs1, type = item.mt_type }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { result = false, type = "", message = "MT Code  already exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPut]
        [ValidateInput(false)]
        //public async Task<ActionResult> updatematerial(int mtid, int gr_qty, string barcode, string mt_type, string mt_no, string mt_nm, string width, string width_unit, string spec, string spec_unit, string price, string price_unit, string photo_file, string re_mark, string sp_cd, string mf_cd, string file, string area, string area_unit, string s_lot_no, string item_vcd, string qc_range_cd, string unit_cd, string bundle_unit, string mt_no_origin, string stick, string stick_unit, string tot_price, string price_least_unit, string mt_cd, string thick, string thick_unit, string m_consumable, int bundle_qty)
        public async Task<ActionResult> updatematerial(d_material_info item)
        {
            if (!ModelState.IsValid) {
                return null;
            }
            string user = Session["userid"] == null ? null : Session["userid"].ToString();
                if (string.IsNullOrEmpty(user))
                {
                    return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
                }
                var materialInfo = await _IdevManagementService.GetDMaterialInfoById(item.mtid);
            if (materialInfo == null)
            {
                return Json(new { result = false, message = "Không tìm thấy tên liệu để sửa" }, JsonRequestBehavior.AllowGet);
            }
            materialInfo.mt_nm = item.mt_nm;
            materialInfo.bundle_qty = item.bundle_qty == null ? 0 : int.Parse(item.bundle_qty);
            materialInfo.bundle_unit = item.bundle_unit;
            materialInfo.width_unit = item.width_unit;
            materialInfo.spec = item.spec;
            materialInfo.spec_unit = item.spec_unit;
            materialInfo.spec_unit = item.spec_unit;
            materialInfo.barcode = item.barcode;
            materialInfo.sp_cd = item.sp_cd;
            materialInfo.unit_cd = item.unit_cd;
            materialInfo.chg_id = user;

            await _IdevManagementService.UpdateDMaterialInfo(materialInfo);
            //var item1 = await _IdevManagementService.GetDMaterialInfoById(materialInfo.mtid);
            var result = await _IdevManagementService.GetListDMaterialInfo(item.mt_type, item.mt_nm, item.mt_no, null, null, item.sp_cd);
            var rs = result.FirstOrDefault();
            return Json(new { result = true, message = "Sửa thành công", data = rs }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> searchmaterial()
        {

            var name = Request["nameData"];
            var type = Request["typeData"];
            var code = Request["codeData"];
            var start = Request.QueryString["start1Data"];
            var end = Request.QueryString["end1Data"];
            var dateConvert = new DateTime();
            if (DateTime.TryParse(end, out dateConvert))
            {
                end = dateConvert.ToString("yyyy/MM/dd");
            }

            if (DateTime.TryParse(start, out dateConvert))
            {
                start = dateConvert.ToString("yyyy/MM/dd");
            }

            var data = await _IdevManagementService.SearchMaterialInfo(name, type, code, start, end);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> searchmaterial_2(Pageing pageing, string type, string name, string code, DateTime? start, DateTime? end, string sp)
        {
            var rs = await _IdevManagementService.SearchMaterial(type, name, code, start, end, sp);
            int totalRecords = rs.Count();
            int totalPages = (int)Math.Ceiling((float)totalRecords / pageing.rows);
            IEnumerable<DMaterialResponse> dataactual = rs.Skip<DMaterialResponse>((pageing.page - 1) * pageing.rows).Take(pageing.rows);
            var result = new
            {
                total = totalPages,
                page = pageing.page,
                records = totalRecords,
                rows = dataactual
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> searchmaterial_replace(Pageing pageing, string type, string name, string code, DateTime? start, DateTime? end, string sp, string MaterialPrarent)
        {
            var rs = await _IdevManagementService.SearchMaterialRepalce(type, name, code, start, end, sp, MaterialPrarent);
            int totalRecords = rs.Count();
            int totalPages = (int)Math.Ceiling((float)totalRecords / pageing.rows);
            IEnumerable<DMaterialResponse> dataactual = rs.Skip<DMaterialResponse>((pageing.page - 1) * pageing.rows).Take(pageing.rows);
            var result = new
            {
                total = totalPages,
                page = pageing.page,
                records = totalRecords,
                rows = dataactual
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //duy write function new
        [HttpPost]
        public async Task<JsonResult> GetlistMaterialInfo([FromBody] MaterialInforRequest item)
        {
            if(item.code != null)
            {
                if (item.code.Contains('['))
                {
                    item.code = item.code.Replace('[', '%');
                }
                if (item.code.Contains(']'))
                {
                    item.code = item.code.Replace(']', '%');
                }
            }

            var materiallinfo = await _IdevManagementService.GetListDMaterialInfo(item.type, item.name, item.code, item.start, item.end, item.sp);
            int totalRecords = materiallinfo.Count();
            int totalPages = (int)Math.Ceiling((float)totalRecords / item.rows);
            IEnumerable<MaterialInfo> dataactual = materiallinfo.Skip<MaterialInfo>((item.page - 1) * item.rows).Take(item.rows);
            var result = new
            {
                total = totalPages,
                page = item.page,
                records = totalRecords,
                rows = dataactual
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> insertMaterialTemp(MaterialInfo d_material_info, List<ModelInsertMaterialTemp> insertmaterial_tmp)
        {
            var data_update = 0;
            var data_create = 0;
            var data_error = 0;
            foreach (var item in insertmaterial_tmp)
            {
                if (item.MT_No == null)
                {
                    data_error++;
                }
                else
                {
                    var mt_no = item.MT_No.ToString().Trim();
                    if (item.Type == "Cutting Material")
                    {
                        mt_no = "C" + mt_no + "-" + item.Width;
                    }
                    var mt_type = "";
                    switch (item.Type.ToString().Trim())
                    {
                        case "Cutting Material":
                            mt_type = "PMT";
                            break;

                        case "Main-Sub Material":
                            mt_type = "MMT";
                            break;

                        case "Adhesive Materials":
                            mt_type = "AM";
                            break;

                        default:
                            mt_type = "CMT";
                            break;
                    }

                    //Int64 list1 = db.d_material_info.Count(x => x.mt_no == mt_no);
                    int list1 = await _IdevManagementService.CountDMaterialInfo(mt_no);
                    try
                    {
                        var checkListSupplierInfo = await _IdevManagementService.GetListSupplierInfo(item.sp_cd);
                        //if (!db.supplier_info.Any(x => x.sp_cd == item.sp_cd))
                        if (checkListSupplierInfo.Count() > 0)
                        {
                            data_error++;
                        }
                        else
                        {
                            if (list1 == 0)
                            {
                                d_material_info.del_yn = "N";
                                d_material_info.barcode = item.Barcode;
                                d_material_info.use_yn = "Y";
                                d_material_info.mt_type = mt_type;
                                d_material_info.mt_no = mt_no;
                                d_material_info.bundle_qty = Int32.Parse(item.bundle_qty);
                                d_material_info.mt_nm = item.Name;
                                d_material_info.sp_cd = item.sp_cd;
                                d_material_info.bundle_unit = item.bundle_unit;
                                d_material_info.width = item.Width;
                                d_material_info.width_unit = "mm";
                                d_material_info.spec = item.Length;
                                d_material_info.spec_unit = "M";
                                d_material_info.chg_dt = DateTime.Now;
                                d_material_info.reg_dt = DateTime.Now;
                                d_material_info.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                                d_material_info.reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                                //lenghth để có thể save được
                                d_material_info.consum_yn = "N";
                                d_material_info.thick_unit = "µm";
                                d_material_info.stick_unit = "g";

                                int mtid = await _IdevManagementService.InsertDMaterialInfo(d_material_info);
                            }
                            else
                            {
                                data_update++;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        data_error++;
                    }
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


        public async Task<ActionResult> ExportToExcel(string type, string name, string code, string start, string end, string sp)
        {
            // Get Data from ajax function
            //var grid = new GridView();
            var dateConvert = new DateTime();

            if (DateTime.TryParse(end, out dateConvert))
            {
                end = dateConvert.ToString("yyyy/MM/dd");
            }

            if (DateTime.TryParse(start, out dateConvert))
            {
                start = dateConvert.ToString("yyyy/MM/dd");
            }

            //string sql = "SELECT * FROM d_material_info AS a "
            //    + "WHERE a.del_yn='N'  "
            //    + "AND (a.mt_nm LIKE '%" + name + "%') "
            //    + "AND (a.mt_type LIKE '%" + type + "%') "
            //    + "AND (a.mt_no LIKE '%" + code + "%') "
            //    + "AND (a.sp_cd LIKE '%" + sp + "%') "
            //    + "AND ('" + start + "'='' OR DATE_FORMAT(a.reg_dt,'%Y/%m/%d') >= DATE_FORMAT('" + start + "','%Y/%m/%d')) "
            //    + "AND ('" + end + "'='' OR DATE_FORMAT(a.reg_dt,'%Y/%m/%d') <= DATE_FORMAT('" + end + "','%Y/%m/%d')) "
            //    + " ORDER BY a.mt_no DESC "
            //    ;

            var data = await _IdevManagementService.GetDataMaterialToExport(name, type, code, sp, start, end);


            var values = data.ToList().AsEnumerable().Select( x => new
            {
                ID = x.mtid,
                Type = x.mt_type,
                Barcode = x.barcode,
                MT_NO = x.mt_no,
                Name = x.mt_nm,
                Unit = x.unit_cd,
                Bundle = x.bundle_qty,
                Bundle_Unit = x.bundle_unit, /*((x.bundle_unit != null || x.bundle_unit != "") ? _IdevManagementService.GetDetailNameFromCom_DT("COM027", x.bundle_unit): null),*/
                Supplier = x.sp_cd,
                Manufacturer = x.mf_cd,
                Width = ((x.width != null) ? x.width : string.Empty),
                Length = ((x.spec != null) ? x.spec : string.Empty),
                Area = ((x.area != null) ? x.area : string.Empty),
                area_unit = x.area_unit,
                Photo = x.photo_file,
                Create_Date = x.reg_dt,
                Create_User = x.reg_id,
                Change_Name = x.chg_id,
                Change_Date = x.chg_dt
            }).ToArray();

            String[] labelList = new string[19] { "ID", "Type", "Barcode", "MT NO", "Name", "Unit", "Bundle Qty", "Bundle Unit", "Supplier", "Manufacturer", "Width (mm)", "Length (M)", "Area(m²)", "area unit", "Price", "Create Date", "Create User", "Change Name", "Change Date" };

            Response.ClearContent();

            Response.AddHeader("content-disposition", "attachment;filename=MaterialManagement.xls");

            Response.AddHeader("Content-Type", "application/vnd.ms-excel");

            WriteHtmlTable(values, Response.Output, labelList);

            Response.End();

            return View("Material");
        }

        #endregion Material

        #region lot in material

        [HttpPost]
        public async Task<ActionResult> update_gr_wmt_select(UpdateQuantityMaterial model)
        {
            StringBuilder idsList = new StringBuilder();
            //if (model.listId != null)
            //{
            //    foreach (var item in model.listId)
            //    {
            //        idsList.Append($"{item},");

            //    }
            //}
            //string lst = new InitMethods().RemoveLastComma(idsList);
            //string[] result = id.Split(new char[] { ',' });
            //var listId = result.ToList();
            //if (gr_qty == 0 || listId.Count < 1)
            //{
            //    return Json(new { result = false, message = "Lỗi hệ thống!!" }, JsonRequestBehavior.AllowGet);
            //}
            var html = "'" + "";
            for (int i = 0; i < model.listId.Count(); i++)
            {
                html += model.listId[i];

                if (i != model.listId.Count() - 1)
                {
                    html += "'" + ',' + "'";
                }
            }
            html = html + "'";
            var CheckbarCode = await _IdevManagementService.CheckBarCode(model.lot_mt_no);
            var chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
            int update = await _IdevManagementService.UpdateQtyWMaterailTam((int)model.gr_qty, (int)model.gr_qty, chg_id, DateTime.Now, html);
            if(CheckbarCode.barcode == "N")
            {
                return Json(new { result = true, message = Constant.Success }, JsonRequestBehavior.AllowGet);
            }
            else {
            if (update > 0) return Json(new { result = true, message = Constant.Success }, JsonRequestBehavior.AllowGet);
            else return Json(new { result = false, message = "Cập nhập sản lượng thất bại!" }, JsonRequestBehavior.AllowGet);
            }
        }
        private bool CheckDatetimeFormat(string value)
        {
            try
            {
                string strValue = value as string;
                if (!string.IsNullOrEmpty(strValue))
                {
                    DateTime aa = DateTime.Parse(strValue);
                    string retur = aa.ToString("yyyy-MM-dd HH:mm:ss.fff");
                }

                return true;
            }
            catch (System.Exception ex)
            {

                return false;
            }
        }

        [HttpPost]
        public async Task<ActionResult> pl_insertw_material(string mt_no, string barcode, double gr_qty, string expore_dt, string dt_of_receipt, string exp_input_dt, string lot_no, int send_qty, string mt_type, int bundle_qty, string bundle_unit)
        {

            if (!ModelState.IsValid)
            {
                return null;
            }
            try
            {
                string user = Session["userid"] == null ? null : Session["userid"].ToString();
                if (string.IsNullOrEmpty(user))
                {
                    return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
                }
                var userID = (Session["userid"] == null ? null : Session["userid"].ToString());
                if (mt_type == "CMT")
                {
                    return Json(new { result = false, kq = "Chỉ liệu cắt mới được in tem. vui lòng chọn loại cắt" }, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(mt_type))
                {
                    return Json(new { result = false, kq = "MT Type rỗng vui lòng chọn lại tên liệu" }, JsonRequestBehavior.AllowGet);
                }
                //bool chkdt = true;
                //chkdt = CheckDatetimeFormat(expore_dt);
                //if (!chkdt)
                //{
                //    return Json(new { result = false, message = "Export Date không đúng vui lòng kiểm tra lại!" }, JsonRequestBehavior.AllowGet);
                //}
                //chkdt = CheckDatetimeFormat(exp_input_dt);
                //if (!chkdt)
                //{
                //    return Json(new { result = false, message = "Expiry Date không đúng vui lòng kiểm tra lại!" }, JsonRequestBehavior.AllowGet);
                //}

                DateTime FormatDate1;
                bool chValidityexp_input_dt = DateTime.TryParseExact(
                      exp_input_dt,
                       "yyyy-MM-dd",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out FormatDate1);

                if (chValidityexp_input_dt == false || FormatDate1.Year < 2015)
                {
                    return Json(new { result = false, kq = "Chọn ngày không có thực, vui lòng chọn lại Expiry Date" }, JsonRequestBehavior.AllowGet);
                }

                DateTime FormatDate2;
                bool chValidityexp_expore_dt = DateTime.TryParseExact(
                      expore_dt,
                       "yyyy-MM-dd",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out FormatDate2);

                if (chValidityexp_expore_dt == false || FormatDate2.Year < 2015)
                {
                    return Json(new { result = false, kq = "Chọn ngày không có thực, vui lòng chọn lại Export Date" }, JsonRequestBehavior.AllowGet);
                }
                DateTime FormatDate3;
                bool chValiditydt_of_receipt = DateTime.TryParseExact(
                      dt_of_receipt,
                       "yyyy-MM-dd",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out FormatDate3);

                if (chValiditydt_of_receipt == false || FormatDate3.Year < 2015)
                {
                    return Json(new { result = false, kq = "Chọn ngày không có thực, vui lòng chọn lại Date of Receive" }, JsonRequestBehavior.AllowGet);
                }

                DateTime dt1 = DateTime.Now; //Today at 00:00:00

                string time_now = dt1.ToString("HHmmss");
                string date = dt1.ToString("yyyyMMdd").Remove(0, 2);
                StringBuilder varname1 = new StringBuilder();

                gr_qty = (bundle_unit == "EA" ? bundle_qty : gr_qty);

                MaterialInfoTam tmwmt = new MaterialInfoTam();
                int kqins = 0;
                List<MaterialInfoTam> lst = new List<MaterialInfoTam>();
                for (int i = 0; i < send_qty; i++)
                {
                    tmwmt = new MaterialInfoTam();
                    tmwmt.mt_no = mt_no;
                    tmwmt.gr_qty = gr_qty;
                    tmwmt.real_qty = gr_qty;
                    tmwmt.mt_type = mt_type;
                    tmwmt.expore_dt = expore_dt;
                    tmwmt.expiry_dt = exp_input_dt;
                    tmwmt.dt_of_receipt = dt_of_receipt;
                    tmwmt.lot_no = lot_no;
                    tmwmt.reg_id = userID;
                    tmwmt.reg_dt = dt1;
                    tmwmt.chg_dt = dt1;
                    tmwmt.chg_id = userID;
                    tmwmt.date = date;
                    tmwmt.status = "000";
                    tmwmt.use_yn = "Y";
                    tmwmt.lot_no = lot_no;

                    if (barcode == "Y")
                    {

                        tmwmt.mt_cd = mt_no + "-" + date + time_now + String.Format("{0, 0:D3}", i + 1);
                        tmwmt.mt_barcode = mt_no + "-" + date + time_now + String.Format("{0, 0:D3}", i + 1);
                        tmwmt.mt_qrcode = mt_no + "-" + date + time_now + String.Format("{0, 0:D3}", i + 1);
                        lst.Add(tmwmt);

                    }

                    else
                    {
                        tmwmt.mt_barcode = "";
                        tmwmt.mt_qrcode = "";
                        tmwmt.mt_cd = mt_no + "-" + date + time_now + String.Format("{0, 0:D3}", i + 1);
                        lst.Add(tmwmt);
                    }
                }
                List<string> termsList = new List<string>();
                var html = "'" + "";
                for (int i = 0; i < lst.Count(); i++)
                {
                    html += lst[i].mt_cd;

                    if (i != lst.Count() - 1)
                    {
                        html += "'" + ',' + "'";
                    }
                }
                html = html + "'";
                //check checkDuplicateCode
                var checkDuplicateCode = await _IdevManagementService.GetListMaterialInfoTam(html);
              //  var checkDuplicateCode = lst.Where(x => listMaterialInfoTamData.Contains(x.mt_cd));
                if (checkDuplicateCode.Count() > 0)
                {
                    return Json(new { result = false, kq = "Đã tồn tại mã " + checkDuplicateCode.FirstOrDefault() + " này ở trong hệ thống !" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    kqins = await _IdevManagementService.InsertWMaterialInfoTmp(lst);

                }

                List<WMaterialTamReponse> listData = new List<WMaterialTamReponse>();
                string material_code = mt_no + "-" + date + time_now;
                var data_return = await _IdevManagementService.GetListDataMaterialInfoTamByMt_Cd(html);
                if(data_return.Count() > 0)
                {
                    listData.AddRange(data_return);
                    return Json(new { result = true, data = listData, message = "Tạo mã thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, kq = "Không thể lưu trữ mã NVL này!" }, JsonRequestBehavior.AllowGet);
                }

                //if (kqins > 0)
                //{
                //    for (int i = 0; i < send_qty; i++)
                //    {
                //        WMaterialTamReponse tmp = new WMaterialTamReponse();
                //        //tmp.wmtid = kqins;
                //        //tmp.seq = i;
                //        tmp.mt_cd = mt_no + "-" + date + time_now + String.Format("{0, 0:D3}", i + 1);
                //        //tmp.wmtid = tmp.mt_cd;
                //        tmp.lot_no = lot_no;
                //        tmp.gr_qty = string.Format("{0:0.##}", gr_qty);
                //        tmp.expiry_dt = exp_input_dt.Replace("-", "");
                //        tmp.dt_of_receipt = dt_of_receipt.Replace("-", "");
                //        tmp.expore_dt = expore_dt.Replace("-", "");
                //        tmp.wmtid = list[i];
                //        listData.Add(tmp);
                //    }
                //    return Json(new { result = true, data = listData, message = "Tạo mã thành công" }, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                //    return Json(new { result = false, message = "Không thể lưu trữ mã NVL này!" }, JsonRequestBehavior.AllowGet);
                //}
            }
            catch (Exception e)
            {
                return Json(new { result = false, kq = "Can not Create" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> PrintQRSupplier()
        {
            string[] keys = Request.Form.AllKeys;
            ViewData["Message"] = Request.Form[keys[0]];
            var a  = Request.Form[keys[0]];
            var id_mt = Request.Form[keys[5]];
            var id = Convert.ToInt32(id_mt);
            var find = await _IdevManagementService.GetDMaterialInfoById(id);
            ViewData["mt_cd"] = find.mt_cd;
            ViewData["mt_nm"] = find.mt_nm;
            ViewData["barcode"] = find.barcode;
            ViewData["width"] = Request.Form[keys[1]];
            ViewData["spec"] = Request.Form[keys[3]];
            ViewData["bundle_qty"] = Request.Form[keys[2]];
            ViewData["bundle_unit"] = Request.Form[keys[4]];
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> QRbarcodeSupplie(string wmtid)
        {
            string[] result = wmtid.Split(new char[] { ',' });
            var ListId = result.ToList();
            var data = await _ISupplierQRServiecs.GetListMaterialInfoTam(ListId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion lot in material

        #region MachineMgt

        public ActionResult MachineMgt()
        {
            return SetLanguage("");
        }

        [HttpGet]
        public async Task<ActionResult> getMachineMgtData(string mc_no)
        {
            var machineData = await _IdevManagementService.GetListMchine();
            return Json(machineData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> insertMachineMgt(MachineInfo machineInfo, string mc_type, string mc_no, string purpose, string mc_nm, string re_mark)
        {
            int count = await _IdevManagementService.countMachineInfo(mc_no);
            if (count > 0)
            {
                return Json(new { result = false, message = Constant.DataExist }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                machineInfo.mc_type = mc_type;
                machineInfo.mc_no = mc_no;
                machineInfo.purpose = purpose;
                machineInfo.mc_nm = mc_nm;
                machineInfo.re_mark = re_mark;
                machineInfo.reg_dt = DateTime.Now;
                machineInfo.chg_dt = DateTime.Now;
                machineInfo.barcode = mc_no;
                machineInfo.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                machineInfo.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();

                var MachineId = await _IdevManagementService.InsertIntoMachineInfo(machineInfo);
                var machine = await _IdevManagementService.checkMachineInfo(MachineId);
                return Json(new { result = true, message = Constant.Success, kq = machine }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPut]
        public async Task<ActionResult> updatetMachineMgt(int mno, string mc_type, string mc_no, string purpose, string mc_nm, string re_mark)
        {
            try
            {
                var machineInfo = await _IdevManagementService.checkMachineInfo(mno);

                if (machineInfo == null)
                {
                    return Json(new { result = false, message = "Không tìm thấy máy " + machineInfo.mc_nm + " trong hệ thống!" }, JsonRequestBehavior.AllowGet);
                }
                machineInfo.mc_type = mc_type;
                machineInfo.mc_no = mc_no;
                machineInfo.purpose = purpose;
                machineInfo.mc_nm = mc_nm;
                machineInfo.re_mark = re_mark;
                machineInfo.chg_dt = DateTime.Now;
                machineInfo.reg_dt = DateTime.Now;
                machineInfo.barcode = mc_no;
                machineInfo.chg_id = Session["userid"] == null ? "" : Session["userid"].ToString();
                machineInfo.reg_id = Session["userid"] == null ? "" : Session["userid"].ToString();

                await _IdevManagementService.UpdateMachineInfo(machineInfo);
                return Json(new { result = true, message = "Cập nhập dữ liệu thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        public async Task<JsonResult> deleteMachineMgt(int mno)
        {
            var machineInfo = await _IdevManagementService.checkMachineInfo(mno);

            if (machineInfo == null)
            {
                return Json(new { result = false, message = "Không tìm thấy máy " + machineInfo.mc_nm + " trong hệ thống" }, JsonRequestBehavior.AllowGet);
            }

            await _IdevManagementService.DeleteMachineInfo(machineInfo.mno);
            return Json(new { result = true, message = "Xóa dữ liệu thành công !" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> searchMachineMgt(string mc_type, string mc_no, string mc_nm, DateTime? start, DateTime? end)
        {
            var result = await _IdevManagementService.SearchMachineInfo(mc_type, mc_no, mc_nm, start, end);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> getType_MachineMgt()
        {
            var type = await _IdevManagementService.GetListCommDT();
            return Json(type, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _PrintQRMachine(string id)
        {
            ViewData["Message"] = id;
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> PrintMultiMachineQRCode(string id)
        {
            var multiIDs = id.TrimStart('[').TrimEnd(']').Split(',');
            var row_data = new List<MachineInfo>();
            for (int i = 0; i < multiIDs.Length; i++)
            {
                var id2 = int.Parse(multiIDs[i]);
                var data = await _IdevManagementService.getListMachineInfoById(id2);
                row_data.Add(data);
            }
            return Json(row_data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> insertMachineExcel(List<ModelInsertMachineExcel> ModelInsertMachineExcel)
        {
            var data_update = 0;
            var data_create = 0;
            var data_error = 0;

            var list = new ArrayList();
            var machineInfo = new MachineInfo();

            if ((ModelInsertMachineExcel == null) && (ModelInsertMachineExcel?.Any() != true))
            {
                return Json(new { result = false, message = "Không thế lấy giữ liệu từ tệp excel hoặc tệp tải lên không có giữ liệu" }, JsonRequestBehavior.AllowGet);
            }

            if (ModelInsertMachineExcel.Count == 0)
            {
                return Json(new { result = false, message = "Không thế lấy giữ liệu từ tệp excel hoặc tệp tải lên không có giữ liệu" }, JsonRequestBehavior.AllowGet);
            };

            foreach (var item in ModelInsertMachineExcel)
            {
                //var style_no = item.style_no.ToString();
                var mc_type = string.IsNullOrEmpty(item.mc_type) ? "" : item.mc_type.ToString();
                var mc_no = item.mc_no;

                if (string.IsNullOrEmpty(item.mc_no))
                {
                    data_error++;
                    continue;
                }

                var mc_nm = string.IsNullOrEmpty(item.mc_nm) ? "" : item.mc_nm.ToString();
                var purpose = string.IsNullOrEmpty(item.purpose) ? "" : item.purpose.ToString();

                //6,20,50,200
                var countList = await _IdevManagementService.countMachineInfo(mc_no);
                try
                {
                    if (countList == 0)
                    {
                        machineInfo.mc_type = mc_type.Replace("\"", "") + "";
                        machineInfo.mc_no = mc_no.Replace("\"", "") + "";
                        machineInfo.mc_nm = mc_nm.Replace("\"", "") + "";
                        machineInfo.purpose = purpose.Replace("\"", "") + "";
                        machineInfo.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                        machineInfo.reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();

                        var result = await _IdevManagementService.InsertIntoMachineInfo(machineInfo);

                        if (result > 0)
                        {
                            data_create++;
                            var list2 = await _IdevManagementService.GetListMachineExcel(mc_no);
                            list.Add(list2);
                        }
                    }
                    else
                    {
                        data_update++;
                    }
                }
                catch (Exception)
                {
                    data_error++;
                }
            }
            return Json(new
            {
                result = true,
                message = "Thành Công!",
                data_update = data_update,
                data_create = data_create,
                data = list,
                data_error = data_error
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion MachineMgt

        #region Mold

        public ActionResult MoldMgt()
        {
            return SetLanguage("");
        }
        [HttpPost]
        public async Task<ActionResult> deleteMold(int mdno)
        {
            try
            {
                var data = await _IdevManagementService.DelateMoldInfo(mdno);
                return Json(new { result = true, data = mdno, message = "Thành Công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new { result = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion Mold

        #region Develop Common

        public ActionResult DevelopCommon()
        {
            return SetLanguage("");
        }

        [HttpGet]
        public async Task<ActionResult> getDevelopCommon()
        {
            var result = await _IdevManagementService.GetListCommMT();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> getDevCommDtData(string mt_cd)
        {
            var result = await _IdevManagementService.GetListDevelopCommonData(mt_cd);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> insertDevCommDt(CommCode comm_dt, string dt_cd, string dt_nm, string dt_exp, string use_yn, string mt_cd, string dt_order)
        {
            try
            {
                string user = Session["userid"] == null ? null : Session["userid"].ToString();
                if (string.IsNullOrEmpty(user))
                {
                    return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
                }
                int count = await _IdevManagementService.CheckComDT(dt_cd, mt_cd);
                if (count > 0)
                {
                    return Json(new { result = false, message = Constant.DataExist }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    int detailOrder = Convert.ToInt32(dt_order);

                    comm_dt.mt_cd = mt_cd;
                    comm_dt.dt_cd = dt_cd.Replace(" ", string.Empty);
                    comm_dt.dt_nm = dt_nm;
                    comm_dt.dt_exp = dt_exp;
                    comm_dt.dt_order = detailOrder;
                    comm_dt.use_yn = use_yn;
                    comm_dt.reg_dt = DateTime.Now;
                    comm_dt.chg_dt = DateTime.Now;
                    comm_dt.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    comm_dt.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    comm_dt.del_yn = "N";

                    var cdid = await _IdevManagementService.InsertIntoComDT(comm_dt);
                    var rs = await _IdevManagementService.GetCommCodeById(cdid);
                    return Json(new { result = true, message = Constant.Success, value = rs }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<JsonResult> insertDevelopCommonMT(CommMt comm_mt, string mt_cd, string mt_nm, string mt_exp, string use_yn)
        {
            try
            {
                string user = Session["userid"] == null ? null : Session["userid"].ToString();
                if (string.IsNullOrEmpty(user))
                {
                    return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
                }
                var comm_mt_list = await _IdevManagementService.GetListCommMT();
                List<CommMt> row_data = new List<CommMt>();
                int id_data = 0;
                foreach (var item in comm_mt_list)
                {
                    var row_data_iteam = new CommMt()
                    {
                        //id = Convert.ToString(id_data),
                        mt_cd = item.mt_cd,
                        mt_nm = item.mt_nm,
                        mt_exp = item.mt_exp,
                        use_yn = item.use_yn,
                        reg_id = item.reg_id,
                        reg_dt = item.reg_dt,
                        chg_id = item.chg_id,
                        chg_dt = item.chg_dt,
                    };
                    row_data.Add(row_data_iteam);
                    id_data++;
                };
                if (id_data == 0)
                {
                    mt_cd = "DEV001";
                }
                else
                {
                    id_data++;
                    if (id_data.ToString().Length == 1)
                    {
                        mt_cd = "DEV00" + id_data.ToString();
                    }
                    if (id_data.ToString().Length == 2)
                    {
                        mt_cd = "DEV0" + id_data.ToString();
                    }
                    if (id_data.ToString().Length == 3)
                    {
                        mt_cd = "DEV" + id_data.ToString();
                    }
                }
                id_data++;
                var checkComMT = await _IdevManagementService.CheckComMT(mt_cd);
                if (checkComMT < 1)
                {
                    comm_mt.mt_cd = mt_cd;
                }
                else
                {
                    if (id_data.ToString().Length == 1)
                    {
                        mt_cd = "DEV00" + id_data.ToString();
                    }
                    if (id_data.ToString().Length == 2)
                    {
                        mt_cd = "DEV0" + id_data.ToString();
                    }
                    if (id_data.ToString().Length == 3)
                    {
                        mt_cd = "DEV" + id_data.ToString();
                    }
                }

                comm_mt.mt_cd = mt_cd;
                comm_mt.div_cd = "DEV";
                comm_mt.mt_nm = mt_nm;
                comm_mt.mt_exp = mt_exp;
                comm_mt.use_yn = use_yn;
                comm_mt.reg_dt = DateTime.Now;
                comm_mt.chg_dt = DateTime.Now;
                comm_mt.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                comm_mt.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();

                var mt_id = await _IdevManagementService.InsertIntoComMT(comm_mt);
                var rs = await _IdevManagementService.GetCommMTById(mt_id);
                return Json(new { result = true, message = Constant.Success, value = rs }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        public async Task<ActionResult> updateDevCommDt(int cdid, string dt_cd, string dt_nm, string dt_exp, string use_yn, string mt_cd, int dt_order)
        {
            try
            {
                string user = Session["userid"] == null ? null : Session["userid"].ToString();
                if (string.IsNullOrEmpty(user))
                {
                    return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
                }
                var commDT = await _IdevManagementService.GetCommCodeById(cdid);
                if (commDT == null)
                {
                    return Json(new { result = false, message = "Không tìm thấy mã " + dt_cd + " này trong hệ thống" }, JsonRequestBehavior.AllowGet);
                }
                commDT.mt_cd = mt_cd;
                commDT.dt_cd = dt_cd;
                commDT.dt_nm = dt_nm;
                commDT.dt_exp = dt_exp;
                commDT.dt_order = dt_order;
                commDT.use_yn = use_yn;
                commDT.reg_dt = DateTime.Now;
                commDT.chg_dt = DateTime.Now;

                await _IdevManagementService.UpdateComDT(commDT);
                return Json(new { result = true, message = Constant.Success }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        public async Task<ActionResult> updateDevelopCommonMT(int mt_id, string mt_cd, string mt_nm, string mt_exp, string use_yn)
        {
            try
            {
                string user = Session["userid"] == null ? null : Session["userid"].ToString();
                if (string.IsNullOrEmpty(user))
                {
                    return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
                }
                var comm_mt = await _IdevManagementService.GetCommMTById(mt_id);
                if (comm_mt == null)
                {
                    return Json(new { result = false, message = "Không tìm thấy mã " + mt_cd + " này trong hệ thống" }, JsonRequestBehavior.AllowGet);
                }
                comm_mt.mt_cd = mt_cd;
                comm_mt.mt_nm = mt_nm;
                comm_mt.mt_exp = mt_exp;
                comm_mt.use_yn = use_yn;
                comm_mt.reg_dt = DateTime.Now;
                comm_mt.chg_dt = DateTime.Now;

                await _IdevManagementService.UpdateComMT(comm_mt);
                return Json(new { result = true, message = Constant.Success, value = comm_mt }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        public async Task<JsonResult> deleteDevelopCommonMT(string Developcommonmaincode)
        {
            var countCommonDT = await _IdevManagementService.CheckComDT(Developcommonmaincode);
            if (countCommonDT > 0)
            {
                for (int i = 0; i < countCommonDT; i++)
                {
                    await _IdevManagementService.DeleteComDT(Developcommonmaincode);
                };
            }
            var result = await _IdevManagementService.CheckComMT(Developcommonmaincode);
            if (result < 1)
            {
                return Json(new { result = false, message = "Không tìm thấy mã " + Developcommonmaincode + " này trong hệ thống" }, JsonRequestBehavior.AllowGet);
            }

            await _IdevManagementService.DeleteComMT(Developcommonmaincode);
            return Json(new { result = true, message = Constant.Success }, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public async Task<JsonResult> deleteDevelopCommondt(string Developcommondt_manincode, string Developcommondt_dtcd)
        {
            var countComDT = await _IdevManagementService.CheckComDT(Developcommondt_dtcd, Developcommondt_manincode);
            if (countComDT < 1)
            {
                return Json(new { result = false, message = "Không tìm thấy mã " + Developcommondt_manincode + " này trong hệ thống" }, JsonRequestBehavior.AllowGet);
            }
            await _IdevManagementService.DeleteComDT(Developcommondt_manincode, Developcommondt_dtcd);
            return Json(new { result = true, message = Constant.Success }, JsonRequestBehavior.AllowGet);
        }

        #endregion Develop

        #region ProductMgt

        public ActionResult ProductMgt()
        {
            return SetLanguage("");
        }

        [HttpGet]
        public async Task<JsonResult> GetProductForBuyer(string style_no, string style_nm, string md_cd)
        {
            var result = await _IdevManagementService.GetListStyleInfo(style_no, style_nm, md_cd);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProduct(string product)
        {
            if (string.IsNullOrEmpty(product))
            {
                product = "";
            }
            var sql = new StringBuilder();
            sql.Append($" SELECT  s.style_no, s.md_cd, s.pack_amt, s.stamp_code")
                .Append($" FROM  d_style_info as s ")
                .Append($" WHERE  s.style_no like  '%{product}%' ")
                .Append($" AND ( s.stamp_code IS NOT NULL  && s.stamp_code <> '' ) ")
                .Append($" order by s.sid desc  ");
            return new InitMethods().JsonResultAndMessageFromQuery(sql, "");
        }

        [HttpGet]
        public async Task<JsonResult> GetStyleMgt()
        {
            var data = await _IdevManagementService.GetListDStyleInfo();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> searchStyle()
        {
            // Get Data from ajax function
            var code = Request["style_no"];
            var code_nm = Request["style_nm"];
            var modecode = Request["md_cd"];
            var projectname = Request.QueryString["prj_nm"];
            var start = (Request.QueryString["start"]);
            var end = (Request.QueryString["end"]);

            var data = await _IdevManagementService.searchStyleInfo(code, code_nm, modecode, projectname, start, end);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> searchModeCode(string md_cd, string md_nm)
        {
            var rs = await _IdevManagementService.SearchModeCode(md_cd, md_nm);
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CreateStyleMgt(d_style_info item)
        {
            try
            {
                string user = Session["userid"] == null ? null : Session["userid"].ToString();
                if (string.IsNullOrEmpty(user))
                {
                    return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
                }
                string trimmed = String.Concat(item.style_no.Where(c => !Char.IsWhiteSpace(c)));
                int countStyleInfo = await _IdevManagementService.CheckProductInStyleInfo(trimmed);
                if (countStyleInfo > 0)
                {
                    return Json(new { result = false, message = Constant.DataExist }, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(item.cust_rev))
                {
                    return Json(new { result = false, message = "Vui lòng chọn Vendor Code" }, JsonRequestBehavior.AllowGet);

                }
                item.style_no = trimmed.ToUpper();
                item.use_yn = "Y";
                item.del_yn = "N";
                item.reg_dt = DateTime.Now;
                item.chg_dt = DateTime.Now;
                if (string.IsNullOrEmpty(item.expiry_month))
                {
                    item.expiry_month = "0";
                }


                item.reg_id = user;

                int sid = await _IdevManagementService.InsertIntoStyleInfo(item);
                item.sid = sid;
                return Json(new { result = true, kq = item, message = Constant.Success }, JsonRequestBehavior.AllowGet);
                //var data = await _IdevManagementService.GetStyleInfo(sid);
            }
            catch (Exception e)
            {

                return Json(new { result = false,  message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
            }



        }

        [HttpPut]
        public async Task<ActionResult> ModifyStyleMgt(d_style_info request)
        {
            string user = Session["userid"] == null ? null : Session["userid"].ToString();
            if (string.IsNullOrEmpty(user))
            {
                return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
            }
            int countStyleInfo = await _IdevManagementService.CheckProductInStyleInfo(request.style_no);
            if (countStyleInfo < 1)
            {
                return Json(new { result = false, message = "Không tìm thấy mã " + request.style_no + " này ở trong hệ thống" }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(request.cust_rev))
            {
                return Json(new { result = false, message = "Vui lòng chọn Vendor Code" }, JsonRequestBehavior.AllowGet);

            }
            var item = await _IdevManagementService.GetStyleInfo(request.sid);

            item.style_no = request.style_no.ToUpper();
            item.style_nm = request.style_nm;
            item.md_cd = request.md_cd;
            item.ssver = request.ssver;
            item.prj_nm = request.prj_nm;
            item.part_nm = request.part_nm;
            item.standard = request.standard;
            item.cust_rev = request.cust_rev;
            item.order_num = request.order_num;
            item.pack_amt = request.pack_amt.ToInt();
            item.cav = request.cav;
            item.bom_type = request.bom_type;
            item.tds_no = request.tds_no;
            item.item_vcd = request.item_vcd;
            item.drawingname = request.drawingname;
            item.loss = request.loss;
            item.Description = request.Description;
            item.productType = request.productType;
            item.qc_range_cd = request.qc_range_cd;

            item.use_yn = "Y";
            item.del_yn = "N";
            //item.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
            item.chg_id = user;

            if (!string.IsNullOrEmpty(request.stamp_code))
            {
                item.stamp_code = request.stamp_code.Trim();
                item.stamp_name = request.stamp_code.Trim();
            }
            else
            {
                item.stamp_code = "";
                item.stamp_name = "";
            }
            if (!string.IsNullOrEmpty(request.expiry_month))
            {
                item.expiry_month = request.expiry_month;
            }
            else
            {
                item.expiry_month = "0";
            }
            if (request.expiry == null)
            {
                item.expiry = null;
            }
            else
            {
                item.expiry = request.expiry;
            }

            await _IdevManagementService.UpdateStyleInfo(item);
            var result = await _IdevManagementService.GetProductByCode(item.style_no);
            return Json(new { result = true, kq = result, message = Constant.Success }, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public async Task<ActionResult> deleteStyle(int sid)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }
            string user = Session["userid"] == null ? null : Session["userid"].ToString();
            var styleInfo = await _IdevManagementService.GetStyleInfo(sid);
            if (string.IsNullOrEmpty(user))
                {
                    return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
                }
            if (styleInfo == null)
            {
                return Json(new { result = false, message = "Không tìm thấy mã " + styleInfo.style_no + " này trong hệ thống" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //kiểm tra product này đã đăng kí ở BOM && ROUTING chưa...
                int isCheckBom = await _IdevManagementService.CheckIsExistProductBom(styleInfo.style_no);
                if (isCheckBom > 0)
                {
                    return Json(new { result = false, message = "PRODUCT CODE này đã được đăng kí ở BOM" }, JsonRequestBehavior.AllowGet);
                }
                int isCheckRouting = await _IdevManagementService.CheckIsExistProductRouting(styleInfo.style_no);
                if (isCheckRouting > 0)
                {
                    return Json(new { result = false, message = "PRODUCT CODE này đã được đăng kí ở ROUTING" }, JsonRequestBehavior.AllowGet);
                }
                await _IdevManagementService.RemoveStyleInfo(styleInfo.sid);
                return Json(new { result = true, message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = true, message = Constant.Success }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetModelcode()
        {
            var modelcode = await _IdevManagementService.GetListModelInfo();
            return Json(new { rows = modelcode }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetBomType(comm_mt comm_dt)
        {
            var ssver = await _IdevManagementService.GetListCommDT("DEV007");
            return Json(ssver, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetParkNm(comm_mt comm_dt)
        {
            var parknm = await _IdevManagementService.GetListCommDT("DEV003");
            return Json(parknm, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetStandard(comm_mt comm_dt)
        {
            var standard = await _IdevManagementService.GetListCommDT("DEV004");
            return Json(standard, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetCustRev(comm_mt comm_dt)
        {
            var custrev = await _IdevManagementService.GetListCommDT("DEV005");
            return Json(custrev, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetOrder(comm_mt comm_dt)
        {
            var order = await _IdevManagementService.GetListCommDT("DEV006");
            return Json(order, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetType(comm_mt comm_dt)
        {
            var type = await _IdevManagementService.GetListCommDT("COM004", "Y");
            return Json(type, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetTdsno(comm_mt comm_dt)
        {
            var tdsno = await _IdevManagementService.GetListCommDT("DEV008");
            return Json(tdsno, JsonRequestBehavior.AllowGet);
        }

        //End Style management
        [HttpPost]
        public async Task<JsonResult> insertProductExcel(d_style_info styleInfo, List<ModelInsertProductExcel> ModelInsertProductExcel)
        {
            var data_update = 0;
            var data_create = 0;
            var data_error = 0;
            var list = new ArrayList();
            if ((ModelInsertProductExcel == null) && (ModelInsertProductExcel?.Any() != true))
            {
                return Json(new { result = false, message = "Không thế lấy dữ liệu từ tệp excel hoặc tệp tải lên không có dữ liệu" }, JsonRequestBehavior.AllowGet);
            }

            if (ModelInsertProductExcel.Count == 0)
            {
                return Json(new { result = false, message = "Không thế lấy dữ liệu từ tệp excel hoặc tệp tải lên không có dữ liệu" }, JsonRequestBehavior.AllowGet);
            };

            foreach (var item in ModelInsertProductExcel)
            {
                var style_no = item.style_no;

                if (string.IsNullOrEmpty(item.style_no))
                {
                    data_error++;
                    continue;
                }

                var style_nm = string.IsNullOrEmpty(item.style_nm) ? "" : item.style_nm.ToString();

                //var list_d_model_info = db.d_model_info.ToList();

                //foreach (var comm in list_d_model_info)
                //{
                //    if (item.md_cd.Trim() == comm.md_cd.Trim())
                //    {
                //        item.md_cd = comm.md_cd;
                //        break;
                //    }
                //};

                var md_cd = string.IsNullOrEmpty(item.md_cd) ? "" : item.md_cd.ToString();

                //item.md_cd.ToString();
                var prj_nm = string.IsNullOrEmpty(item.prj_nm) ? "" : item.prj_nm.ToString();

                var stamp_code = string.IsNullOrEmpty(item.stamp_code) ? "" : item.stamp_code.ToString();

                var pack_amt = item.pack_amt;
                string expiry = "";
                string expiry_month = "";

                var hsd = string.IsNullOrEmpty(item.expiry) ? "" : item.expiry.ToString();
                var isNumeric = int.TryParse(hsd, out int n);
                if (isNumeric)
                {
                    expiry_month = hsd;
                }
                else
                {
                    expiry = hsd;
                }

                var countlist = await _IdevManagementService.CheckProductInStyleInfo(style_no);
                try
                {
                    if (countlist == 0)
                    {
                        styleInfo.style_no = style_no.Replace("\"", "") + "";
                        styleInfo.style_nm = style_nm.Replace("\"", "") + "";
                        styleInfo.md_cd = md_cd.Replace("\"", "") + "";
                        styleInfo.prj_nm = prj_nm.Replace("\"", "") + "";
                        styleInfo.pack_amt = Int32.Parse(pack_amt);
                        styleInfo.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                        styleInfo.reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                        styleInfo.reg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
                        styleInfo.chg_dt = DateTime.Now;

                        var result = await _IdevManagementService.InsertIntoStyleInfo(styleInfo);

                        if (result > 0)
                        {
                            data_create++;
                            var rs = await _IdevManagementService.GetListProductExcel(style_no);
                            var list2 = rs.ToList();
                            await _IdevManagementService.InsertProductExcel();
                            list.Add(list2);


                        }
                    }
                    else
                    {
                        data_update++;
                        return Json(new { result = false, message = "Đã tồn tại mã " + style_no + " này trong hệ thống" }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception)
                {
                    data_error++;
                }
            }
            return Json(new
            {
                result = true,
                message = Constant.Success,
                data_update = data_update,
                data_create = data_create,
                data = list,
                data_error = data_error
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion ProductMgt

        #region ModelManagement

        public ActionResult ModelManagement()
        {
            return SetLanguage("");
        }

        [HttpPost]
        public async Task<ActionResult> insertModelMgt(ModelInfo modelInfo, string md_cd, string md_nm, string use_yn)
        {
            string user = Session["userid"] == null ? null : Session["userid"].ToString();
            if (string.IsNullOrEmpty(user))
            {
                return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
            }
            var countModelInfo = await _IdevManagementService.CheckModelInfo(md_cd);
            if (countModelInfo == 0)
            {
                modelInfo.md_cd = md_cd;
                modelInfo.md_nm = md_nm;
                modelInfo.use_yn = use_yn;
                modelInfo.del_yn = "N";
                modelInfo.reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                modelInfo.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                modelInfo.reg_dt = DateTime.Now;
                modelInfo.chg_dt = DateTime.Now;
                int mdid = await _IdevManagementService.InsertModelInfo(modelInfo);
                var kq = await _IdevManagementService.GetModelInfoById(mdid);
                return Json(new { result = true, data = kq, message = Constant.Success }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Mã " + md_cd + "này đã tồn tại trong hệ thống" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPut]
        public async Task<ActionResult> updateModelMgt(int mdid, string md_cd, string md_nm, string use_yn)
        {
            string user = Session["userid"] == null ? null : Session["userid"].ToString();
            if (string.IsNullOrEmpty(user))
            {
                return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
            }
            var countModelInfo = await _IdevManagementService.CheckModelInfo(md_cd);
            if (countModelInfo > 0)
            {
                var modelInfo = await _IdevManagementService.GetModelInfoById(mdid);
                modelInfo.md_cd = md_cd;
                modelInfo.md_nm = md_nm;
                modelInfo.use_yn = use_yn;
                modelInfo.del_yn = "N";
                modelInfo.reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                modelInfo.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                modelInfo.reg_dt = DateTime.Now;
                modelInfo.chg_dt = DateTime.Now;

                await _IdevManagementService.UpdateModelInfo(modelInfo);
                return Json(new { result = true, data = modelInfo, message = Constant.Success }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Không tìm thấy mã " + md_cd + "này trong hệ thống" }, JsonRequestBehavior.AllowGet);
            }
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> insertModelExcel(ModelInfo d_model_info, List<ModelInsertModelExcel> insertModel_Excel)
        {
            if ((insertModel_Excel == null) && (insertModel_Excel?.Any() != true))
            {
                return Json(new { result = false, message = "Không thế lấy giữ liệu từ tệp excel hoặc tệp tải lên không có giữ liệu" }, JsonRequestBehavior.AllowGet);
            }

            if (insertModel_Excel.Count == 0)
            {
                return Json(new { result = false, message = "Không thế lấy giữ liệu từ tệp excel hoặc tệp tải lên không có giữ liệu" }, JsonRequestBehavior.AllowGet);
            };
            var data_update = 0;
            var data_create = 0;
            var data_error = 0;
            var list = new ArrayList();

            foreach (var item in insertModel_Excel)
            {

                var md_cd = item.code;

                if (string.IsNullOrEmpty(md_cd))
                {
                    data_error++;
                    continue;
                }
                var md_nm = string.IsNullOrEmpty(item.name) ? "" : item.name.ToString();

                var checkModelInfo = await _IdevManagementService.CheckModelInfo(md_cd);
                try
                {
                    if (checkModelInfo == 0)
                    {
                        d_model_info.reg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
                        d_model_info.chg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
                        d_model_info.md_cd = md_cd.Replace("\"", "") + "";
                        d_model_info.md_nm = md_nm.Replace("\"", "") + "";
                        d_model_info.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                        d_model_info.reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();

                        var result = await _IdevManagementService.InsertModelInfo(d_model_info);

                        if (result > 0)
                        {
                            data_create++;

                            //var sql_insert = new StringBuilder();

                            //sql_insert.Append(" SELECT a.mdid,a.md_cd,a.md_nm,a.use_yn,a.reg_id,reg_dt,a.chg_id,chg_dt,'INSERT' AS STATUS ")
                            //         .Append(" FROM d_model_info a ")
                            //          .Append(" WHERE a.md_cd='" + md_cd + "' ")
                            //          .Append(" ORDER BY a.chg_dt ");
                            //var list2 = new InitMethods().ConvertDataTableToList<ModelReturnModelExcel>(sql_insert);
                            var rs = await _IdevManagementService.GetListModelExcel(md_cd);
                            var list2 = rs.ToList();
                            list.Add(list2);
                        }
                    }
                    else
                    {
                        data_update++;
                        return Json(new { result = false, message = "Đã tồn tại mã " + md_cd + " này trong hệ thống" }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                    data_error++;
                }
            }
            return Json(new
            {
                result = true,
                message = Constant.Success,
                data_update = data_update,
                data_create = data_create,
                data_error = data_error,
                data = list
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public async Task<JsonResult> searchModelMgt(Pageing pageing, string md_cd, string md_nm)
        {
            var data = await _IdevManagementService.GetListModelInfo(md_cd, md_nm);
            var count = data.Count();
            int pageIndex = pageing.page;
            int pageSize = pageing.rows;
            int startRow = (pageIndex * pageSize) + 1;
            int totalRecords = count;
            var gtri = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            int totalPages = (gtri < -1 ? 1 : gtri);
            var result = new
            {
                total = totalPages,
                page = pageIndex,
                records = totalRecords,
                rows = data.ToArray().ToPagedList(pageIndex, pageSize),
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public async Task<ActionResult> deleteModel(int mdid)
        {
            var modelInfo = await _IdevManagementService.GetModelInfoById(mdid);
            if (modelInfo == null)
            {
                return Json(new { result = false, message = "Không tìm thấy dữ liệu!" }, JsonRequestBehavior.AllowGet);
            }
            await _IdevManagementService.RemoveModelInfo(mdid);
            return Json(new { result = true, message = Constant.Success, data = modelInfo }, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> ExportToExcelModel(string md_nm, string md_cd)
        {
            //StringBuilder sql = new StringBuilder($" Call ExportToExcelReceiFG('{po}','{product}','{buyer}','{lot_date}','{lot_date_end}'); ");
            //var data = db.Database.SqlQuery<ExportToExcelReceiFG>(sql.ToString());

            //var data = await _ITimsService.ExportToExcelReceiveFG(product, buyer, po_no, lot_date, lot_date_end);
            var data = await _IdevManagementService.GetListModelInfo(md_cd, md_nm);
            var values = data.ToList().AsEnumerable().Select(x => new
            {
                md_cd = x.md_cd,
                md_nm = x.md_nm,
                use_yn = x.use_yn,
                reg_id = x.reg_id,
                reg_dt = x.reg_dt,
                chg_id = x.chg_id,
                chg_dt = x.chg_dt,

            }).ToArray();

            String[] labelList = new string[7] { "Model Code", "Name", "use Y/N", "Create User", "Create Date", "Change User", "Change Date" };

            Response.ClearContent();

            Response.AddHeader("content-disposition", "attachment;filename=ModelManagement.xls");

            Response.AddHeader("Content-Type", "application/vnd.ms-excel");

            new InitMethods().WriteHtmlTable(values, Response.Output, labelList);

            Response.End();


            return View("~/Views/DevManagement/ModelManagement.cshtml");
        }

        #endregion ModelManagement
        #region  Resource Management
        public ActionResult ResourceMgt()
        {
            return SetLanguage("");
        }
        public async Task<ActionResult> PartialView_ResourceMgt()
        {

            try
            {
                var month = (Request["month"] != null ? Request["month"] : "0");
                var productCode = (Request["productCode"] != null ? Request["productCode"] : "");
                var po_no = (Request["po_no"] != null ? Request["po_no"] : "");
                var machineCode = (Request["machineCode"] != "" ? Request["machineCode"] : "SX3-RO-10K-04");

                var time_now = DateTime.Now.AddMonths(int.Parse(month));

                var startOfMonth = new DateTime(time_now.Year, time_now.Month, 1);
                var first = startOfMonth.AddMonths(1);
                var last = first.AddDays(-1);

                var date_start = startOfMonth.ToString("yyyy-MM-dd");
                var date_end = last.ToString("yyyy-MM-dd");

                var result = new ProductActivition();
                var data = await _IdevManagementService.PartialView_ResourceMgt(po_no,productCode,machineCode,date_end,date_start);
                result.ProductActivitionFaile = data.ToList();
               // return Json(result, JsonRequestBehavior.AllowGet);


                var monthReport = DateTime.Now.ToString("yyyy-MM-dd").Substring(0, 7);
                ViewBag.start = date_start;
                ViewBag.end = date_end;
                ViewBag.MonthReport = monthReport;

                return PartialView(result);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
            }

        }
        public async Task<ActionResult> PartialView_dialog_Viewdetail(string at_no, string mc_no)
        {
            try
            {
                ViewBag.at_no = at_no;
                ViewBag.mc_no = mc_no;
                var data = await _IdevManagementService.PartialView_dialog_Viewdetail(at_no, mc_no);
                return PartialView(data);
            }
            catch (Exception)
            {

                return Json(new { result = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> GetDataChart(string at_no, string mc_no)
        {


            try
            {
                ViewBag.at_no = at_no;
                ViewBag.mc_no = mc_no;
                var data = await _IdevManagementService.PartialView_dialog_Viewdetail(at_no, mc_no);
                return Json(new { result = true, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new { result = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
            }
        }
        public class ProductActivition
        {
            public List<modelTableMAchine> ProductActivitionFaile { get; set; }

        }
        public class modelTableMAchine
        {
            public string id { get; set; }
            public string date { get; set; }
            public string mc_no { get; set; }
            public string mc_nm { get; set; }
            public string at_no { get; set; }
            public string productCode { get; set; }
            public string start_dt { get; set; }
            public string end_dt { get; set; }


        }
        #endregion
        #region StaffMgt

        //Begin Layout
        public ActionResult StaffMgt()
        {
            return SetLanguage("");
        }

        [HttpGet]
        public async Task<ActionResult> getgrade()
        {
            var result = await _IdevManagementService.GetListGrande();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetAllDepartments()
        {
            var data = await _IdevManagementService.GetListDepartmentInfo();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> getposition()
        {
            var position = await _IdevManagementService.GetPositionStaff();
            return Json(position, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public async Task<JsonResult> userInfoData()
        {
            try
            {
                var result = await _IdevManagementService.GetListUser();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<JsonResult> userInfoData_staff()
        {
            var result = await _IdevManagementService.GetListStaff();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> insertstaff(string userid, string uname, string position, string userCode, string birthDay, string gender, string department, string joinDay)
        {
            Dictionary<string, string> checkNull = new Dictionary<string, string>();
            checkNull.Add("userid", userid);
            checkNull.Add("uname", uname);
            checkNull.Add("position", position);
            checkNull.Add("userCode", userCode);
         //   checkNull.Add("birthDay", birthDay);
            checkNull.Add("gender", gender);
            checkNull.Add("department", department);
           // checkNull.Add("joinDay", joinDay);

            foreach (var item in checkNull)
            {
                if (string.IsNullOrEmpty(item.Value))
                {
                    return Json(new { flag = false, message = "Điền đầy đủ thông tin để tiếp tục!." }, JsonRequestBehavior.AllowGet);
                }
            }

            var mb_info = new MbInfo();
            var result = await _IdevManagementService.countStaffbyUserId(userid);
            if ((result != 0) && (userid == ""))
            {
                return Json(new { result = false, userid = mb_info.userid, message = "Đã tồn tại " + mb_info.userid + " trong hệ thống" }, JsonRequestBehavior.AllowGet);
            }

            mb_info.userid = userid;
            mb_info.uname = uname;
            mb_info.gender = gender;
            mb_info.lct_cd = "staff";
            mb_info.barcode = userCode;
            mb_info.position_cd = position;
           // mb_info.birth_dt = birthDay.Replace("-", "");
            mb_info.depart_cd = department;
           // mb_info.join_dt = joinDay.Replace("-", "");
            mb_info.del_yn = "N";
            mb_info.reg_dt = DateTime.Now;
            mb_info.chg_dt = DateTime.Now;
            mb_info.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
            mb_info.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
            var checkUser = await _IdevManagementService.checkStaff(mb_info.userid);
            if(checkUser > 0)
            {
                return Json(new { result = false, message = "Đã tồn tại UserId !" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
            var mbid = await _IdevManagementService.InsertStaff(mb_info);
            var rs = await _IdevManagementService.GetStaffbyId(mb_info.userid);
                return Json(new { result = true, message = "Thành công !", kq = rs }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPut]
        public async Task<JsonResult> modifyStaff(string userid, string uname, string position, string userCode, string birthDay, string gender, string department, string joinDay)
        {
            Dictionary<string, string> checkNull = new Dictionary<string, string>();
            checkNull.Add("userid", userid);
            checkNull.Add("uname", uname);
            checkNull.Add("position", position);
            checkNull.Add("userCode", userCode);
            //checkNull.Add("birthDay", birthDay);
            checkNull.Add("gender", gender);
            checkNull.Add("department", department);
            //checkNull.Add("joinDay", joinDay);

            foreach (var item in checkNull)
            {
                if (string.IsNullOrEmpty(item.Value))
                {
                    return Json(new { flag = false, message = "Điền đầy đủ thông tin để tiếp tục!." }, JsonRequestBehavior.AllowGet);
                }
            }

            //var result = db.mb_info.Count(x => x.userid == userid);
            int result = await _IdevManagementService.countStaffbyUserId(userid);
            if ((result > 0) && (userid != ""))
            {
                var mb_info = await _IdevManagementService.GetStaffbyUserId(userid);
                mb_info.uname = uname;
                mb_info.gender = gender;
                mb_info.lct_cd = "staff";
                mb_info.barcode = userCode;
                mb_info.position_cd = position;
               // mb_info.birth_dt = birthDay.Replace("-", "");
                mb_info.depart_cd = department;
             //   mb_info.join_dt = joinDay.Replace("-", "");
                mb_info.del_yn = "N";
                //mb_info.chg_dt = DateTime.Now;
                mb_info.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();

                await _IdevManagementService.UpdateStaff(mb_info);
                var rs = await _IdevManagementService.GetStaffbyId(mb_info.userid);

                var resultauth = await _IdevManagementService.GetMBAuthorInfobyUserId(userid);
                if (resultauth != null)
                {
                    await _IdevManagementService.DeleteMbAuthorInfobyUserId(resultauth.userid);
                }

                return Json(new { result = true, message = "Cập nhập Thành Công !", kq = rs }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Không tìm thấy " + userid + " trong hệ thống !", userid = userid }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> insertUser(string userid, string uname, string upw, string nick_name, string cel_nb, string e_mail, string memo, string grade, string scr_yn, string mail_yn, string joinday, string birthday, string gender, string department, string position)
        {
            string user = Session["userid"] == null ? null : Session["userid"].ToString();
            if (string.IsNullOrEmpty(user))
            {
                return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
            }
            // Insert into mb_info
            var mb_info = new MbInfo();
            var result = await _IdevManagementService.CheckMbInfo(userid);
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
                mb_info.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                mb_info.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                //if (joinday != "") { joinday = (Convert.ToDateTime(joinday)).ToString("yyyyMMdd"); } else { joinday = Convert.ToString(joinday); }
                mb_info.join_dt = joinday;
                //if (birthday != "") { birthday = (Convert.ToDateTime(birthday)).ToString("yyyyMMdd"); } else { birthday = Convert.ToString(birthday); }
                mb_info.birth_dt = birthday;

                mb_info.gender = gender;
                mb_info.depart_cd = department;
                mb_info.position_cd = position;
                mb_info.lct_cd = "user";

                await _IdevManagementService.InsertIntoMbInfo(mb_info);
                var rs = await _IdevManagementService.GetUserById(userid);

                // Insert into mb_author_info
                var mb_author_info = new MbAuthorInfo();
                mb_author_info.userid = userid;
                mb_author_info.at_cd = await _IdevManagementService.GetCodeFromAuthorInfo(grade);
                mb_author_info.reg_dt = DateTime.Now;
                mb_author_info.chg_dt = DateTime.Now;

                await _IdevManagementService.InsertIntoMbAuthorInfo(mb_author_info);

                return Json(new { result = true, value = rs, message = Constant.Success }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Đã tồn tại " + userid + " này trong hệ thống!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPut]
        public async Task<JsonResult> modifyUser(string userid, string uname, string upw, string nick_name, string cel_nb, string e_mail, string memo, string grade, string scr_yn, string mail_yn, string joinday, string birthday, string gender, string department, string position)
        {
            string user = Session["userid"] == null ? null : Session["userid"].ToString();
            if (string.IsNullOrEmpty(user))
            {
                return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
            }
            var mb_info = await _IdevManagementService.GetUserByUserId(userid);
            if ((mb_info != null) && (userid != "") && (upw != ""))
            {
                var at_cd = await _IdevManagementService.GetCodeFromAuthorInfo(grade);
                mb_info.uname = uname;
                mb_info.nick_name = nick_name;
                mb_info.upw = upw;
                mb_info.grade = grade;
                mb_info.cel_nb = cel_nb;
                mb_info.e_mail = e_mail;
                mb_info.mail_yn = mail_yn;
                mb_info.memo = memo;
                mb_info.scr_yn = scr_yn;
                //if (joinday.Length > 8) { joinday = (Convert.ToDateTime(joinday)).ToString("yyyyMMdd"); } else { joinday = Convert.ToString(joinday); }
                mb_info.join_dt = joinday;
                // if (birthday.Length > 8) { birthday = (Convert.ToDateTime(birthday)).ToString("yyyyMMdd"); } else { birthday = Convert.ToString(birthday); }
                mb_info.birth_dt = birthday;
                mb_info.gender = gender;
                mb_info.depart_cd = department;
                mb_info.position_cd = position;

                await _IdevManagementService.UpdateUseṛ(mb_info);

                var mb_author_info = await _IdevManagementService.GetMBAuthorInfobyUserId(userid);
                mb_author_info.at_cd = at_cd;
                if (mb_author_info != null)
                {
                    await _IdevManagementService.UpdateMbUserInfor(mb_author_info);
                }

                return Json(new { result = true, message = Constant.Success, userid = mb_info.userid }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "Không tồn tại " + userid + " này tron hệ thống !" }, JsonRequestBehavior.AllowGet);
            }
        }

         public async Task<ActionResult> getgender()
        {
            var position = await _IdevManagementService.getgender();
            return Json(position, JsonRequestBehavior.AllowGet);
        }
        [HttpDelete]
        public async Task<JsonResult> deleteStaff(string userid)
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

        [HttpGet]
        public async Task<JsonResult> searchUser(string searchType, string keywordInput, string department, string position)
        {
            var result = await _IdevManagementService.SearchUser(searchType, keywordInput, department, position);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> SearchStaff(Pageing page, string searchType, string keywordInput, string position)
        {
            if (string.IsNullOrEmpty(keywordInput))
            {
                keywordInput = "";
            }
            var data = await _IdevManagementService.SearchStaff(searchType, keywordInput, position);
            int start = (page.page - 1) * page.rows;
            int totals = data.Count();
            int totalPages = (int)Math.Ceiling((float)totals / page.rows);
            //List<MbInfo> dataactual = data.Skip<MbInfo>(start).Take(page.rows);
            var jsonReturn = new
            {
                total = totalPages,
                page = page.page,
                records = totals,
                rows = data
            };
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }

        #endregion StaffMgt

        #region TrayBox

        public ActionResult TrayBoxMgt()
        {
            return SetLanguage("");
        }


        [HttpPost]
        public async Task<ActionResult> insertautoTrayBox(string qty_ge, string mc_type)
        {
            DateTime dateTime = DateTime.Now;
            string CurrentDateTime = dateTime.ToString("yyMMddHHmmss");
            var menuCd = string.Empty;
            int qty = Convert.ToInt32(qty_ge);
            var list = new List<BobbinInfo>();
            var bb_no_tmp = string.Empty;

            for (int i = 0; i < qty; i++)
            {
                //bb_no_tmp = string.Format("{0}", new Excute_query().autobarcode((i + 1)));

                bb_no_tmp = String.Format("{0, 0:D3}", i + 1);
                string type = CheckType(mc_type);
                var bb_cd = MergeCode(CurrentDateTime, bb_no_tmp, type);

                var chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                var reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();

                var bb_nm = new Excute_query().StartEndIndex(bb_cd, 0, bb_cd.LastIndexOf("-"));
                var checkBBNo = await _IdevManagementService.CheckBobbinInfo(bb_cd);
                var result = 0;
                if (checkBBNo < 1)
                {
                    result = await _IdevManagementService.CreateTrayBox(bb_cd, chg_id, reg_id, mc_type, bb_nm);
                }

                if (result > 0)
                {
                    var list2 = await _IdevManagementService.GetListTrayBox(bb_cd);
                    list.Add(list2);
                    //list.Add(bb_cd);
                }
            }
            #region MyRegion

            //int index = sb.ToString().LastIndexOf(",");
            //string bb_list = sb.ToString().Substring(0, index);

            //var sql_insert = new StringBuilder();

            //sql_insert.Append(" SELECT d.bb_no, d.bno, d.mc_type, d.mt_cd, d.bb_nm, d.purpose, d.barcode, d.re_mark ")
            //    .Append(" FROM d_bobbin_info d ")
            //    .Append(" WHERE d.bb_no IN (" + bb_list + " )")
            //    .Append(" ORDER BY d.bno DESC ");
            //var list2 = new InitMethods().ConvertDataTableToList<ModelReturnTrayBox>(sql_insert);

            #endregion MyRegion
            return Json(new
            {
                result = true,
                data = list.ToList(),
                message = "Thành công !",
            }, JsonRequestBehavior.AllowGet);

        }

        public string MergeCode(string CurrentDateTime, string menuCd, string type)
        {
            return string.Concat("AUTO-" + type + "-" + CurrentDateTime + menuCd);
        }

        public string CheckType(string type)
        {
            if (type == "BOB")
            {
                type = "BOB";
            }
            else
            {
                type = "TRAY";
            }
            return type;
        }

        [HttpGet]
        public async Task<JsonResult> searchTrayBox(Pageing pageing, string bb_no, string bb_nm)
        {
            int start = (pageing.page - 1) * pageing.rows;
            var result = await _IdevManagementService.SearchTrayBox(bb_no, bb_nm);
            int totals = result.Count();
            int totalPages = (int)Math.Ceiling((float)totals / pageing.rows);
            IEnumerable<BobbinInfo> dataactual = result.Skip<BobbinInfo>(start).Take(pageing.rows);
            var jsonReturn = new
            {
                total = totalPages,
                page = pageing.page,
                records = totals,
                rows = dataactual
            };
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> ExportToExcelTrayBox(string bb_nm, string bb_no)
        {
            var listData = await _IdevManagementService.SearchTrayBox(bb_no, bb_nm);

            var records = listData.ToList().AsEnumerable().Select(x => new
            {
                mc_type = x.mc_type,
                bb_no = x.bb_no,
                bb_nm = x.bb_nm,
                mt_cd = x.mt_cd,
                at_no = x.at_no,
                purpose = x.purpose,
                barcode = x.barcode,
                re_mark = x.re_mark
            }).ToArray();

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=TrayBoxManagement.xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            WriteMoldHtmlTable(records, Response.Output);
            Response.End();
            return View("MoldMgt");
        }
        private void WriteMoldHtmlTable<T>(IEnumerable<T> data, TextWriter output)
        {
            //Writes markup characters and text to an ASP.NET server control output stream. This class provides formatting capabilities that ASP.NET server controls use when rendering markup to clients.
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    //  Create a form to contain the List
                    Table table = new Table();

                    TableRow row = new TableRow();

                    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

                    foreach (PropertyDescriptor prop in props)
                    {
                        TableHeaderCell hcell = new TableHeaderCell();
                        hcell.Text = prop.Name;
                        hcell.BackColor = System.Drawing.Color.Yellow;
                        row.Cells.Add(hcell);
                    }

                    table.Rows.Add(row);

                    //  add each of the data item to the table
                    foreach (T item in data)
                    {
                        row = new TableRow();
                        foreach (PropertyDescriptor prop in props)
                        {
                            TableCell cell = new TableCell();
                            cell.Text = prop.Converter.ConvertToString(prop.GetValue(item));
                            cell.BorderStyle = BorderStyle.Solid;
                            row.Cells.Add(cell);
                        }
                        table.Rows.Add(row);
                    }

                    //  render the table into the htmlwriter
                    table.RenderControl(htw);

                    //  render the htmlwriter into the response
                    output.Write(sw.ToString());
                }
            }
        }
        public ActionResult _Print()
        {
            string[] keys = Request.Form.AllKeys;

            var value = "";
            value = Request.Form[keys[0]];

            ViewData["Message"] = value;
            return View();
        }

        public async Task<ActionResult> _Print2(string id)
        {
            var a2 = id.TrimStart('[').TrimEnd(']').Split(',');
            var row_data = new List<BobbinInfo>();
            for (int i = 0; i < a2.Length; i++)
            {
                var id2 = int.Parse(a2[i]);
                var data = await _IdevManagementService.GetBarCodeOfBobbinInfo(id2);
                row_data.AddRange(data);
            }
            return Json(row_data, JsonRequestBehavior.AllowGet);
        }

        #endregion TrayBox

        #region Print QR

        public ActionResult _printQRUser(string id)
        {
            ViewData["Message"] = id;
            return View();
        }

        public async Task<ActionResult> PrintMultiUserQRCode(string id)
        {
            var multiIDs = id.TrimStart('[').TrimEnd(']').Split(',');
            var row_data = new List<MbInfo>();
            for (int i = 0; i < multiIDs.Length; i++)
            {
                var eachId = multiIDs[i];
                var data = await _IdevManagementService.GetBarCodeOfStaffByUserId(eachId);
                row_data.Add(data);
            }
            return Json(row_data, JsonRequestBehavior.AllowGet);
        }

        #endregion Print QR

        #region Routing

        public ActionResult Rounting()
        {
            return SetLanguage("");
        }

        [HttpGet]
        public async Task<JsonResult> GetTIMSProcesses()
        {
            var rs = await _IcommonService.getListRoutingProccess();
            var result = rs.AsEnumerable().Select(x => new
            {
                ProcessCode = x.dt_cd,
                ProcessName = x.dt_nm
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetTIMSRolls()
        {
            var rs = await _IdevManagementService.GetListCommDT("COM032");
            var result = rs.AsEnumerable().Select(x => new
            {
                Code = x.dt_cd,
                Name = x.dt_nm
            }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> Get_Rounting(Pageing pageing, string product,string process_code)
        {
            var result = await _IdevManagementService.GetRoutinigInfo(product, process_code);
            int totals = result.Count();
            int totalPages = (int)Math.Ceiling((float)totals / pageing.rows);
            var jsonReturn = new
            {
                total = totalPages,
                page = pageing.page,
                records = totals,
                rows = result.ToList()
            };
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> Create_Rounting(DRoutingInfo item)
        {
            try
            {
                string user = Session["userid"] == null ? null : Session["userid"].ToString();
                if (string.IsNullOrEmpty(user))
                {
                    return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
                }
                //kiem tra co product do chua
                var checkProduct = await _IdevManagementService.CheckProductInStyleInfo(item.style_no);
                if (checkProduct < 1)
                {
                    return Json(new { result = false, message = "Product không tìm thấy!" }, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(item.name))
                {
                    return Json(new { result = false, message = "Bạn chưa nhập Process Name!" }, JsonRequestBehavior.AllowGet);
                }
                //tao cong doan
                //type
                item.type = (item.name.StartsWith("ROT") || item.name.StartsWith("STA") ? "SX" : "TIMS");
                //item_vcd
                if (item.name.StartsWith("OQC"))
                {
                    item.item_vcd = "OC000001A";
                }
                else if (item.type == "SX")
                {
                    item.item_vcd = "PC00004A";
                }
                else
                {
                    item.item_vcd = "TI000001A";
                }
                //level
                var check_data = await _IdevManagementService.GetListRoutingInfoByStyleNo(item.style_no, item.process_code);
                var check_con = check_data.Where(x => x.name == item.name).ToList();
                if (check_con.Count() > 0)
                {
                    item.level = check_con.FirstOrDefault().level;
                }
                else
                {
                    item.level = (check_data.Count() + 1);
                }

                item.description = item.description;
                item.reg_dt = DateTime.Now;
                item.chg_dt = DateTime.Now;
                item.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                item.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();

                if (ModelState.IsValid)
                {
                    if (item.isFinish == "Y")
                    {
                        await _IdevManagementService.UpdateIsFinishDRoutingInfo(item.style_no, item.process_code);
                    }
                    int id =  await _IdevManagementService.InsertRoutingInfo(item);

                    var donvi = await _IdevManagementService.GetUnitFromCom_DT("COM032", item.don_vi_pr);
                    var name = await _IdevManagementService.GetUnitFromCom_DT("COM007", item.name);
                    var newItem = new
                    {
                        idr = id,
                        style_no = item.style_no,
                        process_code = item.process_code,
                        name = item.name,
                        level = item.level,
                        don_vi_pr = item.don_vi_pr,
                        type = item.type,
                        item_vcd = item.item_vcd,
                        description = item.description,
                        reg_dt = item.reg_dt,
                        reg_id = item.reg_id,
                        chg_id = item.chg_id,
                        chg_dt = item.chg_dt,
                        IsFinish = item.isFinish,
                        don_vi_prnm = (donvi != null ? donvi.dt_nm : ""),
                        name_pr = (name != null ? name.dt_nm : ""),
                    };

                    //tra ve view
                    return Json(new { result = true, kq = newItem, message = "Thêm dữ liệu thành công !" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = false, message = "Thêm dữ liệu thất bại!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPut]
        public async Task<JsonResult> Modify_Rounting(int idr, string style_no, string name, string don_vi_pr, string description, string isFinish, string process_code)
        {
            try
            {
                string user = Session["userid"] == null ? null : Session["userid"].ToString();
                if (string.IsNullOrEmpty(user))
                {
                    return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
                }
                var find_id = new DRoutingInfo();
                if (idr == 0)
                {
                    //find_id = await _IdevManagementService.GetTop1RoutingInfo();
                    return Json(new { result = false, message = "Product này không có công đoạn!!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    find_id = await _IdevManagementService.GetRoutingInfoById(idr);

                }
                if (find_id == null)
                {
                    return Json(new { result = false, message = "Product này không có công đoạn!!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    find_id.isFinish = isFinish;
                    find_id.name = name;
                    find_id.description = description;
                    find_id.don_vi_pr = don_vi_pr;
                    find_id.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    //type
                    find_id.type = (find_id.name.StartsWith("ROT") || find_id.name.StartsWith("STA") ? "SX" : "TIMS");
                    //item_vcd
                    if (find_id.name.StartsWith("OQC"))
                    {
                        find_id.item_vcd = "OC000001A";
                    }
                    else if (find_id.type == "SX")
                    {
                        find_id.item_vcd = "PC00004A";
                    }
                    else
                    {
                        find_id.item_vcd = "TI000001A";
                    }
                    if (ModelState.IsValid)
                    {
                        if (isFinish == "Y")
                        {
                            await _IdevManagementService.UpdateIsFinishDRoutingInfo(find_id.style_no, find_id.process_code);
                        }
                        await _IdevManagementService.UpdateRoutingInfo(find_id, description, isFinish);

                        var donvi1 = await _IdevManagementService.GetUnitFromCom_DT("COM032", find_id.don_vi_pr);
                        var name1 = await _IdevManagementService.GetUnitFromCom_DT("COM007", find_id.name);

                        var newItem = new
                        {
                            idr = find_id.idr,
                            style_no = find_id.style_no,
                            name = find_id.name,
                            level = find_id.level,
                            don_vi_pr = find_id.don_vi_pr,
                            type = find_id.type,
                            item_vcd = find_id.item_vcd,
                            description = description,
                            IsFinish = isFinish,
                            reg_dt = find_id.reg_dt,
                            reg_id = find_id.reg_id,
                            chg_id = find_id.chg_id,
                            chg_dt = find_id.chg_dt,
                            don_vi_prnm = (donvi1 != null ? donvi1.dt_nm : ""),
                            name_pr = (name1 != null ? name1.dt_nm : ""),
                        };

                        //tra ve view
                        return Json(new { result = true, kq = newItem, message = "Cập nhật dữ liệu thành công !" }, JsonRequestBehavior.AllowGet);
                    }
                    else return Json(new { result = false, message = "Cập nhật dữ liệu thất bại !" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpDelete]
        public async Task<JsonResult> Deleteprocess(int idr, string process_code)
        {
            try
            {
                var find_id = new DRoutingInfo();
                if (idr == 0)
                {
                    //find_id = await _IdevManagementService.GetTop1RoutingInfo();
                    return Json(new { result = false, message = "Vui lòng chọn 1 công đoạn để xóa" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    find_id = await _IdevManagementService.GetRoutingInfoById(idr);
                }

                if (find_id == null)
                {
                    return Json(new { result = false, message = "Không tìm thấy công đoạn này" }, JsonRequestBehavior.AllowGet);
                }

                var checkExist = await _IdevManagementService.CheckExistsProductMaterial3(find_id.style_no, find_id.name, find_id.process_code);
                if(checkExist > 0)
                {
                    return Json(new { result = false, message = "Công đoạn này đang chứa NVL. Hãy xóa NVL trước !" }, JsonRequestBehavior.AllowGet);
                }

                //lấy level cao nhất
                var check_data = await _IdevManagementService.GetListRoutingInfoByStyleNo(find_id.style_no, process_code);
                if (check_data.Count() > 0)
                {
                    var level = check_data.OrderByDescending(x => x.level).Select(x => x.level).FirstOrDefault();
                    if (level == find_id.level)
                    {
                        await _IdevManagementService.DeleteRoutingInfo(find_id.idr);
                        return Json(new { result = true, message = "Xóa dữ liệu thành công !" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, message = "Chỉ cho phép xóa level cao nhất" }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { result = true, message = "Xóa dữ liệu thành công !" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> Get_ProductMaterial(Pageing pageing, string product, string name, string process_code)
        {
            var result = await _IdevManagementService.GetProductMaterial(product, name, process_code);
            int totals = result.Count();
            int totalPages = (int)Math.Ceiling((float)totals / pageing.rows);
            var jsonReturn = new
            {
                total = totalPages,
                page = pageing.page,
                records = totals,
                rows = result.ToList()
            };
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProductMaterial([FromBody] ProductMaterial model)
        {
            try
            {
                string user = Session["userid"] == null ? null : Session["userid"].ToString();
                if (string.IsNullOrEmpty(user))
                {
                    return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
                }
                var rs = await _IdevManagementService.CheckExistsProductMaterial1(model.style_no, model.level, model.mt_no, model.process_code);
                if (rs > 0)
                {
                    return Json(new { result = false, message = "Nguyên vật liệu " + model.mt_no + "đã tồn tại" }, JsonRequestBehavior.AllowGet);
                }

                if (model.buocdap == 0)
                {
                    return Json(new { result = false, message = "Xin vui lòng nhập dữ liệu vào ô (Bước dập/ Số đo) !" }, JsonRequestBehavior.AllowGet);
                }

                if (model.cav == 0)
                {
                    return Json(new { result = false, message = "Xin vui lòng nhập dữ liệu vào ô (CAVIT) !" }, JsonRequestBehavior.AllowGet);
                }

                if (model.need_time == 0)
                {
                    return Json(new { result = false, message = "Xin vui lòng nhập dữ liệu vào ô (Số lần sử dụng) !" }, JsonRequestBehavior.AllowGet);
                }

                if (model.use_yn == "Y")
                {
                    await _IdevManagementService.UpdateMaterialToCalculatePerformance(model.style_no, model.process_code);
                }

                var mt_nm = await _IdevManagementService.GetNameFromMaterialInfo(model.mt_no);
                var item = new ProductMaterial();

                item.style_no = model.style_no;
                item.process_code = model.process_code;
                item.level = model.level;
                item.mt_no = model.mt_no;
                item.name = model.name;
                item.cav = model.cav;
                item.buocdap = model.buocdap;
                item.need_time = model.need_time;
                item.need_m = (model.buocdap / 1000 / model.cav * model.need_time);
                item.use_yn = model.use_yn;
                item.reg_dt = DateTime.Now;
                item.chg_dt = DateTime.Now;
                item.active = model.active;
                item.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                item.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();

                var id = await _IdevManagementService.InsertIntoProductMaterial(item);
                var kq = await _IdevManagementService.GetProductMaterialById(id);
                kq.mt_nm = mt_nm;

                return Json(new { result = true, message = "Bạn đã tạo thành công !!", data = kq }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public async Task<JsonResult> ModifyProductMaterial(ProductMaterial model)
        {
            try
            {
                string user = Session["userid"] == null ? null : Session["userid"].ToString();
                if (string.IsNullOrEmpty(user))
                {
                    return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
                }
                //kiểm tra trong nguyên vật liệu đã có tồn tại không
                var ProductMaterialObject = await _IdevManagementService.GetProductMaterialById(model.Id);
                if (ProductMaterialObject == null)
                {
                    return Json(new { result = false, message = "Nguyên vật liệu " + model.mt_no + "không tồn tại" }, JsonRequestBehavior.AllowGet);
                }
                if (model.buocdap == 0)
                {
                    return Json(new { result = false, message = "Xin vui lòng nhập dữ liệu vào ô (Bước dập/ Số đo) !" }, JsonRequestBehavior.AllowGet);
                }

                if (model.cav == 0)
                {
                    return Json(new { result = false, message = "Xin vui lòng nhập dữ liệu vào ô (CAVIT) !" }, JsonRequestBehavior.AllowGet);
                }

                if (model.need_time == 0)
                {
                    return Json(new { result = false, message = "Xin vui lòng nhập dữ liệu vào ô (Số lần sử dụng) !" }, JsonRequestBehavior.AllowGet);
                }
                var rs = await _IdevManagementService.CheckExistsProductMaterial2(model.style_no, model.level, model.mt_no, model.Id, model.process_code);
                if (rs > 0)
                {
                    return Json(new { result = false, message = "Công đoạn này đã Đăng kí NGUYÊN VẬT LIỆU này. Hãy chọn một NGUYÊN VẬT LIỆU khác" }, JsonRequestBehavior.AllowGet);
                }

                if (model.use_yn == "Y")
                {
                    await _IdevManagementService.UpdateMaterialToCalculatePerformance(ProductMaterialObject.style_no, ProductMaterialObject.process_code);
                }


                ProductMaterialObject.cav = model.cav;
                ProductMaterialObject.need_time = model.need_time;
                ProductMaterialObject.buocdap = model.buocdap;
                ProductMaterialObject.need_m = (model.buocdap / 1000 / model.cav * model.need_time);
                ProductMaterialObject.mt_no = model.mt_no;
                ProductMaterialObject.use_yn = model.use_yn;
                ProductMaterialObject.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                await _IdevManagementService.UpdateProductMaterial(ProductMaterialObject);

                var getMaterialName = await _IdevManagementService.GetMaterialName(ProductMaterialObject.mt_no);
                ProductMaterialObject.mt_nm = getMaterialName;

                return Json(new { result = true, message = "Bạn đã thay đổi thành công !!", data = ProductMaterialObject }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        [HttpDelete]
        public async Task<ActionResult> DeleteProductMaterial(string Id)
        {
            try
            {
                var newId = Convert.ToInt32(Id);
                var ProductMaterialObject = await _IdevManagementService.GetProductMaterialById(newId);
                if (ProductMaterialObject == null)
                {
                    return Json(new { result = false, message = Constant.DataExist }, JsonRequestBehavior.AllowGet);
                }
                await _IdevManagementService.DeleteProductMaterial(ProductMaterialObject.Id);
                //xóa detail
                await _IdevManagementService.DeleteProductMaterialDetail(ProductMaterialObject.style_no, ProductMaterialObject.process_code, ProductMaterialObject.name, ProductMaterialObject.mt_no);
                return Json(new { result = true, id = ProductMaterialObject.Id, message = "Xóa dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json(new { result = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddMaterialchild([FromBody] ProductMaterialDetail model)
        {
            if (!ModelState.IsValid)
                return null;
            try
            {
                ////kierm tra product trong bảng bom_info
                foreach (var item in model.ListMaterial)
                {
                    if(item == model.MaterialParent)
                    {
                        return Json(new { result = false, message = "Không thể thêm nguyên vật liệu cha thành nguyên vật liệu con!" }, JsonRequestBehavior.AllowGet);
                    }
                }


                //List<string> listMaterialNo = model.ListMaterial.ToList();
                //var listMaterialExist = await _IdevManagementService.CheckExistsProductMaterialDetail(model.ProductCode, model.MaterialParent, listMaterialNo);
                //if (listMaterialExist.Count() > 0)
                //{
                //    await _IdevManagementService.DeleteMaterialExists(listMaterialExist);
                //    //return Json(new { result = false, message = "Nguyên vật liệu thay thế bị trùng nhau !" }, JsonRequestBehavior.AllowGet);
                //}

                //insert material lại
                for (int i = 0; i < model.ListMaterial.Length; i++)
                {

                    var MaterialChild = new ProductMaterialDetail();
                    MaterialChild.level = model.level;
                    MaterialChild.name = model.name;
                    MaterialChild.process_code = model.process_code;
                    MaterialChild.ProductCode = model.ProductCode;
                    MaterialChild.MaterialParent = model.MaterialParent;
                    MaterialChild.MaterialNo = model.ListMaterial[i];
                    MaterialChild.CreateDate = DateTime.Now;
                    MaterialChild.ChangeDate = DateTime.Now;
                    MaterialChild.CreateId = Session["userid"] == null ? null : Session["userid"].ToString();
                    MaterialChild.ChangeId = Session["userid"] == null ? null : Session["userid"].ToString();
                    await _IdevManagementService.InsertToMaterialChild(MaterialChild);
                }
                var value = await _IdevManagementService.GetListMaterialChild(model.ProductCode, model.name, model.MaterialParent, model.process_code);
                return Json(new { result = true, message = "Thêm nguyên vật liệu con vào thành công ", data = value }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> getMaterialChild(string ProductCode, string name, string MaterialPrarent, string process_code)
        {
            try
            {
                var value = await _IdevManagementService.GetListMaterialChild(ProductCode, name, MaterialPrarent, process_code);
                return Json(value, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = "Lỗi hệ thống!!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteMaterialChild(int id)
        {
            var rs = await _IdevManagementService.DeleteMaterialChild(id);
            if (rs > 0)
            {
                return Json(new { result = true, message = "Xóa nguyên vật liệu con thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = "Không thể xóa mã này" }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public async Task<JsonResult> Get_StyleProcess(Pageing pageing, string product)
        {
            var result = await _IdevManagementService.GetProductRouting(product);
            int totals = result.Count();
            int totalPages = (int)Math.Ceiling((float)totals / pageing.rows);
            var jsonReturn = new
            {
                total = totalPages,
                page = pageing.page,
                records = totals,
                rows = result.ToList()
            };
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> CreateProductProcess([FromBody] ProductProcess model)
        {
            if (!ModelState.IsValid)
                return null;
            try
            {
                string user = Session["userid"] == null ? null : Session["userid"].ToString();
                if (string.IsNullOrEmpty(user))
                {
                    return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
                }
                var resultProcess = await _IdevManagementService.GetcheckProcessCode(model.style_no);
                ProductProcess item = new ProductProcess();
                if (resultProcess.process_code == 0)
                {
                    item.process_code = 1;
                }

                else
                {
                    item.process_code = resultProcess.process_code.ToInt() + 1;
                }
                item.style_no = model.style_no;
                item.process_name = model.process_name;
                item.description = model.description;
                item.IsApply = model.IsApply;
                item.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                item.reg_dt = DateTime.Now;


                if (model.IsApply == "Y")
                {
                    await _IdevManagementService.UpdateProcessToApply(model.style_no);
                }

                int idProcessinfo = await _IdevManagementService.InsertToProductProcess(item);
                item.id = idProcessinfo;

                return Json(new { result = true, message = Constant.Success, data = item }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> ModifyProductProcess([FromBody] ProductProcess model)
        {
            try
            {
                string user = Session["userid"] == null ? null : Session["userid"].ToString();
                if (string.IsNullOrEmpty(user))
                {
                    return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
                }
                //kiểm tra có tồn tại không
                var ProductProcess = await _IdevManagementService.GetcheckProcessByStyle(model.id);
                if (ProductProcess == null)
                {
                    return Json(new { result = false, message = Constant.DataExist }, JsonRequestBehavior.AllowGet);
                }


                ProductProcess.process_name = model.process_name;
                ProductProcess.description = model.description;
                ProductProcess.IsApply = model.IsApply;

                ProductProcess.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                ProductProcess.chg_dt = DateTime.Now;

                if (model.IsApply == "Y")
                {
                    await _IdevManagementService.UpdateProcessToApply(ProductProcess.style_no);
                }
                await _IdevManagementService.UpdateToProductProcess(ProductProcess);

                return Json(new { result = true, message = Constant.Success, data = ProductProcess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpDelete]
        public async Task<JsonResult> DeleteProductProcess(int id)
        {
            try
            {
                //kiểm tra có tồn tại không
                var ProductProcess = await _IdevManagementService.GetcheckProcessByStyle(id);
                if (ProductProcess == null)
                {
                    return Json(new { result = false, message = Constant.DataExist }, JsonRequestBehavior.AllowGet);
                }
                //kiểm tra công đoạn này có detail chưa trong bảng d_routing_info
                var checkMAkeProcess = await _IdevManagementService.GetListRoutingInfoByStyleNo(ProductProcess.style_no, ProductProcess.process_code.ToString());
                if (checkMAkeProcess.Count() > 0)
                {
                    return Json(new { result = false, message = "Có công đoạn rồi không xóa được" }, JsonRequestBehavior.AllowGet);
                }
                await _IdevManagementService.DeleteProductProcessForId(id);
                return Json(new { result = true, message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion Routing

        public ActionResult Resource()
        {
            return SetLanguage("");
        }

        #region BobbinMgt

        public ActionResult BobbinMgt()
        {
            return SetLanguage("");
        }

        [HttpPost]
        public async Task<ActionResult> insertBobbinMgt(BobbinInfo bobbinInfo, string bb_no, string purpose, string bb_nm, string re_mark, string mc_type)
        {
            int count = await _IdevManagementService.CheckBobbinInfo(bb_no);
            if (count > 0)
            {
                return Json(new { result = false, message = "Đã tồn tại " + bb_no + " này ở trong hệ thống !" }, JsonRequestBehavior.AllowGet);
            }
            bobbinInfo.reg_dt = DateTime.Now;
            bobbinInfo.chg_dt = DateTime.Now;
            bobbinInfo.bb_no = bb_no;
            bobbinInfo.purpose = purpose;
            bobbinInfo.bb_nm = bb_nm;
            bobbinInfo.re_mark = re_mark;
            bobbinInfo.barcode = bb_no;
            bobbinInfo.mc_type = mc_type;
            bobbinInfo.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
            bobbinInfo.reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();

            var bobbin_Id = await _IdevManagementService.InsertIntoBobbinInfo(bobbinInfo);
            var rs = await _IdevManagementService.GetBobbinInfoById(bobbin_Id);
            return Json(new { result = true, message = Constant.Success, kq = rs }, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public async Task<ActionResult> updateBobbinMgt(int bno, string bb_no, string purpose, string bb_nm, string re_mark, string mc_type)
        {

            var bobbinInfo = await _IdevManagementService.GetBobbinInfoById(bno);
            if (bobbinInfo == null)
            {
                return Json(new { result = false, message = "Không tìm thấy " + bobbinInfo.bb_nm + " trong hệ thống" }, JsonRequestBehavior.AllowGet);
            }
            bobbinInfo.purpose = purpose;
            bobbinInfo.bb_nm = bb_nm;
            bobbinInfo.re_mark = re_mark;
            bobbinInfo.mc_type = mc_type;
            bobbinInfo.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
            bobbinInfo.reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
            bobbinInfo.chg_dt = DateTime.Now; // Or your date, as long as it is in DateTime format
            bobbinInfo.reg_dt = DateTime.Now;
            await _IdevManagementService.UpdateBobbinInfo(bobbinInfo);
            var rs = await _IdevManagementService.GetBobbinInfoById(bobbinInfo.bno);
            return Json(new { result = true, message = Constant.Success, kq = rs }, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public async Task<JsonResult> deleteBobbinMgt(int bno)
        {
            var bobbinInfo = await _IdevManagementService.GetBobbinInfoById(bno);

            if (bobbinInfo == null)
            {
                return Json(new { result = false, message = "Không tìm thấy " + bobbinInfo.bb_nm + " trong hệ thống" }, JsonRequestBehavior.AllowGet);
            }
            await _IdevManagementService.DeleteBobbinInfo(bobbinInfo.bno);
            return Json(new { result = true, message = Constant.Success }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> searchBobbinMgt_TSD(string s_bb_no, string s_bb_nm)
        {
            var data = await _IdevManagementService.GetListBobbinInfo(s_bb_no, s_bb_nm);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _Bobbin(string id)
        {
            ViewData["Message"] = id;
            return View();
        }

        public async Task<ActionResult> PrintMultiBobbinQRCode(string id)
        {
            var multiIDs = id.TrimStart('[').TrimEnd(']').Split(',');
            var row_data = new List<BobbinInfo>();
            for (int i = 0; i < multiIDs.Length; i++)
            {
                var id2 = int.Parse(multiIDs[i]);
                var data = await _IdevManagementService.GetBarCodeOfBobbinInfo(id2);
                row_data.AddRange(data);
            }

            return Json(row_data, JsonRequestBehavior.AllowGet);
        }

        #endregion BobbinMgt

        #region Bominfo
        public ActionResult BomMgt()
        {
            return SetLanguage("");
        }
        public async Task<ActionResult> GetBom(Pageing pageing)
        {
            try
            {
                string bom_no = Request["bom_no"] == null ? "" : Request["bom_no"].Trim();
                string product = Request["product"] == null ? "" : Request["product"].Trim();
                string product_nm = Request["product_nm"] == null ? "" : Request["product_nm"].Trim();
                string md_cd = Request["md_cd"] == null ? "" : Request["md_cd"].Trim();
                string mt_no = Request["mt_no"] == null ? "" : Request["mt_no"].Trim();
                string mt_nm = Request["mt_no"] == null ? "" : Request["mt_nm"].Trim();

                var listData = await _IdevManagementService.GetListBomManagement(bom_no, product, product_nm, md_cd, mt_no, mt_nm);
                var result = listData.OrderByDescending(x => x.bid).ToList();
                int totals = result.Count();
                int totalPages = (int)Math.Ceiling((float)totals / pageing.rows);
                var jsonReturn = new
                {
                    total = totalPages,
                    page = pageing.page,
                    records = totals,
                    rows = result
                };
                return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Lỗi hệ thống!!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> getBomdsMaterial(Pageing pageing, string style_no)
        {
            try
            {
                string bom_no = Request["bom_no"] == null ? "" : Request["bom_no"].Trim();
                string product = Request["product"] == null ? "" : Request["product"].Trim();
                string product_nm = Request["product_nm"] == null ? "" : Request["product_nm"].Trim();
                string md_cd = Request["md_cd"] == null ? "" : Request["md_cd"].Trim();
                string mt_no = Request["mt_no"] == null ? "" : Request["mt_no"].Trim();
                string mt_nm = Request["mt_no"] == null ? "" : Request["mt_nm"].Trim();

                var result = await _IdevManagementService.GetListBomManagement(bom_no, style_no, product, product_nm, md_cd, mt_no, mt_nm);
                int totals = result.Count();
                int totalPages = (int)Math.Ceiling((float)totals / pageing.rows);
                var jsonReturn = new
                {
                    total = totalPages,
                    page = pageing.page,
                    records = totals,
                    rows = result.ToList()
                };
                return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Lỗi hệ thống!!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> getBomMaterial(int id)
        {
            var value = await _IdevManagementService.GetListMaterialForBom(id);
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> getBomdsMaterialCap3(string style_no, string mt_no)
        {

            var value = await _IdevManagementService.GetListMaterialForBom(style_no, mt_no);
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CreateBomManagement([FromBody] CreateBomMgtModel model)
        {
            if (!ModelState.IsValid)
                return null;
            try
            {

                //check session
                string user = Session["userid"] == null ? null : Session["userid"].ToString();
                if (string.IsNullOrEmpty(user))
                {
                    return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
                }
                //kiểm tra trong bom đã có tồn tại không
                var rs = await _IdevManagementService.CheckExistedBonInfo(model.ProductCode, model.materialNo);
                if (rs > 0)
                {
                    return Json(new { result = false, message = Constant.DataExist }, JsonRequestBehavior.AllowGet);
                }

                var Bom = new BomInfo();
                Bom.bom_no = Guid.NewGuid().ToString();
                Bom.style_no = model.ProductCode;
                Bom.mt_no = model.materialNo;
                Bom.cav = model.cavit;
                Bom.buocdap = Convert.ToDouble(model.buocdap);
                Bom.need_time = model.need_time;
                var result = Math.Round(Convert.ToDecimal(Convert.ToDouble(model.buocdap) / 1000 / model.cavit * model.need_time), 3);
                Bom.need_m = Convert.ToDouble(result);
                Bom.reg_dt = DateTime.Now;
                Bom.chg_dt = DateTime.Now;
                Bom.del_yn = "N";
                Bom.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                Bom.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();

                if (model.isActive)
                {
                    await _IdevManagementService.UpdateBomToCalculatePerformance(model.ProductCode);
                }
                if (model.isActive)
                {
                    Bom.IsActive = 1;
                }
                else
                {
                    Bom.IsActive = 0;
                }

                int bomId = await _IdevManagementService.InsertToBomInfo(Bom);
                var bomResponse = await _IdevManagementService.GetBomManagementById(bomId);
                return Json(new { result = true, message = Constant.Success, data = bomResponse }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPut]
        public async Task<JsonResult> ModifyBomManagement(BomInfo table, int? isActive)
        {
            if (!ModelState.IsValid)
                return null;
            try
            {
                string user = Session["userid"] == null ? null : Session["userid"].ToString();
                if (string.IsNullOrEmpty(user))
                {
                    return Json(new { result = false, message = "Vui lòng đăng nhập lại, tài khoản bạn không tìm thấy" }, JsonRequestBehavior.AllowGet);
                }
                //kiểm tra trong bom đã có tồn tại không
                var bom = await _IdevManagementService.GetBomInfo(table.bid);
                if (bom == null)
                {
                    return Json(new { result = false, message = Constant.DataExist }, JsonRequestBehavior.AllowGet);
                }
                var rs = await _IdevManagementService.CheckBom(table.style_no, table.mt_no, table.bid);
                if (rs > 0)
                {
                    return Json(new { result = false, message = "Product này không có tồn tại ở trong hệ thống !" }, JsonRequestBehavior.AllowGet);
                }

                bom.cav = table.cav;
                bom.need_time = table.need_time;
                bom.buocdap = table.buocdap;
                var result = Math.Round(Convert.ToDecimal(table.buocdap / 1000 / table.cav * table.need_time), 3);
                bom.need_m = Convert.ToDouble(result);
                bom.mt_no = table.mt_no;
                bom.style_no = table.style_no;
                bom.isapply = "N";
                bom.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                bom.IsActive = isActive;
                if (isActive == 1)
                {
                    await _IdevManagementService.UpdateBomToCalculatePerformance(table.style_no);
                }

                await _IdevManagementService.UpdateBomInfo(bom);
                var BomInfo = await _IdevManagementService.GetListBomÌnfo(bom.bid);
                return Json(new { result = true, message = Constant.Success, data = BomInfo }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public async Task<ActionResult> ADdBomManagement([FromBody] BomCreateMaterialReponse model)
        {
            try
            {
                //kierm tra product trong bảng bom_info
                //var checkBOmExist = _IDomMS.CheckBomExist(model.ProductCode,model.MaterialPrarent);
                var checkBOmExist = await _IdevManagementService.CheckBomExist(model.productCode, model.materialprarent);
                if (checkBOmExist == null)
                {
                    return Json(new { result = false, message = "BOM này khồng tồn tại ở trong hệ thống !" }, JsonRequestBehavior.AllowGet);
                }

                var ListBomMaterialExist = await _IdevManagementService.ListExistsMaterialBomDetail(model.productCode, model.materialprarent, model.ListMaterial);
                if (ListBomMaterialExist.Count() > 0)
                {
                    await _IdevManagementService.DeleteBomMaterialExists(ListBomMaterialExist);
                }

                //insert material lại
                for (int i = 0; i < model.ListMaterial.Count; i++)
                {
                    var MaterialBOM = new BomCreateMaterialReponse();
                    MaterialBOM.productCode = model.productCode;
                    MaterialBOM.materialprarent = model.materialprarent;
                    MaterialBOM.materialno = model.ListMaterial[i];
                    MaterialBOM.CreateDate = DateTime.Now;
                    MaterialBOM.ChangeDate = DateTime.Now;
                    MaterialBOM.CreateId = Session["userid"] == null ? null : Session["userid"].ToString();
                    MaterialBOM.ChangeId = Session["userid"] == null ? null : Session["userid"].ToString();
                    //_IDomMS.InsertToMaterialBom(MaterialBOM);
                    await _IdevManagementService.InsertToMaterialBom(MaterialBOM);
                }
                //int Result = db.Database.ExecuteSqlCommand(sql);
                var value = await _IdevManagementService.GetListMaterialForBom(model.productCode, model.materialprarent);
                return Json(new { result = true, message = "", data = value }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> DeleteMaterialBomManagement(int? id)
        {
            int res = await _IdevManagementService.DeleteMaterialInfo((int)id);
            //_IDomMS.DeleteMaterialBomForId(id);
            return Json(new { result = true, res }, JsonRequestBehavior.AllowGet);
        }
        [HttpDelete]
        public async Task<JsonResult> DelMaterialBomManagement(int bid)
        {
            try
            {
                var bomInfo = await _IdevManagementService.GetBomInfo(bid);
                if (bomInfo == null)
                {
                    return Json(new { result = false, message = "Không tìm thấy dữ liệu ở trong hệ thống" }, JsonRequestBehavior.AllowGet);

                }
                await _IdevManagementService.DelBomMaterial(bid);
                return Json(new { result = true, message = "Xóa thành công", id = bid }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpDelete]
        public async Task<JsonResult> deletebom(int bid)
        {
            try
            {
                var bomInfo = await _IdevManagementService.GetBomInfo(bid);
                if (bomInfo == null)
                {
                    return Json(new { result = false, message = "Không tìm thấy dữ liệu ở trong hệ thống" }, JsonRequestBehavior.AllowGet);

                }
                await _IdevManagementService.DeleteBomInfo(bomInfo.style_no);
                return Json(new { result = true, message = "Xóa thành công", id = bomInfo.bid }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Upload_BOM(BomInfo table, List<BomInsertToExcelRequest> ListBomInsert)
        {
            try
            {
                var count = 0;
                var kiemtra = 0;
                var style = 0;
                var mt_cd = 0;
                var exit_style = 0;
                var cav = 0;
                var buocdap = 0;
                var need_time = 0;
                var bom = "";
                var danhsach = new ArrayList();
                foreach (var item in ListBomInsert)
                {
                    if (item.cav == null)
                    {
                        kiemtra++;
                        cav++;
                    }
                    if (item.buocdap == null)
                    {
                        kiemtra++;
                        buocdap++;
                    }
                    if (item.need_time == null)
                    {
                        kiemtra++;
                        need_time++;
                    }
                    //kiểm tra trong bom đã có tồn tại nguyên vật liệu này chưa
                    var checkStyleNo = await _IdevManagementService.CheckBom(item.style_no);
                    if (checkStyleNo > 0 && count == 0)
                    {
                        kiemtra++;
                        style++;
                    }

                    var ch_mt_no = await _IdevManagementService.GetMaterialNoFromMaterialInfo(item.mt_no);
                    if (ch_mt_no == null)
                    {
                        kiemtra++;
                        mt_cd++;
                    }

                    //check tồn tại product này không
                    var pr = await _IdevManagementService.GetStyleNoFromStyleInfo(item.style_no);
                    if (pr == null)
                    {
                        kiemtra++;
                        exit_style++;
                    }

                    if (kiemtra == 0)
                    {
                        if (bom == "")
                        {
                            var menuCd = string.Empty;
                            var subMenuIdConvert = 0;
                            var ds = await _IdevManagementService.CheckBom();
                            if (ds == 0)
                            {
                                table.bom_no = "B0000000001";
                            }
                            else
                            {
                                var rs = await _IdevManagementService.GetListBom();
                                var list = rs.FirstOrDefault();
                                var subMenuId = list.bom_no.Substring(list.bom_no.Length - 10, 10);
                                int.TryParse(subMenuId, out subMenuIdConvert);
                                menuCd = "B" + string.Format("{0}{1}", menuCd, CreateId((subMenuIdConvert + 1)));
                                table.bom_no = menuCd;
                            }
                            bom = table.bom_no;
                        }
                        else
                        {
                            table.bom_no = bom;
                        }
                        table.buocdap = item.buocdap;
                        table.cav = item.cav;
                        table.need_time = item.need_time;
                        var vvv = Math.Round(Convert.ToDecimal((item.buocdap / 1000 / item.cav * item.need_time)), 2);
                        table.need_m = ((float?)vvv);
                        table.mt_no = ch_mt_no?.mt_no;
                        table.style_no = pr?.style_no;
                        table.reg_dt = DateTime.Now;
                        table.chg_dt = DateTime.Now;
                        table.del_yn = "N";
                        table.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        table.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                        if (ModelState.IsValid)
                        {
                            //db.Entry(table).State = EntityState.Added;
                            //db.SaveChanges();
                            await _IdevManagementService.InsertToBomInfo(table);

                            var data = new
                            {
                                bid = table.bid,
                                bom_no = table.bom_no,
                                mt_no = table.mt_no,
                                mt_nm = ch_mt_no?.mt_nm,
                                style_no = table.style_no,
                                style_nm = pr?.style_nm,
                                md_cd = pr?.md_cd,
                                need_time = table.need_time,
                                cav = table.cav,
                                need_m = table.need_m,
                                buocdap = table.buocdap,
                                reg_dt = table.reg_dt,
                            };
                            danhsach.Add(data);
                        }
                    }
                    count++;
                }
                if (kiemtra == 0 && danhsach.Count > 0)
                {
                    return Json(new
                    {
                        result = true,
                        message = Constant.Success,
                        danhsach = danhsach
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.DataExist, style, mt_cd, exit_style, cav, need_time, buocdap }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpDelete]
        public async Task<ActionResult> DelMaterialBom(int Id)
        {
            var result = await _IdevManagementService.DelMaterialBom(Id);
            if (result > 0)
            {
                return Json(new { result = true, message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = "Không thể xóa mã này" }, JsonRequestBehavior.AllowGet);

        }


        private string CreateId(int id)
        {
            if (id.ToString().Length < 2)
            {
                return string.Format("000000000{0}", id);
            }

            if ((id.ToString().Length < 3) || (id.ToString().Length == 2))
            {
                return string.Format("00000000{0}", id);
            }

            if ((id.ToString().Length < 4) || (id.ToString().Length == 3))
            {
                return string.Format("0000000{0}", id);
            }

            if ((id.ToString().Length < 5) || (id.ToString().Length == 4))
            {
                return string.Format("000000{0}", id);
            }
            if ((id.ToString().Length < 6) || (id.ToString().Length == 5))
            {
                return string.Format("00000{0}", id);
            }
            if ((id.ToString().Length < 7) || (id.ToString().Length == 6))
            {
                return string.Format("0000{0}", id);
            }
            if ((id.ToString().Length < 8) || (id.ToString().Length == 7))
            {
                return string.Format("000{0}", id);
            }
            if ((id.ToString().Length < 9) || (id.ToString().Length == 8))
            {
                return string.Format("00{0}", id);
            }
            if ((id.ToString().Length < 10) || (id.ToString().Length == 9))
            {
                return string.Format("0{0}", id);
            }

            if ((id.ToString().Length < 11) || (id.ToString().Length == 10))
            {
                return string.Format("{0}", id);
            }
            return string.Empty;
        }
        #endregion

        #region Page Staus
        public ActionResult Status()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> DetailContainerComposite([FromBody] DetailContainerCompositeReq req)
        {
            string userid = "";
            if (!String.IsNullOrEmpty(req.bb_no))
            {
                //kiem tra no có ở history không
                var ds_dung = "";
                var trangthai = "";
                if (!string.IsNullOrEmpty(req.bb_no))
                {
                    var checkbbnoMMS = await _IcommonService.FindOneWMaterialInfoLike(req.bb_no);
                    var checkbbnoTIMS = await _IcommonService.FindOneWMaterialInfoLikeTIMS(req.bb_no);
                    if(checkbbnoTIMS != null)
                    {
                        if (checkbbnoTIMS.mt_sts_cd == "005")
                        {
                            var ketluan = "";
                            var time_mapping = "";
                            var lct_cd = "";
                            var listActual = await _IdevManagementService.GetListActual(checkbbnoTIMS.id_actual);
                            var actual = listActual.SingleOrDefault();
                            var process = await checkcondoan(actual.name);
                            var time_mapping_tims = "";
                            var csv = await _ITimsService.GetNameStatusCommCode(checkbbnoTIMS.mt_sts_cd);
                            var lct_cd_tims = (checkbbnoTIMS.lct_cd.StartsWith("006") != null ? "TIMS" : "MMS");
                            if (lct_cd_tims.Equals("TIMS"))
                            {
                                time_mapping_tims = checkbbnoTIMS.reg_dt.ToString("dd-MM-yyyy HH:mm:ss");
                            }

                            if (checkbbnoTIMS.lct_cd.StartsWith("003") || checkbbnoTIMS.lct_cd.StartsWith("004"))
                            {
                                lct_cd_tims = "FG";
                            }
                            time_mapping = checkbbnoTIMS.reg_dt.ToString("dd-MM-yyyy HH:mm:ss");
                            var cong_nhan = await _IdevManagementService.GetStaffIdByIdActualForTIMS(checkbbnoTIMS.id_actual);
                            var data = new
                            {
                                SD = "",
                                bobin = req.bb_no,
                                trangthai = csv,
                                lct_cd = lct_cd_tims,
                                process = process,
                                product = actual.product,
                                po = actual.at_no,
                                time_mapping = time_mapping_tims,
                                cong_nhan = cong_nhan,
                                ketluan = ketluan = "Đang ở công đoạn : " + process,
                                sanluong = checkbbnoTIMS.gr_qty,

                            };
                            return Json(new { result = true, data, number = 3 }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if(checkbbnoMMS != null)
                    {
                        if (checkbbnoMMS.mt_sts_cd == "005")
                        {
                            var ketluan = "";
                            var time_mapping = "";
                            var lct_cd = "";
                            var listActual = await _IdevManagementService.GetListActual(checkbbnoMMS.id_actual);
                            var actual = listActual.SingleOrDefault();
                            var process = await checkcondoan(actual.name);
                            var csv = await _ITimsService.GetNameStatusCommCode(checkbbnoMMS.mt_sts_cd);
                            var lct_cd_tims = (checkbbnoMMS.lct_cd.StartsWith("002") != null ? "MMS" : "TIMS");
                            if (lct_cd_tims.Equals("TIMS"))
                            {
                                time_mapping = checkbbnoMMS.reg_dt.ToString("dd-MM-yyyy HH:mm:ss");
                            }

                            if (checkbbnoMMS.lct_cd.StartsWith("003") || checkbbnoMMS.lct_cd.StartsWith("004"))
                            {
                                lct_cd_tims = "FG";
                            }
                            time_mapping = checkbbnoMMS.reg_dt.ToString("dd-MM-yyyy HH:mm:ss");
                            var cong_nhan = await _IdevManagementService.GetStaffIdByIdActual(checkbbnoMMS.id_actual);
                            var data = new
                            {
                                SD = "",
                                bobin = req.bb_no,
                                trangthai = csv,
                                lct_cd = lct_cd_tims,
                                process = process,
                                product = actual.product,
                                po = actual.at_no,
                                time_mapping = time_mapping,
                                cong_nhan = cong_nhan,
                                ketluan = ketluan = "Đang ở công đoạn : " + process,
                                sanluong = checkbbnoMMS.gr_qty,

                            };
                            return Json(new { result = true, data, number = 3 }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                var history_bb = await _IdevManagementService.GetListBobbinLctHist(req.bb_no);
                if (history_bb.Count() == 1)
                {
                    //kiểm tra n có ở primary bb không
                    //nếu mã bobin rỗng thì update nó
                    var bobin_cha = await _IdevManagementService.GetBobbinInfoByBB_No(req.bb_no);
                    if (bobin_cha != null)
                    {
                        if (String.IsNullOrEmpty(bobin_cha.mt_cd))
                        {
                            await _IcommonService.UpdateBobbinInfo(req.bb_no, history_bb.FirstOrDefault().mt_cd, userid, DateTime.Now);
                        }
                    }
                    //kiểm tra mã đó đang ở đâu nhỉ
                    //ĐANG Ở ĐÂU
                    var mt_cd_bb = history_bb.FirstOrDefault().mt_cd;

                    var primary = await _IcommonService.GetMaterialInfoMMS(mt_cd_bb);
                    //nếu trong bảng tạm không có kiểm tra bảng chính tồn tại không
                    var lct_cd = "";
                    if (primary != null)
                    {
                        var mt_sts_cd = primary.status;
                        trangthai = await checktrangthai(mt_sts_cd);
                        lct_cd = (primary.location_code.StartsWith("002") ? "WIP" : "TIMS");
                        var process = "";
                        var poroduct = "";
                        var po = "";
                        var ketluan = "";
                        var time_mapping = "";
                        var cong_nhan = "";
                        var bobin = (primary.bb_no != null ? primary.bb_no : "");
                        if (primary.id_actual != 0 && primary.id_actual != null)
                        {

                            var listMaterialMapping = await _IdevManagementService.GetListMaterialMappingMMS(mt_cd_bb);
                            var ds = listMaterialMapping.FirstOrDefault();

                            var listActual = await _IdevManagementService.GetListActual(primary.id_actual);
                            var actual = listActual.SingleOrDefault();

                            var bobin_mapping = "";

                            if (ds != null)
                            {
                                var mt = ds.mt_lot;

                                var listMaterialInfoMMS = await _IdevManagementService.GetListDataMaterialInfoMMS(mt);
                                var check_mt_mappng = listMaterialInfoMMS.SingleOrDefault();

                                var listActual2 = await _IdevManagementService.GetListActual(check_mt_mappng.id_actual);
                                var actual2 = listActual2.SingleOrDefault();

                                bobin_mapping = " Và mapping với container này :" + check_mt_mappng.bb_no;
                                process = await checkcondoan(actual2.name);

                                cong_nhan = await _IdevManagementService.GetStaffIdByIdActual(check_mt_mappng.id_actual);
                                time_mapping = (ds.reg_date).ToString("dd-MM-yyyy HH:mm:ss");
                                ketluan = "Đã Được Mapping ở công đoạn này: " + process + " Với Đồ Đựng Này : " + check_mt_mappng.bb_no;
                            }
                            else
                            {
                                if (lct_cd.Equals("WIP"))
                                {
                                    time_mapping = primary.reg_date.ToString("dd-MM-yyyy HH:mm:ss");
                                }

                                if (primary.location_code.StartsWith("003") || primary.location_code.StartsWith("004"))
                                {
                                    lct_cd = "FG";
                                }
                                process = await checkcondoan(actual.name);
                                ketluan = "Đang ở công đoạn : " + process;

                                cong_nhan = await _IdevManagementService.GetStaffIdByIdActual(primary.id_actual);

                            }

                            po = actual.at_no;
                            var actual_primary = await _IdevManagementService.GetActualPrimaryById(po);
                            poroduct = (actual_primary != null ? actual_primary.product : "");
                            //công đoạn
                            time_mapping = primary.reg_date.ToString("dd-MM-yyyy HH:mm:ss");
                        }
                        else
                        {
                            ketluan = "Vẫn chưa được mapping";
                        }
                        var data = new
                        {
                            SD = "",
                            bobin = bobin,
                            trangthai = trangthai,
                            lct_cd = lct_cd,
                            process = process,
                            product = poroduct,
                            po = po,
                            time_mapping = time_mapping,
                            sanluong = primary.gr_qty,
                            cong_nhan = cong_nhan,
                            ketluan = ketluan,
                        };
                        return Json(new { result = true, data, number = 4 }, JsonRequestBehavior.AllowGet);
                    }

                    //Tìm kiếm bobbin bên TIMS
                    var primaryMaterialInfoTims= await _ITimsService.GetMaterialInfoTIMS(mt_cd_bb);
                    //nếu trong bảng tạm không có kiểm tra bảng chính tồn tại không
                    var lct_cd_tims = "";
                    if (primaryMaterialInfoTims != null)
                    {
                        var mt_sts_cd_tims = primaryMaterialInfoTims.status;
                        trangthai = await checktrangthai(mt_sts_cd_tims);
                        lct_cd_tims = (primaryMaterialInfoTims.location_code.StartsWith("006") != null ? "TIMS" : "MMS");
                        var process = "";
                        var poroduct = "";
                        var po = "";
                        var ketluan = "";
                        var time_mapping_tims = "";
                        var cong_nhan = "";
                        var bobin = (primaryMaterialInfoTims.bb_no != null ? primaryMaterialInfoTims.bb_no : "");
                        if (primaryMaterialInfoTims.id_actual != 0 && primaryMaterialInfoTims.id_actual != null)
                        {
                            var listMaterialMappingTims = await _ITimsService.GetListMaterialMappingTIMS(mt_cd_bb);
                            var dsTIMS = listMaterialMappingTims.FirstOrDefault();

                            var listActual = await _IdevManagementService.GetListActual(primaryMaterialInfoTims.id_actual);
                            var actual = listActual.SingleOrDefault();

                            var bobin_mapping_tims = "";
                            if (dsTIMS != null)
                            {
                                var mt = dsTIMS.mt_lot;

                                var listMaterialInfoTIMS = await _ITimsService.GetListDataMaterialInfoTIMSStatus(mt);
                                var check_mt_mappng_tims = listMaterialInfoTIMS.SingleOrDefault();

                                var listActual2 = await _IdevManagementService.GetListActual(check_mt_mappng_tims.id_actual);
                                var actual2 = listActual2.SingleOrDefault();

                                bobin_mapping_tims = " Và mapping với container này :" + check_mt_mappng_tims.bb_no;
                                process = await checkcondoan(actual2.name);

                                cong_nhan = await _IdevManagementService.GetStaffIdByIdActualForTIMS(check_mt_mappng_tims.id_actual);
                                time_mapping_tims = (dsTIMS.reg_date).ToString("dd-MM-yyyy HH:mm:ss");
                                ketluan = "Đã Được Mapping ở công đoạn này: " + process + " Với Đồ Đựng Này : " + check_mt_mappng_tims.bb_no;
                            }
                            else
                            {
                                if (lct_cd_tims.Equals("TIMS"))
                                {
                                    time_mapping_tims = primaryMaterialInfoTims.reg_date.ToString("dd-MM-yyyy HH:mm:ss");
                                }

                                if (primaryMaterialInfoTims.location_code.StartsWith("003") || primaryMaterialInfoTims.location_code.StartsWith("004"))
                                {
                                    lct_cd_tims = "FG";
                                }
                                process = await checkcondoan(actual.name);
                                ketluan = "Đang ở công đoạn : " + process;

                                cong_nhan = await _IdevManagementService.GetStaffIdByIdActualForTIMS(primaryMaterialInfoTims.id_actual);
                            }
                            po = actual.at_no;
                            var actual_primary = await _IdevManagementService.GetActualPrimaryById(po);
                            poroduct = (actual_primary != null ? actual_primary.product : "");
                            //công đoạn
                            time_mapping_tims = primaryMaterialInfoTims.reg_date.ToString("dd-MM-yyyy HH:mm:ss");
                        }
                        else
                        {
                            ketluan = "Vẫn chưa được mapping";
                        }
                        var data = new
                        {
                            SD = "",
                            bobin = bobin,
                            trangthai = trangthai,
                            lct_cd = lct_cd_tims,
                            process = process,
                            product = poroduct,
                            po = po,
                            time_mapping = time_mapping_tims,
                            sanluong = primaryMaterialInfoTims.gr_qty,
                            cong_nhan = cong_nhan,
                            ketluan = ketluan,
                        };
                        return Json(new { result = true, data, number = 4 }, JsonRequestBehavior.AllowGet);
                    }
                }
                else if (history_bb.Count() > 1)
                {
                    //nếu nhiều hơn 1 thì n đang đựng nhiều mã kiểm tra lại
                    var csv = string.Join(", ", history_bb.Select(x => x.mt_cd));
                    trangthai = "Đồ đựng này đựng nhiều mã hơn 1 :" + csv;
                    return Json(new { result = true, number = 5, trangthai }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    //nếu không có thì kiểm tra xem lần cuối bb này đựng cái mã gì

                    // kiểm tra mã này trạng thái 011 không
                    //var dataes = db.w_material_info.Where(x => x.bb_no == bb_no && x.mt_sts_cd.Equals("011")).OrderByDescending(x => x.wmtid).FirstOrDefault();

                    var checkTIMS = await _ITimsService.GetwmtinfotimswithBobbin("011", req.bb_no);
                    var checkMMS = await _ITimsService.GetwmtinfoMMSwithBobbin("011", req.bb_no);


                    if (checkTIMS != null)
                    {
                        //var actual2 = db.w_actual.Where(x => x.id_actual == dataes.id_actual).SingleOrDefault();
                        var actual2 = await _ITimsService.GetWActual(checkTIMS.id_actual);
                        var process = await checkcondoan(actual2.name);
                        var data = new
                        {
                            SD = checkTIMS.wmtid,
                            bobin = checkTIMS.bb_no,
                            trangthai = "Đã bị hủy",
                            lct_cd = checkTIMS.location_code.StartsWith("002") ? "WIP" : "TIMS",
                            process = process,
                            product = checkTIMS.product,
                            po = checkTIMS.at_no,
                            cong_nhan = checkTIMS.staff_id,
                            ketluan = "Trạng thái trước khi bị hủy là: " + await checktrangthai(checkTIMS.sts_update) + " </br>Tài khoản HỦY: " + checkTIMS.chg_id + "  </br>Thời gian HỦY: " + Convert.ToDateTime(checkTIMS.chg_date.ToString()).ToString("dd-MM-yyyy HH:mm:ss"),
                            sanluong = checkTIMS.gr_qty,
                            time_mapping = Convert.ToDateTime(checkTIMS.reg_date.ToString()).ToString("dd-MM-yyyy HH:mm:ss")
                        };
                        return Json(new { result = true, data, number = 3 }, JsonRequestBehavior.AllowGet);
                    };

                    if (checkMMS != null)
                    {
                        //var actual2 = db.w_actual.Where(x => x.id_actual == dataes.id_actual).SingleOrDefault();
                        var id_actual = checkMMS.id_actual;
                        var dataAtno = await _ITimsService.GetWActual(id_actual.ToInt());
                        var process = await checkcondoan(dataAtno.name);
                        var data = new
                        {
                            SD = checkMMS.wmtid,
                            bobin = checkMMS.bb_no,
                            trangthai = "Đã bị hủy",
                            lct_cd = checkMMS.location_code.StartsWith("002") ? "WIP" : "TIMS",
                            process = process,
                            product = dataAtno.product,
                            po = dataAtno.at_no,
                            cong_nhan = checkMMS.staff_id,
                            ketluan = "Trạng thái trước khi bị hủy là: " + await checktrangthai(checkMMS.sts_update) + " </br>Tài khoản HỦY: " + checkMMS.chg_id + "  </br>Thời gian HỦY: " + Convert.ToDateTime(checkMMS.chg_date.ToString()).ToString("dd-MM-yyyy HH:mm:ss"),
                            sanluong = checkMMS.gr_qty,
                            time_mapping = Convert.ToDateTime(checkMMS.reg_date.ToString()).ToString("dd-MM-yyyy HH:mm:ss"),
                        };
                        return Json(new { result = true, data, number = 3 }, JsonRequestBehavior.AllowGet);
                    };
                    //kêu ngta kiểm tra mã cũ này xem tại sao bị xã
                    trangthai = "Bobbin này đang rỗng";
                    return Json(new { result = true, number = 6, trangthai }, JsonRequestBehavior.AllowGet);

                }
            }
            if (!String.IsNullOrEmpty(req.mt_cd))
            {

                //ĐANG Ở ĐÂU
                var checkMaterialMappingOfBobbin = await _actualWOService.CheckMaterialMappingOfBobbbin(req.mt_cd);

                if (checkMaterialMappingOfBobbin == 0)
                {
                    var listMaterialInfoTIM = await _IdevManagementService.GetListMaterialInfoTIMS(req.mt_cd);
                    var primary = listMaterialInfoTIM.SingleOrDefault();

                    //nếu trong bảng tạm không có kiểm tra bảng chính tồn tại không
                    var trangthai = "";
                    var lct_cd = "";
                    if (primary != null)
                    {
                        var mt_sts_cd = primary.status;
                        trangthai = await checktrangthai(mt_sts_cd);
                        lct_cd = (primary.location_code.StartsWith("002") ? "WIP" : "TIMS");
                        var process = "";
                        var poroduct = "";
                        var po = "";
                        var ketluan = "";
                        var time_mapping = "";
                        var cong_nhan = "";

                        var bobin = (primary.bb_no != null ? primary.bb_no : "");
                        if (primary.id_actual != 0 && primary.id_actual != null)
                        {
                            var listMaterialMappingMMS = await _IdevManagementService.GetListMaterialMappingMMS(req.mt_cd);
                            var ds = listMaterialMappingMMS.FirstOrDefault();
                            var listActualMMS = await _IdevManagementService.GetListActual(primary.id_actual);
                            var actual = listActualMMS.SingleOrDefault();
                            var bobin_mapping = "";
                            if (ds != null)
                            {
                                var mt = ds.mt_lot;

                                var listCheckMaterialMappingMMS = await _IdevManagementService.GetListDataMaterialInfoMMS(mt);
                                var check_mt_mappng = listCheckMaterialMappingMMS.SingleOrDefault();

                                var listActualMMS2 = await _IdevManagementService.GetListActual(check_mt_mappng.id_actual);
                                var actual2 = listActualMMS2.SingleOrDefault();
                                bobin_mapping = " Và mapping với container này :" + check_mt_mappng.bb_no;
                                process = await checkcondoan(actual2.name);
                                time_mapping = (ds.reg_date).ToString("dd-MM-yyyy HH:mm:ss");

                                cong_nhan = await _IdevManagementService.GetStaffIdByIdActual(check_mt_mappng.id_actual);

                                ketluan = "Đã Được Mapping ở công đoạn này: " + process + " Với Đồ Đựng Này : " + check_mt_mappng.bb_no;
                            }
                            else
                            {
                                if (primary.location_code.StartsWith("003") || primary.location_code.StartsWith("004"))
                                {
                                    lct_cd = "FG";
                                }
                                process = await checkcondoan(actual.name);
                                ketluan = "Đang ở công đoạn này: " + process;
                                if (req.mt_cd.Contains("ROT") || req.mt_cd.Contains("STA"))
                                {
                                    cong_nhan = await _IdevManagementService.GetStaffIdByIdActual(primary.id_actual);
                                }
                                else
                                {
                                    cong_nhan = await _IdevManagementService.GetStaffIdByIdActualForTIMS(primary.id_actual);
                                }

                            }

                            po = actual.at_no;
                            var actual_primary = await _IdevManagementService.GetActualPrimaryById(po);
                            poroduct = (actual_primary != null ? actual_primary.product : "");
                            //công đoạn
                            time_mapping = primary.reg_date.ToString("dd-MM-yyyy HH:mm:ss");
                        }
                        else
                        {
                            ketluan = "Vẫn chưa được mapping";
                        }
                        var data = new
                        {
                            SD = "",
                            bobin = bobin,
                            trangthai = trangthai,
                            lct_cd = lct_cd,
                            process = process,
                            product = poroduct,
                            po = po,
                            time_mapping = time_mapping,
                            cong_nhan = cong_nhan,
                            ketluan = ketluan,
                            sanluong = primary.gr_qty,

                        };
                        return Json(new { result = true, data, number = 3 }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, message = "Không tìm thấy mã ở trong hệ thống !" }, JsonRequestBehavior.AllowGet);
                    }
                }
                if (checkMaterialMappingOfBobbin == 1)
                {
                    var listMaterialInfoMMS = await _IdevManagementService.GetListDataInventoryProduct(req.mt_cd);
                    var primary = listMaterialInfoMMS.SingleOrDefault();

                    //nếu trong bảng tạm không có kiểm tra bảng chính tồn tại không
                    var trangthai = "";
                    var lct_cd = "";
                    if (primary != null)
                    {
                        var mt_sts_cd = primary.status;
                        trangthai = await checktrangthai(mt_sts_cd);
                        lct_cd = (primary.location_code.StartsWith("002") ? "WIP" : "TIMS");
                        var process = "";
                        var poroduct = "";
                        var po = "";
                        var ketluan = "";
                        var time_mapping = "";
                        var cong_nhan = "";

                        var bobin = (primary.bb_no != null ? primary.bb_no : "");
                        if (primary.id_actual != 0 && primary.id_actual != null)
                        {
                            var listMaterialMappingMMS = await _IdevManagementService.GetListMaterialMappingMMS(req.mt_cd);
                            var ds = listMaterialMappingMMS.FirstOrDefault();
                            var listActualMMS = await _IdevManagementService.GetListActual(primary.id_actual);
                            var actual = listActualMMS.SingleOrDefault();
                            var bobin_mapping = "";
                            if (ds != null)
                            {
                                var mt = ds.mt_cd;

                                var listCheckMaterialMappingMMS = await _IdevManagementService.GetListDataInventoryProduct(mt);
                                var check_mt_mappng = listCheckMaterialMappingMMS.SingleOrDefault();

                                var listActualMMS2 = await _IdevManagementService.GetListActual(check_mt_mappng.id_actual);
                                var actual2 = listActualMMS2.SingleOrDefault();
                                bobin_mapping = " Và mapping với container này :" + check_mt_mappng.bb_no;
                                process = await checkcondoan(actual2.name);
                                time_mapping = (ds.reg_date).ToString("dd-MM-yyyy HH:mm:ss");
                                //cong_nhan = check_mt_mappng.staff_id;
                                cong_nhan = await _IdevManagementService.GetStaffIdByIdActual(check_mt_mappng.id_actual);

                                ketluan = "Đã Được Mapping ở công đoạn này: " + process + " Với Đồ Đựng Này " + check_mt_mappng.bb_no;
                            }
                            else
                            {
                                if (primary.location_code.StartsWith("003") || primary.location_code.StartsWith("004"))
                                {
                                    lct_cd = "FG";
                                }
                                process = await checkcondoan(actual.name);
                                ketluan = "Đang ở công đoạn này: " + process;
                                //cong_nhan = primary.staff_id;
                                cong_nhan = await _IdevManagementService.GetStaffIdByIdActual(primary.id_actual);
                            }

                            po = actual.at_no;
                            var actual_primary = await _IdevManagementService.GetActualPrimaryById(po);
                            poroduct = (actual_primary != null ? actual_primary.product : "");
                            //công đoạn
                            time_mapping = primary.create_date.ToString("dd-MM-yyyy HH:mm:ss");
                        }
                        else
                        {
                            ketluan = "Vẫn chưa được mapping";
                        }
                        var data = new
                        {
                            SD = "",
                            bobin = bobin,
                            trangthai = trangthai,
                            lct_cd = lct_cd,
                            process = process,
                            product = poroduct,
                            po = po,
                            time_mapping = time_mapping,
                            cong_nhan = cong_nhan,
                            ketluan = ketluan,
                            sanluong = primary.gr_qty,

                        };
                        return Json(new { result = true, data, number = 3 }, JsonRequestBehavior.AllowGet);
                    }
                }
                if (checkMaterialMappingOfBobbin == 2)
                {
                    var listMaterialInfoMMS = await _IdevManagementService.GetListDataMaterialInfoMMS(req.mt_cd);
                    var primary = listMaterialInfoMMS.SingleOrDefault();

                    //nếu trong bảng tạm không có kiểm tra bảng chính tồn tại không
                    var trangthai = "";
                    var lct_cd = "";
                    if (primary != null)
                    {
                        var mt_sts_cd = primary.status;
                        trangthai = await checktrangthai(mt_sts_cd);
                        lct_cd = (primary.location_code.StartsWith("002") ? "WIP" : "TIMS");
                        var process = "";
                        var poroduct = "";
                        var po = "";
                        var ketluan = "";
                        var time_mapping = "";
                        var cong_nhan = "";

                        var bobin = (primary.bb_no != null ? primary.bb_no : "");
                        if (primary.id_actual != 0 && primary.id_actual != null)
                        {
                            var listMaterialMappingMMS = await _IdevManagementService.GetListMaterialMappingMMS(req.mt_cd);
                            var ds = listMaterialMappingMMS.FirstOrDefault();
                            var listActualMMS = await _IdevManagementService.GetListActual(primary.id_actual);
                            var actual = listActualMMS.SingleOrDefault();
                            var bobin_mapping = "";
                            if (ds != null)
                            {
                                var mt = ds.mt_lot;

                                var listCheckMaterialMappingMMS = await _IdevManagementService.GetListDataMaterialInfoMMS(mt);
                                var check_mt_mappng = listCheckMaterialMappingMMS.SingleOrDefault();

                                var listActualMMS2 = await _IdevManagementService.GetListActual(check_mt_mappng.id_actual);
                                var actual2 = listActualMMS2.SingleOrDefault();
                                bobin_mapping = " Và mapping với container này :" + check_mt_mappng.bb_no;
                                process = await checkcondoan(actual2.name);
                                time_mapping = (ds.reg_date).ToString("dd-MM-yyyy HH:mm:ss");
                                //cong_nhan = check_mt_mappng.staff_id;
                                cong_nhan = await _IdevManagementService.GetStaffIdByIdActual(check_mt_mappng.id_actual);

                                ketluan = "Đã Được Mapping ở công đoạn này: " + process + " Với Đồ Đựng Này : " + check_mt_mappng.bb_no;
                            }
                            else
                            {
                                if (primary.location_code.StartsWith("003") || primary.location_code.StartsWith("004"))
                                {
                                    lct_cd = "FG";
                                }
                                process = await checkcondoan(actual.name);
                                ketluan = "Đang ở công đoạn này: " + process;
                                //cong_nhan = primary.staff_id;
                                cong_nhan = await _IdevManagementService.GetStaffIdByIdActual(primary.id_actual);
                            }

                            po = actual.at_no;
                            var actual_primary = await _IdevManagementService.GetActualPrimaryById(po);
                            poroduct = (actual_primary != null ? actual_primary.product : "");
                            //công đoạn
                            time_mapping = primary.reg_date.ToString("dd-MM-yyyy HH:mm:ss");
                        }
                        else
                        {
                            ketluan = "Vẫn chưa được mapping";
                        }
                        var data = new
                        {
                            SD = "",
                            bobin = bobin,
                            trangthai = trangthai,
                            lct_cd = lct_cd,
                            process = process,
                            product = poroduct,
                            po = po,
                            time_mapping = time_mapping,
                            cong_nhan = cong_nhan,
                            ketluan = ketluan,
                            sanluong = primary.gr_qty
                        };
                        return Json(new { result = true, data, number = 3 }, JsonRequestBehavior.AllowGet);
                    }
                }

            }
            if (!String.IsNullOrEmpty(req.mt_no))
            {
                var SD = "";
                var trangthai = "";
                var lct_cd = "Chưa Scan vào kHO WIP";
                //kiểm tra mã này có trong bảng tạm không
                var listMaterialInfoTam = await _IdevManagementService.GetListDataMaterialInfoTam(req.mt_no);
                var tam = listMaterialInfoTam.SingleOrDefault();
                if (tam != null)
                {
                    //nếu có kiểm tra xem nó đã có SD chưa
                    SD = (tam.sd_no == null ? "" : tam.sd_no);
                    var ketluan = (SD == "" && tam.remark != "Không tìm thấy mã này" ? " Chưa scan mã này vào SD ở Picking Scan" : "Đã được đưa vào SD:" + SD + " Và chưa được Receiving Scan(WIP)");
                    lct_cd = (SD != "" ? "  WIP" : "");
                    //nếu chưa có sd kiểm tra xem n đã xóa lần nào chưa remark(Không tìm thấy mã này)
                    trangthai = (tam.remark == "" ? "Đã từng xóa mã này khỏi danh SD ở Picking Scan" : await checktrangthai(tam.status) + lct_cd);
                    var data = new
                    {
                        SD = SD,
                        trangthai = trangthai,
                        lct_cd = lct_cd,
                        ketluan = ketluan,
                        process = "",
                        product = "",
                        po = "",
                        sanluong = "",
                        time_mapping = "",
                    };
                    return Json(new { result = true, data, number = 1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var checkMaterialMappingOfBobbin = await _actualWOService.CheckMaterialMappingOfBobbbin(req.mt_no);
                    if (checkMaterialMappingOfBobbin == 0)
                    {
                        return Json(new { result = false, message = "Không tìm thấy " + req.mt_no + " trong hệ thống" });
                    }
                    if (checkMaterialMappingOfBobbin == 1)
                    {
                        var listMaterialInventoryProduct = await _IdevManagementService.GetListDataInventoryProduct(req.mt_no);
                        var primaryInventoryProduct = listMaterialInventoryProduct.SingleOrDefault();
                        if (primaryInventoryProduct != null)
                        {
                            SD = primaryInventoryProduct.sd_no + "</br>" + primaryInventoryProduct.ExportCode;
                            var mt_sts_cd = primaryInventoryProduct.status;
                            trangthai = await checktrangthai(mt_sts_cd);
                            var locationWip = primaryInventoryProduct.location_code;
                            var vitri = await checkvitri(locationWip);
                            lct_cd = (primaryInventoryProduct.location_code.StartsWith("002") ? "WIP" : "TIMS");
                            vitri = (primaryInventoryProduct.ExportCode != "" && primaryInventoryProduct.ExportCode != null ? "Đang ở máy" : vitri);
                            var process = "";
                            var poroduct = "";
                            var po = "";
                            var ketluan = "";
                            var time_mapping = "";
                            if (primaryInventoryProduct.id_actual != 0 && primaryInventoryProduct.id_actual != null)
                            {
                                var listDataActual = await _IdevManagementService.GetListActual(primaryInventoryProduct.id_actual);
                                var actual = listDataActual.SingleOrDefault();
                                po = actual.at_no;
                                var actual_primary = await _IdevManagementService.GetActualPrimaryById(po);

                                poroduct = (actual_primary != null ? actual_primary.product : "");
                                //công đoạn
                                process = await checkcondoan(actual.name);
                                ketluan = "Đã Được Mapping ở công đoạn này: " + process;

                                var listMaterialMappingMMS = await _IdevManagementService.GetListMaterialMappingMMS(req.mt_no);
                                var ds = listMaterialMappingMMS.FirstOrDefault();
                                if (ds != null)
                                {
                                    time_mapping = (ds.reg_date).ToString("dd-MM-yyyy HH:mm:ss");
                                }
                            }
                            else
                            {
                                ketluan = "Vẫn chưa được mapping";
                            }
                                var data = new
                                {
                                    SD = SD,
                                    trangthai = trangthai,
                                    lct_cd = lct_cd,
                                    process = process,
                                    product = poroduct,
                                    po = po,
                                    vitri = vitri,
                                    ketluan = ketluan,
                                    sanluong = primaryInventoryProduct.gr_qty,
                                    time_mapping = time_mapping,
                                };
                                return Json(new { result = true, data, number = 2, message = "Thành Công!" }, JsonRequestBehavior.AllowGet);

                        }
                    }
                    if (checkMaterialMappingOfBobbin == 2)
                    {
                        var listMaterialInfoMMS = await _IdevManagementService.GetListDataMaterialInfoMMS(req.mt_no);
                        var primary = listMaterialInfoMMS.SingleOrDefault();
                        //nếu trong bảng tạm thì kiểm tra bảng chính tồn tại không
                        if (primary != null)
                        {
                            SD = primary.sd_no + "</br>" + primary.ExportCode;
                            var mt_sts_cd = primary.status;
                            trangthai = await checktrangthai(mt_sts_cd);
                            var locationWip = primary.location_code;
                            var vitri = await checkvitri(locationWip);
                            lct_cd = (primary.location_code.StartsWith("002") ? "WIP" : "TIMS");
                            var process = "";
                            var poroduct = "";
                            var po = "";
                            var ketluan = "";
                            var time_mapping = "";
                            if (primary.id_actual != 0 && primary.id_actual != null)
                            {
                                var listDataActual = await _IdevManagementService.GetListActual(primary.id_actual);
                                var actual = listDataActual.SingleOrDefault();
                                po = actual.at_no;
                                var actual_primary = await _IdevManagementService.GetActualPrimaryById(po);

                                poroduct = (actual_primary != null ? actual_primary.product : "");
                                //công đoạn
                                process = await checkcondoan(actual.name);
                                ketluan = "Đã Được Mapping ở công đoạn này: " + process;

                                var listMaterialMappingMMS = await _IdevManagementService.GetListMaterialMappingMMS(req.mt_no);
                                var ds = listMaterialMappingMMS.FirstOrDefault();
                                if (ds != null)
                                {
                                    time_mapping = (ds.reg_date).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                            }
                            else
                            {
                                ketluan = "Vẫn chưa được mapping";
                            }
                            var data = new
                            {
                                SD = SD,
                                trangthai = trangthai,
                                lct_cd = lct_cd,
                                process = process,
                                product = poroduct,
                                po = po,
                                vitri = vitri,
                                ketluan = ketluan,
                                sanluong = primary.gr_qty,
                                time_mapping = time_mapping,
                            };
                            return Json(new { result = true, data, number = 2, message = "Thành Công!" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

            }

        return Json(new { result = false, message = "Lỗi hệ thống!!!" }, JsonRequestBehavior.AllowGet);
        }

        public async Task<string> checktrangthai(string mt_sts_cd)
        {
            var status = await _IdevManagementService.GetDetailNameByDetailCode(mt_sts_cd);
            return status;
        }

        public async Task<string> checkvitri(string lct_Cd)
        {
            var position = await _IdevManagementService.GetLcoationNameByLocationCode(lct_Cd);
            return position;
        }

        public async Task<string> checkcondoan(string name)
        {
            var query = await _IdevManagementService.CheckActual(name);
            return query;
        }

        #endregion

        #region Insert Staff By Upload Excel File

        [HttpPost]
        public async Task<JsonResult> InsertStaffByExcelFile(List<StaffModel> modelList)
        {
            List<string> userIdListTemp = new List<string>();
            if (modelList.Count == 0)
            {
                return Json(new { flag = false, message = "Không thể lấy dữ liệu từ tệp excel Hoặc tệp excel tải lên không có dữ liệu" }, JsonRequestBehavior.AllowGet);
            }
            HashSet<string> hashSet = new HashSet<string>();
            bool isDuplicated = false;
            string duplicatedUserid = "";

            foreach (var item in modelList)
            {
                if (!hashSet.Add(item.Id))
                {
                    isDuplicated = true;
                    duplicatedUserid = item.Id;
                    break;
                }
            }
            if (isDuplicated)
            {
                return Json(new { duplicatedUserid, flag = false, message = $"Tệp Excel trùng ID: {duplicatedUserid}" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var listPositionCode = await _IdevManagementService.GetListCommDT("COM018");
                var listDepartmentCode = await _IdevManagementService.GetListDepartmentInfo();

                foreach (var item in modelList)
                {
                    //get position code for each staff
                    foreach (var comm in listPositionCode)
                    {
                        if (item.PositionName.Trim() == comm.dt_nm.Trim())
                        {
                            item.PositionCode = comm.dt_cd;
                            break;
                        }
                    }

                    //get department code for each staff
                    foreach (var dept in listDepartmentCode)
                    {
                        if (item.DepartmentName.Trim() == dept.depart_nm.Trim())
                        {
                            item.DepartmentCode = dept.depart_cd;
                            break;
                        }
                    }

                    if (item.Gender == "Nữ")
                    {
                        item.Gender = "W";
                    }
                    else { item.Gender = "M"; }

                    userIdListTemp.Add(item.Id);
                }

                var userInsert = Session["userid"] == null ? "" : Session["userid"].ToString();
                var dateInsert = DateTime.Now;

                foreach (var item in modelList)
                {

                    await _IdevManagementService.InsertListStaff(item.Id, item.Name, item.Gender, item.LocationCode, item.BarCode, item.PositionCode, item.BirthDate, item.DepartmentCode, item.JoinDate, userInsert, dateInsert);
                }

                var staff = await _IdevManagementService.GetListStaffbyUserId(userIdListTemp);

                return Json(new { result = true, message = Constant.Success, data = staff }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
            }
        }



        private void WriteHtmlTable<T>(IEnumerable<T> data, TextWriter output, String[] labelList)
        {
            //Writes markup characters and text to an ASP.NET server control output stream. This class provides formatting capabilities that ASP.NET server controls use when rendering markup to clients.
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {

                    Table table = new Table();
                    TableRow row = new TableRow();
                    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

                    foreach (String label in labelList)
                    {
                        TableHeaderCell hcell = new TableHeaderCell();
                        hcell.Text = label;

                        hcell.Font.Bold = true;
                        row.Cells.Add(hcell);
                        row.BorderStyle = BorderStyle.Solid;
                    }
                    table.Rows.Add(row);

                    foreach (T item in data)
                    {
                        row = new TableRow();
                        foreach (PropertyDescriptor prop in props)
                        {
                            TableCell cell = new TableCell();
                            cell.Text = prop.Converter.ConvertToString(prop.GetValue(item));
                            row.Cells.Add(cell);
                            row.BorderStyle = BorderStyle.Solid;
                        }
                        table.Rows.Add(row);
                    }

                    table.RenderControl(htw);

                    output.Write(sw.ToString());
                }
            }
        }


        #endregion Insert Staff By Upload Excel File

    }
}
