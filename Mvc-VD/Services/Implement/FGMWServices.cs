using Dapper;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Respositories.Irepository;
using Mvc_VD.Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mvc_VD.Classes;
using Mvc_VD.Models.Response;
using System.Linq;
using Mvc_VD.Models.WOModel;
using Mvc_VD.Models;
using Mvc_VD.Models.TIMS;
using System.Text;
using Mvc_VD.Models.DTO;
using Mvc_VD.Models.FG;
using System.Web.SessionState;
using System.Web;
using System.Globalization;
using System.Data.SqlClient;
using System.Data;

namespace Mvc_VD.Services.Implement
{
    public class FGMWServices : DbConnection1RepositoryBase, IFGMWServices
    {
        private readonly HttpSessionState Session;
        public FGMWServices(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {
            Session = HttpContext.Current.Session;
        }
        #region Mapping QR Box
        public async Task<IEnumerable<BoxMapping>> GetListBoxMapping(string boxCode, string ProductCode, string sDate, string BuyerCode, int intpage, int introw)
        {
            try
            {
                var result = await base.DbConnection.QueryAsync<BoxMapping>("EXEC [dbo].[GetListBoxMapping]@BoxCode,@ProductCode,@BuyerCode,@Date,@intpage,@introw",
                    new { @BoxCode = boxCode, @ProductCode = ProductCode, @BuyerCode = BuyerCode, @Date = sDate, @intpage = intpage, @introw = introw });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public async Task<IEnumerable<BoxMapping>> GetListBoxMapping1(string boxCode, string ProductCode, string sDate, string BuyerCode, int intpage/*, int introw*/)
        {
            try
            {
                var query = @"SELECT MAX(a.bmno)As bmno, a.bx_no, MAX(a.product) As ProductNo, SUM(a.gr_qty) AS totalQty, MAX(a.status) As status,
	                        (select Top 1 b.dt_nm from comm_dt b where b.mt_cd = 'WHS013' and b.dt_cd = MAX(a.status)) as statusName
                            FROM w_box_mapping a
	                        WHERE (@BoxCode = '' OR @BoxCode IS NULL OR a.bx_no LIKE '%' + @BoxCode +'%')
	                            AND (@ProductCode = '' OR  @ProductCode IS NULL OR a.product LIKE '%' + @ProductCode +'%')
		                        AND (@BuyerCode = 'undefined' OR @BuyerCode IS NULl OR a.buyer_cd LIKE '%' + @BuyerCode + '%')
		                        AND (@Date  = '' OR @Date IS NULL OR a.reg_dt LIKE  @Date + '%')
	                          GROUP BY a.bx_no
							-- Order By MAX(a.bmno) Desc 
                            ORDER BY MAX(a.bmno) Desc -- OFFSET @intpage ROWS FETCH NEXT @introw ROWS ONLY
                        ";
                var result = await base.DbConnection.QueryAsync<BoxMapping>(query, new { @BoxCode = boxCode, @ProductCode = ProductCode, @BuyerCode = BuyerCode, @Date = sDate, @intpage = intpage/*, @introw = introw*/ });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public async Task<int> CountBoxMapping(string boxCode, string ProductCode, string sDate, string BuyerCode)
        {
            try
            {
                var query = @"SELECT Count(*)
                            FROM w_box_mapping a
	                        WHERE (@BoxCode = '' OR @BoxCode IS NULL OR a.bx_no LIKE '%' + @BoxCode +'%')
	                            AND (@ProductCode = '' OR  @ProductCode IS NULL OR a.product LIKE '%'+ @ProductCode +'%')
		                        AND (@BuyerCode = 'undefined' OR @BuyerCode IS NULl OR a.buyer_cd LIKE '%'+ @BuyerCode + '%')
		                        AND (@Date  = '' OR @Date IS NULL OR a.reg_dt LIKE '%'+ @Date + '%')";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @BoxCode = boxCode, @ProductCode = ProductCode, @BuyerCode = BuyerCode, @Date = sDate });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<MappedProductModel>> GetListMappedProducts(string boxCodeMapped, string buyerCode)
        {
            try
            {
                var query = @"SELECT b.bmno as Id,b.mt_cd as MaterialCode, b.buyer_cd as BuyerCode, b.gr_qty as Quantity, b.product as ProductNo, c.lot_no lot_date
                               FROM w_box_mapping AS b
	                            JOIN generalfg AS c ON b.buyer_cd = c.buyer_qr AND  b.bx_no = @BoxMapped

                 where c.buyer_qr like  @BuyerCode +'%'";
                var result = await base.DbConnection.QueryAsync<MappedProductModel>(query, new { @BoxMapped = boxCodeMapped, @BuyerCode = buyerCode });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<string>> GetMaterialCodeFromBoxMapping()
        {
            try
            {
                var query = @"Select mt_cd from w_box_mapping";
                var result = await base.DbConnection.QueryAsync<string>(query);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<string>> GetBuyerCodeFromStampDetail()
        {
            try
            {
                var query = @"Select buyer_qr from stamp_detail Group by buyer_qr";
                var result = await base.DbConnection.QueryAsync<string>(query);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<MaterialInfoTIMS>> GetListMaterialInfoTIMS(string buyerCode, string materialCode, string materialNo)
        {
            try
            {
                var query = @"Select * from w_material_info_tims 
                            Where buyer_code != '' and location_code Like '003%' and gr_qty > 0 And buyer_code = @BuyerCode And material_code = @MateriaCode And mt_no = @MtNo";
                var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(query, new { @BuyerCode = buyerCode, @MateriaCode = materialCode, @MtNo = materialNo });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<StyleInfo>> GetListStyleInfo()
        {
            try
            {
                var query = "Select * From d_style_info";
                var result = await base.DbConnection.QueryAsync<StyleInfo>(query);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<MappedProductModel> ReturnGeneralfgByBuyerQR(string BuyerQR, string vendor)
        {
            try
            {
                var query = $" Select x.id as Id ,x.status as Status, x.md_cd as MaterialCode, SUBSTRING(x.buyer_qr, 0, CHARINDEX('{vendor}',x.buyer_qr ))ProductNo, x.buyer_qr as BuyerCode, x.qty as Quantity , x.lot_no as lot_date                            FROM generalfg AS x  WHERE x.buyer_qr = '{BuyerQR}'";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<MappedProductModel>(query/*, new { @buyerQR = BuyerQR, vendor = vendor }*/);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<FGMappingBoxModel>> GetListDataToCheckMappingBox(string listId)
        {
            try
            {
                //var listData = listId.Split(',');
                //var result = new List<FGMappingBoxModel>();
                //foreach (var item in listData)
                //{
                    var query = $"SELECT c.id, c.buyer_qr, c.qty As standard_qty, c.product_code, c.lot_no As lot_date FROM generalfg c              WHERE c.id in ({listId})  AND c.status = '001'  order by  c.lot_no  ";
                    var result = await base.DbConnection.QueryAsync<FGMappingBoxModel>(query);
                  //  result.Add(rs);
              //  }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<LotDateModel>> GetListLotDateFromStampDetail(string proCode, string stampCode, string listIdStampDetail)
        {
            //try
            //{
            //    var listData = listIdStampDetail.Split(',');
            //    var result = new List<LotDateModel>();
            //    foreach (var item in listData)
            //    {
            //        var query = @"SELECT MIN(b.lot_no) as lot_date from generalfg b where b.product_code = @ProCode AND b.status = '001' and b.id  != @Id";
            //        var rs = await base.DbConnection.QuerySingleOrDefaultAsync<LotDateModel>(query, new { @ProCode = proCode, @StampCode = stampCode, @Id = item });
            //        result.Add(rs);
            //    }
            //    return result;
            //}
            try
            {
                var query = $"SELECT MIN(b.lot_no) as lot_date from generalfg b where b.product_code = '{proCode}' AND b.status = '001' and b.id not in ({listIdStampDetail})";
                var result = await base.DbConnection.QueryAsync<LotDateModel>(query);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<StampDetail> GetStampDetail(string product_code)
        {
            try
            {
                var query = @"Select Top 1 * From stamp_detail Where product_code = @ProCode ";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<StampDetail>(query, new { @ProCode = product_code });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<StyleInfo> GetStyleInfo(string product_code)
        {
            try
            {
                var query = @"	Select Top 1 * From d_style_info Where style_no = @ProCode ";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<StyleInfo>(query, new { @ProCode = product_code });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<ListIntModel>> GetListIntModel(string tempBoxQR)
        {
            try
            {
                var query = @"SELECT Top 1 SUBSTRING(max(a.bx_no), LEN(@tempBoxQR)+1, 3) AS bientang FROM w_box_mapping a 
                            WHERE a.bx_no IS NOT NULL AND a.bx_no LIKE @tempBoxQR+'%'
                            Group By a.bx_no ORDER BY a.bx_no Desc";
                var result = await base.DbConnection.QueryAsync<ListIntModel>(query, new { @tempBoxQR = tempBoxQR });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UpdateStampDetail(string BuyerQR, string BoxQR)
        {
            try
            {
                int result = 0;
                string[] listBuyerQR = BuyerQR.Split(',');
                var html = "'" + "";
                for (int i = 0; i < listBuyerQR.Count(); i++)
                {
                    html += listBuyerQR[i];

                    if (i != listBuyerQR.Count() - 1)
                    {
                        html += "'" + ',' + "'";
                    }
                }
                html = html + "'";
                var query = $"UPDATE stamp_detail SET box_code = '{BoxQR}' WHERE buyer_qr in ({html})";
                    result = await base.DbConnection.ExecuteAsync(query/*, new { @BuyerQR = item, @BoxQR = BoxQR }*/);
              //  }

                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> InsertIntoBoxMapping(string boxQR, string userId, string idStr, string style_no)
        {
            try
            {
                int result = 0;
                //string[] listData = idStr.Split(',');
                //foreach (var item in listData)
                //{
                    var query = $"INSERT INTO w_box_mapping (bx_no, buyer_cd, gr_qty, mapping_dt, status, use_yn, del_yn, reg_id, reg_dt, chg_id, chg_dt,type,product) SELECT '{boxQR}', a.buyer_qr, a.qty, GetDate(), '000', 'Y', 'N', '{userId}', GetDate(), '{userId}', GetDate(), 'SAP',  '{style_no}' FROM  generalfg AS a WHERE  a.id in ({idStr});              Select Scope_Identity()";
                    result = await base.DbConnection.ExecuteAsync(query/*, new { @IDStr = item, @boxQR = boxQR, @UserId = userId }*/);
               // }

                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UpdatePartialGeneralfg(string idStr)
        {
            try
            {
                int result = 0;
                //string[] listData = idStr.Split(',');
                //foreach (var item in listData)
                //{
                    var query = $"UPDATE generalfg  SET status = '010' WHERE id in ({idStr})";
                    result = await base.DbConnection.ExecuteAsync(query/*, new { @IDStr = item }*/);
                //}

                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<BoxMappingResponse>> GetBoxMapping(string boxQR)
        {
            try
            {
                var query = @"	Select box.bx_no As BoxNo, sum(box.gr_qty) As totalQty, min(box.bmno) As id from w_box_mapping As box where box.bx_no = @boxQR group by box.bx_no";
                var result = await base.DbConnection.QueryAsync<BoxMappingResponse>(query, new { @boxQR = boxQR });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<BuyerQRModel> PrintQRCodeForMappingBox(string bx_no)
        {
            try
            {
                var query = @"SELECT MAX(a.bmno) AS id,MAX(a.product) AS product_code, MAX(c.md_cd) AS model,MAX(c.part_nm) AS part_name, MAX(c.prj_nm) AS prj_nm,
                              MAX(b.lot_no) AS lotNo, MAX(c.expiry_month) AS expiry_month, MAX(c.expiry) AS hsd, 
                              MAX(c.ssver) AS ssver, 'DZIH'  AS supplier, MAX(c.drawingname) AS nhietdobaoquan, c.Description,
                              MAX(a.bx_no) AS  bx_no, MAX(a.buyer_cd) AS buyer_cd, SUM(a.gr_qty) AS quantity, MAX(c.stamp_code) AS stamp_code 
                              FROM w_box_mapping AS a 
                              JOIN generalfg AS b ON a.buyer_cd = b.buyer_qr 
                              LEFT JOIN d_style_info AS c ON a.product = c.style_no 
                             WHERE a.bx_no = @BxNo
                             GROUP BY a.bx_no, c.Description";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<BuyerQRModel>(query, new { @BxNo = bx_no });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<string> GetDataStampType(string buyer_qr)
        {
            try
            {
                var query = @"Select stamp_code From stamp_detail where buyer_qr = @QRCode";
                var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @QRCode = buyer_qr });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<generalfg> FindGeneralfg(string buyer_qr)
        {
            try
            {
                var query = @"Select product_code From stamp_detail where buyer_qr = @QRCode";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<generalfg>(query, new { @QRCode = buyer_qr });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<IEnumerable<generalfg>> FindGeneralfg1(string buyer_qr)
        {
            try
            {
                var query = $"Select product_code,buyer_qr From stamp_detail where buyer_qr in ({buyer_qr})";
                var result = await base.DbConnection.QueryAsync<generalfg>(query/*, new { @QRCode = buyer_qr }*/);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<MaterialInfoTIMS>> GetDataBuyerFromMaterialInfoTIMS(string buyer_qr)
        {
            try
            {
                var query = @"Select * From w_material_info_tims where buyer_qr = @BuyerCode And location_code Like '003%'";
                var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(query, new { @BuyerCode = buyer_qr });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<IEnumerable<MappedProductModel>> IsCheckBuyerExist(string buyer_qr)
        {
            try
            {
                var query = $"SELECT id Id , product_code ProductNo, buyer_qr BuyerCode,qty Quantity, FORMAT(CAST(lot_no as datetime), 'yyyy-MM-dd') lot_date, status as Status From generalfg  WHERE buyer_qr in ({buyer_qr})";
                var result = await base.DbConnection.QueryAsync<MappedProductModel>(query/*, new { @BuyerCode = buyer_qr }*/);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<MaterialInfoTIMS> GetDataBuyerFromMaterialInfoTIMSForSAP(string buyer_qr)
        {
            try
            {
                var query = @"Select * From w_material_info_tims where buyer_qr = @BuyerCode And location_code Like '006%' AND status='010'";
                return await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoTIMS>(query, new { @BuyerCode = buyer_qr });
                //return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<int> UpdateWMaterialInfoTimsFromBuyerQR(string buyer_qr, string inputdt, string lctcd, string fromlctcode, string tolctcode, string userid, string status)
        {
            try
            {
                var query = @"Update w_material_info_tims set input_dt=@inputdt,location_code=@lctcd,from_lct_code=@fromlctcode,to_lct_code=@tolctcode,status=@status,chg_id=@chgid where buyer_qr = @BuyerCode";
                return await base.DbConnection.ExecuteAsync(query, new { @BuyerCode = buyer_qr, @inputdt = inputdt, lctcd = lctcd, fromlctcode = fromlctcode, tolctcode = tolctcode, status = status, chgid = userid });
                //return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<Generalfg>> GetDataExistedProductList(string buyer_cd)
        {
            try
            {
                var query = $" select mt_cd,buyer_cd as buyer_qr, status, product as product_code, gr_qty as qty,FORMAT(CAST(reg_dt as datetime), 'yyyy-MM-dd') as lot_no  From w_box_mapping where buyer_cd in ({buyer_cd}) ";
                var result = await base.DbConnection.QueryAsync<Generalfg>(query/*, new { @BuyerCode = buyer_cd }*/);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<w_box_mapping>> GetListTempData(string buyer_cd)
        {
            try
            {
                //var query = @"Select * from w_material_info_tims Where buyer_qr = @BuyerCode And location_code Like '003%' And gr_qty > 0 ";
                var query = $"Select * From w_box_mapping WHERE buyer_cd in ({buyer_cd})";
                var result = await base.DbConnection.QueryAsync<w_box_mapping>(query/*, new { @BuyerCode = buyer_cd }*/);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        #endregion

        #region Shipping Management
        public async Task<IReadOnlyList<DeliveryResponse>> GetListDileveryInfomation(string dl_no, string dl_nm, string productCode, string start, string end)
        {
            try
            {
                var query = @"SELECT MAX(a.dlid) AS dlid, MAX(a.dl_no) AS dl_no, MAX(a.dl_no) AS dl_no1, ISNULL(SUM(g.qty), 0) AS quantity,
                                MAX(a.dl_nm) AS dl_nm, MAX(a.status) AS Status, MAX(a.work_dt) AS work_dt, MAX(a.lct_cd) AS lct_cd, MAX(a.remark) AS remark
                            FROM w_dl_info a
                            LEFT JOIN generalfg g ON  g.dl_no = a.dl_no
                            WHERE a.use_yn = 'Y' 
                                AND (@DlNo ='' OR @DlNo IS NULL OR a.dl_no LIKE '%'+ @DlNo +'%')
                                AND (@DlNm ='' OR @DlNm IS NULL OR a.dl_nm LIKE '%'+ @DlNm +'%')
                                AND (@ProductCode ='' OR @ProductCode IS NULL OR g.product_code Like '%' + @ProductCode + '%') 
                                AND (@Start ='' OR @Start IS NULL OR a.work_dt >=  @Start)
                                AND (@End ='' OR @End IS NULL OR a.work_dt <=  @End)
                                GROUP BY a.dlid
                                Order By a.dlid Desc";
                var rs = await base.DbConnection.QueryAsync<DeliveryResponse>(query, new { @DlNo = dl_no, @DlNm = dl_nm, @ProductCode = productCode, @Start = start, @End = end });
                return rs.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public async Task<DeliveryInfo> GetLastDileveryInfo()
        {
            try
            {
                var query = @"Select Top 1 * from w_dl_info Order By dlid DESC";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<DeliveryInfo>(query);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> InsertIntoDeliveryInfo(DeliveryInfo item)
        {
            try
            {
                var query = @"Insert into w_dl_info (dl_no, dl_nm, status, work_dt, lct_cd, remark, use_yn, reg_id, reg_dt, chg_id, chg_dt)
                            Values (@dl_no, @dl_nm, @status, @work_dt, @lct_cd, @remark, @use_yn, @reg_id, @reg_dt, @chg_id, @chg_dt)
                            Select Scope_Identity()";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<DeliveryInfo> GetLastDileveryInfoById(int dlid)
        {
            try
            {
                var query = @"Select *, dl_no As dl_no1  from w_dl_info where dlid = @Id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<DeliveryInfo>(query, new { @Id = dlid });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdatePartialDeliveryInfo(int dlid, string use_yn)
        {
            try
            {
                var query = @"Update w_dl_info Set use_yn = @UseYn Where dlid = @Id";
                var result = await base.DbConnection.ExecuteAsync(query, new { @UseYn = use_yn, @Id = dlid });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateDeliveryInfo(DeliveryInfo item)
        {
            try
            {
                var query = @"Update w_dl_info Set dl_no = @dl_no, dl_nm = @dl_nm, status = @status, work_dt = @work_dt, lct_cd = @lct_cd, remark = @remark, use_yn = @use_yn, 
                            reg_id = @reg_id, reg_dt = @reg_dt, chg_id = @chg_id, chg_dt = @chg_dt
                            Where dlid = @dlid";
                var result = await base.DbConnection.ExecuteAsync(query, item);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<BoxMapping> CheckStampBox(string box_no, string status)
        {
            try
            {
                var query = @"SELECT  MIN(a.bmno) id, min(a.product) product, sum(a.gr_qty) gr_qty, MIN(a.bx_no) bx_no, MAX(a.buyer_cd) buyer_cd
                            FROM w_box_mapping AS a 
                            WHERE a.bx_no = @BoxNo AND a.status = @Status
                            GROUP BY a.bx_no";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<BoxMapping>(query, new { @BoxNo = box_no, @Status = status });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<BoxMapping> CheckStampBoxExist(string box_no)
        {
            try
            {
                var query = @"SELECT * FROM w_box_mapping WHERE bx_no = @BoxNo";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<BoxMapping>(query, new { @BoxNo = box_no });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UpdatePartialBoxMapping(string user, string html)
        {
            try
            {
                var result = -1;
                //var listBoxNo = html.Split(',');
                //foreach (var item in listBoxNo)
                //{
                    var query = $"UPDATE w_box_mapping SET status = '001', chg_id = '{user}', chg_dt = GETDATE() WHERE bx_no in ({html})";
                    result = await base.DbConnection.ExecuteAsync(query);
              //  }

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdatePartialMaterialInfoTIMS(string dl_no, string user, string listStamp)
        {
            try
            {
                var result = 0;
                //foreach (var item in listStamp)
                //{
                    var query = $"UPDATE  w_material_info_tims SET dl_no = '{dl_no}', status = '000', location_code = '004000000000000000', from_lct_code = '004000000000000000', to_lct_code = '004000000000000000' , chg_id = '{user}' WHERE buyer_qr in ({listStamp})";
                    result = await base.DbConnection.ExecuteAsync(query);
               // }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdatePartialGeneralfg(string dl_no, string user, /*IEnumerable<string>*/string listStamp)
        {
            try
            {
                var result = 0;
                //foreach (var item in listStamp)
                //{
                    var query = $"UPDATE  generalfg  SET dl_no = '{dl_no}' , status = '000' , chg_id =  '{user}' , chg_dt = GetDate() WHERE buyer_qr in ({listStamp})";
                    result = await base.DbConnection.ExecuteAsync(query);
                //}

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<int> UpdatePartialStampDetail(string user, /*IEnumerable<string>*/string listStamp)
        {
            try
            {
                var result = 0;
                //foreach (var item in listStamp)
                //{
                    var query = $"UPDATE stamp_detail SET is_sent = 'Y', chg_id = '{user}', chg_dt= GetDate() WHERE buyer_qr in ({listStamp})";
                    result = await base.DbConnection.ExecuteAsync(query/*, new { @User = user, @NewItem = item }*/);
                //}

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<DeliveryResponse>> GetListDileveryInfo(string dl_no)
        {
            try
            {
                var query = @"SELECT  MAX(a.dlid) AS dlid, MAX(a.dl_no) AS dl_no, MAX(a.dl_no) AS dl_no1, MAX(a.dl_nm) AS dl_nm,  MAX(a.remark) AS remark, 
		                    MAX(a.status) As Status, MAX(a.work_dt) As work_dt, ISNULL(SUM(g.qty),0) AS quantity
                            FROM  w_dl_info AS a 
                            JOIN generalfg AS g ON a.dl_no = g.dl_no
                            WHERE  a.dl_no = @DlNo
		                    Group by a.dlid, a.dl_no, a.dl_nm, a.remark, a.status, a.work_dt ";
                var result = await base.DbConnection.QueryAsync<DeliveryResponse>(query, new { @DlNo = dl_no });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        public async Task<IReadOnlyList<ByerQR>> GetListBuyerQr(string productCode, string poCode, string start, string end)
        {
            try
            {


                //           string sql = @"SELECT a.wmtid,
                //      a.material_code mt_cd,
                //      b.mt_nm,
                //      CONCAT(ISNULL(a.gr_qty, ''), ISNULL(b.unit_cd, '')) lenght,
                //      CONCAT(ISNULL(b.width, 0), '*', ISNULL(a.gr_qty, 0)) AS SIZE,
                //       (case when b.spec is null or b.spec='' then 0 else b.spec end) spec,

                //      CONCAT((CASE
                //                  WHEN b.bundle_unit ='Roll' THEN (a.gr_qty/b.spec)
                //                  ELSE a.gr_qty
                //              END),' ', ISNULL(b.bundle_unit, '')) qty,
                //     a.input_dt AS recevice_dt,
                //	c.at_no AS po,
                //	a.return_date,
                //	a.buyer_qr,
                //	d.product,
                //	c.at_no
                // ,(SELECT TOP 1 dt_nm
                //   FROM comm_dt
                //   WHERE comm_dt.dt_cd=a.status
                //    	AND comm_dt.mt_cd='WHS005') sts_nm


                //, CONVERT(varchar(30),a.end_production_dt, 23) AS end_production_dt,m.style_nm,m.md_cd,a.bb_no,
                // (SELECT TOP 1 lct_nm FROM lct_info WHERE lct_cd=a.location_code  ) lct_nm

                //FROM w_material_info_tims a
                //left JOIN d_material_info b ON a.mt_no=b.mt_no
                //JOIN w_actual c ON a.id_actual = c.id_actual
                //JOIN w_actual_primary d ON d.at_no = c.at_no
                //left	JOIN d_style_info AS m ON d.product=m.style_no
                //		WHERE a.buyer_qr IS NOT null
                //			AND d.product LIKE '%'+@productCode+'%'
                //AND c.at_no LIKE '%'+@poCode+'%'
                //AND  

                //	(  ( (@start != '00010101' ) AND CONVERT(varchar(30),a.input_dt, 23) >=  CONVERT(varchar(30),@start, 23)) or '' = '' )

                // AND (  ( (@end != '99991231' ) AND CONVERT(varchar(30),a.input_dt, 23) <=  CONVERT(varchar(30),@end, 23)) or '' = '' )";
                string sql = @"SELECT a.wmtid,
       a.material_code mt_cd,
       b.mt_nm,
       CONCAT(ISNULL(a.gr_qty, ''), ISNULL(b.unit_cd, '')) lenght,
       CONCAT(ISNULL(b.width, 0), '*', ISNULL(a.gr_qty, 0)) AS SIZE,
        (case when b.spec is null or b.spec='' then 0 else b.spec end) spec,

       CONCAT((CASE
                   WHEN b.bundle_unit ='Roll' THEN (a.gr_qty/b.spec)
                   ELSE a.gr_qty
               END),' ', ISNULL(b.bundle_unit, '')) qty,
      a.input_dt AS recevice_dt,
		c.at_no AS po,
		a.return_date,
		a.buyer_qr,
		d.product,
		c.at_no
  ,(SELECT TOP 1 dt_nm
	   FROM comm_dt
	   WHERE comm_dt.dt_cd=a.status
     	AND comm_dt.mt_cd='WHS005') sts_nm
   

	, CONVERT(varchar(30),a.end_production_dt, 23) AS end_production_dt,m.style_nm,m.md_cd,a.bb_no,
	 (SELECT TOP 1 lct_nm FROM lct_info WHERE lct_cd=a.location_code  ) lct_nm
 
	FROM (
	select wmtid,material_code,gr_qty,at_no,return_date,buyer_qr,id_actual,mt_no,end_production_dt,bb_no,
	location_code,status,input_dt
	from w_material_info_tims where  buyer_qr is not null and buyer_qr!='' and (@productCode='' or @productCode is null or product LIKE '%'+@productCode+'%' )and (@poCode='' or @poCode is null or at_no LIKE '%'+@poCode+'%') and
	(  ( (@start != '00010101' ) AND CONVERT(varchar(30),input_dt, 23) >=  CONVERT(varchar(30),@start, 23)) or '' = '' )
		  
  AND (  ( (@end != '99991231' ) AND CONVERT(varchar(30),input_dt, 23) <=  CONVERT(varchar(30),@end, 23)) or '' = '' )
	) a
	left JOIN d_material_info b ON a.mt_no=b.mt_no
	JOIN (select at_no,id_actual from w_actual where (@poCode='' or @poCode is null or at_no LIKE '%'+@poCode+'%')  and (@productCode='' or @productCode is null or product LIKE '%'+@productCode+'%' ) ) c ON a.id_actual = c.id_actual
	JOIN (select at_no,product from  w_actual_primary  where (@productCode='' or @productCode is null or product LIKE '%'+@productCode+'%' )
	AND (@poCode='' or @poCode is null or at_no LIKE '%'+@poCode+'%'))d ON d.at_no = c.at_no
	left	JOIN d_style_info AS m ON d.product=m.style_no 
		";
                var result = await base.DbConnection.QueryAsync<ByerQR>(sql, new { productCode = productCode, poCode = poCode, start = start, end = end });
                return result.ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IReadOnlyList<DatawActualPrimaryResponse>> GetlistDataActualPrimary(string product, string at_no, string reg_dt)
        {
            string sql = @"SELECT * FROM w_actual_primary as a 
where  (@product='' OR @product is null OR  a.product like '%'+@product+'%' ) 
AND (@atno='' OR @atno is null OR  a.at_no like '%'+@atno+'%' )
AND (@regdt='' OR @regdt IS NULL OR  Convert(date,a.reg_dt) >=  Convert(date,@regdt))
order by a.reg_dt desc";
            var result = await base.DbConnection.QueryAsync<DatawActualPrimaryResponse>(sql, new { product = product, atno = at_no, regdt = reg_dt });
            return result.ToList();
        }
        public async Task<IReadOnlyList<MaterialInfoTIMS>> GetListFGPO(string at_no)
        {
            string sql = @"select Concat(a.buyer_qr,' (',a.gr_qty,')') AS buyer_qr,a.gr_qty from w_material_info_tims a
            inner join w_actual b on a.id_actual=b.id_actual
            where 		a.buyer_qr is not null  and  a.buyer_qr!=''  and b.at_no = @atno and gr_qty>0";
            var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(sql, new { atno = at_no });
            return result.ToList();

        }
        public async Task<string> GetProductforPrimary(string at_no)
        {
            string sql = @"select product from w_actual_primary where at_no = @atno";
            var result = await base.DbConnection.QueryFirstOrDefaultAsync<string>(sql, new { atno = at_no });
            return result;

        }
        public async Task<IReadOnlyList<BoxMapping>> GetlistBoxNobyMTCD()
        {
            string sql = @"select box.bx_no,box.buyer_cd, infotim.material_code AS mt_cd,box.gr_qty
                        from w_box_mapping box  
                        inner join w_material_info_tims infotim on box.mt_cd =infotim.material_code
                        union all 
                        select box.bx_no,box.buyer_cd, infomms.material_code AS mt_cd,box.gr_qty
                        from w_box_mapping box  
                        inner join w_material_info_mms infomms on box.mt_cd =infomms.material_code";
            var result = await base.DbConnection.QueryAsync<BoxMapping>(sql);
            return result.ToList();
        }
        public async Task<IReadOnlyList<d_bobbin_lct_hist>> GetBobinHistory(string bb_no)
        {
            string sql = @"select * from d_bobbin_lct_hist where bb_no=@bbno";
            var result = await base.DbConnection.QueryAsync<d_bobbin_lct_hist>(sql, new { bbno = bb_no });
            return result.ToList();
        }
        public async Task<IReadOnlyList<truyxuatlot>> Truyxuatlistlot(StringBuilder sql)
        {
            StringBuilder sqlquery = new StringBuilder($"declare  @tmpa table(order_lv int,mapping_dt datetime," +

                "reg_dt datetime," +
                "id int," +
                "cha nvarchar(100)," +
                "buyer_qr nvarchar(100)," +
                "mt_cd nvarchar(100)," +
                "mt_nm nvarchar(max)," +
                "cccc nvarchar(100)," +
                "mt_lot nvarchar(100)," +
                "type nvarchar(100)," +
                "bb_no nvarchar(100)," +
                "process nvarchar(100)," +
                "process_cd nvarchar(100)," +
                "congnhan_time nvarchar(max)," +
               "machine nvarchar(max)," +
                "expiry_dt varchar(10)," +
                "dt_of_receipt varchar(10)," +
                "expore_dt varchar(10)," +
                "mt_type nvarchar(100)," +
                "SLLD int," +
                "mt_no nvarchar(100)," +
                "date varchar(10)," +
                "lot_no nvarchar(100)," +
                "size nvarchar(100)); ");
            sqlquery.Append(sql);
            sqlquery.Append("SELECT * FROM @tmpa");

            string sqls = sqlquery.ToString();
            var result = await base.DbConnection.QueryAsync<truyxuatlot>(sqls);
            return result.ToList();

        }

        public async Task<IEnumerable<MaterialInfoTIMS>> GetDataBuyersFromMaterialInfoTIMS(string buyer_qr)
        {
            try
            {
                var query = @"Select * From w_material_info_tims where buyer_qr = @BuyerCode";
                var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(query, new { @BuyerCode = buyer_qr });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        //public async Task<IReadOnlyList<FGGeneral>> GetFGGeneral(string bom_type, string productCode, string productName, string recevice_dt_start, string recevice_dt_end, string buyerCode)
        //{
        //    try
        //    {
        //        string sql = @" SELECT MAX(a.id) AS id, 
        //                         MAX(a.product_code) AS product_code,  MAX(b.style_nm) AS product_name,MAX(a.md_cd) AS md_cd,
        //                         SUM(a.qty) AS qty, max(a.reg_dt) as reg_dt ,max(b.bom_type) as bom_type
        //                         FROM generalfg as a
        //                         Join d_style_info b on b.style_no = a.product_code 
        //                         where a.status in ('001','010') 
        //                      AND   b.bom_type like @bomtypelike 
        //                        AND  @product_Code='' OR  a.product_code like @product_Codelike 
        //                        AND  @product_Name='' OR  b.style_nm like @product_Namelike 
        //                        AND  @buyer_Code='' OR  a.buyer_qr like @buyer_Codelike 
        //                        AND (('0001-01-01'='0001-01-01')  or ( CONVERT(datetime,a.reg_dt,121) >=  CONVERT(datetime,'0001-01-01',121)))
        //                         AND   (('9999-12-31'='9999-12-31')  or ( CONVERT(datetime,a.reg_dt,121) <=  CONVERT(datetime,'9999-12-31',121)))

        //                        group by a.product_code, b.bom_type

        //                            ";




        //        var result = await base.DbConnection.QueryAsync<FGGeneral>(sql,
        //            new
        //            {
        //                bomtype = bom_type == null ? "" : bom_type,
        //                bomtypelike = "%" + bom_type + "%",

        //                product_Code = productCode == null ? "" : productCode,
        //                product_Codelike = "%" + productCode + "%",
        //                product_Name = productName == null ? "" : productName,
        //                product_Namelike = "%" + productName + "%",
        //                buyer_Code = buyerCode == null ? "" : buyerCode,
        //                buyer_Codelike = "%" + buyerCode + "%",
        //                recevicedtstart = recevice_dt_start,
        //                recevicedtend = recevice_dt_end
        //            });
        //        return result.ToList();

        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }

        //}
       public async Task<IEnumerable<FGGeneral>> GetFGGeneral(string bom_type, string productCode, string productName, string recevice_dt_start, string recevice_dt_end, string buyerCode)
        {
            try
            {
                IEnumerable<FGGeneral> datas = null;
                var sql = @" SELECT MAX(a.id) AS id, 
                                 MAX(a.product_code) AS product_code,  MAX(b.style_nm) AS product_name,MAX(a.md_cd) AS md_cd,
                                 SUM(a.qty) AS qty, max(a.reg_dt) as reg_dt ,max(b.bom_type) as bom_type
                                 FROM generalfg as a
                                 Join d_style_info b on b.style_no = a.product_code 
                                 where a.status in ('001','010') 
                              AND  (@bomtype='' OR   b.bom_type like @bomtypelike )
                                AND ( @product_Code='' OR  a.product_code like @product_Codelike )
                                AND  (@product_Name='' OR  b.style_nm like @product_Namelike )
                                AND ( @buyer_Code='' OR  a.buyer_qr like @buyer_Codelike )
                             
                                group by a.product_code, b.bom_type

                                    ";
                datas = base.DbConnection.Query<FGGeneral>(sql, new
                {
                    bomtype = bom_type == null ? "" : bom_type,
                    bomtypelike = "%" + bom_type + "%",

                    product_Code = productCode == null ? "" : productCode,
                    product_Codelike = "%" + productCode + "%",
                    product_Name = productName == null ? "" : productName,
                    product_Namelike = "%" + productName + "%",
                    buyer_Code = buyerCode == null ? "" : buyerCode,
                    buyer_Codelike = "%" + buyerCode + "%"
                });

                datas.ToList();
                if (datas != null)
                {

                    if (string.IsNullOrEmpty(recevice_dt_start) == false)
                    {
                        datas = datas.Where(item => item.reg_dt >= DateTime.ParseExact(recevice_dt_start, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                    }
                    if (string.IsNullOrEmpty(recevice_dt_end) == false)
                    {
                        datas = datas.Where(item => item.reg_dt <= DateTime.ParseExact(recevice_dt_end, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                    }
                }
                return datas;

                //var keyData = string.Format("");
                //IEnumerable<FGGeneral> datas = null;
                //var dataSession = Session[keyData];
                //if (dataSession == null)
                //{
                //    var sql = @" SELECT MAX(a.id) AS id, 
                //                 MAX(a.product_code) AS product_code,  MAX(b.style_nm) AS product_name,MAX(a.md_cd) AS md_cd,
                //                 SUM(a.qty) AS qty, max(a.reg_dt) as reg_dt ,max(b.bom_type) as bom_type
                //                 FROM generalfg as a
                //                 Join d_style_info b on b.style_no = a.product_code 
                //                 where a.status in ('001','010') 


                //                group by a.product_code, b.bom_type

                //                    ";
                //    datas =  base.DbConnection.Query<FGGeneral>(sql);

                //    datas.ToList();
                //    Session[keyData] = datas;
                //}
                //else
                //{
                //    datas = (IEnumerable<FGGeneral>)dataSession;
                //}
                //if (datas != null)
                //{
                //    if (string.IsNullOrEmpty(buyerCode) == false)
                //    {
                //        datas = datas.Where(item => item.buyer_qr.Contains(buyerCode));
                //    }
                //    if (string.IsNullOrEmpty(bom_type) == false)
                //    {
                //        datas = datas.Where(item => item.bom_type.Contains(bom_type));
                //    }
                //    if (string.IsNullOrEmpty(productCode) == false)
                //    {
                //        datas = datas.Where(item => item.product_code.Contains(productCode));
                //    }

                //    if (string.IsNullOrEmpty(productName) == false)
                //    {
                //        datas = datas.Where(item => item.product_name.Contains(productName));
                //    }
                //    if (string.IsNullOrEmpty(recevice_dt_start) == false)
                //    {
                //        datas = datas.Where(item => item.reg_dt.Date >= DateTime.ParseExact(recevice_dt_start, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                //    }
                //    if (string.IsNullOrEmpty(recevice_dt_end) == false)
                //    {
                //        datas = datas.Where(item => item.reg_dt.Date <= DateTime.ParseExact(recevice_dt_end, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                //    }
                //}
                //return datas;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public async Task<IReadOnlyList<FGGenneralExport>> GetFGGeneralExport(string bom_type, string productCode, string productName, string recevice_dt_start, string recevice_dt_end, string buyerCode)
        {
            try
            {
               // IEnumerable<FGGenneralExport> datas = null;
                var sql = @" with getdata as (SELECT 
                                 MAX(a.product_code) AS product_code,  
								 MAX(b.style_nm) AS product_name
								 ,MAX(a.md_cd) AS md_cd,
								 ''buyer_qr,
								 ''at_no,
								 ''lot_no,
                                 SUM(a.qty) AS qty,
								 '' reg_dt ,
								 max(b.bom_type) as bom_type,
								 ''statusName
                                 FROM generalfg as a
                                 Join d_style_info b on b.style_no = a.product_code 
                                 where a.status in ('001','010') 
                            --  AND  (@bomtype='' OR   b.bom_type like @bomtypelike )
                                AND  (@bomtype='' OR @bomtype IS NULL OR b.bom_type like  '%' + @bomtype + '%' )
                                AND ( @product_Code='' OR @product_Code IS NULL OR a.product_code like  '%' + @product_Code + '%')
                                AND  (@product_Name='' OR @product_Name IS NULL OR  b.style_nm like  '%' + @product_Name + '%')
                                AND ( @buyer_Code='' OR @buyer_Code IS NULL OR  a.buyer_qr like  '%' + @buyer_Code + '%' )
						--		AND (@startDate ='' OR @startDate IS NULL OR a.reg_dt >=  @startDate ) 
                          --      AND (@endDate =''  OR @endDate IS NULL OR a.reg_dt <=  @endDate ) 
                                group by a.product_code, b.style_nm 
							
union 

SELECT 
								 a.product_code,
								 ''product_name,
								 ''mt_cd,
								 a.buyer_qr,
								 a.at_no,
								 a.lot_no,
								 a.qty,
								 a.reg_dt,
								 ''bom_type,
								 b.dt_nm statusName
                                 FROM generalfg as a
                                 Join comm_dt b on b.mt_cd ='WHS005' AND b.dt_cd = a.status
                                 where a.status in ('001','010') 
                                   AND  (@product_Code='' OR @product_Code IS NULL OR  a.product_code like '%' + @product_Code + '%'))
select * from getdata order by product_code,product_name desc

                                    ";
              var  datas = await base.DbConnection.QueryAsync<FGGenneralExport>(sql, new
                {
                    bomtype = bom_type,
                    product_Code = productCode,
                    product_Name = productName ,
                    buyer_Code = buyerCode
                    //endDate = recevice_dt_end,
                    //startDate = recevice_dt_start
                });


                 datas.ToList();
                if (datas != null)
                {

                    if (string.IsNullOrEmpty(recevice_dt_start) == false)
                    {
                        var a = DateTime.ParseExact(recevice_dt_start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        datas = datas.Where(item => item.reg_dt >= DateTime.ParseExact(recevice_dt_start, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                    }
                    if (string.IsNullOrEmpty(recevice_dt_end) == false)
                    {
                        datas = datas.Where(item => item.reg_dt <= DateTime.ParseExact(recevice_dt_end, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                    }
                }
                return datas.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public async Task<int> InertFGGeneral(generalfg item)
        {
            try
            {
                        string sql = @"INSERT INTO [dbo].[generalfg]
                   ([buyer_qr]
                   ,[product_code]
                   ,[type]
                   ,[md_cd]
                   ,[status]
                   ,[qty]
                   ,[lot_no]
                   ,[reg_id]
                   ,[reg_dt]
                   ,[at_no]
        )
             VALUES
                   (@buyerqr
                   ,@productcode
                   ,@type
                   ,@mdcd
                   ,@status
                   ,@qty
                   ,@lotno
                   ,@regid
                   ,@regdt
                   ,@atno
        );
Select Scope_Identity()";
                return await base.DbConnection.ExecuteScalarAsync<int>(sql,
                    new
                    {
                        buyerqr = item.buyer_qr,
                        productcode = item.product_code,
                        type = item.type,
                        mdcd = item.md_cd,
                        status = item.status,
                        qty = item.qty,
                        lotno = item.lot_no,
                        regid = item.reg_id,
                        regdt = item.reg_dt,
                        atno = item.at_no
                    });
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<int> InsertStampDTL(stamp_detail item)
        {
            string sql = @"INSERT INTO [dbo].[stamp_detail]
           ([buyer_qr]
           ,[stamp_code]
           ,[product_code]
           ,[vendor_code]
           ,[vendor_line]
           ,[label_printer]
           ,[is_sample]
           ,[pcn]
           ,[lot_date]
           ,[serial_number]
           ,[machine_line]
           ,[shift]
           ,[standard_qty]
           ,[is_sent]
           ,[box_code]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
     VALUES
           (@buyer_qr
           ,@stamp_code
           ,@product_code
           ,@vendor_code
           ,@vendor_line
           ,@label_printer
           ,@is_sample
           ,@pcn
           ,@lot_date
           ,@serial_number
           ,@machine_line
           ,@shift
           ,@standard_qty
           ,@is_sent
           ,@box_code
           ,@reg_id 
,getdate()
           ,@chg_id
,getdate());
Select Scope_Identity()";
            return await base.DbConnection.ExecuteScalarAsync<int>(sql,
                new
                {
                    buyer_qr = item.buyer_qr,
                    stamp_code = item.stamp_code,
                    product_code = item.product_code,
                    vendor_code = item.vendor_code,
                    vendor_line = item.vendor_line,
                    label_printer = item.label_printer,
                    is_sample = item.is_sample,
                    pcn = item.pcn,
                    lot_date = item.lot_date,
                    serial_number = item.serial_number,
                    shift = item.shift,
                    standard_qty = item.standard_qty,
                    machine_line = item.machine_line,
                    is_sent = item.is_sent,
                    box_code = item.box_code,
                    reg_id = item.reg_id,
                    chg_id = item.chg_id
                });
        }
        public async Task<int> CheckGeneralFG(string buyerqr)
        {
            string sql = @"SELECT Count(id) FROM [dbo].[generalfg] WHERE buyer_qr=@buyer_qr";
            return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { buyer_qr = buyerqr });
        }
        public async Task<generalfg> GetGeneralFGByBuyerQR(string buyerqr)
        {
            string sql = @"SELECT * FROM [dbo].[generalfg] WHERE buyer_qr=@buyer_qr";
            return await base.DbConnection.QueryFirstOrDefaultAsync<generalfg>(sql, new { buyer_qr = buyerqr });
        }
        public async Task<string> CheckGeneralFGproduct(int id)
        {
            string sql = @"SELECT product_code FROM [dbo].[generalfg] WHERE id=@id";
            return await base.DbConnection.QueryFirstOrDefaultAsync<string>(sql, new { id = id });
        }

        public async Task<int> CheckStampDTL(string buyerqr)
        {
            string sql = @"SELECT Count(id) FROM [dbo].[stamp_detail] WHERE buyer_qr=@buyer_qr";
            return await base.DbConnection.QueryFirstOrDefaultAsync<int>(sql, new { buyer_qr = buyerqr });
        }
        public async Task<IReadOnlyList<d_style_info>> GetListDStyleInfo()
        {
            string sql = @"SELECT * FROM [dbo].[d_style_info] where active=1";
            var result = await base.DbConnection.QueryAsync<d_style_info>(sql);
            return result.ToList();
        }
        public async Task<IReadOnlyList<FGProductLotPO>> GetListFGProductLotPO(string atno)
        {

            //            string sql = @"select * into #tmpactual from w_actual where at_no=@po

            //SELECT dd.mt_cd,cc.mc_no,cc.start_dt,bb.end_dt 
            //into #tmmppaa
            //FROM (
            //select aa.mc_no--,aa.staff_id
            //,MIN(aa.start_dt) start_dt 
            //FROM(
            //select distinct  a.mt_cd
            //--,e.staff_id
            //,d.mc_no,d.start_dt,d.end_dt
            //from w_material_mapping_mms a
            //inner join w_material_info_mms b on a.mt_lot=b.material_code
            //inner join #tmpactual c on c.id_actual=b.id_actual
            //inner join d_pro_unit_mc d on d.id_actual=c.id_actual and a.reg_date >= d.start_dt and a.reg_date<= d.end_dt
            //) aa
            //group by aa.mc_no
            //) cc
            //INNER JOIN 
            //(select aa.mc_no
            //,MAX(aa.end_dt) end_dt FROM(
            //select distinct  a.mt_cd--,e.staff_id
            //,d.mc_no,d.start_dt,d.end_dt
            //from w_material_mapping_mms a
            //inner join w_material_info_mms b on a.mt_lot=b.material_code
            //inner join #tmpactual c on c.id_actual=b.id_actual
            //inner join d_pro_unit_mc d on d.id_actual=c.id_actual and a.reg_date >= d.start_dt and a.reg_date<= d.end_dt

            //) aa
            //group by aa.mc_no
            //) bb on cc.mc_no=bb.mc_no
            //inner join (select distinct  a.mt_cd
            //,d.mc_no,d.start_dt,d.end_dt
            //from w_material_mapping_mms a
            //inner join w_material_info_mms b on a.mt_lot=b.material_code
            //inner join #tmpactual c on c.id_actual=b.id_actual
            //inner join d_pro_unit_mc d on d.id_actual=c.id_actual and a.reg_date >= d.start_dt and a.reg_date<= d.end_dt
            //) dd on cc.mc_no=dd.mc_no



            //SELECT dd.mt_cd,cc.staff_id,cc.start_dt,bb.end_dt 
            //into #tmmppbb
            //FROM (
            //select aa.staff_id,MIN(aa.start_dt) start_dt FROM(
            //select distinct  a.mt_cd,d.staff_id,d.start_dt,d.end_dt
            //from w_material_mapping_mms a
            //inner join w_material_info_mms b on a.mt_lot=b.material_code
            //inner join #tmpactual c on c.id_actual=b.id_actual
            //inner join d_pro_unit_staff d on d.id_actual=c.id_actual and a.reg_date >= d.start_dt and a.reg_date<= d.end_dt
            //--where c.at_no='PO20210627-040'
            //) aa
            //group by aa.staff_id) cc
            //INNER JOIN 
            //(select aa.staff_id,MAX(aa.end_dt)end_dt FROM(
            //select distinct  a.mt_cd,d.staff_id,d.start_dt,d.end_dt
            //from w_material_mapping_mms a
            //inner join w_material_info_mms b on a.mt_lot=b.material_code
            //inner join #tmpactual c on c.id_actual=b.id_actual
            //inner join d_pro_unit_staff d on d.id_actual=c.id_actual and a.reg_date >= d.start_dt and a.reg_date<= d.end_dt
            //--where c.at_no='PO20210627-040'
            //) aa
            //group by aa.staff_id) bb on cc.staff_id=bb.staff_id
            //inner join (select distinct  a.mt_cd,d.staff_id
            //from w_material_mapping_mms a
            //inner join w_material_info_mms b on a.mt_lot=b.material_code
            //inner join #tmpactual c on c.id_actual=b.id_actual
            //inner join d_pro_unit_staff d on d.id_actual=c.id_actual and a.reg_date >= d.start_dt and a.reg_date<= d.end_dt
            //--where c.at_no='PO20210627-040'
            //) dd on cc.staff_id=dd.staff_id


            //SELECT dd.mt_cd,dd.id_actual,
            //CONCAT('<b>', ee.mc_no, '</b>','<br> <i>Start: ',ee.start_dt ,'<br> End: ',ee.end_dt ) AS machine,
            //	CONCAT('<b>', cc.uname, '</b>', '<i> <br> Start: ',ff.start_dt, ' <br> End: ', ff.end_dt) AS congnhan_time,
            //dd.name,c.dt_nm process,dd.expiry_dt,dd.dt_of_receipt,dd.expore_dt,dd.lot_no,dd.mt_no,SUM(SLLD)SLLD ,
            //(CONCAT(q.width, 'MM x ', q.spec, 'M')) size,q.mt_nm,dd.level
            //into #tmpa
            //FROM(
            //select  mp.mt_cd,wa.id_actual,wa.name,
            //(CASE WHEN mt.gr_qty IS NULL THEN inv.gr_qty
            //ELSE mt.gr_qty END )SLLD,
            //(CASE WHEN mt.expiry_date IS NULL THEN inv.expiry_date
            //ELSE mt.expiry_date END )expiry_dt,
            //(CASE WHEN mt.date_of_receipt IS NULL THEN inv.date_of_receipt
            //ELSE mt.date_of_receipt END )dt_of_receipt,
            //(CASE WHEN mt.export_date IS NULL THEN inv.export_date
            //ELSE mt.export_date END )expore_dt,
            //(CASE WHEN mt.lot_no IS NULL THEN inv.lot_no
            //ELSE mt.lot_no END )lot_no,
            //(CASE WHEN mt.mt_no IS NULL THEN inv.mt_no
            //ELSE mt.mt_no END )mt_no,wa.level
            //from w_material_mapping_mms mp
            //left join w_material_info_mms mt on mt.material_code=mp.mt_cd and mt.material_type!='CMT'
            //left join inventory_products inv on inv.material_code=mp.mt_cd and inv.mt_type!='CMT'
            //join w_material_info_mms inmm on inmm.material_code=mp.mt_lot
            //join #tmpactual wa on wa.id_actual=inmm.id_actual 
            //--where wa.at_no='PO20210627-040' --and mp.use_yn='Y' --and inmm.material_type != 'CMT'
            //) dd
            //LEFT JOIN d_material_info AS q ON q.mt_no =dd.mt_no
            //JOIN comm_dt AS c ON c.dt_cd=dd.name 			AND c.mt_cd ='COM007'
            //inner join  #tmmppaa ee on dd.mt_cd=ee.mt_cd
            //inner join #tmmppbb ff on ff.mt_cd=ee.mt_cd
            //inner join mb_info cc on ff.staff_id=cc.userid 
            //where dd.SLLD>0 --and dd.name='ROT'
            //GROUP BY dd.mt_cd,dd.id_actual,dd.name,c.dt_nm,q.width,q.spec,dd.expiry_dt,
            //dd.dt_of_receipt,dd.expore_dt,dd.lot_no,dd.mt_no,q.mt_nm,dd.level,ee.mc_no,ee.start_dt, ee.end_dt,ff.staff_id,cc.uname,ff.start_dt,ff.end_dt
            //order by dd.name,ff.staff_id,ee.mc_no




            //SELECT mt_no,mt_nm, process,congnhan_time, Sum(SLLD) AS quantity 
            //		 into #tmpb
            //	 FROM #tmpa
            //	 GROUP BY mt_no,mt_nm,process, congnhan_time;
            //	  SELECT  t.mt_cd,
            //            Concat( '<div style=""""text - align:center; """">',t.mt_no, '<br>(', t2.quantity, ')', '</div>' ) as mt_no,
            //            t.process,
            //            t.machine,
            //            t.congnhan_time,
            //            t.size,
            //            t.mt_nm,
            //            CONVERT(VARCHAR(10),CONVERT(DATETIME,t.expiry_dt,120),120)  expiry_dt,
            //            CONVERT(VARCHAR(10),CONVERT(DATETIME,t.dt_of_receipt,120),120)  dt_of_receipt,
            //            CONVERT(VARCHAR(10),CONVERT(DATETIME,t.expore_dt,120),120) expore_dt,
            //            t.lot_no,
            //            t.SLLD
            //    FROM #tmpa t
            //    JOIN #tmpb t2 on t.mt_no = t2.mt_no and t.mt_nm = t2.mt_nm and t.process = t2.process and t.congnhan_time = t2.congnhan_time
            //    ORDER BY t.level asc;
            //            drop table #tmpa
            //	drop table #tmpb

            //drop table #tmmppaa
            //drop table #tmmppbb
            //drop table #tmpactual
            //";
            //           string sql = @"
            //SELECT a.material_code mt_cd,a.mt_no,c.dt_nm AS process,
            //CONCAT('<b>', l.mc_no, '</b>','<br> <i>Start: ',CAST(l.start_dt AS DATETIME),'<br> End: ',CAST( l.end_dt AS DATETIME)) AS machine,
            //CONCAT('<b>', k.uname, '</b>', '<i> <br> Start: ',CAST(k.start_dt AS DATETIME), ' <br> End: ', CAST(k.end_dt AS DATETIME)) AS congnhan_time,

            //(CONCAT(q.width, 'MM x ', q.spec, 'M')) size,
            //           q.mt_nm,
            //          CONVERT(VARCHAR, a.expiry_date,112) expiry_dt,
            //          CONVERT(VARCHAR, a.date_of_receipt,112) dt_of_receipt,
            //          CONVERT(VARCHAR, a.export_date,112) expore_dt,
            //           a.lot_no,
            //           a.gr_qty SLLD,
            //           b.level
            //into #tmpa
            //FROM w_material_info_mms AS a
            //JOIN w_actual AS b ON a.id_actual=b.id_actual
            // left JOIN comm_dt AS c ON c.dt_cd=b.name AND c.mt_cd ='COM007'
            //left JOIN 
            //	(
            //		SELECT k.start_dt,k.end_dt,k.id_actual,n.uname, k.staff_id
            //		FROM d_pro_unit_staff AS k 
            //		JOIN mb_info AS n ON n.userid=k.staff_id
            //	) AS k ON k.id_actual = a.id_actual AND a.id_actual = k.id_actual
            //left JOIN d_pro_unit_mc AS l ON l.id_actual=a.id_actual  AND l.start_dt<=a.reg_date and a.reg_date<=l.end_dt
            //LEFT JOIN d_material_info AS q ON q.mt_no =a.mt_no
            //WHERE  a.material_type!='CMT' AND b.at_no=@po
            //order BY a.material_code,a.mt_no

            // SELECT mt_no,mt_nm, process,congnhan_time, Sum(SLLD) AS quantity 
            //	 into #tmpb
            // FROM #tmpa
            // GROUP BY mt_no,mt_nm,process, congnhan_time;
            //  SELECT  t.mt_cd,
            //           Concat( '<div style=""""text - align:center; """">',t.mt_no, '<br>(', t2.quantity, ')', '</div>' ) as mt_no,
            //           t.process,
            //           t.machine,
            //           t.congnhan_time,
            //           t.size,
            //           t.mt_nm,
            //           t.expiry_dt,
            //           t.dt_of_receipt,
            //           t.expore_dt,
            //           t.lot_no,
            //           t.SLLD
            //   FROM #tmpa t
            //   JOIN #tmpb t2 on t.mt_no = t2.mt_no and t.mt_nm = t2.mt_nm and t.process = t2.process and t.congnhan_time = t2.congnhan_time
            //   ORDER BY t.level asc;
            //           drop table #tmpa
            //drop table #tmpb";
            try
            {
                var sql = await base.DbConnection.QueryAsync<FGProductLotPO>("SET ARITHABORT ON; EXEC[dbo].[SearchHistoryPO]@at_no", new { @at_no = atno }, commandTimeout: 180); 
                return sql.ToList();
            }
            catch (Exception e)
            {

                throw;
            }
          
        }

        //CHARINDEX
        public async Task<IReadOnlyList<generalfg>> GetExportShippingScanToExcel(string dlNo)
        {
            //var listData = dlNo.Split(',');
            //IEnumerable<generalfg> result = new List<generalfg>();
            //foreach (var item in listData)
            //{
                string sql = $"SELECT max(b.id) id, SUM(b.qty) qty, MAX(b.product_code)product, MAX(b.lot_no)AS end_production_dt, MAX(dl.work_dt) work_dt FROM generalfg AS b left JOIN w_dl_info AS  dl ON  b.dl_no = dl.dl_no WHERE dl.dlid in({dlNo}) GROUP BY  b.product_code,b.lot_no ORDER BY b.product_code";
             var   result = await base.DbConnection.QueryAsync<generalfg>(sql/*, new { dlNo = item }*/);
         //   }
            return result.ToList();
        }

        //CHARINDEX
        public async Task<IReadOnlyList<generalfg>> GetShippingDLExportExcel(string dlNo)
        {
            //var listData = dlNo.Split(',');
            //IEnumerable<generalfg> result = new List<generalfg>();
            //foreach (var item in listData)
            //{
                string sql = $"SELECT  c.dl_no, c.buyer_qr,  d.bx_no as box_code, REPLACE(c.lot_no, '-', '')  lot_no FROM generalfg c JOIN w_box_mapping AS d ON c.buyer_qr = d.buyer_cd WHERE c.dl_no IN(SELECT e.dl_no FROM  w_dl_info As e WHERE e.dlid in ({dlNo})) order by  c.buyer_qr";
               var result = await base.DbConnection.QueryAsync<generalfg>(sql/*, new { dlNo = item }*/);
           // }

            return result.ToList();
        }
        public async Task<int> UpdateStampDetailCancel1ThungDelivery(string boxno)
        {
            string sql = @"update stamp_detail set box_code = NULL , is_sent = 'N' where box_code =  @boxno;";
            return await base.DbConnection.ExecuteAsync(sql, new { boxno = boxno });
        }
        public async Task<int> UpdatewmaterialinfowithBuyerCancel1ThungDelivery(string boxno)
        {
            string sql = @"update w_material_info_tims set status = '001' , dl_no  = NULL ,
	 location_code = '003G01000000000000' , from_lct_code = '006000000000000000' , to_lct_code ='003G01000000000000'
	WHERE buyer_qr in  (SELECT buyerr1.buyer_cd FROM w_box_mapping AS buyerr1 WHERE buyerr1.bx_no =@boxno)";
            return await base.DbConnection.ExecuteAsync(sql, new { boxno = boxno });
        }
        public async Task<int> UpdategeneralfgCancel1ThungDelivery(string boxno)
        {
            string sql = @"	update generalfg set status = '001' ,dl_no  = NULL 
	WHERE buyer_qr in  (SELECT buyerr1.buyer_cd FROM w_box_mapping AS buyerr1 WHERE buyerr1.bx_no =@boxno)   ;";
            return await base.DbConnection.ExecuteAsync(sql, new { boxno = boxno });
        }
        public async Task<int> DeletewboxmappingCancel1ThungDelivery(string boxno)
        {
            string sql = @"	DELETE FROM w_box_mapping WHERE bx_no = @boxno;";
            return await base.DbConnection.ExecuteAsync(sql, new { boxno = boxno });
        }
        public async Task<int> UpdatewmaterialinfowithdlnoCancelDeliveryAll(string dlno)
        {
            string sql = @"update w_material_info_tims  set status = '001' , dl_no  = NULL ,
	 location_code = '003G01000000000000' , from_lct_code = '006000000000000000' , to_lct_code ='003G01000000000000'
	WHERE dl_no = @dlno";
            return await base.DbConnection.ExecuteAsync(sql, new { dlno = dlno });
        }
        public async Task<int> DeletewboxmappingdlnoCancelDeliveryAll(string dlno)
        {
            string sql = @"	DELETE FROM w_box_mapping WHERE buyer_cd IN  (SELECT buyerr.buyer_qr FROM w_material_info_tims AS buyerr WHERE buyerr.dl_no = @dlno);";
            return await base.DbConnection.ExecuteAsync(sql, new { dlno = dlno });
        }

        public async Task<int> UpdatestampdetailCancelDeliveryAll(string dlno)
        {
            string sql = @"	update stamp_detail set box_code = NULL , is_sent = 'N' 
	where buyer_qr IN  (SELECT buyerr1.buyer_qr FROM w_material_info_tims AS buyerr1 WHERE buyerr1.dl_no = @dlno);";
            return await base.DbConnection.ExecuteAsync(sql, new { dlno = dlno });
        }
        public async Task<int> DeletewdlinfodlnoCancelDeliveryAll(string dlno)
        {
            string sql = @"	DELETE FROM w_dl_info WHERE dl_no = @dlno;";
            return await base.DbConnection.ExecuteAsync(sql, new { dlno = dlno });
        }
        public async Task<IReadOnlyList<w_dl_info>> Getwdlinfodlno(string dlno)
        {
            string sql = @"select * from w_dl_info where dl_no=@dlno";
            var result = await base.DbConnection.QueryAsync<w_dl_info>(sql, new { dlno = dlno });
            return result.ToList();
        }
        public async Task<IReadOnlyList<spFGWMMSGetFGGeneral>> spFGWMMSGetFGGeneral(string productCode, string poCode, string recevice_dt_start, string recevice_dt_end, string buyerCode)
        {
            string sql = @"SELECT
                d.id_actualpr,
                       SUM((CASE
                                WHEN b.bundle_unit ='Roll' THEN (isnull(a.gr_qty,0)/isnull(b.spec, 1))
                                ELSE isnull(a.gr_qty, 0)
                            END)) qty,
                       b.bundle_unit,

                       d.product,
			                (SELECT top 1 f.style_nm FROM d_style_info f WHERE f.style_no = d.product) AS product_name,
			                (SELECT top 1 f.md_cd FROM d_style_info f WHERE f.style_no = d.product) AS model,
                  (SELECT dt_nm
                   FROM comm_dt
                   WHERE comm_dt.dt_cd=a.status
                     AND comm_dt.mt_cd='WHS005') sts_nm
                FROM w_material_info_tims a
                LEFT JOIN d_material_info b ON a.mt_no=b.mt_no
                JOIN w_actual c ON c.id_actual = a.id_actual
                JOIN w_actual_primary d ON c.at_no = d.at_no

                WHERE a.location_code LIKE '003%'
                  AND d.product LIKE '%'+@productCode+'%'
                   AND a.buyer_qr LIKE '%'+@buyerCode+'%' 
                  AND c.at_no LIKE '%'+@poCode+'%'   
  

                  AND  (((@start != '00010101' AND a.input_dt IS not NULL)  AND (CAST(@start AS INT) <= CAST(Replace(CAST(CONVERT(date,a.input_dt)AS varchar),'-','') AS INT)  ))
                  OR (a.input_dt IS NULL AND 1 = 0)
                  OR (''=''))
                  AND (((@end != '99991231' AND a.input_dt IS not NULL)  AND (CAST(Replace(CAST(CONVERT(date,a.input_dt)AS varchar),'-','') AS INT) <= CAST(@end AS INT)))
                  OR (a.input_dt IS NULL AND 1 = 0)
                  OR (''=''))
  		

                group BY d.id_actualpr,b.bundle_unit,d.product,a.status";
            var result = await base.DbConnection.QueryAsync<spFGWMMSGetFGGeneral>(sql, new { productCode = productCode, buyerCode = buyerCode, poCode = poCode, start = recevice_dt_start, end = recevice_dt_end });
            return result.ToList();
        }
        public async Task<IReadOnlyList<MaterialInfoTIMS>> GetDLShippingScanListPP(string dlno)
        {
            try
            {
                string sql = @"SELECT  c.id AS wmtid, c.buyer_qr,c.qty gr_qty, d.bx_no AS box_no, Convert(varchar(20), c.chg_dt, 120) AS shippingDt, c.lot_no
                               FROM generalfg c
                               JOIN w_box_mapping AS  d ON c.buyer_qr = d.buyer_cd
                               WHERE c.dl_no = @dlno";
                var result = await base.DbConnection.QueryAsync<MaterialInfoTIMS>(sql, new { dlno = dlno });
                return result.ToList();
            }
            catch (Exception e)
            {

                throw e;
            }

        }
        public async Task<IReadOnlyList<w_box_mapping>> Getlistwboxmappingwithboxno(string boxno)
        {
            string sql = @"select * from w_box_mapping where box_no=@boxno";
            var result = await base.DbConnection.QueryAsync<w_box_mapping>(sql, new { boxno = boxno });
            return result.ToList();

        }
        public async Task<IReadOnlyList<ScanBoxModel>> GetBoxCodeScanning(string boxno)
        {
            string sql = @"SELECT
		a.bx_no , SUM(a.gr_qty) AS totalQty, a.status,
		(select top 1 b.dt_nm from comm_dt b where b.mt_cd = 'WHS013' and b.dt_cd = a.status) as statusName
	FROM w_box_mapping a
	WHERE a.bx_no = @boxCode
	GROUP BY a.bx_no,a.status;";
            var result = await base.DbConnection.QueryAsync<ScanBoxModel>(sql, new { boxCode = boxno });
            return result.ToList();
        }
        public async Task<int> UnMappingBox(string boxno)
        {
            string sql = @"update w_material_info_tims set status = '001' where material_code in (select b.mt_cd from w_box_mapping b where b.bx_no = @boxCode);
	update generalfg  set status = '001' where buyer_qr in (select b.buyer_cd from w_box_mapping b where b.bx_no = @boxCode);
	DELETE FROM w_box_mapping WHERE bx_no = @boxCode AND status = '000';
	update stamp_detail set box_code = Null where box_code = @boxCode;";
            return await base.DbConnection.ExecuteAsync(sql, new { boxCode = boxno });
        }
        public async Task<int> UnMappingBuyer(string buyerCode)
        {
            string sql = @"update w_material_info_tims  set status = '001' where buyer_qr = @boxCode;
	DELETE FROM w_box_mapping WHERE buyer_cd = @boxCode AND status = '000';
	update stamp_detail set box_code = NULL where buyer_qr = @boxCode and (is_sent is null or is_sent = 'N');";
            return await base.DbConnection.ExecuteAsync(sql, new { boxCode = buyerCode });
        }

        public async Task<IEnumerable<string>> GetListStampData(string html)
        {
            try
            {

                 var result = new List<string>();
                //var listData = html.Split(',');
                //foreach (var item in listData)
                //{
                    var query = $"SELECT buyer_cd FROM w_box_mapping WHERE bx_no in ({html})";
                    var rs = await base.DbConnection.QueryAsync<string>(query);
                    result.AddRange(rs);
                return result;
                //}
                //return result;

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<Generalfg>> GetListStampBuyerQr(string buyerCode, string prouductCode, int intpage, int introw)
        {
            try
            {
                var query = @"SELECT * FROM generalfg AS a 
                            WHERE a.status ='001' 
                            AND (@BuyerCode = '' OR @BuyerCode IS NULL OR buyer_qr Like '%' + @BuyerCode + '%') 
                            AND (@ProductCode = '' OR @ProductCode IS NULL OR product_code Like '%' + @ProductCode + '%')
                            Order By a.id OFFSET @intpage ROWS FETCH NEXT @introw ROWS ONLY";
                var result = await base.DbConnection.QueryAsync<Generalfg>(query, new { @BuyerCode = buyerCode, @ProductCode = prouductCode, @intpage = intpage, @introw = introw });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public async Task<int> CountStampBuyerQr(string buyerCode, string prouductCode)
        {
            try
            {
                var query = @"SELECT Count(*) FROM generalfg AS a 
                            WHERE a.status ='001' 
                            AND (@BuyerCode = '' OR @BuyerCode IS NULL OR buyer_qr Like '%' + @BuyerCode + '%') 
                            AND (@ProductCode = '' OR @ProductCode IS NULL OR product_code Like '%' + @ProductCode + '%')";
                var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @BuyerCode = buyerCode, @ProductCode = prouductCode });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public async Task<StampDetail> GetDataStampDetail(string id)
        {
            try
            {
                var query = @"Select * From stamp_detail WHERE id = @Id";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<StampDetail>(query, new { @Id = id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<generalfg>> GetListGeneralFGforExport(string productCode, string buyerCode, string recevice_dt_start, string recevice_dt_end)
        {
            try
            {


                string sql = @"SELECT a.[id]
      ,a.[buyer_qr]
      ,a.[product_code]
      ,a.[at_no]
      ,a.[type]
      ,a.[md_cd]
      ,a.[dl_no]
      ,a.[qty]
      ,a.[lot_no]
      ,a.[status] sts_cd
      ,a.[use_yn]
      ,a.[reg_id]
      ,convert(datetime,a.[reg_dt],121)
      ,a.[chg_id]
      ,convert(datetime,a.[chg_dt],121),
                    (SELECT dt_nm FROM comm_dt WHERE comm_dt.mt_cd = 'WHS005' AND comm_dt.dt_cd = a.status) AS statusName
                    FROM generalfg as a  
                    where a.product_code LIKE '%'+ @productCode+ '%' AND a.buyer_qr LIKE '%'+@buyerCode+ '%'
                    AND a.status in ('001', '010')   AND
                    (
                    ((@recevice_dt_start != '00010101' AND a.reg_dt IS not NULL) and convert(date,a.reg_dt, 121) >= convert(date,@recevice_dt_start, 121))
                    or ((a.reg_dt IS NULL)) or ('' = '')
                    )
                    and 
                    (
                    ((@recevice_dt_end != '99991231' AND a.reg_dt IS not NULL) and  convert(date,a.reg_dt, 121) <= convert(date,@recevice_dt_end, 121))
                    or ((a.reg_dt IS NULL)) or ('' = '')
                    )
                    ";
                var result = await base.DbConnection.QueryAsync<generalfg>(sql, new { productCode = productCode, buyerCode = buyerCode, recevice_dt_start = recevice_dt_start, recevice_dt_end = recevice_dt_end });
                return result.ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }




        public async Task<string> GetBuyerQRFromInfoTims(int id)
        {
            try
            {
                var query = @"Select buyer_qr From w_material_info_tims where wmtid = @Id";
                var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { @Id = id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IReadOnlyList<FGShippingScanExportExcelModel>> GetDeliveryFG(string dl_no)
        {
            try
            {
                var query = @"SELECT max(b.id) id, SUM(b.qty) qty, MAX(b.product_code)product, MAX(b.lot_no)AS end_production_dt, 
             MAX(b.dl_no) dl_no, MAX(dl.work_dt) work_dt
                    FROM generalfg AS b
                    left JOIN w_dl_info AS  dl ON  b.dl_no = dl.dl_no
                    WHERE b.dl_no = @dl_no
                    GROUP BY b.product_code,b.lot_no  ORDER BY b.product_code ";

                var result = await base.DbConnection.QueryAsync<FGShippingScanExportExcelModel>(query, new { dl_no = dl_no });
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateQtyGenneral(generalfg item, string id)
        {
            try
            {
                //int result = 0;
                //string[] listData = id.Split(',');
                //foreach (var it in listData)
                //{
                    string sqlupdate = $"Update generalfg SET qty= '{item.qty}' , chg_id = '{item.chg_id}', chg_dt = '{item.chg_dt}'                                            WHERE id in ({id}) ";
                    var result = await base.DbConnection.ExecuteAsync(sqlupdate/*, new { qty = item.qty, chg_id = item.chg_id, chg_dt = item.chg_dt, it = it }*/);
                    //var asf = _db.Database.ExecuteSqlCommand(sqlupdate,
                    //     new MySqlParameter("1", id),
                    //     new MySqlParameter("2", item.qty),
                    //     new MySqlParameter("3", item.chg_id),
                    //     new MySqlParameter("4", item.chg_dt));
               // }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<int> UpdateLotNoGenneral(generalfg item, string id)
        {
            try
            {
                //int result = 0;
                //string[] listData = id.Split(',');
                //foreach (var it in listData)
                //{
                    string sqlupdate = $"Update generalfg SET lot_no= '{item.lot_no}', chg_id = '{item.chg_id}', chg_dt = '{item.chg_dt}'                                            WHERE  id in ({id}) AND ( ( SELECT productType FROM d_style_info WHERE d_style_info.style_no = generalfg.product_code )= 1)";
                   var result = await base.DbConnection.ExecuteAsync(sqlupdate/*, new { lot_no = item.lot_no, chg_id = item.chg_id, chg_dt = item.chg_dt, it = it }*/);
                //}
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<IReadOnlyList<generalfg>> GetFGGeneralById(string id)
        {
            try
            {
                    string sqlupdate = $" select * from generalfg where id in ({id})";
                    var result = await base.DbConnection.QueryAsync<generalfg>(sqlupdate);
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateQtyGeneral(generalfg item)
        {
            try
            {
                int result = 0;
                string sqlupdate = @"Update generalfg SET qty= @qty , chg_id = @chg_id, chg_dt = @chg_dt
                                            WHERE id = @it  ";
                result = await base.DbConnection.ExecuteAsync(sqlupdate, new { qty = item.qty, chg_id = item.chg_id, chg_dt = item.chg_dt, it = item.id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> TotalRecordsSearchShippingSortingFG(string ShippingCode, string productCode, string productName, string description)
        {
            try
            {
                int result = 0;
                string countSql = @"SELECT COUNT(*) 
	                    FROM   shippingfgsorting AS a
	                   Where  a.IsFinish='N' AND ((@ShippingCode='' OR  a.ShippingCode like @ShippingCode ) 
                                AND (@productCode='' OR  a.ProductCode like @productCode )
                                AND (@productName='' OR  a.ProductName like @productName )
                                AND (@description='' OR  a.Description like @description ))
                ";
                result = await base.DbConnection.ExecuteAsync(countSql, new { ShippingCode = ShippingCode, productCode = productCode, productName = productName, description = description });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IReadOnlyList<ShippingFGSortingModel>> GetListSearchShippingSortingFG(string ShippingCode, string productCode, string productName, string description)
        {
            try
            {
                string viewSql = @" SELECT a.* 
              
	                    FROM   shippingfgsorting AS a
	                    Where a.IsFinish='N' AND ((@ShippingCode='' OR  a.ShippingCode like @ShippingCode ) 
                                AND (@productCode='' OR  a.ProductCode like @productCode )
                                AND (@productName='' OR  a.ProductName like @productName )
                                AND (@description='' OR  a.Description like @description ))
	           
                    order by a.id desc ";
                var result = await base.DbConnection.QueryAsync<ShippingFGSortingModel>(viewSql, new { ShippingCode = ShippingCode, productCode = productCode, productName = productName, description = description });
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<ShippingFGSortingModel> GetLastShippingFGSorting()
        {
            try
            {
                string viewSql = @" SELECT top 1 a.ShippingCode, a.id
              
	                    FROM   shippingfgsorting AS a
	           
                    order by a.id desc   ";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<ShippingFGSortingModel>(viewSql);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<int> InsertToShippingFGSorting(ShippingFGSortingModel item)
        {
            try
            {
                string QuerySQL = @"INSERT INTO shippingfgsorting (ShippingCode,ProductCode,ProductName,IsFinish,Description,CreateId,CreateDate,ChangeId,ChangeDate)
            VALUES (@ShippingCode,@productCode, @ProductName, @IsFinish, @description, @CreateId, @CreateDate, @ChangeId, @ChangeDate);
            SELECT SCOPE_IDENTITY();";

                var result = await base.DbConnection.ExecuteAsync(QuerySQL, new
                {
                    ShippingCode = item.ShippingCode,
                    productCode = item.ProductCode,
                    ProductName = item.ProductName,
                    IsFinish = item.IsFinish,
                    description = item.Description,
                    CreateId = item.CreateId,
                    CreateDate = item.CreateDate,
                    ChangeId = item.ChangeId,
                    ChangeDate = item.ChangeDate
                });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<int> ModifyShippingFGSorting(ShippingFGSortingModel item)
        {
            try
            {
                string QuerySQL = @"UPDATE shippingfgsorting SET 
                                  ProductCode=@productCode,ProductName= @ProductName,Description=@description,CreateId=@CreateId,ChangeId=@ChangeId,ChangeDate=@ChangeDate
                                   WHERE id=@ID";
                var result = await base.DbConnection.ExecuteAsync(QuerySQL, new
                {
                    productCode = item.ProductCode,
                    ProductName = item.ProductName,
                    description = item.Description,
                    CreateId = item.CreateId,
                    CreateDate = item.CreateDate,
                    ChangeId = item.ChangeId,
                    ChangeDate = item.ChangeDate,
                    ID = item.id
                });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<generalfg> CheckIsExistBuyerCode(string BuyerCode)
        {
            try
            {
                string QuerySQL = @"SELECT top 1 a.*
                                FROM generalfg as a WHERE a.buyer_qr = @BuyerCode ";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<generalfg>(QuerySQL, new
                {
                    BuyerCode = BuyerCode
                });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateShippingSortingFG(generalfg item, string data)
        {
            try
            {
                int result = 0;
                var listData = data.Split(',');
                foreach (var it in listData)
                {
                    string sqlupdate = @"Update generalfg SET status=@item
                            WHERE  id = @it ";
                    result = await base.DbConnection.ExecuteAsync(sqlupdate, new
                    {
                        item = item.sts_cd,
                        it = it
                    });
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<IReadOnlyList<ShippingFGSortingModel>> CheckBuyerCodeShippingSortingFGById(string data)
        {
            try
            {
                    string sqlupdate = @"select a.* from shippingfgsortingdetail a 
                                        join generalfg b on b.buyer_qr = a.buyer_qr
                                        where b.id = @data";
                   var  result = await base.DbConnection.QueryAsync<ShippingFGSortingModel>(sqlupdate, new
                    {
                       data = data
                   });
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<int> InsertShippingSortingFGDetail(string ShippingCode, string ListId, string UserID)
        {
            try
            {

                int result = 0;
                var listData = ListId.Split(',');
                foreach (var it in listData)
                {
                    var CheckBuyerQR = await CheckBuyerCodeShippingSortingFGById(it);
                    if(CheckBuyerQR.Count() <= 0)
                    {
                        string QuerySQL = @"INSERT INTO shippingfgsortingdetail(ShippingCode,buyer_qr,CreateId,Model,productCode,lot_no,Quantity,location)
                                  SELECT @ShippingCode, a.buyer_qr, @UserID, a.md_cd,a.product_code,a.lot_no, a.qty,a.dl_no
                                  FROM generalfg  as a
                                  WHERE  a.id= @it and @it != 0 ;
                             ";
                        result = await base.DbConnection.ExecuteAsync(QuerySQL, new
                        {
                            ShippingCode = ShippingCode,
                            it = it,
                            UserID = UserID
                        });
                    }
                    else
                    {
                        return -1;
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<IReadOnlyList<ShippingFGSortingModel>> GetListSearchShippingSortingFGPP(string ShippingCode)
        {
            try
            {
                string viewSql = @" SELECT a.* 
              
	                    FROM   shippingfgsorting AS a
	                    Where  a.ShippingCode = @ShippingCode
	                        ";
                var result = await base.DbConnection.QueryAsync<ShippingFGSortingModel>(viewSql, new
                {
                    ShippingCode = ShippingCode
                });
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IReadOnlyList<ShippingFGSortingDetailModel>> GetListShippingFGSorting(string ShippingCode)
        {
            try
            {
                string QuerySQL = @"SELECT a.*,( CASE 
                                         WHEN (a.location  =  '006000000000000000' ) THEN 'TIMS'
                                        WHEN (a.location IS NOT NULL AND a.location  !=  '006000000000000000' )then a.location 
                                       else 'FG' END) AS locationname
                        
                        FROM shippingfgsortingdetail as a WHERE ShippingCode = @ShippingCode";
                var result = await base.DbConnection.QueryAsync<ShippingFGSortingDetailModel>(QuerySQL, new
                {
                    ShippingCode = ShippingCode
                });
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<ShippingFGSortingDetailModel> CheckFGSortingdetail(string buyer_qr)
        {
            try
            {
                string getvalue = @"SELECT top 1 *
                            FROM shippingfgsortingdetail 
                            WHERE buyer_qr = @buyer_qr ";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<ShippingFGSortingDetailModel>(getvalue, new
                {
                    buyer_qr = buyer_qr
                });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<generalfg> isCheckExistGenneral(string buyer_qr)
        {
            try
            {
                string getvalue = @"SELECT top 1 *
                            FROM generalfg  
                            WHERE buyer_qr = @buyer_qr ";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<generalfg>(getvalue, new
                {
                    buyer_qr = buyer_qr
                });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateBuyerGeneral(generalfg item)
        {
            try
            {
                string sqlupdate = @"Update generalfg SET status = @status
                            WHERE  buyer_qr = @buyer_qr ";
                var result = await base.DbConnection.ExecuteAsync(sqlupdate, new
                {
                    status = item.sts_cd,
                    buyer_qr = item.buyer_qr
                });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> DeleteFGSotingBuyer(string buyer_qr)
        {
            try
            {
                string sqlquery = @"DELETE FROM shippingfgsortingdetail WHERE buyer_qr=@buyer_qr  ";
                var result = await base.DbConnection.ExecuteAsync(sqlquery, new
                {
                    buyer_qr = buyer_qr
                });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<IReadOnlyList<ShippingFGSortingDetailModel>> GetShippingScanPPCountbuyer(string ShippingCode)
        {
            try
            {
                string sqlquery = @"SELECT max(a.productCode)productCode ,MAX(a.Model) Model, sum(a.Quantity) sumQuantity, COUNT(a.buyer_qr) countBuyer
                                  FROM shippingfgsortingdetail AS a
                                  WHERE a.ShippingCode =@ShippingCode
                                  GROUP BY a.ShippingCode; ";
                var result = await base.DbConnection.QueryAsync<ShippingFGSortingDetailModel>(sqlquery, new
                {
                    ShippingCode = ShippingCode
                });
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<int> CheckMaterialEP1(string ExportCode)
        {
            try
            {
                string QuerySQL = "SELECT top 1 ExportCode FROM w_material_info WHERE ExportCode = @ExportCode";
                var result = await base.DbConnection.ExecuteAsync(QuerySQL, ExportCode);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> DeleteToExportToMachine(int id)
        {
            try
            {
                string sqlquery = @"DELETE FROM shippingfgsorting WHERE Id = @id";
                var result = await base.DbConnection.ExecuteAsync(sqlquery, new { @id = id });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<ShippingTIMSSortingDetailModel> isCheckExistSF(string ShippingCode, string buyer_qr)
        {
            try
            {
                string getvalue = @"SELECT *
                            FROM shippingtimssortingdetail 
                            WHERE ShippingCode = @ShippingCode and buyer_qr = @buyer_qr";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<ShippingTIMSSortingDetailModel>(getvalue, new { @ShippingCode = ShippingCode, buyer_qr = buyer_qr });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<string> CheckStatus(string status)
        {
            try
            {
                string sqlquery = @"SELECT dt_nm FROM comm_dt WHERE  dt_cd = @status and mt_cd = 'WHS005' ";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<string>(sqlquery, new { @status = status });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<int> UpdateLocationtimSorting(ShippingTIMSSortingDetailModel item)
        {
            try
            {
                string sqlupdate = @"Update shippingtimssortingdetail SET location = @location, ChangeId=@ChangeId
                            WHERE  buyer_qr = @buyer_qr ";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<int>(sqlupdate, new { @location = item.location, @ChangeId = item.ChangeId, @buyer_qr = item.buyer_qr });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<MaterialInfoTIMS> isCheckExistWmaterialBuyer(string buyer_qr)
        {
            try
            {
                string getvalue = @"SELECT *
                            FROM w_material_info_tims 
                            WHERE  buyer_qr = @buyer_qr";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<MaterialInfoTIMS>(getvalue, new { @buyer_qr = buyer_qr });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<int> UpdateWMaterialInfoStatus(MaterialInfoTIMS item)
        {
            try
            {
                string sqlupdate = @"Update w_material_info_tims SET status = @mt_sts_cd, location_code=@lct_cd
                            WHERE  buyer_qr = @buyer_qr ";
                var result = await base.DbConnection.ExecuteAsync(sqlupdate, new { @mt_sts_cd = item.status, @lct_cd = item.location_code, @buyer_qr = item.buyer_qr });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<IReadOnlyList<ShippingTIMSSortingDetailModel>> CheckSTinfo(string ShippingCode)
        {
            try
            {
                string QuerySQL = "SELECT count(*) ShippingCode FROM shippingtimssorting WHERE ShippingCode = @ShippingCode";
                var result = await base.DbConnection.QueryAsync<ShippingTIMSSortingDetailModel>(QuerySQL, new { @ShippingCode = ShippingCode });
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<StyleInfo> GetStyleInfoReplace(string product_code)
        {
            try
            {
                var query = @"	Select Top 1 * From d_style_info Where REPLACE(style_no, '-', '') = @ProCode ";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<StyleInfo>(query, new { @ProCode = product_code });
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public IEnumerable<FGGeneral> GetFGGeneraldetail(string bom_type, string productCode, string productName, string recevice_dt_start, string recevice_dt_end, string buyerCode)
        {
            try
            {
                IEnumerable<FGGeneral> datas = null;
                var sql = @" SELECT a.*, b.dt_nm statusName
                                 FROM generalfg as a
                                 Join comm_dt b on b.mt_cd ='WHS005' AND b.dt_cd = a.status
                                 where a.status in ('001','010') 
                                    and product_code = @ProCode 
                             order by a.reg_dt desc";
                datas = base.DbConnection.Query<FGGeneral>(sql, new { ProCode = productCode });

                datas.ToList();
                if (datas != null)
                {
                    if (string.IsNullOrEmpty(buyerCode) == false)
                    {
                        datas = datas.Where(item => item.buyer_qr.Contains(buyerCode));
                    }
                    if (string.IsNullOrEmpty(bom_type) == false)
                    {
                        datas = datas.Where(item => item.bom_type.Contains(bom_type));
                    }
                    if (string.IsNullOrEmpty(productName) == false)
                    {
                        datas = datas.Where(item => item.product_name.Contains(productName));
                    }
                    if (string.IsNullOrEmpty(recevice_dt_start) == false)
                    {
                        datas = datas.Where(item => item.reg_dt.Date >= DateTime.ParseExact(recevice_dt_start, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                    }
                    if (string.IsNullOrEmpty(recevice_dt_end) == false)
                    {
                        datas = datas.Where(item => item.reg_dt.Date <= DateTime.ParseExact(recevice_dt_end, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                    }
                }
                return datas;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public async Task<string> GetListAuthorMenuInfo(string user_id, string url)
        {

            try
            {
                var query = @"SELECT role from author_menu_info as a  where  a.at_cd = @userid and REPLACE(a.url_link, '/', '') =@url_link";
                var result = await base.DbConnection.ExecuteScalarAsync<string>(query, new { userid = user_id, url_link = url });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> CountListBoxMapping(string boxCode, string ProductCode, string sDate, string BuyerCode)
        {
            var query = @" SELECT Count(MyDerivedTable.bmno)
                                FROM( 
                    SELECT MAX(a.bmno)As bmno, a.bx_no, MAX(a.product) As ProductNo, SUM(a.gr_qty) AS totalQty, MAX(a.status) As status,
	                        (select Top 1 b.dt_nm from comm_dt b where b.mt_cd = 'WHS013' and b.dt_cd = MAX(a.status)) as statusName
                            FROM w_box_mapping a
	                        WHERE (@BoxCode = '' OR @BoxCode IS NULL OR a.bx_no LIKE '%' + @BoxCode +'%')
	                            AND (@ProductCode = '' OR  @ProductCode IS NULL OR a.product LIKE '%'+ @ProductCode +'%')
		                        AND (@BuyerCode = 'undefined' OR @BuyerCode IS NULl OR a.buyer_cd LIKE '%'+ @BuyerCode + '%')
		                        AND (@Date  = '' OR @Date IS NULL OR a.reg_dt LIKE '%'+ @Date + '%')
	                          GROUP BY a.bx_no
							) as MyDerivedTable
                        ";
            var result = await base.DbConnection.ExecuteScalarAsync<int>(query, new { @BoxCode = boxCode, @ProductCode = ProductCode, @BuyerCode = BuyerCode, @Date = sDate });

            return result;
        }
        public async Task<int> UpdateReceFGWMaterial(MaterialInfoTIMS item)
        {
            string sql = @"UPDATE w_material_info_tims set input_dt = @input_dt  ,location_code=@location_code,from_lct_code=@from_lct_code,to_lct_code=@to_lct_code,status=@status,chg_id=@chg_id,chg_date =@chg_date
                      WHERE buyer_qr = @buyer_qr";
            return await base.DbConnection.ExecuteAsync(sql,
                new {
                    input_dt = item.input_dt,
                    location_code = item.location_code,
                    from_lct_code = item.from_lct_code,
                    to_lct_code = item.to_lct_code,
                    status = item.status,
                    chg_id = item.chg_id,
                    chg_date = item.chg_date,
                    buyer_qr = item.buyer_qr

                     });
        }
        public async Task<stamp_detail> FindStamp(string buyer_qr)
        {
            try
            {
                string viewSql = @" SELECT top 1 *
              
	                    FROM   stamp_detail AS a  where a.buyer_qr = @buyer_qr ";
                var result = await base.DbConnection.QueryFirstOrDefaultAsync<stamp_detail>(viewSql, new { buyer_qr = buyer_qr });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<List<string>> GetBuyerQRGenneralFG(string[] sizeList)
        {
            try
            {
                var stringList = new StringListBuyerQR();

                //foreach (var item in sizeList)
                //{
                //    stringList.Add(item);
                //}
                SqlParameter sqlParams =


                        new SqlParameter("@tblBuyerQR", SqlDbType.Structured)
                        {
                            Value = stringList,
                            TypeName = "dbo.UT_StringList"
                        };



                var dt = new DataTable();
                dt.Columns.Add("Code");


                foreach (var item in sizeList)
                {
                    dt.Rows.Add(item);
                }
                var result1 = await base.DbConnection.QueryAsync<string>("set arithabort on; EXEC [dbo].[usp_BuyerQR_Filter] @tblBuyerQR", new { tblBuyerQR = dt.AsTableValuedParameter("UT_StringList") }, commandTimeout: 180);




                //var sql = @"EXEC [dbo].[usp_BuyerQR_Filter] @tblBuyerQR";
                //var result = await base.DbConnection.QueryAsync<string>(sql, new { @tblBuyerQR = sqlParams.Value });
                return (List<string>)result1;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<string>> GetAllBuyerQRGenneralFG(string[] sizeList)
        {
            try
            {
                List<string> DataNoExist = new List<string>();
                foreach (var item in sizeList)
                {
                    var query = @"SELECT buyer_qr from generalfg as a where buyer_qr = @buyerqr ";
                    var result = await base.DbConnection.QueryAsync<string>(query, new { @buyerqr = item});
                    if (result.Count() < 1)
                    {
                        DataNoExist.Add(item);
                    }
                }

                return DataNoExist;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<IEnumerable<FGReceiData>> GetListBuyerQRGenneralFG(string model, string ProductName, List<string> param)
        {
            try
            {
                var dt = new DataTable();
                dt.Columns.Add("Code");


                foreach (var item in param)
                {
                    dt.Rows.Add(item);
                }
                var result =  await base.DbConnection.QueryAsync<FGReceiData>("EXEC [dbo].[FILTER_BuyerMes] @tblBuyerQR,@model,@productname", new { tblBuyerQR = dt.AsTableValuedParameter("UT_StringList"), model = model, @productname= ProductName });

                return result;

                //var sql = @"EXEC [dbo].[usp_BuyerQR_Filter] @tblBuyerQR";
                //var result = await base.DbConnection.QueryAsync<string>(sql, new { @tblBuyerQR = sqlParams.Value });
                //var result = await base.DbConnection.QueryAsync<FGReceiData>(query, new { model = model, ProductName = ProductName, @param = param });
                //var query = @"
                //                WITH FILTER_w_material_info_tims
                //                AS
                //                 ( 	SELECT  m.wmtid AS wmtid,
                //                 m.product ProductNo,
                //                 m.buyer_qr BuyerCode,
                //                 m.gr_qty Quantity,
                //                 'MES'  TypeSystem , 
                //                 m.bb_no,  
                //                @model Model, 
                //                @ProductName ProductName 
                //                 FROM	w_material_info_tims m 
                //                 WHERE m.buyer_qr IN ('@param')),
                //                 FILTER_stam_detail AS 
                //                ( SELECT s.id AS wmtid,       
                //                  s.product_code ProductNo, 
                //                 s.buyer_qr BuyerCode,      
                //                 s.standard_qty Quantity,   
                //                 'SAP' TypeSystem ,          
                //                 '' bb_no ,          
                //                @model Model, 
                //                 @ProductName ProductName 
                //                FROM stamp_detail s
                //                WHERE s.buyer_qr IN   ('@param') ),
                //                 FILTER_RESULT_EXIST AS  (
                //                    SELECT *
                //                    FROM    FILTER_w_material_info_tims UNION 
                //                 SELECT *  FROM    FILTER_stam_detail )
                //                SELECT min(f.wmtid)wmtid, min(f.ProductNo)ProductNo, min(f.BuyerCode) BuyerCode,min(f.Quantity)Quantity,min(f.TypeSystem)TypeSystem, max(f.bb_no)bb_no,min(f.Model)Model,min(f.ProductName)ProductName
                //                FROM    FILTER_RESULT_EXIST f 
                //                 GROUP BY f.BuyerCode  

                //        ";
                //var result = await base.DbConnection.QueryAsync<FGReceiData>(query, new { model = model, ProductName = ProductName, @param = param });
                //return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<w_dl_info> CheckDLExist(int id)
        {
            string getvalue = @"SELECT top 1 a.*
                            FROM w_dl_info AS a
                            WHERE a.dlid = @dlid ";
            var result = await base.DbConnection.QueryFirstOrDefaultAsync<w_dl_info>(getvalue, new { dlid = id });
            return result;
        }

        public async Task<string> CheckAnyGenneral(string dl_no)
        {
            string QuerySQL = "SELECT dl_no FROM generalfg WHERE dl_no = @dl_no";

            var result = await base.DbConnection.QueryFirstOrDefaultAsync<string>(QuerySQL, new { dl_no = dl_no });

            return result;
        }
        public async Task<int> DeleteDeliveryForId(int? id)
        {
            string sqlquery = @"DELETE FROM w_dl_info WHERE dlid=@id  ";
            var result = await base.DbConnection.ExecuteScalarAsync<int>(sqlquery, new { id = id });

            return result;

        }
    }
}