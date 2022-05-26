using Mvc_VD.Respositories.Irepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Mvc_VD.Respositories.Repository
{

    public class DapperDbConnectionFactory : IDbConnectionFactory, IDisposable
    {

        //private readonly string _connectionString;
        private IDbConnection _connection;

        //public DapperDbConnectionFactory(string connectionString)
        //{
        //    this._connectionString = connectionString;
        //}

        //public IDbConnection GetOpenConnection()
        //{
        //    if (this._connection == null || this._connection.State != ConnectionState.Open)
        //    {
        //        this._connection = new SqlConnection(_connectionString);
        //        this._connection.Open();
        //    }

        //    return this._connection;
        //}

        //private IDbConnection _db = null;

        public IDbConnection CreateDbConnection(string connectionName)
        {
            if (this._connection == null || this._connection.State != ConnectionState.Open)
            {
                this._connection = new SqlConnection(connectionName);
                this._connection.Open();
            }

            return this._connection;

        }
        public void Dispose()
        {
            if (this._connection != null && this._connection.State == ConnectionState.Open)
            {
                this._connection.Dispose();
            }
        }

    }

}