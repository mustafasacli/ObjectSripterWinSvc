﻿using ObjectSripterWinCA.Source.Interfaces;
using System.Data;

namespace ObjectSripterWinSvc.Source.Queries
{
    internal class OracleScriptList : IQuery
    {
        public CommandType QueryType { get { return CommandType.Text; } }

        public string[] GetParameterList()
        {
            return new string[] { ":PTYPENAME", ":PNAME", ":POWNER" };
        }

        public string GetQuery()
        {
            return @"SELECT TEXT FROM ALL_SOURCE WHERE TYPE LIKE :PTYPENAME || '%' AND NAME=:PNAME AND OWNER = :POWNER ORDER BY LINE ASC";
            //return @"SELECT TEXT FROM ALL_SOURCE WHERE (TYPE=:PTYPENAME OR TYPE LIKE :PTYPENAME || '%') AND NAME=:PNAME AND OWNER = :POWNER ORDER BY LINE ASC";
        }
    }
}
