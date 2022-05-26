using Mvc_VD.Controllers;
using Mvc_VD.Models;
using Mvc_VD.Models.DTO;
using Mvc_VD.Models.WIP;
using Mvc_VD.Models.WOModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mvc_VD.Services
{
    public interface IWIPService
    {
        IEnumerable<GeneralWIP> GetListGeneralMaterialWIP(string mt_no, string product_cd, string mt_nm, string recevice_dt_start, string sts, string lct_cd);
        IEnumerable<GeneralWIP> GetListGeneralExportToMachine(string mt_no, string product_cd, string mt_nm, string recevice_dt_start, string recevice_dt_end, string sts, string lct_cd, string mt_cd);

        int TotalRecordsSearchGeneralMaterialWIP(string mt_no, string product_cd, string mt_nm, string recevice_dt_start, string sts, string lct_cd);
        w_material_info_tam GetWMaterialInfoTamwithmtcd(string mt_cd);
        w_material_info_tam GetWMaterialInfoTamwithSTS(string mt_cd);
        int GetWMaterialInfoTamwithCount(string sd_no, string mt_sts_cd);
        int DeleteWMaterialTam(string mt_cd);
        void UpdatedSdInfo(int? alert, string sd_sts_cd, string chg_id, string sd_no, DateTime chg_dt);

        IEnumerable<SdInfoModel> GetListSDInfo(string sd_no);
        WMaterialInfo GetListWMaterialInfo(string mt_cd);
        IEnumerable<WMaterialInfo> GetListMaterialInfoBySdNo(string sd_no);
        IEnumerable<WMaterialInfo> GetListMaterialInfoBySdNoMT(string sd_no, string mt_no);
        int UpdateFinishMaterialWIP(string sid, string sd_no);
        bool CheckMaterialNoShipp(string mt_no, string sd_no);
        w_sd_info GetListSd(int id);
        bool CheckMaterialSd(string sd_no);
        int DeleteSDInfo(int sid);
        int DeleteShippingSDInfo(string sd_no);
        bool CheckSdinfo(string sd_no);
        IEnumerable<shippingsdmaterial> GetListShippngMaterial(string sd_no, string mt_no);
        shippingsdmaterial GetShippingExist(int id);
        IEnumerable<MaterialInfoMMS> GetListWMaterial(string sd_no, string mt_no);
        int Deleteshippingsdmaterial(int id);
        int Updateshippingsdmaterial(int id, int Qty);
        int UpdateshippingMeter(int id, int Qty);
        IEnumerable<ExportToMachineModel> GetListSearchExportToMachine(string ExportCode, string ExportName);
        int TotalRecordsSearchExportToMachine(string ExportCode, string ExportName);
        int InsertToExportToMachine(ExportToMachineModel item);
        void ModifyToExportToMachine(ExportToMachineModel item);
        int DeleteToExportToMachine(int id);
        WMaterialInfoNew CheckWMaterialInfo(string materialCode);
        void UpdateMaterialToMachine(WMaterialInfoNew item, string data);
        IEnumerable<WMaterialInfoNew> GetListExportToMachine(string ExportCode);
        IEnumerable<string> GetWmaterialWip(string sd_no);
        WMaterialInfoNew CheckIsExistMaterial(string mt_cd);
        bool CheckMaterialEP(string ExportCode);
        void UpdateReturnMaterialToWIP(WMaterialInfoNew item, string data);
        lct_info CheckIsExistLocation(string lct_cd);
        void UpdateChangeRackMaterialToMachine(WMaterialInfoNew item, string data);
        IEnumerable<ExportToMachineModel> GetListSearchExportToMachinePP(string ExportCode);
        int TotalRecordsSearchExportToMachine(string ExportCode, string ExportName, string ProductCode, string ProductName, string Description);
        IEnumerable<ExportToMachineModel> GetListSearchExportToMachine(string ExportCode, string ExportName, string ProductCode, string ProductName, string Description);
        Object GetgeneralDetail_List(string mt_no, string mt_nm, string lct_cd, string mt_cd, string product_cd, string sts, string recevice_dt_start, string recevice_dt_end);
        List<WIP_ParentInventoryModel> PrintExcelFile(string mt_no, string mt_nm, string lct_cd, string mt_cd, string product_cd, string sts, string recevice_dt_start, string recevice_dt_end);

    }
    public class WIPService : IWIPService
    {
        private Entities _db;
        public WIPService(IDbFactory dbFactory)
        {
            _db = dbFactory.Init();
        }

        public IEnumerable<GeneralWIP> GetListGeneralMaterialWIP(string mt_no, string product_cd, string mt_nm, string recevice_dt_start, string sts, string lct_cd)
        {
            string viewSql = @" SET sql_mode = '';SET @@sql_mode = '';
                            SELECT * 
                    FROM (  
                            SELECT max(a.mt_no)mt_no,max(b.mt_nm)mt_nm,      
                            concat( ROW_NUMBER() OVER (ORDER BY a.wmtid ), 'a') AS wmtid, concat(( Case 
                            WHEN ( 
                            max(`b`.`bundle_unit`) = 'Roll') THEN round((sum(`a`.`gr_qty`) / max(`b`.`spec`)),2) 
                            ELSE round(MAX(`a`.`gr_qty`),2) 
                            END),' ROLL') AS `qty`, 
 
                            SUM( CASE  WHEN a.mt_sts_cd='002' THEN a.gr_qty ELSE 0  END)AS 'DSD',
                            SUM( CASE WHEN (a.mt_sts_cd='001' or a.mt_sts_cd='004') THEN a.gr_qty ELSE 0  END)  AS 'CSD' 
                            FROM w_material_info AS a 
                            LEFT JOIN d_material_info AS b ON a.mt_no=b.mt_no 
                            LEFT JOIN  w_sd_info info ON info.sd_no = a.sd_no 

                            WHERE  FIND_IN_SET(a.mt_sts_cd, @1) != 0
                            AND (@2='' OR  a.mt_no like @6 )
                            AND (@3='' OR b.mt_nm like @7 )  
                            AND (@4='' OR info.product_cd like @8 ) 
                            AND (@9='' OR a.lct_cd like @10 ) 
                             AND (a.ExportCode IS NULL OR a.ExportCode ='')
                            AND (@5='' OR DATE_FORMAT(a.recevice_dt,'%Y/%m/%d') >= DATE_FORMAT(@5,'%Y/%m/%d'))  
                            AND (@5='' OR DATE_FORMAT(a.recevice_dt,'%Y/%m/%d') <= DATE_FORMAT(@5,'%Y/%m/%d')) 
                            AND a.lct_cd  LIKE '002%'
                            AND a.mt_type!='CMT' 
                            GROUP BY a.mt_no
                ) MyDerivedTable";

            return _db.Database.SqlQuery<GeneralWIP>(viewSql,
                new MySqlParameter("1", sts),
                new MySqlParameter("2", mt_no),
                new MySqlParameter("3", mt_nm),
                new MySqlParameter("4", product_cd),
                new MySqlParameter("9", lct_cd),
                new MySqlParameter("5", recevice_dt_start),
                new MySqlParameter("6", "%" + mt_no + "%"),
                new MySqlParameter("7", "%" + mt_nm + "%"),
                new MySqlParameter("8", "%" + product_cd + "%"),
                new MySqlParameter("10", "%" + lct_cd + "%")
                );
        }

        public int TotalRecordsSearchGeneralMaterialWIP(string mt_no, string product_cd, string mt_nm, string recevice_dt_start, string sts, string lct_cd)
        {
            string countSql = @" 
                       SET sql_mode = '';SET @@sql_mode = '';
                            SELECT  COUNT(*) 
                            FROM ( SELECT max(a.mt_no)mt_no,max(b.mt_nm)mt_nm,      
                            concat( ROW_NUMBER() OVER (ORDER BY a.wmtid ), 'a') AS wmtid, concat(( Case 
                            WHEN ( 
                            max(`b`.`bundle_unit`) = 'Roll') THEN round((sum(`a`.`gr_qty`) / max(`b`.`spec`)),2) 
                            ELSE round(MAX(`a`.`gr_qty`),2) 
                            END),' ROLL') AS `qty`, 
 
                            SUM( CASE  WHEN a.mt_sts_cd='002' THEN a.gr_qty ELSE 0  END)AS 'DSD',
                            SUM( CASE WHEN (a.mt_sts_cd='001' or a.mt_sts_cd='004') THEN a.gr_qty ELSE 0  END)  AS 'CSD' 
                            FROM w_material_info AS a 
                            LEFT JOIN d_material_info AS b ON a.mt_no=b.mt_no 
                            LEFT JOIN  w_sd_info info ON info.sd_no = a.sd_no 

                             WHERE FIND_IN_SET(a.mt_sts_cd, @1) != 0
                             AND (@2='' OR  a.mt_no like @6 )
                            AND (@3='' OR b.mt_nm like @7 )  
                            AND (@4='' OR info.product_cd like @8 ) 
                             AND (@9='' OR a.lct_cd like @10 ) 
                           AND (a.ExportCode IS NULL OR a.ExportCode ='')
                            AND (@5='' OR DATE_FORMAT(a.recevice_dt,'%Y/%m/%d') >= DATE_FORMAT(@5,'%Y/%m/%d'))  
                            AND (@5='' OR DATE_FORMAT(a.recevice_dt,'%Y/%m/%d') <= DATE_FORMAT(@5,'%Y/%m/%d')) 
                            AND a.lct_cd  LIKE '002%'
                            AND a.mt_type!='CMT' 
                     
                            GROUP BY a.mt_no

                        ) MyDerivedTable";

            return _db.Database.SqlQuery<int>(countSql,
                    new MySqlParameter("1", sts),
                    new MySqlParameter("2", mt_no),
                    new MySqlParameter("3", mt_nm),
                    new MySqlParameter("4", product_cd),
                    new MySqlParameter("9", lct_cd),
                    new MySqlParameter("5", recevice_dt_start),
                    new MySqlParameter("6", "%" + mt_no + "%"),
                    new MySqlParameter("7", "%" + mt_nm + "%"),
                    new MySqlParameter("8", "%" + product_cd + "%"),
                    new MySqlParameter("10", "%" + lct_cd + "%")
                    ).FirstOrDefault();


        }
        public w_material_info_tam GetWMaterialInfoTamwithmtcd(string mt_cd)
        {
            string QuerySQL = "SELECT * FROM w_material_info_tam WHERE mt_cd = @1 limit 1";
            return _db.Database.SqlQuery<w_material_info_tam>(QuerySQL, new MySqlParameter("1", mt_cd)).FirstOrDefault();
        }

        public w_material_info_tam GetWMaterialInfoTamwithSTS(string mt_cd)
        {
            string QuerySQL = @"SELECT * FROM w_material_info_tam
                                       WHERE mt_cd = @1  and mt_sts_cd = @2 and (mt_barcode != null or mt_barcode != '') and gr_qty > 0
                                            limit 1 ";
            return _db.Database.SqlQuery<w_material_info_tam>(QuerySQL,
                                        new MySqlParameter("1", mt_cd),

                                        new MySqlParameter("2", "000")
                ).FirstOrDefault();
        }

        public int DeleteWMaterialTam(string mt_cd)
        {
            string sqlquery = @"DELETE FROM w_material_info_tam WHERE mt_cd=@1";
            return _db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", mt_cd));
        }

        public int GetWMaterialInfoTamwithCount(string sd_no, string mt_sts_cd)
        {
            string sqlquery = @"SELECT COUNT(*) FROM w_material_info_tam
                            WHERE sd_no=@1 AND mt_sts_cd = @2";
            return _db.Database.SqlQuery<int>(sqlquery,
                new MySqlParameter("1", sd_no),
                new MySqlParameter("2", mt_sts_cd)).FirstOrDefault();
        }

        public void UpdatedSdInfo(int? alert, string sd_sts_cd, string chg_id, string sd_no, DateTime chg_dt)
        {
            string sqlquery = @"UPDATE w_sd_info SET alert=@1,sd_sts_cd =@2 , chg_id = @3, chg_dt = @5
                                     WHERE sd_no=@4";
            _db.Database.ExecuteSqlCommand(sqlquery,
                new MySqlParameter("1", alert),
                new MySqlParameter("2", sd_sts_cd),
                new MySqlParameter("3", chg_id),
                new MySqlParameter("4", sd_no),
                new MySqlParameter("5", chg_dt)
                );
        }

        public IEnumerable<SdInfoModel> GetListSDInfo(string sd_no)
        {
            string getvalue = @"SELECT a.*,
                (select dt_nm  from comm_dt where mt_cd='WHS005' and dt_cd = a.sd_sts_cd) as sts_nm,
                (select lct_nm  from lct_info where lct_cd = a.lct_cd) as lct_nm
                FROM w_sd_info a
               
                WHERE a.sd_no= @1";
            return _db.Database.SqlQuery<SdInfoModel>(getvalue, new MySqlParameter("1", sd_no));
        }

        public WMaterialInfo GetListWMaterialInfo(string mt_cd)
        {
            string getvalue = @"SELECT wmtid as id,mt_cd,lot_no,gr_qty,expiry_dt,dt_of_receipt,expore_dt,mt_sts_cd,

                (select dt_nm  from comm_dt where mt_cd='WHS005' and dt_cd = mt_sts_cd) as sts_nm
                FROM w_material_info a
               
                WHERE mt_cd= @1 limit 1";
            return _db.Database.SqlQuery<WMaterialInfo>(getvalue, new MySqlParameter("1", mt_cd)).FirstOrDefault();
        }
        public IEnumerable<WMaterialInfo> GetListMaterialInfoBySdNo(string sd_no)
        {
            string sql = @"SET sql_mode = '';SET @@sql_mode = '';  SELECT  ROW_NUMBER() OVER () AS wmtid, any_value(abc.mt_no) as mt_no,
                    any_value(abc.SoluongCap) AS SoluongCap, COUNT(info.mt_cd) AS SoLuongNhanDuoc,any_value(abc.meter) AS meter,
                                CASE
                                  WHEN any_value(abc.SoluongCap) > 0 THEN (any_value(abc.SoluongCap) -  COUNT(info.mt_cd)) 
                                  ELSE 0
                                END as SoluongConLai

                                FROM (
                                SELECT  max(a.mt_no) AS mt_no, max(a.id) AS id, sum(a.quantity) AS SoluongCap, max(a.sd_no) AS sd_no, sum(a.meter) AS meter
                                FROM shippingsdmaterial AS a
                                WHERE a.sd_no = @1
                                 GROUP BY a.mt_no
                                ) AS abc
                                left JOIN w_material_info  info ON info.sd_no = abc.sd_no AND abc.mt_no = info.mt_no AND ( info.sts_update IS NULL OR info.sts_update = '')
                                GROUP BY abc.mt_no";

            return _db.Database.SqlQuery<WMaterialInfo>(sql,
                new MySqlParameter("@1", sd_no));
        }
        public IEnumerable<WMaterialInfo> GetListMaterialInfoBySdNoMT(string sd_no, string mt_no)
        {
            string sql = @"SELECT ROW_NUMBER() OVER () AS wmtid, max(abc.mt_no) as mt_no,max(abc.SoluongCap) as SoluongCap, COUNT(info.mt_cd) AS SoLuongNhanDuoc,
                                CASE
                                  WHEN max(abc.SoluongCap) > 0 THEN (max(abc.SoluongCap) -  COUNT(info.mt_cd)) 
                                  ELSE 0
                                END as SoluongConLai

                                FROM (
                                SELECT  max(a.mt_no) AS mt_no,max(a.id) AS id, sum(a.quantity) AS SoluongCap, max(a.sd_no) AS sd_no
                                FROM shippingsdmaterial AS a
                                WHERE a.sd_no = @1
                                group by a.mt_no
                                ) AS abc
                                left JOIN w_material_info  info ON info.sd_no = abc.sd_no AND abc.mt_no = info.mt_no AND ( info.sts_update IS NULL OR info.sts_update = '')
                                Where abc.mt_no = @2 
                                GROUP BY abc.mt_no";

            return _db.Database.SqlQuery<WMaterialInfo>(sql,
                new MySqlParameter("1", sd_no),
                new MySqlParameter("2", mt_no));
        }
        public int UpdateFinishMaterialWIP(string sid, string sd_no)
        {
            string sql = "UPDATE w_sd_info set sd_sts_cd = '001' , alert=0 Where sd_no = @1 AND sid = @2 ";
            return _db.Database.ExecuteSqlCommand(sql, new MySqlParameter("1", sd_no), new MySqlParameter("2", sid));
        }

        public bool CheckMaterialNoShipp(string mt_no, string sd_no)
        {
            string QuerySQL = "SELECT a.id FROM shippingsdmaterial AS a WHERE a.mt_no=@1 and a.sd_no = @2";
            return String.IsNullOrWhiteSpace(_db.Database.SqlQuery<string>(QuerySQL,
                new MySqlParameter("1", mt_no),
                new MySqlParameter("2", sd_no)
                ).FirstOrDefault());
        }
        public w_sd_info GetListSd(int id)
        {
            string getvalue = @"SELECT * FROM w_sd_info WHERE sid = @1";
            return _db.Database.SqlQuery<w_sd_info>(getvalue,
                new MySqlParameter("1", id)).FirstOrDefault();
        }

        public bool CheckMaterialSd(string sd_no)
        {
            string QuerySQL = "SELECT sd_no FROM w_material_info WHERE sd_no = @1 limit 1";
            return String.IsNullOrWhiteSpace(_db.Database.SqlQuery<string>(QuerySQL,
                new MySqlParameter("1", sd_no)).FirstOrDefault());
        }

        public int DeleteSDInfo(int sid)
        {
            string sqlquery = @"DELETE FROM w_sd_info WHERE sid=@1";
            return _db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", sid));
        }

        public int DeleteShippingSDInfo(string sd_no)
        {
            string sqlquery = @"DELETE FROM shippingsdmaterial WHERE sd_no=@1";
            return _db.Database.ExecuteSqlCommand(sqlquery  , new MySqlParameter("1", sd_no));
        }

        public bool CheckSdinfo(string sd_no)
        {
            string QuerySQL = "SELECT sd_no FROM w_sd_info WHERE sd_no = @1 limit 1";
            return String.IsNullOrWhiteSpace(_db.Database.SqlQuery<string>(QuerySQL,
                new MySqlParameter("1", sd_no)).FirstOrDefault());
        }

        public IEnumerable<shippingsdmaterial> GetListShippngMaterial(string sd_no, string mt_no)
        {
            string sql = @"
                            SELECT *
                            FROM shippingsdmaterial
                            WHERE sd_no = @1 AND mt_no = @2
                            ";

            return _db.Database.SqlQuery<shippingsdmaterial>(sql,
                                     new MySqlParameter("1", sd_no),
                                     new MySqlParameter("2", mt_no)
                );
        }
        public shippingsdmaterial GetShippingExist(int id)
        {
            string getvalue = @"SELECT *
                            FROM shippingsdmaterial 
                            WHERE id = @1";
            return _db.Database.SqlQuery<shippingsdmaterial>(getvalue, new MySqlParameter("1", id)).FirstOrDefault();
        }

        public IEnumerable<MaterialInfoMMS> GetListWMaterial(string sd_no, string mt_no)
        {
            string sql = @" SELECT *
                            FROM w_material_info
                            WHERE sd_no = @1 AND mt_no = @2 AND (sts_update IS NULL OR sts_update = '')
                            ";

            return _db.Database.SqlQuery<MaterialInfoMMS>(sql,
                                     new MySqlParameter("1", sd_no),
                                     new MySqlParameter("2", mt_no)
                );
        }

        public int Deleteshippingsdmaterial(int id)
        {
            string sqlquery = @"DELETE FROM shippingsdmaterial WHERE id=@1";
            return _db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", id));

        }

        public int Updateshippingsdmaterial(int id, int Qty)
        {
            string sql = "UPDATE shippingsdmaterial set quantity = @2 Where id = @1 ";
            return _db.Database.ExecuteSqlCommand(sql,
                new MySqlParameter("1", id),
                new MySqlParameter("2", Qty));
        }

        public int UpdateshippingMeter(int id, int Qty)
        {
            string sql = "UPDATE shippingsdmaterial set meter = @2 Where id = @1 ";
            return _db.Database.ExecuteSqlCommand(sql,
                new MySqlParameter("1", id),
                new MySqlParameter("2", Qty));
        }

        public IEnumerable<ExportToMachineModel> GetListSearchExportToMachine(string ExportCode, string ExportName)
        {
            string viewSql = @" SELECT a.* 
              
	                    FROM   exporttomachine AS a
	                    Where  (@1='' OR  a.ExportCode like @4 ) AND (@2='' OR  a.ExportName like @5 )
	           
                    order by a.id desc ";
            return _db.Database.SqlQuery<ExportToMachineModel>(viewSql,
                new MySqlParameter("1", ExportCode),
                new MySqlParameter("2", ExportName),
                new MySqlParameter("4", "%" + ExportCode + "%"),
                new MySqlParameter("5", "%" + ExportName + "%"));

        }

        public int TotalRecordsSearchExportToMachine(string ExportCode, string ExportName)
        {
            string countSql = @"SELECT COUNT(*) 
	                    FROM   exporttomachine AS a
	                    Where  (@1='' OR  a.ExportCode like @3 ) AND (@2='' OR  a.ExportName like @4 )
                ";
            return _db.Database.SqlQuery<int>(countSql,
                 new MySqlParameter("1", ExportCode),
                 new MySqlParameter("2", ExportName),
                 new MySqlParameter("3", "%" + ExportCode + "%"),
                 new MySqlParameter("4", "%" + ExportName + "%")
                 ).FirstOrDefault();
        }

        public void ModifyToExportToMachine(ExportToMachineModel item)
        {
            string QuerySQL = @"UPDATE exporttomachine SET 
     ProductCode=@1,ProductName= @2,MachineCode=@3,Description=@5,CreateId=@6,ChangeId=@7,ChangeDate=@8
            WHERE id=@9";
            _db.Database.ExecuteSqlCommand(QuerySQL,
                new MySqlParameter("1", item.ProductCode),
                new MySqlParameter("2", item.ProductName),
                new MySqlParameter("3", item.MachineCode),
                new MySqlParameter("5", item.Description),
                new MySqlParameter("6", item.CreateId),
                new MySqlParameter("7", item.ChangeId),
                new MySqlParameter("8", item.ChangeDate),
                new MySqlParameter("9", item.Id));
        }

        public int DeleteToExportToMachine(int id)
        {
            string sqlquery = @"DELETE FROM exporttomachine WHERE id=@1";
            return _db.Database.ExecuteSqlCommand(sqlquery, new MySqlParameter("1", id));
        }

        public WMaterialInfoNew CheckWMaterialInfo(string materialCode)
        {
            string QuerySQL = @"SELECT (SELECT lct_nm FROM lct_info WHERE lct_cd =a.lct_cd ) AS locationName, a.* 
                                FROM w_material_info as a WHERE a.mt_cd = @1";
            return _db.Database.SqlQuery<WMaterialInfoNew>(QuerySQL, new MySqlParameter("1", materialCode)).FirstOrDefault();
        }

        public void UpdateMaterialToMachine(WMaterialInfoNew item, string data)
        {
            string sqlupdate = @"Update w_material_info SET ExportCode=@2 ,LoctionMachine = @3,
                    ShippingToMachineDatetime = @4 
                            WHERE  FIND_IN_SET(wmtid,@1) ";
            _db.Database.ExecuteSqlCommand(sqlupdate, new MySqlParameter("1", data),
                                                      new MySqlParameter("2", item.exportCode),
                                                      new MySqlParameter("3", item.locationMachine),
                                                      new MySqlParameter("4", item.ShippingToMachineDatetime)
                 );
        }

        public IEnumerable<WMaterialInfoNew> GetListExportToMachine(string ExportCode)
        {
            string QuerySQL = @"SELECT a.wmtid,a.mt_no, a.at_no, a.ShippingToMachineDatetime, a.mt_cd, a.lot_no, a.gr_qty,                  (SELECT dt_nm FROM comm_dt WHERE mt_cd = 'WHS005' AND dt_cd = a.mt_sts_cd) statusName,
                        a.expore_dt, a.dt_of_receipt, a.expiry_dt, a.LoctionMachine 
                        FROM w_material_info as a WHERE ExportCode = @1";
            return _db.Database.SqlQuery<WMaterialInfoNew>(QuerySQL, new MySqlParameter("1", ExportCode));

        }

        public IEnumerable<string> GetWmaterialWip(string sd_no)
        {
            string QuerySQL = @"SELECT rece_wip_dt,picking_dt,wmtid,  mt_cd,mt_no,lot_no,expiry_dt,dt_of_receipt,expore_dt,gr_qty, sd_no, mt_sts_cd, (
                            SELECT dt_nm
                            FROM comm_dt
                            WHERE mt_cd = 'WHS005' AND dt_cd = mt_sts_cd) AS sts_nm, lct_cd,
                             (
                            SELECT lct_nm
                            FROM lct_info
                            WHERE lct_cd = w_material_info.lct_cd ) AS lct_nm,
                            (
                            SELECT at_no
                            FROM w_actual
                            WHERE w_actual.id_actual = w_material_info.id_actual
                            LIMIT 1) AS po
                            FROM w_material_info
                            WHERE sd_no = @1 AND lct_cd LIKE '002%' ";

            return _db.Database.SqlQuery<string>(QuerySQL, new MySqlParameter("1", sd_no));
        }

        WMaterialInfoNew IWIPService.CheckIsExistMaterial(string mt_cd)
        {
            string getvalue = @"SELECT *
                FROM w_material_info a
               
                WHERE mt_cd= @1  and a.lct_cd like '002%' and (a.ExportCode is null or a.ExportCode ='') limit 1";
            return _db.Database.SqlQuery<WMaterialInfoNew>(getvalue, new MySqlParameter("1", mt_cd)).FirstOrDefault();
        }

        public bool CheckMaterialEP(string ExportCode)
        {
            string QuerySQL = "SELECT ExportCode FROM w_material_info WHERE ExportCode = @1 limit 1";
            return String.IsNullOrWhiteSpace(_db.Database.SqlQuery<string>(QuerySQL,
                new MySqlParameter("1", ExportCode)).FirstOrDefault());
        }

        public void UpdateReturnMaterialToWIP(WMaterialInfoNew item, string data)
        {
            string sqlupdate = @"Update w_material_info SET ExportCode=@2 ,LoctionMachine = @3,chg_dt= @5, chg_id= @6,lct_cd = @7, ShippingToMachineDatetime=@8 , rece_wip_dt = @9
                            WHERE  FIND_IN_SET(wmtid,@1) ";
            _db.Database.ExecuteSqlCommand(sqlupdate, new MySqlParameter("1", data),
                                                      new MySqlParameter("2", item.exportCode),
                                                      new MySqlParameter("3", item.locationMachine),
                                                      new MySqlParameter("4", item.sts_update),
                                                      new MySqlParameter("5", item.chg_dt),
                                                      new MySqlParameter("6", item.chg_id),
                                                      new MySqlParameter("7", item.lct_cd),
                                                      new MySqlParameter("8", item.ShippingToMachineDatetime),
                                                      new MySqlParameter("9", item.rece_wip_dt)
                 );
        }

        public lct_info CheckIsExistLocation(string lct_cd)
        {
            string QuerySQL = @"SELECT  a.* 
                                FROM lct_info as a WHERE a.lct_cd = @1";
            return _db.Database.SqlQuery<lct_info>(QuerySQL, new MySqlParameter("1", lct_cd)).FirstOrDefault();
        }

        public void UpdateChangeRackMaterialToMachine(WMaterialInfoNew item, string data)
        {
            string sqlupdate = @"Update w_material_info SET chg_dt= @5, chg_id= @6, lct_cd = @7, rece_wip_dt =@8
                            WHERE  FIND_IN_SET(wmtid,@1) ";
            _db.Database.ExecuteSqlCommand(sqlupdate, new MySqlParameter("1", data),
                                                      new MySqlParameter("4", item.sts_update),
                                                      new MySqlParameter("5", item.chg_dt),
                                                      new MySqlParameter("6", item.chg_id),
                                                      new MySqlParameter("7", item.lct_cd),
                                                      new MySqlParameter("8", item.rece_wip_dt)
                 );
        }

        public IEnumerable<ExportToMachineModel> GetListSearchExportToMachinePP(string ExportCode)
        {
            string viewSql = @" SELECT a.* 
              
	                    FROM   exporttomachine AS a
	                    Where  a.ExportCode = @1
	                        ";
            return _db.Database.SqlQuery<ExportToMachineModel>(viewSql,
                new MySqlParameter("1", ExportCode));
        }
        public IEnumerable<ExportToMachineModel> GetListSearchExportToMachine(string ExportCode, string ExportName, string ProductCode, string ProductName, string Description)
        {
            string viewSql = @" SELECT a.* 
	                    FROM   exporttomachine AS a
	                    Where  (@1='' OR  a.ExportCode like @5 ) 
                                AND (@2='' OR  a.ProductCode like @6 )
                                AND (@3='' OR  a.ProductName like @7 )
                                AND (@4='' OR  a.Description like @8 )
	           
                    order by a.id desc ";
            return _db.Database.SqlQuery<ExportToMachineModel>(viewSql,
                new MySqlParameter("1", ExportCode),
                new MySqlParameter("2", ProductCode),
                new MySqlParameter("3", ProductName),
                new MySqlParameter("4", Description),
                new MySqlParameter("5", "%" + ExportCode + "%"),
                new MySqlParameter("6", "%" + ProductCode + "%"),
                new MySqlParameter("7", "%" + ProductName + "%"),
                new MySqlParameter("8", "%" + Description + "%"));

        }

        public int TotalRecordsSearchExportToMachine(string ExportCode, string ExportName, string ProductCode, string ProductName, string Description)
        {
            string countSql = @"SELECT COUNT(*) 
	                    FROM   exporttomachine AS a
	                   Where  (@1='' OR  a.ExportCode like @5 ) 
                                AND (@2='' OR  a.ProductCode like @6 )
                                AND (@3='' OR  a.ProductName like @7 )
                                AND (@4='' OR  a.Description like @8 ) AND a.IsFinish='N'
                ";
            return _db.Database.SqlQuery<int>(countSql,
                new MySqlParameter("1", ExportCode),
                new MySqlParameter("2", ProductCode),
                new MySqlParameter("3", ProductName),
                new MySqlParameter("4", Description),
                new MySqlParameter("5", "%" + ExportCode + "%"),
                new MySqlParameter("6", "%" + ProductCode + "%"),
                new MySqlParameter("7", "%" + ProductName + "%"),
                new MySqlParameter("8", "%" + Description + "%")
                 ).FirstOrDefault();
        }

        public int InsertToExportToMachine(ExportToMachineModel item)
        {
            string QuerySQL = @"INSERT INTO exporttomachine (ExportCode,ProductCode,ProductName,MachineCode,IsFinish,Description,CreateId,CreateDate,ChangeId,ChangeDate)
            VALUES (@1,@2, @3, @4, @5, @6, @7, @8, @9, @10);
            SELECT LAST_INSERT_ID();";

            return _db.Database.SqlQuery<int>(QuerySQL,
                new MySqlParameter("1", item.ExportCode),
                new MySqlParameter("2", item.ProductCode),
                new MySqlParameter("3", item.ProductName),
                new MySqlParameter("4", item.MachineCode),
                new MySqlParameter("5", item.IsFinish),
                new MySqlParameter("6", item.Description),
                new MySqlParameter("7", item.CreateId),
                new MySqlParameter("8", item.CreateDate),
                new MySqlParameter("9", item.ChangeId),
                new MySqlParameter("10", item.ChangeDate)).FirstOrDefault();
        }

        public IEnumerable<GeneralWIP> GetListGeneralExportToMachine(string mt_no, string product_cd, string mt_nm, string recevice_dt_start, string recevice_dt_end, string sts, string lct_cd, string mt_cd)
        {
            //string viewSql = @" SET sql_mode = '';SET @@sql_mode = '';
            //                SELECT * 
            //        FROM (  
            //                SELECT max(a.mt_no)mt_no,max(b.mt_nm)mt_nm,      
            //                concat( ROW_NUMBER() OVER (ORDER BY a.wmtid ), 'a') AS wmtid, concat(( Case 
            //                WHEN ( 
            //                max(`b`.`bundle_unit`) = 'Roll')  THEN concat(round((sum(`a`.`gr_qty`) / max(`b`.`spec`)),2), ' Roll') 
            //                 ELSE concat(round(SUM(`a`.`gr_qty`),2) ,' EA')
            //                END)) AS `qty`, 
            //              b.bundle_unit,
            //                SUM( CASE  WHEN a.mt_sts_cd='002' THEN a.gr_qty ELSE 0  END)AS 'DSD',
            //                SUM( CASE WHEN (a.mt_sts_cd='001' or a.mt_sts_cd='004') THEN a.gr_qty ELSE 0  END)  AS 'CSD' 
            //                FROM w_material_info AS a 
            //                LEFT JOIN d_material_info AS b ON a.mt_no=b.mt_no 
            //                LEFT JOIN  w_sd_info info ON info.sd_no = a.sd_no 

            //                WHERE  FIND_IN_SET(a.mt_sts_cd, @1) != 0
            //                AND (@2='' OR  a.mt_no like @6 )
            //                AND (@11='' OR  a.mt_cd like @12 )
            //                AND (@3='' OR b.mt_nm like @7 )  
            //                AND (@4='' OR info.product_cd like @8 ) 
            //                AND (@9='' OR a.lct_cd like @10 ) 
            //                 AND (a.ExportCode IS NULL OR a.ExportCode ='')
            //                AND (@5='' OR DATE_FORMAT(a.rece_wip_dt,'%Y/%m/%d') >= DATE_FORMAT(@5,'%Y/%m/%d'))  
            //                AND (@13='' OR DATE_FORMAT(a.recevice_dt,'%Y/%m/%d') <= DATE_FORMAT(@13,'%Y/%m/%d')) 
            //                AND a.lct_cd  LIKE '002%'
            //                AND a.mt_type!='CMT' 
            //                GROUP BY a.mt_no
            //    ) MyDerivedTable";


            string viewSql = @" SET sql_mode = '';SET @@sql_mode = '';
                            SELECT  max(table1.mt_no)mt_no,max(table1.mt_nm)mt_nm,      
                            concat( ROW_NUMBER() OVER (ORDER BY table1.wmtid ), 'a') AS wmtid, concat(( Case 
                            WHEN ( 
                            max(`table1`.`bundle_unit`) = 'Roll')  THEN concat(round((sum(`table1`.`gr_qty`) / max(`table1`.`spec`)),2), ' Roll') 
                            ELSE concat(round(SUM(`table1`.`gr_qty`),2) ,' EA')
                            END)) AS `qty`, 
 	                        table1.bundle_unit,
                            SUM( CASE WHEN table1.mt_sts_cd='005' THEN table1.gr_qty ELSE 0  END)  AS 'returnMachine' , 
                            SUM( CASE WHEN table1.mt_sts_cd='002' THEN table1.gr_qty ELSE 0  END)AS 'DSD',
                            SUM( CASE WHEN (table1.mt_sts_cd='001' or table1.mt_sts_cd='004') THEN table1.gr_qty ELSE 0  END)  AS 'CSD' ,
                           

                    table1.recevice_dt
                    FROM (  
                            SELECT a.mt_no,b.mt_nm,    a.wmtid,`b`.`bundle_unit`,`a`.`gr_qty`,`b`.`spec`, a.mt_sts_cd,

                              (
                               CASE 
                               WHEN ('08:00:00' <= DATE_FORMAT( CAST( a.rece_wip_dt AS datetime ),'%H:%i:%s') AND  DATE_FORMAT( CAST( a.rece_wip_dt AS datetime ),'%H:%i:%s')  <  '23:59:59') THEN
                               DATE_FORMAT( CAST( a.rece_wip_dt AS DATETIME ),'%Y-%m-%d')

                               when (DATE_FORMAT( CAST( a.rece_wip_dt AS datetime ),'%H:%i:%s')  < '08:00:00') THEN  DATE_FORMAT( CAST( a.rece_wip_dt AS DATETIME ) - interval 1 DAY ,'%Y-%m-%d')
                                 ELSE ''
                               END )  as recevice_dt
                            FROM w_material_info AS a 
                            LEFT JOIN d_material_info AS b ON a.mt_no=b.mt_no 
                            LEFT JOIN  w_sd_info info ON info.sd_no = a.sd_no 

                            WHERE  FIND_IN_SET(a.mt_sts_cd, @1) != 0
                            AND (@2='' OR  a.mt_no like @2 )
                            AND (@11='' OR  a.mt_cd like @11 )
                            AND (@3='' OR b.mt_nm like @3 )  
                            AND (@4='' OR info.product_cd like @4 ) 
                           
                            
                    
                             AND a.mt_type ='PMT' 
                            AND a.ExportCode  IS NOT null
                            AND (a.mt_sts_cd='001' or a.mt_sts_cd='002' or a.mt_sts_cd='005' )
                           
                ) table1
                where  (@5='' OR DATE_FORMAT(table1.recevice_dt,'%Y/%m/%d') >= DATE_FORMAT(@5,'%Y/%m/%d'))  
                    AND (@13='' OR DATE_FORMAT(table1.recevice_dt,'%Y/%m/%d') <= DATE_FORMAT(@13,'%Y/%m/%d'))
  GROUP BY table1.mt_no ORDER BY table1.mt_no
";



            return _db.Database.SqlQuery<GeneralWIP>(viewSql,
                new MySqlParameter("1", sts),
                new MySqlParameter("2", mt_no),
                new MySqlParameter("3", mt_nm),
                new MySqlParameter("4", product_cd),
            //    new MySqlParameter("9", lct_cd),
                new MySqlParameter("11", mt_cd),
                new MySqlParameter("5", recevice_dt_start),
                new MySqlParameter("13", recevice_dt_end),
                new MySqlParameter("6", "%" + mt_no + "%"),
                new MySqlParameter("7", "%" + mt_nm + "%"),
                new MySqlParameter("8", "%" + product_cd + "%"),
                new MySqlParameter("10", "%" + lct_cd + "%"),
                new MySqlParameter("12", "%" + mt_cd + "%")
                );
        }

        public Object GetgeneralDetail_List(string mt_no, string mt_nm, string lct_cd, string mt_cd, string product_cd, string sts, string recevice_dt_start, string recevice_dt_end)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT * FROM ( SELECT a.wmtid,a.mt_cd,a.ExportCode ,b.mt_nm, lct.lct_nm, CONCAT(ifnull(a.gr_qty,''),' ',ifnull(b.unit_cd,'')) lenght,CONCAT(ifnull(b.width,0),'*',ifnull(a.gr_qty,0)) AS size,ifnull(b.spec,0) spec,a.mt_no, ");
            varname1.Append(" (case when b.bundle_unit ='Roll' then  ROUND((a.gr_qty/b.spec),2) ELSE ROUND(a.gr_qty,2) END) qty, b.bundle_unit, ");
            //varname1.Append("a.recevice_dt, a.sd_no,");
            varname1.Append(" a.sd_no,");
            varname1.Append("com.dt_nm sts_nm, ");
            varname1.Append("a.rece_wip_dt recevice_dt, ");
            varname1.Append("(CASE WHEN('08:00:00' <= DATE_FORMAT(CAST(a.rece_wip_dt AS datetime), '%H:%i:%s') AND  DATE_FORMAT(CAST(a.rece_wip_dt AS datetime), '%H:%i:%s') < '23:59:59') THEN DATE_FORMAT(CAST(a.rece_wip_dt AS DATETIME), '%Y-%m-%d')    when(DATE_FORMAT(CAST(a.rece_wip_dt AS datetime), '%H:%i:%s') < '08:00:00') THEN  DATE_FORMAT(CAST(a.rece_wip_dt AS DATETIME) - interval 1 DAY, '%Y-%m-%d') ELSE ''  END) as recevice_dt1 ");
            varname1.Append("FROM w_material_info a ");
            varname1.Append("LEFT JOIN d_material_info  b ON a.mt_no=b.mt_no ");
            varname1.Append(" LEFT JOIN  w_sd_info info ON info.sd_no = a.sd_no  ");
            varname1.Append("LEFT JOIN lct_info AS lct ON a.lct_cd = lct.lct_cd  ");
            varname1.Append(" LEFT JOIN comm_dt as com ON a.mt_sts_cd  = com.dt_cd AND com.mt_cd='WHS005'  ");
            varname1.Append("WHERE a.lct_cd LIKE '002%'  AND a.mt_no='" + mt_no + "' and a.mt_type ='PMT' AND (a.mt_sts_cd='001' or a.mt_sts_cd='002' or a.mt_sts_cd='005' ) and a.ExportCode IS NOT null  ");
            varname1.Append(" AND ('" + mt_nm + "'='' OR  b.mt_nm like '%" + mt_nm + "%' ) ");
            // varname1.Append(" AND ('" + lct_cd + "'='' OR  a.lct_cd like '%" + lct_cd + "%' ) ");
            varname1.Append(" AND ('" + mt_cd + "'='' OR  a.mt_cd like '%" + mt_cd + "%' ) ");
            varname1.Append(" AND ('" + mt_nm + "'='' OR  b.mt_nm like '%" + mt_nm + "%' ) ");
            varname1.Append(" AND ('" + product_cd + "'='' OR  info.product_cd like '%" + product_cd + "%' ) ");
            varname1.Append(" AND ('" + sts + "'='' OR  a.mt_sts_cd in (" + sts + ") ) )AS TABLE1 ");
            varname1.Append(" WHERE ('" + recevice_dt_start + "'='' OR DATE_FORMAT(TABLE1.recevice_dt1,'%Y/%m/%d') >= DATE_FORMAT('" + recevice_dt_start + "','%Y/%m/%d')) ");
            varname1.Append(" AND ('" + recevice_dt_end + "'='' OR DATE_FORMAT(TABLE1.recevice_dt1,'%Y/%m/%d') <= DATE_FORMAT('" + recevice_dt_end + "','%Y/%m/%d')) order by TABLE1.ExportCode asc");

            var data = new InitMethods().ConvertDataTableToJsonAndReturn(varname1);
            return data.Data;
        }

        public List<WIP_ParentInventoryModel> PrintExcelFile(string mt_no, string mt_nm, string lct_cd, string mt_cd, string product_cd, string sts, string recevice_dt_start, string recevice_dt_end)
        {

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT TABLE1.mt_cd, TABLE1.mt_no, TABLE1.mt_nm,   \n");
            varname1.Append("CONCAT((CASE WHEN (MAX(TABLE1.bundle_unit) = 'Roll')  THEN ROUND((SUM(TABLE1.gr_qty) / MAX(TABLE1.spec)),2)     \n");
            varname1.Append("ELSE ROUND(SUM(TABLE1.gr_qty),2) END),' ', MAX(TABLE1.bundle_unit)) AS qty ,    \n");
            varname1.Append(" SUM( CASE  WHEN TABLE1.mt_sts_cd='002' THEN TABLE1.gr_qty ELSE 0  END)AS 'DSD',  \n");
            varname1.Append(" SUM( CASE WHEN (TABLE1.mt_sts_cd='001' or TABLE1.mt_sts_cd='004') THEN TABLE1.gr_qty ELSE 0  END)  AS 'CSD' ,    \n");
            varname1.Append(" SUM(TABLE1.gr_qty) TK,  \n");
            varname1.Append("'' AS lenght,''size,''recevice_dt,''sts_nm, '' lct_nm , '' sd_no , TABLE1.recevice_dt1   \n");
            varname1.Append("  FROM ( SELECT '' mt_cd, a.mt_no, b.mt_nm,  b.bundle_unit,a.gr_qty,b.spec,a.mt_sts_cd,'' AS lenght,   \n");
            varname1.Append("  (CASE WHEN ('08:00:00' <= DATE_FORMAT( CAST( a.   AS datetime ),'%H:%i:%s') AND  DATE_FORMAT( CAST( a.rece_wip_dt AS datetime ),'%H:%i:%s')  <  '23:59:59') THEN     DATE_FORMAT( CAST( a.rece_wip_dt AS DATETIME ),'%Y-%m-%d')   \n");
            varname1.Append("   WHEN (DATE_FORMAT( CAST( a.rece_wip_dt AS datetime ),'%H:%i:%s')  < '08:00:00') THEN  DATE_FORMAT( CAST( a.rece_wip_dt AS DATETIME ) - interval 1 DAY ,'%Y-%m-%d')  \n");
            varname1.Append("    ELSE ''     END )  as recevice_dt1    \n");
            varname1.Append("  FROM w_material_info AS a    \n");
            varname1.Append("  JOIN d_material_info AS b ON a.mt_no = b.mt_no  \n");
            varname1.Append("   LEFT JOIN  w_sd_info info ON info.sd_no = a.sd_no  \n");
            varname1.Append("WHERE a.lct_cd LIKE '002%'  and a.mt_type <> 'CMT' AND a.mt_sts_cd!='005'  ");
            varname1.Append("  AND (a.ExportCode IS NULL OR a.ExportCode = '') ");
            varname1.Append(" AND ('" + mt_no + "'='' OR  a.mt_no like '%" + mt_no + "%' ) ");
            varname1.Append(" AND ('" + product_cd + "'='' OR  info.product_cd like '%" + product_cd + "%' ) ");
            varname1.Append(" AND ('" + mt_cd + "'='' OR  a.mt_cd like '%" + mt_cd + "%' ) ");
            //      varname1.Append(" AND ('" + lct_cd + "'='' OR  a.lct_cd like '%" + lct_cd + "%' ) ");
            varname1.Append(" AND ('" + mt_nm + "'='' OR  b.mt_nm like '%" + mt_nm + "%' ) ");
            varname1.Append(" AND FIND_IN_SET(a.mt_sts_cd, '" + sts + "') != 0 ");
            varname1.Append(" ) TABLE1 ");
            varname1.Append(" WHERE ('" + recevice_dt_start + "'='' OR DATE_FORMAT(TABLE1.recevice_dt1,'%Y/%m/%d') >= DATE_FORMAT('" + recevice_dt_start + "','%Y/%m/%d')) ");
            varname1.Append(" AND ('" + recevice_dt_end + "'='' OR DATE_FORMAT(TABLE1.recevice_dt1,'%Y/%m/%d') <= DATE_FORMAT('" + recevice_dt_end + "','%Y/%m/%d')) ");

            varname1.Append(" GROUP BY TABLE1.mt_no \n");
            varname1.Append("UNION \n");
            varname1.Append(" \n");
            varname1.Append("SELECT * FROM( SELECT  a.mt_cd,a.mt_no,''mt_nm,CONCAT((CASE WHEN b.bundle_unit ='Roll' THEN ROUND((a.gr_qty/b.spec),2) ELSE ROUND(a.gr_qty,2) END),b.bundle_unit)qty, \n");
            varname1.Append("'' DSD,''CSD,''TK, \n");
            varname1.Append("CONCAT(IFNULL(a.gr_qty,''), IFNULL(b.unit_cd,'')) lenght, CONCAT(IFNULL(b.width,0),'*', IFNULL(a.gr_qty,0)) AS size,a.rece_wip_dt, ( \n");
            varname1.Append("SELECT dt_nm \n");
            varname1.Append("FROM comm_dt \n");
            varname1.Append("WHERE comm_dt.dt_cd=a.mt_sts_cd AND comm_dt.mt_cd='WHS005') sts_nm  , location.lct_nm, a.sd_no , \n");
            varname1.Append("  (CASE WHEN ('08:00:00' <= DATE_FORMAT( CAST( a.rece_wip_dt AS datetime ),'%H:%i:%s') AND  DATE_FORMAT( CAST( a.rece_wip_dt AS datetime ),'%H:%i:%s')  <  '23:59:59') THEN     DATE_FORMAT( CAST( a.rece_wip_dt AS DATETIME ),'%Y-%m-%d')   \n");
            varname1.Append("   WHEN (DATE_FORMAT( CAST( a.rece_wip_dt AS datetime ),'%H:%i:%s')  < '08:00:00') THEN  DATE_FORMAT( CAST( a.rece_wip_dt AS DATETIME ) - interval 1 DAY ,'%Y-%m-%d')  \n");
            varname1.Append("    ELSE ''     END )  as recevice_dt1    \n");
            varname1.Append("FROM w_material_info a \n");
            varname1.Append("LEFT JOIN d_material_info b ON a.mt_no=b.mt_no \n");
            varname1.Append(" LEFT JOIN lct_info location ON a.lct_cd=location.lct_cd  \n");
            varname1.Append("  LEFT JOIN comm_dt as com ON a.mt_sts_cd  = com.dt_cd AND com.mt_cd='WHS005'   \n");
            varname1.Append(" LEFT JOIN  w_sd_info info ON info.sd_no = a.sd_no   \n");
            varname1.Append("WHERE a.lct_cd LIKE '002%'  and a.mt_type = 'PMT' AND  (a.mt_sts_cd='001' or a.mt_sts_cd='002' or a.mt_sts_cd='005' ) and a.ExportCode IS NOT null ");
            varname1.Append("  ");
            varname1.Append(" AND ('" + mt_no + "'='' OR  a.mt_no like '%" + mt_no + "%' ) ");
            varname1.Append(" AND ('" + product_cd + "'='' OR  info.product_cd like '%" + product_cd + "%' ) ");
            varname1.Append(" AND ('" + mt_cd + "'='' OR  a.mt_cd like '%" + mt_cd + "%' ) ");
            varname1.Append(" AND ('" + lct_cd + "'='' OR  a.lct_cd like '%" + lct_cd + "%' ) ");
            varname1.Append(" AND ('" + mt_nm + "'='' OR  b.mt_nm like '%" + mt_nm + "%' ) ");
            varname1.Append(" AND FIND_IN_SET(a.mt_sts_cd, '" + sts + "') != 0 ");
            varname1.Append(") TABLE1 ");
            varname1.Append(" WHERE ('" + recevice_dt_start + "'='' OR DATE_FORMAT(TABLE1.recevice_dt1,'%Y/%m/%d') >= DATE_FORMAT('" + recevice_dt_start + "','%Y/%m/%d')) ");
            varname1.Append(" AND ('" + recevice_dt_end + "'='' OR DATE_FORMAT(TABLE1.recevice_dt1,'%Y/%m/%d') <= DATE_FORMAT('" + recevice_dt_end + "','%Y/%m/%d')) ");
            var result = new InitMethods().ConvertDataTable<WIP_ParentInventoryModel>(varname1);
            return result;
        }

    }

}