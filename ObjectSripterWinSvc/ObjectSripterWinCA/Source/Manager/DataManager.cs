using Framework.Data.Core;
using ObjectSripterWinCA.Source.Interfaces;
using ObjectSripterWinCA.Source.Queries;
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

        public List<DbObject> GetObjects(string typeName)
        {
            List<DbObject> objList = new List<DbObject>();

            IQuery q = new OracleObjectList();

            string[] keys = q.GetParameterList();

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { keys[0], this.Connection.Owner },
                { keys[1], typeName }
            };

            DataTable dt = this.Connection.GetData(q.GetQuery(), q.QueryType, parameters);

            foreach (DataRow row in dt.Rows)
            {
                objList.Add(new DbObject
                {
                    NAME = string.Format("{0}", row["NAME"]),
                    OWNER = string.Format("{0}", row["OWNER"]),
                    TYPENAME = string.Format("{0}", row["TYPENAME"])
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

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { keys[0], obj.TYPENAME },
                    { keys[1], obj.NAME },
                    { keys[2], obj.OWNER }
                };

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
