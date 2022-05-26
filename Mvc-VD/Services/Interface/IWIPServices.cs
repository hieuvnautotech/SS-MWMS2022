using Mvc_VD.Models.DTO;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models.Response;
using Mvc_VD.Models.WIP;
using Mvc_VD.Models.WOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Mvc_VD.Services.Interface
{
    public interface IWIPServices
    {
        Task<IEnumerable<ReturnModelResponse>> GetListDataInventoryReturn(string mt_cd);
        Task<ReturnModelResponse> GetListDataToPrintQR(int id);
        Task<ReturnModelResponse> GetListDataUpdatedToPrintQR(string id);
        Task<int> CountMaterialInventoryProduct(int id);
        Task<InventoryProduct> FindMaterialInventoryProductById(int id);
        Task<IReadOnlyList<InventoryProduct>> FindListMaterialInventoryProductById(string id);
        Task<IEnumerable<PrintQRResponse>> GetListPrintQRInventory(List<string> Id);
        Task<int> InsertIntoInventoryProduct(InventoryProduct item);
        Task<InventoryProduct> CheckMaterialsInventoryProduct(string materialCode);
        Task<InventoryProduct> CheckMaterialsInventoryProductByMaterialId(string id);
        Task<int> UpdateMaterialToWIP(InventoryProduct item);
        Task<string> GetDetailName(string mt_cd, string status);

        Task<IEnumerable<CompositeMaterialDetailResponse>> GetCompositeMaterialDetailData(string product, string bb_no, string status);

        #region Export Material

        Task<int> TotalRecordsSearchExportToMachine(string ExportCode, string ProductCode, string ProductName, string Description);
        Task<IEnumerable<ExportToMachineModel>> GetListSearchExportToMachine(string ExportCode, string ProductCode, string ProductName, string Description);
        Task<IEnumerable<ExportToMachineModel>> GetListExportToMachine();
        Task<int> InsertToExportToMachine(ExportToMachineModel item);
        Task<ExportToMachineModel> GetExportToMachineById(int Id);
        Task<int> CheckMaterialEP(string ExportCode);
        Task<int> DeleteToExportToMachine(string ex_cd);
        Task<int> ModifyToExportToMachine(ExportToMachineModel item);
        Task<int> FinishExportToMachine(ExportToMachineModel item);
        Task<IEnumerable<ExportToMachineModel>> GetListSearchExportToMachinePP(string ExportCode);
        Task<IEnumerable<WMaterialInfoNewResponse>> GetListExportToMachine(string ExportCode);
        Task<IEnumerable<ShippingScanPPResponse>> GetListShippingScanPP(string ExportCode);
        Task<IEnumerable<LocationInfo>> GetLocationWIP(string lct_cd);
        Task<LocationInfo> CheckIsExistLocation(string lct_cd);
        Task<IEnumerable<LocationDetailResponse>> GetListLocationDetail(string lct_cd, string myDay);
        Task<InventoryProduct> CheckInInventoryProduct(string materialCode);
        Task<IEnumerable<CommCode>> CheckStatus(string status);
        Task<int> DeleteMaterialInfoTam(string ml_no);
        Task<int> UpdateListMaterialToMachine(InventoryProduct item, List<string> data);
        Task<IEnumerable<InventoryProduct>> GetListInventoryProduct(string mt_cd);
        Task<int> UpdateListMaterialInventoryProduct(int id_actual, string status, DateTime? create_date, DateTime? change_date, string materialid);
        Task<int> UpdateMaterialInventoryProduct(int id_actual, string status, DateTime? create_date, DateTime? change_date, int materialid);
        #endregion

        #region Inventory
        Task<int> TotalRecordsSearchGeneralMaterialWIP(string mt_no, string product_cd, string mt_nm, DateTime? recevice_dt_start, DateTime? recevice_dt_end, string sts, string lct_cd, string mt_cd);

        Task<IEnumerable<GeneralWIP>> GetListGeneralMaterialWIP(string mt_no, string product_cd, string mt_nm,  DateTime? recevice_dt_start, DateTime? recevice_dt_end, string sts, string lct_cd, string mt_cd);

        Task<IEnumerable<GeneralResponse>> GetListGeneral(string at_no, string model, string product, string product_name, string reg_dt_start, string reg_dt_end, string mt_cd, string bb_no, string status);

        Task<IEnumerable<MemoResponse>> GetListMemoInfo(string MTCode, string memoProductCode);

        Task<IEnumerable<GeneralDetailWIP>> GetListGeneralMaterialDetailWIP(string mt_no, string product_cd, string mt_nm, DateTime? recevice_dt_start, DateTime? recevice_dt_end, string sts, string lct_cd, string mt_cd);

        Task<IEnumerable<MemoDetailResponse>> GetListMemoDetail(string mtCodeMemo, string memoWidth, string memoSpec, string productCodeMemo);

        Task<IEnumerable<PrintMaterialQRInventoryResponse>> PrintMaterialQRInventory(string mt_no);

        Task<IEnumerable<GeneralWIP>> GetListGeneralExportToMachine(string mt_no, string product_cd, string mt_nm, DateTime? recevice_dt_start, DateTime? recevice_dt_end, string sts,string mt_cd);
        // Task<int> TotalRecordsSearchGeneralMachine(string mt_no, string product_cd, string mt_nm, DateTime? recevice_dt_start, DateTime? recevice_dt_end, string sts, string lct_cd, string mt_cd);
        Task<IEnumerable<GeneralDetailWIP>> GetListGeneralMaterialDetail(string mt_no, string product_cd, string mt_nm, DateTime? recevice_dt_start, DateTime? recevice_dt_end, string sts, string mt_cd);
        Task<IEnumerable<WIP_ParentInventoryModelExport>> ExportExcelGeneralMaterialDetail(string mt_no,string mt_nm, string sts, string recevice_dt_start, string recevice_dt_end,string ProductCode);
        #endregion

        #region Receving Scan (W)

        Task<MaterialInfoTam> GetDataMaterialInfoTam(string ml_no);

        Task<int> DeleteMaterialInfoTamByCode(string mt_cd);
        #endregion

        #region Shipping Scan (WIP-WMS)
        Task<IEnumerable<ExInfo>> GetListDataExInfo(string ex_no, string ex_nm);
        Task<IEnumerable<ExInfo>> GetListDataExInfo(string ex_no);
        Task<string> GetLastDataExInfo();
        Task<int>InsertIntoExInfo(ExInfo item);
        Task<ExInfo> GetExInfoById(int exid);
        Task<int> UpdateToInvertoryProduct(string ex_no);
        Task<int> DeleteExInfoById(int exid);
        Task<int> UpdateExInfo(ExInfo item);
        Task<int> CheckMaterialInventoryProduct(string mt_cd);
        Task<int> CheckMaterialInfoTam(string mt_cd);
        Task<IEnumerable<MaterialInfoMemo>> GetListDataMaterialMemo(string sd_no);
        Task<IEnumerable<PickingScanPPCountMTNoResponse>> GetPickingScanPP_Count_MT_no(string ex_no);
        Task<IEnumerable<ShippingWIPListPPResponse>> GetShippingWIPListPP(string ex_no);
        Task<int> TotalRecordSearchExportToMachineFinish(string ExportCode, string ProductCode, string ProductName, string Description);
        Task<IEnumerable<ExportToMachineModel>> GetListExportToMachineFinish(string ExportCode, string ProductCode, string ProductName, string Description);

        Task<int> UpdatePartialStatusExInfo(string data);
        Task<int> InsertIntoStatusExInfo(string data);
        Task<int> UpdateShippingMaterialReturn(string data, string ex_no, string full_date, string user);
        Task<IEnumerable<InventoryProduct>> FindMaterialInventoryProductByListId(List<int> listData);

        Task<string> GetDetailNameByComm_DT(string mt_cd, string dt_cd);
        #endregion

        #region Change Rack

        Task<IEnumerable<InventoryProduct>> CheckNewMaterialInventoryProduct(string materialCode);

        Task<int> UpdateChangeRackMaterialToMachine(InventoryProduct item, string ListId);

        Task<int> UpdateReturnMaterialToWIP(InventoryProduct item, string ListId);

        Task<int> InsertReturnMaterialToWIP(string ListId);
        #endregion


        Task<IEnumerable<wipgeneralexportexcel>> ExportMaterialByExcel();
        Task<IEnumerable<WIP_ParentInventoryExport>> PrintMaterialByExcel(string mt_no,string mt_cd,string s_product_cd, string s_locationNAme, string recevice_dt_start, string recevice_dt_end);
        Task<int> UpdateQuantityInventoryProduct(double gr_qry, int id);
        Task<int> UpdateQuantityInventoryProduct(double gr_qry, double real_qty, int id);
        Task<IEnumerable<InventoryProduct>> GetInventoryProductByOrginMaterialCode(string orgin_mt_cd);
        Task<IEnumerable<w_material_model>> UpdateLengthReturn(string id);
        Task<IEnumerable<ExcelInventoryWIPComposite>> printExcelTab2(string at_no, string model, string product, string product_name, string reg_dt_start, string reg_dt_end, string mt_cd, string bb_no, string status);
    }
}