using Mvc_VD.Models.NewVersion;
using Mvc_VD.Respositories.Irepository;
using Mvc_VD.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using Mvc_VD.Classes;
using Newtonsoft.Json;

namespace Mvc_VD.Services.Implement
{
    public class CreateBuyerQRService : DbConnection1RepositoryBase,ICreateBuyerQRService
    {
        public CreateBuyerQRService(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {
        }

        public async Task<IEnumerable<StampMaster>> GetAllStampMaster()
        {
            try
            {
                var query = @"Select * from stamp_master";
                var result = await base.DbConnection.QueryAsync<StampMaster>(query);
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<BuyerQRModel>> GetStamp(int id)
        {
            try
            {
                var query = @"SELECT stamp.id, stamp.buyer_qr ,stamp.product_code, stamp.vendor_line, stamp.stamp_code,c.md_cd model,c.part_nm part_name, c.prj_nm, stamp.standard_qty quantity,stamp.lot_date lotNo,c.expiry_month,c.expiry hsd,stamp.ssver,stamp.vendor_code, c.drawingname as nhietdobaoquan
                             FROM (SELECT a.*
                                    FROM stamp_detail AS a
                                    WHERE a.id = @Id) AS stamp
                            JOIN stamp_master AS b
                            ON stamp.stamp_code = b.stamp_code
                            JOIN d_style_info AS c
                            ON stamp.product_code = c.style_no";
                var result = await base.DbConnection.QueryAsync<BuyerQRModel>(query, new { @Id = id });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> CountListStampDetail(string tempQR)
        {
            try
            {
                var query = @"Select Count(buyer_qr) From stamp_detail where buyer_qr Like @TempQR + '%'";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @TempQR = tempQR });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> InsertStampDetail(List<StampDetail> item)
        {
            try
            {
                string listBuyerQR = JsonConvert.SerializeObject(item);
                var result = await base.DbConnection.ExecuteScalarAsync<int>("EXEC [dbo].[CreateBuyerQR] @ListBuyerQR", new { @ListBuyerQR = listBuyerQR });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

            //try
            //{
            //    var query = @"INSERT INTO stamp_detail(buyer_qr, stamp_code, product_code,ssver, vendor_code, vendor_line, label_printer,
            //                    is_sample, pcn, lot_date, serial_number, machine_line, shift, standard_qty, is_sent, box_code, 
            //                    reg_id, reg_dt, chg_id, chg_dt)
            //                Values(@buyer_qr, @stamp_code, @product_code,@ssver, @vendor_code, @vendor_line, @label_printer, @is_sample,
            //                    @pcn, @lot_date, @serial_number, @machine_line, @shift, @standard_qty, @is_sent, @box_code, 
            //                        @reg_id, @reg_dt, @chg_id, @chg_dt);
            //                Select Scope_Identity()";
            //    var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
            //    return result;
            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}
        }

        public async Task<IEnumerable<StampDetail>> GetListStampDetailByCurrent(int From, int To)
        {
            try
            {
                var query = @"Select * From stamp_detail Where id Between @From And @To";
                var result = await base.DbConnection.QueryAsync<StampDetail>(query, new { @From = From, @To = To});
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<BuyerQRModel>> GetStampNameByCode(string stamp_code)
        {
            try
            {
                var query = $"Select s.id,s.buyer_qr,s.stamp_code,d.style_nm,s.reg_dt,s.product_code,d.md_cd,st.stamp_name " +
                            $" From stamp_detail s " +
                            $" join d_style_info d on d.style_no = s.product_code" +
                            $" join stamp_master st on st.stamp_code = s.stamp_code" +
                            $" Where s.buyer_qr in  ({stamp_code}) order by s.buyer_qr asc";
                var result = await base.DbConnection.QueryAsync<BuyerQRModel>(query/*, new { @Code = stamp_code }*/);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<ListIntModel> GetCountNumberBuyer(string tempQR, string shift)
        {
            try
            {
                string sql = @" SELECT SUBSTRING(MAX(a.buyer_qr), 
                                    LEN(@tempQR)+1, 3) AS bientang, SUBSTRING(MAX(a.buyer_qr),
                                    LEN(@tempQR)+4, 2) AS machine_line  FROM stamp_detail a  
                                    WHERE a.buyer_qr LIKE @tempQRlike  AND a.shift = @shift AND a.machine_line = 
                                    (select max(machine_line)  
                                    from stamp_detail st WHERE st.buyer_qr 
                                    LIKE @tempQRlike AND st.shift =  @shift ) 

                                    ;";
                return await base.DbConnection.QueryFirstOrDefaultAsync<ListIntModel>(sql, new { tempQR = tempQR, tempQRlike = tempQR + "%", shift = shift });
            }
            catch (Exception e)
            {

                throw;
            }

        }

        public async Task<IEnumerable<BuyerQRModel>> InsertStampDetail2(List<StampDetail> item)
        {
            try
            {
                string listBuyerQR = JsonConvert.SerializeObject(item);
                var result = await base.DbConnection.QueryAsync<BuyerQRModel>("EXEC [dbo].[CreateBuyerQR] @ListBuyerQR", new { @ListBuyerQR = listBuyerQR });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}