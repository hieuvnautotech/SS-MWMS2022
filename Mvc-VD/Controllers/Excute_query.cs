using Mvc_VD.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Mvc_VD.Controllers
{
    public class Excute_query
    {
        private Entities db = new Entities();

        public DataTable get_data_from_data_base(StringBuilder query)
        {
            var data = new DataTable();


            using (var cmd = db.Database.Connection.CreateCommand())
            {
                db.Database.Connection.Open();
                cmd.CommandText = query.ToString();
                using (var reader = cmd.ExecuteReader())
                {
                    data.Load(reader);
                }
                db.Database.Connection.Close();
            }
            return data;
        }

        public int Execute_NonQuery(StringBuilder query)
        {
            int result;
            var data = new DataTable();


            using (var cmd = db.Database.Connection.CreateCommand())
            {
                db.Database.Connection.Open();
                cmd.CommandText = query.ToString();
                result = Int32.Parse(cmd.ExecuteNonQuery().ToString());
                db.Database.Connection.Close();
            }
            return result;
        }

        public int Execute_Scalar(StringBuilder query)
        {
            int result;
            var data = new DataTable();

            using (var cmd = db.Database.Connection.CreateCommand())
            {
                db.Database.Connection.Open();
                cmd.CommandText = query.ToString();
                result = Int32.Parse(cmd.ExecuteScalar().ToString());
                db.Database.Connection.Close();
            }
            return result;
        }


        public string Right(string str, int length)
        {
            return str.Substring(str.Length - length, length);
        }


        public string MyLast(string str, int length)
        {
            if (str == null)
                return null;
            else if (str.Length >= length)
                return str.Substring(str.Length - length, length);
            else
                return str;
        }
        public string StartEndIndex(string value, int startIndex, int endIndex)
        {
            if (value == null)
                return null;
            else if (endIndex >= startIndex)

                return value.Substring(startIndex, endIndex);
            else
                return value;

        }
        public string autobarcode(int id)
        {
            if (id.ToString().Length < 2)
            {
                return string.Format("00000{0}", id);
            }

            if ((id.ToString().Length < 3) || (id.ToString().Length == 2))
            {
                return string.Format("0000{0}", id);
            }

            //if ((id.ToString().Length < 4) || (id.ToString().Length == 3))
            //{
            //    return string.Format("000{0}", id);
            //}

            //if ((id.ToString().Length < 5) || (id.ToString().Length == 4))
            //{
            //    return string.Format("00{0}", id);
            //}
            //if ((id.ToString().Length < 6) || (id.ToString().Length == 5))
            //{
            //    return string.Format("0{0}", id);
            //}
            //if ((id.ToString().Length < 7) || (id.ToString().Length == 6))
            //{
            //    return string.Format("{0}", id);
            //}
            return string.Empty;
        }

    }
}