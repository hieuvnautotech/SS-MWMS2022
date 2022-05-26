using Dapper;
using Mvc_VD.data;
using Mvc_VD.Models;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Respositories.Irepository;
using Mvc_VD.Services.ShinsungNew.Iservices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace Mvc_VD.Services.ShinsungNew
{
    public class TestWoServiceNew : DbConnection1RepositoryBase, ItestWoServiceNew
    {
        //private readonly IDbConnection _dbcontext;
        public TestWoServiceNew(IDbConnectionFactory dbConnectionFactory):base(dbConnectionFactory)
        {
            //_dbcontext = dbConnectionFactory.GetDbconection();
        }
        public async Task<IReadOnlyList<Models.MaterialInfoMMS>> GetAllAsync()
        {
            var sql = "SELECT * FROM w_material_info";
            var result = await base.DbConnection.QueryAsync<Models.MaterialInfoMMS>(sql);
            return result.ToList();
        }
        public async Task<IReadOnlyList<d_style_info>> GetStyleInfo()
        {
            var sql = "SELECT * FROM d_style_info";
            var result = await base.DbConnection.QueryAsync<d_style_info>(sql);
            return result.ToList();
        }
    }
}