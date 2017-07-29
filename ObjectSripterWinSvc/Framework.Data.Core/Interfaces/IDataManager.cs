using Framework.Data.Core.Types;
using System;
using System.Collections.Generic;
using System.Data;

namespace Framework.Data.Core.Interfaces
{
    public interface IDataManager
    {
        ISvcConnection Connection { get; set; }

        List<DbObject> GetObjects(string typeName);

        List<string> GetScriptOfObject(DbObject obj);

        List<DbObject> GetTables();

        string GetSelectScript(DbObject obj);

        string DataRow2String(DataRow row);

        Exception GetObjectsError { get; }

        Exception GetScriptOfObjectError { get; }
    }
}
