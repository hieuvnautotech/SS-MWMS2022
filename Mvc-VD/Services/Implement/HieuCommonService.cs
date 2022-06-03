using Dapper;
using Mvc_VD.Models;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models.TIMS;
using Mvc_VD.Models.WIP;
using Mvc_VD.Respositories.Irepository;
using Mvc_VD.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Mvc_VD.Services.Implement
{
    public class HieuCommonService : DbConnection1RepositoryBase, IHieuCommonServices
    {
        public HieuCommonService(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)// này là hàm dựng để xài implement
        {

        }

       
        public async Task<IEnumerable<SdInfos>> GetListSDInfo(string SdNo, string SdName, string ProductCode, string remark)
        {
            try
            {
                string sql = @"SELECT
	            a.*,
	            ( SELECT dt_nm FROM comm_dt WHERE mt_cd = 'WHS005' AND dt_cd = a.status ) AS sts_nm,
	            ( SELECT lct_nm FROM lct_info WHERE lct_cd = a.lct_cd ) AS lct_nm 
            FROM w_sd_info AS a 
            WHERE
	            a.use_yn = 'Y'  and a.active=1
	            AND ( @sdno = '' OR @sdno is null  OR a.sd_no LIKE '%'+@sdno+'%' ) 
	            AND ( @sdnm = '' OR @sdnm is null OR a.sd_nm LIKE '%'+@sdnm+'%' ) 
	            AND ( @procd = '' OR @procd is null OR a.product_cd LIKE '%'+@procd+'%' ) 
	            AND ( @remark = '' OR @remark is null OR a.remark LIKE '%'+@remark+'%' ) 
            ORDER BY sid DESC";
                var result = await base.DbConnection.QueryAsync<SdInfos>(sql, new { sdno = SdNo, sdnm = SdName, procd = ProductCode, remark = remark });
                return result.ToList();
            }
            catch (Exception e)
            {

                throw;
            }

        }


        

        
    }
}