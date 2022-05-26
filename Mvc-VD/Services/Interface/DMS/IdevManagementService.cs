using Mvc_VD.Classes;
using Mvc_VD.Controllers;
using Mvc_VD.Models;
using Mvc_VD.Models.APIRequest;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models.Request;
using Mvc_VD.Models.Response;
using Mvc_VD.Models.WMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static Mvc_VD.Controllers.DevManagementController;
using MaterialInfoMMS = Mvc_VD.Models.NewVersion.MaterialInfoMMS;

namespace Mvc_VD.Services.Interface
{
    public interface IdevManagementService
    {

        #region DMS

        #region BOM
        Task<BomInfo> CheckBomExist(string style_no, string mt_no);
        Task<int> InsertToMaterialBom(BomCreateMaterialReponse item);
        Task<int> DelMaterialBom(int Id);
        Task<BomResponse> GetBomManagementById(int bid);
        Task<IEnumerable<BomResponse>> GetListBomManagement(string bom_no, string product, string product_name, string md_cd, string mt_no, string mt_name);
        Task<IEnumerable<GetMaterialChildForBomResponse>> GetListBomManagement(string bom_no, string styleNo, string product, string product_name, string md_cd, string mt_no, string mt_name);
        Task<IEnumerable<MaterialBomResponse>> GetListMaterialForBom(int id);
        Task<IEnumerable<BomCreateMaterialReponse>> GetListMaterialForBom(string style_no, string mt_no);
        Task<int> CheckBom(string style_no, string mt_no, int Id);
        Task<int> CheckBom(string styleNo);
        Task<int> CheckBom(string bomNo, string mtNo);
        Task<int> CheckBom();
        Task<IEnumerable<BomInfo>> GetListBom();
        Task<int> CheckExistedBonInfo(string productCode, string materialNo);
        Task<BomInfo> GetBomInfo(int id);
        Task<int> UpdateBomToCalculatePerformance(string product);
        Task<IEnumerable<BomInfo>> GetListBomÌnfo(int id);
        Task<int> InsertToBomInfo(BomInfo item);
        Task<int> UpdateBomInfo(BomInfo item);
        Task<int> DeleteBomInfo(string style_no);

        Task<int> DelBomMaterial(int bid);
        #endregion

        #region Material Management
        Task<ProductMaterial> GetTop1ProductMaterial();
        Task<IReadOnlyList<MaterialInfo>> GetListDMaterialInfo(string type, string name, string code, string start, string end, string sp);
        Task<int> InsertDMaterialInfo(MaterialInfo item);
        Task<int> UpdateDMaterialInfo(MaterialInfo item);
        Task<int> DeleteMaterialInfo(int mtid);
        Task<int> InsertTmpCreateQRForMaterial(TmpCreateMaterialInfo item);
        Task<int> InsertWMaterialInfoTmp(List<MaterialInfoTam> item);
        Task<int> CheckTmpMaterial(string MtNo);
        Task<MaterialInfo> GetMaterialNoFromMaterialInfo(string mt_no);
        Task<IEnumerable<MaterialInfo>> GetListDataMaterialNoFromMaterialInfo(string mt_no);
        Task<int> UpdateQtyWMaterailTam(double gr_qty, double real_qty, string chg_id, DateTime chg_dt, string id);
        Task<d_material_info> CheckBarCode(string mt_no);
        Task<IEnumerable<QCItemMaterial>> GetListQCItemMaterial(string item_type, string del_yn);
        Task<MaterialInfo> GetDMaterialInfoById(int mtid);
        Task<string> GetItemNameByItemvcd(string itemVcd);
        Task<int> CountDMaterialInfo(string mt_no);
        Task<IEnumerable<SupplierInfo>> GetListSupplierInfo(string mt_no);
        Task<IEnumerable<SupplierInfo>> GetListSupplierInfoForPopup(string suplier_no, string suplier_nm);
        Task<IEnumerable<ManufacInfo>> GetListManuFacInfoForPopup();
        Task<comm_dt> GetListComDetailForPopup(string dt_cd);
        Task<IEnumerable<MaterialInfo>> SearchMaterialInfo(string name, string type, string code, string start, string end);
        Task<IEnumerable<MaterialInfo>> GetDataMaterialToExport(string name, string type, string code, string sp, string start, string end);
        Task<int> CheckMaterialCode(string mt_cd);
        Task<int> CheckExistMaterialInBom(string mt_no);
        Task<int> CheckIsExistMaterialRouting(string mt_no);
        Task<int> CheckIsExistMaterialRouting2(string mt_no);
        Task<int> CheckIsExistProductBom(string style_no);
        Task<int> CheckIsExistProductRouting(string style_no);
        Task<IEnumerable<WMaterialTamReponse>> GetListDataMaterialInfoTamByMt_Cd(string mt_cd);

        Task<IEnumerable<string>> GetListMaterialInfoTam(string mt_cd);
        #endregion

        #region Develop Common
        Task<IEnumerable<QCItemMaterial>> GetListItemMaterial(string item_type, string del_yn);
        Task<IEnumerable<CommMt>> GetListCommMT();
        Task<IEnumerable<CommCode>> GetListCommDT(string mt_cd);
        Task<int> DelateMoldInfo(int mdno);
        Task<IEnumerable<CommCode>> GetListCommDT(string mt_cd, string use_yn);
        Task<IEnumerable<DevelopCommonResponse>> GetListDevelopCommonData(string mt_cd);
        Task<int> CheckComDT(string dt_cd, string mt_cd);
        Task<int> CheckComDT(string mt_cd);
        Task<int> CheckComMT(string mt_cd);
        Task<int> InsertIntoComDT(CommCode comm_dt);
        Task<int> InsertIntoComMT(CommMt comm_mt);
        Task<int> DeleteComDT(string mt_cd, string dt_cd);
        Task<int> DeleteComDT(string mt_cd);
        Task<int> DeleteComMT(string mt_cd);
        Task<CommCode> GetCommCodeById(int id);
        Task<int> UpdateComDT(CommCode comDT);
        Task<CommMt> GetCommMTById(int id);
        Task<int> UpdateComMT(CommMt comMT);

        #endregion

        #region Product Management
        Task<IReadOnlyList<StyleInfo>> GetListDStyleInfo();
        Task<d_style_info> GetStyleInfo(int id);
        Task<int> RemoveStyleInfo(int id);
        Task<int> InsertIntoStyleInfo(d_style_info item);
        Task<int> UpdateStyleInfo(d_style_info item);
        Task<StyleInfo> GetStyleNoFromStyleInfo(string style_no);
        Task<int> InsertProductExcel();
        Task<IEnumerable<ModelReturnProductExcel>> GetListProductExcel(string style_no);
        Task<IEnumerable<StyleInfo>> searchStyleInfo(string code, string codeName, string modelCode, string projectName, string start, string end);
        Task<int> CheckProductInStyleInfo(string style_no);
        #endregion

        #region Model Management
        Task<IEnumerable<ModelInfo>> GetListModelInfo(string modelCode, string modelName);
        Task<int> CheckModelInfo(string md_cd);
        Task<int> InsertModelInfo(ModelInfo item);
        Task<ModelInfo> GetModelInfoById(int mdid);
        Task<int> RemoveModelInfo(int mdid);
        Task<int> UpdateModelInfo(ModelInfo item);
        Task<IEnumerable<ModelInfo>> GetListModelInfo();
        Task<IEnumerable<ModeCodeResponse>> SearchModeCode(string md_cd, string md_nm);
        Task<IEnumerable<ModelReturnModelExcel>> GetListModelExcel(string md_cd);
        #endregion

        #region Routing
        Task<DRoutingInfo> GetTop1RoutingInfo();
        Task<IEnumerable<DRoutingInfo>> GetListRoutingInfoByStyleNo(string style_no, string process_code);
        Task<int> InsertRoutingInfo(DRoutingInfo item);
        Task<int> UpdateRoutingInfo(DRoutingInfo item, string description, string isFinish);
        Task<int> DeleteRoutingInfo(int idr);
        Task<CommCode> GetUnitFromCom_DT(string mt_cd, string unit);
        Task<string> GetDetailNameFromCom_DT(string mt_cd, string unit);
        Task<DRoutingInfo> GetRoutingInfoById(int id);
        Task<int> CheckExistsProductMaterial1(string style_no, int level, string mt_no, string process_code);
        Task<IEnumerable<ProductMaterialDetail>> CheckExistsProductMaterialDetail(string productCode, string materialParent, List<string> materialNo);
        Task<int> UpdateMaterialToCalculatePerformance(string product, string process_code);
        Task<int> InsertIntoProductMaterial(ProductMaterial item);
        Task<int> UpdateProductMaterial(ProductMaterial item);
        Task<int> DeleteProductMaterial(int id);
        Task<int> DeleteProductMaterialDetail(string ProductCode, string process_code, string name, string MaterialPrarent);
        Task<int> DeleteMaterialChild(int id);
        Task<string> GetMaterialName(string mt_no);
        Task<ProductMaterial> GetProductMaterialById(int Id);
        Task<d_style_info> GetProductByCode(string code);
        Task<int> CheckExistsProductMaterial2(string style_no, int level, string mt_no, int Id, string process_code);
        Task<int> InsertToMaterialChild(ProductMaterialDetail item);
        Task<IEnumerable<ProductMaterialDetail>> GetListMaterialChild(string ProductCode, string name, string MaterialPrarent, string process_code);
        Task<IEnumerable<DMaterialResponse>> SearchMaterial(string type, string name, string code, DateTime? start, DateTime? end, string sp);
        Task<IEnumerable<RoutingInfoResponse>> GetRoutinigInfo(string product, string process_code);
        Task<IEnumerable<ProductMaterial>> GetProductMaterial(string product, string name, string process_code);
        Task<string> GetNameFromMaterialInfo(string mt_no);
        Task<IEnumerable<MaterialBom>> ListExistsMaterialBomDetail(string productCode, string materialParent, List<string> materialBom);
        Task<int> CheckExistsProductMaterial3(string style_no, string name, string process_code);

        Task<int> DeleteMaterialExists(IEnumerable<ProductMaterialDetail> listData);

        Task<int> DeleteBomMaterialExists(IEnumerable<MaterialBom> listData);
        Task<ProductProcess> GetcheckProcessCode(string style_no);
        Task<int> UpdateProcessToApply(string style_no);
        Task<int> InsertToProductProcess(ProductProcess item);
        Task<IEnumerable<ProductProcess>> GetProductRouting(string product);
        Task<ProductProcess> GetcheckProcessByStyle(int id);
        Task<int> UpdateToProductProcess(ProductProcess item);
        Task<int> UpdateIsFinishDRoutingInfo(string style_no, string process_code);
        Task<IEnumerable<DMaterialResponse>> SearchMaterialRepalce(string type, string name, string code, DateTime? start, DateTime? end, string sp, string MaterialPrarent);
        Task<int> DeleteProductProcessForId(int id);
        #endregion

        #endregion

        #region TOOLS (MMS)

        #region Machine (MMS)
        Task<IEnumerable<MachineInfo>> GetListMchine();

        Task<IEnumerable<CommCode>> GetListCommDT();

        Task<IEnumerable<MachineInfo>> SearchMachineInfo(string mc_type_search, string mc_no_search, string mc_nm_search, DateTime? start_search, DateTime? end_search);

        Task<int> countMachineInfo(string mc_no);

        Task<MachineInfo> checkMachineInfo(int mno);

        Task<MachineInfo> getListMachineInfoById(int mno);

        Task<int> InsertIntoMachineInfo(MachineInfo item);

        Task<int> UpdateMachineInfo(MachineInfo item);

        Task<int> DeleteMachineInfo(int mno);

        Task<IEnumerable<ModelReturnMachineExcelResponse>> GetListMachineExcel(string mc_no);
        #endregion

        #region Bobbin Management (MMS)
        Task<IEnumerable<BobbinInfoResponse>> GetListBobbinInfo(string bb_no, string bb_name);
        Task<int> CheckBobbinInfo(string bb_no);
        Task<int> InsertIntoBobbinInfo(BobbinInfo item);
        Task<int> DeleteBobbinInfo(int bno);
        Task<int> UpdateBobbinInfo(BobbinInfo item);
        Task<BobbinInfo> GetBobbinInfoById(int bno);
        Task<IEnumerable<BobbinInfo>> GetBarCodeOfBobbinInfo(int bno);
        #endregion

        #region Staff Management (MMS)
        Task<IEnumerable<MbInfo>> GetListStaff();

        Task<IEnumerable<MbInfo>> SearchStaff(string searchType, string keywordInput, string position);

        Task<IEnumerable<CommCode>> GetPositionStaff();
        Task<IEnumerable<CommCode>> getgender();

        Task<IEnumerable<DepartmentInfo>> GetListDepartmentInfo();

        Task<int> countStaffbyUserId(string userId);

        Task<int> InsertStaff(MbInfo item);
        Task<int> checkStaff(string userId);

        Task<int> InsertListStaff(string Id, string Name, string Gender, string locationCode, string barCode, string positionCode, DateTime? BirthDate, string DepartmentCode, DateTime? JoinDate, string userInsert, DateTime dateInsert );

        Task<MbInfo> GetStaffbyUserId(string UserId);

        Task<IEnumerable<MbInfo>> GetListStaffbyUserId(List<string> UserId);

        Task<MbInfo> GetStaffbyId(string Id);

        Task<MbAuthorInfo> GetMBAuthorInfobyUserId(string UserId);

        Task<int> DeleteStaffbyUserId(string UserId);

        Task<int> DeleteMbAuthorInfobyUserId(string UserId);

        Task<int> UpdateStaff(MbInfo item);

        //Task<int> DeleteListStaffByUserId(List<string> userId);
        #endregion

        #region Status (MMS)
        Task<IEnumerable<BobbinLctHist>> GetListBobbinLctHist(string bb_no);

        Task<BobbinInfo> GetBobbinInfoByBB_No(string bb_no);

        Task<IEnumerable<MaterialMappingMMS>> GetListMaterialMappingMMS(string mt_cd);

        Task<IEnumerable<Actual>> GetListActual(int? id_actual);

        Task<IEnumerable<MaterialInfoMMS>> GetListDataMaterialInfoMMS(string mt_cd);

        Task<ActualPrimary> GetActualPrimaryById (string at_no);

        Task<IEnumerable<MaterialInfoTam>> GetListDataMaterialInfoTam(string mt_cd);

        Task<string> GetStaffIdByIdActual(int id_actual);
        Task<string> GetStaffIdByIdActualForTIMS(int id_actual);
        Task<string> GetDetailNameByDetailCode(string detailCode);

        Task<string> GetLcoationNameByLocationCode(string locationCode);

        Task<MbInfo> GetBarCodeOfStaffByUserId(string userid);

        Task<string> CheckActual(string name);

        Task<IEnumerable<InventoryProduct>> GetListDataInventoryProduct(string mt_cd);

        Task<IEnumerable<MaterialInfoTIMS>> GetListMaterialInfoTIMS(string mt_cd);
        #endregion

        #endregion

        #region STANDARD

        #region User Management

        Task<IEnumerable<MbInfo>> GetListUser();

        Task<IEnumerable<MbInfo>> SearchUser(string searchType, string keywordInput, string department, string position);

        Task<IEnumerable<AuthorInfo>> GetListGrande();
        Task<IEnumerable<modelTableMAchine>> PartialView_ResourceMgt(string po, string productCode, string machineCode, string date_end,string date_start);
        Task<IEnumerable<TableMachineDatetime>> PartialView_dialog_Viewdetail(string at_no, string mc_no);

        Task<string> GetCodeFromAuthorInfo(string grade);

        Task<int> CheckMbInfo(string userid);

        Task<int> InsertIntoMbInfo(MbInfo item);

        Task<int> InsertIntoMbAuthorInfo(MbAuthorInfo item);

        Task<MbInfo> GetUserById(string userid);

        Task<MbInfo> GetUserByUserId(string userId);

        Task<int> UpdateUseṛ(MbInfo item);

        Task<IReadOnlyList<mb_info>> GetListUserById();

        Task<int> UpdateMbUserInfor(MbAuthorInfo item);

        #endregion

        #region Tray Box Management

        Task<int> CreateTrayBox(string bb_cd, string chg_id, string reg_id, string mc_type, string bb_nm);

        Task<BobbinInfo> GetListTrayBox(string bb_no);

        Task<IEnumerable<BobbinInfo>> SearchTrayBox(string bb_no, string bb_nm);

        #endregion

        #endregion

        #region TIMS
        #region Create Buyer QR
        Task<IEnumerable<StyleInfo>> GetListStyleInfo(string style_no, string style_nm, string md_cd);

        #endregion
        #endregion
    }
}