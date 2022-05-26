using Mvc_VD.Models;
using System;
using System.Data;
using System.Data.Common;

namespace Mvc_VD.Services
{
    public interface IEntityService
    {
        DataTable ExecuteToDataTable(DbCommand cmd);
    }

    public class EntityService : IEntityService
    {
        private Entities _db;
        public EntityService(IDbFactory dbFactory)
        {
            _db = dbFactory.Init();
        }
        public DataTable ExecuteToDataTable(DbCommand cmd)
        {
            //if (_db.Database.Connection.State == ConnectionState.Closed)
            //    _db.Database.Connection.Open();
            using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                DataTable DTSchema = reader.GetSchemaTable();
                DataTable DT = new DataTable();
                if (DTSchema != null)
                    if (DTSchema.Rows.Count > 0)
                        for (int i = 0; i < DTSchema.Rows.Count; i++)
                        {
                            //Create new column for each row in schema table
                            //Set properties that are causing errors and add it to our datatable
                            //Rows in schema table are filled with information of columns in our actual table
                            DataColumn Col = new DataColumn(DTSchema.Rows[i]["ColumnName"].ToString(), (Type)DTSchema.Rows[i]["DataType"]);
                            Col.AllowDBNull = true;
                            Col.Unique = false;
                            Col.AutoIncrement = false;
                            DT.Columns.Add(Col);
                        }

                while (reader.Read())
                {
                    //Read data and fill it to our datatable
                    DataRow Row = DT.NewRow();
                    for (int i = 0; i < DT.Columns.Count; i++)
                    {
                        Row[i] = reader[i];
                    }
                    DT.Rows.Add(Row);
                }
                //reader.Close();
                //if (_db.Database.Connection.State == ConnectionState.Open)
                //    _db.Database.Connection.Close();
                return DT;
            }
        }
    }
}