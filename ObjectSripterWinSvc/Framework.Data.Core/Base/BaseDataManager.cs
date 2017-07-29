using Framework.Data.Core.Interfaces;
using Framework.Data.Core.Types;
using Framework.Data.Core.Values;
using System;
using System.Collections.Generic;
using System.Data;

namespace Framework.Data.Core.Base
{
    public abstract class BaseDataManager : IDataManager
    {
        protected BaseDataManager()
        { }

        public abstract ISvcConnection Connection { get; set; }

        public Exception GetObjectsError
        { get; protected set; }

        public Exception GetScriptOfObjectError
        { get; protected set; }

        public abstract List<DbObject> GetObjects(string typeName);

        public abstract List<string> GetScriptOfObject(DbObject obj);

        public abstract string GetSelectScript(DbObject obj);

        public abstract string DataRow2String(DataRow row);

        public abstract List<DbObject> GetTables();

        protected virtual List<DbObject> GetListOfObjects(string typeName, IQuery query)
        {
            List<DbObject> objList = new List<DbObject>();

            if (string.IsNullOrWhiteSpace(typeName))
                return objList;

            //IQuery q = null;
            //if (query == null)
            //    q = new OracleObjectList();
            //else
            IQuery q = query;

            string[] keys = q.GetParameterList();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            foreach (var k in keys)
            {
                switch (k)
                {
                    case AppConstants.Name:
                        parameters[k] = null;
                        break;

                    case AppConstants.Owner:
                        parameters[k] = this.Connection.Owner;
                        break;

                    case AppConstants.Type:
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
                    TYPENAME = typeName.ToUpperInvariant()//string.Format("{0}", row["TYPENAME"])
                });
            }

            return objList;
        }

        protected virtual List<string> GetScriptListOfObject(DbObject obj, IQuery query)
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
                        case AppConstants.Name:
                            parameters[k] = obj.NAME;
                            break;

                        case AppConstants.Owner:
                            parameters[k] = obj.OWNER;
                            break;

                        case AppConstants.Type:
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
                    lst.Add(string.Format("{0}", row["TEXT"]));
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return lst;
        }
    }
}
