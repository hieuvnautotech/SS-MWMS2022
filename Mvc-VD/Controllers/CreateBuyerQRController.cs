using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Mvc_VD.Classes;
using Mvc_VD.Models;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Services.Interface;
using MySql.Data.MySqlClient;

namespace Mvc_VD.Controllers
{
    public class CreateBuyerQRController : BaseController
    {
        private readonly ICreateBuyerQRService _createBuyerQRService;
        private readonly IhomeService _ihomeService;
        public CreateBuyerQRController()
        {
        }

        public CreateBuyerQRController(ICreateBuyerQRService createBuyerQRService,IhomeService ihomeService)
        {
            _createBuyerQRService = createBuyerQRService;
            _ihomeService = ihomeService;


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

        public ActionResult BuyerQR()
        {
            return SetLanguage("~/Views/fgwms/CreateBuyerQR/BuyerQR.cshtml");
        }

        [HttpGet]
        public async Task<JsonResult> Create(string printedDate)
        {
            string productCode = Request["productCode"] == null ? "" : Request["productCode"].Trim();
            string vendorCode = Request["vendorCode"] == null ? "DZIH" : Request["vendorCode"].Trim();
            string vendorLine = Request["vendorLine"] == null ? "A" : Request["vendorLine"].Trim();
            string labelPrinter = Request["labelPrinter"] == null ? "1" : Request["labelPrinter"].Trim();
            string type = Request["type"] == null ? "N" : Request["type"].Trim();
            string pcn = Request["pcn"] == null ? "0" : Request["pcn"].Trim();
            string machineLine = Request["machineLine"] == null ? "01" : Request["machineLine"].Trim();
            string shift = Request["shift"] == null ? "0" : Request["shift"].Trim();
            string quantity = Request["quantity"] == null ? "0" : Request["quantity"].Trim();
            string quantityPerTray = Request["quantityPerTray"] == null ? "0" : Request["quantityPerTray"].Trim();
            string stampCode = Request["stampCode"] == null ? "0" : Request["stampCode"].Trim();
            string ssver = Request["ssver"] == null ? "" : Request["ssver"].Trim().ToUpper();
            //kiểm tra ngày có phải dạng date không, tránh trường hợp 31-09-2021 không tồn tại nhưng vẫn tạo

            if (string.IsNullOrEmpty(vendorCode))
            {
                return Json(new { result = false, message = "HÃY Chọn vendor để tạo tem gói" }, JsonRequestBehavior.AllowGet);
            }

            DateTime date;
            bool chValidity = DateTime.TryParseExact(
                    printedDate,
                     "yyyy-MM-dd",
                     CultureInfo.InvariantCulture,
                     DateTimeStyles.None,
                     out date);
            if (chValidity == false)
            {
                return Json(new { result = false, message = "Chọn ngày không có thực, vui lòng chọn lại" }, JsonRequestBehavior.AllowGet);
            }

            StringBuilder tempBuyerQR = new StringBuilder();
            tempBuyerQR.Append(productCode.Replace("-", ""))
                .Append(vendorCode)
                .Append(vendorLine)
                .Append(labelPrinter)
                .Append(type)
                .Append(pcn)
                .Append(DateFormatByShinsungRule(printedDate));

            string tempQR = tempBuyerQR.ToString();
            try
            {
                int increament = 0;
                StampDetail detail = new StampDetail();
                List<StampDetail> list = new List<StampDetail>();
                var listInt1 = await _createBuyerQRService.GetCountNumberBuyer(tempQR, shift);

                //var zzz = 0;
                //var zzzz = 0;
                for (int i = 0; i < uint.Parse(quantity); i++)
                {
                  
                   
                    if (listInt1.bientang != null && listInt1.bientang != "")
                    {
                        if (listInt1.bientang == "Z99")
                        {
                            int MachineLineAuto = int.Parse(listInt1.machine_line) + 1;
                            machineLine = MachineLineAuto.ToString();
                            if (machineLine.Length < 10)
                            {
                                machineLine = '0' + machineLine;
                            }
                            increament += 1;
                        }
                        else
                        {
                            var x = listInt1.bientang.Substring(0, 1);
                            var y = listInt1.bientang.Substring(1, 1);
                            var z = listInt1.bientang.Substring(2, 1);
                            //if (i == 0)
                            //{
                            //    zzz = z.ToInt();
                            //    zzzz = zzz;
                            //}
                            //else
                            //{

                            //    zzzz = ++zzz;
                            //}
                            string xx;
                            if (x == "Z")
                            {
                                xx = "35";
                            }
                            else
                            {
                                xx = ChangeCharacterToNumber(x).ToString();
                            }
                            string yy = ChangeCharacterToNumber(y).ToString();
                            string zz = ChangeCharacterToNumber(z.ToString()).ToString();
                            string sotang = xx + yy + zz;
                            increament = int.Parse(sotang) + 1;
                            machineLine = listInt1.machine_line;
                        }
                    }
                    else
                    {
                        increament += 1;
                    }



                    string buyerQR = tempQR;

                    detail = new StampDetail();

                    detail.product_code = productCode;
                    detail.ssver = ssver;
                    detail.vendor_code = vendorCode;
                    detail.vendor_line = vendorLine;
                    detail.label_printer = labelPrinter;
                    detail.is_sample = type;
                    detail.pcn = pcn;
                    detail.lot_date = printedDate;
                    detail.serial_number = increament.ToString();
                    detail.machine_line = machineLine;
                    detail.shift = shift;
                    detail.standard_qty = int.Parse(quantityPerTray);
                    detail.stamp_code = stampCode;
                    detail.reg_dt = DateTime.Now;
                    detail.chg_dt = DateTime.Now;
                    detail.reg_id = Session["userid"] == null ? null : Session["userid"].ToString();
                    detail.chg_id = Session["userid"] == null ? null : Session["userid"].ToString();

                    var increamentFormat = BuyerQRSerialFormat(increament);


                    if (stampCode == "002" || stampCode == "009")
                    {
                        detail.buyer_qr = String.Concat(tempQR, increamentFormat, machineLine, shift, ProductQuantityFormatForSDV3(int.Parse(quantityPerTray))).ToUpper();

                    }
                    else
                    {
                        detail.buyer_qr = String.Concat(tempQR, increamentFormat, machineLine, shift, ProductQuantityFormatForBuyerQR(int.Parse(quantityPerTray))).ToUpper();
                    }





                    //if (stampCode == "002" || stampCode == "009")
                    //{
                    //   detail.buyer_qr = String.Concat(tempQR, increamentFormat, machineLine, shift, ProductQuantityFormatForBuyerQR(int.Parse(quantityPerTray))).ToLower();
                    //}
                    //else
                    //{
                    //    detail.buyer_qr = String.Concat(tempQR, increamentFormat, machineLine, shift, ProductQuantityFormatForSDV3(int.Parse(quantityPerTray))).ToLower();
                    //}

                    //var idStamp = await _createBuyerQRService.InsertStampDetail(detail);
                    //var stampDetail = await _createBuyerQRService.GetStampDetailById(idStamp);
                    list.Add(detail);

                    listInt1.bientang = increamentFormat;
                    listInt1.machine_line = machineLine;
                }

                 //await _createBuyerQRService.InsertStampDetail(list);
            
                  
                //List<BuyerQRModel> returnList = new List<BuyerQRModel>();
                //var returnList = await GetBuyerQRShow(list);
                ////List<BuyerQRModel> showList = new List<BuyerQRModel>();
                //foreach (var item in returnList)
                //{
                //    var buyerQRModel = new BuyerQRModel()
                //    {
                //        id = item.id,
                //        buyer_qr = item.buyer_qr.ToUpper(),
                //        stamp_code = item.stamp_code,
                //        product_code = item.product_code,
                //        product_name = item.style_nm,
                //        lotNo = string.Concat(item.reg_dt.Replace("-", "")),
                //        model = item.md_cd,
                //        quantity = int.Parse(quantityPerTray),
                //        stamp_name = item.stamp_name
                //    };

                //   // showList.Add(buyerQRModel);
                //}

            var    showList = await _createBuyerQRService.InsertStampDetail2(list);








                if (showList != null)
                {
                    return Json(new { result = true, data = showList, message = "Tạo mã tem thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = "Không thể tạo mã tem!" }, JsonRequestBehavior.AllowGet);
                }

                }
                catch (Exception e)
                {
                    throw e;
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
        public async Task<IEnumerable<BuyerQRModel>> GetBuyerQRShow(List<StampDetail> list)
        {
            //List<BuyerQRModel> showList = new List<BuyerQRModel>();
            if (list.Count > 0)
            {
                //    foreach (var item in list)
                //    {
                //        var buyerQRModel = new BuyerQRModel()
                //        {
                //id = item.id,
                //buyer_qr = item.buyer_qr.ToUpper(),
                //stamp_code = item.stamp_code,
                //product_code = item.product_code,
                //product_name = item.product_code,
                //lotNo = string.Concat(item.lot_date.Replace("-", "")),
                //model = item.product_code,
                //quantity = item.standard_qty,
                //    stamp_name = await _createBuyerQRService.GetStampNameByCode(item.stamp_code),
                //    };

                //    showList.Add(buyerQRModel);
                //}

                //  return showList;
                var html = "'" + "";
                for (int i = 0; i < list.Count; i++)
                {
                    html += list.ToList()[i].buyer_qr;

                    if (i != list.ToList().Count() - 1)
                    {
                        html += "'" + ',' + "'";
                    }
                }
                html = html + "'";
                var stamp_name = await _createBuyerQRService.GetStampNameByCode(html);
                return stamp_name;
            }

            else
            {
                return null;
            }

    }

        [HttpGet]
        public async Task<JsonResult> GetAllStamp()
        {
            var list = await _createBuyerQRService.GetAllStampMaster();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public string DateFormatByShinsungRule(string input)
        {
            input = input.Replace("-", "");
            string year = ChangeNumberToCharacter(int.Parse(input.Substring(2, 2))).ToString();
            string month = ChangeNumberToCharacter(int.Parse(input.Substring(4, 2))).ToString();
            string date = ChangeNumberToCharacter(int.Parse(input.Substring(6, 2))).ToString();
            StringBuilder result = new StringBuilder();
            result.Append(year);
            result.Append(month);
            result.Append(date);
            return result.ToString();
        }

        public char ChangeNumberToCharacter(int number)
        {
            string temp = number.ToString();
            char result = 'A';
            int subtraction = 0;
            if (number < 10)
            {
                return char.Parse(temp);
            }
            else
            {
                subtraction = number - 10;
                for (int i = 0; i < subtraction; i++)
                {
                    result++;
                }
            }
            return result;
        }

        public string BuyerQRSerialFormat(int number)
        {
            return ProductQuantityFormatForBuyerQR(number);
        }

        public string ProductQuantityFormatForBuyerQR(int quantity)
        {
            string str = quantity.ToString();
            int length = str.Length;

            if (length % 2 == 0)
            {
                if (length == 4)
                {
                    string fisrtTwoCharacters = str.Substring(0, 2);
                    string lastTwoCharacter = str.Substring(2, 2);

                    char a = ChangeNumberToCharacter(int.Parse(fisrtTwoCharacters));
                    StringBuilder result = new StringBuilder();
                    result.Append(a.ToString())
                        .Append(lastTwoCharacter);
                    str = result.ToString();
                }
                else
                {
                    str = string.Concat("0", str);
                }
            }
            else
            {
                if (length == 5)
                {
                    string fisrtTwoCharacters = str.Substring(0, 2);
                    string secondTwoCharacter = str.Substring(2, 2);
                    string lastCharacter = str.Substring(4, 1);

                    char a = ChangeNumberToCharacter(int.Parse(fisrtTwoCharacters));
                    char b = ChangeNumberToCharacter(int.Parse(secondTwoCharacter));
                    StringBuilder result = new StringBuilder();
                    result.Append(a.ToString())
                        .Append(b.ToString())
                        .Append(lastCharacter);
                    str = result.ToString();
                }
                if (length == 1)
                {
                    str = string.Concat("00", str);
                }
            }
            return str;
        }

        public string ProductQuantityFormatForSDV3(int quantity)
        {
            StringBuilder str = new StringBuilder();
            int x = 0;
            int y = 0;
            int z = 0;

            x = (int)Math.Floor((float)quantity / (32 * 32));
            y = (int)Math.Floor((float)(quantity - x * 32 * 32) / 32);
            z = (int)Math.Floor((float)(quantity - (x * 32 * 32) - (y * 32)));

            return str.Append(ChangeNumberToCharacter(x).ToString()).Append(ChangeNumberToCharacter(y).ToString()).Append(ChangeNumberToCharacter(z).ToString()).ToString();
        }

        public string ProductQuantityFormatForBoxQR(int quantity)
        {
            string str = quantity.ToString();
            int length = str.Length;
            if (length == 4)
            {
                str = "0" + str;
            }
            if (length == 3)
            {
                str = "00" + str;
            }
            if (length == 2)
            {
                str = "000" + str;
            }
            if (length == 1)
            {
                str = "0000" + str;
            }

            return str;
        }

        #region PrintQR

        public ActionResult PrintQR(string id)
        {
            ViewData["Message"] = id;
            return View("~/Views/fgwms/CreateBuyerQR/PrintQR.cshtml");

        }

        [HttpGet]
        public async Task<ActionResult> QRbarcodeInfo(string id)
        {
                try
                {
                    var multiIDs = id.TrimStart('[').TrimEnd(']').Split(',');
                    var row_data = new List<BuyerQRModel>();

                    for (int i = 0; i < multiIDs.Length; i++)
                    {
                        var id2 = int.Parse(multiIDs[i]);
                    var rs = await _createBuyerQRService.GetStamp(id2);
                    var data = rs.FirstOrDefault();
                    var itemBuyer = new BuyerQRModel();
                        //add
                    itemBuyer.id = data.id;
                    itemBuyer.model = data.model;
                    itemBuyer.buyer_qr = data.buyer_qr.ToUpper();
                    itemBuyer.vendor_line = data.vendor_line == null ? "" : "(" + data.vendor_line + ")";
                    itemBuyer.part_name = data.part_name;
                    itemBuyer.stamp_code = data.stamp_code;
                    itemBuyer.product_code = data.product_code;
                    itemBuyer.quantity = data.quantity;
                    itemBuyer.nhietdobaoquan = data.nhietdobaoquan;
                    itemBuyer.lotNo = string.Concat(data.lotNo.Replace("-", ""));
                    if (!string.IsNullOrEmpty(data.lotNo))
                    {
                        var ymd = string.Concat(data.lotNo.Replace("-", ""));
                        string y = ymd.Substring(0, 4);
                        string m = ymd.Substring(4, 2);
                        string d = ymd.Substring(6, 2);

                        itemBuyer.nsx = d + "/" + m + "/" + y;
                    }
                    else
                    {
                        itemBuyer.nsx = "";
                    }
                    itemBuyer.prj_nm = (data.prj_nm == "" || data.prj_nm == null) ? "" : data.prj_nm;
                    itemBuyer.ssver = (data.ssver == "" || data.ssver == null) ? "" : data.ssver;
                    itemBuyer.supplier = data.vendor_code == null ? "" :  data.vendor_code;
                    string hsd = "";
                    //if (data.stamp_code == "001")
                    if (data.expiry_month == "0")
                    {
                        hsd = data.hsd;
                    }
                    else
                    {
                        string s = data.lotNo;
                        DateTime dt = DateTime.ParseExact(s, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        if (!string.IsNullOrEmpty(data.expiry_month))
                        {
                            hsd = dt.AddMonths(int.Parse(data.expiry_month)).ToString("dd/MM/yyyy");
                        }
                    }

                    itemBuyer.hsd = hsd;
                    //add view_qc_Model
                    row_data.Add(itemBuyer);
                }
                return Json(new { result = true, row_data }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { result = false, message = e.Message }, JsonRequestBehavior.AllowGet);
                }




        }
        #endregion

    }
}