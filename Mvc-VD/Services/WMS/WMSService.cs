using Mvc_VD.Models;
using Mvc_VD.Models.WIP;
using Mvc_VD.Models.WOModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Services
{
    public interface IWMSService
    {
        IEnumerable<WMaterialInfo> GetListMAterialNo(string mt_no, string style_no);
        d_material_info GetMaterialInfo(int? id);

        int InsertToShippingSDMaterial(shippingsdmaterial item);
        IEnumerable<shippingsdmaterial> Getshippingsdmaterial(string sd_no);

        IEnumerable<shippingsdmaterial> UploadExcel(string jsonObject);
        bool CheckMaterialInfo(string mt_no);

    }
    public class WMSService : IWMSService
    {
        private Entities _db;
        public WMSService(IDbFactory dbFactory)
        {
            _db = dbFactory.Init();
        }
        public IEnumerable<WMaterialInfo> GetListMAterialNo(string mt_no, string style_no)
        {
            //string sql1 = @" SET sql_mode = '';SET @@sql_mode = ''; SELECT mt.id,mt.mt_no,mt.mt_nm, bom.style_no
            //            FROM (
            //            SELECT a.mtid AS id, a.mt_no, a.mt_nm , a.barcode
            //            FROM d_material_info AS a
            //            WHERE a.mt_type != 'MMT') AS mt
            //            left JOIN d_bom_info AS bom
            //            ON mt.mt_no = bom.mt_no
            //            WHERE mt.barcode = 'Y' AND  (@1 ='' OR @1 is null OR  mt.mt_no like @2 ) and
            //              (@3 ='' OR @3 is null OR  bom.style_no like @4 ) 
            //            GROUP BY mt.mt_no";

            string sql1 = @"SET sql_mode = '';SET @@sql_mode = '';
                            SELECT max(mt.mtid) AS id, MAX(mt.mt_no)AS mt_no,max(mt.mt_nm) as mt_nm, max(bom.style_no) AS style_no , '' as meter
                            FROM d_material_info AS mt
                            left JOIN d_bom_info AS bom
                            ON mt.mt_no = bom.mt_no
                            WHERE mt.barcode = 'Y' AND mt.mt_type !='MMT'
                            AND  (@1 ='' OR @1 is null OR  mt.mt_no like @2 ) and
                         (@3 ='' OR @3 is null OR  bom.style_no like @4 )
                            GROUP BY mt.mt_no ";


            return _db.Database.SqlQuery<WMaterialInfo>(sql1,
                new MySqlParameter("1", mt_no != null ? mt_no : ""),
                new MySqlParameter("2", "%" + mt_no + "%") ,
                new MySqlParameter("3", style_no != null ? style_no : ""),
                new MySqlParameter("4", "%" + style_no + "%"));
        }

        public d_material_info GetMaterialInfo(int? id)
        {
            string QuerySQL = "SELECT * FROM d_material_info WHERE mtid = @1";
            return _db.Database.SqlQuery<d_material_info>(QuerySQL, new MySqlParameter("1", id)).FirstOrDefault();
        }

        public IEnumerable<shippingsdmaterial> Getshippingsdmaterial(string sd_no)
        {
                string sqlquery = @"SELECT max(a.sd_no) as sd_no, max(a.mt_no) as mt_no , SUM(a.quantity) AS quantity, SUM(a.meter) AS meter 
            FROM
	            shippingsdmaterial AS a 
            WHERE a.sd_no = @1   GROUP BY a.mt_no ";
                return _db.Database.SqlQuery<shippingsdmaterial>(sqlquery, new MySqlParameter("@1", sd_no));
        }

        public int InsertToShippingSDMaterial(shippingsdmaterial item)
        {
            string QuerySQL = @"INSERT INTO shippingsdmaterial (sd_no,mt_no,quantity,meter,reg_id,reg_dt)
            VALUES (@1,@2, @3, @4, @5, @6);
            SELECT LAST_INSERT_ID();";

            return _db.Database.SqlQuery<int>(QuerySQL, 
                new MySqlParameter("@1", item.sd_no),
                new MySqlParameter("@2", item.mt_no), 
                new MySqlParameter("@3", item.quantity), 
                new MySqlParameter("@4", item.meter),
                new MySqlParameter("@5", item.reg_id),
                new MySqlParameter("@6", item.reg_dt)).FirstOrDefault();
        }

        public IEnumerable<shippingsdmaterial> UploadExcel(string jsonObject)
        {
            string sqlquery = @"CALL SPInsertExcelToShippMT(@1,'','');";
            return _db.Database.SqlQuery<shippingsdmaterial>(sqlquery, new MySqlParameter("@1", jsonObject));
        }
        public bool CheckMaterialInfo(string mt_no)
        {
            string QuerySQL = "SELECT mt_no FROM d_material_info WHERE mt_no = @1 and mt_type != 'MMT' and barcode = 'Y' ";
            return String.IsNullOrWhiteSpace(_db.Database.SqlQuery<string>(QuerySQL,
                new MySqlParameter("1", mt_no)).FirstOrDefault());
        }

       
    }

}