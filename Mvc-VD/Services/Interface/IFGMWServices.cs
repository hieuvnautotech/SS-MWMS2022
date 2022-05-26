using Mvc_VD.Classes;
using Mvc_VD.Models;
using Mvc_VD.Models.DTO;
using Mvc_VD.Models.FG;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models.Response;
using Mvc_VD.Models.TIMS;
using Mvc_VD.Models.WOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mvc_VD.Services.Interface
{
    public interface IFGMWServices
    {
        #region Mapping QR Box
        //Task<IEnumerable<BoxMapping>> GetListBoxMapping(string boxCode, string ProductCode, string sDate, string BuyerCode, int intpage, int introw);

        Task<IEnumerable<BoxMapping>> GetListBoxMapping(string boxCode, string ProductCode, string sDate, string BuyerCode, int intpage, int introw);
        Task<IEnumerable<BoxMapping>> GetListBoxMapping1(string boxCode, string ProductCode, string sDate, string BuyerCode, int intpage/*, int introw*/);

        Task<int> CountBoxMapping(string boxCode, string ProductCode, string sDate, string BuyerCode);

        Task<IEnumerable<MappedProductModel>> GetListMappedProducts(string boxCodeMapped, string buyerCode);

        Task<IEnumerable<string>> GetMaterialCodeFromBoxMapping();

        Task<IEnumerable<string>> GetBuyerCodeFromStampDetail();

        Task<IEnumerable<MaterialInfoTIMS>> GetListMaterialInfoTIMS(string buyerCode, string materialCode, string materialNo);

        Task<IEnumerable<StyleInfo>> GetListStyleInfo();

        Task<MappedProductModel> ReturnGeneralfgByBuyerQR(string BuyerQR, string vendor);

        Task<IEnumerable<FGMappingBoxModel>> GetListDataToCheckMappingBox(string listId);

        Task<IEnumerable<LotDateModel>> GetListLotDateFromStampDetail(string proCode, string stampCode, string listIdStampDetail);

        Task<StampDetail> GetStampDetail(string product_code);

        Task<StyleInfo> GetStyleInfo(string product_code);

        Task<IEnumerable<ListIntModel>> GetListIntModel(string tempBoxQR);

        Task<int> UpdateStampDetail(string BuyerQR, string BoxQR);

        Task<int> InsertIntoBoxMapping(string boxQR, string userId, string idStr, string style_no);

        Task<int> UpdatePartialGeneralfg(string idStr);

        Task<IEnumerable<BoxMappingResponse>> GetBoxMapping(string boxQR);

        Task<BuyerQRModel> PrintQRCodeForMappingBox(string bx_no);

        Task<string> GetDataStampType(string buyer_qr);
        Task<generalfg> FindGeneralfg(string buyer_qr);
        Task<IEnumerable<generalfg>> FindGeneralfg1(string buyer_qr);

        Task<IEnumerable<MaterialInfoTIMS>> GetDataBuyerFromMaterialInfoTIMS(string buyer_qr);
        Task<IEnumerable<MappedProductModel>> IsCheckBuyerExist(string buyer_qr);

        Task<IEnumerable<Generalfg>> GetDataExistedProductList(string buyer_cd);

        Task<IEnumerable<w_box_mapping>> GetListTempData(string buyer_cd);
        #endregion

        #region Shipping Management
        Task<IReadOnlyList<DeliveryResponse>> GetListDileveryInfomation(string dl_no, string dl_nm, string productCode, string start, string end);
        Task<DeliveryInfo> GetLastDileveryInfo();
        Task<int> InsertIntoDeliveryInfo(DeliveryInfo item);
        Task<int> UpdatePartialDeliveryInfo(int dlid, string use_yn);
        Task<DeliveryInfo> GetLastDileveryInfoById(int dlid);
        Task<int> UpdateDeliveryInfo(DeliveryInfo item);
        Task<BoxMapping> CheckStampBox(string box_no, string status);
        Task<BoxMapping> CheckStampBoxExist(string box_no);
        Task<int> UpdatePartialBoxMapping(string user, string html);
        Task<int> UpdatePartialMaterialInfoTIMS(string dl_no ,string user, string listStamp);
        Task<int> UpdatePartialGeneralfg(string dl_no, string user, /*IEnumerable<string>*/string listStamp);
        Task<int> UpdatePartialStampDetail(string user, /*IEnumerable<string>*/ string listStamp);
        Task<IEnumerable<DeliveryResponse>> GetListDileveryInfo(string dl_no);

        #endregion
        Task<IReadOnlyList<ByerQR>> GetListBuyerQr(string productCode, string poCode, string start, string end);
        Task<IReadOnlyList<DatawActualPrimaryResponse>> GetlistDataActualPrimary(string product, string at_no, string reg_dt);
        Task<IReadOnlyList<MaterialInfoTIMS>> GetListFGPO(string at_no);
        Task<string> GetProductforPrimary(string at_no);
        Task<IReadOnlyList<BoxMapping>> GetlistBoxNobyMTCD();
        Task<IReadOnlyList<d_bobbin_lct_hist>> GetBobinHistory(string bb_no);
        Task<IReadOnlyList<truyxuatlot>> Truyxuatlistlot(StringBuilder sql);
        Task<IEnumerable<MaterialInfoTIMS>> GetDataBuyersFromMaterialInfoTIMS(string buyer_qr);
        Task<IEnumerable<FGGeneral>> GetFGGeneral(string bom_type, string productCode, string productName, string recevice_dt_start, string recevice_dt_end, string buyerCode);
        Task<int> InertFGGeneral(generalfg item);
        Task<int> InsertStampDTL(stamp_detail item);
        Task<int> CheckGeneralFG(string buyerqr);
        Task<int> CheckStampDTL(string buyerqr);
        Task<IReadOnlyList<d_style_info>> GetListDStyleInfo();
        Task<generalfg> GetGeneralFGByBuyerQR(string buyerqr);
        Task<MaterialInfoTIMS> GetDataBuyerFromMaterialInfoTIMSForSAP(string buyer_qr);
        Task<int> UpdateWMaterialInfoTimsFromBuyerQR(string buyer_qr, string inputdt, string lctcd, string fromlctcode, string tolctcode, string userid, string status);
        Task<string> CheckGeneralFGproduct(int id);
        Task<IReadOnlyList<FGProductLotPO>> GetListFGProductLotPO(string atno);
        Task<IReadOnlyList<generalfg>> GetExportShippingScanToExcel(string dlNo);
        Task<IReadOnlyList<generalfg>> GetShippingDLExportExcel(string dlNo);
        Task<int> UpdateStampDetailCancel1ThungDelivery(string boxno);
        Task<int> UpdatewmaterialinfowithBuyerCancel1ThungDelivery(string boxno);
        Task<int> UpdategeneralfgCancel1ThungDelivery(string boxno);
        Task<int> DeletewboxmappingCancel1ThungDelivery(string boxno);
        Task<int> UpdatewmaterialinfowithdlnoCancelDeliveryAll(string dlno);
        Task<int> DeletewboxmappingdlnoCancelDeliveryAll(string dlno);
        Task<int> UpdatestampdetailCancelDeliveryAll(string dlno);
        Task<int> DeletewdlinfodlnoCancelDeliveryAll(string dlno);
        Task<w_dl_info> CheckDLExist(int id);
        Task<string> CheckAnyGenneral(string dl_no);
        Task<int> DeleteDeliveryForId(int? id);
        Task<IReadOnlyList<w_dl_info>> Getwdlinfodlno(string dlno);
        Task<IReadOnlyList<spFGWMMSGetFGGeneral>> spFGWMMSGetFGGeneral(string productCode, string poCode, string recevice_dt_start, string recevice_dt_end, string buyerCode);
        Task<IReadOnlyList<MaterialInfoTIMS>> GetDLShippingScanListPP(string dlno);
        Task<IReadOnlyList<w_box_mapping>> Getlistwboxmappingwithboxno(string boxno);
        Task<IReadOnlyList<ScanBoxModel>> GetBoxCodeScanning(string boxno);
        Task<int> UnMappingBuyer(string buyerCode);
        Task<int> UnMappingBox(string boxno);
        Task<IEnumerable<string>> GetListStampData(string html);
        Task<IEnumerable<Generalfg>> GetListStampBuyerQr(string buyerCode, string prouductCode, int intpage, int introw);
        Task<int> CountStampBuyerQr(string buyerCode, string prouductCode);
        Task<StampDetail> GetDataStampDetail(string id);
        Task<List<generalfg>> GetListGeneralFGforExport(string productCode, string buyerCode, string recevice_dt_start, string recevice_dt_end);

        Task<string> GetBuyerQRFromInfoTims(int Id);
        Task<IReadOnlyList<FGShippingScanExportExcelModel>> GetDeliveryFG(string dl_no);
        Task<int> UpdateQtyGenneral(generalfg item, string id);
        Task<int> UpdateLotNoGenneral(generalfg item, string id);
        Task<IReadOnlyList<generalfg>> GetFGGeneralById(string id);
        Task<int> UpdateQtyGeneral(generalfg item);
        Task<int> TotalRecordsSearchShippingSortingFG(string ShippingCode, string productCode, string productName, string description);
        Task<IReadOnlyList<ShippingFGSortingModel>> GetListSearchShippingSortingFG(string ShippingCode, string productCode, string productName, string description);
        Task<ShippingFGSortingModel> GetLastShippingFGSorting();
        Task<int> InsertToShippingFGSorting(ShippingFGSortingModel item);
        Task<int> ModifyShippingFGSorting(ShippingFGSortingModel item);
        Task<generalfg> CheckIsExistBuyerCode(string BuyerCode);
        Task<int> UpdateShippingSortingFG(generalfg item, string data);
        Task<int> InsertShippingSortingFGDetail(string ShippingCode, string ListId, string UserID);
        Task<IReadOnlyList<ShippingFGSortingModel>> GetListSearchShippingSortingFGPP(string ShippingCode);
        Task<IReadOnlyList<ShippingFGSortingDetailModel>> GetListShippingFGSorting(string ShippingCode);
        Task<ShippingFGSortingDetailModel> CheckFGSortingdetail(string buyer_qr);
        Task<generalfg> isCheckExistGenneral(string buyer_qr);
        Task<int> UpdateBuyerGeneral(generalfg item);
        Task<int> DeleteFGSotingBuyer(string buyer_qr);
        Task<IReadOnlyList<ShippingFGSortingDetailModel>> GetShippingScanPPCountbuyer(string ShippingCode);
        Task<int> DeleteToExportToMachine(int id);
        Task<int> CheckMaterialEP1(string ExportCode);
        Task<ShippingTIMSSortingDetailModel> isCheckExistSF(string ShippingCode, string buyer_qr);
        Task<string> CheckStatus(string status);
        Task<int> UpdateLocationtimSorting(ShippingTIMSSortingDetailModel item);
        Task<MaterialInfoTIMS> isCheckExistWmaterialBuyer(string buyer_qr);
        Task<int> UpdateWMaterialInfoStatus(MaterialInfoTIMS item);
        Task<IReadOnlyList<ShippingTIMSSortingDetailModel>> CheckSTinfo(string ShippingCode);
        Task<StyleInfo> GetStyleInfoReplace(string product_code);
        IEnumerable<FGGeneral> GetFGGeneraldetail(string bom_type, string productCode, string productName, string recevice_dt_start, string recevice_dt_end, string buyerCode);
        Task<IReadOnlyList<FGGenneralExport>> GetFGGeneralExport(string bom_type, string productCode, string productName, string recevice_dt_start, string recevice_dt_end, string buyerCode);
        Task<string> GetListAuthorMenuInfo(string user_id, string url);
        Task<int> CountListBoxMapping(string boxCode, string ProductCode, string sDate, string BuyerCode);
        Task<int> UpdateReceFGWMaterial(MaterialInfoTIMS item);
        Task<stamp_detail> FindStamp(string buyer_qr);
        Task<List<string>> GetBuyerQRGenneralFG(string[] sizeList);
        Task<List<string>> GetAllBuyerQRGenneralFG(string[] sizeList);
        Task<IEnumerable<FGReceiData>> GetListBuyerQRGenneralFG(string model, string ProductName, List<string> sizeList);
    }
}