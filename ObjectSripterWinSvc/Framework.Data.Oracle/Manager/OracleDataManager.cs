using Framework.Data.Core.Base;
using Framework.Data.Core.Interfaces;
using Framework.Data.Core.Types;
using Framework.Data.Oracle.Query;
using System;
using System.Collections.Generic;

namespace Framework.Data.Oracle.Manager
{
    public class OracleDataManager : BaseDataManager
    {
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
    }
}
