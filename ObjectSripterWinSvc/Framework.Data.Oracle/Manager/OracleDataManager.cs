using Framework.Data.Core.Base;
using Framework.Data.Core.Interfaces;
using Framework.Data.Core.Types;
using Framework.Data.Core.Utils;
using Framework.Data.Oracle.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Framework.Data.Oracle.Manager
{
    public class OracleDataManager : BaseDataManager
    {
        #region [ Private Fields ]

        private List<string> dateTypes = new List<string>()
        { "date", "datetime2", "datetime", "datetimeoffset", "smalldatetime", "timestamp" };

        #endregion [ Private Fields ]

        public override ISvcConnection Connection { get; set; }

        public override List<DbObject> GetObjects(string typeName)
        {
            this.GetObjectsError = null;
            List<DbObject> objList = null;

            try
            {
                objList = new List<DbObject>();
                objList = base.GetListOfObjects(typeName, new OracleObjectList());

                if (objList == null || objList.Count == 0)
                { objList = base.GetListOfObjects(typeName, new OracleObjectListV3()); }
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
                scriptLst = this.GetScriptListOfObject(obj, new OracleScriptListV2());

                //if metadata script of object has not been found, this will work.
                // Getting Metadata Script Text List Of Object with another version.
                if (scriptLst == null || scriptLst.Count == 0)
                { scriptLst = this.GetScriptListOfObject(obj, new OracleScriptList()); }
            }
            catch (Exception e)
            {
                this.GetScriptOfObjectError = e;
            }

            return scriptLst;
        }

        public override string GetSelectScript(DbObject obj)
        {
            throw new NotImplementedException();
        }

        public override List<DbObject> GetTables()
        {
            List<DbObject> list = null;
            list = GetObjects("TABLE");
            list = list ?? new List<DbObject>();
            return list;
        }

        public override string DataRow2String(DataRow row)
        {
            return GetDataRowAsString(row, row.Table.Columns, new Hashtable());
        }

        #region [ GetDataRowAsString method ]

        protected string GetDataRowAsString(DataRow row, DataColumnCollection dataCols, Hashtable Columns, Dictionary<string, string> blobFields = null)
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

                        if (col.DataType == typeof(decimal))
                        {
                            rowBuilder.Append(string.Format("{0}, ", ConvertUtil.ToDecimal(row[col.ColumnName]).ToString().Replace(',', '.')));
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

                        //if (blobFields != null)
                        //{
                        if (col.DataType == typeof(byte[]) && blobFields.ContainsKey(col.ColumnName))
                        {
                            //rowBuilder.Append(string.Format("{0}, ", blobFields[col.ColumnName]));
                            rowBuilder.Append(string.Format("HEXTORAW('{0}'), ", BitConverter.ToString((byte[])row[col.ColumnName])).Replace("-", ""));
                            continue;
                        }
                        //}

                        strColDataType = string.Format("{0}", Columns[col.ColumnName]).ToLower().Replace("ı", "i");
                        if (dateTypes.AsQueryable().Where(s => strColDataType.StartsWith(s)).Count() > 0)
                        {
                            string dtFormat = string.Empty;

                            dtFormat =
                            strColDataType.StartsWith("timestamp") ?
                            "TO_TIMESTAMP('{0}', 'DD/MM/YYYY HH24:MI:SS.FF'), " :
                            "TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS'), ";

                            //TO_CHAR(COL, 'DD/MM/YYYY HH24:MI:SS') AS COL
                            //"TO_CHAR({0}, 'DD/MM/YYYY HH24:MI:SS.FF') AS {0}, ";

                            rowBuilder.Append(string.Format(dtFormat, string.Format("{0}", row[col.ColumnName])));
                            //string.Format("TO_DATE('{0}', '{1}'), ", string.Format("{0}", row[col.ColumnName]), dtFormat));
                            continue;
                        }

                        rowBuilder.Append(string.Format("'{0}', ", string.Format("{0}", row[col.ColumnName]).Replace("'", "''")));
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
