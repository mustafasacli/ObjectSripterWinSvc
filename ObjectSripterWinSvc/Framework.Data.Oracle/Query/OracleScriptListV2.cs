using Framework.Data.Core.Interfaces;
using System.Data;

namespace Framework.Data.Oracle.Query
{
    internal class OracleScriptListV2 : IQuery
    {
        public CommandType QueryType { get { return CommandType.Text; } }

        public string[] GetParameterList()
        {
            return new string[] { ":PTYPENAME", ":PNAME", ":POWNER" };
        }

        public string GetQuery()
        {
            return
                "SELECT DBMS_METADATA.GET_DDL(:PTYPENAME, :PNAME, :POWNER) AS TEXT FROM DUAL";
        }
    }
}
