using Mvc_VD.Commons;
using Mvc_VD.Models;
using Mvc_VD.Models.DTO;
using Mvc_VD.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using System.IO;
using Mvc_VD.Models.FG;

namespace Mvc_VD.Controllers
{
    public class FGWmsGeneralController : BaseController
    {
        //private Entities db = new Entities();
        public ActionResult General()
        {
            return SetLanguage("");
        }
        private readonly IFGMWServices _IFGMWServices;
        private readonly IhomeService _homeService;
        //2 cai
        public FGWmsGeneralController(IFGMWServices IFGMWServices, IhomeService ihomeService)
        {
            _homeService = ihomeService;
            _IFGMWServices = IFGMWServices;
            //_IWIPService = IWIPService;

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
        public int ChangeCharacterToNumber(string number)
        {
            int temp;
            if (int.TryParse(number, out temp))
            {

                return int.Parse(number);
            }

            char c = 'Z';
            char a = char.Parse(number);
            int i = 35;
            do
            {

                c--;
                i--;
            }
            while (c != a);
            return i;


        }
        public string DateFormatByShinsungRule(string input)
        {
            var y = input.Substring(0, 1);
            var m = input.Substring(1, 1);
            var d = input.Substring(2, 1);
            string year = ChangeCharacterToNumber(y).ToString();
            string month = ChangeCharacterToNumber(m).ToString();
            string date = ChangeCharacterToNumber(d).ToString();
            StringBuilder result = new StringBuilder();
            year = "20" + year + "-";

            if (month.Length < 2)
            {
                month = "0" + month + "-";
            }
            else
            {
                month = month + "-";
            }
            if (date.Length < 2)
            {
                date = "0" + date;
            }

            result.Append(year);
            result.Append(month);
            result.Append(date);
            return result.ToString();
        }
        public async Task<ActionResult> CheckStatusBuyerToSAP(string buyerCode)
        {
            try
            {
                if (string.IsNullOrEmpty(buyerCode))
                {
                    return Json(new { flag = false, message = "Scan Tem gói rỗng hãy scan lại" }, JsonRequestBehavior.AllowGet);
                }
                //kiểm tra mã buyer có trong tồn sap chưa
                // var isExist = _iFGWmsService.FindOneBuyerInfoById(buyerCode);
                var isExist = await _IFGMWServices.GetGeneralFGByBuyerQR(buyerCode);
                if (isExist != null)
                {
                    if (isExist.status.Equals("001"))
                    {
                        return Json(new { result = false, message = "Tem gói đang tồn kho" }, JsonRequestBehavior.AllowGet);
                    }
                    if (isExist.status.Equals("000"))
                    {
                        return Json(new { result = false, message = "Đã giao khách với đơn giao là: " + isExist.dl_no }, JsonRequestBehavior.AllowGet);
                    }
                }
                //insert generalfg
                //lấy product
                //int index = buyerCode.IndexOf("DZIH");
                //if (index == 0)
                //{
                //    return Json(new { result = false, message = "Tem gói không đúng chuẩn phải có DZIH" }, JsonRequestBehavior.AllowGet);
                //}
                buyerCode = buyerCode.ToUpper();
                int index = buyerCode.IndexOf("DZIH");
                if (index < 1)
                {
                    index = buyerCode.IndexOf("EA8D");
                }
                if (index < 1)
                {
                    return Json(new { result = false, message = "Tem gói không hợp lệ vui lòng thử lại" }, JsonRequestBehavior.AllowGet);
                }
                string productcode = buyerCode.Substring(0, index);
                //lấy ra lot_date bằng cách trừ đi product và 8 kí tự sẽ tìm được mã lot_date

                int startBien = index + 8;

                //lot_date
                var date = buyerCode.Substring(startBien, 3);
                string lot_date = DateFormatByShinsungRule(date);

                var ktra_stamp_cd = await _IFGMWServices.GetStyleInfoReplace(productcode);
                if (ktra_stamp_cd == null)
                {
                    return Json(new { result = false, message = "Product chưa được đăng kí" }, JsonRequestBehavior.AllowGet);
                }
                int packing_amount = ktra_stamp_cd.pack_amt;
                //nếu tem in ở mes thì lấy số lượng in ở stamp_detaill
                var StampDetail = await _IFGMWServices.FindStamp(buyerCode);
                if (StampDetail != null)
                {
                    packing_amount = StampDetail.standard_qty;
                    lot_date = StampDetail.lot_date;
                }

                //check format lot_no
                var dateConvert = new DateTime();
                if (DateTime.TryParse(lot_date, out dateConvert))
                {
                    string[] tokens = lot_date.Split('-');
                    if(Int32.Parse(tokens[0]) < 2015)
                    {
                        return Json(new { result = false, message = "Ngày không đúng format" }, JsonRequestBehavior.AllowGet);
                    }
                    lot_date = dateConvert.ToString("yyyy-MM-dd");
                }
                else
                {
                    return Json(new { result = false, message = "Ngày không đúng format" }, JsonRequestBehavior.AllowGet);
                }
                string at_no = "";
                //kiểm tra nó có trùng tem đang chờ nhập kho của MES không thì update lun
                //var checkisExist = _iFGWmsService.CheckBuyerFG(buyerCode);
                var checkisExist = await _IFGMWServices.GetDataBuyerFromMaterialInfoTIMSForSAP(buyerCode);
                if (checkisExist != null)
                {
                    at_no = checkisExist.at_no;
                    //update mã tem ở buyer
                    checkisExist.input_dt = DateTime.Now.ToString("yyyyMMddHHmmss");
                    checkisExist.location_code = "003G01000000000000";
                    checkisExist.from_lct_code = "006000000000000000";
                    checkisExist.to_lct_code = "003G01000000000000";
                    checkisExist.mt_sts_cd = "001";
                    checkisExist.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();

                    await _IFGMWServices.UpdateWMaterialInfoTimsFromBuyerQR(buyerCode, DateTime.Now.ToString("yyyyMMddHHmmss"), "003G01000000000000", "006000000000000000", "003G01000000000000", Session["userid"] == null ? null : Session["userid"].ToString(), "001");
                }

                //insert stamp_detail
                var generalfg = new generalfg()
                {
                    buyer_qr = buyerCode,
                    product_code = ktra_stamp_cd.style_no,
                    md_cd = ktra_stamp_cd.md_cd,
                    qty = packing_amount,
                    lot_no = lot_date,
                    reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString(),
                    reg_dt = DateTime.Now,
                    status = "001",
                    type = "SAP",
                    at_no = at_no
                };
                int idd = await _IFGMWServices.InertFGGeneral(generalfg);

                return Json(new { result = true, message = "Thêm thành công", data = generalfg }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { result = false, message = "Lỗi hệ thống." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public async Task<JsonResult> getFGGeneral(Pageing paging)
        {
            if (Session["authorize"] != null)
            {

                string buyerCode = Request["buyerCode"] == null ? "" : Request["buyerCode"].Trim();
                string productCode = Request["productCode"] == null ? "" : Request["productCode"].Trim();
                string productName = Request["productName"] == null ? "" : Request["productName"].Trim();
                string bom_type = Request["bom_type"] == null ? "" : Request["bom_type"].Trim();
                string recevice_dt_start = Request["recevice_dt_start"] == null ? "" : Request["recevice_dt_start"].Trim();
                string recevice_dt_end = Request["recevice_dt_end"] == null ? "" : Request["recevice_dt_end"].Trim();

                var dateConvert = new DateTime();
                if (DateTime.TryParse(recevice_dt_end, out dateConvert))
                {
                    recevice_dt_end = dateConvert.ToString("yyyy-MM-dd");
                }
                else
                {
                    recevice_dt_end = null;
                }
                if (DateTime.TryParse(recevice_dt_start, out dateConvert))
                {
                    recevice_dt_start = dateConvert.ToString("yyyy-MM-dd");
                }
                else
                {
                    recevice_dt_start = null;
                }

                //StringBuilder varname1 = new StringBuilder();
                //varname1.Append(" SELECT MAX(a.id) AS id, MAX(a.product_code) AS product_code, MAX(a.md_cd) AS md_cd, SUM(a.qty) AS qty, max(a.reg_dt) as reg_dt ,max(b.bom_type) as bom_type ");
                //varname1.Append("FROM generalfg as a Join d_style_info b on b.style_no = a.product_code where a.status in ('001','010')");
                //varname1.Append(" and (('"+ bom_type+ "'='') or (b.bom_type like ('%" + bom_type + "%'))) ");
                ////varname1.Append(" group by a.product_code ");
                //varname1.Append(" and (('"+ productCode + "'='') or ( a.product_code LIKE ('%" + productCode + "%')))");
                //varname1.Append(" AND (('" + buyerCode + "'='') or (a.buyer_qr LIKE ('%" + buyerCode + "%')))");
                //varname1.Append("and (('" + recevice_dt_start + "'='00010101') or ( CONVERT(datetime,a.reg_dt,121) >=  CONVERT(datetime,'" + recevice_dt_start + "',121)))");
                //varname1.Append("and (('" + recevice_dt_end + "'='99991231') or ( CONVERT(datetime,a.reg_dt,121) >=  CONVERT(datetime,'" + recevice_dt_end + "',121)))");

                //varname1.Append(" group by a.product_code, b.bom_type");
                IEnumerable<FGGeneral> data = await _IFGMWServices.GetFGGeneral(bom_type, productCode, productName, recevice_dt_start, recevice_dt_end, buyerCode);

                int start = (paging.page - 1) * paging.rows;
                var rs = data.OrderByDescending(x => x.product_code);
                int totalRecords = data.Count();
                int totalPages = (int)Math.Ceiling((float)totalRecords / paging.rows);
                var datas = rs.Skip<FGGeneral>(start).Take(paging.rows);
                var result = new
                {
                    total = totalPages,
                    page = paging.page,
                    records = totalRecords,
                    rows = datas
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<JsonResult> getFGgeneralDetail(Pageing paging)
        {
            try
            {
                if (Session["authorize"] != null)
                {

                    string id = Request["id"] == null ? "" : Request["id"].Trim();
                    string buyerCode = Request["buyerCode"] == null ? "" : Request["buyerCode"].Trim();
                    string productName = Request["productName"] == null ? "" : Request["productName"].Trim();
                    string recevice_dt_start = Request["recevice_dt_start"] == null ? "" : Request["recevice_dt_start"].Trim();
                    string recevice_dt_end = Request["recevice_dt_end"] == null ? "" : Request["recevice_dt_end"].Trim();
                    string bom_type = Request["bom_type"] == null ? "" : Request["bom_type"].Trim();

                    var dateConvert = new DateTime();
                    if (DateTime.TryParse(recevice_dt_end, out dateConvert))
                    {
                        recevice_dt_end = dateConvert.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        recevice_dt_end = null;
                    }
                    if (DateTime.TryParse(recevice_dt_start, out dateConvert))
                    {
                        recevice_dt_start = dateConvert.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        recevice_dt_start = null;
                    }
                    var id_FG = Convert.ToInt32(id);
                    var productCode = await _IFGMWServices.CheckGeneralFGproduct(id_FG);


                    //StringBuilder varname1 = new StringBuilder();
                    //varname1.Append("SELECT a.*,(SELECT dt_nm FROM comm_dt WHERE comm_dt.mt_cd ='WHS005' AND comm_dt.dt_cd = a.status) AS statusName ");
                    //varname1.Append("FROM generalfg as a ");
                    //varname1.Append(" WHERE  a.status in ('001','010')");
                    //varname1.Append("and (('" + productCode + "'='') or ( a.product_code LIKE ('%" + productCode + "%')))");
                    //varname1.Append(" AND (('" + buyerCode + "'='') or (a.buyer_qr LIKE ('%" + buyerCode + "%')))");
                    //varname1.Append(" and (('" + recevice_dt_start + "'='00010101') or ( CONVERT(datetime,a.reg_dt,121) >=  CONVERT(datetime,'" + recevice_dt_start + "',121)))");
                    //varname1.Append(" and (('" + recevice_dt_end + "'='99991231') or ( CONVERT(datetime,a.reg_dt,121) >=  CONVERT(datetime,'" + recevice_dt_end + "',121)))");

                    IEnumerable<FGGeneral> data = _IFGMWServices.GetFGGeneraldetail(bom_type, productCode, productName, recevice_dt_start, recevice_dt_end, buyerCode);

                    int totalRecords = data.Count();
                    int totalPages = (int)Math.Ceiling((float)totalRecords / (float)paging.rows);
                    var datas = data.OrderByDescending(x => x.buyer_qr).Skip<FGGeneral>((paging.page - 1) * (paging.rows)).Take(paging.rows);
                    var result = new
                    {
                        total = totalPages,
                        page = paging.page,
                        records = totalRecords,
                        rows = datas
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<JsonResult> InsertTemGeneral(generalfg generalfg, stamp_detail stamp_detail, List<ModelInsertTemGeneral> InsertTemGeneral)
        {
            if (Session["authorize"] != null)
            {
                try
                {
                    var data_update = 0;
                    var data_create = 0;
                    var data_error = 0;
                    if (InsertTemGeneral == null)
                    {
                        return Json(new { result = false, message = "File excel rỗng" }, JsonRequestBehavior.AllowGet);
                    }

                    try
                    {
                        foreach (var item in InsertTemGeneral)
                        {
                            if (item.BuyerCode == null)
                            {
                                data_error++;
                            }
                            else
                            {
                                // get productCODE
                                //lấy product
                                string productCode = "";
                                item.BuyerCode = item.BuyerCode.ToUpper();
                                if (item.BuyerCode.Contains("DZIH"))
                                {
                                    productCode = item.BuyerCode.Substring(0, item.BuyerCode.IndexOf("DZIH"));
                                }
                                else
                                {
                                    productCode = item.BuyerCode.Substring(0, item.BuyerCode.IndexOf("EA8D"));
                                }
                                var dataPRoduct = await _IFGMWServices.GetStyleInfoReplace(productCode);
                                // if product no exist in db
                                if (dataPRoduct != null)
                                {
                                    int list1 = await _IFGMWServices.CheckGeneralFG(item.BuyerCode);
                                    try
                                    {

                                        if (list1 == 0)
                                        {

                                            string at_no = "";
                                            //kiểm tra nó có trùng tem đang chờ nhập kho của MES không thì update lun
                                            var isExist = await _IFGMWServices.GetDataBuyersFromMaterialInfoTIMS(item.BuyerCode);
                                            if (isExist.Count() > 0)
                                            {
                                                var datainfo = isExist.FirstOrDefault();
                                                at_no = datainfo.at_no;

                                                //update mã tem ở buyer

                                                datainfo.input_dt = DateTime.Now.ToString("yyyyMMddHHmmss");
                                                datainfo.location_code = "003G01000000000000";
                                                datainfo.from_lct_code = "006000000000000000";
                                                datainfo.to_lct_code = "003G01000000000000";
                                                datainfo.status = "001";
                                                datainfo.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                                                datainfo.chg_date = DateTime.Now;
                                                await _IFGMWServices.UpdateReceFGWMaterial(datainfo);
                                            }

                                            //lấy ra lot_date bằng cách trừ đi product và 8 kí tự sẽ tìm được mã lot_date
                                            var startBien = productCode.Length + 8;
                                            var date = item.BuyerCode.Substring(startBien, 3);
                                            var lot_date = DateFormatByShinsungRule(date);
                                            //lấy số lượng. nếu có thì lấy , ngược lại lấy trong bảng product

                                            int quantity = 0;
                                            if (!string.IsNullOrEmpty(item.Quantity))
                                            {
                                                quantity = Convert.ToInt32(item.Quantity);
                                            }
                                            else
                                            {
                                                quantity = (int)dataPRoduct.pack_amt;
                                            }
                                            //nếu tem in ở mes thì lấy số lượng in ở stamp_detaill
                                            var StampDetail = await _IFGMWServices.FindStamp(item.BuyerCode);
                                            if (StampDetail != null)
                                            {
                                                quantity = StampDetail.standard_qty;
                                                lot_date = StampDetail.lot_date;
                                            }
                                            generalfg.at_no = at_no;
                                            generalfg.md_cd = dataPRoduct.md_cd;
                                            generalfg.lot_no = lot_date;

                                            generalfg.qty = quantity;
                                            generalfg.product_code = dataPRoduct.style_no;
                                            generalfg.status = "001";
                                            generalfg.buyer_qr = item.BuyerCode;
                                            generalfg.reg_dt = DateTime.Now;
                                            generalfg.reg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                                            generalfg.type = "SAP";
                                            int idd = await _IFGMWServices.InertFGGeneral(generalfg);
                                            generalfg.id = idd;

                                            if (idd > 0)
                                            {
                                                data_create++;

                                            }
                                        }
                                        else
                                        {
                                            data_update++;
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        data_error++;
                                    }
                                }
                                else
                                {
                                    data_error++;
                                }

                            }
                        }
                    }
                    catch (Exception)
                    {
                        data_error++;
                    }

                    return Json(new
                    {
                        result = true,
                        data_update = data_update,
                        data_create = data_create,
                        data_error = data_error
                    }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {

                    return Json(new { result = false, message = Constant.ErrorSystem }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }
        public async Task<ActionResult> ExportFGgeneralToExcel(string bom_type, string productCode, string productName, string buyerCode, string recevice_dt_start, string recevice_dt_end)
        {
            if (string.IsNullOrEmpty(productCode))
            {
                productCode = "";
            }
            if (string.IsNullOrEmpty(buyerCode))
            {
                buyerCode = "";
            }
            //if (string.IsNullOrEmpty(recevice_dt_start))
            //{
            //    recevice_dt_start = DateTime.MinValue.ToString("yyyyMMdd");
            //}
            //else
            //{
            //    recevice_dt_start = recevice_dt_start.Replace("-", "");
            //}
            //if (string.IsNullOrEmpty(recevice_dt_end))
            //{
            //    recevice_dt_end = DateTime.MaxValue.ToString("yyyyMMdd");
            //}
            //else
            //{
            //    recevice_dt_end = recevice_dt_end.Replace("-", "");
            //}

            var data = await _IFGMWServices.GetFGGeneralExport(bom_type, productCode, productName,  recevice_dt_start, recevice_dt_end, buyerCode);
            var valuess = data.Select(x => new
            {

                product_code = x.product_code,
                product_name = x.product_name,
                md_cd = x.md_cd,
                buyer_qr = x.buyer_qr,
                at_no = x.at_no,
                lot_no = x.lot_no,
                qty = x.qty,
                reg_dt = x.reg_dt,
                bom_type = x.bom_type,
                statusName = x.statusName,
            }); ;
            String[] labelList = new string[10] {  "Product Code", "Product Name", "Model", "Buyer Code", "PO", "Lot No", "qty", "reg datte", "bom_type", "statusName" };

            Response.ClearContent();

            Response.AddHeader("content-disposition", "attachment;filename=FGInventory.xls");

            Response.AddHeader("Content-Type", "application/vnd.ms-excel");

            new InitMethods().WriteHtmlTable(valuess, Response.Output, labelList);

            Response.End();

            return View();
        }

        public async Task<JsonResult> updateQuantityBuyer(ListIdGenneralFG model)
        {
            try
            {
                StringBuilder idsList = new StringBuilder();
                if (model.listId != null)
                {
                    foreach (var item in model.listId)
                    {
                        idsList.Append($"{item},");

                    }
                }
                string listId = new InitMethods().RemoveLastComma(idsList);

                if (model.gr_qty > 0 && model.listId != null)
                {
                    var userAcount = Session["userid"] == null ? null : Session["userid"].ToString();
                    var userWIP = "";
                    if (string.IsNullOrEmpty(userAcount))
                    {
                        return Json(new { result = false, message = "Vui lòng đăng nhập để sửa số lượng" }, JsonRequestBehavior.AllowGet);
                    }
                    //if (!string.IsNullOrEmpty(userAcount))
                    //{
                    //    var dsMbInfo = _iFGWmsService.GetMbInfoGrade(userAcount);
                    //    userWIP = dsMbInfo.grade;
                    //}
                    //if (userWIP != "Admin")
                    //{
                    //    return Json(new { result = false, message = "Tài khoản thuộc quyền Admin mới được chỉnh sửa" }, JsonRequestBehavior.AllowGet);
                    //}
                    generalfg item = new generalfg();
                    item.qty = (int)model.gr_qty;

                    item.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                    item.chg_dt = DateTime.Now;



                    var id = await _IFGMWServices.UpdateQtyGenneral(item, listId);



                    return Json(new { result = true, message = "Sửa thành công" }, JsonRequestBehavior.AllowGet);

                }
                return Json(new { result = false, message = "Sửa thất bại" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = e }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> updateLotNoBuyer(ListIdGenneralFG model)
        {
            try
            {
                StringBuilder idsList = new StringBuilder();
                if (model.listId != null)
                {
                    foreach (var item in model.listId)
                    {
                        idsList.Append($"{item},");

                    }
                }


                string listId = new InitMethods().RemoveLastComma(idsList);

                if (!string.IsNullOrEmpty(model.lot_no) && model.listId != null)
                {
                    var userAcount = Session["userid"] == null ? null : Session["userid"].ToString();

                    if (string.IsNullOrEmpty(userAcount))
                    {
                        return Json(new { result = false, message = "Vui lòng đăng nhập để sửa số lượng" }, JsonRequestBehavior.AllowGet);
                    }

                    var dateConvert = new DateTime();
                    if (DateTime.TryParse(model.lot_no, out dateConvert))
                    {
                        model.lot_no = dateConvert.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        return Json(new { result = false, message = "Wrong Date" }, JsonRequestBehavior.AllowGet);
                    }
                    generalfg item = new generalfg();
                    item.lot_no = model.lot_no;

                    item.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                    item.chg_dt = DateTime.Now;



                    var id = await _IFGMWServices.UpdateLotNoGenneral(item, listId);
                    var getdata = await _IFGMWServices.GetFGGeneralById(listId);


                    return Json(new { result = true,data1 =getdata, message = "Sửa thành công" }, JsonRequestBehavior.AllowGet);

                }
                return Json(new { result = false, message = "Sửa thất bại" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = e }, JsonRequestBehavior.AllowGet);
            }
        }


        public async Task<JsonResult> updateQuantity(int? buyerQtuantity, int? id)
        {
            try
            {
                if (buyerQtuantity > 0 && id > 0)
                {
                    //var userAcount = Session["userid"] == null ? null : Session["userid"].ToString();
                    //var userWIP = "";
                    //if (!string.IsNullOrEmpty(userAcount))
                    //{
                    //    var dsMbInfo = _iFGWmsService.GetMbInfoGrade(userAcount);
                    //    userWIP = dsMbInfo.grade;
                    //}
                    //if (userWIP != "Admin")
                    //{
                    //    return Json(new { result = false, message = "Tài khoản thuộc quyền Admin mới được chỉnh sửa", id = id }, JsonRequestBehavior.AllowGet);
                    //}

                    var authData = "";

                    if (Session["authData"] != null)
                    {
                        authData = Session["authData"].ToString();
                    }


                    if (Session["authName"] == null)
                    {
                        return Json(new { result = false, message = "Vui lòng đăng nhập!!!" }, JsonRequestBehavior.AllowGet);
                    }
                    var bien = (Session["authName"].ToString() == "Root" ? "true" : Session["wms"]);
                    var check_role = await _IFGMWServices.GetListAuthorMenuInfo(authData, "FGWmsGeneralGeneral");
                    if (check_role == "RL001")
                    {
                        return Json(new { result = false, message = "Tài khoản của bạn chỉ để xem, không được sửa nội dung này!!!" }, JsonRequestBehavior.AllowGet);
                    }

                        generalfg item = new generalfg();
                    item.id = (int)id;
                    item.qty = (int)buyerQtuantity;

                    item.chg_id = (Session["userid"] == null) ? "" : Session["userid"].ToString();
                    item.chg_dt = DateTime.Now;
                    var re = await _IFGMWServices.UpdateQtyGeneral(item);

                    return Json(new { result = true, message = "Cập nhập số lượng thành công" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = false, kq = "Cập nhập số lượng lỗi" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = false, kq = e }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
