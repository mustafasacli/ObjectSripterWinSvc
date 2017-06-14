using Framework.Data.Core.Base;
using Framework.Data.Core.Interfaces;
using Framework.Data.Core.Types;
using Framework.Data.Sql.Query;
using System;
using System.Collections.Generic;
using System.Data;

namespace Framework.Data.Sql.Manager
{
    public class SqlDataManager : BaseDataManager
    {
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
    }
}
