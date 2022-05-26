using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Text;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Reflection;
using Mvc_VD.Models;
using Mvc_VD.Classes;

namespace Mvc_VD.Controllers
{
    public class InitMethods : Controller
    {
        //public databoardEntities db = new databoardEntities();

        public void IsNullOrEmptyString(List<string> list, string[] item)
        {
            AddToList(list, item);
            for (int i = 0; i < list.Count; i++)
            {
                if (string.IsNullOrEmpty(list[i]))
                {
                    list[i] = "";
                }
            }
        }

        public void AddToList(List<string> list, params string[] item)
        {
            for (int i = 0; i < item.Length; i++)
            {
                list.Add(item[i]);
            }
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }

        public List<Dictionary<string, object>> GetTableRows(DataTable data)
        {
            var lstRows = new List<Dictionary<string, object>>();
            Dictionary<string, object> dictRow = null;

            foreach (DataRow row in data.Rows)
            {
                dictRow = new Dictionary<string, object>();
                foreach (DataColumn column in data.Columns)
                {
                    dictRow.Add(column.ColumnName, row[column]);
                }
                lstRows.Add(dictRow);
            }
            return lstRows;
        }

        public JsonResult ReturnJsonResultWithPaging(Pageing pageing, int total, OrderedEnumerableRowCollection<DataRow> result)
        {
            if (pageing.sidx != null)
            {
                if (string.Equals(pageing.sord, "asc", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.OrderBy(x => x.Field<string>(pageing.sidx.Trim()));
                }
                else
                {
                    result = result.OrderByDescending(x => x.Field<string>(pageing.sidx.Trim()));
                }
            }

            DataTable data = new DataTable();
            if (result.Any())
            {
                data = result.Skip((pageing.page - 1) * pageing.rows).Take(pageing.rows).CopyToDataTable<DataRow>();
            }
            int totalPages = (int)Math.Ceiling((float)total / pageing.rows);
            var jsonReturn = new
            {
                total = totalPages,
                page = pageing.page,
                records = total,
                rows = new InitMethods().ConvertDataTableToJson(data).Data
            };
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult ReturnJsonResultWithNoPaging(Pageing pageing, int total, OrderedEnumerableRowCollection<DataRow> result)
        {
            if (pageing.sidx != null)
            {
                if (string.Equals(pageing.sord, "asc", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.OrderBy(x => x.Field<string>(pageing.sidx.Trim()));
                }
                else
                {
                    result = result.OrderByDescending(x => x.Field<string>(pageing.sidx.Trim()));
                }
            }

            DataTable data = new DataTable();
            if (result.Any())
            {
                data = result.CopyToDataTable<DataRow>();
            }
            int totalPages = (int)Math.Ceiling((float)total / pageing.rows);
            var jsonReturn = new
            {
                total = totalPages,
                page = pageing.page,
                records = total,
                rows = new InitMethods().ConvertDataTableToJson(data).Data
            };
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ConvertDataTableToJson(DataTable data)
        {
            return Json(GetTableRows(data), JsonRequestBehavior.AllowGet);
        }

        public DataTable ReturnDataTableNonConstraints(StringBuilder sql)
        {
            using (Entities db = new Entities())
            {
                using (var cmd = db.Database.Connection.CreateCommand())
                {
                    db.Database.Connection.Open();
                    cmd.CommandText = sql.ToString();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.Default))
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
                        db.Database.Connection.Close();
                        return DT;
                    }
                }
            }
        }

        public JsonResult ConvertDataTableToJsonAndReturn(StringBuilder sql)
        {
            DataTable data = ReturnDataTableNonConstraints(sql);
            return Json(GetTableRows(data), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ConvertDataTableToJsonAndstring(string sql)
        {
            DataTable data = ReturnDataTableNonConstraintsring(sql);
            return Json(GetTableRows(data), JsonRequestBehavior.AllowGet);
        }
        public DataTable ReturnDataTableNonConstraintsring(string sql)
        {
            using (Entities db = new Entities())
            {
                using (var cmd = db.Database.Connection.CreateCommand())
                {
                    db.Database.Connection.Open();
                    cmd.CommandText = sql;
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
                        db.Database.Connection.Close();
                        return DT;
                    }
                }
            }
        }

        public JsonResult ReturnJsonResultFromQuery(StringBuilder sql)
        {
            bool check = false;
            DataTable data = ReturnDataTableNonConstraints(sql);
            if (data.Rows.Count > 0)
                check = true;
            return Json(new { jsonResult = GetTableRows(data), flag = check }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsonResultAndMessageFromQuery(StringBuilder sql, string message)
        {
            bool check = false;
            DataTable data = ReturnDataTableNonConstraints(sql);
            if (data.Rows.Count > 0)
                check = true;
            return Json(new { data = GetTableRows(data), result = check, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchAndPaging(StringBuilder countSql, StringBuilder viewSql, string pageIndex, string pageSize)
        {
            using (Entities db = new Entities())
            {
                int index = int.Parse(pageIndex);
                float size = float.Parse(pageSize);
                int totalRecords = db.Database.SqlQuery<int>(countSql.ToString()).FirstOrDefault();
                int totalPages = (int)Math.Ceiling((float)totalRecords / size);

                DataTable data = ReturnDataTableNonConstraints(viewSql);
                var result = new
                {
                    total = totalPages,
                    page = index,
                    records = totalRecords,
                    rows = ConvertDataTableToJson(data).Data
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public Dictionary<string, string> PagingAndOrderBy(Pageing pageing, string orderByStr)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            int pageIndex = pageing.page;
            int pageSize = pageing.rows;
            int start_r = pageing.page > 1 ? ((pageIndex - 1) * pageSize) : pageing.page;
            int end_r = (pageIndex * pageSize);
            string order_by = pageing.sidx != null ? $"{pageing.sidx} {pageing.sord}" : orderByStr;
            list.Add("index", pageIndex.ToString());
            list.Add("size", pageSize.ToString());
            list.Add("start", start_r.ToString());
            list.Add("end", end_r.ToString());
            list.Add("orderBy", order_by);
            return list;
        }

        // using for export to excel in server side
        public void WriteHtmlTable<T>(IEnumerable<T> data, TextWriter output, String[] labelList)
        {
            //Writes markup characters and text to an ASP.NET server control output stream. This class provides formatting capabilities that ASP.NET server controls use when rendering markup to clients.
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    //  Create a form to contain the List
                    Table table = new Table();
                    TableRow row = new TableRow();
                    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

                    // table header from IEnumerable
                    //foreach (PropertyDescriptor prop in props) {
                    //    TableHeaderCell hcell = new TableHeaderCell();
                    //    hcell.Text = prop.Name;
                    //    hcell.BackColor = System.Drawing.Color.Yellow;
                    //    row.Cells.Add(hcell);
                    //}

                    foreach (String label in labelList)
                    {
                        TableHeaderCell hcell = new TableHeaderCell();
                        hcell.Text = label;
                        //hcell.BackColor = System.Drawing.Color.Yellow;
                        hcell.Font.Bold = true;
                        row.Cells.Add(hcell);
                        row.BorderStyle = BorderStyle.Solid;
                    }
                    table.Rows.Add(row);

                    //  add each of the data item to the table
                    foreach (T item in data)
                    {
                        row = new TableRow();
                        foreach (PropertyDescriptor prop in props)
                        {
                            TableCell cell = new TableCell();
                            cell.Text = prop.Converter.ConvertToString(prop.GetValue(item));
                            //cell.BorderStyle = BorderStyle.Solid;
                            row.Cells.Add(cell);
                            row.BorderStyle = BorderStyle.Solid;
                        }
                        table.Rows.Add(row);
                    }

                    //  render the table into the htmlwriter
                    table.RenderControl(htw);

                    //  render the htmlwriter into the response
                    output.Write(sw.ToString());
                }
            }
        }

        // Get string value between 2 symbols.
        public string Between(string value, string a, string b)
        {
            int posA = value.IndexOf(a);
            int posB = value.LastIndexOf(b);
            if (posA == -1)
            {
                return "";
            }
            if (posB == -1)
            {
                return "";
            }
            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= posB)
            {
                return "";
            }
            return value.Substring(adjustedPosA, posB - adjustedPosA);
        }

        // Get string value after [first] symbol.
        public string Before(string value, string a)
        {
            int posA = value.IndexOf(a);
            if (posA == -1)
            {
                return "";
            }
            return value.Substring(0, posA);
        }

        // Get string value after [last] symbol.
        public string After(string value, string a)
        {
            int posA = value.LastIndexOf(a);
            if (posA == -1)
            {
                return "";
            }
            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= value.Length)
            {
                return "";
            }
            return value.Substring(adjustedPosA);
        }

        // auto insert 00000
        public string CreateId(int id)
        {
            if (id.ToString().Length < 2)
            {
                return string.Format("00{0}", id);
            }

            if (id.ToString().Length < 3)
            {
                return string.Format("0{0}", id);
            }

            if (id.ToString().Length < 4)
            {
                return string.Format("{0}", id);
            }

            return string.Empty;
        }

        // Convert List<T> to datatable (generic type)
        public DataTable ConvertListToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        // Convert datatable to list<> (generic type)
        public List<T> ConvertDataTableToList<T>(StringBuilder sql)
        {
            DataTable dt = ReturnDataTableNonConstraints(sql);
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public List<T> ConvertDataTable<T>(StringBuilder sql)
        {
            DataTable dt = ReturnDataTableNonConstraints(sql);
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItemDataTable<T>(row);
                data.Add(item);
            }
            return data;
        }

        private T GetItemDataTable<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    //in case you have a enum/GUID datatype in your model
                    //We will check field's dataType, and convert the value in it.
                    if (pro.Name == column.ColumnName)
                    {
                        try
                        {
                            var convertedValue = GetValueByDataType(pro.PropertyType, dr[column.ColumnName]);
                            pro.SetValue(obj, convertedValue, null);
                        }
                        catch (Exception e)
                        {
                            //ex handle code
                            throw;
                        }
                        //pro.SetValue(obj, dr[column.ColumnName], null);
                    }
                    else
                        continue;
                }
            }
            return obj;
        }

        private object GetValueByDataType(Type propertyType, object o)
        {
            if (o.ToString() == "null")
            {
                return null;
            }
            if (propertyType == (typeof(Guid)) || propertyType == typeof(Guid?))
            {
                return Guid.Parse(o.ToString());
            }
            else if (propertyType == typeof(int) || propertyType.IsEnum)
            {
                return Convert.ToInt32(o);
            }
            else if (propertyType == typeof(decimal))
            {
                return Convert.ToDecimal(o);
            }
            else if (propertyType == typeof(long))
            {
                return Convert.ToInt64(o);
            }
            else if (propertyType == typeof(bool) || propertyType == typeof(bool?))
            {
                return Convert.ToBoolean(o);
            }
            else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
            {
                return Convert.ToDateTime(o);
            }
            return o.ToString();
        }

        private T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        //pro.SetValue(obj, dr[column.ColumnName], null);
                        pro.SetValue(obj, dr[column.ColumnName] == DBNull.Value ? string.Empty :
      dr[column.ColumnName].ToString(), null);
                    else
                        continue;
                }
            }
            return obj;
        }

        public string RemoveLastComma(StringBuilder stringBuilder)
        {
            if (stringBuilder.Length > 0)
            {
                int index = stringBuilder.ToString().LastIndexOf(",");
                return stringBuilder.ToString().Substring(0, index);
            }
            return "''";
        }

        public string RemoveLastHyphen(string str)
        {
            int index = str.LastIndexOf("-");
            return str.Substring(0, index);
        }

        #region Get Common Values
        public List<CommonModel> GetCommon(string code)
        { 
            List<comm_dt> list = new List<comm_dt>();
            List<CommonModel> values = new List<CommonModel>();
            using (Entities db = new Entities())
            {
                list = db.comm_dt.Where(x => x.mt_cd == code).ToList();
                foreach (comm_dt item in list)
                {
                    CommonModel x = new CommonModel();
                    x.Code = item.dt_cd;
                    x.Name = item.dt_nm;
                    x.UseYN = item.use_yn;
                    values.Add(x);
                }
            }
            return values;
        }
        #endregion Get Common Values

        #region Get Department Values
        public List<DepartmentModel> GetDepartment()
        {
            List<DepartmentModel> values = new List<DepartmentModel>();
            using (Entities db = new Entities())
            {
                var list = db.department_info.Where(x => x.use_yn == "Y").ToList();
                
                foreach (department_info item in list)
                {
                    DepartmentModel x = new DepartmentModel();
                    x.Id = item.dpno.ToString();
                    x.Code = item.depart_cd;
                    x.Name = item.depart_nm;
                    values.Add(x);
                }
            }
            return values;
        }
        #endregion

        #region Get Stamp Code
        //public list<stamp_master> getallstamp()
        //{
        //    list<stamp_master> list = new list<stamp_master>();
        //    using (entities db = new entities())
        //    {
        //        list = db.stamp_master;
        //    }
        //    return list;
        //}
        #endregion


        #region FG
        public string DateFormatByShinsungRule(string input)
        {
            input = input.Replace("-", "");
            string year = ChangeNumberToCharacter(int.Parse(input.Substring(2, 2))).ToString();
            string month = ChangeNumberToCharacter(int.Parse(input.Substring(4, 2))).ToString();
            string date = ChangeNumberToCharacter(int.Parse(input.Substring(6, 2))).ToString();
            StringBuilder result = new StringBuilder();
            result.Append(year);
            result.Append(month);
            result.Append(date);
            return result.ToString();
        }
        public char ChangeNumberToCharacter(int number)
        {
            string temp = number.ToString();
            char result = 'A';
            int subtraction = 0;
            if (number < 10)
            {
                return char.Parse(temp);
            }
            else
            {
                subtraction = number - 10;
                for (int i = 0; i < subtraction; i++)
                {
                    result++;
                }
            }
            return result;
        }
        public string BuyerQRSerialFormat(int number)
        {
            return ProductQuantityFormatForBuyerQR(number);
        }
        public string ProductQuantityFormatForBuyerQR(int quantity)
        {
            string str = quantity.ToString();
            int length = str.Length;

            if (length % 2 == 0)
            {
                if (length == 4)
                {
                    string fisrtTwoCharacters = str.Substring(0, 2);
                    string lastTwoCharacter = str.Substring(2, 2);

                    char a = ChangeNumberToCharacter(int.Parse(fisrtTwoCharacters));
                    StringBuilder result = new StringBuilder();
                    result.Append(a.ToString())
                        .Append(lastTwoCharacter);
                    str = result.ToString();
                }
                else
                {
                    str = string.Concat("0", str);
                }
            }
            else
            {
                if (length == 5)
                {
                    string fisrtTwoCharacters = str.Substring(0, 2);
                    string secondTwoCharacter = str.Substring(2, 2);
                    string lastCharacter = str.Substring(4, 1);

                    char a = ChangeNumberToCharacter(int.Parse(fisrtTwoCharacters));
                    char b = ChangeNumberToCharacter(int.Parse(secondTwoCharacter));
                    StringBuilder result = new StringBuilder();
                    result.Append(a.ToString())
                        .Append(b.ToString())
                        .Append(lastCharacter);
                    str = result.ToString();
                }
                if (length == 1)
                {
                    str = string.Concat("00", str);
                }
            }
            return str;
        }
        public string ProductQuantityFormatForBoxQR(int quantity)
        {
            string str = quantity.ToString();
            int length = str.Length;
            if (length == 4)
            {
                str = "0" + str;
            }
            if (length == 3)
            {
                str = "00" + str;
            }
            if (length == 2)
            {
                str = "000" + str;
            }
            if (length == 1)
            {
                str = "0000" + str;
            }

            return str;
        }
        #endregion


    }
}