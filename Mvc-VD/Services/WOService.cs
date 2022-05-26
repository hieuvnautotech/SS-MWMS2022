using Mvc_VD.Classes;
using Mvc_VD.Models;
using Mvc_VD.Models.WOModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Mvc_VD.Services
{

    public interface IWOService
    {
        d_bobbin_info GetBobbinInfo(string bb_no);
        bool CheckBobbinHistory(string bb_no);
        bool CheckStaffShift(int id_actual);
        bool CheckMachineShift(int id_actual);
        IEnumerable<string> GetStaffOfProcess(int id_actual);//lấy danh sách những công nhân trên một công đoạn.
        IEnumerable<string> GetMachineProcess(int id_actual);//lấy danh sách những Máy trên một công đoạn.
        int CountDataMtCodeWmaterialInfo(string mt_cd);
        int InsertToWmaterialInfo(MaterialInfoMMS item);

        MaterialInfoMMS GetWMaterialInfo(int id);
        void UpdateBobbinMTCode(d_bobbin_info item);
        int InsertToBobbinLctHistory(d_bobbin_lct_hist item);
        void UpdateProUnitStaff(string start_dt, string end_dt, string staff_id, string use_yn, string del_yn, int psid);
        IEnumerable<d_pro_unit_mc> GetListProUnitMC(string mc_no);
        int InsertProUnitMC(d_pro_unit_mc item);
       
        string Getmtlot(int idActual);

        int TotalRecordsSearchStaffpp(string userid, string username, string position_cd);
        IEnumerable<StaffPP> GetListSearchStaffpp(string userid, string username, string position_cd, string start, string end, string orderby);
        string GetEndDateProcessUnitStaff(float timenow);

        string GetStatusProcessUnitStaff(string staff_id, int id_actual, string start, string end);
        //ProcessUnitStaff InsertDProUnitStaff(d_pro_unit_staff dprounitstaff, string staffid);
        IEnumerable<ProcessUnitStaff> GetListProcessStaff(int idactual);
       
        IEnumerable<comm_dt> GetListCommDT(string mtcd, string useyn);
        d_pro_unit_staff GetListDProUnitStaff(int psid);
        ProcessUnitStaff GetDProUnitStaff(int  psi);
        int InsertDProUnitStaff(d_pro_unit_staff dprounitstaff);
        string GetStatusdprounitmc(string mcno, int idactual, string start, string end);
        d_pro_unit_mc Getdprounitmc(int pmid);
        int InsertwMaterialMapping(w_material_mapping item);
        MaterialInfoMMS GetWMaterialInfowithmtcd(string mt_cd);
        void UpdateMaterialInfo(MaterialInfoMMS item);
        w_material_info_wo Getwmaterialinfowo(string mt_no, string bbmp_sts_cd, string mt_cd, int idWMaterialInfo);
        int Getsumgrqty(string mt_cd);
        IEnumerable<wo_info_mc_wk_mold> Getwoinfomcwkmold(int idActual);
        void Updatedprounitstaff(d_pro_unit_staff item);
        int GettotalcountProcessUnitStaff(string staff_id, int psid);
        void Updatedprounitmc(d_pro_unit_mc item);

        int Gettotalcountdprounitmc(string mc_no, int id_actual,int pmid);

        int GetStatusdprounitmcupdate(string mcno, int idactual, int pmid, string start, string end);

        IEnumerable<ProcessMachineunit> Getlistdprounitmcfromidactual(int id_actual);
        IEnumerable<DatawActualPrimaryResponse> GetDatawActualPrimary(string product, string product_name, string model, string at_no, string regstart, string regend, int start, int end);
        int GetTotalcountDatawActualPrimary(string product, string product_name, string model, string at_no, string regstart, string regend);
        IEnumerable<DatawActual> GetDatawActual(string at_no);

        IEnumerable<DatawActualPrimaryResponse> GetDatawActualPrimaryFromSP(string product, string product_name, string model, string at_no, string regstart, string regend, string mms);
        IEnumerable<WMaterialInfoAPI> GetDetailActualAPI(int idActual, string mtType);
        IEnumerable<DMachineInfoAPI> GetPopupMachine(string mc_type, string mc_no, string mc_nm);
        IEnumerable<MtDateWebWO> GetMtDateWebs(int id_actual);
        int CheckHetCaMT(int id_actual, string staff_id);
        IEnumerable<CheckUpdateGrty> GetUpdateGrty(int wmtid);
        int CallSPUpdateSLwactual(int id_actual);
        int CheckwMaterialMapping(string mt_cd);
        int Checkwmfaclineqc(string mt_cd);
        int Statusmappingfrommtlot(string mt_cd);
        IEnumerable<w_material_mapping> GetWMaterialMappingfrommtlot(string mt_cd);
        w_material_mapping Getw_material_mappingfromwmmid(int wmmid);
        MaterialInfoMMS Getw_material_infoforDelPP(string mt_cd, string mt_type);
        int Deletew_material_info(int wmtid);
        int GetcountwmaterialmappingFrmtcdamtlot(string mt_cd, string mt_lot);
        d_bobbin_lct_hist GetdbobbinlcthistFrbbnoamtcd(string bb_no, string mt_cd);
        void Deletedbobbinlcthist(int blno);
        IEnumerable<DataMappingW> GetDataMappingW(string mt_cd);
        int CheckwmaterialmappingFinish(string mt_cd, string mt_lot, string mapping_dt);
        IEnumerable<DSMappingSTA> GetDSMappingSta(string mt_cd);
        MaterialInfoMMS GetmaterialinfoforDevice(string mt_cd, string sts_update, string mt_sts_cd, string lct_sts_cd);
        int GetDProUnitStaffforDevice(int id_actual);
        void UpdatedBobbinInfoforDevice(string bb_no, string mt_cd, string chg_id);
        void DeleteDBobbinLctHistforDevice(string bb_no, string mt_cd);
        int GetTotalwMaterialInfoDV(string mt_cd);
        void Updatedbobbinlcthist(d_bobbin_lct_hist item);
        d_bobbin_info GetdbobbininfoforChangebbdv(string bb_no);
        int GettwmaterialinforwithNG(string mt_cd);
        int Gettwproductqc(string mt_cd);
        void UpdateWMaterialMapping(w_material_mapping item);
        int Checkdprounitstafffinishback(int id_actual, string staff_id);
        int Checkexitsreturn(string orgin_mt_cd, string sts_update);
        int Checkwmaterialmappingfromcdtype(string mt_cd, string mt_type);
        void Deletewmaterialmapping(w_material_mapping item);
        w_material_mapping Getmaterialmappingreturn(string mt_cd, string mt_lot);
        int GetwmaterialinfoforDelPP(string orgin_mt_cd, string sts_update);
        d_bobbin_info GetBobbinInfoReturn(string bb_no, string mt_cd);
        void UpdatewmaterialMappingmtcduseyn(string mt_cd, string use_yn, string useyn, string chg_id);
        d_bobbin_lct_hist GetdbobbinlcthistFrbbno(string bb_no);
        IEnumerable<SaveReturn> GetSaveReturn(int wmmid);
        w_actual GetWActual(int id_actual);
        int Getwmaterialmappingformapp(string mt_no, string mt_cd, string mt_lot);
        IEnumerable<d_bobbin_info> Searchdbobbininfo(string bb_no, string bb_nm);
        void InsertMaterialMpping(string mt_cd, string userid, string mt_lot);
        void UpdateMaterialInfofroMpping(string userid, string staff_id, string GetMachineProcess, string mt_type, string mt_lot);
        void UpdateMaterialInffrMpping(string userid, string mt_type, string mt_lot);

        bool CountMaxMapping(string mt_cd,string mt_lot);
        IEnumerable<DatawActualPrimaryResponse> GetProcessingActual(string at_no);
        bool CountMaxBobbin(string mt_cd, int id_actual);
        bool CheckMaterialRetun(string mt_cd);
        mb_info GetMbInfoGrade(string uname);
        IEnumerable<w_actual> GetActual(string at_no, string type);
        // int InsertToBobbinLctHistory(d_bobbin_lct_hist item);
        int InsertWActualInfo(w_actual item);
        bool IsMaterialInfoExist(string mt_no);
        int InsertDMaterial(d_material_info item);
        void UpdateWActual(int id_actual, string description);
        BomInfoModel CheckBom(string style_no);
        bool IsMaterialInfoExistByBom(string productCode,string MaterialNo);
        void UpdateProductDeApply(int id_actualpr, string IsApply);
        DatawActualPrimaryResponse IsCheckApply(string at_no);
        IEnumerable<WActualBom> GetListmaterialbom(string style_no, string at_no);
        IEnumerable<BuyerQRModel> GetdataTemGoi(string product);
    }

    public class WOService : IWOService
    {
        private Entities _db;
        public WOService(IDbFactory dbFactory)
        {
            _db = dbFactory.Init();
        }

        public bool CheckBobbinHistory(string bb_no)
        {
            string QuerySQL = "SELECT bb_no FROM d_bobbin_lct_hist WHERE bb_no = @1";
            return String.IsNullOrWhiteSpace(_db.Database.SqlQuery<string>(QuerySQL, new MySqlParameter("1", bb_no)).FirstOrDefault());
        }

        public bool CheckMachineShift(int id_actual)
        {
            string QuerySQL = "SELECT a.pmid FROM d_pro_unit_mc AS a WHERE a.id_actual=@1 AND (NOW() BETWEEN Date_format(a.start_dt,'%Y-%m-%d %H:%i:%s') AND Date_format(a.end_dt,'%Y-%m-%d %H:%i:%s'))";
            return String.IsNullOrWhiteSpace(_db.Database.SqlQuery<string>(QuerySQL, new MySqlParameter("1", id_actual)).FirstOrDefault());
        }

        public bool CheckStaffShift(int id_actual)
        {
            string QuerySQL = "SELECT a.psid FROM d_pro_unit_staff AS a WHERE a.id_actual=@1 AND (NOW() BETWEEN Date_format(a.start_dt,'%Y-%m-%d %H:%i:%s') AND Date_format(a.end_dt,'%Y-%m-%d %H:%i:%s'))";
            return String.IsNullOrWhiteSpace(_db.Database.SqlQuery<string>(QuerySQL, new MySqlParameter("1", id_actual)).FirstOrDefault());
        }

        public d_bobbin_info GetBobbinInfo(string bb_no)
        {
            string QuerySQL = "SELECT * FROM d_bobbin_info WHERE bb_no = @1";
            return _db.Database.SqlQuery<d_bobbin_info>(QuerySQL, new MySqlParameter("1", bb_no)).FirstOrDefault();
        }
        public IEnumerable<string> GetStaffOfProcess(int id_actual)
        {
            string QuerySQL = "SELECT staff_id FROM d_pro_unit_staff WHERE id_actual = @1 AND Now() BETWEEN Date_format(start_dt, '%Y-%m-%d %H:%i:%s') AND  Date_format(end_dt, '%Y-%m-%d %H:%i:%s')";
            //IEnumerable<string> value= _db.Database.SqlQuery<string>(QuerySQL, new MySqlParameter("1", id_actual));
            return _db.Database.SqlQuery<string>(QuerySQL, new MySqlParameter("1", id_actual));
        }
        public IEnumerable<string> GetMachineProcess(int id_actual)
        {
            string QuerySQL = "SELECT mc_no FROM d_pro_unit_mc WHERE id_actual = @1 AND Now() BETWEEN Date_format(start_dt, '%Y-%m-%d %H:%i:%s') AND  Date_format(end_dt, '%Y-%m-%d %H:%i:%s')";
            return _db.Database.SqlQuery<string>(QuerySQL, new MySqlParameter("1", id_actual));
        }


        //Nga viết
        //public int InsertToWmaterialInfo(int id_actual, string at_no, string product, string staff_id, string machine_id, string mt_type, string mt_cd, string mt_no, double gr_qty, double real_qty, string date, string mt_sts_cd, string bb_no, string lct_cd, string input_dt, string reg_id)
        public int InsertToWmaterialInfo(MaterialInfoMMS item)
        {
            string QuerySQL = @"INSERT INTO w_material_info (id_actual,id_actual_oqc,at_no,product,staff_id,staff_id_oqc,machine_id,mt_type,mt_cd,mt_no,
                gr_qty,real_qty,staff_qty,sp_cd,rd_no,sd_no,ext_no,ex_no,dl_no,recevice_dt,date,return_date,alert_NG,
                expiry_dt,dt_of_receipt,expore_dt,recevice_dt_tims,rece_wip_dt,picking_dt,shipping_wip_dt,end_production_dt,lot_no,mt_barcode,
                mt_qrcode,mt_sts_cd,bb_no,bbmp_sts_cd,lct_cd,lct_sts_cd,from_lct_cd,to_lct_cd,output_dt,input_dt,buyer_qr,
                orgin_mt_cd,remark,sts_update,use_yn,reg_id,reg_dt,chg_id,chg_dt)
            VALUES (@1,@2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14, @15, @16,
            @17,@18,@19,@20,@21,@22,@23,@24,@25,@26,@27,@28,@29,@30,@31,@32,@33,@34,@35,@36,@37,@38,@39,@40,@41,@42,@43,@44,@45,@46,@47,
            @48,@49,@50,@51,@52);
            SELECT LAST_INSERT_ID();";

            return _db.Database.SqlQuery<int>(QuerySQL, new MySqlParameter("@1", item.id_actual),
                new MySqlParameter("@2", item.id_actual_oqc), new MySqlParameter("@3", item.at_no), new MySqlParameter("@4", item.product),
                new MySqlParameter("@5", item.staff_id), new MySqlParameter("@6", item.staff_id_oqc), new MySqlParameter("@7", item.machine_id),
                new MySqlParameter("@8", item.mt_type), new MySqlParameter("@9", item.mt_cd), new MySqlParameter("@10", item.mt_no),
                new MySqlParameter("@11", item.gr_qty), new MySqlParameter("@12", item.real_qty), new MySqlParameter("@13", item.staff_qty),
                new MySqlParameter("@14", item.sp_cd), new MySqlParameter("@15", item.rd_no), new MySqlParameter("@16", item.sd_no),
                new MySqlParameter("@17", item.ext_no), new MySqlParameter("@18", item.ex_no), new MySqlParameter("@19", item.dl_no),
                new MySqlParameter("@20", item.recevice_dt), new MySqlParameter("@21", item.date), new MySqlParameter("@22", item.return_date),
                new MySqlParameter("@23", item.alert_NG), new MySqlParameter("@24", item.expiry_dt), new MySqlParameter("@25", item.dt_of_receipt),
                new MySqlParameter("@26", item.expore_dt), new MySqlParameter("@27", item.recevice_dt_tims), new MySqlParameter("@28", item.rece_wip_dt),
                new MySqlParameter("@29", item.picking_dt), new MySqlParameter("@30", item.shipping_wip_dt), new MySqlParameter("@31", item.end_production_dt),
                new MySqlParameter("@32", item.lot_no), new MySqlParameter("@33", item.mt_barcode), new MySqlParameter("@34", item.mt_qrcode),
                new MySqlParameter("@35", item.mt_sts_cd), new MySqlParameter("@36", item.bb_no), new MySqlParameter("@37", item.bbmp_sts_cd),
                new MySqlParameter("@38", item.lct_cd), new MySqlParameter("@39", item.lct_sts_cd), new MySqlParameter("@40", item.from_lct_cd),
                new MySqlParameter("@41", item.to_lct_cd), new MySqlParameter("@42", item.output_dt), new MySqlParameter("@43", item.input_dt),
                new MySqlParameter("@44", item.buyer_qr), new MySqlParameter("@45", item.orgin_mt_cd), new MySqlParameter("@46", item.remark),
                new MySqlParameter("@47", item.sts_update), new MySqlParameter("@48", item.use_yn), new MySqlParameter("@49", item.reg_id),
                new MySqlParameter("@50", item.reg_dt), new MySqlParameter("@51", item.chg_id), new MySqlParameter("@52", item.chg_dt)).FirstOrDefault();
        }
        public MaterialInfoMMS GetWMaterialInfo(int id)
        {
            string QuerySQL = "SELECT * FROM w_material_info WHERE wmtid = @1";
            return _db.Database.SqlQuery<MaterialInfoMMS>(QuerySQL, new MySqlParameter("1", id)).FirstOrDefault();
        }
        public void UpdateBobbinMTCode(d_bobbin_info item)
        {
            //string QuerySQL = "UPDATE d_bobbin_info SET mt_cd=@1  WHERE bno=@2 ";
            string QuerySQL = @"UPDATE d_bobbin_info SET mc_type=@1,bb_no=@2,mt_cd=@3,bb_nm=@4,purpose=@5,barcode=@6,re_mark=@7,
              use_yn=@8,count_number=@9,del_yn=@10,reg_id=@11,reg_dt=@12,chg_id=@13,chg_dt=@14    
            WHERE bno=@15";
            _db.Database.ExecuteSqlCommand(QuerySQL, new MySqlParameter("1", item.mc_type), new MySqlParameter("2", item.bb_no),
                new MySqlParameter("3", item.mt_cd), new MySqlParameter("4", item.bb_nm), new MySqlParameter("5", item.purpose),
                new MySqlParameter("6", item.barcode), new MySqlParameter("7", item.re_mark), new MySqlParameter("8", item.use_yn),
                new MySqlParameter("9", item.count_number), new MySqlParameter("10", item.del_yn), new MySqlParameter("11", item.reg_id),
                new MySqlParameter("12", item.reg_dt), new MySqlParameter("13", item.chg_id), new MySqlParameter("14", item.chg_dt),
                new MySqlParameter("15", item.bno));
            //_db.Database.ExecuteSqlCommand(QuerySQL, new MySqlParameter("1", mt_cd),
            //                                         new MySqlParameter("2", bno));
        }
        public int InsertToBobbinLctHistory(d_bobbin_lct_hist item)
        {
            string sqlquery = @"INSERT INTO d_bobbin_lct_hist(mc_type,bb_no,mt_cd,bb_nm,start_dt,end_dt,use_yn,del_yn,reg_id,reg_dt,chg_id,chg_dt)
                VALUES(@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12);
                SELECT LAST_INSERT_ID();";
            return _db.Database.SqlQuery<int>(sqlquery, new MySqlParameter("1", item.mc_type),
                                                        new MySqlParameter("2", item.bb_no),
                                                        new MySqlParameter("3", item.mt_cd),
                                                        new MySqlParameter("4", item.bb_nm),
                                                        new MySqlParameter("5", item.start_dt),
                                                        new MySqlParameter("6", item.end_dt),
                                                        new MySqlParameter("7", item.use_yn),
                                                        new MySqlParameter("8", item.del_yn),
                                                        new MySqlParameter("9", item.reg_id),
                                                        new MySqlParameter("10", item.reg_dt),
                                                        new MySqlParameter("11", item.chg_id),
                                                        new MySqlParameter("12", item.chg_dt)).FirstOrDefault();
            //string QuerySQL = " INSERT INTO  d_bobbin_lct_hist (mc_type, bb_no, mt_cd,bb_nm) VALUES (@1, @2, @3,@4);SELECT LAST_INSERT_ID(); ";
            //return _db.Database.SqlQuery<int>(QuerySQL, new MySqlParameter("1", mc_type),
            //                                            new MySqlParameter("2", bb_no),
            //                                            new MySqlParameter("3", mt_cd),
            //                                            new MySqlParameter("4", bb_nm)).FirstOrDefault();
        }

        public int CountDataMtCodeWmaterialInfo(string mt_cd)
        {
            string QuerySQL = " SELECT COUNT(wmtid) FROM  w_material_info  WHERE mt_cd LIKE @1  ";
            return _db.Database.SqlQuery<int>(QuerySQL, new MySqlParameter("1", mt_cd + "%")).FirstOrDefault();
        }

        public IEnumerable<d_pro_unit_mc> GetListProUnitMC(string mc_no)
        {
            string QuerySQL = "SELECT * FROM d_pro_unit_mc WHERE mc_no = @1";
            return _db.Database.SqlQuery<d_pro_unit_mc>(QuerySQL, new MySqlParameter("1", mc_no));
        }

        public int InsertProUnitMC(d_pro_unit_mc item)
        {
            string QuerySQL = @"INSERT INTO d_pro_unit_mc (start_dt, end_dt, remark, mc_no, use_yn, del_yn, reg_dt, chg_dt,id_actual)
                VALUES (@1, @2, @3, @4, @5, @6, @7, @8,@9);
                SELECT LAST_INSERT_ID();";
            return _db.Database.SqlQuery<int>(QuerySQL,
                 new MySqlParameter("@1", item.start_dt),
                 new MySqlParameter("@2", item.end_dt),
                 new MySqlParameter("@3", item.remark),
                 new MySqlParameter("@4", item.mc_no),
                 new MySqlParameter("@5", item.use_yn),
                 new MySqlParameter("@6", item.del_yn),
                 new MySqlParameter("@7", item.reg_dt),
                 new MySqlParameter("@8", item.chg_dt),
                 new MySqlParameter("@9", item.id_actual)).FirstOrDefault();
        }

        public void UpdateProUnitStaff(string start_dt, string end_dt, string staff_id, string use_yn, string del_yn, int psid)
        {
            string QuerySQL = "UPDATE d_pro_unit_staff SET start_dt = @1, end_dt = @2, staff_id = @3, use_yn = @4, del_yn = @5 WHERE psid = @6";
            _db.Database.ExecuteSqlCommand(QuerySQL,
                new MySqlParameter("@1", start_dt),
                new MySqlParameter("@2", end_dt),
                new MySqlParameter("@3", staff_id),
                new MySqlParameter("@4", use_yn),
                new MySqlParameter("@5", del_yn),
                new MySqlParameter("@6", psid));
        }

        public string Getmtlot(int idActual)
        {
            string sqlquery = @"SELECT  mmapp.mt_lot FROM w_material_mapping mmapp 
                    Where (mmapp.bb_no !=null OR mmapp.bb_no !='') AND mmapp.mt_lot in (SELECT minfo.mt_cd from  w_material_info minfo  WHERE minfo.id_actual = @1) ORDER BY mmapp.reg_dt DESC LIMIT 1";
            return _db.Database.SqlQuery<string>(sqlquery, new MySqlParameter("@1", idActual)).FirstOrDefault();
        }

        public int TotalRecordsSearchStaffpp(string userid, string username, string position_cd)
        {
            try
            {
                string countSql = @"SELECT COUNT(*) 
                FROM ( 
                        SELECT cmmdt.dt_nm AS position_cd,
				            a.userid ,
				            ROW_NUMBER() OVER(ORDER BY a.userid DESC) AS RowNum ,
                            a.uname, 
                            a.nick_name, 
                            (
		                    SELECT
			                    c.mc_no 
		                    FROM
			                    d_pro_unit_staff AS b
			                    LEFT JOIN d_pro_unit_mc AS c ON b.id_actual = c.id_actual 
		                    WHERE
			                    a.userid = b.staff_id 
			                    AND c.mc_no IN ( SELECT d.mc_no FROM d_machine_info AS d ) 
		                    ORDER BY
			                    c.chg_dt DESC 
			                    LIMIT 1 
		                    ) AS mc_no 
	                    FROM   mb_info AS a
	                    LEFT JOIN comm_dt cmmdt ON cmmdt.mt_cd = 'COM018' AND cmmdt.dt_cd = a.position_cd
	                    Where a.lct_cd='staff' and  (@1='' OR  a.userid like @4 ) AND (@2='' OR  a.uname like @5 ) AND (@3='' OR  a.position_cd like @6)
                ) MyDerivedTable ";
                return _db.Database.SqlQuery<int>(countSql,
                     new MySqlParameter("1", userid),
                     new MySqlParameter("2", username),
                     new MySqlParameter("3", position_cd),
                     new MySqlParameter("4", "%" + userid + "%"),
                     new MySqlParameter("5", "%" + username + "%"),
                     new MySqlParameter("6", "%" + username + "%")
                     ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
        public IEnumerable<StaffPP> GetListSearchStaffpp(string userid, string username, string position_cd, string start, string end, string orderby)
        {

            string viewSql = @" SELECT * 
                FROM (  
                    SELECT cmmdt.dt_nm AS position_cd,
				            a.userid ,
				            ROW_NUMBER() OVER(ORDER BY a.userid DESC) AS RowNum ,
                            a.uname, 
                            a.nick_name, 
                            (
		                    SELECT
			                    c.mc_no 
		                    FROM
			                    d_pro_unit_staff AS b
			                    LEFT JOIN d_pro_unit_mc AS c ON b.id_actual = c.id_actual 
		                    WHERE
			                    a.userid = b.staff_id 
			                    AND c.mc_no IN ( SELECT d.mc_no FROM d_machine_info AS d ) 
		                    ORDER BY
			                    c.chg_dt DESC 
			                    LIMIT 1 
		                    ) AS mc_no 
	                    FROM   mb_info AS a
	                    LEFT JOIN comm_dt cmmdt ON cmmdt.mt_cd = 'COM018' AND cmmdt.dt_cd = a.position_cd
	                    Where a.lct_cd='staff' and  (@1='' OR  a.userid like @7 ) AND (@2='' OR  a.uname like @8 ) AND (@3='' OR  a.position_cd like @9)
                ) MyDerivedTable 
                 WHERE MyDerivedTable.RowNum BETWEEN @4 AND @5
                @6";
            return _db.Database.SqlQuery<StaffPP>(viewSql,
                new MySqlParameter("1", userid),
                new MySqlParameter("2", username),
                new MySqlParameter("3", position_cd),
                new MySqlParameter("4", start),
                new MySqlParameter("5", end),
                new MySqlParameter("6", orderby),
                new MySqlParameter("7", "%" + userid + "%"),
                new MySqlParameter("8", "%" + username + "%"),
                new MySqlParameter("9", "%" + position_cd + "%"));

        }

        public string Getdtnm(string mdtc, string dtcd)
        {
            string sqlquery = @"SELECT mt_cd FROM comm_dt where mt_cd = '@1 AND dt_cd=@2";
            return _db.Database.SqlQuery<string>(sqlquery, new MySqlParameter("@1", mdtc), new MySqlParameter("@1", dtcd)).FirstOrDefault();
        }

        public string GetEndDateProcessUnitStaff(float timenow)
        {
            string sql = @"SELECT (
	            CASE
	            WHEN 8<=DATE_FORMAT(NOW(),'%H') AND  DATE_FORMAT(NOW(),'%H')<=20 
	            THEN DATE_ADD(CURRENT_DATE(),INTERVAL 20 HOUR)
	            WHEN 20<=DATE_FORMAT(NOW(),'%H') AND  DATE_FORMAT(NOW(),'%H')<=24 
	            THEN DATE_ADD(CURRENT_DATE(),INTERVAL 32 HOUR)
	            WHEN 0<=DATE_FORMAT(NOW(),'%H') AND DATE_FORMAT(NOW(),'%H')<=8 
	            THEN DATE_ADD(CURRENT_DATE(),INTERVAL 8 HOUR)
	            ELSE ''
            END);";
            string sqlenddate = @"SELECT (
		            CASE
		            WHEN 8<=@1 AND  @1<=20 
		            THEN DATE_ADD(CURRENT_DATE(),INTERVAL 20 HOUR)
		            WHEN 20<@1 AND  @1<=24 
		            THEN DATE_ADD(CURRENT_DATE(),INTERVAL 32 HOUR)
		            WHEN 0<=@1 AND @1<8 
		            THEN DATE_ADD(CURRENT_DATE(),INTERVAL 8 HOUR)
		            ELSE ''
	            END
            )";
            return _db.Database.SqlQuery<string>(sqlenddate, new MySqlParameter("@1", timenow)).FirstOrDefault();
        }
        public string GetStatusProcessUnitStaff(string staff_id, int id_actual, string start, string end)
        {
            try
            {
                string sqlcheckstatus = @"
                SELECT (
		            CASE
		            WHEN NOT  EXISTS(SELECT * from d_pro_unit_staff where staff_id=@1)
		            THEN '0'
		            WHEN  EXISTS(SELECT * from d_pro_unit_staff WHERE staff_id =@1 AND id_actual=@2 AND start_dt <= @3  AND @3 <= end_dt)
		            THEN '1'
		            WHEN  EXISTS(SELECT * from d_pro_unit_staff WHERE staff_id =@1 AND id_actual=@2 AND start_dt <= @4  AND @4 <= end_dt)
		            THEN '1'
		            WHEN  EXISTS(SELECT * from d_pro_unit_staff WHERE staff_id =@1 AND start_dt <= @3  AND @3 <= end_dt)
		            THEN 'SELECT psid from d_pro_unit_staff WHERE staff_id =@1 AND start_dt <= @3  AND @3 <= end_dt'
		            WHEN EXISTS(SELECT * from d_pro_unit_staff WHERE staff_id =@1 AND start_dt <= @4  AND @4 <= end_dt)
		            THEN 'SELECT psid from d_pro_unit_staff WHERE staff_id =@1 AND start_dt <= @4  AND @4 <= end_dt'
                    ELSE '0'
	                END
                ) AS a
            ";
                return _db.Database.SqlQuery<string>(sqlcheckstatus, new MySqlParameter("@1", staff_id),
                     new MySqlParameter("@2", id_actual),
                      new MySqlParameter("@3", start),
                       new MySqlParameter("@4", end)).FirstOrDefault();
            }
            catch (Exception e)
            {
                return "";
            }

        }
        public string GetStatusProcessMachineUnitStaff(string staff_id, int id_actual, string start, string end)
        {
            string sqlcheckstatus = @"
                SELECT (
		            CASE
		            WHEN NOT  EXISTS(SELECT * from d_pro_unit_staff where staff_id=@1)
		            THEN '0'
		            WHEN  EXISTS(SELECT * from d_pro_unit_staff WHERE staff_id =@1 AND id_actual=@2 AND start_dt <= @3  AND @3 <= end_dt)
		            THEN '1'
		            WHEN  EXISTS(SELECT * from d_pro_unit_staff WHERE staff_id =@1 AND id_actual=@2 AND start_dt <= @4  AND @4 <= end_dt)
		            THEN '1'
		            WHEN  EXISTS(SELECT * from d_pro_unit_staff WHERE staff_id =@1 AND start_dt <= @3  AND @3 <= end_dt)
		            THEN 'SELECT psid from d_pro_unit_staff WHERE staff_id =@1 AND start_dt <= @3  AND @3 <= end_dt'
		            WHEN EXISTS(SELECT * from d_pro_unit_staff WHERE staff_id =@1 AND start_dt <= @4  AND @4 <= end_dt)
		            THEN 'SELECT psid from d_pro_unit_staff WHERE staff_id =@1 AND start_dt <= @4  AND @4 <= end_dt'
                    ELSE '0'
	                END
                ) AS a
            ";
            return _db.Database.SqlQuery<string>(sqlcheckstatus, new MySqlParameter("@1", staff_id),
                   new MySqlParameter("@2", id_actual),
                   new MySqlParameter("@3", start),
                   new MySqlParameter("@4", end)).FirstOrDefault();
        }

        //public ProcessUnitStaff InsertDProUnitStaff(d_pro_unit_staff dprounitstaff,string staffid)
        //{
        //    //_db.d_pro_unit_staff.Add(dprounitstaff);
        //    //_db.SaveChanges();
        //    string getvalue = @"SELECT a.psid,a.staff_id,b.dt_nm AS staff_tp_nm,c.uname, DATE_FORMAT(CAST(a.start_dt AS datetime),'%Y-%m-%d %H:%i:%s')  AS  start_dt,DATE_FORMAT(CAST(a.end_dt AS datetime),'%Y-%m-%d %H:%i:%s') AS end_dt,a.use_yn
        //        FROM d_pro_unit_staff a
        //        LEFT JOIN comm_dt b ON  b.dt_cd=a.staff_tp AND b.mt_cd='COM013' AND b.use_yn='Y'
        //        LEFT JOIN mb_info c ON c.userid=a.staff_id
        //        where a.psid= @1 AND a.staff_id=@2";
        //    return _db.Database.SqlQuery<ProcessUnitStaff>(getvalue, new MySqlParameter("@1", dprounitstaff.psid),
        //     new MySqlParameter("@2", staffid)).FirstOrDefault();
        //}
        public ProcessUnitStaff GetDProUnitStaff(int psid)
        {
            string getvalue = @"SELECT a.psid,a.staff_id,b.dt_nm AS staff_tp_nm,c.uname, DATE_FORMAT(CAST(a.start_dt AS datetime),'%Y-%m-%d %H:%i:%s')  AS  start_dt,DATE_FORMAT(CAST(a.end_dt AS datetime),'%Y-%m-%d %H:%i:%s') AS end_dt,a.use_yn
                FROM d_pro_unit_staff a
                LEFT JOIN comm_dt b ON  b.dt_cd=a.staff_tp AND b.mt_cd='COM013' AND b.use_yn='Y'
                LEFT JOIN mb_info c ON c.userid=a.staff_id
                where a.psid= @1";
            return _db.Database.SqlQuery<ProcessUnitStaff>(getvalue, new MySqlParameter("@1", psid)).FirstOrDefault();
        }

        public IEnumerable<ProcessUnitStaff> GetListProcessStaff(int idactual)
        {
            string getvalue = @"SELECT a.psid,a.staff_id,b.dt_nm AS staff_tp_nm,c.uname, DATE_FORMAT(CAST(a.start_dt AS datetime),'%Y-%m-%d %H:%i:%s')  AS  start_dt,DATE_FORMAT(CAST(a.end_dt AS datetime),'%Y-%m-%d %H:%i:%s') AS end_dt,a.use_yn
                                FROM d_pro_unit_staff a
                                LEFT JOIN comm_dt b ON  b.dt_cd=a.staff_tp AND b.mt_cd='COM013' AND b.use_yn='Y'
                                LEFT JOIN mb_info c ON c.userid=a.staff_id
                                where a.id_actual= @1";
            return _db.Database.SqlQuery<ProcessUnitStaff>(getvalue, new MySqlParameter("@1", idactual)).ToList();
            //return _db.Database.SqlQuery<ProcessUnitStaff>(getvalue, new SqlParameter("@1", idactual)).ToList();
        }



        public IEnumerable<comm_dt> GetListCommDT(string mtcd, string useyn)
        {
            //ORDER BY dt_cd DESC
            string querysql = @"select * from comm_dt 
                WHERE mt_cd=@1 AND use_yn=@2
                ORDER BY dt_cd DESC";
            return _db.Database.SqlQuery<comm_dt>(querysql,
                new MySqlParameter("1", mtcd),
                new MySqlParameter("2", useyn));
        }

        public d_pro_unit_staff GetListDProUnitStaff(int psid)
        {
            string sqlquery = @"SELECT * FROM d_pro_unit_staff WHERE psid=@1";
            return _db.Database.SqlQuery<d_pro_unit_staff>(sqlquery, new MySqlParameter("@1", psid)).FirstOrDefault();
        }
        public int InsertDProUnitStaff(d_pro_unit_staff dprounitstaff)
        {
            string sqlinsert = @"Insert into d_pro_unit_staff(staff_id,staff_tp,use_yn,del_yn,chg_dt,reg_dt,start_dt,end_dt,id_actual) 
                Values(@1,@2,@3,@4,@5,@6,@7,@8,@9);
                SELECT LAST_INSERT_ID();
            ";
            return _db.Database.SqlQuery<int>(sqlinsert, new MySqlParameter("@1", dprounitstaff.staff_id),
                new MySqlParameter("@2", dprounitstaff.staff_tp),
                new MySqlParameter("@3", dprounitstaff.use_yn),
                new MySqlParameter("@4", dprounitstaff.del_yn),
                new MySqlParameter("@5", dprounitstaff.chg_dt),
                new MySqlParameter("@6", dprounitstaff.reg_dt),
                new MySqlParameter("@7", dprounitstaff.start_dt),
                new MySqlParameter("@8", dprounitstaff.end_dt),
                new MySqlParameter("@9", dprounitstaff.id_actual)).FirstOrDefault();
        }
        public string GetStatusdprounitmc(string mcno, int idactual, string start, string end)
        {
            string sqlcheckstatus = @"
                SELECT (
		            CASE
		            WHEN NOT  EXISTS(SELECT * from d_pro_unit_mc where mc_no=@1)
		            THEN '0'
		            WHEN  EXISTS(SELECT * from d_pro_unit_mc WHERE mc_no =@1 AND id_actual=@2 AND start_dt <= @3  AND @3 <= end_dt)
		            THEN '1'
		            WHEN  EXISTS(SELECT * from d_pro_unit_mc WHERE mc_no =@1 AND id_actual=@2 AND start_dt <= @4  AND @4 <= end_dt)
		            THEN '1'
		            WHEN  EXISTS(SELECT * from d_pro_unit_mc WHERE mc_no =@1 AND start_dt <= @3  AND @3 <= end_dt)
		            THEN 'SELECT pmid from d_pro_unit_mc WHERE mc_no =@1   AND start_dt <= @3  AND @3 <= end_dt'
		            WHEN EXISTS(SELECT * from d_pro_unit_mc WHERE mc_no =@1 AND start_dt <= @4  AND @4 <= end_dt)
		            THEN 'SELECT pmid from d_pro_unit_mc WHERE mc_no =@1 AND start_dt <= @4  AND @4 <= end_dt'
                    ELSE '0'
	                END
                ) AS a
            ";
            return _db.Database.SqlQuery<string>(sqlcheckstatus, new MySqlParameter("@1", mcno),
                 new MySqlParameter("@2", idactual),
                  new MySqlParameter("@3", start),
                   new MySqlParameter("@4", end)).FirstOrDefault();
        }

        public d_pro_unit_mc Getdprounitmc(int pmid)
        {
            string sql = @"SELECT * FROM d_pro_unit_mc WHERE pmid=@1";
            return _db.Database.SqlQuery<d_pro_unit_mc>(sql, new MySqlParameter("@1", pmid)).FirstOrDefault();
        }
        public int InsertwMaterialMapping(w_material_mapping item)
        {
            string sqlinsert = @"INSERT INTO w_material_mapping(mt_lot,mt_no,mt_cd,mapping_dt,bb_no,remark,use_yn,chg_dt,reg_id,chg_id,reg_dt,sts_share,del_yn) 
            VALUES(@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13);
            SELECT LAST_INSERT_ID();";
            return _db.Database.SqlQuery<int>(sqlinsert, new MySqlParameter("@1", item.mt_lot),
                new MySqlParameter("@2", item.mt_no),
                new MySqlParameter("@3", item.mt_cd),
                new MySqlParameter("@4", item.mapping_dt),
                new MySqlParameter("@5", item.bb_no),
                new MySqlParameter("@6", item.remark),
                new MySqlParameter("@7", item.use_yn),
                new MySqlParameter("@8", item.chg_dt),
                new MySqlParameter("@9", item.reg_id),
                new MySqlParameter("@10", item.chg_id),
                new MySqlParameter("@11", item.reg_dt),
                new MySqlParameter("@12", item.sts_share),
                new MySqlParameter("@13", item.del_yn)).FirstOrDefault();
        }
        public MaterialInfoMMS GetWMaterialInfowithmtcd(string mt_cd)
        {
            string QuerySQL = "SELECT * FROM w_material_info WHERE mt_cd = @1";
            return _db.Database.SqlQuery<MaterialInfoMMS>(QuerySQL, new MySqlParameter("1", mt_cd)).FirstOrDefault();
        }

        public void UpdateMaterialInfo(MaterialInfoMMS item)
        {
            string sqlupdate = @"Update w_material_info SET id_actual=@1,id_actual_oqc=@2,at_no=@3,product=@4,staff_id=@5,
            staff_id_oqc=@6,machine_id=@7,mt_type=@8,mt_cd=@9,mt_no=@10,gr_qty=@11,real_qty=@12,staff_qty=@13,sp_cd=@14,
            rd_no=@15,sd_no=@16,ext_no=@17,ex_no=@18,dl_no=@19,recevice_dt=@20,date=@21,return_date=@22,alert_NG=@23,expiry_dt=@24,
            dt_of_receipt=@25,expore_dt=@26,recevice_dt_tims=@27,rece_wip_dt=@28,picking_dt=@29,shipping_wip_dt=@30,
            end_production_dt=@31,lot_no=@32,mt_barcode=@33,mt_qrcode=@34,mt_sts_cd=@35,bb_no=@36,bbmp_sts_cd=@37,
            lct_cd=@38,lct_sts_cd=@39,from_lct_cd=@40,to_lct_cd=@41,output_dt=@42,input_dt=@43,buyer_qr=@44,orgin_mt_cd=@45,
            remark=@46,sts_update=@47,use_yn=@48,reg_id=@49,reg_dt=@50,chg_id=@51,chg_dt=@52
            WHERE wmtid=@0";
            _db.Database.ExecuteSqlCommand(sqlupdate, new MySqlParameter("@0", item.wmtid), new MySqlParameter("@1", item.id_actual),
                new MySqlParameter("@2", item.id_actual_oqc), new MySqlParameter("@3", item.at_no), new MySqlParameter("@4", item.product),
                new MySqlParameter("@5", item.staff_id), new MySqlParameter("@6", item.staff_id_oqc), new MySqlParameter("@7", item.machine_id),
                new MySqlParameter("@8", item.mt_type), new MySqlParameter("@9", item.mt_cd), new MySqlParameter("@10", item.mt_no),
                new MySqlParameter("@11", item.gr_qty), new MySqlParameter("@12", item.real_qty), new MySqlParameter("@13", item.staff_qty),
                new MySqlParameter("@14", item.sp_cd), new MySqlParameter("@15", item.rd_no), new MySqlParameter("@16", item.sd_no),
                new MySqlParameter("@17", item.ext_no), new MySqlParameter("@18", item.ex_no), new MySqlParameter("@19", item.dl_no),
                new MySqlParameter("@20", item.recevice_dt), new MySqlParameter("@21", item.date), new MySqlParameter("@22", item.return_date),
                new MySqlParameter("@23", item.alert_NG), new MySqlParameter("@24", item.expiry_dt), new MySqlParameter("@25", item.dt_of_receipt),
                new MySqlParameter("@26", item.expore_dt), new MySqlParameter("@27", item.recevice_dt_tims), new MySqlParameter("@28", item.rece_wip_dt),
                new MySqlParameter("@29", item.picking_dt), new MySqlParameter("@30", item.shipping_wip_dt), new MySqlParameter("@31", item.end_production_dt),
                new MySqlParameter("@32", item.lot_no), new MySqlParameter("@33", item.mt_barcode), new MySqlParameter("@34", item.mt_qrcode),
                new MySqlParameter("@35", item.mt_sts_cd), new MySqlParameter("@36", item.bb_no), new MySqlParameter("@37", item.bbmp_sts_cd),
                new MySqlParameter("@38", item.lct_cd), new MySqlParameter("@39", item.lct_sts_cd), new MySqlParameter("@40", item.from_lct_cd),
                new MySqlParameter("@41", item.to_lct_cd), new MySqlParameter("@42", item.output_dt), new MySqlParameter("@43", item.input_dt),
                new MySqlParameter("@44", item.buyer_qr), new MySqlParameter("@45", item.orgin_mt_cd), new MySqlParameter("@46", item.remark),
                new MySqlParameter("@47", item.sts_update), new MySqlParameter("@48", item.use_yn), new MySqlParameter("@49", item.reg_id),
                new MySqlParameter("@50", item.reg_dt), new MySqlParameter("@51", item.chg_id), new MySqlParameter("@52", item.chg_dt));
        }
        public w_material_info_wo Getwmaterialinfowo(string mt_no, string bbmp_sts_cd, string mt_cd, int idWMaterialInfo)
        {
            string sqlinfowo = @"SELECT m.wmtid,
                    DATE_FORMAT(CAST(m.reg_dt AS datetime),'%Y-%m-%d %H:%i:%s') as reg_dt,
                    m.mt_cd,
                    @1 AS mt_no,
                    (Select cmmdt.dt_nm FROM comm_dt cmmdt where cmmdt.mt_cd='MMS007' AND cmmdt.dt_cd=@2)  AS bbmp_sts_cd,
                    m.gr_qty,
                    m.mt_qrcode,
                    a.lct_nm AS lct_cd,
                    m.bb_no,
                    m.mt_barcode,
                    (SELECT count(*) FROM w_material_mapping where mt_lot=@3) AS count_table2
                    FROM w_material_info m
                    LEFT JOIN lct_info a ON m.lct_cd = a.lct_cd
                    where m.wmtid = @4";
            return _db.Database.SqlQuery<w_material_info_wo>(sqlinfowo, new MySqlParameter("@1", mt_no),
                new MySqlParameter("@2", bbmp_sts_cd),
                new MySqlParameter("@3", mt_cd),
                new MySqlParameter("@4", idWMaterialInfo)).FirstOrDefault();
        }
        public int Getsumgrqty(string mt_cd)
        {
            string sqlsum = @"SELECT SUM(gr_qty) FROM w_material_info WHERE mt_cd LIKE @1";
            return _db.Database.SqlQuery<int>(sqlsum, new MySqlParameter("@1", "%" + mt_cd + "%")).FirstOrDefault();
        }
        public IEnumerable<wo_info_mc_wk_mold> Getwoinfomcwkmold(int idActual)
        {
            string sqlquery = @"SELECT a.id_actual,
                a.mc_no code,
                a.use_yn,a.pmid, 
                '' AS staff_tp, 
                DATE_FORMAT(CAST(a.start_dt AS datetime),'%Y-%m-%d %H:%i:%s') as start_dt, 
                DATE_FORMAT(CAST(a.end_dt AS datetime),'%Y-%m-%d %H:%i:%s') as end_dt, 
                (CASE 
	                WHEN (SELECT COUNT(*) FROM d_machine_info WHERE mc_no=a.mc_no) >0 THEN 'machine' 
	                WHEN (SELECT COUNT(*) FROM d_mold_info WHERE md_no=a.mc_no) >0 THEN 'mold' 
                END)type, 
                (CASE 
	                WHEN (SELECT COUNT(*) FROM d_machine_info WHERE mc_no=a.mc_no) >0 THEN (SELECT mc_nm FROM d_machine_info WHERE mc_no=a.mc_no) 
	                WHEN (SELECT COUNT(*) FROM d_mold_info WHERE md_no=a.mc_no) >0 THEN (SELECT md_nm FROM d_mold_info WHERE md_no=a.mc_no) 
                END)name, 
                 (case when NOW() BETWEEN  DATE_FORMAT(a.start_dt, '%Y-%m-%d %H:%i:%s') AND DATE_FORMAT(a.end_dt, '%Y-%m-%d %H:%i:%s') then 'HT' ELSE 'QK' END)het_ca 
                from d_pro_unit_mc AS a 
                WHERE a.id_actual=@1 
                UNION 
                SELECT id_actual,
                staff_id code,
                use_yn,psid,  
                staff_tp, 
                DATE_FORMAT(CAST(start_dt AS datetime),'%Y-%m-%d %H:%i:%s') as start_dt, 
                DATE_FORMAT(CAST(end_dt AS datetime),'%Y-%m-%d %H:%i:%s') as end_dt, 
                 ('worker')type,
                 (SELECT uname FROM mb_info WHERE userid=staff_id) name ,
                (case when NOW() BETWEEN  DATE_FORMAT(d_pro_unit_staff.start_dt, '%Y-%m-%d %H:%i:%s') AND DATE_FORMAT(d_pro_unit_staff.end_dt, '%Y-%m-%d %H:%i:%s') then 'HT' ELSE 'QK' END)het_ca 
                FROM d_pro_unit_staff 
                WHERE id_actual=@1";
            return _db.Database.SqlQuery<wo_info_mc_wk_mold>(sqlquery, new MySqlParameter("1", idActual));
        }
        public void Updatedprounitstaff(d_pro_unit_staff item)
        {
            string sqlupdate = @"UPDATE d_pro_unit_staff SET staff_id=@1,actual=@2,defect=@3,
                id_actual=@4,staff_tp=@5,start_dt=@6,end_dt=@7,
                use_yn=@8,del_yn=@9,reg_id=@10,reg_dt=@11,chg_id=@12,
                chg_dt=@13 WHERE psid=@14";
            _db.Database.ExecuteSqlCommand(sqlupdate, new MySqlParameter("@1", item.staff_id), new MySqlParameter("@2", item.actual),
            new MySqlParameter("@3", item.defect), new MySqlParameter("@4", item.id_actual), new MySqlParameter("@5", item.staff_tp),
            new MySqlParameter("@6", item.start_dt), new MySqlParameter("@7", item.end_dt), new MySqlParameter("@8", item.use_yn),
            new MySqlParameter("@9", item.del_yn), new MySqlParameter("@10", item.reg_id), new MySqlParameter("@11", item.reg_dt),
            new MySqlParameter("@12", item.chg_id), new MySqlParameter("@13", item.chg_dt), new MySqlParameter("@14", item.psid));
        }
        public int GettotalcountProcessUnitStaff(string staff_id, int psid)
        {
            try
            {
                string sqlquery = @"SELECT COUNT(*) FROM d_pro_unit_staff WHERE staff_id=@1 AND psid != @2";
                return _db.Database.SqlQuery<int>(sqlquery, new MySqlParameter("@1", staff_id), new MySqlParameter("@2", psid)).FirstOrDefault();
            }
            catch (Exception e)
            {
                return 0;
            }

        }
        public void Updatedprounitmc(d_pro_unit_mc item)
        {
            try
            {
                string sqlquery = @"UPDATE d_pro_unit_mc SET id_actual=@1,start_dt=@2,end_dt=@3,
            remark=@4,mc_no=@5,use_yn=@6,del_yn=@7,reg_id=@8,reg_dt=@9,chg_id=@10,chg_dt=@11 WHERE pmid=@12";
                _db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("@1", item.id_actual),
                    new MySqlParameter("@2", item.start_dt), new MySqlParameter("@3", item.end_dt),
                    new MySqlParameter("@4", item.remark), new MySqlParameter("@5", item.mc_no),
                    new MySqlParameter("@6", item.use_yn), new MySqlParameter("@7", item.del_yn),
                    new MySqlParameter("@8", item.reg_id), new MySqlParameter("@9", item.reg_dt),
                    new MySqlParameter("@10", item.chg_id), new MySqlParameter("@11", item.chg_dt),
                    new MySqlParameter("@12", item.pmid));
            }
            catch (Exception e)
            {

            }

        }
        public int Gettotalcountdprounitmc(string mc_no, int id_actual, int pmid)
        {
            string sqlquery = @"SELECT COUNT(*) FROM d_pro_unit_mc WHERE mc_no=@1 AND id_actual=@2 AND pmid != @3";
            return _db.Database.SqlQuery<int>(sqlquery, new MySqlParameter("@1", mc_no), new MySqlParameter("@2", id_actual),
                new MySqlParameter("@3", pmid)).FirstOrDefault();
        }
        public int GetStatusdprounitmcupdate(string mcno, int idactual, int pmid, string start, string end)
        {
            //string sqlcheckstatus = @"SELECT COUNT(*) FROM d_pro_unit_mc WHERE mc_no=@1 AND id_actual=@2 AND pmid != @3 AND ((start_dt <= @3  AND @3 <= end_dt) OR (start_dt <= @4  AND @4 <= end_dt));";
            string sqlcheckstatus = @"SELECT COUNT(*) FROM d_pro_unit_mc WHERE mc_no=@1 AND id_actual=@2 AND pmid != @3
            AND ((start_dt <= @4  AND @4 <= end_dt) OR (start_dt <= @5  AND @5 <= end_dt)) ";
            return _db.Database.SqlQuery<int>(sqlcheckstatus, new MySqlParameter("@1", mcno),
                 new MySqlParameter("@2", idactual),
                 new MySqlParameter("@3", pmid),
                  new MySqlParameter("@4", start),
                   new MySqlParameter("@5", end)).FirstOrDefault();
        }
        public IEnumerable<ProcessMachineunit> Getlistdprounitmcfromidactual(int id_actual)
        {
            string sqlquery = @"SELECT pmid,id_actual,DATE_FORMAT(CAST(start_dt AS datetime),'%Y-%m-%d %H:%i:%s') as start_dt,
            DATE_FORMAT(CAST(end_dt AS datetime), '%Y-%m-%d %H:%i:%s') as end_dt,remark,mc_no,use_yn FROM d_pro_unit_mc WHERE id_actual=@1";
            //string sqlquery = @"SELECT pmid,id_actual,DATE_FORMAT(CAST(start_dt AS datetime),'%Y-%m-%d %H:%i:%s') as start_dt,
            //DATE_FORMAT(CAST(end_dt AS datetime), '%Y-%m-%d %H:%i:%s') as end_dt,remark,mc_no,use_yn,del_yn,reg_id,reg_dt,chg_id,chg_dt FROM d_pro_unit_mc WHERE id_actual=@1";
            return _db.Database.SqlQuery<ProcessMachineunit>(sqlquery, new MySqlParameter("1", id_actual));
        }
        public IEnumerable<DatawActualPrimaryResponse> GetDatawActualPrimary(string product, string product_name, string model, string at_no, string regstart, string regend, int start, int end)
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
                        b.style_nm AS style_nm ,
                        a.IsApply
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
	                        a.finish_yn IN ( 'N')
	                        AND ( @1 = '' OR a.product LIKE @2 ) 
	                        AND ( @3 = '' OR b.style_nm LIKE @4 ) 
	                        AND ( @5 = '' OR b.md_cd LIKE @6) 
	                        AND ( @7 = '' OR a.at_no LIKE @8 ) 
                            AND ((@9 ='' AND @10='' ) OR (DATE_FORMAT( a.reg_dt, '%Y/%m/%d' ) BETWEEN DATE_FORMAT( @9, '%Y/%m/%d' ) AND DATE_FORMAT( @10, '%Y/%m/%d' )))
                ) mydevice";
            return _db.Database.SqlQuery<DatawActualPrimaryResponse>(sqlquery, new MySqlParameter("1", product == null ? "" : product),
                new MySqlParameter("2", "%" + product + "%"), new MySqlParameter("3", product_name == null ? "" : product_name),
                new MySqlParameter("4", "%" + product_name + "%"), new MySqlParameter("5", model == null ? "" : model),
                new MySqlParameter("6", "%" +model + "%"),new MySqlParameter("7", at_no == null ? "" : at_no),
                new MySqlParameter("8", "%" + at_no + "%"),new MySqlParameter("9", regstart == null ? "" : regstart),
                new MySqlParameter("10", regend == null ? "" : regend));
        }

        public int GetTotalcountDatawActualPrimary(string product, string product_name, string model, string at_no, string regstart, string regend)
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
	                        GROUP BY
		                        b.at_no 
	                        ) AS subquery ON subquery.at_no = a.at_no 
                        WHERE
	                        a.finish_yn IN ( 'N', 'YT' ) 
	                        AND ( @1 = '' OR a.product LIKE @2 ) 
	                        AND ( @3 = '' OR b.style_nm LIKE @4 ) 
	                        AND ( @5 = '' OR b.md_cd LIKE @6) 
	                        AND ( @7 = '' OR a.at_no LIKE @8 ) 
                            AND ((@9 ='' AND @10='' ) OR (DATE_FORMAT( a.reg_dt, '%Y/%m/%d' ) BETWEEN DATE_FORMAT( @9, '%Y/%m/%d' ) AND DATE_FORMAT( @10, '%Y/%m/%d' )))";
            return _db.Database.SqlQuery<int>(sqlquery, new MySqlParameter("1", product == null ? "" : product),
                new MySqlParameter("2", "%" + product == null ? "" : product + "%"), new MySqlParameter("3", product_name == null ? "" : product_name),
                new MySqlParameter("4", "%" + product_name == null ? "" : product_name + "%"), new MySqlParameter("5", model == null ? "" : model),
                new MySqlParameter("6", "%" + model == null ? "" : model + "%"), new MySqlParameter("7", at_no == null ? "" : at_no),
                new MySqlParameter("8", "%" + at_no == null ? "" : at_no + "%"), new MySqlParameter("9", regstart == null ? "" : regstart),
                new MySqlParameter("10", regend == null ? "" : regend)).FirstOrDefault();
        }
        public IEnumerable<DatawActual> GetDatawActual(string at_no)
        {
            //string sqlquery = @"CALL spMMS_GetTIMSActualInfo(@1,'','');";
            string sqlquery = @"CALL SPGetActualInfo(@1,'','');";
            return _db.Database.SqlQuery<DatawActual>(sqlquery, new MySqlParameter("@1", at_no));
        }

        public IEnumerable<DatawActualPrimaryResponse> GetDatawActualPrimaryFromSP(string product, string product_name, string model, string at_no, string regstart, string regend, string mms)
        {
            string sqlquery = @"Call GetData_wActual_Primary(@1,@2,@3,@4,@5,@6,@7);";
            return _db.Database.SqlQuery<DatawActualPrimaryResponse>(sqlquery, new MySqlParameter("1", product==null?"": product), new MySqlParameter("2", product_name == null ? "" : product_name), new MySqlParameter("3", model == null ? "" : model), new MySqlParameter("4", at_no == null ? "" : at_no)
                , new MySqlParameter("5", regstart == null ? "" : regstart), new MySqlParameter("6", regend == null ? "" : regend), new MySqlParameter("7", mms));
        }
        public IEnumerable<WMaterialInfoAPI> GetDetailActualAPI(int idActual, string mtType)
        {
            string sqlquery = @"SELECT	wmtid,mt_cd,gr_qty,bb_no,real_qty AS sl_tru_ng,DATE_FORMAT( CAST( reg_dt AS datetime ),'%Y-%m-%d %H:%i:%s') reg_dt,DATE_FORMAT( CAST( reg_dt AS datetime ),'%Y-%m-%d') reg_date,
            (CASE 
	            WHEN ('08:00:00' <= DATE_FORMAT( CAST( reg_dt AS datetime ),'%H:%i:%s') AND   DATE_FORMAT( CAST( reg_dt AS datetime ),'%H:%i:%s')  <=  '20:00:00') THEN
		            'CN'
	            ELSE
		            'CD'
            END)  as ca FROM w_material_info WHERE id_actual = @1 AND mt_type = @2";
            return _db.Database.SqlQuery<WMaterialInfoAPI>(sqlquery, new MySqlParameter("1", idActual), new MySqlParameter("2", mtType));
        }
        public IEnumerable<DMachineInfoAPI> GetPopupMachine(string mc_type, string mc_no, string mc_nm)
        {
            //string sqlquery = @"SET sql_mode = '';SET @@sql_mode = '';
            //SELECT* FROM(SELECT a.*, 
            //(CASE 
            //    WHEN b.mc_no IS NOT NULL THEN 'mapping' 
            //    ELSE 'Unmapping' 
            //END) su_dung
            //FROM d_machine_info AS a 
            //LEFT JOIN d_pro_unit_mc AS b ON a.mc_no=b.mc_no WHERE (@1='' OR   a.mc_type like @2) and (@3='' OR   a.mc_no like @4) and (@5='' OR   a.mc_nm like @6)  
            //GROUP BY a.mno ORDER BY su_dung='Unmapping' desc)MyDerivedTable ORDER BY MyDerivedTable.mc_no DESC";
            string sqlquery = @"SET sql_mode = '';SET @@sql_mode = '';
                SELECT * FROM(SELECT a.mno, ANY_VALUE(a.mc_type) mc_type, ANY_VALUE(a.mc_no) mc_no,
		ANY_VALUE(a.mc_nm) mc_nm, ANY_VALUE(a.purpose) purpose, ANY_VALUE(a.color) color, ANY_VALUE(a.barcode) barcode
		, ANY_VALUE(a.re_mark) re_mark, ANY_VALUE(a.use_yn) use_yn, ANY_VALUE(a.del_yn) del_yn, ANY_VALUE(a.reg_id) reg_id
		, ANY_VALUE(a.reg_dt) reg_dt, ANY_VALUE(a.chg_id) chg_id, ANY_VALUE(a.chg_dt) chg_dt
		, ANY_VALUE(CASE 
                                    WHEN c.end_dt IS NULL THEN 'Unmapping'
								                    WHEN  NOW()> DATE_FORMAT(CAST(c.end_dt AS datetime),'%Y-%m-%d %H:%i:%s') THEN 'Unmapping'
								                    WHEN NOW() < DATE_FORMAT(CAST(c.end_dt AS datetime),'%Y-%m-%d %H:%i:%s') THEN 'Mapping'
                                    ELSE 'Unmapping' 
                                END) su_dung
                                FROM d_machine_info AS a 
                                LEFT JOIN (select b.mc_no,max(b.end_dt) AS end_dt
                    from d_pro_unit_mc b
                    GROUP BY b.mc_no) AS c ON a.mc_no=c.mc_no WHERE (@1='' OR   a.mc_type like @2) and (@3='' OR   a.mc_no like @4) and (@5='' OR   a.mc_nm like @6)  
                    GROUP BY a.mno 
                    ORDER BY su_dung='Unmapping' desc)MyDerivedTable ORDER BY MyDerivedTable.mc_no DESC";
            return _db.Database.SqlQuery<DMachineInfoAPI>(sqlquery, new MySqlParameter("1", mc_type),
                new MySqlParameter("2", "%" + mc_type + "%"), new MySqlParameter("3", mc_no), new MySqlParameter("4", "%" + mc_no + "%"),
                new MySqlParameter("5", mc_nm), new MySqlParameter("6", "%" + mc_nm + "%"));
        }

        public IEnumerable<MtDateWebWO> GetMtDateWebs(int id_actual)
        {
            string sqlquery = @"SELECT
                'PC00004A' AS QC,
	            a.id_actual,
	            a.staff_id,
	           DATE_FORMAT(CAST(a.reg_dt AS datetime),'%Y-%m-%d %H:%i:%s') AS reg_dt,
	            a.wmtid,
	            DATE_FORMAT(CAST(a.date AS datetime),'%Y-%m-%d') AS DATE ,
	            a.mt_cd,
	            a.mt_no,
	            a.gr_qty,
	            b.dt_nm AS bbmp_sts_cd,
	            a.mt_qrcode,
	            a.bb_no,
	            DATE_FORMAT(CAST(a.chg_dt AS datetime),'%Y-%m-%d %H:%i:%s') AS chg_dt,
	            IFNULL( a.real_qty - d.sltrung, a.real_qty ) sl_tru_ng,
	            IFNULL(c.count_table2,0) AS count_table2,
                (SELECT COUNT(*) FROM d_pro_unit_staff WHERE id_actual=@1 AND a.staff_id LIKE CONCAT('%',staff_id,'%') and (NOW() BETWEEN  Date_format(start_dt, '%Y-%m-%d %H:%i:%s')  and DATE_FORMAT(end_dt, '%Y-%m-%d %H:%i:%s')) AND (a.reg_dt BETWEEN  Date_format(start_dt, '%Y-%m-%d %H:%i:%s') and DATE_FORMAT(end_dt, '%Y-%m-%d %H:%i:%s')))het_ca 
	            /*IF(IFNULL(e.staff_id,'0') !='0',1,0) AS het_ca*/
            FROM
	            w_material_info AS a 
            LEFT JOIN comm_dt b ON b.dt_cd = a.bbmp_sts_cd AND  b.mt_cd = 'MMS007' 
            LEFT JOIN (SELECT COUNT(mt_lot) AS count_table2,mt_lot FROM w_material_mapping 
            GROUP BY mt_lot
            ) c ON c.mt_lot = a.mt_cd
            LEFT JOIN (
            SELECT ( check_qty - ok_qty ) AS sltrung,ml_no  FROM m_facline_qc WHERE ml_tims IS NULL
            ) d ON d.ml_no = a.mt_cd
            /*LEFT JOIN(
	            SELECT
			            staff_id,start_dt,end_dt
		            FROM
			            d_pro_unit_staff 
		            WHERE
			            id_actual = @1	
			            AND (NOW() BETWEEN Date_format( start_dt, '%Y-%m-%d %H:%i:%s' ) AND DATE_FORMAT( end_dt, '%Y-%m-%d %H:%i:%s' ))
            ) e ON a.staff_id LIKE CONCAT('%',e.staff_id,'%') AND (Date_format(a.reg_dt, '%Y-%m-%d %H:%i:%s')  BETWEEN  Date_format(e.start_dt, '%Y-%m-%d %H:%i:%s') 
             and DATE_FORMAT(e.end_dt, '%Y-%m-%d %H:%i:%s'))*/

            WHERE
	            a.id_actual = @1 AND a.mt_type = 'CMT' AND a.orgin_mt_cd IS NULL
            ORDER BY a.wmtid DESC";
            return _db.Database.SqlQuery<MtDateWebWO>(sqlquery, new MySqlParameter("1", id_actual));
        }
        public int CheckHetCaMT(int id_actual, string staff_id)
        {
            string sqlcheckca = @"SELECT COUNT(*) FROM d_pro_unit_staff AS a 
                WHERE a.id_actual=@1 and @2 like concat('%',a.staff_id,'%')  AND (NOW() BETWEEN 
                Date_format(a.start_dt, '%Y-%m-%d %H:%i:%s') 
                    AND Date_format(a.end_dt, '%Y-%m-%d %H:%i:%s'))";
            return _db.Database.SqlQuery<int>(sqlcheckca, new MySqlParameter("1", id_actual), new MySqlParameter("2", staff_id)).FirstOrDefault();
        }
        public IEnumerable<CheckUpdateGrty> GetUpdateGrty(int wmtid)
        {
            string sqlquery = @"SELECT a.wmtid,
                    DATE_FORMAT(CAST(a.date AS datetime),'%Y-%m-%d') as date,
                    a.mt_cd,
                    a.mt_no,
                    a.gr_qty,
                    a.bb_no,
                    a.mt_barcode,
                    a.chg_dt,
                    IFNULL(b.gr_qty,0) AS sl_tru_ng,
                    IFNULL(c.count_table2,0) AS count_table2
                    FROM w_material_info a
                    LEFT JOIN (SELECT SUM(gr_qty) AS gr_qty,orgin_mt_cd,mt_cd FROM w_material_info GROUP BY orgin_mt_cd,mt_cd) b ON b.orgin_mt_cd=a.mt_cd AND b.mt_cd LIKE CONCAT('%',a.mt_cd,'-RT','%')
                    LEFT JOIN(SELECT count(*) AS count_table2,mt_lot FROM w_material_mapping GROUP BY(mt_lot)) c ON c.mt_lot=a.mt_cd
                    WHERE a.wmtid = @1 
                    ORDER BY a.reg_dt DESC";
            return _db.Database.SqlQuery<CheckUpdateGrty>(sqlquery, new MySqlParameter("1", wmtid));
        }
        public int CallSPUpdateSLwactual(int id_actual)
        {
            string sqlquery = @"CALL Update_SL_w_actual(@1)";
            return _db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", id_actual));
        }
        public int CheckwMaterialMapping(string mt_cd)
        {
            string sqlquery = @"SELECT COUNT(*) FROM w_material_mapping WHERE mt_cd =@1";
            return _db.Database.SqlQuery<int>(sqlquery, new MySqlParameter("1", mt_cd)).FirstOrDefault();
        }
        public int Checkwmfaclineqc(string mt_cd)
        {
            string sqlquery = @"SELECT COUNT(*) FROM m_facline_qc WHERE ml_no =@1";
            return _db.Database.SqlQuery<int>(sqlquery, new MySqlParameter("1", mt_cd)).FirstOrDefault();
        }
        public int Statusmappingfrommtlot(string mt_cd)
        {
            string sqlquery = @"SELECT COUNT(*) FROM w_material_mapping WHERE mt_lot=@1 AND use_yn='N'";
            return _db.Database.SqlQuery<int>(sqlquery, new MySqlParameter("1", mt_cd)).FirstOrDefault();
        }
        public IEnumerable<w_material_mapping> GetWMaterialMappingfrommtlot(string mt_cd)
        {
            string sqlquery = @"SELECT * FROM w_material_mapping WHERE mt_lot=@1";
            return _db.Database.SqlQuery<w_material_mapping>(sqlquery, new MySqlParameter("1", mt_cd));
        }
        public w_material_mapping Getw_material_mappingfromwmmid(int wmmid)
        {
            try
            {
                string sqlquery = @"SELECT * FROM w_material_mapping WHERE wmmid=@1";
                return _db.Database.SqlQuery<w_material_mapping>(sqlquery, new MySqlParameter("@1", wmmid)).FirstOrDefault();
            }
            catch (Exception e)
            {
                return new w_material_mapping();
            }
        }
        public MaterialInfoMMS Getw_material_infoforDelPP(string mt_cd, string mt_type)
        {
            string sqlquery = @"SELECT * FROM w_material_info WHERE mt_cd=@1 AND mt_type !=@2";
            return _db.Database.SqlQuery<MaterialInfoMMS>(sqlquery, new MySqlParameter("1", mt_cd), new MySqlParameter("2", mt_type)).FirstOrDefault();
        }
        public int Deletew_material_info(int wmtid)
        {
            string sqlquery = @"DELETE FROM w_material_info WHERE wmtid=@1";
            return _db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", wmtid));
        }
        public int GetcountwmaterialmappingFrmtcdamtlot(string mt_cd, string mt_lot)
        {
            string sqlquery = @"SELECT COUNT(*) FROM w_material_mapping WHERE mt_cd=@1 AND mt_lot != @2";
            return _db.Database.SqlQuery<int>(sqlquery, new MySqlParameter("1", mt_cd), new MySqlParameter("2", mt_lot)).FirstOrDefault();
        }
        public d_bobbin_lct_hist GetdbobbinlcthistFrbbnoamtcd(string bb_no, string mt_cd)
        {
            string sqlquery = @"SELECT * FROM d_bobbin_lct_hist WHERE bb_no=@1 AND mt_cd=@2";
            return _db.Database.SqlQuery<d_bobbin_lct_hist>(sqlquery, new MySqlParameter("1", bb_no), new MySqlParameter("2", mt_cd)).FirstOrDefault();
        }
        public void Deletedbobbinlcthist(int blno)
        {
            string sqlquery = @"DELETE FROM d_bobbin_lct_hist WHERE blno=@1";
            _db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", blno));
        }
        public IEnumerable<DataMappingW> GetDataMappingW(string mt_cd)
        {
            string sqlquery = @"SELECT
	            a.wmmid,
	            a.mt_lot,
	            a.mt_cd,
	            b.mt_type,
	            DATE_FORMAT( a.mapping_dt, '%Y-%m-%d %H:%i:%s' ) mapping_dt,
	            a.use_yn,
	            DATE_FORMAT( a.reg_dt, '%Y-%m-%d %H:%i:%s' ) reg_dt,
	            b.gr_qty,
	            b.mt_no,
	            b.bb_no,
	            ( CASE WHEN a.use_yn = 'N' THEN b.gr_qty ELSE 0 END ) Used,(
	            CASE
			
			            WHEN a.use_yn = 'N' THEN
			            Find_Remain ( a.mt_cd, b.orgin_mt_cd, a.reg_dt ) ELSE 0 
		            END 
		            ) Remain 
	            FROM
		            w_material_mapping AS a
	            LEFT JOIN w_material_info AS b ON a.mt_cd = b.mt_cd 
	            WHERE
		            a.mt_lot = @1 
	            ORDER BY
	            a.use_yn = 'Y' DESC,
              
	            a.mt_no DESC";
            return _db.Database.SqlQuery<DataMappingW>(sqlquery, new MySqlParameter("1", mt_cd));
        }
        public int CheckwmaterialmappingFinish(string mt_cd, string mt_lot, string mapping_dt)
        {
            string sqlquery = @"SELECT COUNT(*) FROM w_material_mapping WHERE mt_cd=@1 AND mt_lot != @2 AND mapping_dt > @3";
            return _db.Database.SqlQuery<int>(sqlquery, new MySqlParameter("1", mt_cd), new MySqlParameter("2", mt_lot), new MySqlParameter("3", mapping_dt)).FirstOrDefault();
        }
        public IEnumerable<DSMappingSTA> GetDSMappingSta(string mt_cd)
        {
            string sqlquery = @"SELECT a.wmtid,
            DATE_FORMAT(a.date,'%Y-%m-%d') as date,
            a.mt_cd,a.mt_no,a.real_qty AS gr_qty,a.real_qty AS gr_qty1,b.dt_nm as bbmp_sts_cd,
            a.mt_qrcode,
            (SELECT lct_nm FROM lct_info WHERE lct_nm LIKE 'FAC%' AND lct_cd=a.lct_cd LIMIT 1) AS lct_cd
            ,a.bb_no,a.mt_barcode,Date_format(a.chg_dt, '%Y-%m-%d %H:%i:%s') AS chg_dt,a.gr_qty AS real_qty,a.real_qty AS sl_tru_ng
            FROM w_material_info a
            LEFT JOIN comm_dt b ON a.bbmp_sts_cd=b.dt_cd AND b.mt_cd='MMS007'
            WHERE a.mt_cd LIKE @1 AND a.mt_sts_cd != '003'  ORDER BY a.gr_qty DESC ";
            return _db.Database.SqlQuery<DSMappingSTA>(sqlquery, new MySqlParameter("@1", mt_cd + "%"));
        }

        public MaterialInfoMMS GetmaterialinfoforDevice(string mt_cd, string sts_update, string mt_sts_cd, string lct_sts_cd)
        {
            string sqlquery = @"SELECT * FROM w_material_info WHERE mt_cd=@1 AND sts_update=@2 AND mt_sts_cd=@3 AND gr_qty > 0 AND lct_sts_cd=@4";
            return _db.Database.SqlQuery<MaterialInfoMMS>(sqlquery, new MySqlParameter("@1", mt_cd), new MySqlParameter("@2", sts_update),
              new MySqlParameter("@3", mt_sts_cd), new MySqlParameter("@4", lct_sts_cd)).FirstOrDefault();
        }
        public int GetDProUnitStaffforDevice(int id_actual)
        {
            string sqlquery = @"SELECT COUNT(*) FROM d_pro_unit_staff AS a 
            WHERE a.id_actual=@1 AND (NOW() BETWEEN 
            Date_format(a.start_dt, '%Y-%m-%d %H:%i:%s') 
             AND Date_format(a.end_dt, '%Y-%m-%d %H:%i:%s'))";
            return _db.Database.SqlQuery<int>(sqlquery, new MySqlParameter("1", id_actual)).FirstOrDefault();
        }
        public void UpdatedBobbinInfoforDevice(string bb_no, string mt_cd, string chg_id)
        {
            string sqlquery = @"UPDATE d_bobbin_info SET chg_id=@3,mt_cd =NUll WHERE bb_no=@1 AND mt_cd=@2";
            _db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", bb_no), new MySqlParameter("2", mt_cd), new MySqlParameter("3", chg_id));
        }
        public void DeleteDBobbinLctHistforDevice(string bb_no, string mt_cd)
        {
            string sqlquery = @"DELETE FROM d_bobbin_lct_hist WHERE bb_no=@1 AND mt_cd=@2";
            _db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", bb_no), new MySqlParameter("2", mt_cd));
        }
        public int GetTotalwMaterialInfoDV(string mt_cd)
        {
            string sqlquery = @"SELECT COUNT(*) FROM w_material_info WHERE mt_cd LIKE @1";
            return _db.Database.SqlQuery<int>(sqlquery, new MySqlParameter("1", "%" + mt_cd + "%")).FirstOrDefault();
        }
        public void Updatedbobbinlcthist(d_bobbin_lct_hist item)
        {
            string sqlquery = @"UPDATE d_bobbin_lct_hist SET mc_type=@1,bb_no=@2,mt_cd=@3,bb_nm=@4,start_dt=@5,end_dt=@6,use_yn=@7,del_yn=@8,reg_id=@9,reg_dt=@10,chg_id=@11,chg_dt=@12 WHERE blno=@13";
            _db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("@1", item.mc_type), new MySqlParameter("@2", item.bb_no)
                , new MySqlParameter("@3", item.mt_cd), new MySqlParameter("@4", item.bb_nm), new MySqlParameter("@5", item.start_dt), new MySqlParameter("@6", item.end_dt)
                , new MySqlParameter("@7", item.use_yn), new MySqlParameter("@8", item.del_yn), new MySqlParameter("@9", item.reg_id)
                , new MySqlParameter("@10", item.reg_dt), new MySqlParameter("@11", item.chg_id), new MySqlParameter("@12", item.chg_dt), new MySqlParameter("@13", item.blno));
        }
        public d_bobbin_info GetdbobbininfoforChangebbdv(string bb_no)
        {
            string sqlquery = @"SELECT * FROM d_bobbin_info WHERE bb_no=@1 and(mt_cd IS NULL OR mt_cd='')";
            return _db.Database.SqlQuery<d_bobbin_info>(sqlquery, new MySqlParameter("1", bb_no)).FirstOrDefault();
        }
        public int GettwmaterialinforwithNG(string mt_cd)
        {
            string sqlquery = @"SELECT COUNT(*) FROM w_material_info WHERE mt_cd=@1";
            return _db.Database.SqlQuery<int>(sqlquery,new MySqlParameter("@1",mt_cd)).FirstOrDefault();
        }
        public int Gettwproductqc(string mt_cd)
        {
            string sqlquery = @"SELECT COUNT(pqno) FROM w_product_qc WHERE ml_no=@1";
            return _db.Database.SqlQuery<int>(sqlquery,new MySqlParameter("1",mt_cd)).FirstOrDefault();
        }
        public void UpdateWMaterialMapping(w_material_mapping item)
        {
            string sqlup = @"UPDATE w_material_mapping SET mt_lot=@1,mt_cd=@2,mt_no=@3,mapping_dt=@4,bb_no=@5,remark=@6,
            sts_share=@7,use_yn=@8,del_yn=@9,reg_id=@10,reg_dt=@11,chg_id=@12,chg_dt=@13 WHERE wmmid=@14";
            _db.Database.ExecuteSqlCommand(sqlup,new MySqlParameter("@1",item.mt_lot), new MySqlParameter("@2", item.mt_cd)
                , new MySqlParameter("@3", item.mt_no), new MySqlParameter("@4", item.mapping_dt), new MySqlParameter("@5", item.bb_no)
                , new MySqlParameter("@6", item.remark), new MySqlParameter("@7", item.sts_share), new MySqlParameter("@8", item.use_yn)
                , new MySqlParameter("@9", item.del_yn), new MySqlParameter("@10", item.reg_id), new MySqlParameter("@11", item.reg_dt)
                , new MySqlParameter("@12", item.chg_id), new MySqlParameter("@13", item.chg_dt), new MySqlParameter("@14", item.wmmid));
        }
        public int Checkdprounitstafffinishback(int id_actual,string staff_id)
        {
            string sql = @"SELECT COUNT(*) FROM d_pro_unit_staff AS a 
            WHERE a.id_actual=@1 and @2 like concat('%',a.staff_id,'%')  AND (NOW() BETWEEN 
            Date_format(a.start_dt, '%Y-%m-%d %H:%i:%s') 
             AND Date_format(a.end_dt, '%Y-%m-%d %H:%i:%s'))";
            return _db.Database.SqlQuery<int>(sql, new MySqlParameter("@1", id_actual), new MySqlParameter("@2", staff_id)).FirstOrDefault();
        }
        public int Checkexitsreturn(string orgin_mt_cd,string sts_update)
        {
            string sqlquery = @"SELECT COUNT(*) FROM w_material_info WHERE orgin_mt_cd=@1 AND sts_update=@2";
            return _db.Database.SqlQuery<int>(sqlquery,new MySqlParameter("@1", orgin_mt_cd), new MySqlParameter("@2", sts_update)).FirstOrDefault();
        }
        public int Checkwmaterialmappingfromcdtype(string mt_cd,string mt_type)
        {
            string sqlquery = @"SELECT COUNT(*) FROM w_material_mapping WHERE mt_cd=@1 AND mt_type!=@2";
            return _db.Database.SqlQuery<int>(sqlquery,new MySqlParameter("@1",mt_cd), new MySqlParameter("@2", mt_type)).FirstOrDefault();
        }
        public void Deletewmaterialmapping(w_material_mapping item)
        {
            string sqlquery = @"DELETE FROM w_material_mapping WHERE wmmid=@1";
            _db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", item.wmmid));
        }
        public w_material_mapping Getmaterialmappingreturn(string mt_cd, string mt_lot)
        {
            string sqlquery = @"SELECT * FROM w_material_mapping WHERE mt_cd=@1 AND mt_lot=@2";
            return _db.Database.SqlQuery<w_material_mapping>(sqlquery, new MySqlParameter("@1", mt_cd), new MySqlParameter("@2", mt_lot)).FirstOrDefault();
        }
        public int GetwmaterialinfoforDelPP(string orgin_mt_cd, string sts_update)
        {
            string sqlquery = @"SELECT COUNT(*) FROM w_material_info WHERE orgin_mt_cd=@1 AND sts_update =@2";
            return _db.Database.SqlQuery<int>(sqlquery, new MySqlParameter("@1", orgin_mt_cd), new MySqlParameter("@2", sts_update)).FirstOrDefault();
        }
        public d_bobbin_info GetBobbinInfoReturn(string bb_no,string mt_cd)
        {
            string QuerySQL = "SELECT * FROM d_bobbin_info WHERE bb_no = @1 AND mt_cd=@2";
            return _db.Database.SqlQuery<d_bobbin_info>(QuerySQL, new MySqlParameter("@1", bb_no), new MySqlParameter("@2", mt_cd)).FirstOrDefault();
        }
        public void UpdatewmaterialMappingmtcduseyn(string mt_cd,string use_yn,string useyn,string chg_id)
        {
            string sqlquery = @"UPDATE w_material_mapping SET use_yn=@3,chg_id=@4 WHERE mt_cd=@1 AND use_yn=@2";
            _db.Database.SqlQuery<w_material_mapping>(sqlquery, new MySqlParameter("@1", mt_cd), new MySqlParameter("@2", use_yn),
                new MySqlParameter("@3", useyn), new MySqlParameter("@1", chg_id));
        }
        public d_bobbin_lct_hist GetdbobbinlcthistFrbbno(string bb_no)
        {
            string sqlquery = @"SELECT * FROM d_bobbin_lct_hist WHERE bb_no=@1";
            return _db.Database.SqlQuery<d_bobbin_lct_hist>(sqlquery, new MySqlParameter("1", bb_no)).FirstOrDefault();
        }
        public IEnumerable<SaveReturn> GetSaveReturn(int wmmid)
        {
            string sqlvarname2 = @"SELECT
	                a.wmmid,
	                a.mt_lot,
	                a.mt_cd,
	                DATE_FORMAT( a.mapping_dt, '%Y-%m-%d %H:%i:%s' ) mapping_dt,
	                a.use_yn,
	                a.reg_dt,
	                b.gr_qty,
	                b.mt_no,
	                b.bb_no,
	                ( CASE WHEN a.use_yn = 'N' THEN b.gr_qty ELSE 0 END ) Used,(
	                CASE
			
			                WHEN a.use_yn = 'N' THEN
			                Find_Remain ( a.mt_cd, b.orgin_mt_cd, a.reg_dt ) ELSE 0 
		                END 
		                ) Remain 
	                FROM
		                w_material_mapping AS a
		                LEFT JOIN w_material_info AS b ON a.mt_cd = b.mt_cd 
	                WHERE
		                a.wmmid = @1 
	                ORDER BY
	                a.use_yn = 'Y' DESC,
	                a.reg_dt DESC";
            return _db.Database.SqlQuery<SaveReturn>(sqlvarname2, new MySqlParameter("@1", wmmid));
        }
        public w_actual GetWActual(int id_actual)
        {
            string sql = @"SELECT * FROM w_actual WHERE id_actual=@1";
            return _db.Database.SqlQuery<w_actual>(sql, new MySqlParameter("@1", id_actual)).FirstOrDefault();
        }
        public int Getwmaterialmappingformapp(string mt_no,string mt_cd,string mt_lot)
        {
            string sql = @"SELECT COUNT(*) FROM w_material_mapping WHERE mt_no=@1 AND mt_cd=@2 AND mt_lot=@3";
            return _db.Database.SqlQuery<int>(sql,new MySqlParameter("@1",mt_no), new MySqlParameter("@2", mt_cd), new MySqlParameter("@3", mt_lot)).FirstOrDefault();
        }
        public IEnumerable<d_bobbin_info> Searchdbobbininfo(string bb_no, string bb_nm)
        {
            string sqlquery = @"SELECT
	            a.bno,
	            a.mc_type,
	            a.bb_no,
	            a.mt_cd,
	            a.bb_nm,
	            a.use_yn,
	            a.purpose,
	            a.barcode,
	            a.re_mark,
	            a.count_number,
	            a.del_yn,
	            a.reg_id,
	            a.reg_dt,
	            a.chg_id,
	            a.chg_dt 
            FROM
	            d_bobbin_info AS a 
            WHERE
	            ( a.mt_cd = '' OR a.mt_cd IS NULL ) 
	            AND ( @1 = '' OR a.bb_no LIKE @2 ) 
	            AND (
	            @3 = '' 
	            OR a.bb_nm LIKE @4)";
            return _db.Database.SqlQuery<d_bobbin_info>(sqlquery, new MySqlParameter("@1", bb_no), new MySqlParameter("@2","%"+ bb_no+"%")
                , new MySqlParameter("@3", bb_nm), new MySqlParameter("@4","%"+ bb_nm+"%"));

        }
        public void InsertMaterialMpping(string mt_cd,string userid,string mt_lot)
        {
            //string wheree= "1=1"
            //   if (!string.IsNullOrWhiteSpace(mt_cd))
            //    {
            //    wheree += "";
            //    }
            string sqll = @"INSERT INTO w_material_mapping(mt_lot,mt_no,mt_cd,mapping_dt,bb_no,remark,use_yn,chg_dt,reg_id,chg_id,reg_dt)
                    SELECT @1 AS mt_lot,
                    mt_no,
                    mt_cd,
                    DATE_FORMAT(NOW(),'%Y%m%d%H%i%s') AS mapping_dt,
                    bb_no,
                    remark,
                    use_yn,
                    NOW() AS chg_dt,
                    @2 AS reg_id,
                    @2 AS chg_id,
                    NOW()  AS reg_dt
                    FROM w_material_mapping WHERE mt_lot=@3 AND use_yn='Y'";
            _db.Database.ExecuteSqlCommand(sqll, new MySqlParameter("@1",mt_cd),
            new MySqlParameter("@2", userid), new MySqlParameter("@3", mt_lot));
        }
        public void UpdateMaterialInfofroMpping(string userid,string staff_id,string GetMachineProcess,string mt_type,string mt_lot)
        {
            string sqlupdate1 = @"UPDATE w_material_info SET chg_id=@1, staff_id = (CASE  
                          WHEN LOCATE(staff_id,@2)>0 THEN @2
                          ELSE  CONCAT(staff_id,',',@2) 
                        END), 
                 machine_id = (CASE  
                          WHEN LOCATE(machine_id,@3)>0 THEN @3
                          ELSE CONCAT(machine_id,',',@3) 
                        END)
             WHERE mt_type != @4 AND mt_cd IN (SELECT mt_cd FROM w_material_mapping WHERE mt_lot=@5 AND use_yn='Y');";

            _db.Database.ExecuteSqlCommand(sqlupdate1, new MySqlParameter("@1", userid)
                , new MySqlParameter("@2", staff_id), new MySqlParameter("@3", GetMachineProcess)
                , new MySqlParameter("@4", mt_type), new MySqlParameter("@5", mt_lot));

        }
        public void UpdateMaterialInffrMpping(string userid,string mt_type,string mt_lot)
        {
            string sqlupdate2 = @"UPDATE w_material_info SET chg_id=@1
             WHERE mt_type = @2 AND mt_cd IN (SELECT mt_cd FROM w_material_mapping WHERE mt_lot=@3 AND use_yn='Y');";
            _db.Database.ExecuteSqlCommand(sqlupdate2, new MySqlParameter("@1", userid)
                , new MySqlParameter("@2", mt_type), new MySqlParameter("@3", mt_lot));
        }

        public bool CountMaxMapping(string mt_cd, string mt_lot)
        {
            string QuerySQL = @"SELECT a.wmmid FROM w_material_mapping as a
                                 WHERE a.wmmid = (
                                SELECT  MAX(b.wmmid)
                                FROM w_material_mapping AS b
                                WHERE b.mt_cd =@1 ) AND a.mt_cd =@1 and   a.mt_lot =@2";
            return String.IsNullOrWhiteSpace(_db.Database.SqlQuery<string>(QuerySQL, 
                new MySqlParameter("1", mt_cd),
                new MySqlParameter("2", mt_lot))
                .FirstOrDefault());
            //nếu data nullorEmpty thì về true
        }

        public IEnumerable<DatawActualPrimaryResponse> GetProcessingActual(string at_no)
        {
            
                string sqlquery = @"SELECT 
                        a.id_actualpr, a.id_actualpr as  id_actualpr1,  a.at_no, a.type,
                        a.target,
                        a.product,
                        a.remark,
                        0 process_count,
                      
                      
                        b.md_cd AS md_cd,
                        b.style_nm AS style_nm 
                        FROM
	                        w_actual_primary AS a
	                        LEFT JOIN d_style_info b ON a.product = b.style_no
	                       
                        WHERE a.at_no = @1";

            return _db.Database.SqlQuery<DatawActualPrimaryResponse>(sqlquery, new MySqlParameter("1", at_no));
        

        }

        public bool CountMaxBobbin(string mt_cd, int id_actual)
        {
            //string QuerySQL = @"SELECT a.bb_no
            //                        FROM w_material_info AS a
            //                        WHERE a.mt_cd =  @1 AND  a.wmtid = (
            //                        SELECT MAX(b.wmtid)
            //                        FROM w_material_info AS b
            //                        WHERE 
            //                        CURDATE() = DATE_FORMAT(b.reg_dt, '%Y-%m-%d') 
            //                        AND b.lct_cd = '002002000000000000'
            //                        AND b.id_actual = @2
            //                        AND b.mt_type = 'CMT'
            //                        AND b.orgin_mt_cd IS NULL); ";
            string QuerySQL = @"SELECT a.bb_no
                                    FROM w_material_info AS a
                                    WHERE a.mt_cd =  @1 AND  a.wmtid = (
                                    SELECT MAX(b.wmtid)
                                    FROM w_material_info AS b
                                    WHERE
                                    b.id_actual = @2
                                    AND b.mt_type = 'CMT'
                                    AND b.orgin_mt_cd IS NULL)";
            return String.IsNullOrWhiteSpace(_db.Database.SqlQuery<string>(QuerySQL,
                new MySqlParameter("1", mt_cd),
                new MySqlParameter("2", id_actual)
                )
                .FirstOrDefault());
            //nếu data nullorEmpty thì về true
        }

        public bool CheckMaterialRetun(string mt_cd)
        {
            string QuerySQL = @"SELECT a.mt_cd FROM w_material_info AS a WHERE a.orgin_mt_cd = @1
                                LIMIT 1";
            return String.IsNullOrWhiteSpace(_db.Database.SqlQuery<string>(QuerySQL, 
                new MySqlParameter("1", mt_cd)).FirstOrDefault());
        }

        public mb_info GetMbInfoGrade(string uname)
        {
            string sqlquery = @"SELECT * FROM mb_info WHERE userid=@1 ";
            return _db.Database.SqlQuery<mb_info>(sqlquery,
                new MySqlParameter("1", uname)).FirstOrDefault();
        }

        public IEnumerable<w_actual> GetActual(string at_no, string type)
        {
            string sqlquery = @"SELECT a.*
            FROM w_actual a
           
            WHERE a.at_no = @1 AND a.type = @2 ";
            return _db.Database.SqlQuery<w_actual>(sqlquery, 
                new MySqlParameter("1", at_no),
                new MySqlParameter("2", type)
                );
        }

        public int InsertWActualInfo(w_actual item)
        {
            string sqlinsert = @"INSERT INTO w_actual(at_no,type,product,actual,defect,name,level,date,don_vi_pr,item_vcd,reg_id,reg_dt,chg_id,chg_dt) 
            VALUES(@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14);
            SELECT LAST_INSERT_ID();";
            return _db.Database.SqlQuery<int>(sqlinsert, 
                new MySqlParameter("1", item.at_no),
                new MySqlParameter("2", item.type),
                new MySqlParameter("3", item.product),
                new MySqlParameter("4", item.actual),
                new MySqlParameter("5", item.defect),
                new MySqlParameter("6", item.name),
                new MySqlParameter("7", item.level),
                new MySqlParameter("8", item.date),
                new MySqlParameter("9", item.don_vi_pr),
                new MySqlParameter("10", item.item_vcd),
                new MySqlParameter("11", item.reg_id),
                new MySqlParameter("12", item.reg_dt),
                new MySqlParameter("13", item.chg_id),
                new MySqlParameter("14", item.chg_dt)).FirstOrDefault();
        }

        public bool IsMaterialInfoExist(string mt_no)
        {
            string QuerySQL = "SELECT true FROM d_material_info WHERE mt_no = @1";

            var result =  _db.Database.SqlQuery<bool>(QuerySQL,
                new MySqlParameter("1", mt_no)).FirstOrDefault();

            return result;
        }
        public int InsertDMaterial(d_material_info item)
        {
            string sqlinsert = @"INSERT INTO d_material_info(mt_no,mt_nm,mt_type,reg_dt,bundle_unit,chg_dt,reg_id,chg_id,use_yn,del_yn) 
            VALUES(@1,@2,@3,@4,@5,@6,@7,@8,@9,@10);
            SELECT LAST_INSERT_ID();";
            return _db.Database.SqlQuery<int>(sqlinsert,
                new MySqlParameter("1", item.mt_no),
                new MySqlParameter("2", item.mt_nm),
                new MySqlParameter("3", item.mt_type),
                new MySqlParameter("4", item.reg_dt),
                new MySqlParameter("5", item.bundle_unit),
                new MySqlParameter("6", item.chg_dt),
                new MySqlParameter("7", item.reg_id),
                new MySqlParameter("8", item.chg_id),
                new MySqlParameter("9", item.use_yn),
                new MySqlParameter("10", item.del_yn)).FirstOrDefault();
        }

        public void UpdateWActual(int id_actual,string description)
        {
            string sqlquery = @"UPDATE w_actual SET description=@2 WHERE id_actual=@1 ";
            _db.Database.ExecuteSqlCommand(sqlquery,
                new MySqlParameter("1", id_actual),
                new MySqlParameter("2", description)
                );
        }

        public BomInfoModel CheckBom(string style_no)
        {
            string QuerySQL = "SELECT * FROM d_bom_info WHERE style_no = @1 and IsApply = 'Y'";
            return _db.Database.SqlQuery<BomInfoModel>(QuerySQL, 
                new MySqlParameter("1", style_no)).FirstOrDefault();
        }

        public bool IsMaterialInfoExistByBom(string productCode, string MaterialNo)
        {
            string QuerySQL = @"SELECT CASE 
            WHEN EXISTS(
                    SELECT a.style_no, a.mt_no, b.MaterialNo
                    FROM d_bom_info a LEFT JOIN materialbom b ON a.style_no = b.ProductCode

                    WHERE a.style_no =@1 AND (a.mt_no = @2 OR b.MaterialNo = @2)
					) THEN true
             ELSE false
            END ";

            var result = _db.Database.SqlQuery<bool>(QuerySQL,
                new MySqlParameter("1", productCode),
                new MySqlParameter("2", MaterialNo)
                ).FirstOrDefault();

            return result;
        }

        public void UpdateProductDeApply(int id_actualpr, string IsApply)
        {
            string sqlupdate = @"Update w_actual_primary SET IsApply=@2 WHERE id_actualpr=@1";
            _db.Database.ExecuteSqlCommand(sqlupdate, new MySqlParameter("1", id_actualpr),
                                                      new MySqlParameter("2", IsApply)
                 );
        }

        public DatawActualPrimaryResponse IsCheckApply(string at_no)
        {
            string QuerySQL = "SELECT * FROM w_actual_primary WHERE at_no = @1 and IsApply = 'Y'";
            return _db.Database.SqlQuery<DatawActualPrimaryResponse>(QuerySQL,
                new MySqlParameter("1", at_no)).FirstOrDefault();
        }

        public IEnumerable<WActualBom> GetListmaterialbom(string style_no, string at_no)
        {
            //string getvalue = @"SELECT tableTam.actual,(SELECT mt_nm FROM d_material_info WHERE mt_no = tableTam.mt_no) AS materialName , tableTam.M_LIEU as mLieu ,((1/ bom.need_m)*bom.tableTam.M_LIEU) AS Actulalythuyet, tableTam.mt_no as MaterialNo
            //            FROM (
            //            SELECT sum(Wmterial.gr_qty) AS M_LIEU, ANY_VALUE(Wmterial.mt_no) AS mt_no,ANY_VALUE(actual.actual)  AS actual, ANY_VALUE(Wmterial.product) AS product
            //            FROM w_material_info AS Wmterial
            //            left JOIN w_actual AS actual
            //            ON Wmterial.at_no = actual.at_no 
            //            WHERE actual.type ='TIMS' AND actual.name ='OQC' AND Wmterial.mt_type <> 'CMT' AND 
            //            Wmterial.mt_no IN (
            //            SELECT d_bom_info.mt_no
            //            FROM d_bom_info
            //            WHERE d_bom_info.style_no = Wmterial.product ) AND actual.at_no= @2
            //            AND actual.product =@1
            //            GROUP BY Wmterial.mt_no) tableTam
            //            LEFT JOIN d_bom_info AS bom
            //            ON  tableTam.product = bom.style_no  GROUP BY tableTam.mt_no ";

            string getvalue = @" CALL GetListMaterialForBom(@1,@2);  ";
            return _db.Database.SqlQuery<WActualBom>(getvalue, 
                
                new MySqlParameter("1", style_no),
                new MySqlParameter("2", at_no)
                );
        }

        public IEnumerable<BuyerQRModel> GetdataTemGoi(string product)
        {
            string sqlquery = @"SELECT a.id, a.buyer_qr , a.product_code ,  (SELECT style_nm FROM d_style_info WHERE style_no = a.product_code) AS product_name, 
                (SELECT md_cd FROM d_style_info WHERE style_no = a.product_code) AS model, 
                (SELECT stamp_name FROM stamp_master WHERE stamp_code = a.stamp_code) AS stamp_name,
                a.standard_qty AS quantity,
                REPLACE(a.lot_date, '-','') AS lotNo
                FROM stamp_detail AS a
                 WHERE a.product_code = @1 and a.buyer_qr NOT IN (SELECT buyer_qr FROM w_material_info WHERE buyer_qr IS NOT NULL );";
            return _db.Database.SqlQuery<BuyerQRModel>(sqlquery, 
                new MySqlParameter("1", product));
        }
    }
}
