using Framework.Data.Core.Base;
using Framework.Data.Core.Interfaces;
using Framework.Data.Core.Types;
using Framework.Data.Core.Utils;
using Framework.Data.Sql.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Framework.Data.Sql.Manager
{
    public class SqlDataManager : BaseDataManager
    {
        #region [ Private Fields ]

        private List<string> dateTypes = new List<string>() { "date", "datetime2", "datetime", "datetimeoffset", "smalldatetime", "timestamp" };

        #endregion [ Private Fields ]

        public override ISvcConnection Connection { get; set; }

        public override List<DbObject> GetObjects(string typeName)
        {
            this.GetObjectsError = null;
            List<DbObject> objList = null;

            try
            {
                objList = new List<DbObject>();
                objList = this.GetListOfObjects(typeName, new SqlObjectList());
            }
            catch (Exception e)
            {
                this.GetObjectsError = e;
            }

            return objList;
        }

        public override List<string> GetScriptOfObject(DbObject obj)
        {
            this.GetScriptOfObjectError = null;
            List<string> scriptLst = null;

            try
            {
                scriptLst = new List<string>();
                scriptLst = this.GetScriptListOfObject(obj, new SqlScriptList());
            }
            catch (Exception e)
            {
                this.GetScriptOfObjectError = e;
            }

            return scriptLst;
        }

        protected override List<DbObject> GetListOfObjects(string typeName, IQuery query)
        {
            List<DbObject> objList = new List<DbObject>();

            if (string.IsNullOrWhiteSpace(typeName))
                return objList;

            IQuery q = query;

            string[] keys = q.GetParameterList();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            foreach (var k in keys)
            {
                switch (k)
                {
                    case "@objname":
                        parameters[k] = null;
                        break;

                    case "@POWNER":
                        parameters[k] = this.Connection.Owner;
                        break;

                    case "@PTYPENAME":
                        parameters[k] = typeName.Trim().ToUpperInvariant();
                        break;

                    default:
                        parameters[k] = null;
                        break;
                }
            }

            DataTable dt = this.Connection.GetData(q.GetQuery(), q.QueryType, parameters);

            foreach (DataRow row in dt.Rows)
            {
                objList.Add(new DbObject
                {
                    NAME = string.Format("{0}", row["NAME"]),
                    OWNER = string.Format("{0}", row["OWNER"]),
                    TYPENAME = typeName.ToUpperInvariant()
                });
            }

            return objList;
        }

        protected override List<string> GetScriptListOfObject(DbObject obj, IQuery query)
        {
            List<string> lst = new List<string>();

            try
            {
                string[] keys = query.GetParameterList();

                Dictionary<string, object> parameters = new Dictionary<string, object>();

                foreach (var k in keys)
                {
                    switch (k)
                    {
                        case "@objname":
                            parameters[k] = string.Join(".", obj.OWNER, obj.NAME);
                            break;

                        case "@POWNER":
                            parameters[k] = obj.OWNER;
                            break;

                        case "@PTYPENAME":
                            parameters[k] = obj.TYPENAME.ToUpperInvariant();
                            break;

                        default:
                            parameters[k] = null;
                            break;
                    }
                }

                DataTable dt = this.Connection.GetData(query.GetQuery(), query.QueryType, parameters);

                foreach (DataRow row in dt.Rows)
                {
                    lst.Add(string.Format("{0}", row[0]));
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return lst;
        }

        public override List<DbObject> GetTables()
        {
            List<DbObject> list = null;
            list = GetObjects("U");
            list = list ?? new List<DbObject>();
            return list;
        }

        public override string GetSelectScript(DbObject obj)
        {
            throw new NotImplementedException();
        }

        public override string DataRow2String(DataRow row)
        {
            //throw new NotImplementedException();
            return GetDataRowAsString(row, row.Table.Columns, new Hashtable());
        }

        #region [ GetDataRowAsString method ]

        public string GetDataRowAsString(DataRow row, DataColumnCollection dataCols, Hashtable Columns)
        {
            try
            {
                StringBuilder rowBuilder = new StringBuilder();
                string strColDataType;

                foreach (DataColumn col in dataCols)
                {
                    if (ConvertUtil.IsNullOrDbNull(row[col.ColumnName]))
                    {
                        rowBuilder.Append("NULL, ");
                        continue;
                    }
                    else
                    {
                        if (col.DataType == typeof(bool))
                        {
                            rowBuilder.Append(string.Format("{0}, ", ConvertUtil.ToBool(row[col.ColumnName]) ? 1 : 0));
                            continue;
                        }

                        if (col.DataType == typeof(double))
                        {
                            rowBuilder.Append(string.Format("{0}, ", ConvertUtil.ToDouble(row[col.ColumnName]).ToString().Replace(',', '.')));
                            continue;
                        }

                        if (col.DataType == typeof(float))
                        {
                            rowBuilder.Append(string.Format("{0}, ", ConvertUtil.ToFloat(row[col.ColumnName]).ToString().Replace(',', '.')));
                            continue;
                        }

                        if (col.DataType == typeof(int) ||
                            col.DataType == typeof(short) ||
                            col.DataType == typeof(byte))
                        {
                            rowBuilder.Append(string.Format("{0}, ", ConvertUtil.ToInt(row[col.ColumnName])));
                            continue;
                        }

                        if (col.DataType == typeof(long))
                        {
                            rowBuilder.Append(string.Format("{0}, ", ConvertUtil.ToLong(row[col.ColumnName])));
                            continue;
                        }

                        if (col.DataType == typeof(byte[]))
                        {
                            rowBuilder.Append(string.Format("0x{0}, ", BitConverter.ToString((byte[])row[col.ColumnName])).Replace("-", ""));
                            continue;
                        }

                        strColDataType = string.Format("{0}", Columns[col.ColumnName]).ToLower().Replace("ı", "i");
                        if (dateTypes.Contains(strColDataType))
                        {
                            rowBuilder.Append(string.Format("CAST(N'{0}' AS {1}), ", string.Format("{0}", row[col.ColumnName]), Columns[col.ColumnName]));
                            continue;
                        }

                        rowBuilder.Append(string.Format("N'{0}', ", string.Format("{0}", row[col.ColumnName]).Replace("'", "''")));
                    }
                }
                string rowStr = rowBuilder.ToString();
                rowStr = rowStr.TrimEnd().TrimEnd(',');
                return rowStr;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion [ GetDataRowAsString method ]
    }
}
