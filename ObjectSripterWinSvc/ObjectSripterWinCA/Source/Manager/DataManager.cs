using Framework.Data.Core;
using ObjectSripterWinCA.Source.Interfaces;
using ObjectSripterWinCA.Source.Queries;
using ObjectSripterWinCA.Source.Values;
using System;
using System.Collections.Generic;
using System.Data;

namespace ObjectSripterWinCA.Source.Manager
{
    internal class DataManager
    {
        private static Lazy<DataManager> lazyInst = new Lazy<DataManager>(() => new DataManager());

        private DataManager()
        {
        }

        public static DataManager Instance { get { return lazyInst.Value; } }

        public ISvcConnection Connection { get; set; }

        public List<DbObject> GetObjects(string typeName, IQuery query = null)
        {
            List<DbObject> objList = new List<DbObject>();

            IQuery q = null;
            if (query == null)
                q = new OracleObjectList();
            else
                q = query;

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
                        parameters[k] = typeName.ToUpperInvariant();
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

        public List<string> GetScriptOfObject(DbObject obj, IQuery query)
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
