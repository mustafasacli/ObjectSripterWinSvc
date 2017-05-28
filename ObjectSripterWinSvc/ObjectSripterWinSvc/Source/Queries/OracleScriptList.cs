using ObjectSripterWinSvc.Source.Interfaces;
using System.Data;

namespace ObjectSripterWinSvc.Source.Queries
{
    class OracleScriptList : IQuery
    {
        public CommandType QueryType { get { return CommandType.Text; } }

        public string[] GetParameterList()
        {
            return new string[] { ":POWNER", ":PNAME", ":PTYPENAME" };
        }

        public string GetQuery()
        {
            return @"SELECT TEXT
                     FROM ALL_SOURCE
                     WHERE OWNER = :POWNER
                     AND NAME=:PNAME
                     AND TYPE=:PTYPENAME
                     ORDER BY LINE ASC";
        }
    }
}
