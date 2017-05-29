using ObjectSripterWinCA.Source.Interfaces;
using System.Data;

namespace ObjectSripterWinCA.Source.Queries
{
    class OracleObjectListV2 : IQuery
    {
        public CommandType QueryType { get { return CommandType.Text; } }

        public string[] GetParameterList()
        {
            return new string[] { ":POWNER", ":PTYPENAME" };
        }

        public string GetQuery()
        {
            return
            @"SELECT AO.OWNER, AO.OBJECT_NAME AS NAME, AO.OBJECT_TYPE AS TYPENAME FROM ALL_OBJECTS AO WHERE AO.OWNER = :POWNER AND AO.OBJECT_TYPE = :PTYPENAME";
        }
    }
}
