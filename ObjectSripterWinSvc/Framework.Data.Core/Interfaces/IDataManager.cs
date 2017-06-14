﻿using Framework.Data.Core.Types;
using System;
using System.Collections.Generic;

namespace Framework.Data.Core.Interfaces
{
    public interface IDataManager
    {
        ISvcConnection Connection { get; set; }

        List<DbObject> GetObjects(string typeName);

        List<string> GetScriptOfObject(DbObject obj);

        Exception GetObjectsError { get; }

        Exception GetScriptOfObjectError { get; }
    }
}
