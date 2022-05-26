using Mvc_VD.Classes;
using Mvc_VD.Models;
//using Mvc_VD.Models.DMS;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models.Request;
using Mvc_VD.Models.Response;
using Mvc_VD.Models.TIMS;
using Mvc_VD.Models.WOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MaterialInfoMMS = Mvc_VD.Models.NewVersion.MaterialInfoMMS;

namespace Mvc_VD.Services.Interface.MMS
{
    public interface IActualWOService
    {
        Task<int> GetTotalMaterialInfoDVTims(string mt_cd);
        Task<string> GetBBNOWMaterialInfoByMaterialCodeMMS(string mt_cd);
        Task<string> GetBBNOWMaterialInfoByMaterialCodeTIMS(string mt_cd);
        Task<IReadOnlyList<MaterialInfoMMS>> GetWMaterialInfoByMaterialCd(string mt_cd);
        Task<IReadOnlyList<MaterialInfoMMS>> GetTIMSInfoByMaterialCd(string mt_cd);

        #region ActualWO
        Task<IEnumerable<CommCode>> getListActualProccess();
        Task<IEnumerable<StyleInfo>> getListProduct();
        Task<IEnumerable<DatawActualPrimaryResponse>> getListPO(DataWActualPrimaryReq item);
        Task<IEnumerable<DatawActualPrimaryResponse>> getListFinishPO(DataWActualPrimaryReq item);
        Task<IEnumerable<ActualResponse>> getListActualOfProductOrder(string atNo);
        Task<int> GetPO(string atNo);
        Task<int> GetRedoPO(int id_actualpr);
        Task<int> UpdatePO(string finish_yn, int id_actualpr);
        Task<int> insertActualInfo(Actual item);
        Task<int> insertMaterialForActual(MaterialInfo item);
        Task<int> removeActualInfo(int id);
        Task<int> checkProductExisted(string product);
        Task<int> checkMaterialForActuals(string mt_no);
        Task<IEnumerable<w_actual>> checkActualExisted(string at_no);
        Task<int> removeProductOrder(int id);
        Task<int> updateProductOrder(ActualPrimaryModify item);
        Task<IEnumerable<DatawActualPrimaryResponse>> GetProcessingActual(string at_no);
        Task<Actual> GetActual(int id_actual);
        Task<int> UpdateDescriptionActual(int id_actual, string description);
        Task<w_actual_primary> CheckPOExisted(string name);
        Task<int> CheckPOExisted1(string name);
        Task<MaterialInfoMMS> GetMaterial(string bb_no, string mt_cd);
        Task<int> insertNewProductOrder(ActualPrimary productOrder);
        Task<IEnumerable<MaterialInfoMMS>> CheckMaterialOKInfoMMS(string mt_cd);
        #endregion

        #region Mapping Machine
        Task<IEnumerable<ProcessMachineunit>> GetListMachineFromIdActual(int id_actual);
        Task<IEnumerable<DMachineInfoAPI>> GetPopupMachine(string mc_type, string mc_no, string mc_nm, int page, int row);
        Task<string> GetStatusMachine(string mcno, int idactual, string start, string end);

        Task<int> InsertProUnitMC(DProUnitMachine item);
        Task<int> CheckMachineInfo(string mc_no);
        Task<int> GetTotalCountMachine(string mc_no, int id_actual, int pmid);
        Task<int> GetStatusMachineUpdate(string mcno, int idactual, int pmid, DateTime start, DateTime end);
        Task<ProcessMachineunit> GetMachineById(int Id);
        Task<int> UpdateMachine(ProcessMachineunit item);
        Task<int> removeMachineMapping(string Id, DateTime timeNow);
        Task<IEnumerable<comm_dt>> GetMachineType();

        Task<int> CountListPopupMachine(string mc_type, string mc_no, string mc_nm);
        #endregion

        #region Mapping Staff
        Task<int> removeStaffMapping(string Id, DateTime timeNow);
        Task<IEnumerable<comm_dt>> GetStaff(string mtcd, string useyn);
        Task<IEnumerable<ProcessUnitStaff>> GetListProcessStaff(int idactual);
        Task<int> TotalRecordsSearchStaffpp(string userid, string username, string position_cd);
        Task<IEnumerable<StaffPP>> GetListSearchStaffpp(string userid, string username, string position_cd, int pageinit, int rowinit);
        Task<int> CountListSearchStaffpp(string userid, string username, string position_cd);
        Task<string> GetStatusStaff(string staff_id, int id_actual, DateTime start, DateTime end);
        Task<int> InsertUnitStaff(DProUnitStaff item);
        Task<ProcessUnitStaff> GetStaffById(int psid);
        Task<IEnumerable<d_pro_unit_staff>> checkStaffInActual(int id_actual);
        Task<int> CheckUnitStaff(string staffId);
        Task<int> CheckProcessUnitStaff(int psid, string staff_id, DateTime StartDate, DateTime EndDate);
        Task<d_pro_unit_staff> GetUnitStaffById(int psid);
        Task<int> UpdateUnitStaff(d_pro_unit_staff item);
        #endregion

        #region Routinng
        Task<int> UpdateProductDeApply(int id_actualpr, string IsApply);
        Task<IEnumerable<DRoutingInfo>> GetRoutingMMS(string productCode, string productprocess_code);
        #endregion

        #region Mapping Container
        Task<int> CheckNewWorkerAndMachine(int id_actual);
        Task<IEnumerable<BobbinResponse>> GetListBobbin(string bb_no, string bb_nm, string mt_cd, int? id_actual, int page, int row);
        Task<int> CountListBobbin(string bb_no, string bb_nm, string mt_cd, int? id_actual);
        Task<int> CheckStaffShift(int id_actual);
        Task<int> CheckMachineShift(int id_actual);
        Task<int> CountDataMaterialInfo(string mt_cd);
        Task<IEnumerable<string>> GetStaffOnProcess(int id_actual);
        Task<IEnumerable<string>> GetMachineOnProcess(int id_actual);
        Task<Models.NewVersion.MaterialInfoMMS> GetDetailMaterialInfoMMS(int id);
        Task<int> InsertMaterialInfoMMS(MaterialInfoMMS item);
        Task<int> checkFullMtCode(string id);

        Task<int> InsertMaterialInfoMMSByModel(ChangeBobbinForMaterialChildrenResponse item);
        Task<int> UpdateBobbinMTCode(BobbinInfo item);
        Task<int> InsertToBobbinHistory(BobbinLctHist item);
        Task<string> CheckBobbinInheritance(int idActual);
        Task<string> CheckBobbinInheritance1(string Actual);
        Task<string> GetMaterialByIdActual(int idActual, string FullMtCode);
        Task<int> InsertMaterialMapping(string mt_cd, string userid, string mt_lot);
        Task<int> UpdateMaterialInfoFromMappingOtherType(string userid, string mt_type, string mt_lot);
        Task<int> UpdateMaterialInfoFromMappingByType(string userid, string mt_type, string mt_lot);
        Task<w_material_info_wo> GetMaterialInfoWo(string mt_no, string mt_cd, int idWMaterialInfo);
        Task<int> GetSumGroupQty(string material_code);
        Task<IEnumerable<InventoryProduct>> PopupCompositeMaterial(string mt_cd);
        Task<string> CheckMaxBobbin(string mt_cd, int id_actual);
        Task<string> CheckBobbinInventoryProduct(string mt_cd, int id_actual);
        Task<MaterialInfoMMS> GetWMaterialInfoByMaterialCode(string mt_cd);
        Task<int> GetTotalDevideWMaterialInfoByMaterialCode(string mt_cd);
        Task<InventoryProduct> GetMaterialFromInventoryProducts(string mt_cd);
        //Task<MaterialInfoMMS> GetMaterialFromInventoryPorudtc(string mt_mapping);
        Task<int> CheckMaterialMapping(string mt_cd);
        Task<BobbinInfo> GetBobbinInfo(string bb_no);
        Task<BobbinLctHist> GetBobbinLctHist(string bb_no);
        Task<int> UpdateBobbinInfo(BobbinInfo item);
        Task<string> IsCheckApply(string at_no);
        //Task<bool> IsMaterialInfoExistByBom(string productCode, string MaterialNo);
        Task<IEnumerable<CommCode>> CheckStatus(string mt_cd, string status);
        Task<int> GetCountMaterialMapping(string mt_cd, string mt_lot);
        Task<string> GetStaffFromActual(int id_actual);
        Task<IEnumerable<string>> GetListStaffFromActual(int id_actual);
        Task<int> CheckShiftOfStaff(int id_actual, IEnumerable<string> staff_id);
        Task<int> CheckHetCaMT(int id_actual, string mt_cd);
        Task<int> GetMaterialMapping(string mt_no, string mt_cd, string mt_lot);
        Task<int> InsertMaterialMapping(MaterialMappingMMS item);
        Task<IEnumerable<DProUnitStaff>> CheckStaff(int id_actual);
        Task<IEnumerable<DProUnitMachine>> CheckMachine(int id_actual);
        Task<int> UpdateMaterialInfoMMS(MaterialInfoMMS item);
        Task<int> UpdatemtcdforBobbinInfoForRedo(string bbno, string mtcd);
        Task<DBobbinInfo> FindOneDBobbinInfo(string item);
        Task<int> InsertBobbinHist(DBobbinLctHist bobbinhist);
        Task<int> InsertMaterialMappingReturn(int count, int quantity, int numberDV, string status, string chg_id, string mt_cd);
        Task<IEnumerable<SaveReturn>> GetDataSaveReturn(int id);
        Task<IEnumerable<MtDateWebWO>> GetMtDateWebs(int id_actual);
        Task<IEnumerable<SaveReturn>> GetDataMappingMaterial(string mt_cd);
        Task<int> StatusMapping(string mt_cd);
        Task<IEnumerable<MaterialMappingMMS>> GetMaterialMappingByMaterialCode(string mt_cd);
        Task<MaterialMappingMMS> GetMaterialMappingById(int wmmid);
        Task<MaterialInfoMMS> GetMaterialInfoforBobbin(string mt_cd, string mt_type);
        Task<int> DeleteMaterialMapping(int Id);
        Task<string> GetBBNoFromMaterialMMS(string mt_cd, string mt_no);

        Task<BobbinLctHist> GetBobbinLctHistory(string bb_no, string mt_cd);
        Task<int> DeleteMaterialInfoMMs(int wmtid);
        Task<IEnumerable<CommCode>> GetTypeMaterial();
        Task<MbInfo> GetMbInfoGrade(string uname);
        Task<int> CheckFaclineqc(string mt_cd);
        Task<IEnumerable<CheckUpdateGrty>> GetUpdateGrty(int wmtid);
        Task<IEnumerable<MaterialToDeviceResponse>> GetMaterialToDevice(int id_actual);
        Task<MaterialInfoMMS> GetMaterialInfoOfDevice(string mt_cd, string status);
        Task<int> GetUnitStaffforDevice(int id_actual);
        Task<int> UpdatedBobbinInfoForDevice(string bb_no, string mt_cd, string chg_id);
        Task<int> DeleteDBobbinLctHistForDevice(string bb_no, string mt_cd);
        Task<int> GetTotalMaterialInfoDV(string mt_cd);
        Task<int> UpdateMaterialMappingMMS(MaterialMappingMMS item);
        Task<IEnumerable<BobbinInfo>> SearchBobbinInfo(string bb_no, string bb_nm);
        Task<BobbinLctHist> GetBobbinLctHistFrom(string bb_no, string mt_cd);
        Task<int> UpdateBobbinLctHist(BobbinLctHist item);
        Task<int> InsertToBobbinLctHistory(BobbinLctHist item);
        Task<int> DeleteBobbinLctHist(int blno);
        Task<int> UpdateMaterialInfoDivideMMS(string bb_no, int wmtid, string type);
        Task<string> GetAtNoByActual(int id_actual);
        Task<IEnumerable<int>> GetMaterialMappingNVL(string style_no, string mt_cd, string name, string process_code);
        Task<ActualMaxLevelResponse> NameBTP(string at_no, string name);
        Task<string> IsBTPExistByMapping(string mtLot, string mt_no);
        Task<string> IsMaterialInfoExistByProcess(string productCode, string name, string MaterialNo, string process_code);
        Task<string> CheckMaterialRetun(string mt_cd);
        Task<string> CheckInventoryProductRetun(string mt_cd);
        Task<int> CheckMaterialMappingFinish(string mt_cd, string mt_lot, DateTime mapping_dt);
        Task<int> GetIdActual();
        Task<int> CheckExitReturn(string orgin_mt_cd);
        Task<MaterialMappingMMS> GetMaterialMappingReturn(string mt_cd, string mt_lot);
        Task<int> CheckMaterialMappingForRedo(string mt_cd);
        Task<IEnumerable<MaterialInfoMMS>> CheckMaterialMappingContainer(string mt_cd);
        Task<IEnumerable<MaterialInfoMMS>> CheckMaterialMappingContainer1(string mt_cd);
        Task<int> CountMaxMapping(string mt_cd, string mt_lot);
        Task<BobbinInfo> GetBobbinInfoReturn(string bb_no, string mt_cd);
        Task<int> UpdateMaterialMappingForDivide(string mt_cd, string use_yn, string useyn, string chg_id);

        Task<int> InsertMaterialMappingReturn(int count, int quantity, string status, string chg_id, string mt_cd);
        Task<int> CheckMaterialToMapping(string mt_cd);
        Task<int> UpdatePartialInventoryProduct(int id, string status, double qty, string change_id, DateTime? change_date);
        Task<InventoryProduct> GetDataInventoryProduct(string  mt_cd);
        Task<int> CheckMaterialMappingOfBobbbin(string mt_mapping);
        Task<InventoryProduct> GetMaterialByInventoryProduct(string mt_cd);
        Task<int> UpdateInventoryProduct(InventoryProduct item);
        Task<int> CountInventoryProduct(string mt_cd);
        Task<IEnumerable<string>> GetStaffFromActualByInventoryProduct(int id_actual);
        Task<InventoryProduct> GetDetailInventoryProduct(int Id);
        Task<ActualPrimary> GetDataActualPrimary(string at_no);
        Task<MaterialInfoMMS> GetWMaterialInfoByMaterialCodeTIMS(string mt_cd);
        Task<int> GetTotalWMaterialInfoByMaterialCodeTIMS(string mt_cd);
        Task<int> GetTotalWMaterialInfoByMaterialCodeMGTIMS(string mt_cd);
        Task<int> GetTotalWMaterialInfoByMaterialCodeMGMMS(string mt_cd);
        Task<MaterialMappingMMS> CheckwMaterialMappingMax(string mt_cd);

        #endregion

        #region Get Tổng Hợp
        Task<IEnumerable<wo_info_mc_wk_mold>> GetInfoMcWkMold(int idActual);
        Task<IEnumerable<WMaterialInfoAPI>> GetDetailActualAPI(int id_actual, string mt_type, string date, string shift);
        #endregion

        #region Print NG
        Task<IEnumerable<NGResponse>> GetListNG(string code, string at_no, string product);
        Task<IEnumerable<MaterialInfoMMS>> GetListMaterialOK();
        Task<MaterialInfoMMS> CheckMaterialNG_OK(string mt_cd);
        Task<int> InsertMaterialDown(MaterialDown item);
        Task<MaterialInfoMMS> CheckMaterialLotNG_OK(string mt_cd);
        Task<InventoryProduct> CheckMaterialLotNG_OK_InvenotryProduct(string mt_cd);
        #endregion

        #region Print QR Code
        Task<PrintQRMaterialResponse> PrintfQRCode(int id);

        Task <IEnumerable<LocationInfo>> GetNameFromLocationInfo(string lct_cd);
        #endregion

        #region QC
        Task<IEnumerable<FaclineQC>> GetListFacline_Qc(string item_vcd, string mt_cd);
        Task<IEnumerable<FaclineQCValue>> GetListFacline_qc_value(string fq_no);
        Task<IEnumerable<FaclineQC>> GetListDataFacline_Qc(string mt_cd);
        Task<IEnumerable<FaclineQC>> GetListDataFacline_Qc();
        Task<IEnumerable<MaterialMappingMMS>> GetMaterialMappingByCode(string mt_cd);
        Task<int> DeleteDataFacline_Qc(string fq_no);
        Task<int> UpdateDataMaterialInfoMMS(string mt_cd, int gr_qty, int real_qty);
        Task<int> UpdateDataMaterialInfoMMS(string mt_cd, int gr_qty);
        Task<int> UpdateDataActualInfo(int id_actual, int qty);
        Task<QCItemMaterial> GetDateQcItemMaterial(string item_vcd);
        Task<int> InsertIntoDataFaclineQC(FaclineQC item);
        Task<IEnumerable<MaterialInfoMMS>> GetMaterialInfoMMSByCode(string mt_cd);
        Task<int> InsertIntoDataMaterialInfoMMS(string Bien, int check_qty, int ok_qty, string reg_id, string chg_id, string mt_cd);

        Task<int> UpdateQtyForMaterialInfoMMS1(string mt_cd, int check_qty, int ok_qty);

        Task<int> UpdateQtyForMaterialInfoMMS2(string mt_cd, int check_qty, int ok_qty);

        Task<int> UpdateDecfectQtyMaterialInfoMMS(int id_actual, float defectQty);

        Task<int> GetSumQtyDProUnitMachineByIdActual(int id_actual);

        Task<int> GetDefectQtyByIdActual(int id_actual);

        Task<int> GetRealQtyMaterialInfoMMSByIdActual(int id_actual);

        Task<int> UpdateQtyActual(int id_actual, double total);

        Task<IEnumerable<QCItemCheckMaterial>> GetListDataQCItemCheckMaterial(string item_vcd);
        Task<IEnumerable<QCItemCheckMaterial>> GetAllDataQCItemCheckMaterial();

        Task<IEnumerable<QCItemCheckDetail>> GetAllDataQCItemCheckDetail();
        #endregion

        #region Mold
        Task<int> CountMoldInfo(string md_no, string md_nm);
        Task<IEnumerable<MoldInfo>> GetListDataMoldInfo(string md_no, string md_nm);
        Task<IEnumerable<DProUnitMold>> GetListMoldInfoDataByIdActual(int id_actual);
        Task<DProUnitMold> CheckMoldInfoData(string md_no,int id_actual);
        #endregion

        Task<int> GetWMTIDWMaterialInfoByMaterialCodeMMS(string mt_cd);
        Task<int> GetWMTIDWMaterialInfoByMaterialCodeTIMS(string mt_cd);
        Task<IEnumerable<WActualBom>> GetListDataBomMaterial(string style_no, string at_no, string process_code);
        Task<InventoryProduct> GetMaterialInventoryProductforBobbin(string mt_cd, string mt_type);
        Task<IEnumerable<WActualBom>> GetListOfSubstituteMaterials(string ProductCode, string at_no, string mt_no, string process_code);
        Task<int> GetIdMachineData(string mc_no, string startDate, string endDate);
        Task<Actual> GetActualDataByMaterialCode(string mt_cd);
        Task<BobbinInfo> GetBobbinInfoChangebbdv(string bb_no);
        Task<string> GetBobbinHistoryData(string bb_no);
        Task<int> Checkwmfaclineqc(string mt_cd);
        Task<int> Gettwproductqc(string mt_cd);
        Task<int> CountMoldInfoData(string md_no);
        Task<int> InsertIntoMoldInfoData(MoldInfo item);
        Task<MoldInfo> GetMoldInfoDataById(int Id);
        Task<MoldInfo> ModifyMoldInfo(int mdno, string md_no, string md_nm, string purpose, string re_mark);
        Task<IEnumerable<BobbinInfo>> GetListBobbinInfoData(string mt_cd);
        Task<int> CheckExistMaterialMapping(string mt_lot, string mt_cd);
        Task<int> InsertIntoMoldInfo(DProUnitMold item);
        Task<DProUnitMold> GetDMoldUnitDataById(int mdid);
        Task<string> GetStatusMold(string md_no, int id_actual, string start, string end);
        Task<int> GetIdMoldData(string md_no, string startDate, string endDate);
        Task<int> UpdateMold(DProUnitMold item);
        Task<int> DeleteMold(int mdid);
        Task<int> DeleteMaterialMappingMMSByCode(string mt_cd);
        Task<int> CheckBobbinHistoryLocation(string bb_no, string mt_cd);

        Task<int> CheckExistedMaterialMappingMMS(string mt_lot, string mt_cd);
        Task<int> CalQuantityForActual(int id_actual);
        Task<int> UpdateActualForWActual(int qty, int id_actual);
        Task<WMaterialInfoNew> GetWMaterialInfo(int id);
        Task<int> UpdateWMaterialInfoDescription(int wmtid, string description);
        Task<IReadOnlyList<ProductProcess>> GetProductProcesses(string product);
        Task<ActualPrimaryModify> GetPoInfo(int id_actualpr);
        Task<bool> CheckDeletePO(string at_no);
        Task<bool> DeleteMachine(string id_actual);
        Task<bool> DeleteStaff(string id_actual);
        Task<bool> DeleteProcess(string id_actual);

        //Phong Vũ
        Task<DateTime> GetTimeOfMaxContainerByStaffId(int psid);
        Task<DateTime> GetTimeOfMinContainerByStaffId(int psid);
        Task<DateTime> GetTimeOfMaxContainerByMachineId(int pmid);
        Task<DateTime> GetTimeOfMinContainerByMachineId(int pmid);
        Task<IEnumerable<WActualBom>> GetListmaterialbomdetail(string product, string at_no, string mt_no, string shift_dt, string shift_name);
        Task<IReadOnlyList<w_material_down>> GetDetailMaterialNG(string mt_cd);

    }
}
