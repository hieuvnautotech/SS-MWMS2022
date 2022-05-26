using Mvc_VD.Classes;
using Mvc_VD.Controllers;
using Mvc_VD.Models;
using Mvc_VD.Models.TIMS;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Mvc_VD.Services
{
    public interface ITIMSService
    {
        JsonResult InsertRDInfo(w_rd_info w_rd_info);
        w_rd_info UpdateRDInfo(w_rd_info w_rd_info);
        WRdInfo DeleteRDInfo(w_rd_info w_rd_info);
        IEnumerable<TimsReceivingScanModel> Get_List_Material_TimsReceiving_PO(string po_no, string product, string input_dt, string bb_no);
        JsonResult GetInventoryInfoGeneral(string mtCode, string sVBobbinCd, string prd_cd, string recDateStart, string recDateEnd, string mtNoSpecific, string status, string po);
        //bao add
        WMaterialInfoTmp FindOneMaterialInfoByMTLot(string mt_lot);
        MaterialInfoMMS FindOneMaterialInfoById(int wmmid);
        //get bobin
        DBobbinLctHist FindOneBobbin_lct_hist(string bb_no);
        DBobbinInfo FindOneDBobbinInfo(string bb_no);
        bool CheckExistMaterialMappingById(string mt_cd);
        int InsertBobbinHist(DBobbinLctHist bobbinhist);
        int InsertToBobbinHist(DBobbinLctHist bobbinhist);
        int DeleteBobbinHist(DBobbinLctHist bobbinhist); //xóa bobbin xả
        int UpdateBobbinInfo(string chg_id, string mt_cd, int count_number, int bno);
        int UpdateBobbinInfo(string chg_id, string mt_cd, int bno);

        int UpdateDefectBobbin();
        int InsertMaterialInfo(string mt_cd, int check_qty, int ok_qty, string reg_id, string mt_lot);
        //void InsertMaterialInfo(string orgin_mt_cd, string mt_cd, double? real_qty, string reg_id, string mt_lot);
        int UpdateWActualById(int? id_actual, double? defect);
        WActual FindOneWActual(int? id);
        WActualPrimary FindOneWActualPrimaryByAtNo(string at_no);
        int UpdateUseYnMaterialMapping(string useyn, int wmmid);
        int UpdateUseYnMaterialMapping(string useyn, string chg_id, int wmmid);
        int UpdateMaterialInfoQty(double? gr_qty, double? real_qty, int wmtid);
        IEnumerable<MFaclineQC> FindAllMFaclineQc(string mt_cd, string mt_lot);
        DProUnitStaffAPI FindOneDProUnitStaffById(int psid); // process
        IEnumerable<DProUnitStaffAPI> FindDProUnitStaffByStaffId(int psid, string staff_id); // process
        int UpdateDProUnitStaff(DProUnitStaffAPI item);
        DProUnitStaffAPI GetDProUnitStaff(int psid);
        bool CheckExistFacline(string mt_cd);
        WMaterialMapping FindOneWMaterialMappingById(int wmmid);
        int UpdateWMaterialInfoById(string mt_sts_cd, string chg_id, int wmtid);
        int UpdateWMaterialMappingById(string sts_share, int wmtid);
        DBobbinInfo FindOneDBobbinInfo(string bb_no, string mt_cd);
        void DeleteBobbinHist(int blno);
        // IEnumerable<WMaterialnfo> FindWMaterialnfo();
        IEnumerable<WMaterialInfoOQCAPI> GetListLotOQCpp(int id_actual_oqc, string staff_id_oqc);
        bool CheckStaffShift(int id_actual, string staff_id);
        int CheckStaffShiftForId(int? id_actual, int psid);
        IEnumerable<truyxuatlot> Truyxuatlistlot(string mt_cd, string tentam, string buyer_qr);
        IEnumerable<WActualPrimaryAPI> GetDatawActualPrimaryFromSP(string product, string at_no, string reg_dt, string tims);
        IEnumerable<WActualPrimary> GetDatawActualPrimary(string product, string at_no, string reg_dt, int start, int end);
        int GetTotalcountDatawActualPrimary(string product, string at_no, string reg_dt);
        IEnumerable<DProUnitStaffAPI> GetTIMSListStaffByActualId(int id_actual, string staff_id, string StartDate, string EndDate);
        IEnumerable<WMaterialnfo> FindAllWMaterialInfoByStaffIdOQC(string staff_id_oqc, int id_actual, string start_dt, string ent_dt);
        IEnumerable<WMaterialnfo> FindAllWMaterialInfoByStaffId(string staff_id, int id_actual, string start_dt, string end_dt);
        WMaterialnfo FindOneWMaterialInfoByMTQRCode(string mt_qrcode);
        WMaterialnfo FindWMaterialInfo(string mt_cd, string mt_sts_cd1, string mt_sts_cd2, string lct_cd, string lct_sts_cd, int id_actual);
        IEnumerable<WMaterialnfo> FindWMaterialInfo(string mt_cd);
        IEnumerable<DBobbinLctHist> FindAllBobbin_lct_histByBBNo(string bb_no);
        IEnumerable<OKReason> GetOKReason(string mt_lot, string PrimaryProduct, string don_vi_pr, string mt_cd, string product, string mt_no, string bb_no);
        int CheckShift(int psid);
        int CheckShift(int psid, int id_actual);
        int UpdateMaterialInfoQty(int defec_t, int check_qty, int ok_qty, string bien);
        int UpdateMaterialInfoQty(int qty, string bien);
        int UpdateMtCdBobbinHistInfo(string mt_cd, int blno);
        IEnumerable<WMaterialInfoTmp> GetListMaterial(string wmtid, string mt_cd, string mt_no);
        void UpdateMaterial(double? quantity, int wmtid);
        void UpdateMaterial(double? quantity, int wmtid, string mt_sts_cd);
        void UpdateBobbinInfo(string chg_id, string bb_no, string mt_cd);
        void DeleteBobbinHistory(string bb_no, string mt_cd);
        int CountMaterialInfo(string LikeCondition);
        void InsertMergeMaterial(WMaterialInfoTmp data);
        bool CheckExistMaterialMapping(string mt_cd, string mt_lot);
        int InsertMaterialMapping(w_material_mapping data);
        int UpdateMaterialInfoRemain(double? remain, string mt_sts_cd, int wmtid);
        int UpdateFaclineQty(int check_qty, int ok_qty, int fqno);
        IEnumerable<MFaclineQC> FindFaclineQcByFq_no(string fq_no);
        int InsertMFaclineQC(m_facline_qc mFaclineQC);
        IEnumerable<DBobbinLctHist> FindAllBobbin_lct_histByBBNoMTCd(string bb_no, string mt_cd);
        int InsertBobbinHist(d_bobbin_lct_hist bobbin_hist, string nameaaa);
        IEnumerable<MaterialInfoMMS> FindAllMaterialInfoReturn(string wmtid);
        IEnumerable<w_product_qc> FindAllWProductQcByMl_no(string mt_cd);
        int UpdateWActualById(double? defect, string chg_id, int? id_actual);
        IEnumerable<WMaterialMapping> FindAllWMaterialMappingByMtLotMtCd(string mt_cd, string mt_lot);
        IEnumerable<WMaterialMapping> FindAllWMaterialMappingByMtLot(string mt_cd);
        IEnumerable<WMaterialnfo> FindWMaterialInfoByMt_cdId_actual(string mt_cd, int id_actual);
        IEnumerable<w_actual> GetListWActualForProcess(string at_no);
        bool CheckWMaterialHasNG(string mt_cd);
        bool CheckFaclineQCHasNG(string mt_cd);
        bool CheckWProductQCHasNG(string mt_cd);
        d_bobbin_info GetOneDBobbinInfoWithMtCdIsNULL(string bb_no);
        MaterialInfoMMS CheckWmaterialForDiv(string bb_no);
        IEnumerable<MaterialInfoMMS> FindAllMaterialByMtCdLike(string bien_first);
        int InsertWMaterialInfoToBobin(MaterialInfoMMS a);
        IEnumerable<w_material_mapping> FindOneWMaterialMappingByIdActualAndStaffId(int id_actual, string staff_id);
        WMaterialnfo FindOneWMaterialInfo(int wmtid);
        WMaterialnfo FindOneWMaterialInfo(int idReturn, string mt_no, string bbmp_sts_cd);
        int SumGroupQtyWMaterialInfo(string mt_cd);
        IEnumerable<WMaterialMapping> FindAllWMaterialMappingByMTCdAndNotMtLot(string mt_cd, string mt_lot);
        void UpdateActualStaff(int? id_actual, int psid);
        int UpdateNGPO(int check_qty, int ok_qty, string bien);
        int UpdateContainerOutput(string mt_cd, int qty);
        void UpdateDefectActual(double? defect, int id_actual);
        WMaterialnfo FindOneWMaterialInfoLike(string mt_cd, string lct_cd, string lct_sts_cd);
        WMaterialnfo FindOneWMaterialInfoMMS007(int wmtid);
        int InsertWMaterialMapping(w_material_mapping data);
        int UpdateMaterialStaffQty(int? staff_qty, string mt_sts_cd, int wmtid);
        MaterialInfoMMS FindAllMeterialChangestPacking(int wmtid);
        int FindDProUnitStaffChangestsPacking(int? id_actual, int psid);
        int InsertWMaterialMappingWithNoMtLot(w_material_mapping data);
        IEnumerable<NGPO> GetNotGoodPO(string product, string mt_cd, string mt_no, string PO);
        IEnumerable<WActualPrimary> GetActualPrimary(string product, string product_name, string model, string at_no, string regstart, string regend);
        IEnumerable<WActualPrimaryAPI> GetDatawActualPrimarySP(string product, string product_name, string model, string at_no, string regstart, string regend);
        IEnumerable<WActualAPI> GetTIMSActualInfoBySP(string at_no);
        IEnumerable<DProUnitStaffAPI> GetTIMSListStaff(int id_actual, string staff_id, string StartDate, string EndDate);
        bool CheckExistUserByUserid(string userId);
        IEnumerable<WMaterialnfo> GetTIMSActualDetailByStaff(string staff_id, int id_actual, string start_dt, string end_dt);
        IEnumerable<w_material_mapping> FindAllMaterialMappingByMtCd(string mt_cd);
        IEnumerable<MaterialInfoMMS> FindWMaterialInfoByMTQRCode(string mt_qrcode);
        int UpdateWMaterialInfoById(string mt_sts_cd, string chg_id, int? id_actual, int wmtid);
        int UpdateWMaterialInfoInput(double? gr_qty, string mt_sts_cd, int wmtid);
        int UpdateFinishTIMS(int remain, string html, string mt_cd, string mt_lot);
        IEnumerable<WMaterialMapping> GetDatasMaterialMapping(string mt_cd);
        int UpdateReturnstPacking(string wmtid);
        int CheckMaterialMapping(string mt_cd, string mt_lot);
        w_actual_primary GetwactualprimaryFratno(string at_no);
        IEnumerable<m_facline_qc> Getmfaclineqc(string ml_no, string fq_no, string ml_tims);
        m_facline_qc GetmfaclineqcSearch(string fq_no);
        void UpdateMaterialmappinguseyn(string mt_lot, string mt_cd, string use_yn);
        void UpdateGrqtyMaterialInfo(string mt_cd, int gr_qty, string mt_sts_cd);
        void UpdateMaterialMappingUseyn(string mt_cd, string use_yn);
        int CheckGrqtyMaterialInfo(string mt_cd, string mt_sts_cd);
        void DeleteDBobbinLctHistforDevice(string mt_cd);
        void UpdateBobbinInfowithmtcd(string mt_cd);
        int GetDefactActual(int id_actual);
        int GetSumQtyFacline(string ml_tims);
        IEnumerable<TimsReceivingScanModel> GetListMaterialTimsReceivingPO(string po_no, string product, string input_dt, string bb_no);
        MaterialInfoMMS FindOneMaterialInfoByMTCdBBNo(string mt_cd, string bb_no);
        IEnumerable<TimsReceivingScanModel> FindAllTimsReceivingScanByInBBNo(string bb_no);
        int UpdateMTQR_RDList(string full_date, string user, string bb_no);
        bool CheckExistDuplicateTime(string staff_id, string start, string end, int psid);
        DProUnitStaffAPI FindOneDProUnitStaffReturnByPsid(int psid);
        int InsertDProUnitStaff(d_pro_unit_staff a);
        IEnumerable<DProUnitStaffAPI> FindDProUnitStaffByStaffIdIdActual(int? id_actual, string staff_id);
        int UpdateWMaterialInfoById(int? id_actual_oqc, string staff_id_oqc, string mt_sts_cd, string chg_id, int wmtid);
        int CallSPCHIATIMS(int gr_qty, int number_dv, string mt_cd, string bb_no);
        bool CheckWMaterialMapForDV(string mt_cd);
        int SumGrqtyDSWMaterialDV(string mt_cd);
        IEnumerable<WMaterialnfoDV> DSWMaterialDV(string mt_cd);

        int CheckWMaterialMappingForRedo(string mt_cd);
        void UpdateWMaterialQtyForRedo(double? gr_qty, string mt_cd);
        void DeleteWMaterialQtyForRedo(string mt_cd);
        void DeleteWMaterialMappingForRedo(string mt_cd);
        void DeleteBoBbinHisForRedo(string blno);
        void UpdateBobbinInfoForRedo(string blno);
        int CallSPkhoiphucbobin(string mt_cd, string bb_no);
        int ChangeBobinTimsDv(string bb_no, int wmtid, string bb_no2, string mt_cd);
        IEnumerable<w_material_mapping> FindOneWMaterialMappingByIdActualAndStaffIdOQC(int id_actual, string staff_id_oqc);
        int DeleteDProUnitStaff(int? id_actual, int psid, string staff_id);
        IEnumerable<WMaterialnfo> FindAllWMaterialInfoByStaffIdOQC(string staff_id_oqc, int id_actual_oqc);
        int UpdateDefectActual(double? defect, string chg_id, int id_actual);
        IEnumerable<WMaterialnfo> FindDetailActualStaffOQC(string staff_id_oqc, int id_actual, string end_dt, string start_dt);
        qc_item_mt Getqcitemmt(string item_vcd);

        int GetOneDBWithBuyer(string bb_no);
        string GetUname(string userid);
        IEnumerable<WMaterialMapping> FindAllWMaterialMappingByMtcdUseYn(string mt_cd, string use_yn);
        IEnumerable<WMaterialnfo> GetDataWActualSX(string at_no);
        IEnumerable<WActual> FindAllWActualByAtNo(string at_no);
        int InsertWActual(w_actual actual);
        IEnumerable<WMaterialnfo> GetListBoBinPopup(int id_actual, string bb_no, string bb_nm, string mt_cd);
        IEnumerable<WMaterialMapping> FindMaterialMappingByMtcdUseYn(string mt_lot, string use_yn);
        IEnumerable<DProUnitStaffAPI> GetTIMSListStaffOQC(int id_actual, string staff_id, string StartDate, string EndDate);
        w_actual GetProcessWactual(int id_actual);
        int GetDefectChangestspacking(string start_dt, string end_dt, string staff_id, int id_actual);
        int GetRealQTYChangestspacking(string start_dt, string end_dt, string staff_id, int id_actual);
        int GetGrQtyChangestspacking(string start_dt, string end_dt, string staff_id, int id_actual);
        void UpdateDprounitstaffChangestspacking(int sanluong, int defect_return, int id_actual, int psid);
        void UpdateactualDprounitstaffChangestspacking(double sanluong, int id_actual, int psid);
        void UpdatewactualChangestspacking(int id_actual, int SumActual);
        int GetSumactualChangestspacking(int id_actual);
        int GetsumActualmaterialinfoChangestspacking(int id_actual);
        IEnumerable<MaterialInfoMMS> FindAllMeterialChangestPackingweb(string wmtid);
        int InsertwmaterialinfoDevideSta(int sl_chia, string mt_cd_dv, string mt_cd);
        void UpdateMappingDevideSta(string bobbin, string mt_cd);
        void DeleteBobbinHistoryDevideSta(string mt_cd);
        IEnumerable<WMaterialInfoTmp> Getdbobbinlcthist(int id_actual, string at_no, string bb_nm, string bb_no);
        int UpdateWMaterialInfoByIdSingle(string mt_sts_cd, string chg_id, DateTime? end_production_dt, int wmtid);
        int UpdateWMaterialInfoByIdMultiple(string mt_sts_cd, string chg_id, DateTime? end_production_dt, string wmtid);
        //int FindDProUnitStaffChangestsPackingweb(int? id_actual, int psid);
        int GetQrcodeBuyer(string buyer_qr);
        d_bobbin_lct_hist GetdbobbinlcthistFrbbno(string bb_no);
        bool CheckQRBuyer(string bb_no);
        IEnumerable<MaterialInfoMMS> CheckwmaterialinfoMappingbuyer(string mt_cd, string mt_sts_cd, string lct_cd);
        string GetProductWactualPrimary(string mt_cd, string mt_sts_cd, string lct_cd);
        stamp_detail Getstampdetail(string buyer_qr);
        d_style_info GetStyleNo(string style_no);
        void Insertstampdetail(stamp_detail item);
        void Deletedbobbininfo(int bno);
        WMaterialInfoStamp ViewStatusTemGoi(string buyerCode);
        w_box_mapping CheckTemGoimappingBox(string buyerCode);
    }

    public class TIMSService : ITIMSService
    {
        private readonly Entities db;
        private readonly HttpSessionState Session;
        public TIMSService(IDbFactory dbFactory)
        {
            db = dbFactory.Init();
            Session = HttpContext.Current.Session;
        }
        public WRdInfo DeleteRDInfo(w_rd_info w_rd_info)
        {
            //  var KTTT = db.w_rd_info.Find(w_rd_info.rid);
            string sql = @"Select  
                             rid ,rd_no,rd_nm,rd_sts_cd,lct_cd,receiving_dt,remark,use_yn,
                             reg_id,reg_dt,chg_id,chg_dt
                             from w_rd_info;
                            from w_rd_info  where rid= @1 ";
            var KTTT = db.Database.SqlQuery<WRdInfo>(sql).FirstOrDefault();
            if (KTTT == null)
            {
                return null;
            }
            if (!(KTTT.rd_sts_cd.Equals("000")))
            {
                return KTTT;
            }
            //UPDATE sd_list
            //var sql1 = new StringBuilder();
            //sql1.Append(" UPDATE  w_material_info  ")
            //    .Append(" SET w_material_info.rd_no = '' , w_material_info.mt_sts_cd = '001', ")

            //    .Append("   w_material_info.lct_cd = '002000000000000000' , w_material_info.recevice_dt_tims = '' ,")
            //    .Append("   w_material_info.to_lct_cd = '002000000000000000' ")
            //    .Append(" WHERE w_material_info.rd_no ='" + KTTT.rd_no + "' ");
            string sqlUpdate = @"UPDATE  w_material_info SET w_material_info.rd_no = '' , w_material_info.mt_sts_cd = '001', w_material_info.lct_cd = '002000000000000000' , w_material_info.recevice_dt_tims = '' ,
                    w_material_info.to_lct_cd = '002000000000000000' WHERE w_material_info.rd_no = @1
                        ";
            try
            {
                db.Database.ExecuteSqlCommand(sqlUpdate, new MySqlParameter("1", KTTT.rd_no));
            }
            catch (Exception e)
            {
                return KTTT;
            }
            //using (var cmd = db.Database.Connection.CreateCommand())
            //{
            //    db.Database.Connection.Open();
            //    cmd.CommandText = sql1.ToString();
            //    var reader = cmd.ExecuteNonQuery();
            //    int result1 = Int32.Parse(reader.ToString());
            //    db.Database.Connection.Close();
            //}
            //db.Entry(KTTT).State = EntityState.Deleted;
            //db.SaveChanges();
            return KTTT;
        }
        public IEnumerable<TimsReceivingScanModel> Get_List_Material_TimsReceiving_PO(string po_no, string product, string input_dt, string bb_no)
        {
            string sql = @"SELECT * FROM view_tims_receiving_scan where (@po_no='' OR at_no LIKE '%'@po_no'%') and (@bb_no='' OR bb_no LIKE '%'@bb_no'%') and (@product='' OR product LIKE '%'@product'%') and (@input_dt='' OR DATE_FORMAT(@input_dt, '%Y-%m-%d') like '%'@input_dt'%');";

            IEnumerable<TimsReceivingScanModel> data = db.Database.SqlQuery<TimsReceivingScanModel>(sql, new MySqlParameter("@po_no", po_no ?? ""), new MySqlParameter("@bb_no", bb_no ?? ""), new MySqlParameter("@product", product ?? ""), new MySqlParameter("@input_dt", input_dt ?? ""));
            return data;
        }
        public JsonResult InsertRDInfo(w_rd_info w_rd_info)
        {
            #region Tang tự động

            String rd_no = "RD1";

            var rd_no_last = db.w_rd_info.ToList().LastOrDefault();
            if (rd_no_last != null)
            {
                var rd_noCode = rd_no_last.rd_no;
                rd_no = string.Concat("RD", (int.Parse(rd_noCode.Substring(2)) + 1).ToString());
            }
            #endregion Tang tự động
            w_rd_info.rd_no = rd_no;

            DateTime dt = DateTime.Now;
            string day_now = dt.ToString("yyyyMMdd");
            w_rd_info.receiving_dt = day_now;

            w_rd_info.rd_sts_cd = "000";
            w_rd_info.reg_id = HttpContext.Current.Session["authName"] == null ? null : HttpContext.Current.Session["authName"].ToString();
            w_rd_info.chg_id = HttpContext.Current.Session["authName"] == null ? null : HttpContext.Current.Session["authName"].ToString();
            w_rd_info.use_yn = "Y";

            w_rd_info.reg_dt = DateTime.Now;
            w_rd_info.chg_dt = DateTime.Now;

            db.w_rd_info.Add(w_rd_info);
            db.SaveChanges();

            var sql = new StringBuilder();
            sql.Append(" SELECT a.* ")

               .Append(" FROM w_rd_info as a ")
                .Append(" WHERE a.rd_no='" + rd_no + "'  ")

                .Append(" order by rid desc ");

            var ddd = new InitMethods().ConvertDataTableToJsonAndReturn(sql);
            return ddd;
        }
        public w_rd_info UpdateRDInfo(w_rd_info w_rd_info)
        {
            w_rd_info KTTT = db.w_rd_info.Find(w_rd_info.rid);
            if (KTTT == null)
            {
                return null;
            }
            //KTTT.rd_nm = w_rd_info.rd_nm;
            //KTTT.remark = w_rd_info.remark;

            //KTTT.chg_id = Session["authName"]?.ToString();
            //db.Entry(KTTT).State = EntityState.Modified;
            //db.SaveChanges();
            string sqlUpdate = @"Update w_rd_info Set rd_nm = @1 ,remark=@2 ,chg_id= @3 where rid=@4   ";
            db.Database.ExecuteSqlCommand(sqlUpdate);
            return KTTT;

        }

        public JsonResult GetInventoryInfoGeneral(string mtCode, string sVBobbinCd, string prd_cd, string recDateStart, string recDateEnd, string mtNoSpecific, string status, string po)
        {
            StringBuilder varname1 = new StringBuilder("SET sql_mode = '';SET @@sql_mode = '';");
            var sts_search = "";
            var bien = "";
            int dem = 0;
            switch (status)
            {
                case "008":
                    sts_search = " HCK >0";
                    dem = 1;
                    break;
                case "002":
                    sts_search = " DKT >0";
                    dem = 1;
                    break;
                case "010":
                    sts_search = " HDG >0";
                    dem = 1;
                    break;
                case "002,HCOQC":
                    sts_search = " CKT >0";
                    dem = 1;
                    break;
            }
            if (dem == 1)
            {
                varname1.Append("SELECT * from ( \n");
            }
            varname1.Append("SELECT a.product product_cd,c.style_nm product_nm,c.md_cd,a.at_no, \n");

            if (dem == 1 && status != "008") { varname1.Append("(0)HCK, \n"); }
            else
            {
                varname1.Append("SUM(CASE WHEN a.mt_sts_cd='008' \n");
                varname1.Append("			AND	 ('" + mtCode + "'='' OR a.mt_cd LIKE CONCAT('%','" + mtCode + "','%')) \n");
                varname1.Append("			AND ('" + sVBobbinCd + "'='' OR a.bb_no LIKE CONCAT('%','" + sVBobbinCd + "','%')) \n");
                varname1.Append("			AND ('" + po + "'='' OR a.at_no LIKE CONCAT('%','" + po + "','%')) \n");
                varname1.Append("			AND ('" + prd_cd + "'='' OR a.product LIKE CONCAT('%','" + prd_cd + "','%')) \n");
                varname1.Append("			AND ('" + recDateStart + "'='' OR DATE_FORMAT(a.end_production_dt,'%Y/%m/%d') >= DATE_FORMAT('" + recDateStart + "','%Y/%m/%d')) \n");
                varname1.Append("			AND ('" + recDateEnd + "'='' OR DATE_FORMAT(a.end_production_dt,'%Y/%m/%d') <= DATE_FORMAT('" + recDateEnd + "','%Y/%m/%d')) \n");
                varname1.Append(" THEN a.gr_qty  ELSE 0  END )AS 'HCK', \n");
            }
            if (dem == 1 && status != "002") { varname1.Append("(0)DKT, \n"); }
            else
            {
                varname1.Append("SUM(CASE WHEN a.mt_sts_cd='002' \n");
                varname1.Append("			AND	 ('" + mtCode + "'='' OR a.mt_cd LIKE CONCAT('%','" + mtCode + "','%')) \n");
                varname1.Append("			AND ('" + sVBobbinCd + "'='' OR a.bb_no LIKE CONCAT('%','" + sVBobbinCd + "','%')) \n");
                varname1.Append("			AND ('" + po + "'='' OR a.at_no LIKE CONCAT('%','" + po + "','%')) \n");
                varname1.Append("			AND ('" + prd_cd + "'='' OR a.product LIKE CONCAT('%','" + prd_cd + "','%')) \n");
                varname1.Append("			AND ('" + recDateStart + "'='' OR DATE_FORMAT(a.end_production_dt,'%Y/%m/%d') >= DATE_FORMAT('" + recDateStart + "','%Y/%m/%d')) \n");
                varname1.Append("			AND ('" + recDateEnd + "'='' OR DATE_FORMAT(a.end_production_dt,'%Y/%m/%d') <= DATE_FORMAT('" + recDateEnd + "','%Y/%m/%d')) \n");


                varname1.Append("AND a.id_actual !=( \n");
                varname1.Append("SELECT MAX(v.id_actual) id_actual FROM w_actual AS v \n");
                varname1.Append("WHERE v.at_no=a.at_no AND v.`type`='TIMS' AND v.name!='OQC' \n");
                varname1.Append(") \n");
                varname1.Append("THEN a.gr_qty  ELSE 0  END) AS 'DKT', \n");
            }
            if (dem == 1 && status != "010") { varname1.Append("(0)HDG, \n"); }
            else
            {
                varname1.Append("sum(CASE WHEN a.mt_sts_cd='010' \n");
                varname1.Append("			AND	 ('" + mtCode + "'='' OR a.mt_cd LIKE CONCAT('%','" + mtCode + "','%')) \n");
                varname1.Append("			AND ('" + sVBobbinCd + "'='' OR a.bb_no LIKE CONCAT('%','" + sVBobbinCd + "','%')) \n");
                varname1.Append("			AND ('" + po + "'='' OR a.at_no LIKE CONCAT('%','" + po + "','%')) \n");
                varname1.Append("			AND ('" + prd_cd + "'='' OR a.product LIKE CONCAT('%','" + prd_cd + "','%')) \n");
                varname1.Append("			AND ('" + recDateStart + "'='' OR DATE_FORMAT(a.end_production_dt,'%Y/%m/%d') >= DATE_FORMAT('" + recDateStart + "','%Y/%m/%d')) \n");
                varname1.Append("			AND ('" + recDateEnd + "'='' OR DATE_FORMAT(a.end_production_dt,'%Y/%m/%d') <= DATE_FORMAT('" + recDateEnd + "','%Y/%m/%d')) \n");
                varname1.Append("THEN a.gr_qty  ELSE 0  END) AS 'HDG', \n");
            }
            if (dem == 1 && status != "002,HCOQC") { varname1.Append("(0)CKT \n"); }
            else
            {
                varname1.Append("sum(CASE WHEN a.mt_sts_cd='002' \n");
                varname1.Append("			AND	 ('" + mtCode + "'='' OR a.mt_cd LIKE CONCAT('%','" + mtCode + "','%')) \n");
                varname1.Append("			AND ('" + sVBobbinCd + "'='' OR a.bb_no LIKE CONCAT('%','" + sVBobbinCd + "','%')) \n");
                varname1.Append("			AND ('" + po + "'='' OR a.at_no LIKE CONCAT('%','" + po + "','%')) \n");
                varname1.Append("			AND ('" + prd_cd + "'='' OR a.product LIKE CONCAT('%','" + prd_cd + "','%')) \n");
                varname1.Append("			AND ('" + recDateStart + "'='' OR DATE_FORMAT(a.end_production_dt,'%Y/%m/%d') >= DATE_FORMAT('" + recDateStart + "','%Y/%m/%d')) \n");
                varname1.Append("			AND ('" + recDateEnd + "'='' OR DATE_FORMAT(a.end_production_dt,'%Y/%m/%d') <= DATE_FORMAT('" + recDateEnd + "','%Y/%m/%d')) \n");
                varname1.Append("AND a.id_actual =( \n");
                varname1.Append("SELECT MAX(v.id_actual) id_actual FROM w_actual AS v \n");
                varname1.Append("WHERE v.at_no=a.at_no AND v.`type`='TIMS' AND v.name!='OQC' \n");
                varname1.Append(") \n");
                varname1.Append("THEN a.gr_qty  ELSE 0  END) AS 'CKT' \n");
            }
            varname1.Append("FROM   `w_material_info` `a` \n");
            varname1.Append("          LEFT JOIN `d_material_info` `b` ON  `b`.`mt_no` = `a`.`mt_no` \n");
            varname1.Append("			JOIN d_style_info AS c ON a.product=c.style_no \n");
            varname1.Append("WHERE  `a`.`lct_cd` = '006000000000000000' \n");
            varname1.Append("         AND ( `a`.`mt_sts_cd`!= '000'  and `a`.`mt_sts_cd`!= '003' and `a`.`mt_sts_cd`!= '005' ) \n");
            varname1.Append("			AND	 ('" + mtCode + "'='' OR a.mt_cd LIKE CONCAT('%','" + mtCode + "','%')) \n");
            varname1.Append("			AND ('" + sVBobbinCd + "'='' OR a.bb_no LIKE CONCAT('%','" + sVBobbinCd + "','%')) \n");
            varname1.Append("			AND ('" + po + "'='' OR a.at_no LIKE CONCAT('%','" + po + "','%')) \n");
            varname1.Append("			AND ('" + prd_cd + "'='' OR a.product LIKE CONCAT('%','" + prd_cd + "','%')) \n");
            varname1.Append("			AND ('" + recDateStart + "'='' OR DATE_FORMAT(a.end_production_dt,'%Y/%m/%d') >= DATE_FORMAT('" + recDateStart + "','%Y/%m/%d')) \n");
            varname1.Append("			AND ('" + recDateEnd + "'='' OR DATE_FORMAT(a.end_production_dt,'%Y/%m/%d') <= DATE_FORMAT('" + recDateEnd + "','%Y/%m/%d')) \n");
            varname1.Append("          \n");
            varname1.Append("         GROUP BY a.product ");
            if (dem == 1)
            {
                varname1.Append(") table_view where " + sts_search + " \n");
            }
            var listInfo = new InitMethods().ConvertDataTableToJsonAndReturn(varname1);
            return listInfo;
        }
        //bao add
        public WMaterialInfoTmp FindOneMaterialInfoByMTLot(string mt_lot)
        {
            string sql = @"SELECT wmtid,id_actual,mt_no,bb_no,gr_qty,mt_cd FROM w_material_info WHERE mt_cd= @1";
            return db.Database.SqlQuery<WMaterialInfoTmp>(sql, new MySqlParameter("1", mt_lot)).SingleOrDefault();
        }
        public bool CheckExistMaterialMappingById(string ml_tims)
        {
            // if return 1 => co ton tai , 0 la ko ton tai
            string sql = @"SELECT  
                           EXISTS
                           (SELECT * FROM w_material_mapping WHERE mt_lot = @1 )";
            int count = db.Database.SqlQuery<int>(sql, new MySqlParameter("1", ml_tims)).SingleOrDefault();
            if (count > 1)
            {
                return true;
            }
            return false;
        }
        public DBobbinLctHist FindOneBobbin_lct_hist(string bb_no)
        {
            string sql = @"select blno, mc_type,bb_no,bb_nm,mt_cd,start_dt,end_dt,use_yn,del_yn,reg_id,reg_dt,chg_id,chg_dt 
                            from d_bobbin_lct_hist
                            where bb_no = @1 ";
            return db.Database.SqlQuery<DBobbinLctHist>(sql, new MySqlParameter("1", bb_no)).SingleOrDefault();
        }
        public DBobbinInfo FindOneDBobbinInfo(string bb_no)
        {
            string sql = @"select bno,mc_type,bb_no,mt_cd,bb_nm,purpose,barcode,re_mark,
                           use_yn,del_yn,reg_id,reg_dt,chg_id,chg_dt 
                           from d_bobbin_info where bb_no = @1 ";
            return db.Database.SqlQuery<DBobbinInfo>(sql, new MySqlParameter("1", bb_no)).SingleOrDefault();
        }
        public DBobbinInfo FindOneDBobbinInfo(string bb_no, string mt_cd)
        {
            string sql = @"select bno,mc_type,bb_no,mt_cd,bb_nm,purpose,barcode,re_mark,count_number,
                           use_yn,del_yn,reg_id,reg_dt,chg_id,chg_dt 
                           from d_bobbin_info where bb_no = @1 AND mt_cd = @2";
            return db.Database.SqlQuery<DBobbinInfo>(sql, new MySqlParameter("1", bb_no), new MySqlParameter("2", mt_cd)).SingleOrDefault();
        }

        //insert bobbin xa tuc la bobbin hist
        public int InsertBobbinHist(DBobbinLctHist bobbinhist)
        {
            string QuerySQL = @"insert into 
                                d_bobbin_lct_hist(bb_no,mt_cd,reg_dt,chg_dt,chg_id,reg_id,use_yn,del_yn) 
                                values(@1,@2,@3,@4,@5,@6,@7,@8);SELECT LAST_INSERT_ID() ";
            return db.Database.SqlQuery<int>(QuerySQL,
                  new MySqlParameter("1", bobbinhist.bb_no),
                  new MySqlParameter("2", bobbinhist.mt_cd),
                  new MySqlParameter("3", bobbinhist.reg_dt),
                  new MySqlParameter("4", bobbinhist.chg_dt),
                  new MySqlParameter("5", bobbinhist.chg_id),
                  new MySqlParameter("6", bobbinhist.reg_id),
                  new MySqlParameter("7", bobbinhist.use_yn),
                  new MySqlParameter("8", bobbinhist.del_yn)).FirstOrDefault();
        }

        public int UpdateBobbinInfo(string chg_id, string mt_cd, int count_number, int bno)
        {
            string QuerySQL = @"Update d_bobbin_info set 
                                    chg_id = @1,
                                    mt_cd = @2,
                                    count_number =@3 where bno = @4
                                ";
            return db.Database.ExecuteSqlCommand(QuerySQL,
                  new MySqlParameter("1", chg_id),
                  new MySqlParameter("2", mt_cd),
                  new MySqlParameter("3", count_number),
                  new MySqlParameter("4", bno));

        }
        public int UpdateBobbinInfo(string chg_id, string mt_cd, int bno)
        {
            string QuerySQL = @"Update d_bobbin_info set 
                                    chg_id = @1,
                                    mt_cd = @2
                                    where bno = @3";
            return db.Database.ExecuteSqlCommand(QuerySQL,
                  new MySqlParameter("1", chg_id),
                  new MySqlParameter("2", mt_cd),
                  new MySqlParameter("3", bno));

        }
        public int DeleteBobbinHist(DBobbinLctHist bobbinhist)
        {
            throw new Exception();
        }
        public int UpdateDefectBobbin()
        {
            throw new Exception();
        }
        public int InsertMaterialInfo(string bien, int check_qty, int ok_qty, string reg_id, string mt_lot)
        {
            string QuerySQL = @"INSERT INTO w_material_info(at_no,product,id_actual,mt_type,mt_cd,
                                            alert_NG,mt_no,gr_qty,date,expiry_dt,dt_of_receipt,expore_dt,lot_no,
                                            mt_barcode,mt_qrcode,mt_sts_cd,use_yn,reg_id,reg_dt,chg_id,chg_dt, orgin_mt_cd,lct_cd,real_qty)
                                            SELECT at_no,product,0,mt_type,@1,
                                            1,mt_no,@2,date,expiry_dt,dt_of_receipt,expore_dt,lot_no,
                                            @3,@4,'003',use_yn,@5,@6,null,@7,mt_cd,lct_cd,@8
                                            FROM   w_material_info
                                            where mt_cd = @9 ; Select last_insert_id(); ";
            return db.Database.SqlQuery<int>(QuerySQL,
                  new MySqlParameter("1", bien),
                  new MySqlParameter("2", check_qty - ok_qty),
                  new MySqlParameter("3", bien),
                  new MySqlParameter("4", bien),
                  new MySqlParameter("5", reg_id),
                  new MySqlParameter("6", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                  new MySqlParameter("7", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                  new MySqlParameter("8", check_qty - ok_qty),
                  new MySqlParameter("9", mt_lot)).FirstOrDefault();

            // DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
        }
        public int UpdateWActualById(int? id_actual, double? defect)
        {
            //  // update_defect.defect = (update_defect.defect - defec_t) + (check_qty - ok_qty);
            string QuerySQL = "Update w_actual set defect = @1 where id_actual = @2";
            return db.Database.ExecuteSqlCommand(QuerySQL,
                new MySqlParameter("1", defect),
                new MySqlParameter("2", id_actual)
                );
        }
        public WActual FindOneWActual(int? id_actual)
        {
            string sql = @"select id_actual ,at_no,type,product,
                         actual,defect,name,level,date,don_vi_pr,item_vcd,reg_id,reg_dt,chg_id,chg_dt
                         from w_actual where id_actual = @1 limit 1";
            return db.Database.SqlQuery<WActual>(sql, new MySqlParameter("1", id_actual)).SingleOrDefault();
        }
        public int UpdateUseYnMaterialMapping(string useyn, int wmmid)
        {
            string QuerySQL = "Update w_material_mapping set use_yn = @1 where wmmid = @2";
            return db.Database.ExecuteSqlCommand(QuerySQL,
                new MySqlParameter("1", useyn),
                new MySqlParameter("2", wmmid)
                );
        }
        public int UpdateMaterialInfoQty(double? gr_qty, double? real_qty, int wmtid)
        {
            string QuerySQL = "Update w_material_info set gr_qty = @1 ,real_qty=@2  where wmtid = @3";
            return db.Database.ExecuteSqlCommand(QuerySQL,
                new MySqlParameter("1", gr_qty),
                new MySqlParameter("2", real_qty),
                new MySqlParameter("3", wmtid));
        }
        public WActualPrimary FindOneWActualPrimaryByAtNo(string at_no)
        {
            string sql = @"select id_actualpr,at_no,type,target,product,remark,finish_yn,reg_id,reg_dt,chg_id,chg_dt
                         from w_actual_primary  where at_no = @1 limit 1 ";
            return db.Database.SqlQuery<WActualPrimary>(sql, new MySqlParameter("1", at_no)).SingleOrDefault();
        }
        public IEnumerable<MFaclineQC> FindAllMFaclineQc(string mt_cd, string mt_lot)
        {
            string sql = @" select  fqno,fq_no,ml_no,ml_tims,product_cd,shift,at_no,
                            work_dt,item_vcd,item_nm,item_exp,check_qty,ok_qty,reg_id,reg_dt,chg_id,chg_dt
                            from m_facline_qc 
                            where ml_no = @1 
                            AND fq_no like '%TI%' 
                            AND ml_tims = @2 ";
            return db.Database.SqlQuery<MFaclineQC>(sql, new MySqlParameter("1", mt_cd),
                                                      new MySqlParameter("2", mt_lot));

        }
        public DProUnitStaffAPI FindOneDProUnitStaffById(int psid)
        {
            string sql = @" SELECT 
                            psid as psid,staff_id as staff_id,actual as actual,defect as defect,id_actual as id_actual,staff_tp as staff_tp,start_dt as start_dt,end_dt as end_dt,
                            use_yn as use_yn,del_yn as del_yn ,reg_id as reg_id,reg_dt as reg_dt,chg_id as chg_id,chg_dt as chg_dt
                            FROM d_pro_unit_staff
                            Where psid = @1 ;";
            return db.Database.SqlQuery<DProUnitStaffAPI>(sql, new MySqlParameter("1", psid)).SingleOrDefault();

        }
        public IEnumerable<DProUnitStaffAPI> FindDProUnitStaffByStaffId(int psid, string staff_id)
        {
            string sql = @" SELECT 
                            psid,staff_id,actual,defect,id_actual,staff_tp,start_dt,end_dt,
                            use_yn,del_yn,reg_id,reg_dt,chg_id,chg_dt
                            FROM d_pro_unit_staff
                            Where psid <> @1 AND staff_id= @2 ;";
            return db.Database.SqlQuery<DProUnitStaffAPI>(sql, new MySqlParameter("1", psid), new MySqlParameter("2", staff_id));
        }
        public int UpdateDProUnitStaff(DProUnitStaffAPI item)
        {
            string QuerySQL = @"Update d_pro_unit_staff SET start_dt=@1, end_dt=@2, staff_id=@3,use_yn=@4,del_yn=@5 ,
                     id_actual=@7     
where  psid = @6  ";
            return db.Database.ExecuteSqlCommand(QuerySQL,
                new MySqlParameter("1", item.start_dt),
                new MySqlParameter("2", item.end_dt),
                new MySqlParameter("3", item.staff_id),
                new MySqlParameter("4", item.use_yn),
                new MySqlParameter("5", item.del_yn),
                new MySqlParameter("6", item.psid),
                new MySqlParameter("7", item.id_actual)
                );
        }
        public DProUnitStaffAPI GetDProUnitStaff(int psid)
        {
            string getvalue = @"SELECT a.psid,a.staff_id,b.dt_nm AS staff_tp_nm,c.uname, DATE_FORMAT(CAST(a.start_dt AS datetime),'%Y-%m-%d %H:%i:%s')  AS  start_dt,DATE_FORMAT(CAST(a.end_dt AS datetime),'%Y-%m-%d %H:%i:%s') AS end_dt,a.use_yn
                FROM d_pro_unit_staff a
                LEFT JOIN comm_dt b ON  b.dt_cd=a.staff_tp AND b.mt_cd='COM013' AND b.use_yn='Y'
                LEFT JOIN mb_info c ON c.userid=a.staff_id
                where a.psid= @1";
            return db.Database.SqlQuery<DProUnitStaffAPI>(getvalue, new MySqlParameter("1", psid)).FirstOrDefault();
        }
        public bool CheckExistFacline(string mt_cd)
        {
            // if return 1 => co ton tai , 0 la ko ton tai
            string sql = @"SELECT  
                           EXISTS
                           (SELECT ml_tims FROM m_facline_qc WHERE ml_tims = @1 )";
            int count = db.Database.SqlQuery<int>(sql, new MySqlParameter("1", mt_cd)).SingleOrDefault();
            if (count == 1)
            {
                return true;
            }
            return false;
        }
        public WMaterialMapping FindOneWMaterialMappingById(int wmmid)
        {
            string getvalue = @"select 
                                        wmmid  ,
                                        mt_lot ,
                                        mt_cd  ,
                                        mt_no  ,
                                        mapping_dt,
                                        bb_no     ,
                                        remark   , 
                                        sts_share ,
                                        use_yn    ,
                                        del_yn    ,
                                        reg_id   , 
                                         reg_dt,
                                         chg_id,
                                         chg_dt
                                    from w_material_mapping where wmmid = @1
                                    ";
            return db.Database.SqlQuery<WMaterialMapping>(getvalue, new MySqlParameter("1", wmmid)).FirstOrDefault();
        }
        public MaterialInfoMMS FindOneMaterialInfoById(int wmmid)
        {
            string sql = @"select * from  w_material_info where wmtid = @1 limit 1";
            return db.Database.SqlQuery<MaterialInfoMMS>(sql, new MySqlParameter("1", wmmid)).SingleOrDefault();
        }
        public int UpdateUseYnMaterialMapping(string useyn, string chg_id, int wmmid)
        {
            string QuerySQL = @"Update w_material_mapping set use_yn = @1 , chg_id = @2
                                                    where wmmid = @3";
            return db.Database.ExecuteSqlCommand(QuerySQL,
                new MySqlParameter("1", useyn),
                new MySqlParameter("2", chg_id),
                new MySqlParameter("3", wmmid)
                );
        }
        public int UpdateWMaterialInfoById(string mt_sts_cd, string chg_id, int wmtid)
        {
            string sql = @"Update w_material_info
                                    set mt_sts_cd = @1,chg_id =@2
                                    where wmtid = @3 ";
            return db.Database.ExecuteSqlCommand(sql, new MySqlParameter("@1", mt_sts_cd),
                                                new MySqlParameter("2", chg_id),
                                                new MySqlParameter("3", wmtid));

        }
        public void DeleteBobbinHist(int blno)
        {
            string sql = @"Delete from d_bobbin_lct_hist
                                    where blno = @1 ";
            db.Database.ExecuteSqlCommand(sql, new MySqlParameter("1", blno));

        }
        public int UpdateWMaterialMappingById(string sts_share, int wmmid)
        {
            string sql = @"Update w_material_mapping
                                    set sts_share = @1
                                    where wmmid = @2 ";
            return db.Database.ExecuteSqlCommand(sql, new MySqlParameter("1", sts_share),
                                                new MySqlParameter("2", wmmid));
        }
        public int InsertToBobbinHist(DBobbinLctHist bobbinhist)
        {
            string sqlquery = @"INSERT INTO d_bobbin_lct_hist(mc_type,bb_no,mt_cd,bb_nm,start_dt,end_dt,use_yn,del_yn,reg_id,reg_dt,chg_id,chg_dt)
                VALUES(@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12);
                SELECT LAST_INSERT_ID();";
            return db.Database.SqlQuery<int>(sqlquery, new MySqlParameter("1", bobbinhist.mc_type),
                                                        new MySqlParameter("2", bobbinhist.bb_no),
                                                        new MySqlParameter("3", bobbinhist.mt_cd),
                                                        new MySqlParameter("4", bobbinhist.bb_nm),
                                                        new MySqlParameter("5", bobbinhist.start_dt),
                                                        new MySqlParameter("6", bobbinhist.end_dt),
                                                        new MySqlParameter("7", bobbinhist.use_yn),
                                                        new MySqlParameter("8", bobbinhist.del_yn),
                                                        new MySqlParameter("9", bobbinhist.reg_id),
                                                        new MySqlParameter("10", bobbinhist.reg_dt),
                                                        new MySqlParameter("11", bobbinhist.chg_id),
                                                        new MySqlParameter("12", bobbinhist.chg_dt)).FirstOrDefault();
        }
        public IEnumerable<WMaterialInfoOQCAPI> GetListLotOQCpp(int id_actual_oqc, string staff_id_oqc)
        {
            string viewSql = @" SELECT  wmtid,bb_no,mt_no,mt_cd,gr_qty,'0' as count_ng
               FROM w_material_info 
	                   WHERE id_actual_oqc = @1 AND mt_sts_cd = '009' AND staff_id_oqc = @2";
            return db.Database.SqlQuery<WMaterialInfoOQCAPI>(viewSql,
            new MySqlParameter("1", id_actual_oqc),
            new MySqlParameter("2", staff_id_oqc));
        }
        public bool CheckStaffShift(int id_actual, string staff_id)
        {
            string QuerySQL = "SELECT a.psid FROM d_pro_unit_staff AS a WHERE a.id_actual=@1 AND  a.staff_id= @2 AND (NOW() BETWEEN Date_format(a.start_dt,'%Y-%m-%d %H:%i:%s') AND Date_format(a.end_dt,'%Y-%m-%d %H:%i:%s'))";
            return String.IsNullOrWhiteSpace(db.Database.SqlQuery<string>(QuerySQL,
                new MySqlParameter("1", id_actual),
                new MySqlParameter("2", staff_id)
                ).FirstOrDefault());
        }
        public IEnumerable<truyxuatlot> Truyxuatlistlot(string mt_cd, string tentam, string buyer_qr)
        {
            string sqlquery = @"Call spTesting2(@1,@2,@3);";
            return db.Database.SqlQuery<truyxuatlot>(sqlquery, new MySqlParameter("1", mt_cd),
                                                               new MySqlParameter("2", tentam),
                                                               new MySqlParameter("3", buyer_qr)
                                                              );
        }
        public IEnumerable<WActualPrimaryAPI> GetDatawActualPrimaryFromSP(string product, string at_no, string reg_dt, string tims)
        {
            string sqlquery = @"Call spfgwms_namtesting(@1,@2,@3,@4);";
            return db.Database.SqlQuery<WActualPrimaryAPI>(sqlquery, new MySqlParameter("1", product), new MySqlParameter("2", at_no), new MySqlParameter("3", reg_dt), new MySqlParameter("4", tims));
        }
        public IEnumerable<WActualPrimary> GetDatawActualPrimary(string product, string at_no, string reg_dt, int start, int end)
        {
            string sqlquery = @"SELECT * 
                FROM(
                    
                    SELECT ROW_NUMBER() OVER(ORDER BY a.id_actualpr DESC) AS RowNum,
                        a.id_actualpr, a.at_no, a.type,( 0 ) totalTarget,
                        a.target,
                        a.product,
                        a.remark,
                        0 process_count,
                        0 actual,
                        subquery.at_no AS poRun,
                        b.md_cd AS md_cd,
                        b.style_nm AS style_nm 
                        FROM
	                        w_actual_primary AS a
	                        LEFT JOIN d_style_info b ON a.product = b.style_no
	                        LEFT JOIN (
	                        SELECT
		                        b.at_no 
	                        FROM
		                        d_pro_unit_staff AS a
		                        JOIN w_actual AS b ON a.id_actual = b.id_actual
		                        JOIN w_actual_primary AS c ON b.at_no = c.at_no 
	                        WHERE
		                        (
			                        NOW() BETWEEN DATE_FORMAT( a.start_dt, '%Y-%m-%d %H:%i:%s' ) 
		                        AND DATE_FORMAT( a.end_dt, '%Y-%m-%d %H:%i:%s' )) 
		                        AND c.finish_yn = 'N' 
		                        AND b.type = 'SX' 
	                        GROUP BY b.at_no 
	                        ) AS subquery ON subquery.at_no = a.at_no 
                        WHERE
	                        a.finish_yn IN ( 'N', 'YT' ) 
	                        AND ( @1 = '' OR a.product LIKE @2 ) 
	                        AND ( @3 = '' OR a.at_no LIKE @4 ) 
	                        AND (@5 = '' OR DATE_FORMAT( a.reg_dt, '%Y/%m/%d' ) >= DATE_FORMAT( @5, '%Y/%m/%d' )) 
	                        AND (@5 = '' OR DATE_FORMAT( a.reg_dt, '%Y/%m/%d' ) <= DATE_FORMAT( @5, '%Y/%m/%d' ))
                        
                ) mydevice where mydevice.RowNum BETWEEN @6 AND @7";
            return db.Database.SqlQuery<WActualPrimary>(sqlquery, new MySqlParameter("1", product == null ? "" : product),
                new MySqlParameter("@2", "%" + product == null ? "" : product + "%"), new MySqlParameter("3", at_no == null ? "" : at_no), new MySqlParameter("4", "%" + at_no == null ? "" : at_no + "%"),
                new MySqlParameter("5", reg_dt == null ? "" : reg_dt), new MySqlParameter("6", start), new MySqlParameter("7", end));
        }
        public int GetTotalcountDatawActualPrimary(string product, string at_no, string reg_dt)
        {
            string sqlquery = @"
                    SELECT COUNT(a.id_actualpr)
                        FROM
	                        w_actual_primary AS a
	                        LEFT JOIN d_style_info b ON a.product = b.style_no
	                        LEFT JOIN (
	                        SELECT
		                        b.at_no 
	                        FROM
		                        d_pro_unit_staff AS a
		                        JOIN w_actual AS b ON a.id_actual = b.id_actual
		                        JOIN w_actual_primary AS c ON b.at_no = c.at_no 
	                        WHERE
		                        (
			                        NOW() BETWEEN DATE_FORMAT( a.start_dt, '%Y-%m-%d %H:%i:%s' ) 
		                        AND DATE_FORMAT( a.end_dt, '%Y-%m-%d %H:%i:%s' )) 
		                        AND c.finish_yn = 'N' 
		                        AND b.type = 'SX' 
	                        GROUP BY b.at_no 
	                        ) AS subquery ON subquery.at_no = a.at_no 
                        WHERE
	                        a.finish_yn IN ( 'N', 'YT' ) 
	                        AND ( @1 = '' OR a.product LIKE @2 ) 
	                        AND ( @3 = '' OR a.at_no LIKE @4 ) 
	                        AND (@5 = '' OR DATE_FORMAT( a.reg_dt, '%Y/%m/%d' ) >= DATE_FORMAT( @5, '%Y/%m/%d' )) 
	                        AND (@5 = '' OR DATE_FORMAT( a.reg_dt, '%Y/%m/%d' ) <= DATE_FORMAT( @5, '%Y/%m/%d' ))";
            return db.Database.SqlQuery<int>(sqlquery, new MySqlParameter("1", product == null ? "" : product),
                new MySqlParameter("2", "%" + product == null ? "" : product + "%"), new MySqlParameter("3", at_no == null ? "" : at_no), new MySqlParameter("4", "%" + at_no == null ? "" : at_no + "%"),
                new MySqlParameter("5", reg_dt == null ? "" : reg_dt)).FirstOrDefault();
        }
        public IEnumerable<DProUnitStaffAPI> GetTIMSListStaffByActualId(int id_actual, string staff_id, string StartDate, string EndDate)
        {
            string sql = @"SELECT a.psid, a.staff_id,a.actual ActualQty,a.use_yn,a.staff_tp,
                     (SELECT uname FROM mb_info WHERE userid=a.staff_id)uname, (a.defect)Defective
                     ,DATE_FORMAT(CAST(a.start_dt AS datetime),'%Y-%m-%d %H:%i:%s') AS start_dt,
                     concat(substring(a.end_dt,1,4),'-', substring(a.end_dt,5,2),'-', substring(a.end_dt,7,2),' ', substring(a.end_dt,9,2),':', substring(a.end_dt,11,2),':', substring(a.end_dt,13,2)) end_dt,
                    (case when NOW() BETWEEN  DATE_FORMAT(a.start_dt, '%Y-%m-%d %H:%i:%s') AND DATE_FORMAT(a.end_dt, '%Y-%m-%d %H:%i:%s') then 'HT' ELSE 'QK' END)het_ca
                    FROM d_pro_unit_staff AS a
                    WHERE a.id_actual= @1
                    AND (@2 ='' OR a.staff_id LIKE @3)
                    AND (@4 ='' OR DATE_FORMAT(a.start_dt,'%Y/%m/%d') >= DATE_FORMAT(@5,'%Y/%m/%d') )
                    AND( @6 = '' OR DATE_FORMAT(a.end_dt, '%Y/%m/%d') <= DATE_FORMAT(@7, '%Y/%m/%d')); ";
            return db.Database.SqlQuery<DProUnitStaffAPI>(sql, new MySqlParameter("1", id_actual),
                                                                new MySqlParameter("2", staff_id),
                                                                new MySqlParameter("3", "%" + staff_id + "%"),
                                                                new MySqlParameter("4", StartDate),
                                                                new MySqlParameter("5", StartDate),
                                                                new MySqlParameter("6", EndDate),
                                                                new MySqlParameter("7", EndDate));

        }
        public IEnumerable<WMaterialnfo> FindAllWMaterialInfoByStaffIdOQC(string staff_id_oqc, int id_actual, string start_dt, string end_dt)
        {
            string sql = @"SELECT 
                                wmtid  as wmtid   ,
                                reg_dt as date    ,
                                mt_cd  as mt_cd   ,
                                mt_no  as mt_no   ,
                                real_qty  as real_qty ,
                                gr_qty  as gr_qty     ,
                                gr_qty  as gr_qty1    ,
                                mt_qrcode as mt_qrcode ,
                                bb_no  as bb_no    ,
                                mt_barcode as mt_barcode,
                                chg_dt   as chg_dt 
                            from w_material_info
                            WHERE mt_sts_cd != '003'
                            and staff_id_oqc = @1 AND id_actual_oqc =@2  AND ((SELECT k.reg_dt FROM w_material_mapping AS k  WHERE k.mt_lot IS NULL AND mt_cd=k.mt_cd LIMIT 1) between DATE_FORMAT(@3, '%Y-%m-%d %H:%i:%s') and DATE_FORMAT(@4, '%Y-%m-%d %H:%i:%s')) Order by reg_dt desc";
            return db.Database.SqlQuery<WMaterialnfo>(sql, new MySqlParameter("1", staff_id_oqc),
                                                                new MySqlParameter("2", id_actual),
                                                                new MySqlParameter("3", start_dt),
                                                                new MySqlParameter("4", end_dt));
        }
        public IEnumerable<WMaterialnfo> FindAllWMaterialInfoByStaffId(string staff_id, int id_actual, string start_dt, string end_dt)
        {
            string sql = @"SELECT 
                                wmtid  as wmtid   ,                            
                                mt_cd  as mt_cd   ,
                                mt_no  as mt_no   ,
                                real_qty  as real_qty ,
                                gr_qty  as gr_qty     ,
                                gr_qty  as gr_qty1    ,
                                mt_qrcode as mt_qrcode ,
                                bb_no  as bb_no    ,
                                mt_barcode as mt_barcode,
                                DATE_FORMAT(chg_dt, '%Y-%m-%d %H:%i:%s') as chg_dt ,
                                DATE_FORMAT(reg_dt, '%Y-%m-%d %H:%i:%s') as date ,
                                DATE_FORMAT(reg_dt, '%Y-%m-%d %H:%i:%s') as reg_dt 
                            from w_material_info
                            WHERE mt_sts_cd != '003'
                            and staff_id = @1 AND id_actual =@2 
                            AND (reg_dt between DATE_FORMAT(CAST(@3 AS datetime), '%Y-%m-%d %H:%i:%s') and DATE_FORMAT(CAST(@4 AS datetime), '%Y-%m-%d %H:%i:%s')) and orgin_mt_cd IS  NULL
                            Order by reg_dt desc
                            ";
            return db.Database.SqlQuery<WMaterialnfo>(sql, new MySqlParameter("1", staff_id),
                                                                new MySqlParameter("2", id_actual),
                                                                new MySqlParameter("3", start_dt),
                                                                new MySqlParameter("4", end_dt));
        }
        public WMaterialnfo FindOneWMaterialInfoByMTQRCode(string mt_qrcode)
        {
            string sql = @"select 
                            wmtid , id_actual , id_actual_oqc,at_no,product,staff_id,staff_id_oqc,machine_id,mt_type,mt_cd,mt_no,
                            gr_qty,real_qty,staff_qty,sp_cd,rd_no,sd_no,ext_no,ex_no,dl_no,recevice_dt,date,return_date,
                            alert_NG,expiry_dt,dt_of_receipt,expore_dt,recevice_dt_tims,rece_wip_dt,picking_dt,shipping_wip_dt,
                            end_production_dt,lot_no,mt_barcode,mt_qrcode,mt_sts_cd,bb_no,bbmp_sts_cd,lct_cd,lct_sts_cd,from_lct_cd,to_lct_cd,
                            output_dt,input_dt,buyer_qr,orgin_mt_cd,remark,sts_update,use_yn,reg_id,chg_id,date,reg_dt,chg_dt
                             from w_material_info
                            where mt_qrcode = @1 ";
            return db.Database.SqlQuery<WMaterialnfo>(sql, new MySqlParameter("1", mt_qrcode)).SingleOrDefault();

        }
        public IEnumerable<DBobbinLctHist> FindAllBobbin_lct_histByBBNo(string bb_no)
        {
            string sql = @"select blno, mc_type,bb_no,bb_nm,mt_cd,start_dt,end_dt,use_yn,del_yn,reg_id,reg_dt,chg_id,chg_dt 
                            from d_bobbin_lct_hist
                            where bb_no = @1 ";
            return db.Database.SqlQuery<DBobbinLctHist>(sql, new MySqlParameter("1", bb_no));
        }
        public IEnumerable<DBobbinLctHist> FindAllBobbin_lct_histByBBNoMTCd(string bb_no, string mt_cd)
        {
            string sql = @"select blno, mc_type,bb_no,bb_nm,mt_cd,start_dt,end_dt,use_yn,del_yn,reg_id,reg_dt,chg_id,chg_dt 
                            from d_bobbin_lct_hist
                            where bb_no = @1 and mt_cd =@2  ";
            return db.Database.SqlQuery<DBobbinLctHist>(sql, new MySqlParameter("1", bb_no),
                                                                new MySqlParameter("2", mt_cd)
                );
        }
        public WMaterialnfo FindWMaterialInfo(string mt_cd, string mt_sts_cd1, string mt_sts_cd2, string lct_cd, string lct_sts_cd, int id_actual)
        {
            string sql = @" select 
                            wmtid , id_actual , id_actual_oqc,at_no,product,staff_id,staff_id_oqc,machine_id,mt_type,mt_cd,mt_no,
                            gr_qty,real_qty,staff_qty,sp_cd,rd_no,sd_no,ext_no,ex_no,dl_no,recevice_dt,date,return_date,
                            alert_NG,expiry_dt,dt_of_receipt,expore_dt,recevice_dt_tims,rece_wip_dt,picking_dt,shipping_wip_dt,
                            end_production_dt,lot_no,mt_barcode,mt_qrcode,mt_sts_cd,bb_no,bbmp_sts_cd,lct_cd,lct_sts_cd,from_lct_cd,to_lct_cd,
                            output_dt,input_dt,buyer_qr,orgin_mt_cd,remark,sts_update,use_yn,reg_id,chg_id,date,reg_dt,chg_dt
                            from w_material_info 
                            where mt_cd = @1 AND  (mt_sts_cd = @2 OR mt_sts_cd = @3)
                            AND lct_cd.like @4 AND lct_sts_cd = '101' AND gr_qty > 0
                            AND(@5 != id_actual OR @6 = id_actual AND id_actual_oqc is not null AND id_actual_oqc <> 0) ";
            return db.Database.SqlQuery<WMaterialnfo>(sql,
                new MySqlParameter("1", mt_cd),
                new MySqlParameter("2", mt_sts_cd1),
                new MySqlParameter("3", mt_sts_cd2),
                new MySqlParameter("4", "%" + "006" + "%"),
                new MySqlParameter("5", id_actual),
                new MySqlParameter("6", id_actual)
                ).SingleOrDefault();
        }
        public IEnumerable<WMaterialnfo> FindWMaterialInfo(string mt_cd)
        {
            string sql = @" select 
                            wmtid , id_actual , id_actual_oqc,at_no,product,staff_id,staff_id_oqc,machine_id,mt_type,mt_cd,mt_no,
                            gr_qty,real_qty,staff_qty,sp_cd,rd_no,sd_no,ext_no,ex_no,dl_no,recevice_dt,date,return_date,
                            alert_NG,expiry_dt,dt_of_receipt,expore_dt,recevice_dt_tims,rece_wip_dt,picking_dt,shipping_wip_dt,
                            end_production_dt,lot_no,mt_barcode,mt_qrcode,mt_sts_cd,bb_no,bbmp_sts_cd,lct_cd,lct_sts_cd,from_lct_cd,to_lct_cd,
                            output_dt,input_dt,buyer_qr,orgin_mt_cd,remark,sts_update,use_yn,reg_id,chg_id,date,reg_dt,chg_dt
                            from w_material_info 
                            where mt_cd = @1  ";
            return db.Database.SqlQuery<WMaterialnfo>(sql, new MySqlParameter("1", mt_cd));
        }
        public IEnumerable<OKReason> GetOKReason(string mt_lot, string PrimaryProduct, string don_vi_pr, string mt_cd, string product, string mt_no, string bb_no)
        {
            string sqlquery = @"SELECT a.wmtid,a.mt_cd,a.mt_no,a.gr_qty,a.bb_no,b.at_no 
FROM w_material_info AS a 
JOIN w_actual AS b ON a.id_actual=b.id_actual 
JOIN w_actual_primary AS c ON b.at_no=c.at_no 
JOIN d_bobbin_lct_hist AS d ON a.mt_cd=d.mt_cd 
WHERE a.lct_cd LIKE '006%' AND a.gr_qty> 0 AND a.mt_cd !=@1 AND c.product=@2 AND b.don_vi_pr=@3 AND (@4='' OR a.mt_cd LIKE @5) AND (@6='' OR c.product LIKE @7) AND (@8='' OR a.mt_no LIKE @9) AND (@10='' OR a.bb_no LIKE @11) ORDER BY a.wmtid DESC";
            return db.Database.SqlQuery<OKReason>(sqlquery, new MySqlParameter("1", mt_lot), new MySqlParameter("2", PrimaryProduct), new MySqlParameter("3", don_vi_pr), new MySqlParameter("4", mt_cd)
                , new MySqlParameter("5", '%' + mt_cd + '%'), new MySqlParameter("6", product), new MySqlParameter("7", '%' + product + '%'), new MySqlParameter("8", mt_no), new MySqlParameter("9", '%' + mt_no + '%'),
                new MySqlParameter("10", bb_no), new MySqlParameter("11", '%' + bb_no + '%'));
        }
        public int CheckShift(int psid)
        {
            string sql = @"SELECT count(a.psid) FROM d_pro_unit_staff AS a  WHERE  
                            a.psid= @1 AND (NOW() BETWEEN Date_format(a.start_dt , '%Y-%m-%d %H:%i:%s') AND Date_format(a.end_dt, '%Y-%m-%d %H:%i:%s'))";
            return db.Database.SqlQuery<int>(sql, new MySqlParameter("@1", psid)).FirstOrDefault();
        }

        public int CheckShift(int psid, int id_actual)
        {
            string sql = @"SELECT count(a.psid) FROM d_pro_unit_staff AS a WHERE a.psid=@1 AND a.id_actual=@2 AND (NOW() BETWEEN Date_format(a.start_dt ,'%Y-%m-%d %H:%i:%s')
                    AND Date_format(a.end_dt,'%Y-%m-%d %H:%i:%s'))";
            return db.Database.SqlQuery<int>(sql, new MySqlParameter("@1", psid), new MySqlParameter("@2", id_actual)).FirstOrDefault();
        }
        public int UpdateMaterialInfoQty(int defec_t, int check_qty, int ok_qty, string bien)
        {
            string sql = @" Update w_material_info set
                                      gr_qty = (gr_qty - @1) + @2,
                                      real_qty = (real_qty - @3) +@4
                                      where mt_cd =@5 ";
            return db.Database.ExecuteSqlCommand(sql,
                new MySqlParameter("1", defec_t),
                new MySqlParameter("2", check_qty - ok_qty),
                new MySqlParameter("3", defec_t),
                new MySqlParameter("4", check_qty - ok_qty),
                new MySqlParameter("5", bien)
                );
        }
        public int UpdateMtCdBobbinHistInfo(string mt_cd, int blno)
        {
            string sql = @" Update d_bobbin_lct_hist set mt_cd = @1 where blno = @2 ";
            return db.Database.ExecuteSqlCommand(sql,
                new MySqlParameter("1", mt_cd),
                new MySqlParameter("2", blno));
        }
        public int UpdateMaterialInfoRemain(double? remain, string mt_sts_cd, int wmtid)
        {
            string sql = @"Update w_material_info set gr_qty=@1 , mt_sts_cd=@2 where wmtid = @3 ";
            return db.Database.ExecuteSqlCommand(sql,
              new MySqlParameter("1", remain),
              new MySqlParameter("2", mt_sts_cd),
              new MySqlParameter("3", wmtid));
        }
        public int UpdateFaclineQty(int check_qty, int ok_qty, int fqno)
        {
            string sql_update_faclineQty = @"Update m_facline_qc set check_qty= @1 , ok_qty = @2 where fqno = @3";
            return db.Database.ExecuteSqlCommand(sql_update_faclineQty,
                   new MySqlParameter("@1", check_qty),
                   new MySqlParameter("@2", ok_qty),
                   new MySqlParameter("@3", fqno));
        }
        public IEnumerable<MFaclineQC> FindFaclineQcByFq_no(string fq_no)
        {
            string sql = @"select  fqno,fq_no,ml_no,ml_tims,product_cd,shift,at_no,
                            work_dt,item_vcd,item_nm,item_exp,check_qty,ok_qty,reg_id,reg_dt,chg_id,chg_dt
                            from m_facline_qc 
                            where fq_no like @1
                           ";
            return db.Database.SqlQuery<MFaclineQC>(sql, new MySqlParameter("@1", fq_no + "%"));
        }
        public int InsertMFaclineQC(m_facline_qc mFaclineQC)
        {
            string sqlquery = @"insert into m_facline_qc(fq_no,ml_no,ml_tims,work_dt,reg_dt,chg_dt,item_nm,item_exp,reg_id,chg_id,check_qty,ok_qty,item_vcd,product_cd,shift,at_no) 
                                        values(@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16) ; Select last_insert_id();";
            return db.Database.SqlQuery<int>(sqlquery, new MySqlParameter("1", mFaclineQC.fq_no),
                                                        new MySqlParameter("2", mFaclineQC.ml_no),
                                                        new MySqlParameter("3", mFaclineQC.ml_tims),
                                                        new MySqlParameter("4", mFaclineQC.work_dt),
                                                        new MySqlParameter("5", mFaclineQC.reg_dt),
                                                        new MySqlParameter("6", mFaclineQC.chg_dt),
                                                        new MySqlParameter("7", mFaclineQC.item_nm),
                                                        new MySqlParameter("8", mFaclineQC.item_exp),
                                                        new MySqlParameter("9", mFaclineQC.reg_id),
                                                        new MySqlParameter("10", mFaclineQC.chg_id),
                                                        new MySqlParameter("11", mFaclineQC.check_qty),
                                                        new MySqlParameter("12", mFaclineQC.ok_qty),
                                                        new MySqlParameter("13", mFaclineQC.item_vcd),
                                                        new MySqlParameter("14", mFaclineQC.product_cd),
                                                        new MySqlParameter("15", mFaclineQC.shift),
                                                        new MySqlParameter("16", mFaclineQC.at_no)).FirstOrDefault();
        }
        public int UpdateMaterialInfoQty(int qty, string bien)
        {
            string QuerySQL = @"UPDATE w_material_info Set  gr_qty=(gr_qty+ @1),real_qty=(real_qty+ @2)
                                      WHERE mt_cd= @3 ";
            return db.Database.ExecuteSqlCommand(QuerySQL,
                new MySqlParameter("1", qty),
                new MySqlParameter("2", qty),
                new MySqlParameter("3", bien));
        }
        public int InsertBobbinHist(d_bobbin_lct_hist bobbin_hist, string nameaaa)   //cai name nay de phan biet voi ham kia thoi chu ko co tac dung truyen vao
        {
            string QuerySQL = @"insert into 
                                d_bobbin_lct_hist(bb_no,bb_nm,mt_cd,mc_type,reg_id,chg_id,reg_dt,chg_dt,del_yn,use_yn) ;
                                values(@1,@2,@3,@4,@5,@6,@7,@8,@9,@10); SELECT LAST_INSERT_ID() ";
            return db.Database.SqlQuery<int>(QuerySQL,
                  new MySqlParameter("1", bobbin_hist.bb_no),
                  new MySqlParameter("2", bobbin_hist.bb_nm),
                  new MySqlParameter("3", bobbin_hist.mt_cd),
                  new MySqlParameter("4", bobbin_hist.mc_type),
                  new MySqlParameter("5", bobbin_hist.reg_id),
                  new MySqlParameter("6", bobbin_hist.chg_id),
                  new MySqlParameter("7", bobbin_hist.reg_dt),
                  new MySqlParameter("8", bobbin_hist.chg_dt),
                  new MySqlParameter("9", bobbin_hist.del_yn),
                  new MySqlParameter("10", bobbin_hist.use_yn)
                  ).FirstOrDefault();
        }

        public IEnumerable<WMaterialInfoTmp> GetListMaterial(string wmtid, string mt_cd, string mt_no)
        {
            string sqlquery = @"SELECT wmtid,id_actual,id_actual_oqc,at_no,product,staff_id,staff_id_oqc,mt_type,mt_cd,mt_no,gr_qty,
real_qty,date,end_production_dt,mt_barcode,mt_qrcode,mt_sts_cd,bb_no,bbmp_sts_cd,lct_cd,
lct_sts_cd,from_lct_cd,input_dt,orgin_mt_cd,sts_update,use_yn,reg_id,reg_dt,chg_id,chg_dt 
FROM w_material_info WHERE FIND_IN_SET(wmtid, @1) != 0  AND gr_qty> 0 AND mt_cd !=@2 AND mt_no=@3 ORDER BY gr_qty DESC";
            return db.Database.SqlQuery<WMaterialInfoTmp>(sqlquery,
                new MySqlParameter("1", wmtid),
                new MySqlParameter("2", mt_cd), new MySqlParameter("3", mt_no));
        }




        public void UpdateMaterial(double? quantity, int wmtid)
        {
            string sqlquery = "UPDATE w_material_info SET gr_qty=@1 WHERE wmtid=@2";
            db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", quantity), new MySqlParameter("2", wmtid));
        }

        public void UpdateMaterial(double? quantity, int wmtid, string mt_sts_cd)
        {
            string sqlquery = "UPDATE w_material_info SET gr_qty=@1,mt_sts_cd=@3 WHERE wmtid=@2";
            db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", quantity), new MySqlParameter("2", wmtid), new MySqlParameter("3", mt_sts_cd));
        }

        public void UpdateBobbinInfo(string chg_id, string bb_no, string mt_cd)
        {
            string sqlquery = "UPDATE d_bobbin_info SET chg_id=@1,mt_cd=NULL WHERE bb_no=@2 AND mt_cd=@3";
            db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", chg_id), new MySqlParameter("2", bb_no), new MySqlParameter("3", mt_cd));
        }

        public void DeleteBobbinHistory(string bb_no, string mt_cd)
        {
            string sqlquery = "DELETE FROM d_bobbin_lct_hist WHERE bb_no=@1 AND mt_cd=@2";
            db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", bb_no), new MySqlParameter("2", mt_cd));
        }

        public int CountMaterialInfo(string LikeCondition)
        {
            string sqlquery = "SELECT count(wmtid) FROM w_material_info WHERE mt_cd LIKE @1";
            return db.Database.SqlQuery<int>(sqlquery, new MySqlParameter("1", LikeCondition)).FirstOrDefault();
        }

        public void InsertMergeMaterial(WMaterialInfoTmp data)
        {
            string sqlquery = "INSERT INTO w_material_info (id_actual,id_actual_oqc,at_no,product,staff_id,staff_id_oqc,mt_type,mt_cd,mt_no,gr_qty,real_qty,date,end_production_dt,mt_barcode,mt_qrcode,mt_sts_cd,bb_no,bbmp_sts_cd,lct_cd,lct_sts_cd,from_lct_cd,input_dt,orgin_mt_cd,sts_update,use_yn,reg_id,reg_dt,chg_id,chg_dt) VALUES (@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14, @15, @16, @17, @18, @19, @20, @21, @22, @23, @24, @25, @26, @27, @28, @29);";
            db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", data.id_actual), new MySqlParameter("2", data.id_actual_oqc), new MySqlParameter("3", data.at_no), new MySqlParameter("4", data.product), new MySqlParameter("5", data.staff_id), new MySqlParameter("6", data.staff_id_oqc), new MySqlParameter("7", data.mt_type), new MySqlParameter("8", data.mt_cd), new MySqlParameter("9", data.mt_no), new MySqlParameter("10", data.gr_qty), new MySqlParameter("11", data.real_qty), new MySqlParameter("12", data.date), new MySqlParameter("13", data.end_production_dt), new MySqlParameter("14", data.mt_barcode), new MySqlParameter("15", data.mt_qrcode), new MySqlParameter("16", data.mt_sts_cd), new MySqlParameter("17", data.bb_no), new MySqlParameter("18", data.bbmp_sts_cd), new MySqlParameter("19", data.lct_cd), new MySqlParameter("20", data.lct_sts_cd), new MySqlParameter("21", data.from_lct_cd), new MySqlParameter("22", data.input_dt), new MySqlParameter("23", data.orgin_mt_cd), new MySqlParameter("24", data.sts_update), new MySqlParameter("25", data.use_yn), new MySqlParameter("26", data.reg_id), new MySqlParameter("27", data.reg_dt), new MySqlParameter("28", data.chg_id), new MySqlParameter("29", data.chg_dt));
        }

        public bool CheckExistMaterialMapping(string mt_cd, string mt_lot)
        {
            string sqlquery = "SELECT wmmid FROM w_material_mapping WHERE mt_cd=@1 AND mt_lot=@2";
            return db.Database.SqlQuery<int?>(sqlquery, new MySqlParameter("1", mt_cd), new MySqlParameter("2", mt_lot)).FirstOrDefault().HasValue;
        }

        public int InsertMaterialMapping(w_material_mapping data)
        {
            string sqlquery = @"INSERT INTO w_material_mapping (mt_cd,mt_lot,mt_no,mapping_dt,bb_no,use_yn,del_yn,chg_id,reg_id,reg_dt,chg_dt,remark) VALUES (@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12);
                                 SELECT LAST_INSERT_ID();";
            return db.Database.SqlQuery<int>(sqlquery,
                 new MySqlParameter("1", data.mt_cd),
                 new MySqlParameter("2", data.mt_lot),
                 new MySqlParameter("3", data.mt_no),
                 new MySqlParameter("4", data.mapping_dt),
                 new MySqlParameter("5", data.bb_no),
                 new MySqlParameter("6", data.use_yn),
                 new MySqlParameter("7", data.del_yn),
                 new MySqlParameter("8", data.chg_id),
                 new MySqlParameter("9", data.reg_id),
                 new MySqlParameter("10", data.reg_dt),
                 new MySqlParameter("11", data.chg_dt),
                 new MySqlParameter("12", data.remark)
                 ).FirstOrDefault();
        }
    
        public IEnumerable<MaterialInfoMMS> FindAllMaterialInfoReturn(string wmtid)
        {
            string sql = "Select * from w_material_info where wmtid =@1 AND mt_sts_cd='009' AND lct_cd like '%006%' ";
            return db.Database.SqlQuery<MaterialInfoMMS>(sql, new MySqlParameter("1", wmtid));
        }
        public IEnumerable<w_product_qc> FindAllWProductQcByMl_no(string mt_cd)
        {
            string sql = @"select * from w_product_qc where ml_no = @1  ";
            return db.Database.SqlQuery<w_product_qc>(sql, new MySqlParameter("1", mt_cd));
        }
        public int UpdateWActualById(double? defect, string chg_id, int? id_actual)
        {
            string QuerySQL = @"update w_actual set defect=@1 ,chg_id =@2  where  id_actual = @3 ; ";
            return db.Database.ExecuteSqlCommand(QuerySQL,
                new MySqlParameter("1", defect),
                new MySqlParameter("2", chg_id),
                new MySqlParameter("3", id_actual));
        }

        public int UpdateWMaterialInfoByIdSingle(string mt_sts_cd, string chg_id, DateTime? end_production_dt, int wmtid)
        {
            string sql = "UPDATE w_material_info SET mt_sts_cd=@1,chg_id=@2,end_production_dt=@3 WHERE wmtid=@4;";
            return db.Database.ExecuteSqlCommand(sql, new MySqlParameter("1", mt_sts_cd),
                                                new MySqlParameter("2", chg_id),
                                                new MySqlParameter("3", end_production_dt),
                                                new MySqlParameter("4", wmtid));
        }

        public int UpdateWMaterialInfoByIdMultiple(string mt_sts_cd, string chg_id, DateTime? end_production_dt, string wmtid)
        {
            string sql = "UPDATE w_material_info SET mt_sts_cd=@1,chg_id=@2,end_production_dt=@3 WHERE FIND_IN_SET(wmtid,@4) !=0;";
            return db.Database.ExecuteSqlCommand(sql, new MySqlParameter("1", mt_sts_cd),
                                                      new MySqlParameter("2", chg_id),
                                                      new MySqlParameter("3", end_production_dt),
                                                      new MySqlParameter("4", wmtid));
        }
        public IEnumerable<WMaterialMapping> FindAllWMaterialMappingByMtLotMtCd(string mt_cd, string mt_lot)
        {
            string getvalue = @"select 
                                        wmmid  ,
                                        mt_lot ,
                                        mt_cd  ,
                                        mt_no  ,
                                        mapping_dt,
                                        bb_no     ,
                                        remark   , 
                                        sts_share ,
                                        use_yn    ,
                                        del_yn    ,
                                        reg_id   , 
                                         reg_dt,
                                         chg_id,
                                         chg_dt
                                    from w_material_mapping where mt_cd = @1 and mt_lot =@2
                                    Order by mapping_dt desc
                                    ";
            return db.Database.SqlQuery<WMaterialMapping>(getvalue, new MySqlParameter("1", mt_cd),
                                                                    new MySqlParameter("2", mt_lot)
                );
        }
        public int UpdateActualStaffStoreProcedure(int id_actual, int psid)
        {
            return 0;
        }
        public IEnumerable<WMaterialMapping> FindAllWMaterialMappingByMtLot(string mt_cd)
        {
            string sql = @"SELECT a.wmmid as wmmid
                                     ,a.mt_lot as mt_lot
                                     ,a.mt_cd as mt_cd
                                     ,DATE_FORMAT(a.mapping_dt, '%Y-%m-%d %H:%i:%s') as mapping_dt
                                     ,a.use_yn as use_yn
                                     ,a.reg_dt as reg_dt
                                     ,b.gr_qty as gr_qty
                                     ,b.mt_no as mt_no
                                     ,b.bb_no as bb_no
                                     ,(CASE WHEN a.use_yn='N' THEN b.gr_qty ELSE 0 END ) as  Used ,
                                    (CASE WHEN a.use_yn='N' THEN Find_Remain(a.mt_cd,b.orgin_mt_cd,a.reg_dt) ELSE 0 END) as Remain 
                                    FROM w_material_mapping AS a
                                    JOIN w_material_info AS b ON a.mt_cd=b.mt_cd
                                    WHERE a.mt_lot= @1
                                    ORDER BY a.use_yn='Y';";
            return db.Database.SqlQuery<WMaterialMapping>(sql, new MySqlParameter("1", mt_cd));

        }
        public IEnumerable<WMaterialnfo> FindWMaterialInfoByMt_cdId_actual(string mt_cd, int id_actual)
        {
            string sql2 = @" select * from w_material_info x
                                        where 
                                        x.mt_cd = @1 AND  
                                        (x.mt_sts_cd =@2 OR x.mt_sts_cd =@3) AND
                                        x.lct_cd like '006%' AND x.lct_sts_cd =@4 AND x.gr_qty > 0 AND
                                        (@5 <> x.id_actual  OR(@6 = x.id_actual AND x.id_actual_oqc <> null  AND x.id_actual_oqc <> 0))";
            return db.Database.SqlQuery<WMaterialnfo>(sql2,
                                                            new MySqlParameter("1", mt_cd),
                                                            new MySqlParameter("2", "008"),
                                                            new MySqlParameter("3", "002"),
                                                            new MySqlParameter("4", "101"),
                                                            new MySqlParameter("5", id_actual),
                                                            new MySqlParameter("6", id_actual));
        }

        public IEnumerable<w_actual> GetListWActualForProcess(string at_no)
        {
            string sql = @"SELECT * 
                            FROM w_actual
                            WHERE at_no = @1 AND type = 'TIMS' AND name != 'OQC'";
            return db.Database.SqlQuery<w_actual>(sql, new MySqlParameter("1", at_no));
        }
        public bool CheckWMaterialHasNG(string mt_cd)
        {
            string QuerySQL = "SELECT mt_cd FROM w_material_info WHERE mt_cd like @1 AND lct_cd ='006000000000000000'";
            return String.IsNullOrWhiteSpace(db.Database.SqlQuery<string>(QuerySQL,
                new MySqlParameter("1", mt_cd + "-NG%")).FirstOrDefault());
        }

        public d_bobbin_info GetOneDBobbinInfoWithMtCdIsNULL(string bb_no)
        {
            string sql_ds_bb = @"select * from d_bobbin_info where bb_no = @1 and ( mt_cd is null OR mt_cd ='' )  limit 1";
            return db.Database.SqlQuery<d_bobbin_info>(sql_ds_bb, new MySqlParameter("1", bb_no)).SingleOrDefault();
        }
        public IEnumerable<MaterialInfoMMS> FindAllMaterialByMtCdLike(string bien_first)
        {
            var sql_ds = @"Select * from w_material_info where mt_cd like @1 ";
            return db.Database.SqlQuery<MaterialInfoMMS>(sql_ds, new MySqlParameter("1", "%" + bien_first + "%"));
        }
        public int InsertWMaterialInfoToBobin(MaterialInfoMMS a)
        {
            var sql_insert = @" insert into w_material_info(mt_cd, mt_barcode,mt_qrcode, date,product,at_no,input_dt,mt_type,bb_no,real_qty,staff_id,mt_no,bbmp_sts_cd,
                                                            mt_sts_cd,lct_cd,from_lct_cd,lct_sts_cd,id_actual,use_yn,sts_update,gr_qty,chg_dt,chg_id,reg_id,reg_dt )
                                  values(@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16,@17,@18,@19,@20,@21,@22,@23,@24,@25);SELECT LAST_INSERT_ID()";
            int idReturn = db.Database.SqlQuery<int>(sql_insert, new MySqlParameter("@1", a.mt_cd),
                                                      new MySqlParameter("@2", a.mt_barcode),
                                                      new MySqlParameter("@3", a.mt_qrcode),
                                                      new MySqlParameter("@4", a.date),
                                                      new MySqlParameter("@5", a.product),
                                                      new MySqlParameter("@6", a.at_no),
                                                      new MySqlParameter("@7", a.input_dt),
                                                      new MySqlParameter("@8", a.mt_type),
                                                      new MySqlParameter("@9", a.bb_no),
                                                      new MySqlParameter("@10", a.real_qty),
                                                      new MySqlParameter("@11", a.staff_id),
                                                      new MySqlParameter("@12", a.mt_no),
                                                      new MySqlParameter("@13", a.bbmp_sts_cd),
                                                      new MySqlParameter("@14", a.mt_sts_cd),
                                                      new MySqlParameter("@15", a.lct_cd),
                                                      new MySqlParameter("@16", a.from_lct_cd),
                                                      new MySqlParameter("@17", a.lct_sts_cd),
                                                      new MySqlParameter("@18", a.id_actual),
                                                      new MySqlParameter("@19", a.use_yn),
                                                      new MySqlParameter("@20", a.sts_update),
                                                      new MySqlParameter("@21", a.gr_qty),
                                                      new MySqlParameter("@22", a.chg_dt),
                                                      new MySqlParameter("@23", a.chg_id),
                                                      new MySqlParameter("@24", a.reg_id),
                                                      new MySqlParameter("@25", a.reg_dt)).FirstOrDefault();
            return idReturn;
        }
        public IEnumerable<w_material_mapping> FindOneWMaterialMappingByIdActualAndStaffId(int id_actual, string staff_id)
        {
            string sql_w_material_mapping = @"Select * from w_material_mapping i inner join w_material_info j ON i.mt_lot = j.mt_cd
                                                where j.id_actual = @1 AND j.staff_id = @2 Order by i.reg_dt desc ";
            return db.Database.SqlQuery<w_material_mapping>(sql_w_material_mapping,
                                                               new MySqlParameter("1", id_actual),
                                                               new MySqlParameter("2", staff_id));
        }
        public WMaterialnfo FindOneWMaterialInfo(int wmtid)
        {
            string sql = @" select 
                            wmtid , id_actual , id_actual_oqc,at_no,product,staff_id,staff_id_oqc,machine_id,mt_type,mt_cd,mt_no,
                            gr_qty,real_qty,staff_qty,sp_cd,rd_no,sd_no,ext_no,ex_no,dl_no,recevice_dt,date,return_date,
                            alert_NG,expiry_dt,dt_of_receipt,expore_dt,recevice_dt_tims,rece_wip_dt,picking_dt,shipping_wip_dt,
                            end_production_dt,lot_no,mt_barcode,mt_qrcode,mt_sts_cd,bb_no,bbmp_sts_cd,lct_cd,lct_sts_cd,from_lct_cd,to_lct_cd,
                            output_dt,input_dt,buyer_qr,orgin_mt_cd,remark,sts_update,use_yn,reg_id,chg_id,date,reg_dt,chg_dt
                            from w_material_info 
                            where wmtid = @1  ";
            return db.Database.SqlQuery<WMaterialnfo>(sql, new MySqlParameter("1", wmtid)).FirstOrDefault();
        }
        public WMaterialnfo FindOneWMaterialInfo(int idReturn, string mt_no, string bbmp_sts_cd)
        {
            var sql_ds1 = @"select wmtid ,DATE_FORMAT(CAST(m.reg_dt AS datetime),'%Y-%m-%d %H:%i:%s') as date, 
                                m.mt_cd,m.mt_no,m.gr_qty,m.mt_qrcode,m.lct_cd,m.bb_no,m.mt_barcode,m.real_qty
                                ,m.bbmp_sts_cd 
                                from w_material_info m 
                                left join comm_dt ccdt on ccdt.dt_cd = @3 AND ccdt.mt_cd ='MMS007'
                                left join lct_info lctinfo on lctinfo.lct_cd = m.lct_cd
                                where m.wmtid = @1 and m.mt_no = @2";
            return db.Database.SqlQuery<WMaterialnfo>(sql_ds1, new MySqlParameter("1", idReturn),
                                                                         new MySqlParameter("@2", mt_no),
                                                                      new MySqlParameter("@3", bbmp_sts_cd)).FirstOrDefault();
        }
        public int SumGroupQtyWMaterialInfo(string mt_cd)
        {
            string sqlsum = @"SELECT SUM(gr_qty) FROM w_material_info WHERE mt_cd LIKE @1";
            int giatri = db.Database.SqlQuery<int>(sqlsum, new MySqlParameter("@1", mt_cd + "%")).FirstOrDefault();
            return giatri;
        }
        public IEnumerable<WMaterialMapping> FindAllWMaterialMappingByMTCdAndNotMtLot(string mt_cd, string mt_lot)
        {
            string getvalue = @"select 
                                        wmmid  ,
                                        mt_lot ,
                                        mt_cd  ,
                                        mt_no  ,
                                        mapping_dt,
                                        bb_no     ,
                                        remark   , 
                                        sts_share ,
                                        use_yn    ,
                                        del_yn    ,
                                        reg_id   , 
                                         reg_dt,
                                         chg_id,
                                         chg_dt
                                    from w_material_mapping where mt_cd = @1 and mt_lot <> @2
                                    ";
            return db.Database.SqlQuery<WMaterialMapping>(getvalue, new MySqlParameter("1", mt_cd),
                                                                    new MySqlParameter("2", mt_lot)
                );
        }
        public void UpdateActualStaff(int? id_actual, int psid)
        {
            string sql = " CALL Update_SL_TIMS(@1, @2);CALL Update_SL_w_actual(@3); ";
            db.Database.ExecuteSqlCommand(sql, new MySqlParameter("1", id_actual),
                                                new MySqlParameter("2", psid),
                                                new MySqlParameter("3", id_actual)
                );
        }
        public int UpdateNGPO(int check_qty, int ok_qty, string bien) //bien = mt_cd
        {
            string sql = @"UPDATE w_material_info SET gr_qty=(gr_qty+ @1),
                                                      real_qty=(real_qty+ @2)
                                                      WHERE mt_cd=@3 ";
            return db.Database.ExecuteSqlCommand(sql, new MySqlParameter("1", (check_qty - ok_qty)),
                                                 new MySqlParameter("2", (check_qty - ok_qty)),
                                                 new MySqlParameter("3", bien));
        }
        public int UpdateContainerOutput(string mt_cd, int qty)
        {
            string sql = @"UPDATE w_material_info a
                        SET gr_qty=@2,real_qty=@2
                        WHERE a.mt_cd=@1";
            return db.Database.ExecuteSqlCommand(sql, new MySqlParameter("@1", mt_cd), new MySqlParameter("@2", qty));
        }
        public void UpdateDefectActual(double? defect, int id_actual)
        {
            string sql = @"
                    Update w_actual SET defect = @1 
                    where id_actual = @2 ;  ";
            db.Database.ExecuteSqlCommand(sql, new MySqlParameter("1", defect),
                                                new MySqlParameter("2", id_actual));
        }
        public WMaterialnfo FindOneWMaterialInfoLike(string mt_cd, string lct_cd, string lct_sts_cd)
        {
            string sql = @"select 
                            wmtid , id_actual , id_actual_oqc,at_no,product,staff_id,staff_id_oqc,machine_id,mt_type,mt_cd,mt_no,
                            gr_qty,real_qty,staff_qty,sp_cd,rd_no,sd_no,ext_no,ex_no,dl_no,recevice_dt,date,return_date,
                            alert_NG,expiry_dt,dt_of_receipt,expore_dt,recevice_dt_tims,rece_wip_dt,picking_dt,shipping_wip_dt,
                            end_production_dt,lot_no,mt_barcode,mt_qrcode,mt_sts_cd,bb_no,bbmp_sts_cd,lct_cd,lct_sts_cd,from_lct_cd,to_lct_cd,
                            output_dt,input_dt,buyer_qr,orgin_mt_cd,remark,sts_update,use_yn,reg_id,chg_id,date,reg_dt,chg_dt
                            from w_material_info 
                            where mt_cd = @1 and lct_cd like '006%' AND lct_sts_cd = '101'";
            return db.Database.SqlQuery<WMaterialnfo>(sql, new MySqlParameter("1", mt_cd)).SingleOrDefault();
        }
        public WMaterialnfo FindOneWMaterialInfoMMS007(int wmtid)
        {
            string sql = @"     select 
                                a.wmtid as wmtid,
                                DATE_FORMAT(a.date,'%Y-%m-%d %H:%i:%s')  as date,
                                com_dt.dt_nm as bbmp_sts_cd,
                                a.mt_cd as mt_cd,
                                a.mt_no as mt_no,
                                a.gr_qty as gr_qty,
                                a.gr_qty as gr_qty1,
                                com_dt.dt_nm ,com_dt.mt_cd as test_mt_cd,
                                a.mt_qrcode,a.lct_cd ,l.lct_nm,a.bb_no,a.mt_barcode,a.chg_dt
                                ,a.gr_qty as sl_tru_ng
                                ,a.real_qty
                                from w_material_info a 
                                inner join comm_dt com_dt on com_dt.dt_cd = a.bbmp_sts_cd and com_dt.mt_cd ='MMS007'
                                inner join lct_info l on l.lct_cd = a.lct_cd  
                                where wmtid = @1;";
            return db.Database.SqlQuery<WMaterialnfo>(sql, new MySqlParameter("1", wmtid)).FirstOrDefault();
        }
        public int InsertWMaterialMapping(w_material_mapping data)
        {
            string sqlquery = @"INSERT INTO w_material_mapping (
                                                                    mt_lot,
                                                                    mt_no,
                                                                    mt_cd,
                                                                    mapping_dt,
                                                                    bb_no,
                                                                    remark,
                                                                    use_yn
                                                                    ,reg_id
                                                                    ,chg_id
                                                                    ,reg_dt) VALUES (@1,@2,@3,@4,@5,@6,@7,@8,@9,@10);";
            return db.Database.SqlQuery<int>(sqlquery,
                 new MySqlParameter("1", data.mt_lot),
                 new MySqlParameter("2", data.mt_no),
                 new MySqlParameter("3", data.mt_cd),
                 new MySqlParameter("4", data.mapping_dt),
                 new MySqlParameter("5", data.bb_no),
                 new MySqlParameter("6", data.remark),
                 new MySqlParameter("7", data.use_yn),
                 new MySqlParameter("8", data.reg_id),
                 new MySqlParameter("9", data.chg_id),
                 new MySqlParameter("10", data.reg_dt)
                 ).FirstOrDefault();
        }
        public int UpdateMaterialStaffQty(int? staff_qty, string mt_sts_cd, int wmtid)
        {
            string sql = @"Update w_material_info 
                                    SET staff_qty = @1 ,mt_sts_cd=@2
                                    Where wmtid = @3";
            return db.Database.ExecuteSqlCommand(sql, new MySqlParameter("1", staff_qty),
                                                 new MySqlParameter("2", mt_sts_cd),
                                                 new MySqlParameter("3", wmtid));
        }
        public bool CheckFaclineQCHasNG(string mt_cd)
        {
            string QuerySQL = "SELECT ml_no FROM m_facline_qc WHERE ml_no = @1";
            return String.IsNullOrWhiteSpace(db.Database.SqlQuery<string>(QuerySQL,
                new MySqlParameter("1", mt_cd)).FirstOrDefault());
            //nếu có data thì return = false
        }
        public bool CheckWProductQCHasNG(string mt_cd)
        {
            string QuerySQL = "SELECT ml_no FROM w_product_qc WHERE ml_no = @1";
            return String.IsNullOrWhiteSpace(db.Database.SqlQuery<string>(QuerySQL,
                new MySqlParameter("1", mt_cd)).FirstOrDefault());
        }

        public IEnumerable<NGPO> GetNotGoodPO(string product, string mt_cd, string mt_no, string PO)
        {
            string QuerySQL = "SELECT mt_sts_cd,mt_no,wmtid,mt_cd,product,mt_no,gr_qty,(CASE WHEN mt_cd NOT LIKE 'po%' THEN (SELECT at_no FROM w_actual WHERE id_actual=id_actual LIMIT 1) ELSE (substring(mt_cd,1,11)) END) at_no FROM w_material_info AS a WHERE mt_sts_cd='012' AND gr_qty> 0 AND mt_no LIKE @1 AND (@2='' OR mt_cd LIKE @3) AND (@4='' OR mt_no LIKE @5) AND (@6='' OR mt_cd LIKE @7) ORDER BY wmtid;";
            return db.Database.SqlQuery<NGPO>(QuerySQL, new MySqlParameter("1", product + "%"), new MySqlParameter("2", mt_cd), new MySqlParameter("3", "%" + mt_cd + "%"), new MySqlParameter("4", mt_no), new MySqlParameter("5", "%" + mt_no + "%"), new MySqlParameter("6", PO), new MySqlParameter("7", "%" + PO + "%"));
        }
        public MaterialInfoMMS CheckWmaterialForDiv(string mt_cd)
        {
            string QuerySQL = "SELECT * FROM w_material_info WHERE mt_cd = @1 AND sts_update ='composite' AND mt_sts_cd = '002' AND gr_qty > 0  limit 1";
            return db.Database.SqlQuery<MaterialInfoMMS>(QuerySQL, new MySqlParameter("1", mt_cd)).SingleOrDefault();
        }
        public MaterialInfoMMS FindAllMeterialChangestPacking(int wmtid)
        {
            string QuerySQL = "SELECT * FROM w_material_info WHERE wmtid = @1 AND mt_sts_cd='009' AND lct_cd='006000000000000000';";
            return db.Database.SqlQuery<MaterialInfoMMS>(QuerySQL, new MySqlParameter("1", wmtid)).FirstOrDefault();
        }
        public IEnumerable<MaterialInfoMMS> FindAllMeterialChangestPackingweb(string wmtid)
        {
            string QuerySQL = "SELECT * FROM w_material_info WHERE FIND_IN_SET(wmtid, @1) !=0 AND mt_sts_cd='009' AND lct_cd='006000000000000000'";
            return db.Database.SqlQuery<MaterialInfoMMS>(QuerySQL, new MySqlParameter("1", wmtid));
        }
        public int FindDProUnitStaffChangestsPacking(int? id_actual, int psid)
        {
            //String varname2 = "";
            //varname2 = varname2 + "SELECT * FROM d_pro_unit_staff AS a " + "\n";
            //varname2 = varname2 + "WHERE a.id_actual=@1 and a.psid=@2 AND (NOW() BETWEEN " + "\n";
            //varname2 = varname2 + "Date_format(a.start_dt, '%Y-%m-%d %H:%i:%s') " + "\n";
            //varname2 = varname2 + " AND Date_format(a.end_dt, '%Y-%m-%d %H:%i:%s'))";
            string sql = @"SELECT COUNT(psid) FROM d_pro_unit_staff WHERE id_actual=@1 AND psid=@2 AND (NOW() BETWEEN Date_format(start_dt,'%Y-%m-%d %H:%i:%s') AND Date_format(end_dt,'%Y-%m-%d %H:%i:%s'));";
            return db.Database.SqlQuery<int>(sql, new MySqlParameter("1", id_actual), new MySqlParameter("2", psid)).FirstOrDefault();
        }
        //public int FindDProUnitStaffChangestsPackingweb(int? id_actual, int psid)
        //{

        //    string sql = @"SELECT COUNT(a.staff_id) FROM d_pro_unit_staff AS a  WHERE a.id_actual=@1 and a.psid=@2 AND (NOW() BETWEEN
        //                            Date_format(a.start_dt, '%Y-%m-%d %H:%i:%s') AND Date_format(a.end_dt, '%Y-%m-%d %H:%i:%s'))";
        //    return db.Database.SqlQuery<int>(sql, new MySqlParameter("1", id_actual), new MySqlParameter("2", psid)).FirstOrDefault();
        //}
        public int InsertWMaterialMappingWithNoMtLot(w_material_mapping data)
        {
            string sqlquery = @"INSERT INTO w_material_mapping (
                                                                    mt_no,
                                                                    mt_cd,
                                                                    mapping_dt,
                                                                    bb_no,
                                                                    remark,
                                                                    use_yn
                                                                    ,reg_id
                                                                    ,chg_id
                                                                    ,reg_dt) VALUES (@1,@2,@3,@4,@5,@6,@7,@8,@9); SELECT LAST_INSERT_ID(); ";
            return db.Database.SqlQuery<int>(sqlquery,
                 new MySqlParameter("1", data.mt_no),
                 new MySqlParameter("2", data.mt_cd),
                 new MySqlParameter("3", data.mapping_dt),
                 new MySqlParameter("4", data.bb_no),
                 new MySqlParameter("5", data.remark),
                 new MySqlParameter("6", data.use_yn),
                 new MySqlParameter("7", data.reg_id),
                 new MySqlParameter("8", data.chg_id),
                 new MySqlParameter("9", data.reg_dt)
                 ).FirstOrDefault();
        }

        public int CheckStaffShiftForId(int? id_actual, int psid)
        {
            string QuerySQL = "SELECT COUNT(psid) FROM d_pro_unit_staff  WHERE id_actual=@1 AND psid= @2 AND (NOW() BETWEEN Date_format(start_dt,'%Y-%m-%d %H:%i:%s') AND Date_format(end_dt,'%Y-%m-%d %H:%i:%s'))";
            return db.Database.SqlQuery<int>(QuerySQL,
                new MySqlParameter("1", id_actual),
                new MySqlParameter("2", psid)
                ).FirstOrDefault();
        }
        public int CallSPCHIATIMS(int gr_qty, int number_dv, string mt_cd, string bb_no)
        {
            string sqlquery = @"CALL chia_tims(@1,@2,@3,@4)";
            return db.Database.ExecuteSqlCommand(sqlquery,
                new MySqlParameter("1", gr_qty),
                new MySqlParameter("2", number_dv),
                new MySqlParameter("3", mt_cd),
                new MySqlParameter("4", bb_no)
                );
        }
        public bool CheckWMaterialMapForDV(string mt_cd)
        {
            string QuerySQL = "SELECT wmmid FROM w_material_mapping WHERE mt_cd like @1";
            return String.IsNullOrWhiteSpace(db.Database.SqlQuery<string>(QuerySQL, new MySqlParameter("1", mt_cd + "-DV%")).FirstOrDefault());
        }

        public int CheckWMaterialMappingForRedo(string mt_cd)
        {
            string sqlquery = @"SELECT COUNT(*) FROM w_material_mapping WHERE mt_cd !=@1 AND mt_lot IN (SELECT mt_cd FROM w_material_info WHERE mt_cd LIKE @2)";
            return db.Database.SqlQuery<int>(sqlquery,
                new MySqlParameter("1", mt_cd),
                new MySqlParameter("2", "%" + mt_cd + "-DV" + "%")
                ).FirstOrDefault();
        }
        public IEnumerable<WActualPrimary> GetActualPrimary(string product, string product_name, string model, string at_no, string regstart, string regend)
        {
            string sql1 = @"SELECT a.id_actualpr,a.at_no,a.`type`,(0)totalTarget,a.target,a.product,a.remark,0 process_count,0 actual,  subquery.at_no AS poRun,
                    b.md_cd  AS md_cd,b.style_nm  AS style_nm
                    FROM w_actual_primary AS a
                    LEFT JOIN d_style_info b ON a.product = b.style_no
                    left JOIN
                    (	SELECT  b.at_no  FROM d_pro_unit_staff AS a JOIN w_actual AS b ON a.id_actual = b.id_actual JOIN w_actual_primary AS c ON b.at_no = c.at_no WHERE
                       (NOW() BETWEEN DATE_FORMAT(a.start_dt,'%Y-%m-%d %H:%i:%s') AND  DATE_FORMAT(a.end_dt,'%Y-%m-%d %H:%i:%s')) AND  c.finish_yn ='N'	   AND b.`type` ='TIMS'  GROUP BY b.at_no) AS subquery 	ON subquery.at_no = a.at_no
                      WHERE a.finish_yn IN  ('N','Y') 
                     AND (@1 ='' OR @1 is null OR  a.product like @2 )
                     AND (@3 ='' OR @3 is null OR b.style_nm like @4 )
                     AND (@5 ='' OR @5 is null OR b.md_cd like @6 )
                     AND (@7 ='' OR @7 is null OR a.at_no like @8 )
                     AND (  (@9 ='' AND @10='' ) OR  (DATE_FORMAT( a.reg_dt, '%Y/%m/%d' ) BETWEEN DATE_FORMAT( @9, '%Y/%m/%d' ) AND DATE_FORMAT( @10, '%Y/%m/%d' )) )
                     Order By a.id_actualpr desc
                    ";
            return db.Database.SqlQuery<WActualPrimary>(sql1,
                new MySqlParameter("1", product != null ? product : ""),
                new MySqlParameter("2", "%" + product + "%"),
                new MySqlParameter("3", product_name != null ? product_name : ""),
                new MySqlParameter("4", "%" + product_name + "%"),
                new MySqlParameter("5", model != null ? model : ""),
                new MySqlParameter("6", "%" + model + "%"),
                new MySqlParameter("7", at_no != null ? at_no : ""),
                new MySqlParameter("8", "%" + at_no + "%"),
                new MySqlParameter("9", regstart != null ? regstart : String.Empty),
                new MySqlParameter("10", regend != null ? regend : String.Empty));
        }
        public IEnumerable<WActualPrimaryAPI> GetDatawActualPrimarySP(string product, string product_name, string model, string at_no, string regstart, string regend)
        {
            string sqlquery = @"Call GetData_wActual_Primary(@1,@2,@3,@4,@5,@6,'TIMS');";
            return db.Database.SqlQuery<WActualPrimaryAPI>(sqlquery, new MySqlParameter("1", product == null ? "" : product), new MySqlParameter("2", product_name == null ? "" : product_name), new MySqlParameter("3", model == null ? "" : model), new MySqlParameter("4", at_no == null ? "" : at_no)
                , new MySqlParameter("5", regstart == null ? "" : regstart), new MySqlParameter("6", regend == null ? "" : regend));
        }
        public IEnumerable<WActualAPI> GetTIMSActualInfoBySP(string at_no)
        {
            string sql = @"CALL spTIMS_GetData_WActual(@1); ";
            return db.Database.SqlQuery<WActualAPI>(sql, new MySqlParameter("1", at_no));
        }
        public IEnumerable<DProUnitStaffAPI> GetTIMSListStaff(int id_actual, string staff_id, string StartDate, string EndDate)
        {
            //     string sql1 = @" SELECT a.psid, a.staff_id,a.actual ActualQty,a.use_yn,a.staff_tp,(SELECT uname FROM mb_info WHERE userid=a.staff_id)uname, (a.defect)Defective,
            //          DATE_FORMAT(CAST(a.reg_dt AS datetime),'%Y-%m-%d %H:%i:%s') AS reg_dt,
            //             DATE_FORMAT(CAST(a.chg_dt AS datetime),'%Y-%m-%d %H:%i:%s') AS chg_dt,
            //                         DATE_FORMAT(CAST(a.start_dt AS datetime),'%Y-%m-%d %H:%i:%s') AS start_dt,
            //                   concat(substring(a.end_dt,1,4),'-', substring(a.end_dt,5,2),'-', substring(a.end_dt,7,2),' ', substring(a.end_dt,9,2),':', substring(a.end_dt,11,2),':', substring(a.end_dt,13,2)) end_dt,(case when NOW() BETWEEN  DATE_FORMAT(a.start_dt, '%Y-%m-%d %H:%i:%s') AND DATE_FORMAT(a.end_dt, '%Y-%m-%d %H:%i:%s') then 'HT' ELSE 'QK' END)het_ca FROM d_pro_unit_staff AS a WHERE a.id_actual= @1 
            //                     AND (@2='' OR a.staff_id LIKE @3 )
            //                     AND (@4='' OR DATE_FORMAT(a.start_dt,'%Y/%m/%d') >= DATE_FORMAT(@5,'%Y/%m/%d') )
            //                     AND (@6='' OR DATE_FORMAT(a.end_dt,'%Y/%m/%d') <= DATE_FORMAT(@7,'%Y/%m/%d') )
            //                     order by a.psid desc
            //             ";
            string sql2 = @"SELECT
	                        w.at_no,
	                        a.psid,
	                        a.staff_id,
	                        a.actual ActualQty,
	                        a.use_yn,
	                        a.staff_tp,
	                        b.uname uname,
	                        ( a.defect ) Defective,
	                        DATE_FORMAT( CAST( a.reg_dt AS datetime ), '%Y-%m-%d %H:%i:%s' ) AS reg_dt,
	                        DATE_FORMAT( CAST( a.chg_dt AS datetime ), '%Y-%m-%d %H:%i:%s' ) AS chg_dt,
	                        DATE_FORMAT( CAST( a.start_dt AS datetime ), '%Y-%m-%d %H:%i:%s' ) AS start_dt,
	                        DATE_FORMAT( CAST( a.end_dt AS datetime ), '%Y-%m-%d %H:%i:%s' ) AS end_dt,
	                        (CASE
			                        WHEN NOW() BETWEEN DATE_FORMAT( a.start_dt, '%Y-%m-%d %H:%i:%s' ) AND DATE_FORMAT( a.end_dt, '%Y-%m-%d %H:%i:%s' ) 
			                        THEN 'HT' 
			                        ELSE 'QK' 
		                        END ) het_ca 
                        FROM
	                        d_pro_unit_staff AS a
	                        LEFT JOIN w_actual w ON w.id_actual = a.id_actual 
	                        LEFT JOIN mb_info b ON b.userid = a.staff_id 
                        WHERE a.id_actual= @1
                                AND (@2='' OR a.staff_id LIKE @3 )
                                AND (@4='' OR DATE_FORMAT(a.start_dt,'%Y/%m/%d') >= DATE_FORMAT(@5,'%Y/%m/%d') )
                                AND (@6='' OR DATE_FORMAT(a.end_dt,'%Y/%m/%d') <= DATE_FORMAT(@7,'%Y/%m/%d') )
                                order by a.end_dt desc , a.psid desc";
            //            string sql3 = @"
            //                    SELECT  w.at_no, a.psid, a.staff_id, a.actual ActualQty,abc.actual_cn,abc.actual_cd,
            //                     a.use_yn, a.staff_tp,
            //                    (SELECT uname FROM mb_info WHERE userid = a.staff_id) uname,
            // DATE_FORMAT(CAST(a.reg_dt AS datetime),'%Y-%m-%d %H:%i:%s') AS reg_dt,
            //                    DATE_FORMAT(CAST(a.chg_dt AS datetime),'%Y-%m-%d %H:%i:%s') AS chg_dt,
            //                                DATE_FORMAT(CAST(a.start_dt AS datetime),'%Y-%m-%d %H:%i:%s') AS start_dt,
            //							DATE_FORMAT(CAST(a.end_dt AS datetime),'%Y-%m-%d %H:%i:%s') AS  end_dt,
            //                        (a.defect) Defective,    
            //                        (CASE   WHEN NOW() BETWEEN DATE_FORMAT(a.start_dt, '%Y-%m-%d %H:%i:%s') AND DATE_FORMAT(a.end_dt, '%Y-%m-%d %H:%i:%s') THEN 'HT'
            //                            ELSE 'QK'
            //                        END) het_ca
            //                    FROM
            //                        d_pro_unit_staff AS a    LEFT JOIN    w_actual w ON w.id_actual = a.id_actual
            //	                    LEFT JOIN
            //                        (SELECT 
            //			                    staff_id,
            //                                gr_qty,
            //                                real_qty,
            //                                 (
            //		                              CASE
            //		                                 WHEN 8<= DATE_FORMAT(w.reg_dt, '%H') AND DATE_FORMAT(w.reg_dt, '%H')<=20 
            //                                           THEN  'CN' ELSE 'CD'  END
            //                                 ) as ShiftName,
            //                                (CASE
            //                                    WHEN  8 <= DATE_FORMAT(w.reg_dt, '%H')
            //                                          AND DATE_FORMAT(w.reg_dt, '%H') <= 20
            //                                    THEN  SUM(w.gr_qty)
            //                                END) AS actual_cn,
            //            (CASE
            //                WHEN  8 > DATE_FORMAT(w.reg_dt, '%H') OR DATE_FORMAT(w.reg_dt, '%H') > 20
            //                THEN  SUM(w.gr_qty)
            //            END) AS actual_cd
            //			FROM
            //			w_material_info w
            //			WHERE
            //				mt_sts_cd != '003' 
            //                    And w.id_actual = @1
            //			GROUP BY staff_id
            //      ) abc ON abc.staff_id = a.staff_id  AND a.actual is not null
            //WHERE
            //    a.id_actual = @1
            //        AND (@2='' OR a.staff_id LIKE @3 )
            //        AND (@4='' OR DATE_FORMAT(a.start_dt,'%Y/%m/%d') >= DATE_FORMAT(@5,'%Y/%m/%d') )
            //                            AND (@6='' OR DATE_FORMAT(a.end_dt,'%Y/%m/%d') <= DATE_FORMAT(@7,'%Y/%m/%d') )
            //                            order by a.end_dt desc , a.psid desc";
            return db.Database.SqlQuery<DProUnitStaffAPI>(sql2,
                new MySqlParameter("1", id_actual),
                new MySqlParameter("2", staff_id),
                new MySqlParameter("3", "%" + staff_id + "%"),
                new MySqlParameter("4", StartDate),
                new MySqlParameter("5", StartDate),
                new MySqlParameter("6", EndDate),
                new MySqlParameter("7", EndDate));
        }
        public IEnumerable<DProUnitStaffAPI> GetTIMSListStaffOQC(int id_actual, string staff_id, string StartDate, string EndDate)
        {
            string sql3 = @"
                    SELECT  w.at_no, a.psid, a.staff_id, a.actual ActualQty,abc.actual_cn,abc.actual_cd,
                     a.use_yn, a.staff_tp,
                    (SELECT uname FROM mb_info WHERE userid = a.staff_id) uname,
 DATE_FORMAT(CAST(a.reg_dt AS datetime),'%Y-%m-%d %H:%i:%s') AS reg_dt,
                    DATE_FORMAT(CAST(a.chg_dt AS datetime),'%Y-%m-%d %H:%i:%s') AS chg_dt,
                                DATE_FORMAT(CAST(a.start_dt AS datetime),'%Y-%m-%d %H:%i:%s') AS start_dt,
							DATE_FORMAT(CAST(a.end_dt AS datetime),'%Y-%m-%d %H:%i:%s') AS  end_dt,
                        (a.defect) Defective,    
                        (CASE   WHEN NOW() BETWEEN DATE_FORMAT(a.start_dt, '%Y-%m-%d %H:%i:%s') AND DATE_FORMAT(a.end_dt, '%Y-%m-%d %H:%i:%s') THEN 'HT'
                            ELSE 'QK'
                        END) het_ca
                    FROM
                        d_pro_unit_staff AS a    LEFT JOIN    w_actual w ON w.id_actual = a.id_actual
	                    LEFT JOIN
                        (SELECT ANY_VALUE(id_actual_oqc) id_actual_oqc,
			                    staff_id,
                                ANY_VALUE(gr_qty) gr_qty,
                                ANY_VALUE(real_qt) real_qt,
                                 ANY_VALUE(
		                              CASE
		                                 WHEN 8<= DATE_FORMAT(w.reg_dt, '%H') AND DATE_FORMAT(w.reg_dt, '%H')<=20 
                                           THEN  'CN' ELSE 'CD'  END
                                 ) as ShiftName,
                                ANY_VALUE(CASE
                                    WHEN  8 <= DATE_FORMAT(w.reg_dt, '%H')
                                          AND DATE_FORMAT(w.reg_dt, '%H') <= 20
                                    THEN  SUM(w.gr_qty)
                                END) AS actual_cn,
            (CASE
                WHEN  8 > DATE_FORMAT(w.reg_dt, '%H') OR DATE_FORMAT(w.reg_dt, '%H') > 20
                THEN  SUM(w.gr_qty)
            END) AS actual_cd
			FROM
			w_material_info w
			WHERE
				mt_sts_cd != '003' 
                   And w.id_actual_oqc = @1
			GROUP BY staff_id
      ) abc ON abc.id_actual_oqc = a.id_actual AND a.actual is not null
WHERE
    a.id_actual = @1
        AND (@2='' OR a.staff_id LIKE @3 )
        AND (@4='' OR DATE_FORMAT(a.start_dt,'%Y/%m/%d') >= DATE_FORMAT(@5,'%Y/%m/%d') )
                            AND (@6='' OR DATE_FORMAT(a.end_dt,'%Y/%m/%d') <= DATE_FORMAT(@7,'%Y/%m/%d') )
                            order by a.end_dt desc , a.psid desc";
            return db.Database.SqlQuery<DProUnitStaffAPI>(sql3,
                new MySqlParameter("1", id_actual),
                new MySqlParameter("2", staff_id),
                new MySqlParameter("3", "%" + staff_id + "%"),
                new MySqlParameter("4", StartDate),
                new MySqlParameter("5", StartDate),
                new MySqlParameter("6", EndDate),
                new MySqlParameter("7", EndDate));
        }
        public bool CheckExistUserByUserid(string userId)
        {
            string sql = @"SELECT  
                              EXISTS
                             (SELECT * FROM mb_info WHERE userid = @1 )";
            int count = db.Database.SqlQuery<int>(sql, new MySqlParameter("1", userId)).SingleOrDefault();
            if (count == 1)
            {
                return true;
            }
            return false;
        }
        public IEnumerable<WMaterialnfo> GetTIMSActualDetailByStaff(string staff_id, int id_actual, string start_dt, string end_dt)
        {
            string sql1 = @"SELECT 
                                wmtid  as wmtid   ,                            
                                mt_cd  as mt_cd   ,
                                mt_no  as mt_no   ,
                                real_qty  as real_qty ,
                                gr_qty  as gr_qty     ,
                                gr_qty  as gr_qty1    ,
                                mt_qrcode as mt_qrcode ,
                                bb_no  as bb_no    ,
                                mt_barcode as mt_barcode,
                                DATE_FORMAT(chg_dt, '%Y-%m-%d %H:%i:%s') as chg_dt ,
                                DATE_FORMAT(reg_dt, '%Y-%m-%d %H:%i:%s') as date ,
                                (DATE_FORMAT( reg_dt, '%Y/%m/%d %H:%i:%s' )) as reg_dt,
                                (
		                                CASE
		                                WHEN 8<= DATE_FORMAT(w.reg_dt, '%H') AND DATE_FORMAT(w.reg_dt, '%H')<=20 
                                        THEN  'Ca Ngày'
                                        WHEN 20 < DATE_FORMAT(w.reg_dt, '%H') AND DATE_FORMAT(w.reg_dt, '%H')<= 24
                                        THEN 'Ca Đêm'
                                        WHEN 0 <= DATE_FORMAT(w.reg_dt, '%H') AND DATE_FORMAT(w.reg_dt, '%H')< 8
                                        THEN 'Ca Đêm'
                                        ELSE ''  END
                                 ) as ShiftName
                            from w_material_info w
                            WHERE mt_sts_cd != '003'
                            and staff_id = @1 AND id_actual =@2 
                            AND (reg_dt between DATE_FORMAT(CAST(@3 AS datetime), '%Y-%m-%d %H:%i:%s') and DATE_FORMAT(CAST(@4 AS datetime), '%Y-%m-%d %H:%i:%s')) and 
                    mt_cd NOT IN (SELECT mt_cd FROM w_material_info WHERE id_actual=@5 AND orgin_mt_cd IS NOT null and mt_sts_cd!='003'  AND mt_cd LIKE CONCAT(orgin_mt_cd,'-MG%')  UNION SELECT orgin_mt_cd FROM w_material_info WHERE orgin_mt_cd IS NOT NULL
                    AND id_actual=@6 AND mt_cd LIKE CONCAT(orgin_mt_cd,'-DV%') AND mt_sts_cd!='003' AND staff_id=@7)
                    ";
            return db.Database.SqlQuery<WMaterialnfo>(sql1,
                new MySqlParameter("1", staff_id),
                new MySqlParameter("2", id_actual),
                new MySqlParameter("3", start_dt),
                new MySqlParameter("4", end_dt),
                new MySqlParameter("5", id_actual),
                new MySqlParameter("6", id_actual),
                new MySqlParameter("7", staff_id)
                );
        }
        public IEnumerable<w_material_mapping> FindAllMaterialMappingByMtCd(string mt_cd)
        {
            string sql = @"select * from w_material_mapping where mt_cd =@1";
            return db.Database.SqlQuery<w_material_mapping>(sql, new MySqlParameter("1", mt_cd));
        }
        public IEnumerable<MaterialInfoMMS> FindWMaterialInfoByMTQRCode(string mt_qrcode)
        {
            string sql = @"select *
                            from w_material_info
                            where mt_qrcode = @1 ";
            return db.Database.SqlQuery<MaterialInfoMMS>(sql, new MySqlParameter("1", mt_qrcode));
        }
        public int UpdateWMaterialInfoById(string mt_sts_cd, string chg_id, int? id_actual, int wmtid)
        {
            string sql = @"Update w_material_info
                                    set mt_sts_cd = @1,chg_id =@2,id_actual =@3
                                    where wmtid = @4 ";
            return db.Database.ExecuteSqlCommand(sql, new MySqlParameter("@1", mt_sts_cd),
                                                new MySqlParameter("2", chg_id),
                                                new MySqlParameter("3", id_actual),
                                                new MySqlParameter("4", wmtid)
                                                );
        }
        public int UpdateWMaterialInfoInput(double? gr_qty, string mt_sts_cd, int wmtid)
        {
            string sql = @"Update w_material_info
                                    set mt_sts_cd = @1,gr_qty =@2
                                    where wmtid = @3 ";
            return db.Database.ExecuteSqlCommand(sql, new MySqlParameter("@1", mt_sts_cd),
                                                new MySqlParameter("2", gr_qty),
                                                new MySqlParameter("3", wmtid)
                                                );
        }
        public int UpdateFinishTIMS(int remain, string html, string mt_cd, string mt_lot)
        {
            string sql = @"UPDATE w_material_info SET gr_qty= @1 ,mt_sts_cd=@2 
                            WHERE mt_cd =@3 ; CALL Finish_Tims(@4); CALL xa_container(@5) ;
                            UPDATE w_material_mapping SET use_yn='Y' where mt_lot=@6 and mt_cd=@7 ;
                            ";
            return db.Database.ExecuteSqlCommand(sql,
                new MySqlParameter("1", remain),
                new MySqlParameter("2", html),
                new MySqlParameter("3", mt_cd),
                new MySqlParameter("4", mt_cd),
                new MySqlParameter("5", mt_cd),
                new MySqlParameter("6", mt_lot),
                new MySqlParameter("7", mt_cd));
        }

        public int SumGrqtyDSWMaterialDV(string mt_cd)
        {
            string sqlquery = @"SELECT CASE
						WHEN SUM(gr_qty) > 0 THEN SUM(gr_qty)
						   ELSE 0
					     END as tinhtong
                        FROM w_material_info  WHERE mt_cd LIKE @1 ";
            return db.Database.SqlQuery<int>(sqlquery, new MySqlParameter("@1", mt_cd + "-DV%")).FirstOrDefault();
        }
        public IEnumerable<WMaterialMapping> GetDatasMaterialMapping(string mt_cd)
        {
            string sql = @"SELECT a.wmmid as wmmid,a.mt_lot as mt_lot,a.mt_cd as mt_cd,DATE_FORMAT(a.mapping_dt, '%Y-%m-%d %H:%i:%s') as mapping_dt
                                     ,a.use_yn as use_yn ,a.reg_dt as reg_dt ,b.gr_qty as gr_qty ,b.mt_no as mt_no,b.bb_no as bb_no,
                                     (CASE WHEN a.use_yn='N' THEN b.gr_qty ELSE 0 END) as Used,
                                     (CASE WHEN a.use_yn='N' THEN Find_Remain(a.mt_cd,b.orgin_mt_cd,a.reg_dt) ELSE 0 END) as Remain 
                                     FROM w_material_mapping AS a JOIN w_material_info AS b ON a.mt_cd=b.mt_cd
                                     WHERE a.mt_lot= @1 and a.mapping_dt like @2
                                     ORDER BY a.use_yn='Y' DESC,a.reg_dt DESC";
            return db.Database.SqlQuery<WMaterialMapping>(sql, new MySqlParameter("1", mt_cd),
                new MySqlParameter("2", DateTime.Now.ToString("yyyyMMddHHmm") + "%"));
        }
        public int UpdateReturnstPacking(string wmtid)
        {
            string sqlupdatemtinfo = @" Update w_material_info set mt_sts_cd = '008' , chg_id=@1
                                            Where wmtid =@2 AND mt_sts_cd='009' AND lct_cd like '%006%'";
            int result1 = db.Database.ExecuteSqlCommand(sqlupdatemtinfo, new MySqlParameter("1", (Session["userid"] == null) ? "" : Session["userid"].ToString()),
                    new MySqlParameter("2", wmtid));


            int resultMain = 0;
            if (result1 != 0)
            {
                string sqlupdatewactual = @"update w_actual c
                inner join (select (a.defect+b.gr_qty) as defectup ,a.id_actual
                from w_actual a
                join w_material_info b on a.id_actual = b.id_actual_oqc and 
                b.wmtid =@1 AND b.mt_sts_cd='009' AND b.lct_cd like '%006%') d on c.id_actual=d.id_actual
                set c.defect=d.defectup";
                resultMain = db.Database.ExecuteSqlCommand(sqlupdatewactual, new MySqlParameter("1", wmtid));
            }
            return resultMain;
        }

        public void UpdateWMaterialQtyForRedo(double? gr_qty, string mt_cd)
        {
            try
            {
                string sqlquery = @"UPDATE w_material_info SET mt_sts_cd='002',gr_qty=@1  WHERE mt_cd=@2";

                db.Database.ExecuteSqlCommand(sqlquery,
                    new MySqlParameter("1", gr_qty),
                    new MySqlParameter("2", mt_cd));

            }
            catch (Exception)
            {

                //throw;
            }

        }
        public IEnumerable<WMaterialnfoDV> DSWMaterialDV(string mt_cd)
        {
            string sqlquery = @"SELECT Bobbin.bno, BobbinHis.blno
            FROM w_material_info AS Material
            JOIN d_bobbin_info AS Bobbin ON Material.bb_no = Bobbin.bb_no
            JOIN d_bobbin_lct_hist AS BobbinHis  ON Material.bb_no = BobbinHis.bb_no
            WHERE Material.mt_cd LIKE @1 ";
            return db.Database.SqlQuery<WMaterialnfoDV>(sqlquery, new MySqlParameter("1", mt_cd + "-DV%"));
        }
        public void DeleteWMaterialQtyForRedo(string mt_cd)
        {
            string sqlquery = @"DELETE FROM w_material_info  WHERE mt_cd LIKE CONCAT(@1,'-DV%'); ";

            db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", mt_cd));
        }

        public void DeleteWMaterialMappingForRedo(string mt_cd)
        {
            string sqlquery = @"DELETE FROM w_material_mapping  WHERE mt_cd = @1; ";
            db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", mt_cd));
        }

        public void DeleteBoBbinHisForRedo(string blno)
        {
            string sqlquery = @"DELETE FROM d_bobbin_lct_hist  WHERE  FIND_IN_SET(blno, @1); ";
            db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", blno));

        }

        public void UpdateBobbinInfoForRedo(string bno)
        {
            string sqlquery = @"UPDATE d_bobbin_info SET  mt_cd=NULL  WHERE  FIND_IN_SET(bno, @1);";
            db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", bno));

        }

        public int CallSPkhoiphucbobin(string mt_cd, string bb_no)
        {
            string sqlquery = @"CALL khoiphucbobin(@1,@2)";
            return db.Database.ExecuteSqlCommand(sqlquery,
                new MySqlParameter("1", mt_cd),
                new MySqlParameter("2", bb_no)
                );
        }

        public int UpdateUseYnMaterialMappingByMTCd(string useyn, string chg_id, int wmmid)
        {
            string QuerySQL = @"Update w_material_mapping 
                                                    set use_yn = @1 , 
                                                        chg_id = @2
                                                   where wmmid = @3 AND use_yn='Y' ";
            return db.Database.ExecuteSqlCommand(QuerySQL,
                new MySqlParameter("1", useyn),
                new MySqlParameter("2", chg_id),
                new MySqlParameter("3", wmmid)
                );
        }
        public IEnumerable<TimsReceivingScanModel> GetListMaterialTimsReceivingPO(string po_no, string product, string input_dt, string bb_no)
        {
            string sql2 = @"SELECT wmtid,id_actual,mt_cd,bb_no,gr_qty,from_lct_cd, 
                            lct_sts_cd,mt_sts_cd,
                            DATE_FORMAT(input_dt , '%Y-%m-%d %H:%i:%s') as input_dt
                            ,at_no,product,sts_nm,mt_type_nm,from_lct_nm
                            FROM view_tims_receiving_scan where (@po_no='' OR at_no LIKE '%'@po_no'%')
                            and (@bb_no='' OR bb_no LIKE '%'@bb_no'%') and (@product='' OR product LIKE '%'@product'%')
                            and (@input_dt='' OR DATE_FORMAT(@input_dt, '%Y-%m-%d') like '%'@input_dt'%');
                        ";
            return db.Database.SqlQuery<TimsReceivingScanModel>(sql2, new MySqlParameter("@po_no", po_no ?? ""),
                                                                       new MySqlParameter("@bb_no", bb_no ?? ""),
                                                                       new MySqlParameter("@product", product ?? ""),
                                                                       new MySqlParameter("@input_dt", input_dt ?? ""));
        }
        public MaterialInfoMMS FindOneMaterialInfoByMTCdBBNo(string mt_cd, string bb_no)
        {
            string sql = @"Select * from w_material_info where mt_cd =@1 AND bb_no=@2";
            return db.Database.SqlQuery<MaterialInfoMMS>(sql, new MySqlParameter("1", mt_cd),
                                                                        new MySqlParameter("2", bb_no)).SingleOrDefault();
        }
        public IEnumerable<TimsReceivingScanModel> FindAllTimsReceivingScanByInBBNo(string bb_no)
        {
            string sql = @"SELECT 
                                            wmtid       as wmtid      ,
                                            id_actual   as id_actual  ,
                                            mt_cd       as mt_cd      ,
                                            bb_no       as bb_no      ,
                                            gr_qty      as gr_qty     ,
                                            from_lct_cd as from_lct_cd,
                                            lct_sts_cd  as lct_sts_cd ,
                                            mt_sts_cd   as mt_sts_cd  ,
                                            at_no       as at_no      ,
                                            product     as product    ,
                                            sts_nm      as sts_nm     ,
                                            mt_type_nm  as mt_type_nm ,
                                            from_lct_nm as from_lct_nm,
                                            input_dt    as input_dt   
                        FROM view_tims_receiving_scan WHERE bb_no =@1";
            return db.Database.SqlQuery<TimsReceivingScanModel>(sql, new MySqlParameter("1", bb_no));
        }
        public int UpdateMTQR_RDList(string full_date, string user, string bb_no)
        {
            //var sql2 = new StringBuilder();
            //sql2.Append(" UPDATE  w_material_info w ")
            //.Append("  SET  w_material_info.mt_sts_cd = '008', ")
            //.Append("   w_material_info.lct_cd = '006000000000000000' , w_material_info.recevice_dt_tims = '" + full_date + "' ,")
            //.Append("   w_material_info.to_lct_cd = '006000000000000000' ,w_material_info.chg_id = '" + user + "'  ,w_material_info.chg_dt = '" + full_date + "'  ")
            //.Append(" WHERE wmtid in (SELECT wmtid FROM view_tims_receiving_scan WHERE bb_no = '" + bb_no + "' ) ");
            string sql = @"UPDATE w_material_info w SET w.mt_sts_cd = '008', w.lct_cd = '006000000000000000' , w.recevice_dt_tims = @1, 
                w.to_lct_cd = '006000000000000000' ,w.chg_id =@2  ,w.chg_dt =@3
                WHERE wmtid in (SELECT wmtid FROM view_tims_receiving_scan WHERE bb_no =@4 )";
            return db.Database.ExecuteSqlCommand(sql, new MySqlParameter("1", full_date),
                new MySqlParameter("2", user),
                new MySqlParameter("3", full_date),
                new MySqlParameter("4", bb_no)
                );
        }
        public bool CheckExistDuplicateTime(string staff_id, string start, string end, int psid)
        {
            string sql = @" SELECT  
                           EXISTS (      SELECT psid FROM d_pro_unit_staff AS a 
                                         WHERE a.staff_id = @1, AND ( DATE_FORMAT(a.start_dt,'%Y-%m-%d %H:%i:%s') >= DATE_FORMAT(@2, '%Y-%m-%d %H:%i:%s') )
                                                   AND( DATE_FORMAT(a.end_dt,'%Y-%m-%d %H:%i:%s') <= DATE_FORMAT(@3, '%Y-%m-%d %H:%i:%s') AND a.psid != @4 )        )
                        ";

            int count = db.Database.SqlQuery<int>(sql).SingleOrDefault();
            return count == 1;
        }
        public DProUnitStaffAPI FindOneDProUnitStaffReturnByPsid(int psid)
        {
            string sql = @"SELECT a.psid,a.staff_id,a.staff_tp,a.id_actual
                            ,b.dt_nm AS staff_tp_nm,
                           c.uname, DATE_FORMAT(a.start_dt,'%Y-%m-%d %H:%i:%s') AS start_dt,
                                    DATE_FORMAT(a.end_dt  ,'%Y-%m-%d %H:%i:%s') AS end_dt,
                            a.use_yn
                FROM d_pro_unit_staff a
                LEFT JOIN comm_dt b ON  b.dt_cd=a.staff_tp AND b.mt_cd='COM013' AND b.use_yn='Y'
                LEFT JOIN mb_info c ON c.userid=a.staff_id
                where a.psid= @1 ";

            return db.Database.SqlQuery<DProUnitStaffAPI>(sql, new MySqlParameter("1", psid)).SingleOrDefault();
        }
        public int InsertDProUnitStaff(d_pro_unit_staff a)
        {
            string QuerySQL = @"Insert Into d_pro_unit_staff(start_dt,end_dt,staff_id,use_yn,del_yn,chg_dt,reg_dt,id_actual) 
                                    values(@1, @2, @3, @4, @5, @6,@7,@8); SELECT LAST_INSERT_ID();";
            return db.Database.SqlQuery<int>(QuerySQL, new MySqlParameter("1", a.start_dt),
                                                      new MySqlParameter("2", a.end_dt),
                                                      new MySqlParameter("3", a.staff_id),
                                                      new MySqlParameter("4", a.use_yn),
                                                      new MySqlParameter("5", a.del_yn),
                                                      new MySqlParameter("6", a.chg_dt),
                                                      new MySqlParameter("7", a.reg_dt), new MySqlParameter("8", a.id_actual)).FirstOrDefault();
        }
        public IEnumerable<DProUnitStaffAPI> FindDProUnitStaffByStaffIdIdActual(int? id_actual, string staff_id)
        {
            string sql = @"SELECT 
                            psid,staff_id,actual,defect,id_actual,staff_tp,start_dt,end_dt,
                            use_yn,del_yn,reg_id,reg_dt,chg_id,chg_dt
                            FROM d_pro_unit_staff
                            Where id_actual = @1 AND staff_id= @2 ;";
            return db.Database.SqlQuery<DProUnitStaffAPI>(sql, new MySqlParameter("1", id_actual), new MySqlParameter("2", staff_id));
        }
        public int UpdateWMaterialInfoById(int? id_actual_oqc, string staff_id_oqc, string mt_sts_cd, string chg_id, int wmtid)
        {
            string sqlUpdate = @"Update w_material_info SET id_actual_oqc=@1 ,
                                                            staff_id_oqc=@2,
                                                            mt_sts_cd=@3,
                                                            chg_id=@4 
                                    where 
                            mt_sts_cd <> '009' AND lct_cd like '006%' AND wmtid=@5 ";
            return db.Database.ExecuteSqlCommand(sqlUpdate, new MySqlParameter("1", id_actual_oqc),
                                                      new MySqlParameter("2", staff_id_oqc),
                                                      new MySqlParameter("3", mt_sts_cd),
                                                      new MySqlParameter("4", chg_id),
                                                      new MySqlParameter("5", wmtid)
                                                      );
        }
        public int CheckMaterialMapping(string mt_cd, string mt_lot)
        {
            string sqlcheckmapping = @"SELECT COUNT(*) FROM w_material_mapping a 
            WHERE a.mt_cd = @1 AND a.mt_lot !=@2 AND  a.mapping_dt >(SELECT b.mapping_dt FROM w_material_mapping b WHERE b.mt_cd =@1 AND b.mt_lot = @2 ORDER BY b.mapping_dt DESC)";
            return db.Database.SqlQuery<int>(sqlcheckmapping, new MySqlParameter("@1", mt_cd), new MySqlParameter("@2", mt_lot)).FirstOrDefault();
        }
        public w_actual_primary GetwactualprimaryFratno(string at_no)
        {
            string sqlquery = @"SELECT * FROM w_actual_primary WHERE at_no=@1";
            return db.Database.SqlQuery<w_actual_primary>(sqlquery, new MySqlParameter("@1", at_no)).FirstOrDefault();
        }
        public IEnumerable<m_facline_qc> Getmfaclineqc(string ml_no, string fq_no, string ml_tims)
        {
            string sqlquery = @"SELECT * FROM m_facline_qc WHERE ml_no=@1 AND fq_no LIKE @2 AND ml_tims=@3";
            return db.Database.SqlQuery<m_facline_qc>(sqlquery, new MySqlParameter("@1", ml_no), new MySqlParameter("@2", "%" + fq_no + "%"),
                new MySqlParameter("@3", ml_tims));
        }
        public m_facline_qc GetmfaclineqcSearch(string fq_no)
        {
            string sqlquery = @"SELECT * FROM m_facline_qc WHERE  fq_no LIKE @1  ORDER BY fq_no ASC";
            return db.Database.SqlQuery<m_facline_qc>(sqlquery, new MySqlParameter("@1", "%" + fq_no + "%")).FirstOrDefault();
        }
        public void UpdateMaterialmappinguseyn(string mt_lot, string mt_cd, string use_yn)
        {
            string sql = @"UPDATE w_material_mapping SET use_yn=@3 where mt_lot=@1 and mt_cd=@2;";
            db.Database.ExecuteSqlCommand(sql, new MySqlParameter("@1", mt_lot), new MySqlParameter("@2", mt_cd), new MySqlParameter("@3", use_yn));
        }
        public void UpdateGrqtyMaterialInfo(string mt_cd, int gr_qty, string mt_sts_cd)
        {
            string sql = @"UPDATE w_material_info SET gr_qty=@1,mt_sts_cd=@3 WHERE mt_cd=@2";
            db.Database.ExecuteSqlCommand(sql, new MySqlParameter("@1", gr_qty), new MySqlParameter("@2", mt_cd), new MySqlParameter("@3", mt_sts_cd));
        }
        public void UpdateMaterialMappingUseyn(string mt_cd, string use_yn)
        {
            string sql = @"UPDATE w_material_mapping SET use_yn=@2 where mt_cd=@1;";
            db.Database.ExecuteSqlCommand(sql, new MySqlParameter("@1", mt_cd), new MySqlParameter("@2", use_yn));
        }
        public int CheckGrqtyMaterialInfo(string mt_cd, string mt_sts_cd)
        {
            string sql = "SELECT COUNT(wmtid) FROM w_material_info WHERE mt_cd=@1 AND mt_sts_cd=@2 AND gr_qty<=0";
            return db.Database.SqlQuery<int>(sql, new MySqlParameter("@1", mt_cd), new MySqlParameter("@2", mt_sts_cd)).FirstOrDefault();
        }
        public void DeleteDBobbinLctHistforDevice(string mt_cd)
        {
            string sqlquery = @"DELETE FROM d_bobbin_lct_hist WHERE  mt_cd=@1";
            db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", mt_cd));
        }
        public void UpdateBobbinInfowithmtcd(string mt_cd)
        {
            string sqlquery = "UPDATE d_bobbin_info SET mt_cd=NULL WHERE  mt_cd=@1";
            db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", mt_cd));
        }
        public int GetDefactActual(int id_actual)
        {
            string sql = @"SELECT SUM(k.check_qty-k.ok_qty)
            FROM m_facline_qc AS k 
            WHERE k.ml_tims IN (

            SELECT l.mt_cd FROM w_material_info AS l 
            WHERE  l.id_actual=@1)";
            return db.Database.SqlQuery<int>(sql, new MySqlParameter("@1", id_actual)).FirstOrDefault();
        }
        public int GetSumQtyFacline(string ml_tims)
        {
            string sql = @"SELECT SUM(b.ok_qty) AS qty FROM m_facline_qc b WHERE b.ml_tims=@1 GROUP BY b.ml_tims";
            return db.Database.SqlQuery<int>(sql, new MySqlParameter("@1", ml_tims)).FirstOrDefault();
        }
        public qc_item_mt Getqcitemmt(string item_vcd)
        {
            string sql = @"SELECT * FROM qc_item_mt WHERE item_vcd=@1";
            return db.Database.SqlQuery<qc_item_mt>(sql, new MySqlParameter("@1", item_vcd)).FirstOrDefault();
        }
        public int ChangeBobinTimsDv(string bb_no, int wmtid, string bb_no2, string mt_cd)
        {
            string sqlsp = @" CALL change_bobin_tims_dv(@1,@2,@3,@4) ";
            return db.Database.ExecuteSqlCommand(sqlsp,
                new MySqlParameter("@1", bb_no),
                new MySqlParameter("@2", wmtid),
                new MySqlParameter("@3", bb_no2),
                new MySqlParameter("@4", mt_cd));
        }
        public IEnumerable<w_material_mapping> FindOneWMaterialMappingByIdActualAndStaffIdOQC(int id_actual, string staff_id_oqc)
        {
            string sql_w_material_mapping = @"Select * from w_material_mapping i inner join w_material_info j ON i.mt_lot = j.mt_cd
                                                where j.id_actual = @1 AND j.staff_id_oqc = @2 Order by i.reg_dt desc ";
            return db.Database.SqlQuery<w_material_mapping>(sql_w_material_mapping,
                                                               new MySqlParameter("1", id_actual),
                                                               new MySqlParameter("2", staff_id_oqc));
        }
        public int DeleteDProUnitStaff(int? id_actual, int psid, string staff_id)
        {
            string sql = @"Delete from d_pro_unit_staff where id_actual =@1 AND psid =@2 AND staff_id =@3;";
            return db.Database.ExecuteSqlCommand(sql, new MySqlParameter("1", id_actual),
                                                  new MySqlParameter("2", psid),
                                                  new MySqlParameter("3", staff_id));
        }
        public IEnumerable<WMaterialnfo> FindAllWMaterialInfoByStaffIdOQC(string staff_id_oqc, int id_actual_oqc)
        {
            string sql = @"SELECT 
                                wmtid  as wmtid   ,
                                reg_dt as date    ,
                                mt_cd  as mt_cd   ,
                                mt_no  as mt_no   ,
                                real_qty  as real_qty ,
                                gr_qty  as gr_qty     ,
                                gr_qty  as gr_qty1    ,
                                mt_qrcode as mt_qrcode ,
                                bb_no  as bb_no    ,
                                mt_barcode as mt_barcode,
                                chg_dt   as chg_dt 
                            from w_material_info
                            Where
                             id_actual_oqc = @1 AND staff_id_oqc=@2 
                            Order by reg_dt desc";
            return db.Database.SqlQuery<WMaterialnfo>(sql, new MySqlParameter("1", id_actual_oqc),
                                                                new MySqlParameter("2", staff_id_oqc));
        }
        public int UpdateDefectActual(double? defect, string chg_id, int id_actual)
        {
            string sql = @"Update w_actual SET defect=@1, chg_id=@2 Where  id_actual=@3 ";
            return db.Database.ExecuteSqlCommand(sql, new MySqlParameter("1", defect),
                                                     new MySqlParameter("2", chg_id),
                                                     new MySqlParameter("3", id_actual)
                                                     );
        }
        public IEnumerable<WMaterialnfo> FindDetailActualStaffOQC(string staff_id_oqc, int id_actual, string end_dt, string start_dt)
        {
            string sql = @"SELECT 
                                wmtid  as wmtid   ,
                                reg_dt as date    ,
                                mt_cd  as mt_cd   ,
                                mt_no  as mt_no   ,
                                real_qty  as real_qty ,
                                gr_qty  as gr_qty     ,
                                gr_qty  as gr_qty1    ,
                                mt_qrcode as mt_qrcode ,
                                bb_no  as bb_no    ,
                                mt_barcode as mt_barcode,
                                chg_dt   as chg_dt ,
                                (DATE_FORMAT( reg_dt, '%Y/%m/%d %H:%i:%s' )) as reg_dt,
                                (
		                                CASE
		                                WHEN 8<= DATE_FORMAT(w.reg_dt, '%H') AND DATE_FORMAT(w.reg_dt, '%H')<=20 
                                        THEN  'Ca Ngày'
                                        WHEN 20 < DATE_FORMAT(w.reg_dt, '%H') AND DATE_FORMAT(w.reg_dt, '%H')<= 24
                                        THEN 'Ca Đêm'
                                        WHEN 0 <= DATE_FORMAT(w.reg_dt, '%H') AND DATE_FORMAT(w.reg_dt, '%H')< 8
                                        THEN 'Ca Đêm'
                                        ELSE ''  END
                                 ) as ShiftName
                            from w_material_info w
                            WHERE mt_sts_cd != '003' 
                            AND w.end_production_dt  > DATE_FORMAT(CAST( @4 AS datetime ), '%Y-%m-%d %H:%i:%s')
					        AND w.end_production_dt  < DATE_FORMAT(CAST( @3 AS datetime ), '%Y-%m-%d %H:%i:%s')
                            and staff_id_oqc = @1 AND id_actual_oqc =@2  
                           /*AND
                            (SELECT k.reg_dt FROM w_material_mapping AS k  WHERE k.mt_lot IS NULL AND mt_cd=k.mt_cd LIMIT 1) < DATE_FORMAT(@3, '%Y-%m-%d %H:%i:%s')*/
                             Order by reg_dt desc";
            return db.Database.SqlQuery<WMaterialnfo>(sql, new MySqlParameter("1", staff_id_oqc),
                                                                new MySqlParameter("2", id_actual),
                                                                new MySqlParameter("3", end_dt),
                                                                new MySqlParameter("4", start_dt));
        }

        public int GetOneDBWithBuyer(string bb_no)
        {

            string sql = @"SELECT COUNT(wmtid) from w_material_info where bb_no = @1 AND buyer_qr IS NOT NULL  ORDER BY wmtid DESC  limit 1";
            return db.Database.SqlQuery<int>(sql, new MySqlParameter("@1", bb_no)).FirstOrDefault();


        }
        public string GetUname(string userid)
        {
            string sql = @"SELECT uname FROM mb_info WHERE userid=@1";
            return db.Database.SqlQuery<string>(sql, new MySqlParameter("@1", userid)).FirstOrDefault();
        }
        public IEnumerable<WMaterialMapping> FindAllWMaterialMappingByMtcdUseYn(string mt_cd, string use_yn)
        {
            string getvalue = @"select 
                                        wmmid  ,
                                        mt_lot ,
                                        mt_cd  ,
                                        mt_no  ,
                                        mapping_dt,
                                        bb_no     ,
                                        remark   , 
                                        sts_share ,
                                        use_yn    ,
                                        del_yn    ,
                                        reg_id   , 
                                         reg_dt,
                                         chg_id,
                                         chg_dt
                                    from w_material_mapping where mt_cd = @1 AND use_yn =@2
                                    ";
            return db.Database.SqlQuery<WMaterialMapping>(getvalue, new MySqlParameter("1", mt_cd),
                                                                    new MySqlParameter("2", use_yn));
        }
        public IEnumerable<WMaterialnfo> GetDataWActualSX(string at_no)
        {
            //StringBuilder sql = new StringBuilder($"SELECT a.wmtid,a.mt_cd,a.bb_no, \n");
            //sql.Append(" IF(a.gr_qty > 0,a.gr_qty, (SELECT d.gr_qty FROM w_material_mapping AS c JOIN w_material_info AS d ON c.mt_cd = d.mt_cd WHERE c.mt_lot =a.mt_cd  LIMIT 1)) AS gr_qty, ");
            //sql.Append(" IF(a.real_qty > 0,a.real_qty,(SELECT d.real_qty FROM w_material_mapping AS c JOIN w_material_info AS d ON c.mt_cd = d.mt_cd WHERE c.mt_lot =a.mt_cd LIMIT 1))  as real_qty,  ");
            //sql.Append("(SELECT dt_nm FROM comm_dt WHERE mt_cd='WHS005' AND dt_cd=a.mt_sts_cd limit 1)mt_sts_cd,a.recevice_dt_tims,b.`type` FROM w_material_info AS a \n");
            //sql.Append("JOIN w_actual AS b ON a.id_actual=b.id_actual \n");
            //sql.Append("WHERE a.mt_sts_cd in('008','002') and b.at_no='" + at_no + "' AND a.lct_cd LIKE '006%' \n");
            //DataTable dt = new InitMethods().ReturnDataTableNonConstraints(sql);
            string sql = @"SELECT 
                                    a.wmtid,
                                    a.mt_cd,
                                    a.bb_no,
                                    a.gr_qty,
                                    a.real_qty,
                                    c.dt_nm as mt_sts_cd,
                                   IF(a.recevice_dt_tims = '' OR a.recevice_dt_tims IS NULL ,a.input_dt,a.recevice_dt_tims) AS recevice_dt_tims, 
                                  
                                    b.`type`
                                FROM
                                    w_material_info AS a
                                    JOIN w_actual AS b ON a.id_actual = b.id_actual
                                    JOIN comm_dt c ON c.dt_cd = a.mt_sts_cd and c.mt_cd = 'WHS005'
                                WHERE
                                    a.mt_sts_cd IN ('008','002') AND a.lct_cd LIKE '006%' AND b.at_no= @1;";
            return db.Database.SqlQuery<WMaterialnfo>(sql, new MySqlParameter("1", at_no));
        }
        public IEnumerable<WActual> FindAllWActualByAtNo(string at_no)
        {
            string sql = @"select id_actual , at_no,type,product,actual,defect,name,level,date,don_vi_pr,item_vcd,
	                        reg_id,chg_id,chg_dt
                         from w_actual where at_no =@1 ;";
            return db.Database.SqlQuery<WActual>(sql, new MySqlParameter("1", at_no));
        }
        public int InsertWActual(w_actual actual)
        {
            string sqlInsert = @"Insert into w_actual(at_no,type,product,actual,defect,name,level,date,don_vi_pr,item_vcd,reg_id,reg_dt,chg_id,chg_dt) values(
                         @1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14); SELECT LAST_INSERT_ID() ; ";

            return db.Database.SqlQuery<int>(sqlInsert, new MySqlParameter("1", actual.at_no),
                                                        new MySqlParameter("2", actual.type),
                                                        new MySqlParameter("3", actual.product),
                                                        new MySqlParameter("4", actual.actual),
                                                        new MySqlParameter("5", actual.defect),
                                                        new MySqlParameter("6", actual.name),
                                                        new MySqlParameter("7", actual.level),
                                                        new MySqlParameter("8", actual.date),
                                                        new MySqlParameter("9", actual.don_vi_pr),
                                                        new MySqlParameter("10", actual.item_vcd),
                                                        new MySqlParameter("11", actual.reg_id),
                                                        new MySqlParameter("12", actual.reg_dt),
                                                        new MySqlParameter("13", actual.chg_id),
                                                        new MySqlParameter("14", actual.chg_dt)).SingleOrDefault();
        }
        public IEnumerable<WMaterialnfo> GetListBoBinPopup(int id_actual, string bb_no, string bb_nm, string mt_cd)
        {
            string sql = @"
                                SELECT 
                                concat(b.blno,'-0') as  bno,
                                b.mc_type,b.bb_no,b.mt_cd,b.bb_nm,b.use_yn,('')purpose,('')barcode,
                                ('')re_mark,(0)count_number,('N')del_yn,b.reg_id,b.reg_dt,b.chg_id,b.chg_dt 
                                FROM d_bobbin_lct_hist AS b JOIN w_material_info AS c ON b.mt_cd=c.mt_cd
                                JOIN w_actual AS d ON c.id_actual=d.id_actual
                                WHERE (c.id_actual!= @id_actual or
                                (c.id_actual= @id_actual AND c.mt_sts_cd in ('002','008') AND c.id_actual_oqc IS  null))
                                 AND c.lct_cd LIKE '006%' and c.mt_sts_cd in ('002','008') and c.gr_qty>0
                                 and d.at_no=(select at_no from w_actual where id_actual=@id_actual limit 1)
                                 and (@bb_no ='' or b.bb_no LIKE @bb_no_like )
                                 and (@bb_nm ='' or b.bb_nm LIKE @bb_nm_like)
                                 and (@mt_cd ='' or b.mt_cd LIKE @mt_cd_like)
                                 UNION ALL 
                                 SELECT  a.bno,a.mc_type,a.bb_no,a.mt_cd,a.bb_nm,a.use_yn,a.purpose,('')barcode ,
                                 a.re_mark,a.count_number,a.del_yn,a.reg_id,a.reg_dt,a.chg_id,a.chg_dt FROM d_bobbin_info AS a
                                 WHERE (a.mt_cd=''  OR a.mt_cd IS NULL) 
                                 and (@bb_no ='' or a.bb_no LIKE @bb_no_like)
                                 and (@bb_nm ='' or a.bb_nm LIKE @bb_nm_like)
                                 and (@mt_cd ='' or a.mt_cd LIKE @mt_cd_like)";

            return db.Database.SqlQuery<WMaterialnfo>(sql, new MySqlParameter("@id_actual", id_actual),
                                                             new MySqlParameter("@bb_no", bb_no),
                                                             new MySqlParameter("@bb_no_like", "%" + bb_no + "%"),
                                                             new MySqlParameter("@bb_nm", bb_nm),
                                                             new MySqlParameter("@bb_nm_like", "%" + bb_nm + "%"),
                                                             new MySqlParameter("@mt_cd", mt_cd),
                                                             new MySqlParameter("mt_cd_like", "%" + mt_cd + "%")
                                                            );
        }
        public IEnumerable<WMaterialMapping> FindMaterialMappingByMtcdUseYn(string mt_lot, string use_yn)
        {
            string sql = @"select 
                                         wmmid  ,
                                         mt_lot ,
                                         mt_cd  ,
                                         mt_no  ,
                                         mapping_dt,
                                         bb_no     ,
                                         remark    , 
                                         sts_share ,
                                         use_yn    ,
                                         del_yn    ,
                                         reg_id    , 
                                         reg_dt   ,
                                         chg_id   ,
                                         chg_dt
                                    from w_material_mapping where mt_lot = @1 AND use_yn =@2
                                    ";
            return db.Database.SqlQuery<WMaterialMapping>(sql, new MySqlParameter("1", mt_lot),
                                                                    new MySqlParameter("2", use_yn));
        }
        public w_actual GetProcessWactual(int id_actual)
        {
            string sql = @"SELECT * FROM w_actual WHERE id_actual=@1";
            return db.Database.SqlQuery<w_actual>(sql, new MySqlParameter("1", id_actual)).FirstOrDefault();
        }
        public int GetRealQTYChangestspacking(string start_dt, string end_dt, string staff_id, int id_actual)
        {
            string sql = @"SELECT SUM(l.real_qty)
                    FROM w_material_info AS l
                    WHERE l.mt_sts_cd!='003' AND 
                    (l.reg_dt BETWEEN DATE_FORMAT(@1, '%Y-%m-%d %H:%i:%s') AND DATE_FORMAT(@2, '%Y-%m-%d %H:%i:%s')) AND
                     l.staff_id=@3 AND l.id_actual=@4 AND l.orgin_mt_cd IS NULL";
            return db.Database.SqlQuery<int>(sql, new MySqlParameter("1", start_dt), new MySqlParameter("2", end_dt)
                , new MySqlParameter("%" + "3" + "%", staff_id), new MySqlParameter("4", id_actual)).FirstOrDefault();
        }
        public int GetDefectChangestspacking(string start_dt, string end_dt, string staff_id, int id_actual)
        {
            string sql = @"SELECT SUM(check_qty-ok_qty)
            FROM m_facline_qc AS k 
            WHERE k.ml_tims IN (
            SELECT l.mt_cd FROM w_material_info AS l
            WHERE l.staff_id=@3 AND l.reg_dt BETWEEN 
            DATE_FORMAT(@1, '%Y-%m-%d %H:%i:%s') AND DATE_FORMAT(@2, '%Y-%m-%d %H:%i:%s')
            AND l.id_actual=@4)";
            return db.Database.SqlQuery<int>(sql, new MySqlParameter("1", start_dt), new MySqlParameter("2", end_dt)
                , new MySqlParameter("3", staff_id), new MySqlParameter("4", id_actual)).FirstOrDefault();
        }
        public int GetGrQtyChangestspacking(string start_dt, string end_dt, string staff_id, int id_actual)
        {
            string sql = @"SELECT SUM(l.gr_qty) FROM w_material_info as l WHERE l.mt_sts_cd!='003' and 
            ((SELECT k.reg_dt FROM w_material_mapping AS k  WHERE k.mt_lot IS NULL AND l.mt_cd=k.mt_cd LIMIT 1) 
            BETWEEN DATE_FORMAT(@1, '%Y-%m-%d %H:%i:%s') AND DATE_FORMAT(@2, '%Y-%m-%d %H:%i:%s')) and
             l.staff_id_oqc=@3 AND l.id_actual_oqc=@4";
            return db.Database.SqlQuery<int>(sql, new MySqlParameter("1", start_dt), new MySqlParameter("2", end_dt)
                , new MySqlParameter("3", staff_id), new MySqlParameter("4", id_actual)).FirstOrDefault();
        }
        public void UpdateDprounitstaffChangestspacking(int sanluong, int defect_return, int id_actual, int psid)
        {
            string sql = @"UPDATE d_pro_unit_staff
            SET actual =Ifnull(@1,0),defect=Ifnull(@2,0)
            WHERE id_actual=@3 AND  psid=@4;";
            db.Database.ExecuteSqlCommand(sql, new MySqlParameter("1", sanluong), new MySqlParameter("2", defect_return)
                , new MySqlParameter("3", id_actual), new MySqlParameter("4", psid));
        }
        public void UpdateactualDprounitstaffChangestspacking(double sanluong, int id_actual, int psid)
        {
            string sql = @"UPDATE d_pro_unit_staff
            SET actual = Ifnull(actual,0) + Ifnull(@1, 0)
            WHERE id_actual=@2 AND  psid=@3;";
            db.Database.ExecuteSqlCommand(sql, new MySqlParameter("1", sanluong)
                , new MySqlParameter("2", id_actual), new MySqlParameter("3", psid));
        }
        public int GetSumactualChangestspacking(int id_actual)
        {
            string sql = @"SELECT SUM( Ifnull(staff.actual,0))
            FROM d_pro_unit_staff staff
            WHERE staff.id_actual =@1";
            return db.Database.SqlQuery<int>(sql, new MySqlParameter("1", id_actual)).FirstOrDefault();
        }
        public void UpdatewactualChangestspacking(int id_actual, int SumActual)
        {
            string sql = @"UPDATE w_actual 
            SET actual = Ifnull(@1,0)
            WHERE w_actual.id_actual = @2 ;";
            db.Database.ExecuteSqlCommand(sql, new MySqlParameter("1", SumActual), new MySqlParameter("2", id_actual));
        }
        public int GetsumActualmaterialinfoChangestspacking(int id_actual)
        {
            string sql = @"SELECT (b.realqty - a.defect) soluong 
            FROM w_actual a
            INNER JOIN(SELECT SUM(real_qty) realqty,id_actual FROM w_material_info  WHERE id_actual = @1 AND mt_type='CMT' AND orgin_mt_cd IS NULL GROUP BY id_actual) b ON a.id_actual  = b.id_actual 
            WHERE a.id_actual=@1";
            return db.Database.SqlQuery<int>(sql, new MySqlParameter("1", id_actual)).FirstOrDefault();
        }
        public int InsertwmaterialinfoDevideSta(int sl_chia, string mt_cd_dv, string mt_cd)
        {
            string sql = @"INSERT INTO w_material_info
                ( id_actual, id_actual_oqc, at_no, product, staff_id, staff_id_oqc, machine_id, mt_type, mt_cd, mt_no, gr_qty,
                real_qty, staff_qty, sp_cd, rd_no, sd_no, ext_no, ex_no, dl_no, recevice_dt, date, return_date, alert_NG,
                expiry_dt, dt_of_receipt, expore_dt, recevice_dt_tims, rece_wip_dt, picking_dt, shipping_wip_dt, 
	            end_production_dt, lot_no, mt_barcode, mt_qrcode, mt_sts_cd, bb_no, bbmp_sts_cd, lct_cd, lct_sts_cd, 
	            from_lct_cd, to_lct_cd, output_dt, input_dt, buyer_qr,
                orgin_mt_cd, remark, sts_update, use_yn, reg_id, reg_dt, chg_id, chg_dt)
            SELECT id_actual, id_actual_oqc, at_no, product, staff_id, staff_id_oqc, machine_id, mt_type, @2, mt_no,@1 AS slchia, @1 AS sl_chia, 
            staff_qty, sp_cd, rd_no, sd_no, ext_no, ex_no, dl_no, recevice_dt, date, return_date, alert_NG, expiry_dt, 
            dt_of_receipt, expore_dt, recevice_dt_tims, rece_wip_dt, picking_dt, shipping_wip_dt, end_production_dt, 
            lot_no,@2,@2 AS mtcddv, '002', null, bbmp_sts_cd, lct_cd, lct_sts_cd, from_lct_cd, 
            to_lct_cd, output_dt, input_dt, buyer_qr,
                @3, remark, sts_update, use_yn, reg_id,  NOW(), chg_id,  NOW()
                FROM w_material_info 
                WHERE mt_cd=@3;
            SELECT LAST_INSERT_ID();";

            return db.Database.SqlQuery<int>(sql, new MySqlParameter("1", sl_chia), new MySqlParameter("2", mt_cd_dv), new MySqlParameter("3", mt_cd)).FirstOrDefault();
        }
        public void UpdateMappingDevideSta(string bobbin, string mt_cd)
        {

                    string sql = @"SET sql_mode = '';SET @@sql_mode = '';
                INSERT INTO w_material_mapping (mt_lot, mt_cd, mt_no, mapping_dt, bb_no, remark, sts_share, use_yn, del_yn, reg_id, reg_dt, chg_id, chg_dt) 
                SELECT  mt_cd,@2, ANY_VALUE(mt_no) mt_no, DATE_FORMAT(NOW(), '%Y%m%d%H%i%s') , @1,ANY_VALUE(remark) remark, NULL, 'Y', 'N', ANY_VALUE(reg_id) reg_id, NOW(), ANY_VALUE(chg_id) chg_id, NOW()
                FROM w_material_info WHERE orgin_mt_cd=@2 AND mt_cd  LIKE '%-DV%' GROUP BY mt_cd;";
                db.Database.ExecuteSqlCommand(sql, new MySqlParameter("1", bobbin), new MySqlParameter("2", mt_cd));
            
            

        }
        public void DeleteBobbinHistoryDevideSta(string mt_cd)
        {
            string sqlquery = "DELETE FROM d_bobbin_lct_hist WHERE  mt_cd=@1";
            db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", mt_cd));
        }
        public IEnumerable<WMaterialInfoTmp> Getdbobbinlcthist(int id_actual, string at_no, string bb_nm, string bb_no)
        {
            string sqlquery = @"SELECT wmtid,id_actual,id_actual_oqc,at_no,product,staff_id,staff_id_oqc,
            mt_type,mt_cd,mt_no,gr_qty,real_qty,date,end_production_dt,mt_barcode,mt_qrcode,mt_sts_cd,bb_no,bbmp_sts_cd,
            lct_cd,lct_sts_cd,from_lct_cd,input_dt,orgin_mt_cd,sts_update,use_yn,reg_id,reg_dt,chg_id,chg_dt 
            FROM w_material_info WHERE bb_no is not NULL AND gr_qty> 0 AND mt_sts_cd in ('002','008') AND lct_cd='006000000000000000' AND id_actual=(SELECT id_actual FROM w_actual WHERE id_actual !=@1 AND at_no=@2 AND type='TIMS' ORDER BY `level` DESC LIMIT 1);";

            if (string.IsNullOrEmpty(bb_no))
            {
                return db.Database.SqlQuery<WMaterialInfoTmp>(sqlquery,
                new MySqlParameter("1", id_actual),
                new MySqlParameter("2", at_no));
            }
            else
            {
                return db.Database.SqlQuery<WMaterialInfoTmp>(sqlquery,
                new MySqlParameter("1", id_actual),
                new MySqlParameter("2", at_no)).Where(i => i.bb_no.Contains(bb_no));
            }
        }
        public int GetQrcodeBuyer(string buyer_qr)
        {
            string sql = @"SELECT COUNT(wmtid) FROM w_material_info WHERE buyer_qr=@1";
            return db.Database.SqlQuery<int>(sql,new MySqlParameter("@1", buyer_qr)).FirstOrDefault();
        }
        public d_bobbin_lct_hist GetdbobbinlcthistFrbbno(string bb_no)
        {
            string sqlquery = @"SELECT * FROM d_bobbin_lct_hist WHERE bb_no=@1";
            return db.Database.SqlQuery<d_bobbin_lct_hist>(sqlquery, new MySqlParameter("1", bb_no)).FirstOrDefault();
        }
        public bool CheckQRBuyer(string bb_no)
        {
            string sql = @"SELECT buyer_qr FROM w_material_info WHERE bb_no=@1";
            return string.IsNullOrEmpty(db.Database.SqlQuery<string>(sql,new MySqlParameter("@1", bb_no)).FirstOrDefault());
        }
        public IEnumerable<MaterialInfoMMS> CheckwmaterialinfoMappingbuyer(string mt_cd,string mt_sts_cd,string lct_cd)
        {
            string sql = @"Select * from w_material_info where mt_cd=@1 and mt_sts_cd=@2 and lct_cd = @3";
            return db.Database.SqlQuery<MaterialInfoMMS>(sql, new MySqlParameter("@1", mt_cd), new MySqlParameter("@2", mt_sts_cd), new MySqlParameter("@3", lct_cd));
        }
        public string GetProductWactualPrimary(string mt_cd, string mt_sts_cd, string lct_cd)
        {
            string sql = @"Select c.product from w_material_info a
            inner join w_actual b on a.id_actual = b.id_actual
            inner join w_actual_primary c on a.at_no = c.at_no
            where  a.mt_cd=@1 and a.mt_sts_cd=@2 and a.lct_cd = @3";
            return db.Database.SqlQuery<string>(sql, new MySqlParameter("@1", mt_cd), new MySqlParameter("@2", mt_sts_cd), new MySqlParameter("@3",  lct_cd)).FirstOrDefault();
        }
        public stamp_detail Getstampdetail(string buyer_qr)
        {
            string sql = @"SELECT * FROM stamp_detail WHERE buyer_qr=@1";
            return db.Database.SqlQuery<stamp_detail>(sql, new MySqlParameter("@1", buyer_qr)).FirstOrDefault();
        }
        public d_style_info GetStyleNo(string style_no)
        {
            string sql = @"Select * From d_style_info Where style_no=@1";
           return db.Database.SqlQuery<d_style_info>(sql,new MySqlParameter("@1", style_no)).FirstOrDefault();
        }
        public void Insertstampdetail(stamp_detail item)
        {
            string sql = @"Insert into stamp_detail(buyer_qr,stamp_code,product_code,vendor_code,vendor_line,label_printer,is_sample,pcn,lot_date,serial_number,machine_line,shift,standard_qty,is_sent,box_code,reg_id,reg_dt,chg_id,chg_dt)
            Values(@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16,NOW(),@17,NOW())";
            db.Database.ExecuteSqlCommand(sql,new MySqlParameter("@1",item.buyer_qr)
                , new MySqlParameter("@2", item.stamp_code), new MySqlParameter("@3", item.product_code), new MySqlParameter("@4", item.vendor_code)
                , new MySqlParameter("@5", item.vendor_line), new MySqlParameter("@6", item.label_printer), new MySqlParameter("@7", item.is_sample)
                , new MySqlParameter("@8", item.pcn), new MySqlParameter("@9", item.lot_date), new MySqlParameter("@10", item.serial_number)
                , new MySqlParameter("@11", item.machine_line), new MySqlParameter("@12", item.shift), new MySqlParameter("@13", item.standard_qty)
                , new MySqlParameter("@14", item.is_sent), new MySqlParameter("@15", item.box_code), new MySqlParameter("@16", item.reg_id)
                , new MySqlParameter("@17", item.chg_id));
        }
        public void Deletedbobbininfo(int bno)
        {
            string sql = "Delete From d_bobbin_info where bno=@1";
            db.Database.ExecuteSqlCommand(sql,new MySqlParameter("@1", bno));
        }

        public WMaterialInfoStamp ViewStatusTemGoi(string buyerCode)
        {
            string QuerySQL = @"  SELECT a.wmtid,a.mt_cd,a.chg_dt,a.chg_id, a.buyer_qr, a.gr_qty,a.mt_sts_cd, a.lct_cd,a.product,
                (SELECT dt_nm FROM comm_dt WHERE mt_cd = 'WHS005' and a.mt_sts_cd = dt_cd) AS NameStatus,
                (SELECT lct_nm FROM lct_info WHERE a.lct_cd = lct_cd) AS locationName
                FROM w_material_info AS a
                WHERE a.buyer_qr = @1  ";
            return db.Database.SqlQuery<WMaterialInfoStamp>(QuerySQL, new MySqlParameter("1", buyerCode)).FirstOrDefault();
        }

        public w_box_mapping CheckTemGoimappingBox(string buyerCode)
        {
            string QuerySQL = "SELECT * FROM w_box_mapping WHERE buyer_cd = @1  ";
            return db.Database.SqlQuery<w_box_mapping>(QuerySQL, new MySqlParameter("1", buyerCode)).FirstOrDefault();
        }

    }
}