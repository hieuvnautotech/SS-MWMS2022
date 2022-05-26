using Mvc_VD.Models;
using Mvc_VD.Models.Response;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Services
{
    public interface IDMSService
    {
        IEnumerable<MaterialBomModel> GetListmaterialbom(int id);
        BomInfoModel GetListBom(int id);
        void UpdateMaterialBomDeTinhHieuSuat(int MaterialBOMID,int BOMID);
        int InsertToBomInfo(d_bom_info item, bool IsActive);
        d_bom_info CheckBomExist(string style_no, string mt_no);
        int DeleteMaterialBom(string style_no, int? MaterialID);
        int DeleteMaterialBomForId(int? MaterialBOMID);
        int DelMaterial(int bid);
        int DeleteBomInfo(string style_no);
        int DeleteAaLlMaterialBom(string BOMID);
        void UpdateBomInfo(d_bom_info item, int? isActive);
        void UpdateBomDeTinhHieuSuat(string product);
        int InsertToWmaterialTam(w_material_info_tam item, string _barcode, int send_qty, string date, string time_now);
        IEnumerable<WMaterialTamReponse> GetListTam(string mt_no, string date, string time_now, int send_qty);
        void UpdateQtyWMaterailTam(w_material_info_tam item, string id);
        void UpdateProductDeApply(int bid, string IsApply);
        int InsertToMaterialBom(CreateBomMaterialModel item);
        IEnumerable<CreateBomMaterialModel> GetListmaterialbomcap3(string style_no, string mt_no);
        int DelMaterialBom(int Id);
    }
    public class DMSService : IDMSService
    {
        private Entities _db;
        public DMSService(IDbFactory dbFactory)
        {
            _db = dbFactory.Init();
        }
        public IEnumerable<MaterialBomModel> GetListmaterialbom(int id)
        {
            string getvalue = @"SELECT MaterialBOMID,IsActive,
                (SELECT mt_no FROM d_material_info WHERE mtid = MaterialID) AS materialNo,
                (SELECT mt_nm FROM d_material_info WHERE mtid = MaterialID) AS materialName
                FROM materialbom a
              
                where a.BOMID= @1 order by a.BOMID desc";
            return _db.Database.SqlQuery<MaterialBomModel>(getvalue, new MySqlParameter("@1", id));
        }
        public void UpdateMaterialBomDeTinhHieuSuat(int BOMID , int MaterialBOMID)
        {
            string sqlupdate = @"Update materialbom SET IsActive=0 WHERE BOMID=@1;
                                 Update materialbom SET IsActive=1 WHERE MaterialBOMID=@2; ";
            _db.Database.ExecuteSqlCommand(sqlupdate, new MySqlParameter("@1", BOMID),
                                                      new MySqlParameter("@2", MaterialBOMID )
                 );


        }
        public int InsertToBomInfo(d_bom_info item, bool IsActive)
        {
            string QuerySQL = @"INSERT INTO d_bom_info
                (bom_no,style_no,mt_no,need_time,cav,need_m,buocdap,del_yn,reg_id,reg_dt,chg_id,chg_dt, IsActive)
            VALUES (@1,@2, @3, @4, @5, @6, @7, @8, @9, @10,@9, @10,@11);
            SELECT LAST_INSERT_ID();";

            return _db.Database.SqlQuery<int>(QuerySQL, new MySqlParameter("@1", item.bom_no),
                new MySqlParameter("@2", item.style_no), new MySqlParameter("@3", item.mt_no), new MySqlParameter("@4", item.need_time),
                new MySqlParameter("@5", item.cav), new MySqlParameter("@6", item.need_m), new MySqlParameter("@7", item.buocdap),
                new MySqlParameter("@8", item.del_yn), new MySqlParameter("@9", item.reg_id),
                new MySqlParameter("@10", item.reg_dt), new MySqlParameter("@11", IsActive)).FirstOrDefault();
        }


        public BomInfoModel GetListBom(int id)
        {
            string getvalue = @"SELECT a.*,b.md_cd,b.style_nm,
                                (SELECT mt_nm from d_material_info where a.mt_no = mt_no limit 1) as mt_nm
                            FROM d_bom_info AS a
                            JOIN d_style_info AS b ON a.style_no=b.style_no
                            
                            WHERE a.bid = @1";
            return _db.Database.SqlQuery<BomInfoModel>(getvalue, new MySqlParameter("1", id)).FirstOrDefault();
        }

        public d_bom_info CheckBomExist(string style_no, string mt_no)
        {
            string QuerySQL = "SELECT * FROM d_bom_info WHERE style_no = @1 and mt_no = @2";
            return _db.Database.SqlQuery<d_bom_info>(QuerySQL, 
                new MySqlParameter("1", style_no),
                new MySqlParameter("2", mt_no)
                ).FirstOrDefault();
           
        }

        public int DeleteMaterialBom(string style_no, int? MaterialID)
        {
            string sqlquery = @"DELETE FROM materialbom WHERE ProductCode=@1 AND MaterialID = @2 ";
            return _db.Database.ExecuteSqlCommand(sqlquery, 
                new MySqlParameter("1", style_no),
                   new MySqlParameter("2", MaterialID)
                );
        }

        public int DeleteMaterialBomForId(int? MaterialBOMID)
        {
            string sqlquery = @"DELETE FROM materialbom WHERE MaterialBOMID=@1  ";
            return _db.Database.ExecuteSqlCommand(sqlquery,
                new MySqlParameter("1", MaterialBOMID)
                );
        }
        public int DelMaterial(int bid)
        {
            string sqlquery = @"DELETE FROM d_bom_info WHERE bid=@1 ";
            return _db.Database.ExecuteSqlCommand(sqlquery,
                new MySqlParameter("1", bid)
                );
        }
        public int DeleteBomInfo(string style_no)
        {
            string sqlquery = @"DELETE FROM d_bom_info WHERE  FIND_IN_SET(style_no, @1) != 0  ";
            return _db.Database.ExecuteSqlCommand(sqlquery,
                new MySqlParameter("1", style_no)
                );
        }

       

        public int DeleteAaLlMaterialBom(string BOMID)
        {
            string sqlquery = @"DELETE FROM materialbom WHERE  FIND_IN_SET(BOMID, @1) != 0  ";
            return _db.Database.ExecuteSqlCommand(sqlquery,
                new MySqlParameter("1", BOMID)
                );
        }

        public void UpdateBomInfo(d_bom_info item, int? isActive)
        {
            string sqlupdate = @"Update d_bom_info SET style_no = @0,mt_no=@1,need_time=@2,cav=@3,need_m=@4,buocdap=@5,
            isActive=@6,reg_id=@7,chg_dt=@8
            WHERE bid=@9";
            _db.Database.ExecuteSqlCommand(sqlupdate, new MySqlParameter("@0", item.style_no),
                new MySqlParameter("@1", item.mt_no),
                new MySqlParameter("@2", item.need_time), 
                new MySqlParameter("@3", item.cav), 
                new MySqlParameter("@4", item.need_m),
                new MySqlParameter("@5", item.buocdap), 
                new MySqlParameter("@6", isActive),
                new MySqlParameter("@7", item.reg_id),
                new MySqlParameter("@8", item.chg_dt),
                new MySqlParameter("@9", item.bid));
        }

        public void UpdateBomDeTinhHieuSuat(string product)
        {
            string sqlupdate = @"Update d_bom_info SET IsActive=0 WHERE style_no=@1; ";
            _db.Database.ExecuteSqlCommand(sqlupdate, new MySqlParameter("@1", product)
                 );
        }
       
        public int InsertToWmaterialTam(w_material_info_tam item, string _barcode, int send_qty, string date, string time_now)
        {
            string sqlquery = @"CALL insert_lot_tam(@1, @2, @3, @4,@5,@6,@7,@8,@9,@10,@11,@12)";
            return _db.Database.ExecuteSqlCommand(sqlquery, 
                new MySqlParameter("1", item.mt_no),
                new MySqlParameter("2", item.gr_qty),
                new MySqlParameter("3", item.expore_dt),
                new MySqlParameter("4", item.dt_of_receipt),
                new MySqlParameter("5", item.expiry_dt),
                new MySqlParameter("6", item.lot_no),
                new MySqlParameter("7", item.mt_type),
                new MySqlParameter("8", item.reg_id),
                new MySqlParameter("9", _barcode),
                new MySqlParameter("10", send_qty),
                new MySqlParameter("11", date),
                new MySqlParameter("12", time_now)

                );

        }

        public IEnumerable<WMaterialTamReponse> GetListTam(string mt_no,string date, string time_now, int send_qty)
        {
            string sqlquery = @"SELECT wmtid, mt_cd,lot_no,gr_qty,expiry_dt,dt_of_receipt,expore_dt
                             FROM w_material_info_tam 
                             WHERE mt_cd LIKE CONCAT(@1,'-CP-', @2, @3,'%')
                            limit @4 ;";
            return _db.Database.SqlQuery<WMaterialTamReponse>(sqlquery, 
                new MySqlParameter("1", mt_no),
                new MySqlParameter("2", date),
                new MySqlParameter("3", time_now),
                new MySqlParameter("4", send_qty)
                );
        }

        public void UpdateQtyWMaterailTam(w_material_info_tam item, string id)
        {
            string sqlupdate = @"Update w_material_info_tam SET gr_qty= @2 , real_qty = @2, chg_id = @3, chg_dt = @4
                                            WHERE FIND_IN_SET(wmtid, @1) != 0 ";
            _db.Database.ExecuteSqlCommand(sqlupdate,
                new MySqlParameter("1", id),
                new MySqlParameter("2", item.gr_qty),
                new MySqlParameter("3", item.chg_id),
                new MySqlParameter("4", item.chg_dt));
            
        }

        public void UpdateProductDeApply(int bid, string IsApply)
        {
            string sqlupdate = @"Update d_bom_info SET IsApply=@2 WHERE bid=@1";
            _db.Database.ExecuteSqlCommand(sqlupdate, new MySqlParameter("1", bid),
                                                      new MySqlParameter("2", IsApply)
                 );
        }

        public int InsertToMaterialBom(CreateBomMaterialModel item)
        {
            string QuerySQL = @"REPLACE  INTO materialbom
                (ProductCode,MaterialPrarent,MaterialNo,CreateId,CreateDate,ChangeId,ChangeDate)
            VALUES (@1,@2, @3, @4, @5,@6,@7);
            SELECT LAST_INSERT_ID();";

            return _db.Database.SqlQuery<int>(QuerySQL, 
                new MySqlParameter("1", item.ProductCode),
                new MySqlParameter("2", item.MaterialPrarent),
                new MySqlParameter("3", item.MaterialNo), 
                new MySqlParameter("4", item.CreateId),
                new MySqlParameter("5", item.CreateDate),
                new MySqlParameter("6", item.ChangeId),
                new MySqlParameter("7", item.ChangeDate)
                ).FirstOrDefault();
        }

        public IEnumerable<CreateBomMaterialModel> GetListmaterialbomcap3(string style_no, string mt_no)
        {
            string getvalue = @"SELECT *,
                (SELECT mt_nm FROM d_material_info WHERE mt_no = a.MaterialNo) AS materialName
                FROM materialbom a
              
                where a.ProductCode= @1 and a.MaterialPrarent =@2  order by a.id desc";
            return _db.Database.SqlQuery<CreateBomMaterialModel>(getvalue,
                new MySqlParameter("1", style_no),
                new MySqlParameter("2", mt_no)
                );
        }

        public int DelMaterialBom(int Id)
        {
            string sqlquery = @"DELETE FROM materialbom WHERE Id=@1 ";
            return _db.Database.ExecuteSqlCommand(sqlquery,
                new MySqlParameter("1", Id)
                );
        }
    }

}