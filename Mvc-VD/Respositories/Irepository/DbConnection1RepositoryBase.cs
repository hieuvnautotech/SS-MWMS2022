using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Mvc_VD.Respositories.Irepository  
{
    //public interface IdbConnection1RepositoryBase
    //{
    //    IDbConnection GetDbconection();
    //}
    public abstract class DbConnection1RepositoryBase 
    {
        public IDbConnection DbConnection { get; private set; }

        //public IDbConnection GetDbconection()
        //{
        //    //this.DbConnection = dbConnectionFactory.CreateDbConnection(ConfigurationManager.ConnectionStrings["MSSQL"].ConnectionString);
        //    return DbConnection;
        //}
        public DbConnection1RepositoryBase(IDbConnectionFactory dbConnectionFactory)
        {
            // Now it's the time to pick the right connection string!
            // Enum is used. No magic string!
            this.DbConnection = dbConnectionFactory.CreateDbConnection(ConfigurationManager.ConnectionStrings["MSSQL"].ConnectionString);
        }
    }
}